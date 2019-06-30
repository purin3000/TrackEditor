using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    public class ActivationEditorTrack : EditorTrack
    {
        const string labelName = "Activate";

        public ActivationTrack.TrackData trackData = new ActivationTrack.TrackData();

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
        public ActivationTrack.ElementData elementData = new ActivationTrack.ElementData();
    }
}
