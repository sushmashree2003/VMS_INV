using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{

    [Table("ISM_Mst_Route_Category")]
    public class ISM_Mst_Route_CategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMRC_CategoryId { get; set; }
        public string IMRC_CategoryName { get; set; }
        public bool IMRC_ActiveFlag { get; set; }
        public long MI_Id { get; set; }
        public  long? IMRC_Createdby { get; set; }
        public DateTime? IMRC_CreatedDate { get; set; }
        public long? IMRC_Updateby { get; set; }
        public DateTime? IMRC_UpdateDate { get; set; }
    }
}
