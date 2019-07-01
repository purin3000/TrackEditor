using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    using ParentTrack = GameObjectTrack;

    public class ActivationTrack
    {
        [System.Serializable]
        public class TrackData
        {
        }

        [System.Serializable]
        public class ElementData
        {
        }

        public class PlayerTrack : ParentTrack.ChildPlayerTrackBase
        {
            public TrackData trackData;
        }

        public class PlayerElement : ParentTrack.ChildPlayerElementBase
        {
            public ElementData elementData;
        }
    }
}
