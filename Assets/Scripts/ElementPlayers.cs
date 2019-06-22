using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace track_editor
{
    public abstract class IElementPlayer
    {
        public abstract int start { get; }
        public abstract int end{ get; }

        public virtual void OnStart(TrackAssetPlayer context) { }
        public virtual void OnEnd(TrackAssetPlayer context) { }
    }

    public class ActivationElementPlayer : IElementPlayer
    {
        ActivationSerializeElement elementSerialize;

        public ActivationElementPlayer(ActivationSerializeElement trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            Debug.LogFormat("Activation start:{0} {1}", start, gameObjectTrack.target);

            gameObjectTrack.target.SetActive(true);
        }

        public override void OnEnd(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            gameObjectTrack.target.SetActive(false);

            Debug.LogFormat("Activation end:{0} {1}", end, gameObjectTrack.target);
        }
    }

    public class PositionElementPlayer : IElementPlayer
    {
        PositionSerializeElement elementSerialize;

        public PositionElementPlayer(PositionSerializeElement trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            Debug.LogFormat("Position start:{0}", start);

            gameObjectTrack.target.transform.localPosition = elementSerialize.localPosition;
        }

        //public override void OnEnd(ElementPlayerContext context)
        //{
        //    var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

        //    Debug.LogFormat("Position end:{0}", end);
        //}
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
