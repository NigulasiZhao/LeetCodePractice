using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using Newtonsoft.Json;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.EssentialFactor;
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;
using System.Data;
using System.Text;
using static GISWaterSupplyAndSewageServer.CommonTools.ChidrenTree;
using Microsoft.AspNetCore.Mvc;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.EssentialFactor
{
    /// <summary>
    /// 要素管理
    /// </summary>
    public class EssentialFactorController : Controller
    {
        private readonly IEssentialFactorDAL _essentialFactorDAL;

        public EssentialFactorController(IEssentialFactorDAL essentialFactorDAL)
        {
            _essentialFactorDAL = essentialFactorDAL;
        }

        /// <summary>
        /// 获取要素信息
        /// </summary>
        /// <param name="ParInfo">其他台账查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "ID", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _essentialFactorDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] GISWaterSupplyAndSewageServer.Model.EssentialFactor.GIS_EssentialFactor value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            if (value.PID == "" || value.PID == "0")
            {
                value.PID = Guid.Empty.ToString();
            }
            return _essentialFactorDAL.Add(value);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] GISWaterSupplyAndSewageServer.Model.EssentialFactor.GIS_EssentialFactor value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            return _essentialFactorDAL.Update(value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            return _essentialFactorDAL.Delete(new GISWaterSupplyAndSewageServer.Model.EssentialFactor.GIS_EssentialFactor { ID = ID });
        }

        /// <summary>
        /// 获取要素树形结构
        /// </summary>
        /// <param name="pID">父编号</param>
        /// <param name="eFType">要素类别 1：固定要素2：自定义要素</param>
        [HttpGet]
        public MessageEntity GetTree(string pID = "00000000-0000-0000-0000-000000000000", int eFType = 0)
        {
            ChidrenTree childtree = new ChidrenTree();
            List<GISWaterSupplyAndSewageServer.Model.EssentialFactor.GIS_EssentialFactor> list = _essentialFactorDAL.GetTree(pID, eFType);
            //List<TreeChildViewModel> treeViewModels = new List<TreeChildViewModel>();
            //treeViewModels = childtree.AddChildN(list, pID);
            List<TreeModel> treeViewModels = new List<TreeModel>();
            treeViewModels = childtree.ConversionList(list, pID, "ID", "PID", "EFName");
            return MessageEntityTool.GetMessage(treeViewModels.Count(), treeViewModels, true, "", treeViewModels.Count());
        }
    }
}
