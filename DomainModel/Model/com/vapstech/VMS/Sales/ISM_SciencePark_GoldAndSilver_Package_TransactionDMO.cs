using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_SciencePark_GoldAndSilver_Package_Transaction")]
    public class ISM_SciencePark_GoldAndSilver_Package_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISGS_Id { get; set; }
        public long? ISGSISMSBOM_Id { get; set; }
        public long? ISGSISMSLE_Id { get; set; }
        public string ISGSISMSPSI_ItemFlag { get; set; }
        public string ISGSISMSPSI_ItemsDescription { get; set; }
        public string ISGSIMSV_TotalCostPerQty { get; set; }
        public string ISGSISPSIF_AmountScincePark { get; set; }
        public string ISGSISPSIF_AmtScincePark { get; set; }
        public string ISGSISPSIF_Cost { get; set; }
        public string ISGSISPSIF_CostNew { get; set; }
        public string ISGSISPSIF_ItemsName { get; set; }
        public string ISGSISPSIF_Quantity { get; set; }
        public string ISGSISPSIF_Quantitys { get; set; }
        public string ISGSScienceParkFlag { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
    }
}
