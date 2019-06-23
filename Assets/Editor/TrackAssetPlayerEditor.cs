using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace track_editor
{
    [CustomEditor(typeof(TrackAssetPlayer))]
    class TrackAssetPlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (EditorApplication.isPlaying) {
                if (GUILayout.Button("再生")) {
                    var ta = target as TrackAssetPlayer;
                    ta.Play();
                }
            }
        }
    }
}

