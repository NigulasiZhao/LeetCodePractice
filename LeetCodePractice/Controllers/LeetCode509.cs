﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 斐波那契数
    /// </summary>
    public class LeetCode509 : Controller
    {
        /// <summary>
        /// 斐波那契数，通常用 F(n) 表示，形成的序列称为 斐波那契数列 。该数列由 0 和 1 开始，后面的每一项数字都是前面两项数字的和。也就是
        /// F(0) = 0，F(1) = 1
        /// F(n) = F(n - 1) + F(n - 2)，其中 n > 1
        /// 给你 n ，请计算 F(n) 。
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        [HttpGet]
        public int Fib(int n)
        {
            int first = 0;
            int second = 1;
            Dictionary<int, int> dic = new Dictionary<int, int>();
            if (n < 2)
            {
                return n;
            }
            else
            {
                for (int i = 2; i < n; i++)
                {
                    int temp = first + second;
                    first = second;
                    second = temp;
                }
                return first + second;
            }
        }
    }
}