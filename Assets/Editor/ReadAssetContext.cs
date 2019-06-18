using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using track_editor_fw;


namespace track_editor
{

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

        TrackAsset asset;

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
        List<EditorElement> elements = new List<EditorElement>();

        public ReadAssetContext(TrackAsset asset)
        {
            this.asset = asset;
        }

        public void ReadAsset()
        {
            // トラック構築
            foreach (var trackSerialize in asset.rootTracks) {
                var track = CreateTrack<RootTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            foreach (var trackSerialize in asset.gameObjectTracks) {
                var track = CreateTrack<GameObjectTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            foreach (var trackSerialize in asset.activationTracks) {
                var track = CreateTrack<ActivationTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            foreach (var trackSerialize in asset.positionTracks) {
                var track = CreateTrack<PositionTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            foreach (var trackSerialize in asset.animationTracks) {
                var track = CreateTrack<AnimationTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            // エレメント構築
            foreach (var elementSerialize in asset.activationElements) {
                var element = CreateElement<ActivationElement>(elementSerialize);
                element.ReadAsset(elementSerialize);
            }

            foreach (var elementSerialize in asset.positionElements) {
                var element = CreateElement<PositionElement>(elementSerialize);
                element.ReadAsset(elementSerialize);
            }

            foreach (var elementSerialize in asset.animationElements) {
                var element = CreateElement<AnimationElement>(elementSerialize);
                element.ReadAsset(elementSerialize);
            }

        }

        public void UpdateHierarchy(TrackEditor manager, string rootTrackName)
        {
            // 親子階層を設定
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

        T CreateTrack<T>(TrackSerialize trackSerialize) where T : EditorTrack, new()
        {
            T track = new T();

            trackPairs.Add(new TrackPair(track, trackSerialize));
            trackTable.Add(trackSerialize.uniqueName, track);
            tracks.Add(track);

            return track;
        }

        T CreateElement<T>(ElementSerialize elementSerialize) where T : EditorElement, new()
        {
            T element = new T();

            var track = trackTable[elementSerialize.parent];
            track.elements.Add(element);
            elements.Add(element);

            element.LoadInitialize(elementSerialize.start, elementSerialize.length, track);
            return element;
        }
    }
}

