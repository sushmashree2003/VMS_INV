using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class Lead_Target_MappingnewDTO
    {

        public long User_Id { get; set; }
        public long MI_Id { get; set; }

        public long ISMSLETARM_Id { get; set; }

        public long HRME_Id { get; set; }


        //raj
        public Array departmentdropdown { get; set; }
        public Array employeedropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array TargetMappinglists { get; set; }

        public Emptwolists[] Emptwolist { get; set; }
        public Array status_dd { get; set; }
        public Array emp_list { get; set; }
        public Array emp_list_cat { get; set; }

        public Array emp_target_count { get; set; }
        
        public string retrunMsg { get; set; }
        public long?[] hrmdes_IdList { get; set; }
        public long?[] hrmD_IdList { get; set; }

        public catDTO[] saleItemTax { get; set; }
        public targetedit[] tagtedit { get; set; }
        public Employeelist[] Emplistnew { get; set; }
        public class Emptwolists
        {
            public long HRME_Id { get; set; }

           
        }
        public class Employeelist
        {
            public long HRME_Id { get; set; }
            public catDTO[] selectedcat { get; set; }
        }
        public class catDTO
        {
            public long ISMMTCAT_Id { get; set; }
            public long ISMSLETARM_Numbers { get; set; }
            public string ISMMTCAT_TaskCategoryName { get; set; }
            public string ISMSLETARM_Periodicity { get; set; }
           


        }
        public class targetedit
        {
            public long ISMMTCAT_Id { get; set; }
            public long ISMSLETARM_Numbers { get; set; }
            public string ISMMTCAT_TaskCategoryName { get; set; }
            public string ISMSLETARM_Periodicity { get; set; }
            public long HRME_Id { get; set; }
            public DateTime? ISMSLETARM_CreatedDate { get; set; }
            public long ISMSLETARM_Id { get; set; }
        }


        public long ISMMTCAT_Id { get; set; }
        public long ISMSLETARM_Numbers { get; set; }
        public string ISMSLETARM_Periodicity { get; set; }
        public long ISMSLETARM_ApprovedBy { get; set; }
        public bool ISMSLETARM_ActiveFlag { get; set; }
        public DateTime? ISMSLETARM_UpdatedDate { get; set; }

        public string TargetCreatedDate { get; set; }


    }
}
