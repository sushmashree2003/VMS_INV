using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class regis : CommonParamDTO
    {
        public int AOAD_ID { get; set; }
        public string student_name { get; set; }
        public int AMCL_ID { get; set; }
        public string Email_id { get; set; }
        public string EmailConfirmed { get; set; }
        public string Mobileno { get; set; }
        public string username { get; set; }

        public long? admittedyer { get; set; }
        public long? leftyear { get; set; }
        public long?  leftclassid { get; set; }

        public long? classidadmitted { get; set; }

        public string password { get; set; }

        public char schoolcollege { get; set; }

        public string Logintype { get; set; }
        public bool twofactorcheck { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
        public string user_ip_address { get; set; }
        public long MI_Id { get; set; } 
        public DateTime Entry_Date { get; set; }
        public string Machine_Ip_Address { get; set; }
        public string returnMsg { get; set; }
        public long User_Id { get; set; }
        public string token { get; set; }
        public string confirmationLink { get; set; }
        public string profile_pic_path { get; set; }

        public string RoleTypeFlag { get; set; }

        public string Name { get; set; }

        public string Special { get; set; }

        public string Alumni { get; set; }

          public long? courseadmittted { get; set; }

        public long? courseleft { get; set; }
        public long? branchadmittted { get; set; }
        public long? branchleft { get; set; }

        public long? semadmittted { get; set; }
        public long? semleft { get; set; }


        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string logindetails { get; set; }

        public long ASMAY_Id { get; set; }
        public long HRCD_Id { get; set; }

        public Array transnumconfig { get; set; }


        public Array PaymentDetailsList { get; set; }

        public string mobiledeviceid { get; set; }


    }

    public class logindetailsArray
    {
        public string ip { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public string browser { get; set; }
        public string version { get; set; }
        public string os { get; set; }
    }
}




