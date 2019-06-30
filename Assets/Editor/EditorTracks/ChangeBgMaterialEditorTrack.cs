using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentTrackData = ChangeBgMaterialTrack.TrackData;
    using CurrentElementData = ChangeBgMaterialTrack.ElementData;

    public class ChangeBgMaterialEditorTrack : EditorTrack
    {
        public const string labelName = "ChangeBgMat";

        public CurrentTrackData trackData = new CurrentTrackData();

        public ChangeBgMaterialEditorTrack()
        {
            name = labelName;
        }

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            HeaderDrawerImpl($"Remove {name} Track");
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

        public override EditorElement CreateElement() { return new ChangeBgMaterialEditorElement(); }
    }

    class ChangeBgMaterialEditorElement : EditorElement
    {
        public CurrentElementData elementData = new CurrentElementData();

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            elementData.material = (Material)EditorGUILayout.ObjectField("Material", elementData.material, typeof(Material), true);
        }
    }
}
