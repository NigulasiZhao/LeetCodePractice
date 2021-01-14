using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LeetCode14 : Controller
    {
        /// <summary>
        /// 编写一个函数来查找字符串数组中的最长公共前缀。如果不存在公共前缀，返回空字符串 ""。
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public string LongestCommonPrefix(string[] strs)
        {
            if (strs.Length == 0 || strs == null)
            {
                return "";
            }
            string StrFlag = strs[0];
            for (int i = 1; i < strs.Length; i++)
            {
                StrFlag = OperateStr(StrFlag, strs[i]);
                if (StrFlag.Length == 0)
                {
                    break;
                }
            }
            return StrFlag;
        }

        public string OperateStr(string str1, string str2)
        {
            int length = Math.Min(str1.Length, str2.Length);
            int index = 0;
            while (index < length && str1[index] == str2[index])
            {
                index++;
            }
            return str1.Substring(0, index);
        }
    }
}
