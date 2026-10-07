using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class RoomBankDeatilsDTO
    {
        public long MI_Id { get; set; }
        public long HRMRBD_Id { get; set; }
        public long HRMBD_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long UserId { get; set; }
        public string HRMBD_BankName { get; set; }
        public string HRMR_RoomName { get; set; }
        public bool HRMRBD_ActiveFlag { get; set; }
        public Array rooms { get; set; }
        public Array banklist { get; set; }
        public Array get_levl { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; } 

    }
}

