using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace track_editor2
{
    using GameObjectEditorTrack = EditorTrack<GameObjectTrack.TrackData>;
    using ActivationEditorTrack = EditorTrack<ActivationTrack.TrackData>;
    using ActivationEditorElement = EditorElement<ActivationTrack.ElementData>;

    //using GameObjectAssetTrack = AssetTrack<GameObjectTrack.TrackData>;
    //using ActivationAssetTrack = AssetTrack<ActivationTrack.TrackData>;
    //using ActivationAssetElement = AssetElement<ActivationTrack.ElementData>;

    [System.Serializable]
    public class GameObjectAssetTrack : AssetTrack<GameObjectTrack.TrackData> { }

    [System.Serializable]
    public class ActivationAssetTrack : AssetTrack<ActivationTrack.TrackData> { }

    [System.Serializable]
    public class ActivationAssetElement : AssetElement<ActivationTrack.ElementData> { }

    public class NewTrackData
    {
        [MenuItem("Test/NewTrackData")]
        static void Open()
        {
            {
                List<IEditorTrack> editorTracks = new List<IEditorTrack>();
                List<IEditorElement> editorElements = new List<IEditorElement>();


                IEditorTrack gameObjectTrack = new GameObjectEditorTrack();
                editorTracks.Add(gameObjectTrack);

                IEditorTrack activationTrack = new ActivationEditorTrack();
                editorTracks.Add(activationTrack);

                IEditorElement activationElement = new ActivationEditorElement();
                editorElements.Add(activationElement);


                gameObjectTrack.AddTrack(activationTrack);

                activationTrack.AddElement(activationElement);


                TrackSerializer seri = new TrackSerializer();

                foreach (var editorTrack in editorTracks) {
                    seri.SerializeTrack(editorTrack);
                }

                foreach (var editorElement in editorElements) {
                    seri.SerializeElement(editorElement);
                }

                var str = JsonUtility.ToJson(seri, true);


                Debug.Log(str);

            }

            //var str = JsonUtility.ToJson(test, true);

            //Debug.Log(str);
            Debug.Log("test");

            //GetWindow<TrackEditorWindow>("TrackEditorExample");
        }
    }

    [System.Serializable]
    public class TrackSerializer
    {
        [SerializeField]
        List<GameObjectAssetTrack> GameObjectTracks = new List<GameObjectAssetTrack>();

        [SerializeField]
        List<ActivationAssetTrack> ActivationTracks = new List<ActivationAssetTrack>();

        [SerializeField]
        List<ActivationAssetElement> ActivationElements = new List<ActivationAssetElement>();

        public void SerializeTrack(IEditorTrack track)
        {
            if (track is GameObjectEditorTrack) { GameObjectTracks.Add(GameObjectEditorTrack.Serialize<GameObjectAssetTrack>(track)); return; }

            if (track is ActivationEditorTrack) { ActivationTracks.Add(ActivationEditorTrack.Serialize<ActivationAssetTrack>(track)); return; }
        }

        public void SerializeElement(IEditorElement element)
        {
            if (element is ActivationEditorElement) { ActivationElements.Add(ActivationEditorElement.Serialize<ActivationAssetElement>(element)); return; }
        }
    }

    public class TrackDeserializer
    {
        List<GameObjectEditorTrack> GameObjectTracks = new List<GameObjectEditorTrack>();
        List<ActivationEditorTrack> ActivationTracks = new List<ActivationEditorTrack>();
        List<ActivationEditorElement> ActivationElements = new List<ActivationEditorElement>();

        public void DeserizlizeTrack(IAssetTrack track)
        {
            if (track is GameObjectAssetTrack) { GameObjectTracks.Add(GameObjectEditorTrack.Deserialize(track)); return; }

            if (track is ActivationAssetTrack) { ActivationTracks.Add(ActivationEditorTrack.Deserialize(track)); return; }
        }

        public void DeserializeElement(IAssetElement element)
        {
            if (element is ActivationAssetElement) { ActivationElements.Add(ActivationEditorElement.Deserialize(element)); return; }
        }
    }


    public class GameObjectTrack
    {
        [System.Serializable]
        public class TrackData
        {
            public GameObject target;

            public bool activate;

            public bool currentPlayer;
        }
    }

    public class ActivationTrack
    {
        [System.Serializable]
        public class TrackData
        {
        }


        [System.Serializable]
        public class ElementData
        {
        }
    }

    public interface IAssetTrack
    {
        void SetName(string name);
    }

    public interface IAssetElement
    {
        void SetName(string name);
    }

    [System.Serializable]
    public class AssetTrack<T> : IAssetTrack where T : class
    {
        public string uniqueName;
        public string name;
        public string parent;
        public string[] childs;
        public string[] elements;

        //public T data;

        public void SetName(string name)
        {
            this.name = name;
        }
    }

    [System.Serializable]
    public class AssetElement<T> : IAssetElement where T : class
    {
        public string uniqueName;
        public string name;
        public string parent;
        public string[] childs;
        public string[] elements;

        //public T data;

        public void SetName(string name)
        {
            this.name = name;
        }
    }

    public interface IEditorTrack
    {
        string name { get; set; }
        IEditorTrack parent { get; set; }
        void AddTrack(IEditorTrack track);
        void AddElement(IEditorElement element);
        //IEditorTrack[] childs { get; set; }
        //IEditorElement[] elements { get; set; }
    }

    public interface IEditorElement
    {
        string name { get; set; }
        IEditorTrack parent { get; set; }
    }

    public class EditorTrack<T> : IEditorTrack where T : class, new()
    {
        public string name { get; set; }
        public IEditorTrack parent { get; set; }

        public void AddTrack(IEditorTrack track)
        {
            childs.Add(track);
            track.parent = this;
        }
        public void AddElement(IEditorElement element)
        {
            elements.Add(element);
            element.parent = this;
        }

        public List<IEditorTrack> childs { get; set; } = new List<IEditorTrack>();
        public List<IEditorElement> elements { get; set; } = new List<IEditorElement>();

        public T data;

        public static AssetTrackClass Serialize<AssetTrackClass>(IEditorTrack src) where AssetTrackClass:IAssetTrack, new()
        {
            EditorTrack<T> track = (EditorTrack<T>)src;
            AssetTrackClass ret = new AssetTrackClass();

            ret.SetName(track.name);
            //ret.parent = track.parent;
            //ret.childs = track.childs;
            //ret.elements = track.elements;
            //ret.data = track.data;

            return ret;
        }

        public static EditorTrack<T> Deserialize(IAssetTrack src)
        {
            AssetTrack<T> track = (AssetTrack<T>)src;
            EditorTrack<T> ret = new EditorTrack<T>();

            ret.name = track.name;
            //ret.parent = parent;
            //ret.childs = childs;
            //ret.elements = elements;
            //ret.data = track.data;

            return ret;
        }
    }

    public class EditorElement<T> : IEditorElement where T : class, new()
    {
        public string name { get; set; }
        public IEditorTrack parent { get; set; }

        public T data;

        public static AssetElementClass Serialize<AssetElementClass>(IEditorElement src) where AssetElementClass:IAssetElement, new()
        {
            EditorElement<T> element = (EditorElement<T>)src;
            AssetElementClass ret = new AssetElementClass();

            ret.SetName(element.name);
            //ret.parent = parent;
            //ret.data = element.data;

            return ret;
        }

        public static EditorElement<T> Deserialize(IAssetElement src)
        {
            AssetElement<T> element = (AssetElement<T>)src;
            EditorElement<T> ret = new EditorElement<T>();

            ret.name = element.name;
            //ret.parent = parent;
            //ret.childs = childs;
            //ret.elements = elements;
            //ret.data = element.data;

            return ret;
        }
    }
}
