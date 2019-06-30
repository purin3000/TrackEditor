using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
     [System.Serializable]
    public class AssetElement
    {
        public int elementIndex;
        public string name;
        public int parentTrackIndex;
        public int start;
        public int length;

        public int end => start + end;

        public void Initialize(int elementIndex, string name, int parentTrackIndex, int start, int length)
        {
            this.elementIndex = elementIndex;
            this.name = name;
            this.parentTrackIndex = parentTrackIndex;
            this.start = start;
            this.length = length;
        }
    }
}
