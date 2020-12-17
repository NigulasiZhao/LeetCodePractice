using GisPlateform.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GisPlateformForCore.Model.PlanningEngine.InspectionSettings
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Event")]
    public  class Ins_Event
    {

        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string EventID { set; get; }
        /// <summary>
        /// 工单编号
        /// </summary>
        [DataMember]
        public string EventCode { set; get; }
        /// <summary>
        /// 事件地址
        /// </summary>
        [DataMember]
        public string EventAddress { set; get; }
        /// <summary>
        /// 上报时间
        /// </summary>
        [DataMember]
        public DateTime UpTime { set; get; }
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
        /// 1:一般  2:紧急  3:加急
        /// </summary>
        [DataMember]
        public int UrgencyId { set; get; }
        /// <summary>
        /// 2小时-抢险类   4小时-正常维修   6小时-暂缓处理
        /// </summary>
        [DataMember]
        public int HandlerLevelId { set; get; }
        /// <summary>
        /// 分派人id
        /// </summary>
        [DataMember]
        public string DispatchPersonId { set; get; }

        /// <summary>
        /// 分派人
        /// </summary>
        [DataMember]
        public string DispatchPersonName { set; get; }
        /// <summary>
        /// 执行人id
        /// </summary>
        [DataMember]
        public string ExecPersonId { set; get; }
        /// <summary>
        /// 执行人
        /// </summary>
        [DataMember]
        public string ExecPersonName { set; get; }
        /// <summary>
        /// 执行人部门id
        /// </summary>
        [DataMember]
        public string ExecDetpID { set; get; }
        /// <summary>
        /// 执行人部门
        /// </summary>
        [DataMember]
        public string ExecDetptName { set; get; }
        /// <summary>
        /// 派单时间
        /// </summary>
        [DataMember]
        public DateTime OrderTime { set; get; }
        /// <summary>
        /// 预计完成时间
        /// </summary>
        [DataMember]
        public DateTime PreEndTime { set; get; }
        /// <summary>
        /// 状态    0  正常    1退单
        /// </summary>
        [DataMember]
        public int orderStatus { set; get; }
        /// <summary>
        /// 现场照片  以｜分割每个图片地址
        /// </summary>
        [DataMember]
        public string EventPictures { set; get; }
        /// <summary>
        /// 现场音频  以｜分割每个图片地址
        /// </summary>
        [DataMember]
        public string EventAudio { set; get; }
        /// <summary>
        /// 现场视频  以｜分割每个图片地址
        /// </summary>
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
        /// 事件更新时间
        /// </summary>
        [DataMember]
        public DateTime EventUpdateTime { set; get; }
        /// <summary>
        /// 是否有效   1有效   0无效  4:退单 5延期
        /// </summary>
        [DataMember]
        public int IsValid { set; get; }
        /// <summary>
        /// 删除状态  1已删除   0未删除
        /// </summary>
        [DataMember]
        public int DeleteStatus { set; get; }
        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        public int ExecTime { set; get; }
        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember]
        public string LinkMan { set; get; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember]
        public string LinkCall { set; get; }
        /// <summary>
        /// 事件状态 关联状态表
        /// </summary>
        [DataMember]
        public int EventStatus { set; get; }
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
        public string[] Base64Image { set; get; }

        /// <summary>
        /// 音频
        /// </summary>
        [DataMember]
        public string[] Base64Audio { set; get; }

        /// <summary>
        /// 视频
        /// </summary>
        [DataMember]
        public string[] Base64Video { set; get; }
    }
}
