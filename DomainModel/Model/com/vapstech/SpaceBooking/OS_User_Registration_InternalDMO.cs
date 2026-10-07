using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.SpaceBooking
{
    [Table("OS_User_Registration_Internal")]
    public class OS_User_Registration_InternalDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OSUREGI_Id { get; set; }
        public long OSUREG_Id { get; set; }
        public long? HRME_Id { get; set; }
        public DateTime OSUREGI_CreatedDate { get; set; }
        public DateTime OSUREGI_UpdatedDate { get; set; }
        public long OSUREGI_CreatedBy { get; set; }
        public long OSUREGI_UpdatedBy { get; set; }

    }
}
