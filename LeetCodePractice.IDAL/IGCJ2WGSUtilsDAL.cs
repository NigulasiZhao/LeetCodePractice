using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISWaterSupplyAndSewageServer.Model;
using GISWaterSupplyAndSewageServer.Model.BaseEntity;
namespace GISWaterSupplyAndSewageServer.IDAL
{
    public interface   IGCJ2WGSUtilsDAL : IDependency
    {
        MessageEntity gcj02towgs84(double wgs_lon, double wgs_lat);

        MessageEntity wgs84togcj02(double wgs_lon, double wgs_lat);
        MessageEntity wgs84togcj02List(List<double[]> listCoordinate);
        MessageEntity gcj02towgs84List(List<double[]> listCoordinate);
    }
}
