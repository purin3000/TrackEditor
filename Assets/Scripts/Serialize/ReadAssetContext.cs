using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using track_editor;


namespace track_editor
{

    /// <summary>
    /// アセット読み込み時に使用する一時情報
    /// </summary>
    public class ReadAssetContext
    {
        public class TrackPair
        {
            public TrackPair(EditorTrack track, SerializeTrack serialize)
            {
                this.track = track;
                this.serialize = serialize;
            }

            public EditorTrack track;
            public SerializeTrack serialize;
        }

        public TrackAsset asset;

        /// <summary>
        /// TrackBaseとserializeTrackの対応付
        /// 順序も重要で、親から設定する必要があるためListになってます。
        /// </summary>
        public List<TrackPair> trackPairs = new List<TrackPair>();

        /// <summary>
        /// TrackBase検索用
        /// </summary>
        public Dictionary<string, EditorTrack> trackTable = new Dictionary<string, EditorTrack>();

        public List<EditorTrack> tracks = new List<EditorTrack>();
        public List<EditorElement> elements = new List<EditorElement>();

        public ReadAssetContext(TrackAsset asset)
        {
            this.asset = asset;
        }

        public void ReadAsset()
        {
            // トラック構築
            foreach (var serializeTrack in asset.rootTracks) {
                var track = createEditorTrack<RootTrackData>(serializeTrack);
                track.ReadAsset(serializeTrack);
            }

            foreach (var serializeTrack in asset.gameObjectTracks) {
                var track = createEditorTrack<GameObjectTrackData>(serializeTrack);
                track.ReadAsset(serializeTrack);
            }

            foreach (var serializeTrack in asset.activationTracks) {
                var track = createEditorTrack<ActivationTrackData>(serializeTrack);
                track.ReadAsset(serializeTrack);
            }

            foreach (var serializeTrack in asset.positionTracks) {
                var track = createEditorTrack<PositionTrackData>(serializeTrack);
                track.ReadAsset(serializeTrack);
            }

            foreach (var serializeTrack in asset.animationTracks) {
                var track = createEditorTrack<AnimationTrackData>(serializeTrack);
                track.ReadAsset(serializeTrack);
            }

            // エレメント構築
            foreach (var serializeElement in asset.activationElements) {
                var element = createEditorElement<ActivationElement>(serializeElement);
                element.ReadAsset(serializeElement);
            }

            foreach (var serializeElement in asset.positionElements) {
                var element = createEditorElement<PositionElement>(serializeElement);
                element.ReadAsset(serializeElement);
            }

            foreach (var serializeElement in asset.animationElements) {
                var element = createEditorElement<AnimationElement>(serializeElement);
                element.ReadAsset(serializeElement);
            }
        }

        EditorTrackClass createEditorTrack<EditorTrackClass>(SerializeTrack serializeTrack)
            where EditorTrackClass : EditorTrack, new()
        {
            EditorTrackClass track = new EditorTrackClass();

            trackPairs.Add(new TrackPair(track, serializeTrack));
            trackTable.Add(serializeTrack.uniqueName, track);
            tracks.Add(track);

            return track;
        }

        EditorElementClass createEditorElement<EditorElementClass>(SerializeElement serializeElement) where EditorElementClass : EditorElement, new()
        {
            EditorElementClass element = new EditorElementClass();

            var track = trackTable[serializeElement.parent];
            track.elements.Add(element);
            elements.Add(element);

            element.LoadInitialize(serializeElement.name, serializeElement.start, serializeElement.length, track);
            return element;
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
    }
}

