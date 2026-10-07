using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_Candidates_Exam_Answersheet")]
    public class OT_Candidates_Exam_AnswersheetDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OTCANDEXAS_Id { get; set; }
        public long OTCANDEX_Id { get; set; }
        public string OTCANDEXAS_AnswerSheetFile { get; set; }
        public string OTCANDEXAS_AnswerSheetPath { get; set; }
        public bool OTCANDEXAS_ActiveFlg { get; set; }
        public long OTCANDEXAS_CreatedBy { get; set; }
        public DateTime OTCANDEXAS_CreatedDate { get; set; }
        public long OTCANDEXAS_UpdatedBy { get; set; }
        public DateTime OTCANDEXAS_UpdatedDate { get; set; }
        public long OTMOEQ_Id { get; set; }
    }
}