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
                root.currentFrame = EditorGUILayout.IntField("Frame", root.currentFrame);
                root.frameLength = EditorGUILayout.IntField("Length", root.frameLength);
            }
        }
    }
}
