using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Master_ChannelPartner_Documents")]
    public class ISM_Sales_Master_ChannelPartner_DocumentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSMCPD_Id { get; set; }
        public long? ISMSMCP_Id { get; set; }
        public string ISMSMCPD_DocumentName { get; set; }
        public string ISMSMCPD_DocumentPath { get; set; }
        public bool ISMSMCPD_ActiveFlag { get; set; }
        public DateTime? ISMSMCPD_CreatedDate { get; set; }
        public DateTime? ISMSMCPD_UpdatedDate { get; set; }
        public long ISMSMCPD_CreatedBy { get; set; }
        public long ISMSMCPD_UpdatedBy { get; set; }
    }
}
