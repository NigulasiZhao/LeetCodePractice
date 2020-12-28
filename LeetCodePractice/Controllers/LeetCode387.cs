using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 给定一个字符串，找到它的第一个不重复的字符，并返回它的索引。如果不存在，则返回 -1。
    /// </summary>
    public class LeetCode387 : Controller
    {
        [HttpGet]
        public int FirstUniqChar(string s)
        {
            Dictionary<char, int> Dic = new Dictionary<char, int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (Dic.ContainsKey(s[i]))
                {
                    Dic[s[i]] += 1;
                }
                else
                {
                    Dic.Add(s[i], 1);
                }
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (Dic[s[i]] == 1)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
