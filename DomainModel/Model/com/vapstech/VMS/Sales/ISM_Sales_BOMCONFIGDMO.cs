using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_BOM_config")]
    public class ISM_Sales_BOMCONFIGDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSBC_ID { get; set; }
        public long ISMSBOM_Id { get; set; }
        public long ISMSPS_ID { get; set; }
        public bool ISMSBC_ActiveFlag { get; set; }

        public long ISMSBC_CreatedBy { get; set; }
        public long ISMSBC_UpdatedBy { get; set; }
        public DateTime? ISMSBOM_CreatedDate { get; set; }
        public DateTime? ISMSBC_UpdatedDate { get; set; }
    }
}
