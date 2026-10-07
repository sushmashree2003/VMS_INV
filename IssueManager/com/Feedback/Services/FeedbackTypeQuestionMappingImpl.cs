using DataAccessMsSqlServerProvider.FeedBack;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DataAccessMsSqlServerProvider.FeedBack;
using DomainModel.Model.FeedBack;
using DomainModel.Model.NAAC.FeedBack;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;

namespace NaacServiceHub.FeedBack.Services
{
    public class FeedbackTypeQuestionMappingImpl : Interface.FeedbackTypeQuestionMappingInterface
    {
        public FeedBackContext _context;
        public DomainModelMsSqlServerContext _domainContext;
        public FeedbackTypeQuestionMappingImpl(FeedBackContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _domainContext = db;
        }
        public FeedbackTypeQuestionMappingDTO getdetails(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                data.feedbacktype = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.getdetails = (from a in _context.Feedback_Type_QuestionsDMO
                                   from b in _context.FeedBackMasterTypeDMO
                                   from c in _context.Feedback_Master_QuestionsDMO
                                   where (a.FMTY_Id == b.FMTY_Id && c.FMQE_Id == a.FMQE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                   && b.FMTY_ActiveFlag == true && c.FMQE_ActiveFlag == true)
                                   select new FeedbackTypeQuestionMappingDTO
                                   {
                                       FMTY_Id = a.FMTY_Id,
                                       FMQE_Id = a.FMQE_Id,
                                       FMQE_FeedbackQuestions = c.FMQE_FeedbackQuestions,
                                       FMTY_FeedbackTypeName = b.FMTY_FeedbackTypeName,
                                       FMTQ_TQOrder = a.FMTQ_TQOrder,
                                       FMTQ_Id = a.FMTQ_Id,
                                       FMTQ_ActiveFlag = a.FMTQ_ActiveFlag,
                                       FMTY_QuestionwiseOptionFlg = b.FMTY_QuestionwiseOptionFlg
                                   }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO onchnagetype(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                var checkquestionwiseflag = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                var getmappedids = (from a in _context.FeedBackMasterTypeDMO
                                    from b in _context.Feedback_Master_QuestionsDMO
                                    from c in _context.Feedback_Type_QuestionsDMO
                                    where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                    && c.FMTY_Id == data.FMTY_Id && b.FMQE_ActiveFlag == true)
                                    select new FeedbackTypeQuestionMappingDTO
                                    {
                                        FMQE_Id = c.FMQE_Id

                                    }).Distinct().ToList();

                for (int k = 0; k < getmappedids.Count(); k++)
                {
                    ids.Add(getmappedids[k].FMQE_Id);
                }

                data.feedbackquestions = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id
                 && a.FMQE_ActiveFlag == true && !ids.Contains(a.FMQE_Id)).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();

                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // When Feedback Type Not A Question Wise Options
        public FeedbackTypeQuestionMappingDTO savedata(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                if (data.FeedbackTypeQuestionMappingTempDTO.Count() > 0)
                {
                    int rowcount = _context.Feedback_Type_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    for (int k = 0; k < data.FeedbackTypeQuestionMappingTempDTO.Count(); k++)
                    {
                        rowcount = rowcount + 1;

                        Feedback_Type_QuestionsDMO dmo = new Feedback_Type_QuestionsDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.FMTY_Id = data.FMTY_Id;
                        dmo.FMQE_Id = data.FeedbackTypeQuestionMappingTempDTO[k].FMQE_Id;
                        dmo.FMTQ_TQOrder = rowcount;
                        dmo.FMTQ_ActiveFlag = true;
                        dmo.FMTQ_CreatedBy = data.userid;
                        dmo.FMTQ_UpdatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        _context.Add(dmo);
                    }
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        Feedbacknotification(data, "Feedback Uploaded in VMS Kindly Update", "Feedback Uploaded in VMS Kindly Update", data.MI_Id,data.FMTY_FeedbackTypeName);
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
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO activedeactive(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }


                if (data.FMTQ_Id > 0 && data.mappeddetailscount == "continue")
                {
                    var result = _context.Feedback_Type_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTQ_Id == data.FMTQ_Id);
                    if (result.FMTQ_ActiveFlag == false)
                    {
                        result.FMTQ_ActiveFlag = true;
                    }
                    else
                    {
                        result.FMTQ_ActiveFlag = false;
                    }
                    result.FMTQ_UpdatedBy = data.userid;
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
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
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO getorder(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                if (data.FeedbackTypeQuestionMappingTemporderDTO.Count() > 0)
                {
                    int rowcount = 0;
                    for (int k = 0; k < data.FeedbackTypeQuestionMappingTemporderDTO.Count(); k++)
                    {
                        rowcount = rowcount + 1;
                        var result = _context.Feedback_Type_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTQ_Id == data.FeedbackTypeQuestionMappingTemporderDTO[k].FMTQ_Id);
                        result.FMTQ_TQOrder = rowcount;
                        result.UpdatedDate = DateTime.Now;
                        result.FMTQ_UpdatedBy = data.userid;
                        _context.Update(result);
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
            }
            return data;
        }

        // When Feedback Type Is A Question Wise Options
        public FeedbackTypeQuestionMappingDTO onchangequestion(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                List<long> optionsid = new List<long>();

                var getoptions = (from a in _context.Feedback_Type_QuestionsDMO
                                  from b in _context.Feedback_Master_QuestionsDMO
                                  from c in _context.FeedBackMasterTypeDMO
                                  from d in _context.Feedback_Type_Questions_OptionsDMO
                                  from e in _context.Feedback_Master_OptionsDMO
                                  where (a.FMQE_Id == b.FMQE_Id && a.FMTY_Id == c.FMTY_Id && a.FMTQ_Id == d.FMTQ_Id && d.FMOP_Id == e.FMOP_Id
                                  && a.FMQE_Id == data.FMQE_Id && a.FMTY_Id == data.FMTY_Id && a.MI_Id == data.MI_Id)
                                  select d.FMOP_Id).Distinct().ToArray();


                foreach (var c in getoptions)
                {
                    optionsid.Add(c);
                }

                data.getoptions = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && !optionsid.Contains(a.FMOP_Id)
                && a.FMOP_ActiveFlag == true).OrderBy(a => a.FMOP_FOOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO getquestionwiseoption(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                data.getquestionsoptions = (from a in _context.Feedback_Type_QuestionsDMO
                                            from b in _context.Feedback_Master_QuestionsDMO
                                            from c in _context.FeedBackMasterTypeDMO
                                            from d in _context.Feedback_Type_Questions_OptionsDMO
                                            from e in _context.Feedback_Master_OptionsDMO
                                            where (a.FMQE_Id == b.FMQE_Id && a.FMTY_Id == c.FMTY_Id && a.FMTQ_Id == d.FMTQ_Id && d.FMOP_Id == e.FMOP_Id
                                             && a.FMQE_Id == data.FMQE_Id && a.FMTY_Id == data.FMTY_Id && a.MI_Id == data.MI_Id)
                                            select new FeedbackTypeQuestionMappingDTO
                                            {
                                                FMOP_Id = d.FMOP_Id,
                                                FMOP_OptionName = e.FMOP_FeedbackOptions,
                                                FMTQO_Id = d.FMTQO_Id,
                                                FMTQO_TQOOrder = d.FMTQO_TQOOrder,
                                                FMOP_FeedbackORemarks = e.FMOP_FeedbackORemarks,
                                                FMTQO_ActiveFlag = d.FMTQO_ActiveFlag
                                            }).Distinct().OrderBy(a => a.FMTQO_TQOOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public FeedbackTypeQuestionMappingDTO savedatanew(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                if (data.temp_question.Length > 0)
                {
                    int rowcount = _context.Feedback_Type_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    int rowcountnew = (from b in _context.Feedback_Type_Questions_OptionsDMO
                                       from cd in _context.Feedback_Type_QuestionsDMO
                                       where (b.FMTQ_Id == cd.FMTQ_Id && cd.FMTY_Id == data.FMTY_Id && cd.MI_Id == data.MI_Id)
                                       select b).Count();

                    foreach (var c in data.temp_question)
                    {
                        rowcount = rowcount + 1;

                        Feedback_Type_QuestionsDMO dmo = new Feedback_Type_QuestionsDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.FMTY_Id = c.FMTY_Id;
                        dmo.FMQE_Id = c.FMQE_Id;
                        dmo.FMTQ_TQOrder = rowcount;
                        dmo.FMTQ_ActiveFlag = true;
                        dmo.FMTQ_CreatedBy = data.userid;
                        dmo.FMTQ_UpdatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        _context.Add(dmo);

                        if (!c.FMQE_ManualEntryFlg)
                        {
                            foreach (var d in c.optiondetails)
                            {
                                rowcountnew += 1;

                                Feedback_Type_Questions_OptionsDMO dmooptions = new Feedback_Type_Questions_OptionsDMO();

                                dmooptions.FMTQ_Id = dmo.FMTQ_Id;
                                dmooptions.FMOP_Id = d.FMOP_Id;
                                dmooptions.FMTQO_TQOOrder = rowcountnew;
                                dmooptions.FMTQO_ActiveFlag = true;
                                dmooptions.FMTQO_CreatedBy = data.userid;
                                dmooptions.FMTQO_UpdatedBy = data.userid;
                                dmooptions.CreatedDate = DateTime.UtcNow;
                                dmooptions.UpdatedDate = DateTime.UtcNow;
                                _context.Add(dmooptions);
                            }
                        }
                    }

                    var k = _context.SaveChanges();
                    if (k > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO deactiveoption(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                if (data.FMTQO_Id > 0 && data.mappeddetailscount == "continue")
                {
                    var result = _context.Feedback_Type_Questions_OptionsDMO.Single(a => a.FMTQO_Id == data.FMTQO_Id);
                    if (result.FMTQO_ActiveFlag == true)
                    {
                        result.FMTQO_ActiveFlag = false;
                    }
                    else
                    {
                        result.FMTQO_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.UtcNow;
                    result.FMTQO_UpdatedBy = data.userid;
                    _context.Update(result);

                    var k = _context.SaveChanges();
                    if (k > 0)
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
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO getordernew(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                if (data.temp_question_order_TemporderDTO.Count() > 0)
                {
                    int rowcount = 0;

                    foreach (var c in data.temp_question_order_TemporderDTO)
                    {
                        rowcount = rowcount + 1;
                        var result = _context.Feedback_Type_Questions_OptionsDMO.Single(a => a.FMTQO_Id == c.FMTQO_Id);
                        result.FMTQO_TQOOrder = rowcount;
                        result.UpdatedDate = DateTime.Now;
                        result.FMTQO_UpdatedBy = data.userid;
                        _context.Update(result);
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
            }
            return data;
        }

        // Type Option Mapping 
        public FeedbackTypeOptionMappingDTO optiongetdetails(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                data.feedbacktype = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true
               ).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();
                //&& b.FMTY_QuestionwiseOptionFlg == true
                data.getdetails = (from a in _context.Feedback_Type_OptionsDMO
                                   from b in _context.FeedBackMasterTypeDMO
                                   from c in _context.Feedback_Master_OptionsDMO
                                   where (a.FMTY_Id == b.FMTY_Id && c.FMOP_Id == a.FMOP_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                   && b.FMTY_ActiveFlag == true && c.FMOP_ActiveFlag == true)
                                   select new FeedbackTypeOptionMappingDTO
                                   {
                                       FMTY_Id = a.FMTY_Id,
                                       FMOP_Id = a.FMOP_Id,
                                       FMOP_FeedbackOptions = c.FMOP_FeedbackOptions,
                                       FMTY_FeedbackTypeName = b.FMTY_FeedbackTypeName,
                                       FMTO_TQOrder = a.FMTO_TQOrder,
                                       FMTO_Id = a.FMTO_Id,
                                       FMTO_ActiveFlag = a.FMTO_ActiveFlag
                                   }).Distinct().OrderBy(a => a.FMTO_TQOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeOptionMappingDTO optiononchnagetype(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                var getmappedids = (from a in _context.FeedBackMasterTypeDMO
                                    from b in _context.Feedback_Master_OptionsDMO
                                    from c in _context.Feedback_Type_OptionsDMO
                                    where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                    && c.FMTY_Id == data.FMTY_Id && b.FMOP_ActiveFlag == true)
                                    select new FeedbackTypeOptionMappingDTO
                                    {
                                        FMOP_Id = c.FMOP_Id

                                    }).Distinct().ToList();

                for (int k = 0; k < getmappedids.Count(); k++)
                {
                    ids.Add(getmappedids[k].FMOP_Id);
                }

                data.feedbackoptions = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id
                 && a.FMOP_ActiveFlag == true && !ids.Contains(a.FMOP_Id)).Distinct().OrderBy(a => a.FMOP_FOOrder).ToArray();

                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeOptionMappingDTO optionsavedata(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                if (data.FeedbackTypeOptionMappingTempDTO.Count() > 0)
                {
                    int rowcount = _context.Feedback_Type_OptionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    for (int k = 0; k < data.FeedbackTypeOptionMappingTempDTO.Count(); k++)
                    {
                        rowcount = rowcount + 1;

                        Feedback_Type_OptionsDMO dmo = new Feedback_Type_OptionsDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.FMTY_Id = data.FMTY_Id;
                        dmo.FMOP_Id = data.FeedbackTypeOptionMappingTempDTO[k].FMOP_Id;
                        dmo.FMTO_TQOrder = rowcount;
                        dmo.FMTO_ActiveFlag = true;
                        dmo.FMTO_CreatedBy = data.userid;
                        dmo.FMTO_UpdatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        _context.Add(dmo);
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
            }
            return data;
        }
        public FeedbackTypeOptionMappingDTO optionactivedeactive(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                if (data.FMTO_Id > 0 && data.mappeddetailscount == "continue")
                {
                    var result = _context.Feedback_Type_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTO_Id == data.FMTO_Id);
                    if (result.FMTO_ActiveFlag == false)
                    {
                        result.FMTO_ActiveFlag = true;
                    }
                    else
                    {
                        result.FMTO_ActiveFlag = false;
                    }
                    result.FMTO_UpdatedBy = data.userid;
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
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
            }
            return data;
        }
        public FeedbackTypeOptionMappingDTO optiongetorder(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                if (data.FeedbackTypeOptionMappingTemporderDTO.Count() > 0)
                {
                    int rowcount = 0;
                    for (int k = 0; k < data.FeedbackTypeOptionMappingTemporderDTO.Count(); k++)
                    {
                        rowcount = rowcount + 1;
                        var result = _context.Feedback_Type_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTO_Id == data.FeedbackTypeOptionMappingTemporderDTO[k].FMTO_Id);
                        result.FMTO_TQOrder = rowcount;
                        result.UpdatedDate = DateTime.Now;
                        result.FMTO_UpdatedBy = data.userid;
                        _context.Update(result);
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
            }
            return data;
        }

        public void Feedbacknotification(FeedbackTypeQuestionMappingDTO obj,  string subject, string body, long id,string feedbackname)
        {
            try
            {

                var receivarmailid = _context.MasterEmployee.Where(m => m.HRMD_Id == id && m.HRME_ActiveFlag == true && m.HRME_LeftFlag == true).ToList();
             
               // string mailid = "Vapsregister1@gmail.com"; //"goutamkumar@vapstech.com";

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _domainContext.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("FeedbackNotification", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _domainContext.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _domainContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;

                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }
                    if (receivarmailid != null) {
                        if(receivarmailid.Count>0)
                        {
                            for(int i=0;i< receivarmailid.Count;i++)
                            {
                                message.AddTo(receivarmailid[i].HRME_EmailId);
                            }
                        }
                      
                    }
             

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                      
                        message.HtmlContent = message.HtmlContent.Replace("[FEEDBACKNAME]", obj.FMTY_FeedbackTypeName);
                        message.HtmlContent = message.HtmlContent.Replace("[link]", "https://vapsmanagementsoftware.azurewebsites.net/#/login/");
              
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }
                    if (receivarmailid != null)
                    {
                        if (receivarmailid.Count > 0)
                        {
                            for (int i = 0; i < receivarmailid.Count; i++)
                            {
                                using (var cmd = _domainContext.Database.GetDbConnection().CreateCommand())
                                {
                                    var template1010 = _domainContext.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("FeedbackNotification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                                    var moduleid = _domainContext.Institution_Module.Where(k => template1010.Contains(k.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                                    var modulename = _domainContext.masterModule.Where(j => moduleid.Contains(j.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                    cmd.CommandText = "IVRM_Email_Outgoing_1";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                                    {
                                        Value = receivarmailid[i].HRME_EmailId
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                                    {
                                        Value = Mailmsg
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                                    {
                                        Value = subject
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                                    {
                                        Value = "Staff"
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();
                                    try
                                    {
                                        using (var dataReader = cmd.ExecuteReader())
                                        {
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //return ex.Message;
                                    }
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
        }
    }
}
