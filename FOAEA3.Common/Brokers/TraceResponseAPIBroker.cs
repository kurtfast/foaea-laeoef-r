﻿using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOAEA3.Common.Brokers
{
    public class TraceResponseAPIBroker : ITraceResponseAPIBroker
    {
        private IAPIBrokerHelper ApiHelper { get; }

        public TraceResponseAPIBroker(IAPIBrokerHelper apiHelper)
        {
            ApiHelper = apiHelper;
        }

        public async Task InsertBulkDataAsync(List<TraceResponseData> responseData)
        {
            _ = await ApiHelper.PostDataAsync<TraceResponseData, List<TraceResponseData>>("api/v1/traceResponses/bulk",
                                                                                    responseData);
        }

        public async Task MarkTraceResultsAsViewedAsync(string enfService)
        {
            await ApiHelper.SendCommandAsync("api/v1/traceResponses/MarkResultsAsViewed?enfService=" + enfService);
        }
    }
}
