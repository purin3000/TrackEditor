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
    // +--------+----------------+          |
    // | track  | scroll element |          |
    // |        |                |          |
    // |        |                |          |
    // +--------+----------------+----------+

    public class TrackEditorSettings
    {
        public int headerHeight = 40;

        public int propertyWidth = 300;

        public int trackWidth = 200;
        public int trackHeight = 50;
    }

    public interface ITrackData
    {
        void DrawTrack(Rect rect);

        void DrawElement(Rect rect);

        float CalcElementWidth();

        float CalcElementHeight();
    }

    public class TrackEditorFramework
    {
        public TrackEditorSettings settings { get; private set; }

        public List<ITrackData> tracks = new List<ITrackData>();

        Vector2 scrPos;


        public TrackEditorFramework(TrackEditorSettings settings)
        {
            this.settings = settings;
        }

        public void OnGUI(Rect rect)
        {
            var height = rect.height - 22;

            Rect rectHeader = new Rect(rect.x, rect.y, rect.width - settings.propertyWidth, settings.headerHeight);
            Rect rectProperty = new Rect(rect.x + rect.width - settings.propertyWidth, rect.y, settings.propertyWidth, height);
            Rect rectTrack = new Rect(rect.x, rect.y + settings.headerHeight, settings.trackWidth, height - settings.headerHeight);
            Rect rectElement = new Rect(rect.x + settings.trackWidth, rect.y + settings.headerHeight, rect.width - settings.trackWidth - settings.propertyWidth, height - settings.headerHeight);

            using (new GUILayout.AreaScope(rectHeader, "header", "box")) {
                drawHeader(rectHeader);
            }

            using (new GUILayout.AreaScope(rectProperty, "property", "box")) {
                drawProperty(rectProperty);
            }

            using (new GUILayout.AreaScope(rectTrack, "track", "box")) {
                drawTrack(rectTrack);
            }

            using (new GUILayout.AreaScope(rectElement, "element")) {
                drawElement(rectElement);
            }
        }

        void drawHeader(Rect rect)
        {
        }

        void drawProperty(Rect rect)
        {
        }

        float calcElementWidth(Rect rect)
        {
            float width = tracks.Max(element => element.CalcElementWidth());
            return Mathf.Max(width, rect.width - 24);
        }

        float calcElementHeight(Rect rect)
        {
            float height = tracks.Sum(element => element.CalcElementHeight());
            return Mathf.Max(height, rect.height);
        }

        void drawTrack(Rect rect)
        {
            float elementWidth = settings.trackWidth;
            float elementHeight = calcElementHeight(rect);

            using (new GUI.ClipScope(new Rect(0, 0, rect.width, rect.height))) {
                int i = 0;
                foreach (var track in tracks.ToArray()) {
                    Rect trackRect = new Rect(0, settings.trackHeight * i++ - scrPos.y, elementWidth, elementHeight);
                    track.DrawTrack(trackRect);
                }
            }
        }

        void drawElement(Rect rect)
        {
            float elementWidth = calcElementWidth(rect);
            float elementHeight = calcElementHeight(rect);

            using (new GUI.ClipScope(new Rect(0, 0, rect.width - 16, rect.height - 16))) {
                int i = 0;
                foreach (var track in tracks.ToArray()) {
                    Rect elementRect = new Rect(-scrPos.x, settings.trackHeight * i++ - scrPos.y, track.CalcElementWidth(), elementHeight);
                    track.DrawElement(elementRect);
                }
            }

            using (var scope = new EditorGUILayout.ScrollViewScope(scrPos)) {
                GUILayout.Label("", GUILayout.Width(elementWidth), GUILayout.Height(elementHeight));

                scrPos = scope.scrollPosition;
            }
        }
    }
}

