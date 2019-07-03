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

        public virtual void Initialize()
        {
        }

        public void Deserialize(string name, EditorTrack parent, int start, int length)
        {
            Debug.Assert(parent != null);
            this.name = name;
            this.parent = parent;
            this.start = start;
            this.length = length;
        }

        public abstract void ElementHeaderDrawer();

        public abstract void PropertyDrawer(Rect rect);

        public abstract void ElementDrawer(Rect rect);

        protected void ElementHeaderDrawerImpl(string labelName)
        {
            GUILayout.Label($"{labelName} Element [{name}]");
            if (GUILayout.Button($"Remove {labelName} Element [{name}]")) {
                parent.manager.RemoveElement(parent, this);
            }
        }

        protected void PropertyDrawerImpl(Rect rect, string labelName)
        {
            DrawNameImpl(labelName);
            DrawStartImpl();
            DrawLengthImpl();
            GUISpace();
        }

        protected void ElementDrawerImpl(Rect rect)
        {
            Rect labelRect = new Rect(rect.x + pixelScale * start - scrollPos.x, rect.y - scrollPos.y, pixelScale * length, trackHeight);
            GUI.Label(rect, "", IsSelection ? "flow node 5 on" : "flow node 5");
        }

        protected void GUISpace()
        {
            GUILayout.Space(15);
        }

        protected void DrawNameImpl(string labelName)
        {
            EditorGUILayout.LabelField($"{labelName} Element [{name}]");
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
    }
}
