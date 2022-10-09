using System.Collections.Generic;

namespace LeetCodeCSharp
{
    public class LeetCode_46
    {
        public IList<IList<int>> Permute(int[] nums)
        {
            if (nums == null || nums.Length == 0)
            {
                return new List<IList<int>>();
            }

            var numsList = new List<int>(nums);
            var ans = new List<IList<int>>();

            BacktrackPermute(numsList, 0, ans);

            return ans;
        }

        public void BacktrackPermute(IList<int> nums, int idx, IList<IList<int>> ans)
        {
            if (idx == nums.Count)
            {
                ans.Add(new List<int>(nums));
            }
            else
            {
                for (int i = idx; i < nums.Count; ++i)
                {
                    Swap(nums, i, idx);
                    BacktrackPermute(nums, idx + 1, ans);
                    Swap(nums, i, idx);
                }
            }
        }

        public void Swap<T>(IList<T> list, int a, int b)
        {
            T temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
    }
}