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
        readonly static string[] Tracks = {
            "Root",
            "GameObject",
            "Activation",
        };

        readonly static string[] Elements = {
            "Activation",
        };

        const string pathTrackEditor_gen = "Assets/NewFormat/Editor/TrackEditor_gen.cs";
        const string pathTrackAsset_gen = "Assets/NewFormat/TrackAsset_gen.cs";

        const string templateTrackEditor_gen = @"// Auto Generate Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public static partial class TrackSerializer
    {
        static void editorToAssetInternal(TrackAsset2 asset, List<EditorTrack> editorTracks, List<EditorElement> editorElements)
        {
@@TO_ASSET
        }

        static void assetToEditorInternal(TrackAsset2 asset, List<EditorTrack> editorTracks, List<EditorElement> editorElements)
        {
@@TO_EDITOR
        }
    }
@@EDITOR_CLASS
}
";

        const string templateTrackAsset_gen = @"// Auto Generate Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
{
    public partial class TrackAsset2 : MonoBehaviour
    {
@@MEMBER
    }

    public partial class TrackAssetPlayer2 : MonoBehaviour
    {
        void initalizeTracksAndElements()
        {
            int trackCount = 0;
            int elementCount = 0;
@@COUNT
            playerTracks = new PlayerTrackBase[trackCount];
            playerElements = new PlayerElementBase[elementCount];
@@INIT
            foreach (var trackPlayer in playerTracks) {
                trackPlayer.parent = getTrackPlayer(trackPlayer.parentTrackIndex);
            }

            foreach (var elementPlayer in playerElements) {
                elementPlayer.parent = getTrackPlayer(elementPlayer.parentTrackIndex);
            }
        }
    }
@@ASSET_CLASS
}
";



        [MenuItem("Test/Generate NewTrackEditor Code")]
        static void GenerateCode()
        {
            GenerateTrackEditor_gen();
            GenerateTrackAsset_gen();
        }

        static void GenerateTrackEditor_gen()
        {
            string filename = pathTrackEditor_gen;
            string output = templateTrackEditor_gen;

            {
                StringBuilder sb = new StringBuilder();
                foreach (var name in Tracks) {
                    sb.AppendLine($@"
            asset.{name}Tracks.Clear();
            foreach (var editorTrack in getEditorTracks<{name}EditorTrack>(editorTracks)) {{
                var assetTrack = Serialize<{name}AssetTrack>(editorTracks, editorTrack);
                assetTrack.trackData = editorTrack.trackData; 
                asset.{name}Tracks.Add(assetTrack);
            }}
");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"
            asset.{name}Elements.Clear();
            foreach (var editorElement in getEditorElements<{name}EditorElement>(editorElements)) {{
                var assetElement = Serialize<{name}AssetElement>(editorTracks, editorElements, editorElement);
                assetElement.elementData = editorElement.elementData;
                asset.{name}Elements.Add(assetElement);
            }}
");
                }
                output = output.Replace("@@TO_ASSET", sb.ToString());
            }

            {
                StringBuilder sb = new StringBuilder();
                foreach (var name in Tracks) {
                    sb.AppendLine($@"
            foreach (var assetTrack in asset.{name}Tracks) {{
                var editorTrack = Deserialize<{name}EditorTrack>(assetTrack);
                editorTrack.trackData = assetTrack.trackData;
                editorTracks.Add(editorTrack);
            }}
");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"
            foreach (var assetElement in asset.{name}Elements) {{
                var editorElement = Deserialize<{name}EditorElement>(editorTracks, assetElement);
                editorElement.elementData = assetElement.elementData;
                editorElements.Add(editorElement);
            }}
");
                }
                output = output.Replace("@@TO_EDITOR", sb.ToString());
            }

//            {
//                StringBuilder sb = new StringBuilder();

//                foreach (var name in Tracks) {
//                    sb.AppendLine($@"
//    public class {name}EditorTrack : EditorTrack {{
//        public {name}Track.TrackData trackData = new {name}Track.TrackData();
//    }}
//");
//                }

//                foreach (var name in Elements) {
//                    sb.AppendLine($@"
//    public class {name}EditorElement : EditorElement {{
//        public {name}Track.ElementData elementData = new {name}Track.ElementData();
//    }}
//");
//                }
//                output = output.Replace("@@EDITOR_CLASS", sb.ToString());
//            }
            output = output.Replace("@@EDITOR_CLASS", "");

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
        [HideInInspector]
        public List<{name}AssetTrack> {name}Tracks = new List<{name}AssetTrack>();
");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"
        [SerializeField]
        [HideInInspector]
        public List<{name}AssetElement> {name}Elements = new List<{name}AssetElement>();
");
                }
                output = output.Replace("@@MEMBER", sb.ToString());
            }

            {
                StringBuilder sb = new StringBuilder();
                foreach (var name in Tracks) {
                    sb.AppendLine($@"            trackCount += asset.{name}Tracks.Count;");
                }

                foreach (var name in Elements) {
                    sb.AppendLine($@"            elementCount += asset.{name}Elements.Count;");
                }
                output = output.Replace("@@COUNT", sb.ToString());
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
            foreach (var assetElement in asset.{name}Elements) {{
                var playerElement = createPlayerElement<{name}Track.PlayerElement>(assetElement);
                playerElement.elementData = assetElement.elementData;
                playerElements[assetElement.elementIndex] = playerElement;
            }}

");
                }
                output = output.Replace("@@INIT", sb.ToString());
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
    public class {name}AssetElement : AssetElement {{
        public {name}Track.ElementData elementData = new {name}Track.ElementData();
    }}
");
                }
                output = output.Replace("@@ASSET_CLASS", sb.ToString());
            }

            File.WriteAllText(filename, output);
            AssetDatabase.ImportAsset(filename);
            Debug.Log(filename);
        }

#if false
        [MenuItem("Test/Create NewTrackData")]
        public static TrackAsset2 CreateNewTrackData()
        {
            //string json;

            {
                List<EditorTrack> editorTracks = new List<EditorTrack>();
                List<EditorElement> editorElements = new List<EditorElement>();


                var rootTrack = new RootEditorTrack();
                editorTracks.Add(rootTrack);
                rootTrack.name = "Root";

                var gameObjectTrack1 = new GameObjectEditorTrack();
                editorTracks.Add(gameObjectTrack1);
                gameObjectTrack1.name = "GameObjeTrack1";
                gameObjectTrack1.trackData.activate = true;

                var gameObjectTrack2 = new GameObjectEditorTrack();
                editorTracks.Add(gameObjectTrack2);
                gameObjectTrack2.name = "GameObjeTrack2";
                gameObjectTrack2.trackData.activate = false;
                gameObjectTrack2.trackData.currentPlayer = true;

                var activationTrack1 = new ActivationEditorTrack();
                editorTracks.Add(activationTrack1);
                activationTrack1.name = "ActivationTrack";

                var activationTrack2 = new ActivationEditorTrack();
                editorTracks.Add(activationTrack2);
                activationTrack2.name = "ActivationTrack2";

                var activationElement1 = new ActivationEditorElement();
                editorElements.Add(activationElement1);
                activationElement1.name = "ActivationElement";

                var activationElement2 = new ActivationEditorElement();
                editorElements.Add(activationElement2);
                activationElement2.name = "ActivationElement2";

                rootTrack.AddTrack(gameObjectTrack1);
                rootTrack.AddTrack(gameObjectTrack2);

                gameObjectTrack2.AddTrack(activationTrack1);
                gameObjectTrack2.AddTrack(activationTrack2);

                activationTrack2.AddElement(activationElement1);
                activationTrack2.AddElement(activationElement2);

                var path = GameObjectUtility.GetUniqueNameForSibling(null, "TrackAsset");

                var asset = new GameObject(path).AddComponent<TrackAsset2>();

                TrackSerializer.EditorToAsset(manager, asset);

                EditorUtility.SetDirty(asset);

                return asset;
                //json = JsonUtility.ToJson(asset, true);
            }

            //Debug.Log(json);

            {
                //    List<IEditorTrack> editorTracks = new List<IEditorTrack>();
                //    List<IEditorElement> editorElements = new List<IEditorElement>();

                //    var asset = JsonUtility.FromJson<TrackAsset>(json);

                //    TrackDeserializer.AssetToEditor(asset, editorTracks, editorElements);

                //    Debug.Log("test");
            }
        }
#endif
    }
}
