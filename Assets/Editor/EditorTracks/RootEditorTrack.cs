using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using CurrentTrackData = RootTrack.TrackData;

    class RootEditorTrack
    {
        public const string labelName = "Root";

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
                manager.AddTrack(this, new TrackGroupEditorTrack.EditorTrackData());
            }

            public override Rect CalcDrawTrackChildRect(Rect rect, EditorTrack child, float ofsy)
            {
                return new Rect(rect.x, rect.y + ofsy, rect.width, child.CalcTrackHeight());
            }

            public override void TrackHeaderDrawer() { }
            public override void TrackLabelDrawer(Rect rect) { }
            public override void TrackPropertyDrawer(Rect rect)
            {
                name = EditorGUILayout.TextField($"{labelName} Name", name);

                //DrawIndexMoveImpl();

                var table = new[] {
                    new { Name = "Group",      Type = typeof(TrackGroupEditorTrack.EditorTrackData) },
                    new { Name = "GameObject", Type = typeof(GameObjectEditorTrack.EditorTrackData) },
                };

                foreach (var info in table) {
                    AddTrackImpl(info.Name, info.Type);
                }
            }
        }
    }
}

