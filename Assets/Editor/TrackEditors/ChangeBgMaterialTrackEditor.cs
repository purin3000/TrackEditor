using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor
{
    using CurrentSerializeElement = ChangeBgMaterialTrack.SerializeElement;

    public class ChangeBgMaterialTrackEditor
    {

        public const string name = "ChangeBgMat";

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
            public Material material;

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

                material = (Material)EditorGUILayout.ObjectField("Material", material, typeof(Material), true);
            }

            public override void WriteAsset(SerializeElementBase serializeElement)
            {
                var serialize = serializeElement as CurrentSerializeElement;
                serialize.material = material;
            }

            public override void ReadAsset(SerializeElementBase serializeElement)
            {
                var serialize = serializeElement as CurrentSerializeElement;
                material = serialize.material;
            }
        }
    }
}

