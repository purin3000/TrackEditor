using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public class ModelResource : MonoBehaviour
    {
        public Animator animator => null;
        public void PlayAnimationClip(AnimationClip clip, float per, float fff) { }

    }

    public class AnimationTrack
    {
        [System.Serializable]
        public class TrackData
        {
        }

        [System.Serializable]
        public class ElementData
        {
            public int blend;
            public float speed = 1.0f;
            public AnimationClip clip;
        }

        public class PlayerTrack : TrackAssetPlayer.PlayerTrackBase
        {
            public TrackData trackData;
        }

        public class PlayerElement : TrackAssetPlayer.PlayerElementBase
        {
            public ElementData elementData;

            GameObject gameObject => (parent.parent as GameObjectTrack.PlayerTrack).gameObject;

            ModelResource model => (parent.parent as GameObjectTrack.PlayerTrack).model;

            public override void OnElementStart(TrackAssetPlayer context)
            {
                //Debug.LogFormat("Animation start:{0}", start);

                if (model) {
                    model.animator.speed = elementData.speed * context.GetPlaySpeed();
                    model.PlayAnimationClip(elementData.clip, elementData.blend / 60.0f, 0.0f);

                    context.latestPlayRequest[model] = this;
                }
            }

            public override void OnElementEnd(TrackAssetPlayer context)
            {
                if (model) {
                    if (context.latestPlayRequest[model] == this) {
                        // アニメーションが変化していなければ終了処理を呼ぶ
                        Debug.LogFormat("Animation end:{0}", end);
                    }
                }
            }
        }
    }
}
