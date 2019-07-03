using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using ParentEditorTrack = GameObjectEditorTrack;
    using CurrentElementData = ActivationTrack.ElementData;
    
    public class ActivationEditorElementData : ParentEditorTrack.ChildEditorElementBase
    {
        public const string labelName = "Activate";

        public CurrentElementData elementData = new CurrentElementData();

        public override void ElementHeaderDrawer()
        {
            ElementHeaderDrawerImpl(labelName);
        }

        public override void PropertyDrawer(Rect rect)
        {
            PropertyDrawerImpl(rect, labelName);
        }

        public override void ElementDrawer(Rect rect)
        {
            ElementDrawerImpl(rect);
        }
    }
}
