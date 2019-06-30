using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace track_editor2
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

        [SerializeField]
        float playSpeed = 1.0f;

        Rect rectGUI;

        TrackAssetPlayer player;

        private void OnEnable()
        {
            manager = new TrackEditor(new TrackEditorSettings());

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
            TrackSerializer.AssetToEditor(manager, loadAsset);

            asset = loadAsset;
            assetName = loadAsset.name;
        }

        void saveAsset()
        {
            if (asset) {
                TrackSerializer.EditorToAsset(manager, asset);
            }
        }
        
        void drawHeader()
        {

            using (new GUILayout.HorizontalScope("box", GUILayout.Height(90))) {
                using (new GUILayout.VerticalScope(GUILayout.Width(position.width * 0.5f))) {

                    using (new GUILayout.HorizontalScope()) {

                        using (new EditorGUI.DisabledScope(manager.lockMode)) {
                            using (var changedScope = new EditorGUI.ChangeCheckScope()) {
                                asset = (TrackAsset)EditorGUILayout.ObjectField("TrackAsset", asset, typeof(TrackAsset), true);

                                if (changedScope.changed) {
                                    if (asset != null) {
                                        loadAsset(asset);
                                    } else {
                                        assetName = null;
                                        manager = new TrackEditor(new TrackEditorSettings());
                                    }
                                }
                            }
                        }

                        using (manager.CreateValueChangedScope()) {

                            if (GUILayout.Button("New Data", GUILayout.Width(80))) {
                                manager = new TrackEditor(new TrackEditorSettings());

                                loadAsset(CreateNewTrackData());
                            }
                        }
                    }

                    using (manager.CreateValueChangedScope()) {
                        manager.currentFrame = Mathf.Max(0, EditorGUILayout.IntField("Frame", manager.currentFrame));
                        manager.frameLength = Mathf.Max(0, EditorGUILayout.IntField("FrameLength", manager.frameLength));
                        manager.playSpeed = EditorGUILayout.Slider("Play Speed", manager.playSpeed, 0.0f, 5.0f);
                    }

                    using (manager.CreateValueChangedScope()) {
                        using (new GUILayout.HorizontalScope()) {
                            if (GUILayout.Button("Add GameObject Track")) {
                                var track = manager.AddTrack(manager.top, new GameObjectEditorTrack());
                                track.name = string.Format("Track:{0}", manager.top.childs.Count + 1);
                                manager.SetSelectionTrack(track);
                            }

                            if (GUILayout.Button("Add Camera Track")) {
                                var track = manager.AddTrack(manager.top, new CameraEditorTrack());
                                track.name = string.Format("Track:{0}", manager.top.childs.Count + 1); 
                                manager.SetSelectionTrack(track);

                            }
                        }
                    }
                }

                if (EditorApplication.isPlaying) {

                    using (new EditorGUI.DisabledScope(this.asset == null)) {

                        using (new GUILayout.VerticalScope()) {

                            gridScale = EditorGUILayout.IntSlider("Grid Scale", gridScale, 1, manager.gridScaleMax);
                            manager.gridScale = gridScale;

                            playSpeed = EditorGUILayout.Slider("Play Speed", playSpeed, 0.0f, 5.0f);

                            if (GUILayout.Button("再生")) {
                                if (player) {
                                    GameObject.Destroy(player.gameObject);
                                }

                                var inst = Instantiate(asset);
                                
                                player = inst.gameObject.AddComponent<TrackAssetPlayer>();
                                player.Play(inst);
                                player.SetPlaySpeed(playSpeed);
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
                                    if (manager.selectionTrack.selectionElement != null) {
                                        manager.selectionTrack.selectionElement.HeaderDrawer();

                                    } else {
                                        manager.selectionTrack.HeaderDrawer();
                                    }
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

        TrackAsset CreateNewTrackData()
        {
            var rootTrack = new RootEditorTrack();
            manager.SetRootTrack(rootTrack);


            var gameObjectTrack1 = new GameObjectEditorTrack();
            gameObjectTrack1.name = "GameObjeTrack1";
            gameObjectTrack1.trackData.activate = true;
            manager.AddTrack(rootTrack, gameObjectTrack1);

            var gameObjectTrack2 = new GameObjectEditorTrack();
            gameObjectTrack2.name = "GameObjeTrack2";
            gameObjectTrack2.trackData.activate = false;
            gameObjectTrack2.trackData.currentPlayer = true;
            manager.AddTrack(rootTrack, gameObjectTrack2);

            var activationTrack1 = new ActivationEditorTrack();
            activationTrack1.name = "ActivationTrack1";
            manager.AddTrack(gameObjectTrack2, activationTrack1);

            var activationTrack2 = new ActivationEditorTrack();
            activationTrack2.name = "ActivationTrack2";
            manager.AddTrack(gameObjectTrack2, activationTrack2);

            var activationElement1 = new ActivationEditorElement();
            activationElement1.name = "ActivationElement";
            manager.AddElement(activationTrack2, activationElement1);

            var activationElement2 = new ActivationEditorElement();
            activationElement2.name = "ActivationElement2";
            manager.AddElement(activationTrack2, activationElement2);



            var path = GameObjectUtility.GetUniqueNameForSibling(null, "TrackAsset");

            var asset = new GameObject(path).AddComponent<track_editor2.TrackAsset>();

            TrackSerializer.EditorToAsset(manager, asset);

            EditorUtility.SetDirty(asset);

            return asset;
        }
    }
}
