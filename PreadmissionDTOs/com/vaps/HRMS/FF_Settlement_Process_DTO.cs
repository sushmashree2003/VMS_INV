using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class FF_Settlement_Process_DTO
    {
      
        public Array griddisplay { get; set; }
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
        public long HRESA_Id { get; set; }
        public long HREFFS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HREFFS_EntryDate { get; set; }
        public int HREFFS_AdvYear { get; set; }
        public string HREFFS_AdvMonth { get; set; }
        public decimal HREFFS_AppliedAmount { get; set; }
        public string HREFFS_Remarks { get; set; }
        public string HREFFSA_Remarks { get; set; }
        public bool? HREFFS_ActiveFlag { get; set; }
        public string HREFFS_AdvStatus { get; set; }
        public decimal HREFFS_SanctinedAmount { get; set; }
        public string HREFFS_ModeOfPayment { get; set; }
        public long HREFFS_ReferenceNo { get; set; }
        public DateTime? HREFFS_CreatedDate { get; set; }
        public DateTime? HREFFS_UpdatedDate { get; set; }
        public long? HREFFS_CreatedBy { get; set; }
        public long? HREFFS_UpdatedBy { get; set; }
        public Array empadvaList { get; set; }
        public string retrunMsg { get; set; }

        public bool returnval { get; set; }

        public Array approvaldisplay { get; set; }
        public Array aprovedlist { get; set; }


        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }


        public Array employeedropdown { get; set; }

        public Array leaveyeardropdown { get; set; }
        public Array monthdropdown { get; set; }
        public Array modeOfPaymentdropdown { get; set; }
        public Array fillyear { get; set; }
        public Array fillmonth { get; set; }
        public Array employees { get; set; }
        public Array approvalReport { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public monthdto[] MONTHID { get; set; }
        public employeeDTO[] HRMEID { get; set; }

        public HR_ConfigurationDTO configurationDetails { get; set; }

        public decimal? empGrossSal { get; set; }
        public decimal? totalAppliedAmount { get; set; }
        public string searchfilter { get; set; }


        public long HRESAA_Id { get; set; }
        public long yearid { get; set; }
        public long IVRMUL_Id { get; set; }
        public string rdbbutton { get; set; }
        public string Status_Type { get; set; }     
        public string HREFFSA_Status { get; set; }
        public DateTime HREFFSA_Date { get; set; }
        public long HREFFSA_SanctionLevel { get; set; }
        public long HREFFSASanctionLevel { get; set; }
        public bool HREFFSA_ActiveFlag { get; set; }
        public long? HREFFSA_CreatedBy { get; set; }
        public long? HREFFSA_UpdatedBy { get; set; }

        public class monthdto
        {
            public long IVRM_Month_Id { get; set; }
        }
        public class employeeDTO
        {
            public long HRME_Id { get; set; }
        }

        public long UserId { get; set; }
        public long Emp_Code { get; set; }


        public Array display_data { get; set; }
        public Array getloaddetails { get; set; }
        public Array modalview { get; set; }
        public Array viewlist { get; set; }

        public string returnduplicatestatus { get; set; }


        //-----ism_certificate_request
        public long maxmumlevel { get; set; }
        public string RoleType { get; set; }
        public long roleid { get; set; }

        public long ISMCERTREQAPP_Id { get; set; }
        public long HRMEDS_Id { get; set; }



        public Array Cerfificatelist { get; set; }

        public string viewattachment { get; set; }
        public string attachment { get; set; }

        public string HRMEM_EmailId { get; set; }
        public string email { get; set; }
        public Array emaildetails { get; set; }
        public Array pagePrivilege { get; set; }

        public bool approved_flag { get; set; }
        public DateTime receiningdate { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public Array document { get; set; }

        public grid_array[] grid_arrays { get; set; }

        public class grid_array
        {
            public long HRME_Id { get; set; }

            public long? UserId { get; set; }
            public bool FinalFlg { get; set; }

        }
    }
}