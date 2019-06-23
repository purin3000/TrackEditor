using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor;

namespace track_editor
{
    public class TrackData : EditorTrack
    {
        protected SerializeTrackClass WriteAssetImpl<SerializeTrackClass>(List<SerializeTrackClass> serializeList, WriteAssetContext context) where SerializeTrackClass : SerializeTrack, new ()
        {
            // 対応するシリアライズ用のクラスを作って
            var serializeTrack = new SerializeTrackClass();

            SerializeUtility.InitializeTrackSerialize(serializeTrack, this, context);

            // リストへ追加
            serializeList.Add(serializeTrack);

            return serializeTrack;
        }

        protected void RemoveTrackImpl(string label)
        {
            if (selectionElement == null) {
                if (GUILayout.Button(label)) {
                    manager.RemoveTrack(parent, this);
                }
                //using (new GUILayout.VerticalScope()) {
                //}
            }

        }

        protected void RemoveElementImpl()
        {
            selectionElement?.HeaderDrawer();
        }

        protected void AddElementImpl<T>(string label) where T : EditorElement,new()
        {
            using (new GUILayout.VerticalScope()) {
                if (GUILayout.Button(label)) {
                    var element = new T();
                    element.name = string.Format("{0}:{1}", name, elements.Count);
                    manager.AddElement(this, element);
                }
            }
        }

        protected void TrackDrawerImpl(Rect rect, string label)
        {
            Rect rectLabel = new Rect(rect.x + 3, rect.y + 3, rect.width - 6, rect.height - 6);
            GUI.Label(rectLabel, label, IsSelection ? "flow node 3 on" : "flow node 2");
        }
    }
}
