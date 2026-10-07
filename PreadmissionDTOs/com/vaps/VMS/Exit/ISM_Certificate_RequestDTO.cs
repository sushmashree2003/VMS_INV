
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Exit

{
    public class ISM_Certificate_RequestDTO
    {
        public long ISMCERTREQ_Id { get; set; }
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long Emp_Code { get; set; }
        public DateTime ISMCERTREQ_RequestDate { get; set; }
        public string ISMCERTREQ_CertificateName { get; set; }
        public string ISMCERTREQ_Remarks { get; set; }
        public string ISMCERTREQ_ReceivingMode { get; set; }
        public string ISMCERTREQ_FileName { get; set; }
        public string ISMCERTREQ_FilePath { get; set; }
        public bool ISMCERTREQ_ActiveFlag { get; set; }
        public string ISMCERTREQ_AppRejFlag { get; set; }
        public string ISMCERTREQ_RecivingAddress { get; set; }
        public bool returnval { get; set; }
        public Array display_data { get; set; }
        public Array getloaddetails { get; set; }
        public Array modalview { get; set; }

        public string returnduplicatestatus { get; set; }
        public string HRME_EmployeeFirstName { get; set; }


        //----------------- DH task Verification-----
        public Array tasklist { get; set; }



        //-----ism_certificate_request
        public long maxmumlevel { get; set; }
        public string RoleType { get; set; }
        public long roleid { get; set; }

        public long ISMCERTREQAPP_Id { get; set; }
        public long HRMEDS_Id { get; set; }

        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }

        public long yearid { get; set; }
        public string rdbbutton { get; set; }
        public string Status_Type { get; set; }
        public Array certificateapprovalReport { get; set; }
        public Array viewlist { get; set; }
        public Array Cerfificatelist { get; set; }
        public Array fillyear { get; set; }
        public Array fillmonth { get; set; }
        public Array employees { get; set; }
        public string viewattachment { get; set; }
        public string attachment { get; set; }
        public string ISMCERTREQAPP_Remarks { get; set; }
        public string HRMEM_EmailId { get; set; }
        public string email { get; set; }
        public Array emaildetails { get; set; }
        public Array pagePrivilege { get; set; }
        public string ISMCERTREQAPP_AppRejFlag { get; set; }
        public DateTime ISMCERTREQAPP_ApprovedDate { get; set; }
        public string ISMCERTREQAPP_ReceiningDate { get; set; }
        public string ISMCERTREQAPP_ReceivingTime { get; set; }
        public bool ISMCERTREQAPP_FinalFlg { get; set; }
        public bool approved_flag { get; set; }
        public DateTime receiningdate { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public Array document { get; set; }
        public Array aprovedlist { get; set; }
        public Array checklist { get; set; }
        public grid_array[] grid_arrays { get; set; }
        public monthdto[] MONTHID { get; set; }
        public employeeDTO[] HRMEID { get; set; }
        public doc_list_new[] doc_list2 { get; set; }

    }
    public class doc_list_new
    {
        public long ISMRESGMCL_Id { get; set; }
        public string Remarks { get; set; }
        public decimal? DamageAmount { get; set; }

    }
    public class grid_array
    {
        public long HRME_Id { get; set; }
        public long ISMCERTREQ_Id { get; set; }
        public string ISMCERTREQAPP_ReceiningDate { get; set; }
        public string ISMCERTREQAPP_Remarks { get; set; }
        public string ISMCERTREQAPP_AppRejFlag { get; set; }
        public long? UserId { get; set; }
        public bool FinalFlg { get; set; }

    }

    public class monthdto
    {
        public long IVRM_Month_Id { get; set; }
    }
    public class employeeDTO
    {
        public long HRME_Id { get; set; }
    }

}
