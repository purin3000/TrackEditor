using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    [System.Serializable]
    public class ActivationSerializeTrack : SerializeTrack
    {
    }

    /// <summary>
    /// エレメントのシリアライズデータ
    /// </summary>
    [System.Serializable]
    public class ActivationSerializeElement : SerializeElement
    {
        public override IElementPlayer CreatePlayer()
        {
            return new ActivationElementPlayer(this);
        }
    }

    public class ActivationTrackData : TrackData
    {
#if UNITY_EDITOR
        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl("Remove Activation Track");

            RemoveElementImpl();
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            GUILayout.Space(15);

            DrawIndexMoveImpl();

            GUILayout.Space(15);

            AddElementImpl<ActivationElement>("Add Activation Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, "Activation");
        }
#endif
    }

    public class ActivationElement : TrackElement
    {
#if UNITY_EDITOR
        public override void HeaderDrawer()
        {
            RemoveElementImpl("Remove Activation Elememnt");
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);
        }
#endif
    }

    public class ActivationElementPlayer : IElementPlayer
    {
        ActivationSerializeElement elementSerialize;

        public ActivationElementPlayer(ActivationSerializeElement trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            Debug.LogFormat("Activation start:{0} {1}", start, gameObjectTrack.target);

            gameObjectTrack.target.SetActive(true);
        }

        public override void OnEnd(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            gameObjectTrack.target.SetActive(false);

            Debug.LogFormat("Activation end:{0} {1}", end, gameObjectTrack.target);
        }
    }

}

