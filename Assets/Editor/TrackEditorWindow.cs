using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

using track_editor;

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

        TrackAsset asset;

        [SerializeField]
        string assetName;

        [SerializeField]
        int gridScale = 4;

        Rect rectGUI;

        TrackAssetPlayer player;

        private void OnEnable()
        {
            manager = new TrackEditor(new TrackEditorSettings(), new RootTrackData());

            //for (int i = 0; i < 3; ++i) {
            //    _manager.AddTrack(_manager.top, "Track:" + i, new GameObjectTrackData());
            //}

            autoReload();
        }

        private void OnDisable()
        {
            if (asset) {
                assetName = asset.name;
            }
        }

        private void Update()
        {
            if (asset == null) {
                autoReload();
            }

            if (player != null && player.IsPlaying) {
                manager.currentFrame = player.currentFrame;
                Repaint();
            }
        }

        private void OnGUI()
        {
            manager.valueChanged = false;
            manager.lockMode = EditorApplication.isPlayingOrWillChangePlaymode;


            drawHeader();

            manager.OnGUI(new Rect(rectGUI.x + 10 , rectGUI.y + 10 + rectGUI.height, position.width - 20, position.height - rectGUI.y - rectGUI.height - 20));

            if (manager.valueChanged) {
                Repaint();
                saveAsset();
            }
        }

        void autoReload()
        {
            if (!string.IsNullOrEmpty(assetName)) {
                asset = FindObjectsOfType<TrackAsset>().Where(obj => obj.name == assetName).FirstOrDefault();
                if (asset) {
                    loadAsset(asset);
                }
            }
        }

        void loadAsset(TrackAsset loadAsset)
        {
            asset = SerializeUtility.LoadAsset(manager, loadAsset);
            assetName = asset.name;
        }

        void saveAsset()
        {
            SerializeUtility.SaveAsset(manager, asset);
        }

        void drawHeader()
        {

            using (new GUILayout.HorizontalScope("box", GUILayout.Height(90))) {
                using (new GUILayout.VerticalScope(GUILayout.Width(position.width * 0.5f))) {

                    using (new GUILayout.HorizontalScope()) {

                        using (new EditorGUI.DisabledScope(manager.lockMode)) {
                            using (var changedScope = new EditorGUI.ChangeCheckScope()) {
                                asset = (TrackAsset)EditorGUILayout.ObjectField("TrackAsset", asset, typeof(TrackAsset), true);

                                if (changedScope.changed && asset != null) {
                                    loadAsset(asset);
                                }
                            }
                        }

                        using (manager.CreateValueChangedScope()) {

                            if (GUILayout.Button("New Data", GUILayout.Width(80))) {
                                var path = GameObjectUtility.GetUniqueNameForSibling(null, "TrackAsset");

                                loadAsset(SerializeUtility.SaveGameObject(manager, path));
                            }
                        }
                    }

                    using (manager.CreateValueChangedScope()) {
                        manager.currentFrame = Mathf.Max(0, EditorGUILayout.IntField("Frame", manager.currentFrame));
                        manager.frameLength = Mathf.Max(0, EditorGUILayout.IntField("FrameLength", manager.frameLength));
                    }

                    using (manager.CreateValueChangedScope()) {
                        if (GUILayout.Button("Add GameObject Track")) {
                            var track = manager.AddTrack(manager.top, string.Format("Track:{0}", manager.top.childs.Count + 1), new GameObjectTrackData());
                            manager.SetSelectionTrack(track);

                        }
                    }
                }

                if (EditorApplication.isPlaying) {

                    using (new EditorGUI.DisabledScope(this.asset == null)) {

                        using (new GUILayout.VerticalScope()) {

                            gridScale = EditorGUILayout.IntSlider("Grid Scale", gridScale, 1, manager.gridScaleMax);
                            manager.gridScale = gridScale;

                            if (GUILayout.Button("再生")) {
                                if (player) {
                                    GameObject.Destroy(player.gameObject);
                                }

                                var inst = Instantiate(asset);
                                
                                player = inst.gameObject.AddComponent<TrackAssetPlayer>();
                                player.Play(inst);
                            }

                            if (player != null && player.IsPlaying) {
                                if (GUILayout.Button("再生停止")) {
                                    player.Stop();
                                }
                            }

                        }
                        using (new GUILayout.VerticalScope()) {
                        }
                    }

                } else {

                    using (manager.CreateValueChangedScope()) {
                        using (new GUILayout.VerticalScope()) {

                            gridScale = EditorGUILayout.IntSlider("Grid Scale", gridScale, 1, manager.gridScaleMax);
                            manager.gridScale = gridScale;

                            //GUILayout.Space(5);

                            if (manager.selectionTrack != null) {
                                using (new GUILayout.VerticalScope()) {
                                    manager.selectionTrack.HeaderDrawer();
                                }

                            } else {
                                GUILayout.Label("Track 未選択");
                            }
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
