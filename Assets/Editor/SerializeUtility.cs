using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

using track_editor_fw;

namespace track_editor
{
    public static class SerializeUtility
    {
        public static void SaveGameObject(TrackEditor manager, string assetPath)
        {
            var asset = GameObject.Find(assetPath)?.GetComponent<TrackAsset>();
            if (asset == null) {
                asset = (new GameObject(assetPath)).AddComponent<TrackAsset>();
                Undo.RegisterCompleteObjectUndo(asset, "create");
            }

            var context = new WriteAssetContext(asset, manager);

            context.WriteAsset();

            EditorUtility.SetDirty(asset);
        }

        public static TrackAsset LoadGameObject(TrackEditor manager, string assetPath)
        {
            var asset = GameObject.Find(assetPath)?.GetComponent<TrackAsset>();
            var context = new ReadAssetContext(asset);

            context.ReadAsset();

            // トラックの階層構築
            context.UpdateHierarchy(manager, asset.rootTracks[0].uniqueName);

            return asset;
        }

        public static void SaveJson(TrackEditor manager, string assetPath)
        {
            var asset = new TrackAsset();
            var context = new WriteAssetContext(asset, manager);

            context.WriteAsset();

            File.WriteAllText(assetPath, JsonUtility.ToJson(asset, true));
        }

        public static TrackAsset LoadJson(TrackEditor manager, string assetPath)
        {
            var asset = JsonUtility.FromJson<TrackAsset>(File.ReadAllText(assetPath));
            var context = new ReadAssetContext(asset);

            context.ReadAsset();

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
