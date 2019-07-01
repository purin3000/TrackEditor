// Auto Generate Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public partial class TrackAsset : MonoBehaviour
    {

        [SerializeField]
        //[HideInInspector]
        public List<RootAssetTrack> RootTracks = new List<RootAssetTrack>();


        [SerializeField]
        //[HideInInspector]
        public List<GameObjectAssetTrack> GameObjectTracks = new List<GameObjectAssetTrack>();


        [SerializeField]
        //[HideInInspector]
        public List<ActivationAssetTrack> ActivationTracks = new List<ActivationAssetTrack>();


        [SerializeField]
        //[HideInInspector]
        public List<AnimationAssetTrack> AnimationTracks = new List<AnimationAssetTrack>();


        [SerializeField]
        //[HideInInspector]
        public List<TransformAssetTrack> TransformTracks = new List<TransformAssetTrack>();


        [SerializeField]
        //[HideInInspector]
        public List<ActivationAssetElement> ActivationElements = new List<ActivationAssetElement>();


        [SerializeField]
        //[HideInInspector]
        public List<TransformAssetElement> TransformElements = new List<TransformAssetElement>();


        [SerializeField]
        //[HideInInspector]
        public List<AnimationAssetElement> AnimationElements = new List<AnimationAssetElement>();


    }

    public partial class TrackAssetPlayer : MonoBehaviour
    {
        void initalizeTracksAndElements()
        {
            int trackCount = 0;
            int elementCount = 0;
            trackCount += asset.RootTracks.Count;
            trackCount += asset.GameObjectTracks.Count;
            trackCount += asset.ActivationTracks.Count;
            trackCount += asset.AnimationTracks.Count;
            trackCount += asset.TransformTracks.Count;
            elementCount += asset.ActivationElements.Count;
            elementCount += asset.TransformElements.Count;
            elementCount += asset.AnimationElements.Count;

            playerTracks = new PlayerTrackBase[trackCount];
            playerElements = new PlayerElementBase[elementCount];

            foreach (var assetTrack in asset.RootTracks) {
                var playerTrack = createPlayerTrack<RootTrack.PlayerTrack>(assetTrack);
                playerTrack.trackData = assetTrack.trackData;
                playerTracks[assetTrack.trackIndex] = playerTrack;
            }



            foreach (var assetTrack in asset.GameObjectTracks) {
                var playerTrack = createPlayerTrack<GameObjectTrack.PlayerTrack>(assetTrack);
                playerTrack.trackData = assetTrack.trackData;
                playerTracks[assetTrack.trackIndex] = playerTrack;
            }



            foreach (var assetTrack in asset.ActivationTracks) {
                var playerTrack = createPlayerTrack<ActivationTrack.PlayerTrack>(assetTrack);
                playerTrack.trackData = assetTrack.trackData;
                playerTracks[assetTrack.trackIndex] = playerTrack;
            }



            foreach (var assetTrack in asset.AnimationTracks) {
                var playerTrack = createPlayerTrack<AnimationTrack.PlayerTrack>(assetTrack);
                playerTrack.trackData = assetTrack.trackData;
                playerTracks[assetTrack.trackIndex] = playerTrack;
            }



            foreach (var assetTrack in asset.TransformTracks) {
                var playerTrack = createPlayerTrack<TransformTrack.PlayerTrack>(assetTrack);
                playerTrack.trackData = assetTrack.trackData;
                playerTracks[assetTrack.trackIndex] = playerTrack;
            }



            foreach (var assetElement in asset.ActivationElements) {
                var playerElement = createPlayerElement<ActivationTrack.PlayerElement>(assetElement);
                playerElement.elementData = assetElement.elementData;
                playerElements[assetElement.elementIndex] = playerElement;
            }



            foreach (var assetElement in asset.TransformElements) {
                var playerElement = createPlayerElement<TransformTrack.PlayerElement>(assetElement);
                playerElement.elementData = assetElement.elementData;
                playerElements[assetElement.elementIndex] = playerElement;
            }



            foreach (var assetElement in asset.AnimationElements) {
                var playerElement = createPlayerElement<AnimationTrack.PlayerElement>(assetElement);
                playerElement.elementData = assetElement.elementData;
                playerElements[assetElement.elementIndex] = playerElement;
            }



            foreach (var trackPlayer in playerTracks) {
                trackPlayer.parent = getTrackPlayer(trackPlayer.parentTrackIndex);
            }

            foreach (var elementPlayer in playerElements) {
                elementPlayer.parent = getTrackPlayer(elementPlayer.parentTrackIndex);
            }
        }
    }

    [System.Serializable]
    public class RootAssetTrack : AssetTrack {
        public RootTrack.TrackData trackData = new RootTrack.TrackData();
    }


    [System.Serializable]
    public class GameObjectAssetTrack : AssetTrack {
        public GameObjectTrack.TrackData trackData = new GameObjectTrack.TrackData();
    }


    [System.Serializable]
    public class ActivationAssetTrack : AssetTrack {
        public ActivationTrack.TrackData trackData = new ActivationTrack.TrackData();
    }


    [System.Serializable]
    public class AnimationAssetTrack : AssetTrack {
        public AnimationTrack.TrackData trackData = new AnimationTrack.TrackData();
    }


    [System.Serializable]
    public class TransformAssetTrack : AssetTrack {
        public TransformTrack.TrackData trackData = new TransformTrack.TrackData();
    }


    [System.Serializable]
    public class ActivationAssetElement : AssetElement {
        public ActivationTrack.ElementData elementData = new ActivationTrack.ElementData();
    }


    [System.Serializable]
    public class TransformAssetElement : AssetElement {
        public TransformTrack.ElementData elementData = new TransformTrack.ElementData();
    }


    [System.Serializable]
    public class AnimationAssetElement : AssetElement {
        public AnimationTrack.ElementData elementData = new AnimationTrack.ElementData();
    }


}
