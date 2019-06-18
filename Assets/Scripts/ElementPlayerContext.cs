using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace track_editor
{
    public class ElementPlayerContext
    {
        public TrackAsset asset;

        public List<IElementPlayer> playElements = new List<IElementPlayer>();

        Dictionary<string, TrackSerialize> tracks = new Dictionary<string, TrackSerialize>();
        Dictionary<string, ElementSerialize> elements = new Dictionary<string, ElementSerialize>();

        public ElementPlayerContext(TrackAsset asset)
        {
            this.asset = asset;

            tracks.Clear();
            elements.Clear();
            playElements.Clear();

            foreach (var trackSerialize in asset.rootTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }
            foreach (var trackSerialize in asset.gameObjectTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }
            foreach (var trackSerialize in asset.activationTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }
            foreach (var trackSerialize in asset.positionTracks) { tracks.Add(trackSerialize.uniqueName, trackSerialize); }

            foreach (var trackSerialize in asset.activationElements) { elements.Add(trackSerialize.uniqueName, trackSerialize); playElements.Add(new ActivationElementPlayer(trackSerialize)); }
            foreach (var trackSerialize in asset.positionElements) { elements.Add(trackSerialize.uniqueName, trackSerialize); playElements.Add(new PositionElementPlayer(trackSerialize)); }

            playElements = playElements.OrderBy(elem => elem.start).ToList();
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


    public interface IElementPlayer
    {
        int start { get; }
        int length { get; }

        void Apply(ElementPlayerContext context);
    }

    public class ActivationElementPlayer : IElementPlayer
    {
        ActivationElementSerialize elementSerialize;

        public ActivationElementPlayer(ActivationElementSerialize trackSerialize) { this.elementSerialize = trackSerialize; }

        public int start { get => elementSerialize.start; }
        public int length { get => elementSerialize.length; }

        public void Apply(ElementPlayerContext context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

            Debug.LogFormat("Activation start:{0} {1}", start, gameObjectTrack.go);
        }
    }

    public class PositionElementPlayer : IElementPlayer
    {
        PositionElementSerialize elementSerialize;

        public PositionElementPlayer(PositionElementSerialize trackSerialize) { this.elementSerialize = trackSerialize; }

        public int start { get => elementSerialize.start; }
        public int length { get => elementSerialize.length; }

        public void Apply(ElementPlayerContext context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

            Debug.LogFormat("Position start:{0}", start);
        }
    }
}
