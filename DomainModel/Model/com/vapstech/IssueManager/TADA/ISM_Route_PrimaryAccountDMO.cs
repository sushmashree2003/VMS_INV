using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("ISM_Route_PrimaryAccount")]
    public   class ISM_Route_PrimaryAccountDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IRPA_ID { get; set; }
        public long ISMSMSO_ID { get; set; }
        public bool IRPA_Status { get; set; }
        public int IRPA_Order { get; set; }
        public long IRPA_Createdby { get; set; }
        public DateTime? IRPA_CreatedDate { get; set; }
        public long IRPA_Updateby { get; set; }
        public DateTime? IRPA_UpdateDate { get; set; }

    }
}
