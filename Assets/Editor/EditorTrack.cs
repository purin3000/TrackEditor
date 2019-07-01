using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace track_editor2
{
    public abstract class EditorTrack
    {
        public string name;
        public EditorTrack parent;
        public int parentIndex;

        public List<EditorTrack> childs = new List<EditorTrack>();
        public List<EditorElement> elements = new List<EditorElement>();

        public TrackEditor manager;

        public EditorElement selectionElement;

        public float trackHeight => manager.trackHeight;

        public bool IsSelection => manager.selectionTrack == this;

        public float pixelScale => manager.pixelScale;

        public Vector2 scrollPos => manager.scrollPos;

        public bool expand = true;

        public List<EditorTrack> removeTracks = new List<EditorTrack>();

        public List<EditorElement> removeElements = new List<EditorElement>();

        public int nestLevel { get; set; } = 0;

        public void Initialize(TrackEditor manager, string name, int parentIndex)
        {
            this.name = name;
            this.parentIndex = parentIndex;
            this.manager = manager;
        }

        public void UpdateNestLevel()
        {
            nestLevel = 0;
            var current = parent;
            while (current != null) {
                ++nestLevel;
                current = current.parent;
            }
        }

        public void AddTrack(EditorTrack child)
        {
            childs.Add(child);
            child.parent = this;
            child.manager = manager;
            child.UpdateNestLevel();
        }

        public void AddElement(EditorElement element)
        {
            elements.Add(element);
            element.parent = this;
        }

        public abstract void TrackHeaderDrawer();

        public abstract void TrackLabelDrawer(Rect rect);

        public abstract void TrackPropertyDrawer(Rect rect);

        public virtual float CalcElementWidth()
        {
            if (0 < childs.Count) {
                return childs.Max(child => child.CalcElementWidth());
            }

            if (0 < elements.Count) {
                return elements.Max(element => (element.start + element.length) * manager.pixelScale);
            }
            return 200;
        }

        public virtual float CalcTrackHeight()
        {
            if (expand && 0 < childs.Count) {
                return childs.Sum(child => child.CalcTrackHeight());
            }
            return trackHeight;
        }

        public virtual EditorElement CreateElement() { return null; }

        protected void MainTrackDrawerImpl(Rect rect, string labelName)
        {
            Rect rectLabel = CalcTrackLabelRect(rect);
            GUI.Label(rectLabel, "", IsSelection ? "flow node 0 on" : "flow node 0");

            Rect rectObj = CalcTrackObjectRect(rect);
            EditorGUI.LabelField(rectObj, $"{labelName}");
        }

        protected void SubTrackLabelDrawerImpl(Rect rect, string labelName)
        {
            Rect rectLabel = new Rect(rect.x + 3, rect.y + 3, rect.width - 6, rect.height - 6);
            GUI.Label(rectLabel, labelName, IsSelection ? "flow node 3 on" : "flow node 2");
        }

        protected void SubTrackPropertyDrawerImpl(Rect rect, string labelName)
        {
            name = EditorGUILayout.TextField("Name", name);

            DrawIndexMoveImpl();

            AddElementImpl($"Add Element [{labelName}]");
        }

        protected Rect CalcTrackLabelRect(Rect rect)
        {
            return new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
        }

        protected Rect CalcTrackObjectRect(Rect rect)
        {
            Rect rectLabel = CalcTrackLabelRect(rect);
            return new Rect(rectLabel.x, rect.y + (rectLabel.height - EditorGUIUtility.singleLineHeight) * 0.5f, rectLabel.width * 0.6f, EditorGUIUtility.singleLineHeight);
        }

        protected void HeaderDrawerImpl(string labelName)
        {
            GUILayout.Label(string.Format("Track:{0}", name));

            RemoveTrackImpl($"Remove Track [{labelName}]");

            RemoveElementImpl();
        }

        protected void DrawIndexMoveImpl()
        {
            GUILayout.Space(15);

            using (new GUILayout.VerticalScope()) {
                if (GUILayout.Button("上へ移動")) {
                    var index = parent.childs.IndexOf(this) - 1;
                    if (0 <= index) {
                        parent.childs.SwapAt(index, index + 1);
                    }
                }
                if (GUILayout.Button("下へ移動")) {
                    var index = parent.childs.IndexOf(this) + 1;
                    if (index < parent.childs.Count) {
                        parent.childs.SwapAt(index, index - 1);
                    }
                }
            }

            GUILayout.Space(15);
        }

        protected void RemoveTrackImpl(string label)
        {
            if (selectionElement == null) {
                if (GUILayout.Button(label)) {
                    manager.RemoveTrack(parent, this);
                }
            }
        }

        protected void RemoveElementImpl()
        {
            selectionElement?.ElementHeaderDrawer();
        }

        protected void AddElementImpl(string label)
        {
            using (new GUILayout.VerticalScope()) {
                if (GUILayout.Button(label)) {
                    var element = CreateElement();
                    if (element != null) {
                        element.name = string.Format("{0}:{1}", name, elements.Count);
                        manager.AddElement(this, element);
                    }
                }
            }
        }
    }
}
