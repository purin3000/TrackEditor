using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using track_editor_fw;


namespace track_editor_example
{
    /// <summary>
    /// アセット書き込み時に使用する一時情報
    /// </summary>
    public class WriteAssetContext
    {
        public TrackEditorAsset asset;
        public List<EditorTrack> trackBaseList;
        public List<EditorTrackElement> elementBaseList;

        public List<RootTrackData> rootTracks = new List<RootTrackData>();
        public List<GameObjectTrackData> gameObjectTracks = new List<GameObjectTrackData>();
        public List<ActivationTrackData> activationTracks = new List<ActivationTrackData>();
        public List<PositionTrackData> positionTracks = new List<PositionTrackData>();

        public List<ActivationElement> activationElements = new List<ActivationElement>();
        public List<PositionElement> positionElements = new List<PositionElement>();

        List<T> GetTracks<T>() where T : EditorTrack
        {
            return trackBaseList.Where(obj => obj is T).Select(obj => (T)obj).ToList();

        }

        List<T> GetElements<T>() where T : EditorTrackElement
        {
            return elementBaseList.Where(obj => obj is T).Select(obj => (T)obj).ToList();

        }

        public WriteAssetContext(TrackEditorAsset asset, EditorTrack top)
        {
            this.asset = asset;
            trackBaseList = listupTrackBase(new List<EditorTrack>(), top);
            elementBaseList = listupElementBase(new List<EditorTrackElement>(), top);

            rootTracks = GetTracks<RootTrackData>();
            gameObjectTracks = GetTracks<GameObjectTrackData>();
            activationTracks = GetTracks<ActivationTrackData>();
            positionTracks = GetTracks<PositionTrackData>();

            activationElements = GetElements<ActivationElement>();
            positionElements = GetElements<PositionElement>();
        }

        static List<EditorTrack> listupTrackBase(List<EditorTrack> list, EditorTrack track)
        {
            list.Add(track);
            foreach (var child in track.childs) {
                listupTrackBase(list, child);
            }
            return list;
        }

        static List<EditorTrackElement> listupElementBase(List<EditorTrackElement> list, EditorTrack track)
        {
            list.AddRange(track.elements);
            foreach (var child in track.childs) {
                listupElementBase(list, child);
            }
            return list;
        }

        public string MakeTrackName(EditorTrack track)
        {
            return string.Format("Track{0}", trackBaseList.IndexOf(track));
        }

        public string MakeElementName(EditorTrackElement elementBase)
        {
            return string.Format("Element{0}", elementBaseList.IndexOf(elementBase));
        }
    }

    /// <summary>
    /// アセット読み込み時に使用する一時情報
    /// </summary>
    public class ReadAssetContext
    {
        public class TrackPair
        {
            public TrackPair(EditorTrack track, TrackSerialize serialize)
            {
                this.track = track;
                this.serialize = serialize;
            }

            public EditorTrack track;
            public TrackSerialize serialize;
        }

        /// <summary>
        /// TrackBaseとTrackSerializeの対応付
        /// 順序も重要で、親から設定する必要があるためListになってます。
        /// </summary>
        public List<TrackPair> trackPairs = new List<TrackPair>();

        /// <summary>
        /// TrackBase検索用
        /// </summary>
        public Dictionary<string, EditorTrack> trackTable = new Dictionary<string, EditorTrack>();

        List<EditorTrack> tracks = new List<EditorTrack>();
        List<EditorTrackElement> elements = new List<EditorTrackElement>();

        public T CreateTrack<T>(TrackSerialize trackSerialize) where T : EditorTrack, new()
        {
            T track = new T();

            trackPairs.Add(new TrackPair(track, trackSerialize));
            trackTable.Add(trackSerialize.uniqueName, track);
            tracks.Add(track);

            return track;
        }

        public T CreateElement<T>(ElementSerialize elementSerialize) where T : EditorTrackElement, new()
        {
            T element = new T();
            var track = trackTable[elementSerialize.parent];
            track.elements.Add(element);
            elements.Add(element);

            element.LoadInitialize(elementSerialize.start, elementSerialize.length, track);
            return element;
        }

        public void UpdateHierarchy(TrackEditor manager, string rootTrackName)
        {
            foreach (var trackPair in trackPairs) {
                List<EditorTrack> childs = new List<EditorTrack>();
                foreach (var child in trackPair.serialize.childs) {
                    //Debug.Log(child);
                    childs.Add(trackTable[child]);
                }
                trackPair.track.childs = childs;

                EditorTrack parent;
                if (!trackTable.TryGetValue(trackPair.serialize.parent, out parent)) {
                    parent = null;
                }
                trackPair.track.LoadInitialize(manager, trackPair.serialize.name, parent);
            }

            // ルートを設定
            manager.top = trackTable[rootTrackName];
        }
    }
}
