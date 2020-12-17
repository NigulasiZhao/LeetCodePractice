using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;

namespace GISWaterSupplyAndSewageServer.IDAL.CoordinateTransf
{
    public interface ICoordinateTransDAL : IDependency
    {

        MessageEntity Bd09togcj02(double lng, double lat);
        MessageEntity Gcj02tobd09(double lng, double lat);
        MessageEntity Wgs84togcj02(double lng, double lat);
        MessageEntity Gcj02towgs84(double lng, double lat);
        MessageEntity wgs84togcj02List(List<double[]> listCoordinate);
        MessageEntity gcj02towgs84List(List<double[]> listCoordinate);
        MessageEntity Bd09togcj02List(List<double[]> listCoordinate);
        MessageEntity Gcj02tobd09List(List<double[]> listCoordinate);
    }
}
