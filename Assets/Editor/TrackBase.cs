using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor_fw
{
    public class TrackBase
    {
        public string name;

        public TrackEditor trackEditor { get; private set; }

        public TrackBase parent { get; private set; }

        public List<TrackBase> childs { get; private set; } = new List<TrackBase>();

        public List<ElementBase> elements { get; private set; } = new List<ElementBase>();

        public ElementBase selectionElement { get; private set; }

        public bool expand { get; set; } = true;

        public bool IsSelection { get => trackEditor.selectionTrack == this; }

        List<TrackBase> _removeRequest = new List<TrackBase>();

        int _nestLevel = 0;

        public virtual void Initialize(TrackEditor trackEditor, string name, TrackBase parent)
        {
            this.name = name;
            this.trackEditor = trackEditor;
            this.parent = parent;

            _nestLevel = 0;
            var current = parent;
            while (current != null) {
                ++_nestLevel;
                current = current.parent;
            }
        }

        public T AddTrack<T>(string name, T child) where T : TrackBase
        {
            childs.Add(child);
            child.Initialize(trackEditor, name, this);
            return child;
        }

        public void RemoveTrack(TrackBase track)
        {
            _removeRequest.Add(track);
        }

        public T AddElement<T>(T element) where T : ElementBase
        {
            elements.Add(element);
            element.Initialize(this);
            return element;
        }

        public void SetSelectionElement(ElementBase element)
        {
            selectionElement = element;
        }

        public void DrawTrack(Rect rect) {

            TrackDrawer(rect);

            if (expand) {
                // 深さに応じて表示位置をずらす
                var slideSize = (_nestLevel == 0) ? 0.0f : rect.width * trackEditor.settings.childTrackSlide;

                float x = rect.x + slideSize;
                float y = rect.y;
                float width  = rect.width - slideSize;
                foreach (var child in childs) {
                    Rect rectChild = new Rect(x, y, width, child.CalcTrackHeight());
                    child.DrawTrack(rectChild);
                    y += rectChild.height;
                }
            }

            if (Event.current.type == EventType.MouseDown) {
                if (rect.Contains(Event.current.mousePosition)) {
                    if (childs.Count == 0) {
                        OnTrackSelection();

                    } else {
                        if (IsSelection) {
                            expand = !expand;
                            trackEditor.Repaint();

                        } else {
                            trackEditor.SetSelectionTrack(this);
                        }

                    }
                    Event.current.Use();
                }
            }

            // 安全なタイミングで削除
            foreach (var track in _removeRequest) {
                int index = childs.IndexOf(track);
                if (index != -1) {
                    childs.RemoveAt(index);
                }
            }
            _removeRequest.Clear();
        }

        public void DrawElement(Rect rect) {
            if (childs.Count == 0) {
                Rect rectElement = new Rect(rect.x, rect.y, rect.width, trackEditor.settings.trackHeight);
                foreach (var element in elements) {

                    element.DrawElement(rectElement);

                    if (Event.current.type == EventType.MouseDown) {
                        if (rect.Contains(Event.current.mousePosition)) {
                            SetSelectionElement(element);
                            Event.current.Use();
                        }
                    }
                }
            } else {
                if (expand) {
                    float y = rect.y;
                    foreach (var child in childs) {
                        Rect rectChild = new Rect(rect.x, y, rect.width, child.CalcTrackHeight());
                        child.DrawElement(rectChild);
                        y += rectChild.height;
                    }
                }
            }
        }

        public virtual void TrackDrawer(Rect rect)
        {
            Rect labelRect = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
            GUI.Label(labelRect, "DrawTrack:" + name, IsSelection ? "flow node 0 on" : "flow node 0");
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            GUI.Label(new Rect(Vector2.zero, rect.size), "DrawProperty:" + name, "box");


            if (selectionElement != null) {
                Rect elementRect = new Rect(rect.x, rect.y + 40, rect.width, rect.height - 40);
                selectionElement.PropertyDrawer(elementRect);
            }
        }

        public virtual void OnTrackSelection()
        {
            trackEditor.SetSelectionTrack(this);
        }

        public virtual void OnElementSelection()
        {
            trackEditor.SetSelectionTrack(this);
        }

        public virtual float CalcElementWidth()
        {
            if (0 < childs.Count) {
                return childs.Max(element => element.CalcElementWidth());
            }
            return 200;
        }

        public virtual float CalcTrackHeight()
        {
            if (expand && 0 < childs.Count) {
                return childs.Sum(element => element.CalcTrackHeight());
            }
            return trackEditor.settings.trackHeight;
        }
    }
}
