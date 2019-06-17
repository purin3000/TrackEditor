using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

namespace track_editor_example
{
    /// <summary>
    /// トラック情報。RootTrackDataはTrackEditor.top専用
    /// </summary>
    public class RootTrackData : EditorTrack
    {
        public void WriteAsset(WriteAssetContext context)
        {
            var trackSerialize = new RootTrackSerialize();

            SerializeUtility.InitializeTrackSerialize(trackSerialize, this, context);

            context.asset.rootTracks.Add(trackSerialize);
        }

        public void ReadAsset(RootTrackSerialize trackSerialize)
        {
        }
    }

    public class GameObjectTrackData : EditorTrack
    {
        public GameObject go;

        public override void Initialize(TrackEditor manager, string name, EditorTrack parent)
        {
            base.Initialize(manager, name, parent);

            // 初期化時にトラックを生成する場合、Initialize()以降に行う必要がある
            manager.AddTrack(this, "Activation", new ActivationTrackData());
            manager.AddTrack(this, "GameObject", new PositionTrackData());
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width + 2, rect.height - 4);
            if (go) {
                GUI.Label(rectLabel, go.name, IsSelection ? "flow node 0 on" : "flow node 0");
            } else {
                GUI.Label(rectLabel, "None", IsSelection ? "flow node 0 on" : "flow node 0");
            }
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            using (new GUILayout.HorizontalScope()) {
                if (GUILayout.Button("上へ移動")) {
                    var index = parent.childs.IndexOf(this) - 1;
                    if (0 <= index) {
                        parent.childs.SwapAt(index, index + 1);
                    }
                }
                if (GUILayout.Button("下へ移動")) {
                    var index = parent.childs.IndexOf(this) + 1;
                    if (index < parent.childs.Count) {
                        parent.childs.SwapAt(index, index - 1);
                    }
                }
            }

            if (GUILayout.Button("Remove")) {
                manager.RemoveTrack(parent, this);
            }

            go = (GameObject)EditorGUILayout.ObjectField(go, typeof(GameObject), true);
        }

        public void WriteAsset(WriteAssetContext context)
        {
            var trackSerialize = new GameObjectTrackSerialize();

            SerializeUtility.InitializeTrackSerialize(trackSerialize, this, context);

            trackSerialize.go = go;

            context.asset.gameObjectTracks.Add(trackSerialize);
        }

        public void ReadAsset(GameObjectTrackSerialize trackSerialize)
        {
            go = trackSerialize.go;
        }
    }

    public class ActivationTrackData : EditorTrack
    {
        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            if (GUILayout.Button("Add")) {
                manager.AddElement(this, new ActivationElement());
            }
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width + 2, rect.height - 4);
            GUI.Label(rectLabel, "Activation", IsSelection ? "flow node 2 on" : "flow node 2");
        }

        public void WriteAsset(WriteAssetContext context)
        {
            var trackSerialize = new ActivationTrackSerialize();

            SerializeUtility.InitializeTrackSerialize(trackSerialize, this, context);

            context.asset.activationTracks.Add(trackSerialize);
        }

        public void ReadAsset(ActivationTrackSerialize trackSerialize)
        {
        }
    }

    public class PositionTrackData : EditorTrack
    {
        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            if (GUILayout.Button("Add")) {
                manager.AddElement(this, new PositionElement());
            }
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width + 2, rect.height - 4);
            GUI.Label(rectLabel, "Position", IsSelection ? "flow node 2 on" : "flow node 2");
        }

        public void WriteAsset(WriteAssetContext context)
        {
            var trackSerialize = new PositionTrackSerialize();

            SerializeUtility.InitializeTrackSerialize(trackSerialize, this, context);

            context.asset.positionTracks.Add(trackSerialize);
        }

        public void ReadAsset(PositionTrackSerialize trackSerialize)
        {
        }
    }
}
