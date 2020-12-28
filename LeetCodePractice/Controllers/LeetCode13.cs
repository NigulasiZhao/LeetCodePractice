using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeetCodePractice.Controllers
{
    /// <summary>
    /// 罗马数字转整数
    /// </summary>
    public class LeetCode13 : Controller
    {
        /// <summary>
        /// 罗马数字包含以下七种字符: I， V， X， L，C，D 和 M。
        /// 字符          数值
        ///  I             1
        ///  V             5
        ///  X             10
        ///  L             50
        ///  C             100
        ///  D             500
        ///  M             1000
        ///  例如， 罗马数字 2 写做 II ，即为两个并列的 1。12 写做 XII ，即为 X + II 。 27 写做  XXVII, 即为 XX + V + II 。
        ///通常情况下，罗马数字中小的数字在大的数字的右边。但也存在特例，例如 4 不写做 IIII，而是 IV。数字 1 在数字 5 的左边，所表示的数等于大数 5 减小数 1 得到的数值 4 。同样地，数字 9 表示为 IX。这个特殊的规则只适用于以下六种情况：
        ///I 可以放在 V(5) 和 X(10) 的左边，来表示 4 和 9。
        ///X 可以放在 L(50) 和 C(100) 的左边，来表示 40 和 90。 
        ///C 可以放在 D(500) 和 M(1000) 的左边，来表示 400 和 900。
        ///给定一个罗马数字，将其转换成整数。输入确保在 1 到 3999 的范围内。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpGet]
        public int RomanToInt(string s)
        {
            s = s.Replace("IV", "E");
            s = s.Replace("IX", "F");
            s = s.Replace("XL", "G");
            s = s.Replace("XC", "H");
            s = s.Replace("CD", "J");
            s = s.Replace("CM", "K");
            int sum = 0;
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case 'M':
                        sum += 1000;
                        break;
                    case 'D':
                        sum += 500;
                        break;
                    case 'C':
                        sum += 100;
                        break;
                    case 'L':
                        sum += 50;
                        break;
                    case 'X':
                        sum += 10;
                        break;
                    case 'V':
                        sum += 5;
                        break;
                    case 'I':
                        sum += 1;
                        break;
                    case 'E':
                        sum += 4;
                        break;
                    case 'F':
                        sum += 9;
                        break;
                    case 'G':
                        sum += 40;
                        break;
                    case 'H':
                        sum += 90;
                        break;
                    case 'J':
                        sum += 400;
                        break;
                    case 'K':
                        sum += 900;
                        break;
                    default:
                        break;
                }
            }
            return sum;
        }

        [HttpGet]
        public int RomanToIntTest1(string s)
        {
            int sum = 0;
            int fnum = GetNumber(s[0]);
            for (int i = 1; i < s.Length; i++)
            {
                int snum = GetNumber(s[i]);
                if (fnum >= snum)
                {
                    sum += fnum;
                }
                else
                {
                    sum -= fnum;
                }
                fnum = snum;
            }
            sum += fnum;
            return sum;
        }
        [HttpGet]
        public int GetNumber(char a)
        {
            switch (a)
            {
                case 'M':
                    return 1000;
                    break;
                case 'D':
                    return 500;
                    break;
                case 'C':
                    return 100;
                    break;
                case 'L':
                    return 50;
                    break;
                case 'X':
                    return 10;
                    break;
                case 'V':
                    return 5;
                    break;
                case 'I':
                    return 1;
                    break;
                default:
                    return 0;
                    break;
            }
        }
    }
}
