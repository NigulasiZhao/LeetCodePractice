using System;
using System.Collections.Generic;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.PlanningEngine.Enterprise.Dto
{
    public class StatisticalDataOutput
    {
        public int EnterpriseCount { get; set; }

        public int TenderCount { get; set; }

        public List<EnterpriseTypeStatistical> EnterpriseTypeList { get; set; }

        public List<EnterpriseScoreStatistical> EnterpriseScoreList { get; set; }

        public List<EnterpriseRankStatistical> DescRankList { get; set; }

        public List<EnterpriseRankStatistical> AscRankList { get; set; }
    }
    public class EnterpriseAvgScoreStatistical
    {
        public string Enterpriseid { get; set; }
        public string EnterpriseName { get; set; }
        public int Checkscore { get; set; }
        public string MiShape { get; set; }
    }
    public class EnterpriseTypeStatistical
    {
        public string EnterpriseType { get; set; }
        public int EnterpriseTypeCount { get; set; }
    }
    public class EnterpriseScoreStatistical
    {
        public string EnterpriseScoreType { get; set; }
        public decimal EnterpriseScoreCount { get; set; }
        public List<EnterpriseAvgScoreStatistical> DataList { get; set; }

        public EnterpriseScoreStatistical()
        {
            DataList = new List<EnterpriseAvgScoreStatistical>();
        }
    }

    public class EnterpriseRankStatistical
    {
        public string Enterprisename { get; set; }
        public decimal AvgScore { get; set; }
        public int Ranking { get; set; }
    }

    public class EnterprisePointInfo
    {
        public string Mishpae { get; set; }
        public decimal Enterpriseid { get; set; }
    }
}
