using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.InspectionSettings
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_WorkOrder_Oper_History")]
  public  class Ins_WorkOrder_Oper_History
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string HistoryId { set; get; }
        /// <summary>
        /// 工单编号
        /// </summary>
        [DataMember]
        public string EventID { set; get; }
        /// <summary>
        /// 步骤ID                                                                  
        /// </summary>
        [DataMember]
        public int OperId { set; get; }
        /// <summary>
        /// 上传图片路径
        /// </summary>
        [DataMember]
        public string Pictures { set; get; }
        /// <summary>
        /// 步骤处理时间
        /// </summary>		
        [DataMember]
        public DateTime OperTime
        {
            set; get;
        }
        /// <summary>
        /// 操作意见
        /// </summary>
        [DataMember]
        public string OperRemarks { set; get; }
        /// <summary>
        /// 分派人id
        /// </summary>
        [DataMember]
        public string DispatchPersonID { set; get; }
        /// <summary>
        /// 分派人
        /// </summary>
        [DataMember]
        public string DispatchPersonName { set; get; }
        /// <summary>
        /// 分派人部门id
        /// </summary>
        [DataMember]
        public string DispatchDetpID { set; get; }
        /// <summary>
        /// 分派人部门名称
        /// </summary>
        [DataMember]
        public string DispatchDetptName { set; get; }

        [DataMember]
        public string ExecPersonId { set; get; }
        
        [DataMember]
        public string ExecPersonName { set; get; }
      
        [DataMember]
        public string ExecDetpID { set; get; }
       
        [DataMember]
        public string ExecDetptName { set; get; }
        /// <summary>
        /// 事件状态
        /// </summary>
        [DataMember]
        public int IsValid { set; get; }
        /// <summary>
        /// 满意度
        /// </summary>
        [DataMember]
        public string Satisfaction { set; get; }
          
    }
}
