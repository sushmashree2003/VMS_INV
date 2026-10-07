using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Student_HeightWeight", Schema = "SPC")]
    public class BMICalculationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int SPCCBMI_ID { get; set; }
        //public long MI_Id { get; set; }
        //public long ASMAY_Id { get; set; }
        //public long ASMCL_Id { get; set; }
        //public long ASMS_Id { get; set; }
        //public long AMST_Id { get; set; }
        //public decimal? AMST_Weight { get; set; }
        //public decimal? AMST_Height { get; set; }
        //public decimal? AMST_BMI { get; set; }
        //public string SPCC_BMI_Remarks { get; set; }
        //public bool SPCCBMI_ActiveFlag { get; set; }


        public long SPCCSHW_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime SPCCSHW_AsOnDate { get; set; }
        public decimal? SPCCSHW_Height { get; set; }
        public decimal? SPCCSHW_Weight { get; set; }
        public decimal? SPCCSHW_BMI { get; set; }
        public string SPCCSHW_BMIRemark { get; set; }
        public bool SPCCMHW_ActiveFlag { get; set; }




    }
}
