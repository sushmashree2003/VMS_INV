using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_MeetingRoom")]
    public class ISM_Master_MeetingRoom_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMMER_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMMVNE_Id { get; set; }
        public string ISMMMER_MeetingRoomName { get; set; }
        public string ISMMMER_FloorName { get; set; }
        public string ISMMMER_Facilities { get; set; }
        public bool ISMMMER_ActiveFlag { get; set; }
        public long ISMMMER_CreatedBy { get; set; }
        public long ISMMMER_UpdatedBy { get; set; }

    }
}
