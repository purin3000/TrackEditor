using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

namespace track_editor
{
    public class TrackDataBase : EditorTrack
    {
        protected TrackSerializeClass WriteAssetImpl<TrackSerializeClass>(List<TrackSerializeClass> serializeList, WriteAssetContext context) where TrackSerializeClass : TrackSerialize, new ()
        {
            // 対応するシリアライズ用のクラスを作って
            var trackSerialize = new TrackSerializeClass();

            SerializeUtility.InitializeTrackSerialize(trackSerialize, this, context);

            // リストへ追加
            serializeList.Add(trackSerialize);

            return trackSerialize;
        }


        protected void RemoveTrackImpl(string label)
        {
            if (GUILayout.Button(label)) {
                manager.RemoveTrack(parent, this);
            }

            GUILayout.Space(15);
        }

        protected void AddElementImpl<T>(string label) where T : EditorElement,new()
        {
            if (GUILayout.Button(label)) {
                manager.AddElement(this, new T());
            }
        }
    }

    /// <summary>
    /// トラック情報
    /// RootTrackDataはTrackEditor.top専用
    /// </summary>
    public class RootTrackData : TrackDataBase
    {
        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.rootTracks, context);
        }

        public void ReadAsset(RootTrackSerialize trackSerialize)
        {
        }
    }

    /// <summary>
    /// サブトラック情報
    /// RootTrackDataにぶら下がります
    /// </summary>
    public class GameObjectTrackData : TrackDataBase
    {
        public GameObject target;

        public override void Initialize(TrackEditor manager, string name, EditorTrack parent)
        {
            base.Initialize(manager, name, parent);

            // 初期化時にトラックを生成する場合、Initialize()以降に行う必要がある
            manager.AddTrack(this, "Activation", new ActivationTrackData());
            manager.AddTrack(this, "Position", new PositionTrackData());
            manager.AddTrack(this, "Animation", new AnimationTrackData());
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width + 2, rect.height - 4);
            GUI.Label(rectLabel, "", IsSelection ? "flow node 0 on" : "flow node 0");

            Rect rectObj = new Rect(rectLabel.x, rect.y + (rectLabel.height - EditorGUIUtility.singleLineHeight) * 0.5f, rectLabel.width * 0.6f, EditorGUIUtility.singleLineHeight);
            target = (GameObject)EditorGUI.ObjectField(rectObj, target, typeof(GameObject), true);
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            RemoveTrackImpl("Remove GameObject Track");

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

            GUILayout.Space(10);

            target = (GameObject)EditorGUILayout.ObjectField(target, typeof(GameObject), true);

            GUILayout.Space(10);

            if (GUILayout.Button("Add Activation Track")) {
                manager.AddTrack(this, "Activation", new ActivationTrackData());
            }

            if (GUILayout.Button("Add Position Track")) {
                manager.AddTrack(this, "Position", new PositionTrackData());
            }

            if (GUILayout.Button("Add Animation Track")) {
                manager.AddTrack(this, "Animation", new AnimationTrackData());
            }
        }

        public void WriteAsset(WriteAssetContext context)
        {
            var trackSerialize = WriteAssetImpl(context.asset.gameObjectTracks, context);

            trackSerialize.target = target;
        }

        public void ReadAsset(GameObjectTrackSerialize trackSerialize)
        {
            // WriteAssetは全てをシリアライズするが、
            // ReadAssetはシステム側で共通部分のシリアライズを行うので、ここでは特別なパラメーターだけを復元すれば良い

            target = trackSerialize.target;
        }
    }

    public class ActivationTrackData : TrackDataBase
    {
        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            RemoveTrackImpl("Remove Activation Track");

            AddElementImpl<ActivationElement>("Add Activation Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width + 2, rect.height - 4);
            GUI.Label(rectLabel, "Activation", IsSelection ? "flow node 2 on" : "flow node 2");
        }

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.activationTracks, context);
        }

        public void ReadAsset(ActivationTrackSerialize trackSerialize)
        {
        }
    }

    public class PositionTrackData : TrackDataBase
    {
        public PositionTrackData()
        {
            isFixedLength = true; 
        }


        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            RemoveTrackImpl("Remove Position Track");

            AddElementImpl<PositionElement>("Add Position Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width + 2, rect.height - 4);
            GUI.Label(rectLabel, "Position", IsSelection ? "flow node 2 on" : "flow node 2");
        }

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.positionTracks, context);
        }

        public void ReadAsset(PositionTrackSerialize trackSerialize)
        {
        }
    }

    public class AnimationTrackData : TrackDataBase
    {
        public AnimationTrackData()
        {
            isFixedLength = true;
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            RemoveTrackImpl("Remove Animation Track");

            AddElementImpl<AnimationElement>("Add Animation Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width + 2, rect.height - 4);
            GUI.Label(rectLabel, "Animation", IsSelection ? "flow node 2 on" : "flow node 2");
        }

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.positionTracks, context);
        }

        public void ReadAsset(AnimationTrackSerialize trackSerialize)
        {
        }
    }
}
