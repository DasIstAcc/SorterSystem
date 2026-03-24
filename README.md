# SorterSystem
 A simple sorting system.

 Allows for indirect usage of sorting algorithms.
 You can add any sorting algorithm in a method, put it in a new class, derived from 'SorterClass' 
 and you can use it in the main program to test this sorting algorithm capabilities by comparing it to another ones.


<span style="color: green;">Here</span> are some already implemented in the SorterSystem.cs sorting algorythms and their usage in main Program.cs
```
BubbleSort<T> bblSorter = new BubbleSort<T>();
ShakeSort<T> shkSorter = new ShakeSort<T>();
ChooseSort<T> chsSorter = new ChooseSort<T>();
MergeSort<T> mrgSorter = new MergeSort<T>();
QuickSort<T> qckSorter = new QuickSort<T>();
```



<span style="color: green;">The example</span> of how you should implement new sorting algorithm:
```
internal class ShakeSort<T> : SorterClass<T> where T : IComparable
{
    public override void Sort(T[] array)
```
