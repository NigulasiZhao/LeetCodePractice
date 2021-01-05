using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LeetCodePractice.Controllers
{
    public class LeetCode830 : Controller
    {
        [HttpGet]
        public IList<IList<int>> LargeGroupPositions(string s)
        {
            //IList<IList<int>> groupList = new List<IList<int>>();
            //for (int i = 0; i <= s.Length - 3;) //顺序遍历，-3因为大于3个的才算较大分组。
            //{
            //    int j = i; //从当前索引位置开始，+1比对直到不同值时跳出。
            //    while (j < s.Length && s[j] == s[i])
            //    {
            //        j++;
            //    }
            //    if (j - i >= 3)//连续相同的最后一个索引减去当前索引是否满足较大分组
            //    {
            //        groupList.Add(new List<int> { i, j - 1 });
            //    }
            //    i = j;//移动索引到下一个字母
            //}
            //return groupList;
            IList<IList<int>> ret = new List<IList<int>>();
            int n = s.Length;
            int num = 1;
            for (int i = 0; i < n; i++)
            {
                if (i == n - 1 || s[i] != s[i + 1])
                {
                    if (num >= 3)
                    {
                        ret.Add(new List<int> { i - num + 1, i });
                    }
                    num = 1;
                }
                else
                {
                    num++;
                }
            }
            return ret;
        }
    }
}
