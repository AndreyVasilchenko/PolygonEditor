/*using System;
using System.Collections.Generic;

namespace CL_MartinezRuedaClipping
{
    public partial class MartinezRuedaClipping
    {

//          public delegate int FuncComporator<T>(T a, T b) where T : IComparable;

          public class Queue
          {
            FuncComporator<SweepEvent> DEFAULT_COMPARATOR;
            FuncComporator<SweepEvent> _comparator;
            List<SweepEvent> data;
            int length;

            int DEF_COMPARATOR<T>(T a, T b)
            {
                return a < b ? -1 : a > b ? 1 : 0;
            }

            Queue(List<SweepEvent> data, FuncComporator<SweepEvent> comparator)
            {
                this.DEFAULT_COMPARATOR = DEF_COMPARATOR;
                this._comparator = comparator ?? DEFAULT_COMPARATOR;
                this.data = new List<SweepEvent>();
                this.length = 0;
                if (data != null)
                {
                    this.length = data.Count;
                    for (int i = 0; i < data.Count; i++)
                        this.data.Add(data[i]);
                }
            }*/


            // First element
/*            public SweepEvent peek()
            {
                return this.data[0];
            }*/


            /**
             * @return {*}
             */
/*            public SweepEvent pop()
            {
                List<SweepEvent> _data = this.data;
                SweepEvent first = _data[0];
                data.RemoveAt(_data.Count - 1);
                int size = this.length--;
                SweepEvent last = _data[size - 1];

                if (size == 0)
                {
                    return first;
                }

                _data[0] = last;
                int current = 0;
                var compare = this._comparator;

                while (current < size)
                {
                    int largest = current;
                    int left = (2 * current) + 1;
                    int right = (2 * current) + 2;

                    if (left < size && compare(_data[left], _data[largest]) > 0)
                    {
                        largest = left;
                    }

                    if (right < size && compare(_data[right], _data[largest]) > 0)
                    {
                        largest = right;
                    }

                    if (largest == current)
                        break;

                    this._swap(largest, current);
                    current = largest;
                }

                return first;
            }*/

/*            public int push(SweepEvent element)
            {
                this.data.Add(element);
                this.length = this.data.Count;
                int current_idx = this.length - 1;
                FuncComporator<SweepEvent> compare = this._comparator;
                var data = this.data;

                while (current_idx > 0)
                {
                    int parent_idx = (int)Math.Floor((double)((current_idx - 1) / 2));
                    if (compare(data[current_idx], data[parent_idx]) > 0)
                        break;
                    this._swap(parent_idx, current_idx);
                    current_idx = parent_idx;
                }

                return this.length;
            }*/

/*            public int size()
            {
                return (this.length);
            }*/

            /**
            * @param {Function} fn
            * @param {*}        context
            */
/*            public void forEach(fn, context)
            {
                this.data.forEach(fn, context);
            }*/

/*            public void _swap(int a_idx, int b_idx)
            {
                var temp = this.data[a_idx];
                this.data[a_idx] = this.data[b_idx];
                this.data[b_idx] = temp;
            }
        }
    }
}*/
