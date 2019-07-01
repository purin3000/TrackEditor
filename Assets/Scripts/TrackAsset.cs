using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public partial class TrackAsset : MonoBehaviour
    {
        public int frameLength = 100;
        public float playSpeed = 1.0f;

        //[HideInInspector]
        public int trackCount;

        //[HideInInspector]
        public int elementCount;
    }
}

