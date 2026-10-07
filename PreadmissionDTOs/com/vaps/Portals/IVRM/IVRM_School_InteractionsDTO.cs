using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.IVRM
{
    public class IVRM_School_InteractionsDTO
    {

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string trans_id { get; set; }
        public string interactionid { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public string userflag { get; set; }
        public bool IVRMGC_EnableSTIntFlg { get; set; }
        public bool IVRMGC_EnableCTIntFlg { get; set; }
        public bool IVRMGC_EnableHODIntFlg { get; set; }
        public bool IVRMGC_EnablePrincipalIntFlg { get; set; }
        public bool IVRMGC_EnableASIntFlg { get; set; }

        public long asmclid { get; set; }
        public long asmsid { get; set; }
        public long AMST_Id { get; set; }
        public long student_Id { get; set; }
        public long HRME_Id { get; set; }
        public long employee_Id { get; set; }
        public string AMST_AdmNo { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public long IVRMRT_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public bool IVRMGC_EnableECIntFlg { get; set; }
        public Array studentlist { get; set; }

        public string Role_flag { get; set; }
        public Array roletype { get; set; }
        public Array configflag { get; set; }
 
        public Array get_replies { get; set; }
        public Array viewmessage { get; set; }
        
        public Array getinboxmsg { get; set; }
        public long inboxcount { get; set; }

        public Array staffwiseclasslist { get; set; }
        public Array get_student { get; set; }

        public string employeeName { get; set; }
        public string studentName { get; set; }

        public long ISMINT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMINT_InteractionId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ISMINT_ComposedByFlg { get; set; }
        public string ISMINT_GroupOrIndFlg { get; set; }
        public long ISMINT_ComposedById { get; set; }
        public string ISMINT_Subject { get; set; }
        public DateTime ISMINT_DateTime { get; set; }
        public string ISMINT_Interaction { get; set; }
        public bool ISMINT_ActiveFlag { get; set; }
        public long ISMINT_CreatedBy { get; set; }
        public long ISMINT_UpdatedBy { get; set; }

        public long ISTINT_Id { get; set; }
        public long ISTINT_ToId { get; set; }
        public string ISTINT_ToFlg { get; set; }
        public long ISTINT_ComposedById { get; set; }
        public string ISTINT_Interaction { get; set; }
        public DateTime ISTINT_DateTime { get; set; }
        public string ISTINT_ComposedByFlg { get; set; }
        public int ISTINT_InteractionOrder { get; set; }
        public bool ISTINT_ActiveFlag { get; set; }
        public long ISTINT_CreatedBy { get; set; }
        public long ISTINT_UpdatedBy { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public string roleflg { get; set; }
        public Array getdetails { get; set; }
        public Array deviceids { get; set; }
        public arraySTDTO[] arrayST { get; set; }
        public arrayStudentDTO[] arrayStudent { get; set; }
        public arrayTeachersDTO[] arrayTeachers { get; set; }
    }

    public class arraySTDTO
    {
        public long HRME_Id { get; set; }
    }
    public class arrayStudentDTO
    {
        public long AMST_Id { get; set; }
    }
    public class arrayTeachersDTO
    {
        public long HRME_Id { get; set; }
    }
    public class numberingdto
    {
        public long IMN_Id { get; set; }
        public long MI_Id { get; set; }
        public string IMN_AutoManualFlag { get; set; }
        public string IMN_DuplicatesFlag { get; set; }
        public string IMN_StartingNo { get; set; }
        public string IMN_WidthNumeric { get; set; }
        public string IMN_ZeroPrefixFlag { get; set; }
        public bool IMN_PrefixAcadYearCode { get; set; }
        public string IMN_PrefixParticular { get; set; }
        public bool IMN_SuffixAcadYearCode { get; set; }
        public string IMN_SuffixParticular { get; set; }
        public string IMN_RestartNumFlag { get; set; }
        public string IMN_Flag { get; set; }
        public bool? IMN_PrefixFinYearCode { get; set; }
        public bool? IMN_PrefixCalYearCode { get; set; }
        public bool? IMN_SuffixFinYearCode { get; set; }
        public bool? IMN_SuffixCalYearCode { get; set; }
    }
}