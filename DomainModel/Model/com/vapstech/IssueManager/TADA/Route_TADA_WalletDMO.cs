using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("Route_TADA_Wallet")]
    public class Route_TADA_WalletDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RTW_Id { get; set; }
        public long RTW_EmpID { get; set; }
        public DateTime? RTW_Date { get; set; }
        public decimal? RTW_Wallet { get; set; }
    }
}
