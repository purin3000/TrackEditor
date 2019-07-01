using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using ParentEditorTrack = GameObjectEditorTrack;
    using CurrentTrackData = AnimationTrack.TrackData;
    using CurrentElementData = AnimationTrack.ElementData;

    class AnimationEditorTrack
    {
        public const string labelName = "Animation";

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
                //base.PropertyDrawer(rect);

                DrawNameImpl();

                elementData.clip = (AnimationClip)EditorGUILayout.ObjectField("Clip", elementData.clip, typeof(AnimationClip), false);

                DrawStartImpl();
                DrawLengthImpl();

                elementData.blend = EditorGUILayout.IntField("Blend Frame", elementData.blend);

                elementData.speed = EditorGUILayout.Slider("Speed", elementData.speed, 0.0f, 5.0f);

                if (elementData.clip) {
                    if (elementData.speed != 0) {
                        length = Mathf.Max(1, (int)(elementData.clip.length / elementData.speed * 60));
                    } else {
                        length = Mathf.Max(1, parent.manager.frameLength - start);
                    }
                }
            }

            public override void ElementDrawer(Rect rect)
            {
                base.ElementDrawer(rect);

                if (elementData.clip) {
                    GUI.Label(rect, elementData.clip.name);
                }
            }
        }
    }
}
