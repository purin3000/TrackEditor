using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    using CurrentSerializeElement = CameraChangeTrack.SerializeElement;

    public class CameraChangeTrackEditor
    {
        public const string name = "CamChange";

        public static TrackData CreateTrack() => new Track();

        public class Track : TrackData
        {
            public override void HeaderDrawer()
            {
                base.HeaderDrawer();

                HeaderDrawerImpl($"Remove {name} Track");
            }

            public override void TrackDrawer(Rect rect)
            {
                TrackDrawerImpl(rect, name);
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                PropertyDrawerImpl(rect, $"Add {name} Element");
            }

            public override TrackElement CreateElement() { return new Element(); }
        }

        public class Element : TrackElement
        {
            //public string startCameraName;
            //public string endCameraName;
            //public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;

            public override void HeaderDrawer()
            {
                RemoveElementImpl($"Remove {name} Elememnt");
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                //startCameraName = EditorGUILayout.TextField("Start CameraName", startCameraName);
                //endCameraName = EditorGUILayout.TextField("End CameraName", endCameraName);
                //easeType = (DG.Tweening.Ease)EditorGUILayout.EnumPopup("EaseType", easeType);
            }

            public override void WriteAsset(SerializeElementBase serializeElement)
            {
                //var serialize = serializeElement as CurrentSerializeElement;
                //serialize.startCameraName = startCameraName;
                //serialize.endCameraName = endCameraName;
                //serialize.easeType = easeType;
            }

            public override void ReadAsset(SerializeElementBase serializeElement)
            {
                //var serialize = serializeElement as CurrentSerializeElement;
                //startCameraName = serialize.startCameraName;
                //endCameraName = serialize.endCameraName;
                //easeType = serialize.easeType;
            }
        }

    }
}

