using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentTrackData = TrackGroupTrack.TrackData;

    public class TrackGroupEditorTrack
    {
        const string labelName = "Group";

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

                manager.AddTrack(this, new GameObjectEditorTrack.EditorTrackData());
            }

            public override Rect CalcDrawTrackChildRect(Rect rect, EditorTrack child, float ofsy)
            {
                var slideSize = rect.width * 0.08f;

                float x = rect.x + slideSize;
                float y = rect.y + ofsy;
                float width = rect.width - slideSize;

                return new Rect(x, y, width, child.CalcTrackHeight());
            }

            public override void TrackHeaderDrawer()
            {
                HeaderDrawerImpl(labelName);
            }

            public override void TrackLabelDrawer(Rect rect)
            {
                Rect rectLabel = CalcTrackLabelRect(rect);
                GUI.Label(rectLabel, $"{name}", IsSelection ? "flow node 1 on" : "flow node 1");

                if (!expand) {
                    Rect rectObj = CalcTrackObjectRect(rect);
                    var rectExp = rectObj;
                    rectExp.x += rect.width * 0.8f;
                    EditorGUI.LabelField(rectExp, "[+]");
                }
            }

            public override void TrackPropertyDrawer(Rect rect)
            {
                DrawNameImpl(labelName);

                trackData.activate = EditorGUILayout.Toggle("Activate", trackData.activate);

                GUISpace();
                DrawIndexMoveImpl();

                var table = new[] {
                    new { Name = "GameObject", Type = typeof(GameObjectEditorTrack.EditorTrackData) },
                };

                foreach (var info in table) {
                    AddTrackImpl(info.Name, info.Type);
                }
            }
        }
    }
}
