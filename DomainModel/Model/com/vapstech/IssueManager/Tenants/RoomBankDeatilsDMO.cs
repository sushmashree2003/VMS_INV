 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("HR_Master_Room_BankDetails")]
    public class RoomBankDeatilsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMRBD_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long HRMBD_Id { get; set; }
        public bool HRMRBD_ActiveFlag { get; set; }
        public DateTime ? HRMRBD_CreatedDate { get; set; }
        public DateTime? HRMRBD_UpdatedDate { get; set; }
        public long? HRMRBD_CreatedBy { get; set; }
        public long? HRMRBD_UpdatedBy { get; set; }
      



    }
}

