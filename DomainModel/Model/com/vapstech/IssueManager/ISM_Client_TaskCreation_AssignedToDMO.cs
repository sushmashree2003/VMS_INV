using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Client_TaskCreation_AssignedTo")]
    public class ISM_Client_TaskCreation_AssignedToDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCCATO_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCCATO_Status { get; set; }
        public long ISMTCCATO_AssignedBy { get; set; }
        public long ISMTCCATO_AssignedTo { get; set; }
        public DateTime? ISMTCCATO_AssignedDate { get; set; }
        public bool ISMTCCATO_ActiveFlg { get; set; }
        public long ISMTCCATO_CreatedBy { get; set; }
        public long ISMTCCATO_UpdatedBy { get; set; }

    }
}
