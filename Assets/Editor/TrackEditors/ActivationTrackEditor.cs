using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    public class ActivationTrackEditor
    {
        public const string name = "Activation";

        public static TrackData CreateTrack() => new Track();

        public class Track : TrackData
        {
            public override void HeaderDrawer()
            {
                base.HeaderDrawer();

                HeaderDrawerImpl($"Remove {name} Track");
            }

            public override void TrackDrawer(Rect rect)
            {
                TrackDrawerImpl(rect, name);
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                PropertyDrawerImpl(rect, $"Add {name} Element");
            }

            public override TrackElement CreateElement() { return new Element(); }
        }

        public class Element : TrackElement
        {
            public override void HeaderDrawer()
            {
                RemoveElementImpl($"Remove {name} Elememnt");
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);
            }
        }
    }
}

