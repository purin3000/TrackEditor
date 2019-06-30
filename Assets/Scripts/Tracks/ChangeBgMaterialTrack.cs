using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public class ChangeBgMaterialTrack
    {
        [System.Serializable]
        public class TrackData
        {
        }

        [System.Serializable]
        public class ElementData
        {
            public Material material;
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
                //FieldManager.Instance.cameraSwitcher.ChangeBgMaterial(elementData.material);
            }
        }
    }
}
