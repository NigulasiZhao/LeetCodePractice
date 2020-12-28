using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeetCodePractice.Controllers
{
    public class LeetCode188 : Controller
    {
        [HttpGet]
        public int MaxProfit(int k, int[] prices)
        {
            //int result = 0;
            //List<int> list = prices.ToList();
            //for (int i = 0; i < k / 2; i++)
            //{
            //    if (list.Count < 2)
            //    {
            //        break;
            //    }
            //    int MinValue = int.MaxValue;
            //    int MaxValue = list.Max();
            //    int flag = list.IndexOf(MaxValue) + 1;
            //    for (int j = flag; j < list.Count; j++)
            //    {
            //        if (MinValue > list[j])
            //        {
            //            MinValue = list[j];
            //        }
            //    }
            //    list.Remove(MinValue);
            //    list.Remove(MaxValue);
            //    result += MaxValue - MinValue;
            //}
            //return result;


            int result = 0;
            List<int> list = prices.ToList();
            for (int i = 0; i < k / 2; i++)
            {
                if (list.Count < 2)
                {
                    break;
                }
                int MinValue = list.Min();
                int flag = list.IndexOf(MinValue) + 1;
                while (flag == list.Count())
                {
                    MinValue = list.Where(e => e != MinValue).Min();
                }
                int flag1 = list.IndexOf(MinValue) + 1;
                int MaxValue = 0;

                for (int j = flag1; j < list.Count; j++)
                {
                    if (MaxValue < list[j])
                    {
                        MaxValue = list[j];
                    }
                }
                list.Remove(MinValue);
                list.Remove(MaxValue);
                result += MaxValue - MinValue;
            }
            return result;
        }
    }
}
