using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace GISWaterSupplyAndSewageServer.Model.AlarmSetting
{
    /// <summary>
    /// 其他台账
    /// </summary>

    [Serializable]
    [DataContract]
    [System.ComponentModel.DataAnnotations.Schema.Table("GIS_ALARM_SETTING")]
    public  class GIS_Alarm_Setting
    {
        [DataMember]
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Guid)]
        public string ID { get; set; }
        [DataMember]
        public string LayerName { get; set; }
        [DataMember]
        [Column(DataType = DataType.IsClob)]
        public string AlarmCondtion { get; set; }
        [DataMember]
        public int OrderNO { get; set; }
        [DataMember]
        public int Enabled { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public string CreateId { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
        [DataMember]
        [Column(FilterType = FilterType.IsNotUpdate, DataType = DataType.IsDateTime)]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public string ModifyId { get; set; }
        [DataMember]
        public string ModifyBy { get; set; }
        [DataMember]
        [Column(FilterType = FilterType.IsNotInsert, DataType = DataType.IsDateTime)]
        public DateTime ModifyTime { get; set; }
        [DataMember]
        public string DevicesName { get; set; }
    }
}
