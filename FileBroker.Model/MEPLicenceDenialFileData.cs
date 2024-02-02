using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileBroker.Model
{
    public struct MEPLicenceDenial_RecType01
    {
        [Required] public string RecType;
        [Required] public string Cycle;
        [Required] public string FileDate;
        [Required] public string TermsAccepted;
    }

    public struct MEPLicenceDenial_RecTypeBase
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

    public struct MEPLicenceDenial_RecType31
    {
        [Required] public string RecType;
        [Required] public string dat_Subm_SubmCd;
        [Required] public string dat_Appl_CtrlCd;

        public string dat_Appl_Dbtr_LngCd;
        [Required] public string dat_LicSup_NoticeSntTDbtr_Dte;
        public string dat_LicSup_Dbtr_Brth_CtryCd;
        public string dat_LicSup_Dbtr_Brth_CityNme;
        public string dat_LicSup_Dbtr_EyesColorCd;
        public string dat_LicSup_Dbtr_HeightUOMCd;
        public string dat_LicSup_Dbtr_HeightQty;
        public string dat_LicSup_Dbtr_PhoneNumber;
        public string dat_LicSup_Dbtr_EmailAddress;
        [Required] public string dat_Appl_Dbtr_Addr_Ln;
        public string dat_Appl_Dbtr_Addr_Ln1;
        [Required] public string dat_Appl_Dbtr_Addr_CityNme;
        [Required] public string dat_Appl_Dbtr_Addr_PrvCd;
        [Required] public string dat_Appl_Dbtr_Addr_CtryCd;
        [Required] public string dat_Appl_Dbtr_Addr_PCd;
        public string dat_LicSup_Dbtr_EmplNme;
        public string dat_LicSup_Dbtr_EmplAddr_Ln;
        public string dat_LicSup_Dbtr_EmplAddr_Ln1;
        public string dat_LicSup_Dbtr_EmplAddr_CtyNme;
        public string dat_LicSup_Dbtr_EmplAddr_PrvCd;
        public string dat_LicSup_Dbtr_EmplAddr_CtryCd;
        public string dat_LicSup_Dbtr_EmplAddr_PCd;

        [Required] public string dat_LicSup_SupportOrder_Dte;
        [Required] public string dat_LicSup_CourtNme;
        [Required] public string dat_Appl_Crdtr_SurNme;
        [Required] public string dat_Appl_Crdtr_FrstNme;
        public string dat_Appl_Crdtr_MddleNme;
        [Required] public string dat_LicSup_PymPr_Cd;
        public string dat_LicSup_NrOfPymntsInDefault;
        public string dat_LicSup_AmntOfArrears;
        [Required] public string dat_LicSup_Declaration;
    }

    public struct MEPLicenceDenial_RecType41
    {
        [Required] public string RecType;
        [Required] public string dat_Subm_SubmCd;
        [Required] public string dat_Appl_CtrlCd;

        [Required] public string dat_Appl_Dbtr_Last_Addr_Ln;
        public string dat_Appl_Dbtr_Last_Addr_Ln1;
        [Required] public string dat_Appl_Dbtr_Last_Addr_CityNme;
        [Required] public string dat_Appl_Dbtr_Last_Addr_PrvCd;
        [Required] public string dat_Appl_Dbtr_Last_Addr_CtryCd;
        [Required] public string dat_Appl_Dbtr_Last_Addr_PCd;

        [Required] public string RefSusp_Issuing_SubmCd;
        [Required] public string RefSusp_Appl_CtrlNr;
    }

    public struct MEPLicenceDenial_RecType99
    {
        [Required] public string RecType;
        [Required] public string ResponseCnt;
    }

    public struct MEPLicenceDenial_LicenceDenialDataSet
    {
        [Required] public MEPLicenceDenial_RecType01 LICAPPIN01;
        public List<MEPLicenceDenial_RecTypeBase> LICAPPIN30;
        public List<MEPLicenceDenial_RecType31> LICAPPIN31;
        public List<MEPLicenceDenial_RecTypeBase> LICAPPIN40;
        public List<MEPLicenceDenial_RecType41> LICAPPIN41;
        [Required] public MEPLicenceDenial_RecType99 LICAPPIN99;
    }

    public struct MEPLicenceDenial_LicenceDenialDataSetSingle
    {
        [Required] public MEPLicenceDenial_RecType01 LICAPPIN01;
        public MEPLicenceDenial_RecTypeBase LICAPPIN30;
        public MEPLicenceDenial_RecType31 LICAPPIN31;
        public MEPLicenceDenial_RecTypeBase LICAPPIN40;
        public MEPLicenceDenial_RecType41 LICAPPIN41;
        [Required] public MEPLicenceDenial_RecType99 LICAPPIN99;
    }


    public class MEPLicenceDenialFileData
    {
        [Required] public MEPLicenceDenial_LicenceDenialDataSet NewDataSet;

        public MEPLicenceDenialFileData()
        {
            NewDataSet.LICAPPIN30 = new List<MEPLicenceDenial_RecTypeBase>();
            NewDataSet.LICAPPIN31 = new List<MEPLicenceDenial_RecType31>();
            NewDataSet.LICAPPIN40 = new List<MEPLicenceDenial_RecTypeBase>();
            NewDataSet.LICAPPIN41 = new List<MEPLicenceDenial_RecType41>();
        }
    }

    public class MEPLicenceDenialFileDataSingle
    {
        [Required] public MEPLicenceDenial_LicenceDenialDataSetSingle NewDataSet;
    }
}
