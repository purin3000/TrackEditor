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
        public static TrackAsset SaveAsset(TrackEditor manager, TrackAsset asset)
        {
            var context = new WriteAssetContext(asset, manager);
            context.WriteAsset();
            return asset;
        }

        public static TrackAsset LoadAsset(TrackEditor manager, TrackAsset asset)
        {
            var context = new ReadAssetContext(asset);
            context.ReadAsset();

            // トラックの階層構築
            context.UpdateHierarchy(manager, asset.rootTracks[0].uniqueName);
            return asset;
        }

#if false
        public static TrackAsset SaveAsset(TrackEditor manager, string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<TrackAsset>(assetPath);
            if (asset == null) {
                asset = ScriptableObject.CreateInstance<TrackAsset>();
                AssetDatabase.CreateAsset(asset, assetPath);
                Undo.RegisterCompleteObjectUndo(asset, "create");
            }

            var newAsset = SaveAsset(manager, asset);
            EditorUtility.SetDirty(newAsset);
            return newAsset;
        }

        public static TrackAsset LoadAsset(TrackEditor manager, string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<TrackAsset>(assetPath);
            return LoadAsset(manager, asset);
        }
#endif

#if true
        public static TrackAsset SaveGameObject(TrackEditor manager, string assetPath)
        {
            var asset = GameObject.Find(assetPath)?.GetComponent<TrackAsset>();
            if (asset == null) {
                asset = (new GameObject(assetPath)).AddComponent<TrackAsset>();
                Undo.RegisterCompleteObjectUndo(asset, "create");
            }

            var newAsset = SaveAsset(manager, asset);
            EditorUtility.SetDirty(newAsset);
            return newAsset;
        }

        public static TrackAsset LoadGameObject(TrackEditor manager, string assetPath)
        {
            var asset = GameObject.Find(assetPath)?.GetComponent<TrackAsset>();
            return LoadAsset(manager, asset);
        }
#endif

#if false
        public static TrackAsset SaveJson(TrackEditor manager, string assetPath)
        {
            var asset = new TrackAsset();
            var newAsset = SaveAsset(manager, asset);
            File.WriteAllText(assetPath, JsonUtility.ToJson(newAsset, true));
            AssetDatabase.ImportAsset(assetPath);
            return newAsset;
        }

        public static TrackAsset LoadJson(TrackEditor manager, string assetPath)
        {
            var asset = JsonUtility.FromJson<TrackAsset>(File.ReadAllText(assetPath));
            return LoadAsset(manager, asset);
        }
#endif

        public static void InitializeTrackSerialize(SerializeTrack trackSerialize, EditorTrack track, WriteAssetContext context)
        {
            trackSerialize.uniqueName = context.MakeTrackName(track);
            trackSerialize.name = track.name;
            trackSerialize.parent = context.MakeTrackName(track.parent);
            trackSerialize.childs = track.childs.Select(child => context.MakeTrackName(child)).ToArray();
            trackSerialize.elements = track.elements.Select(element => context.MakeElementName(element)).ToArray();
        }

        public static void InitializeElementSerialize(SerializeElement elementSerialize, EditorElement element, WriteAssetContext context)
        {
            elementSerialize.uniqueName = context.MakeElementName(element);
            elementSerialize.parent = context.MakeTrackName(element.parent);
            elementSerialize.start = element.start;
            elementSerialize.length = element.length;
        }
    }

}
