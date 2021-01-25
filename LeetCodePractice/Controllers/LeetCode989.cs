using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 数组形式的整数加法
    /// 
    /// </summary>
    public class LeetCode989 : Controller
    {
        /// <summary>
        /// 对于非负整数 X 而言，X 的数组形式是每位数字按从左到右的顺序形成的数组。例如，如果 X = 1231，那么其数组形式为 [1,2,3,1]。
        /// 给定非负整数 X 的数组形式 A，返回整数 X+K 的数组形式。
        /// 示例 1：
        /// 输入：A = [1,2,0,0], K = 34
        /// 输出：[1,2,3,4]
        /// 解释：1200 + 34 = 1234
        /// 示例 2：
        /// 输入：A = [2,7,4], K = 181
        /// 输出：[4,5,5]
        /// 解释：274 + 181 = 455
        /// 示例 3：
        /// 输入：A = [2,1,5], K = 806
        /// 输出：[1,0,2,1]
        /// 解释：215 + 806 = 1021
        /// 示例 4：
        /// 输入：A = [9,9,9,9,9,9,9,9,9,9], K = 1
        /// 输出：[1,0,0,0,0,0,0,0,0,0,0]
        /// 解释：9999999999 + 1 = 10000000000
        /// </summary>
        /// <param name="A"></param>
        /// <param name="K"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<int> AddToArrayForm(int[] A, int K)
        {
            List<int> result = new List<int>();
            int length = A.Length;
            for (int i = length - 1; i >= 0; --i)
            {
                int sum = A[i] + K % 10;
                K /= 10;
                if (sum >= 10)
                {
                    K++;
                    sum -= 10;
                }
                result.Add(sum);
            }
            for (; K > 0; K /= 10)
            {
                result.Add(K % 10);
            }
            result.Reverse();
            return result;
        }
    }
}
