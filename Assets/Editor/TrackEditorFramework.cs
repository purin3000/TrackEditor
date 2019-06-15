using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace track_editor_fw
{
    // Layout
    // +-------------------------+----------+
    // | header                  | property |
    // +--------+----------------+ v scroll |
    // |        | time           |          |
    // |        | h scroll       |          |
    // +--------+----------------+          |
    // | track  | element        |          |
    // | v scr  | vh scroll      |          |
    // |        |                |          |
    // |        |                |          |
    // +--------+----------------+----------+

    public class TrackEditorSettings
    {
        public float pixelScale = 5.0f;

        public int headerHeight = 40;
        public int timeHeight = 20;

        public int propertyWidth = 300;

        public int trackWidth = 200;
        public int trackHeight = 50;

        public float childTrackSlide = 0.5f;
    }

    public class TrackEditor
    {
        public int frameLength = 100;

        public int currentFrame = 0;

        public TrackEditorSettings settings { get; private set; }

        public TrackBase top { get; private set; }

        public HeaderBase header { get; private set; }

        public TrackBase selectionTrack { get; private set; }

        public Vector2 scrollPos { get => scrPos; }

        public bool repaintRequest { get; private set; }


        Vector2 scrPos;



        public TrackEditor(TrackEditorSettings settings)
        {
            this.settings = settings;
            top = new TrackBase();
            top.Initialize(this, "top", null);
        }

        public void SetHeader(HeaderBase header)
        {
            this.header = header;
        }

        public void SetSelectionTrack(TrackBase track)
        {
            selectionTrack = track;
            Repaint();
        }

        public void Repaint()
        {
            repaintRequest |= true;
        }

        public void OnGUI(Rect rect)
        {
            repaintRequest = false;

            var headerWidth = rect.width - settings.propertyWidth;
            var timeY = rect.y + settings.headerHeight;
            var propertyX = rect.x + rect.width - settings.propertyWidth;
            var propertyHeight = rect.height;
            var elementX = rect.x + settings.trackWidth;
            var elementY = rect.y + settings.headerHeight + settings.timeHeight;
            var elementWidth = rect.width - settings.trackWidth - settings.propertyWidth;
            var elementHeight = rect.height - settings.headerHeight - settings.timeHeight;

            Rect rectHeader = new Rect(rect.x, rect.y, headerWidth, settings.headerHeight);
            Rect rectTime = new Rect(elementX, timeY, elementWidth, settings.timeHeight);
            Rect rectProperty = new Rect(propertyX, rect.y, settings.propertyWidth, propertyHeight);
            Rect rectTrack = new Rect(rect.x, elementY, settings.trackWidth, elementHeight);
            Rect rectElement = new Rect(elementX, elementY, elementWidth, elementHeight);

            using (new GUILayout.AreaScope(rectHeader)) {
                drawHeader(rectHeader);
            }

            // time + track + element
            {
                Rect rectTimeAndElement = new Rect(rectTime.x, rectTime.y, rectElement.width - 16, rectTime.height + rectTrack.height - 16);
                Rect rectTrackAndElement = new Rect(rectTrack.x, rectTrack.y, rectTrack.width + rectElement.width - 16, rectTrack.height - 16);

                using (new GUILayout.AreaScope(rectTime)) {
                    drawTime(rectTime);
                }

                using (new GUILayout.AreaScope(rectTrack)) {
                    drawTrack(rectTrack);
                }

                if (0 < rectElement.width && 0 < rectElement.height) {
                    using (new GUILayout.AreaScope(rectElement)) {
                        drawElement(rectElement);
                    }

                    drawFrameLine(rectTimeAndElement);
                }


                if (0 < rectElement.width) {
                    drawAreaLine(rectTimeAndElement);
                    drawAreaLine(rectTrackAndElement);
                }
            }

            using (new GUILayout.AreaScope(rectProperty)) {
                drawProperty(rectProperty);
            }
        }

      void drawAreaLine(Rect rect)
        {
            Handles.color = Color.gray;

            var points = new Vector3[] {
                new Vector3(rect.x, rect.y, 0),
                new Vector3(rect.x + rect.width, rect.y, 0),
                new Vector3(rect.x + rect.width, rect.y + rect.height, 0),
                new Vector3(rect.x, rect.y + rect.height, 0),
            };

            var indexies = new int[] {
                0, 1, 1, 2, 2, 3, 3, 0,
            };

            Handles.DrawLines(points, indexies);
        }

        void drawFrameLine(Rect rect)
        {
            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) {
                var mousePos = Event.current.mousePosition;

                if (rect.Contains(mousePos)) {
                    var relativePos = mousePos.x - rect.x + scrPos.x;
                    int frame = (int)(relativePos / settings.pixelScale);

                    currentFrame = frame;
                    GUI.FocusControl("");   
                    Repaint();

                    Event.current.Use();
                }
            }


            {
                var x = rect.x + currentFrame * settings.pixelScale - scrPos.x;
                if (rect.Contains(new Vector2(x, rect.y))) {
                    Handles.color = Color.white;
                    Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.y + rect.height, 0));
                }
            }


            {
                var x = rect.x + frameLength * settings.pixelScale - scrPos.x;
                if (rect.Contains(new Vector2(x, rect.y))) {
                    Handles.color = Color.red;
                    Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.y + rect.height, 0));
                }
            }
        }

        void drawHeader(Rect rect)
        {
            header?.DrawHeader(rect);
        }

        void drawProperty(Rect rect)
        {
            if (selectionTrack != null) {
                Rect rectProperty = new Rect(0, 0, rect.width, rect.height);

                using (new GUILayout.AreaScope(rectProperty, "", "box")) {
                    using (new GUILayout.VerticalScope()) {

                        selectionTrack.DrawProperty(rect);
                    }
                }
            }
        }

        void drawTime(Rect rect)
        {
            using (new GUI.ClipScope(new Rect(0, 0, rect.width - 16, rect.height))) {   // スクロールバー端の描画の都合で範囲調整
                float x = settings.pixelScale * 10;
                int frame = 10;
                while (x < frameLength * 10 + 100) {
                    GUI.Label(new Rect(x - scrPos.x, 0, 40, 40), frame.ToString());

                    x += settings.pixelScale * 10;
                    frame += 10;
                }
            }
        }

        void drawTrack(Rect rect)
        {
            using (new GUI.ClipScope(new Rect(0, 0, rect.width, rect.height - 16))) {   // スクロールバー端の描画の都合で範囲調整
                Rect trackRect = new Rect(0, -scrPos.y, settings.trackWidth, top.CalcTrackHeight());
                top.DrawTrack(trackRect);
            }
        }

        void drawElement(Rect rect)
        {
            using (new GUI.ClipScope(new Rect(0, 0, rect.width - 16, rect.height - 16))) {   // スクロールバー端の描画の都合で範囲調整
                Rect trackRect = new Rect(-scrPos.x, -scrPos.y, 0, settings.trackHeight);
                top.DrawElement(trackRect);
            }

            using (var scope = new EditorGUILayout.ScrollViewScope(scrPos)) {
                float scrollWidth = top.CalcElementWidth();
                float scrollHeight = top.CalcTrackHeight();

                GUILayout.Label("", GUILayout.Width(scrollWidth - 24), GUILayout.Height(scrollHeight - 20));

                scrPos = scope.scrollPosition;
            }
        }
    }
}

