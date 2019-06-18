using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using track_editor_fw;

namespace track_editor
{
    public class TestHeader : EditorHeader
    {
        public TestHeader(TrackEditor manager)
           : base(manager)
        {
        }

        public override void DrawHeader(Rect rect)
        {
            base.DrawHeader(rect);

            using (new GUILayout.HorizontalScope()) {

                if (GUILayout.Button("Add ObjectTrack")) {
                    var track = manager.AddTrack(manager.top, string.Format("Track:{0}", manager.top.childs.Count + 1), new GameObjectTrackData());
                    manager.SetSelectionTrack(track);

                }

                if (GUILayout.Button("Save")) {
                    SerializeUtility.SaveGameObject(manager, "TrackAsset");
                }

                if (GUILayout.Button("Load")) {
                    SerializeUtility.LoadGameObject(manager, "TrackAsset");
                }
            }
        }
    }
}
