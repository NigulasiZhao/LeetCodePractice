using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeetCodePractice.Controllers
{
    public class LeetCode1 : Controller
    {
        /// <summary>
        /// 给定一个整数数组 nums 和一个目标值 target，请你在该数组中找出和为目标值的那 两个 整数，并返回他们的数组下标。你可以假设每种输入只会对应一个答案。但是，数组中同一个元素不能使用两遍。
        /// </summary>
        [HttpGet]
        public int[] SumTwoNumbers(int[] nums, int target)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {

                    if (nums[i] + nums[j] == target)
                    {
                        return new int[] { i, j };
                    }

                }
            }
            return new int[] { 0, 0 };
        }

        [HttpGet]
        public int[] SumTwoNumbersHash(int[] nums, int target)
        {
            Dictionary<int, int> Dic = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
            {
                if (Dic.ContainsKey(target - nums[i]) /*&& Dic[target - nums[i]] != i*/)
                {
                    return new int[] { i, Dic[target - nums[i]] };
                }
                if (!Dic.ContainsKey(nums[i]))
                {
                    Dic.Add(nums[i], i);
                }
            }
            return new int[] { 0, 0 };
        }
    }
}
