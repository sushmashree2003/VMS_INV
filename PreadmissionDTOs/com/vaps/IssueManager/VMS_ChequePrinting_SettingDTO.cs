using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class VMS_ChequePrinting_SettingDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long VCPSET_Id { get; set; }
        public string VCPSET_PrintingTemplateId { get; set; }
        public string VCPSET_ChequeImageFileName { get; set; }
        public string VCPSET_ChequeImageFilePath { get; set; }
        public string VCPSET_Remarks { get; set; }
        public long HRMBD_Id { get; set; }
        public long User_Id { get; set; }
        public long Id { get; set; }
        public Array get_vmsCheque { get; set; }
        public string HRMBD_ActiveFlag { get; set; }
        public string MI_Name { get; set; }
        public long UserId { get; set; }
        public Array get_mi_list { get; set; }
        public bool VCPSET_ActiveFlg { get; set; }
        public string HRMBD_BankName { get; set; }
        public long HRME_Id { get; set; }
        public long VCPSET_CreatedBy { get; set; }
        public long VCPSET_UpdatedBy { get; set; }
        public Array banklist { get; set; }
        public Array alldata { get; set; }
        public Array allcompany { get; set; }
        public long? HRMDES_Id { get; set; }
        public string optionflag { get; set; }
        public long VPAYVOU_Id { get; set; }

    }
}
