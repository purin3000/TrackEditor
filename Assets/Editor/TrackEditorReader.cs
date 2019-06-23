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
        public class TrackPair
        {
            public TrackPair(TrackData track, SerializeTrack serialize)
            {
                this.track = track;
                this.serialize = serialize;
            }

            public TrackData track;
            public SerializeTrack serialize;
        }

        public TrackAsset asset;
        public TrackEditor manager;

        /// <summary>
        /// TrackBaseとserializeTrackの対応付
        /// 順序も重要で、親から設定する必要があるためListになってます。
        /// </summary>
        public List<TrackPair> trackPairs = new List<TrackPair>();

        /// <summary>
        /// TrackBase検索用
        /// </summary>
        public Dictionary<string, TrackData> trackTable = new Dictionary<string, TrackData>();

        public List<TrackData> tracks = new List<TrackData>();
        public List<TrackElement> elements = new List<TrackElement>();

        public TrackEditorReader(TrackAsset asset, TrackEditor manager)
        {
            this.asset = asset;
            this.manager = manager;
        }

        public void ReadAsset()
        {
            ReadTracks<RootSerializeTrack, RootTrackData>(asset.rootTracks);
            ReadTracks<GameObjectSerializeTrack, GameObjectTrackData>(asset.gameObjectTracks);
            ReadTracks<ActivationSerializeTrack, ActivationTrackData>(asset.activationTracks);
            ReadTracks<PositionSerializeTrack, PositionTrackData>(asset.positionTracks);
            ReadTracks<AnimationSerializeTrack, AnimationTrackData>(asset.animationTracks);

            ReadElements<ActivationSerializeElement, ActivationElement>(asset.activationElements);
            ReadElements<PositionSerializeElement, PositionElement>(asset.positionElements);
            ReadElements<AnimationSerializeElement, AnimationElement>(asset.animationElements);

            // トラックの階層構築
            updateHierarchy(manager, asset.rootTracks[0].uniqueName);
        }

        void ReadTracks<SerializeTrackClass, TrackDataClass>(List<SerializeTrackClass> serializeTracks)
            where SerializeTrackClass : SerializeTrack
            where TrackDataClass : TrackData, new()
        {
            foreach (var serializeTrack in serializeTracks) {
                TrackDataClass track = new TrackDataClass();

                trackPairs.Add(new TrackPair(track, serializeTrack));
                trackTable.Add(serializeTrack.uniqueName, track);
                tracks.Add(track);

                track.ReadAsset(serializeTrack);
            }
        }

        void ReadElements<SerializeElementClass, ElementClass>(List<SerializeElementClass> serializeElements)
            where SerializeElementClass : SerializeElement
            where ElementClass : TrackElement, new()
        {
            foreach (var serializeElement in serializeElements) {
                ElementClass element = new ElementClass();

                var track = trackTable[serializeElement.parent];
                track.elements.Add(element);
                elements.Add(element);

                element.LoadInitialize(serializeElement.name, serializeElement.start, serializeElement.length, track);

                element.ReadAsset(serializeElement);
            }
        }

        void updateHierarchy(TrackEditor manager, string rootTrackName)
        {
            // 親子階層を設定
            foreach (var trackPair in trackPairs) {
                List<TrackData> childs = new List<TrackData>();
                foreach (var child in trackPair.serialize.childs) {
                    //Debug.Log(child);
                    childs.Add(trackTable[child]);
                }
                trackPair.track.childs = childs;

                TrackData parent;
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

