using GisPlateform.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GisPlateformForCore.Model.PlanningEngine.InspectionSettings
{

    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Event_Leak")]
    public class Ins_Event_Leak
    {

        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string LeakId { set; get; }
        [DataMember]
        public string EventID { set; get; }
        
        /// <summary>
        /// 工单编号
        /// </summary>
        [DataMember]
        public int leakPointType { set; get; }
        [DataMember]
        public string LeakPipeMaterial { set; get; }
        [DataMember]
        public string LeakGound { set; get; }
        [DataMember]
        public string LeakPipeDiameter { set; get; }
        /// <summary>
        /// 事件地址
        /// </summary>
        [DataMember]
        public string EventAddress { set; get; }
    
        /// <summary>
        /// 上传人id
        /// </summary>
        [DataMember]
        public string UppersonId { set; get; }
        /// <summary>
        /// 上传人
        /// </summary>
        [DataMember]
        public string UpName { set; get; }
        /// <summary>
        /// 上传部门id
        /// </summary>
        [DataMember]
        public string UpDeptId { set; get; }
        /// <summary>
        /// 上传部门
        /// </summary>
        [DataMember]
        public string UpDeptName { set; get; }
        /// <summary>
        /// 事件类型编号 关联事件类型表
        /// </summary>
        [DataMember]
        public string EventTypeId { set; get; }
        /// <summary>
        /// 关联事件类型 中事件内容编号
        /// </summary>
        [DataMember]
        public string EventTypeId2 { set; get; }
        /// <summary>
        /// 关联事件上报来源表编号
        /// </summary>
        [DataMember]
        public int EventFromId { set; get; }
     
        /// <summary>
        /// 现场照片  以｜分割每个图片地址
        /// </summary>
        [DataMember]
        public string EventPictures { set; get; }
        [DataMember]
        public string EventVideo { set; get; }
        
        /// <summary>
        /// 事件备注
        /// </summary>
        [DataMember]
        public string EventDesc { set; get; }
        /// <summary>
        /// X坐标
        /// </summary>
        [DataMember]
        public string EventX { set; get; }
        /// <summary>
        /// Y坐标
        /// </summary>
        [DataMember]
        public string EventY { set; get; }
    
        /// <summary>
        /// 删除状态  1已删除   0未删除
        /// </summary>
        [DataMember]
        public int DeleteStatus { set; get; }
      
        /// <summary>
        /// 网格分区名称
        /// </summary>
        [DataMember]
        public string RangName { set; get; }
        /// <summary>
        /// 巡检上报id
        /// </summary>
        [DataMember]
        public string Plan_task_id { set; get; }

        /// <summary>
        /// 照片
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotInsert)]
        public string[] Base64Image { set; get; }


    }
}

