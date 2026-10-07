using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class Lead_To_ClientDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array leadlist_dd { get; set; }
        public Array project_list { get; set; }
        public Array get_department { get; set; }
        public long HRMD_Id { get; set; }
        public Array module_list { get; set; }
        public long ISMMPR_Id { get; set; }
        public Array components_dd { get; set; }
        public components_ddlist[] complist { get; set; }
    }
    public class components_ddlist
    {
        public long ISMCLTC_Id { get; set; }
    }
}
