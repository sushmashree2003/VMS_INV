using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Master_ChannelPartner")]
    public class ISM_Sales_Master_ChannelPartnerDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSMCP_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMCP_ChannelPartnerName { get; set; }
        public string ISMSMCP_CPAddress { get; set; }
        public long ISMSMCP_CPContactNo { get; set; }
        public string ISMSMCP_CPEmailId { get; set; }
        public bool ISMSMCP_ActiveFlag { get; set; }
        public DateTime ISMSMCP_CreatedDate { get; set; }
        public DateTime ISMSMCP_UpdatedDate { get; set; }
        public long ISMSMCP_CreatedBy { get; set; }
        public long ISMSMCP_UpdatedBy { get; set; }
        public long? ISMSMCP_AgreementDuration { get; set; }
        public DateTime? ISMSMCP_AgreementStartDate { get; set; }
        public DateTime? ISMSMCP_ActiveFromDate { get; set; }
        public DateTime? ISMSMCP_ActiveToDate { get; set; }
        public bool? ISMSMCP_RenewedFlg { get; set; }

        public List<ISM_Sales_Master_ChannelPartner_DocumentsDMO> ISM_Sales_Master_ChannelPartner_DocumentsDMO_con { get; set; }

    }
}
