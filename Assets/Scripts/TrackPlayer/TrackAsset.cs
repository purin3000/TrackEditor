using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor
{
    /// <summary>
    /// アセットとして保存されるオブジェクト
    /// 
    /// このオブジェクトをプレハブやScriptableObjectにしてランタイムで使用
    /// </summary>
    [System.Serializable]
    public class TrackAsset : MonoBehaviour
    {
        public int frameLength = 100;

        [HideInInspector]
        public List<RootSerializeTrack> rootTracks = new List<RootSerializeTrack>();
        [HideInInspector]
        public List<GameObjectSerializeTrack> gameObjectTracks = new List<GameObjectSerializeTrack>();
        [HideInInspector]
        public List<ActivationSerializeTrack> activationTracks = new List<ActivationSerializeTrack>();
        [HideInInspector]
        public List<PositionSerializeTrack> positionTracks = new List<PositionSerializeTrack>();
        [HideInInspector]
        public List<AnimationSerializeTrack> animationTracks = new List<AnimationSerializeTrack>();

        [HideInInspector]
        public List<ActivationSerializeElement> activationElements = new List<ActivationSerializeElement>();
        [HideInInspector]
        public List<PositionSerializeElement> positionElements = new List<PositionSerializeElement>();
        [HideInInspector]
        public List<AnimationSerializeElement> animationElements = new List<AnimationSerializeElement>();

        public void WriteAsset(int frameLength)
        {
            this.frameLength = frameLength;

            rootTracks.Clear();
            gameObjectTracks.Clear();
            activationTracks.Clear();
            positionTracks.Clear();
            animationTracks.Clear();

            activationElements.Clear();
            positionElements.Clear();
            animationElements.Clear();
        }
    }

    /// <summary>
    /// トラックの階層構造
    /// 
    /// シリアライズの制約でトラックを直接参照させることが出来ないため、
    /// トラックは一意な名前で保持し、あとで関連付けます
    /// </summary>
    [System.Serializable]
    public abstract class SerializeTrack
    {
        public string uniqueName;

        public string name;
        public string parent;
        public string[] childs;
        public string[] elements;
    }

    /// <summary>
    /// エレメントの基本情報
    /// 
    /// シリアライズの制約でトラックを直接参照させることが出来ないため、
    /// トラックは一意な名前で保持し、あとで関連付けます
    /// </summary>
    [System.Serializable]
    public abstract class SerializeElement
    {
        public string uniqueName;

        public string name;
        public string parent;
        public int start;
        public int length;

        public int end { get => start + length; }

        public abstract IElementPlayer CreatePlayer();
    }

}

