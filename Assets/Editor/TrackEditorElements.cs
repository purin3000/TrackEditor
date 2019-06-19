using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

namespace track_editor
{
    public class TrackElemet : EditorElement
    {
        protected ElementSerializeClass WriteAssetImpl<ElementSerializeClass>(List<ElementSerializeClass> serializeList, WriteAssetContext context) where ElementSerializeClass : SerializeElement, new()
        {
            var elementSerialize = new ElementSerializeClass();

            SerializeUtility.InitializeElementSerialize(elementSerialize, this, context);

            serializeList.Add(elementSerialize);

            return elementSerialize;
        }

        protected void RemoveElementImpl(string label)
        {
            if (GUILayout.Button(label)) {
                parent.manager.RemoveElement(parent, this);
            }

            GUILayout.Space(15);
        }
    }

    public class ActivationElement : TrackElemet
    {
        public override void PropertyDrawer(Rect rect)
        {
            RemoveElementImpl("Remove Activation Elememnt");

            base.PropertyDrawer(rect);
        }

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.activationElements, context);
        }

        public void ReadAsset(ActivationSerializeElement serializeElement)
        {
        }
    }

    public class PositionElement : TrackElemet
    {
        public GameObject target { get => (parent.parent as GameObjectTrackData)?.target; }

        public Vector3 localPosition;

        public override void PropertyDrawer(Rect rect)
        {
            RemoveElementImpl("Remove Position Elememnt");

            base.PropertyDrawer(rect);

            localPosition = EditorGUILayout.Vector3Field("Local Position", localPosition);

            GUILayout.Space(10);

            if (GUILayout.Button("オブジェクトから座標取得")) {
                var go = target;
                if (go) {
                    localPosition = go.transform.localPosition;
                }
            }

            if (GUILayout.Button("オブジェクトへ設定")) {
                var go = target;
                if (go) {
                    go.transform.localPosition = localPosition;
                }
            }
        }

        public void WriteAsset(WriteAssetContext context)
        {
            var elementSerialize = WriteAssetImpl(context.asset.positionElements, context);
            elementSerialize.localPosition = localPosition;
        }

        public void ReadAsset(PositionSerializeElement serializeElement)
        {
            localPosition = serializeElement.localPosition;
        }
    }

    public class AnimationElement : TrackElemet
    {
        public int blend;
        public AnimationClip clip;

        public override void PropertyDrawer(Rect rect)
        {
            RemoveElementImpl("Remove Animation Elememnt");

            base.PropertyDrawer(rect);

            blend = EditorGUILayout.IntField("Blend Frame", blend);

            clip = (AnimationClip)EditorGUILayout.ObjectField("Clip", clip, typeof(AnimationClip), false);
        }

        public void WriteAsset(WriteAssetContext context)
        {
            var elementSerialize = WriteAssetImpl(context.asset.animationElements, context);
            elementSerialize.blend = blend;
            elementSerialize.clip = clip;
        }

        public void ReadAsset(AnimationSerializeElement serializeElement)
        {
            blend = serializeElement.blend;
            clip = serializeElement.clip;
        }
    }

}
