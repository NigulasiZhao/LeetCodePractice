using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;

namespace GISWaterSupplyAndSewageServer.IDAL.CoordinateTransf
{
    public interface IProjNetCoordinateDAL : IDependency
    {
        IList<double[]> S2000ToWGS84List(List<double[]> pts);
        double[] S2000ToWGS84(double[] pts);
        IList<double[]> WGS84ToS2000List(List<double[]> pts);
        double[] WGS84ToS2000(double[] pts);
        IList<double[]> GCJ02ToS2000List(List<double[]> pts);
        double[] GCJ02ToS2000(double[] pts);
        MessageEntity S2000ToGCJ02List(List<double[]> pts);
        MessageEntity S2000ToGCJ02(double[] pts);
    }
}
