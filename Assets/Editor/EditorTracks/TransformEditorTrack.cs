using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using ParentEditorTrack = GameObjectEditorTrack;
    using CurrentElementData = TransformTrack.ElementData;

    public class TransformEditorElementData : ParentEditorTrack.ChildEditorElementBase
    {
        const string labelName = "Transform";

        public CurrentElementData elementData = new CurrentElementData();

        public override void Initialize()
        {
            isFixedLength = true;
        }

        public override void ElementHeaderDrawer()
        {
            ElementHeaderDrawerImpl(labelName);
        }

        public override void PropertyDrawer(Rect rect)
        {
            PropertyDrawerImpl(rect, labelName);

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

            GUISpace();

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

        public override void ElementDrawer(Rect rect)
        {
            ElementDrawerImpl(rect);
        }
    }
}
