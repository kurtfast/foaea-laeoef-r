﻿using FOAEA3.Business.Areas.Application;
using FOAEA3.Model.Interfaces;
using FOAEA3.Model.Interfaces.Repository;
using System.Threading.Tasks;

namespace FOAEA3.Business.Areas.BackendProcesses
{
    public class FTBatchWithoutTransactionNoticeProcess
    {
        private readonly IFoaeaConfigurationHelper Config;
        private readonly IRepositories DB;
        private readonly IRepositories_Finance DBfinance;

        public FTBatchWithoutTransactionNoticeProcess(IRepositories repositories, IRepositories_Finance repositories_finance,
                                                      IFoaeaConfigurationHelper config)
        {
            Config = config;
            DB = repositories;
            DBfinance = repositories_finance;
        }

        public async Task RunAsync()
        {
            var prodAudit = DB.ProductionAuditTable;

            await prodAudit.InsertAsync("FT Batch Without Transaction Notice Process", $"FT Batch Without Transaction Notice Process Started", "O");

            var interceptionManager = new InterceptionManager(DB, DBfinance, Config);

            await interceptionManager.FTBatchNotification_CheckFTTransactionsAdded();

            await prodAudit.InsertAsync("FT Batch Without Transaction Notice Process", $"FT Batch Without Transaction Notice Process Completed", "O");
        }
    }
}