using GISWaterSupplyAndSewageServer.IDAL;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.EquipmentFormsRelation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.EquipmentFormsRelation
{
   public interface IIns_Customize_FormlistDAL : IDependency
    {

        /// <summary>
        /// 获得表单列表
        /// </summary>
        /// <returns></returns>
        MessageEntity GetList(List<ParameterInfo> parInfo, string sort, string ordering, int num, int page, string sqlCondition);
    }
}