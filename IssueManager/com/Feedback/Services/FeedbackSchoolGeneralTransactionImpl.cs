using DataAccessMsSqlServerProvider.FeedBack;
using DomainModel.Model.FeedBack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;


namespace NaacServiceHub.FeedBack.Services
{
    public class FeedbackSchoolGeneralTransactionImpl : Interface.FeedbackSchoolGeneralTransactionInterface
    {
        public FeedBackContext _context;
        ILogger<FeedbackSchoolGeneralTransactionImpl> _log;
        public FeedbackSchoolGeneralTransactionImpl(FeedBackContext context, ILogger<FeedbackSchoolGeneralTransactionImpl> log)
        {
            _context = context;
            _log = log;
        }

        public FeedbackSchoolGeneralTransactionDTO getdetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                var rolenamet = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                if (rolenamet.ToUpper() == "STUDENT")
                {
                    data.Flag = "Student";
                }
                else if (rolenamet.ToUpper() == "PARENTS")
                {
                    data.Flag = "Parents";
                }
                else if (rolenamet.ToUpper() == "ALUMNI")
                {
                    data.Flag = "Alumni";
                }
                else
                {
                    data.Flag = "Staff";
                }


                data.typelistload = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_ActiveFlag == true).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();


                data.mappedquestiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                               from b in _context.Feedback_Master_QuestionsDMO
                                               from c in _context.Feedback_Type_QuestionsDMO
                                               where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.FMTY_ActiveFlag == true && b.FMQE_ActiveFlag == true
                                               && c.FMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMQE_ActiveFlag == true && a.FMTY_ActiveFlag == true
                                               && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false)
                                               select new FeedbackSchoolGeneralTransactionDTO
                                               {
                                                   FMTY_Id = c.FMTY_Id,
                                                   FMQE_Id = c.FMQE_Id,
                                                   FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                   FMTQ_TQOrder = c.FMTQ_TQOrder
                                               }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();

                data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                             from b in _context.Feedback_Master_OptionsDMO
                                             from c in _context.Feedback_Type_OptionsDMO
                                             where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                             && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true && c.FMTO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false)
                                             select new FeedbackSchoolGeneralTransactionDTO
                                             {
                                                 FMTY_Id = c.FMTY_Id,
                                                 FMOP_Id = c.FMOP_Id,
                                                 FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                 FMTO_TOOrder = c.FMTO_TQOrder
                                             }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new FeedbackSchoolGeneralTransactionDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                ).ToList();

                if (rolenamet.ToUpper() == "STUDENT")
                {
                    var checkamcstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.userid).ToList();
                    var check_dataenter = (_context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id ==
                    Convert.ToInt64(checkamcstid.FirstOrDefault().AMST_ID) && a.FSSTR_FeedbackDate.Date == (DateTime.Now).Date && a.FSSTR_StudParFlg == data.Flag)).ToList();
                    data.count = check_dataenter.Count();

                    data.AMST_Id = checkamcstid.FirstOrDefault().AMST_ID;

                }
                else if (rolenamet.ToUpper() == "PARENTS")
                {
                    var checkamcstid = _context.StudentAppUserLoginDMO.Where(a => a.FAT_APP_ID == data.userid || a.MOT_APP_ID == data.userid).ToList();
                    var check_dataenter = (_context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id ==
                    Convert.ToInt64(checkamcstid.FirstOrDefault().AMST_ID) && a.FSSTR_FeedbackDate.Date == (DateTime.Now).Date && a.FSSTR_StudParFlg == data.Flag)).ToList();

                    data.count = check_dataenter.Count();
                    data.AMST_Id = checkamcstid.FirstOrDefault().AMST_ID;
                }
                else if (rolenamet.ToUpper() == "ALUMNI")
                {
                    data.Flag = "Alumni";
                }
                else
                {
                    var checkamcstid = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();

                    var check_dataenter = (_context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id ==
                    Convert.ToInt64(checkamcstid.FirstOrDefault().Emp_Code) && a.FSSTR_FeedbackDate.Date == (DateTime.Now).Date && a.FSTTR_ActiveFlag == true)).ToList();
                    data.count = check_dataenter.Count();
                }


                if (data.Flag != "Alumni" && data.Flag != "Staff")
                {
                    var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                             from b in _context.Adm_M_Student
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S")
                                             select new FeedbackSchoolGeneralTransactionDTO
                                             {
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                             }).Distinct().ToList();

                    data.getstudentdetails = getstudentdetails.ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO getfeedback(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                var rolenamet = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                if (rolenamet.ToUpper() == "STUDENT")
                {
                    data.Flag = "Student";
                }
                else if (rolenamet.ToUpper() == "PARENTS")
                {
                    data.Flag = "Parents";
                }
                else if (rolenamet.ToUpper() == "ALUMNI")
                {
                    data.Flag = "Alumni";
                }
                else
                {
                    data.Flag = "Staff";
                }


                var typelist = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag
                && a.FMTY_SubjectSpecificFlag == false && a.FMTY_Id == data.FMTY_Id).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.typelist = typelist.ToArray();

                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_ActiveFlag == true).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();

                data.mappedquestiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                               from b in _context.Feedback_Master_QuestionsDMO
                                               from c in _context.Feedback_Type_QuestionsDMO
                                               where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.FMTY_ActiveFlag == true && b.FMQE_ActiveFlag == true
                                               && c.FMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMQE_ActiveFlag == true && a.FMTY_ActiveFlag == true
                                               && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false && a.FMTY_Id == data.FMTY_Id)
                                               select new FeedbackSchoolGeneralTransactionDTO
                                               {
                                                   FMTY_Id = c.FMTY_Id,
                                                   FMQE_Id = c.FMQE_Id,
                                                   FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                   FMTQ_TQOrder = c.FMTQ_TQOrder,
                                                   FMQE_ManualEntryFlg = b.FMQE_ManualEntryFlg
                                               }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();



                if (typelist.FirstOrDefault().FMTY_QuestionwiseOptionFlg == false)
                {
                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                 from b in _context.Feedback_Master_OptionsDMO
                                                 from c in _context.Feedback_Type_OptionsDMO
                                                 where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                                 && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true && c.FMTO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false && a.FMTY_Id == data.FMTY_Id)
                                                 select new FeedbackSchoolGeneralTransactionDTO
                                                 {
                                                     FMTY_Id = c.FMTY_Id,
                                                     FMOP_Id = c.FMOP_Id,
                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                     FMTO_TOOrder = c.FMTO_TQOrder
                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                }
                else
                {
                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                 from b in _context.Feedback_Master_OptionsDMO
                                                 from c in _context.Feedback_Type_QuestionsDMO
                                                 from d in _context.Feedback_Type_Questions_OptionsDMO
                                                 where (d.FMTQ_Id == c.FMTQ_Id && a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == d.FMOP_Id && a.FMTY_ActiveFlag == true
                                                 && b.FMOP_ActiveFlag == true && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true
                                                 && d.FMTQO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false
                                                 && a.FMTY_Id == data.FMTY_Id && c.FMTQ_ActiveFlag == true)
                                                 select new FeedbackTransactionDTO
                                                 {
                                                     FMTY_Id = c.FMTY_Id,
                                                     FMOP_Id = d.FMOP_Id,
                                                     FMQE_Id = c.FMQE_Id,
                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                     FMTO_TOOrder = d.FMTQO_TQOOrder
                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                }

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                long checkdatecount = 0;
                long? feedbacknooftimes = 0;

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new FeedbackSchoolGeneralTransactionDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                if (rolenamet.ToUpper() == "STUDENT")
                {
                    var checkamcstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.userid).ToList();
                    var check_dataenter = (_context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.AMST_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().AMST_ID) && a.FMTY_Id == data.FMTY_Id
                    && a.FSSTR_StudParFlg == data.Flag)).ToList();
                    data.count = check_dataenter.Count();

                    var count = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                      && a.AMST_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().AMST_ID) && a.FMTY_Id == data.FMTY_Id
                      && a.FSSTR_StudParFlg == data.Flag && a.ASMAY_Id == data.ASMAY_Id).Distinct().Count();

                    checkdatecount = count;

                    feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByStudent;

                }
                else if (rolenamet.ToUpper() == "PARENTS")
                {
                    var checkamcstid = _context.StudentAppUserLoginDMO.Where(a => a.FAT_APP_ID == data.userid).ToList();

                    if (checkamcstid.Count == 0)
                    {
                        checkamcstid = _context.StudentAppUserLoginDMO.Where(a => a.MOT_APP_ID == data.userid).ToList();
                    }

                    var check_dataenter = (_context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.AMST_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().AMST_ID) && a.FMTY_Id == data.FMTY_Id

                    && a.FSSTR_StudParFlg == data.Flag && a.FSSTR_CreatedBy == data.userid)).ToList();

                    data.count = check_dataenter.Count();

                    var count = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id
                      && a.AMST_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().AMST_ID) && a.FMTY_Id == data.FMTY_Id
                      && a.FSSTR_StudParFlg == data.Flag && a.ASMAY_Id == data.ASMAY_Id && a.FSSTR_CreatedBy == data.userid).Distinct().Count();

                    checkdatecount = count;

                    feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByParent;

                }
                else if (rolenamet.ToUpper() == "ALUMNI")
                {
                    data.Flag = "Alumni";
                    feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByAlumni;
                }
                else
                {
                    var checkamcstid = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();
                    var check_dataenter = (_context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id ==
                    Convert.ToInt64(checkamcstid.FirstOrDefault().Emp_Code) && a.FMTY_Id == data.FMTY_Id && a.FSTTR_ActiveFlag == true)).ToList();
                    data.count = check_dataenter.Count();

                    var count1 = (from a in _context.Feedback_Staff_TransactionDMO
                                  from b in _context.AcademicYear
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == Convert.ToInt64(checkamcstid.FirstOrDefault().Emp_Code)
                                  && a.FMTY_Id == data.FMTY_Id && a.FSTTR_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id
                                  && Convert.ToDateTime(b.ASMAY_From_Date) <= Convert.ToDateTime(a.FSSTR_FeedbackDate.Date)
                                  && Convert.ToDateTime(b.ASMAY_To_Date) >= Convert.ToDateTime(a.FSSTR_FeedbackDate.Date))
                                  select a).Distinct().Count();

                    checkdatecount = count1;

                    feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByStaff;
                }

                if ((feedbacknooftimes * data.mappedquestiondeetails.Length) > checkdatecount)
                {
                    data.count = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO savedata(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                if (data.Flag.ToUpper() == "STUDENT" || data.Flag.ToUpper() == "PARENTS")
                {
                    if (data.Flag.ToUpper() == "PARENTS")
                    {
                        var getamstid = _context.StudentAppUserLoginDMO.Where(a => a.FAT_APP_ID == data.userid).ToList();
                        if (getamstid.Count() > 0)
                        {
                            data.AMST_Id = getamstid.FirstOrDefault().AMST_ID;
                        }
                        else
                        {
                            var getamstid1 = _context.StudentAppUserLoginDMO.Where(a => a.MOT_APP_ID == data.userid).ToList();
                            if (getamstid1.Count() > 0)
                            {
                                data.AMST_Id = getamstid1.FirstOrDefault().AMST_ID;
                            }
                        }
                    }
                    else
                    {
                        data.AMST_Id = data.AMST_Id;
                    }



                    var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                             from b in _context.Adm_M_Student
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S")
                                             select new FeedbackSchoolGeneralTransactionDTO
                                             {
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                             }).Distinct().ToList();



                    if (data.temp.Count() > 0)
                    {
                        for (int k = 0; k < data.temp.Count(); k++)
                        {
                            for (int j = 0; j < data.temp[k].ques.Count(); j++)
                            {
                                Feedback_School_Student_TransactionDMO dmo = new Feedback_School_Student_TransactionDMO();
                                if (data.temp[k].ques[j].manualflg == true)
                                {
                                    dmo.FSSTR_FeedBack = data.temp[k].ques[j].name;
                                }
                                else
                                {
                                    dmo.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                    dmo.FSSTR_FeedBack = "";
                                }
                                dmo.MI_Id = data.MI_Id;
                                dmo.AMST_Id = data.AMST_Id;
                                dmo.ASMAY_Id = data.ASMAY_Id;
                                dmo.ASMCL_Id = Convert.ToInt64(getstudentdetails.FirstOrDefault().ASMCL_Id);
                                dmo.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                                dmo.FSSTR_FeedbackDate = DateTime.Now;
                                dmo.FMTY_Id = data.temp[k].FMTY_Id;
                                dmo.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                dmo.FSSTR_StudParFlg = data.Flag;
                                dmo.FSSTR_ActiveFlag = true;
                                dmo.FSSTR_UpdatedBy = data.userid;
                                dmo.FSSTR_CreatedBy = data.userid;
                                dmo.UpdatedDate = DateTime.Now;
                                dmo.CreatedDate = DateTime.Now;
                                _context.Add(dmo);
                            }
                        }

                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                else if (data.Flag.ToUpper() == "ALUMNI")
                {
                    var checkamcstid = _context.Alumni_User_LoginDMO_con.Where(a => a.IVRMUL_Id == data.userid).ToList();

                    data.AMST_Id = Convert.ToInt64(checkamcstid.FirstOrDefault().ALSREG_Id);

                    if (data.temp.Count() > 0)
                    {
                        for (int k = 0; k < data.temp.Count(); k++)
                        {
                            for (int j = 0; j < data.temp[k].ques.Count(); j++)
                            {
                                Feedback_Alumni_TransactionDMO dmo = new Feedback_Alumni_TransactionDMO();

                                if (data.temp[k].ques[j].manualflg == true)
                                {
                                    dmo.FALTR_FeedBack = data.temp[k].ques[j].name;
                                }
                                else
                                {
                                    dmo.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                    dmo.FALTR_FeedBack = "";
                                }
                                dmo.MI_Id = data.MI_Id;
                                dmo.ALMST_Id = data.AMST_Id;
                                //dmo.ASMAY_ID = data.ASMAY_Id;
                                dmo.FALTR_FeedbackDate = DateTime.Now;
                                dmo.FMTY_Id = data.temp[k].FMTY_Id;
                                dmo.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                dmo.FALTR_ActiveFlag = true;

                                dmo.FALTR_UpdatedBy = data.userid;
                                dmo.FALTR_CreatedBy = data.userid;
                                dmo.CreatedDate = DateTime.Now;
                                dmo.UpdatedDate = DateTime.Now;
                                _context.Add(dmo);
                            }
                        }

                        int i = _context.SaveChanges();
                        if (i > 0)
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
                    var checkamcstid = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();
                    data.HRME_Id = checkamcstid.FirstOrDefault().Emp_Code;

                    if (data.temp.Count() > 0)
                    {
                        for (int k = 0; k < data.temp.Count(); k++)
                        {
                            for (int j = 0; j < data.temp[k].ques.Count(); j++)
                            {
                                Feedback_Staff_TransactionDMO staff = new Feedback_Staff_TransactionDMO();
                                if (data.temp[k].ques[j].manualflg == true)
                                {
                                    staff.FSTTR_FeedBack = data.temp[k].ques[j].name;
                                }
                                else
                                {
                                    staff.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                    staff.FSTTR_FeedBack = "";
                                }
                                staff.MI_Id = data.MI_Id;
                                staff.HRME_Id = data.HRME_Id;
                                staff.FSSTR_FeedbackDate = DateTime.Now;
                                staff.FMTY_Id = data.temp[k].FMTY_Id;
                                staff.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                staff.FSTTR_ActiveFlag = true;
                                staff.FSTTR_CreatedBy = data.userid;
                                staff.FSTTR_UpdatedBy = data.userid;
                                staff.CreatedDate = DateTime.Now;
                                staff.UpdatedDate = DateTime.Now;
                                _context.Add(staff);
                            }
                        }
                    }
                    if (data.temp.Count() > 0)
                    {
                        int i = _context.SaveChanges();
                        if (i > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
                _log.LogInformation("College Student Feedback Save Parents :" + ex.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO getstudentstaffdetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S")
                                         select new FeedbackSchoolGeneralTransactionDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                if (getstudentdetails.Count() > 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "School_Get_Studentwise_SubjectList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;

                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.getsubjectlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO getstaffname(FeedbackSchoolGeneralTransactionDTO data)
        {

            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S")
                                         select new FeedbackSchoolGeneralTransactionDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                         }).Distinct().ToList();

                List<FeedbackSchoolGeneralTransactionDTO> details = new List<FeedbackSchoolGeneralTransactionDTO>();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Get_Staff_Subject_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = getstudentdetails.FirstOrDefault().ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                details.Add(new FeedbackSchoolGeneralTransactionDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                    staffname = (dataReader["staffname"]).ToString(),
                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.typelist = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_ActiveFlag == true).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();


                data.mappedquestiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                               from b in _context.Feedback_Master_QuestionsDMO
                                               from c in _context.Feedback_Type_QuestionsDMO
                                               where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.FMTY_ActiveFlag == true && b.FMQE_ActiveFlag == true
                                               && c.FMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMQE_ActiveFlag == true && a.FMTY_ActiveFlag == true
                                               && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true)
                                               select new FeedbackSchoolGeneralTransactionDTO
                                               {
                                                   FMTY_Id = c.FMTY_Id,
                                                   FMQE_Id = c.FMQE_Id,
                                                   FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                   FMTQ_TQOrder = c.FMTQ_TQOrder
                                               }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();


                data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                             from b in _context.Feedback_Master_OptionsDMO
                                             from c in _context.Feedback_Type_OptionsDMO
                                             where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                             && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true && c.FMTO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true)
                                             select new FeedbackSchoolGeneralTransactionDTO
                                             {
                                                 FMTY_Id = c.FMTY_Id,
                                                 FMOP_Id = c.FMOP_Id,
                                                 FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                 FMTO_TOOrder = c.FMTO_TQOrder
                                             }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(fromdatecon.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new FeedbackSchoolGeneralTransactionDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                ).ToList();


                var checkamcstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.userid).ToList();


                if (details.Count() > 0)
                {
                    data.HRME_Id = details.FirstOrDefault().HRME_Id;

                    data.getstaffdetails = details.ToArray();

                    var check_dataenter = (_context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id &&
                    a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                    && a.FSSTST_FeedbackDate.Date == (DateTime.Now).Date &&
                    a.ISMS_Id == data.ISMS_Id && a.AMST_Id == data.AMST_Id && a.HRME_Id == data.HRME_Id)).ToList();

                    data.count = check_dataenter.Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO getfeedbacksubject(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S")
                                         select new FeedbackSchoolGeneralTransactionDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                         }).Distinct().ToList();

                var typelist = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag
                && a.FMTY_SubjectSpecificFlag == true && a.FMTY_Id == data.FMTY_Id).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.typelist = typelist.ToArray();

                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_ActiveFlag == true).Distinct().
                    OrderBy(a => a.FMQE_FQOrder).ToArray();


                data.mappedquestiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                               from b in _context.Feedback_Master_QuestionsDMO
                                               from c in _context.Feedback_Type_QuestionsDMO
                                               where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.FMTY_ActiveFlag == true && b.FMQE_ActiveFlag == true
                                               && c.FMTQ_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMQE_ActiveFlag == true && a.FMTY_ActiveFlag == true
                                               && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true && a.FMTY_Id == data.FMTY_Id)
                                               select new FeedbackSchoolGeneralTransactionDTO
                                               {
                                                   FMTY_Id = c.FMTY_Id,
                                                   FMQE_Id = c.FMQE_Id,
                                                   FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                   FMTQ_TQOrder = c.FMTQ_TQOrder,
                                                   FMQE_ManualEntryFlg = b.FMQE_ManualEntryFlg
                                               }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();

                if (typelist.FirstOrDefault().FMTY_QuestionwiseOptionFlg == false)
                {
                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                 from b in _context.Feedback_Master_OptionsDMO
                                                 from c in _context.Feedback_Type_OptionsDMO
                                                 where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                                 && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true && c.FMTO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == true && a.FMTY_Id == data.FMTY_Id)
                                                 select new FeedbackSchoolGeneralTransactionDTO
                                                 {
                                                     FMTY_Id = c.FMTY_Id,
                                                     FMOP_Id = c.FMOP_Id,
                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                     FMTO_TOOrder = c.FMTO_TQOrder
                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                }
                else
                {
                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                 from b in _context.Feedback_Master_OptionsDMO
                                                 from c in _context.Feedback_Type_QuestionsDMO
                                                 from d in _context.Feedback_Type_Questions_OptionsDMO
                                                 where (d.FMTQ_Id == c.FMTQ_Id && a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == d.FMOP_Id && a.FMTY_ActiveFlag == true
                                                 && b.FMOP_ActiveFlag == true && a.FMTY_ActiveFlag == true && a.MI_Id == data.MI_Id && b.FMOP_ActiveFlag == true
                                                 && d.FMTQO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false
                                                 && a.FMTY_Id == data.FMTY_Id && c.FMTQ_ActiveFlag == true)
                                                 select new FeedbackTransactionDTO
                                                 {
                                                     FMTY_Id = c.FMTY_Id,
                                                     FMOP_Id = d.FMOP_Id,
                                                     FMQE_Id = c.FMQE_Id,
                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                     FMTO_TOOrder = d.FMTQO_TQOOrder
                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                }
                long checkdatecount = 0;
                long? feedbacknooftimes = 0;

                var check_dataenter = (_context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ISMS_Id == data.ISMS_Id && a.AMST_Id == data.AMST_Id && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id)).ToList();
                data.count = check_dataenter.Count();

                var count = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id
                && a.ASMCL_Id == getstudentdetails.FirstOrDefault().ASMCL_Id && a.ASMS_Id == getstudentdetails.FirstOrDefault().ASMS_Id
                && a.ISMS_Id == data.ISMS_Id && a.AMST_Id == data.AMST_Id && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id).Count();

                checkdatecount = count;

                feedbacknooftimes = typelist.FirstOrDefault().FMTY_NOFPerYearByStudent;

                if ((feedbacknooftimes * data.mappedquestiondeetails.Length) > checkdatecount)
                {
                    data.count = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO studentstaffsavedata(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {

                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();

                data.Flag = check_rolenameget.FirstOrDefault().IVRMRT_Role;

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S")
                                         select new FeedbackSchoolGeneralTransactionDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                         }).Distinct().ToList();


                if (data.temp.Count() > 0)
                {
                    for (int k = 0; k < data.temp.Count(); k++)
                    {
                        for (int j = 0; j < data.temp[k].ques.Count(); j++)
                        {
                            Feedback_School_StudentToStaffDMO dmo = new Feedback_School_StudentToStaffDMO();
                            if (data.temp[k].ques[j].manualflg == true)
                            {
                                dmo.FSSTST_FeedBack = data.temp[k].ques[j].name;
                            }
                            else
                            {
                                dmo.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                dmo.FSSTST_FeedBack = "";
                            }

                            dmo.MI_Id = data.MI_Id;
                            dmo.AMST_Id = data.AMST_Id;
                            dmo.ASMAY_Id = data.ASMAY_Id;
                            dmo.ASMS_Id = Convert.ToInt64(getstudentdetails.FirstOrDefault().ASMS_Id);
                            dmo.ASMCL_Id = Convert.ToInt64(getstudentdetails.FirstOrDefault().ASMCL_Id);
                            dmo.HRME_Id = data.HRME_Id;
                            dmo.ISMS_Id = data.ISMS_Id;
                            dmo.FSSTST_FeedbackDate = DateTime.Now;
                            dmo.FMTY_Id = data.temp[k].FMTY_Id;
                            dmo.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                            dmo.FSSTST_ActiveFlag = true;
                            dmo.FSSTST_UpdatedBy = data.userid;
                            dmo.FSSTST_CreatedBy = data.userid;
                            dmo.UpdatedDate = DateTime.Now;
                            dmo.CreatedDate = DateTime.Now;
                            _context.Add(dmo);
                        }
                    }

                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
                _log.LogInformation("College Student Feedback Save Parents :" + ex.Message);
            }
            return data;
        }

        //Feedback_Staff_Transaction
        public FeedbackSchoolGeneralTransactionDTO getFeedback_Staff_Transactiondetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                data.emplist = (from a in _context.MasterEmployee
                                where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                select new FeedbackSchoolGeneralTransactionDTO
                                {
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id
                                }).Distinct().ToArray();

                data.typelistload = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_ActiveFlag == true).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();

                var useremp = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).ToList();
                data.TrainerHRME_Id = useremp.FirstOrDefault().Emp_Code;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeedbackStaffTransaction_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.TrainerHRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.getstaffdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO saveFeedback_Staff_Transactiondetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                long FSTTR_Id = 0;
                var emplist = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).ToList();
                data.TrainerHRME_Id = emplist.FirstOrDefault().Emp_Code;
                if (data.temp.Count() > 0)
                {
                    for (int k = 0; k < data.temp.Count(); k++)
                    {
                        Feedback_Staff_TransactionDMO staff = new Feedback_Staff_TransactionDMO();
                        staff.MI_Id = data.MI_Id;
                        staff.HRME_Id = data.TrainerHRME_Id;
                        staff.FSSTR_FeedbackDate = DateTime.Now;
                        staff.FMTY_Id = data.temp[k].FMTY_Id;
                        staff.FSTTR_ActiveFlag = true;
                        staff.FSTTR_CreatedBy = data.userid;
                        staff.FSTTR_UpdatedBy = data.userid;
                        staff.CreatedDate = DateTime.Now;
                        staff.UpdatedDate = DateTime.Now;
                        _context.Add(staff);
                        _context.SaveChanges();
                        FSTTR_Id = staff.FSTTR_Id;
                        for (int j = 0; j < data.temp[k].ques.Count(); j++)
                        {

                            if (data.temp[k].ques[j].manualflg == true && data.temp[k].ques[j].name != null)
                            {
                                Feedback_Staff_Transaction_DetailsDMO staffDetails = new Feedback_Staff_Transaction_DetailsDMO();
                                staffDetails.FSTTRD_FeedBack = data.temp[k].ques[j].name;
                                staffDetails.FSTTR_Id = FSTTR_Id;
                                staffDetails.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                staffDetails.FMOP_Id = data.FMOP_Id;
                                staffDetails.FSTTRD_ActiveFlag = true;
                                staffDetails.FSTTRD_CreatedDate = DateTime.Now;
                                staffDetails.FSTTRD_UpdatedDate = DateTime.Now;
                                staffDetails.FSTTRD_CreatedBy = data.userid;
                                staffDetails.FSTTRD_UpdatedBy = data.userid;
                                _context.Add(staffDetails);
                                _context.SaveChanges();

                                Feedback_Staff_Transaction_ToStaffDMO staffToDetails = new Feedback_Staff_Transaction_ToStaffDMO();
                                staffToDetails.HRME_Id = data.HRME_Id;
                                staffToDetails.FSTTR_Id = FSTTR_Id;
                                staffToDetails.FSTTRTST_ActiveFlag = true;
                                staffToDetails.FSTTRTST_CreatedBy = data.userid;
                                staffToDetails.FSTTRTST_UpdatedBy = data.userid;
                                staffToDetails.FSTTRTST_CreatedDate = DateTime.Now;
                                staffToDetails.FSTTRTST_UpdatedDate = DateTime.Now;
                                _context.Add(staffToDetails);
                                _context.SaveChanges();

                            }
                            else if (data.temp[k].ques[j].manualflg == false && data.temp[k].ques[j].opt != null)
                            {
                                if (data.temp[k].ques[j].name != null)
                                {
                                    long FMOPId = long.Parse(data.temp[k].ques[j].name);
                                    for (int i = 0; i < data.temp[k].ques[j].opt.Count(); i++)
                                    {
                                        if (FMOPId == data.temp[k].ques[j].opt[i].FMOP_Id)
                                        {
                                            Feedback_Staff_Transaction_DetailsDMO staffDetails = new Feedback_Staff_Transaction_DetailsDMO();
                                            staffDetails.FSTTRD_FeedBack = data.temp[k].ques[j].opt[i].FMOP_FeedbackOptions;
                                            staffDetails.FSTTR_Id = FSTTR_Id;
                                            staffDetails.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                            staffDetails.FMOP_Id = FMOPId;
                                            staffDetails.FSTTRD_ActiveFlag = true;
                                            staffDetails.FSTTRD_CreatedDate = DateTime.Now;
                                            staffDetails.FSTTRD_UpdatedDate = DateTime.Now;
                                            staffDetails.FSTTRD_CreatedBy = data.userid;
                                            staffDetails.FSTTRD_UpdatedBy = data.userid;
                                            _context.Add(staffDetails);
                                            _context.SaveChanges();

                                            Feedback_Staff_Transaction_ToStaffDMO staffToDetails = new Feedback_Staff_Transaction_ToStaffDMO();
                                            staffToDetails.HRME_Id = data.HRME_Id;
                                            staffToDetails.FSTTR_Id = FSTTR_Id;
                                            staffToDetails.FSTTRTST_ActiveFlag = true;
                                            staffToDetails.FSTTRTST_CreatedBy = data.userid;
                                            staffToDetails.FSTTRTST_UpdatedBy = data.userid;
                                            staffToDetails.FSTTRTST_CreatedDate = DateTime.Now;
                                            staffToDetails.FSTTRTST_UpdatedDate = DateTime.Now;
                                            _context.Add(staffToDetails);
                                            _context.SaveChanges();

                                        }

                                    }
                                }
                            }




                        }
                    }
                }
                if (data.temp.Count() > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO certificateFeedback_Staff_Transactiondetails(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeedbackStaffTransaction_Certificate_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FSTTR_Id", SqlDbType.BigInt)
                    {
                        Value = data.FSTTR_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        data.getstaffdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeedbackSchoolGeneralTransactionDTO getfeedback1(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                 data.Flag = "Staff";
                var CurrentDate = DateTime.Now;
                var checkamcstid1 = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();
                var getfeedbacktypeid = _context.FeedBackMasterTypeDMO.Where(T => T.FMTY_FeedbackTypeName == data.FMTY_FeedbackTypeName ).ToList();

                var getiehrmeids = _context.FO_Biometric_VAPS_IEMapping_DMO.Where(I => I.FOBVIEM_ActiveFlg == true && I.FOBVIEM_HRMEId == Convert.ToInt64(checkamcstid1.FirstOrDefault().Emp_Code)).ToList();

                if (getiehrmeids.Count==0)
                {
                    var Feedback = _context.Feedback_Staff_TransactionDMO.Where(a => a.HRME_Id == Convert.ToInt64(checkamcstid1.FirstOrDefault().Emp_Code) && a.MI_Id == data.MI_Id && a.FSSTR_FeedbackDate.Date == CurrentDate.Date && a.FMTY_Id == getfeedbacktypeid[0].FMTY_Id).ToList();
                    //var Feedback1 = _context.Feedback_Staff_TransactionDMO
                    //    .Where(a => a.HRME_Id == Convert.ToInt64(checkamcstid1.FirstOrDefault().Emp_Code)
                    //    && a.MI_Id == data.MI_Id && a.FSSTR_FeedbackDate.Date == CurrentDate).ToList();

                    DateTime twoPmToday = DateTime.Today.AddHours(15);
                    if (CurrentDate > twoPmToday)
                    {
                        string[] leavecode = new string[2];
                        leavecode[0] = "PH";
                        leavecode[1] = "OH";
                        var leaveyear = _context.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_FromDate <= DateTime.Today && t.HRMLY_ToDate >= DateTime.Today && t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).ToArray().FirstOrDefault();
                        var checkHoliday = (from a in _context.HolidayWorkingDayTypeDMO
                                            join b in _context.FO_Master_HolidayWorkingDay_DatesDMO
                                            on a.MI_Id equals b.MI_Id
                                            where leavecode.Contains(a.FOHTWD_HolidayWDTypeFlag)
                                                  && a.FOHWDT_ActiveFlg
                                                  && b.HRMLY_Id == leaveyear.HRMLY_Id
                                                  && b.FOHWDT_Id == a.FOHWDT_Id
                                                  && b.FOMHWDD_FromDate == DateTime.Today
                                                  && b.FOMHWDD_Name != "Sunday"
                                                  && b.FOMHWD_ActiveFlg
                                            select new
                                            {
                                                b.FOMHWDD_Name,
                                                a.FOHTWD_HolidayWDTypeFlag,
                                                b.FOMHWDD_FromDate
                                            })
                           .OrderBy(x => x.FOMHWDD_FromDate)
                           .ToList();

                        if (checkHoliday.Count <= 0)
                        {
                            if (Feedback.Count > 0)
                            {
                                data.Given = "Given";
                            }
                            else
                            {
                                data.Notgiven = "Notgiven";
                                //QUESTIONS
                                var typelist = _context.FeedBackMasterTypeDMO.Where(a =>  a.FMTY_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false && a.FMTY_FeedbackTypeName==data.FMTY_FeedbackTypeName).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                                data.typelist = typelist.ToArray();

                                data.questionlist = _context.Feedback_Master_QuestionsDMO.Where(a =>  a.FMQE_ActiveFlag == true).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();

                                data.mappedquestiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                               from b in _context.Feedback_Master_QuestionsDMO
                                                               from c in _context.Feedback_Type_QuestionsDMO
                                                               where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.FMTY_ActiveFlag == true && b.FMQE_ActiveFlag == true
                                                               && c.FMTQ_ActiveFlag == true &&  b.FMQE_ActiveFlag == true && a.FMTY_ActiveFlag == true
                                                               && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false && a.FMTY_FeedbackTypeName== data.FMTY_FeedbackTypeName)
                                                               select new FeedbackSchoolGeneralTransactionDTO
                                                               {
                                                                   FMTY_Id = c.FMTY_Id,
                                                                   FMQE_Id = c.FMQE_Id,
                                                                   FMQE_FeedbackQuestions = b.FMQE_FeedbackQuestions,
                                                                   FMTQ_TQOrder = c.FMTQ_TQOrder,
                                                                   FMQE_ManualEntryFlg = b.FMQE_ManualEntryFlg
                                                               }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();

                                if (typelist.FirstOrDefault().FMTY_QuestionwiseOptionFlg == false)
                                {
                                    data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                                 from b in _context.Feedback_Master_OptionsDMO
                                                                 from c in _context.Feedback_Type_OptionsDMO
                                                                 where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                                                 && a.FMTY_ActiveFlag == true &&   b.FMOP_ActiveFlag == true && c.FMTO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false && a.FMTY_FeedbackTypeName== data.FMTY_FeedbackTypeName)
                                                                 select new FeedbackSchoolGeneralTransactionDTO
                                                                 {
                                                                     FMTY_Id = c.FMTY_Id,
                                                                     FMOP_Id = c.FMOP_Id,
                                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                                     FMTO_TOOrder = c.FMTO_TQOrder
                                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                                }
                                else
                                {
                                 data.mappedoptiondeetails = (from a in _context.FeedBackMasterTypeDMO
                                                                 from b in _context.Feedback_Master_OptionsDMO
                                                                 from c in _context.Feedback_Type_QuestionsDMO
                                                                 from d in _context.Feedback_Type_Questions_OptionsDMO
                                                                 where (d.FMTQ_Id == c.FMTQ_Id && a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == d.FMOP_Id && a.FMTY_ActiveFlag == true
                                                                 && b.FMOP_ActiveFlag == true && a.FMTY_ActiveFlag == true && b.FMOP_ActiveFlag == true
                                                                 && d.FMTQO_ActiveFlag == true && a.FMTY_StakeHolderFlag == data.Flag && a.FMTY_SubjectSpecificFlag == false
                                                                 && a.FMTY_FeedbackTypeName== data.FMTY_FeedbackTypeName && c.FMTQ_ActiveFlag == true)
                                                                 select new FeedbackTransactionDTO
                                                                 {
                                                                     FMTY_Id = c.FMTY_Id,
                                                                     FMOP_Id = d.FMOP_Id,
                                                                     FMQE_Id = c.FMQE_Id,
                                                                     FMOP_FeedbackOptions = b.FMOP_FeedbackOptions,
                                                                     FMTO_TOOrder = d.FMTQO_TQOOrder
                                                                 }).Distinct().OrderBy(a => a.FMTO_TOOrder).ToArray();
                                }

                            }
                        }
                    }
                }
              

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackSchoolGeneralTransactionDTO savecanteen(FeedbackSchoolGeneralTransactionDTO data)
        {
            try
            {
                var check_rolenameget = _context.MasterRoleType.Where(a => a.IVRMRT_Id == data.roleId).ToList();
                if (data.Flag=="canteen")
                {
                    var hrme_id = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();
                    data.HRME_Id = hrme_id.FirstOrDefault().Emp_Code;

                    if (data.temp.Count() > 0)
                    {
                        for (int k = 0; k < data.temp.Count(); k++)
                        {
                            for (int j = 0; j < data.temp[k].ques.Count(); j++)
                            {
                                Feedback_Staff_TransactionDMO staff = new Feedback_Staff_TransactionDMO();
                                if (data.temp[k].ques[j].manualflg == true)
                                {
                                    staff.FSTTR_FeedBack = data.temp[k].ques[j].name;
                                }
                                else
                                {
                                    staff.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                    if (data.temp[k].ques[j].message == " ")
                                    {
                                        staff.FSTTR_FeedBack = " ";
                                    }
                                    else
                                    {
                                        staff.FSTTR_FeedBack = data.temp[k].ques[j].message;
                                    }
                                }
                                staff.MI_Id = data.MI_Id;
                                staff.HRME_Id = data.HRME_Id;
                                staff.FSSTR_FeedbackDate = DateTime.Now;
                                staff.FMTY_Id = data.temp[k].FMTY_Id;
                                staff.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                staff.FSTTR_ActiveFlag = true;
                                staff.FSTTR_CreatedBy = data.userid;
                                staff.FSTTR_UpdatedBy = data.userid;
                                staff.CreatedDate = DateTime.Now;
                                staff.UpdatedDate = DateTime.Now;
                                _context.Add(staff);
                            }
                        }

                        if (data.temp.Count() > 0)
                        {
                            int i = _context.SaveChanges();
                            if (i > 0)
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
                else {
                    var hrme_id = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();
                    data.HRME_Id = hrme_id.FirstOrDefault().Emp_Code;

                    if (data.temp.Count() > 0)
                    {
                        for (int k = 0; k < data.temp.Count(); k++)
                        {
                            for (int j = 0; j < data.temp[k].ques.Count(); j++)
                            {
                                Feedback_Staff_TransactionDMO staff = new Feedback_Staff_TransactionDMO();
                                if (data.temp[k].ques[j].manualflg == true)
                                {
                                    staff.FSTTR_FeedBack = data.temp[k].ques[j].name;
                                }
                                else
                                {
                                    staff.FMOP_Id = Convert.ToInt64(data.temp[k].ques[j].name);
                                    if ( data.temp[k].ques[j].message==null)
                                    {
                                        staff.FSTTR_FeedBack = " ";
                                    }
                                    else
                                    {
                                        staff.FSTTR_FeedBack = data.temp[k].ques[j].message;
                                    }
                                   
                                }
                                staff.MI_Id = data.MI_Id;
                                staff.HRME_Id = data.HRME_Id;
                                staff.FSSTR_FeedbackDate = DateTime.Now;
                                staff.FMTY_Id = data.temp[k].FMTY_Id;
                                staff.FMQE_Id = data.temp[k].ques[j].FMQE_Id;
                                staff.FSTTR_ActiveFlag = true;
                                staff.FSTTR_CreatedBy = data.userid;
                                staff.FSTTR_UpdatedBy = data.userid;
                                staff.CreatedDate = DateTime.Now;
                                staff.UpdatedDate = DateTime.Now;
                                _context.Add(staff);
                            }
                        }

                        if (data.temp.Count() > 0)
                        {
                            int i = _context.SaveChanges();
                            if (i > 0)
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

                   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
                _log.LogInformation("College Student Feedback Save Parents :" + ex.Message);
            }
            return data;
        }
    }
}
