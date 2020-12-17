using System;
using System.Collections.Generic;
using System.Text;

namespace GisPlateformForCore.Model.PlanningEngine.InspectionSettings
{
  public  class Ins_EventList
    {
        public string EventID { set; get; }
        public DateTime UpTime { set; get; }
        public string EventFromName { set; get; }
        public string EventCode { set; get; }

        public string ExecTime { set; get; }

        public string UrgencyName { set; get; }
        public string IsValid{ set; get; }

        public string IsValidName { set; get; }
        public string EventTypeName { set; get; }
        public string EventTypeName2 { set; get; }
        public string UpName { set; get; }
        public string ExecPersonName { set; get; }
        public string ExecDetptName { set; get; }
        public string LinkMan { set; get; }
        public string LinkCall { set; get; }
        public DateTime EventUpdateTime { set; get; }
        public string Eventaddress { set; get; }
        public string Eventdesc { set; get; }
        public int OperId { set; get; }
        public string OperName { set; get; }
        public string OperName2 { set; get; }
        public string EventX { set; get; }
        public string EventY { set; get; }
        public string EventPictures { set; get; }
        public string EventAudio { set; get; }
        public string EventVideo { set; get; }
        public decimal Distance { set; get; }
        public int IsFinish { set; get; }
        public string RangName { set; get; }
    }
}
