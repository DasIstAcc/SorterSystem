using System.Drawing;
using System.Reflection;
using IronXL;

namespace SortAnalysis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] sizes = { 60, 600, 6000, 60000, 600000 };

            Console.WriteLine("Enter a maximum execution time for algorithms (in milliseconds):");
            string? str = Console.ReadLine();
            int delay;

            if (!int.TryParse(str, out delay))
            {
                delay = 5000;
            }

            foreach (var size in sizes)
            {
                SortingComparer<byte> scByte = new SortingComparer<byte>(delay);
                scByte.Aggregate(FillWithRandomBytes(size));

                SortingComparer<int> scInt = new SortingComparer<int>(delay);
                scInt.Aggregate(FillWithRandomInts(size));

                SortingComparer<double> scDouble = new SortingComparer<double>(delay);
                scDouble.Aggregate(FillWithRandomDoubless(size));

                SortingComparer<string> scString = new SortingComparer<string>(delay);
                scString.Aggregate(FillWithRandomStrings(size));

                SortingComparer<DateTime> scDates = new SortingComparer<DateTime>(delay);
                scDates.Aggregate(FillWithRandomDates(size));
            }
        }

        private static int[] FillWithRandomInts(int length) 
        {
            int[] ints = new int[length];

            Random rand = new Random();

            for (int i = 0; i < length; i++)
            {
                ints[i] = rand.Next(100);
            }

            return ints;
        }

        private static byte[] FillWithRandomBytes(int length)
        {
            byte[] bytes = new byte[length];

            Random rand = new Random();

            rand.NextBytes(bytes);
            
            return bytes;
        }

        private static double[] FillWithRandomDoubless(int length)
        {
            double[] doubles = new double[length];

            Random rand = new Random();

            for (int i = 0; i < length; i++)
            {
                doubles[i] = rand.NextDouble();
            }

            return doubles;
        }

        private static string[] FillWithRandomStrings(int lenght)
        {
            string[] strings = new string[lenght];

            Random rand = new Random();

            string[] x_1 = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
            string[] x_2 = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string[] x_3 = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string[] x_4 = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string[] x_5 = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            for (int i = 0; i < lenght; i++)
            {
                strings[i] = x_1[rand.Next(26)] + x_2[rand.Next(26)] + x_3[rand.Next(26)] + x_4[rand.Next(26)] + x_5[rand.Next(26)];
            }

            return strings;
        }

        private static DateTime[] FillWithRandomDates(int lenght)
        {
            DateTime[] dates = new DateTime[lenght];

            Random rand = new Random();

            for (int i = 0;i < lenght; i++)
            {
                dates[i] = new DateTime(rand.Next(rand.Next()));
            }

            return dates;
        }

        
    }

    class SortingComparer<T> where T : IComparable
    {
        T[] data;

        int thread_delay = 5000;

        public SortingComparer(int thread_delay) : this()
        {
            this.thread_delay = thread_delay;
        }

        public SortingComparer()
        {
            data = new T[10];
        }


        public void ApplyNoise(int amount)
        {
            Random rand = new Random();

            for (int i = 0; i < amount; i++)
            {
                int index_1 = rand.Next(data.Length);
                int index_2 = rand.Next(data.Length);
                var temp = data[index_1];
                data[index_1] = data[index_2];
                data[index_2] = temp;
            }
        }

        public void RefillWithSimilar()
        {
            for (int i = 0; i < data.Length * 0.8f; i++)
            {
                data[i] = data[0];
            }
        }

        public void Aggregate(T[] data)
        {
            this.data = data;

            T[] temp = new T[data.Length];
            data.CopyTo(temp, 0);

            SorterSystem<T> sorterSystem = new SorterSystem<T>(data, thread_delay);

            WorkBook book = WorkBook.Load(@"D:\For_Study\Razrabotka i analiz\Семестровая\Данные эксперимента.xlsx");

            string name = data.Length + "L-" + typeof(T);
            book.WorkSheets.Remove(book.GetWorkSheet(name));
            WorkSheet sheet = book.CreateWorkSheet(name);

            ApplyAllSorts(sorterSystem, sheet, 1);
            Array.Sort(data);
            ApplyNoise(data.Length / 20);
            Console.WriteLine("Applied minor noise. Repeating process...");
            ApplyAllSorts(sorterSystem, sheet, 5);
            temp.CopyTo(data, 0);
            RefillWithSimilar();
            Console.WriteLine("Refilled with similar values. Repeating process...");
            ApplyAllSorts(sorterSystem, sheet, 10);
            temp.CopyTo(data, 0);


            book.Save();
            book.Close();
        }

        

        private void ApplyAllSorts(SorterSystem<T> sorterSystem, WorkSheet sheet, int row)
        {
            BubbleSort<T> bblSorter = new BubbleSort<T>();
            ShakeSort<T> shkSorter = new ShakeSort<T>();
            ChooseSort<T> chsSorter = new ChooseSort<T>();
            MergeSort<T> mrgSorter = new MergeSort<T>();
            QuickSort<T> qckSorter = new QuickSort<T>();

            Console.WriteLine("Sorting comparsion begin on " + data.Length + " size array of " + typeof(T));
            Console.WriteLine("  Bubble sort:");

            sheet["A" + row].Value = "Bubble";
            int counter = row + 1;

            for (int i = 0; i < 3; i++)
            {
                sheet["A" + counter].Value = sorterSystem.StartSorting(bblSorter.Sort);
                counter++;
            }

            Console.WriteLine("  Shake sort:");

            sheet["B1"].Value = "Shake";
            counter = row + 1;

            for (int i = 0; i < 3; i++)
            {
                sheet["B" + counter].Value = sorterSystem.StartSorting(shkSorter.Sort);
                counter++;
            }

            Console.WriteLine("  Choose sort:");

            sheet["C1"].Value = "Choose";
            counter = row + 1;

            for (int i = 0; i < 3; i++)
            {
                sheet["C" + counter].Value = sorterSystem.StartSorting(chsSorter.Sort);
                counter++;
            }
            
            Console.WriteLine("  Merge sort:");

            sheet["D1"].Value = "Merge";
            counter = row + 1;

            for (int i = 0; i < 3; i++)
            {
                sheet["D" + counter].Value = sorterSystem.StartSorting(mrgSorter.Sort);
                counter++;
            }

            Console.WriteLine("  Quick sort:");

            sheet["E1"].Value = "Quick";
            counter = row + 1;

            for (int i = 0; i < 3; i++)
            {
                sheet["E" + counter].Value = sorterSystem.StartSorting(qckSorter.Sort);
                counter++;
            }

            Console.WriteLine("  CSharp sort:");

            sheet["F1"].Value = "CSharp";
            counter = row + 1;

            for (int i = 0; i < 3; i++)
            {
                sheet["F" + counter].Value = sorterSystem.StartSorting(Array.Sort);
                counter++;
            }
        }
    }
}