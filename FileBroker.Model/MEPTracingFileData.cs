using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileBroker.Model
{
    public struct MEPTracing_RecType01
    {
        [Required] public string RecType;
        [Required] public string Cycle;
        [Required] public string FileDate;
        [Required] public string TermsAccepted;
    }

    public struct MEPTracing_RecType20
    {
        [Required] public string RecType;
        [Required] public string dat_Subm_SubmCd;
        [Required] public string dat_Appl_CtrlCd;
        [Required] public string dat_Appl_Source_RfrNr;
        [Required] public string dat_Appl_EnfSrvCd;
        [Required] public string dat_Subm_Rcpt_SubmCd;
        [Required] public string dat_Appl_Lgl_Dte;
        [Required] public string dat_Appl_Dbtr_SurNme;
        [Required] public string dat_Appl_Dbtr_FrstNme;
        public string dat_Appl_Dbtr_MddleNme;
        [Required] public string dat_Appl_Dbtr_Brth_Dte;
        [Required] public string dat_Appl_Dbtr_Gendr_Cd;
        public string dat_Appl_Dbtr_Entrd_SIN;
        public string dat_Appl_Dbtr_Parent_SurNme_Birth;
        public string dat_Appl_CommSubm_Text;
        [Required] public string dat_Appl_Rcptfrm_dte;
        [Required] public string dat_Appl_AppCtgy_Cd;
        [Required] public string dat_Appl_Group_Batch_Cd;
        [Required] public string dat_Appl_Medium_Cd;
        [Required] public string dat_Appl_Affdvt_Doc_TypCd;
        [Required] public string dat_Appl_Reas_Cd;
        public string dat_Appl_Reactv_Dte;
        [Required] public string dat_Appl_LiSt_Cd;
        [Required] public string Maintenance_ActionCd;
        public string dat_New_Owner_RcptSubmCd;
        public string dat_New_Owner_SubmCd;
        public string dat_Update_SubmCd;
    }

    public struct MEPTracing_RecType21
    {
        [Required] public string RecType;
        [Required] public string dat_Subm_SubmCd;
        [Required] public string dat_Appl_CtrlCd;
        [Required] public string dat_Appl_Crdtr_SurNme;
        [Required] public string dat_Appl_Crdtr_FrstNme;
        public string dat_Appl_Crdtr_MddleNme;
        public string dat_FamPro_Cd;
        public string dat_Trace_Child_Text;
        public string dat_Trace_Breach_Text;
        public string dat_Trace_ReasGround_Text;
        public string dat_InfoBank_Cd;
        public string dat_Statute_Cd;
    }

    public struct Tax_Data_Info
    {
        public string Tax_Year;
        public List<string> Tax_Form;
    }

    public struct FinancialDetails
    {
        public List<Tax_Data_Info> Tax_Data;
    }

    public struct MEPTracing_RecType22
    {
        [Required] public string RecType;
        [Required] public string dat_Subm_SubmCd;
        [Required] public string dat_Appl_CtrlCd;
        [Required] public string dat_Trace_Dbtr_PhoneNumber;
        [Required] public string dat_Trace_Dbtr_EmailAddress;
        [Required] public string dat_Trace_Declaration;
        [Required] public string dat_Tracing_Info;
        [Required] public string dat_SIN_Information;
        [Required] public string dat_Financial_Information;
        public FinancialDetails dat_Financial_Details;
    }

    public struct MEPTracing_RecType99
    {
        [Required] public string RecType;
        [Required] public string ResponseCnt;
    }

    public struct MEPTracing_TracingDataSet
    {
        [Required] public MEPTracing_RecType01 TRCAPPIN01;
        public List<MEPTracing_RecType20> TRCAPPIN20;
        public List<MEPTracing_RecType21> TRCAPPIN21;
        public List<MEPTracing_RecType22> TRCAPPIN22;
        [Required] public MEPTracing_RecType99 TRCAPPIN99;
    }

    public struct MEPTracing_TracingDataSetSingle
    {
        [Required] public MEPTracing_RecType01 TRCAPPIN01;
        public MEPTracing_RecType20 TRCAPPIN20;
        public MEPTracing_RecType21 TRCAPPIN21;
        public MEPTracing_RecType22 TRCAPPIN22;
        [Required] public MEPTracing_RecType99 TRCAPPIN99;
    }

    public class MEPTracingFileData
    {
        [Required] public MEPTracing_TracingDataSet NewDataSet;
    }

    public class MEPTracingFileDataSingle
    {
        [Required] public MEPTracing_TracingDataSetSingle NewDataSet;
    }

}
