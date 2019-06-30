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

            public bool activate;

            public bool currentPlayer;
        }

        public class PlayerTrack : TrackAssetPlayer2.PlayerTrackBase
        {
            public TrackData trackData;

            public ModelResource model;

            public GameObject gameObject;

            //public override void OnTrackStart(TrackAssetPlayer2 context)
            //{
            //    FieldManager.Instance.ChangeMode(FieldManager.Mode.Event);
            //    FieldManager.Instance.fieldPlayer.moveByEvent = true;
            //    FieldManager.Instance.fieldPlayer.useNavMeshAgent = false;

            //    if (trackData.currentPlayer) {
            //        model = FieldManager.Instance.fieldPlayer.characterModel;
            //        gameObject = FieldManager.Instance.fieldPlayer.gameObject;

            //    } else {
            //        var go = trackData.target;

            //        var walker = go.GetComponent<FieldWalker>();
            //        if (walker) {
            //            model = walker.characterModel;
            //        } else {
            //            var obj = go.GetComponent<FieldObject>();
            //            if (obj) {
            //                model = obj.model.GetComponent<ModelResource>();
            //            } else {
            //                model = go.GetComponent<ModelResource>();
            //            }
            //        }

            //        if (go) {
            //            go.SetActive(trackData.activate);
            //        }

            //        gameObject = go;
            //    }
            //}

            //public override void OnTrackEnd(TrackAssetPlayer2 context)
            //{
            //    FieldManager.Instance.ChangeMode(FieldManager.Mode.FreeWalk);
            //    FieldManager.Instance.fieldPlayer.moveByEvent = false;
            //    FieldManager.Instance.fieldPlayer.useNavMeshAgent = true;
            //}
        }
    }
}
