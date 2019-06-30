using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    public abstract class EditorElement
    {
        public string name;
        public EditorTrack parent;
        public int start;
        public int length = 1;

        public int end => start + end;

        public bool IsSelection => parent.selectionElement == this;

        public float pixelScale => parent.pixelScale;

        public float trackHeight => parent.trackHeight;

        public Vector2 scrollPos => parent.scrollPos;

        public Vector2 mouseOffset;

        public bool isDrag;

        public bool isLengthDrag;

        public bool isFixedLength;


        public void Initialize(string name, EditorTrack parent, int start, int length)
        {
            Debug.Assert(parent != null);
            this.name = name;
            this.parent = parent;
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

        protected void RemoveElementImpl(string label)
        {
            if (GUILayout.Button(label)) {
                parent.manager.RemoveElement(parent, this);
            }
        }

        protected void DrawNameImpl()
        {
            name = EditorGUILayout.TextField("Name", name);
        }

        protected void DrawStartImpl()
        {
            start = Mathf.Max(0, EditorGUILayout.IntField("Start", start));
        }

        protected void DrawLengthImpl()
        {
            using (new EditorGUI.DisabledScope(isFixedLength)) {
                length = Mathf.Max(0, EditorGUILayout.IntField("Length", length));

                if (isFixedLength) {
                    GUILayout.Label("Lengthは固定されています");
                }
            }
        }

        protected void DrawIndexMoveImpl()
        {
            using (new GUILayout.VerticalScope()) {
                if (GUILayout.Button("上へ移動")) {
                    var index = parent.elements.IndexOf(this) - 1;
                    if (0 <= index) {
                        parent.elements.SwapAt(index, index + 1);
                    }
                }
                if (GUILayout.Button("下へ移動")) {
                    var index = parent.elements.IndexOf(this) + 1;
                    if (index < parent.elements.Count) {
                        parent.elements.SwapAt(index, index - 1);
                    }
                }
            }
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            //DrawIndexMoveImpl();
            DrawNameImpl();
            DrawStartImpl();
            DrawLengthImpl();
        }

    }
}
