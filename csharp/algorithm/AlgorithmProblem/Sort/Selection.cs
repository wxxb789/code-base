using AlgorithmProblem.Utils;

namespace AlgorithmProblem.Sort
{
    public class Selection
    {
        public static void SelectionSort(int[] arr)
        {
            if (arr == null || arr.Length < 2)
            {
                return;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < arr.Length; j++)
                {
                    minIdx = arr[j] < arr[minIdx] ? j : minIdx;
                }

                Array.Swap(arr, i, minIdx);
            }
        }
    }
}