using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("ISM_RoutePlan_DepartureDetails")]
    public class ISM_RoutePlan_DepartureDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IRPD_ID { get; set; }
        public long IER_ID { get; set; }
        public long IVRMMS_Id { get; set; }
        public DateTime? IRPD_DepartureDate { get; set; }
        public DateTime? IRPD_ArrivalDate { get; set; } 
        public string IRPD_DepartureTime { get; set; }
        public string IRPD_ArrivalTime { get; set; }
        public long? IRPD_Createdby { get; set; }
        public DateTime? IRPD_CreatedDate { get; set; }
        public long? IRPD_Updateby { get; set; }
        public DateTime? IRPD_UpdateDate { get; set; }
        public int? IRPD_Order { get; set; }
    }
}
