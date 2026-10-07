using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("ISM_Client_MER_Files")]
    public class ISM_Client_MER_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLMERFL_Id { get; set; }
        public long ISMCLMER_Id { get; set; }
        public string ISMCLMERFL_FileName { get; set; }
        public string ISMCLMERFL_FileDesc { get; set; }
        public string ISMCLMERFL_FilePath { get; set; }
        public bool ISMCLMERFL_ApprovedFlg { get; set; }
        public long ISMCLMERFL_ApprovedBy_HRME_Id { get; set; }
        public string ISMCLMERFL_ApproverRemarks { get; set; }
        public DateTime? ISMCLMERFL_ApprovedDate { get; set; }
        public bool ISMCLMERFL_ActiveFlag { get; set; }
        public long ISMCLMERFL_CreatedBy { get; set; }
        public long ISMCLMERFL_UpdatedBy { get; set; }
        public long IMMM_ID { get; set; }
        public DateTime ISMCLMERFL_CreatedDate { get; set; }
        public DateTime ISMCLMERFL_UpdatedDate { get; set; }
    }
}
