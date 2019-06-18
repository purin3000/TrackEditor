using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor_fw
{
    /// <summary>
    /// ヘッダ領域描画
    /// </summary>
    public interface ITrackEditorHeader
    {
        void DrawHeader(TrackEditor manager, Rect rect);
    }

    //public class TrackEditorHeaderExample : ITrackEditorHeader
    //{
    //    public virtual void DrawHeader(TrackEditor manager, Rect rect)
    //    {
    //        using (new GUILayout.HorizontalScope()) {
    //            manager.currentFrame = Mathf.Max(0, EditorGUILayout.IntField("Frame", manager.currentFrame));
    //            manager.frameLength = Mathf.Max(0, EditorGUILayout.IntField("Length", manager.frameLength));
    //            manager.gridScale = EditorGUILayout.IntSlider("Scale", (int)manager.gridScale, 1, manager.gridScaleMax);
    //        }
    //    }
    //}
}
