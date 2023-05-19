﻿using DBHelper;
using FOAEA3.Data.Base;
using FOAEA3.Model;
using FOAEA3.Model.Base;
using FOAEA3.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FOAEA3.Data.DB
{
    internal class DBControlBatch : DBbase, IControlBatchRepository
    {
        public DBControlBatch(IDBTools mainDB) : base(mainDB)
        {

        }

        public DataList<ControlBatchData> GetFADAReadyBatch(string EnfSrv_Source_Cd = "", string DAFABatchID = "")
        {
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(EnfSrv_Source_Cd))
                parameters.Add("EnfSrv_Source_Cd", EnfSrv_Source_Cd);

            if (!string.IsNullOrEmpty(DAFABatchID))
                parameters.Add("DAFABatchID", DAFABatchID);

            var data = MainDB.GetDataFromStoredProc<ControlBatchData>("CtrlBatchGetDAFAReadyBatches", parameters, FillDataFromReader);

            return new DataList<ControlBatchData>(data, MainDB.LastError);
        }

        public void CreateXFControlBatch(ControlBatchData values, out string returnCode, out string batchID, out string reasonCode, out string reasonText)
        {
            var parameters = new Dictionary<string, object>() {
                { "chrEnfSrv_Src_Cd", "FO01" },
                { "chrBatchType_Cd", values.BatchType_Cd },
                { "dtmBatch_Post_Dte", values.Batch_Post_Dte },
                { "sntBatchLiSt_Cd", values.BatchLiSt_Cd},
                { "intBatch_Pend_Ind", values.Batch_Pend_Ind}
            };

            if (!string.IsNullOrEmpty(values.Batch_Id)) parameters.Add("chrBatch_Id", values.Batch_Id);
            if (!string.IsNullOrEmpty(values.EnfSrv_Src_Cd)) parameters.Add("chrEnfSrv_Src_Cd", values.EnfSrv_Src_Cd);
            if (!string.IsNullOrEmpty(values.DataEntryBatch_Id)) parameters.Add("chrDataEntryBatch_Id", values.DataEntryBatch_Id);
            if (values.Batch_Compl_Dte.HasValue) parameters.Add("dtmBatch_Compl_Dte", values.Batch_Compl_Dte.Value);
            if (!string.IsNullOrEmpty(values.Medium_Cd)) parameters.Add("chrMedium_Cd", values.Medium_Cd);
            if (values.SourceRecCnt.HasValue) parameters.Add("intSourceRecCnt", values.SourceRecCnt.Value);
            if (values.DoJRecCnt.HasValue) parameters.Add("intDoJRecCnt", values.DoJRecCnt.Value);
            if (values.SourceTtlAmt_Money.HasValue) parameters.Add("curSourceTtlAmt_Money", values.SourceTtlAmt_Money.Value);
            if (values.DoJTtlAmt_Money.HasValue) parameters.Add("curDoJTtlAmt_Money", values.DoJTtlAmt_Money.Value);
            if (values.Batch_Reas_Cd.HasValue) parameters.Add("intBatch_Reas_Cd", values.Batch_Reas_Cd.Value);
            if (values.PendTtlAmt_Money.HasValue) parameters.Add("curPendTtlAmt_Money", values.PendTtlAmt_Money.Value);
            
            var outputParameters = new Dictionary<string, string> {
                {"intReturnCode", "I"},
                {"chrBatchId", "C"},
                {"intReasonCode", "I"},
                {"vchReasonText", "S"}
            };

            var data = MainDB.GetDataFromStoredProcViaReturnParameters("fp_XFCtrlBatch", parameters, outputParameters);

            returnCode = data["intReturnCode"].ToString();
            batchID = data["chrBatchId"].ToString();
            reasonCode = data["intReasonCode"].ToString();
            reasonText = data["vchReasonText"].ToString();
        }
        
        private void FillDataFromReader(IDBHelperReader rdr, ControlBatchData data)
        {
            data.Batch_Id = rdr["Batch_Id"] as string;
            data.EnfSrv_Src_Cd = rdr["EnfSrv_Src_Cd"] as string; // can be null 
            data.DataEntryBatch_Id = rdr["DataEntryBatch_Id"] as string; // can be null 
            data.BatchType_Cd = rdr["BatchType_Cd"] as string;
            data.Batch_Post_Dte = (DateTime)rdr["Batch_Post_Dte"];
            data.Batch_Compl_Dte = rdr["Batch_Compl_Dte"] as DateTime?; // can be null 
            data.Medium_Cd = rdr["Medium_Cd"] as string;
            data.SourceRecCnt = rdr["SourceRecCnt"] as int?; // can be null 
            data.DoJRecCnt = rdr["DoJRecCnt"] as int?; // can be null 
            data.SourceTtlAmt_Money = rdr["SourceTtlAmt_Money"] as decimal?; // can be null 
            data.DoJTtlAmt_Money = rdr["DoJTtlAmt_Money"] as decimal?; // can be null 
            data.BatchLiSt_Cd = (short)rdr["BatchLiSt_Cd"];
            data.Batch_Reas_Cd = rdr["Batch_Reas_Cd"] as int?; // can be null 
            data.Batch_Pend_Ind = (byte)rdr["Batch_Pend_Ind"];
            data.PendTtlAmt_Money = rdr["PendTtlAmt_Money"] as decimal?; // can be null 
            data.FeesTtlAmt_Money = rdr["FeesTtlAmt_Money"] as decimal?; // can be null 
        }
    }
}
