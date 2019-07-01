using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    using CurrentTrackData = RootTrack.TrackData;

    class RootEditorTrack
    {
        public class EditorTrackData : EditorTrack
        {
            public CurrentTrackData trackData = new CurrentTrackData();

            public override void TrackHeaderDrawer() { }
            public override void TrackLabelDrawer(Rect rect) { }
            public override void TrackPropertyDrawer(Rect rect) { }
        }
    }
}

