using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    /// <summary>
    /// トラックのシリアライズデータ
    /// </summary>
    [System.Serializable]
    public class RootSerializeTrack : SerializeTrack
    {
    }

    /// <summary>
    /// トラック情報
    /// RootTrackDataはTrackEditor.top専用
    /// </summary>
    public class RootTrackData : TrackData
    {
        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.rootTracks, context);
        }

        public void ReadAsset(RootSerializeTrack serializeTrack)
        {
        }
    }

}

