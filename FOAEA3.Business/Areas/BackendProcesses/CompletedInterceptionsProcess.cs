﻿using FOAEA3.Business.Areas.Application;
using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using FOAEA3.Model.Interfaces.Repository;
using System.Threading.Tasks;

namespace FOAEA3.Business.BackendProcesses
{
    public class CompletedInterceptionsProcess
    {
        private readonly IRepositories DB;
        private readonly IRepositories_Finance DBfinance;
        private readonly IFoaeaConfigurationHelper Config;

        public CompletedInterceptionsProcess(IRepositories repositories, IRepositories_Finance repositoriesFinance,
                                             IFoaeaConfigurationHelper config)
        {
            DB = repositories;
            DBfinance = repositoriesFinance;
            Config = config;
        }

        public async Task RunAsync()
        {
            var prodAudit = DB.ProductionAuditTable;

            await prodAudit.InsertAsync("Completed I01 Process", "Completed I01 Process Started", "O");

            var applTerminated = await DB.InterceptionTable.GetTerminatedI01Async();

            foreach (var appl in applTerminated)
            {
                var manager = new InterceptionManager((InterceptionApplicationData)appl, DB, DBfinance, Config);
                await manager.CompleteApplication();
            }

            await prodAudit.InsertAsync("Completed I01 Process", "Completed I01 Process Completed", "O");
        }
    }
}
