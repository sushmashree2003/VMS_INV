using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
   

    [Table("Vaps_Projects_InstWiseTasks")]
    public class ViewOfDeploymentTasksDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VPIWT_Id { get; set; }
        public long VPSD_Id { get; set; }
     
        public long ISMTCR_Id { get; set; }
     
        public bool VPIWT_ActiveFlag { get; set; }
        public DateTime VPIWT_CreatedDate { get; set; }
        public DateTime VPIWT_UpdatedDate { get; set; }
      
    }
}
