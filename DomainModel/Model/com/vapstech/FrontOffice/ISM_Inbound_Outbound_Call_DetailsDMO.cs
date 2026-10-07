using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("ISM_Inbound_Outbound_Call_Details")]
    public class ISM_Inbound_Outbound_Call_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

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
        public string ISMIOC_CallNo { get; set; }
    }
}     
     
















       
