using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Stu_MP_Promo_Subjectwise_Groupwise", Schema = "Exm")]
    public class Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESTMPPSG_Id { get; set; }
        [ForeignKey("ESTMPPS_Id")]
        public int ESTMPPS_Id { get; set; }
        public int EMPSG_Id { get; set; }
        public decimal? ESTMPPSG_GroupMaxMarks { get; set; }
        public decimal? ESTMPPSG_GroupObtMarks { get; set; }
        public string ESTMPPSG_GroupObtGrade { get; set; }        
       
    }
}
