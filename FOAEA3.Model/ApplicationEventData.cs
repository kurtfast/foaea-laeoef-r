﻿using FOAEA3.Model.Enums;
using FOAEA3.Resources.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace FOAEA3.Model
{
    public class ApplicationEventData
    {
        public EventQueue Queue { get; set; }

        public int Event_Id { get; set; }

        public string Appl_CtrlCd { get; set; }

        public string Subm_SubmCd { get; set; }

        public string Appl_EnfSrv_Cd { get; set; }

        public string Subm_Recpt_SubmCd { get; set; }

        public string Event_RecptSubm_ActvStCd { get; set; }

        public DateTime? Appl_Rcptfrm_Dte { get; set; }

        public string Subm_Update_SubmCd { get; set; }

        [DisplayFormat(DataFormatString = DateTimeHelper.YYYY_MM_DD_HH_MM_SS)]
        public DateTime Event_TimeStamp { get; set; }

        public DateTime? Event_Compl_Dte { get; set; }

        public EventCode? Event_Reas_Cd { get; set; }

        public string Event_Reas_Text { get; set; }

        public string Event_Priority_Ind { get; set; }

        public DateTime Event_Effctv_Dte { get; set; }

        public string ActvSt_Cd { get; set; }

        public ApplicationState AppLiSt_Cd { get; set; }

        public string AppCtgy_Cd { get; set; }
    }
}
