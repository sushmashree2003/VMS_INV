using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Identity;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class StudentGatePassImpl : Interfaces.StudentGatePassInterface
    {
        public DomainModelMsSqlServerContext _db;
        public VisitorsManagementContext _visctxt;
        public DomainModelMsSqlServerContext _context;
        private readonly UserManager<ApplicationUser> _UserManager;
        public StudentGatePassImpl(DomainModelMsSqlServerContext db, VisitorsManagementContext context, DomainModelMsSqlServerContext pad, UserManager<ApplicationUser> usermanager)
        {
            _db = db;
            _visctxt = context;
            _context = pad;
            _UserManager = usermanager;
        }
        public StudentGatePass_DTO getdetails(StudentGatePass_DTO dTO)
        {
            try
            {
                dTO.yearlist = _db.AcademicYear.Where(t => t.MI_Id == dTO.MI_Id && t.ASMAY_ActiveFlag == 1 && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                dTO.alldata = (from a in _visctxt.Gate_Pass_Student_DMO
                               from b in _visctxt.Adm_M_Student
                               from c in _visctxt.School_Adm_Y_StudentDMO
                               from d in _visctxt.admissionClass
                               from e in _visctxt.masterSection
                               where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == c.ASMS_Id && a.MI_Id == dTO.MI_Id && c.ASMAY_Id == dTO.ASMAY_Id && b.AMST_SOL == "S" && c.AMAY_ActiveFlag == 1)
                               select new StudentGatePass_DTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   GPHS_Id = a.GPHS_Id,
                                   GPHS_GatePassNo = a.GPHS_GatePassNo,
                                   GPHS_IDCardNo = a.GPHS_IDCardNo,
                                   GPHS_DateTime = a.GPHS_DateTime,
                                   GPHS_Remarks = a.GPHS_Remarks,
                                   ASMCL_ClassName = d.ASMCL_ClassName,
                                   ASMC_SectionName = e.ASMC_SectionName,
                                   ASMCL_Id = d.ASMCL_Id,
                                   ASMS_Id = e.ASMS_Id,
                                   ASMAY_Id = c.ASMAY_Id,
                                   studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) ? "" : ' ' + b.AMST_LastName),
                                   AMST_AdmNo = b.AMST_AdmNo,
                                   GPHS_ActiveFlg = a.GPHS_ActiveFlg,
                                   GPHS_ReceiverName = a.GPHS_ReceiverName,
                                   GPHS_ReceiverPhoneNo = a.GPHS_ReceiverPhoneNo,
                                   GPHS_ReceiverIdProof = a.GPHS_ReceiverIdProof,
                                   GPHS_SentFlg = a.GPHS_SentFlg,
                               }).Distinct().OrderByDescending(t => t.GPHS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }
        public StudentGatePass_DTO get_class(StudentGatePass_DTO dto)
        {
            try
            {
                dto.classList = (from a in _db.School_Adm_Y_StudentDMO
                                 from b in _db.School_M_Class
                                 where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag == true)
                                 select new StudentGatePass_DTO
                                 {
                                     ASMCL_Id = b.ASMCL_Id,
                                     ASMCL_ClassName = b.ASMCL_ClassName
                                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public StudentGatePass_DTO get_section(StudentGatePass_DTO dto)
        {
            try
            {
                dto.SectionList = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.School_M_Section
                                   where (a.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.ASMC_ActiveFlag == 1)
                                   select new StudentGatePass_DTO
                                   {
                                       ASMS_Id = b.ASMS_Id,
                                       ASMC_SectionName = b.ASMC_SectionName
                                   }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public StudentGatePass_DTO get_student(StudentGatePass_DTO dto)
        {
            try
            {
                dto.StudentList = (from a in _db.School_Adm_Y_StudentDMO
                                   from c in _db.School_M_Class
                                   from s in _db.School_M_Section
                                   from b in _db.Adm_M_Student
                                   where (b.MI_Id == c.MI_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && b.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                                   select new StudentGatePass_DTO
                                   {
                                       AMST_Id = b.AMST_Id,
                                       studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                       AMST_AdmNo = b.AMST_AdmNo,
                                       ASMCL_Id = c.ASMCL_Id,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMS_Id = s.ASMS_Id,
                                       ASMC_SectionName = s.ASMC_SectionName
                                   }).Distinct().OrderBy(b => b.studentname).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public async Task<StudentGatePass_DTO> saverecordAsync(StudentGatePass_DTO data)
        {
            try
            {
                string genotp = "";
                string studSecretCode = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();

                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "StudentGatePass").ToList();
                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }
                if (data.GPHS_Id == 0)
                {
                    ApplicationUser user = new ApplicationUser();
                    var studentdata = (from t in _context.SchoolYearWiseStudent
                                       from a in _context.Adm_M_Student
                                       where (t.AMST_Id == a.AMST_Id && a.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.ASMAY_Id == data.ASMAY_Id)
                                       select a
                               ).Distinct().ToList();
                    CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                    genotp = generate.getOTP();
                    if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                        data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                        data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                    }

                    var Duplicate = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.GPHS_GatePassNo == data.trans_id && t.GPHS_IDCardNo == data.GPHS_IDCardNo && t.GPHS_DateTime == data.GPHS_DateTime).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        string SecretCode = "";
                        var checkSecretcode = (from b in _visctxt.Adm_M_Student
                                               where (b.MI_Id == data.MI_Id && b.AMST_Id == data.AMST_Id /*amst_ids*/
                                               && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                                               select new StudentGatePass_DTO
                                               {
                                                   AMST_SecretCode = b.AMST_SecretCode,
                                               }).Distinct().ToList();

                        SecretCode = checkSecretcode.SingleOrDefault().AMST_SecretCode;
                        if (SecretCode == null || SecretCode == "")
                        {                           
                            studSecretCode = generate.getFourDigitOTP();

                            var stud = _visctxt.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id).Single();

                            stud.AMST_SecretCode = studSecretCode;
                            stud.UpdatedDate = DateTime.Now;

                            _visctxt.Update(stud);
                        }

                        Gate_Pass_Student_DMO obj = new Gate_Pass_Student_DMO();

                        obj.MI_Id = data.MI_Id;
                        obj.AMST_Id = data.AMST_Id;
                        obj.GPHS_GatePassNo = data.trans_id;
                        obj.GPHS_IDCardNo = data.GPHS_IDCardNo;
                        obj.GPHS_DateTime = indianTime;
                        obj.GPHS_Remarks = data.GPHS_Remarks;
                        obj.GPHS_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.GPHS_CreatedBy = data.UserId;
                        obj.GPHS_UpdatedBy = data.UserId;
                        obj.GPHS_ReceiverName = data.GPHS_ReceiverName;
                        obj.GPHS_ReceiverPhoneNo = data.GPHS_ReceiverPhoneNo;
                        obj.GPHS_ReceiverIdProof = data.GPHS_ReceiverIdProof;
                        obj.GPHS_SecretCode = studSecretCode;
                        obj.GPHS_OTP = Convert.ToInt64(genotp);
                        obj.GPHS_SentFlg = false;
                        _visctxt.Add(obj);
                        int rowAffected = _visctxt.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                            var rowdata = _context.Gate_Pass_Student_DMO.OrderByDescending(d => d.GPHS_Id).First();
                            long? mobileNo = studentdata.SingleOrDefault().AMST_FatherMobleNo;
                            //mobileNo = Convert.ToInt64("9771237044");
                            long AMST_Id = studentdata.SingleOrDefault().AMST_Id;
                            long UserID = rowdata.GPHS_Id;

                            SMS sms = new SMS(_context);
                            string s = await sms.SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", Convert.ToInt64(genotp), UserID, AMST_Id);

                            data.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number!";
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.GPHS_Id > 0)
                {
                    var Duplicate = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id != data.GPHS_Id && t.AMST_Id == data.AMST_Id && t.GPHS_GatePassNo == data.GPHS_GatePassNo && t.GPHS_IDCardNo == data.GPHS_IDCardNo && t.GPHS_DateTime == data.GPHS_DateTime).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id);

                        result.AMST_Id = data.AMST_Id;
                        result.GPHS_IDCardNo = data.GPHS_IDCardNo;
                        result.GPHS_DateTime = indianTime;
                        result.GPHS_Remarks = data.GPHS_Remarks;
                        result.UpdatedDate = DateTime.Now;
                        result.GPHS_UpdatedBy = data.UserId;
                        result.GPHS_ReceiverName = data.GPHS_ReceiverName;
                        result.GPHS_ReceiverPhoneNo = data.GPHS_ReceiverPhoneNo;
                        result.GPHS_ReceiverIdProof = data.GPHS_ReceiverIdProof;

                        _visctxt.Update(result);
                        int rowAffected = _visctxt.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO deactive(StudentGatePass_DTO data)
        {
            try
            {
                var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.GPHS_Id == data.GPHS_Id && t.MI_Id == data.MI_Id);

                if (result.GPHS_ActiveFlg == true)
                {
                    result.GPHS_ActiveFlg = false;
                }
                else if (result.GPHS_ActiveFlg == false)
                {
                    result.GPHS_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _visctxt.Update(result);
                int rowAffected = _visctxt.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO editrecord(StudentGatePass_DTO data)
        {
            try
            {
                data.editlist = (from a in _visctxt.Gate_Pass_Student_DMO
                                 from b in _visctxt.Adm_M_Student
                                 from c in _visctxt.School_Adm_Y_StudentDMO
                                 from d in _visctxt.admissionClass
                                 from e in _visctxt.masterSection
                                 where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && a.GPHS_Id == data.GPHS_Id && b.AMST_SOL == "S" && c.AMAY_ActiveFlag == 1)
                                 select new StudentGatePass_DTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                     GPHS_Id = a.GPHS_Id,
                                     GPHS_GatePassNo = a.GPHS_GatePassNo,
                                     GPHS_IDCardNo = a.GPHS_IDCardNo,
                                     GPHS_DateTime = a.GPHS_DateTime,
                                     GPHS_Remarks = a.GPHS_Remarks,
                                     ASMCL_Id = d.ASMCL_Id,
                                     ASMS_Id = e.ASMS_Id,
                                     ASMAY_Id = c.ASMAY_Id,
                                     GPHS_ReceiverName = a.GPHS_ReceiverName,
                                     GPHS_ReceiverPhoneNo = a.GPHS_ReceiverPhoneNo,
                                     GPHS_ReceiverIdProof = a.GPHS_ReceiverIdProof,

                                 }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO checkstudentdata(StudentGatePass_DTO data)
        {
            try
            {
                var singlestudent = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).ToList();
                data.singlestudentdata = singlestudent.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO get_otpverification(StudentGatePass_DTO data)
        {
            try
            {
                long? otp = 0;

                var gatepassid = (from a in _visctxt.Gate_Pass_Student_DMO
                                  where (a.GPHS_Id == data.GPHS_Id)
                                  select new StudentGatePass_DTO
                                  {
                                      GPHS_OTP = a.GPHS_OTP
                                  }).Distinct().ToList();

                otp = gatepassid.Select(t => t.GPHS_OTP).Single();

                if (otp == data.GPHS_OTP)
                {
                    DateTime datechkkkkk = new DateTime();
                    datechkkkkk = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).UpdatedDate;
                    string checkmobnokk = datechkkkkk.ToString();

                    if (checkmobnokk != "")
                    {
                        DateTime newdate = DateTime.Now;
                        TimeSpan ts = DateTime.Now - datechkkkkk;
                        //double ts2 = ts.TotalHours;
                        double t22 = ts.TotalMinutes;

                        if (15 <= t22)
                        {
                            data.message = "Your Time is Over. Please Resend Your OTP!";
                        }
                        else
                        {
                            var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id);

                            result.GPHS_SentFlg = true;
                            result.UpdatedDate = DateTime.Now;
                            result.GPHS_UpdatedBy = data.UserId;
                            _visctxt.Update(result);
                            int rowAffected = _visctxt.SaveChanges();

                            if (rowAffected > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                    else
                    {
                        data.message = "Please Register Your Mobile Number!";
                    }
                }
                else
                {
                    data.message = "Please Enter Valid OTP Number!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<StudentGatePass_DTO> resendotp(StudentGatePass_DTO data)
        {
            try
            {
                string studSecretCode = "";
                if (data.GPHS_Id > 0)
                {
                    DateTime datechkkkkk = new DateTime();
                    datechkkkkk = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).UpdatedDate;
                    string checkmobnokk = datechkkkkk.ToString();
                    if (checkmobnokk != "")
                    {
                        DateTime newdate = DateTime.Now;
                        TimeSpan ts = DateTime.Now - datechkkkkk;
                        //double ts2 = ts.TotalHours;
                        double t22 = ts.TotalMinutes;

                        if (15 <= t22)
                        {
                            string genotp = "";
                            ApplicationUser user = new ApplicationUser();

                            var gphsid = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).Select(t => t.AMST_Id).Distinct().ToList();
                            var studentdata = (from x in _context.Adm_M_Student
                                               where (x.MI_Id == data.MI_Id && gphsid.Contains(x.AMST_Id)
                                               && x.AMST_SOL == "S")
                                               select x).Distinct().ToList();
                            long? mobileNo = new long?();
                            mobileNo = studentdata.SingleOrDefault().AMST_FatherMobleNo;
                            //mobileNo = Convert.ToInt64("9771237044");
                            long AMST_Id = studentdata.SingleOrDefault().AMST_Id;
                            long UserID = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).SingleOrDefault().GPHS_Id;
                            CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                            genotp = generate.getOTP();
                            SMS sms = new SMS(_context);

                            string s = await sms.SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", Convert.ToInt64(genotp), UserID, AMST_Id);

                            data.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number!";

                            string SecretCode = "";
                            var amstids = (from t in _visctxt.Gate_Pass_Student_DMO
                                           where (t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id)
                                           select new StudentGatePass_DTO
                                           {
                                               AMST_Id = t.AMST_Id
                                           }).Distinct().ToList();
                            long amst_ids = amstids.SingleOrDefault().AMST_Id;                           
                            var checkSecretcode = (from b in _visctxt.Adm_M_Student
                                                   where (b.MI_Id == data.MI_Id && b.AMST_Id.Equals(amst_ids) && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                                                   select new StudentGatePass_DTO
                                                   {
                                                       AMST_SecretCode = b.AMST_SecretCode,
                                                   }).Distinct().ToList();

                            SecretCode = checkSecretcode.Select(t => t.AMST_SecretCode).SingleOrDefault();
                            if (SecretCode == null || SecretCode == "")
                            {                                
                                studSecretCode = generate.getFourDigitOTP();

                                var stud = _visctxt.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id.Equals(amst_ids)).SingleOrDefault();

                                stud.AMST_SecretCode = studSecretCode;
                                stud.UpdatedDate = DateTime.Now;

                                _visctxt.Update(stud);
                            }

                            var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id);

                            result.UpdatedDate = DateTime.Now;
                            result.GPHS_UpdatedBy = data.UserId;
                            result.GPHS_OTP = Convert.ToInt64(genotp);

                            _visctxt.Update(result);
                            int rowAffected = _visctxt.SaveChanges();

                            if (rowAffected > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            var mobileotp = (from t in _visctxt.Gate_Pass_Student_DMO where (t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id) select t).Distinct().ToList();

                            var gphsid22 = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).Select(t => t.AMST_Id).Distinct().ToList();
                            var studentdata22 = (from f in _context.Adm_M_Student
                                                 where (f.MI_Id == data.MI_Id && gphsid22.Contains(f.AMST_Id)
                                                 && f.AMST_SOL == "S")
                                                 select f).Distinct().ToList();

                            long? mobileNo = studentdata22.SingleOrDefault().AMST_FatherMobleNo;
                            //mobileNo = Convert.ToInt64("9771237044");
                            long AMST_Id = studentdata22.SingleOrDefault().AMST_Id;
                            long? gatepassotp = mobileotp.SingleOrDefault().GPHS_OTP;

                            long UserID = _visctxt.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).SingleOrDefault().GPHS_Id;

                            SMS sms = new SMS(_context);
                            string s = await sms.SendSMSForMobile(data.MI_Id, Convert.ToInt64(mobileNo), "MOBILEOTP", Convert.ToInt64(gatepassotp), UserID, AMST_Id);

                            data.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number!";
                        }
                    }
                    else
                    {
                        data.message = "Please Register Your Mobile Number!";
                    }
                }

                var getotpid = _context.Gate_Pass_Student_DMO.Where(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).Select(t => t.GPHS_Id).Distinct().ToList();
                data.otpid = getotpid.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO get_otpverification22(StudentGatePass_DTO data)
        {
            try
            {
                long? otp = 0;
                var gatepassid = (from a in _visctxt.Gate_Pass_Student_DMO
                                  where (a.GPHS_Id == data.GPHS_Id)
                                  select new StudentGatePass_DTO
                                  {
                                      GPHS_OTP = a.GPHS_OTP
                                  }).Distinct().ToList();

                otp = gatepassid.Select(t => t.GPHS_OTP).Single();

                if (otp == data.GPHS_OTP)
                {
                    DateTime datechkkkkk = new DateTime();
                    datechkkkkk = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id).UpdatedDate;
                    string checkmobnokk = datechkkkkk.ToString();
                    if (checkmobnokk != "")
                    {
                        DateTime newdate = DateTime.Now;
                        TimeSpan ts = DateTime.Now - datechkkkkk;
                        //double ts2 = ts.TotalHours;
                        double t22 = ts.TotalMinutes;
                        if (15 <= t22)
                        {
                            data.message = "Your Time is Over. Again Resend Your OTP!";
                        }
                        else
                        {
                            var result = _visctxt.Gate_Pass_Student_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHS_Id == data.GPHS_Id);
                            result.GPHS_SentFlg = true;
                            result.UpdatedDate = DateTime.Now;
                            result.GPHS_UpdatedBy = data.UserId;
                            _visctxt.Update(result);
                            int rowAffected = _visctxt.SaveChanges();
                            if (rowAffected > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                }
                else
                {
                    data.message = "Please Enter Valid OTP Number!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentGatePass_DTO printbutton(StudentGatePass_DTO data)
        {
            try
            {
                var  currentstuddatalist = (from a in _visctxt.Gate_Pass_Student_DMO
                                        from b in _visctxt.Adm_M_Student
                                        from y in _visctxt.School_Adm_Y_StudentDMO
                                        from c in _visctxt.admissionClass
                                        from s in _visctxt.masterSection
                                        where (a.GPHS_Id == data.GPHS_Id && y.ASMS_Id == s.ASMS_Id
                                        && a.MI_Id==data.MI_Id && a.AMST_Id == b.AMST_Id && y.AMST_Id == a.AMST_Id
                                        && y.ASMAY_Id == data.ASMAY_Id && b.MI_Id==a.MI_Id && y.ASMCL_Id == c.ASMCL_Id )
                                        select new StudentGatePass_DTO
                                        {
                                            GPHS_Id = a.GPHS_Id,
                                            GPHS_GatePassNo = a.GPHS_GatePassNo,
                                            GPHS_IDCardNo = a.GPHS_IDCardNo,
                                            GPHS_DateTime = a.GPHS_DateTime,
                                            GPHS_Remarks = a.GPHS_Remarks,
                                            studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) ? "" : ' ' + b.AMST_LastName),
                                            AMST_AdmNo = b.AMST_AdmNo,
                                            ASMC_SectionName = s.ASMC_SectionName,
                                            ASMCL_ClassName = c.ASMCL_ClassName,
                                            AMST_MobileNo = b.AMST_MobileNo,
                                            GPHS_ReceiverName = a.GPHS_ReceiverName,
                                            GPHS_ReceiverPhoneNo = a.GPHS_ReceiverPhoneNo,
                                            GPHS_ReceiverIdProof = a.GPHS_ReceiverIdProof,
                                            GPHS_SentFlg = a.GPHS_SentFlg,
                                            AMST_Photoname = b.AMST_Photoname,

                                        }).Distinct().OrderByDescending(t => t.GPHS_Id).ToList();
                data.currentstuddata = currentstuddatalist.ToArray();

                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();

                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                string accountname = "", accesskey = "", html = "";
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }
                //if (currentstuddatalist.Count > 0)
                //{
                //    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + data.MI_Id, currentstuddatalist[0].AMC_PAApplicationName + ".html", 0);
                //}
                //else
                //{
                //    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + data.MI_Id, "Admissionform.html", 0);
                //}
                //string pathToHTMLFile = @"E:\Aman\2019 Code\November\IVRM_VS_Code_19112019\IVRM_VS_19th_NOV_2019\IVRMUX\wwwroot\StudentGatePassJSH.html";
                //string pathToHTMLFile = @"E:\Aman\2019 Code\November\IVRM_VS_Code_19112019\IVRM_VS_19th_NOV_2019\IVRMUX\wwwroot\gatepassstudent.html";
                //data.htmldata = System.IO.File.ReadAllText(pathToHTMLFile);

                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "gatepass/" + data.MI_Id, "StudentGatePass.html", 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                data.htmldata = html;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
