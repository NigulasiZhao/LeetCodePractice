using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Model.ResultDto
{
  public  class Ins_EventStepDto
    {
        public string EventID { set; get; }
        public string EventCode { set; get; }
        public string EventAddress { set; get; }

        public DateTime UpTime { set; get; }
        public string EventFromName { set; get; }
        public string UpName { set; get; }
        public string UpDeptname { set; get; }
        public string EventTypeName { set; get; }
        public string EventTypeName2 { set; get; }
        public string UrgencyName { set; get; }
        public string ExecTime { set; get; }
        public string LinkMan { set; get; }
        public string LinkCall { set; get; }
        public string DetptName { set; get; }
        public string PersonName { set; get; }
        public string IsValidName { set; get; }
        public int IsValid { set; get; }
        public string Eventdesc { set; get; }
        public string EventX { set; get; }
        public string EventY{ set; get; }
        public string Dispatchpersonname { set; get; }
        public string DispatchDetptName{ set; get; }
        public DateTime Opertime { set; get; }
        public string Pictures { set; get; }
        public string Execpersonname { set; get; }
        public string ExecDetptname { set; get; }
        public string OperId { set; get; }
        public string Opername { set; get; }
        public string Opername2 { set; get; }
        public string Operremarks { set; get; }
        public string Satisfaction { set; get; }

        public DateTime PostponeTime { set; get; }
        public DateTime PreEndTime { set; get; }
        public string Cause { set; get; }
    }
}
