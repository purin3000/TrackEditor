using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    using ElementPlayerImpl = CameraTrack.ElementPlayerImpl;

    public class CameraChangeTrack
    {
        [System.Serializable]
        public class SerializeTrack : SerializeTrackBase
        {
        }

        [System.Serializable]
        public class SerializeElement : SerializeElementBase
        {
            //public string startCameraName;
            //public string endCameraName;
            //public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;

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
                //FieldManager.Instance.cameraSwitcher.SwitchCameraFromName(serializeElement.startCameraName);

                //if (!string.IsNullOrEmpty(serializeElement.endCameraName)) {
                //    FieldManager.Instance.cameraSwitcher.SwitchCameraFromName(serializeElement.endCameraName, serializeElement.length / 60.0f * context.GetPlayScale(), serializeElement.easeType);
                //}
            }

            public override void OnEnd(TrackAssetPlayer context)
            {
            }
        }

    }
}

