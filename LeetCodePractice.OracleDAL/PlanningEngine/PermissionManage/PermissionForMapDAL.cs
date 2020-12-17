using Dapper;
using Dapper.Oracle;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
using GISWaterSupplyAndSewageServer.CommonTools;
using GISWaterSupplyAndSewageServer.Database;
using GISWaterSupplyAndSewageServer.IDAL.PlanningEngine.PermissionManage;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Permission.Dto;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using static GISWaterSupplyAndSewageServer.CommonTools.ChidrenTree;

namespace GISWaterSupplyAndSewageServer.OracleDAL.PlanningEngine.PermissionManage
{
    public class PermissionForMapDAL : IPermissionForMapDAL
    {
        /// <summary>
        /// 组织机构查询接口
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <param name="SearchType">0：查询集团公司，1：查询部门</param>
        /// <returns></returns>
        public MessageEntity GetOrganizationList(string Access_Token)
        {
            ChidrenTree childtree = new ChidrenTree();
            string UniWaterUrl = Appsettings.app(new string[] { "BPMDomain" });
            using (WebClient webClient = new WebClient())
            {
                string data = JsonConvert.SerializeObject(new
                {
                    access_token = Access_Token
                });
                #region
                //请求头
                webClient.Headers.Add("Content-Type", "application/json");
                //发送数据
                string responseData = webClient.UploadString(new Uri(UniWaterUrl + "/hdl/uniwater/v1.0/dep/tree.json"), "POST", data);
                UniResult<UniOrganization> Result = JsonConvert.DeserializeObject<UniResult<UniOrganization>>(responseData);
                if (Result.Code == 0)
                {
                    List<UniOrganization> OrganizationList = GetMenuInfoList(Result.Response);
                    List<UniOrganization> list = new List<UniOrganization>();
                    list = OrganizationList.FindAll(e => e.level == "C");
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].children = null;
                    }
                    return MessageEntityTool.GetMessage(list.Count(), list);
                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "统一平台接口出错");
                }
            }
            #endregion
        }
        /// <summary>
        /// 部门查询接口
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <param name="SearchType"></param>
        /// <returns></returns>
        public MessageEntity GetDepartmentList(string Access_Token)
        {
            ChidrenTree childtree = new ChidrenTree();
            string UniWaterUrl = Appsettings.app(new string[] { "BPMDomain" });
            using (WebClient webClient = new WebClient())
            {
                string data = JsonConvert.SerializeObject(new
                {
                    access_token = Access_Token
                });
                #region
                //请求头
                webClient.Headers.Add("Content-Type", "application/json");
                //发送数据
                string responseData = webClient.UploadString(new Uri(UniWaterUrl + "/hdl/uniwater/v1.0/dep/tree.json"), "POST", data);
                UniResult<UniOrganization> Result = JsonConvert.DeserializeObject<UniResult<UniOrganization>>(responseData);
                if (Result.Code == 0)
                {
                    List<UniOrganization> OrganizationList = GetMenuInfoList(Result.Response);
                    List<UniOrganization> list = new List<UniOrganization>();
                    List<TreeModel> treeViewModels = new List<TreeModel>();

                    list = OrganizationList.FindAll(e => e.level == "D");
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].children = null;
                    }
                    treeViewModels = childtree.ConversionList(list, "", "_id", "pid", "name", "", true);

                    return MessageEntityTool.GetMessage(treeViewModels.Count(), treeViewModels);
                }
                else
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "统一平台接口出错");
                }
            }
            #endregion
        }
        /// <summary>
        /// 用户查询接口
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <returns></returns>
        public MessageEntity GetUserList(string Access_Token)
        {
            UniResult<UniUserInfo> Result = GetAllUserInfo(Access_Token);
            if (Result.Code == 0)
            {
                return MessageEntityTool.GetMessage(Result.Response.Count(), Result.Response);
            }
            else
            {
                return MessageEntityTool.GetMessage(ErrorType.SqlError, "统一平台接口出错");
            }
        }
        /// <summary>
        /// 获取图层接口
        /// </summary>
        /// <returns></returns>
        public MessageEntity GetPipeInfoList()
        {
            ChidrenTree childtree = new ChidrenTree();
            string ArcgisServerUrl = Appsettings.app(new string[] { "ArcgisServer", "Url" });
            string ArcgisPipeUrl = Appsettings.app(new string[] { "ArcgisServer", "Services", "PipeQuery" });
            List<TreeModel> treeViewModels = new List<TreeModel>();
            using (WebClient webClient = new WebClient())
            {
                //获取BPM所有流程信息接口地址
                string Url = ArcgisServerUrl + ArcgisPipeUrl + "?f=json";
                #region
                //发送数据
                string responseData = Encoding.UTF8.GetString(webClient.DownloadData(Url));
                ArcgisPipeInfo ArcgisPipeInfoModel = JsonConvert.DeserializeObject<ArcgisPipeInfo>(responseData);
                if (ArcgisPipeInfoModel.layers.Count == 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "未查询到图层信息");
                }
                else
                {
                    treeViewModels = childtree.ConversionList(ArcgisPipeInfoModel.layers, "0", "id", "parentLayerId", "name");
                    return MessageEntityTool.GetMessage(treeViewModels.Count(), treeViewModels);
                }
                #endregion
            }

        }
        /// <summary>
        /// 获取区域接口
        /// </summary>
        /// <returns></returns>
        public MessageEntity GetAreaInfoList()
        {
            ChidrenTree childtree = new ChidrenTree();
            string ArcgisServerUrl = Appsettings.app(new string[] { "ArcgisServer", "Url" });
            string ArcgisParentAreaUrl = Appsettings.app(new string[] { "ArcgisServer", "Services", "ParentAreaQuery" });
            string ArcgisChildAreaUrl = Appsettings.app(new string[] { "ArcgisServer", "Services", "ChildAreaQuery" });
            List<TreeModel> treeViewModels = new List<TreeModel>();
            using (WebClient webClient = new WebClient())
            {
                #region
                //发送数据
                string ParentResponseData = Encoding.UTF8.GetString(webClient.DownloadData(ArcgisServerUrl + ArcgisParentAreaUrl + "?where=1=1&outFields=*&f=pjson"));
                string ChildResponseData = Encoding.UTF8.GetString(webClient.DownloadData(ArcgisServerUrl + ArcgisChildAreaUrl + "?where=1=1&outFields=*&f=pjson"));
                ArcgisAreaInfo ParentAreaModel = JsonConvert.DeserializeObject<ArcgisAreaInfo>(ParentResponseData);
                ArcgisAreaInfo ChildAreaModel = JsonConvert.DeserializeObject<ArcgisAreaInfo>(ChildResponseData);
                List<ArcgisAreaInfoAttributes> AreaInfoList = new List<ArcgisAreaInfoAttributes>();
                for (int i = 0; i < ParentAreaModel.features.Count; i++)
                {
                    ParentAreaModel.features[i].attributes.A_FIRSTCODE = "";
                    AreaInfoList.Add(ParentAreaModel.features[i].attributes);
                }
                for (int i = 0; i < ChildAreaModel.features.Count; i++)
                {
                    AreaInfoList.Add(ChildAreaModel.features[i].attributes);
                }
                if (AreaInfoList.Count == 0)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, "未查询到区域信息");
                }
                else
                {
                    treeViewModels = childtree.ConversionList(AreaInfoList, "", "OCODE", "A_FIRSTCODE", "OTITLE");
                    return MessageEntityTool.GetMessage(treeViewModels.Count(), treeViewModels);
                }
                #endregion
            }

        }
        /// <summary>
        /// 保存组织架构的区域权限
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public MessageEntity SaveAreaPermission(SaveAreaPermissionDto Model)
        {
            List<TreeModel> OrganizationList = GetAllOrganizationList(Model.Access_Token, Model.OrganizationId);
            Permission_OrganizationArea InsertModel = new Permission_OrganizationArea();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    for (int i = 0; i < OrganizationList.Count; i++)
                    {
                        conn.Execute(string.Format(@"DELETE FROM Permission_OrganizationArea WHERE OrganizationId = '{0}'", OrganizationList[i].ID), null, transaction);
                        for (int j = 0; j < Model.OCode.Length; j++)
                        {
                            InsertModel = new Permission_OrganizationArea();
                            InsertModel.OrganizationId = OrganizationList[i].ID;
                            InsertModel.OrganizationType = OrganizationList[i].CODE;
                            InsertModel.OCode = Model.OCode[j];
                            conn.Execute(DapperExtentions.MakeInsertSql(InsertModel), InsertModel, transaction);
                        }
                    }
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(0, null, true, "保存成功", 0);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 保存部门图层权限
        /// </summary>
        /// <returns></returns>
        public MessageEntity SavePipePermissionForDepartment(SavePipePermissionForDepartmentDto Model)
        {
            Permission_DepartmentLayer InsertModel = new Permission_DepartmentLayer();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    List<TreeModel> OrganizationList = GetAllOrganizationList(Model.Access_Token, Model.DepartmentId);
                    string OrganizationIds = string.Join(",", OrganizationList.Select(e => e.ID).ToArray());
                    UniResult<UniUserInfo> UniUserInfoModel = GetAllUserInfo(Model.Access_Token);
                    List<UniUserInfo> UserList = UniUserInfoModel.Response.FindAll(e => OrganizationIds.Contains(e.group));

                    IDbTransaction transaction = conn.BeginTransaction();
                    for (int l = 0; l < OrganizationList.Count; l++)
                    {
                        conn.Execute(string.Format(@"DELETE FROM Permission_DepartmentLayer WHERE DepartmentId = '{0}'", OrganizationList[l].ID), null, transaction);
                        for (int i = 0; i < Model.LayerInfoList.Count; i++)
                        {
                            InsertModel = new Permission_DepartmentLayer();
                            InsertModel.DepartmentId = OrganizationList[l].ID;
                            InsertModel.IsReadonly = Model.LayerInfoList[i].IsReadonly;
                            InsertModel.LayerName = Model.LayerInfoList[i].LayerName;
                            conn.Execute(DapperExtentions.MakeInsertSql(InsertModel), InsertModel, transaction);
                        }
                    }
                    Permission_UserLayer UserInsertModel = new Permission_UserLayer();
                    for (int j = 0; j < UserList.Count; j++)
                    {
                        conn.Execute(string.Format(@"DELETE FROM Permission_UserLayer WHERE UserId = '{0}'", UserList[j]._id), null, transaction);
                        for (int k = 0; k < Model.LayerInfoList.Count; k++)
                        {
                            UserInsertModel = new Permission_UserLayer();
                            UserInsertModel.UserId = UserList[j]._id;
                            UserInsertModel.DepartmentId = UserList[j].group;
                            UserInsertModel.IsReadonly = Model.LayerInfoList[k].IsReadonly;
                            UserInsertModel.LayerName = Model.LayerInfoList[k].LayerName;
                            conn.Execute(DapperExtentions.MakeInsertSql(UserInsertModel), UserInsertModel, transaction);
                        }
                    }
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(0, null, true, "保存成功", 0);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 保存用户图层权限
        /// </summary>
        /// <returns></returns>
        public MessageEntity SavePipePermissionForUser(SavePipePermissionForUserDto Model)
        {
            Permission_UserLayer InsertModel = new Permission_UserLayer();
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    IDbTransaction transaction = conn.BeginTransaction();
                    conn.Execute(string.Format(@"DELETE FROM Permission_UserLayer WHERE UserId = '{0}'", Model.UserId), null, transaction);
                    for (int i = 0; i < Model.LayerInfoList.Count; i++)
                    {
                        InsertModel = new Permission_UserLayer();
                        InsertModel.UserId = Model.UserId;
                        InsertModel.DepartmentId = Model.DepartmentId;
                        InsertModel.IsReadonly = Model.LayerInfoList[i].IsReadonly;
                        InsertModel.LayerName = Model.LayerInfoList[i].LayerName;
                        conn.Execute(DapperExtentions.MakeInsertSql(InsertModel), InsertModel, transaction);
                    }
                    transaction.Commit();
                    return MessageEntityTool.GetMessage(0, null, true, "保存成功", 0);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 获取机构区域权限信息
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <returns></returns>
        public MessageEntity GetAreaPermissionInfo(string OrganizationId)
        {
            List<Permission_OrganizationArea> list = new List<Permission_OrganizationArea>();
            string sql = string.Format(@"SELECT PO.Id,PO.OrganizationId,PO.OrganizationType,PO.OCode FROM Permission_OrganizationArea PO WHERE PO.OrganizationId ='{0}' ", OrganizationId);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    list = conn.Query<Permission_OrganizationArea>(sql).ToList();
                    return MessageEntityTool.GetMessage(list.Count, list);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 获取部门图层权限信息
        /// </summary>
        /// <returns></returns>
        public MessageEntity GetPipePermissionInfoForDepartment(string DepartmentId)
        {
            List<Permission_DepartmentLayer> list = new List<Permission_DepartmentLayer>();
            string sql = string.Format(@"SELECT PD.Id,PD.DepartmentId,PD.LayerName,PD.IsReadonly FROM Permission_DepartmentLayer PD WHERE PD.DepartmentId ='{0}' ", DepartmentId);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    list = conn.Query<Permission_DepartmentLayer>(sql).ToList();
                    return MessageEntityTool.GetMessage(list.Count, list);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 获取用户图层权限信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public MessageEntity GetPipePermissionInfoForUser(string UserId)
        {
            List<Permission_UserLayer> list = new List<Permission_UserLayer>();
            string sql = string.Format(@"SELECT PU.Id,PU.UserId,PU.LayerName,PU.DepartmentId,PU.IsReadonly FROM Permission_UserLayer PU WHERE PU.UserId ='{0}' ", UserId);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    list = conn.Query<Permission_UserLayer>(sql).ToList();
                    return MessageEntityTool.GetMessage(list.Count, list);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }
        }
        /// <summary>
        /// 获取用户所拥有的区域及图层权限信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public MessageEntity GetUserPipeAndLayerPermissionInfo(string UserId)
        {
            UserPipeAndLayerPermissionInfoOutput Result = new UserPipeAndLayerPermissionInfoOutput();
            List<Permission_UserLayer> LayerList = new List<Permission_UserLayer>();
            List<Permission_OrganizationArea> AreaList = new List<Permission_OrganizationArea>();
            List<Permission_OrganizationArea> AreaDetailList = new List<Permission_OrganizationArea>();
            string AreaSql = string.Empty;
            string LayerSql = string.Format(@"SELECT PU.Id,PU.UserId,PU.LayerName,PU.DepartmentId,PU.IsReadonly FROM Permission_UserLayer PU WHERE PU.UserId ='{0}' ", UserId);
            using (var conn = ConnectionFactory.GetDBConn(ConnectionFactory.DBConnNames.ORCL))
            {
                try
                {
                    LayerList = conn.Query<Permission_UserLayer>(LayerSql).ToList();
                    string[] DepartmentIds = LayerList.Select(e => e.DepartmentId).Distinct().ToArray();
                    for (int i = 0; i < DepartmentIds.Length; i++)
                    {
                        AreaSql = string.Format(@"SELECT PO.Id,PO.OrganizationId,PO.OrganizationType,PO.OCode FROM Permission_OrganizationArea PO WHERE PO.OrganizationId ='{0}' ", DepartmentIds[i]);
                        AreaDetailList = conn.Query<Permission_OrganizationArea>(AreaSql).ToList();
                        if (AreaDetailList.Count > 0)
                        {
                            AreaList.AddRange(AreaDetailList);
                        }
                    }
                    Result.UserId = UserId;
                    Result.LayerList = LayerList;
                    Result.AreaList = AreaList;
                    return MessageEntityTool.GetMessage(1, Result);
                }
                catch (Exception e)
                {
                    return MessageEntityTool.GetMessage(ErrorType.SqlError, e.Message);
                }
            }

        }
        /// <summary>
        /// 用户信息查询接口
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <returns></returns>
        public UniResult<UniUserInfo> GetAllUserInfo(string Access_Token)
        {
            try
            {
                string UniWaterUrl = Appsettings.app(new string[] { "BPMDomain" });
                using (WebClient webClient = new WebClient())
                {
                    string data = JsonConvert.SerializeObject(new
                    {
                        access_token = Access_Token
                    });
                    //请求头
                    webClient.Headers.Add("Content-Type", "application/json");
                    //发送数据
                    string responseData = webClient.UploadString(new Uri(UniWaterUrl + "/hdl/uniwater/v1.0/user/list.json"), "POST", data);
                    UniResult<UniUserInfo> Result = JsonConvert.DeserializeObject<UniResult<UniUserInfo>>(responseData);
                    if (Result.Code == 0)
                    {
                        return Result;
                    }
                    else
                    {
                        return new UniResult<UniUserInfo>();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 组织机构查询接口
        /// </summary>
        /// <param name="Access_Token"></param>
        /// <param name="SearchType">0：查询集团公司，1：查询部门</param>
        /// <returns></returns>
        public List<TreeModel> GetAllOrganizationList(string Access_Token, string OrganizationId)
        {
            ChidrenTree childtree = new ChidrenTree();
            List<TreeModel> treeViewModels = new List<TreeModel>();
            string UniWaterUrl = Appsettings.app(new string[] { "BPMDomain" });
            using (WebClient webClient = new WebClient())
            {
                string data = JsonConvert.SerializeObject(new
                {
                    access_token = Access_Token
                });
                #region
                //请求头
                webClient.Headers.Add("Content-Type", "application/json");
                //发送数据
                string responseData = webClient.UploadString(new Uri(UniWaterUrl + "/hdl/uniwater/v1.0/dep/tree.json"), "POST", data);
                UniResult<UniOrganization> Result = JsonConvert.DeserializeObject<UniResult<UniOrganization>>(responseData);
                if (Result.Code == 0)
                {
                    List<UniOrganization> OrganizationList = GetMenuInfoList(Result.Response);

                    treeViewModels = childtree.ConversionList(OrganizationList, OrganizationId, "_id", "pid", "name", "level");
                    UniOrganization FatherModel = OrganizationList.Find(e => e._id == OrganizationId);
                    treeViewModels = GetOrganizationInfoList(treeViewModels);
                    if (FatherModel != null)
                    {
                        treeViewModels.Add(new TreeModel
                        {
                            ID = FatherModel._id,
                            PARENTID = FatherModel.pid,
                            NAME = FatherModel.name,
                            CODE = FatherModel.level
                        });
                    }
                    return treeViewModels;
                }
                else
                {
                    return treeViewModels;
                }
            }
            #endregion
        }
        #region 组织架构反向递归
        /// <summary>
        /// 将父子级数据结构转换为普通list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<UniOrganization> GetMenuInfoList(List<UniOrganization> list)
        {
            List<UniOrganization> Resultlist = new List<UniOrganization>();
            foreach (var item in list)
            {
                OperationChildData(Resultlist, item);
                Resultlist.Add(item);
            }
            return Resultlist;
        }
        /// <summary>
        /// 递归子级数据
        /// </summary>
        /// <param name="treeDataList">树形列表数据</param>
        /// <param name="parentItem">父级model</param>
        public static void OperationChildData(List<UniOrganization> AllList, UniOrganization item)
        {
            if (item.children != null)
            {
                if (item.children.Count > 0)
                {
                    AllList.AddRange(item.children);
                    foreach (var subItem in item.children)
                    {
                        OperationChildData(AllList, subItem);
                    }
                }
            }
        }

        /// <summary>
        /// 将父子级数据结构转换为普通list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TreeModel> GetOrganizationInfoList(List<TreeModel> list)
        {
            List<TreeModel> Resultlist = new List<TreeModel>();
            foreach (var item in list)
            {
                OperationOrganizationChildData(Resultlist, item);
                Resultlist.Add(item);
            }
            return Resultlist;
        }
        /// <summary>
        /// 递归子级数据
        /// </summary>
        /// <param name="treeDataList">树形列表数据</param>
        /// <param name="parentItem">父级model</param>
        public static void OperationOrganizationChildData(List<TreeModel> AllList, TreeModel item)
        {
            if (item.TREECHILDREN != null)
            {
                if (item.TREECHILDREN.Count > 0)
                {
                    AllList.AddRange(item.TREECHILDREN);
                    foreach (var subItem in item.TREECHILDREN)
                    {
                        OperationOrganizationChildData(AllList, subItem);
                    }
                }
            }
        }
        #endregion
    }
}
