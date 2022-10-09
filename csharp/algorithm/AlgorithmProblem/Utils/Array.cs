using System.Runtime.InteropServices;

namespace AlgorithmProblem.Utils
{
    public class Array
    {
        public static void Swap(int[] arr, int i, int j)
        {
            int tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
    }
}