using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace track_editor2
{
    public static partial class TrackSerializer
    {
        public static void EditorToAsset(TrackEditor manager, TrackAsset2 asset)
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
        }

        public static void AssetToEditor(TrackEditor manager, TrackAsset2 asset)
        {
            List<EditorTrack> editorTracks = new List<EditorTrack>();
            List<EditorElement> editorElements = new List<EditorElement>();

            assetToEditorInternal(asset, editorTracks, editorElements);

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

            foreach (var editorTrack in editorTracks) {
                editorTrack.UpdateNestLevel();
            }

            manager.top = editorTracks[0];
        }

        static AssetTrackClass Serialize<AssetTrackClass>(List<EditorTrack> editorTracks, EditorTrack editorTrack) where AssetTrackClass : AssetTrack, new()
        {
            var assetTrack = new AssetTrackClass();

            int trackIndex = GetTrackIndex(editorTrack, editorTracks);
            int parentTrackIndex = GetTrackIndex(editorTrack.parent, editorTracks);

            assetTrack.Initialize(trackIndex, editorTrack.name, parentTrackIndex);

            return assetTrack;
        }

        static EditorTrackClass Deserialize<EditorTrackClass>(AssetTrack assetTrack) where EditorTrackClass:EditorTrack,new()
        {
            var editorTrack = new EditorTrackClass();

            editorTrack.Initialize(null, assetTrack.name, assetTrack.parentTrackIndex);

            return editorTrack;
        }

        static AssetElementClass Serialize<AssetElementClass>(List<EditorTrack> editorTracks, List<EditorElement> editorElements, EditorElement editorElement) where AssetElementClass : AssetElement, new()
        {
            var assetElement = new AssetElementClass();

            int elementIndex = GetElementIndex(editorElement, editorElements);
            int parentTrackIndex = GetTrackIndex(editorElement.parent, editorTracks);

            assetElement.Initialize(elementIndex, editorElement.name, parentTrackIndex, editorElement.start, editorElement.length);

            return assetElement;
        }

        static EditorElementClass Deserialize<EditorElementClass>(List<EditorTrack> editorTracks, AssetElement assetElement) where EditorElementClass:EditorElement,new()
        {
            var editorElement = new EditorElementClass();

            editorElement.Initialize(assetElement.name, GetTrack(assetElement.parentTrackIndex, editorTracks), assetElement.start, assetElement.length);

            return editorElement;
        }

        static EditorTrack GetTrack(int parentTrackIndex, List<EditorTrack> editorTracks)
        {
            if (parentTrackIndex != -1) {
                return editorTracks[parentTrackIndex];
            }
            return null;
        }

        static int GetTrackIndex(EditorTrack editorTrack, List<EditorTrack> editorTracks)
        {
            if (editorTrack != null) {
                return editorTracks.IndexOf(editorTrack);
            }
            return -1;
        }

        static int GetElementIndex(EditorElement editorElement, List<EditorElement> editorElements)
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
