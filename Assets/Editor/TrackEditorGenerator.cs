using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

namespace track_editor2
{
    public static class TrackEditorGenerator
    {

        [MenuItem("Test/TrackEditor Generate Code")]
        static void GenerateCode()
        {
            GenerateTrackEditor_gen();
            GenerateTrackAsset_gen();
        }

        //-------------------------------------------------------------------------------------------------------
        readonly static string[] Tracks = {
            "Root",
            "TrackGroup",

            "GameObject",
        };

        //-------------------------------------------------------------------------------------------------------
        readonly static string[] Elements = {
            "Activation",
            "Transform",
            "Animation",
        };

        //-------------------------------------------------------------------------------------------------------
        const string pathTrackEditor_gen = "Assets/Editor/TrackEditor_gen.cs";
        const string pathTrackAsset_gen  = "Assets/Scripts/TrackAsset_gen.cs";


        //-------------------------------------------------------------------------------------------------------

        const string templateTrackEditor_gen = @"// Auto Generate Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public static partial class TrackSerializer
    {
        static void editorToAssetInternal(TrackAsset asset, List<EditorTrack> editorTracks, List<EditorElement> editorElements)
        {
$TO_ASSET$

            asset.trackCount = editorTracks.Count;
            asset.elementCount = editorElements.Count;

        }

        static void assetToEditorInternal(TrackAsset asset, EditorTrack[] editorTracks, EditorElement[] editorElements)
        {
$TO_EDITOR$
        }
    }
}
";

        const string templateTrackAsset_gen = @"// Auto Generate Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public partial class TrackAsset : MonoBehaviour
    {
$MEMBER$
    }

    public partial class TrackAssetPlayer : MonoBehaviour
    {
        void initalizeTracksAndElements()
        {
            int trackCount = 0;
            int elementCount = 0;
$COUNT$
            playerTracks = new PlayerTrackBase[trackCount];
            playerElements = new PlayerElementBase[elementCount];
$INIT$
            foreach (var trackPlayer in playerTracks) {
                trackPlayer.parent = getTrackPlayer(trackPlayer.parentTrackIndex);
            }

            foreach (var elementPlayer in playerElements) {
                elementPlayer.parent = getTrackPlayer(elementPlayer.parentTrackIndex);
            }
        }
    }
$ASSET_CLASS$
}
";


        static void GenerateTrackEditor_gen()
        {
            string filename = pathTrackEditor_gen;
            string output = templateTrackEditor_gen;

            {
                StringBuilder sb = new StringBuilder();
                foreach (var name in Tracks) {
                    sb.AppendLine($@"
            asset.{name}Tracks.Clear();
            foreach (var editorTrack in getEditorTracks<{name}EditorTrack.EditorTrackData>(editorTracks)) {{
                var assetTrack = Serialize<{name}AssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.{name}Tracks.Add(assetTrack);
            }}
");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"
            asset.{name}Tracks.Clear();
            foreach (var editorTrack in getEditorTracks<{name}EditorTrack.EditorTrackData>(editorTracks)) {{
                var assetTrack = Serialize<{name}AssetTrack>(editorTracks, editorTrack);
                asset.{name}Tracks.Add(assetTrack);
            }}
");
                    sb.AppendLine($@"
            asset.{name}Elements.Clear();
            foreach (var editorElement in getEditorElements<{name}EditorTrack.EditorElementData>(editorElements)) {{
                var assetElement = Serialize<{name}AssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.{name}Elements.Add(assetElement);
            }}
");
                }
                output = output.Replace("$TO_ASSET$", sb.ToString());
            }

            {
                StringBuilder sb = new StringBuilder();
                foreach (var name in Tracks) {
                    sb.AppendLine($@"
            foreach (var assetTrack in asset.{name}Tracks) {{
                var editorTrack = Deserialize<{name}EditorTrack.EditorTrackData>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }}
");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"
            foreach (var assetTrack in asset.{name}Tracks) {{
                var editorTrack = Deserialize<{name}EditorTrack.EditorTrackData>(assetTrack);
                editorTracks[assetTrack.trackIndex] = editorTrack;
            }}
");

                    sb.AppendLine($@"
            foreach (var assetElement in asset.{name}Elements) {{
                var editorElement = Deserialize<{name}EditorTrack.EditorElementData>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements[assetElement.elementIndex] = editorElement;
            }}
");
                }
                output = output.Replace("$TO_EDITOR$", sb.ToString());
            }

            File.WriteAllText(filename, output);
            AssetDatabase.ImportAsset(filename);
            Debug.Log(filename);
        }

        static void GenerateTrackAsset_gen()
        {
            string filename = pathTrackAsset_gen;
            string output = templateTrackAsset_gen;

            {
                StringBuilder sb = new StringBuilder();

                foreach (var name in Tracks) {
                    sb.AppendLine($@"
        [SerializeField]
        //[HideInInspector]
        public List<{name}AssetTrack> {name}Tracks = new List<{name}AssetTrack>();
");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"
        [SerializeField]
        //[HideInInspector]
        public List<{name}AssetTrack> {name}Tracks = new List<{name}AssetTrack>();
");
                    sb.AppendLine($@"
        [SerializeField]
        //[HideInInspector]
        public List<{name}AssetElement> {name}Elements = new List<{name}AssetElement>();
");
                }
                output = output.Replace("$MEMBER$", sb.ToString());
            }

            {
                StringBuilder sb = new StringBuilder();
                foreach (var name in Tracks) {
                    sb.AppendLine($@"            trackCount += asset.{name}Tracks.Count;");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"            trackCount += asset.{name}Tracks.Count;");
                    sb.AppendLine($@"            elementCount += asset.{name}Elements.Count;");
                }
                output = output.Replace("$COUNT$", sb.ToString());
            }

            {
                StringBuilder sb = new StringBuilder();

                foreach (var name in Tracks) {
                    sb.AppendLine($@"
            foreach (var assetTrack in asset.{name}Tracks) {{
                var playerTrack = createPlayerTrack<{name}Track.PlayerTrack>(assetTrack);
                playerTrack.trackData = assetTrack.trackData;
                playerTracks[assetTrack.trackIndex] = playerTrack;
            }}

");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"
            foreach (var assetTrack in asset.{name}Tracks) {{
                var playerTrack = createPlayerTrack<{name}Track.PlayerTrack>(assetTrack);
                playerTracks[assetTrack.trackIndex] = playerTrack;
            }}

");

                    sb.AppendLine($@"
            foreach (var assetElement in asset.{name}Elements) {{
                var playerElement = createPlayerElement<{name}Track.PlayerElement>(assetElement);
                playerElement.elementData = assetElement.elementData;
                playerElements[assetElement.elementIndex] = playerElement;
            }}

");
                }
                output = output.Replace("$INIT$", sb.ToString());
            }

            {
                StringBuilder sb = new StringBuilder();

                foreach (var name in Tracks) {
                    sb.AppendLine($@"
    [System.Serializable]
    public class {name}AssetTrack : AssetTrack {{
        public {name}Track.TrackData trackData = new {name}Track.TrackData();
    }}
");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"
    [System.Serializable]
    public class {name}AssetTrack : AssetTrack {{
    }}
");

                    sb.AppendLine($@"
    [System.Serializable]
    public class {name}AssetElement : AssetElement {{
        public {name}Track.ElementData elementData = new {name}Track.ElementData();
    }}
");
                }
                output = output.Replace("$ASSET_CLASS$", sb.ToString());
            }

            File.WriteAllText(filename, output);
            AssetDatabase.ImportAsset(filename);
            Debug.Log(filename);
        }
    }
}
