using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using ParentEditorTrack = GameObjectEditorTrack;
    using CurrentTrackData = TransformTrack.TrackData;
    using CurrentElementData = TransformTrack.ElementData;

    public class TransformEditorTrack
    {
        public const string labelName = "Transform";

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

            public EditorElementData()
            {
                isFixedLength = true;
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                using (new GUILayout.VerticalScope("box")) {
                    elementData.usePosition = EditorGUILayout.Toggle("Use Position", elementData.usePosition);
                    elementData.localPosition = EditorGUILayout.Vector3Field("Local Position", elementData.localPosition);
                }

                using (new GUILayout.VerticalScope("box")) {
                    elementData.useRotation = EditorGUILayout.Toggle("Use Rotation", elementData.useRotation);
                    elementData.localRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Local Rotation", elementData.localRotation.eulerAngles));
                }

                using (new GUILayout.VerticalScope("box")) {
                    elementData.useScale = EditorGUILayout.Toggle("Use Scale", elementData.useScale);
                    elementData.localScale = EditorGUILayout.Vector3Field("Local Scale", elementData.localScale);
                }

                GUILayout.Space(10);

                if (GUILayout.Button("オブジェクトから座標取得")) {
                    var go = target;
                    if (go) {
                        elementData.localPosition = go.transform.localPosition;
                        elementData.localRotation = go.transform.localRotation;
                        elementData.localScale = go.transform.localScale;
                    }
                }

                if (GUILayout.Button("オブジェクトへ設定")) {
                    var go = target;
                    if (go) {
                        go.transform.localPosition = elementData.localPosition;
                        go.transform.localRotation = elementData.localRotation;
                        go.transform.localScale = elementData.localScale;
                    }
                }
            }
        }
    }
}
