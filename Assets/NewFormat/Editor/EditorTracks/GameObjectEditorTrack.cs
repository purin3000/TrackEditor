using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentTrackData = GameObjectTrack.TrackData;

    public class GameObjectEditorTrack : EditorTrack
    {
        const string labelName = "GameObject";

        public CurrentTrackData trackData = new CurrentTrackData();

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl($"Remove {labelName} Track");
        }

        public override void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
            GUI.Label(rectLabel, "", IsSelection ? "flow node 0 on" : "flow node 0");

            Rect rectObj = new Rect(rectLabel.x, rect.y + (rectLabel.height - EditorGUIUtility.singleLineHeight) * 0.5f, rectLabel.width * 0.6f, EditorGUIUtility.singleLineHeight);
            if (trackData.currentPlayer) {
                EditorGUI.LabelField(rectObj, "Player");
            } else {
                using (new EditorGUI.DisabledScope(trackData.currentPlayer)) {
                    trackData.target = (GameObject)EditorGUI.ObjectField(rectObj, trackData.target, typeof(GameObject), true);
                }
            }

            if (expand && 2 <= childs.Count) {
                EditorGUI.LabelField(rectLabel, $"{labelName} GameObject");
            }
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            using (new EditorGUI.DisabledScope(trackData.currentPlayer)) {
                trackData.target = (GameObject)EditorGUILayout.ObjectField(trackData.target, typeof(GameObject), true);
            }

            trackData.activate = EditorGUILayout.Toggle("Activate", trackData.activate);

            trackData.currentPlayer = EditorGUILayout.Toggle("Current Player", trackData.currentPlayer);


            DrawIndexMoveImpl();

            //if (GUILayout.Button($"Add {ActivationTrackEditor.name} Track")) {
            //    manager.AddTrack(this, ActivationTrackEditor.name, ActivationTrackEditor.CreateTrack());
            //}

            //if (GUILayout.Button($"Add {TransformTrackEditor.name} Track")) {
            //    manager.AddTrack(this, TransformTrackEditor.name, TransformTrackEditor.CreateTrack());
            //}

            //if (GUILayout.Button($"Add {AnimationTrackEditor.name} Track")) {
            //    manager.AddTrack(this, AnimationTrackEditor.name, AnimationTrackEditor.CreateTrack());
            //}

            //if (GUILayout.Button($"Add {ScriptTrackEditor.name} Track")) {
            //    manager.AddTrack(this, ScriptTrackEditor.name, ScriptTrackEditor.CreateTrack());
            //}
        }
    }
}
