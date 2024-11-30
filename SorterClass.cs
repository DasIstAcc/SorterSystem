using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortAnalysis
{
    internal abstract class SorterClass<T> where T : IComparable
    {
        public abstract void Sort(T[] array);

        protected void swap(T[] values, int i, int j)
        {
            T tmp = values[i];
            values[i] = values[j];
            values[j] = tmp;
        }
    }

    //interface ICompatible<T>
    //{
    //    public static T operator >(T t1, T t2);
    //}
}
