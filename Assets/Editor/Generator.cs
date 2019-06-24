using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace track_editor
{
    public class Generator
    {
        readonly static string[] trackTable = {
            "Root",
            "GameObject",
            "Activation",
            "Position",
            "Animation",
        };

        readonly static string[] elementTable = {
            "Activation",
            "Position",
            "Animation",
        };

        const string pathTrackEditorAsset = "Assets/Editor/TrackEditorAsset_gen.cs";
        const string pathTrackAsset       = "Assets/Scripts/TrackAsset_gen.cs";
        const string pathTrackAssetPlayer = "Assets/Scripts/TrackAssetPlayer_gen.cs";

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
                    stringBuilder.AppendLine($"List<{dat}TrackData> {dat}Tracks = new List<{dat}TrackData>();");
                }

                foreach (var dat in elementTable) {
                    stringBuilder.AppendLine($"List<{dat}Element> {dat}Elements = new List<{dat}Element>();");
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
using System.Collections.Generic;
using UnityEditor;
namespace track_editor {
    public partial class TrackEditorAsset {

@@VARIABLE

        void readAssetInternal(TrackEditorReader reader) {

@@READER

            reader.updateHierarchy(manager);
        }

        void writeAssetInternal(TrackEditorWriter writer) {

@@WRITER

        }
    }
}
";

            var str = template.Replace("@@VARIABLE", strVariable);
            str = str.Replace("@@READER", strReader);
            str = str.Replace("@@WRITER", strWriter);

            System.IO.File.WriteAllText(pathTrackEditorAsset, str);
            AssetDatabase.ImportAsset(pathTrackEditorAsset);
        }

        static void genTrackAsset()
        {
            string strVariable;

            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var dat in trackTable) {
                    stringBuilder.AppendLine($"[HideInInspector]");
                    stringBuilder.AppendLine($"public List<{dat}SerializeTrack> {dat}Tracks = new List<{dat}SerializeTrack>();");
                }

                foreach (var dat in elementTable) {
                    stringBuilder.AppendLine($"[HideInInspector]");
                    stringBuilder.AppendLine($"public List<{dat}SerializeElement> {dat}Elements = new List<{dat}SerializeElement>();");
                }

                strVariable = stringBuilder.ToString();
            }

            string template = @"
// Auto Generate Code
using System.Collections.Generic;
using UnityEngine;
namespace track_editor {
    public partial class TrackAsset {

@@VARIABLE

    }
}
";

            var str = template.Replace("@@VARIABLE", strVariable);

            System.IO.File.WriteAllText(pathTrackAsset, str);
            AssetDatabase.ImportAsset(pathTrackAsset);
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
using System.Collections.Generic;
using UnityEngine;
namespace track_editor {
    public partial class TrackAssetPlayer {
        void addTrackAndElement()        
        {

@@INIT

        }
    }
}
";

            var str = template.Replace("@@INIT", strVariable);

            System.IO.File.WriteAllText(pathTrackAssetPlayer, str);
            AssetDatabase.ImportAsset(pathTrackAssetPlayer);
        }
    }
}
