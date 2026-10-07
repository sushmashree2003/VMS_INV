using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_SciencePark_Boot")]
    public class ISM_Sales_Lead_SciencePark_BootDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMLESPB_Id { get; set; }
        public long ISMSLET_Id { get; set; }
        public string ISMLESPB_First_Year { get; set; }
        public string ISMLESPB_Second_Year { get; set; }
        public string ISMLESPB_Third_Year { get; set; }
        public string ISMLESPB_Fourth_Year { get; set; }
        public string ISMLESPB_Fifth_Year { get; set; }
        public bool ISMLESPB_ActiveFlag { get; set; }
        public string ISMLESPB_Flag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public string ISMLESPB_FlagName { get; set; }
    }
}
