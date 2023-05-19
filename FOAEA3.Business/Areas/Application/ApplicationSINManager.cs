﻿using FOAEA3.Resources.Helpers;
using FOAEA3.Model;
using FOAEA3.Model.Base;
using FOAEA3.Model.Enums;
using FOAEA3.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FOAEA3.Business.Areas.Application
{
    internal class ApplicationSINManager
    {
        private IRepositories Repositories { get; }

        private ApplicationData Application { get; }
        private ApplicationManager ApplicationManager { get; }
        private ApplicationEventManager EventManager { get => ApplicationManager.EventManager; }

        public ApplicationSINManager(ApplicationData application, ApplicationManager manager)
        {
            Repositories = manager.Repositories;
            Application = application;
            ApplicationManager = manager;
        }

        public List<SINOutgoingFederalData> GetFederalOutgoingData(int maxRecords, string activeState, ApplicationState lifeState,
                                                                   string enfServiceCode)
        {
            var sinDB = Repositories.SINResultRepository;
            return sinDB.GetFederalSINOutgoingData(maxRecords, activeState, lifeState, enfServiceCode);
        }

        public void SINconfirmationBypass(string applDbtrEntrdSIN, string lastUpdateUser, bool swapNames = false,
                                          string HRDCcomments = "")
        {
            Application.Appl_LastUpdate_Usr = lastUpdateUser;
            Application.Appl_LastUpdate_Dte = DateTime.Now;

            if (Application.AppLiSt_Cd == ApplicationState.SIN_NOT_CONFIRMED_5)
            {
                EventManager.AddEvent(EventCode.C51116_SIN_HAS_BEEN_MANUALLY_CONFIRMED);
                EventManager.SaveEvents();

                SINconfirmation(true, applDbtrEntrdSIN, lastUpdateUser, swapNames, HRDCcomments);
            }
            else
            {
                EventManager.AddEvent(EventCode.C50933_INVALID_OPERATION_FROM_THE_CURRENT_LIFE_STATE, $"Inv. action {(int)Application.AppLiSt_Cd} <> 5");
                EventManager.SaveEvents();

                return;
            }

            ApplicationManager.LoadApplication(Application.Appl_EnfSrv_Cd, Application.Appl_CtrlCd);

            Application.Messages.AddInformation(EventCode.C50620_VALID_APPLICATION);

        }

        private void SINconfirmation(bool isSinConfirmed, string confirmedSin, string lastUpdateUser, bool swapNames = false,
                                     string HRDCcomments = "")
        {

            if (swapNames)
            {
                string surname = Application.Appl_Dbtr_SurNme;
                Application.Appl_Dbtr_SurNme = Application.Appl_Dbtr_FrstNme;
                Application.Appl_Dbtr_FrstNme = surname;
            }

            Application.Appl_LastUpdate_Usr = lastUpdateUser;
            Application.Appl_LastUpdate_Dte = DateTime.Now;

            if (Application.AppLiSt_Cd.NotIn(ApplicationState.SIN_CONFIRMATION_PENDING_3, ApplicationState.SIN_NOT_CONFIRMED_5))
            {
                EventManager.AddEvent(EventCode.C50933_INVALID_OPERATION_FROM_THE_CURRENT_LIFE_STATE, $"Inv. action {(int)Application.AppLiSt_Cd} <>3, <>5");
                EventManager.SaveEvents();

                return;
            }

            if (isSinConfirmed)
            {
                if (ValidationHelper.IsValidSinNumberMod10(confirmedSin))
                {
                    Application.Appl_Dbtr_Cnfrmd_SIN = confirmedSin;
                    Application.Appl_SIN_Cnfrmd_Ind = 1;

                    ApplicationManager.SetNewStateTo(ApplicationState.SIN_CONFIRMED_4);
                }
                else
                {
                    EventManager.AddEvent(EventCode.C50523_INVALID_SIN, appState: ApplicationState.INVALID_APPLICATION_1);

                    ApplicationManager.SetNewStateTo(ApplicationState.INVALID_APPLICATION_1);
                }
            }
            else
            {
                Application.Appl_Dbtr_Cnfrmd_SIN = null;
                Application.Appl_SIN_Cnfrmd_Ind = 0;

                ApplicationManager.SetNewStateTo(ApplicationState.SIN_NOT_CONFIRMED_5);
            }

            ApplicationManager.MakeUpperCase();
            ApplicationManager.UpdateApplicationNoValidation();

            UpdateSINChangeHistory(HRDCcomments);
           
            EventManager.SaveEvents();
        }

        public void UpdateSINChangeHistory(string HRDCcomments = "")
        {
            if (Application.Appl_SIN_Cnfrmd_Ind == 1)
            {
                var sinChangeHistoryData = new SINChangeHistoryData
                {
                    Appl_CtrlCd = Application.Appl_CtrlCd,
                    Appl_EnfSrv_Cd = Application.Appl_EnfSrv_Cd,
                    Appl_Dbtr_SurNme = Application.Appl_Dbtr_SurNme,
                    Appl_Dbtr_FrstNme = Application.Appl_Dbtr_FrstNme,
                    Appl_Dbtr_MddleNme = Application.Appl_Dbtr_MddleNme,
                    AppLiSt_Cd = (int)Application.AppLiSt_Cd,
                    Appl_Dbtr_Cnfrmd_SIN = Application.Appl_Dbtr_Cnfrmd_SIN
                };

                if (!string.IsNullOrEmpty(HRDCcomments) && (HRDCcomments.Contains("ESDC")))
                    sinChangeHistoryData.SINChangeHistoryUser = "ESDC_XLSFile";
                else
                    sinChangeHistoryData.SINChangeHistoryUser = Application.Appl_LastUpdate_Usr;

                if (!string.IsNullOrEmpty(HRDCcomments))
                    sinChangeHistoryData.SINChangeHistoryComment = HRDCcomments;
                else
                {
                    sinChangeHistoryData.SINChangeHistoryComment = "SIN Confirmation";
                    EventManager.AddEvent(EventCode.C50650_SIN_CONFIRMED, eventReasonText: GetSINResultsEventText());
                }

                Repositories.SINChangeHistoryRepository.CreateSINChangeHistory(sinChangeHistoryData);
            }

        }


        public string GetSINResultsEventText()
        {
            string result = string.Empty;

            var data = Repositories.SINResultRepository.GetSINResults(Application.Appl_EnfSrv_Cd, Application.Appl_CtrlCd);
            var sinData = data.Items.FirstOrDefault();

            if (sinData != null)
            {
                result = $"{sinData.SVR_DOB_TolCd}{sinData.SVR_GvnNme_TolCd}{sinData.SVR_MddlNme_TolCd}" +
                         $"{sinData.SVR_SurNme_TolCd}{sinData.SVR_MotherNme_TolCd}{sinData.SVR_Gendr_TolCd}";
                result += ((sinData.ValStat_Cd == 0) || (sinData.ValStat_Cd == 10)) ? "Y" : "N";
            }

            return result;
        }

        public DataList<SINResultData> GetSINResults()
        {
            return Repositories.SINResultRepository.GetSINResults(Application.Appl_EnfSrv_Cd, Application.Appl_CtrlCd);
        }

        public DataList<SINResultWithHistoryData> GetSINResultsWithHistory()
        {
            return Repositories.SINResultRepository.GetSINResultsWithHistory(Application.Appl_EnfSrv_Cd, Application.Appl_CtrlCd);
        }

        public void CreateResultData(List<SINResultData> resultData)
        {
            var responsesDB = Repositories.SINResultRepository;
            responsesDB.InsertBulkData(resultData);
        }

        public string GetSinForApplication()
        {
            string sin = string.Empty;

            if (Application.Appl_SIN_Cnfrmd_Ind == 1)
            {
                if (!string.IsNullOrEmpty(Application.Appl_Dbtr_Cnfrmd_SIN))
                    sin = Application.Appl_Dbtr_Cnfrmd_SIN;
            }
            else if (!string.IsNullOrEmpty(Application.Appl_Dbtr_Entrd_SIN))
                sin = Application.Appl_Dbtr_Entrd_SIN;

            return sin;
        }

    }
}
