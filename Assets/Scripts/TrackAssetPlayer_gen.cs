
// Auto Generate Code
using System.Collections.Generic;
using UnityEngine;
namespace track_editor {
    public partial class TrackAssetPlayer {
        void addTrackAndElement()        
        {

addTrack(asset.RootTracks);
addTrack(asset.GameObjectTracks);
addTrack(asset.ActivationTracks);
addTrack(asset.PositionTracks);
addTrack(asset.AnimationTracks);
addElement(asset.ActivationElements);
addElement(asset.PositionElements);
addElement(asset.AnimationElements);


        }
    }
}
