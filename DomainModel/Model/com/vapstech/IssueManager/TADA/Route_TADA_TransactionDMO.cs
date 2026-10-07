using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("Route_TADA_Transaction")]
    public class Route_TADA_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RTT_ID { get; set; }
        public long RTW_Id { get; set; }
        public long RTW_IERId { get; set; }
        public decimal? RTW_Route_Wallet { get; set; }
        public decimal? RTW_Route_OpenBalance { get; set; }
        public DateTime? RTW_Route_Date { get; set; }
        public decimal? RTW_Route_AMt { get; set; }
        public decimal? RTW_Route_CloseBalance { get; set; }
    }
}
