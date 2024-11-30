using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortAnalysis
{
    internal class SorterSystem<T> where T : IComparable
    {
        T[] data_array;
        T[] temp_array;

        int thread_delay = 5000;

        public SorterSystem(T[] data, int thread_delay) : this (data)
        {
            this.thread_delay = thread_delay;
        }

        public SorterSystem(T[] data)
        {
            data_array = new T[data.Length];
            temp_array = new T[data.Length];
            data.CopyTo(data_array, 0);
            data.CopyTo(temp_array, 0);
        }

        public double StartSorting(Action<T[]> sort)
        {
            Stopwatch timer = new Stopwatch();
            data_array.CopyTo(temp_array, 0);
            timer.Start();
            var thread = new Thread(() => sort(temp_array));
            thread.Start();
            if (!thread.Join(thread_delay))
            {
                Console.WriteLine("Sort type is too ineffective (took over " + thread_delay / 1000 + " sec) aborting...");
                return -1;
            }
            timer.Stop();

            double timeElapsed = (double)timer.ElapsedTicks / Stopwatch.Frequency;
            Console.WriteLine("Sorting is over. Sorting time: " + timeElapsed + " seconds");

            return timeElapsed;
        }

        public void GetData(T[] container)
        {
            temp_array.CopyTo(container, 0);
        }

        public void PrintData()
        {
            foreach(var e in temp_array)
            {
                Console.Write(e.ToString() + " ");
            }
            Console.WriteLine();
        }
    }


    internal class BubbleSort<T> : SorterClass<T> where T : IComparable
    {
        public override void Sort(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i; j < array.Length; j++)
                {
                    if (array[i].CompareTo(array[j]) > 0)
                    {
                        swap(array, i, j);
                    }
                }
            }
        }
    }

    internal class ShakeSort<T> : SorterClass<T> where T : IComparable
    {
        public override void Sort(T[] array)
        {
            int i, j, k, m = array.Length;

            for (i = 0; i < m;)
            {
                for (j = i + 1; j < m; j++)
                {
                    if (array[j].CompareTo(array[j - 1]) < 0)
                    {
                        swap(array, j, j - 1);
                    }
                }
                m--;
                for (k = m - 1; k > i; k--)
                {
                    if (array[k].CompareTo(array[k - 1]) < 0)
                    {
                        swap(array, k, k - 1);
                    }
                }
                i++;
            }
        }
    }

    internal class ChooseSort<T> : SorterClass<T> where T : IComparable
    {
        public override void Sort(T[] array)
        {
            int min;

            for (int i = 0; i < array.Length; i++)
            {
                min = i;

                for (int j = i + 1; j < array.Length; j++)
                {
                    min = (array[j].CompareTo(array[min]) < 0) ? j : min;
                }

                if (i != min)
                {
                    swap(array, i, min);
                }
            }
        }
    }

    internal class MergeSort<T> : SorterClass<T> where T : IComparable
    {
        public override void Sort(T[] array)
        {
            SortArray(array, 0, array.Length - 1);
        }

        private T[] SortArray(T[] arr, int left, int right)
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;

                SortArray(arr, left, middle);
                SortArray(arr, middle + 1, right);

                MergeArray(arr, left, middle, right);
            }

            return arr;
        }

        private void MergeArray(T[] arr, int left, int middle, int right)
        {
            var leftArrLenght = middle - left + 1;
            var rightArrLength = right - middle;

            var leftTempArr = new T[leftArrLenght];
            var rightTempArr = new T[rightArrLength];

            int i, j;

            for (i = 0; i < leftArrLenght; i++) leftTempArr[i] = arr[left + i];
            for (j = 0; j < rightArrLength; j++) rightTempArr[j] = arr[middle + 1 + j];

            i = 0; j = 0;
            int k = left;
            
            while (i < leftArrLenght && j < rightArrLength)
            {
                if (leftTempArr[i].CompareTo(rightTempArr[j]) <= 0)
                {
                    arr[k++] = leftTempArr[i++];
                }
                else
                {
                    arr[k++] = rightTempArr[j++];
                }
            }

            while (i < leftArrLenght)
            {
                arr[k++] = leftTempArr[i++];
            }
            while (j < rightArrLength)
            {
                arr[k++] = rightTempArr[j++];
            }
        }

        private void merge(T[] arr, int left, int right)
        {
            if (left == right) return;

            if (right - left == 1)
            {
                if (arr[right].CompareTo(arr[left]) < 0)
                {
                    swap(arr, left, right);
                    return;
                }
            }

            int med = (right + 1) / 2;
            
            merge(arr, left, med);
            merge(arr, med + 1, right);


        }
    }

    internal class QuickSort<T> : SorterClass<T> where T : IComparable
    {
        public override void Sort(T[] array)
        {
            QSort(array, 0, array.Length - 1);
        }

        T[] QSort(T[] arr, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
            {
                return arr;
            }

            var pointIndex = Partition(arr, minIndex, maxIndex);
            QSort(arr, minIndex, pointIndex - 1);
            QSort(arr, pointIndex + 1, maxIndex);

            return arr;
        }

        int Partition(T[] arr, int minIndex, int maxIndex)
        {
            var point = minIndex - 1;
            for (int i = minIndex; i < maxIndex; i++)
            {
                if (arr[i].CompareTo(arr[maxIndex]) < 0)
                {
                    point++;
                    swap(arr, point, i);
                }
            }

            point++;
            swap(arr, point, maxIndex);
            return point;
        }
    }
}
