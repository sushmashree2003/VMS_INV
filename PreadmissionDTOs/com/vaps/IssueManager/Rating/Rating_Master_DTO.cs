using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Rating
{
    public class Rating_Master_DTO
    {
        public long MI_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        public long UserId { get; set; }
        public long? HRMDC_ID { get; set; }
        public long MappedCount { get; set; }
        public long? CreatedBy { get; set; }
        public string FlagOnLoadOrOnChangeInst { get; set; }
        public string returnmsg { get; set; }
        public bool returnval { get; set; }
        public Array getuserinstitution { get; set; }
        public DateTime? CreatedDate { get; set; }
        // Master Type
        public long RATMT_Id { get; set; }
        public string RATMT_RatingTypeName { get; set; }
        public bool? RATMT_DepartmentwiseFlg { get; set; }
        public string RATMT_PeriodicityFlg { get; set; }
        public bool? RATMT_QuestionwiseOptionFlg { get; set; }
        public bool? RATMT_ActiveFlag { get; set; }
        public Array GetMasterTypeLoadData { get; set; }
        public Array GetMaterTypeEditData { get; set; }
        public Array CheckRatingTypeMapped { get; set; }

        //Master Classification
        public long RATMQECL_Id { get; set; }
        public string RATMQECL_QuestionsClassification { get; set; }
        public long RATMQECL_RQOrder { get; set; }
        public bool? RATMQECL_ActiveFlag { get; set; }
        public Array getMasterQuestionClassificationLoadData { get; set; }
        public Array GetQuestionClassificationEditData { get; set; }
        public Array CheckRatingQuestionClassificationMapped { get; set; }

        //Master Sub Classification 
        public long RATMQECLSUB_Id { get; set; }
        public string RATMQECLSUB_QuestionsSubclassification { get; set; }
        public long RATMQECLSUB_RQOrder { get; set; }
        public bool RATMQECLSUB_ActiveFlag { get; set; }
        public long? RATMQECLSUB_CreatedBy { get; set; }
        public DateTime? RATMQECLSUB_CreatedDate { get; set; }
        public Array getQuestionClassificationData { get; set; }
        public Array getMasterQuestionsubClassificationLoadData { get; set; }
        public Array GetQuestionSubClassificationEditData { get; set; }
        public Array CheckRatingQuestionSubClassificationMapped { get; set; }

        //Master Questions
        public long RATMQE_Id { get; set; }

        public long RATMQEAUT_Id { get; set; }
        public string RATMQE_RatingQuestions { get; set; }
        public bool? RATMQE_ManualEntryFlg { get; set; }
        public bool? RATMQE_AutoEntryFlg { get; set; }
        public string RATMQE_RatingsQRemarks { get; set; }
        public int RATMQE_RQOrder { get; set; }
        public bool? RATMQE_ActiveFlag { get; set; }
        public string RATMQE_QuestionFlg { get; set; }
        public Array GetMasterQuestionLoadData { get; set; }
        public Array GetMaterQuestionEditData { get; set; }
        public Array GetMasterQuestionOrderData { get; set; }
        public Array getQuestionSubClassificationdata { get; set; }

        public Temp_Question_Order[] Temp_Question_Order { get; set; }

        //Master Option
        public long RATMOP_Id { get; set; }
        public string RATMOP_RatingOptions { get; set; }
        public long RATMOP_OptionsValue { get; set; }
        public string RATMOP_RatingORemarks { get; set; }
        public int? RATMOP_ROOrder { get; set; }
        public bool? RATMOP_ActiveFlag { get; set; }
        public Array GetMasterOptionLoadData { get; set; }
        public Array GetMasterOptionEditData { get; set; }
        public Array GetMasterOptionOrderData { get; set; }
        public Temp_Option_Order[] Temp_Option_Order { get; set; }

        //Type Question Mapping
        public Array GetMasterTypeData { get; set; }
        public Array GetMasterQuestionData { get; set; }
        public Array getTypeMasterQuestionMapping { get; set; }
        public Array GetDepartmentData { get; set; }
        public Array GetQuestionDeptDesigData { get; set; }
        public Array getDesginationData { get; set; }
        public long RATMTQ_Id { get; set; }
        public bool? RATMTQ_ActiveFlag { get; set; }

        // Type Question Department
        public long RATMTQD_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }

        public string HRMDES_DesignationName { get; set; }
        public Array ViewQuestionDeptDesigData { get; set; }
        public long Deptid { get; set; }
        public long Designationid { get; set; }
        public bool CheckBoxDeptDes { get; set; }

        //Type Question Option
        public long RATMTQO_Id { get; set; }
        public Array GetMasterOptionData { get; set; }
        public Array GetMasterTypeQuestionOptionMapping { get; set; }
        public Array ViewMasterTypeQuestionOptionMapping { get; set; }
        public Array ViewAuthoritydata { get; set; }
        public Temp_Department[] Temp_Department { get; set; }
        public Temp_Option[] Temp_Option { get; set; }
        public GetMasterQuestionDatalists[] GetMasterQuestionDatalist { get; set; }
        public bool? RATMT_LInkedToRelievingFlg { get; set; }
        public Array GetDesginationDataAuthority { get; set; }

        // Rating Authority
        public long RATMAUT_Id { get; set; }
        public bool? RATMAUT_ActiveFlag { get; set; }
        public DateTime? RATMAUT_CreatedDate { get; set; }
        public long RATMAUT_CreatedBy { get; set; }
        public Array getMasterAuthority { get; set; }
        public designationlistarrays[] designationlistarray { get; set; }

        //OverRating
        public long RATOVERR_Id { get; set; }
        public decimal RATOVERR_FromValue { get; set; }
        public decimal RATOVERR_ToValue { get; set; }
        public decimal RATOVERR_Rating { get; set; }
        public Array OverAllRatingGrid { get; set; }
        public Array EditDataOverAll { get; set; }
        public Array viewAuthorities { get; set; }
        public Array OverAllTypeGrid { get; set; }
        public Array getAuthority { get; set; }

        //Rating_Master_Authority_Auestion
        public long RATMAUTTYQ_Id { get; set; }
        public bool RATMAUTTYQ_ActiveFlag { get; set; }
        public long RATMAUTTYQ_CreatedBy { get; set; }
        public DateTime RATMAUTTYQ_CreatedDate { get; set; }



        public Array GetRatingTypeQuestionMappingLoadData { get; set; }
        public Array getRatingTypeData { get; set; }

        public Array getRatingTypeQuestionData { get; set; }
        public questionlistarray1[] questionlistarray { get; set; }

        public int RATMTQ_TQOrder { get; set; }
        public Array viewarray { get; set; }

        // Sudarshan 29-11-2022

        public long[] RATMT_Array { get; set; }
        public Department_Array1[] Department_Array { get; set; }



    }

    public class designationlistarrays
    {
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
    }

    public class questionlistarray1
    {
        public long RATMQE_Id { get; set; }
        public long RATMT_Id { get; set; }
        public long RATMTQ_Id { get; set; }
    }

    public class Department_Array1
    {
        public long HRMD_Id { get; set; }


    }

    public class Temp_Question_Order
    {
        public long RATMQE_Id { get; set; }
        public string RATMQE_RatingQuestions { get; set; }
        public int RATMQE_RQOrder { get; set; }

    }

    public class Temp_Option_Order
    {
        public long RATMOP_Id { get; set; }
        public string RATMOP_RatingOptions { get; set; }
        public int RATMOP_ROOrder { get; set; }
    }

    public class Temp_Department
    {
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Temp_Designation[] TempDesgin { get; set; }
    }
    public class Temp_Designation
    {
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
    }

    public class Temp_Option
    {
        public long RATMOP_Id { get; set; }
        public string RATMOP_RatingOptions { get; set; }
    }
    public class GetMasterQuestionDatalists
    {
        public long RATMQE_Id { get; set; }

    }
}
