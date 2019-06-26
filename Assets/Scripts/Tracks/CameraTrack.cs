using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    public class CameraTrack
    {
        [System.Serializable]
        public class SerializeTrack : SerializeTrackBase
        {
            public override TrackPlayerBase CreatePlayer()
            {
                return new TrackPlayer(this);
            }
        }

        public class TrackPlayer : TrackPlayerBase
        {
            SerializeTrack serializeTrack;

            //CameraImageInfo restoreCamera;

            public TrackPlayer(SerializeTrack serializeTrack)
            {
                this.serializeTrack = serializeTrack;
            }

            public override void OnTrackStart(TrackAssetPlayer context)
            {
                //restoreCamera = FieldManager.Instance.cameraSwitcher.CurrentCameraImageInfo;
            }

            public override void OnTrackEnd(TrackAssetPlayer context)
            {
                //FieldManager.Instance.cameraSwitcher.SwitchCameraFromName(restoreCamera.name);
                //FieldManager.Instance.cameraSwitcher.Zoom(1.0f);
            }
        }

        public abstract class ElementPlayerImpl : ElementPlayerBase
        {

        }
    }
}

