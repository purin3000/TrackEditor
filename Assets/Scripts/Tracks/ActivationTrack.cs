﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    using ParentTrack = GameObjectTrack;

    public class ActivationTrack
    {
        [System.Serializable]
        public class ElementData
        {
        }

        public class PlayerTrack : ParentTrack.ChildPlayerTrackBase
        {
        }

        public class PlayerElement : ParentTrack.ChildPlayerElementBase
        {
            public ElementData elementData;

            public override void OnElementStart(TrackAssetPlayer player)
            {
                gameObject.SetActive(true);
            }

            public override void OnElementEnd(TrackAssetPlayer player)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
