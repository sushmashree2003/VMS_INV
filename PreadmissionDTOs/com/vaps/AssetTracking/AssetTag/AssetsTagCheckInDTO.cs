using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.AssetTracking.AssetTag
{
   public class AssetsTagCheckInDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string message { get; set; }
        public Array instlist { get; set; }
        public Array get_checkin { get; set; }
        public Array get_checkindc { get; set; }
        public long INVATCI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public long INVMLO_Id { get; set; }
        public string returnval { get; set; }
        public long? HRME_Id { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
        public string INVATCI_ReceivedBy { get; set; }
        public DateTime? INVATCI_CheckInDate { get; set; }
        public decimal INVATCI_CheckInQty { get; set; }
        public string INVATCI_CheckInRemarks { get; set; }
        public string INVATCI_No { get; set; }
        public bool INVATCI_ActiveFlg { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string trans_id { get; set; }
        public AseetList[] AssetIditems { get; set; }
    }
    public class AseetList
    {
        public long AssetId { get; set; }
    }
}
