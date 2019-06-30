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

            RemoveTrackImpl($"Remove {name} Track");
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

            if (GUILayout.Button($"Add {ActivationEditorTrack.labelName} Track")) {
                manager.AddTrack(this, new ActivationEditorTrack());
            }

            if (GUILayout.Button($"Add {TransformEditorTrack.labelName} Track")) {
                manager.AddTrack(this, new TransformEditorTrack());
            }

            if (GUILayout.Button($"Add {AnimationEditorTrack.labelName} Track")) {
                manager.AddTrack(this, new AnimationEditorTrack());
            }

            //if (GUILayout.Button($"Add {ScriptTrackEditor.name} Track")) {
            //    manager.AddTrack(this, ScriptTrackEditor.name, ScriptTrackEditor.CreateTrack());
            //}
        }
    }
}
