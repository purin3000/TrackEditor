using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor_fw
{
    public class HeaderBase
    {
        public TrackEditor root { get; private set; }

        public HeaderBase(TrackEditor root)
        {
            this.root = root;
        }

        public virtual void DrawHeader(Rect rect)
        {
            using (new GUILayout.HorizontalScope()) {
                root.currentFrame = Mathf.Max(0, EditorGUILayout.IntField("Frame", root.currentFrame));
                root.frameLength = Mathf.Max(0, EditorGUILayout.IntField("Length", root.frameLength));
            }
        }
    }
}
