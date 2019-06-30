using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentTrackData = CameraChangeTrack.TrackData;
    using CurrentElementData = CameraChangeTrack.ElementData;

    public class CameraChangeEditorTrack : EditorTrack
    {
        public const string labelName = "CamChange";

        public CurrentTrackData trackData = new CurrentTrackData();

        public CameraChangeEditorTrack()
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

        public override EditorElement CreateElement() { return new CameraChangeEditorElement(); }
    }

    class CameraChangeEditorElement : EditorElement
    {
        public CurrentElementData elementData = new CurrentElementData();

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            elementData.startCameraName = EditorGUILayout.TextField("Start CameraName", elementData.startCameraName);
            elementData.endCameraName = EditorGUILayout.TextField("End CameraName", elementData.endCameraName);
            //elementData.easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup("EaseType", elementData.easeType);
        }
    }
}
