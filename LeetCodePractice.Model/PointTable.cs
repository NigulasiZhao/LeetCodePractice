using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;

namespace GISWaterSupplyAndSewageServer.Model
{
    [Serializable]
    public class PointTable
    {
        public int OBJECTID { set; get; }
        public string SHAPE { set; get; }
        public string equipment_number { set; get; }
        public string caliber { set; get; }
        public decimal elevation { set; get; }
        public decimal depth { set; get; }
        public string Installation_address { set; get; }
        public string construction_unit { set; get; }
        public string management_unit { set; get; }
        public DateTime completion_date { set; get; }
        public DateTime laying_age { set; get; }
        public string embedding_mode { set; get; }
        public decimal coordinate_x { set; get; }
        public decimal coordinate_y { set; get; }
        public string equipment_type { set; get; }
        public string switch_type { set; get; }
        public string Interface_form { set; get; }
        public string remarks { set; get; }
        #region 节点字段
        public string Coversize { set; get; }
        public string CoverMaterial { set; get; }
        #endregion
        #region 管网字段
        public decimal length { set; get; }
        public string material_science { set; get; }
        public decimal startingpoint_elevation { set; get; }
        public decimal endpoint_elevation { set; get; }
        public decimal startingpoint_depth { set; get; }
        public decimal endpoint_depth { set; get; }
        public decimal startingpoint_x { set; get; }
        public decimal startingpoint_y { set; get; }
        public decimal endpoint_x { set; get; }
        public decimal endpoint_y { set; get; }
        #endregion
    }
}
