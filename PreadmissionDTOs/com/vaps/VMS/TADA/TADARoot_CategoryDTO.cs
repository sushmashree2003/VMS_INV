using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public class TADARoot_CategoryDTO : CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public bool Flag { get; set; }
        public long HRME_Id { get; set; }
        public string Categoryname { get; set; }
        public string Statename { get; set; }
        public long StateId { get; set; }
        public long? IMER_ApplicableRegion { get; set; }
        public Array GetArray { get; set; }
        public Array GetEmployeeList { get; set; }
        public Array GetstateList { get; set; }
        public Array TADAPrimaryList { get; set; }
        public Array tADA_PrimaryAccountList { get; set; }
        public Array categorynamelists { get; set; }
        public Array statenamelists { get; set; }
        public Array StateList { get; set; }
        public Array Categorylist { get; set; }
        public Array Route_PrimaryDetails { get; set; }
        public Array primarytypeCount { get; set; }
        public Array primarytypelist { get; set; }
        

        public long categoryId { get; set; }

        public int createdBy { get; set; }
        public DateTime? createddate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? Updateddate { get; set; }

        public decimal? Percentage { get; set; }
        public long? IMECM_MinTarget { get; set; }
        public long? IMECM_MaxTarget { get; set; }


        public bool IRPA_Status { get; set; }
        public long IMECM_id { get; set; }
        public long ISMSMSO_ID { get; set; }

        public long IMECM_EmpId { get; set; }
        public long IMRC_CategoryId { get; set; }
        public decimal? IMRC_Percentage { get; set; }
        public bool IMECM_ActiveFlag { get; set; }
        public long? IMECM_Createdby { get; set; }
        public DateTime? IMECM_CreatedDate { get; set; }
        public long? IMECM_Updateby { get; set; }
        public DateTime? IMECM_UpdateDate { get; set; }
        public bool returnvalue { get; set; }
        public string returnval { get; set; }
        public string Empname { get; set; }
        public string Message { get; set; }
        public string retrunMsg { get; set; }
        
        public GetLIst[] GetList { get; set; }
        public RegionList[] regionList { get; set; }
        public ArrayPrimaryAccount[] arrayPrimaryAccount { get; set; }
        public class RegionList
        {
           public long IVRMMS_Id { get; set; }
        }
       

        public Array   GeatLoadCategory { get; set; }
        public class GetLIst
        {
            public long percentage { get; set; }
            public long categoryId { get; set; }
            public long HRME_Id { get; set; }
            public long MI_Id { get; set; }
            public long IMECM_id { get; set; }
            public long? IMECM_MinTarget { get; set; }
            public long? IMECM_MaxTarget { get; set; }


        }
        public long IMER_Id { get; set; }
  
        public long IMER_EmpId { get; set; }
        public long IVRMMS_Id { get; set; }
        public bool IMER_ActiveFlag { get; set; }
        public long IMER_Createdby { get; set; }
        public DateTime? IMER_CreatedDate { get; set; }
        public long IMER_Updateby { get; set; }
        public DateTime? IMER_UpdateDate { get; set; }
        public bool IMRC_ActiveFlag { get; set; }
        public long? IMRC_Createdby { get; set; }
        public DateTime? IMRC_CreatedDate { get; set; }
        public long? IMRC_Updateby { get; set; }
        public DateTime? IMRC_UpdateDate { get; set; }
        public TADARoot_CategoryDTO[] Primaryaccount { get; set; }
    
        public long IRPA_ID { get; set; }
       
        public int IRPA_Order { get; set; }
        public long IRPA_Createdby { get; set; }
        public DateTime? IRPA_CreatedDate { get; set; }
        public long IRPA_Updateby { get; set; }
        public DateTime? IRPA_UpdateDate { get; set; }
        public class ArrayPrimaryAccount
        {
            public long ISMSMSO_ID { get; set; }
        }
    }
    
      
    
}
