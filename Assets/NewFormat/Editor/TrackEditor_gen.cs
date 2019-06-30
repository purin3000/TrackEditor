// Auto Generate Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public static partial class TrackSerializer
    {
        static void editorToAssetInternal(TrackAsset2 asset, List<EditorTrack> editorTracks, List<EditorElement> editorElements)
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


            asset.ActivationTracks.Clear();
            foreach (var editorTrack in getEditorTracks<ActivationEditorTrack>(editorTracks)) {
                var assetTrack = Serialize<ActivationAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.ActivationTracks.Add(assetTrack);
            }


            asset.ActivationElements.Clear();
            foreach (var editorElement in getEditorElements<ActivationEditorElement>(editorElements)) {
                var assetElement = Serialize<ActivationAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.ActivationElements.Add(assetElement);
            }


        }

        static void assetToEditorInternal(TrackAsset2 asset, List<EditorTrack> editorTracks, List<EditorElement> editorElements)
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


            foreach (var assetTrack in asset.ActivationTracks) {
                var editorTrack = Deserialize<ActivationEditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }


            foreach (var assetElement in asset.ActivationElements) {
                var editorElement = Deserialize<ActivationEditorElement>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements.Add(editorElement);
            }


        }
    }

}
