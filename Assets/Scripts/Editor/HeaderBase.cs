using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor_fw
{
    public class HeaderBase
    {
        public TrackManager manager { get; private set; }

        public HeaderBase(TrackManager manager)
        {
            this.manager = manager;
        }

        public virtual void DrawHeader(Rect rect)
        {
            using (new GUILayout.HorizontalScope()) {
                manager.currentFrame = Mathf.Max(0, EditorGUILayout.IntField("Frame", manager.currentFrame));
                manager.frameLength = Mathf.Max(0, EditorGUILayout.IntField("Length", manager.frameLength));
                manager.gridScale = EditorGUILayout.IntSlider("Scale", (int)manager.gridScale, 1, manager.gridScaleMax);
            }
        }
    }
}
