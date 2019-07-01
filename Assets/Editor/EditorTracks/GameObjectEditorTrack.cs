using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentTrackData = GameObjectTrack.TrackData;

    public class GameObjectEditorTrack
    {
        const string labelName = "GameObject";

        public class EditorTrackData : EditorTrack
        {
            public CurrentTrackData trackData = new CurrentTrackData();

            public override void TrackHeaderDrawer()
            {
                HeaderDrawerImpl(labelName);
            }

            public override void TrackLabelDrawer(Rect rect)
            {
                Rect rectLabel = CalcTrackLabelRect(rect);
                GUI.Label(rectLabel, "", IsSelection ? "flow node 0 on" : "flow node 0");

                Rect rectObj = CalcTrackObjectRect(rect);
                trackData.target = (GameObject)EditorGUI.ObjectField(rectObj, trackData.target, typeof(GameObject), true);

                if (expand && 2 <= childs.Count) {
                    EditorGUI.LabelField(rectLabel, $"{name}");
                }
            }

            public override void TrackPropertyDrawer(Rect rect)
            {
                name = EditorGUILayout.TextField("Name", name);

                trackData.target = (GameObject)EditorGUILayout.ObjectField(trackData.target, typeof(GameObject), true);

                trackData.activate = EditorGUILayout.Toggle("Activate", trackData.activate);


                DrawIndexMoveImpl();

                var table = new[] {
                    new { Name = "Activation", Type = typeof(ActivationEditorTrack.EditorTrackData) },
                    new { Name = "Transform", Type = typeof(TransformEditorTrack.EditorTrackData) },
                    new { Name = "Animation", Type = typeof(AnimationEditorTrack.EditorTrackData) },
                };

                foreach (var info in table) {
                    if (GUILayout.Button($"Add Track [{info.Name}]")) {
                        manager.AddTrack(this, (EditorTrack)System.Activator.CreateInstance(info.Type));
                    }
                }
            }
        }

        public abstract class ChildEditorTrackBase : EditorTrack
        {
        }

        public abstract class ChildEditorElementBase : EditorElement
        {
            public GameObject target => (parent.parent as EditorTrackData).trackData.target;
        }
    }
}
