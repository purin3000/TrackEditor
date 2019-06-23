using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    /// <summary>
    /// TrackEditor実装用のエレメント情報
    /// </summary>
    public class TrackElement
    {
        public TrackData parent { get; private set; }

        public int start;
        public int length;
        public string name;

        public bool IsSelection { get => parent.selectionElement == this; }

        public float pixelScale { get => parent.pixelScale; }

        public float trackHeight { get => parent.trackHeight; }

        public Vector2 scrollPos { get => parent.scrollPos; }

        public Vector2 mouseOffset { get; set; }

        public bool isDrag { get; set; }

        public bool isLengthDrag { get; set; }

        public virtual void Initialize(TrackData parent)
        {
            this.parent = parent;
            this.start = parent.manager.currentFrame;
            this.length = 1;
        }

        public void LoadInitialize(string name, int start, int length, TrackData parent)
        {
            this.parent = parent;
            this.name = name;
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
            using (new EditorGUI.DisabledScope(parent.isFixedLength)) {
                length = Mathf.Max(0, EditorGUILayout.IntField("Length", length));

                if (parent.isFixedLength) {
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

        protected ElementSerializeClass WriteAssetImpl<ElementSerializeClass>(List<ElementSerializeClass> serializeList, WriteAssetContext context) where ElementSerializeClass : SerializeElement, new()
        {
            var elementSerialize = new ElementSerializeClass();

            SerializeUtility.InitializeElementSerialize(elementSerialize, this, context);

            serializeList.Add(elementSerialize);

            return elementSerialize;
        }

    }
}
