﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOAEA3.Model.Interfaces.Broker
{
    public interface ILicenceDenialEventAPIBroker
    {
        Task<List<ApplicationEventData>> GetRequestedLICINEventsAsync(string enfSrvCd, string appl_EnfSrv_Cd, string appl_CtrlCd);
        Task<List<ApplicationEventDetailData>> GetRequestedLICINEventDetailsAsync(string enfSrvCd, string appl_EnfSrv_Cd, string appl_CtrlCd);

    }
}