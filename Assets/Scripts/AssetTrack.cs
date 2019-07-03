using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    [System.Serializable]
    public class AssetTrack
    {
        public int trackIndex;
        public string name;
        public int parentTrackIndex;
        public bool expand;

        public void Initialize(int trackIndex, string name, int parentTrackIndex, bool expand)
        {
            this.trackIndex = trackIndex;
            this.name = name;
            this.parentTrackIndex = parentTrackIndex;
            this.expand = expand;
        }
    }

    public static class ContainerSwap
    {
        public static void SwapAt<T>(this List<T> list, int indexA, int indexB)
        {
            var tmp = list[indexB];
            list[indexB] = list[indexA];
            list[indexA] = tmp;
        }

        public static int IndexOf<T>(this IEnumerable<T> list, T obj)
        {
            var ite = list.GetEnumerator();
            int i = 0;

            while (ite.MoveNext()) {
                if (ite.Current.Equals(obj)) {
                    return i;
                }
                ++i;
            }

            return -1;
        }
    }
}
