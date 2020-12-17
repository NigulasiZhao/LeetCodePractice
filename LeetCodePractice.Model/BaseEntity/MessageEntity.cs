using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.BaseEntity
{
    /// <summary>
    /// 操作返回的消息类
    /// </summary>
    public class MessageEntity
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 返回的消息
        /// </summary>
        public string Msg { get; set; }

        public string ExceptionMsg { set; get; }


        public DataInfo Data { set; get; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Flag { get; set; }

        public override string ToString()
        {
            return string.Format("执行结果：{0} 影响行数：{1} Msg:{2}", Flag, Data.Rows, Msg);
        }
        public int ErrorType { set; get; }
        public string ErrorTypeDesc { set; get; }


    }
    public class DataInfo
    {
        /// <summary>
        /// 行数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int TotalRows { get; set; }

        public object Result { get; set; }
    }




    /// <summary>
    /// 消息组装类
    /// </summary>
    public class MessageEntityTool
    {
        /// <summary>
        /// 组合消息类信息
        /// </summary>
        /// <param name="str">错误信息</param>
        /// <param name="isSuccess">操作是否成功 </param>
        /// <returns>消息类</returns>
        public static MessageEntity GetMessage(int rows, object obj = null, bool flag = true, string msg = "完成", int totalRows = 0)
        {
            var DataInfo = new DataInfo { Rows = rows, Result = obj, TotalRows = totalRows };
            var message = new MessageEntity { Msg = msg, Flag = flag, Title = "提示信息", Data = DataInfo };
            message.ErrorType = (int)ErrorType.Success;
            message.ErrorTypeDesc = ErrorType.Success.ToString();
            if (totalRows == 0)
                message.Data.TotalRows = rows;
            return message;
        }

        /// <summary>
        /// 获取默认的消息信息
        /// </summary>
        public static MessageEntity GetMessage(ErrorType errorType, string exceptionMsg = null, string msg = null, string title = null, bool flag = false)
        {

            #region 设置默认的消息信息

            var message = new MessageEntity();
            message.ErrorType = (int)errorType;
            message.ErrorTypeDesc = errorType.ToString();
            if (msg == null)
            {
                switch (errorType)
                {
                    case ErrorType.SqlError:
                        message.Flag = false;
                        message.Msg = "系统内部发生错误，请联系管理员！";
                        message.Title = "提示信息";
                        break;
                    case ErrorType.SystemError:
                        message.Flag = false;
                        message.Msg = "系统发生严重错误，请联系管理员！";
                        message.Title = "提示信息";
                        break;
                    case ErrorType.OprationError:
                        message.Flag = false;
                        message.Msg = "操作错误，请重试！";
                        message.Title = "提示信息";
                        break;
                    case ErrorType.NotUnique:
                        message.Flag = false;
                        message.Msg = "字段值重复，请重新输入!";
                        message.Title = "提示信息";
                        break;
                    case ErrorType.NoLogin:
                        message.Flag = false;
                        message.Msg = "登陆已经过期,请重新登陆！";
                        message.Title = "提示信息";
                        break;
                    case ErrorType.NoAuthority:
                        message.Flag = false;
                        message.Msg = "您没有权限访问该功能请联系管理员！";
                        message.Title = "提示信息";
                        break;
                    case ErrorType.OutOfTime:
                        message.Flag = false;
                        message.Msg = "登陆过期！";
                        message.Title = "提示信息";
                        break;
                    case ErrorType.NotAvilebalToken:
                        message.Flag = false;
                        message.Msg = "Token无效！";
                        message.Title = "提示信息";
                        break;
                    case ErrorType.FieldError:
                        message.Flag = false;
                        message.Msg = "参数有误！";
                        message.Title = "提示信息";
                        break;
                }
            }
            else
            {
                message.Msg = msg;
            }
            if (exceptionMsg != null)
                message.ExceptionMsg = exceptionMsg;
            if (string.IsNullOrEmpty(message.Msg))
                message.Msg = exceptionMsg;
            if (title != null)
                message.Title = title;
            message.Flag = flag;

            message.Data = new DataInfo() { Rows = 0, TotalRows = 0, Result = null };
            return message;

            #endregion
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrorType
    {
        SqlError,
        SystemError,
        OprationError,
        Success,
        NotUnique,
        NoLogin,
        NoAuthority,
        FieldError,
        OutOfTime,
        NotAvilebalToken
    }
}
