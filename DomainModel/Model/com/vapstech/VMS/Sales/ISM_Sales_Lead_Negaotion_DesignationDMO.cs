using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{

    [Table("ISM_Sales_Lead_Negaotion_Designation")]
    public class ISM_Sales_Lead_Negaotion_DesignationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISLIDN_ID { get; set; }
        public long HRME_Id { get; set; }
        public long ISLIDN_Value { get; set; }
        public string ISLIDN_Remark { get; set; }
        public bool ISLIDN_ActiveFlag { get; set; }
        public DateTime? ISLIDN_CreatedDate { get; set; }
        public DateTime? ISLIDN_UpdatedDate { get; set; }
        public long ISLIDN_CreatedBy { get; set; }
        public long ISLIDN_UpdatedBy { get; set; }

    }
}
