﻿using DBHelper;
using FOAEA3.Data.Base;
using FOAEA3.Model;
using FOAEA3.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOAEA3.Data.DB
{
    internal class DBFamilyProvision : DBbase, IFamilyProvisionRepository
    {
        public DBFamilyProvision(IDBTools mainDB) : base(mainDB)
        {
        }

        public List<FamilyProvisionData> GetFamilyProvisions()
        {
            return MainDB.GetAllData<FamilyProvisionData>("FamPro", FillDataFromReader);
        }

        private void FillDataFromReader(IDBHelperReader rdr, FamilyProvisionData data)
        {
            data.FamPro_Cd = rdr["FamPro_Cd"] as string;
            data.FamPro_Txt_E = rdr["FamPro_Txt_E"] as string;
            data.FamPro_Txt_F = rdr["FamPro_Txt_F"] as string;
            data.ActvSt_Cd = rdr["ActvSt_Cd"] as string;
        }
    }
}
