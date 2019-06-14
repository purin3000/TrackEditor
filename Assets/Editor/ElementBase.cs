using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor_fw
{
    public class ElementBase
    {
        public int start;
        public int length;

        public TrackBase parent { get; private set; }

        public bool IsSelection { get => parent.selectionElement == this; }

        public virtual void Initialize(TrackBase parent)
        {
            this.parent = parent;
            this.start = parent.trackEditor.currentFrame;
            this.length = 1;
        }

        public void DrawElement(Rect rect)
        {
            ElementDrawer(rect);
        }

        public virtual void ElementDrawer(Rect rect)
        {
            var pixelScale = parent.trackEditor.settings.pixelScale;
            var scrPos = parent.trackEditor.scrollPos;

            //Rect labelRect = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
            Rect labelRect = new Rect(rect.x + pixelScale * start - scrPos.x, rect.y - scrPos.y, pixelScale * length, parent.trackEditor.settings.trackHeight);
            GUI.Label(labelRect, "", IsSelection ? "flow node 5 on" : "flow node 5");
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            rect.x = 0;
            rect.y = 0;
            using (new GUILayout.AreaScope(rect, "", "box")) {
                using (new GUILayout.VerticalScope()) {

                    GUILayout.Label("Prop");
                }
            }
        }
    }
}
