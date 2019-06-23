#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    // Layout
    // +--------+----------------+----------+
    // |        | time           | property |
    // |        | h scroll       |          |
    // +--------+----------------+ v scroll |
    // | track  | element        |          |
    // | v scr  | vh scroll      |          |
    // |        |                |          |
    // |        |                |          |
    // +--------+----------------+----------+

    public class TrackEditorSettings
    {
        public float pixelScale = 5.0f;

        public int timeHeight = 20;

        public int propertyWidth = 300;

        public int trackWidth = 200;
        public int trackHeight = 32;

        public float childTrackSlide = 0.6f;

        public int gridScaleMax = 30;
    }
}


#endif
