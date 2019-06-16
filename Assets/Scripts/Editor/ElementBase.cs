using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor_fw
{
    [System.Serializable]
    public class ElementBase
#if UNITY_EDITOR
        : ElementGUIParam
#endif
    {
        public int start;
        public int length;

#if UNITY_EDITOR
        public override void Initialize(TrackBase parent)
        {
            base.Initialize(parent);

            this.start = parent.manager.currentFrame;
            this.length = 1;
        }

        public virtual void ElementDrawer(Rect rect)
        {
            Rect labelRect = new Rect(rect.x + pixelScale * start - scrollPos.x, rect.y - scrollPos.y, pixelScale * length, trackHeight);
            GUI.Label(rect, "", IsSelection ? "flow node 5 on" : "flow node 5");
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            GUILayout.Label("Element");
            start = Mathf.Max(0, EditorGUILayout.IntField("Start", start));
            length = Mathf.Max(0, EditorGUILayout.IntField("Length", length));
        }
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// エディット時のみ使用可能なもの
    /// </summary>
    public class ElementGUIParam
    {
        public virtual void Initialize(TrackBase parent)
        {
            this.parent = parent;
        }

        public TrackBase parent { get; private set; }

        public bool IsSelection { get => parent.selectionElement == this; }

        public float pixelScale { get => parent.pixelScale; }

        public float trackHeight { get => parent.trackHeight; }

        public Vector2 scrollPos { get => parent.scrollPos; }

        public Vector2 mouseOffset { get; set; }

        public bool isDrag { get; set; }

        public bool isLengthDrag { get; set; }
    }
#endif
}
