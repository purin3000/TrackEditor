using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public class CameraChangeTrack
    {
        [System.Serializable]
        public class TrackData
        {
        }

        [System.Serializable]
        public class ElementData
        {
            public string startCameraName;
            public string endCameraName;
            //public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;

        }

        public class PlayerTrack : TrackAssetPlayer.PlayerTrackBase
        {
            public TrackData trackData;
        }

        public class PlayerElement : TrackAssetPlayer.PlayerElementBase
        {
            public ElementData elementData;

            public override void OnElementStart(TrackAssetPlayer context)
            {
                //FieldManager.Instance.cameraSwitcher.SwitchCameraFromName(elementData.startCameraName);

                //if (!string.IsNullOrEmpty(elementData.endCameraName)) {
                //    FieldManager.Instance.cameraSwitcher.SwitchCameraFromName(elementData.endCameraName, length / 60.0f * context.GetPlayScale(), elementData.easeType);                }
            }
        }
    }
}
