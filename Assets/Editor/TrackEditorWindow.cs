using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

namespace track_editor_example
{
    public class TrackEditorWindow : EditorWindow
    {
        [MenuItem("Test/TrackEditorWindow")]
        static void Open()
        {
            GetWindow<TrackEditorWindow>("TrackEditorExample");
        }

        TrackEditor _manager = new TrackEditor(new TrackEditorSettings(), new RootTrackData());

        private void OnEnable()
        {
            for (int i = 0; i < 3; ++i) {
                _manager.AddTrack(_manager.top, "Track:" + i, new GameObjectTrackData());
            }

            _manager.SetHeader(new TestHeader(_manager));
            _manager.SetSelectionTrack(_manager.top.childs[2]);
        }

        private void OnGUI()
        {
            _manager.OnGUI(new Rect(10, 10, Screen.width - 20, Screen.height - 20 - 24));

            if (_manager.repaintRequest) {
                Repaint();
            }
        }
    }
}
