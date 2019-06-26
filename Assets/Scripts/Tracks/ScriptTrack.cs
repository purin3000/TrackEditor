using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    public class ScriptTrack
    {

        [System.Serializable]
        public class SerializeTrack : SerializeTrackBase
        {
        }

        /// <summary>
        /// エレメントのシリアライズデータ
        /// </summary>
        [System.Serializable]
        public class SerializeElement : SerializeElementBase
        {
            public override ElementPlayerBase CreatePlayer()
            {
                return new ElementPlayer(this);
            }
        }

        public class ElementPlayer : ElementPlayerBase
        {
            SerializeElement elementSerialize;

            public ElementPlayer(SerializeElement trackSerialize) { this.elementSerialize = trackSerialize; }

            public override int start { get => elementSerialize.start; }
            public override int end { get => elementSerialize.end; }

            public override void OnStart(TrackAssetPlayer context)
            {
                //Debug.LogFormat("Activation start:{0} {1}", start, gameObjectTrack.target);
            }

            public override void OnEnd(TrackAssetPlayer context)
            {
                //Debug.LogFormat("Activation end:{0} {1}", end, gameObjectTrack.target);
            }
        }
    }
}

