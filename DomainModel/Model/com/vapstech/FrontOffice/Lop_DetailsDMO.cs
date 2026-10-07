using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("HR_Additional_LopDetails")]
    public class Lop_DetailsDMO //: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRALD_ID { get; set; }
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }        
        public string HRALD_Type { get; set; }
        public string HRALD_Remarks { get; set; }
        public DateTime HRALD_Date { get; set; }
        public string HRALD_File { get; set; }
        public bool HRALD_Activeflag { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
        public long Createdby { get; set; }
        public long Updatedby { get; set; }
        public long ISMTCR_Id { get; set; }


    }
}
