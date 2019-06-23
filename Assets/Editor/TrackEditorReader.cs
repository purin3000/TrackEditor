using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace track_editor
{
    /// <summary>
    /// TrackAssetからの読み込み
    /// </summary>
    public class TrackEditorReader
    {
        TrackAsset asset;
        TrackEditor manager;

        List<RootTrackData> rootTracks = new List<RootTrackData>();
        List<GameObjectTrackData> gameObjectTracks = new List<GameObjectTrackData>();
        List<ActivationTrackData> activationTracks = new List<ActivationTrackData>();
        List<PositionTrackData> positionTracks = new List<PositionTrackData>();
        List<AnimationTrackData> animationTracks = new List<AnimationTrackData>();

        List<ActivationElement> activationElements = new List<ActivationElement>();
        List<PositionElement> positionElements = new List<PositionElement>();
        List<AnimationElement> animationElements = new List<AnimationElement>();

        List<TrackPair> allTracks = new List<TrackPair>();
        List<ElementPair> allElements = new List<ElementPair>();

        Dictionary<string, TrackData> trackTable = new Dictionary<string, TrackData>();

        public TrackEditorReader(TrackAsset asset, TrackEditor manager)
        {
            this.asset = asset;
            this.manager = manager;
        }

        public void ReadAsset()
        {
            ReadTracks(asset.rootTracks, rootTracks);
            ReadTracks(asset.gameObjectTracks, gameObjectTracks);
            ReadTracks(asset.activationTracks, activationTracks);
            ReadTracks(asset.positionTracks, positionTracks);
            ReadTracks(asset.animationTracks, animationTracks);

            ReadElements(asset.activationElements, activationElements);
            ReadElements(asset.positionElements, positionElements);
            ReadElements(asset.animationElements, animationElements);

            // トラックの階層構築
            updateHierarchy(manager, asset.rootTracks[0].uniqueName);
        }

        void ReadTracks<SerializeTrackClass, TrackDataClass>(List<SerializeTrackClass> serializeTracks, List<TrackDataClass> tracks)
            where SerializeTrackClass : SerializeTrack
            where TrackDataClass : TrackData, new()
        {
            foreach (var serializeTrack in serializeTracks) {
                TrackDataClass track = new TrackDataClass();

                tracks.Add(track);
                allTracks.Add(new TrackPair(track, serializeTrack));
                trackTable.Add(serializeTrack.uniqueName, track);

                track.ReadAsset(serializeTrack);
            }
        }

        void ReadElements<SerializeElementClass, ElementClass>(List<SerializeElementClass> serializeElements, List<ElementClass> elements)
            where SerializeElementClass : SerializeElement
            where ElementClass : TrackElement, new()
        {
            foreach (var serializeElement in serializeElements) {
                ElementClass element = new ElementClass();

                elements.Add(element);
                allElements.Add(new ElementPair(element, serializeElement));

                element.ReadAsset(serializeElement);
            }
        }

        void updateHierarchy(TrackEditor manager, string rootTrackName)
        {
            // 親子階層を設定
            foreach (var trackPair in allTracks) {
                var serializeTrack = trackPair.serializeTrack;
                TrackData parent;

                if (trackTable.TryGetValue(serializeTrack.parent, out parent)) {
                    //parent =
                }

                List<TrackData> childs = new List<TrackData>();
                foreach (var child in serializeTrack.childs) {
                    //Debug.Log(child);
                    childs.Add(trackTable[child]);
                }

                trackPair.track.LoadInitialize(manager, serializeTrack.name, parent, childs);
            }

            foreach (var trackPair in allTracks) {
                trackPair.track.UpdateNestLevel();
            }

            foreach (var elementPair in allElements) {
                var serializeElement = elementPair.serializeElement;
                var track = trackTable[serializeElement.parent];

                elementPair.element.LoadInitialize(serializeElement.name, serializeElement.start, serializeElement.length, track);
                track.elements.Add(elementPair.element);
            }

            // ルートを設定
            manager.top = rootTracks[0];
        }
        struct TrackPair
        {
            public TrackPair(TrackData track, SerializeTrack serializeTrack)
            {
                this.track = track;
                this.serializeTrack = serializeTrack;
            }

            public TrackData track;
            public SerializeTrack serializeTrack;
        }

        struct ElementPair
        {
            public ElementPair(TrackElement element, SerializeElement serializeElement)
            {
                this.element = element;
                this.serializeElement = serializeElement;
            }

            public TrackElement element;
            public SerializeElement serializeElement;
        }

    }
}

