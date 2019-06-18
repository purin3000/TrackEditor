using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using track_editor_fw;


namespace track_editor
{
    /// <summary>
    /// アセット書き込み時に使用する一時情報
    /// </summary>
    public class WriteAssetContext
    {
        public TrackAsset asset;
        public TrackEditor manager;

        public List<EditorTrack> trackBaseList;
        public List<EditorElement> elementBaseList;

        public List<RootTrackData> rootTracks = new List<RootTrackData>();
        public List<GameObjectTrackData> gameObjectTracks = new List<GameObjectTrackData>();
        public List<ActivationTrackData> activationTracks = new List<ActivationTrackData>();
        public List<PositionTrackData> positionTracks = new List<PositionTrackData>();
        public List<AnimationTrackData> animationTracks = new List<AnimationTrackData>();

        public List<ActivationElement> activationElements = new List<ActivationElement>();
        public List<PositionElement> positionElements = new List<PositionElement>();
        public List<AnimationElement> animationElements = new List<AnimationElement>();

        public WriteAssetContext(TrackAsset asset, TrackEditor manager)
        {
            var top = manager.top;

            this.asset = asset;
            this.manager = manager;

            trackBaseList = listupTrackBase(new List<EditorTrack>(), top);
            elementBaseList = listupElementBase(new List<EditorElement>(), top);

            rootTracks = GetTracks<RootTrackData>();
            gameObjectTracks = GetTracks<GameObjectTrackData>();
            activationTracks = GetTracks<ActivationTrackData>();
            positionTracks = GetTracks<PositionTrackData>();
            animationTracks = GetTracks<AnimationTrackData>();

            activationElements = GetElements<ActivationElement>();
            positionElements = GetElements<PositionElement>();
            animationElements = GetElements<AnimationElement>();
        }

        public void WriteAsset()
        {

            asset.WriteAsset(manager.frameLength);

            // トラック書き出し
            foreach (var track in rootTracks) { track.WriteAsset(this); }
            foreach (var track in gameObjectTracks) { track.WriteAsset(this); }
            foreach (var track in activationTracks) { track.WriteAsset(this); }
            foreach (var track in positionTracks) { track.WriteAsset(this); }
            foreach (var track in animationTracks) { track.WriteAsset(this); }

            // エレメント書き出し
            foreach (var element in activationElements) { element.WriteAsset(this); }
            foreach (var element in positionElements) { element.WriteAsset(this); }
            foreach (var element in animationElements) { element.WriteAsset(this); }
        }

        List<T> GetTracks<T>() where T : EditorTrack
        {
            return trackBaseList.Where(obj => obj is T).Select(obj => (T)obj).ToList();

        }

        List<T> GetElements<T>() where T : EditorElement
        {
            return elementBaseList.Where(obj => obj is T).Select(obj => (T)obj).ToList();

        }

        static List<EditorTrack> listupTrackBase(List<EditorTrack> list, EditorTrack track)
        {
            list.Add(track);
            foreach (var child in track.childs) {
                listupTrackBase(list, child);
            }
            return list;
        }

        static List<EditorElement> listupElementBase(List<EditorElement> list, EditorTrack track)
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

        public string MakeElementName(EditorElement elementBase)
        {
            return string.Format("Element{0}", elementBaseList.IndexOf(elementBase));
        }
    }
}
