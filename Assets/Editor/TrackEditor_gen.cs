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


            asset.TrackGroupTracks.Clear();
            foreach (var editorTrack in getEditorTracks<TrackGroupEditorTrack.EditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<TrackGroupAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.TrackGroupTracks.Add(assetTrack);
            }


            asset.GameObjectTracks.Clear();
            foreach (var editorTrack in getEditorTracks<GameObjectEditorTrack.EditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<GameObjectAssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.GameObjectTracks.Add(assetTrack);
            }


            asset.ActivationTracks.Clear();
            foreach (var editorTrack in getEditorTracks<ActivationEditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<ActivationAssetTrack>(editorTracks, editorTrack);
                asset.ActivationTracks.Add(assetTrack);
            }


            asset.ActivationElements.Clear();
            foreach (var editorElement in getEditorElements<ActivationEditorElementData>(editorElements)) {
                var assetElement = Serialize<ActivationAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.ActivationElements.Add(assetElement);
            }


            asset.TransformTracks.Clear();
            foreach (var editorTrack in getEditorTracks<TransformEditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<TransformAssetTrack>(editorTracks, editorTrack);
                asset.TransformTracks.Add(assetTrack);
            }


            asset.TransformElements.Clear();
            foreach (var editorElement in getEditorElements<TransformEditorElementData>(editorElements)) {
                var assetElement = Serialize<TransformAssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.TransformElements.Add(assetElement);
            }


            asset.AnimationTracks.Clear();
            foreach (var editorTrack in getEditorTracks<AnimationEditorTrackData>(editorTracks)) {
                var assetTrack = Serialize<AnimationAssetTrack>(editorTracks, editorTrack);
                asset.AnimationTracks.Add(assetTrack);
            }


            asset.AnimationElements.Clear();
            foreach (var editorElement in getEditorElements<AnimationEditorElementData>(editorElements)) {
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


            foreach (var assetTrack in asset.TrackGroupTracks) {
                var editorTrack = Deserialize<TrackGroupEditorTrack.EditorTrackData>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }


            foreach (var assetTrack in asset.GameObjectTracks) {
                var editorTrack = Deserialize<GameObjectEditorTrack.EditorTrackData>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }


            foreach (var assetTrack in asset.ActivationTracks) {
                var editorTrack = Deserialize<ActivationEditorTrackData>(assetTrack);
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }


            foreach (var assetElement in asset.ActivationElements) {
                var editorElement = Deserialize<ActivationEditorElementData>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements[assetElement.elementIndex] = editorElement;
            }


            foreach (var assetTrack in asset.TransformTracks) {
                var editorTrack = Deserialize<TransformEditorTrackData>(assetTrack);
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }


            foreach (var assetElement in asset.TransformElements) {
                var editorElement = Deserialize<TransformEditorElementData>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements[assetElement.elementIndex] = editorElement;
            }


            foreach (var assetTrack in asset.AnimationTracks) {
                var editorTrack = Deserialize<AnimationEditorTrackData>(assetTrack);
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }


            foreach (var assetElement in asset.AnimationElements) {
                var editorElement = Deserialize<AnimationEditorElementData>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements[assetElement.elementIndex] = editorElement;
            }


        }
    }

 
    public class TransformEditorTrackData : EditorTrackImplBase
    {
        public const string labelName = "Transform";

        public TransformEditorTrackData()
            : base(labelName) { }

        public override EditorElement CreateElement()
        {
            return CreateElementImpl<TransformEditorElementData>($"{labelName}:{elements.Count}");
        }
    }

    public class ActivationEditorTrackData : EditorTrackImplBase
    {
        public const string labelName = "Activate";

        public ActivationEditorTrackData() : base(labelName) { }

        public override EditorElement CreateElement()
        {
            return CreateElementImpl<ActivationEditorElementData>($"{labelName}:{elements.Count}");
        }
    }

    public class AnimationEditorTrackData : EditorTrackImplBase
    {
        public const string labelName = "Animation";

        public AnimationEditorTrackData() : base(labelName) { }

        public override EditorElement CreateElement()
        {
            return CreateElementImpl<AnimationEditorElementData>($"{labelName}:{elements.Count}");
        }
    }


}
