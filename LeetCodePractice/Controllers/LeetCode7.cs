using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 给出一个 32 位的有符号整数，你需要将这个整数中每位上的数字进行反转。
    /// </summary>
    public class LeetCode7 : Controller
    {
        /// <summary>
        /// 给出一个 32 位的有符号整数，你需要将这个整数中每位上的数字进行反转。
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [HttpGet]
        public int Reverse(int x)
        {
            int result = 0;
            while (x != 0)
            {
                //tmp记录每次循环的最后一位数
                int tmp = x % 10;
                //处理下一次循环中x的值
                x /= 10;
                if (result > int.MaxValue / 10 || result == int.MaxValue / 10 && tmp > 7)
                    return 0;
                if (result < int.MinValue / 10 || result == int.MinValue / 10 && tmp < -8)
                    return 0;
                result = result * 10 + tmp;
            }
            return result;
        }
    }
}
