using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    public class GameObjectTrack
    {
        [System.Serializable]
        public class SerializeTrack : SerializeTrackBase
        {
            public GameObject target;

            public bool activate;

            public bool currentPlayer;

            public override TrackPlayerBase CreatePlayer()
            {
                return new TrackPlayer(this);
            }
        }

        public class TrackPlayer : TrackPlayerBase
        {
            SerializeTrack serializeTrack;

            public ModelResource model { get; private set; }

            public GameObject gameObject { get; private set; }

            public TrackPlayer(SerializeTrack serializeTrack)
            {
                this.serializeTrack = serializeTrack;
            }

            public override void OnTrackStart(TrackAssetPlayer context)
            {
                //FieldManager.Instance.ChangeMode(FieldManager.Mode.Event);
                //FieldManager.Instance.fieldPlayer.moveByEvent = true;
                //FieldManager.Instance.fieldPlayer.useNavMeshAgent = false;

                //if (serializeTrack.currentPlayer) {
                //    model = FieldManager.Instance.fieldPlayer.characterModel;
                //    gameObject = FieldManager.Instance.fieldPlayer.gameObject;

                //} else {
                //    var go = serializeTrack.target;

                //    var walker = go.GetComponent<FieldWalker>();
                //    if (walker) {
                //        model = walker.characterModel;
                //    } else {
                //        var obj = go.GetComponent<FieldObject>();
                //        if (obj) {
                //            model = obj.model.GetComponent<ModelResource>();
                //        } else {
                //            model = go.GetComponent<ModelResource>();
                //        }
                //    }

                //    if (go) {
                //        go.SetActive(serializeTrack.activate);
                //    }

                //    gameObject = go;
                //}
            }

            public override void OnTrackEnd(TrackAssetPlayer context)
            {
                //FieldManager.Instance.ChangeMode(FieldManager.Mode.FreeWalk);
                //FieldManager.Instance.fieldPlayer.moveByEvent = false;
                //FieldManager.Instance.fieldPlayer.useNavMeshAgent = true;
            }
        }

        public abstract class ElementPlayerImpl : ElementPlayerBase
        {
            protected GameObject GetGameObject()
            {
                var gameObjectTrack = parent.parent as TrackPlayer;

                return gameObjectTrack.gameObject;
            }

            protected ModelResource GetModel()
            {
                var gameObjectTrack = parent.parent as TrackPlayer;

                return gameObjectTrack.model;
            }
        }
    }
}

