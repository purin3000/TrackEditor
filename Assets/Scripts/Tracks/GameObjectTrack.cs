using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public class GameObjectTrack
    {
        [System.Serializable]
        public class TrackData
        {
            public GameObject target;

            public bool activate = true;

            public bool currentPlayer;
        }

        public class PlayerTrack : TrackAssetPlayer.PlayerTrackBase
        {
            public TrackData trackData;

            public GameObject gameObject;

            public override void OnTrackStart(TrackAssetPlayer context)
            {
                if (gameObject) {
                    gameObject.SetActive(trackData.activate);
                }
            }
        }

        public class ChildPlayerTrackBase : TrackAssetPlayer.PlayerTrackBase
        {
        }

        public class ChildPlayerElementBase : TrackAssetPlayer.PlayerElementBase
        {
            public GameObject gameObject => (parent.parent as PlayerTrack).gameObject;
        }
    }
}
