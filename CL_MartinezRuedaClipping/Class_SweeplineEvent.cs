using System;
using System.Windows;

namespace CL_MartinezRuedaClipping
{

    public partial class MartinezRuedaClipping
    {
        internal class SweepEvent// : IComparable
        {
            public bool left;                  // Is left endpoint?                                    / Это точка с левым обходом? "Это стартовая точка"
            public Point point;                                                                        //   Координата события
            public SweepEvent otherEvent;      // Other edge reference                                 / Cсылка на другое ребро
            public bool isSubject;             // Belongs to source or clipping polygon                / Принадлежит источнику или обрезающему многоугольнику
            public EdgeType type;              // Edge contribution type                               / Тип ребра
            public bool inOut;                 // In-out transition for the sweepline crossing polygon / Переход на выходе для полигона пересечения спермы - Google-перевод )))
            public bool otherInOut;
            public SweepEvent prevInResult;    // Previous event in result?                            / Предыдущее событие в результате?
            public bool inResult;              // Does event belong to result?                         / Является ли событие результатом?
            public bool resultInOut;
            public bool isExteriorRing;        //  Внешнее кольцо
            public int contourId;
            public RedBlackTreeIterator iterator;
            public int pos;

            internal SweepEvent(Point point, bool left, SweepEvent otherEvent, bool isSubject, EdgeType? edgeType = null)
            {
                this.left = left;
                this.point = point;
                this.otherEvent = otherEvent;
                this.isSubject = isSubject;
                this.type = edgeType ?? EdgeType.NORMAL;
                this.inOut = false;
                this.otherInOut = false;
                this.prevInResult = null;
                this.inResult = false;

                // connection step
                this.resultInOut = false;
                this.isExteriorRing = true;
            }

            internal bool isBelow(Point p)
            {
                return this.left ? (signedArea(this.point, this.otherEvent.point, p) > 0)  :  (signedArea(this.otherEvent.point, this.point, p) > 0);
            }

            internal bool isAbove(Point p)
            {
                return !this.isBelow(p);
            }

            internal bool isVertical()
            {
                return this.point.X == this.otherEvent.point.X;
            }
        }
    }
}