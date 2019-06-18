using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    public class TrackAssetPlayer : MonoBehaviour
    {
        public TrackAsset asset;

        public bool playOnAwake = false;

        public bool IsPlaying { get => 0 <= playCurrent; }

        public bool IsPlayEnd { get => (playContext != null) && playCurrent == playContext.playElements.Count; }

        ElementPlayerContext playContext = null;

        int playCurrent = -1;

        float time = 0.0f;
        float speed = 1.0f;


        private void Play(TrackAsset asset)
        {
            this.asset = asset;
        }

        private void Play()
        {
            playCurrent = 0;
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

                while (playCurrent < playContext.playElements.Count) {
                    var element = playContext.playElements[playCurrent];

                    if (time * 60.0f < element.start) {
                        break;
                    }

                    element.Apply(playContext);
                    ++playCurrent;
                }

                time += Time.deltaTime * speed;
            }
        }
    }
}

