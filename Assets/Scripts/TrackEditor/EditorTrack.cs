using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace track_editor
{
    /// <summary>
    /// TrackEditor実装用のトラック情報
    /// </summary>
    public class EditorTrack
    {
        public string name;

        public EditorTrack parent { get; set; }

        public List<EditorTrack> childs = new List<EditorTrack>();

        public List<EditorElement> elements = new List<EditorElement>();

        public EditorElement selectionElement { get; set; }

        public TrackEditor manager { get; private set; }

        public float trackHeight { get => manager.trackHeight; }

        public bool IsSelection { get => manager.selectionTrack == this; }

        public float pixelScale { get => manager.pixelScale; }

        public Vector2 scrollPos { get => manager.scrollPos; }

        public bool expand { get; set; } = true;

        public bool isFixedLength { get; set; }

        public List<EditorTrack> removeTracks { get; set; } = new List<EditorTrack>();

        public List<EditorElement> removeElements { get; set; } = new List<EditorElement>();

        public int nestLevel { get; set; } = 0;

        public virtual void Initialize(TrackEditor manager, string name, EditorTrack parent)
        {
            this.manager = manager;
            this.parent = parent;

            nestLevel = 0;
            var current = parent;
            while (current != null) {
                ++nestLevel;
                current = current.parent;
            }

            this.name = name;
        }

        public void LoadInitialize(TrackEditor manager, string name, EditorTrack parent)
        {
            this.manager = manager;
            this.parent = parent;

            nestLevel = 0;
            var current = parent;
            while (current != null) {
                ++nestLevel;
                current = current.parent;
            }

            this.name = name;
        }

        public virtual void HeaderDrawer()
        {
            GUILayout.Label(string.Format("Track:{0}", name));
        }

        public virtual void TrackDrawer(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + 2, rect.y + 2, rect.width - 4, rect.height - 4);
            GUI.Label(rectLabel, "DrawTrack:" + name, IsSelection ? "flow node 0 on" : "flow node 0");
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

        protected void DrawNameImpl()
        {
            name = EditorGUILayout.TextField("Name", name);
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
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            DrawNameImpl();
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
