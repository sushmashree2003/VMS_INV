using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMS_Master_FileCabinetDTO
    {
        public long FMSMFCBT_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMSMFCBT_FileCabinetName { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long FMSMFCBT_NoOfColumns { get; set; }
        public long FMSMFCBT_NoOfRows { get; set; }
        public bool FMSMFCBT_ActiveFlg { get; set; }
        public DateTime? FMSMFCBT_CreatedDate { get; set; }
        public DateTime? FMSMFCBT_UpdatedDate { get; set; }
        public long? FMSMFCBT_CreatedBy { get; set; }
        public long? FMSMFCBT_UpdatedBy { get; set; }
    }
}
