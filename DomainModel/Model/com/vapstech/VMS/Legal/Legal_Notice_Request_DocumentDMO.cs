using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{

    [Table("Legal_Notice_Request_Document")]
    public class Legal_Notice_Request_DocumentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LNRD_Id { get; set; }
        public long LNR_Id { get; set; }
        public string LNRD_Document { get; set; }
        public DateTime? LNRD_CreatedDate { get; set; }
        public DateTime? LNRD_UpdatedDate { get; set; }
        public long LNRD_CreatedBy { get; set; }
        public long LNRD_UpdatedBy { get; set; }      

    }
}
