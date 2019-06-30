using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public class CameraTrack
    {
        [System.Serializable]
        public class TrackData
        {
        }

        public class PlayerTrack : TrackAssetPlayer.PlayerTrackBase
        {
            public TrackData trackData;

            //CameraImageInfo restoreCamera;

            //public override void OnTrackStart(TrackAssetPlayer context)
            //{
            //    restoreCamera = FieldManager.Instance.cameraSwitcher.CurrentCameraImageInfo;
            //}

            //public override void OnTrackEnd(TrackAssetPlayer context)
            //{
            //    FieldManager.Instance.cameraSwitcher.SwitchCameraFromName(restoreCamera.name);
            //    FieldManager.Instance.cameraSwitcher.Zoom(1.0f);
            //}
        }
    }
}
