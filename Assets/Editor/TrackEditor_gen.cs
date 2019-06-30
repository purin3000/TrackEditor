// Auto Generate Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public static partial class TrackSerializer
    {
        static void editorToAssetInternal(TrackAsset asset, List<EditorTrack> editorTracks, List<EditorElement> editorElements)
        {

            asset.RootTracks.Clear();
            foreach (var editorTrack in getEditorTracks<RootEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<RootAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.RootTracks.Add(assetTrack);
            }


            asset.GameObjectTracks.Clear();
            foreach (var editorTrack in getEditorTracks<GameObjectEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<GameObjectAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.GameObjectTracks.Add(assetTrack);
            }


            asset.CameraTracks.Clear();
            foreach (var editorTrack in getEditorTracks<CameraEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<CameraAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.CameraTracks.Add(assetTrack);
            }


            asset.ActivationTracks.Clear();
            foreach (var editorTrack in getEditorTracks<ActivationEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<ActivationAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.ActivationTracks.Add(assetTrack);
            }


            asset.AnimationTracks.Clear();
            foreach (var editorTrack in getEditorTracks<AnimationEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<AnimationAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.AnimationTracks.Add(assetTrack);
            }


            asset.TransformTracks.Clear();
            foreach (var editorTrack in getEditorTracks<TransformEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<TransformAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.TransformTracks.Add(assetTrack);
            }


            asset.CameraChangeTracks.Clear();
            foreach (var editorTrack in getEditorTracks<CameraChangeEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<CameraChangeAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.CameraChangeTracks.Add(assetTrack);
            }


            asset.ChangeBgMaterialTracks.Clear();
            foreach (var editorTrack in getEditorTracks<ChangeBgMaterialEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<ChangeBgMaterialAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.ChangeBgMaterialTracks.Add(assetTrack);
            }


            asset.ActivationElements.Clear();
            foreach (var editorElement in getEditorElements<ActivationEditorElement>(editorElements)) {
                var assetElement = Serialize<ActivationAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.ActivationElements.Add(assetElement);
            }


            asset.TransformElements.Clear();
            foreach (var editorElement in getEditorElements<TransformEditorElement>(editorElements)) {
                var assetElement = Serialize<TransformAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.TransformElements.Add(assetElement);
            }


            asset.AnimationElements.Clear();
            foreach (var editorElement in getEditorElements<AnimationEditorElement>(editorElements)) {
                var assetElement = Serialize<AnimationAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.AnimationElements.Add(assetElement);
            }


            asset.CameraChangeElements.Clear();
            foreach (var editorElement in getEditorElements<CameraChangeEditorElement>(editorElements)) {
                var assetElement = Serialize<CameraChangeAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.CameraChangeElements.Add(assetElement);
            }


            asset.ChangeBgMaterialElements.Clear();
            foreach (var editorElement in getEditorElements<ChangeBgMaterialEditorElement>(editorElements)) {
                var assetElement = Serialize<ChangeBgMaterialAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.ChangeBgMaterialElements.Add(assetElement);
            }


        }

        static void assetToEditorInternal(TrackAsset asset, List<EditorTrack> editorTracks, List<EditorElement> editorElements)
        {

            foreach (var assetTrack in asset.RootTracks) {
                var editorTrack = Deserialize<RootEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetTrack in asset.GameObjectTracks) {
                var editorTrack = Deserialize<GameObjectEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetTrack in asset.CameraTracks) {
                var editorTrack = Deserialize<CameraEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetTrack in asset.ActivationTracks) {
                var editorTrack = Deserialize<ActivationEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetTrack in asset.AnimationTracks) {
                var editorTrack = Deserialize<AnimationEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetTrack in asset.TransformTracks) {
                var editorTrack = Deserialize<TransformEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetTrack in asset.CameraChangeTracks) {
                var editorTrack = Deserialize<CameraChangeEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetTrack in asset.ChangeBgMaterialTracks) {
                var editorTrack = Deserialize<ChangeBgMaterialEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetElement in asset.ActivationElements) {
                var editorElement = Deserialize<ActivationEditorElement>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements.Add(editorElement);
            }


            foreach (var assetElement in asset.TransformElements) {
                var editorElement = Deserialize<TransformEditorElement>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements.Add(editorElement);
            }


            foreach (var assetElement in asset.AnimationElements) {
                var editorElement = Deserialize<AnimationEditorElement>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements.Add(editorElement);
            }


            foreach (var assetElement in asset.CameraChangeElements) {
                var editorElement = Deserialize<CameraChangeEditorElement>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements.Add(editorElement);
            }


            foreach (var assetElement in asset.ChangeBgMaterialElements) {
                var editorElement = Deserialize<ChangeBgMaterialEditorElement>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements.Add(editorElement);
            }


        }
    }

}
