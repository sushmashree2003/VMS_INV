using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    //[Table("FO_Inward_Files", Schema = "VM")]
    [Table("FO_Inward_FilesUpload", Schema = "VM")]
    public class FO_Inward_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        //public long FOINF_Id { get; set; }
        ////public long MI_Id { get; set; }
        //public long FOIN_Id { get; set; }
        //public string FOINF_FileName { get; set; }
        //public string FOINF_Filedesc { get; set; }
        //public string FOINF_FilePath { get; set; }
        //public bool FOINF_ActiveFlg { get; set; }
        //public string FOINF_StatusFlg { get; set; }
        public long FOINFU_Id { get; set; }
        //public long MI_Id { get; set; }
        public long FOIN_Id { get; set; }
        public string FOINFU_FileName { get; set; }
        public string FOINFU_Filedesc { get; set; }
        public string FOINFU_FilePath { get; set; }
        public bool FOINFU_ActiveFlg { get; set; }
        public DateTime FOINFU_CreatedDate { get; set; }
        public DateTime FOINFU_UpdatedDate { get; set; }
        public long FOINFU_CreatedBy { get; set; }
        public long FOINFU_UpdatedBy { get; set; }


    }
}
