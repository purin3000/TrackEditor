using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.animationTracks, context);
        }

        public void ReadAsset(AnimationSerializeTrack serializeTrack)
        {
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


            var anim = gameObjectTrack.target.GetComponent<Animator>();
            var info = anim.GetCurrentAnimatorStateInfo(0);


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

