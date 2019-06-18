using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace track_editor
{
    public class ElementPlayerContext
    {
        public TrackAsset asset;

        public List<IElementPlayer> playStartElements;
        public List<IElementPlayer> playEndElements;

        Dictionary<string, TrackSerialize> tracks = new Dictionary<string, TrackSerialize>();
        Dictionary<string, ElementSerialize> elements = new Dictionary<string, ElementSerialize>();
        List<IElementPlayer> elementPlayers = new List<IElementPlayer>();

        public ElementPlayerContext(TrackAsset asset)
        {
            this.asset = asset;

            tracks.Clear();
            elements.Clear();
            elementPlayers.Clear();

            foreach (var trackSerialize in asset.rootTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }
            foreach (var trackSerialize in asset.gameObjectTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }
            foreach (var trackSerialize in asset.activationTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }
            foreach (var trackSerialize in asset.positionTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }
            foreach (var trackSerialize in asset.animationTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }

            foreach (var trackSerialize in asset.activationElements) { elements.Add(trackSerialize.uniqueName, trackSerialize); elementPlayers.Add(new ActivationElementPlayer(trackSerialize)); }
            foreach (var trackSerialize in asset.positionElements) { elements.Add(trackSerialize.uniqueName, trackSerialize); elementPlayers.Add(new PositionElementPlayer(trackSerialize)); }
            foreach (var trackSerialize in asset.animationElements) { elements.Add(trackSerialize.uniqueName, trackSerialize); elementPlayers.Add(new AnimationElementPlayer(trackSerialize)); }

            playStartElements = elementPlayers.OrderBy(elem => elem.start).ToList();
            playEndElements = elementPlayers.OrderBy(elem => elem.end).ToList();
        }


        public T GetTrack<T>(ElementSerialize elementSerialize) where T : TrackSerialize
        {
            return tracks[elementSerialize.parent] as T;
        }

        public T GetParentTrack<T>(ElementSerialize elementSerialize) where T : TrackSerialize
        {
            return tracks[tracks[elementSerialize.parent].parent] as T;
        }
    }


    public abstract class IElementPlayer
    {
        public abstract int start { get; }
        public abstract int end{ get; }

        public virtual void OnStart(ElementPlayerContext context) { }
        public virtual void OnEnd(ElementPlayerContext context) { }
    }

    public class ActivationElementPlayer : IElementPlayer
    {
        ActivationElementSerialize elementSerialize;

        public ActivationElementPlayer(ActivationElementSerialize trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(ElementPlayerContext context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

            Debug.LogFormat("Activation start:{0} {1}", start, gameObjectTrack.target);

            gameObjectTrack.target.SetActive(true);
        }

        public override void OnEnd(ElementPlayerContext context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

            gameObjectTrack.target.SetActive(false);

            Debug.LogFormat("Activation end:{0} {1}", end, gameObjectTrack.target);
        }
    }

    public class PositionElementPlayer : IElementPlayer
    {
        PositionElementSerialize elementSerialize;

        public PositionElementPlayer(PositionElementSerialize trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(ElementPlayerContext context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

            Debug.LogFormat("Position start:{0}", start);

            gameObjectTrack.target.transform.localPosition = elementSerialize.localPosition;
        }

        //public override void OnEnd(ElementPlayerContext context)
        //{
        //    var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

        //    Debug.LogFormat("Position end:{0}", end);
        //}
    }

    public class AnimationElementPlayer : IElementPlayer
    {
        AnimationElementSerialize elementSerialize;

        public AnimationElementPlayer(AnimationElementSerialize trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(ElementPlayerContext context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

            Debug.LogFormat("Animation start:{0}", start);

            //gameObjectTrack.target.transform.localPosition = elementSerialize.localPosition;
        }

        public override void OnEnd(ElementPlayerContext context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

            Debug.LogFormat("Position end:{0}", end);
        }
    }
}
