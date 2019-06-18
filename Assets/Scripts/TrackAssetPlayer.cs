using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace track_editor
{
    public class TrackAssetPlayer : MonoBehaviour
    {
        public TrackAsset asset;

        public bool playOnAwake = false;

        public bool IsPlaying { get => 0 <= playStartCurrent; }

        public bool IsPlayEnd { get => (playContext != null) && playStartCurrent == playContext.playStartElements.Count; }

        ElementPlayerContext playContext = null;

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

            playContext = new ElementPlayerContext(asset);
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
            if (playContext != null && IsPlaying) {

                while (playStartCurrent < playContext.playStartElements.Count) {
                    var element = playContext.playStartElements[playStartCurrent];

                    if (time * 60.0f < element.start) {
                        break;
                    }

                    element.OnStart(playContext);
                    ++playStartCurrent;
                }


                while (playEndCurrent < playContext.playEndElements.Count) {
                    var element = playContext.playEndElements[playEndCurrent];

                    if (time * 60.0f < element.end) {
                        break;
                    }

                    element.OnEnd(playContext);
                    ++playEndCurrent;
                }


                time += Time.deltaTime * speed;
            }
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

