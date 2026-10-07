using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("FO_Outward_FilesUpload", Schema = "VM")]
    public class FO_Outward_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FOOUTFU_Id { get; set; }
        public long FOOUT_Id { get; set; }
        public string FOOUTFU_FileName { get; set; }
        public string FOOUTFU_Filedesc { get; set; }
        public string FOOUTFU_FilePath { get; set; }
        public bool FOOUTFU_ActiveFlg { get; set; }
        public DateTime FOOUTFU_CreatedDate { get; set; }
        public DateTime FOOUTFU_UpdatedDate { get; set; }
        public long FOOUTFU_CreatedBy { get; set; }
        public long FOOUTFU_UpdatedBy { get; set; }


    }
}
