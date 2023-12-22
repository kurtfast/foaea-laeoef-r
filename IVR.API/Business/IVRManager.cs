using FOAEA3.IVR.Data;
using FOAEA3.IVR.Data.DB;

namespace FOAEA3.IVR.Business
{
    public class IVRManager
    {
        //private IIVRRepository DB { get; }
       
        private DBIVR dbIVR { get; set; }

    //public IVRManager(IIVRRepository db)
    //{
    //    DB = db;
    //}
        public IVRManager()
        {
            dbIVR = new DBIVR();
        }

        public async Task<CheckSinReturnData> GetSinCount(CheckSinGetData data)
        {
            return await dbIVR.CheckSinCount(data);
        }

        public async Task<CheckCreditorIdReturnData> CheckCreditorId(CheckCreditorIdGetData data)
        {
            return await dbIVR.CheckCreditorId(data);
        }

        public async Task<CheckControlCodeReturnData> CheckControlCode(CheckControlCodeGetData data)
        {
            return await dbIVR.CheckControlCode(data);
        }

        public async Task<CheckDebtorIdReturnData> CheckDebtorId(CheckDebtorIdGetData data)
        {
            return await dbIVR.CheckDebtorId(data);
        }

        public async Task<CheckDebtorLetterReturnData> CheckDebtorLetter(CheckDebtorLetterGetData data)
        {
            return await dbIVR.CheckDebtorLetter(data);
        }

        public async Task<GetAgencyReturnData> GetAgency(GetAgencyGetData data)
        {
            return await dbIVR.GetAgency(data);
        }

        public async Task<GetAgencyDebReturnData> GetAgencyDeb(GetAgencyDebGetData data)
        {
            return await dbIVR.GetAgencyDeb(data);
        }

        public async Task<GetApplControlCodeReturnData> GetApplControlCode(GetApplControlCodeGetData data)
        {
            return await dbIVR.GetApplControlCode(data);
        }

        public async Task<GetApplEnforcementCodeReturnData> GetApplEnforcementCode(GetApplEnforcementCodeGetData data)
        {
            return await dbIVR.GetApplEnforcementCode(data);
        }

        public async Task<GetHoldbackConditionReturnData> GetHoldbackCondition(GetHoldbackConditionGetData data)
        {
            return await dbIVR.GetHoldbackCondition(data);
        }

        public async Task<GetL01AgencyReturnData> GetL01Agency(GetL01AgencyGetData data)
        {
            return await dbIVR.GetL01Agency(data);
        }

        public async Task<List<GetPaymentsReturnData>> GetPayments(GetPaymentsGetData data)
        {
            return await dbIVR.GetPayments(data);
        }

        public async Task<GetSummonsReturnData> GetSummons(GetSummonsGetData data)
        {
            return await dbIVR.GetSummons(data);
        }
    }
}
