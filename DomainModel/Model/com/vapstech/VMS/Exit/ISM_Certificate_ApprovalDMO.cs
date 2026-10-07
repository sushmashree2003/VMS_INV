
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
    {
        [Table("ISM_Certificate_Request_ApprovedBy")]
        public class ISM_Certificate_ApprovalDMO
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long ISMCERTREQAPP_Id { get; set; }
            public long ISMCERTREQ_Id { get; set; }
            public long HRME_Id { get; set; }
            public DateTime ISMCERTREQAPP_ApprovedDate { get; set; }
            public string ISMCERTREQAPP_ReceiningDate { get; set; }
            public string ISMCERTREQAPP_Remarks { get; set; }
            public bool ISMCERTREQAPP_FinalFlg { get; set; }
            public bool ISMCERTREQAPP_ActiveFlag { get; set; }  
            public DateTime ISMCERTREQAPP_CreatedDate { get; set; }
            public DateTime ISMCERTREQAPP_UpdatedDate { get; set; }
            public long ISMCERTREQAPP_CreatedBy { get; set; }
            public long ISMCERTREQAPP_UpdatedBy { get; set; }
            public string ISMCERTREQAPP_AppRejFlag { get; set; }
            public string ISMCERTREQAPP_ACK { get; set; }
        }
    }
