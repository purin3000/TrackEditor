using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentTrackData = AnimationTrack.TrackData;
    using CurrentElementData = AnimationTrack.ElementData;

    public class AnimationEditorTrack : EditorTrack
    {
        public const string labelName = "Animation";

        public CurrentTrackData trackData = new CurrentTrackData();

        public AnimationEditorTrack()
        {
            name = labelName;
        }

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            HeaderDrawerImpl($"Remove {name} Track");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, labelName);
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            PropertyDrawerImpl(rect, $"Add {labelName} Element");
        }

        public override EditorElement CreateElement() { return new AnimationEditorElement(); }
    }

    class AnimationEditorElement : EditorElement
    {
        public CurrentElementData elementData = new CurrentElementData();

        public AnimationEditorElement()
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
