﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
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

        public class PlayerTrack : TrackAssetPlayer2.PlayerTrackBase
        {
            public TrackData trackData;
        }

        public class PlayerElement : TrackAssetPlayer2.PlayerElementBase
        {
            public ElementData elementData;

            GameObject gameObject => (parent.parent as GameObjectTrack.PlayerTrack).gameObject;

            public override void OnElementStart(TrackAssetPlayer2 context)
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