using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor2
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

        public int timeHeight = 24;

        public int propertyWidth = 300;

        public int trackWidth = 200;
        public int trackHeight = 32;

        public int gridScaleMax = 30;
    }
}
