﻿using DBHelper;
using FOAEA3.Data.Base;
using FOAEA3.Model;
using FOAEA3.Model.Base;
using FOAEA3.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FOAEA3.Data.DB
{
    internal class DBSummFAFR_DE : DBbase, ISummFAFR_DERepository
    {
        public DBSummFAFR_DE(IDBTools mainDB) : base(mainDB)
        {

        }

        public DataList<SummFAFR_DE_Data> GetSummFaFrDe(int summFAFR_Id)
        {
            var parameters = new Dictionary<string, object>() {
                { "CtrlSummFaFrId", summFAFR_Id }
            };

            var data = MainDB.GetDataFromStoredProc<SummFAFR_DE_Data>("GetSummFaFrDEForDivertFunds", parameters, FillDataFromReader);
            
            return new DataList<SummFAFR_DE_Data>(data, MainDB.LastError);

        }

        public DataList<SummFAFR_DE_Data> GetSummFaFrDeReadyBatches(string enfSrv_Src_Cd, string DAFABatchId)
        {
            var parameters = new Dictionary<string, object>() {
                { "chrEnfSrv_Src_Cd", enfSrv_Src_Cd },
                { "chrReadyBatchId", DAFABatchId }
            };

            var data = MainDB.GetDataFromStoredProc<SummFAFR_DE_Data>("SummFaFrDeGetReadyBatches", parameters, FillDataFromReader);

            return new DataList<SummFAFR_DE_Data>(data, MainDB.LastError);

        }

        private void FillDataFromReader(IDBHelperReader rdr, SummFAFR_DE_Data data)
        {
            data.SummFAFR_Id = (int)rdr["SummFAFR_Id"]; 
            data.Appl_EnfSrv_Cd = rdr["Appl_EnfSrv_Cd"] as string; // can be null 
            data.Appl_CtrlCd = rdr["Appl_CtrlCd"] as string; // can be null 
            data.Batch_Id = rdr["Batch_Id"] as string; // can be null
            data.EnfSrv_Src_Cd = rdr["EnfSrv_Src_Cd"] as string; // can be null 
            data.SummFAFR_FA_Payable_Dte = rdr["SummFAFR_FA_Payable_Dte"] as DateTime?; // can be null 
            data.SummFAFR_AvailDbtrAmt_Money = rdr["SummFAFR_AvailDbtrAmt_Money"] as decimal?; // can be null 
            data.Dbtr_Id = rdr["Dbtr_Id"] as string; // can be null 

            if (rdr.ColumnExists("SummFAFR_OrigFA_Ind")) data.SummFAFR_OrigFA_Ind = rdr["SummFAFR_OrigFA_Ind"] as byte?; // can be null 
            if (rdr.ColumnExists("SummFAFRLiSt_Cd")) data.SummFAFRLiSt_Cd = rdr["SummFAFRLiSt_Cd"] as short?; // can be null 
            if (rdr.ColumnExists("SummFAFR_Reas_Cd")) data.SummFAFR_Reas_Cd = rdr["SummFAFR_Reas_Cd"] as int?; // can be null 
            if (rdr.ColumnExists("EnfSrv_Loc_Cd")) data.EnfSrv_Loc_Cd = rdr["EnfSrv_Loc_Cd"] as string; // can be null 
            if (rdr.ColumnExists("EnfSrv_SubLoc_Cd")) data.EnfSrv_SubLoc_Cd = rdr["EnfSrv_SubLoc_Cd"] as string; // can be null 
            if (rdr.ColumnExists("SummFAFR_FA_Pym_Id")) data.SummFAFR_FA_Pym_Id = rdr["SummFAFR_FA_Pym_Id"] as string; // can be null 
            if (rdr.ColumnExists("SummFAFR_MultiSumm_Ind")) data.SummFAFR_MultiSumm_Ind = rdr["SummFAFR_MultiSumm_Ind"] as byte?; // can be null 
            if (rdr.ColumnExists("SummFAFR_Post_Dte")) data.SummFAFR_Post_Dte = rdr["SummFAFR_Post_Dte"] as DateTime?; // can be null 
            if (rdr.ColumnExists("SummFAFR_AvailAmt_Money")) data.SummFAFR_AvailAmt_Money = rdr["SummFAFR_AvailAmt_Money"] as decimal?; // can be null 
            if (rdr.ColumnExists("SummFAFR_Comments")) data.SummFAFR_Comments = rdr["SummFAFR_Comments"] as string; // can be null 

            if (rdr.ColumnExists("SummFAFR_OrigFA_IndDesc"))
            {
                string origDesc = rdr["SummFAFR_OrigFA_IndDesc"] as string;
                if (origDesc == "FA")
                    data.SummFAFR_OrigFA_Ind = 1;
                else
                    data.SummFAFR_OrigFA_Ind = 0;
            }
         }

    }
}
