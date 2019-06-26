using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor
{
    /// <summary>
    /// TrackAssetへの書き込み
    /// </summary>
    public class TrackEditorWriter
    {
        public List<TrackData> trackBaseList;
        public List<TrackElement> elementBaseList;

        public static void WriteAsset(TrackAsset asset, TrackEditor manager)
        {
            var data = new TrackEditorAsset(asset, manager);

            var context = new TrackEditorWriter();

            data.WriteAsset(context);
        }

        /// <summary>
        /// TrackDataからSerializeTrackへ書き込み
        /// </summary>
        /// <typeparam name="TrackDataClass"></typeparam>
        /// <typeparam name="SerializeTrackClass"></typeparam>
        /// <param name="tracks"></param>
        /// <param name="serializeTracks"></param>
        public void writeTracks<TrackDataClass, SerializeTrackClass>(List<TrackDataClass> tracks, List<SerializeTrackClass> serializeTracks)
            where TrackDataClass : TrackData
            where SerializeTrackClass : SerializeTrackBase, new()
        {
            getEditorTracks(tracks);

            serializeTracks.Clear();

            foreach (var track in tracks) {
                // 対応するシリアライズ用のクラスを作って
                var serializeTrack = new SerializeTrackClass();

                SerializeUtility.InitializeTrackSerialize(serializeTrack, track, this);

                // リストへ追加
                serializeTracks.Add(serializeTrack);

                track.WriteAsset(serializeTrack);
            }
        }

        public void writeElements<TrackElementClass, SerializeElementClass>(List<TrackElementClass> elements, List<SerializeElementClass> serializeElements)
            where TrackElementClass : TrackElement
            where SerializeElementClass : SerializeElementBase, new()
        {
            getEditorElements(elements);

            serializeElements.Clear();

            foreach (var element in elements) {
                var serializeElement = new SerializeElementClass();

                SerializeUtility.InitializeElementSerialize(serializeElement, element, this);

                serializeElements.Add(serializeElement);

                element.WriteAsset(serializeElement);
            }
        }

        void getEditorTracks<TrackDataClass>(List<TrackDataClass> list) where TrackDataClass : TrackData
        {
            list.Clear();
            list.AddRange(trackBaseList.Where(obj => obj is TrackDataClass).Select(obj => (TrackDataClass)obj));

        }

        void getEditorElements<TrackElementClass>(List<TrackElementClass> list) where TrackElementClass : TrackElement
        {
            list.Clear();
            list.AddRange(elementBaseList.Where(obj => obj is TrackElementClass).Select(obj => (TrackElementClass)obj));
        }

        public List<TrackData> trackListup(List<TrackData> list, TrackData track)
        {
            list.Add(track);
            foreach (var child in track.childs) {
                trackListup(list, child);
            }
            return list;
        }

        public List<TrackElement> elementListup(List<TrackElement> list, TrackData track)
        {
            list.AddRange(track.elements);
            foreach (var child in track.childs) {
                elementListup(list, child);
            }
            return list;
        }

        public string MakeTrackName(TrackData track)
        {
            return string.Format("Track{0}", trackBaseList.IndexOf(track));
        }

        public string MakeElementName(TrackElement elementBase)
        {
            return string.Format("Element{0}", elementBaseList.IndexOf(elementBase));
        }
    }
}
