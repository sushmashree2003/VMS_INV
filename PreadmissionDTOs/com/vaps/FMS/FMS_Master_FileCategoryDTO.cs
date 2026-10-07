using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMS_Master_FileCategoryDTO
    {
        public long FMSMFC_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMSMFC_FileCategoryName { get; set; }
        public string FMSMFC_FileCategoryCode { get; set; }
        public bool FMSMFC_ActiveFlg { get; set; }
        public DateTime? FMSMFC_CreatedDate { get; set; }
        public DateTime? FMSMFC_UpdatedDate { get; set; }
        public long? FMSMFC_CreatedBy { get; set; }
        public long? FMSMFC_UpdatedBy { get; set; }
    }
}
