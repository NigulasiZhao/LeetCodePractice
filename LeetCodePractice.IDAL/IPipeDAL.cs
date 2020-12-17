using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using System;
using System.Collections.Generic;
using System.Data;

namespace GISWaterSupplyAndSewageServer.IDAL
{
    public interface IPipeDAL : IDependency
    {
        /// <summary>
        /// 获取管网/设备分页查询
        /// </summary>
        /// <param name="layer">图层</param>
        /// <param name="Installation_address">地址</param>
        /// <param name="material_science">材料</param>
        /// <param name="minCaliber">最小口径</param>
        /// <param name="maxCaliber">最大口径</param>
        /// <param name="startCompletion_date">起始安装日期</param>
        /// <param name="endCompletion_date">结束安装日期</param>
        /// <param name="sort">排序字段</param>
        /// <param name="ordering">asc/desc</param>
        /// <param name="num">每页数量</param>
        /// <param name="page">第几页</param>
        /// <returns></returns>
        List<PointTable> GetPipeAndPointByPage(string layer, string installation_address, string material_science, string minCaliber, string maxCaliber, DateTime? startCompletion_date, DateTime? endCompletion_date, string sort, string ordering, int num, int page, out string errMessge, out MessageEntity result);

        List<Object> GetLayers(out string errMessge);

        List<string> GetMaterial_science(out string errMessge);

        List<string> GetCaliber(out string errMessge);

        List<string> Getinstallation_addresses(out string errMessge);

        List<dynamic> GetPipeAndPointStatistics(string layer, string installation_address, string material_science, string caliber, DateTime? startCompletion_date, DateTime? endCompletion_date, string sort, string ordering, string[] groupFields, out string errMessge, out MessageEntity result);

        Dictionary<string, List<dynamic>> GetRealatedValveAndPipeByPipeId(string[] pipeIds, out string errorMsg);

        string GetCurveHeng(DataSet sender);

        Pipe Get(string latitude, string longitude, out string errMessge);
        void CreateMeterRelatedData(List<dynamic> pipeRelated,out List<object> userinfo,out List<object> meterInfo, out List<object> communityInfo, out List<object> buildingInfo, IDbConnection kMconn);
    }
}
