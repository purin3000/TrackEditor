﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    public class PositionTrackData : TrackData
    {
        public PositionTrackData()
        {
            isFixedLength = true;
        }

        public override void HeaderDrawer()
        {
            base.HeaderDrawer();

            RemoveTrackImpl("Remove Position Track");

            RemoveElementImpl();
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            GUILayout.Space(15);

            DrawIndexMoveImpl();

            GUILayout.Space(15);

            AddElementImpl<PositionElement>("Add Position Element");
        }

        public override void TrackDrawer(Rect rect)
        {
            TrackDrawerImpl(rect, "Position");
        }
    }

    public class PositionElement : TrackElement
    {
        public GameObject target { get => (parent.parent as GameObjectTrackData)?.target; }

        public Vector3 localPosition;

        public override void HeaderDrawer()
        {
            RemoveElementImpl("Remove Position Elememnt");
        }

        public override void PropertyDrawer(Rect rect)
        {
            base.PropertyDrawer(rect);

            localPosition = EditorGUILayout.Vector3Field("Local Position", localPosition);

            GUILayout.Space(10);

            if (GUILayout.Button("オブジェクトから座標取得")) {
                var go = target;
                if (go) {
                    localPosition = go.transform.localPosition;
                }
            }

            if (GUILayout.Button("オブジェクトへ設定")) {
                var go = target;
                if (go) {
                    go.transform.localPosition = localPosition;
                }
            }
        }

        public override void WriteAsset(SerializeElement serializeElement)
        {
            var serialize = serializeElement as PositionSerializeElement;
            serialize.localPosition = localPosition;
        }

        public override void ReadAsset(SerializeElement serializeElement)
        {
            var serialize = serializeElement as PositionSerializeElement;
            localPosition = serialize.localPosition;
        }
    }
}
