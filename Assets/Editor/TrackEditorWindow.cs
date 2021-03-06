﻿using System.Collections;
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

        [SerializeField]
        int currentFrame = 0;

        [SerializeField]
        Vector2 scrPos;

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

            manager.currentFrame = currentFrame;
            manager.scrollPos = scrPos;

            drawHeader();

            manager.OnGUI(new Rect(rectGUI.x + 10, rectGUI.y + 10 + rectGUI.height, position.width - 20, position.height - rectGUI.y - rectGUI.height - 20));

            currentFrame = manager.currentFrame;
            scrPos = manager.scrollPos;

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

                    //using (manager.CreateValueChangedScope()) {

                    //    using (new GUILayout.HorizontalScope()) {
                    //        var table = new[] {
                    //            new { Name = "Group",      Type = typeof(TrackGroupEditorTrack.EditorTrackData) },
                    //            new { Name = "GameObject", Type = typeof(GameObjectEditorTrack.EditorTrackData) },
                    //            new { Name = "Camera",     Type = typeof(CameraEditorTrack.EditorTrackData) },
                    //            new { Name = "PostEffect", Type = typeof(PostEffectEditorTrack.EditorTrackData) },
                    //        };

                    //        foreach (var info in table) {

                    //            if (GUILayout.Button($"Add [{info.Name}]")) {
                    //                var track = manager.AddTrack(manager.top, (EditorTrack)ReflectionUtil.CreateObject(info.Type));
                    //                track.name = $"{info.Name} Track";
                    //                manager.SetSelectionTrack(track);
                    //            }
                    //        }

                    //    }
                    //}
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

                            if (manager.selectionTrack != null) {
                                using (new GUILayout.VerticalScope()) {
                                    manager.selectionTrack.TrackHeaderDrawer();
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
            var rootTrack = new RootEditorTrack.EditorTrackData();
            manager.SetRootTrack(rootTrack);

            var path = GameObjectUtility.GetUniqueNameForSibling(null, "TrackAsset");

            var asset = new GameObject(path).AddComponent<TrackAsset>();

            TrackSerializer.EditorToAsset(manager, asset);

            EditorUtility.SetDirty(asset);

            return asset;
        }
    }
}
