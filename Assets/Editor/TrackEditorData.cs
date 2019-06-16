using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace track_editor_fw
{
    public class TrackEditorData : ScriptableObject
    {
        public int frameLength = 100;

        public List<Track> tracks = new List<Track>();
        public List<Element> elements = new List<Element>();

        [System.Serializable]
        public class Track
        {
            public string name;
            public int parent;
            public List<int> elements = new List<int>();
        }

        [System.Serializable]
        public class Element
        {
            public int start;
            public int length;
            public int parent;
        }


#if UNITY_EDITOR
        public static TrackEditorData Save(TrackEditor data, string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<TrackEditorData>(assetPath);
            if (asset == null) {
                asset = CreateInstance<TrackEditorData>();
                AssetDatabase.CreateAsset(asset, assetPath);
            }

            asset.frameLength = data.frameLength;

            List<TrackBase> trackBaseList = listupTrackBase(new List<TrackBase>(), data.top);
            List<ElementBase> elementBaseList = listupElementBase(new List<ElementBase>(), data.top);

            asset.tracks.Clear();
            foreach (var trackBase in trackBaseList) {
                var track = new Track();
                track.name = trackBase.name;
                track.parent = trackBaseList.IndexOf(trackBase.parent);
                foreach (var element in elementBaseList) {
                    track.elements.Add(elementBaseList.IndexOf(element));
                }
                asset.tracks.Add(track);
            }

            asset.elements.Clear();
            foreach (var elementBase in elementBaseList) {
                var element = new Element();
                element.start = elementBase.start;
                element.length = elementBase.length;
                element.parent = trackBaseList.IndexOf(elementBase.parent);
                asset.elements.Add(element);
            }

            EditorUtility.SetDirty(asset);

            return asset;
        }

        static List<TrackBase> listupTrackBase(List<TrackBase> list, TrackBase track)
        {
            list.Add(track);
            foreach (var child in track.childs) {
                listupTrackBase(list, child);
            }
            return list;
        }

        static List<ElementBase> listupElementBase(List<ElementBase> list, TrackBase track)
        {
            list.AddRange(track.elements);
            foreach (var child in track.childs) {
                listupElementBase(list, child);
            }
            return list;
        }
#endif
    }
}
