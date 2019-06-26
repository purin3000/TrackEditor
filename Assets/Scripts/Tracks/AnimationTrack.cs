using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    using ElementPlayerImpl = GameObjectTrack.ElementPlayerImpl;

    public class AnimationTrack
    {
        [System.Serializable]
        public class SerializeTrack : SerializeTrackBase
        {
        }

        [System.Serializable]
        public class SerializeElement : SerializeElementBase
        {
            public int blend;
            public float speed = 1.0f;
            public AnimationClip clip;

            public override ElementPlayerBase CreatePlayer()
            {
                return new ElementPlayer(this);
            }
        }

        public class ElementPlayer : ElementPlayerImpl
        {
            SerializeElement serializeElement;

            public ElementPlayer(SerializeElement serializeElement) {
                this.serializeElement = serializeElement;
            }

            public override int start { get => serializeElement.start; }
            public override int end { get => serializeElement.end; }

            public override void OnStart(TrackAssetPlayer context)
            {
                //var model = GetModel();
                ////Debug.LogFormat("Animation start:{0}", start);

                //if (model) {
                //    model.animator.speed = serializeElement.speed * context.GetPlaySpeed();
                //    model.PlayAnimationClip(serializeElement.clip, serializeElement.blend / 60.0f, 0.0f);

                //    context.latestPlayRequest[model] = this;
                //}
            }

            public override void OnEnd(TrackAssetPlayer context)
            {
                //var model = GetModel();
                //if (model) {
                //    if (context.latestPlayRequest[model] == this) {
                //        // アニメーションが変化していなければ終了処理を呼ぶ
                //        Debug.LogFormat("Animation end:{0}", end);
                //    }
                //}
            }
        }
    }
}

