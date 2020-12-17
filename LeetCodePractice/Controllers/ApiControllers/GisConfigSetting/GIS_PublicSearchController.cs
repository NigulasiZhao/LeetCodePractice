using System.Collections.Generic;
using System.Linq;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.IDAL.GisConfigSetting;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.GisConfigSetting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GISWaterSupplyAndSewageServer.Controllers.ApiControllers.GisConfigSetting
{
    /// <summary>
    /// 搜素配置管理
    /// </summary>
    public class GIS_PublicSearchController : Controller
    {
        private readonly IGIS_PublicSearchDAL _PublicSearchDAL;

        public GIS_PublicSearchController(IGIS_PublicSearchDAL LayerTypeDAL)
        {
            _PublicSearchDAL = LayerTypeDAL;
        }

        /// <summary>
        /// 获取搜索配置
        /// </summary>
        /// <param name="ParInfo">查询列表[{ParName:"字段名",ParValue:"字段值",DataType:"数据类型：string|number|bool|condition",LinkType:and|or}]</param>
        /// <param name="sort"></param>
        /// <param name="ordering"></param>
        /// <param name="num"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageEntity Get(string ParInfo, string sort = "CreatedTime", string ordering = "desc", int num = 20, int page = 1)
        {
            List<ParameterInfo> list = JsonConvert.DeserializeObject<List<ParameterInfo>>(ParInfo);
            SqlCondition sqlWhere = new SqlCondition();
            string sqlCondition = sqlWhere.getParInfo(list);
            var result = _PublicSearchDAL.GetList(list, sort, ordering, num, page, sqlCondition);
            return result;
        }

        /// <summary>
        /// 新增搜索配置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageEntity Post([FromBody] GIS_PublicSearch value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            return _PublicSearchDAL.Add(value);
        }

        /// <summary>
        /// 修改搜索配置
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public MessageEntity Put(string ID, [FromBody] GIS_PublicSearch value)
        {
            if (value == null)
            {
                return MessageEntityTool.GetMessage(ErrorType.FieldError);
            }
            value.ID = ID;
            return _PublicSearchDAL.Update(value);
        }

        /// <summary>
        /// 删除搜索数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public MessageEntity Delete(string ID)
        {
            //递归删除所有子项 及子项包含的子表
            //先删除父元素 及子表数据
            var modeInfo = _PublicSearchDAL.GetInfo(ID);
            _PublicSearchDAL.Delete(modeInfo);
            //获取所有搜索数据
            List<GIS_PublicSearch> pSearchlist = _PublicSearchDAL.GetPublicSearchInfo();
            return getDeleteInfo(pSearchlist, ID);

        }
        [HttpGet]
        private MessageEntity getDeleteInfo(List<GIS_PublicSearch> pSearchlist, string pID)
        {
            var data = pSearchlist.Where(x => x.PID == pID);//获取数据
            var count = 0;
            foreach (GIS_PublicSearch item in data)
            {
                count++;
                DeleteInfo(pSearchlist, item);
            }
            return MessageEntityTool.GetMessage(count);
        }
        [HttpDelete]
        private MessageEntity DeleteInfo(List<GIS_PublicSearch> pSearchlist, GIS_PublicSearch model)
        {
            var modeInfo = _PublicSearchDAL.GetInfo(model.ID);
            MessageEntity mess = _PublicSearchDAL.Delete(modeInfo);
            if (mess.Data.Rows >= 0)
            {
                getDeleteInfo(pSearchlist, model.ID);
            }
            return MessageEntityTool.GetMessage(mess.Data.Rows);
        }
    }
}
