using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 最后一块石头的重量
    /// </summary>
    public class Leetcode1046 : Controller
    {
        /// <summary>
        /// 有一堆石头，每块石头的重量都是正整数。
        ///每一回合，从中选出两块 最重的 石头，然后将它们一起粉碎。假设石头的重量分别为 x 和 y，且 x <= y。那么粉碎的可能结果如下：
        ///如果 x == y，那么两块石头都会被完全粉碎；
        ///如果 x != y，那么重量为 x 的石头将会完全粉碎，而重量为 y 的石头新重量为 y-x。
        ///最后，最多只会剩下一块石头。返回此石头的重量。如果没有石头剩下，就返回 0。
        /// </summary>
        /// <param name="stones"></param>
        /// <returns></returns>
        [HttpGet]
        public int LastStoneWeight(int[] stones)
        {
            List<int> list = stones.ToList();
            while (list.Count > 1)
            {
                int a = list.Max();
                list.Remove(a);
                int b = list.Max();
                list.Remove(b);
                if (a != b)
                {
                    list.Add(a - b);
                }
            }
            return list.Count > 0 ? list.FirstOrDefault() : 0;
            //if (stones.Length == 2)
            //{
            //    return Math.Abs(stones[0] - stones[1]);
            //}
            //if (stones.Length == 1)
            //{
            //    return stones[0];
            //}
            //Array.Sort(stones);
            //if (stones[stones.Length - 3] == 0)
            //{
            //    return stones[stones.Length - 1] - stones[stones.Length - 2];
            //}
            //stones[stones.Length - 1] = stones[stones.Length - 1] - stones[stones.Length - 2];
            //stones[stones.Length - 2] = 0;
            //return LastStoneWeight(stones);
        }
    }
}
