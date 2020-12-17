using Dapper;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.Plan;
using GISWaterSupplyAndSewageServer.Model.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GISWaterSupplyAndSewageServer.CommonTools.ChidrenTree;

namespace GISWaterSupplyAndSewageServer.OracleDAL.Plan
{
    public class Ins_Plan_TypeDAL : IIns_Plan_TypeDAL
    {
        /// <summary>
        /// 添加巡检类型
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Add(Ins_Plan_Type model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                StringBuilder SqlInsertEqu = new StringBuilder();
                var rows = 0;
                var insertSql = DapperExtentions.MakeInsertSql(model);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql, model);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 删除巡检类型
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Delete(Ins_Plan_Type model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var excSql = DapperExtentions.MakeDeleteSql(model);
                if (string.IsNullOrEmpty(excSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(excSql, model);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 根据ID获取巡检类型
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Ins_Plan_Type GetInfo(string ID)
        {
            List<Ins_Plan_Type> _ListField = new List<Ins_Plan_Type>();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                _ListField = conn.Query<Ins_Plan_Type>("select * from Ins_Plan_Type t where Plan_Type_Id='" + ID + "'").ToList();
            }
            if (_ListField.Count > 0)
            {
                return _ListField[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获得巡检类型信息
        /// </summary>
        /// <param name="parInfo">参数参数信息</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">升序/降序</param>
        /// <param name="num">当前页</param>
        /// <param name="page">每页数据行数</param>
        /// <returns></returns>
        public MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition)
        {
            string sql = @"   select ipt.* from Ins_Plan_Type ipt 
                             " + sqlCondition;
            var ResultList = DapperExtentions.EntityForSqlToPager<Ins_Plan_Type>(sql, sort, ordering, num, page, out MessageEntity result, ConnectionFactory.DBConnNames.ORCL);
            return result;
        }

        public MessageEntity IsExistPlanType(Ins_Plan_Type model, int isAdd)
        {
            string errorMsg = "";
            string strWhere = "";
            if (!string.IsNullOrEmpty(model.Plan_Type_Name))
            {
                strWhere += $" and Plan_Type_Name ='{model.Plan_Type_Name}' ";

            }
            if (!string.IsNullOrEmpty(model.ParentTypeId))
            {
                strWhere += $" and ParentTypeId ='{model.ParentTypeId}' ";

            }
            //修改的时候判断是否重复，要排除自己
            if (isAdd == 0)
            {
                strWhere += $" and Plan_Type_Id <>'{model.Plan_Type_Id}'";
            }
            string query = $@" select Plan_Type_Id,Plan_Type_Name from Ins_Plan_Type where 1=1  {strWhere}";
            try
            {
                using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
                {
                    IList<Ins_Plan_Type> result = conn.Query<Ins_Plan_Type>(query).ToList();
                    return MessageEntityTool.GetMessage(1, result);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
            }
        }

        /// <summary>
        ///修改巡检类型信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public MessageEntity Update(Ins_Plan_Type model)
        {
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                var rows = 0;
                var insertSql = DapperExtentions.MakeUpdateSql(model);
                if (string.IsNullOrEmpty(insertSql))
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "请检查实体类");
                }
                try
                {
                    rows = conn.Execute(insertSql, model);
                    return MessageEntityTool.GetMessage(rows);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }

        }
        #region 获取巡检类型及其区域信息
        /// <summary>
        /// 获取巡检类型及明细信息
        /// </summary>
        /// <returns></returns>
        public MessageEntity GetTreeList()
        {
            List<TreeInsPlanType> list = new List<TreeInsPlanType>();
            ChidrenTree childtree = new ChidrenTree();
            string sql = @"   select ipt.* from Ins_Plan_Type ipt where Plan_Type_Name = '区域巡检' or Plan_Type_Name = '路线巡检'";
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                list = conn.Query<TreeInsPlanType>(sql).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Plan_Type_Name == "区域巡检")
                    {
                        List<Model.PlanningEngine.Plan.Ins_Range> AreaRangeList = new List<Model.PlanningEngine.Plan.Ins_Range>();
                        AreaRangeList = conn.Query<Model.PlanningEngine.Plan.Ins_Range>("SELECT * FROM Ins_Range WHERE TYPE = 1 and Range_parentid is not null").ToList();
                        list[i].Ins_RangeList = childtree.ConversionList(AreaRangeList, "00000000-0000-0000-0000-000000000000", "Range_id", "Range_parentid", "Range_name");
                    }
                    if (list[i].Plan_Type_Name == "路线巡检")
                    {
                        List<Model.PlanningEngine.Plan.Ins_Range> RouteRangeList = new List<Model.PlanningEngine.Plan.Ins_Range>();
                        RouteRangeList = conn.Query<Model.PlanningEngine.Plan.Ins_Range>("SELECT * FROM Ins_Range WHERE TYPE = 2 and Range_parentid is not null ").ToList();
                        list[i].Ins_RangeList = childtree.ConversionList(RouteRangeList, "00000000-0000-0000-0000-000000000000", "Range_id", "Range_parentid", "Range_name");
                    }
                }
            }
            return MessageEntityTool.GetMessage(list.Count, list);
        }
        public class TreeInsPlanType {
            /// <summary>
            /// 巡检类型主键
            /// </summary>
            public string Plan_Type_Id { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Plan_Type_Name { get; set; }
            /// <summary>
            /// 父节点
            /// </summary>
            public string ParentTypeId { get; set; }

            /// <summary>
            /// 操作人
            /// </summary>
            public string Operater { get; set; }

            /// <summary>
            /// 设备类型列表
            /// </summary>
            public List<TreeModel> Ins_RangeList { get; set; }
        }
        #endregion
    }
}
