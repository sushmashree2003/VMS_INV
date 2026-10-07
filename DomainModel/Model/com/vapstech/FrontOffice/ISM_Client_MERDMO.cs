using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("ISM_Client_MER")]
    public class ISM_Client_MERDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLMER_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMCLMER_Year { get; set; }
        public long Month_Id { get; set; }
        public bool ISMCLMER_ActiveFlag { get; set; }
        public long ISMCLMER_CreatedBy { get; set; }
        public long ISMCLMER_UpdatedBy { get; set; }
        public DateTime ISMCLMER_CreatedDate { get; set; }
        public DateTime ISMCLMER_UpdatedDate { get; set; }
        public List<ISM_Client_MER_FilesDMO> ISM_Client_MER_FilesDMO { get; set; }
    }
}
