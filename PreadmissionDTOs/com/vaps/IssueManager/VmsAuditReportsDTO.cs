using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class VmsAuditReportsDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array MI_List { get; set; }
       public bool roleflag { get; set; }
        public long IVRMRT_Id { get; set; }
        public Array departmentdropdown { get; set; }
        public comapanylist[] comapanylist { get; set; }
    }
    public class comapanylist
    {
        public long MI_Id { get; set; }
    } 
}
