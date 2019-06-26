
// Auto Generate Code
#define ENABLE_GEN_CODE

using System.Collections.Generic;
using UnityEditor;
namespace track_editor {
    public partial class TrackEditorAsset {

#if ENABLE_GEN_CODE
List<RootTrackEditor.Track> RootTracks = new List<RootTrackEditor.Track>();
List<GameObjectTrackEditor.Track> GameObjectTracks = new List<GameObjectTrackEditor.Track>();
List<CameraTrackEditor.Track> CameraTracks = new List<CameraTrackEditor.Track>();
List<ActivationTrackEditor.Track> ActivationTracks = new List<ActivationTrackEditor.Track>();
List<TransformTrackEditor.Track> TransformTracks = new List<TransformTrackEditor.Track>();
List<AnimationTrackEditor.Track> AnimationTracks = new List<AnimationTrackEditor.Track>();
List<CameraChangeTrackEditor.Track> CameraChangeTracks = new List<CameraChangeTrackEditor.Track>();
List<ChangeBgMaterialTrackEditor.Track> ChangeBgMaterialTracks = new List<ChangeBgMaterialTrackEditor.Track>();
List<ActivationTrackEditor.Element> ActivationElements = new List<ActivationTrackEditor.Element>();
List<TransformTrackEditor.Element> TransformElements = new List<TransformTrackEditor.Element>();
List<AnimationTrackEditor.Element> AnimationElements = new List<AnimationTrackEditor.Element>();
List<CameraChangeTrackEditor.Element> CameraChangeElements = new List<CameraChangeTrackEditor.Element>();
List<ChangeBgMaterialTrackEditor.Element> ChangeBgMaterialElements = new List<ChangeBgMaterialTrackEditor.Element>();

#endif

        void readAssetInternal(TrackEditorReader reader) {

#if ENABLE_GEN_CODE
reader.readTracks(asset.RootTracks, RootTracks);
reader.readTracks(asset.GameObjectTracks, GameObjectTracks);
reader.readTracks(asset.CameraTracks, CameraTracks);
reader.readTracks(asset.ActivationTracks, ActivationTracks);
reader.readTracks(asset.TransformTracks, TransformTracks);
reader.readTracks(asset.AnimationTracks, AnimationTracks);
reader.readTracks(asset.CameraChangeTracks, CameraChangeTracks);
reader.readTracks(asset.ChangeBgMaterialTracks, ChangeBgMaterialTracks);
reader.readElements(asset.ActivationElements, ActivationElements);
reader.readElements(asset.TransformElements, TransformElements);
reader.readElements(asset.AnimationElements, AnimationElements);
reader.readElements(asset.CameraChangeElements, CameraChangeElements);
reader.readElements(asset.ChangeBgMaterialElements, ChangeBgMaterialElements);

#endif

            reader.updateHierarchy(manager);
        }

        void writeAssetInternal(TrackEditorWriter writer) {

#if ENABLE_GEN_CODE
writer.writeTracks(RootTracks, asset.RootTracks);
writer.writeTracks(GameObjectTracks, asset.GameObjectTracks);
writer.writeTracks(CameraTracks, asset.CameraTracks);
writer.writeTracks(ActivationTracks, asset.ActivationTracks);
writer.writeTracks(TransformTracks, asset.TransformTracks);
writer.writeTracks(AnimationTracks, asset.AnimationTracks);
writer.writeTracks(CameraChangeTracks, asset.CameraChangeTracks);
writer.writeTracks(ChangeBgMaterialTracks, asset.ChangeBgMaterialTracks);
writer.writeElements(ActivationElements, asset.ActivationElements);
writer.writeElements(TransformElements, asset.TransformElements);
writer.writeElements(AnimationElements, asset.AnimationElements);
writer.writeElements(CameraChangeElements, asset.CameraChangeElements);
writer.writeElements(ChangeBgMaterialElements, asset.ChangeBgMaterialElements);

#endif

        }
    }
}
