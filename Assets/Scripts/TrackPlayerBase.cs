using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace track_editor
{
    public class TrackPlayerBase
    {
        public TrackPlayerBase parent { get; private set; }

        public virtual void Initialize(TrackPlayerBase parent, TrackAssetPlayer context)
        {
            this.parent = parent;
        }

        public virtual void OnTrackStart(TrackAssetPlayer context) { }
        public virtual void OnTrackEnd(TrackAssetPlayer context) { }
    }
}
