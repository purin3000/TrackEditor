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
    }

    public class ActivationElement : TrackElemet
    {
        public override void HeaderDrawer()
        {
            RemoveElementImpl("Remove Activation Elememnt");
        }

        public override void PropertyDrawer(Rect rect)
        {
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

        public override void HeaderDrawer()
        {
            RemoveElementImpl("Remove Position Elememnt");
        }

        public override void PropertyDrawer(Rect rect)
        {
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
        public float speed = 1.0f;

        public override void HeaderDrawer()
        {
            RemoveElementImpl("Remove Animation Elememnt");
        }

        public override void PropertyDrawer(Rect rect)
        {
            //base.PropertyDrawer(rect);

            DrawNameImpl();

            clip = (AnimationClip)EditorGUILayout.ObjectField("Clip", clip, typeof(AnimationClip), false);

            DrawStartImpl();
            DrawLengthImpl();

            blend = EditorGUILayout.IntField("Blend Frame", blend);

            speed = EditorGUILayout.Slider("Speed", speed, 0.0f, 5.0f);

            if (clip) {
                if (speed != 0) {
                    length = Mathf.Max(1, (int)(clip.length / speed * 60));
                } else {
                    length = Mathf.Max(1, parent.manager.frameLength - start);
                }
            }
        }

        public override void ElementDrawer(Rect rect)
        {
            base.ElementDrawer(rect);

            if (clip) {
                GUI.Label(rect, clip.name);
            }
        }


        public void WriteAsset(WriteAssetContext context)
        {
            var elementSerialize = WriteAssetImpl(context.asset.animationElements, context);
            elementSerialize.blend = blend;
            elementSerialize.clip = clip;
            elementSerialize.speed = speed;
        }

        public void ReadAsset(AnimationSerializeElement serializeElement)
        {
            blend = serializeElement.blend;
            clip = serializeElement.clip;
            speed = serializeElement.speed;
        }
    }

}
