using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Legal
{
    public class Legal_Notice_RequestDTO
    {
        public long LNR_Id { get; set; }
        public long MCST_Id { get; set; }        
        public long LNRH_Id { get; set; }
        public long LNC_Id { get; set; }
        public long INVMS_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
     
        public string dept_code { get; set; }
        public string MCST_Name { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        //public string departmentdatacode { get; set; }
        public string LNRH_Status { get; set; }
        public string INVMS_SupplierName { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public long HRME_Id { get; set; }
        public string LNC_Category_Name { get; set; }
        public long RequestingFor_id { get; set; }
        public long ASMAY_ID { get; set; }
        public string Role_flag { get; set; }
        public Array admindata { get; set; }
        public Array gridcourt { get; set; }
        public Array geteditdata { get; set; }
        public Array courtcomplex { get; set; }
        public Array courtestablishment { get; set; }
        public Array casetype { get; set; }
        public Array gridcasetypemapping { get; set; }
        public long IVRMRT_Id { get; set; }
        public string LNR_Remarks { get; set; }
        public string returnvales { get; set; }
        public string LNR_Status { get; set; }
        public string returnmsg { get; set; }
        public bool LNR_ActiveFlag { get; set; }
        public bool MCST_ActiveFlag { get; set; }
        public long FLAG { get; set; }
        public Array FlagOnLoadOrOnChangeInst { get; set; }
        public Array gridcasetype { get; set; }
        public Array geteditcase { get; set; }
        public DateTime? LNR_CreatedDate { get; set; }
        public DateTime? LNR_UpdatedDate { get; set; }
        public long? LNR_CreatedBy { get; set; }
        public long? LNR_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long LNRD_Id { get; set; }
        public Array employees { get; set; }
        public Array clientlist { get; set; }
        public Array vendorlist { get; set; }
        public Array category { get; set; }
        public Array category1 { get; set; }
        public Array griddata { get; set; }
        public Array griddata1 { get; set; }
        public filedto[] FilePath_Array { get; set; }
        public string LNRD_Document { get; set; }
        public Legal_Notice_RequestDTO[] temp1 { get; set; }

        //LNC_Court_Casetype_Mapping
        public long CCM_Id { get; set; }         
        public string CCM_Court { get; set; }
        public string CCM_Case { get; set; }
        public bool CCM_ActiveFlag { get; set; }
        public DateTime CCM_CreatedDate { get; set; }
        public DateTime CCM_UpdatedDate { get; set; }
        public long? CCM_CreatedBy { get; set; }
        public long? CCM_UpdatedBY { get; set; }


        //LNC_Master_Court_Type
        public long MCT_Id { get; set; }    
        public string MCT_Type { get; set; }
        public bool MCT_ActiveFlag { get; set; }
        public DateTime MCT_CreatedDate { get; set; }
        public DateTime MCT_UpdatedDate { get; set; }
        public long MCT_CreatedBy { get; set; }
        public long MCT_UpdatedBy { get; set; }
        public string update { get; set; }

        //LNC_Master_Court
        public long MC_Id { get; set; }      
        public string MC_Name { get; set; }
        public bool MC_ActiveFlag { get; set; }
        public DateTime MC_CreatedDate { get; set; }
        public DateTime MC_UpdatedDate { get; set; }
        public long? MC_CreatedBy { get; set; }
        public long? MC_UpdatedBy { get; set; }

        public class filedto
        {
            public string LNRD_Document { get; set; }
            public long LNRD_Id { get; set; }        
        }
    }
}
