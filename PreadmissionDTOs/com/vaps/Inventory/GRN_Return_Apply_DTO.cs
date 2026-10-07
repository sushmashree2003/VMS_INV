using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class GRN_Return_Apply_DTO
    {
      
        public long MI_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMS_Id { get; set; }
        public long UserId { get; set; }
        public long INVMGRNRET_Id { get; set; }
        public string INVMGRNRETAPP_GRNRETNo { get; set; }
        public string returnduplicatestatus { get; set; }
        public DateTime INVMGRNRETAPP_ReturnDate { get; set; }
        public decimal INVMGRNRETAPP_TotalAmount { get; set; }
        public string INVMGRNRETAPP_Remarks { get; set; }
        public long ASMAY_Id { get; set; }
        public bool returnval { get; set; }
        public Array grn_m_return { get; set; }
        public Array grn_item_return { get; set; }
       public arrayretgrn1[] arrayretgrn { get; set; }

        public class arrayretgrn1
        {
            public long INVMI_Id { get; set; }
            public long INVMUOM_Id { get; set; }
            public long INVTPIAPP_ApprovedQty { get; set; }
            public decimal invtpI_ApproxAmount { get; set; }
            public string INVTGRNRETAPP_ReturnNaration { get; set; }
            public string flag { get; set; }
        }
    }
}
