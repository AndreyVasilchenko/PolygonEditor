using System;
using System.Collections.Generic;


namespace CL_MartinezRuedaClipping
{
    public partial class MartinezRuedaClipping
    {
        internal class TinyQueue
        {
            //FuncComporator DEFAULT_COMPARATOR;
            internal Comporator compare;
            internal List<SweepEvent> data;
            internal int length;

            internal TinyQueue(List<SweepEvent> data, Comporator compare)
            {
                //if (!(this instanceof TinyQueue)) 
                //    return new TinyQueue(data, compare);

                this.data = data ?? new List<SweepEvent>(); ;
                this.length = this.data.Count;
                this.compare = compare;// ?? defaultCompare;

                if (data != null)
                    for (int i = (int)Math.Floor((double)this.length / 2.0); i >= 0; i--)
                        this._down(i);
            }

            //public int defaultCompare(SweepEvent a, SweepEvent b)
            //{
            //    return a < b ? -1 : a > b ? 1 : 0;
            //}

            internal void push(SweepEvent item)
            {
                this.data.Add(item);
                this.length++;
                this._up(this.length - 1);
            }

            internal void _up(int pos)
            {
                List<SweepEvent> data = this.data;
                Comporator compare = this.compare;

                while (pos > 0)
                {
                    int parent = (int)Math.Floor((double)(pos - 1) / 2.0);
                    if (compare(data[pos], data[parent]) < 0)
                    {
                        swap(data, parent, pos);
                        pos = parent;
                    }
                    else
                        break;
                }
            }

            internal SweepEvent pop()
            {
                SweepEvent top = this.data[0];
                this.data[0] = this.data[this.length - 1];
                this.data.RemoveAt(this.length - 1);
                this.length--;
                this._down(0);
                return top;
            }

            internal void _down(int pos)
            {
                List<SweepEvent> data = this.data;
                Comporator compare = this.compare;
                int len = this.length;

                while (true)
                {
                    int left = 2 * pos + 1,
                        right = left + 1,
                        min = pos;

                    if (left < len && compare(data[left], data[min]) < 0)
                        min = left;
                    if (right < len && compare(data[right], data[min]) < 0)
                        min = right;

                    if (min == pos)
                        return;

                    swap(data, min, pos);
                    pos = min;
                }
            }

            internal void swap(List<SweepEvent> data, int i, int j)
            {
                var tmp = data[i];
                data[i] = data[j];
                data[j] = tmp;
            }

            internal SweepEvent peek()
            {
                return this.data[0];
            }

        }
    }
}
