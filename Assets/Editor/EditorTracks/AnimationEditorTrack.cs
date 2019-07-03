using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using ParentEditorTrack = GameObjectEditorTrack;
    using CurrentElementData = AnimationTrack.ElementData;

    class AnimationEditorElementData : ParentEditorTrack.ChildEditorElementBase
    {
        public const string labelName = "Animation";

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
            DrawNameImpl(labelName);

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
            ElementDrawerImpl(rect);

            if (elementData.clip) {
                GUI.Label(rect, elementData.clip.name);
            }
        }
    }
}
