using System;
using System.Windows;
using System.Collections.Generic;


namespace CL_MartinezRuedaClipping
{
    public partial class MartinezRuedaClipping
    {
        internal enum Operation : int
        {
            INTERSECTION = 0,
            UNION = 1,
            DIFFERENCE = 2,
            XOR = 3,
        }

        static Point[][][] EMPTY = new Point[][][] { };

        internal static int contourId = 0;
        //var max = Math.max;
        //var min = Math.min;

        /**
         * @param  {Array<Number>} s1
         * @param  {Array<Number>} s2
         * @param  {Boolean}         isSubject
         * @param  {Queue}           eventQueue
         * @param  {Array<Number>}  bbox
         */
        internal static void processSegment(Point s1, Point s2, bool isSubject, int depth, TinyQueue eventQueue, double[] bbox, bool isExteriorRing) 
        {
            // Possible degenerate condition.
            // if (equals(s1, s2)) return;
            var e1 = new SweepEvent(s1, false, null, isSubject);
            var e2 = new SweepEvent(s2, false, e1,   isSubject);
            e1.otherEvent = e2;

            e1.contourId = e2.contourId = depth;
            if (!isExteriorRing) 
            {
                e1.isExteriorRing = false;
                e2.isExteriorRing = false;
            }
          
            if (compareEvents(e1, e2) > 0) 
                e2.left = true;
            else
                e1.left = true;

            bbox[0] = Math.Min(bbox[0], s1.X);
            bbox[1] = Math.Min(bbox[1], s1.Y);
            bbox[2] = Math.Max(bbox[2], s1.X);
            bbox[3] = Math.Max(bbox[3], s1.Y);

            // Pushing it so the queue is sorted from left to right, with object on the left having the highest priority.
            eventQueue.push(e1);
            eventQueue.push(e2);
        }

        internal static void processPolygon(Point[] contourOrHole, bool isSubject, int depth, TinyQueue queue, double[] bbox, bool isExteriorRing) 
        {
            int len = contourOrHole.GetLength(0) - 1;
            for (int i = 0; i < len; i++) 
            {
                processSegment(contourOrHole[i], contourOrHole[i + 1], isSubject, depth + 1, queue, bbox, isExteriorRing);
            }
        }

        internal static TinyQueue fillQueue(Point[][][] subject, Point[][][] clipping, double[] sbbox, double[] cbbox)
        {
            TinyQueue eventQueue = new TinyQueue(null, compareEvents);
            Point[][] polygonSet;
            bool isExteriorRing;

            int len= subject.GetLength(0);
            for (int i = 0; i < len; i++)
            {
                polygonSet = subject[i];
                int jj = polygonSet.GetLength(0);
                for (int j = 0;  j < jj; j++) 
                {
                    isExteriorRing=  (j == 0);
                    if(isExteriorRing) 
                        contourId++;

                    processPolygon(polygonSet[j], true, contourId, eventQueue, sbbox, isExteriorRing);
                }
            }

            for (int i = 0, ii = clipping.GetLength(0); i < ii; i++) 
            {
                polygonSet = clipping[i];
                int jj = polygonSet.GetLength(0);
                for (int j = 0; j < jj; j++)
                {
                    isExteriorRing = (j == 0);
                    if (isExteriorRing) 
                        contourId++;
                    processPolygon(polygonSet[j], false, contourId, eventQueue, cbbox, isExteriorRing);
                }
            }

            return eventQueue;
        }

        internal static void computeFields(SweepEvent _event, SweepEvent prev, Operation operation)
        {
            if (prev == null) {// compute inOut and otherInOut fields
                _event.inOut      = false;
                _event.otherInOut = true;
            } 
            else {  
                if (_event.isSubject == prev.isSubject) {   // previous line segment in sweepline belongs to the same polygon
                    _event.inOut = !prev.inOut;
                    _event.otherInOut = prev.otherInOut;
                }
                else {                                      // previous line segment in sweepline belongs to the clipping polygon
                    _event.inOut      = !prev.otherInOut;
                    _event.otherInOut = prev.isVertical() ? !prev.inOut : prev.inOut;
                }
                
                if ( prev != null) {                        // compute prevInResult field
                    _event.prevInResult = (!inResult(prev, operation) || prev.isVertical()) ? prev.prevInResult : prev;
                }
            }
          // check if the line segment belongs to the Boolean operation
          _event.inResult = inResult(_event, operation);
        }

        internal static bool inResult(SweepEvent _event, Operation operation)
        {
            switch (_event.type)
            {
                case EdgeType.NORMAL:
                    switch (operation)
                    {
                        case Operation.INTERSECTION:
                                return !_event.otherInOut;
                        case Operation.UNION:
                                return _event.otherInOut;
                        case Operation.DIFFERENCE:
                                return (_event.isSubject && _event.otherInOut) || (!_event.isSubject && !_event.otherInOut);
                        case Operation.XOR:
                                return true;
                    }
                    break;
          
                case EdgeType.SAME_TRANSITION:
                    return operation == Operation.INTERSECTION || operation == Operation.UNION;
            
                case EdgeType.DIFFERENT_TRANSITION:
                    return operation == Operation.DIFFERENCE;
            
                case EdgeType.NON_CONTRIBUTING:
                    return false;
            }
            return false;
        }

        internal static int possibleIntersection(SweepEvent se1, SweepEvent se2, TinyQueue queue)
        {
            // that disallows self-intersecting polygons,
            // did cost us half a day, so I'll leave it
            // out of respect
            // if (se1.isSubject === se2.isSubject) return;
            var inter = SegmentIntersection( se1.point, se1.otherEvent.point, se2.point, se2.otherEvent.point);

            var nintersections = inter != null ? inter.Length : 0;
            
            if (nintersections == 0)
                return 0; // no intersection

            // the line segments intersect at an endpoint of both line segments
            if ((nintersections == 1)  &&  (equals(se1.point, se2.point) || equals(se1.otherEvent.point, se2.otherEvent.point))) 
                return 0;

            if (nintersections == 2  &&  se1.isSubject == se2.isSubject) {
            // if(se1.contourId === se2.contourId){
            // console.warn('Edges of the same polygon overlap',
            //   se1.point, se1.otherEvent.point, se2.point, se2.otherEvent.point);
            // }
            //throw new Error('Edges of the same polygon overlap');
                return 0;
            }

            // The line segments associated to se1 and se2 intersect
            if ( nintersections == 1 )
            {
                // if the intersection point is not an endpoint of se1
                if (!equals(se1.point, inter[0]) && !equals(se1.otherEvent.point, inter[0]))
                    divideSegment(se1, inter[0], queue);
                
                // if the intersection point is not an endpoint of se2
                if (!equals(se2.point, inter[0]) && !equals(se2.otherEvent.point, inter[0])) 
                    divideSegment(se2, inter[0], queue);
                
                return 1;
            }

            // The line segments associated to se1 and se2 overlap
            var events        = new List<SweepEvent>();
            var leftCoincide  = false;
            var rightCoincide = false;

            if( equals(se1.point, se2.point) ){
                leftCoincide = true; // linked
            }
            else if (compareEvents(se1, se2) == 1){
                events.Add(se2);
                events.Add(se1);
            }
            else {
                events.Add(se1);
                events.Add(se2);
            }

            if ( equals(se1.otherEvent.point, se2.otherEvent.point) ){
                rightCoincide = true;
            }
            else if (compareEvents(se1.otherEvent, se2.otherEvent) == 1){
                events.Add(se2.otherEvent);
                events.Add(se1.otherEvent);
            }
            else {
                events.Add(se1.otherEvent);
                events.Add(se2.otherEvent);
            }

            if( (leftCoincide && rightCoincide) || leftCoincide )                   // both line segments are equal or share the left endpoint
            {
                se1.type = EdgeType.NON_CONTRIBUTING;
                se2.type = (se1.inOut == se2.inOut) ? EdgeType.SAME_TRANSITION : EdgeType.DIFFERENT_TRANSITION;

                if (leftCoincide && !rightCoincide) 
                    // honestly no idea, but changing events selection from [2, 1] to [0, 1] fixes the overlapping self-intersecting polygons issue
                    divideSegment(events[1].otherEvent, events[0].point, queue);
                
                return 2;
            }

            if (rightCoincide) {                                                    // the line segments share the right endpoint
                divideSegment(events[0], events[1].point, queue);
                return 3;
            }

            if (events[0] != events[3].otherEvent) {            // no line segment includes totally the other one
                divideSegment(events[0], events[1].point, queue);
                divideSegment(events[1], events[2].point, queue);
                return 3;
            }

            // one line segment includes the other one
            divideSegment(events[0], events[1].point, queue);
            divideSegment(events[3].otherEvent, events[2].point, queue);

            return 3;
        }

        internal static TinyQueue divideSegment(SweepEvent se, Point p, TinyQueue queue)  
        {
            var r = new SweepEvent(p, false, se,            se.isSubject);
            var l = new SweepEvent(p, true,  se.otherEvent, se.isSubject);

            if (equals(se.point, se.otherEvent.point))
            {
                MessageBox.Show("what is that?"+se.ToString());
            }

            r.contourId = l.contourId = se.contourId;

            // avoid a rounding error. The left _event would be processed after the right _event
            if (compareEvents(l, se.otherEvent) > 0) {
                se.otherEvent.left = true;
                l.left = false;
            }

            // avoid a rounding error. The left _event would be processed after the right _event
            // if (compareEvents(se, r) > 0) {}

            se.otherEvent.otherEvent = l;
            se.otherEvent = r;

            queue.push(l);
            queue.push(r);

            return queue;
        }


        ///* eslint-disable no-unused-vars, no-debugger, no-undef */
        //function iteratorEquals(it1, it2) {
        //  return it1._cursor === it2._cursor;
        //}


        //function _renderSweepLine(sweepLine, pos, _event)
        //{
        //var map = window.map;
        //if (!map) return;
        //if (window.sws) window.sws.forEach(function (p) {
        //map.removeLayer(p);
        //});
        //window.sws = [];
        //sweepLine.forEach(function (e) {
        //var poly = L.polyline([
        //    e.point.slice().reverse(),
        //    e.otherEvent.point.slice().reverse()
        //], {color: 'green'}).addTo(map);
        //window.sws.push(poly);
        //});

        //if (window.vt) map.removeLayer(window.vt);
        //var v = pos.slice();
        //var b = map.getBounds();
        //window.vt = L.polyline([
        //[b.getNorth(), v[0]],
        //[b.getSouth(), v[0]]
        //], {color: 'green', weight: 1}).addTo(map);

        //if (window.ps) map.removeLayer(window.ps);
        //window.ps = L.polyline([
        //_event.point.slice().reverse(),
        //_event.otherEvent.point.slice().reverse()
        //], {color: 'black', weight: 9, opacity: 0.4}).addTo(map);
        //debugger;
        //}
        ///* eslint-enable no-unused-vars, no-debugger, no-undef */


        internal static List<SweepEvent> subdivideSegments(TinyQueue eventQueue, Point[][][] subject, Point[][][] clipping, double[] sbbox, double[] cbbox, Operation operation)
        {
            RedBlackTree sweepLine = new RedBlackTree(_comp_Segm);
            List<SweepEvent> sortedEvents = new List<SweepEvent>();

            double rightbound = Math.Min(sbbox[2], cbbox[2]);

            RedBlackTreeIterator prev, next;

            SweepEvent prevEvent, prevprevEvent;

            while( eventQueue.length > 0 )
            {
                SweepEvent _event = eventQueue.pop();
                sortedEvents.Add(_event);

                // optimization by bboxes for intersection and difference goes here  - коммент оригинала
                if ( (operation == Operation.INTERSECTION && _event.point.X > rightbound) || (operation == Operation.DIFFERENCE   && _event.point.X > sbbox[2]))
                    break;

                if (_event.left) 
                {
                    sweepLine = sweepLine.insert(_event,_event);
                    //_renderSweepLine(sweepLine, _event.point, _event);  - коммент оригинала

                    next = sweepLine.find(_event);
                    prev = sweepLine.find(_event);
                    _event.iterator = sweepLine.find(_event);

                    if (!prev.node.Equals(sweepLine.begin.node)){                                            // ??? (prev.node !== sweepLine.begin)
                        prev.prev();
                    } 
                    else {
                        prev = sweepLine.begin;
                        prev.prev();
                        prev.next();
                    }
                    next.next();

                    //---  prevEvent = (prev.key || null), prevprevEvent;
                    prevEvent = (SweepEvent)Convert.ChangeType(prev.key, typeof(SweepEvent));
                    computeFields(_event, prevEvent, operation);
                    if (next.node != null) {
                        if (possibleIntersection(_event, (SweepEvent)Convert.ChangeType(next.key, typeof(SweepEvent)), eventQueue) == 2) {
                            computeFields(_event, prevEvent, operation);
                            computeFields(_event, (SweepEvent)Convert.ChangeType(next.key, typeof(SweepEvent)), operation);
                        }
                    }

                    if (prev.node!=null) 
                    {
                        if (possibleIntersection((SweepEvent)Convert.ChangeType(prev.key, typeof(SweepEvent)), _event, eventQueue) == 2) {
                            RedBlackTreeIterator prevprev = sweepLine.find((SweepEvent)Convert.ChangeType(prev.key, typeof(SweepEvent)));
                            if (!prevprev.node.Equals(sweepLine.begin.node)) {                          //prevprev.node != sweepLine.begin
                                prevprev.prev();
                            } 
                            else {
                                prevprev = sweepLine.find((SweepEvent)Convert.ChangeType(sweepLine.end.key, typeof(SweepEvent)));
                                prevprev.next();
                            }
                            prevprevEvent = (SweepEvent)Convert.ChangeType(prevprev.key, typeof(SweepEvent));
                            computeFields(prevEvent, prevprevEvent, operation);
                            computeFields(_event, prevEvent, operation);
                        }
                    }
                } 
                else {
                    _event = _event.otherEvent;
                    next = sweepLine.find(_event);
                    prev = sweepLine.find(_event);

                    // _renderSweepLine(sweepLine, _event.otherEvent.point, _event);  - коммент оригинала

                    if ( !(prev != null  &&  next != null) )
                        continue;

                    if ( !prev.node.Equals(sweepLine.begin.node)) {                                     // prev.node !=  sweepLine.begin
                        prev.prev();
                    } 
                    else {
                        prev = sweepLine.begin;
                        prev.prev();
                        prev.next();
                    }
                    next.next();
                    sweepLine = sweepLine.remove(_event);

                    // _renderSweepLine(sweepLine, _event.otherEvent.point, _event);  - коммент оригинала

                    if (next.node != null  &&  prev.node != null)
                    {
                        if ( prev.node.value != null  &&  next.node.value != null )//-- if (typeof prev.node.value != 'undefined' && typeof next.node.value != 'undefined') 
                        {
                            possibleIntersection((SweepEvent)Convert.ChangeType(prev.key, typeof(SweepEvent)), (SweepEvent)Convert.ChangeType(next.key, typeof(SweepEvent)), eventQueue);
                        }
                    }
                }
            }
          return sortedEvents;
        }

        /**
         * @param  {Array.<SweepEvent>} sortedEvents
         * @return {Array.<SweepEvent>}
         */
        internal static List<SweepEvent> orderEvents(List<SweepEvent> sortedEvents)
        {
            SweepEvent _event, tmp;
            List<SweepEvent> resultEvents = new List<SweepEvent>();

            for (int i = 0; i < sortedEvents.Count; i++)
            {
                _event = sortedEvents[i];
                if((_event.left && _event.inResult) || (!_event.left && _event.otherEvent.inResult)) 
                {
                    resultEvents.Add(_event);
                }
            }

            // Due to overlapping edges the resultEvents array can be not wholly sorted
            bool sorted = false;
            while( !sorted )
            {
                sorted = true;
                for (int i = 0; i < resultEvents.Count; i++) 
                {
                    if ((i + 1) < resultEvents.Count && compareEvents(resultEvents[i], resultEvents[i + 1]) == 1)
                    {
                        tmp = resultEvents[i];
                        resultEvents[i] = resultEvents[i + 1];
                        resultEvents[i + 1] = tmp;
                        sorted = false;
                    }
                }
            }

            for (int i = 0; i < resultEvents.Count; i++) 
            {
                resultEvents[i].pos = i;
            }

            for (int i = 0; i < resultEvents.Count; i++) 
            {
                if( !resultEvents[i].left )
                {
                    var temp = resultEvents[i].pos;
                    resultEvents[i].pos = resultEvents[i].otherEvent.pos;
                    resultEvents[i].otherEvent.pos = temp;
                }
            }

            return resultEvents;
        }

        /**
         * @param  {Array.<SweepEvent>} sortedEvents
         * @return {Array.<*>} polygons
         */
        internal static Point[][][] connectEdges(List<SweepEvent> sortedEvents)
        {
            List<SweepEvent> resultEvents = orderEvents(sortedEvents);

            // "false"-filled array
            bool[] processed = new bool[resultEvents.Count];    //new Array(resultEvents.length);
            processed.Initialize();
            
            List<List<List<Point>>> MultiPoligon = new List<List<List<Point>>>();

            for (int i = 0; i < resultEvents.Count; i++)
            {
                if( processed[i] )
                    continue;
                
                List<List<Point>> contour = new List<List<Point>>();

                if (!resultEvents[i].isExteriorRing){
                    //--result[result.length - 1].push([contour]);
                    MultiPoligon.Insert(MultiPoligon.Count - 1,contour);
                } 
                else{
                    //--result.push(contour);
                    MultiPoligon.Add(contour);
                }

                var ringId = MultiPoligon.Count-1;//result.length - 1;
                int pos = i;

                Point initial = resultEvents[i].point;
                // initial.push(resultEvents[i].isExteriorRing); - этот коммент в оригинале
                contour.Add(new List<Point>());
                contour[0].Add(initial);

                while (pos >= i)
                {
                    processed[pos] = true;

                    if (resultEvents[pos].left) {
                        resultEvents[pos].resultInOut = false;
                        resultEvents[pos].contourId   = ringId;
                    } 
                    else {
                        resultEvents[pos].otherEvent.resultInOut = true;
                        resultEvents[pos].otherEvent.contourId   = ringId;
                    }

                    pos = resultEvents[pos].pos;
                    processed[pos] = true;
                    // resultEvents[pos].point.push(resultEvents[pos].isExteriorRing); - этот коммент в оригинале

                    contour[0].Add(resultEvents[pos].point);
                    pos = nextPos(pos, resultEvents, processed);
                }

                pos = (pos == -1 ? i : pos);

                processed[pos] = processed[resultEvents[pos].pos] = true;
                resultEvents[pos].otherEvent.resultInOut = true;
                resultEvents[pos].otherEvent.contourId   = ringId;
            }

            for(int i = 0; i < MultiPoligon.Count; i++)
            {
                List<List<Point>> polygon = MultiPoligon[i];
                for (int j = 0; j < polygon.Count; j++)
                {
                    List<Point> polygonContour = polygon[j];
                    for (int k = 0; k < polygonContour.Count; k++)
                    {
                        Point coords = polygonContour[k];
                        if (coords == null/*typeof coords[0] != 'number'*/) {
                            
                            //--polygon.push(coords[0]);   --- ????
                            
                            //--polygon.splice(j, 1);
                            polygon.RemoveAt(j);
                        }
                    }
                }
            }
            Point[][][] result = ListToArray(MultiPoligon); 
            return result;
        }


        /* @param  {Number} pos
         * @param  {Array.<SweepEvent>} resultEvents
         * @param  {Array.<Boolean>}    processed
         * @return {Number} */
        internal static int nextPos(int pos, List<SweepEvent> resultEvents, bool[] processed)
        {
            int newPos = pos + 1;
            
            while( newPos < resultEvents.Count  &&  equals(resultEvents[newPos].point, resultEvents[pos].point) ) 
            {
                if( !processed[newPos] ) 
                    return newPos;
                else 
                    newPos = newPos + 1;
            }

            newPos = pos - 1;

            while( newPos > -1 && processed[newPos]) {
                newPos = newPos - 1;
            }
            return newPos;
        }


        internal static Point[][][] trivialOperation(Point[][][] subject, Point[][][] clipping, Operation operation)
        {
            Point[][][] result = null;

            if (subject.Length * clipping.Length == 0)
            {
                if (operation == Operation.INTERSECTION) {
                    result = EMPTY;
                } 
                else if (operation == Operation.DIFFERENCE) {
                    result = subject;
                } 
                else if (operation == Operation.UNION || operation == Operation.XOR) {
                    result = (subject.Length == 0) ? clipping : subject;
                }
            }
            return result;
        }


        internal static Point[][][] compareBBoxes(Point[][][] subject, Point[][][] clipping, double[] sbbox, double[] cbbox, Operation operation)
        {
            Point[][][] result = null;

            if (sbbox[0] > cbbox[2] || cbbox[0] > sbbox[2] || sbbox[1] > cbbox[3] || cbbox[1] > sbbox[3] ) 
            {
                if (operation ==  Operation.INTERSECTION) {
                    result = EMPTY;
                } 
                else if (operation == Operation.DIFFERENCE) {
                    result = subject;
                } 
                else if (operation == Operation.UNION || operation == Operation.XOR) {
                    result = ConcatArrays(subject, clipping);
                }
            }
            return result;
        }


        internal static Point[][][] boolean(object _subject, object _clipping, Operation operation)
        {
            Point[][][] subject;
            if (_subject is GeoPolygon)
                subject = new Point[][][] { (Point[][])((GeoPolygon)_subject).coordinates };
            else if (_subject is GeoMultiPolygon)
                subject = (Point[][][])((GeoMultiPolygon)_subject).coordinates;
            else if (_subject is Point[][][])
                subject = _subject as Point[][][];
            else
            {
                MessageBox.Show("Обрабатываются только полигоны и мульти-полигоны! Субъект к таковым не относится!");
                return (null);
            }
            
            Point[][][] clipping;
            if( _clipping is GeoPolygon)
                clipping = new Point[][][]{(Point[][])((GeoPolygon)_clipping).coordinates};
            else if( _clipping is GeoMultiPolygon )
                clipping = (Point[][][])((GeoMultiPolygon)_clipping).coordinates;
            else if (_clipping is Point[][][])
                clipping = _clipping as Point[][][];
            else
            {
                MessageBox.Show("Обрабатываются только полигоны и мульти-полигоны! Клиппер к таковым не относится!");
                return (null);
            }
            
            
            Point[][][] trivial = trivialOperation(subject, clipping, operation);
            if (trivial != null) {
                return trivial == EMPTY ? null : trivial;
            }

            double[] sbbox = new double[] { double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity };
            double[] cbbox = new double[] { double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity };

            var eventQueue = fillQueue(subject, clipping, sbbox, cbbox);

            trivial = compareBBoxes(subject, clipping, sbbox, cbbox, operation);
            if (trivial != null) {
                return trivial == EMPTY ? null : trivial;
            }

            var sortedEvents = subdivideSegments(eventQueue, subject, clipping, sbbox, cbbox, operation);
            
            return connectEdges(sortedEvents);
        }


        public static Point[][][] union(object subject, object clipping)
        {
          return boolean(subject, clipping, Operation.UNION);
        }


        public static Point[][][] diff(object subject, object clipping)
        {
          return boolean(subject, clipping, Operation.DIFFERENCE);
        }


        public static Point[][][] xor(object subject, object clipping)
        {
          return boolean(subject, clipping, Operation.XOR);
        }


        public static Point[][][] intersection(object subject, object clipping) 
        {
          return boolean(subject, clipping, Operation.INTERSECTION);
        }








        internal static Point[][][] ListToArray(List<List<List<Point>>> multiPoligon)
        {
            Point[][][] result = new Point[multiPoligon.Count][][];
            for (int i = 0; i < multiPoligon.Count; i++)
            {
                result[i] = new Point[multiPoligon[i].Count][];
                for (int j = 0; j < multiPoligon[i].Count; j++)
                {
                    result[i][j] = new Point[multiPoligon[i][j].Count];
                    for (int k = 0; k < multiPoligon[i][j].Count; k++)
                    {
                        result[i][j][k] = multiPoligon[i][j][k];
                    }
                }
            }

            return result;
        }

        public static Point[][][] ConcatArrays(Point[][][] arr1, Point[][][] arr2)
        {
            int len1 = arr1.Length + arr2.Length;

            Point[][][] concatArr = new Point[len1][][];
            
            for (int i = 0; i < arr1.Length; i++)
            {
                concatArr[i] = new Point[arr1[i].Length][];
                for (int j = 0; j < arr1[i].Length; j++)
                {
                    concatArr[i][j] = new Point[arr1[i][j].Length];
                    for (int k = 0; k < arr1[i][j].Length; k++)
                    {
                        concatArr[i][j][k] = arr1[i][j][k];
                    }
                }
            }

            for (int i = arr1.Length, i2= 0;  i < len1;  i++, i2++)
            {
                concatArr[i] = new Point[arr2[i2].Length][];
                for (int j = 0; j < arr2[i2].Length; j++)
                {
                    concatArr[i][j] = new Point[arr2[i2][j].Length];
                    for (int k = 0; k < arr2[i2][j].Length; k++)
                    {
                        concatArr[i][j][k] = arr2[i2][j][k];
                    }
                }
            }

            return concatArr;
        }

    }
}