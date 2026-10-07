using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_TaskCreation_Response")]
    public class ISM_TaskCreation_ResponseDMO: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCRRES_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMTCRRES_ResponseDate { get; set; }
        public string ISMTCRRES_Response { get; set; }
        public string ISMTCRRES_ResponseAttachment { get; set; }
        public bool ISMTCRRES_ActiveFlg { get; set; }     
        public long ISMTCRRES_CreatedBy { get; set; }
        public long ISMTCRRES_UpdatedBy { get; set; }
        public string ISMTCRRES_Status { get; set; }
        public string ISMTCRRES_ResponsePageName { get; set; }

    }
}
