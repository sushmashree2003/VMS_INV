using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMS_Correspondance_ReportDTO
    {
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long Userid { get; set; }
        public long FMSCOR_Id { get; set; }
        public string ClientSupplierFlgflag { get; set; }
        public string SearchValue { get; set; }
        public string flag { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public long ISMMPR_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public long INVMS_Id { get; set; }
        public string INVMS_SupplierName { get; set; }
        public long ISMSLE_Id { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public Array getclient { get; set; }
        public Array getsupplier { get; set; }
        public Array getsaleslead { get; set; }
        public Array getproject { get; set; }
        public Array getreport { get; set; }
        public Array getcabintdetails { get; set; }
        public Array getfilesdetails { get; set; }
        public Array getmodedetails { get; set; }
        public Array getdetails { get; set; }
        public Array getfilecategory { get; set; }
        public Array institutionlist { get; set; }
        public FMS_Correspondance_Report_Client_DTO[] FMS_Correspondance_Report_Client_DTO { get; set; }
        public FMS_Correspondance_Report_Project_DTO[] FMS_Correspondance_Report_Project_DTO { get; set; }
        public FMS_Correspondance_Report_Supplier_DTO[] FMS_Correspondance_Report_Supplier_DTO { get; set; }
        public FMS_Correspondance_Report_Sales_DTO[] FMS_Correspondance_Report_Sales_DTO { get; set; }
        public File_Category_DTO[] File_Category_DTO { get; set; }
    }
    public class FMS_Correspondance_Report_Client_DTO
    {
        public long ISMMCLT_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
    }
    public class FMS_Correspondance_Report_Project_DTO
    {
        public long ISMMPR_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
    }
    public class FMS_Correspondance_Report_Supplier_DTO
    {
        public long INVMS_Id { get; set; }
        public string INVMS_SupplierName { get; set; }
    }
    public class FMS_Correspondance_Report_Sales_DTO
    {
        public long ISMSLE_Id { get; set; }
        public string ISMSLE_LeadName { get; set; }
    }
    public class File_Category_DTO
    {
        public long FMSMFC_Id { get; set; }       
    }
}