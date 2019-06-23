using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using track_editor;

namespace track_editor
{
    public class TrackElemet : EditorElement
    {
        protected ElementSerializeClass WriteAssetImpl<ElementSerializeClass>(List<ElementSerializeClass> serializeList, WriteAssetContext context) where ElementSerializeClass : SerializeElement, new()
        {
            var elementSerialize = new ElementSerializeClass();

            SerializeUtility.InitializeElementSerialize(elementSerialize, this, context);

            serializeList.Add(elementSerialize);

            return elementSerialize;
        }
    }
}
