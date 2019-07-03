using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    using ParentTrack = GameObjectTrack;

    public class AnimationTrack
    {
        [System.Serializable]
        public class ElementData
        {
            public int blend;
            public float speed = 1.0f;
            public AnimationClip clip;
        }

        public class PlayerTrack : ParentTrack.ChildPlayerTrackBase
        {
        }

        public class PlayerElement : ParentTrack.ChildPlayerElementBase
        {
            public ElementData elementData;

            public override void OnElementStart(TrackAssetPlayer context)
            {
            }

            public override void OnElementEnd(TrackAssetPlayer context)
            {
            }
        }
    }
}
