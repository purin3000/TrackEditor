using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor_fw
{
    /// <summary>
    /// ヘッダ領域描画用
    /// </summary>
    public class EditorHeader
    {
        public TrackEditor manager { get; private set; }

        public EditorHeader(TrackEditor manager)
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
