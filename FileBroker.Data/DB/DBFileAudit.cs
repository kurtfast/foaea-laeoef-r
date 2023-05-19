﻿using DBHelper;
using FileBroker.Model;
using FileBroker.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace FileBroker.Data.DB
{
    public class DBFileAudit : IFileAuditRepository
    {
        private IDBTools MainDB { get; }

        public DBFileAudit(IDBTools mainDB)
        {
            MainDB = mainDB;
        }

        public List<FileAuditData> GetFileAuditDataForFile(string fileName)
        {
            var parameters = new Dictionary<string, object>
            {
                {"InboundFilename", fileName + ".XML" }
            };

            return MainDB.GetDataFromStoredProc<FileAuditData>("FileAuditSelect", parameters, FillAuditDataFromReader);


        }

        public void InsertFileAuditData(FileAuditData data)
        {
            var parameters = new Dictionary<string, object>
            {
                {"Appl_EnfSrv_Cd", data.Appl_EnfSrv_Cd},
                {"Appl_CtrlCd", data.Appl_CtrlCd},
                {"Appl_Source_RfrNr", data.Appl_Source_RfrNr},
                {"ApplicationMessage", data.ApplicationMessage},
                {"InboundFilename", data.InboundFilename}
            };

            MainDB.ExecProc("FileAuditInsert", parameters);
        }

        public void MarkFileAuditCompletedForFile(string fileName)
        {
            var parameters = new Dictionary<string, object>
            {
                {"InboundFilename", fileName }
            };

            MainDB.ExecProc("FileAuditComplete", parameters);
        }

        public void MarkFileAuditCompletedForItem(FileAuditData data)
        {
            var parameters = new Dictionary<string, object>
            {
                {"Appl_EnfSrv_Cd", data.Appl_EnfSrv_Cd },
                {"Appl_CtrlCd", data.Appl_CtrlCd },
                {"Appl_Source_RfrNr", data.Appl_Source_RfrNr},
                {"InboundFilename", data.InboundFilename }
            };

            MainDB.ExecProc("FileAuditUpdate", parameters);
        }

        private void FillAuditDataFromReader(IDBHelperReader rdr, FileAuditData data)
        {
            data.Appl_EnfSrv_Cd = rdr["EnforcementServiceCode"] as string;
            data.Appl_CtrlCd = rdr["ControlCode"] as string;
            data.Appl_Source_RfrNr = rdr["SourceReferenceNumber"] as string;
            data.ApplicationMessage = rdr["ApplicationMessage"] as string;
                        
            //data.Appl_EnfSrv_Cd = rdr["Appl_EnfSrv_Cd"] as string;
            //data.Appl_CtrlCd = rdr["Appl_CtrlCd"] as string;
            //data.Appl_Source_RfrNr = rdr["Appl_Source_RfrNr"] as string;
            //data.ApplicationMessage = rdr["ApplicationMessage"] as string; // can be null 
            //data.InboundFilename = rdr["InboundFilename"] as string; // can be null 
            //data.Timestamp = rdr["Timestamp"] as DateTime?; // can be null 
            //data.IsCompleted = rdr["IsCompleted"] as bool?; // can be null 
        }
    }
}
