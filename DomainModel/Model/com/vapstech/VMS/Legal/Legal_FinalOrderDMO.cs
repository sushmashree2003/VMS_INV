using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_FinalOrder")]
    public class Legal_FinalOrderDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LFO_Id { get; set; }
        public long LCD_Id { get; set; }
        public string LFO_OrderDetails { get; set; }
        public string LFO_OrderNumber { get; set; }
        public DateTime? LFO_Date { get; set; }
        public bool LFO_ActiveFlag { get; set; }   
        public DateTime? LFO_CreatedDate { get; set; }        
        public DateTime? LFO_UpdatedDate { get; set; }
        public long LFO_CreatedBy { get; set; }
        public long LFO_UpdatedBy { get; set; }
    }
}
