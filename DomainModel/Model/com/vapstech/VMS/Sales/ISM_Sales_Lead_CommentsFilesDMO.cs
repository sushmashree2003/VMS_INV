using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Comments_Files")]
    public class ISM_Sales_Lead_CommentsFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLECF_Id { get; set; }
        public long ISMSLECOM_Id { get; set; }
        public string ISMSLECF_FileName { get; set; }
        public string ISMSLECF_FileDesc { get; set; }
        public string ISMSLECF_FilePath { get; set; }
        public DateTime? ISMSLECF_CreatedDate { get; set; }
        public DateTime? ISMSLECF_UpdatedDate { get; set; }
        public long ISMSLECF_CreatedBy { get; set; }
        public long ISMSLECF_UpdatedBy { get; set; }

      
    }
}
