using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Rating
{
    public class Rating_TransactionDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public long RATMT_Id { get; set; }
        public string FlagOnLoadOrOnChangeInst { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array GetRatingMasterType { get; set; }
        public Array GetEmployeeDetails { get; set; }
        public Array getuserinstitution { get; set; }

        public Array GetTypeQuestions { get; set; }
        public Array GetTypeQuestionOptions { get; set; }
        public Array GetSavedDetails { get; set; }
        public long RATMOP_OptionsValue { get; set; }
        public string RATMOP_RatingOptions { get; set; }
        public string RATMT_RatingTypeName { get; set; }
        public string MI_Name { get; set; }
        public int RATMOP_ROOrder { get; set; }
        public long RATMQE_Id { get; set; }
        public long RATMOP_Id { get; set; }
        public DateTime? RATTR_FeedbackDate { get; set; }
        public DateTime? RATTR_CreatedDate { get; set; }
        public Save_Rating_Details[] Save_Rating_Details { get; set; }
        public Save_AutoRating_Details[] Save_AutoRating_Details { get; set; }
        public bool returnval { get; set; }
        public bool roleflag { get; set; }
        public string Role_flag { get; set; }
        public bool? RATMT_QuestionwiseOptionFlg { get; set; }
        public long IVRMRT_Id { get; set; }
        public string empname { get; set; }
        public string empdepartment { get; set; }
        public Array yearlist { get; set; }

        public Array Month_array { get; set; }
        public Array get_userEmplist { get; set; }

        public Array DepartmentList { get; set; }

        public Array Designation { get; set; }
        public Array consolidateData { get; set; }
        public long HRMDES_Id { get; set; }
        public long year { get; set; }
        public long monthid { get; set; }
        public long HRMD_Id { get; set; }
        public arrayuserEmployeeDTO[] arrayuserEmp { get; set; }

        public arrayuserdesDTO[] arrayuserdes { get; set; }
        public DepartmentDTO[] departmentlist { get; set; }
        public monthListDTO[] monthList { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public Array GetSavedDataOnLoad { get; set; }
        public Array ratingtype { get; set; }

        public ratingtypelists[] ratingtypelist { get; set; }


        //Akash
        public Array getRatingTypeQuestion { get; set; }
        public long RATMTQ_Id { get; set; }
        public string typeNameList { get; set; }
        public string questionNameList { get; set; }
        public long RATMTQR_Id { get; set; }
        public string returnmsg { get; set; }
        public decimal RATMTQR_FromValue { get; set; }
        public decimal RATMTQR_ToValue { get; set; }
        public string RATMTQR_Rating { get; set; }
        public long RATMTQR_UpdatedBy { get; set; }
        public DateTime RATMTQR_UpdatedDate { get; set; }
        public bool RATMTQR_ActiveFlag { get; set; }
        public long RATMTQR_CreatedBy { get; set; }
        public DateTime RATMTQR_CreatedDate { get; set; }
        public Array overAllTypeGrid { get; set; }
        public decimal fromvaluelist { get; set; }
        public decimal tovaluelist { get; set; }
        public string ratinglist { get; set; }
        public Array EditDataOverAll { get; set; }
        public Array getRatingType { get; set; }
        public Array getRatingQuestion { get; set; }

        public string flag { get; set; }
        public Array GetEmployeeUnratedDetails { get; set; }

        // sudarshan 01-12-2022
        public string RATMQE_RatingQuestions { get; set; }
        public bool? RATMQE_ManualEntryFlg { get; set; }
        public int RATMQE_RQOrder { get; set; }

        public DateTime? HRME_DOJ { get; set; }
        public long RATTR_Id { get; set; }
        public bool? RATMQE_AutomaticFlg { get; set; }
        public Array employeeprofile { get; set; }
        public Array getRatingTypedata { get; set; }
        public Array viewEmployeeRating { get; set; }
        public Array prioritylist { get; set; }
        public Array getTypeQuestionTransaction { get; set; }
        public Array getRatingTypeTransaction { get; set; }
        public Array getTypeQuestionOptionTransaction { get; set; }
        public string RATMQE_RatingsQRemarks { get; set; }
        public Array autoratingdetail_lieo { get; set; }
        public Array autoratingdetail_ul { get; set; }
        public Array autoratingdetail_deviation { get; set; }
        public Array autoCaluclatedRating { get; set; }

       
        // Sudarshan 20-06-2023
        public long RATMQECL_Id { get; set; }
        public string RATMQECL_QuestionsClassification { get; set; }
        public long RATMQECLSUB_Id { get; set; }
        public string RATMQECLSUB_QuestionsSubclassification { get; set; }
        public Save_Behaviour_Rating_Details[] Save_Behaviour_Rating_Detail { get; set; }
        public Array getRatingClassificationTransaction { get; set; }
        public Array getRatingSubClassificationTransaction { get; set; }
        public Array getDresscodetransaction { get; set; }
        public Array getAppearancetransaction { get; set; }
        public Array getbehaviourtransaction { get; set; }
        public Array GetTypeClassificationQuestions { get; set; }
        public Array GetTypeSubClassificationQuestions { get; set; }
        public employeedata[] employeedata { get; set; }
        public long ISMMTCAT_Id { get; set; }
        public Array ratingRoleCategoryEmployeerTasks { get; set; }
        public Array taskcategoryquestionoptionlist { get; set; }
        public long RATTCT_Id { get; set; }
        public Save_Rating_TaskCategory[] Save_Rating_TaskCategory { get; set; }
    }
    public class DepartmentDTO
    {
        public int HRMDC_ID { get; set; }        
        public int HRMD_ID { get; set; }       
    }
    public class arrayuserEmployeeDTO
    {
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
    }
    public class arrayuserdesDTO
    {
        public long HRMDES_Id { get; set; }
    }
    public class monthListDTO
    {
        public int IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
    }
    public class Save_Rating_Details
    {
        public long RATMT_Id { get; set; }
        public classificationlist[] classificationlist { get; set; }

    }
    public class classificationlist
    {
        public long RATMQECL_Id { get; set; }
        public string RATMQECL_QuestionsClassification { get; set; }
        public subclassificationlist[] subclassificationlist { get; set; }
    }
    public class subclassificationlist
    {
        public long RATMQECLSUB_Id { get; set; }
        public string RATMQECLSUB_QuestionsSubclassification { get; set; }
        public questionlist[] questionlist { get; set; }
    }

    public class questionlist
    {
        public long RATMQE_Id { get; set; }
        public long RATTR_Id { get; set; }
        public string name { get; set; }
        public bool RATMQE_ManualEntryFlg { get; set; }
    }

    public class Save_AutoRating_Details
    {
        public long RATMT_Id { get; set; }
        public questionclassificationdata[] questionclassificationdata { get; set; }
       
    }
    public class questionclassificationdata
    {
        public long RATMQECL_Id { get; set; }
        public string ratmqecL_QuestionsClassification { get; set; }
        public questionsubclassificationdata[] questionsubclassificationdata { get; set; }
    }
    public class questionsubclassificationdata
    {
        public long RATMQECLSUB_Id { get; set; }
        public string RATMQECLSUB_QuestionsSubclassification { get; set; }
        public questionlistdata[] questionlistdata { get; set; }
    }
    public class questionlistdata
    {
       public long RATTR_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public long RATMT_Id { get; set; }
        public long RATMOP_Id { get; set; }
        public int RATMOP_OptionsValue { get; set; }
    }

    public class ratingtypelists
    {
        public long RATMT_Id { get; set; }
    }
    public class Save_Behaviour_Rating_Details
    {
        public long HRME_Id { get; set; }
        public long RATMT_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public string name { get; set; }
        public long RATTR_Id { get; set; }
        public bool RATMQE_ManualEntryFlg { get; set; }
    }
    public class employeedata
    {
        public long HRME_Id { get; set; }
    }
    public class Save_Rating_TaskCategory
    {
        public long ISMTCR_Id { get; set; }
        public long HRME_Id { get; set;}
        public int RATMOP_OptionsValue { get; set; }
        public string ISMMTCAT_TaskCategoryName { get; set; }
    }
}
