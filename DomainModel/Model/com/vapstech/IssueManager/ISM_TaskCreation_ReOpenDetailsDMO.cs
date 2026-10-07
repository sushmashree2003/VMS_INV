using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_TaskCreation_ReOpenDetails")]
    public class ISM_TaskCreation_ReOpenDetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCRRPD_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public DateTime ISMTCRRPD_ReOpenDate { get; set; }
        public string ISMTCRRPD_Remarks { get; set; }
        public string ISMTCRRPD_Status { get; set; }
        public bool ISMTCRRPD_ActiveFlg { get; set; }
        public long ISMTCRRPD_CreatedBy { get; set; }
        public long ISMTCRRPD_UpdatedBy { get; set; }



    }
}
