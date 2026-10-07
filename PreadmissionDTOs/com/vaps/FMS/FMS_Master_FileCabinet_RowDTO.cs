using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMS_Master_FileCabinet_RowDTO
    {
        public long FMSMFCR_Id { get; set; }
        public long FMSMFCBT_Id { get; set; }
        public string FMSMFCR_CabinetRowName { get; set; }
        public bool FMSMFCR_ActiveFlg { get; set; }
        public DateTime? FMSMFCR_CreatedDate { get; set; }
        public DateTime? FMSMFCR_UpdatedDate { get; set; }
        public long? FMSMFCR_CreatedBy { get; set; }
        public long? FMSMFCR_UpdatedBy { get; set; }
    }
}
