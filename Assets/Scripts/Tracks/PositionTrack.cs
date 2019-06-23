using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{

    [System.Serializable]
    public class PositionSerializeTrack : SerializeTrack
    {
    }

    [System.Serializable]
    public class PositionSerializeElement : SerializeElement
    {
        public Vector3 localPosition;

        public override IElementPlayer CreatePlayer()
        {
            return new PositionElementPlayer(this);
        }
    }

    public class PositionElementPlayer : IElementPlayer
    {
        PositionSerializeElement elementSerialize;

        public PositionElementPlayer(PositionSerializeElement trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            Debug.LogFormat("Position start:{0}", start);

            gameObjectTrack.target.transform.localPosition = elementSerialize.localPosition;
        }

        //public override void OnEnd(ElementPlayerContext context)
        //{
        //    var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

        //    Debug.LogFormat("Position end:{0}", end);
        //}
    }
}

