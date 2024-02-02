using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileBroker.Model
{
    public struct MEPSwearing_RecType01
    {
        [Required] public string RecType;
        [Required] public string Cycle;
        [Required] public string FileDate;
        [Required] public string EnfSrv_Cd;
    }

    public struct MEPSwearing_RecType61
    {
        [Required] public string RecType;
        [Required] public string EnfSrv_Cd;
        [Required] public string Appl_CtrlCd;
        [Required] public string Subm_Affdvt_SubmCd;
        [Required] public string Affdvt_Sworn_Dte;
    }

    public struct MEPSwearing_RecType99
    {
        [Required] public string RecType;
        [Required] public string CountOfDetailRecords;
    }

    public struct MEPSwearing_SwearingDataSet
    {
        [Required] public MEPSwearing_RecType01 Header;
        public List<MEPSwearing_RecType61> SwearingDetail;
        [Required] public MEPSwearing_RecType99 Trailer;
    }

    public struct MEPSwearing_SwearingDataSetSingle
    {
        [Required] public MEPSwearing_RecType01 Header;
        public MEPSwearing_RecType61 SwearingDetail;
        [Required] public MEPSwearing_RecType99 Trailer;
    }

    public class MEPSwearingFileData
    {
        [Required] public MEPSwearing_SwearingDataSet NewDataSet;
    }

    public class MEPSwearingFileDataSingle
    {
        [Required] public MEPSwearing_SwearingDataSetSingle NewDataSet;
    }

}
