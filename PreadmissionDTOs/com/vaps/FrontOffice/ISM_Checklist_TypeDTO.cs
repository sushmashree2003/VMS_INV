using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class ISM_Checklist_TypeDTO
    {
        public long ISMCLT_Id { get; set; }
        public string ISMCLT_Name { get; set; }
        public long ISMCLT_UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool ISM_ActiveFlag { get; set; }
        public long ISMCLT_CreatedBy { get; set; }
        public long ISMCLT_UpdatedBy { get; set; }
    }
}
