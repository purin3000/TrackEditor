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
    public partial class TrackAsset : MonoBehaviour
    {
        public int frameLength = 100;
        public float playSpeed = 1.0f;
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

