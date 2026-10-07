using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Master_Module_BOM")]
    public class ISM_Master_Module_BOMDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IMMB_ID { get; set; }
        public long IMMB_IVRMMID { get; set; }
        public long IMMB_BOMID { get; set; }
        public string IMMB_InsType { get; set; }
        public bool IMMB_ActiveFlag { get; set; }
        public long IMMB_CreatedBy { get; set; }
        public long IMMB_UpdatedBy { get; set; }
        public DateTime? IMMB_CreatedDate { get; set; }
        public DateTime? IMMB_UpdatedDate { get; set; }
        public int? IMMB_IVRMMID_Order { get; set; }

    }
}
