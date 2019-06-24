using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    public class ActivationTrackData : TrackData
    {
        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            HeaderDrawerImpl("Remove Activation Track");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, "Activation");
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            PropertyDrawerImpl(rect, "Add Activation Element");
        }

        public override TrackElement CreateElement() { return new ActivationElement(); }
    }

    public class ActivationElement : TrackElement
    {
        public override void HeaderDrawer()
        {
            RemoveElementImpl("Remove Activation Elememnt");
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);
        }
    }
}

