using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Exit
{
    [Table("ISM_Resignation_ChecKLists_ApprovedBy")]
    public class ISM_Resignation_ChecKLists_ApprovedBy_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRESGCLAPP_Id { get; set; }
        public long ISMRESGCL_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMRESGCLAPP_Remarks { get; set; }
        public string ISMRESGCLAPP_FileName { get; set; }
        public string ISMRESGCLAPP_FilePath { get; set; }
        public bool ISMRESGCLAPP_ActiveFlag { get; set; }
        public DateTime ISMRESGCLAPP_CreatedDate { get; set; }
        public DateTime ISMRESGCLAPP_UpdatedDate { get; set; }
        public long ISMRESGCLAPP_CreatedBy { get; set; }
        public long ISMRESGCLAPP_UpdatedBy { get; set; }
        public string ISMRESGCLAPP_Status { get; set; }

    }
}
