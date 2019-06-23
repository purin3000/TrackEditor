using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    public class AnimationTrackData : TrackData
    {
        public AnimationTrackData()
        {
            isFixedLength = true;
        }

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl("Remove Animation Track");

            RemoveElementImpl();
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            GUILayout.Space(15);

            DrawIndexMoveImpl();

            GUILayout.Space(15);

            AddElementImpl<AnimationElement>("Add Animation Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, "Animation");
        }
    }

    public class AnimationElement : TrackElement
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

        public override void WriteAsset(SerializeElement serializeElement)
        {
            var serialize = serializeElement as AnimationSerializeElement;
            serialize.blend = blend;
            serialize.clip = clip;
            serialize.speed = speed;
        }

        public override void ReadAsset(SerializeElement serializeElement)
        {
            var serialize = serializeElement as AnimationSerializeElement;
            blend = serialize.blend;
            clip = serialize.clip;
            speed = serialize.speed;
        }
    }
}