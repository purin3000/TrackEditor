using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor2
{
    public partial class TrackAssetPlayer : MonoBehaviour
    {
        public TrackAsset asset;

        public bool playOnAwake = false;

        [SerializeField]
        float playSpeed = 1.0f;

        public int currentFrame { get => (int)(time * 60.0f); }

        public void SetPlaySpeed(float speed) => playSpeed = speed;

        public float GetPlaySpeed() => playSpeed * asset.playSpeed;
        public float GetPlayScale()
        {
            var speed = GetPlaySpeed();
            if (speed != 0.0f) {
                return 1.0f / speed;
            } else {
                Debug.Assert(false);
                return 0.0f;
            }
        }


        public bool IsPlaying { get => isPlaying; }

        public bool IsPlayEnd { get => !isPlaying || asset.frameLength <= currentFrame; }


        public delegate void OnPlayEnd(TrackAssetPlayer player);
        public delegate void OnPlayStart(TrackAssetPlayer player);

        public event OnPlayStart onPlayStart;
        public event OnPlayEnd onPlayEnd;


        public Dictionary<GameObject, AnimationTrack.PlayerElement> latestPlayRequest = new Dictionary<GameObject, AnimationTrack.PlayerElement>();

        PlayerTrackBase[] playerTracks = { };
        PlayerElementBase[] playerElements = { };

        List<PlayerElementBase> startCommandList;
        List<PlayerElementBase> endCommandList;

        bool isPlaying;
        int startIndex = -1;
        int endIndex = -1;

        float time = 0.0f;

        private void Start()
        {
            if (playOnAwake && asset) {
                Play(asset);
            }
        }

        void Update()
        {
            if (isPlaying) {
                if (0 <= startIndex && time * 60 <= asset.frameLength) {
                    while (startIndex < startCommandList.Count) {
                        var element = startCommandList[startIndex];

                        if (time * 60.0f < element.start) {
                            break;
                        }

                        element.OnElementStart(this);
                        ++startIndex;
                    }


                    while (endIndex < endCommandList.Count) {
                        var element = endCommandList[endIndex];

                        if (time * 60.0f < element.end) {
                            break;
                        }

                        element.OnElementEnd(this);
                        ++endIndex;
                    }


                    time += Time.deltaTime * GetPlaySpeed();

                } else {
                    isPlaying = false;

                    playEnd();
                }
            }
        }

        public void Play()
        {
            isPlaying = true;
            startIndex = 0;
            endIndex = 0;
            time = 0.0f;

            initalizeTracksAndElements();

            startCommandList = playerElements.OrderBy(element => element.start).ToList();
            endCommandList = playerElements.OrderBy(element => element.end).ToList();

            playStart();
        }

        public void Play(TrackAsset asset)
        {
            this.asset = asset;

            Play();
        }

        public void Stop()
        {
            isPlaying = false;
        }


        void playStart()
        {
            foreach (var playerTrack in playerTracks) {
                playerTrack.OnTrackStart(this);
            }

            onPlayStart?.Invoke(this);
        }

        void playEnd()
        {
            foreach (var playerTrack in playerTracks) {
                playerTrack.OnTrackEnd(this);
            }

            onPlayEnd?.Invoke(this);
        }

        PlayerTrackBase getTrackPlayer(int trackIndex)
        {
            if (trackIndex != -1) {
                return playerTracks[trackIndex];
            }
            return null;
        }

        PlayerTrackClass createPlayerTrack<PlayerTrackClass>(AssetTrack assetTrack) where PlayerTrackClass : PlayerTrackBase, new()
        {
            var playerTrack = new PlayerTrackClass();
            playerTrack.Initialize(assetTrack.name, assetTrack.parentTrackIndex);
            return playerTrack;
        }

        PlayerElementClass createPlayerElement<PlayerElementClass>(AssetElement assetElement) where PlayerElementClass : PlayerElementBase, new()
        {
            var playerElement = new PlayerElementClass();
            playerElement.Initialize(assetElement.name, assetElement.parentTrackIndex, assetElement.start, assetElement.length);
            return playerElement;
        }

        public abstract class PlayerTrackBase
        {
            public string name;
            public PlayerTrackBase parent;
            public int parentTrackIndex;

            public void Initialize(string name, int parentTrackIndex)
            {
                this.name = name;
                this.parentTrackIndex = parentTrackIndex;
            }

            public virtual void OnTrackInitialize(TrackAssetPlayer player) { }
            public virtual void OnTrackStart(TrackAssetPlayer player) { }
            public virtual void OnTrackEnd(TrackAssetPlayer player) { }
        }

        public abstract class PlayerElementBase
        {
            public string name;
            public PlayerTrackBase parent;
            public int start;
            public int length;
            public int parentTrackIndex;

            public int end => start + length;

            public void Initialize(string name, int parentTrackIndex, int start, int length)
            {
                this.name = name;
                this.parentTrackIndex = parentTrackIndex;
                this.start = start;
                this.length = length;
            }

            public virtual void OnElementStart(TrackAssetPlayer player) { }
            public virtual void OnElementEnd(TrackAssetPlayer player) { }
        }
    }
}
