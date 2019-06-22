using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

namespace track_editor
{
    public class TrackData : EditorTrack
    {
        protected SerializeTrackClass WriteAssetImpl<SerializeTrackClass>(List<SerializeTrackClass> serializeList, WriteAssetContext context) where SerializeTrackClass : SerializeTrack, new ()
        {
            // 対応するシリアライズ用のクラスを作って
            var serializeTrack = new SerializeTrackClass();

            SerializeUtility.InitializeTrackSerialize(serializeTrack, this, context);

            // リストへ追加
            serializeList.Add(serializeTrack);

            return serializeTrack;
        }

        protected void RemoveTrackImpl(string label)
        {
            if (selectionElement == null) {
                if (GUILayout.Button(label)) {
                    manager.RemoveTrack(parent, this);
                }
                //using (new GUILayout.VerticalScope()) {
                //}
            }

        }

        protected void RemoveElementImpl()
        {
            selectionElement?.HeaderDrawer();
        }

        protected void AddElementImpl<T>(string label) where T : EditorElement,new()
        {
            using (new GUILayout.VerticalScope()) {
                if (GUILayout.Button(label)) {
                    var element = new T();
                    element.name = string.Format("{0}:{1}", name, elements.Count);
                    manager.AddElement(this, element);
                }
            }
        }

        protected void TrackDrawerImpl(Rect rect, string label)
        {
            Rect rectLabel = new Rect(rect.x + 3, rect.y + 3, rect.width - 6, rect.height - 6);
            GUI.Label(rectLabel, label, IsSelection ? "flow node 3 on" : "flow node 2");
        }
    }

    /// <summary>
    /// トラック情報
    /// RootTrackDataはTrackEditor.top専用
    /// </summary>
    public class RootTrackData : TrackData
    {
        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.rootTracks, context);
        }

        public void ReadAsset(RootSerializeTrack serializeTrack)
        {
        }
    }

    /// <summary>
    /// サブトラック情報
    /// RootTrackDataにぶら下がります
    /// </summary>
    public class GameObjectTrackData : TrackData
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

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl("Remove GameObject Track");
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
            GUI.Label(rectLabel, "", IsSelection ? "flow node 0 on" : "flow node 0");

            Rect rectObj = new Rect(rectLabel.x, rect.y + (rectLabel.height - EditorGUIUtility.singleLineHeight) * 0.5f, rectLabel.width * 0.6f, EditorGUIUtility.singleLineHeight);
            target = (GameObject)EditorGUI.ObjectField(rectObj, target, typeof(GameObject), true);
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            target = (GameObject)EditorGUILayout.ObjectField(target, typeof(GameObject), true);

            GUILayout.Space(15);

            using (new GUILayout.VerticalScope()) {
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

            GUILayout.Space(15);

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
            var serializeTrack = WriteAssetImpl(context.asset.gameObjectTracks, context);

            serializeTrack.target = target;
        }

        public void ReadAsset(GameObjectSerializeTrack serializeTrack)
        {
            // WriteAssetは全てをシリアライズするが、
            // ReadAssetはシステム側で共通部分のシリアライズを行うので、ここでは特別なパラメーターだけを復元すれば良い

            target = serializeTrack.target;
        }
    }

    public class ActivationTrackData : TrackData
    {
        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl("Remove Activation Track");

            RemoveElementImpl();
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            AddElementImpl<ActivationElement>("Add Activation Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, "Activation");
        }

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.activationTracks, context);
        }

        public void ReadAsset(ActivationSerializeTrack serializeTrack)
        {
        }
    }

    public class PositionTrackData : TrackData
    {
        public PositionTrackData()
        {
            isFixedLength = true; 
        }

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl("Remove Position Track");

            RemoveElementImpl();
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            AddElementImpl<PositionElement>("Add Position Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, "Position");
        }

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.positionTracks, context);
        }

        public void ReadAsset(PositionSerializeTrack serializeTrack)
        {
        }
    }

    public class AnimationTrackData : TrackData
    {
        public AnimationTrackData()
        {
            isFixedLength = true;
        }

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl("Remove Animation Track");

            RemoveElementImpl();
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            AddElementImpl<AnimationElement>("Add Animation Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, "Animation");
        }

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.animationTracks, context);
        }

        public void ReadAsset(AnimationSerializeTrack serializeTrack)
        {
        }
    }
}
