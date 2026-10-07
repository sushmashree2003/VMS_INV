using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class LoginDTO : CommonParamDTO
    {
        public long AMST_Id { get; set; }
        public string message { get; set; }
        public Array moduleList { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string logindetails { get; set; }
        public Array moduleListonly { get; set; }
        public Array pageList { get; set; }
        public Array moduleCategoryList { get; set; }
        // Added on 5-11-2016
        public long roleId { get; set; }
        public bool IsLogged { get; set; }
        // Added on 5-11-2016
        // Added on 10-11-2016 to store login user id
        public long userId { get; set; }
        public string Email { get; set; }

        public string Mobile { get; set; }
        public string newPassword { get; set; }
        // Added on 10-11-2016 to store login user id

        // Added on 16-11-2016 to store login user MI id
        public long IVRM_MO_Id { get; set; }
        // public long IVRM_MI_Id { get; set; }
        public List<MI_ID_DTO> Mi_Id_List { get; set; }
        public long ASMAY_Id { get; set; }

        public long IMFY_Id { get; set; }

        public long role { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }

        public string mobileprivileges { get; set; }
        public string Useremail { get; set; }

        public long IVRMIMP_Id { get; set; }
        public string usermob { get; set; }

        public string Userappname { get; set; }
        public Array userData { get; set; }

        public long MI_ID { get; set; }
        //added on 20 dec 2016
        public string otp { get; set; }
        public string userOTP { get; set; }
        public string mobileNo { get; set; }

        public Array fillstaff { get; set; }
        public string schoolcollegeflag { get; set; }
        public string UserImage { get; set; }
        public string UserImagePath { get; set; }

        //Date:26-12-2016 for displaying privileges.
        public Array privileges { get; set; }
        public long pageId { get; set; }
        public bool IVRMIRP_AddFlag { get; set; }
        public bool IVRMIRP_UpdateFlag { get; set; }
        public bool IVRMIRP_DeleteFlag { get; set; }
        public bool IVRMIRP_ReportFlag { get; set; }
        public bool IVRMIRP_SearchFlag { get; set; }
        public bool IVRMIRP_ProcessFlag { get; set; }

        public string MOBILEOTP { get; set; }

        public string EMAILOTP { get; set; }

        public string UserNameVerifyStatus { get; set; }

        public string returnMsg { get; set; }

        public string Pagename { get; set; }

        public string Pageicon { get; set; }

        public string Pageurl { get; set; }

        public string ForgetPasswordSelection { get; set; }
   
        public long totalfeed { get; set; }
        public long notifycount { get; set; }
        // Added on 16-11-2016 to store login user MI id


        //added on 11 jan

        public Array configlist { get; set; }

        public Array Staffmobileappprivileges { get; set; }

        public Array transnumconfig { get; set; }

        public DateTime cutoffdate { get; set; }

        public DateTime subscriptionenddate { get; set; }

        public bool subscriptionFlag { get; set; }


        public Array feeconfiglist { get; set; }
        public Array institutionlist { get; set; }
        public Array multiAdminSelectedDetails { get; set; }
        public Array admissioncongigurationList { get; set; }

        public string ASMAY_Year { get; set; }

        public string flag { get; set; }


        public Array MIdata { get; set; }


        public Array fillyear { get; set; }

        public Array ivrmconfiglist { get; set; }

        public Array fillinstitution { get; set; }

        public Array EmailarrayList { get; set; }

        public Array PhonearrayList { get; set; }

        public Array fillinstition { get; set; }

        public Array allyeargetlogin { get; set; }

        public Array allclasslogin { get; set; }

        public string subDomainName { get; set; }

        public Array Manadatoryfields { get; set; }
        public string htmldata { get; set; }

        public Array studentdata { get; set; }


        public string admno { get; set; }
        public string clsnme { get; set; }
        public string secnme { get; set; }
        public DateTime dob { get; set; }
        public string emailid { get; set; }
        public long mobno { get; set; }
        public string imgnme { get; set; }
        public string studname { get; set; }

        public string studnamehalf { get; set; }
        public bool payment { get; set; }
        public bool registration { get; set; }
        public string roleforlogin { get; set; }
        public long empcode { get; set; }
        public long MI_Idforlogin { get; set; }
        public string employeename { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string employee_id { get; set; }
        public Array fillinstition1 { get; set; }

        public Array filldashpagemap { get; set; }

        public Array pageexists { get; set; }

        public string clickedlinkname { get; set; }
        public string instituteName { get; set; }
        public string instituteLogo { get; set; }
        public long RoleTypeId { get; set; }
        public long Emp_Id { get; set; }
        public Array storagedetails { get; set; }

        public string searchfilter { get; set; }
        public string usertype { get; set; }

        public Array userlist { get; set; }

        public string usernametype { get; set; }
       public string department { get; set; }
       public string designation { get; set; }
        public Array filldata { get; set; }
        public Array no_planner_creation { get; set; }
        public string columnnames { get; set; }
        public string totalworkingdays { get; set; }
        public long count_d { get; set; }
    }
}
