using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using ParentEditorTrack = GameObjectEditorTrack;
    using CurrentTrackData = ActivationTrack.TrackData;
    using CurrentElementData = ActivationTrack.ElementData;
    
    public class ActivationEditorTrack
    {
        public const string labelName = "Activate";

        public class EditorTrackData : ParentEditorTrack.ChildEditorTrackBase
        {
            public CurrentTrackData trackData = new CurrentTrackData();

            public EditorTrackData()
            {
                name = labelName;
            }

            public override void TrackHeaderDrawer()
            {
                HeaderDrawerImpl(labelName);
            }

            public override void TrackLabelDrawer(Rect rect)
            {
                SubTrackLabelDrawerImpl(rect, labelName);
            }

            public override void TrackPropertyDrawer(Rect rect)
            {
                SubTrackPropertyDrawerImpl(rect, labelName);
            }

            public override EditorElement CreateElement() { return new EditorElementData(); }
        }

        public class EditorElementData : ParentEditorTrack.ChildEditorElementBase
        {
            public CurrentElementData elementData = new CurrentElementData();
        }
    }
}
