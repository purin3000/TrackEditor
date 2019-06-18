using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor
{
    public class TrackAssetPlayer : MonoBehaviour
    {
        public TrackAsset asset;

        public bool playOnAwake = false;

        public bool IsPlaying { get => 0 <= playCurrent; }

        public bool IsPlayEnd { get => playCurrent == playElements.Count; }

        Dictionary<string, TrackSerialize> tracks = new Dictionary<string, TrackSerialize>();
        Dictionary<string, ElementSerialize> elements = new Dictionary<string, ElementSerialize>();

        List<IElementPlayer> playElements = new List<IElementPlayer>();

        int playCurrent = -1;

        float time = 0.0f;
        float speed = 1.0f;





        interface IElementPlayer
        {
            int start { get; }
            int length { get; }

            void Apply();
        }

        class ActivationElementPlayer : IElementPlayer
        {
            ActivationElementSerialize trackSerialize;

            public ActivationElementPlayer(ActivationElementSerialize trackSerialize) { this.trackSerialize = trackSerialize; }

            public int start { get => trackSerialize.start; }
            public int length { get => trackSerialize.length; }

            public void Apply()
            {
                Debug.LogFormat("Activation start:{0}", start);
            }
        }

        class PositionElementPlayer : IElementPlayer
        {
            PositionElementSerialize trackSerialize;

            public PositionElementPlayer(PositionElementSerialize trackSerialize) { this.trackSerialize = trackSerialize; }

            public int start { get => trackSerialize.start; }
            public int length { get => trackSerialize.length; }

            public void Apply()
            {
                Debug.LogFormat("Position start:{0}", start);
            }
        }



        private void Play(TrackAsset asset)
        {
            this.asset = asset;
        }

        private void Play()
        {
            playCurrent = 0;
            time = 0.0f;

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

        void Start()
        {
            if (playOnAwake) {
                Play();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (IsPlaying) {

                while (playCurrent < playElements.Count) {
                    var element = playElements[playCurrent];

                    if (time * 60.0f < element.start) {
                        break;
                    }

                    element.Apply();
                    ++playCurrent;
                }

                time += Time.deltaTime * speed;
            }
        }
    }
}

