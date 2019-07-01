using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    using ParentTrack = GameObjectTrack;

    public class TransformTrack
    {
        [System.Serializable]
        public class TrackData
        {
        }

        [System.Serializable]
        public class ElementData
        {
            public Vector3 localPosition;
            public Quaternion localRotation = Quaternion.identity;
            public Vector3 localScale = Vector3.one;

            public bool usePosition = true;
            public bool useRotation = true;
            public bool useScale = true;
        }

        public class PlayerTrack : ParentTrack.ChildPlayerTrackBase
        {
            public TrackData trackData;
        }

        public class PlayerElement : ParentTrack.ChildPlayerElementBase
        {
            public ElementData elementData;

            public override void OnElementStart(TrackAssetPlayer context)
            {
                GameObject go = gameObject;

                if (elementData.usePosition) {
                    go.transform.localPosition = elementData.localPosition;
                }

                if (elementData.useRotation) {
                    go.transform.localRotation = elementData.localRotation;
                }

                if (elementData.useScale) {
                    go.transform.localScale = elementData.localScale;
                }
            }
        }
    }
}
