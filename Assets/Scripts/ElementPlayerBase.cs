using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace track_editor
{
    public abstract class ElementPlayerBase
    {
        protected TrackPlayerBase parent { get; private set; }

        public abstract int start { get; }
        public abstract int end{ get; }

        public virtual void Initialize(TrackPlayerBase parent, TrackAssetPlayer context)
        {
            this.parent = parent;
        }

        public virtual void OnStart(TrackAssetPlayer context) { }
        public virtual void OnEnd(TrackAssetPlayer context) { }
    }
}
