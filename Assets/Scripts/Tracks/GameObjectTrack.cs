using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace track_editor
{
    [System.Serializable]
    public class GameObjectSerializeTrack : SerializeTrack
    {
        public GameObject target;
    }


    /// <summary>
    /// サブトラック情報
    /// RootTrackDataにぶら下がります
    /// </summary>
    public class GameObjectTrackData : TrackData
    {
        public GameObject target;

#if UNITY_EDITOR
        public override void Initialize(TrackEditor manager, string name, TrackData parent)
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

            DrawIndexMoveImpl();

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
#endif

        public override void WriteAsset(SerializeTrack serializeTrack)
        {
            var serialize = serializeTrack as GameObjectSerializeTrack;
            serialize.target = target;
        }

        public override void ReadAsset(SerializeTrack serializeTrack)
        {
            var serialize = serializeTrack as GameObjectSerializeTrack;
            target = serialize.target;
        }
    }


}

