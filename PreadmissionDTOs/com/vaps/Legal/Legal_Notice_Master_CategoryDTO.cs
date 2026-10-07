using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Legal
{
    public class Legal_Notice_Master_CategoryDTO
    {
        public long LNC_Id { get; set; }
        public string LNC_Category_Name { get; set; }
        public bool LNC_ActiveFlag { get; set; }
        public DateTime? LNC_CreatedDate { get; set; }
        public DateTime? LNC_UpdatedDate { get; set; }
        public long LNC_CreatedBy { get; set; }
        public long LNC_UpdatedBy { get; set; }
    }
}
