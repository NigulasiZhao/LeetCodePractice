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
    [System.ComponentModel.DataAnnotations.Schema.Table("Ins_Plan_Equipment_Info")]
    public class Ins_Plan_Equipment_Info
    {
        /// <summary>
        /// 设备信息主键
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string Equipment_Info_Id { get; set; }

        /// <summary>
        /// 计划设备主键
        /// </summary>
        [DataMember]
        public string Plan_Equipment_Id { get; set; }
        /// <summary>
        /// 设施ID
        /// </summary>
        [DataMember]
        public string GlobID { get; set; }
        /// <summary>
        /// 设施编号
        /// </summary>
        [DataMember]
        public string Equipment_Info_Code { get; set; }

        /// <summary>
        /// 设施名称
        /// </summary>
        [DataMember]
        public string Equipment_Info_Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// 口径
        /// </summary>
        [DataMember]
        public string Caliber { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        public string Lon { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        public string Lat { get; set; }

        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string Geometry { get; set; }
        /// <summary>
        ///类型 1：设备 2：管线
        /// </summary>
        [DataMember]
        public int EquType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime Create_Time { get; set; }

        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public string WKTGeometry { get; set; }
    }
}
