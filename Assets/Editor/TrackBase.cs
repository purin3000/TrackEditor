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

        public float trackHeight { get => trackEditor.settings.trackHeight; }

        public bool expand { get; set; } = true;

        public bool IsSelection { get => trackEditor.selectionTrack == this; }

        List<TrackBase> _removeTracks = new List<TrackBase>();
        List<ElementBase> _removeElements = new List<ElementBase>();

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
            _removeTracks.Add(track);

            if (trackEditor.selectionTrack == track) {
                trackEditor.SetSelectionTrack(null);

                if (2 <= childs.Count) {
                    int index = childs.IndexOf(track) + 1;
                    if (childs.Count <= index) {
                        index -= 2;
                    }

                    if (0 <= index && index < childs.Count) {
                        trackEditor.SetSelectionTrack(childs[index]);
                    }
                }
            }
        }

        public void RemoveElement(ElementBase element)
        {
            _removeElements.Add(element);

            if (selectionElement == element) {
                selectionElement = null;

                if (2 <= elements.Count) {
                    int index = elements.IndexOf(element) + 1;
                    if (childs.Count <= index) {
                        index -= 2;
                    }

                    if (0 <= index && index < elements.Count) {
                        selectionElement = elements[index];
                    }
                }
            }
        }

        public T AddElement<T>(T element) where T : ElementBase
        {
            elements.Add(element);
            element.Initialize(this);
            SetSelectionElement(element);
            return element;
        }

        public void SetSelectionElement(ElementBase element)
        {
            selectionElement = element;

            // 選択した要素を必ず先頭に置く
            var index = elements.IndexOf(element);
            if (0 < index) {
                elements.SwapAt(0, 1);
            }
        }

        public void Repaint()
        {
            trackEditor.Repaint();
        }

        public void DrawTrack(Rect rect) {

            if (0 < _nestLevel) {
                TrackDrawer(rect);
            }

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
            foreach (var track in _removeTracks) {
                int index = childs.IndexOf(track);
                if (index != -1) {
                    childs.RemoveAt(index);
                }
            }
            _removeTracks.Clear();

            foreach (var element in _removeElements) {
                int index = elements.IndexOf(element);
                if (index != -1) {
                    elements.RemoveAt(index);
                }
            }
            _removeElements.Clear();
        }

        public void DrawElement(Rect rect) {

            // マウスイベントの取得順序と描画順序は逆
            Rect rectElement = new Rect(rect.x, rect.y, rect.width, trackHeight);
            for (int i = 0; i < elements.Count; ++i) {
                elements[i].UpdateElement(rectElement);
            }

            for (int i = elements.Count-1; 0 <= i; --i) {
                elements[i].DrawElement(rectElement);
            }

            if (expand) {
                float y = rect.y;
                foreach (var child in childs) {
                    Rect rectChild = new Rect(rect.x, y, rect.width, child.CalcTrackHeight());
                    child.DrawElement(rectChild);
                    y += rectChild.height;
                }
            }
        }

        public void DrawProperty(Rect rect)
        {
            using (new GUILayout.VerticalScope("box")) {
                PropertyDrawer(rect);

            }

            if (selectionElement != null) {
                using (new GUILayout.VerticalScope("box")) {
                    selectionElement.PropertyDrawer(rect);

                }
            }
        }

        public virtual void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
            GUI.Label(rectLabel, "DrawTrack:" + name, IsSelection ? "flow node 0 on" : "flow node 0");
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            //GUI.Label(new Rect(Vector2.zero, rect.size), "DrawProperty:" + name, "box");
            GUILayout.Label("Type:" + name);
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
                return childs.Max(child => child.CalcElementWidth());
            }

            if (0 < elements.Count) {
                return elements.Max(element => (element.start + element.length) * trackEditor.pixelScale);
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

    static class ContainerSwap
    {
        public static void SwapAt<T>(this List<T> list, int indexA, int indexB)
        {
            var tmp = list[indexB];
            list[indexB] = list[indexA];
            list[indexA] = tmp;
        }
    }
}
