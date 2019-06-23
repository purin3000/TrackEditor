using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            //    model.animator.speed = elementSerialize.speed * context.speed;
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

