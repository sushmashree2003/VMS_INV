using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sale_MOU")]
    public class ISM_Sales_Lead_MOU
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSMOU_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long ISMSMOU_Studentstrength { get; set; }
        public decimal ISMSMOU_Amount { get; set; }
        public string ISMSMOU_Packagename { get; set; }
        public string ISMSMOUA_ProductName { get; set; }
        public bool ISMSMOU_ActiveFlag { get; set; }
        public DateTime? ISMSMOU_CreatedDate { get; set; }
        public DateTime? ISMSMOU_UpdatedDate { get; set; }
        public long ISMSMOU_CreatedBy { get; set; }
        public long ISMSMOU_UpdatedBy { get; set; }
        public List<ISM_Sales_Lead_MOU_Attachments> ISM_Sales_Lead_MOU_Attachments { get; set; }
    }
}
