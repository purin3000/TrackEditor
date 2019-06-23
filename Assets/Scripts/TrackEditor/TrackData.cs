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
    public class TrackData
    {
        public string name;

        public TrackData parent { get; set; }

        public List<TrackData> childs = new List<TrackData>();

        public List<TrackElement> elements = new List<TrackElement>();

        public TrackElement selectionElement { get; set; }

        public TrackEditor manager { get; private set; }

        public float trackHeight { get => manager.trackHeight; }

        public bool IsSelection { get => manager.selectionTrack == this; }

        public float pixelScale { get => manager.pixelScale; }

        public Vector2 scrollPos { get => manager.scrollPos; }

        public bool expand { get; set; } = true;

        public bool isFixedLength { get; set; }

        public List<TrackData> removeTracks { get; set; } = new List<TrackData>();

        public List<TrackElement> removeElements { get; set; } = new List<TrackElement>();

        public int nestLevel { get; set; } = 0;

        public virtual void Initialize(TrackEditor manager, string name, TrackData parent)
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

        public void LoadInitialize(TrackEditor manager, string name, TrackData parent)
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


        public virtual void WriteAsset(SerializeTrack serializeTrack)
        {
        }

        public virtual void ReadAsset(SerializeTrack serializeTrack)
        {
        }



        protected void RemoveTrackImpl(string label)
        {
            if (selectionElement == null) {
                if (GUILayout.Button(label)) {
                    manager.RemoveTrack(parent, this);
                }
                //using (new GUILayout.VerticalScope()) {
                //}
            }

        }

        protected void RemoveElementImpl()
        {
            selectionElement?.HeaderDrawer();
        }

        protected void AddElementImpl<T>(string label) where T : TrackElement, new()
        {
            using (new GUILayout.VerticalScope()) {
                if (GUILayout.Button(label)) {
                    var element = new T();
                    element.name = string.Format("{0}:{1}", name, elements.Count);
                    manager.AddElement(this, element);
                }
            }
        }

        protected void TrackDrawerImpl(Rect rect, string label)
        {
            Rect rectLabel = new Rect(rect.x + 3, rect.y + 3, rect.width - 6, rect.height - 6);
            GUI.Label(rectLabel, label, IsSelection ? "flow node 3 on" : "flow node 2");
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
