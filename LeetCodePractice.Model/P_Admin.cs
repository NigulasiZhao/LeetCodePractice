using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DapperExtensions.Mapper;
using GISWaterSupplyAndSewageServer.Model.AttributePack;

namespace GISWaterSupplyAndSewageServer.Model
{
    /// <summary>
	/// 类P_Admin。
	/// </summary>
	[Serializable]

    [DataContract]
    public partial class P_Admin : BaseEntity.BaseModel
    {

        #region Model
        [DataMember]
        /// <summary>
        /// iAdminID
        /// </summary>
        [Column(FilterType = FilterType.IsPrimaryKey, PrimaryKeyType = PrimaryKeyType.Identity)]
        public int iAdminID
        {
            set;
            get;
        }
        /// <summary>
        /// iDeptID
        /// </summary>

        [DataMember]
        public int iDeptID
        {
            set;
            get;
        }

        /// <summary>
        /// iRoleID
        /// </summary>

        [DataMember]
        public int iRoleID
        {
            set;
            get;
        }

        /// <summary>
        /// cAdminName
        /// </summary>

        [DataMember]
        public string cAdminName
        {
            set;
            get;
        }
        /// <summary>
        /// CJobNumber
        /// </summary>

        [DataMember]
        public string CJobNumber
        {
            set;
            get;
        }
        /// <summary>
        /// cAdminSex
        /// </summary>

        [Column(FilterType = FilterType.IsNotUpdate)]

        [DataMember]
        public string cAdminSex
        {
            set;
            get;
        }
        /// <summary>
        /// cAdminPassWord
        /// </summary>
        [Column(FilterType = FilterType.IsNotUpdate)]
        [DataMember]
        public string cAdminPassWord
        {
            set;
            get;
        }
        /// <summary>
        /// cAdminTel
        /// </summary>

        [DataMember]
        public string cAdminTel
        {
            set;
            get;
        }
        /// <summary>
        /// cAdminEmail
        /// </summary>

        [DataMember]
        public string cAdminEmail
        {
            set;
            get;
        }
        /// <summary>
        /// iIsLocked
        /// </summary>
        [DataMember]
        public int iIsLocked
        {
            set;
            get;
        }
        /// <summary>
        /// dExpireDate
        /// </summary>
        [DataMember]
        public DateTime? dExpireDate
        {
            set;
            get;
        }
        /// <summary>
        /// iIsAllowChangePWD
        /// </summary>
        [DataMember]
        public int iIsAllowChangePWD
        {
            set;
            get;
        }
        /// <summary>
        /// cLastLoginIP
        /// </summary>
        [DataMember]
        public string cLastLoginIP
        {
            set;
            get;
        }
        /// <summary>
        /// dLastLoginTime
        /// </summary>
        [DataMember]
        public DateTime? dLastLoginTime
        {
            set;
            get;
        }
        /// <summary>
        /// dLastLogoutTime
        /// </summary>
        [DataMember]
        public DateTime? dLastLogoutTime
        {
            set;
            get;
        }
        /// <summary>
        /// iLoginTimes
        /// </summary>
        [DataMember]
        public int? iLoginTimes
        {
            set;
            get;
        }
        /// <summary>
        /// cTitlePic
        /// </summary>
        [DataMember]
        public string cTitlePic
        {
            set;
            get;
        }
        /// <summary>
        /// iSkinID
        /// </summary>
        [DataMember]
        public int? iSkinID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string LoginTicket
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Smid
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? JueSeID
        {
            set;
            get;
        }
        /// <summary>
        /// 是否是其部门的负责人 0 不是 ，1是
        /// </summary>
        [DataMember]
        public int? IsNumOne
        {
            set;
            get;
        }
        /// <summary>
        /// 层级 1高层 2中层 3基层
        /// </summary>
        [DataMember]
        public int? Level
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string AllPinyin
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsDelete
        {
            set;
            get;
        }
        /// <summary>
        /// 是否有权限份派工单 0::否 1:是
        /// </summary>
        [DataMember]
        public int? IsAssignment
        {
            set;
            get;
        }
        /// <summary>
        /// iDeptIDs 部门权限，关于待办事项，人员选择所有部门权限，待办可以查询所有部门下的工单信息，如果只查询自己的待办，部门权限不用勾选
        /// </summary>		
        [DataMember]
        public string iDeptIDs
        {
            set; get;
        }
        [DataMember]
        [Column(FilterType = FilterType.IsNotEdit)]
        public string Token { set; get; }

        public string HdId { set; get; }
        public string HdNo { set; get; }
        #endregion Model



    }

}

