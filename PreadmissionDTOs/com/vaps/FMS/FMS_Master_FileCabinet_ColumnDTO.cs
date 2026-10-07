using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMS_Master_FileCabinet_ColumnDTO
    {
        public long FMSMFCC_Id { get; set; }
        public long FMSMFCBT_Id { get; set; }
        public string FMSMFCC_CabinetColumnName { get; set; }
        public bool FMSMFCC_ActiveFlg { get; set; }
        public DateTime? FMSMFCC_CreatedDate { get; set; }
        public DateTime? FMSMFCC_UpdatedDate { get; set; }
        public long? FMSMFCC_CreatedBy { get; set; }
        public long? FMSMFCC_UpdatedBy { get; set; }
    }
}
