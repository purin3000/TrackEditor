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
        TrackAsset asset;
        TrackEditor manager;

        List<TrackData> trackBaseList;
        List<TrackElement> elementBaseList;

        List<RootTrackData> rootTracks = new List<RootTrackData>();
        List<GameObjectTrackData> gameObjectTracks = new List<GameObjectTrackData>();
        List<ActivationTrackData> activationTracks = new List<ActivationTrackData>();
        List<PositionTrackData> positionTracks = new List<PositionTrackData>();
        List<AnimationTrackData> animationTracks = new List<AnimationTrackData>();

        List<ActivationElement> activationElements = new List<ActivationElement>();
        List<PositionElement> positionElements = new List<PositionElement>();
        List<AnimationElement> animationElements = new List<AnimationElement>();

        public TrackEditorWriter(TrackAsset asset, TrackEditor manager)
        {
            var top = manager.top;

            this.asset = asset;
            this.manager = manager;

            trackBaseList = editorTrackListup(new List<TrackData>(), top);
            elementBaseList = editorElementListup(new List<TrackElement>(), top);

            rootTracks = getEditorTracks<RootTrackData>();
            gameObjectTracks = getEditorTracks<GameObjectTrackData>();
            activationTracks = getEditorTracks<ActivationTrackData>();
            positionTracks = getEditorTracks<PositionTrackData>();
            animationTracks = getEditorTracks<AnimationTrackData>();

            activationElements = getEditorElements<ActivationElement>();
            positionElements = getEditorElements<PositionElement>();
            animationElements = getEditorElements<AnimationElement>();
        }

        public void WriteAsset()
        {
            asset.WriteAsset(manager.frameLength);

            // トラック書き出し
            WriteTracks(rootTracks, asset.rootTracks);
            WriteTracks(gameObjectTracks, asset.gameObjectTracks);
            WriteTracks(activationTracks, asset.activationTracks);
            WriteTracks(positionTracks, asset.positionTracks);
            WriteTracks(animationTracks, asset.animationTracks);

            // エレメント書き出し
            WriteElements(activationElements, asset.activationElements);
            WriteElements(positionElements, asset.positionElements);
            WriteElements(animationElements, asset.animationElements);
        }


        void WriteTracks<TrackDataClass, SerializeTrackClass>(List<TrackDataClass> tracks, List<SerializeTrackClass> serializeTracks)
            where TrackDataClass : TrackData
            where SerializeTrackClass : SerializeTrack, new()
        {
            foreach (var track in tracks) {
                // 対応するシリアライズ用のクラスを作って
                var serializeTrack = new SerializeTrackClass();

                SerializeUtility.InitializeTrackSerialize(serializeTrack, track, this);

                // リストへ追加
                serializeTracks.Add(serializeTrack);

                track.WriteAsset(serializeTrack);
            }
        }

        void WriteElements<TrackElementClass, SerializeElementClass>(List<TrackElementClass> elements, List<SerializeElementClass> serializeElements)
            where TrackElementClass : TrackElement
            where SerializeElementClass : SerializeElement, new()
        {
            foreach (var element in elements) {
                var serializeElement = new SerializeElementClass();

                SerializeUtility.InitializeElementSerialize(serializeElement, element, this);

                serializeElements.Add(serializeElement);

                element.WriteAsset(serializeElement);
            }
        }

        List<T> getEditorTracks<T>() where T : TrackData
        {
            return trackBaseList.Where(obj => obj is T).Select(obj => (T)obj).ToList();

        }

        List<T> getEditorElements<T>() where T : TrackElement
        {
            return elementBaseList.Where(obj => obj is T).Select(obj => (T)obj).ToList();

        }

        static List<TrackData> editorTrackListup(List<TrackData> list, TrackData track)
        {
            list.Add(track);
            foreach (var child in track.childs) {
                editorTrackListup(list, child);
            }
            return list;
        }

        static List<TrackElement> editorElementListup(List<TrackElement> list, TrackData track)
        {
            list.AddRange(track.elements);
            foreach (var child in track.childs) {
                editorElementListup(list, child);
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
