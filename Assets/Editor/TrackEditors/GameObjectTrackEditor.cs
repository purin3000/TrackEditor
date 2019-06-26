using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    using CurrentSerializeTrack = GameObjectTrack.SerializeTrack;

    public class GameObjectTrackEditor
    {
        public const string name = "GameObject";

        public static TrackData CreateTrack() => new Track();

        /// <summary>
        /// サブトラック情報
        /// RootTrackDataにぶら下がります
        /// </summary>
        public class Track : TrackData
        {
            public GameObject target;

            public bool activate = true;

            public bool currentPlayer = false;

            public override void Initialize(TrackEditor manager, string name, TrackData parent)
            {
                base.Initialize(manager, name, parent);

                // 初期化時にトラックを生成する場合、Initialize()以降に行う必要がある
                manager.AddTrack(this, ActivationTrackEditor.name, ActivationTrackEditor.CreateTrack());
                //manager.AddTrack(this, PositionTrackDrawer.name, PositionTrackDrawer.CreateTrack());
                //manager.AddTrack(this, AnimationTrackDrawer.name, AnimationTrackDrawer.CreateTrack());
            }

            public override void HeaderDrawer()
            {
                base.HeaderDrawer();

                RemoveTrackImpl($"Remove {name} Track");
            }

            public override void TrackDrawer(Rect rect)
            {
                Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
                GUI.Label(rectLabel, "", IsSelection ? "flow node 0 on" : "flow node 0");

                Rect rectObj = new Rect(rectLabel.x, rect.y + (rectLabel.height - EditorGUIUtility.singleLineHeight) * 0.5f, rectLabel.width * 0.6f, EditorGUIUtility.singleLineHeight);
                if (currentPlayer) {
                    EditorGUI.LabelField(rectObj, "Player");
                } else {
                    using (new EditorGUI.DisabledScope(currentPlayer)) {
                        target = (GameObject)EditorGUI.ObjectField(rectObj, target, typeof(GameObject), true);
                    }
                }

                if (expand && 2 <= childs.Count) {
                    EditorGUI.LabelField(rectLabel, $"{name} {GameObjectTrackEditor.name}");
                }
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                using (new EditorGUI.DisabledScope(currentPlayer)) {
                    target = (GameObject)EditorGUILayout.ObjectField(target, typeof(GameObject), true);
                }

                activate = EditorGUILayout.Toggle("Activate", activate);

                currentPlayer = EditorGUILayout.Toggle("Current Player", currentPlayer);


                DrawIndexMoveImpl();

                if (GUILayout.Button($"Add {ActivationTrackEditor.name} Track")) {
                    manager.AddTrack(this, ActivationTrackEditor.name, ActivationTrackEditor.CreateTrack());
                }

                if (GUILayout.Button($"Add {TransformTrackEditor.name} Track")) {
                    manager.AddTrack(this, TransformTrackEditor.name, TransformTrackEditor.CreateTrack());
                }

                if (GUILayout.Button($"Add {AnimationTrackEditor.name} Track")) {
                    manager.AddTrack(this, AnimationTrackEditor.name, AnimationTrackEditor.CreateTrack());
                }
            }

            public override void WriteAsset(SerializeTrackBase serializeTrack)
            {
                var serialize = serializeTrack as CurrentSerializeTrack;
                serialize.target = target;
                serialize.activate = activate;
                serialize.currentPlayer = currentPlayer;
            }

            public override void ReadAsset(SerializeTrackBase serializeTrack)
            {
                var serialize = serializeTrack as CurrentSerializeTrack;
                target = serialize.target;
                activate = serialize.activate;
                currentPlayer = serialize.currentPlayer;
            }
        }
    }
}