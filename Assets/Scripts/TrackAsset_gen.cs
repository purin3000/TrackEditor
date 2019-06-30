// Auto Generate Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public partial class TrackAsset : MonoBehaviour
    {

        [SerializeField]
        [HideInInspector]
        public List<RootAssetTrack> RootTracks = new List<RootAssetTrack>();


        [SerializeField]
        [HideInInspector]
        public List<GameObjectAssetTrack> GameObjectTracks = new List<GameObjectAssetTrack>();


        [SerializeField]
        [HideInInspector]
        public List<CameraAssetTrack> CameraTracks = new List<CameraAssetTrack>();


        [SerializeField]
        [HideInInspector]
        public List<ActivationAssetTrack> ActivationTracks = new List<ActivationAssetTrack>();


        [SerializeField]
        [HideInInspector]
        public List<AnimationAssetTrack> AnimationTracks = new List<AnimationAssetTrack>();


        [SerializeField]
        [HideInInspector]
        public List<TransformAssetTrack> TransformTracks = new List<TransformAssetTrack>();


        [SerializeField]
        [HideInInspector]
        public List<CameraChangeAssetTrack> CameraChangeTracks = new List<CameraChangeAssetTrack>();


        [SerializeField]
        [HideInInspector]
        public List<ChangeBgMaterialAssetTrack> ChangeBgMaterialTracks = new List<ChangeBgMaterialAssetTrack>();


        [SerializeField]
        [HideInInspector]
        public List<ActivationAssetElement> ActivationElements = new List<ActivationAssetElement>();


        [SerializeField]
        [HideInInspector]
        public List<TransformAssetElement> TransformElements = new List<TransformAssetElement>();


        [SerializeField]
        [HideInInspector]
        public List<AnimationAssetElement> AnimationElements = new List<AnimationAssetElement>();


        [SerializeField]
        [HideInInspector]
        public List<CameraChangeAssetElement> CameraChangeElements = new List<CameraChangeAssetElement>();


        [SerializeField]
        [HideInInspector]
        public List<ChangeBgMaterialAssetElement> ChangeBgMaterialElements = new List<ChangeBgMaterialAssetElement>();


    }

    public partial class TrackAssetPlayer : MonoBehaviour
    {
        void initalizeTracksAndElements()
        {
            int trackCount = 0;
            int elementCount = 0;
            trackCount += asset.RootTracks.Count;
            trackCount += asset.GameObjectTracks.Count;
            trackCount += asset.CameraTracks.Count;
            trackCount += asset.ActivationTracks.Count;
            trackCount += asset.AnimationTracks.Count;
            trackCount += asset.TransformTracks.Count;
            trackCount += asset.CameraChangeTracks.Count;
            trackCount += asset.ChangeBgMaterialTracks.Count;
            elementCount += asset.ActivationElements.Count;
            elementCount += asset.TransformElements.Count;
            elementCount += asset.AnimationElements.Count;
            elementCount += asset.CameraChangeElements.Count;
            elementCount += asset.ChangeBgMaterialElements.Count;

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



            foreach (var assetTrack in asset.CameraTracks) {
                var playerTrack = createPlayerTrack<CameraTrack.PlayerTrack>(assetTrack);
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



            foreach (var assetTrack in asset.CameraChangeTracks) {
                var playerTrack = createPlayerTrack<CameraChangeTrack.PlayerTrack>(assetTrack);
                playerTrack.trackData = assetTrack.trackData;
                playerTracks[assetTrack.trackIndex] = playerTrack;
            }



            foreach (var assetTrack in asset.ChangeBgMaterialTracks) {
                var playerTrack = createPlayerTrack<ChangeBgMaterialTrack.PlayerTrack>(assetTrack);
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



            foreach (var assetElement in asset.CameraChangeElements) {
                var playerElement = createPlayerElement<CameraChangeTrack.PlayerElement>(assetElement);
                playerElement.elementData = assetElement.elementData;
                playerElements[assetElement.elementIndex] = playerElement;
            }



            foreach (var assetElement in asset.ChangeBgMaterialElements) {
                var playerElement = createPlayerElement<ChangeBgMaterialTrack.PlayerElement>(assetElement);
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
    public class CameraAssetTrack : AssetTrack {
        public CameraTrack.TrackData trackData = new CameraTrack.TrackData();
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
    public class CameraChangeAssetTrack : AssetTrack {
        public CameraChangeTrack.TrackData trackData = new CameraChangeTrack.TrackData();
    }


    [System.Serializable]
    public class ChangeBgMaterialAssetTrack : AssetTrack {
        public ChangeBgMaterialTrack.TrackData trackData = new ChangeBgMaterialTrack.TrackData();
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


    [System.Serializable]
    public class CameraChangeAssetElement : AssetElement {
        public CameraChangeTrack.ElementData elementData = new CameraChangeTrack.ElementData();
    }


    [System.Serializable]
    public class ChangeBgMaterialAssetElement : AssetElement {
        public ChangeBgMaterialTrack.ElementData elementData = new ChangeBgMaterialTrack.ElementData();
    }


}
