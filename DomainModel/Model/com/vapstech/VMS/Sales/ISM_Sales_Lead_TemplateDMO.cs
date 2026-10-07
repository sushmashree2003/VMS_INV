using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Template")]
    public class ISM_Sales_Lead_TemplateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLET_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long USER_Id { get; set; }
        public string ISMSLET_HtmlTemplate { get; set; }
        public string ISMSLET_HtmlPdf { get; set; }
        public string ISMSLET_HtmlName { get; set; }
        public bool? ISMSLET_QuotationSent { get; set; }
        public string ISMSBOM_Id { get; set; }
        public string ISMSMPR_Id { get; set; }
        public string ISMSLET_Status { get; set; }
        public DateTime? ISMSLET_CreatedDate { get; set; }
        public DateTime? ISMSLET_UpdatedDate { get; set; }
        public long ISMSLET_CreatedBy { get; set; }
        public long ISMSLET_UpdatedBy { get; set; }
        public long? ISMSBOM_ApprovedBy { get; set; }
        public string ISMSLET_ApprovedRemark { get; set; }
        public bool ISMSLET_ActiveFlag { get; set; }
        public string ISMSLET_Amount_I { get; set; }
        public string ISMSLET_Amount_Final { get; set; }
        public string ISMSLET_Amount_IMD { get; set; }
        public string ISMSLET_Amount_FinalMD { get; set; }
        public string ISMSLET_Amount_Old { get; set; }
        public string ISMSLET_Amount_Final_old { get; set; }
        public string ISMSLET_flag { get; set; }
        public decimal? ISMSLET_Cost { get; set; }
        public decimal? ISMSLET_CostApproved { get; set; }


    }
}
//ISMSLET_Cost

