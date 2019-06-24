# TrackEditor

エディタ拡張でTimeline的なトラック編集を行うためのフレームワークです。

要素(TrackData,TrackElement)を定義することで、GUIで編集し、アセットを出力(TrackAsset)することが出来ます。

サンプルではGameObjectにTrackAssetを保存し、再生時にTrackAssetPlayerで再生が可能です。

定義の追加は簡単なものでも数箇所の書き換えが必要ですが、基本的にはコピペして修正で済む形になっています。


![エディタ画面](Images/TrackEditor.png "エディタ画面")


## 処理の大体の説明

要素を追加する場合、以下のクラスの書き換えが必要です。

- TrackAsset 読み書きされるデータ
- TrackAssetWriter TrackAssetへの書き出し
- TrackAssetReader TrackAssetからの読み出し
- TrackDrawer エディターGUI
- Tracks TrackAssetPlayerでの再生時処理


以下のクラスは基本的に触らなくてよいはず。

- TrackEditor エディターの各種GUI制御
- TrackData トラック情報
- TrackElement トラック内の要素
- TrackEditorWindow

