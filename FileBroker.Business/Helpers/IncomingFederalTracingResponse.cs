﻿using FileBroker.Model;
using FOAEA3.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileBroker.Business.Helpers
{
    public class IncomingFederalTracingResponse
    {
        public static List<TraceResponseData> GenerateFromFileData(FedTracingFileBase dataFromFile, string enfSrvCd,
                                                                   List<TraceCycleQuantityData> cycles, ref List<string> errors)
        {
            var responseData = new List<TraceResponseData>();

            try
            {
                var max03 = 0;
                var max04 = 0;

                // residential
                foreach (var residentialData in dataFromFile.TRCINResidentials)
                    ExtractSectionResidential(dataFromFile.TRCIN02, residentialData.Value, responseData, enfSrvCd, ref max03);

                // exmployer
                max04 = max03 + 1;
                foreach (var employerData in dataFromFile.TRCINEmployers)
                    ExtractSectionEmployer(dataFromFile.TRCIN02, employerData.Value, responseData, enfSrvCd, ref max04);

                int maxRsp = responseData.Max(m => m.TrcRsp_SeqNr);

                // locked
                int section2recordId = ExtractSpecial(dataFromFile.TRCIN02, responseData, enfSrvCd, maxRsp, "00",
                                                      "Locked: re-instate in 1 month", "Protégé: Réactivez dans 1 mois");

                // not found
                section2recordId = ExtractSpecial(dataFromFile.TRCIN02, responseData, enfSrvCd, maxRsp, "01",
                                                  "No address found", "Aucune adresse retrouvée");

                // remove any "blanks"
                responseData.RemoveAll(r => string.IsNullOrEmpty(r.Appl_CtrlCd) || string.IsNullOrEmpty(r.Appl_EnfSrv_Cd));

                UpdateTraceCycleNumber(responseData, cycles);
            }
            catch (Exception e)
            {
                errors.Add("Error generating from file data: " + e.Message);
            }

            return responseData;
        }

        private static void ExtractSectionResidential(List<FedTracing_RecType02> tracingIN02, List<FedTracing_RecTypeResidential> tracingResidential,
                                                      List<TraceResponseData> responseData, string enfSrvCd, ref int max)
        {
            var responseDataTemp = new List<TraceResponseData>();

            var data = from d in tracingIN02
                       join emp in tracingResidential
                                on new { d.dat_Appl_EnfSrvCd, d.dat_Appl_CtrlCd } equals new { emp.dat_Appl_EnfSrvCd, emp.dat_Appl_CtrlCd }
                       into empResponse
                       where string.IsNullOrEmpty(d.dat_TrcSt_Cd?.Trim())
                       from r in empResponse
                       select new
                       {
                           r.dat_Appl_CtrlCd,
                           r.dat_Appl_EnfSrvCd,
                           d.dat_TrcSt_Cd,
                           r.dat_TrcRsp_Addr_Ln,
                           r.dat_TrcRsp_Addr_Ln1,
                           r.dat_TrcRsp_Addr_CityNme,
                           r.dat_TrcRsp_Addr_PrvCd,
                           r.dat_TrcRsp_Addr_CtryCd,
                           r.dat_TrcRsp_Addr_PCd,
                           r.dat_TrcRsp_Addr_LstUpdte,
                           r.RecType
                       };

            int section3recordId = 1;

            foreach (var item in data)
            {
                var newResponse03 = new TraceResponseData
                {
                    Appl_EnfSrv_Cd = item.dat_Appl_EnfSrvCd,
                    Appl_CtrlCd = item.dat_Appl_CtrlCd,
                    EnfSrv_Cd = enfSrvCd,
                    TrcRsp_Rcpt_Dte = DateTime.Now,
                    TrcRsp_SeqNr = section3recordId++ + max,
                    TrcSt_Cd = string.IsNullOrEmpty(item.dat_TrcSt_Cd.Trim()) ? "03" : item.dat_TrcSt_Cd,
                    TrcRsp_Addr_Ln = item.dat_TrcRsp_Addr_Ln,
                    TrcRsp_Addr_Ln1 = item.dat_TrcRsp_Addr_Ln1,
                    TrcRsp_Addr_CityNme = item.dat_TrcRsp_Addr_CityNme,
                    TrcRsp_Addr_PrvCd = item.dat_TrcRsp_Addr_PrvCd,
                    TrcRsp_Addr_CtryCd = item.dat_TrcRsp_Addr_CtryCd.Trim() == "CA" ? string.Empty : item.dat_TrcRsp_Addr_CtryCd,
                    TrcRsp_Addr_PCd = item.dat_TrcRsp_Addr_PCd,
                    TrcRsp_Addr_LstUpdte = item.dat_TrcRsp_Addr_LstUpdte,
                    AddrTyp_Cd = "R",
                    Prcs_RecType = item.RecType,
                    ActvSt_Cd = "C"
                };

                responseDataTemp.Add(newResponse03);
            }

            var c = responseDataTemp.Where(m => m.Appl_CtrlCd == "035971");

            // remove duplicates
            var responseDataTempNoDuplicates = responseDataTemp
                .OrderByDescending(m => m.TrcRsp_Addr_LstUpdte)
                .GroupBy(m => new
                {
                    m.Appl_EnfSrv_Cd,
                    m.Appl_CtrlCd,
                    m.EnfSrv_Cd,
                    m.TrcRsp_EmplNme,
                    m.TrcRsp_EmplNme1,
                    m.TrcSt_Cd,
                    m.TrcRsp_Addr_Ln,
                    m.TrcRsp_Addr_Ln1,
                    m.TrcRsp_Addr_CityNme,
                    m.TrcRsp_Addr_PrvCd,
                    m.TrcRsp_Addr_CtryCd,
                    m.TrcRsp_Addr_PCd,
                    m.Prcs_RecType
                })
                .Select(g => g.First());

            var e = responseDataTemp.Where(m => m.Appl_CtrlCd == "035971");

            responseData.AddRange(responseDataTempNoDuplicates);


            max += responseDataTempNoDuplicates.Count();

        }

        private static void ExtractSectionEmployer(List<FedTracing_RecType02> tracingIN02, List<FedTracing_RecTypeEmployer> tracingIN04,
                                                   List<TraceResponseData> responseData, string enfSrvCd, ref int max)
        {
            var responseDataTemp = new List<TraceResponseData>();

            var data = from d in tracingIN02
                       join emp in tracingIN04
                               on new { d.dat_Appl_EnfSrvCd, d.dat_Appl_CtrlCd } equals new { emp.dat_Appl_EnfSrvCd, emp.dat_Appl_CtrlCd }
                       into empResponse
                       where string.IsNullOrEmpty(d.dat_TrcSt_Cd?.Trim())
                       from r in empResponse
                       select new
                       {
                           r.dat_Appl_CtrlCd,
                           r.dat_Appl_EnfSrvCd,
                           r.dat_TrcRcp_EmplNme,
                           r.dat_TrcRcp_EmplNme1,
                           d.dat_TrcSt_Cd,
                           r.dat_TrcRsp_Addr_Ln,
                           r.dat_TrcRsp_Addr_Ln1,
                           r.dat_TrcRsp_Addr_CityNme,
                           r.dat_TrcRsp_Addr_PrvCd,
                           r.dat_TrcRsp_Addr_CtryCd,
                           r.dat_TrcRsp_Addr_PCd,
                           r.dat_TrcRsp_Addr_LstUpdte,
                           r.RecType
                       };

            int section4recordId = 1;

            foreach (var item in data)
            {
                var newResponse04 = new TraceResponseData
                {
                    Appl_EnfSrv_Cd = item.dat_Appl_EnfSrvCd,
                    Appl_CtrlCd = item.dat_Appl_CtrlCd,
                    EnfSrv_Cd = enfSrvCd,
                    TrcRsp_Rcpt_Dte = DateTime.Now,
                    TrcRsp_SeqNr = section4recordId++ + max,
                    TrcRsp_EmplNme = item.dat_TrcRcp_EmplNme,
                    TrcRsp_EmplNme1 = item.dat_TrcRcp_EmplNme1,
                    TrcSt_Cd = string.IsNullOrEmpty(item.dat_TrcSt_Cd.Trim()) ? "03" : item.dat_TrcSt_Cd,
                    TrcRsp_Addr_Ln = item.dat_TrcRsp_Addr_Ln,
                    TrcRsp_Addr_Ln1 = item.dat_TrcRsp_Addr_Ln1,
                    TrcRsp_Addr_CityNme = item.dat_TrcRsp_Addr_CityNme,
                    TrcRsp_Addr_PrvCd = item.dat_TrcRsp_Addr_PrvCd,
                    TrcRsp_Addr_CtryCd = item.dat_TrcRsp_Addr_CtryCd.Trim() == "CA" ? string.Empty : item.dat_TrcRsp_Addr_CtryCd,
                    TrcRsp_Addr_PCd = item.dat_TrcRsp_Addr_PCd,
                    TrcRsp_Addr_LstUpdte = item.dat_TrcRsp_Addr_LstUpdte,
                    AddrTyp_Cd = "E",
                    Prcs_RecType = item.RecType,
                    ActvSt_Cd = "C"
                };

                responseDataTemp.Add(newResponse04);
            }

            // remove duplicates
            var responseDataTemp2 = responseDataTemp
                .OrderByDescending(m => m.TrcRsp_Addr_LstUpdte)
                .GroupBy(m => new
                {
                    m.Appl_EnfSrv_Cd,
                    m.Appl_CtrlCd,
                    m.EnfSrv_Cd,
                    m.TrcRsp_EmplNme,
                    m.TrcRsp_EmplNme1,
                    m.TrcSt_Cd,
                    m.TrcRsp_Addr_Ln,
                    m.TrcRsp_Addr_Ln1,
                    m.TrcRsp_Addr_CityNme,
                    m.TrcRsp_Addr_PrvCd,
                    m.TrcRsp_Addr_CtryCd,
                    m.TrcRsp_Addr_PCd,
                    m.Prcs_RecType
                })
                .Select(g => g.First());

            responseData.AddRange(responseDataTemp2);

            max += responseDataTemp2.Count();
        }

        private static int ExtractSpecial(List<FedTracing_RecType02> tracingIN02, List<TraceResponseData> responseData,
                                         string enfSrvCd, int maxRsp, string traceState,
                                         string englishMessage, string frenchMessage)
        {
            var dataLocked = from d in tracingIN02
                             where d.dat_TrcSt_Cd == traceState
                             select new
                             {
                                 d.dat_Appl_CtrlCd,
                                 d.dat_Appl_EnfSrvCd,
                                 d.dat_TrcSt_Cd,
                                 d.RecType
                             };

            int section2recordId = 1;

            foreach (var itemLocked in dataLocked)
            {
                var newResponseLocked = new TraceResponseData
                {
                    Appl_EnfSrv_Cd = itemLocked.dat_Appl_EnfSrvCd,
                    Appl_CtrlCd = itemLocked.dat_Appl_CtrlCd,
                    EnfSrv_Cd = enfSrvCd,
                    TrcRsp_Rcpt_Dte = DateTime.Now,
                    TrcRsp_SeqNr = section2recordId++ + maxRsp,
                    TrcSt_Cd = itemLocked.dat_TrcSt_Cd,
                    TrcRsp_Addr_Ln = englishMessage,
                    TrcRsp_Addr_Ln1 = frenchMessage,
                    TrcRsp_Addr_CityNme = "",
                    AddrTyp_Cd = "R",
                    Prcs_RecType = itemLocked.RecType,
                    ActvSt_Cd = "C"
                };

                responseData.Add(newResponseLocked);
            }

            return section2recordId;
        }

        private static void UpdateTraceCycleNumber(List<TraceResponseData> responseData,
                                                   List<TraceCycleQuantityData> cycles)
        {
            // TODO: special process for NETP

            foreach (var row in responseData)
            {
                foreach (var cycleRow in cycles)
                {
                    if ((cycleRow.Appl_CtrlCd == row.Appl_CtrlCd) && (cycleRow.Appl_EnfSrv_Cd == row.Appl_EnfSrv_Cd))
                    {
                        row.TrcRsp_Trace_CyclNr = (short)cycleRow.Trace_Cycl_Qty;
                        break;
                    }
                }
            }

        }

    }
}
