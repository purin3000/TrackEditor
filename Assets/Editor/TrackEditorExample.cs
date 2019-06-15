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

    TrackEditor _trackEditor = new TrackEditor(new TrackEditorSettings());

    private void OnEnable()
    {
        for (int i = 0; i < 10; ++i) {
            _trackEditor.top.AddTrack("Track:" + i, new ObjectTrackData());
        }

        _trackEditor.SetHeader(new TestHeader(_trackEditor));
        _trackEditor.SetSelectionTrack(_trackEditor.top.childs[2]);
    }

    private void OnGUI()
    {
        _trackEditor.OnGUI(new Rect(10, 10, Screen.width - 20, Screen.height - 20 - 24));

        if (_trackEditor.repaintRequest) {
            Repaint();
        }
    }

    class TestHeader : HeaderBase
    {
        public TestHeader(TrackEditor root)
           : base(root)
        {
        }
    }

    class ObjectTrackData : TrackBase
    {
        public override void Initialize(TrackEditor trackEditor, string name, TrackBase parent)
        {
            base.Initialize(trackEditor, name, parent);

            // 初期化時にトラックを生成する場合、Initialize()以降に行う必要がある
            AddTrack("Object", new PositionTrackData());
            AddTrack("Object", new PositionTrackData());
        }
    }

    class PositionTrackData : TrackBase
    {
        public override void PropertyDrawer(Rect rect)
        {
            if (GUILayout.Button("Add")) {
                AddElement(new PositionElement());
            }
        }

        class PositionElement : ElementBase
        {
            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                if (GUILayout.Button("Remove")) {
                    parent.RemoveElement(this);
                }
            }
        }
    }
}
