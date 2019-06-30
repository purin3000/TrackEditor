using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public class RootTrack
    {
        [System.Serializable]
        public class TrackData
        {
        }

        public class PlayerTrack : TrackAssetPlayer2.PlayerTrackBase
        {
            public TrackData trackData;
        }
    }
}
