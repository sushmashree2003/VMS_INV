using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("Vaps_ProjectsDetails")]
    public class ViewOfDeploymentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VPSD_Id { get; set; }

        public string VPSD_Name { get; set; }
        public DateTime? VPSD_Date { get; set; }
        public string VPSD_Environment { get; set; }
        public string VPSD_Path { get; set; }
        public string VPSD_CodeType { get; set; }
     
        public bool VPSD_ActiveFlag { get; set; }
        public long VPSD_CreatedBy { get; set; }
        public long VPSD_UpdatedBy { get; set; }
        public DateTime VPSD_UpdatedDate { get; set; }
        public DateTime VPSD_CreatedDate { get; set; }
        public long MI_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public List<ViewOfDeploymentTasksDMO> ViewOfDeploymentTasksDMO { get; set; }


    }
}
