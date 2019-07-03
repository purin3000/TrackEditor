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

            public override EditorElement CreateElement()
            {
                return null;
            }

            public override void Initialize()
            {
                name = labelName;
                manager.AddTrack(this, new ActivationEditorTrackData());
            }

            public override void TrackHeaderDrawer()
            {
                HeaderDrawerImpl(labelName);
            }

            public override void TrackLabelDrawer(Rect rect)
            {
                Rect rectLabel = CalcTrackLabelRect(rect);
                GUI.Label(rectLabel, "", IsSelection ? "flow node 0 on" : "flow node 0");

                Rect rectObj = CalcTrackObjectRect(rect);
                if (trackData.currentPlayer) {
                    EditorGUI.LabelField(rectObj, "Player");
                } else {
                    using (new EditorGUI.DisabledScope(trackData.currentPlayer)) {
                        trackData.target = (GameObject)EditorGUI.ObjectField(rectObj, trackData.target, typeof(GameObject), true);
                    }
                }

                if (expand && 2 <= childs.Count) {
                    EditorGUI.LabelField(rectLabel, $"{name}");
                }

                if (!expand) {
                    var rectExp = rectObj;
                    rectExp.x += rect.width * 0.8f;
                    EditorGUI.LabelField(rectExp, "[+]");
                }
            }

            public override void TrackPropertyDrawer(Rect rect)
            {
                DrawNameImpl(labelName);

                using (new EditorGUI.DisabledScope(trackData.currentPlayer)) {
                    trackData.target = (GameObject)EditorGUILayout.ObjectField(trackData.target, typeof(GameObject), true);
                }

                trackData.activate = EditorGUILayout.Toggle("Activate", trackData.activate);

                trackData.currentPlayer = EditorGUILayout.Toggle("Current Player", trackData.currentPlayer);

                GUISpace();
                DrawIndexMoveImpl();

                var table = new[] {
                    new { Name = "Activation", Type = typeof(ActivationEditorTrackData) },
                    new { Name = "Transform",  Type = typeof(TransformEditorTrackData) },
                    new { Name = "Animation",  Type = typeof(AnimationEditorTrackData) },
                };

                foreach (var info in table) {
                    AddTrackImpl(info.Name, info.Type);
                }
            }
        }

        public abstract class ChildEditorTrackBase : EditorTrack
        {
            string labelName;

            protected ChildEditorTrackBase(string labelName)
            {
                this.labelName = labelName;
            }

            public override void Initialize()
            {
                name = labelName;
            }

            public override void TrackHeaderDrawer()
            {
                HeaderDrawerImpl(labelName);
            }

            public override void TrackLabelDrawer(Rect rect)
            {
                SubTrackLabelDrawerImpl(rect, labelName);
            }

            public override void TrackPropertyDrawer(Rect rect)
            {
                SubTrackPropertyDrawerImpl(rect, labelName);
            }
        }

        public abstract class ChildEditorElementBase : EditorElement
        {
            public GameObject target => (parent.parent as EditorTrackData).trackData.target;
        }
    }
}
