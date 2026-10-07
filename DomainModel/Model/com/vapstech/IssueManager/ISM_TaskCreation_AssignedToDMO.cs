using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_TaskCreation_AssignedTo")]
    public class ISM_TaskCreation_AssignedToDMO: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCRASTO_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMTCRASTO_AssignedDate { get; set; }
        public long ISMTCRASTO_AssignedBy { get; set; }
        public string ISMTCRASTO_Remarks { get; set; }
        public DateTime? ISMTCRASTO_StartDate { get; set; }
        public DateTime? ISMTCRASTO_EndDate { get; set; }
        public decimal ISMTCRASTO_EffortInHrs { get; set; }
        public bool ISMTCRASTO_ActiveFlg { get; set; }
        public string ISMTCRASTO_TimeRequiredFlg { get; set; }

        public long ISMTCRASTO_CreatedBy { get; set; }
        public long ISMTCRASTO_UpdatedBy { get; set; }



    }
}
