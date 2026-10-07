using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory.Purchase
{
   public class INV_RequisitionNotificationDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public Array get_purchaserequisition { get; set; }
        public Array get_Details { get; set; }
        public bool rolflag { get; set; }
        public long HRME_Id { get; set; }
        public Array get_mi_list { get; set; }
        public arrayPR[] arrayPR { get; set; }
        public long INVMPR_Id { get; set; }
        public string logo { get; set; }
        public string Flag { get; set; }
        public Array Purchaseorderarray { get; set; }
    }
    public class arrayPR
    {
        public long INVMPR_Id { get; set; }
    }
}
