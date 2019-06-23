using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace track_editor
{
    [System.Serializable]
    public class AnimationSerializeTrack : SerializeTrack
    {
    }

    [System.Serializable]
    public class AnimationSerializeElement : SerializeElement
    {
        public int blend;
        public float speed = 1.0f;
        public AnimationClip clip;

        public override IElementPlayer CreatePlayer()
        {
            return new AnimationElementPlayer(this);
        }
    }

#if UNITY_EDITOR
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
#endif

    public class AnimationElementPlayer : IElementPlayer
    {
        AnimationSerializeElement elementSerialize;

        public AnimationElementPlayer(AnimationSerializeElement trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            Debug.LogFormat("Animation start:{0}", start);


            //var anim = gameObjectTrack.target.GetComponent<Animator>();
            //var info = anim.GetCurrentAnimatorStateInfo(0);


            //ModelResource model = null;

            //var walker = gameObjectTrack.target.GetComponent<FieldWalker>();
            //if (walker) {
            //    model = walker.characterModel;
            //} else {
            //    model = gameObjectTrack.target.GetComponent<ModelResource>();
            //}

            //if (model) {
            //    model.animator.speed = elementSerialize.speed;
            //    model.PlayAnimationClip(elementSerialize.clip, elementSerialize.blend / 60.0f, 0.0f);
            //}
        }

        public override void OnEnd(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            Debug.LogFormat("Position end:{0}", end);
        }
    }

}

