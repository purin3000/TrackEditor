using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor_fw
{
    /// <summary>
    /// TrackEditor実装用のエレメント情報
    /// </summary>
    public class EditorElement
    {
        public EditorTrack parent { get; private set; }

        public int start;
        public int length;
        public string name;

        public bool IsSelection { get => parent.selectionElement == this; }

        public float pixelScale { get => parent.pixelScale; }

        public float trackHeight { get => parent.trackHeight; }

        public Vector2 scrollPos { get => parent.scrollPos; }

        public Vector2 mouseOffset { get; set; }

        public bool isDrag { get; set; }

        public bool isLengthDrag { get; set; }

        public virtual void Initialize(EditorTrack parent)
        {
            this.parent = parent;
            this.start = parent.manager.currentFrame;
            this.length = 1;
        }

        public void LoadInitialize(string name, int start, int length, EditorTrack parent)
        {
            this.parent = parent;
            this.name = name;
            this.start = start;
            this.length = length;
        }

        public virtual void HeaderDrawer()
        {

        }

        public virtual void ElementDrawer(Rect rect)
        {
            Rect labelRect = new Rect(rect.x + pixelScale * start - scrollPos.x, rect.y - scrollPos.y, pixelScale * length, trackHeight);
            GUI.Label(rect, "", IsSelection ? "flow node 5 on" : "flow node 5");
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            name = EditorGUILayout.TextField("Name", name);

            start = Mathf.Max(0, EditorGUILayout.IntField("Start", start));

            using (new EditorGUI.DisabledScope(parent.isFixedLength)) {
                length = Mathf.Max(0, EditorGUILayout.IntField("Length", length));

                if (parent.isFixedLength) {
                    GUILayout.Label("Lengthは固定されています");
                }
            }
        }
    }
}
