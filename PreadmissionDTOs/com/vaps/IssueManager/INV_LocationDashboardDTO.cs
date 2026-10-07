using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class INV_LocationDashboardDTO
    {
        public long MI_ID { get; set; }
        public Array floordetails { get; set; }
        public Array employeedetails { get; set; }
        public Array employeedetailsCommon { get; set; }
        public Array employeelist { get; set; }
        public Array locationdetails { get; set; }
        public string Flag { get; set; }
        public long HRME_Id { get; set; }
        public locationlistarrays[] locationlistarrays { get; set; }
        public locationlistarrays[] unselectedlocationlistarrays { get; set; }
        public Array checkoutemployeelist { get; set; }
        public long User_Id { get; set; }
        public string Supplier_Name { get; set; }
        public Array Supplier_Details { get; set; }
        public Array Voucher_Details { get; set; }
        public Array Get_Instution { get; set; }
        public Array inv_locationdashboardsp_list { get; set; }
        //Added on 080525
        public long INVMLO_Id { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        //Sanjai
        public bool hasPendingConfirmation { get; set; }
        public Array pendingAssetList { get; set; }
        public Array locationRoomList { get; set; }
        public long? To_INVMLO_Id { get; set; }
        public locationlistarrays[] selectedPendingAssets { get; set; }
    }
    public class locationlistarrays
    {
        public long? MI_Id { get; set; }
        public long? HRME_Id { get; set; }
        public long? INVMI_Id { get; set; }
        public long? INVAAT_Id { get; set; }
        public long? INVMLO_Id { get; set; }
        public long? INVMST_Id { get; set; }
        public long? INVATCO_Id { get; set; }
        public string Supplier_Name { get; set; }
        public string VPAYVOU_StatusFlg { get; set; }
        public long? VPAYVOU_Id { get; set; }
        public bool? selected { get; set; }
    }

}
