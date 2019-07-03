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
        public int parentIndex = -1;
        public bool expand = true;

        public List<EditorTrack> childs = new List<EditorTrack>();
        public List<EditorElement> elements = new List<EditorElement>();

        public TrackEditor manager;

        public EditorElement selectionElement;

        public float trackHeight => manager.trackHeight;

        public bool IsSelection => manager.selectionTrack == this;

        public float pixelScale => manager.pixelScale;

        public Vector2 scrollPos => manager.scrollPos;

        public List<EditorTrack> removeTracks = new List<EditorTrack>();

        public List<EditorElement> removeElements = new List<EditorElement>();

        public virtual void Initialize()
        {
        }

        public void Deserialize(TrackEditor manager, string name, int parentIndex, bool expand)
        {
            this.name = name;
            this.parentIndex = parentIndex;
            this.manager = manager;
            this.expand = expand;
        }

        public void AddTrack(EditorTrack child)
        {
            childs.Add(child);
            child.parent = this;
            child.manager = manager;
            child.Initialize();
        }

        public void AddElement(EditorElement element)
        {
            elements.Add(element);
            element.parent = this;
            element.Initialize();
        }

        public abstract void TrackHeaderDrawer();

        public abstract void TrackLabelDrawer(Rect rect);

        public abstract void TrackPropertyDrawer(Rect rect);

        public abstract EditorElement CreateElement();

        protected ElementClass CreateElementImpl<ElementClass>(string labelName) where ElementClass : EditorElement, new()
        {
            var element = new ElementClass();
            element.name = labelName;
            return element;
        }

        protected void MainTrackLabelDrawerImpl(Rect rect, string labelName)
        {
            Rect rectLabel = CalcTrackLabelRect(rect);
            GUI.Label(rectLabel, "", IsSelection ? "flow node 0 on" : "flow node 0");

            Rect rectObj = CalcTrackObjectRect(rect);
            EditorGUI.LabelField(rectObj, $"{labelName}");

            if (!expand) {
                var rectExp = rectObj;
                rectExp.x += rect.width * 0.8f;
                EditorGUI.LabelField(rectExp, "[+]");
            }
        }

        protected void SubTrackLabelDrawerImpl(Rect rect, string labelName)
        {
            Rect rectLabel = new Rect(rect.x + 3, rect.y + 3, rect.width - 6, rect.height - 6);
            GUI.Label(rectLabel, labelName, IsSelection ? "flow node 3 on" : "flow node 2");
        }

        protected void SubTrackPropertyDrawerImpl(Rect rect, string labelName)
        {
            DrawNameImpl(labelName);
            DrawIndexMoveImpl();
            AddElementImpl(labelName);
        }

        protected void AddElementImpl(string labelName)
        {
            using (new GUILayout.VerticalScope()) {
                if (GUILayout.Button($"Add Element [{labelName}]")) {
                    var element = CreateElement();
                    if (element != null) {
                        manager.AddElement(this, element);
                    }
                }
            }
        }

        protected void HeaderDrawerImpl(string labelName)
        {
            if (selectionElement == null) {
                GUILayout.Label($"{labelName} Track [{name}]");
                if (GUILayout.Button($"Remove {labelName} Track [{name}]")) {
                    manager.RemoveTrack(parent, this);
                }
            } else {
                selectionElement.ElementHeaderDrawer();
            }
        }

        protected void DrawNameImpl(string labelName)
        {
            EditorGUILayout.LabelField($"{labelName} Track [{name}]");
            name = EditorGUILayout.TextField("Name", name);

            GUISpace();
        }

        protected void AddTrackImpl(string name, System.Type t)
        {
            if (GUILayout.Button($"Add Track [{name}]")) {
                var track = (EditorTrack)System.Activator.CreateInstance(t);
                manager.AddTrack(this, track);
                manager.SetSelectionTrack(track);
            }
        }

        protected void GUISpace()
        {
            GUILayout.Space(15);
        }

        protected void DrawIndexMoveImpl()
        {
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

            GUISpace();
        }
        
        public virtual Rect CalcDrawTrackChildRect(Rect rect, EditorTrack child, float ofsy)
        {
            var slideSize = rect.width * 0.6f;

            float x = rect.x + slideSize;
            float y = rect.y + ofsy;
            float width = rect.width - slideSize;

            return new Rect(x, y, width, child.CalcTrackHeight());
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

    }
}
