using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

public class TrackEditorExample : EditorWindow
{
    [MenuItem("Test/TrackEditorWindow")]
    static void Open()
    {
        GetWindow<TrackEditorExample>("TrackEditorExample");
    }

    TrackManager _manager = new TrackManager(new TrackEditorSettings());

    private void OnEnable()
    {
        for (int i = 0; i < 10; ++i) {
            _manager.AddTrack(_manager.top, "Track:" + i, new ObjectTrackData());
        }

        _manager.SetHeader(new TestHeader(_manager));
        _manager.SetSelectionTrack(_manager.top.childs[2]);
    }

    private void OnGUI()
    {
        _manager.OnGUI(new Rect(10, 10, Screen.width - 20, Screen.height - 20 - 24));

        if (_manager.repaintRequest) {
            Repaint();
        }
    }

    [System.Serializable]
    public class TestHeader : HeaderBase
    {
        public TestHeader(TrackManager manager)
           : base(manager)
        {
        }

        public override void DrawHeader(Rect rect)
        {
            base.DrawHeader(rect);

            using (new GUILayout.HorizontalScope()) {

                if (GUILayout.Button("Add ObjectTrack")) {
                    var track = manager.AddTrack(manager.top, string.Format("Track:{0}", manager.top.childs.Count + 1), new ObjectTrackData());
                    manager.SetSelectionTrack(track);

                }

                if (GUILayout.Button("Save")) {
                    manager.Save<TrackEditorExampleAsset>("Assets/TrackEditorData.asset");
                }

                if (GUILayout.Button("Load")) {

                }
            }
        }
    }

    class ObjectTrackData : TrackBase
    {
        public override void Initialize(TrackManager manager, string name, TrackBase parent)
        {
            base.Initialize(manager, name, parent);

            // 初期化時にトラックを生成する場合、Initialize()以降に行う必要がある
            manager.AddTrack(this, "Object", new PositionTrackData());
            manager.AddTrack(this, "Object", new PositionTrackData());
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            if (GUILayout.Button("Remove")) {
                manager.RemoveTrack(parent, this);
            }
        }

        public override void Write<T>(T asset)
        {
        }
    }

    class PositionTrackData : TrackBase
    {
        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            if (GUILayout.Button("Add")) {
                manager.AddElement(this, new PositionElement());
            }
        }

        class PositionElement : ElementBase
        {
            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                if (GUILayout.Button("Remove")) {
                    parent.manager.RemoveElement(parent, this);
                }
            }
        }
    }
}
