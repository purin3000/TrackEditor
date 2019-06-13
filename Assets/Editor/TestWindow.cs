using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor_fw;

public class TestWindow : EditorWindow
{
    [MenuItem("Window/TestWindow")]
    static void open()
    {
        GetWindow<TestWindow>("トラックエディターテスト");
    }

    TrackEditorFramework _trackEditor = new TrackEditorFramework(new TrackEditorSettings());

    class TestTrackData : ITrackData
    {
        string name;
        public TestTrackData(string str)
        {
            name = str;
        }

        public void DrawElement(Rect rect)
        {
            GUI.Label(rect, name, "box");
        }

        public void DrawTrack(Rect rect)
        {
            GUI.Label(rect, name, "box");
        }

        public float CalcElementWidth()
        {
            return 400;
        }

        public float CalcElementHeight()
        {
            return 50;
        }
    }

    private void OnEnable()
    {
        List<ITrackData> list = new List<ITrackData>();
        for (int i= 0; i<5;++i) {
            list.Add(new TestTrackData("test" + i));
        }

        _trackEditor.tracks = list;  
    }

    private void OnGUI()
    {
        _trackEditor.OnGUI(new Rect(0, 0, Screen.width, Screen.height));
    }  
}
