﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOAEA3.Model.Interfaces.Broker
{
    public interface IFinancialAPIBroker
    {
        IAPIBrokerHelper ApiHelper { get; }
        string Token { get; set; }

        Task<List<CR_PADReventData>> GetActiveCR_PADReventsAsync(string enfSrv);
        Task CloseCR_PADReventsAsync(string batchId, string enfSrv);
        Task<List<BlockFundData>> GetBlockFundsAsync(string enfSrv);
        Task<List<IFMSdata>> GetIFMSasync(string batchId);
        Task CloseControlBatch(string batchId);
    }
}
