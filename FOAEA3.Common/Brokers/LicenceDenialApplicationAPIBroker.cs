﻿using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using FOAEA3.Model.Interfaces.Broker;
using FOAEA3.Resources.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOAEA3.Common.Brokers
{
    public class LicenceDenialApplicationAPIBroker : ILicenceDenialApplicationAPIBroker, IVersionSupport
    {
        private IAPIBrokerHelper ApiHelper { get; }

        public LicenceDenialApplicationAPIBroker(IAPIBrokerHelper apiHelper)
        {
            ApiHelper = apiHelper;
        }

        public async Task<string> GetVersionAsync()
        {
            string apiCall = $"api/v1/licencedenials/Version";
            return await ApiHelper.GetStringAsync(apiCall, maxAttempts: 1);
        }

        public async Task<string> GetConnectionAsync()
        {
            string apiCall = $"api/v1/licencedenials/DB";
            return await ApiHelper.GetStringAsync(apiCall, maxAttempts: 1);
        }

        public async Task<LicenceDenialApplicationData> GetApplicationAsync(string dat_Appl_EnfSrvCd, string dat_Appl_CtrlCd)
        {
            string key = ApplKey.MakeKey(dat_Appl_EnfSrvCd, dat_Appl_CtrlCd);
            string apiCall = $"api/v1/licencedenials/{key}";
            return await ApiHelper.GetDataAsync<LicenceDenialApplicationData>(apiCall);
        }

        public async Task<List<LicenceDenialOutgoingFederalData>> GetOutgoingFederalLicenceDenialRequestsAsync(int maxRecords,
                                                                                  string activeState,
                                                                                  int lifeState,
                                                                                  string enfServiceCode)
        {
            string baseCall = "api/v1/OutgoingFederalLicenceDenialRequests";
            string apiCall = $"{baseCall}?maxRecords={maxRecords}&activeState={activeState}" +
                                        $"&lifeState={lifeState}&enfServiceCode={enfServiceCode}";
            return await ApiHelper.GetDataAsync<List<LicenceDenialOutgoingFederalData>>(apiCall);
        }

        public async Task<List<LicenceDenialToApplData>> GetLicenceDenialToApplDataAsync(string fedSource)
        {
            string apiCall = $"api/v1/licenceDenials/LicenceDenialToApplication?federalSource={fedSource}";
            return await ApiHelper.GetDataAsync<List<LicenceDenialToApplData>>(apiCall);
        }

        public async Task<LicenceDenialApplicationData> ProcessLicenceDenialResponseAsync(string appl_EnfSrv_Cd, string appl_CtrlCd)
        {
            var appData = new LicenceDenialApplicationData
            {
                Appl_EnfSrv_Cd = appl_EnfSrv_Cd,
                Appl_CtrlCd = appl_CtrlCd,
                Subm_SubmCd = "MSGBRO"
            };
            string key = ApplKey.MakeKey(appl_EnfSrv_Cd, appl_CtrlCd);
            string apiCall = $"api/v1/licenceDenials/{key}/ProcessLicenceDenialResponse";
            return await ApiHelper.PutDataAsync<LicenceDenialApplicationData, LicenceDenialApplicationData>(apiCall, appData);
        }

        public async Task<List<LicenceDenialOutgoingProvincialData>> GetOutgoingProvincialLicenceDenialDataAsync(int maxRecords, string activeState,
                                                                                                string recipientCode)
        {
            string baseCall = "api/v1/OutgoingProvincialLicenceDenialResults";
            string apiCall = $"{baseCall}?maxRecords={maxRecords}&activeState={activeState}" +
                                        $"&recipientCode={recipientCode}&isXML=true";
            return await ApiHelper.GetDataAsync<List<LicenceDenialOutgoingProvincialData>>(apiCall);
        }

    }
}