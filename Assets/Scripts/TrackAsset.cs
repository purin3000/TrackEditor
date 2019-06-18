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
    public class TrackAsset : MonoBehaviour
    {
        public int frameLength = 100;

        public List<RootTrackSerialize> rootTracks = new List<RootTrackSerialize>();
        public List<GameObjectTrackSerialize> gameObjectTracks = new List<GameObjectTrackSerialize>();
        public List<ActivationTrackSerialize> activationTracks = new List<ActivationTrackSerialize>();
        public List<PositionTrackSerialize> positionTracks = new List<PositionTrackSerialize>();

        public List<ActivationElementSerialize> activationElements = new List<ActivationElementSerialize>();
        public List<PositionElementSerialize> positionElements = new List<PositionElementSerialize>();

        public void WriteAsset(int frameLength)
        {
            this.frameLength = frameLength;

            rootTracks.Clear();
            gameObjectTracks.Clear();
            activationTracks.Clear();
            positionTracks.Clear();

            activationElements.Clear();
            positionElements.Clear();
        }
    }

    /// <summary>
    /// トラックのシリアライズデータ
    /// </summary>
    [System.Serializable]
    public class RootTrackSerialize : TrackSerialize
    {
    }

    [System.Serializable]
    public class GameObjectTrackSerialize : TrackSerialize
    {
        public GameObject go;
    }

    [System.Serializable]
    public class ActivationTrackSerialize : TrackSerialize
    {
    }

    [System.Serializable]
    public class PositionTrackSerialize : TrackSerialize
    {
    }

    /// <summary>
    /// エレメントのシリアライズデータ
    /// </summary>
    [System.Serializable]
    public class ActivationElementSerialize : ElementSerialize
    {
    }

    [System.Serializable]
    public class PositionElementSerialize : ElementSerialize
    {
        public Vector3 position;
    }

    /// <summary>
    /// トラックの階層構造
    /// 
    /// シリアライズの制約でトラックを直接参照させることが出来ないため、
    /// トラックは一意な名前で保持し、あとで関連付けます
    /// </summary>
    [System.Serializable]
    public abstract class TrackSerialize
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
    public abstract class ElementSerialize
    {
        public string uniqueName;

        public string parent;
        public int start;
        public int length;
    }

}

