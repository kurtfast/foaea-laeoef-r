﻿using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using FOAEA3.Model.Interfaces.Broker;
using System;
using System.Threading.Tasks;

namespace FOAEA3.Common.Brokers
{
    public class ProductionAuditAPIBroker : IProductionAuditAPIBroker
    {
        private IAPIBrokerHelper ApiHelper { get; }

        public ProductionAuditAPIBroker(IAPIBrokerHelper apiHelper)
        {
            ApiHelper = apiHelper;
        }

        public async Task InsertAsync(string processName, string description, string audience, DateTime? completedDate = null)
        {
            var productionAuditData = new ProductionAuditData
            {
                Process_name = processName,
                Description = description,
                Audience = audience,
                Compl_dte = completedDate
            };

            _ = await ApiHelper.PostDataAsync<ProductionAuditData, ProductionAuditData>("api/v1/productionAudits",
                                                                                  productionAuditData);
        }

        public async Task InsertAsync(ProductionAuditData productionAuditData)
        {
            _ = await ApiHelper.PostDataAsync<ProductionAuditData, ProductionAuditData>("api/v1/productionAudits",
                                                                                  productionAuditData);
        }
    }
}
