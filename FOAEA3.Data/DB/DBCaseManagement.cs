﻿using DBHelper;
using FOAEA3.Data.Base;
using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using System.Collections.Generic;

namespace FOAEA3.Data.DB
{
    public class DBCaseManagement : DBbase, ICaseManagementRepository
    {
        public DBCaseManagement(IDBTools mainDB) : base(mainDB)
        {

        }

        public void CreateCaseManagement(CaseManagementData caseManagementData)
        {
            var parameters = new Dictionary<string, object>
            {
                {"Appl_EnfSrv_Cd", caseManagementData.Appl_EnfSrv_Cd },
                {"Appl_CtrlCd", caseManagementData.Appl_CtrlCd },
                {"CaseMgmnt_Dte", caseManagementData.CaseMgmnt_Dte },
                {"Subm_SubmCd", caseManagementData.Subm_SubmCd },
                {"Subm_Recpt_SubmCd", caseManagementData.Subm_Recpt_SubmCd }
            };

            MainDB.ExecProc("CaseMgmnt_Insert", parameters);
        }
    }
}
