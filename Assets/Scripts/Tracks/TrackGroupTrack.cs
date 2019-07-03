using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public class TrackGroupTrack
    {
        [System.Serializable]
        public class TrackData
        {
            public bool activate = true;
        }

        public class PlayerTrack : TrackAssetPlayer.PlayerTrackBase
        {
            public TrackData trackData;
        }
    }
}
