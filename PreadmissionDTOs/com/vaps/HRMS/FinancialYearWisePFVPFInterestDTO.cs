using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class FinancialYearWisePFVPFInterestDTO
    {
        public Array GetMaterTypeEditData { get; set; }
        public Array GetMasterOptionLoadData { get; set; }
        public string retrunMsg { get; set; }
        public long MI_Id { get; set; }
        public Array leaveyeardropdown { get; set; }
        public decimal HRMPFVPFINT_PFInterestRate { get; set; }
        public decimal HRMPFVPFINT_VPFInterestRate { get; set; }
        public string IMFY_FinancialYear { get; set; }
        public long HRMPFVPFINT_Id { get; set; }
        public bool HRMPFVPFINT_ActiveFlg { get; set; }
        public string message { get; set; }
        public long IMFY_Id { get; set; }
        public DateTime HRMPFVPFINT_CreatedDate { get; set; }
        public DateTime HRMPFVPFINT_UpdatedDate { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        //public Array financialyear { get; set; }


        public Array get_store { get; set; }

    }
}
