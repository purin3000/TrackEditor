using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor_fw
{
    public class HeaderBase
    {
        public TrackManager trackEditor { get; private set; }

        public HeaderBase(TrackManager root)
        {
            this.trackEditor = root;
        }

        public virtual void DrawHeader(Rect rect)
        {
            using (new GUILayout.HorizontalScope()) {
                trackEditor.currentFrame = Mathf.Max(0, EditorGUILayout.IntField("Frame", trackEditor.currentFrame));
                trackEditor.frameLength = Mathf.Max(0, EditorGUILayout.IntField("Length", trackEditor.frameLength));
                trackEditor.gridScale = EditorGUILayout.IntSlider("Scale", (int)trackEditor.gridScale, 1, trackEditor.gridScaleMax);
            }
        }
    }
}
