using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LeetCodePractice.Model
{
    /// <summary>
    /// 参数数据类型
    /// </summary>
    public class ParameterInfo
    {
        /// <summary>
        /// 参数名称 :如果DataType 为 condition 条件时，则参数名称可以为空
        /// </summary>
        [DataMember]
        public string ParName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        [DataMember]
        public string ParValue { get; set; }
        /// <summary>
        /// 参数数据类型 string,number,bool,condition
        /// </summary>
        [DataMember]
        public string DataType { get; set; }

        /// <summary>
        /// 连接接类型 and 或者 or 
        /// </summary>
        [DataMember]
        public string LinkType { get; set; }
    }
}
