using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Incentive_Dept")]
    public class ISM_Sales_Lead_Incentive_DeptDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISLID_ID { get; set; }
        public long HRMD_ID { get; set; }
        public long ISLID_Value { get; set; }
        public string ISLID_Remark { get; set; }
        public bool ISLID_ActiveFlag { get; set; }
        public DateTime? ISLID_CreatedDate { get; set; }
        public DateTime? ISLID_UpdatedDate { get; set; }
        public long ISLID_CreatedBy { get; set; }
        public long ISLID_UpdatedBy { get; set; }

        
    }
}
