using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    using ElementPlayerImpl = CameraTrack.ElementPlayerImpl;

    public class ChangeBgMaterialTrack
    {
        [System.Serializable]
        public class SerializeTrack : SerializeTrackBase
        {
        }

        [System.Serializable]
        public class SerializeElement : SerializeElementBase
        {
            public Material material;

            public override ElementPlayerBase CreatePlayer()
            {
                return new ElementPlayer(this);
            }
        }

        public class ElementPlayer : ElementPlayerImpl
        {
            SerializeElement serializeElement;

            public ElementPlayer(SerializeElement serializeElement) { this.serializeElement = serializeElement; }

            public override int start { get => serializeElement.start; }
            public override int end { get => serializeElement.end; }

            public override void OnStart(TrackAssetPlayer context)
            {
                //FieldManager.Instance.cameraSwitcher.ChangeBgMaterial(serializeElement.material);
            }
        }
    }
}

