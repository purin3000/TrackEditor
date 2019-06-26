using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace track_editor
{
    public class TrackEditorGenerator
    {
        readonly static string[] trackTable = {
            "Root",
            "GameObject",
            "Camera",

            "Activation",
            "Transform",
            "Animation",
            "CameraChange",
            "ChangeBgMaterial",
        };

        readonly static string[] elementTable = {
            "Activation",
            "Transform",
            "Animation",
            "CameraChange",
            "ChangeBgMaterial",
        };

        const string pathTrackEditorAsset = "Assets/App/Tools/CutSceneEditor/Editor/TrackEditorAsset_gen.cs";
        const string pathTrackAsset = "Assets/App/Tools/CutSceneEditor/Scripts/TrackAsset_gen.cs";
        const string pathTrackAssetPlayer = "Assets/App/Tools/CutSceneEditor/Scripts/TrackAssetPlayer_gen.cs";

        [MenuItem("Test/TrackEditorGenerate")]
        static void genTest()
        {
            genTrackEditorAsset();
            genTrackAsset();
            genTrackAssetPlayer();
        }

        static void genTrackEditorAsset()
        {
            string strVariable, strReader, strWriter;

            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var dat in trackTable) {
                    stringBuilder.AppendLine($"List<{dat}TrackEditor.Track> {dat}Tracks = new List<{dat}TrackEditor.Track>();");
                }

                foreach (var dat in elementTable) {
                    stringBuilder.AppendLine($"List<{dat}TrackEditor.Element> {dat}Elements = new List<{dat}TrackEditor.Element>();");
                }
                strVariable = stringBuilder.ToString();
            }

            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var dat in trackTable) {
                    stringBuilder.AppendLine($"reader.readTracks(asset.{dat}Tracks, {dat}Tracks);");
                }

                foreach (var dat in elementTable) {
                    stringBuilder.AppendLine($"reader.readElements(asset.{dat}Elements, {dat}Elements);");
                }
                strReader = stringBuilder.ToString();
            }


            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var dat in trackTable) {
                    stringBuilder.AppendLine($"writer.writeTracks({dat}Tracks, asset.{dat}Tracks);");
                }

                foreach (var dat in elementTable) {
                    stringBuilder.AppendLine($"writer.writeElements({dat}Elements, asset.{dat}Elements);");
                }
                strWriter = stringBuilder.ToString();
            }

            string template = @"
// Auto Generate Code
#define ENABLE_GEN_CODE

using System.Collections.Generic;
using UnityEditor;
namespace track_editor {
    public partial class TrackEditorAsset {

#if ENABLE_GEN_CODE
@@VARIABLE
#endif

        void readAssetInternal(TrackEditorReader reader) {

#if ENABLE_GEN_CODE
@@READER
#endif

            reader.updateHierarchy(manager);
        }

        void writeAssetInternal(TrackEditorWriter writer) {

#if ENABLE_GEN_CODE
@@WRITER
#endif

        }
    }
}
";

            var str = template.Replace("@@VARIABLE", strVariable);
            str = str.Replace("@@READER", strReader);
            str = str.Replace("@@WRITER", strWriter);

            System.IO.File.WriteAllText(pathTrackEditorAsset, str);
            AssetDatabase.ImportAsset(pathTrackEditorAsset);
            Debug.Log(pathTrackEditorAsset);
        }

        static void genTrackAsset()
        {
            string strVariable;

            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var dat in trackTable) {
                    stringBuilder.AppendLine($"[HideInInspector]");
                    stringBuilder.AppendLine($"public List<{dat}Track.SerializeTrack> {dat}Tracks = new List<{dat}Track.SerializeTrack>();");
                }

                foreach (var dat in elementTable) {
                    stringBuilder.AppendLine($"[HideInInspector]");
                    stringBuilder.AppendLine($"public List<{dat}Track.SerializeElement> {dat}Elements = new List<{dat}Track.SerializeElement>();");
                }

                strVariable = stringBuilder.ToString();
            }

            string template = @"
// Auto Generate Code
#define ENABLE_GEN_CODE

using System.Collections.Generic;
using UnityEngine;
namespace track_editor {
    public partial class TrackAsset {

#if ENABLE_GEN_CODE
@@VARIABLE
#endif

    }
}
";

            var str = template.Replace("@@VARIABLE", strVariable);

            System.IO.File.WriteAllText(pathTrackAsset, str);
            AssetDatabase.ImportAsset(pathTrackAsset);
            Debug.Log(pathTrackAsset);
        }

        static void genTrackAssetPlayer()
        {
            string strVariable;

            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var dat in trackTable) {
                    stringBuilder.AppendLine($"addTrack(asset.{dat}Tracks);");
                }

                foreach (var dat in elementTable) {
                    stringBuilder.AppendLine($"addElement(asset.{dat}Elements);");
                }

                strVariable = stringBuilder.ToString();
            }

            string template = @"
// Auto Generate Code
#define ENABLE_GEN_CODE

using System.Collections.Generic;
using UnityEngine;
namespace track_editor {
    public partial class TrackAssetPlayer {
        void addTrackAndElement()        
        {

#if ENABLE_GEN_CODE
@@INIT
#endif

        }
    }
}
";

            var str = template.Replace("@@INIT", strVariable);

            System.IO.File.WriteAllText(pathTrackAssetPlayer, str);
            AssetDatabase.ImportAsset(pathTrackAssetPlayer);
            Debug.Log(pathTrackAssetPlayer);
        }
    }
}
