using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace track_editor_fw
{
    public class TrackManager
    {
        public int frameLength = 100;

        public int currentFrame = 0;

        public float gridScale = 4.0f;

        public float pixelScale { get => 5.0f / gridScale * settings.pixelScale; }

        public float trackHeight { get => settings.trackHeight; }

        public int gridScaleMax { get => settings.gridScaleMax; }

        public float childTrackSlide { get => settings.childTrackSlide; }

        private TrackEditorSettings settings { get; set; }

        public TrackBase top { get; private set; }

        public HeaderBase header { get; private set; }

        public TrackBase selectionTrack { get; private set; }

        public Vector2 scrollPos { get => scrPos; }

        public bool repaintRequest { get; private set; }

        public float elementWidth { get; private set; }

        Vector2 scrPos;


        public TrackManager(TrackEditorSettings settings)
        {
            this.settings = settings;
            top = new TrackBase();
            top.Initialize(this, "top", null);
        }

        public void SetHeader(HeaderBase header)
        {
            this.header = header;
        }

        public void SetSelectionTrack(TrackBase track)
        {
            selectionTrack = track;
            Repaint();
        }

        public void SetSelectionElement(TrackBase track, ElementBase element)
        {
            SetSelectionTrack(track);

            track.selectionElement = element;

            // 選択した要素を必ず先頭に置く。描画優先度とイベント判定優先度に影響する
            var index = track.elements.IndexOf(element);
            if (0 < index) {
                track.elements.SwapAt(0, 1);
            }

            Repaint();
        }

        public T AddTrack<T>(TrackBase parent, string name, T child) where T : TrackBase
        {
            parent.childs.Add(child);
            child.Initialize(this, name, parent);
            return child;
        }

        public void RemoveTrack(TrackBase parent,TrackBase track)
        {
            parent.removeTracks.Add(track);

            if (selectionTrack == track) {
                SetSelectionTrack(null);

                if (2 <= parent.childs.Count) {
                    int index = parent.childs.IndexOf(track) + 1;
                    if (parent.childs.Count <= index) {
                        index -= 2;
                    }

                    if (0 <= index && index < parent.childs.Count) {
                        SetSelectionTrack(parent.childs[index]);
                    }
                }
            }
        }

        public T AddElement<T>(TrackBase track, T element) where T : ElementBase
        {
            track.elements.Add(element);
            element.Initialize(track);
            SetSelectionElement(track, element);
            return element;
        }

        public void RemoveElement(TrackBase track, ElementBase element)
        {
            track.removeElements.Add(element);

            if (track.selectionElement == element) {
                track.selectionElement = null;

                if (2 <= track.elements.Count) {
                    int index = track.elements.IndexOf(element) + 1;
                    if (track.childs.Count <= index) {
                        index -= 2;
                    }

                    if (0 <= index && index < track.elements.Count) {
                        track.selectionElement = track.elements[index];
                    }
                }
            }
        }

        public void Repaint()
        {
            repaintRequest |= true;
        }

        public void OnGUI(Rect rect)
        {
            repaintRequest = false;

            var headerWidth = rect.width - settings.propertyWidth;
            var timeY = rect.y + settings.headerHeight;
            var propertyX = rect.x + rect.width - settings.propertyWidth;
            var propertyHeight = rect.height;
            var elementX = rect.x + settings.trackWidth;
            var elementY = rect.y + settings.headerHeight + settings.timeHeight;
            var elementWidth = rect.width - settings.trackWidth - settings.propertyWidth;
            var elementHeight = rect.height - settings.headerHeight - settings.timeHeight;

            Rect rectHeader = new Rect(rect.x, rect.y, headerWidth - 16, settings.headerHeight);
            Rect rectTime = new Rect(elementX, timeY, elementWidth, settings.timeHeight);
            Rect rectProperty = new Rect(propertyX, rect.y, settings.propertyWidth, propertyHeight);
            Rect rectTrack = new Rect(rect.x, elementY, settings.trackWidth, elementHeight);
            Rect rectElement = new Rect(elementX, elementY, elementWidth, elementHeight);

            this.elementWidth = elementWidth;

            using (new GUILayout.AreaScope(rectHeader)) {
                drawHeader(rectHeader);
            }

            // time + track + element
            {
                Rect rectTimeAndElement = new Rect(rectTime.x, rectTime.y, rectElement.width - 16, rectTime.height + rectTrack.height - 16);
                Rect rectTrackAndElement = new Rect(rectTrack.x, rectTrack.y, rectTrack.width + rectElement.width - 16, rectTrack.height - 16);

                using (new GUILayout.AreaScope(rectTime)) {
                    drawTime(rectTime);
                }

                using (new GUILayout.AreaScope(rectTrack)) {
                    drawTrack(rectTrack);
                }

                if (0 < rectElement.width && 0 < rectElement.height) {
                    using (new GUILayout.AreaScope(rectElement)) {
                        drawElement(rectElement);
                    }

                    drawFrameLine(rectTimeAndElement);
                }


                if (0 < rectElement.width) {
                    drawAreaLine(rectTimeAndElement);
                    drawAreaLine(rectTrackAndElement);
                }
            }

            using (new GUILayout.AreaScope(rectProperty)) {
                drawProperty(rectProperty);
            }
        }

        void drawAreaLine(Rect rect)
        {
            Handles.color = Color.gray;

            var points = new Vector3[] {
                new Vector3(rect.x, rect.y, 0),
                new Vector3(rect.x + rect.width, rect.y, 0),
                new Vector3(rect.x + rect.width, rect.y + rect.height, 0),
                new Vector3(rect.x, rect.y + rect.height, 0),
            };

            var indexies = new int[] {
                0, 1, 1, 2, 2, 3, 3, 0,
            };

            Handles.DrawLines(points, indexies);
        }

        void drawFrameLine(Rect rect)
        {
            // カレントフレーム変更
            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) {
                var mousePos = Event.current.mousePosition;

                if (rect.Contains(mousePos)) {
                    var relativePos = mousePos.x - rect.x + scrPos.x;
                    int frame = (int)(relativePos / pixelScale);

                    currentFrame = frame;
                    GUI.FocusControl("");
                    Repaint();

                    Event.current.Use();
                }
            }

            // カレントフレームの縦線
            {
                //var x = PixelToRectWidth(rect, FrameToPixel(currentFrame));
                var x = rect.x + currentFrame * pixelScale - scrPos.x;
                if (rect.Contains(new Vector2(x, rect.y))) {
                    Handles.color = Color.white;
                    Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.y + rect.height, 0));
                }
            }

            // 最終フレームの縦線
            {
                var x = rect.x + frameLength * pixelScale - scrPos.x;
                if (rect.Contains(new Vector2(x, rect.y))) {
                    Handles.color = Color.red;
                    Handles.DrawLine(new Vector3(x, rect.y, 0), new Vector3(x, rect.y + rect.height, 0));
                }
            }
        }

        void drawHeader(Rect rect)
        {
            header?.DrawHeader(rect);
        }

        void drawProperty(Rect rect)
        {
            if (selectionTrack != null) {
                Rect rectProperty = new Rect(0, 0, rect.width, rect.height);

                using (new GUILayout.AreaScope(rectProperty)) {
                    using (new GUILayout.VerticalScope()) {
                        var track = selectionTrack;

                        using (new GUILayout.VerticalScope("box")) {
                            track.PropertyDrawer(rect);

                        }

                        if (track.selectionElement != null) {
                            using (new GUILayout.VerticalScope("box")) {
                                track.selectionElement.PropertyDrawer(rect);

                            }
                        }
                    }
                }
            }
        }

        void drawTime(Rect rect)
        {
            int baseFrame = 10;

            if (15 < gridScale) {
                baseFrame = 100;
            } else if (10 < gridScale) {
                baseFrame = 50;
            } else if (5 < gridScale) {
                baseFrame = 20;
            }

            // フレーム数表示
            using (new GUI.ClipScope(new Rect(0, 0, rect.width - 16, rect.height))) {   // スクロールバー端の描画の都合で範囲調整
                var start= (int)(scrollPos.x / pixelScale);
                var end = (int)((scrollPos.x + rect.width - 16) / pixelScale);

                for (int frame = start; frame <= end; frame += baseFrame) {
                    float x = frame * pixelScale;

                    GUI.Label(new Rect(x - scrPos.x, 0, 40, 40), frame.ToString());
                }
            }
        }

        void drawTrack(Rect rect)
        {
            using (new GUI.ClipScope(new Rect(0, 0, rect.width, rect.height - 16))) {   // スクロールバー端の描画の都合で範囲調整
                Rect rectTrack = new Rect(0, -scrPos.y, settings.trackWidth, top.CalcTrackHeight());
                drawTrack(top, rectTrack);
            }
        }

        void drawElement(Rect rect)
        {
            using (new GUI.ClipScope(new Rect(0, 0, rect.width - 16, rect.height - 16))) {   // スクロールバー端の描画の都合で範囲調整
                Rect rectTrack = new Rect(-scrPos.x, -scrPos.y, 0, settings.trackHeight);
                DrawElement(top, rectTrack);
            }

            using (var scope = new EditorGUILayout.ScrollViewScope(scrPos)) {
                float scrollWidth = Mathf.Max(top.CalcElementWidth(), (frameLength + 30) * pixelScale);
                float scrollHeight = top.CalcTrackHeight() + settings.trackHeight;

                // ScrollViewを動かすためのダミー領域
                GUILayout.Label("", GUILayout.Width(scrollWidth - 24), GUILayout.Height(scrollHeight - 20));

                scrPos = scope.scrollPosition;
            }

            // 縦線
            {
                var start = (int)(scrollPos.x / pixelScale);
                var end = (int)((scrollPos.x + rect.width - 16) / pixelScale);

                // 1フレーム単位
                if (gridScale < 7) {
                    for (int frame = start; frame <= end; ++frame) {
                        var x = frame * pixelScale - scrollPos.x;
                        if (rect.Contains(new Vector2(x + rect.x, rect.y))) {
                            Handles.color = new Color(0, 0, 0, 0.1f);
                            Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, rect.height - 16, 0));
                        }
                    }
                }

                // 10フレーム単位
                {
                    for (int frame = start; frame <= end; frame += 10) {
                        var x = frame * pixelScale - scrollPos.x;
                        if (rect.Contains(new Vector2(x + rect.x, rect.y))) {
                            Handles.color = new Color(0, 0, 0, 0.1f);
                            Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, rect.height - 16, 0));
                        }
                    }
                }
            }

            // 横線
            {
                var start = (int)(scrollPos.y / settings.trackHeight);
                var end = (int)((scrollPos.y + rect.height - 16) / settings.trackHeight);

                for (int index = start; index <= end; ++index) {
                    var y = index * settings.trackHeight - scrollPos.y;
                    if (rect.Contains(new Vector2(rect.x, rect.y + y))) {
                        Handles.color = new Color(0, 0, 0, 0.3f);
                        Handles.DrawLine(new Vector3(0, y, 0), new Vector3(rect.width - 16, y, 0));
                    }
                }
            }

            updateTrackSelect(rect);
        }

        void updateTrackSelect(Rect rect)
        {
            if (Event.current.type == EventType.MouseDown) {
                var mousePos = Event.current.mousePosition;

                Rect mouseRect = new Rect(0, 0, rect.width, rect.height);
                if (mouseRect.Contains(mousePos)) {
                    var relativePos = mousePos.y + scrollPos.y;
                    int trackIndex = (int)(relativePos / settings.trackHeight);

                    int index = 0;
                    foreach (var track in top.childs) {
                        if (track.expand) {
                            foreach (var child in track.childs) {
                                if (index == trackIndex) {
                                    SetSelectionTrack(child);
                                    return;
                                }
                                ++index;
                            }
                        } else {
                            if (index == trackIndex) {
                                SetSelectionTrack(track);
                                return;
                            }
                            ++index;
                        }
                    }

                    //Event.current.Use();
                }
            }
        }

        public TrackEditorAsset Save<T>(string assetPath) where T : TrackEditorAsset
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null) {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, assetPath);
            }

            asset.frameLength = frameLength;


            List<TrackBase> trackBaseList = listupTrackBase(new List<TrackBase>(), top);
            List<ElementBase> elementBaseList = listupElementBase(new List<ElementBase>(), top);

            foreach (var trackBase in trackBaseList) {
                trackBase.Write(asset);

                //foreach (var element in elementBaseList) {
                //    track.elements.Add(elementBaseList.IndexOf(element));
                //}
            }



            //List<TrackBase> trackBaseList = listupTrackBase(new List<TrackBase>(), data.top);
            //List<ElementBase> elementBaseList = listupElementBase(new List<ElementBase>(), data.top);

            //asset.tracks.Clear();
            //foreach (var trackBase in trackBaseList) {
            //    var track = new Track();
            //    track.name = trackBase.name;
            //    track.parent = trackBaseList.IndexOf(trackBase.parent);
            //    foreach (var element in elementBaseList) {
            //        track.elements.Add(elementBaseList.IndexOf(element));
            //    }
            //    asset.tracks.Add(track);
            //}

            //asset.elements.Clear();
            //foreach (var elementBase in elementBaseList) {
            //    var element = new Element();
            //    element.start = elementBase.start;
            //    element.length = elementBase.length;
            //    element.parent = trackBaseList.IndexOf(elementBase.parent);
            //    asset.elements.Add(element);
            //}

            EditorUtility.SetDirty(asset);

            return asset;
        }

        static List<TrackBase> listupTrackBase(List<TrackBase> list, TrackBase track)
        {
            list.Add(track);
            foreach (var child in track.childs) {
                listupTrackBase(list, child);
            }
            return list;
        }

        static List<ElementBase> listupElementBase(List<ElementBase> list, TrackBase track)
        {
            list.AddRange(track.elements);
            foreach (var child in track.childs) {
                listupElementBase(list, child);
            }
            return list;
        }

        void drawTrack(TrackBase track, Rect rect)
        {
            if (0 < track.nestLevel) {
                track.TrackDrawer(rect);
            }

            if (track.expand) {
                // 深さに応じて表示位置をずらす
                var slideSize = (track.nestLevel == 0) ? 0.0f : rect.width * childTrackSlide;

                float x = rect.x + slideSize;
                float y = rect.y;
                float width = rect.width - slideSize;
                foreach (var child in track.childs) {
                    Rect rectChild = new Rect(x, y, width, child.CalcTrackHeight());
                    drawTrack(child, rectChild);
                    y += rectChild.height;
                }
            }

            if (Event.current.type == EventType.MouseDown) {
                if (rect.Contains(Event.current.mousePosition)) {
                    if (track.childs.Count == 0) {
                        SetSelectionTrack(track);

                    } else {
                        if (track.IsSelection) {
                            track.expand = !track.expand;
                            Repaint();

                        } else {
                            SetSelectionTrack(track);
                        }

                    }
                    Event.current.Use();
                }
            }

            // 安全なタイミングで削除
            foreach (var removeTrack in track.removeTracks) {
                int index = track.childs.IndexOf(removeTrack);
                if (index != -1) {
                    track.childs.RemoveAt(index);
                }
            }
            track.removeTracks.Clear();

            foreach (var removeElement in track.removeElements) {
                int index = track.elements.IndexOf(removeElement);
                if (index != -1) {
                    track.elements.RemoveAt(index);
                }
            }
            track.removeElements.Clear();
        }

        void DrawElement(TrackBase track, Rect rect)
        {

            // マウスイベントの取得順序と描画順序は逆
            Rect rectElement = new Rect(rect.x, rect.y, rect.width, trackHeight);
            for (int i = 0; i < track.elements.Count; ++i) {
                updateElement(track, track.elements[i], rectElement);
            }

            for (int i = track.elements.Count - 1; 0 <= i; --i) {
                drawElement(track.elements[i], rectElement);
            }

            if (track.expand) {
                float y = rect.y;
                foreach (var child in track.childs) {
                    Rect rectChild = new Rect(rect.x, y, rect.width, child.CalcTrackHeight());
                    DrawElement(child, rectChild);
                    y += rectChild.height;
                }
            }
        }

        /// <summary>
        /// ドラッグ関連
        /// 描画優先度とイベント優先度が逆のため、DrawElementとUpdateElemenetに関数を分離して回す必要がある
        /// </summary>
        /// <param name="rect"></param>
        void updateElement(TrackBase track, ElementBase element, Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + pixelScale * element.start - scrollPos.x, rect.y - scrollPos.y, pixelScale * element.length, trackHeight);
            Rect rectLength = new Rect(rectLabel.x + rectLabel.width, rect.y - scrollPos.y, pixelScale * 1, trackHeight);

            if (Event.current.type == EventType.MouseDown) {
                if (rectLabel.Contains(Event.current.mousePosition)) {
                    SetSelectionElement(track, element);

                    element.isDrag = true;
                    element.mouseOffset = rectLabel.position - Event.current.mousePosition;

                    Event.current.Use();

                } else if (rectLength.Contains(Event.current.mousePosition)) {
                    element.isLengthDrag = true;
                    element.mouseOffset = rectLength.position - Event.current.mousePosition;

                    Event.current.Use();
                }

            } else if (Event.current.type == EventType.MouseUp) {
                element.isDrag = false;
                element.isLengthDrag = false;

            } else if (Event.current.type == EventType.MouseDrag) {
                if (element.isDrag) {
                    var currentFrame = (int)((Event.current.mousePosition.x - rect.x + scrollPos.x + element.mouseOffset.x) / pixelScale);

                    element.start = currentFrame;
                    Repaint();

                    Event.current.Use();

                } else if (element.isLengthDrag) {
                    var currentFrame = (int)((Event.current.mousePosition.x - rect.x + scrollPos.x + element.mouseOffset.x) / pixelScale);

                    element.length = Mathf.Max(1, currentFrame - element.start);
                    Repaint();

                    Event.current.Use();
                }
            }
        }

        void drawElement(ElementBase element, Rect rect)
        {
            Rect rectLabel = new Rect(rect.x + pixelScale * element.start - scrollPos.x, rect.y - scrollPos.y, pixelScale * element.length, trackHeight);

            element.ElementDrawer(rectLabel);
        }



    }
}

