using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor_fw
{
    public class ElementBase
    {
        public int start;
        public int length;

        public TrackBase parent { get; private set; }

        public bool IsSelection { get => parent.selectionElement == this; }


        Vector2 mouseOffset;
        bool isDrag;

        public virtual void Initialize(TrackBase parent)
        {
            this.parent = parent;
            this.start = parent.trackEditor.currentFrame;
            this.length = 1;
        }

        public void DrawElement(Rect rect)
        {
            var pixelScale = parent.trackEditor.pixelScale;
            var scrPos = parent.trackEditor.scrollPos;

            Rect labelRect = new Rect(rect.x + pixelScale * start - scrPos.x, rect.y - scrPos.y, pixelScale * length, parent.trackEditor.settings.trackHeight);

            ElementDrawer(labelRect);

            if (Event.current.type == EventType.MouseDown) {
                if (labelRect.Contains(Event.current.mousePosition)) {
                    parent.SetSelectionElement(this);
                    parent.Repaint();
                    Event.current.Use();

                    mouseOffset = labelRect.position - Event.current.mousePosition;
                    isDrag = true;
                }

            } else if (Event.current.type == EventType.MouseUp) {
                isDrag = false;

            } else if (Event.current.type == EventType.MouseDrag) {
                if (isDrag) {
                    var currentFrame = (int)((Event.current.mousePosition.x - rect.x + scrPos.x + mouseOffset.x) / pixelScale);

                    start = currentFrame;

                    parent.Repaint();

                    Event.current.Use();
                }
            }
        }

        public virtual void ElementDrawer(Rect rect)
        {
            var pixelScale = parent.trackEditor.pixelScale;
            var scrPos = parent.trackEditor.scrollPos;

            //Rect labelRect = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
            Rect labelRect = new Rect(rect.x + pixelScale * start - scrPos.x, rect.y - scrPos.y, pixelScale * length, parent.trackEditor.settings.trackHeight);
            GUI.Label(rect, "", IsSelection ? "flow node 5 on" : "flow node 5");
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            GUILayout.Label("Element");
            start = Mathf.Max(0, EditorGUILayout.IntField("Start", start));
            length = Mathf.Max(0, EditorGUILayout.IntField("Length", length));
        }
    }
}
