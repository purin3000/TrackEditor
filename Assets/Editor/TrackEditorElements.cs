using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

namespace track_editor
{
    public class ActivationElement : EditorTrackElement
    {
        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            if (GUILayout.Button("Remove")) {
                parent.manager.RemoveElement(parent, this);
            }
        }

        public void WriteAsset(WriteAssetContext context)
        {
            var elementSerialize = new ActivationElementSerialize();

            SerializeUtility.InitializeElementSerialize(elementSerialize, this, context);

            context.asset.activationElements.Add(elementSerialize);

        }

        public void ReadAsset(ActivationElementSerialize elementSerialize)
        {
        }
    }

    public class PositionElement : EditorTrackElement
    {
        public Vector3 position;

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            if (GUILayout.Button("Remove")) {
                parent.manager.RemoveElement(parent, this);
            }

            position = EditorGUILayout.Vector3Field("Position", position);
        }

        public void WriteAsset(WriteAssetContext context)
        {
            var elementSerialize = new PositionElementSerialize();

            SerializeUtility.InitializeElementSerialize(elementSerialize, this, context);

            elementSerialize.position = position;

            context.asset.positionElements.Add(elementSerialize);

        }

        public void ReadAsset(PositionElementSerialize elementSerialize)
        {
            position = elementSerialize.position;
        }
    }
}
