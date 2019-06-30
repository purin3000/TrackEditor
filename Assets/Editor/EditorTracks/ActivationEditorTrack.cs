using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentElementData = ActivationTrack.ElementData;

    public class ActivationEditorTrack : EditorTrack
    {
        public const string labelName = "Activate";

        public ActivationTrack.TrackData trackData = new ActivationTrack.TrackData();

        public ActivationEditorTrack()
        {
            name = labelName;
        }

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            HeaderDrawerImpl($"Remove {labelName} Track");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, labelName);
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            PropertyDrawerImpl(rect, $"Add {labelName} Element");
        }

        public override EditorElement CreateElement() { return new ActivationEditorElement(); }
    }

    public class ActivationEditorElement : EditorElement
    {
        public CurrentElementData elementData = new CurrentElementData();
    }
}
