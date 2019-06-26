using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    public class CameraTrackEditor
    {
        public const string name = "Camera";

        public static TrackData CreateTrack() => new Track();

        public class Track : TrackData
        {
            public override void HeaderDrawer()
            {
                base.HeaderDrawer();

                if (selectionElement == null) {
                    RemoveTrackImpl($"Remove {name} Track");

                } else {
                    RemoveElementImpl();
                }
            }

            public override void TrackDrawer(Rect rect)
            {
                base.TrackDrawer(rect);

                Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
                Rect rectObj = new Rect(rectLabel.x, rect.y + (rectLabel.height - EditorGUIUtility.singleLineHeight) * 0.5f, rectLabel.width * 0.6f, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(rectObj, $"{name} {CameraTrackEditor.name}");
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                //DrawIndexMoveImpl();

                //AddElementImpl($"Add {name} Element");

                DrawIndexMoveImpl();

                if (GUILayout.Button($"Add {CameraChangeTrackEditor.name} Track")) {
                    manager.AddTrack(this, CameraChangeTrackEditor.name, CameraChangeTrackEditor.CreateTrack());
                }

                if (GUILayout.Button($"Add {ChangeBgMaterialTrackEditor.name} Track")) {
                    manager.AddTrack(this, ChangeBgMaterialTrackEditor.name, ChangeBgMaterialTrackEditor.CreateTrack());
                }
            }
        }
    }
}