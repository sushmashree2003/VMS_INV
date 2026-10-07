using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AutoMapper;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using CommonLibrary;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentTCImpl : Interfaces.StudentTCInterface
    {


        string Student_first_name_null;
        string Student_middle_name_null;
        string Student_last_name_null;
        string Student_reg_no_null;
        string Student_Name_Var;
        string stsno;
        string Student_adm_no_null;
        string Student_dob_words_null;
        string Student_IC_Caste_name_null;
        long Student_Mobile_no__null;
        string Student_Amc_name_null;
        int Student__age__null;
        string Student_Gender_null;
        long Student_aadhar_no_null;
        string Student_Blood_Group_null;
        int Student_IC_Id_null;
        string Student_first_null;

        private static ConcurrentDictionary<string, StudentTCDTO> _login =
            new ConcurrentDictionary<string, StudentTCDTO>();
        // create context obj
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ActivateDeactivateContext _act;
        ILogger<StudentTCImpl> _tcimpl;
        //private readonly UserManager<ApplicationUser> _UserManager;
        //private readonly ILogger<StudentAdmissionImp> _log;

        public StudentTCImpl(DomainModelMsSqlServerContext db, ActivateDeactivateContext act, ILogger<StudentTCImpl> _impltc)
        {
            _db = db;
            _act = act;
            _tcimpl = _impltc;
        }

        //Loading Intial Data
        public async Task<StudentTCDTO> GetStudentInitialData(StudentTCDTO DTO)
        {
            StudentTCDTO attdto = new StudentTCDTO();
            List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
            admissionconfigurationsettings = _db.AdmissionStandardDMO.AsNoTracking().Where(t => t.MI_Id == DTO.MI_Id).ToList();
            attdto.admissioncongigurationList = admissionconfigurationsettings.ToArray();

            List<StudentTCDTO> ln = new List<StudentTCDTO>();

            List<MasterAcademic> allyear = new List<MasterAcademic>();
            allyear = _db.AcademicYear.Where(t => t.MI_Id == DTO.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            attdto.academicList = allyear.ToArray();

            attdto.currentYear = _db.AcademicYear.Where(a => a.MI_Id == DTO.MI_Id && a.Is_Active == true && a.ASMAY_Id == DTO.ASMAY_Id).ToArray();


            List<School_M_Class> allclass = new List<School_M_Class>();
            allclass = _db.admissioncls.Where(s => s.MI_Id == DTO.MI_Id && s.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToList();
            attdto.classlist = allclass.ToArray();

            List<School_M_Section> allsection = new List<School_M_Section>();
            allsection = _db.Section.Where(y => y.MI_Id == DTO.MI_Id && y.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToList();
            attdto.SectionList = allsection.ToArray();


            //--------------student tc----------------

            attdto.admTransNumSetting = await _db.Master_Numbering.Where(t => t.MI_Id == attdto.MI_Id && t.IMN_Flag.Equals("tcno")).ToArrayAsync();

            //--------------------------------

            //getting payment type for tc
            attdto.tcpermanentpayment = await _db.AdmissionStandardDMO.Where(t => t.MI_Id == attdto.MI_Id).ToArrayAsync();

            attdto.classlistnew = _db.School_M_Class.Where(a => a.MI_Id == attdto.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();

            return attdto;
        }
        public StudentTCDTO getStatusDetails(StudentTCDTO status)
        {
            List<StudentTCDTO> ln = new List<StudentTCDTO>();
            StudentTCDTO attdto = new StudentTCDTO();
            return attdto;
        }

        //for saving the result

        //public StudentTCDTO saveTcdet1(StudentTCDTO tc_dto)
        //{
        //    try
        //    {
        //        StudentTC tc_dmo = Mapper.Map<StudentTC>(tc_dto);
        //        var Tc_Amst_Count = _db.Student_TC.Where(d => d.AMST_Id == tc_dto.AMST_Id).ToList();
        //        if (Tc_Amst_Count.Count > 0)
        //        {
        //            //    Console.WriteLine("Record already exists");
        //            //    tc_dto.returnval = false;
        //            _db.Student_TC.Update(tc_dmo);
        //            var flag = _db.SaveChanges();

        //            var update_flag1 = _db.SaveChanges();

        //            if (update_flag1 == 1)
        //            {
        //                tc_dto.returnval = true;
        //            }
        //            else
        //            {
        //                tc_dto.returnval = false;
        //            }
        //        }

        //        else
        //        {
        //            var studDet = _db.Adm_M_Student.Where(d => d.AMST_Id == tc_dto.AMST_Id).ToList();
        //            var section = _act.school_Adm_Y_StudentDMO.Where(d => d.AMST_Id == tc_dto.AMST_Id).ToList();

        //            if (section.Count == 0)
        //            {
        //                tc_dmo.ASMCL_Id = 0;
        //                tc_dmo.ASMS_Id = 0;
        //            }

        //            else
        //            {
        //                tc_dmo.ASMCL_Id = section.FirstOrDefault().ASMCL_Id;
        //                tc_dmo.ASMS_Id = section.FirstOrDefault().ASMS_Id;
        //            }


        //            tc_dmo.ASMAY_Id = studDet.FirstOrDefault().ASMAY_Id;
        //            tc_dmo.IMC_Id = studDet.FirstOrDefault().IC_Id;
        //            tc_dmo.MI_Id = studDet.FirstOrDefault().MI_Id;
        //            tc_dmo.CreatedDate = DateTime.Now;
        //            tc_dmo.UpdatedDate = DateTime.Now;


        //            //active student decative student 
        //            if (tc_dto.ASTC_ActiveFlag == "S" || tc_dto.ASTC_ActiveFlag == "D")
        //            {
        //                if (tc_dto.ASTC_TemporaryFlag == 1)
        //                {
        //                    tc_dmo.ASTC_ActiveFlag = "T";
        //                    tc_dmo.ASTC_TemporaryFlag = 1;
        //                    _db.Student_TC.Add(tc_dmo);
        //                }
        //                else
        //                {
        //                    tc_dmo.ASTC_ActiveFlag = "L";
        //                    tc_dmo.ASTC_TemporaryFlag = 0;
        //                    _db.Student_TC.Add(tc_dmo);
        //                }
        //            }
        //            else
        //            {

        //            }
        //            //  tc_dmo.ASTC_ActiveFlag = "L"; 

        //            var flag = _db.SaveChanges();

        //            var update_ADM = _db.Adm_M_Student.Single(d => d.AMST_Id == tc_dto.AMST_Id);
        //            var update_admy = _db.School_Adm_Y_StudentDMO.Single(d => d.AMST_Id == tc_dto.AMST_Id);

        //            if (update_ADM.AMST_SOL == "S" || update_ADM.AMST_SOL == "D")
        //            {
        //                update_ADM.AMST_SOL = "L";
        //                update_ADM.AMST_ActiveFlag = 0;
        //                update_admy.AMAY_ActiveFlag = 0;

        //                //update_ADM.UpdatedDate = DateTime.Now();
        //                _db.Adm_M_Student.Update(update_ADM);
        //                _db.School_Adm_Y_StudentDMO.Update(update_admy);

        //                var update_flag = _db.SaveChanges();

        //                if (update_flag == 1)
        //                {
        //                    tc_dto.returnval = true;
        //                }
        //                else
        //                {
        //                    tc_dto.returnval = false;
        //                }
        //            }

        //            if (flag == 1)
        //            {
        //                tc_dto.returnval = true;
        //            }
        //            else
        //            {
        //                tc_dto.returnval = false;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return tc_dto;
        //}



        //for getting the intial TC-details
        public StudentTCDTO gettcDetails(StudentTCDTO MIID)
        {

            StudentTCDTO studDto = new StudentTCDTO();

            string photoPath;
            string IFSC_Code_null;
            string BirthPlace_null;
            string BirthCertNO_null;
            string StuCasteCertiNo_null;
            string fathername_get;
            string BPLCardNo_null;

            string StuBankAccNo_null;

            string emailId_null;

            DateTime? ASMAYFromDate;

            //string Class_Studying_null;
            string Class_name_null;

            string section_name_null;

            string ASMAY_Year_null;
            string Mother_Tongue_null;
            string Student_adm_no_null_lbl;


            DateTime? ASTC_PromotionDate_null;
            string ASTC_MediumOfINStruction_null;
            string ASTC_LanguageStudied_null;
            string ASTC_ElectivesStudied_null;
            string ASTC_Scholarship_null;
            string ASTC_MedicallyExam_null;
            string ASTC_TCNO_null;
            DateTime? ASTC_TCApplicationDate_null;
            //DateTime? ASTC_TCIssueDate_null;
            string ASTC_LeavingReason_null;
            DateTime? ASTC_TCDate_null;
            DateTime? ASTC_LastAttendedDate_null;

            DateTime? AMST_DOB_NULL;
            long? ASTC_WorkingDays_null;
            long? ASTC_AttendedDays_null;
            string ASTC_FeePaid_null;
            string ASTC_FeeConcession_null;
            string ASTC_LastExamDetails_null;
            string ASTC_Result_null;
            string ASTC_ResultDetails_null;
            string ASTC_Qual_PromotionFlag_null;
            string ASTC_Qual_Class_null;
            string ASTC_NCCDetails_null;
            string ASTC_ExtraActivities_null;
            string ASTC_Conduct_null;
            string ASTC_Remarks_null;

            decimal? ftp_tobepaid_amt;

            int ASMCL_Order_null;
            int ASMC_Order_null;
            int feetotalpaid_null;
            string AMST_Nationality_Null;
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    List<StudentTCDTO> stud = new List<StudentTCDTO>();
                    cmd.CommandText = "StudentTcDetailsById";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (MIID.AMST_Id > 0 && MIID.Status_flag != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.BigInt) { Value = MIID.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = MIID.Status_flag });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MIID.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = MIID.ASMAY_Id });
                    }
                    if (MIID.Status_flag == "L" || MIID.Status_flag == "T")
                    {
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["AMST_Photoname"] != System.DBNull.Value)
                                    {
                                        photoPath = Convert.ToString(dataReader["AMST_Photoname"]);
                                    }
                                    else
                                    {
                                        photoPath = null;
                                    }
                                    #region
                                    if (dataReader["AMST_Date"] != System.DBNull.Value)
                                    {
                                        ASMAYFromDate = Convert.ToDateTime(dataReader["AMST_Date"]);
                                    }
                                    else
                                    {
                                        ASMAYFromDate = null;
                                    }

                                    if (dataReader["AMST_StuBankIFSC_Code"] != System.DBNull.Value)
                                    {
                                        IFSC_Code_null = Convert.ToString(dataReader["AMST_StuBankIFSC_Code"]);
                                    }
                                    else
                                    {
                                        IFSC_Code_null = "Not Available";
                                    }

                                    if (dataReader["AMST_BirthPlace"] != System.DBNull.Value)
                                    {
                                        BirthPlace_null = Convert.ToString(dataReader["AMST_BirthPlace"]);
                                    }
                                    else
                                    {
                                        BirthPlace_null = "Not Available";
                                    }
                                    if (dataReader["AMST_BirthCertNO"] != System.DBNull.Value)
                                    {
                                        BirthCertNO_null = Convert.ToString(dataReader["AMST_BirthCertNO"]);
                                    }
                                    else
                                    {
                                        BirthCertNO_null = "Not Available";
                                    }


                                    if (dataReader["AMST_StuCasteCertiNo"] != System.DBNull.Value)
                                    {
                                        StuCasteCertiNo_null = Convert.ToString(dataReader["AMST_StuCasteCertiNo"]);
                                    }
                                    else
                                    {
                                        StuCasteCertiNo_null = "Not Available";
                                    }

                                    if (dataReader["father"] != System.DBNull.Value)
                                    {
                                        fathername_get = Convert.ToString(dataReader["father"]);
                                    }
                                    else
                                    {
                                        fathername_get = "Not Available";
                                    }

                                    if (dataReader["AMST_emailId"] != System.DBNull.Value)
                                    {
                                        emailId_null = Convert.ToString(dataReader["AMST_emailId"]);
                                    }
                                    else
                                    {
                                        emailId_null = "Not Available";
                                    }

                                    if (dataReader["AMST_StuBankAccNo"] != System.DBNull.Value)
                                    {
                                        StuBankAccNo_null = Convert.ToString(dataReader["AMST_StuBankAccNo"]);
                                    }
                                    else
                                    {
                                        StuBankAccNo_null = "Not Available";
                                    }
                                    if (dataReader["AMST_BPLCardNo"] != System.DBNull.Value)
                                    {
                                        BPLCardNo_null = Convert.ToString(dataReader["AMST_BPLCardNo"]);
                                    }
                                    else
                                    {
                                        BPLCardNo_null = "Not Available";
                                    }

                                    if (dataReader["ASMCL_ClassName"] != System.DBNull.Value)
                                    {
                                        Class_name_null = Convert.ToString(dataReader["ASMCL_ClassName"]);
                                    }
                                    else
                                    {
                                        Class_name_null = "Not Available";
                                    }


                                    if (dataReader["ASMC_SectionName"] != System.DBNull.Value)
                                    {
                                        section_name_null = Convert.ToString(dataReader["ASMC_SectionName"]);
                                    }
                                    else
                                    {
                                        section_name_null = "Not Available";
                                    }


                                    //if (dataReader["ClassStudying"] != System.DBNull.Value)
                                    //{
                                    //    Class_Studying_null = Convert.ToString(dataReader["ClassStudying"]);
                                    //}
                                    //else
                                    //{
                                    //    Class_Studying_null = "Not Available";
                                    //}


                                    if (dataReader["ASMAY_Year"] != System.DBNull.Value)
                                    {
                                        ASMAY_Year_null = Convert.ToString(dataReader["ASMAY_Year"]);
                                    }
                                    else
                                    {
                                        ASMAY_Year_null = "Not Available";
                                    }

                                    if (dataReader["AMST_MotherTongue"] != System.DBNull.Value)
                                    {
                                        Mother_Tongue_null = Convert.ToString(dataReader["AMST_MotherTongue"]);
                                    }
                                    else
                                    {
                                        Mother_Tongue_null = "Not Available";
                                    }

                                    if (dataReader["student"] != System.DBNull.Value)
                                    {
                                        Student_first_name_null = Convert.ToString(dataReader["student"]);
                                    }
                                    else
                                    {
                                        Student_first_name_null = null;
                                    }

                                    //if (dataReader["AMST_MiddleName"] != System.DBNull.Value)
                                    //{
                                    //    Student_middle_name_null = Convert.ToString(dataReader["AMST_MiddleName"]);
                                    //}
                                    //else
                                    //{
                                    //    Student_middle_name_null = null;
                                    //}

                                    //if (dataReader["AMST_LastName"] != System.DBNull.Value)
                                    //{
                                    //    Student_last_name_null = Convert.ToString(dataReader["AMST_LastName"]);
                                    //}
                                    //else
                                    //{
                                    //    Student_last_name_null = null;
                                    //}
                                    if (dataReader["AMST_RegistrationNo"] != System.DBNull.Value)
                                    {
                                        Student_reg_no_null = Convert.ToString(dataReader["AMST_RegistrationNo"]);
                                    }
                                    else
                                    {
                                        Student_reg_no_null = "Not Available";
                                    }

                                    if (dataReader["admno"] != System.DBNull.Value)
                                    {
                                        Student_adm_no_null = Convert.ToString(dataReader["admno"]);
                                    }
                                    else
                                    {
                                        Student_adm_no_null = null;
                                    }


                                    if (dataReader["admno"] != System.DBNull.Value)
                                    {
                                        Student_adm_no_null_lbl = Convert.ToString(dataReader["admno"]);
                                    }
                                    else
                                    {
                                        Student_adm_no_null_lbl = "Not Available";
                                    }
                                    if (dataReader["AMST_DOB_Words"] != System.DBNull.Value)
                                    {
                                        Student_dob_words_null = Convert.ToString(dataReader["AMST_DOB_Words"]);
                                    }
                                    else
                                    {
                                        Student_dob_words_null = "Not Available";
                                    }
                                    if (dataReader["IMC_CasteName"] != System.DBNull.Value)
                                    {
                                        Student_IC_Caste_name_null = Convert.ToString(dataReader["IMC_CasteName"]);
                                    }
                                    else
                                    {
                                        Student_IC_Caste_name_null = "Not Available";
                                    }

                                    if (dataReader["AMST_MobileNo"] != System.DBNull.Value)
                                    {
                                        Student_Mobile_no__null = Convert.ToInt64(dataReader["AMST_MobileNo"]);
                                    }
                                    else
                                    {
                                        Student_Mobile_no__null = 0;
                                    }
                                    if (dataReader["AMC_Name"] != System.DBNull.Value)
                                    {
                                        Student_Amc_name_null = Convert.ToString(dataReader["AMC_Name"]);
                                    }
                                    else
                                    {
                                        Student_Amc_name_null = "Not Available";
                                    }


                                    if (dataReader["PASR_Age"] != System.DBNull.Value)
                                    {
                                        Student__age__null = Convert.ToInt32(dataReader["PASR_Age"]);
                                    }
                                    else
                                    {
                                        Student__age__null = 0;
                                    }
                                    if (dataReader["AMST_Sex"] != System.DBNull.Value)
                                    {
                                        Student_Gender_null = Convert.ToString(dataReader["AMST_Sex"]);
                                    }
                                    else
                                    {
                                        Student_Gender_null = "Not Available";
                                    }

                                    if (dataReader["AMST_AadharNo"] != System.DBNull.Value)
                                    {
                                        Student_aadhar_no_null = Convert.ToInt64(dataReader["AMST_AadharNo"]);
                                    }
                                    else
                                    {
                                        Student_aadhar_no_null = 0;
                                    }

                                    if (dataReader["AMST_BloodGroup"] != System.DBNull.Value)
                                    {
                                        Student_Blood_Group_null = Convert.ToString(dataReader["AMST_BloodGroup"]);
                                    }
                                    else
                                    {
                                        Student_Blood_Group_null = "Not Available";
                                    }

                                    if (dataReader["IMC_Id"] != System.DBNull.Value)
                                    {
                                        Student_IC_Id_null = Convert.ToInt32(dataReader["IMC_Id"]);
                                    }
                                    else
                                    {
                                        Student_IC_Id_null = 0;
                                    }

                                    if (dataReader["ASTC_PromotionDate"] != System.DBNull.Value)
                                    {
                                        ASTC_PromotionDate_null = Convert.ToDateTime(dataReader["ASTC_PromotionDate"]);
                                    }
                                    else
                                    {
                                        ASTC_PromotionDate_null = null;
                                    }
                                    if (dataReader["ASTC_MediumOfINStruction"] != System.DBNull.Value)
                                    {
                                        ASTC_MediumOfINStruction_null = Convert.ToString(dataReader["ASTC_MediumOfINStruction"]);
                                    }
                                    else
                                    {
                                        ASTC_MediumOfINStruction_null = null;
                                    }

                                    if (dataReader["ASTC_LanguageStudied"] != System.DBNull.Value)
                                    {
                                        ASTC_LanguageStudied_null = Convert.ToString(dataReader["ASTC_LanguageStudied"]);
                                    }
                                    else
                                    {
                                        ASTC_LanguageStudied_null = null;
                                    }

                                    if (dataReader["ASTC_ElectivesStudied"] != System.DBNull.Value)
                                    {
                                        ASTC_ElectivesStudied_null = Convert.ToString(dataReader["ASTC_ElectivesStudied"]);
                                    }
                                    else
                                    {
                                        ASTC_ElectivesStudied_null = null;
                                    }
                                    if (dataReader["ASTC_Scholarship"] != System.DBNull.Value)
                                    {
                                        ASTC_Scholarship_null = Convert.ToString(dataReader["ASTC_Scholarship"]);
                                    }
                                    else
                                    {
                                        ASTC_Scholarship_null = null;
                                    }
                                    if (dataReader["ASTC_MedicallyExam"] != System.DBNull.Value)
                                    {
                                        ASTC_MedicallyExam_null = Convert.ToString(dataReader["ASTC_MedicallyExam"]);
                                    }
                                    else
                                    {
                                        ASTC_MedicallyExam_null = null;
                                    }
                                    if (dataReader["ASTC_TCNO"] != System.DBNull.Value)
                                    {
                                        ASTC_TCNO_null = Convert.ToString(dataReader["ASTC_TCNO"]);
                                    }
                                    else
                                    {
                                        ASTC_TCNO_null = null;
                                    }

                                    if (dataReader["ASTC_TCApplicationDate"] != System.DBNull.Value)
                                    {
                                        ASTC_TCApplicationDate_null = Convert.ToDateTime(dataReader["ASTC_TCApplicationDate"]);
                                    }
                                    else
                                    {
                                        ASTC_TCApplicationDate_null = null;
                                    }
                                    //if (dataReader["ASTC_TCIssueDate"] != System.DBNull.Value)
                                    //{
                                    //    ASTC_TCIssueDate_null = Convert.ToDateTime(dataReader["ASTC_TCIssueDate"]);
                                    //}
                                    //else
                                    //{
                                    //    ASTC_TCIssueDate_null = null;
                                    //}

                                    if (dataReader["ASTC_LeavingReason"] != System.DBNull.Value)
                                    {
                                        ASTC_LeavingReason_null = Convert.ToString(dataReader["ASTC_LeavingReason"]);
                                    }
                                    else
                                    {
                                        ASTC_LeavingReason_null = null;
                                    }
                                    if (dataReader["ASTC_TCDate"] != System.DBNull.Value)
                                    {
                                        ASTC_TCDate_null = Convert.ToDateTime(dataReader["ASTC_TCDate"]);
                                    }
                                    else
                                    {
                                        ASTC_TCDate_null = null;
                                    }
                                    if (dataReader["ASTC_LastAttendedDate"] != System.DBNull.Value)
                                    {
                                        ASTC_LastAttendedDate_null = Convert.ToDateTime(dataReader["ASTC_LastAttendedDate"]);
                                    }
                                    else
                                    {
                                        ASTC_LastAttendedDate_null = null;
                                    }


                                    if (dataReader["ASTC_WorkingDays"] != System.DBNull.Value)
                                    {
                                        ASTC_WorkingDays_null = Convert.ToInt64(dataReader["ASTC_WorkingDays"]);
                                    }
                                    else
                                    {
                                        ASTC_WorkingDays_null = null;
                                    }
                                    if (dataReader["ASTC_AttendedDays"] != System.DBNull.Value)
                                    {
                                        ASTC_AttendedDays_null = Convert.ToInt64(dataReader["ASTC_AttendedDays"]);
                                    }
                                    else
                                    {
                                        ASTC_AttendedDays_null = null;
                                    }
                                    if (dataReader["ASTC_FeePaid"] != System.DBNull.Value)
                                    {
                                        ASTC_FeePaid_null = Convert.ToString(dataReader["ASTC_FeePaid"]);
                                    }
                                    else
                                    {
                                        ASTC_FeePaid_null = null;
                                    }
                                    if (dataReader["ASTC_FeeConcession"] != System.DBNull.Value)
                                    {
                                        ASTC_FeeConcession_null = Convert.ToString(dataReader["ASTC_FeeConcession"]);
                                    }
                                    else
                                    {
                                        ASTC_FeeConcession_null = null;
                                    }


                                    if (dataReader["ASTC_LastExamDetails"] != System.DBNull.Value)
                                    {
                                        ASTC_LastExamDetails_null = Convert.ToString(dataReader["ASTC_LastExamDetails"]);
                                    }
                                    else
                                    {
                                        ASTC_LastExamDetails_null = null;
                                    }

                                    if (dataReader["ASTC_Result"] != System.DBNull.Value)
                                    {
                                        ASTC_Result_null = Convert.ToString(dataReader["ASTC_Result"]);
                                    }
                                    else
                                    {
                                        ASTC_Result_null = null;
                                    }

                                    if (dataReader["ASTC_ResultDetails"] != System.DBNull.Value)
                                    {
                                        ASTC_ResultDetails_null = Convert.ToString(dataReader["ASTC_ResultDetails"]);
                                    }
                                    else
                                    {
                                        ASTC_ResultDetails_null = null;
                                    }

                                    if (dataReader["ASTC_Qual_PromotionFlag"] != System.DBNull.Value)
                                    {
                                        ASTC_Qual_PromotionFlag_null = Convert.ToString(dataReader["ASTC_Qual_PromotionFlag"]);
                                    }
                                    else
                                    {
                                        ASTC_Qual_PromotionFlag_null = null;
                                    }
                                    if (dataReader["ASTC_Qual_Class"] != System.DBNull.Value)
                                    {
                                        ASTC_Qual_Class_null = Convert.ToString(dataReader["ASTC_Qual_Class"]);
                                    }
                                    else
                                    {
                                        ASTC_Qual_Class_null = null;
                                    }
                                    if (dataReader["ASTC_NCCDetails"] != System.DBNull.Value)
                                    {
                                        ASTC_NCCDetails_null = Convert.ToString(dataReader["ASTC_NCCDetails"]);
                                    }
                                    else
                                    {
                                        ASTC_NCCDetails_null = null;
                                    }


                                    if (dataReader["ASTC_ExtraActivities"] != System.DBNull.Value)
                                    {
                                        ASTC_ExtraActivities_null = Convert.ToString(dataReader["ASTC_ExtraActivities"]);
                                    }
                                    else
                                    {
                                        ASTC_ExtraActivities_null = null;
                                    }

                                    if (dataReader["ASTC_Conduct"] != System.DBNull.Value)
                                    {
                                        ASTC_Conduct_null = Convert.ToString(dataReader["ASTC_Conduct"]);
                                    }
                                    else
                                    {
                                        ASTC_Conduct_null = null;
                                    }


                                    if (dataReader["ASTC_Remarks"] != System.DBNull.Value)
                                    {
                                        ASTC_Remarks_null = Convert.ToString(dataReader["ASTC_Remarks"]);
                                    }
                                    else
                                    {
                                        ASTC_Remarks_null = null;
                                    }


                                    #endregion

                                    if (dataReader["ASMCL_Order"] != System.DBNull.Value)
                                    {
                                        ASMCL_Order_null = Convert.ToInt32(dataReader["ASMCL_Order"]);
                                    }
                                    else
                                    {
                                        ASMCL_Order_null = 0;
                                    }
                                    if (dataReader["ASMC_Order"] != System.DBNull.Value)
                                    {
                                        ASMC_Order_null = Convert.ToInt32(dataReader["ASMC_Order"]);
                                    }
                                    else
                                    {
                                        ASMC_Order_null = 0;
                                    }
                                    if (dataReader["FSS_ToBePaid"] != System.DBNull.Value)
                                    {
                                        feetotalpaid_null = Convert.ToInt32(dataReader["FSS_ToBePaid"]);
                                    }
                                    else
                                    {
                                        feetotalpaid_null = 0;
                                    }
                                    if (dataReader["AMST_DOB"] != System.DBNull.Value)
                                    {
                                        AMST_DOB_NULL = Convert.ToDateTime(dataReader["AMST_DOB"]);
                                    }
                                    else
                                    {
                                        AMST_DOB_NULL = null;
                                    }
                                    if (dataReader["AMST_Nationality"] != System.DBNull.Value)
                                    {
                                        AMST_Nationality_Null = dataReader["AMST_Nationality"].ToString();
                                    }
                                    else
                                    {
                                        AMST_Nationality_Null = "";
                                    }
                                    if (MIID.adm_num_flag == "A")
                                    {
                                        Student_Name_Var = Student_adm_no_null + ':' + Student_first_name_null;
                                    }
                                    else if (MIID.adm_num_flag == "N")
                                    {
                                        Student_Name_Var = Student_first_name_null + ':' + Student_adm_no_null;
                                    }
                                    if (dataReader["stsno"] != System.DBNull.Value)
                                    {
                                        stsno = dataReader["stsno"].ToString();
                                    }
                                    else
                                    {
                                        stsno = "";
                                    }
                                    stud.Add(new StudentTCDTO
                                    {
                                        AMST_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                        StudentName = Student_Name_Var,
                                        //ClassStudying = Class_Studying_null,
                                        ASMCL_ClassName = Class_name_null,
                                        ASMC_SectionName = section_name_null,
                                        RegistrationNo = Student_reg_no_null,
                                        // AMST_RegistrationNo = Convert.ToInt64(dataReader["AMST_RegistrationNo"].ToString()),
                                        AdmNo = Student_adm_no_null_lbl,
                                        ASMAY_Year = ASMAY_Year_null,
                                        AMC_Name = Student_Amc_name_null,
                                        AMST_DOB = AMST_DOB_NULL,
                                        AMST_DOB_Words = Student_dob_words_null,
                                        AMST_StuBankIFSC_Code = IFSC_Code_null,
                                        AMST_BirthPlace = BirthPlace_null,
                                        AMST_BirthCertNO = BirthCertNO_null,
                                        AMST_StuCasteCertiNo = StuCasteCertiNo_null,
                                        PASR_Age = Student__age__null,
                                        AMST_Sex = Student_Gender_null,
                                        AMST_MotherTongue = Mother_Tongue_null,
                                        AMST_Nationality = AMST_Nationality_Null,
                                        AMST_MobileNo = Student_Mobile_no__null,
                                        AMST_emailId = emailId_null,
                                        AMST_AadharNo = Student_aadhar_no_null,
                                        AMST_BloodGroup = Student_Blood_Group_null,
                                        AMST_StuBankAccNo = StuBankAccNo_null,
                                        AMST_BPLCardNo = BPLCardNo_null,
                                        AMST_Date = ASMAYFromDate,
                                        StudPhoto = photoPath,
                                        //ASMAY_From_Date = ASMAYFromDate,
                                        class_order = ASMCL_Order_null - 1,
                                        section_order = ASMC_Order_null,
                                        IC_CasteName = Student_IC_Caste_name_null,
                                        IMC_Id = Student_IC_Id_null,
                                        ASTC_PromotionDate = ASTC_PromotionDate_null,
                                        ASTC_MediumOfINStruction = ASTC_MediumOfINStruction_null,
                                        ASTC_LanguageStudied = ASTC_LanguageStudied_null,
                                        ASTC_ElectivesStudied = ASTC_ElectivesStudied_null,
                                        ASTC_Scholarship = ASTC_Scholarship_null,
                                        ASTC_MedicallyExam = ASTC_MedicallyExam_null,
                                        ASTC_TCNO = ASTC_TCNO_null,
                                        ASTC_TCApplicationDate = ASTC_TCApplicationDate_null,
                                        // ASTC_TCIssueDate = ASTC_TCIssueDate_null,
                                        ASTC_LeavingReason = ASTC_LeavingReason_null,
                                        ASTC_TCDate = ASTC_TCDate_null,
                                        ASTC_LastAttendedDate = ASTC_LastAttendedDate_null,
                                        ASTC_WorkingDays = ASTC_WorkingDays_null,
                                        ASTC_AttendedDays = ASTC_AttendedDays_null,
                                        ASTC_FeePaid = ASTC_FeePaid_null,
                                        ASTC_FeeConcession = ASTC_FeeConcession_null,
                                        ASTC_LastExamDetails = ASTC_LastExamDetails_null,
                                        ASTC_Result = ASTC_Result_null,
                                        ASTC_ResultDetails = ASTC_ResultDetails_null,
                                        ASTC_Qual_PromotionFlag = ASTC_Qual_PromotionFlag_null,
                                        ASTC_Qual_Class = ASTC_Qual_Class_null,
                                        ASTC_NCCDetails = ASTC_NCCDetails_null,
                                        ASTC_ExtraActivities = ASTC_ExtraActivities_null,
                                        ASTC_Conduct = ASTC_Conduct_null,
                                        ASTC_Remarks = ASTC_Remarks_null,
                                        feetobepaid = feetotalpaid_null,
                                        fathername = fathername_get,
                                        stsno = stsno

                                        //ClassStudied=_db.School_M_Class.Where(d=>d.ASMCL_Order == Convert.ToInt32(dataReader["ASMCL_Order"]) - 1).Select(s=>s.ASMCL_ClassName).ToList(),
                                    });
                                    studDto.StudPhoto = stud.FirstOrDefault().StudPhoto;
                                    studDto.studentListById = stud.Distinct().GroupBy(x => x.RegistrationNo).Select(x => x.First()).ToArray();
                                    // studDto.studentListById = stud.Select(t => t.StudentName).Distinct().ToArray();
                                }
                            }
                            if (MIID.AMST_Id > 0)
                            {
                                if (stud.FirstOrDefault().class_order > 0)
                                {
                                    int classorder = stud.Select(x => x.class_order).Single();
                                    var getclassName = _db.School_M_Class.Where(d => d.ASMCL_Order == classorder && d.MI_Id == MIID.MI_Id).Select(x => x.ASMCL_ClassName).FirstOrDefault();
                                    var Pre_Class_Id = _db.School_M_Class.Where(d => d.ASMCL_Order == classorder && d.MI_Id == MIID.MI_Id).Select(x => x.ASMCL_Id).FirstOrDefault();
                                    studDto.ClassStudied = getclassName;
                                    studDto.Pre_Class_Id = Pre_Class_Id;
                                }
                                if (stud.FirstOrDefault().section_order > 0)
                                {
                                    int sectionorder = stud.Select(x => x.section_order).Single();
                                    var getsectionName = _db.School_M_Section.Where(d => d.ASMC_Order == sectionorder && d.MI_Id == MIID.MI_Id).Select(x => x.ASMC_SectionName).FirstOrDefault();
                                    var Section_Id = _db.School_M_Section.Where(d => d.ASMC_Order == sectionorder && d.MI_Id == MIID.MI_Id).Select(x => x.ASMS_Id).FirstOrDefault();
                                    studDto.previous_section = getsectionName;
                                    studDto.Section_Id = Section_Id;
                                }
                                var Tc_Amst_Count = _db.Student_TC.Where(d => d.AMST_Id == MIID.AMST_Id).ToList();
                                try
                                {
                                    if (Tc_Amst_Count.Count > 0)
                                    {
                                        Console.WriteLine("Record already exists");
                                        studDto.returnval = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                        try
                        {
                            //sp for attendance
                            List<StudentTCDTO> result = new List<StudentTCDTO>();
                            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd1.CommandText = "Adm_Get_Toalclassheld_Attendance_Student_TC";
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MIID.MI_Id) });
                                cmd1.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = Convert.ToString(MIID.ASMAY_Id) });
                                cmd1.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.VarChar) { Value = Convert.ToString(MIID.AMST_Id) });
                                cmd1.Parameters.Add(new SqlParameter("@AMST_SOL", SqlDbType.VarChar) { Value = MIID.Status_flag });
                                if (cmd1.Connection.State != ConnectionState.Open)
                                    cmd1.Connection.Open();

                                var retObject1 = new List<dynamic>();
                                try
                                {
                                    using (var dataReader1 = cmd1.ExecuteReader())
                                    {
                                        while (dataReader1.Read())
                                        {
                                            result.Add(new StudentTCDTO
                                            {
                                                countclass = Convert.ToInt32(dataReader1["Classes"]),
                                                attclasscount = Convert.ToInt32(dataReader1["Attendance"])
                                            });

                                        }
                                    }
                                    if (result.Count > 0)
                                    {
                                        studDto.countclass = result.FirstOrDefault().countclass;
                                        studDto.attclasscount = result.FirstOrDefault().attclasscount;
                                    }
                                    else
                                    {
                                        //studDto.message = "Please Enter The Number Of Class Held For Particular Month In Master Class Held";
                                    }

                                    var dataatt = (from a in _db.Adm_studentAttendance
                                                   from b in _db.Adm_studentAttendanceStudents
                                                   where a.ASA_Id == b.ASA_Id && a.MI_Id == MIID.MI_Id && b.AMST_Id == MIID.AMST_Id
                                                   select new StudentTCDTO
                                                   {
                                                       todateatt = a.ASA_ToDate
                                                   }).Distinct().ToList();

                                    studDto.todateatt = dataatt.Max(x => x.todateatt);

                                }
                                catch (Exception ex)
                                {
                                    _tcimpl.LogInformation("TC attendance count sp error :'" + ex.Message + "'");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _tcimpl.LogInformation("TC attendance count: '" + ex.Message + "'");
                        }

                        try
                        {
                            studDto.get_elective_subjects = (from a in _db.StudentMappingDMO
                                                             from b in _db.AcademicYear
                                                             from c in _db.Adm_M_Student
                                                             from d in _db.SchoolYearWiseStudent
                                                             from e in _db.MasterSubjectList
                                                             where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id && a.MI_Id == MIID.MI_Id && a.ASMAY_Id == MIID.ASMAY_Id && a.AMST_Id == MIID.AMST_Id && a.ESTSU_ElecetiveFlag == true && e.ISMS_LanguageFlg == 0)
                                                             select new StudentTCDTO
                                                             {
                                                                 subjectname = e.ISMS_SubjectName
                                                             }).Distinct().ToArray();

                            studDto.get_elective_subjects_language = (from a in _db.StudentMappingDMO
                                                                      from b in _db.AcademicYear
                                                                      from c in _db.Adm_M_Student
                                                                      from d in _db.SchoolYearWiseStudent
                                                                      from e in _db.MasterSubjectList
                                                                      where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id && a.MI_Id == MIID.MI_Id && a.ASMAY_Id == MIID.ASMAY_Id && a.AMST_Id == MIID.AMST_Id && a.ESTSU_ElecetiveFlag == true && e.ISMS_LanguageFlg == 1)
                                                                      select new StudentTCDTO
                                                                      {
                                                                          subjectname = e.ISMS_SubjectName
                                                                      }).Distinct().ToArray();
                        }
                        catch (Exception ex)
                        {
                            _tcimpl.LogInformation("TC Exam count: '" + ex.Message + "'");
                        }



                        return studDto;
                    }

                    else
                    {
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["AMST_Photoname"] != System.DBNull.Value && dataReader["AMST_Photoname"] != null)
                                    {
                                        photoPath = Convert.ToString(dataReader["AMST_Photoname"]);
                                    }
                                    else
                                    {
                                        photoPath = null;
                                    }
                                    if (dataReader["AMST_Date"] != System.DBNull.Value)
                                    {
                                        ASMAYFromDate = Convert.ToDateTime(dataReader["AMST_Date"]);
                                    }
                                    else
                                    {
                                        ASMAYFromDate = null;
                                    }

                                    if (dataReader["AMST_StuBankIFSC_Code"] != System.DBNull.Value)
                                    {
                                        IFSC_Code_null = Convert.ToString(dataReader["AMST_StuBankIFSC_Code"]);
                                    }
                                    else
                                    {
                                        IFSC_Code_null = "Not Available";
                                    }
                                    if (dataReader["AMST_BirthPlace"] != System.DBNull.Value)
                                    {
                                        BirthPlace_null = Convert.ToString(dataReader["AMST_BirthPlace"]);
                                    }
                                    else
                                    {
                                        BirthPlace_null = "Not Available";
                                    }
                                    if (dataReader["AMST_BirthCertNO"] != System.DBNull.Value)
                                    {
                                        BirthCertNO_null = Convert.ToString(dataReader["AMST_BirthCertNO"]);
                                    }
                                    else
                                    {
                                        BirthCertNO_null = "Not Available";
                                    }
                                    if (dataReader["AMST_StuCasteCertiNo"] != System.DBNull.Value)
                                    {
                                        StuCasteCertiNo_null = Convert.ToString(dataReader["AMST_StuCasteCertiNo"]);
                                    }
                                    else
                                    {
                                        StuCasteCertiNo_null = "Not Available";
                                    }
                                    if (dataReader["AMST_emailId"] != System.DBNull.Value)
                                    {
                                        emailId_null = Convert.ToString(dataReader["AMST_emailId"]);
                                    }
                                    else
                                    {
                                        emailId_null = "Not Available";
                                    }

                                    if (dataReader["AMST_StuBankAccNo"] != System.DBNull.Value)
                                    {
                                        StuBankAccNo_null = Convert.ToString(dataReader["AMST_StuBankAccNo"]);
                                    }
                                    else
                                    {
                                        StuBankAccNo_null = "Not Available";
                                    }
                                    if (dataReader["AMST_BPLCardNo"] != System.DBNull.Value)
                                    {
                                        BPLCardNo_null = Convert.ToString(dataReader["AMST_BPLCardNo"]);
                                    }
                                    else
                                    {
                                        BPLCardNo_null = "Not Available";
                                    }

                                    if (dataReader["ASMCL_ClassName"] != System.DBNull.Value)
                                    {
                                        Class_name_null = Convert.ToString(dataReader["ASMCL_ClassName"]);
                                    }
                                    else
                                    {
                                        Class_name_null = "Not Available";
                                    }

                                    if (dataReader["ASMC_SectionName"] != System.DBNull.Value)
                                    {
                                        section_name_null = Convert.ToString(dataReader["ASMC_SectionName"]);
                                    }
                                    else
                                    {
                                        section_name_null = "Not Available";
                                    }
                                    if (dataReader["ASMAY_Year"] != System.DBNull.Value)
                                    {
                                        ASMAY_Year_null = Convert.ToString(dataReader["ASMAY_Year"]);
                                    }
                                    else
                                    {
                                        ASMAY_Year_null = "Not Available";
                                    }

                                    if (dataReader["AMST_MotherTongue"] != System.DBNull.Value)
                                    {
                                        Mother_Tongue_null = Convert.ToString(dataReader["AMST_MotherTongue"]);
                                    }
                                    else
                                    {
                                        Mother_Tongue_null = "Not Available";
                                    }
                                    if (dataReader["ASMCL_Order"] != System.DBNull.Value)
                                    {
                                        ASMCL_Order_null = Convert.ToInt32(dataReader["ASMCL_Order"]);
                                    }
                                    else
                                    {
                                        ASMCL_Order_null = 1;
                                    }
                                    if (dataReader["ASMC_Order"] != System.DBNull.Value)
                                    {
                                        ASMC_Order_null = Convert.ToInt32(dataReader["ASMC_Order"]);
                                    }
                                    else
                                    {
                                        ASMC_Order_null = 0;
                                    }

                                    if (dataReader["student"] != System.DBNull.Value)
                                    {
                                        Student_first_name_null = Convert.ToString(dataReader["student"]);
                                    }
                                    else
                                    {
                                        Student_first_name_null = null;
                                    }
                                    if (dataReader["father"] != System.DBNull.Value)
                                    {
                                        fathername_get = Convert.ToString(dataReader["father"]);
                                    }
                                    else
                                    {
                                        fathername_get = "Not Available";
                                    }
                                    //if (dataReader["AMST_MiddleName"] != System.DBNull.Value)
                                    //{
                                    //    Student_middle_name_null = Convert.ToString(dataReader["AMST_MiddleName"]);
                                    //}
                                    //else
                                    //{
                                    //    Student_middle_name_null = null;
                                    //}

                                    //if (dataReader["AMST_LastName"] != System.DBNull.Value)
                                    //{
                                    //    Student_last_name_null = Convert.ToString(dataReader["AMST_LastName"]);
                                    //}
                                    //else
                                    //{
                                    //    Student_last_name_null = null;
                                    //}
                                    if (dataReader["AMST_RegistrationNo"] != System.DBNull.Value)
                                    {
                                        Student_reg_no_null = Convert.ToString(dataReader["AMST_RegistrationNo"]);
                                    }
                                    else
                                    {
                                        Student_reg_no_null = "Not Available";
                                    }

                                    if (dataReader["admno"] != System.DBNull.Value)
                                    {
                                        Student_adm_no_null = Convert.ToString(dataReader["admno"]);
                                    }
                                    else
                                    {
                                        Student_adm_no_null = null;
                                    }
                                    if (dataReader["admno"] != System.DBNull.Value)
                                    {
                                        Student_adm_no_null_lbl = Convert.ToString(dataReader["admno"]);
                                    }
                                    else
                                    {
                                        Student_adm_no_null_lbl = "Not Available";
                                    }
                                    if (dataReader["AMST_DOB_Words"] != System.DBNull.Value)
                                    {
                                        Student_dob_words_null = Convert.ToString(dataReader["AMST_DOB_Words"]);
                                    }
                                    else
                                    {
                                        Student_dob_words_null = "Not Available";
                                    }
                                    if (dataReader["IMC_CasteName"] != System.DBNull.Value)
                                    {
                                        Student_IC_Caste_name_null = Convert.ToString(dataReader["IMC_CasteName"]);
                                    }
                                    else
                                    {
                                        Student_IC_Caste_name_null = "Not Available";
                                    }
                                    if (dataReader["AMST_MobileNo"] != System.DBNull.Value)
                                    {
                                        Student_Mobile_no__null = Convert.ToInt64(dataReader["AMST_MobileNo"]);
                                    }
                                    else
                                    {
                                        Student_Mobile_no__null = 0;
                                    }
                                    if (dataReader["AMC_Name"] != System.DBNull.Value)
                                    {
                                        Student_Amc_name_null = Convert.ToString(dataReader["AMC_Name"]);
                                    }
                                    else
                                    {
                                        Student_Amc_name_null = "Not Available";
                                    }
                                    if (dataReader["PASR_Age"] != System.DBNull.Value)
                                    {
                                        Student__age__null = Convert.ToInt32(dataReader["PASR_Age"]);
                                    }
                                    else
                                    {
                                        Student__age__null = 0;
                                    }
                                    if (dataReader["AMST_Sex"] != System.DBNull.Value)
                                    {
                                        Student_Gender_null = Convert.ToString(dataReader["AMST_Sex"]);
                                    }
                                    else
                                    {
                                        Student_Gender_null = "Not Available";
                                    }

                                    if (dataReader["AMST_AadharNo"] != System.DBNull.Value)
                                    {
                                        Student_aadhar_no_null = Convert.ToInt64(dataReader["AMST_AadharNo"]);
                                    }
                                    else
                                    {
                                        Student_aadhar_no_null = 0;
                                    }
                                    if (dataReader["AMST_BloodGroup"] != System.DBNull.Value)
                                    {
                                        Student_Blood_Group_null = Convert.ToString(dataReader["AMST_BloodGroup"]);
                                    }
                                    else
                                    {
                                        Student_Blood_Group_null = "Not Available";
                                    }

                                    if (dataReader["IMC_Id"] != System.DBNull.Value)
                                    {
                                        Student_IC_Id_null = Convert.ToInt32(dataReader["IMC_Id"]);
                                    }
                                    else
                                    {
                                        Student_IC_Id_null = 0;
                                    }

                                    if (dataReader["FSS_ToBePaid"] != System.DBNull.Value)
                                    {
                                        feetotalpaid_null = Convert.ToInt32(dataReader["FSS_ToBePaid"]);
                                    }
                                    else
                                    {
                                        feetotalpaid_null = 0;
                                    }

                                    if (dataReader["AMST_DOB"] != System.DBNull.Value)
                                    {
                                        AMST_DOB_NULL = Convert.ToDateTime(dataReader["AMST_DOB"]);
                                    }
                                    else
                                    {
                                        AMST_DOB_NULL = null;
                                    }
                                    if (dataReader["AMST_Nationality"] != System.DBNull.Value)
                                    {
                                        AMST_Nationality_Null = dataReader["AMST_Nationality"].ToString(); ;
                                    }
                                    else
                                    {
                                        AMST_Nationality_Null = "";
                                    }
                                    if (MIID.adm_num_flag == "A")
                                    {
                                        Student_Name_Var = Student_adm_no_null + ':' + Student_first_name_null;
                                    }
                                    else if (MIID.adm_num_flag == "N")
                                    {
                                        Student_Name_Var = Student_first_name_null + ':' + Student_adm_no_null;
                                    }
                                    if (dataReader["stsno"] != System.DBNull.Value)
                                    {
                                        stsno = dataReader["stsno"].ToString();
                                    }
                                    else
                                    {
                                        stsno = "";
                                    }
                                    stud.Add(new StudentTCDTO
                                    {
                                        AMST_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                        StudentName = Student_Name_Var,
                                        ASMCL_ClassName = Class_name_null,
                                        ASMC_SectionName = section_name_null,
                                        RegistrationNo = Student_reg_no_null,
                                        AdmNo = Student_adm_no_null_lbl,
                                        ASMAY_Year = ASMAY_Year_null,
                                        AMC_Name = Student_Amc_name_null,
                                        AMST_DOB = AMST_DOB_NULL,
                                        AMST_DOB_Words = Student_dob_words_null,
                                        AMST_StuBankIFSC_Code = IFSC_Code_null,
                                        AMST_BirthPlace = BirthPlace_null,
                                        AMST_BirthCertNO = BirthCertNO_null,
                                        AMST_StuCasteCertiNo = StuCasteCertiNo_null,
                                        PASR_Age = Student__age__null,
                                        AMST_Sex = Student_Gender_null,
                                        AMST_MotherTongue = Mother_Tongue_null,
                                        AMST_Nationality = AMST_Nationality_Null,
                                        AMST_MobileNo = Student_Mobile_no__null,
                                        AMST_emailId = emailId_null,
                                        AMST_AadharNo = Student_aadhar_no_null,
                                        AMST_BloodGroup = Student_Blood_Group_null,
                                        AMST_StuBankAccNo = StuBankAccNo_null,
                                        AMST_BPLCardNo = BPLCardNo_null,
                                        AMST_Date = ASMAYFromDate,
                                        StudPhoto = photoPath,
                                        class_order = ASMCL_Order_null - 1,
                                        section_order = ASMC_Order_null,
                                        IC_CasteName = Student_IC_Caste_name_null,
                                        IMC_Id = Student_IC_Id_null,
                                        feetobepaid = feetotalpaid_null,
                                        fathername = fathername_get,
                                        stsno = stsno
                                        //ClassStudied=_db.School_M_Class.Where(d=>d.ASMCL_Order == Convert.ToInt32(dataReader["ASMCL_Order"]) - 1).Select(s=>s.ASMCL_ClassName).ToList(),
                                    });

                                    studDto.StudPhoto = stud.FirstOrDefault().StudPhoto;
                                    studDto.studentListById = stud.Distinct().GroupBy(x => x.RegistrationNo).Select(x => x.First()).ToArray();
                                    // studDto.studentListById = stud.Select(t => t.StudentName).Distinct().ToArray();
                                }
                                if (!dataReader.HasRows)
                                {
                                    studDto.studentListById = null;

                                }
                            }
                            if (MIID.AMST_Id > 0)
                            {
                                if (stud.FirstOrDefault().class_order > 0)
                                {
                                    int classorder = stud.Select(x => x.class_order).Single();
                                    var getclassName = _db.School_M_Class.Where(d => d.ASMCL_Order == classorder && d.MI_Id == MIID.MI_Id).Select(x => x.ASMCL_ClassName).Single();
                                    var Pre_Class_Id = _db.School_M_Class.Where(d => d.ASMCL_Order == classorder && d.MI_Id == MIID.MI_Id).Select(x => x.ASMCL_Id).Single();
                                    if (getclassName != null)
                                    {
                                        studDto.ClassStudied = getclassName;
                                        studDto.Pre_Class_Id = Pre_Class_Id;
                                    }
                                    else
                                    {
                                        studDto.ClassStudied = "Not Avaiable";
                                        studDto.Pre_Class_Id = 0;
                                    }
                                }
                                else
                                {
                                    studDto.ClassStudied = null;
                                    stud.FirstOrDefault().class_order = 0;
                                }
                                if (stud.FirstOrDefault().section_order > 0)
                                {
                                    int sectionorder = stud.Select(x => x.section_order).Single();
                                    var getsectionName = _db.School_M_Section.Where(d => d.ASMC_Order == sectionorder && d.MI_Id == MIID.MI_Id).Select(x => x.ASMC_SectionName).Single();
                                    var Section_Id = _db.School_M_Section.Where(d => d.ASMC_Order == sectionorder && d.MI_Id == MIID.MI_Id).Select(x => x.ASMS_Id).Single();
                                    if (getsectionName != null)
                                    {
                                        studDto.previous_section = getsectionName;
                                        studDto.Section_Id = Section_Id;
                                    }
                                    else
                                    {
                                        studDto.previous_section = "Not Available";
                                        studDto.Section_Id = 0;
                                    }
                                }
                                else
                                {
                                    studDto.previous_section = null;
                                    stud.FirstOrDefault().section_order = 0;
                                }
                                var Tc_Amst_Count = _db.Student_TC.Where(d => d.AMST_Id == MIID.AMST_Id).ToList();
                                try
                                {
                                    if (Tc_Amst_Count.Count > 0)
                                    {
                                        Console.WriteLine("Record already exists");
                                        studDto.returnval = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            try
            {
                //sp for attendance
                List<StudentTCDTO> result = new List<StudentTCDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Get_Toalclassheld_Attendance_Student_TC";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MIID.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = Convert.ToString(MIID.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.VarChar) { Value = Convert.ToString(MIID.AMST_Id) });
                    cmd.Parameters.Add(new SqlParameter("@AMST_SOL", SqlDbType.VarChar) { Value = MIID.Status_flag });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                result.Add(new StudentTCDTO
                                {
                                    countclass = Convert.ToInt32(dataReader1["Classes"]),
                                    attclasscount = Convert.ToInt32(dataReader1["Attendance"])
                                });

                            }
                        }
                        if (result.Count > 0)
                        {
                            studDto.countclass = result.FirstOrDefault().countclass;
                            studDto.attclasscount = result.FirstOrDefault().attclasscount;
                        }
                        else
                        {
                            //studDto.message = "Please Enter The Number Of Class Held For Particular Month In Master Class Held";
                        }

                        var dataatt = (from a in _db.Adm_studentAttendance
                                       from b in _db.Adm_studentAttendanceStudents
                                       where a.ASA_Id == b.ASA_Id && a.MI_Id == MIID.MI_Id && b.AMST_Id == MIID.AMST_Id
                                       select new StudentTCDTO
                                       {
                                           todateatt = a.ASA_ToDate
                                       }).Distinct().ToList();

                        studDto.todateatt = dataatt.Max(x => x.todateatt);

                    }
                    catch (Exception ex)
                    {
                        _tcimpl.LogInformation("TC attendance count sp error :'" + ex.Message + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                _tcimpl.LogInformation("TC attendance count: '" + ex.Message + "'");
            }
            try
            {
                studDto.get_elective_subjects = (from a in _db.StudentMappingDMO
                                                 from b in _db.AcademicYear
                                                 from c in _db.Adm_M_Student
                                                 from d in _db.SchoolYearWiseStudent
                                                 from e in _db.MasterSubjectList
                                                 where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id && a.MI_Id == MIID.MI_Id && a.ASMAY_Id == MIID.ASMAY_Id && a.AMST_Id == MIID.AMST_Id && a.ESTSU_ElecetiveFlag == true && e.ISMS_LanguageFlg == 0)
                                                 select new StudentTCDTO
                                                 {
                                                     subjectname = e.ISMS_SubjectName
                                                 }).Distinct().ToArray();

                studDto.get_elective_subjects_language = (from a in _db.StudentMappingDMO
                                                          from b in _db.AcademicYear
                                                          from c in _db.Adm_M_Student
                                                          from d in _db.SchoolYearWiseStudent
                                                          from e in _db.MasterSubjectList
                                                          where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id && a.MI_Id == MIID.MI_Id && a.ASMAY_Id == MIID.ASMAY_Id && a.AMST_Id == MIID.AMST_Id && a.ESTSU_ElecetiveFlag == true && e.ISMS_LanguageFlg == 1)
                                                          select new StudentTCDTO
                                                          {
                                                              subjectname = e.ISMS_SubjectName
                                                          }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _tcimpl.LogInformation("TC Exam count: '" + ex.Message + "'");
            }


            try
            {
                //Fee Due
                List<StudentTCDTO> result = new List<StudentTCDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentTcFeeDetailsById";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMST_ID", SqlDbType.BigInt) { Value = MIID.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = MIID.Status_flag });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MIID.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = MIID.ASMAY_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                result.Add(new StudentTCDTO
                                {
                                    feetobepaid = Convert.ToInt32(dataReader1["FSS_ToBePaid"]),
                                });

                            }
                        }
                        if (result.Count > 0)
                        {
                            studDto.feetobepaid = result.FirstOrDefault().feetobepaid;
                        }
                        else
                        {
                            studDto.feetobepaid = 0;
                        }

                        var dataatt = (from a in _db.Adm_studentAttendance
                                       from b in _db.Adm_studentAttendanceStudents
                                       where a.ASA_Id == b.ASA_Id && a.MI_Id == MIID.MI_Id && b.AMST_Id == MIID.AMST_Id
                                       select new StudentTCDTO
                                       {
                                           todateatt = a.ASA_ToDate
                                       }).Distinct().ToList();

                        studDto.todateatt = dataatt.Max(x => x.todateatt);

                    }
                    catch (Exception ex)
                    {
                        _tcimpl.LogInformation("TC attendance count sp error :'" + ex.Message + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                _tcimpl.LogInformation("TC attendance count: '" + ex.Message + "'");
            }

            return studDto;
        }

        //for Saving the result
        public async Task<StudentTCDTO> saveTcdet(StudentTCDTO tc_dto)
        {
            var Tc_Amst_Count = 0;

            try
            {
                //update tc 
                StudentTC tc_dmo = Mapper.Map<StudentTC>(tc_dto);
                Tc_Amst_Count = _db.Student_TC.Where(d => d.AMST_Id == tc_dto.AMST_Id).Count();

                // updating the tc details for temporary student records(it will become left students)
                if (Tc_Amst_Count > 0)
                {
                    var Tc_Amst_Count1 = _db.Student_TC.Where(d => d.AMST_Id == tc_dto.AMST_Id).Single();

                    if (Tc_Amst_Count1.ASTC_ActiveFlag == "T")
                    {
                        var update_tc = _db.Student_TC.Single(d => d.ASTC_Id == Tc_Amst_Count1.ASTC_Id);

                        update_tc.ASTC_TemporaryFlag = 0;
                        update_tc.ASTC_ActiveFlag = "L";
                        update_tc.UpdatedDate = DateTime.Now;
                        update_tc.ASTC_AttendedDays = Convert.ToInt64(tc_dto.ASTC_AttendedDays);
                        update_tc.ASTC_Conduct = tc_dto.ASTC_Conduct;
                        update_tc.ASTC_LastAttendedDate = Convert.ToDateTime(tc_dto.ASTC_LastAttendedDate);
                        update_tc.ASTC_TCDate = Convert.ToDateTime(tc_dto.ASTC_TCDate);
                        update_tc.ASTC_TCApplicationDate = Convert.ToDateTime(tc_dto.ASTC_TCApplicationDate);
                        update_tc.ASTC_PromotionDate = Convert.ToDateTime(tc_dto.ASTC_PromotionDate);

                        _db.Student_TC.Update(update_tc);

                        var update_ADM = _db.Adm_M_Student.Single(d => d.AMST_Id == tc_dto.AMST_Id);
                        var update_admy = _db.School_Adm_Y_StudentDMO.Single(d => d.AMST_Id == tc_dto.AMST_Id && d.AMAY_ActiveFlag == 1
                        && d.ASMAY_Id == tc_dto.ASMAY_Id);

                        update_ADM.AMST_SOL = "L";
                        update_ADM.AMST_ActiveFlag = 0;
                        update_admy.AMAY_ActiveFlag = 0;
                        update_ADM.UpdatedDate = DateTime.Now;
                        update_admy.UpdatedDate = DateTime.Now;

                        _db.Adm_M_Student.Update(update_ADM);
                        _db.School_Adm_Y_StudentDMO.Update(update_admy);

                        // var flag = _db.SaveChanges();

                        var update_flag1 = _db.SaveChanges();

                        if (update_flag1 >= 1)
                        {
                            tc_dto.returnval = true;

                            // Concession Procedure Related To Fee

                            try
                            {
                                var getconcesstionflag = (from a in _db.Adm_M_Student
                                                          from b in _db.Fee_Master_ConcessionDMO
                                                          where (a.AMST_Concession_Type == b.FMCC_Id && a.AMST_Id == tc_dto.AMST_Id && a.MI_Id == tc_dto.MI_Id)
                                                          select b).Distinct().ToList();

                                if (getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "S" || getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "E" || getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "R")
                                {
                                    var outputval = _db.Database.ExecuteSqlCommand("DELETE_STUDENT_ON_TC  @p0,@p1,@p2,@p3",
                                     tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id, getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper());

                                    if (outputval >= 1)
                                    {
                                        tc_dto.returnval = true;
                                    }
                                    else
                                    {
                                        tc_dto.returnval = true;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            // Updating The Role id and Role Type for Alumni


                            try
                            {
                                // Updating The Role id and Role Type for Alumni

                                var getstdappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == tc_dto.AMST_Id).Select(a => a.STD_APP_ID).ToList();

                                var getroleid = _db.applicationRole.Where(a => a.Name.Equals("Alumni", StringComparison.OrdinalIgnoreCase)).ToList();

                                var getroletypeid = _db.MasterRoleType.Where(a => a.IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase)).ToList();

                                var getapplicationuserrole = _db.appUserRole.Where(a => a.UserId == getstdappid[0]).ToList();
                                if (getapplicationuserrole.Count() > 0)
                                {
                                    var getapplicationuserrolenew = _db.appUserRole.Single(a => a.UserId == getstdappid[0]);

                                    getapplicationuserrolenew.RoleId = getroleid.FirstOrDefault().Id;
                                    getapplicationuserrolenew.RoleTypeId = getroletypeid.FirstOrDefault().IVRMRT_Id;

                                    _db.Update(getapplicationuserrolenew);
                                    var id = _db.SaveChanges();
                                    if (id > 0)
                                    {
                                        tc_dto.returnval = true;
                                    }
                                    else
                                    {
                                        tc_dto.returnval = true;
                                    }
                                }

                                //var chckuser = _db.UserRoleWithInstituteDMO.Where(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]).ToList();

                                //if (chckuser.Count() > 0)
                                //{
                                //    var updatecheckuser = _db.UserRoleWithInstituteDMO.Single(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]);

                                //    updatecheckuser.Activeflag = 0;
                                //    _db.Update(updatecheckuser);
                                //    var i = _db.SaveChanges();
                                //    if (i > 0)
                                //    {
                                //        tc_dto.returnval = true;
                                //    }
                                //    else
                                //    {
                                //        tc_dto.returnval = true;
                                //    }
                                //}
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            try
                            {
                                var outputval = _db.Database.ExecuteSqlCommand("Alumini_Students_Insert  @p0,@p1,@p2",
                                       tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id);

                                if (outputval >= 1)
                                {
                                    tc_dto.returnval = true;
                                }
                                else
                                {
                                    tc_dto.returnval = true;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            //var getstdappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == tc_dto.AMST_Id).Select(a => a.STD_APP_ID).ToList();

                            //var chckuser = _db.UserRoleWithInstituteDMO.Where(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]).ToList();

                            //if (chckuser.Count() > 0)
                            //{
                            //    var updatecheckuser = _db.UserRoleWithInstituteDMO.Single(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]);

                            //    updatecheckuser.Activeflag = 0;
                            //    _db.Update(updatecheckuser);
                            //    var i = _db.SaveChanges();
                            //    if (i > 0)
                            //    {
                            //        tc_dto.returnval = true;
                            //    }
                            //    else
                            //    {
                            //        tc_dto.returnval = true;
                            //    }
                            //}
                        }
                        else
                        {
                            tc_dto.returnval = false;
                        }
                    }

                    //updating the left students records

                    if (Tc_Amst_Count1.ASTC_ActiveFlag == "L")
                    {
                        var update_tc1 = _db.Student_TC.Single(d => d.ASTC_Id == Tc_Amst_Count1.ASTC_Id);

                        update_tc1.ASTC_AttendedDays = Convert.ToInt64(tc_dto.ASTC_AttendedDays);
                        update_tc1.ASTC_Conduct = tc_dto.ASTC_Conduct;
                        update_tc1.ASTC_LastAttendedDate = Convert.ToDateTime(tc_dto.ASTC_LastAttendedDate);
                        update_tc1.ASTC_TCDate = Convert.ToDateTime(tc_dto.ASTC_TCDate);
                        update_tc1.ASTC_TCApplicationDate = Convert.ToDateTime(tc_dto.ASTC_TCApplicationDate);
                        update_tc1.ASTC_PromotionDate = Convert.ToDateTime(tc_dto.ASTC_PromotionDate);

                        update_tc1.ASTC_ElectivesStudied = tc_dto.ASTC_ElectivesStudied;
                        update_tc1.ASTC_ExtraActivities = tc_dto.ASTC_ExtraActivities;
                        update_tc1.ASTC_FeeConcession = tc_dto.ASTC_FeeConcession;
                        update_tc1.ASTC_FeePaid = tc_dto.ASTC_FeePaid;
                        update_tc1.ASTC_LanguageStudied = tc_dto.ASTC_LanguageStudied;
                        update_tc1.ASTC_LastExamDetails = tc_dto.ASTC_LastExamDetails;
                        update_tc1.ASTC_LeavingReason = tc_dto.ASTC_LeavingReason;
                        update_tc1.ASTC_MedicallyExam = tc_dto.ASTC_MedicallyExam;
                        update_tc1.ASTC_MediumOfINStruction = tc_dto.ASTC_MediumOfINStruction;
                        update_tc1.ASTC_NCCDetails = tc_dto.ASTC_NCCDetails;
                        update_tc1.ASTC_Qual_Class = tc_dto.ASTC_Qual_Class;
                        update_tc1.ASTC_Qual_PromotionFlag = tc_dto.ASTC_Qual_PromotionFlag;
                        update_tc1.ASTC_Remarks = tc_dto.ASTC_Remarks;
                        update_tc1.ASTC_Result = tc_dto.ASTC_Result;
                        update_tc1.ASTC_ResultDetails = tc_dto.ASTC_ResultDetails;
                        update_tc1.ASTC_Scholarship = tc_dto.ASTC_Scholarship;
                        update_tc1.ASTC_WorkingDays = Convert.ToInt64(tc_dto.ASTC_WorkingDays);
                        update_tc1.Last_Class_Studied = tc_dto.Last_Class_Studied;
                        update_tc1.UpdatedDate = DateTime.Now;
                        _db.Student_TC.Update(update_tc1);

                        var update_flag1 = _db.SaveChanges();

                        if (update_flag1 >= 1)
                        {
                            tc_dto.returnval = true;
                        }
                        else
                        {
                            tc_dto.returnval = false;
                        }
                    }
                }


                // saving the tc details for active /deactive students for both temp/permanent tc
                else
                {
                    if (tc_dto.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        tc_dto.transnumconfigsettings.MI_Id = tc_dmo.MI_Id;
                        tc_dto.transnumconfigsettings.ASMAY_Id = tc_dmo.ASMAY_Id;
                        //MM2.AMC_Id = mas.AMC_Id;
                        tc_dmo.ASTC_TCNO = a.GenerateNumber(tc_dto.transnumconfigsettings);
                    }
                    //-
                    if (tc_dto.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                    {
                        //Manual regNo.
                        var tcnoduplicate = _db.Student_TC.Where(d => d.ASTC_TCNO == tc_dto.ASTC_TCNO && d.MI_Id == tc_dto.MI_Id).Count();
                        if (tcnoduplicate > 0)
                        {
                            tc_dto.tcflagexists = "TC No. Already Exists";
                            return tc_dto;
                        }
                    }


                    var studDet = _db.Adm_M_Student.Where(d => d.AMST_Id == tc_dto.AMST_Id).ToList();
                    var section = _act.school_Adm_Y_StudentDMO.Where(d => d.AMST_Id == tc_dto.AMST_Id && d.ASMAY_Id == tc_dto.ASMAY_Id).ToList();

                    if (section.Count == 0)
                    {
                        tc_dmo.ASMCL_Id = 0;
                        tc_dmo.ASMS_Id = 0;
                    }

                    else
                    {
                        for (int i = 0; i < section.Count; i++)
                        {
                            tc_dmo.ASMCL_Id = section[i].ASMCL_Id;
                            tc_dmo.ASMS_Id = section[i].ASMS_Id;
                        }
                    }

                    //tc_dmo.ASMAY_Id = studDet.FirstOrDefault().ASMAY_Id;
                    tc_dmo.ASMAY_Id = tc_dto.ASMAY_Id;
                    tc_dmo.IMC_Id = studDet.FirstOrDefault().IC_Id;
                    tc_dmo.MI_Id = studDet.FirstOrDefault().MI_Id;

                    //saving the new records for active student/decative student(it will become either temp or left according to ASTC_TemporaryFlag) 
                    if (tc_dto.ASTC_ActiveFlag == "S" || tc_dto.ASTC_ActiveFlag == "D")
                    {
                        if (tc_dto.ASTC_TemporaryFlag == 1)
                        {
                            tc_dmo.ASTC_ActiveFlag = "T";
                            tc_dmo.ASTC_TemporaryFlag = 1;
                            tc_dmo.CreatedDate = DateTime.Now;
                            tc_dmo.UpdatedDate = DateTime.Now;
                            _db.Student_TC.Add(tc_dmo);
                            var flag = _db.SaveChanges();
                            if (flag == 1)
                            {
                                tc_dto.returnval = true;
                            }
                            else
                            {
                                tc_dto.returnval = false;
                            }
                        }

                        else
                        {
                            tc_dmo.ASTC_ActiveFlag = "L";
                            tc_dmo.ASTC_TemporaryFlag = 0;
                            tc_dmo.CreatedDate = DateTime.Now;
                            tc_dmo.UpdatedDate = DateTime.Now;
                            _db.Student_TC.Add(tc_dmo);
                            var flag = _db.SaveChanges();

                            if (flag == 1)
                            {
                                tc_dto.returnval = true;

                                try
                                {
                                    // Concession Procedure Related To Fee

                                    var getconcesstionflag = (from a in _db.Adm_M_Student
                                                              from b in _db.Fee_Master_ConcessionDMO
                                                              where (a.AMST_Concession_Type == b.FMCC_Id && a.AMST_Id == tc_dto.AMST_Id && a.MI_Id == tc_dto.MI_Id)
                                                              select b).Distinct().ToList();

                                    if (getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "S" || getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "E" || getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper() == "R")
                                    {
                                        var outputval = _db.Database.ExecuteSqlCommand("DELETE_STUDENT_ON_TC  @p0,@p1,@p2,@p3",
                                         tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id, getconcesstionflag.FirstOrDefault().FMCC_ConcessionFlag.ToUpper());

                                        if (outputval >= 1)
                                        {
                                            tc_dto.returnval = true;
                                        }
                                        else
                                        {
                                            tc_dto.returnval = true;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                try
                                {
                                    // Updating The Role id and Role Type for Alumni

                                    var getstdappid = _db.StudentAppUserLoginDMO.Where(a => a.AMST_ID == tc_dto.AMST_Id).Select(a => a.STD_APP_ID).ToList();

                                    var getroleid = _db.applicationRole.Where(a => a.Name.Equals("Alumni", StringComparison.OrdinalIgnoreCase)).ToList();

                                    var getroletypeid = _db.MasterRoleType.Where(a => a.IVRMRT_Role.Equals("Alumni", StringComparison.OrdinalIgnoreCase)).ToList();

                                    var getapplicationuserrole = _db.appUserRole.Where(a => a.UserId == getstdappid[0]).ToList();
                                    if (getapplicationuserrole.Count() > 0)
                                    {
                                        var getapplicationuserrolenew = _db.appUserRole.Single(a => a.UserId == getstdappid[0]);

                                        getapplicationuserrolenew.RoleId = getroleid.FirstOrDefault().Id;
                                        getapplicationuserrolenew.RoleTypeId = getroletypeid.FirstOrDefault().IVRMRT_Id;

                                        _db.Update(getapplicationuserrolenew);
                                        var id = _db.SaveChanges();
                                        if (id > 0)
                                        {
                                            tc_dto.returnval = true;
                                        }
                                        else
                                        {
                                            tc_dto.returnval = true;
                                        }
                                    }

                                    //var chckuser = _db.UserRoleWithInstituteDMO.Where(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]).ToList();

                                    //if (chckuser.Count() > 0)
                                    //{
                                    //    var updatecheckuser = _db.UserRoleWithInstituteDMO.Single(a => a.MI_Id == tc_dto.MI_Id && a.Id == getstdappid[0]);

                                    //    updatecheckuser.Activeflag = 0;
                                    //    _db.Update(updatecheckuser);
                                    //    var i = _db.SaveChanges();
                                    //    if (i > 0)
                                    //    {
                                    //        tc_dto.returnval = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        tc_dto.returnval = true;
                                    //    }
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                try
                                {
                                    var outputval = _db.Database.ExecuteSqlCommand("Alumini_Students_Insert  @p0,@p1,@p2",
                                           tc_dto.MI_Id, tc_dto.ASMAY_Id, tc_dto.AMST_Id);

                                    if (outputval >= 1)
                                    {
                                        tc_dto.returnval = true;
                                    }
                                    else
                                    {
                                        tc_dto.returnval = true;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                            }
                            else
                            {
                                tc_dto.returnval = false;
                            }
                        }
                    }


                    //updating the records for active student/decative student(it will become either temp or left according to ASTC_TemporaryFlag)                    


                    var update_ADM = _db.Adm_M_Student.FirstOrDefault(d => d.AMST_Id == tc_dto.AMST_Id);
                    var update_admy = _db.School_Adm_Y_StudentDMO.FirstOrDefault(d => d.AMST_Id == tc_dto.AMST_Id && d.AMAY_ActiveFlag == 1
                    && d.ASMAY_Id == tc_dto.ASMAY_Id);

                    if (update_ADM.AMST_SOL == "S" || update_ADM.AMST_SOL == "D")
                    {
                        if (tc_dto.ASTC_TemporaryFlag == 1)
                        {
                            update_ADM.AMST_SOL = "T";
                            update_ADM.AMST_ActiveFlag = 1;
                            update_admy.AMAY_ActiveFlag = 1;
                        }
                        else
                        {
                            update_ADM.AMST_SOL = "L";
                            update_ADM.AMST_ActiveFlag = 0;
                            update_admy.AMAY_ActiveFlag = 0;
                        }

                        update_ADM.UpdatedDate = DateTime.Now;
                        _db.Adm_M_Student.Update(update_ADM);
                        _db.School_Adm_Y_StudentDMO.Update(update_admy);

                        var update_flag = _db.SaveChanges();

                        if (update_flag >= 1)
                        {
                            tc_dto.returnval = true;
                        }
                        else
                        {
                            tc_dto.returnval = false;
                        }
                    }
                }

                //////////////////////////////////////////Sending Mail and SMS///////////////////////////////////////////////////////////
                if (tc_dto.returnval == true && tc_dto.Email_flag.Equals("Yes"))
                {
                    var admConfig = _db.AdmissionStandardDMO.Single(t => t.MI_Id == tc_dto.MI_Id);
                    var studDetail = _db.Adm_M_Student.Where(t => t.MI_Id == tc_dto.MI_Id && t.AMST_Id == tc_dto.AMST_Id).ToList();

                    var Tc_Amst_Count1 = _db.Student_TC.Where(d => d.AMST_Id == tc_dto.AMST_Id).Single();
                    var update_tc1 = _db.Student_TC.Single(d => d.ASTC_Id == Tc_Amst_Count1.ASTC_Id);

                    if (admConfig.ASC_DefaultSMS_Flag == "M") //To Mother MAIL/SMS
                    {

                        SMS sms = new SMS(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else
                        if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MotherMobileNo), "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else
                        {

                        }

                        Email Email = new Email(_db);

                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else
                        if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_MotherEmailId, "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                        else
                        {

                        }
                    }
                    else if (admConfig.ASC_DefaultSMS_Flag == "F")  //To Father MAIL/SMS
                    {
                        SMS sms = new SMS(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else
                        if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_FatherMobleNo), "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else
                        {

                        }
                        Email Email = new Email(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_FatheremailId, "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                        else
                        {

                        }
                    }
                    else     //To Student MAIL/SMS
                    {
                        SMS sms = new SMS(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else
                        if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string s = await sms.sendSms(tc_dto.MI_Id, Convert.ToInt64(studDetail.FirstOrDefault().AMST_MobileNo), "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (s == "success")
                            {
                                tc_dto.Email_flag = "sms_sent";
                            }
                        }
                        else
                        {

                        }

                        Email Email = new Email(_db);
                        if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 1 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL_TEMPORARY", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("S")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && (tc_dto.ASTC_ActiveFlag.Equals("D")))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }

                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("T") && studDetail.FirstOrDefault().AMST_Id == tc_dto.AMST_Id)
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                        else if (tc_dto.ASTC_TemporaryFlag == 0 && tc_dto.ASTC_ActiveFlag.Equals("L") && (update_tc1.AMST_Id == tc_dto.AMST_Id))
                        {
                            string m = Email.sendmail(tc_dto.MI_Id, studDetail.FirstOrDefault().AMST_emailId, "STUDENT_TC_EMAIL_UPDATION", tc_dto.AMST_Id);
                            if (m == "success")
                            {
                                tc_dto.Email_flag = "sent";
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tc_dto;
        }
        public StudentTCDTO chk_tc_dup(StudentTCDTO tc_no_dup)
        {


            try
            {
                var tcnoduplicate = _db.Student_TC.Where(d => d.ASTC_TCNO == tc_no_dup.ASTC_TCNO && d.MI_Id == tc_no_dup.MI_Id).Count();
                if (tcnoduplicate > 0)
                {
                    tc_no_dup.tcflagexists = "TC No. Already Exists";
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tc_no_dup;
        }
        public async Task<StudentTCDTO> getstudent_name_list(StudentTCDTO get_stu_name_list)
        {
            get_stu_name_list.admTransNumSetting = await _db.Master_Numbering.Where(t => t.MI_Id == get_stu_name_list.MI_Id && t.IMN_Flag.Equals("tcno")).ToArrayAsync();

            //get_stu_name_list.admTransNumSetting = await _db.Master_Numbering.Where(t => t.MI_Id == get_stu_name_list.MI_Id).ToArrayAsync();

            StudentTCDTO attdto = new StudentTCDTO();
            List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
            admissionconfigurationsettings = _db.AdmissionStandardDMO.AsNoTracking().Where(t => t.MI_Id == get_stu_name_list.MI_Id).ToList();
            get_stu_name_list.admissioncongigurationList = admissionconfigurationsettings.ToArray();

            if (admissionconfigurationsettings.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
            {
                get_stu_name_list.adm_num_flag = "A";
            }
            else if (admissionconfigurationsettings.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
            {
                get_stu_name_list.adm_num_flag = "N";
            }
            get_stu_name_list.tcpermanentpayment = await _db.AdmissionStandardDMO.Where(t => t.MI_Id == get_stu_name_list.MI_Id).ToArrayAsync();

            get_stu_name_list.classlistnew = _db.School_M_Class.Where(a => a.MI_Id == get_stu_name_list.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();

            get_stu_name_list.get_elective_subjects = (from a in _db.StudentMappingDMO
                                                       from b in _db.AcademicYear
                                                       from c in _db.Adm_M_Student
                                                       from d in _db.SchoolYearWiseStudent
                                                       from e in _db.MasterSubjectList
                                                       where (a.ASMAY_Id == b.ASMAY_Id && d.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && e.ISMS_Id == a.ISMS_Id && a.MI_Id == get_stu_name_list.MI_Id && a.ASMAY_Id == get_stu_name_list.ASMAY_Id && a.AMST_Id == get_stu_name_list.AMST_Id && a.ESTSU_ElecetiveFlag == true)
                                                       select new StudentTCDTO
                                                       {
                                                           subjectname = e.ISMS_SubjectName
                                                       }).Distinct().ToArray();


            List<MasterAcademic> allyear = new List<MasterAcademic>();
            allyear = _db.AcademicYear.Where(t => t.MI_Id == get_stu_name_list.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            get_stu_name_list.academicList = allyear.ToArray();

            get_stu_name_list.currentYear = _db.AcademicYear.Where(a => a.MI_Id == get_stu_name_list.MI_Id && a.Is_Active == true && a.ASMAY_Id == get_stu_name_list.ASMAY_Id).ToArray();


            try
            {
                List<StudentTCDTO> ln = new List<StudentTCDTO>();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return get_stu_name_list;
        }

        //searching 
        public StudentTCDTO searchfilter(StudentTCDTO data)
        {
            try
            {
                // var flag = "";
                //List<AdmissionStandardDMO> admissionconfigurationsettings = new List<AdmissionStandardDMO>();
                //admissionconfigurationsettings = _db.AdmissionStandardDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id).ToList();
                //data.admissioncongigurationList = admissionconfigurationsettings.ToArray();

                //if (admissionconfigurationsettings.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
                //{
                //    flag = "A";
                //}
                //else if (admissionconfigurationsettings.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
                //{
                //    flag = "N";
                //}
                data.searchfilter = data.searchfilter.ToUpper();
                int activeflag = 0;
                int activeflagyear = 0;

                if (data.AMST_SOL == "S")
                {
                    activeflag = 1;
                    activeflagyear = 1;
                }
                else if (data.AMST_SOL == "L")
                {
                    activeflag = 0;
                    activeflagyear = 0;
                }
                else if (data.AMST_SOL == "D")
                {
                    activeflag = 1;
                    activeflagyear = 1;
                }
                else if (data.AMST_SOL == "T")
                {
                    activeflag = 1;
                    activeflagyear = 1;
                }
                if (data.allorindividual == "All")
                {
                    if (data.flag == "A")
                    {
                        data.studentlistsearch = (from a in _db.Adm_M_Student
                                                  from b in _db.SchoolYearWiseStudent
                                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear && (a.AMST_AdmNo.StartsWith(data.searchfilter)))
                                                  select new StudentTCDTO
                                                  {
                                                      AMST_Id = a.AMST_Id,
                                                      StudentName = (a.AMST_AdmNo + ':' + (a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                                                  }).ToArray();
                    }
                    else if (data.flag == "N")
                    {
                        data.studentlistsearch = (from a in _db.Adm_M_Student
                                                  from b in _db.SchoolYearWiseStudent
                                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                                  select new StudentTCDTO
                                                  {
                                                      AMST_Id = a.AMST_Id,
                                                      StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                                                  }).ToArray();
                    }

                }
                else
                {
                    //if (data.flag == "A")
                    //{
                    //    data.studentlistsearch = (from a in _db.Adm_M_Student
                    //                              from b in _db.SchoolYearWiseStudent
                    //                              where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && (a.AMST_AdmNo.StartsWith(data.searchfilter)))
                    //                              select new StudentTCDTO
                    //                              {
                    //                                  AMST_Id = a.AMST_Id,
                    //                                  StudentName = (a.AMST_AdmNo + ':' + (a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                    //                              }).ToArray();
                    //}

                    //else if (data.flag == "N")
                    //{
                    //    data.studentlistsearch = (from a in _db.Adm_M_Student
                    //                              from b in _db.SchoolYearWiseStudent
                    //                              where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                    //                              select new StudentTCDTO
                    //                              {
                    //                                  AMST_Id = a.AMST_Id,
                    //                                  StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                    //                              }).ToArray();
                    //}
                    if (data.flag == "A")
                    {
                        data.studentlistsearch = (from a in _db.Adm_M_Student
                                                  from b in _db.SchoolYearWiseStudent
                                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear && (a.AMST_AdmNo.StartsWith(data.searchfilter)))
                                                  select new StudentTCDTO
                                                  {
                                                      AMST_Id = a.AMST_Id,
                                                      StudentName = (a.AMST_AdmNo + ':' + (a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                                                  }).ToArray();
                    }

                    else if (data.flag == "N")
                    {
                        data.studentlistsearch = (from a in _db.Adm_M_Student
                                                  from b in _db.SchoolYearWiseStudent
                                                  where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL.Equals(data.AMST_SOL) && a.AMST_ActiveFlag == activeflag && b.AMAY_ActiveFlag == activeflagyear && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' + a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || a.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || a.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                                  select new StudentTCDTO
                                                  {
                                                      AMST_Id = a.AMST_Id,
                                                      StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                                                  }).ToArray();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}

