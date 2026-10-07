using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_TaskCreation_TransferredTo")]
    public class ISM_TaskCreation_TransferredToDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCRTRTO_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMTCRTRTO_TransferredDate { get; set; }
        public long ISMTCRTRTO_TransferredBy { get; set; }
        public string ISMTCRTRTO_Remarks { get; set; }
        public DateTime? ISMTCRTRTO_StartDate { get; set; }
        public DateTime? ISMTCRTRTO_EndDate { get; set; }
        public decimal ISMTCRTRTO_EffortInHrs { get; set; }
        public bool ISMTCRTRTO_ActiveFlg { get; set; }
        public long ISMTCRTRTO_CreatedBy { get; set; }
        public long ISMTCRTRTO_UpdatedBy { get; set; }

    }
}
