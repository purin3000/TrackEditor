using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor_fw
{
    [System.Serializable]
    public class TrackBase
#if UNITY_EDITOR
        : TrackGUIParam
#endif
    {
        public string name;

        public List<TrackBase> childs = new List<TrackBase>();

        public List<ElementBase> elements = new List<ElementBase>();

#if UNITY_EDITOR
        public override void Initialize(TrackManager manager, string name, TrackBase parent)
        {
            base.Initialize(manager, name, parent);

            this.name = name;
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

        public virtual void Write<T>(T asset) where T : TrackEditorAsset
        {

        }
#endif
    }

#if UNITY_EDITOR
    public class TrackGUIParam
    {
        public virtual void Initialize(TrackManager manager, string name, TrackBase parent)
        {
            this.manager = manager;
            this.parent = parent;

            nestLevel = 0;
            var current = parent;
            while (current != null) {
                ++nestLevel;
                current = current.parent;
            }
        }

        public TrackManager manager { get; private set; }

        public TrackBase parent { get; private set; }

        public ElementBase selectionElement { get; set; }

        public float trackHeight { get => manager.trackHeight; }

        public bool IsSelection { get => manager.selectionTrack == this; }

        public float pixelScale { get => manager.pixelScale; }

        public Vector2 scrollPos { get => manager.scrollPos; }

        public bool expand { get; set; } = true;

        public List<TrackBase> removeTracks { get; set; } = new List<TrackBase>();

        public List<ElementBase> removeElements { get; set; } = new List<ElementBase>();

        public int nestLevel { get; set; } = 0;
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
#endif
}
