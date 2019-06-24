using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace track_editor
{
    /// <summary>
    /// エディターで使用するアセット定義
    /// 
    /// 実際のところTrackAssetとの読み書きにしか使用しません
    /// </summary>
    public partial class TrackEditorAsset
    {
        TrackAsset asset;
        TrackEditor manager;

        public void ReadAsset(TrackEditorReader reader)
        {
            readAssetInternal(reader);

            manager.frameLength = asset.frameLength;
            manager.playSpeed = asset.playSpeed;

            // ルートを設定
            manager.top = RootTracks[0];
        }

        public void WriteAsset(TrackEditorWriter writer)
        {
            writer.trackBaseList = writer.trackListup(new List<TrackData>(), manager.top);
            writer.elementBaseList = writer.elementListup(new List<TrackElement>(), manager.top);

            writeAssetInternal(writer);

            asset.frameLength = manager.frameLength;
            asset.playSpeed = manager.playSpeed;
        }

        public TrackEditorAsset(TrackAsset asset, TrackEditor manager)
        {
            this.asset = asset;
            this.manager = manager;
        }
    }
}
