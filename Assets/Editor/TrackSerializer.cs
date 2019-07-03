using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace track_editor2
{
    public static partial class TrackSerializer
    {
        public static void EditorToAsset(TrackEditor manager, TrackAsset asset)
        {
            List<EditorTrack> editorTracks = new List<EditorTrack>();
            List<EditorElement> editorElements = new List<EditorElement>();

            System.Func<EditorTrack, EditorTrack> listup = null;
            listup = (editorTrack) => {
                editorTracks.Add(editorTrack);
                editorElements.AddRange(editorTrack.elements);

                foreach (var child in editorTrack.childs) {
                    listup.Invoke(child);
                }
                return null;
            };

            listup(manager.top);

            editorToAssetInternal(asset, editorTracks, editorElements);

            asset.frameLength = manager.frameLength;

            EditorUtility.SetDirty(asset);
        }

        public static void AssetToEditor(TrackEditor manager, TrackAsset asset)
        {
            EditorTrack[] editorTracks = new EditorTrack[asset.trackCount];
            EditorElement[] editorElements = new EditorElement[asset.elementCount];

            assetToEditorInternal(asset, editorTracks, editorElements);

            // 要素が無くなったときはNULLが入っているはずなので、有効なデータだけで再構成しておく
            editorTracks = editorTracks.Where(obj => obj != null).ToArray();
            editorElements = editorElements.Where(obj => obj != null).ToArray();


            foreach (var editorTrack in editorTracks) {
                editorTrack.parent = GetTrack(editorTrack.parentIndex, editorTracks);
                editorTrack.manager = manager;

                if (editorTrack.parent != null) {
                    editorTrack.parent.childs.Add(editorTrack);
                }
            }

            foreach (var editorElement in editorElements) {
                editorElement.parent.elements.Add(editorElement);
            }

            manager.frameLength = asset.frameLength;

            manager.top = editorTracks[0];
        }

        static AssetTrackClass Serialize<AssetTrackClass>(IEnumerable<EditorTrack> editorTracks, EditorTrack editorTrack) where AssetTrackClass : AssetTrack, new()
        {
            var assetTrack = new AssetTrackClass();

            int trackIndex = GetTrackIndex(editorTrack, editorTracks);
            int parentTrackIndex = GetTrackIndex(editorTrack.parent, editorTracks);

            assetTrack.Initialize(trackIndex, editorTrack.name, parentTrackIndex, editorTrack.expand);

            return assetTrack;
        }

        static EditorTrackClass Deserialize<EditorTrackClass>(AssetTrack assetTrack) where EditorTrackClass : EditorTrack, new()
        {
            var editorTrack = new EditorTrackClass();

            editorTrack.Deserialize(null, assetTrack.name, assetTrack.parentTrackIndex, assetTrack.expand);

            return editorTrack;
        }

        static AssetElementClass Serialize<AssetElementClass>(IEnumerable<EditorTrack> editorTracks, List<EditorElement> editorElements, EditorElement editorElement) where AssetElementClass : AssetElement, new()
        {
            var assetElement = new AssetElementClass();

            int elementIndex = GetElementIndex(editorElement, editorElements);
            int parentTrackIndex = GetTrackIndex(editorElement.parent, editorTracks);

            assetElement.Initialize(elementIndex, editorElement.name, parentTrackIndex, editorElement.start, editorElement.length);

            return assetElement;
        }

        static EditorElementClass Deserialize<EditorElementClass>(IEnumerable<EditorTrack> editorTracks, AssetElement assetElement) where EditorElementClass : EditorElement, new()
        {
            var editorElement = new EditorElementClass();

            editorElement.Deserialize(assetElement.name, GetTrack(assetElement.parentTrackIndex, editorTracks), assetElement.start, assetElement.length);

            return editorElement;
        }

        static EditorTrack GetTrack(int parentTrackIndex, IEnumerable<EditorTrack> editorTracks)
        {
            if (parentTrackIndex != -1) {
                return editorTracks.ElementAt(parentTrackIndex);
            }
            return null;
        }

        static int GetTrackIndex(EditorTrack editorTrack, IEnumerable<EditorTrack> editorTracks)
        {
            if (editorTrack != null) {
                return editorTracks.IndexOf(editorTrack);
            }
            return -1;
        }

        static int GetElementIndex(EditorElement editorElement, IEnumerable<EditorElement> editorElements)
        {
            if (editorElement != null) {
                return editorElements.IndexOf(editorElement);
            }
            return -1;
        }

        static IEnumerable<EditorTrackClass> getEditorTracks<EditorTrackClass>(List<EditorTrack> editorTracks) where EditorTrackClass : EditorTrack
        {
            return editorTracks.Where(obj => obj is EditorTrackClass).Select(editorTrack => (EditorTrackClass)editorTrack);
        }

        static IEnumerable<EditorElementClass> getEditorElements<EditorElementClass>(List<EditorElement> editorElements) where EditorElementClass : EditorElement
        {
            return editorElements.Where(obj => obj is EditorElementClass).Select(editorElement => (EditorElementClass)editorElement);
        }


    }
}
