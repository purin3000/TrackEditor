using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    using CurrentSerializeElement = TransformTrack.SerializeElement;

    public class TransformTrackEditor
    {
        public const string name = "Transform";

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
            public GameObject target { get => (parent.parent as GameObjectTrackEditor.Track)?.target; }

            public Vector3 localPosition;
            public Quaternion localRotation = Quaternion.identity;
            public Vector3 localScale = Vector3.one;

            public bool usePosition = true;
            public bool useRotation = true;
            public bool useScale = true;

            public Element()
            {
                isFixedLength = true;
            }

            public override void HeaderDrawer()
            {
                RemoveElementImpl($"Remove {name} Elememnt");
            }

            public override void PropertyDrawer(Rect rect)
            {
                base.PropertyDrawer(rect);

                using (new GUILayout.VerticalScope("box")) {
                    usePosition = EditorGUILayout.Toggle("Use Position", usePosition);
                    localPosition = EditorGUILayout.Vector3Field("Local Position", localPosition);
                }

                using (new GUILayout.VerticalScope("box")) {
                    useRotation = EditorGUILayout.Toggle("Use Rotation", useRotation);
                    localRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Local Rotation", localRotation.eulerAngles));
                }

                using (new GUILayout.VerticalScope("box")) {
                    useScale = EditorGUILayout.Toggle("Use Scale", useScale);
                    localScale = EditorGUILayout.Vector3Field("Local Scale", localScale);
                }

                GUILayout.Space(10);

                if (GUILayout.Button("オブジェクトから座標取得")) {
                    var go = target;
                    if (go) {
                        localPosition = go.transform.localPosition;
                        localRotation = go.transform.localRotation;
                        localScale = go.transform.localScale;
                    }
                }

                if (GUILayout.Button("オブジェクトへ設定")) {
                    var go = target;
                    if (go) {
                        go.transform.localPosition = localPosition;
                        go.transform.localRotation = localRotation;
                        go.transform.localScale = localScale;
                    }
                }
            }

            public override void WriteAsset(SerializeElementBase serializeElement)
            {
                var serialize = serializeElement as CurrentSerializeElement;
                serialize.localPosition = localPosition;
                serialize.localRotation = localRotation;
                serialize.localScale    = localScale;

                serialize.usePosition = usePosition;
                serialize.useRotation = useRotation;
                serialize.useScale    = useScale;

            }

            public override void ReadAsset(SerializeElementBase serializeElement)
            {
                var serialize = serializeElement as CurrentSerializeElement;
                localPosition = serialize.localPosition;
                localRotation = serialize.localRotation;
                localScale    = serialize.localScale;

                usePosition = serialize.usePosition;
                useRotation = serialize.useRotation;
                useScale    = serialize.useScale;
            }
        }
    }
}
