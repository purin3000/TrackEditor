using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

using track_editor_fw;

namespace track_editor_example
{
    /// <summary>
    /// シリアライズの仕組みがポリモーフィズムに対応していないため、
    /// 具体的なクラスを指定する必要があります。
    /// </summary>
    public static class SerializeUtility
    {
        public static void SaveJson(TrackEditor manager, string assetPath)
        {
            var asset = new TrackEditorAsset(manager.frameLength);
            var context = new WriteAssetContext(asset, manager.top);

            // トラック書き出し
            foreach (var track in context.rootTracks) {
                track.WriteAsset(context);
            }

            foreach (var track in context.gameObjectTracks) {
                track.WriteAsset(context);
            }

            foreach (var track in context.activationTracks) {
                track.WriteAsset(context);
            }

            foreach (var track in context.positionTracks) {
                track.WriteAsset(context);
            }

            // エレメント書き出し
            foreach (var element in context.activationElements) {
                element.WriteAsset(context);
            }

            foreach (var element in context.positionElements) {
                element.WriteAsset(context);
            }

            File.WriteAllText(assetPath, JsonUtility.ToJson(asset, true));
        }

        public static TrackEditorAsset LoadJson(TrackEditor manager, string assetPath)
        {
            var asset = JsonUtility.FromJson<TrackEditorAsset>(File.ReadAllText(assetPath));
            var context = new ReadAssetContext();

            // トラック構築
            foreach (var trackSerialize in asset.rootTracks) {
                var track = context.CreateTrack<RootTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            foreach (var trackSerialize in asset.gameObjectTracks) {
                var track = context.CreateTrack<GameObjectTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            foreach (var trackSerialize in asset.activationTracks) {
                var track = context.CreateTrack<ActivationTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            foreach (var trackSerialize in asset.positionTracks) {
                var track = context.CreateTrack<PositionTrackData>(trackSerialize);
                track.ReadAsset(trackSerialize);
            }

            // エレメント構築
            foreach (var elementSerialize in asset.activationElements) {
                var element = context.CreateElement<ActivationElement>(elementSerialize);
                element.ReadAsset(elementSerialize);
            }

            foreach (var elementSerialize in asset.positionElements) {
                var element = context.CreateElement<PositionElement>(elementSerialize);
                element.ReadAsset(elementSerialize);
            }

            // トラックの階層構築
            context.UpdateHierarchy(manager, asset.rootTracks[0].uniqueName);

            return asset;
        }

        public static void InitializeTrackSerialize(TrackSerialize trackSerialize, EditorTrack track, WriteAssetContext context)
        {
            trackSerialize.uniqueName = context.MakeTrackName(track);
            trackSerialize.name = track.name;
            trackSerialize.parent = context.MakeTrackName(track.parent);
            trackSerialize.childs = track.childs.Select(child => context.MakeTrackName(child)).ToArray();
            trackSerialize.elements = track.elements.Select(element => context.MakeElementName(element)).ToArray();
        }

        public static void InitializeElementSerialize(ElementSerialize elementSerialize, EditorTrackElement element, WriteAssetContext context)
        {
            elementSerialize.uniqueName = context.MakeElementName(element);
            elementSerialize.parent = context.MakeTrackName(element.parent);
            elementSerialize.start = element.start;
            elementSerialize.length = element.length;
        }
    }

}
