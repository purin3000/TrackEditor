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

#if UNITY_EDITOR
    /// <summary>
    /// トラック情報
    /// </summary>
    public class RootTrackData : TrackData
    {
    }
#endif
}

