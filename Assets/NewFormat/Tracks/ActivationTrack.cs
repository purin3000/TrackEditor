using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
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

        public class PlayerTrack : TrackAssetPlayer2.PlayerTrackBase
        {
            public TrackData trackData;
        }

        public class PlayerElement : TrackAssetPlayer2.PlayerElementBase
        {
            public ElementData elementData;
        }
    }
}
