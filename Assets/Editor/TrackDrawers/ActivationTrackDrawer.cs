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

            RemoveTrackImpl("Remove Activation Track");

            RemoveElementImpl();
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            GUILayout.Space(15);

            DrawIndexMoveImpl();

            GUILayout.Space(15);

            AddElementImpl<ActivationElement>("Add Activation Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, "Activation");
        }
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

