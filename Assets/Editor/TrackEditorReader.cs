using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor
{
    /// <summary>
    /// TrackAssetからの読み込み
    /// </summary>
    public class TrackEditorReader
    {
        List<TrackPair> allTracks = new List<TrackPair>();
        List<ElementPair> allElements = new List<ElementPair>();
        Dictionary<string, TrackData> trackTable = new Dictionary<string, TrackData>();

        public static void ReadAsset(TrackAsset asset, TrackEditor manager)
        {
            var data = new TrackEditorAsset(asset, manager);

            var context = new TrackEditorReader();

            data.ReadAsset(context);
        }

        /// <summary>
        /// SerializeTrackからTrackElementへ読み込み
        /// </summary>
        /// <typeparam name="SerializeTrackClass"></typeparam>
        /// <typeparam name="TrackDataClass"></typeparam>
        /// <param name="serializeTracks"></param>
        /// <param name="tracks"></param>
        public void readTracks<SerializeTrackClass, TrackDataClass>(List<SerializeTrackClass> serializeTracks, List<TrackDataClass> tracks)
            where SerializeTrackClass : SerializeTrackBase
            where TrackDataClass : TrackData, new()
        {
            foreach (var serializeTrack in serializeTracks) {
                TrackDataClass track = new TrackDataClass();

                tracks.Add(track);
                allTracks.Add(new TrackPair(track, serializeTrack));
                trackTable.Add(serializeTrack.uniqueName, track);

                track.ReadAsset(serializeTrack);
            }
        }

        public void readElements<SerializeElementClass, ElementClass>(List<SerializeElementClass> serializeElements, List<ElementClass> elements)
            where SerializeElementClass : SerializeElementBase
            where ElementClass : TrackElement, new()
        {
            foreach (var serializeElement in serializeElements) {
                ElementClass element = new ElementClass();

                elements.Add(element);
                allElements.Add(new ElementPair(element, serializeElement));

                element.ReadAsset(serializeElement);
            }
        }

        /// <summary>
        /// 階層構築
        /// </summary>
        /// <param name="manager"></param>
        public void updateHierarchy(TrackEditor manager)
        {
   
            // 親子階層を設定
            foreach (var trackPair in allTracks) {
                var serializeTrack = trackPair.serializeTrack;
                TrackData parent;

                if (trackTable.TryGetValue(serializeTrack.parent, out parent)) {
                    // parentが無効な場合はnullになれば良い
                }

                // 無効な親を持つエレメントを削除
                serializeTrack.childs = serializeTrack.childs.Where(dat => trackTable.ContainsKey(dat)).ToArray();

                List<TrackData> childs = new List<TrackData>();
                foreach (var child in serializeTrack.childs) {
                    childs.Add(trackTable[child]);
                }

                trackPair.track.LoadInitialize(manager, serializeTrack.name, parent, childs);
            }

            foreach (var trackPair in allTracks) {
                trackPair.track.UpdateNestLevel();
            }

            foreach (var elementPair in allElements) {
                var serializeElement = elementPair.serializeElement;
                var track = trackTable[serializeElement.parent];

                elementPair.element.LoadInitialize(serializeElement.name, serializeElement.start, serializeElement.length, track);
                track.elements.Add(elementPair.element);
            }
        }

        struct TrackPair
        {
            public TrackPair(TrackData track, SerializeTrackBase serializeTrack)
            {
                this.track = track;
                this.serializeTrack = serializeTrack;
            }

            public TrackData track;
            public SerializeTrackBase serializeTrack;
        }

        struct ElementPair
        {
            public ElementPair(TrackElement element, SerializeElementBase serializeElement)
            {
                this.element = element;
                this.serializeElement = serializeElement;
            }

            public TrackElement element;
            public SerializeElementBase serializeElement;
        }

    }
}

