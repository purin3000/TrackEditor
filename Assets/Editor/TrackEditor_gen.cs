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
            foreach (var editorTrack in getEditorTracks<RootEditorTrack.EditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<RootAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.RootTracks.Add(assetTrack);
            }


            asset.GameObjectTracks.Clear();
            foreach (var editorTrack in getEditorTracks<GameObjectEditorTrack.EditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<GameObjectAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.GameObjectTracks.Add(assetTrack);
            }



            asset.ActivationTracks.Clear();
            foreach (var editorTrack in getEditorTracks<ActivationEditorTrack.EditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<ActivationAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.ActivationTracks.Add(assetTrack);
            }


            asset.AnimationTracks.Clear();
            foreach (var editorTrack in getEditorTracks<AnimationEditorTrack.EditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<AnimationAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.AnimationTracks.Add(assetTrack);
            }


            asset.TransformTracks.Clear();
            foreach (var editorTrack in getEditorTracks<TransformEditorTrack.EditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<TransformAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.TransformTracks.Add(assetTrack);
            }

            foreach (var editorElement in getEditorElements<ActivationEditorTrack.EditorElementData>(editorElements)) {
                var assetElement = Serialize<ActivationAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.ActivationElements.Add(assetElement);
            }


            asset.TransformElements.Clear();
            foreach (var editorElement in getEditorElements<TransformEditorTrack.EditorElementData>(editorElements)) {
                var assetElement = Serialize<TransformAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.TransformElements.Add(assetElement);
            }


            asset.AnimationElements.Clear();
            foreach (var editorElement in getEditorElements<AnimationEditorTrack.EditorElementData>(editorElements)) {
                var assetElement = Serialize<AnimationAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.AnimationElements.Add(assetElement);
            }



            asset.trackCount = editorTracks.Count;
            asset.elementCount = editorElements.Count;

        }

        static void assetToEditorInternal(TrackAsset asset, EditorTrack[] editorTracks, EditorElement[] editorElements)
        {

            foreach (var assetTrack in asset.RootTracks) {
                var editorTrack = Deserialize<RootEditorTrack.EditorTrackData>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }


            foreach (var assetTrack in asset.GameObjectTracks) {
                var editorTrack = Deserialize<GameObjectEditorTrack.EditorTrackData>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }




            foreach (var assetTrack in asset.ActivationTracks) {
                var editorTrack = Deserialize<ActivationEditorTrack.EditorTrackData>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }


            foreach (var assetTrack in asset.AnimationTracks) {
                var editorTrack = Deserialize<AnimationEditorTrack.EditorTrackData>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }


            foreach (var assetTrack in asset.TransformTracks) {
                var editorTrack = Deserialize<TransformEditorTrack.EditorTrackData>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }




            foreach (var assetElement in asset.ActivationElements) {
                var editorElement = Deserialize<ActivationEditorTrack.EditorElementData>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements[assetElement.elementIndex] = editorElement;
            }


            foreach (var assetElement in asset.TransformElements) {
                var editorElement = Deserialize<TransformEditorTrack.EditorElementData>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements[assetElement.elementIndex] = editorElement;
            }


            foreach (var assetElement in asset.AnimationElements) {
                var editorElement = Deserialize<AnimationEditorTrack.EditorElementData>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements[assetElement.elementIndex] = editorElement;
            }


        }
    }
}
