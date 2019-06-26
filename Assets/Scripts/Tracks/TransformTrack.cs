using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    using ElementPlayerImpl = GameObjectTrack.ElementPlayerImpl;

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


        [System.Serializable]
        public class SerializeTrack : SerializeTrackBase
        {
        }

        [System.Serializable]
        public class SerializeElement : SerializeElementBase
        {
            public Vector3 localPosition;
            public Quaternion localRotation = Quaternion.identity;
            public Vector3 localScale = Vector3.one;

            public bool usePosition = true;
            public bool useRotation = true;
            public bool useScale = true;

            public override ElementPlayerBase CreatePlayer()
            {
                return new ElementPlayer(this);
            }
        }

        public class ElementPlayer : ElementPlayerImpl
        {
            SerializeElement elementSerialize;

            public ElementPlayer(SerializeElement trackSerialize) { this.elementSerialize = trackSerialize; }

            public override int start { get => elementSerialize.start; }
            public override int end { get => elementSerialize.end; }

            public override void OnStart(TrackAssetPlayer context)
            {
                var go = GetGameObject();

                if (elementSerialize.usePosition) {
                    go.transform.localPosition = elementSerialize.localPosition;
                }

                if (elementSerialize.useRotation) {
                    go.transform.localRotation = elementSerialize.localRotation;
                }

                if (elementSerialize.useScale) {
                    go.transform.localScale = elementSerialize.localScale;
                }
            }
        }
    }
}

