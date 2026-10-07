using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_Venue")]
    public class MasterVenues_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMVNE_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMVNE_VenueName { get; set; }
        public string ISMMVNE_VenueAddress { get; set; }
        public bool  ISMMVNE_ActiveFlag { get; set; }
        public long?  ISMMVNE_CreatedBy { get; set; }
        public long?  ISMMVNE_UpdatedBy { get; set; }

    }
}
