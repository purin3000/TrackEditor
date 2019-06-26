using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    using CurrentSerializeElement = AnimationTrack.SerializeElement;

    public class AnimationTrackEditor
    {
        public const string name = "Animation";

        public static TrackData CreateTrack() => new Track();

        public class Track : TrackData
        {
            public override void HeaderDrawer()
            {
                base.HeaderDrawer();

                HeaderDrawerImpl($"Remove {name} Track");
            }

            public override void TrackDrawer(Rect rect)
            {
                TrackDrawerImpl(rect, name);
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                PropertyDrawerImpl(rect, $"Add {name} Element");
            }

            public override TrackElement CreateElement() { return new Element(); }
        }

        public class Element : TrackElement
        {
            public int blend;
            public AnimationClip clip;
            public float speed = 1.0f;

            public Element()
            {
                isFixedLength = true;
            }

            public override void HeaderDrawer()
            {
                RemoveElementImpl($"Remove {name} Elememnt");
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

            public override void WriteAsset(SerializeElementBase serializeElement)
            {
                var serialize = serializeElement as CurrentSerializeElement;
                serialize.blend = blend;
                serialize.clip = clip;
                serialize.speed = speed;
            }

            public override void ReadAsset(SerializeElementBase serializeElement)
            {
                var serialize = serializeElement as CurrentSerializeElement;
                blend = serialize.blend;
                clip = serialize.clip;
                speed = serialize.speed;
            }
        }
    }
}