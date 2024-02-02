using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileBroker.Model
{
    public struct MEPInterception_RecType01
    {
        [Required] public string RecType;
        [Required] public string Cycle;
        [Required] public string FileDate;
        [Required] public string TermsAccepted;
    }

    public struct MEPInterception_RecType10
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

    public struct MEPInterception_RecType11
    {
        [Required] public string RecType;
        [Required] public string dat_Subm_SubmCd;
        [Required] public string dat_Appl_CtrlCd;
        [Required] public string dat_Appl_Dbtr_LngCd;
        [Required] public string dat_Appl_Dbtr_Addr_Ln;
        public string dat_Appl_Dbtr_Addr_Ln1;
        [Required] public string dat_Appl_Dbtr_Addr_CityNme;
        [Required] public string dat_Appl_Dbtr_Addr_CtryCd;
        [Required] public string dat_Appl_Dbtr_Addr_PCd;
        [Required] public string dat_Appl_Dbtr_Addr_PrvCd;
        [Required] public string dat_Appl_Crdtr_SurNme;
        [Required] public string dat_Appl_Crdtr_FrstNme;
        public string dat_Appl_Crdtr_MddleNme;
        public string dat_Appl_Crdtr_Brth_Dte;
    }

    public struct MEPInterception_RecType12
    {
        [Required] public string RecType;
        [Required] public string dat_Subm_SubmCd;
        [Required] public string dat_Appl_CtrlCd;
        public string dat_IntFinH_LmpSum_Money;
        public string dat_IntFinH_Perpym_Money;
        public string dat_PymPr_Cd;
        public string dat_IntFinH_CmlPrPym_Ind;
        public string dat_IntFinH_NextRecalc_Dte;
        [Required] public string dat_HldbCtg_Cd;
        public string dat_IntFinH_DfHldbPrcnt;
        public string dat_IntFinH_DefHldbAmn_Money;
        public string dat_IntFinH_DefHldbAmn_Period;
        public string dat_IntFinH_VarIss_Dte;
    }

    public struct MEPInterception_RecType13
    {
        [Required] public string RecType;
        [Required] public string dat_Subm_SubmCd;
        [Required] public string dat_Appl_CtrlCd;
        [Required] public string dat_EnfSrv_Cd;
        [Required] public string dat_HldbCtg_Cd;
        public string dat_HldbCnd_SrcHldbPrcnt;
        public string dat_HldbCnd_Hldb_Amn_Money;
        public string dat_HldbCnd_MxmPerChq_Money;
    }

    public struct MEPInterception_RecType99
    {
        [Required] public string RecType;
        [Required] public string ResponseCnt;
    }

    public struct MEPInterception_InterceptionDataSet
    {
        [Required] public MEPInterception_RecType01 INTAPPIN01;
        public List<MEPInterception_RecType10> INTAPPIN10;
        public List<MEPInterception_RecType11> INTAPPIN11;
        public List<MEPInterception_RecType12> INTAPPIN12;
        public List<MEPInterception_RecType13> INTAPPIN13;
        [Required] public MEPInterception_RecType99 INTAPPIN99;
    }

    public struct MEPInterception_InterceptionDataSetSingle
    {
        [Required] public MEPInterception_RecType01 INTAPPIN01;
        public MEPInterception_RecType10 INTAPPIN10;
        public MEPInterception_RecType11 INTAPPIN11;
        public MEPInterception_RecType12 INTAPPIN12;
        public List<MEPInterception_RecType13> INTAPPIN13;
        [Required] public MEPInterception_RecType99 INTAPPIN99;
    }

    public struct MEPInterception_InterceptionDataSetSingleSource
    {
        [Required] public MEPInterception_RecType01 INTAPPIN01;
        public MEPInterception_RecType10 INTAPPIN10;
        public MEPInterception_RecType11 INTAPPIN11;
        public MEPInterception_RecType12 INTAPPIN12;
        public MEPInterception_RecType13 INTAPPIN13;
        [Required] public MEPInterception_RecType99 INTAPPIN99;
    }

    public struct MEPInterception_InterceptionDataSetNoSource
    {
        [Required] public MEPInterception_RecType01 INTAPPIN01;
        public MEPInterception_RecType10 INTAPPIN10;
        public MEPInterception_RecType11 INTAPPIN11;
        public MEPInterception_RecType12 INTAPPIN12;
        [Required] public MEPInterception_RecType99 INTAPPIN99;
    }

    public class MEPInterceptionFileData
    {
        [Required] public MEPInterception_InterceptionDataSet NewDataSet;

        public MEPInterceptionFileData()
        {
            NewDataSet.INTAPPIN10 = new List<MEPInterception_RecType10>();
            NewDataSet.INTAPPIN11 = new List<MEPInterception_RecType11>();
            NewDataSet.INTAPPIN12 = new List<MEPInterception_RecType12>();
            NewDataSet.INTAPPIN13 = new List<MEPInterception_RecType13>();
        }
    }

    public class MEPInterceptionFileDataSingle
    {
        [Required] public MEPInterception_InterceptionDataSetSingle NewDataSet;

        public MEPInterceptionFileDataSingle()
        {
            NewDataSet.INTAPPIN13 = new List<MEPInterception_RecType13>();
        }
    }

    public class MEPInterceptionFileDataSingleSource
    {
        [Required] public MEPInterception_InterceptionDataSetSingleSource NewDataSet;
    }

    public class MEPInterceptionFileDataNoSource
    {
        [Required] public MEPInterception_InterceptionDataSetNoSource NewDataSet;
    }

}
