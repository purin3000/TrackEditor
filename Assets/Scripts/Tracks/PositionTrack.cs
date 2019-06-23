using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{

    [System.Serializable]
    public class PositionSerializeTrack : SerializeTrack
    {
    }

    [System.Serializable]
    public class PositionSerializeElement : SerializeElement
    {
        public Vector3 localPosition;

        public override IElementPlayer CreatePlayer()
        {
            return new PositionElementPlayer(this);
        }
    }

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

        public void WriteAsset(WriteAssetContext context)
        {
            WriteAssetImpl(context.asset.positionTracks, context);
        }

        public void ReadAsset(PositionSerializeTrack serializeTrack)
        {
        }
    }

    public class PositionElement : TrackElemet
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

        public void WriteAsset(WriteAssetContext context)
        {
            var elementSerialize = WriteAssetImpl(context.asset.positionElements, context);
            elementSerialize.localPosition = localPosition;
        }

        public void ReadAsset(PositionSerializeElement serializeElement)
        {
            localPosition = serializeElement.localPosition;
        }
    }

    public class PositionElementPlayer : IElementPlayer
    {
        PositionSerializeElement elementSerialize;

        public PositionElementPlayer(PositionSerializeElement trackSerialize) { this.elementSerialize = trackSerialize; }

        public override int start { get => elementSerialize.start; }
        public override int end { get => elementSerialize.end; }

        public override void OnStart(TrackAssetPlayer context)
        {
            var gameObjectTrack = context.GetParentTrack<GameObjectSerializeTrack>(elementSerialize);

            Debug.LogFormat("Position start:{0}", start);

            gameObjectTrack.target.transform.localPosition = elementSerialize.localPosition;
        }

        //public override void OnEnd(ElementPlayerContext context)
        //{
        //    var gameObjectTrack = context.GetParentTrack<GameObjectTrackSerialize>(elementSerialize);

        //    Debug.LogFormat("Position end:{0}", end);
        //}
    }

}

