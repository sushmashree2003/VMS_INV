using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class ISM_Inbound_Outbound_Call_DetailsDTO
    {
        public long ISMIOC_Id { get; set; }
        public DateTime ISMIOC_Date { get; set; }
        public string ISMIOC_From { get; set; }
        public string ISMIOC_To { get; set; }
        public string ISMIOC_Purpose { get; set; }
        public string ISMIOC_Name { get; set; }
        public string ISMIOC_Remarks { get; set; }
        public string ISMIOC_TransferToName { get; set; }
        public long ISMIOC_UserId { get; set; }
        public string ISMIOC_InOutFlag { get; set; }
        public DateTime? ISMIOC_CreatedDate { get; set; }
        public DateTime? ISMIOC_UpdatedDate { get; set; }
        public Array inoutboundlist { get; set; }
        public string returnvalue { get; set; }
        public long UserId { get; set; }
        public Array editdata { get; set; }
        public DateTime ISMIOC_Datefrom { get; set; }
        public DateTime ISMIOC_Dateto { get; set; }
        public string ISMIOC_CallNo { get; set; }
    }
}
