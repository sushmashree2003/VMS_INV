using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.SpaceBooking
{
    [Table("OS_Space_Booking")]
    public class OS_Space_Booking 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OSSPBOOK_Id { get; set; }
        public long MI_Id { get; set; }
        public long OSUREG_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long OSSPBOOK_NoOfPeople { get; set; }
        public long OSSPBOOK_BillingPinCode { get; set; }
        public long IVRMMS_Id { get; set; }
        public long IVRMMC_Id { get; set; }
        public long? OSSPBOOK_CreatedBy { get; set; }
        public long? OSSPBOOK_UpdatedBy { get; set; }
        public string OSSPBOOK_TransactionNo { get; set; }
        public string OSSPBOOK_DurationType { get; set; }
        public string OSSPBOOK_StartTime { get; set; }
        public string OSSPBOOK_EndTime { get; set; }
        public string OSSPBOOK_BillingName { get; set; }
        public string OSSPBOOK_GSTNo { get; set; }
        public string OSSPBOOK_BillingAdd1 { get; set; }
        public string OSSPBOOK_BillingAdd2 { get; set; }
        public string OSSPBOOK_BillingAdd3 { get; set; }
        public string OSSPBOOK_BillingAdd4 { get; set; }
        public string OSSPBOOK_BillingCity { get; set; }
        public string OSSPBOOK_Status  { get; set; }
        public DateTime? OSSPBOOK_StartDate { get; set; }
        public DateTime? OSSPBOOK_EndDate { get; set; }
        public DateTime? OSSPBOOK_CreatedDate { get; set; }
        public DateTime? OSSPBOOK_UpdatedDate { get; set; }
        public bool OSSPBOOK_ActiveFlag { get; set; }
    }
}
