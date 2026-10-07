using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_PushNotification")]
    public class ISM_PushNotificationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMPN_Id { get; set; }
        public long MI_Id { get; set; }
        public long? ISM_CommonId { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? ISM_CreatedDate { get; set; }
        public string ISMPN_Notification { get; set; }
        public DateTime ISMPN_Date { get; set; }
        public DateTime ISMPN_Time { get; set; }
        public string ISMPN_Type { get; set; }
        public bool ISMPN_ActiveFlg { get; set; }
    }
}
