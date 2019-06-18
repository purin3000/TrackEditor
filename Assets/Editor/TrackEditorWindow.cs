using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

namespace track_editor
{
    public class TrackEditorWindow : EditorWindow, ITrackEditorHeader
    {
        [MenuItem("Fantasian/Test/TrackEditorWindow")]
        static void Open()
        {
            GetWindow<TrackEditorWindow>("TrackEditorExample");
        }

        TrackEditor _manager;

        TrackAsset asset;

        [SerializeField]
        bool reinit1 = false;

        [SerializeField]
        bool reinit2 = false;

        [SerializeField]
        string reinitObjectName;

        private void OnEnable()
        {
            _manager = new TrackEditor(new TrackEditorSettings(), new RootTrackData(), this);

            //for (int i = 0; i < 3; ++i) {
            //    _manager.AddTrack(_manager.top, "Track:" + i, new GameObjectTrackData());
            //}

            if (EditorApplication.isPlayingOrWillChangePlaymode) {
                // Unityは再生すると再生開始時と再生終了時の2回もオブジェクト初期化が入る。
                // この影響で通常だとEditorWindowの編集状態が失われてしまうが、
                // うまくフラグを立てて分岐することで、その都度データを読み直してます。
                // 初期化は2回来るのにOnEnableは1回しか来ないとか、本当に勘弁してほしいのだが。
                //
                // ちなみにTrackAssetはシーン内のオブジェクトのため、
                // 例えSerializeFieldを指定しても内部的にシーンの読み替えが入るらしく、
                // そのときにシーンのオブジェクトは参照が外れるため、通常の方法だと確実に参照を維持できません。
                
                reinit1 = true;
                reinit2 = true;
            }
        }

        private void OnDisable()
        {
            // 覚えておいてあとで再初期化に使う
            if (asset) {
                reinitObjectName = asset.name;
            }
        }

        private void OnGUI()
        {
            if (!string.IsNullOrEmpty(reinitObjectName)) {
                if (EditorApplication.isPlaying) {
                    if (reinit1) {
                        Debug.Log("再初期化1");
                        reload();
                        reinit1 = false;

                    }
                } else {
                    if (reinit2) {
                        Debug.Log("再初期化2");
                        reload();
                        reinit2 = false;
                    }
                }
            }

            _manager.OnGUI(new Rect(10, 10, Screen.width - 20, Screen.height - 20 - 24));

            if (_manager.repaintRequest) {
                Repaint();
            }
        }

        void reload()
        {
            var assetList = GameObject.FindObjectsOfType<TrackAsset>();

            foreach (var asset in assetList) {
                if (asset.name == reinitObjectName) {
                    this.asset = SerializeUtility.LoadAsset(_manager, asset);
                    return;
                }
            }
        }

        public void DrawHeader(TrackEditor manager, Rect rect)
        {
            using (new GUILayout.HorizontalScope()) {

                using (new GUILayout.VerticalScope()) {
                    using (var changedScope = new EditorGUI.ChangeCheckScope()) {
                        asset = (TrackAsset)EditorGUILayout.ObjectField("TrackAsset", asset, typeof(TrackAsset), true);

                        if (changedScope.changed && asset != null) {
                            asset = SerializeUtility.LoadAsset(manager, asset);
                        }
                    }

                    using (new GUILayout.HorizontalScope()) {
                        manager.currentFrame = Mathf.Max(0, EditorGUILayout.IntField("Frame", manager.currentFrame));
                        manager.frameLength = Mathf.Max(0, EditorGUILayout.IntField("Length", manager.frameLength));
                    }

                    if (GUILayout.Button("Add GameObject Track")) {
                        var track = manager.AddTrack(manager.top, string.Format("Track:{0}", manager.top.childs.Count + 1), new GameObjectTrackData());
                        manager.SetSelectionTrack(track);

                    }
                }

                using (new GUILayout.VerticalScope()) {

                    using (new GUILayout.HorizontalScope()) {
                        if (GUILayout.Button("New Data")) {
                            var path = GameObjectUtility.GetUniqueNameForSibling(null, "TrackAsset");

                            asset = SerializeUtility.SaveGameObject(manager, path);
                        }

                        using (new EditorGUI.DisabledScope(asset == null)) {
                            if (GUILayout.Button("Save")) {
                                SerializeUtility.SaveAsset(manager, asset);
                            }

                            if (GUILayout.Button("Load")) {
                                asset = SerializeUtility.LoadAsset(manager, asset);
                            }
                        }
                    }

                    GUILayout.Space(10);

                    manager.gridScale = EditorGUILayout.IntSlider("Scale", (int)manager.gridScale, 1, manager.gridScaleMax);
                }
            }
        }
    }
}
