using GISWaterSupplyAndSewageServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer.CommonTools
{
    public class SqlCondition
    {
        public string getParInfo(List<ParameterInfo> parInfo)
        {
            string sqlCondition = " where 1=1 ";

            if (parInfo.Count > 0)
            {
                parInfo.ForEach(parinfo =>
                {
                    switch (parinfo.DataType)
                    {
                        case "string":
                            sqlCondition = sqlCondition + " " + parinfo.LinkType + " " + parinfo.ParName + " like '%" + parinfo.ParValue + "%' ";
                            break;
                        case "number":
                            sqlCondition = sqlCondition + " " + parinfo.LinkType + " " + parinfo.ParName + " = " + parinfo.ParValue;
                            break;
                        case "bool":
                            sqlCondition = sqlCondition + " " + parinfo.LinkType + " " + parinfo.ParName + " = " + parinfo.ParValue;
                            break;
                        case "startTime":
                            sqlCondition = sqlCondition + " " + parinfo.LinkType + " " + parinfo.ParName + " >= to_date('" + parinfo.ParValue+ "','YYYY-mm-dd HH24:Mi:SS')";
                            break;
                        case "endTime":
                            parinfo.ParValue = DateTime.Parse( parinfo.ParValue).AddDays(1).AddSeconds(-1).ToString();
                            sqlCondition = sqlCondition + " " + parinfo.LinkType + " " + parinfo.ParName + " <= to_date('" + parinfo.ParValue + "','YYYY-mm-dd HH24:Mi:SS')";
                            break;
                        case "condition":
                            sqlCondition = sqlCondition + " " + parinfo.LinkType + " " + parinfo.ParValue;
                            break;
                    }
                });
            }
            return sqlCondition;
        }
    }
}
