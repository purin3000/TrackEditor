using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor_fw
{
    public class ElementBase
    {
        public int start;
        public int length;

        public TrackBase parent { get; private set; }

        public float pixelScale { get => parent.trackEditor.pixelScale; }

        public float trackHeight { get => parent.trackEditor.settings.trackHeight; }

        public Vector2 scrollPos { get => parent.trackEditor.scrollPos; }



        public bool IsSelection { get => parent.selectionElement == this; }


        Vector2 mouseOffset;
        bool isDrag;
        bool isLengthDrag;

        public virtual void Initialize(TrackBase parent)
        {
            this.parent = parent;
            this.start = parent.trackEditor.currentFrame;
            this.length = 1;
        }

        public void Selection()
        {
            parent.Selection();
            parent.SetSelectionElement(this);
            parent.Repaint();
        }

        /// <summary>
        /// ドラッグ関連
        /// 描画優先度とイベント優先度が逆のため、DrawElementとUpdateElemenetに関数を分離して回す必要がある
        /// </summary>
        /// <param name="rect"></param>
        public void UpdateElement(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + pixelScale * start - scrollPos.x, rect.y - scrollPos.y, pixelScale * length, trackHeight);
            Rect rectLength = new Rect(rectLabel.x + rectLabel.width, rect.y - scrollPos.y, pixelScale * 1, trackHeight);

            if (Event.current.type == EventType.MouseDown) {
                if (rectLabel.Contains(Event.current.mousePosition)) {
                    Selection();

                    isDrag = true;
                    mouseOffset = rectLabel.position - Event.current.mousePosition;

                    Event.current.Use();

                } else if (rectLength.Contains(Event.current.mousePosition)) {
                    //DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                    //int id = GUIUtility.GetControlID(FocusType.Passive);
                    //DragAndDrop.activeControlID = id;

                    isLengthDrag = true;
                    mouseOffset = rectLength.position - Event.current.mousePosition;

                    Event.current.Use();
                }

            } else if (Event.current.type == EventType.MouseUp) {
                isDrag = false;
                isLengthDrag = false;

                //DragAndDrop.visualMode = DragAndDropVisualMode.None;
                //DragAndDrop.activeControlID = 0;

            } else if (Event.current.type == EventType.MouseDrag) {
                if (isDrag) {
                    var currentFrame = (int)((Event.current.mousePosition.x - rect.x + scrollPos.x + mouseOffset.x) / pixelScale);

                    start = currentFrame;
                    parent.Repaint();

                    Event.current.Use();

                } else if (isLengthDrag) {
                    var currentFrame = (int)((Event.current.mousePosition.x - rect.x + scrollPos.x + mouseOffset.x) / pixelScale);

                    length = Mathf.Max(1, currentFrame - start);
                    parent.Repaint();

                    Event.current.Use();
                }
            }
        }

        public void DrawElement(Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + pixelScale * start - scrollPos.x, rect.y - scrollPos.y, pixelScale * length, trackHeight);

            ElementDrawer(rectLabel);
        }

        public virtual void ElementDrawer(Rect rect)
        {
            Rect labelRect = new Rect(rect.x + pixelScale * start - scrollPos.x, rect.y - scrollPos.y, pixelScale * length, trackHeight);
            GUI.Label(rect, "", IsSelection ? "flow node 5 on" : "flow node 5");
        }

        public virtual void PropertyDrawer(Rect rect)
        {
            GUILayout.Label("Element");
            start = Mathf.Max(0, EditorGUILayout.IntField("Start", start));
            length = Mathf.Max(0, EditorGUILayout.IntField("Length", length));
        }
    }
}
