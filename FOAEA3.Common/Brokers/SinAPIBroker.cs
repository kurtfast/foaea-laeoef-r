﻿using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using System.Collections.Generic;

namespace FOAEA3.Common.Brokers
{
    public class SinAPIBroker : ISinAPIBroker
    {
        private IAPIBrokerHelper ApiHelper { get; }

        public SinAPIBroker(IAPIBrokerHelper apiHelper)
        {
            ApiHelper = apiHelper;
        }

        public List<SINOutgoingFederalData> GetOutgoingFederalSins(int maxRecords, string activeState,
                                                                   int lifeState, string enfServiceCode)
        {
            string baseCall = "api/v1/OutgoingFederalSins";
            string apiCall = $"{baseCall}?maxRecords={maxRecords}&activeState={activeState}" +
                             $"&lifeState={lifeState}&enfServiceCode={enfServiceCode}";
            return ApiHelper.GetDataAsync<List<SINOutgoingFederalData>>(apiCall).Result;
        }

    }
}
