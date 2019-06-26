
// Auto Generate Code
#define ENABLE_GEN_CODE

using System.Collections.Generic;
using UnityEngine;
namespace track_editor {
    public partial class TrackAssetPlayer {
        void addTrackAndElement()        
        {

#if ENABLE_GEN_CODE
addTrack(asset.RootTracks);
addTrack(asset.GameObjectTracks);
addTrack(asset.ActivationTracks);
addTrack(asset.TransformTracks);
addTrack(asset.AnimationTracks);
addElement(asset.ActivationElements);
addElement(asset.TransformElements);
addElement(asset.AnimationElements);

#endif

        }
    }
}
