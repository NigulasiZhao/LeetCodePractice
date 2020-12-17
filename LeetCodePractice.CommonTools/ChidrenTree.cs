
using GISWaterSupplyAndSewageServer.Model.EssentialFactor;
using GISWaterSupplyAndSewageServer.Model.PlanningEngine.Plan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.CommonTools
{
    public class ChidrenTree
    {
        #region 公用递归
        /// <summary>
        /// 构建树形结构类
        /// </summary>
        public class TreeModel
        {
            public string ID { set; get; }
            public string PARENTID { set; get; }
            public string NAME { set; get; }
            public string CODE { set; get; }
            public List<TreeModel> TREECHILDREN { set; get; }

        }
        /// <summary>
        /// 公用递归(反射转换List)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allList">数据列表</param>
        /// <param name="parentId">父级ID</param>
        /// <param name="idField">id字段名臣</param>
        /// <param name="parentIdField">父级id字段名称</param>
        /// <param name="nameField">name字段名称</param>
        /// <param name="nameCode">Code字段名称</param>
        /// <param name="Auto">无视父级ID，自动排序</param>
        /// <returns></returns>
        public List<TreeModel> ConversionList<T>(List<T> allList, string parentId, string idField, string parentIdField, string nameField, string nameCode = "", bool Auto = false)
        {
            List<TreeModel> list = new List<TreeModel>();
            TreeModel model = null;
            foreach (var item in allList)
            {
                model = new TreeModel();
                foreach (System.Reflection.PropertyInfo p in item.GetType().GetProperties())
                {
                    if (p.Name == idField)
                    {
                        model.ID = p.GetValue(item).ToString();
                    }
                    if (p.Name == parentIdField)
                    {
                        model.PARENTID = p.GetValue(item).ToString();
                    }
                    if (p.Name == nameField)
                    {
                        model.NAME = p.GetValue(item).ToString();
                    }
                    if (nameCode != "")
                    {
                        if (p.Name == nameCode)
                        {
                            model.CODE = p.GetValue(item).ToString();
                        }
                    }
                }
                list.Add(model);
            }
            return OperationParentData(list, parentId, Auto);
        }
        /// <summary>
        /// 公用递归(处理递归最父级数据)
        /// </summary>
        /// <param name="treeDataList">树形列表数据</param>
        /// <param name="parentId">父级Id</param>
        /// <returns></returns>
        public List<TreeModel> OperationParentData(List<TreeModel> treeDataList, string parentId, bool Auto)
        {
            List<TreeModel> data = new List<TreeModel>();
            if (Auto)
            {
                for (int i = 0; i < treeDataList.Count; i++)
                {
                    if (treeDataList.FindAll(e => e.PARENTID == treeDataList[i].ID).Count != 0)
                    {
                        data.Add(treeDataList[i]);
                    }
                }
            }
            else
            {
                data = treeDataList.Where(x => x.PARENTID == parentId).ToList();
            }
            List<TreeModel> list = new List<TreeModel>();
            foreach (var item in data)
            {
                OperationChildData(treeDataList, item);
                list.Add(item);
            }
            return list;
        }
        /// <summary>
        /// 公用递归(递归子级数据)
        /// </summary>
        /// <param name="treeDataList">树形列表数据</param>
        /// <param name="parentItem">父级model</param>
        public void OperationChildData(List<TreeModel> treeDataList, TreeModel parentItem)
        {
            var subItems = treeDataList.Where(ee => ee.PARENTID == parentItem.ID).ToList();
            if (subItems.Count != 0)
            {
                parentItem.TREECHILDREN = new List<TreeModel>();
                parentItem.TREECHILDREN.AddRange(subItems);
                foreach (var subItem in subItems)
                {
                    OperationChildData(treeDataList, subItem);
                }
            }
        }
        #endregion
        #region 老版递归
        //public List<TreeChildViewModel> AddChildN(List<GIS_EssentialFactor> esslist, string Pid)
        //{
        //    var data = esslist.Where(x => x.PID == Pid);//获取数据
        //    List<TreeChildViewModel> list = new List<TreeChildViewModel>();
        //    foreach (var item in data)
        //    {
        //        //这一块主要是转换成TreeChidViewModel的值.
        //        TreeChildViewModel childViewModel = new TreeChildViewModel();
        //        childViewModel.ID = item.ID;
        //        childViewModel.NAME = item.EFName;
        //        childViewModel.TREECHILDREN = GetChildList(esslist, childViewModel);
        //        list.Add(childViewModel);
        //    }
        //    return list;
        //}
        //private List<TreeChildViewModel> GetChildList(List<GISWaterSupplyAndSewageServer.Model.EssentialFactor.GIS_EssentialFactor> esslist, TreeChildViewModel treeChildView)
        //{
        //    if (!esslist.Exists(x => x.PID == treeChildView.ID))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return AddChildN(esslist, treeChildView.ID);
        //    }
        //}
        ///// <summary>
        ///// 获取计划模板大类树形目录
        ///// </summary>
        ///// <param name="planlist"></param>
        ///// <param name="Pid"></param>
        ///// <returns></returns>
        //public List<TreeChildViewModel> AddPlanTempChild(List<Ins_Plan_Templatetype> planlist, string Pid)
        //{
        //    var data = planlist.Where(x => x.Templatetype_parentid == Pid);//获取数据
        //    List<TreeChildViewModel> list = new List<TreeChildViewModel>();
        //    foreach (var item in data)
        //    {
        //        //这一块主要是转换成TreeChidViewModel的值.
        //        TreeChildViewModel childViewModel = new TreeChildViewModel();
        //        childViewModel.ID = item.Plan_templatetype_id;
        //        childViewModel.NAME = item.Templatetype_name;
        //        childViewModel.TREECHILDREN = GetPlaneChildList(planlist, childViewModel);
        //        list.Add(childViewModel);
        //    }
        //    return list;
        //}
        //private List<TreeChildViewModel> GetPlaneChildList(List<Ins_Plan_Templatetype> esslist, TreeChildViewModel treeChildView)
        //{
        //    if (!esslist.Exists(x => x.Templatetype_parentid == treeChildView.ID))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return AddPlanTempChild(esslist, treeChildView.ID);
        //    }
        //}
        ///// <summary>
        ///// 获取巡检范围树形结构
        ///// </summary>
        ///// <param name="planlist"></param>
        ///// <param name="Pid"></param>
        ///// <returns></returns>
        //public List<TreeChildViewModel> AddInsRangeChild(List<Ins_Range> planlist, string Pid)
        //{
        //    var data = planlist.Where(x => x.Range_parentid == Pid);//获取数据
        //    List<TreeChildViewModel> list = new List<TreeChildViewModel>();
        //    foreach (var item in data)
        //    {
        //        //这一块主要是转换成TreeChidViewModel的值.
        //        TreeChildViewModel childViewModel = new TreeChildViewModel();
        //        childViewModel.ID = item.Range_id;
        //        childViewModel.NAME = item.range_name;
        //        childViewModel.TREECHILDREN = GetInsRangeChildList(planlist, childViewModel);
        //        list.Add(childViewModel);
        //    }
        //    return list;
        //}
        //private List<TreeChildViewModel> GetInsRangeChildList(List<Ins_Range> esslist, TreeChildViewModel treeChildView)
        //{
        //    if (!esslist.Exists(x => x.Range_id == treeChildView.ID))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return AddInsRangeChild(esslist, treeChildView.ID);
        //    }
        //}
        //public class TreeChildViewModel
        //{
        //    public string ID { set; get; }
        //    public string NAME { set; get; }
        //    public List<TreeChildViewModel> TREECHILDREN { set; get; }
        //}
        #endregion
    }
}
