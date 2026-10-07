using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{

    public class HR_Emp_SalaryAdvanceDTO : CommonParamDTO
    {
        public long HRME_Id { get; set; }
        public long HRESA_Id { get; set; }
        public long MI_Id { get; set; }
       
        public DateTime HRESA_EntryDate { get; set; }
        public string HRESA_AdvYear { get; set; }
        public string HRESA_AdvMonth { get; set; }
        public decimal? HRESA_AppliedAmount { get; set; }
        public string HRESA_Remarks { get; set; }
        public bool? HRESA_ActiveFlag { get; set; }
        public string HRESA_AdvStatus { get; set; }
        // public Array CheckRoleTypeMapped { get; set; }

        public decimal? HRESA_SanctinedAmount { get; set; }
        public string HRESA_ModeOfPayment { get; set; }
        public long HRESA_ReferenceNo { get; set; }
        public Array empadvaList { get; set; }
        public Array preadvance { get; set; }
        
        public Array advanceapproval { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public bool returnval { get; set; }
        public Array griddisplay { get; set; }
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
        public string IVRM_Month_Name { get; set; }
        public monthdto[] MONTHID { get; set; }
        public employeeDTO[] HRMEID { get; set; }

        public HR_ConfigurationDTO configurationDetails { get; set; }

        public decimal? empGrossSal { get; set; }
        public decimal? empEarningGrossSal { get; set; }
        public decimal? empDeductionGrossSal { get; set; }
        public decimal? totalAppliedAmount { get; set; }
        public string searchfilter { get; set; }
        public long LogInUserId { get; set; }


        public long HRESAA_Id { get; set; }
        public long yearid { get; set; }
        public long IVRMUL_Id { get; set; }
        public string rdbbutton { get; set; }
        public string Status_Type { get; set; }
        public string HRESAA_Remarks { get; set; }
        public DateTime HRESAA_Date { get; set; }
        public int HRESAA_SanctionLevel { get; set; }
        public bool HRESAA_ActiveFlag { get; set; }
        public long? HRESAA_CreatedBy { get; set; }
        public long? HRESAA_UpdatedBy { get; set; }
        public string HRESAA_Status { get; set; }
        public decimal? HRESAA_Amount { get; set; }

        public class monthdto
        {
            public string IVRM_Month_Name { get; set; }
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


