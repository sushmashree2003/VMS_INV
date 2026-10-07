using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMS_CorrespondenceDTO
    {
        public long FMSCOR_Id { get; set; }
        public long FMSCOREMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long FMSMFC_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long FMSCORCL_Id { get; set; }
        public long FMSCORSL_Id { get; set; }
        public long FMSCORSUP_Id { get; set; }
        public DateTime? FMSCOR_Date { get; set; }
        public string FMSCOR_ClientSupplierFlg { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public string INVMS_SupplierName { get; set; }
        public string FMSCOR_Subject { get; set; }
        public string FMSCOR_RefernceNo { get; set; }
        public string FMSCOR_Description { get; set; }
        public string FMSCOR_LetterHeadNo { get; set; }
        public string FMSCOR_ToName { get; set; }
        public string TabNo { get; set; }
        public bool FMSCOR_ActiveFlg { get; set; }
        public DateTime? FMSCOR_CreatedDate { get; set; }
        public DateTime? FMSCOR_UpdatedDate { get; set; }
        public long? FMSCOR_CreatedBy { get; set; }
        public long? FMSCOR_UpdatedBy { get; set; }
        public long? userid { get; set; }
        public Array getclient { get; set; }
        public Array getproject { get; set; }
        public Array getsuppliers { get; set; }
        public Array getsaleslead { get; set; }
        public Array getfilecategory { get; set; }
        public Array institutiondeptdetails { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long? ISMMPR_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long INVMS_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public string saveorupdate { get; set; }
        public bool returnval { get; set; }
        public Array getfirstgriddata { get; set; }
        public string FMSMFC_FileCategoryName { get; set; }
        public string IMFY_FinancialYear { get; set; }
        public string manualorautorefno { get; set; }
        public string messageval { get; set; }
        public string secondorthirdtab { get; set; }
        public Array geteditdatafirsttab { get; set; }
        public Array geteditclientdata { get; set; }
        public Array geteditsupplierdata { get; set; }
        public Array geteditsalesdata { get; set; }
        public Array getgeneralconfig { get; set; }
        public Array getviewclientdata { get; set; }
        public Array getviewsalesdata { get; set; }
        public Array getviewsupplierdata { get; set; }
        public Array checkcabinetdetails { get; set; }

        public Array get_lead_details { get; set; }
        
        public bool activeflag { get; set; }
        public bool? FMSCOREMP_ActiveFlg { get; set; }
        public long detailsid { get; set; }
        public long details_pkid { get; set; }
        public employeelist[] employeelist { get; set; }
        public Array getemployeelist { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeName { get; set; }
        public Array getviewemployeedetails { get; set; }
        public Array geteditemployeedetails { get; set; }

        /*SECOND TAB*/
        public Array getclientfiles { get; set; }
        public Array getprojectfiles { get; set; }
        public Array getsuppliersfiles { get; set; }
        public Array getsalesleadfiles { get; set; }
        public Array getfilecategoryfiles { get; set; }
        public Array getphysicalfilelist { get; set; }
        public Array getcabinetlist { get; set; }
        public Array getreferncenolist { get; set; }
        public Array getsecondtabdata { get; set; }
        public Array getrows { get; set; }
        public Array getcolumns { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public long FMSMFCBT_Id { get; set; }
        public long FMSMPFN_Id { get; set; }
        public long FMSMFCR_Id { get; set; }
        public long FMSMFCC_Id { get; set; }
        public long FMSCORFL_Id { get; set; }
        public string physicalfilename { get; set; }
        public string cabinetname { get; set; }
        public string cabinetrow { get; set; }
        public string cabinetcolumn { get; set; }
        public bool FMSCORFL_ActiveFlg { get; set; }
        public Array getfmcoriddetails { get; set; }
        public Array getfilesdetails { get; set; }
        public Array getsecondeditdata { get; set; }
        public Array filenamedetails { get; set; }
        public FMS_Temp_UploadFiles[] FMS_Temp_UploadFiles { get; set; }

        /*THIRD TAB*/
        public Array getclientmode { get; set; }
        public Array getprojectmode { get; set; }
        public Array getsuppliersmode { get; set; }
        public Array getsalesleadmode { get; set; }
        public Array getfilecategorymode { get; set; }
        public Array getdispatchmode { get; set; }
        public Array getthirdeditdata { get; set; }
        public Array getfmcoriddetails1 { get; set; }
        public Array geteditclientdata_third { get; set; }
        public Array geteditsupplierdata_third { get; set; }
        public Array geteditsalesdata_third { get; set; }
        public Array getthirdtabdata { get; set; }
        public long FMSCORMD_Id { get; set; }
        public long FMSMDMD_Id { get; set; }
        public long FMSCORFLSC_Id { get; set; }
        public long countflag { get; set; }
        public string FMSCORMD_ReferenceNo { get; set; }
        public DateTime? FMSCORMD_SentDate { get; set; }
        public FMS_Temp_UploadFiles_Mode[] FMS_Temp_UploadFiles_Mode { get; set; }
        public bool FMSCORMD_ActiveFlg { get; set; }
        public string FMSMTMD_DispatchMode { get; set; }
        public string FMSCORMD_FileName { get; set; }
        public string FMSCORMD_FilePath { get; set; }
        public string username { get; set; }
        public FlagReferncetype[] flagtypes { get; set; }
        public Array getrefnodetails { get; set; }
        public Array getclientdetails { get; set; }
        public Array getreferncedetails { get; set; }
        public Array getviewfile { get; set; }
    }
    public class FMS_Temp_UploadFiles
    {
        public long fmscorflsC_Id { get; set; }
        public string fmscorfL_SoftCopyFileName { get; set; }
        public string fmscorfL_SoftCopyFilePath { get; set; }
    }
    public class FMS_Temp_UploadFiles_Mode
    {
        public long fmscormD_Id { get; set; }
        public string fmscormD_FileName { get; set; }
        public string fmscormD_FilePath { get; set; }
    } 
    public class FlagReferncetype
    {      
        public string FMSCOR_ClientSupplierFlg { get; set; }        
    }

    public class employeelist
    {
        public long HRME_Id { get; set; }
        public long FMSCOREMP_Id { get; set; }
    }
}
