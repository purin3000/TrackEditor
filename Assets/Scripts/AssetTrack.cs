using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    [System.Serializable]
    public class AssetTrack
    {
        public int trackIndex;
        public string name;
        public int parentTrackIndex;

        public void Initialize(int trackIndex, string name, int parentTrackIndex)
        {
            this.trackIndex = trackIndex;
            this.name = name;
            this.parentTrackIndex = parentTrackIndex;
        }
    }
}
