
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("ISM_Certificate_Request")]
    public class ISM_Certificate_RequestDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCERTREQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMCERTREQ_RequestDate { get; set; }
        public string ISMCERTREQ_CertificateName { get; set; }
        public string ISMCERTREQ_Remarks { get; set; }
        public string ISMCERTREQ_ReceivingMode { get; set; }
        public string ISMCERTREQ_FileName { get; set; }
        public string ISMCERTREQ_FilePath { get; set; }
        public bool ISMCERTREQ_ActiveFlag { get; set; }
        public DateTime ISMCERTREQ_CreatedDate { get; set; }
        public DateTime ISMCERTREQ_UpdatedDate { get; set; }
        public long ISMCERTREQ_CreatedBy { get; set; }
        public long ISMCERTREQ_UpdatedBy { get; set; }
        public string ISMCERTREQ_AppRejFlag { get; set; }
        public string ISMCERTREQ_RequestTypeFlag { get; set; }
        public string ISMCERTREQ_RecivingAddress { get; set; }


    }
}
