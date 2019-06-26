
// Auto Generate Code
#define ENABLE_GEN_CODE

using System.Collections.Generic;
using UnityEditor;
namespace track_editor {
    public partial class TrackEditorAsset {

#if ENABLE_GEN_CODE
List<RootTrackEditor.Track> RootTracks = new List<RootTrackEditor.Track>();
List<GameObjectTrackEditor.Track> GameObjectTracks = new List<GameObjectTrackEditor.Track>();
List<ActivationTrackEditor.Track> ActivationTracks = new List<ActivationTrackEditor.Track>();
List<TransformTrackEditor.Track> TransformTracks = new List<TransformTrackEditor.Track>();
List<AnimationTrackEditor.Track> AnimationTracks = new List<AnimationTrackEditor.Track>();
List<ActivationTrackEditor.Element> ActivationElements = new List<ActivationTrackEditor.Element>();
List<TransformTrackEditor.Element> TransformElements = new List<TransformTrackEditor.Element>();
List<AnimationTrackEditor.Element> AnimationElements = new List<AnimationTrackEditor.Element>();

#endif

        void readAssetInternal(TrackEditorReader reader) {

#if ENABLE_GEN_CODE
reader.readTracks(asset.RootTracks, RootTracks);
reader.readTracks(asset.GameObjectTracks, GameObjectTracks);
reader.readTracks(asset.ActivationTracks, ActivationTracks);
reader.readTracks(asset.TransformTracks, TransformTracks);
reader.readTracks(asset.AnimationTracks, AnimationTracks);
reader.readElements(asset.ActivationElements, ActivationElements);
reader.readElements(asset.TransformElements, TransformElements);
reader.readElements(asset.AnimationElements, AnimationElements);

#endif

            reader.updateHierarchy(manager);
        }

        void writeAssetInternal(TrackEditorWriter writer) {

#if ENABLE_GEN_CODE
writer.writeTracks(RootTracks, asset.RootTracks);
writer.writeTracks(GameObjectTracks, asset.GameObjectTracks);
writer.writeTracks(ActivationTracks, asset.ActivationTracks);
writer.writeTracks(TransformTracks, asset.TransformTracks);
writer.writeTracks(AnimationTracks, asset.AnimationTracks);
writer.writeElements(ActivationElements, asset.ActivationElements);
writer.writeElements(TransformElements, asset.TransformElements);
writer.writeElements(AnimationElements, asset.AnimationElements);

#endif

        }
    }
}
