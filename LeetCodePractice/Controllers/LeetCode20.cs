using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 20. 有效的括号
    /// </summary>
    public class LeetCode20 : Controller
    {
        /// <summary>
        /// 给定一个只包括 '('，')'，'{'，'}'，'['，']' 的字符串 s ，判断字符串是否有效。
        /// 有效字符串需满足：
        /// 左括号必须用相同类型的右括号闭合。
        /// 左括号必须以正确的顺序闭合。
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpGet]
        public bool IsValid(string s)
        {
            Stack<char> st = new Stack<char>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(' || s[i] == '{' || s[i] == '[')
                {
                    st.Push(s[i]);//如果是左括号，入栈
                }
                else if (st.Count == 0)//右括号且栈为空，false
                {
                    return false;
                }
                else if (s[i] == ')' && st.Pop() != '(' || s[i] == '}' && st.Pop() != '{' || s[i] == ']' && st.Pop() != '[')//右括号且栈顶左括号不匹配，false
                {
                    return false;
                }
            }
            return st.Count == 0;//如果栈为空，括号全部配对完，返回true
        }
    }
}
