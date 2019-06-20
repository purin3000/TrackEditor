﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace track_editor
{
    public class TrackAssetPlayer : MonoBehaviour
    {
        public TrackAsset asset;

        public bool playOnAwake = false;

        public int currentFrame { get => (int)(time * 60.0f); }

        public bool IsPlaying { get => 0 <= playStartCurrent && time * 60 < asset.frameLength; }

        public bool IsPlayEnd { get => 0 <= playEndCurrent && asset.frameLength <= time * 60; }


        public List<IElementPlayer> playStartElements;
        public List<IElementPlayer> playEndElements;

        Dictionary<string, SerializeTrack> tracks = new Dictionary<string, SerializeTrack>();
        Dictionary<string, SerializeElement> elements = new Dictionary<string, SerializeElement>();
        List<IElementPlayer> elementPlayers = new List<IElementPlayer>();


        int playStartCurrent = -1;
        int playEndCurrent = -1;

        float time = 0.0f;
        float speed = 1.0f;


        private void Play(TrackAsset asset)
        {
            this.asset = asset;
        }

        private void Play()
        {
            playStartCurrent = 0;
            playEndCurrent = 0;
            time = 0.0f;

            tracks.Clear();
            elements.Clear();
            elementPlayers.Clear();

            addTrack(asset.rootTracks);
            addTrack(asset.gameObjectTracks);
            addTrack(asset.activationTracks);
            addTrack(asset.positionTracks);
            addTrack(asset.animationTracks);

            addElement(asset.activationElements);
            addElement(asset.positionElements);
            addElement(asset.animationElements);

            playStartElements = elementPlayers.OrderBy(elem => elem.start).ToList();
            playEndElements = elementPlayers.OrderBy(elem => elem.end).ToList();

        }

        void Start()
        {
            if (playOnAwake) {
                Play();
            }
        }

        void Update()
        {
            if (!IsPlaying) {
                return;
            }

            while (playStartCurrent < playStartElements.Count) {
                var element = playStartElements[playStartCurrent];

                if (time * 60.0f < element.start) {
                    break;
                }

                element.OnStart(this);
                ++playStartCurrent;
            }


            while (playEndCurrent < playEndElements.Count) {
                var element = playEndElements[playEndCurrent];

                if (time * 60.0f < element.end) {
                    break;
                }

                element.OnEnd(this);
                ++playEndCurrent;
            }


            time += Time.deltaTime * speed;
        }


        void addTrack<SerializeTrackClass>(List<SerializeTrackClass> serializeList) where SerializeTrackClass : SerializeTrack
        {
            foreach (var serializeTrack in serializeList) {
                tracks.Add(serializeTrack.uniqueName, serializeTrack);
            }
        }

        void addElement<SerializeElementClass>(List<SerializeElementClass> serializeList) where SerializeElementClass : SerializeElement
        {
            foreach (var serializeElement in serializeList) {
                elements.Add(serializeElement.uniqueName, serializeElement);
                elementPlayers.Add(serializeElement.CreatePlayer());
            }
        }

        public T GetTrack<T>(SerializeElement elementSerialize) where T : SerializeTrack
        {
            return tracks[elementSerialize.parent] as T;
        }

        public T GetParentTrack<T>(SerializeElement elementSerialize) where T : SerializeTrack
        {
            return tracks[tracks[elementSerialize.parent].parent] as T;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(TrackAssetPlayer))]
        class TrackAssetPlayerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (EditorApplication.isPlaying) {
                    if (GUILayout.Button("再生")) {
                        var ta = target as TrackAssetPlayer;
                        ta.Play();
                    }
                }
            }
        }
#endif
    }
}

