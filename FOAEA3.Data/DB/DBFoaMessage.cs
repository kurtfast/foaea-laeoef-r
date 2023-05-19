﻿using DBHelper;
using FOAEA3.Data.Base;
using FOAEA3.Model.Exceptions;
using FOAEA3.Model.Interfaces;
using FOAEA3.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using FOAEA3.Model.Enums;

namespace FOAEA3.Data.DB
{
    public class DBFoaMessage : DBbase, IFoaEventsRepository
    {
        public MessageDataList Messages { get; set; }

        private class FoaMessageData
        {
            public int Error { get; set; }
            public short Severity { get; set; }
            public short Dlevel { get; set; }
            public string Description { get; set; }
            public short? MsgLangId { get; set; }
        }

        public DBFoaMessage(IDBTools mainDB) : base(mainDB)
        {
            Messages = new MessageDataList();
        }

        public FoaEventDataDictionary GetAllFoaMessages()
        {
            var result = new FoaEventDataDictionary();

            string connStr = MainDB.ConnectionString;
            try
            {
                var data = MainDB.GetAllData<FoaMessageData>("FoaMessages", FillFoaMessageDataFromReader);

                if (!string.IsNullOrEmpty(MainDB.LastError))
                    Messages.AddSystemError(MainDB.LastError);

                foreach (var eventData in data)
                {
                    EventCode eventCode = (EventCode)eventData.Error;
                    if (!result.ContainsKey(eventCode))
                    {
                        var newEventData = new FoaEventData
                        {
                            Error = eventData.Error,
                            Dlevel = eventData.Dlevel,
                            Severity = eventData.Severity,
                        };

                        var thisCode = (EventCode)eventData.Error;

                        result.FoaEvents.Add(thisCode.ToString(), newEventData);
                    }

                    if (eventData.MsgLangId == 1033)
                        result[eventCode].Description_e = eventData.Description;
                    else
                        result[eventCode].Description_f = eventData.Description;
                }

                return result;
            }
            catch (Exception e)
            {
                if (Thread.CurrentPrincipal != null)
                    e.Data.Add("user", Thread.CurrentPrincipal.Identity.Name);
                e.Data.Add("connection", connStr);
                throw new ReferenceDataException("Could not load FoaMessages! ", e);
            }
        }

        private void FillFoaMessageDataFromReader(IDBHelperReader rdr, FoaMessageData data)
        {
            data.Error = (int)rdr["error"];
            data.Severity = (short)rdr["severity"];
            data.Dlevel = (short)rdr["dlevel"];
            data.Description = rdr["description"] as string;
            data.MsgLangId = rdr["msglangid"] as short?; // can be null
        }

    }
}
