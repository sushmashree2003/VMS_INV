using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Notifications")]
    public class ISM_NotificationsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMNO_Id { get; set; }
        public long MI_Id { get; set; }      
        public long HRME_Id { get; set; }
        public string ISMNO_Notification { get; set; }
        public DateTime ISMNO_NoticationDate { get; set; }      
        public string ISMNO_NotificationType { get; set; }
        public bool ISMNO_ReadFlg { get; set; }
        public bool ISMNO_MakeUnReadFlg { get; set; }
        public bool ISMNO_ActiveFlag { get; set; }
        public string ISMNO_Response { get; set; }
        public string ISMNO_ModeFlg { get; set; }
        public string ISMNO_RedirectURL { get; set; }
        public long ISMNO_CreatedBy { get; set; }
        public long ISMNO_UpdatedBy { get; set; }
      
    }
}
