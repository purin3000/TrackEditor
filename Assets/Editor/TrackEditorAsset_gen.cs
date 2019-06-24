
// Auto Generate Code
using System.Collections.Generic;
using UnityEditor;
namespace track_editor {
    public partial class TrackEditorAsset {

List<RootTrackData> RootTracks = new List<RootTrackData>();
List<GameObjectTrackData> GameObjectTracks = new List<GameObjectTrackData>();
List<ActivationTrackData> ActivationTracks = new List<ActivationTrackData>();
List<PositionTrackData> PositionTracks = new List<PositionTrackData>();
List<AnimationTrackData> AnimationTracks = new List<AnimationTrackData>();
List<ActivationElement> ActivationElements = new List<ActivationElement>();
List<PositionElement> PositionElements = new List<PositionElement>();
List<AnimationElement> AnimationElements = new List<AnimationElement>();


        void readAssetInternal(TrackEditorReader reader) {

reader.readTracks(asset.RootTracks, RootTracks);
reader.readTracks(asset.GameObjectTracks, GameObjectTracks);
reader.readTracks(asset.ActivationTracks, ActivationTracks);
reader.readTracks(asset.PositionTracks, PositionTracks);
reader.readTracks(asset.AnimationTracks, AnimationTracks);
reader.readElements(asset.ActivationElements, ActivationElements);
reader.readElements(asset.PositionElements, PositionElements);
reader.readElements(asset.AnimationElements, AnimationElements);


            reader.updateHierarchy(manager);
        }

        void writeAssetInternal(TrackEditorWriter writer) {

writer.writeTracks(RootTracks, asset.RootTracks);
writer.writeTracks(GameObjectTracks, asset.GameObjectTracks);
writer.writeTracks(ActivationTracks, asset.ActivationTracks);
writer.writeTracks(PositionTracks, asset.PositionTracks);
writer.writeTracks(AnimationTracks, asset.AnimationTracks);
writer.writeElements(ActivationElements, asset.ActivationElements);
writer.writeElements(PositionElements, asset.PositionElements);
writer.writeElements(AnimationElements, asset.AnimationElements);


        }
    }
}
