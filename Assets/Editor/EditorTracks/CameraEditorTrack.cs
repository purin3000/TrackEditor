using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentTrackData = CameraTrack.TrackData;

    public class CameraEditorTrack : EditorTrack
    {
        const string labelName = "Camera";

        public CurrentTrackData trackData = new CurrentTrackData();

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl($"Remove {labelName} Track");
        }

        public override void TrackDrawer(Rect rect)
        {
            base.TrackDrawer(rect);

            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
            Rect rectObj = new Rect(rectLabel.x, rect.y + (rectLabel.height - EditorGUIUtility.singleLineHeight) * 0.5f, rectLabel.width * 0.6f, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rectObj, $"{name} {labelName}");
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            //DrawIndexMoveImpl();

            //AddElementImpl($"Add {name} Element");

            DrawIndexMoveImpl();

            if (GUILayout.Button($"Add {CameraChangeEditorTrack.labelName} Track")) {
                manager.AddTrack(this, new CameraChangeEditorTrack());
            }

            if (GUILayout.Button($"Add {ChangeBgMaterialEditorTrack.labelName} Track")) {
                manager.AddTrack(this, new ChangeBgMaterialEditorTrack());
            }
        }
    }
}
