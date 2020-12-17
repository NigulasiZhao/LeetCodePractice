using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.PlanManage
{
    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plan")]
    public class Ins_Plan
    {
        /// <summary>
        /// 巡检计划主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Plan_Id { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        [DataMember]
        public string Plan_Name { get; set; }

        /// <summary>
        /// 计划周期
        /// </summary>
        [DataMember]
        public string PlanCycleId { get; set; }

        /// <summary>
        /// 是否常规 0:常规 1:临时
        /// </summary>
        [DataMember]
        public int IsNormalPlan { get; set; }

        /// <summary>
        /// 是否反馈 0:需反馈 1::仅到位
        /// </summary>
        [DataMember]
        public int IsFeedBack { get; set; }

        /// <summary>
        /// 巡检计划类型主键
        /// </summary>
        [DataMember]
        public string Plan_Type_Id { get; set; }

        /// <summary>
        /// 巡检区域名称 网格区域时默认最后一个名称
        /// </summary>
        [DataMember]
        public string Range_Name { get; set; }

        /// <summary>
        /// 区域坐标信息
        /// </summary>
        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string Geometry { get; set; }

        /// <summary>
        /// 巡检方式1  车巡   2  徒步
        /// </summary>
        [DataMember]
        public int MoveType { get; set; }

        /// <summary>
        /// 计划模板大类id
        /// </summary>
        [DataMember]
        public string Plan_TemplateType_Id { get; set; }

        /// <summary>
        /// 计划模板id
        /// </summary>
        [DataMember]
        public string PlanTtemplate_Id { get; set; }

        /// <summary>
        /// 新建人ID
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsString)]
        public string Create_Person_Id { get; set; }

        /// <summary>
        /// 新建人姓名
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsString)]
        public string Create_Person_Name { get; set; }

        /// <summary>
        /// 新建时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime Create_Time { get; set; }

        /// <summary>
        /// 分派状态 0：未分派 1 分派中 2分派失败 3分派成功
        /// </summary>
        [DataMember]
        public int Assign_State { get; set; }
        /// <summary>
        /// 巡检区域id(包含网格id或区域id)
        /// </summary>
        [DataMember]
        public string Range_Id { get; set; }
        
        /// <summary>
        /// 设备类型列表
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public List<Ins_Plan_Equipment> Ins_Plan_EquipmentList { get; set; }


        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public string WKTGeometry { get; set; }

    }
}
