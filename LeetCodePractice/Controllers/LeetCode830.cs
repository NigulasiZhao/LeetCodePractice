using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 较大分组的位置
    /// </summary>
    public class LeetCode830 : Controller
    {
        /// <summary>
        /// 在一个由小写字母构成的字符串 s 中，包含由一些连续的相同字符所构成的分组。例如，
        /// 在字符串 s = "abbxxxxzyy" 中，就含有 "a", "bb", "xxxx", "z" 和 "yy" 这样的一些分组。分组可以用区间[start, end] 表示，其中 start 和 end 分别表示该分组的起始和终止位置的下标。
        /// 上例中的 "xxxx" 分组用区间表示为[3, 6] 。我们称所有包含大于或等于三个连续字符的分组为 较大分组 。
        /// 找到每一个 较大分组 的区间，按起始位置下标递增顺序排序后，返回结果。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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
