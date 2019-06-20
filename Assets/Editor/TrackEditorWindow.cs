using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

using track_editor_fw;

namespace track_editor
{
    public class TrackEditorWindow : EditorWindow
    {
        [MenuItem("Test/TrackEditorWindow")]
        static void Open()
        {
            GetWindow<TrackEditorWindow>("TrackEditorExample");
        }

        TrackEditor manager;

        [SerializeField]
        TrackAsset asset;

        [SerializeField]
        string reinitObjectName;

        Rect rectGUI;

        private void OnEnable()
        {
            manager = new TrackEditor(new TrackEditorSettings(), new RootTrackData());

            //for (int i = 0; i < 3; ++i) {
            //    _manager.AddTrack(_manager.top, "Track:" + i, new GameObjectTrackData());
            //}
        }

        private void OnDisable()
        {
        }

        private void Update()
        {
            if (EditorApplication.isPlaying) {
                foreach (var player in GameObject.FindObjectsOfType<TrackAssetPlayer>()) {
                    if (player.IsPlaying) {
                        if (this.asset != player.asset) {
                            this.asset = SerializeUtility.LoadAsset(manager, asset);
                        }

                        manager.currentFrame = player.currentFrame;
                        Repaint();
                    }
                }
            }
        }

        private void OnGUI()
        {
            assetAutoLoad();

            drawHeader();


            manager.OnGUI(new Rect(rectGUI.x + 10 , rectGUI.y + 10 + rectGUI.height, position.width - 20, position.height - rectGUI.y - rectGUI.height - 20));

            if (manager.repaintRequest) {
                Repaint();
            }
        }

        /// <summary>
        /// オブジェクト消滅を検知して読み直す機構
        /// 通常だと再生するたびにオブジェクトが失われてしまう
        /// </summary>
        void assetAutoLoad()
        {
            if (!string.IsNullOrEmpty(reinitObjectName) && asset == null) {
                var sceneAsset = GameObject.FindObjectsOfType<TrackAsset>().Where(obj => obj.name == reinitObjectName).FirstOrDefault();
                if (sceneAsset) {
                    loadAsset(sceneAsset);
                }
            }
        }

        void loadAsset(TrackAsset loadAsset)
        {
            asset = SerializeUtility.LoadAsset(manager, loadAsset);
            reinitObjectName = asset.name;
        }

        void drawHeader()
        {
            using (new GUILayout.HorizontalScope("box", GUILayout.Height(90))) {
                using (new GUILayout.VerticalScope(GUILayout.Width(position.width * 0.4f))) {

                    using (new EditorGUI.DisabledScope(manager.lockMode)) {

                        using (var changedScope = new EditorGUI.ChangeCheckScope()) {
                            asset = (TrackAsset)EditorGUILayout.ObjectField("TrackAsset", asset, typeof(TrackAsset), true);

                            if (changedScope.changed && asset != null) {
                                loadAsset(asset);
                            }
                        }

                        manager.currentFrame = Mathf.Max(0, EditorGUILayout.IntField("Frame", manager.currentFrame));
                        manager.frameLength = Mathf.Max(0, EditorGUILayout.IntField("FrameLength", manager.frameLength));
                    }

                    manager.gridScale = EditorGUILayout.IntSlider("Grid Scale", (int)manager.gridScale, 1, manager.gridScaleMax);
                }

                using (new EditorGUI.DisabledScope(manager.lockMode)) {
                    using (new GUILayout.VerticalScope()) {
                        if (manager.selectionTrack != null) {
                            using (new GUILayout.VerticalScope()) {
                                manager.selectionTrack.HeaderDrawer();
                            }

                        } else {
                            if (GUILayout.Button("Add GameObject Track")) {
                                var track = manager.AddTrack(manager.top, string.Format("Track:{0}", manager.top.childs.Count + 1), new GameObjectTrackData());
                                manager.SetSelectionTrack(track);

                            }
                        }
                    }

                    using (new GUILayout.VerticalScope(GUILayout.Width(position.width * 0.3f))) {
                        using (new EditorGUI.DisabledScope(asset == null)) {
                            if (GUILayout.Button("Save")) {
                                SerializeUtility.SaveAsset(manager, asset);
                            }

                            if (GUILayout.Button("Load")) {
                                asset = SerializeUtility.LoadAsset(manager, asset);
                            }
                        }

                        if (GUILayout.Button("New Data")) {
                            var path = GameObjectUtility.GetUniqueNameForSibling(null, "TrackAsset");

                            loadAsset(SerializeUtility.SaveGameObject(manager, path));
                        }
                    }

                }
            }

            // 最後に描画された要素から描画領域を計算
            if (Event.current.type == EventType.Repaint) {
                rectGUI = GUILayoutUtility.GetLastRect();
            }
        }
    }
}
