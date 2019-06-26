using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor
{
    public delegate void OnPlayEnd(TrackAssetPlayer player);
    public delegate void OnPlayStart(TrackAssetPlayer player);

    public class ModelResource { }


    public partial class TrackAssetPlayer : MonoBehaviour
    {
        public TrackAsset asset;

        public bool playOnAwake = false;

        [SerializeField]
        float playSpeed = 1.0f;

        public int currentFrame { get => (int)(time * 60.0f); }

        public void SetPlaySpeed(float speed) => playSpeed = speed;

        public float GetPlaySpeed() => playSpeed * asset.playSpeed;
        public float GetPlayScale() {
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

        public event OnPlayStart onPlayStart;
        public event OnPlayEnd onPlayEnd;


        public Dictionary<ModelResource, AnimationTrack.ElementPlayer> latestPlayRequest = new Dictionary<ModelResource, AnimationTrack.ElementPlayer>();

        Dictionary<string, TrackInfo> trackInfoTable = new Dictionary<string, TrackInfo>();

        List<TrackPlayerBase> trackPlayers = new List<TrackPlayerBase>();
        List<ElementPlayerBase> elementPlayers = new List<ElementPlayerBase>();

        List<ElementPlayerBase> startCommandList;
        List<ElementPlayerBase> endCommandList;

        bool isPlaying;
        int startIndex = -1;
        int endIndex = -1;

        float time = 0.0f;


        class TrackInfo
        {
            public TrackInfo(SerializeTrackBase serializeTrack, TrackPlayerBase trackPlayer)
            {
                this.serializeTrack = serializeTrack;
                this.trackPlayer = trackPlayer;
            }

            public SerializeTrackBase serializeTrack;
            public TrackPlayerBase trackPlayer;
        }


        public void Play(TrackAsset asset)
        {
            this.asset = asset;

            Play();
        }

        public void Play()
        {
            isPlaying = true;
            startIndex = 0;
            endIndex = 0;
            time = 0.0f;

            createCommandList();

            playStart();
        }

        public void Stop()
        {
            isPlaying = false;
        }

        void Start()
        {
            if (playOnAwake) {
                Play();
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

                        element.OnStart(this);
                        ++startIndex;
                    }


                    while (endIndex < endCommandList.Count) {
                        var element = endCommandList[endIndex];

                        if (time * 60.0f < element.end) {
                            break;
                        }

                        element.OnEnd(this);
                        ++endIndex;
                    }


                    time += Time.deltaTime * GetPlaySpeed();

                } else {
                    isPlaying = false;

                    playEnd();
                }
            }
        }

        void playStart()
        {
            foreach (var trackPlayer in trackPlayers) {
                trackPlayer.OnTrackStart(this);
            }

            onPlayStart?.Invoke(this);
        }

        void playEnd()
        {
            foreach (var trackPlayer in trackPlayers) {
                trackPlayer.OnTrackEnd(this);
            }

            onPlayEnd?.Invoke(this);
        }

        void createCommandList()
        {
            trackInfoTable.Clear();

            trackPlayers.Clear();
            elementPlayers.Clear();

            addTrackAndElement();

            // トラック同士の親を設定。エレメントはaddElement時に設定済み
            foreach (var trackInfo in trackInfoTable) {
                var serializeTrack = trackInfo.Value.serializeTrack;
                var trackPlayer = trackInfo.Value.trackPlayer;

                TrackInfo parent;
                if (trackInfoTable.TryGetValue(serializeTrack.parent, out parent)) {
                    trackPlayer.Initialize(parent.trackPlayer, this);
                } else {
                    trackPlayer.Initialize(null, this);
                }
            }

            startCommandList = elementPlayers.OrderBy(elem => elem.start).ToList();
            endCommandList = elementPlayers.OrderBy(elem => elem.end).ToList();

        }

        void addTrack<SerializeTrackClass>(List<SerializeTrackClass> serializeList) where SerializeTrackClass : SerializeTrackBase
        {
            foreach (var serializeTrack in serializeList) {
                var trackPlayer = serializeTrack.CreatePlayer();

                trackPlayers.Add(trackPlayer);

                trackInfoTable.Add(serializeTrack.uniqueName, new TrackInfo(serializeTrack, trackPlayer));
            }
        }

        void addElement<SerializeElementClass>(List<SerializeElementClass> serializeList) where SerializeElementClass : SerializeElementBase
        {
            foreach (var serializeElement in serializeList) {
                var elementPlayer = serializeElement.CreatePlayer();

                elementPlayers.Add(elementPlayer);

                elementPlayer.Initialize(trackInfoTable[serializeElement.parent].trackPlayer, this);
            }
        }
    }
}

