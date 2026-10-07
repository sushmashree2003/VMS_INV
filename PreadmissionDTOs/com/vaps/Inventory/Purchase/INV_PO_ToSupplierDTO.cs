using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Purchase.Inventory
{
    public class INV_PO_ToSupplierDTO
    {
        public string returnduplicatestatus { get; set; }

        public long ISES_Id { get; set; }
        public long IVRMIM_Id { get; set; }
        public string ISES_Template_Name { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string trans_id { get; set; }
        public string mailmsg { get; set; }
        public long INVPITS_Id { get; set; }
        public long INVMS_Id { get; set; }
        public long INVMPO_Id { get; set; }
        public string INVPITS_SupplierName { get; set; }
        public long INVPITS_ContactNo { get; set; }
        public string INVPITS_EmailId { get; set; }

        public string INVTPOSUP_SupplierName { get; set; }
        public string INVTPOSUP_MailAttatchment { get; set; }
    
        public long? INVTPOSUP_ContactNo { get; set; }
        public DateTime INVPITS_SMSSentDate { get; set; }
        public DateTime INVPITS_MailSentDate { get; set; }
        public bool INVPITS_ActiveFlg { get; set; }
        public bool attachmentflag { get; set; }
        public long INVPITS_CreatedBy { get; set; }
        public long INVPITS_UpdatedBy { get; set; }

        public Array get_supplier { get; set; }
        public Array gridlist { get; set; }
        public Array templatelist { get; set; }
        public Array get_piNo { get; set; }
        public Array get_pidetails { get; set; }

        public supplierArrayDTO[] supplierArray { get; set; }
        public pocolumns[] columnids { get; set; }
    }
  
    public class pocolumns
    {
        public long ID { get; set; }
        public string coulmn { get; set; }
        public string title { get; set; }

    }
         

}
