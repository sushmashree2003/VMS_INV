using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_CA_Demo_Ratings")]
    public class ISM_CA_Demo_RatingsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ICDR_ID { get; set; }
        public long MI_id { get; set; }
        public long ICDR_GivenBy_EmpID { get; set; }
        public long ICDR_ProductID { get; set; }
        public long ICDR_GivenTo_EmpId { get; set; }
        public DateTime? ICDR_DemoDate { get; set; }
        public string ICDR_Remarks { get; set; }
        public decimal? ICDR_Ratings { get; set; }
        public bool ICDR_Active { get; set; }
        public long  ICDR_CreatedBy { get; set; }
        public DateTime?  ICDR_CreatedDate { get; set; }
        public long ICDR_Updateby { get; set; }
        public DateTime?  ICDR_UpdateDate { get; set; }
        public long? IVRM_Month_Id { get; set; }
        public string IMFY_FinancialYear { get; set; }
        //ISM_CA_StudentStrength_Mapping 
    }
}
