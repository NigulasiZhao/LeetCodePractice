using GISWaterSupplyAndSewageServer.Model.AttributePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.Model.Common
{

    [Serializable]
    [DataContract]
    public  class CoordinateModel
    {
        /// <summary>
        /// 坐标集合
        /// </summary>
        [DataMember]
        public List<double[]> listCoordinate { get; set; }
    }
}
