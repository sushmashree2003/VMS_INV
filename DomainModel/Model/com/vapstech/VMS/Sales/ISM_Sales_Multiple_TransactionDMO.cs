using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Multiple_Transaction")]
    public class ISM_Sales_Multiple_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMT_Id { get; set; }

        public long? ISMSBOM_Id { get; set; }
        public string ISMMT_BOMName { get; set; }
        public long? ISMSLE_Id { get; set; }
        public long? ISMSPSI_ID { get; set; }
        public long? ISPSIF_Id { get; set; }
        public long? ISMSLET_Id { get; set; }
        public string ISMMT_ItemsName { get; set; }
        public string ISMMT_ItemFlag { get; set; }
        public string ISMMT_ItemsDescription { get; set; }
        public decimal? ISMMT_CostAprdMD { get; set; }
        public decimal? ISMMT_CostOriginal { get; set; }
        public decimal? ISMMT_Quantity { get; set; }
        public decimal? ISMMT_Quantitys { get; set; }
        public decimal? ISMMT_TotalPRCost { get; set; }
        public bool ISMMT_ActiveFlag { get; set; }
        public long? ISMMT_CreatedBy { get; set; }
        public long? ISMMT_UpdatedBy { get; set; }
        public DateTime? ISMMT_CreatedDate { get; set; }
        public DateTime? ISMMT_UpdatedDate { get; set; }

    }
}
