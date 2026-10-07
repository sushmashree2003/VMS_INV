using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class OnlineTestCandidateImpl : Interfaces.OnlineTestCandidateInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public OnlineTestCandidateImpl(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        #region  MASTER EXAM
        //MASTER QUESTION
        #region LOAD QUESTION PAPER
        public OnlineTestCandidateDTO loadExamdata(OnlineTestCandidateDTO data)
        {
            try
            {

                //data.positionlist = _VMSContext.HR_Master_PositionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMP_ActiveFlg == true).Distinct().OrderBy(e => e.HRMP_Position).ToArray();

                //var exampaperlist = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                //                     from b in _VMSContext.HR_Master_PositionDMO
                //                     where a.MI_Id == b.MI_Id && a.OTQPTYP_ActiveFlg == true && b.HRMP_ActiveFlg == true && a.MI_Id == b.MI_Id && a.HRMP_Id == b.HRMP_Id && a.HRMP_Id == data.HRMP_Id

                //                     select new OnlineTestCandidateDTO
                //                     {
                //                         OTQPTYP_Id=a.OTQPTYP_Id,
                //                         HRMP_Id = a.HRMP_Id,
                //                         OTQPTYP_QuestionPaperName = a.OTQPTYP_QuestionPaperName,
                //                         OTQPTYP_QuestionPaperDesc = a.OTQPTYP_QuestionPaperDesc,
                //                    //  totalquestion = _VMSContext.OT_Master_OE_QuestionsDMO.Where(w => w.MI_Id == a.MI_Id && w.OTQPTYP_Id == a.OTQPTYP_Id && w.HRMP_Id == data.HRMP_Id && w.OTMOEQ_ActiveFlg == true).ToList().Count(), 
                //             //            maxmarks= _VMSContext.OT_Master_OE_QuestionsDMO.Where(w => w.MI_Id == a.MI_Id && w.OTQPTYP_Id == a.OTQPTYP_Id && w.HRMP_Id == data.HRMP_Id && w.OTMOEQ_ActiveFlg == true).Distinct().ToList().Sum(e=>e.OTMOEQ_Marks)
                //                     }).Distinct().ToList();

                //*********************************** User Employee Mapped List ********************************// 



                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_QUESTIONPAPERFOREXAM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@USERNAME", SqlDbType.Char)
                    {
                        Value = data.User_Name
                    });
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getallexamdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                // data.getallexamdetails = exampaperlist.ToArray();
                data.gettodaysexamdetails = data.getallexamdetails;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region GET  QUESTIONS AND OPTIONS
        public OnlineTestCandidateDTO getQuestion(OnlineTestCandidateDTO data)
        {
            try
            {

                List<OnlineTestCandidateDTO> getexamquestionlist = new List<OnlineTestCandidateDTO>();

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_OT_Exam_RandomQns";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMP_Id",
            SqlDbType.VarChar)
                    {
                        Value = data.HRMP_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@OTQPTYP_Id",
          SqlDbType.VarChar)
                    {
                        Value = data.OTQPTYP_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                getexamquestionlist.Add(new OnlineTestCandidateDTO
                                {
                                    OTQPTYP_Id = Convert.ToInt64(dataReader["OTQPTYP_Id"]),
                                    OTQPTYP_QuestionPaperName = Convert.ToString(dataReader["OTQPTYP_QuestionPaperName"]),
                                    OTQPTYP_QuestionPaperDesc = Convert.ToString(dataReader["OTQPTYP_QuestionPaperDesc"]),
                                    OTMOEQ_Id = Convert.ToInt64(dataReader["OTMOEQ_Id"]),
                                    OTMOEQ_Question = Convert.ToString(dataReader["OTMOEQ_Question"]),
                                    OTMOEQ_SubjectiveFlg = Convert.ToBoolean(dataReader["OTMOEQ_SubjectiveFlg"]),
                                    OTMOEQ_AnswerUploadFlag = Convert.ToBoolean(dataReader["OTMOEQ_AnswerUploadFlag"]),
                                    OTMOEQ_Marks = Convert.ToDecimal(dataReader["OTMOEQ_Marks"]),

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //var getexamquestionlist = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                //                           from b in _VMSContext.OT_Master_OE_QuestionsDMO
                //                           where (a.OTQPTYP_Id == b.OTQPTYP_Id && a.HRMP_Id == b.HRMP_Id && a.OTQPTYP_ActiveFlg == true
                //                           && a.HRMP_Id == data.HRMP_Id && b.OTQPTYP_Id == data.OTQPTYP_Id && b.OTMOEQ_ActiveFlg==true)
                //                           select new OnlineTestCandidateDTO
                //                           {
                //                               OTQPTYP_Id = a.OTQPTYP_Id,
                //                               OTQPTYP_QuestionPaperName = a.OTQPTYP_QuestionPaperName,
                //                               OTQPTYP_QuestionPaperDesc = a.OTQPTYP_QuestionPaperDesc,
                //                               OTMOEQ_Id = b.OTMOEQ_Id,
                //                               OTMOEQ_Question = b.OTMOEQ_Question,
                //                               OTMOEQ_SubjectiveFlg = b.OTMOEQ_SubjectiveFlg,
                //                               OTMOEQ_AnswerUploadFlag = b.OTMOEQ_AnswerUploadFlag
                //                           }).Distinct().ToList();

                List<long> questionids = new List<long>();

                foreach (var c in getexamquestionlist)
                {
                    questionids.Add(c.OTMOEQ_Id);
                }

                data.getexamquestionlist = getexamquestionlist.ToArray();

                data.getquestionoptionlist = (from a in _VMSContext.OT_Master_OE_QuestionsDMO
                                              from b in _VMSContext.OT_Master_OE_QNS_OptionsDMO
                                              where (a.OTMOEQ_Id == b.OTMOEQ_Id && a.OTMOEQ_ActiveFlg == true && b.OTMOEQOA_ActiveFlg == true
                                               && questionids.Contains(b.OTMOEQ_Id) && a.MI_Id == data.MI_Id && a.HRMP_Id == data.HRMP_Id)
                                              select new OnlineTestCandidateDTO
                                              {
                                                  OTMOEQ_Id = a.OTMOEQ_Id,
                                                  OTMOEQOA_Id = b.OTMOEQOA_Id,
                                                  OTMOEQOA_Option = b.OTMOEQOA_Option,
                                                  OTMOEQOA_OptionCode = b.OTMOEQOA_OptionCode,
                                                  OTMOEQOA_AnswerFlag = b.OTMOEQOA_AnswerFlag,
                                              }).Distinct().OrderBy(a => a.OTMOEQ_Id).OrderBy(a => a.OTMOEQOA_OptionCode).ToArray();




                data.getquestiondoclist = (from a in _VMSContext.OT_Master_OE_QuestionsDMO
                                           from b in _VMSContext.OT_Master_OE_Questions_FilesDMO
                                           where (a.OTMOEQ_Id == b.OTMOEQ_Id && a.OTMOEQ_ActiveFlg == true && b.OTMOEQF_ActiveFlag == true && a.MI_Id == data.MI_Id && a.OTQPTYP_Id == data.OTQPTYP_Id && questionids.Contains(b.OTMOEQ_Id))
                                           select new OnlineTestCandidateDTO
                                           {
                                               OTMOEQ_Id = a.OTMOEQ_Id,
                                               OTMOEQF_FileName = b.OTMOEQF_FileName,
                                               OTMOEQF_FilePath = b.OTMOEQF_FilePath
                                           }).Distinct().ToArray();


                data.getoptionwisefiles = (from a in _VMSContext.OT_Master_OE_QuestionsDMO
                                           from b in _VMSContext.OT_Master_OE_QNS_OptionsDMO
                                           from c in _VMSContext.OT_Master_OE_QNS_Options_FilesDMO
                                           where (a.OTMOEQ_Id == b.OTMOEQ_Id && b.OTMOEQOA_Id == c.OTMOEQOA_Id && a.OTMOEQ_ActiveFlg == true && b.OTMOEQOA_ActiveFlg == true && c.OTMOEQOAF_ActiveFlag == true && a.MI_Id == data.MI_Id && a.OTQPTYP_Id == data.OTQPTYP_Id && questionids.Contains(b.OTMOEQ_Id))
                                           select new OnlineTestCandidateDTO
                                           {
                                               OTMOEQ_Id = a.OTMOEQ_Id,
                                               OTMOEQOA_Id = b.OTMOEQOA_Id,
                                               OTMOEQOAF_FileName = c.OTMOEQOAF_FileName,
                                               OTMOEQOAF_FilePath = c.OTMOEQOAF_FilePath
                                           }).Distinct().ToArray();
            }

            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        #endregion

        #region Save Or Update ANSWER
        public OnlineTestCandidateDTO Saveanswer(OnlineTestCandidateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkexamresult = _VMSContext.OT_Candidates_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.HRCD_Id == data.HRCD_Id && a.OTCANDEX_Id == data.OTCANDEX_Id && a.OTQPTYP_Id == data.OTQPTYP_Id).ToList();

                if (checkexamresult.Count > 0)
                {
                    var checkresult = _VMSContext.OT_Candidates_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.HRCD_Id == data.HRCD_Id && a.OTCANDEX_Id == data.OTCANDEX_Id && a.OTQPTYP_Id == data.OTQPTYP_Id);

                    checkresult.OTCANDEX_TotalTime = data.OTCANDEX_TotalTime;
                    checkresult.OTCANDEX_UpdatedBy = data.Userid;
                    checkresult.OTCANDEX_UpdatedDate = indiantime0;
                    _VMSContext.Update(checkresult);

                    if (data.OTMOEQ_SubjectiveFlg == false)
                    {

                        var checkquestionresult = _VMSContext.OT_Candidates_Exam_AnswerDMO.Where(a => a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id
                        && a.OTMOEQ_Id == data.saveanswerlsttemp[0].OTMOEQ_Id).ToList();

                        if (checkquestionresult.Count > 0)
                        {
                            var checkquestionansresult = _VMSContext.OT_Candidates_Exam_AnswerDMO.Single(a => a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id
                                && a.OTMOEQ_Id == data.saveanswerlsttemp[0].OTMOEQ_Id);

                            checkquestionansresult.OTMOEQOA_Id = data.saveanswerlsttemp[0].OTMOEQOA_Id;
                            checkquestionansresult.OTCANDEXANS_CorrectAnsFlg = data.saveanswerlsttemp[0].OTMOEQOA_AnswerFlag;
                            checkquestionansresult.OTCANDEXANS_UpdatedBy = data.Userid;
                            checkquestionansresult.OTCANDEXANS_UpdatedDate = indiantime0;
                            _VMSContext.Update(checkquestionansresult);

                        }
                        else
                        {
                            foreach (var c in data.saveanswerlsttemp)
                            {
                                OT_Candidates_Exam_AnswerDMO obj1 = new OT_Candidates_Exam_AnswerDMO();

                                obj1.OTCANDEX_Id = checkexamresult.FirstOrDefault().OTCANDEX_Id;
                                obj1.OTMOEQ_Id = c.OTMOEQ_Id;
                                obj1.OTMOEQOA_Id = c.OTMOEQOA_Id;
                                obj1.OTCANDEXANS_CorrectAnsFlg = c.OTMOEQOA_AnswerFlag;
                                obj1.OTCANDEXANS_ActiveFlg = true;
                                obj1.OTCANDEXANS_CreatedBy = data.Userid;
                                obj1.OTCANDEXANS_UpdatedBy = data.Userid;
                                obj1.OTCANDEXANS_CreatedDate = indiantime0;
                                obj1.OTCANDEXANS_UpdatedDate = indiantime0;
                                _VMSContext.Add(obj1);
                            }
                        }
                    }
                    else
                    {
                        var checkquestionsubjectiveresult = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id
                        && a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id).ToList();

                        if (checkquestionsubjectiveresult.Count > 0)
                        {
                            var quessubjectiveresult = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Single(a => a.OTMOEQ_Id == data.OTMOEQ_Id
                            && a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id);

                            quessubjectiveresult.OTCANDEXSANS_Answer = data.OTCANDEXSANS_Answer; ;
                            quessubjectiveresult.OTCANDEXSANS_UpdatedDate = indiantime0;
                            quessubjectiveresult.OTCANDEXSANS_UpdatedBy = data.Userid;
                            _VMSContext.Update(quessubjectiveresult);

                        }
                        else
                        {
                            OT_Candidates_Exam_SubjectiveAnswerDMO obj2 = new OT_Candidates_Exam_SubjectiveAnswerDMO();
                            obj2.OTCANDEX_Id = checkexamresult.FirstOrDefault().OTCANDEX_Id;
                            obj2.OTCANDEXSANS_Answer = data.OTCANDEXSANS_Answer;
                            obj2.OTMOEQ_Id = data.OTMOEQ_Id;
                            obj2.OTCANDEXSANS_ActiveFlg = true;
                            obj2.OTCANDEXSANS_CreatedDate = indiantime0;
                            obj2.OTCANDEXSANS_CreatedBy = data.Userid;
                            obj2.OTCANDEXSANS_UpdatedDate = indiantime0;
                            obj2.OTCANDEXSANS_UpdatedBy = data.Userid;
                            _VMSContext.Add(obj2);
                        }

                        var listfiles = _VMSContext.OT_Candidates_Exam_AnswersheetDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id && a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id).ToList();

                        if (listfiles.Count > 0)
                        {
                            foreach (var item in listfiles)
                            {
                                _VMSContext.Remove(item);
                            }
                        }

                        if (data.tempoptionsfiles != null && data.tempoptionsfiles.Length > 0)
                        {

                            foreach (var itemf in data.tempoptionsfiles)
                            {
                                OT_Candidates_Exam_AnswersheetDMO objfiles = new OT_Candidates_Exam_AnswersheetDMO();
                                objfiles.OTMOEQ_Id = data.OTMOEQ_Id;
                                objfiles.OTCANDEX_Id = checkexamresult.FirstOrDefault().OTCANDEX_Id;
                                objfiles.OTCANDEXAS_AnswerSheetFile = itemf.OTCANDEXAS_AnswerSheetFile;
                                objfiles.OTCANDEXAS_AnswerSheetPath = itemf.OTCANDEXAS_AnswerSheetPath;
                                objfiles.OTCANDEXAS_UpdatedDate = indiantime0;
                                objfiles.OTCANDEXAS_CreatedDate = indiantime0;
                                objfiles.OTCANDEXAS_CreatedBy = data.Userid;
                                objfiles.OTCANDEXAS_UpdatedBy = data.Userid;
                                objfiles.OTCANDEXAS_ActiveFlg = true;

                                _VMSContext.Add(objfiles);
                            }



                        }


                    }
                    var i = _VMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Update";
                    }
                    else
                    {
                        data.message = "Failed";
                    }
                }
                else
                {
                    OT_Candidates_ExamDMO cobj = new OT_Candidates_ExamDMO();

                    cobj.MI_Id = data.MI_Id;
                    cobj.HRCD_Id = data.HRCD_Id;
                    cobj.OTQPTYP_Id = data.OTQPTYP_Id;
                    cobj.OTCANDEX_TotalTime = data.OTCANDEX_TotalTime;
                    cobj.OTCANDEX_Date = indiantime0;
                    cobj.OTCANDEX_ActiveFlg = true;
                    cobj.OTCANDEX_CreatedBy = data.Userid;
                    cobj.OTCANDEX_UpdatedBy = data.Userid;
                    cobj.OTCANDEX_CreatedDate = indiantime0;
                    cobj.OTCANDEX_UpdatedDate = indiantime0;

                    _VMSContext.Add(cobj);

                    if (data.OTMOEQ_SubjectiveFlg == false)
                    {
                        foreach (var c in data.saveanswerlsttemp)
                        {
                            OT_Candidates_Exam_AnswerDMO obobj = new OT_Candidates_Exam_AnswerDMO();

                            obobj.OTCANDEX_Id = cobj.OTCANDEX_Id;
                            obobj.OTMOEQ_Id = c.OTMOEQ_Id;
                            obobj.OTMOEQOA_Id = c.OTMOEQOA_Id;

                            obobj.OTCANDEXANS_CorrectAnsFlg = c.OTMOEQOA_AnswerFlag;
                            obobj.OTCANDEXANS_ActiveFlg = true;
                            obobj.OTCANDEXANS_CreatedBy = data.Userid;
                            obobj.OTCANDEXANS_UpdatedBy = data.Userid;
                            obobj.OTCANDEXANS_CreatedDate = indiantime0;
                            obobj.OTCANDEXANS_UpdatedDate = indiantime0;
                            _VMSContext.Add(obobj);
                        }
                    }

                    else
                    {
                        OT_Candidates_Exam_SubjectiveAnswerDMO subobj = new OT_Candidates_Exam_SubjectiveAnswerDMO();
                        subobj.OTCANDEX_Id = cobj.OTCANDEX_Id;
                        subobj.OTMOEQ_Id = data.OTMOEQ_Id;
                        subobj.OTCANDEXSANS_Answer = data.OTCANDEXSANS_Answer;
                        subobj.OTCANDEXSANS_ActiveFlg = true;
                        subobj.OTCANDEXSANS_CreatedDate = indiantime0;
                        subobj.OTCANDEXSANS_CreatedBy = data.Userid;
                        subobj.OTCANDEXSANS_UpdatedDate = indiantime0;
                        subobj.OTCANDEXSANS_UpdatedBy = data.Userid;
                        _VMSContext.Add(subobj);
                    }


                    if (data.tempoptionsfiles != null && data.tempoptionsfiles.Length > 0)
                    {

                        foreach (var itemf in data.tempoptionsfiles)
                        {
                            OT_Candidates_Exam_AnswersheetDMO objfiles = new OT_Candidates_Exam_AnswersheetDMO();
                            objfiles.OTMOEQ_Id = data.OTMOEQ_Id;
                            objfiles.OTCANDEX_Id = cobj.OTCANDEX_Id;
                            objfiles.OTCANDEXAS_AnswerSheetFile = itemf.OTCANDEXAS_AnswerSheetFile;
                            objfiles.OTCANDEXAS_AnswerSheetPath = itemf.OTCANDEXAS_AnswerSheetPath;
                            objfiles.OTCANDEXAS_UpdatedDate = indiantime0;
                            objfiles.OTCANDEXAS_CreatedDate = indiantime0;
                            objfiles.OTCANDEXAS_CreatedBy = data.Userid;
                            objfiles.OTCANDEXAS_UpdatedBy = data.Userid;
                            objfiles.OTCANDEXAS_ActiveFlg = true;

                            _VMSContext.Add(objfiles);
                        }



                    }
                    var i = _VMSContext.SaveChanges();
                    if (i > 0)
                    {

                        data.OTCANDEX_Id = cobj.OTCANDEX_Id;
                        data.message = "Saved";
                    }
                    else
                    {
                        data.message = "Failed";
                    }
                }



                List<OnlineTestCandidateDTO> objectivequestions = new List<OnlineTestCandidateDTO>();
                List<OnlineTestCandidateDTO> subjectivequestions = new List<OnlineTestCandidateDTO>();
                List<OnlineTestCandidateDTO> allquestion = new List<OnlineTestCandidateDTO>();

                objectivequestions = (from a in _VMSContext.OT_Master_OE_QuestionsDMO
                                      from c in _VMSContext.OT_Master_OE_QNS_OptionsDMO
                                      from d in _VMSContext.OT_Candidates_ExamDMO
                                      from e in _VMSContext.OT_Candidates_Exam_AnswerDMO
                                      where (a.OTMOEQ_Id == c.OTMOEQ_Id && a.MI_Id == data.MI_Id && e.OTMOEQOA_Id == c.OTMOEQOA_Id
                                      && d.OTCANDEX_Id == e.OTCANDEX_Id && d.HRCD_Id == data.HRCD_Id && a.OTMOEQ_SubjectiveFlg == false && d.OTCANDEX_Id == data.OTCANDEX_Id && a.OTQPTYP_Id == data.OTQPTYP_Id)
                                      select new OnlineTestCandidateDTO
                                      {
                                          OTMOEQ_Id = e.OTMOEQ_Id,
                                          OTMOEQOA_Id = e.OTMOEQOA_Id,
                                          OTMOEQ_Question = a.OTMOEQ_Question,
                                          OTMOEQOA_OptionCode = c.OTMOEQOA_OptionCode,
                                          OTMOEQOA_Option = c.OTMOEQOA_Option,
                                          OTMOEQOA_AnswerFlag = c.OTMOEQOA_AnswerFlag,
                                          OTMOEQ_SubjectiveFlg = a.OTMOEQ_SubjectiveFlg
                                      }).Distinct().ToList();

                subjectivequestions = (from a in _VMSContext.OT_Master_OE_QuestionsDMO
                                       from d in _VMSContext.OT_Candidates_ExamDMO
                                       from e in _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO
                                       where (a.OTMOEQ_Id == e.OTMOEQ_Id && a.MI_Id == data.MI_Id
                                       && d.OTCANDEX_Id == e.OTCANDEX_Id && d.HRCD_Id == data.HRCD_Id && a.OTQPTYP_Id == data.OTQPTYP_Id
                                       && d.OTCANDEX_Id == data.OTCANDEX_Id && a.OTMOEQ_SubjectiveFlg == true)
                                       select new OnlineTestCandidateDTO
                                       {
                                           OTMOEQ_Id = e.OTMOEQ_Id,
                                           OTMOEQ_Question = a.OTMOEQ_Question,
                                           OTMOEQ_SubjectiveFlg = a.OTMOEQ_SubjectiveFlg,
                                           OTCANDEXSANS_Answer = e.OTCANDEXSANS_Answer
                                       }).Distinct().ToList();


                foreach (var c in objectivequestions)
                {
                    allquestion.Add(c);
                }

                foreach (var c in subjectivequestions)
                {
                    allquestion.Add(c);
                }

                data.getsavedanswer = allquestion.ToArray();

            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region SUBMIT PAPER
        public OnlineTestCandidateDTO submitexam(OnlineTestCandidateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //var getquestiondetails = _VMSContext.OT_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.OTMOEQ_Id == data.OTMOEQ_Id).ToList();
                //data.geteditmasterquestion = getquestiondetails.ToArray();

                //data.HRMP_Id = getquestiondetails.FirstOrDefault().HRMP_Id;
                //data.OTQPTYP_Id = getquestiondetails.FirstOrDefault().OTQPTYP_Id;

                //data.getSavedOptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();

                //data.papertypelist = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                //                      from b in _VMSContext.HR_Master_PositionDMO
                //                      where a.MI_Id == b.MI_Id && a.OTQPTYP_ActiveFlg == true && b.HRMP_ActiveFlg == true && a.MI_Id == b.MI_Id && a.HRMP_Id == b.HRMP_Id && a.HRMP_Id == data.HRMP_Id

                //                      select a).Distinct().ToArray();



                if (data.LP_OnlineFinalDetails != null && data.LP_OnlineFinalDetails.Length > 0)
                {
                    var checkexamresult = _VMSContext.OT_Candidates_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.OTQPTYP_Id == data.OTQPTYP_Id
                     && a.OTCANDEX_Id == data.OTCANDEX_Id).ToList();

                    if (checkexamresult.Count > 0)
                    {
                        var checkresult = _VMSContext.OT_Candidates_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.OTCANDEX_Id == data.OTCANDEX_Id
                        && a.OTQPTYP_Id == data.OTQPTYP_Id);

                        checkresult.OTCANDEX_TotalTime = data.OTCANDEX_TotalTime;
                        checkresult.OTCANDEX_UpdatedBy = data.Userid;
                        checkresult.OTCANDEX_UpdatedDate = indiantime0;
                        _VMSContext.Update(checkresult);

                        foreach (var c in data.LP_OnlineFinalDetails)
                        {
                            if (c.OTMOEQ_SubjectiveFlg == false)
                            {
                                var QuizeQuastions = c.QuizeQuastions > 0 ? c.QuizeQuastions : null;

                                var checkquestionresult = _VMSContext.OT_Candidates_Exam_AnswerDMO.Where(a => a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id
                                && a.OTMOEQ_Id == c.OTMOEQ_Id).ToList();

                                if (checkquestionresult.Count > 0)
                                {
                                    var checkquestionansresult = _VMSContext.OT_Candidates_Exam_AnswerDMO.Single(a => a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id && a.OTMOEQ_Id == c.OTMOEQ_Id);

                                    checkquestionansresult.OTMOEQOA_Id = QuizeQuastions;
                                    checkquestionansresult.OTCANDEXANS_CorrectAnsFlg = c.OTMOEQOA_AnswerFlag;
                                    checkquestionansresult.OTCANDEXANS_UpdatedBy = data.Userid;
                                    checkquestionansresult.OTCANDEXANS_UpdatedDate = indiantime0;
                                    _VMSContext.Update(checkquestionansresult);

                                }
                                else
                                {
                                    //if (c.QuizeQuastions > 0)
                                    //{
                                    OT_Candidates_Exam_AnswerDMO obb = new OT_Candidates_Exam_AnswerDMO();

                                    obb.OTCANDEX_Id = checkexamresult.FirstOrDefault().OTCANDEX_Id;
                                    obb.OTMOEQ_Id = c.OTMOEQ_Id;
                                    obb.OTMOEQOA_Id = QuizeQuastions;
                                    obb.OTCANDEXANS_CorrectAnsFlg = c.OTMOEQOA_AnswerFlag;
                                    obb.OTCANDEXANS_ActiveFlg = true;
                                    obb.OTCANDEXANS_CreatedBy = data.Userid;
                                    obb.OTCANDEXANS_UpdatedBy = data.Userid;
                                    obb.OTCANDEXANS_CreatedDate = indiantime0;
                                    obb.OTCANDEXANS_UpdatedDate = indiantime0;
                                    _VMSContext.Add(obb);
                                    //}
                                }
                            }
                            else
                            {
                                var checkquestionsubjectiveresult = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Where(a => a.OTMOEQ_Id == c.OTMOEQ_Id
                                && a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id).ToList();

                                if (checkquestionsubjectiveresult.Count > 0)
                                {
                                    var quessubjectiveresult = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Single(a => a.OTMOEQ_Id == c.OTMOEQ_Id
                                    && a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id);

                                    quessubjectiveresult.OTCANDEXSANS_Answer = c.answer;
                                    quessubjectiveresult.OTCANDEXSANS_UpdatedDate = indiantime0;
                                    quessubjectiveresult.OTCANDEXSANS_UpdatedBy = data.Userid;
                                    _VMSContext.Update(quessubjectiveresult);

                                }
                                else
                                {
                                    OT_Candidates_Exam_SubjectiveAnswerDMO objaa = new OT_Candidates_Exam_SubjectiveAnswerDMO();
                                    objaa.OTCANDEX_Id = checkexamresult.FirstOrDefault().OTCANDEX_Id;
                                    objaa.OTCANDEXSANS_Answer = c.answer;
                                    objaa.OTMOEQ_Id = c.OTMOEQ_Id;
                                    objaa.OTCANDEXSANS_ActiveFlg = true;
                                    objaa.OTCANDEXSANS_CreatedDate = indiantime0;
                                    objaa.OTCANDEXSANS_CreatedBy = data.Userid;
                                    objaa.OTCANDEXSANS_UpdatedDate = indiantime0;
                                    objaa.OTCANDEXSANS_UpdatedBy = data.Userid;
                                    _VMSContext.Add(objaa);
                                }
                            }

                            var listfiles = _VMSContext.OT_Candidates_Exam_AnswersheetDMO.Where(a => a.OTMOEQ_Id == c.OTMOEQ_Id && a.OTCANDEX_Id == data.OTCANDEX_Id).ToList();

                            if (listfiles.Count > 0)
                            {
                                foreach (var item in listfiles)
                                {
                                    _VMSContext.Remove(item);
                                }
                            }


                            if (c.tempoptionsfiles != null && c.tempoptionsfiles.Length > 0)
                            {
                                foreach (var itemf in c.tempoptionsfiles)
                                {
                                    OT_Candidates_Exam_AnswersheetDMO objfiles = new OT_Candidates_Exam_AnswersheetDMO();
                                    objfiles.OTMOEQ_Id = c.OTMOEQ_Id;
                                    objfiles.OTCANDEX_Id = data.OTCANDEX_Id;
                                    objfiles.OTCANDEXAS_AnswerSheetFile = itemf.OTCANDEXAS_AnswerSheetFile;
                                    objfiles.OTCANDEXAS_AnswerSheetPath = itemf.OTCANDEXAS_AnswerSheetPath;
                                    objfiles.OTCANDEXAS_UpdatedDate = indiantime0;
                                    objfiles.OTCANDEXAS_CreatedDate = indiantime0;
                                    objfiles.OTCANDEXAS_CreatedBy = data.Userid;
                                    objfiles.OTCANDEXAS_UpdatedBy = data.Userid;
                                    objfiles.OTCANDEXAS_ActiveFlg = true;

                                    _VMSContext.Add(objfiles);
                                }
                            }

                        }

                        var i = _VMSContext.SaveChanges();
                        if (i > 0)
                        {
                            if (data.OTCANDEX_Id == 0)
                            {
                                var list = _VMSContext.OT_Candidates_ExamDMO.Where(w => w.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id).ToList();
                                if (list.Count > 0)
                                {
                                    data.OTCANDEX_Id = list.FirstOrDefault().OTCANDEX_Id;
                                }

                            }
                            data.message = "Update";
                        }
                        else
                        {
                            data.message = "Failed";
                        }
                    }
                    else
                    {
                        OT_Candidates_ExamDMO oce = new OT_Candidates_ExamDMO();

                        oce.MI_Id = data.MI_Id;
                        oce.HRCD_Id = data.HRCD_Id;
                        oce.OTQPTYP_Id = data.OTQPTYP_Id;
                        oce.OTCANDEX_TotalTime = data.OTCANDEX_TotalTime;
                        oce.OTCANDEX_Date = indiantime0;
                        oce.OTCANDEX_ActiveFlg = true;
                        oce.OTCANDEX_CreatedBy = data.Userid;
                        oce.OTCANDEX_UpdatedBy = data.Userid;
                        oce.OTCANDEX_CreatedDate = indiantime0;
                        oce.OTCANDEX_UpdatedDate = indiantime0;
                        _VMSContext.Add(oce);

                        foreach (var c in data.LP_OnlineFinalDetails)
                        {
                            if (c.OTMOEQ_SubjectiveFlg == false)
                            {
                                var QuizeQuastions = c.QuizeQuastions > 0 ? c.QuizeQuastions : null;

                                //if (c.QuizeQuastions > 0)
                                //{
                                OT_Candidates_Exam_AnswerDMO OTGD = new OT_Candidates_Exam_AnswerDMO();

                                OTGD.OTCANDEX_Id = oce.OTCANDEX_Id;
                                OTGD.OTMOEQ_Id = c.OTMOEQ_Id;
                                OTGD.OTMOEQOA_Id = QuizeQuastions;
                                OTGD.OTCANDEXANS_CorrectAnsFlg = c.OTMOEQOA_AnswerFlag;
                                OTGD.OTCANDEXANS_ActiveFlg = true;
                                OTGD.OTCANDEXANS_CreatedBy = data.Userid;
                                OTGD.OTCANDEXANS_UpdatedBy = data.Userid;
                                OTGD.OTCANDEXANS_CreatedDate = indiantime0;
                                OTGD.OTCANDEXANS_UpdatedDate = indiantime0;
                                _VMSContext.Add(OTGD);
                                //}
                            }

                            else
                            {
                                OT_Candidates_Exam_SubjectiveAnswerDMO FRT = new OT_Candidates_Exam_SubjectiveAnswerDMO();
                                FRT.OTCANDEX_Id = oce.OTCANDEX_Id;
                                FRT.OTMOEQ_Id = c.OTMOEQ_Id;
                                FRT.OTCANDEXSANS_Answer = c.answer;
                                FRT.OTCANDEXSANS_ActiveFlg = true;
                                FRT.OTCANDEXSANS_CreatedDate = indiantime0;
                                FRT.OTCANDEXSANS_UpdatedBy = data.Userid;
                                FRT.OTCANDEXSANS_UpdatedDate = indiantime0;
                                FRT.OTCANDEXSANS_CreatedBy = data.Userid;
                                _VMSContext.Add(FRT);
                            }


                            var listfiles = _VMSContext.OT_Candidates_Exam_AnswersheetDMO.Where(a => a.OTMOEQ_Id == c.OTMOEQ_Id && a.OTCANDEX_Id == oce.OTCANDEX_Id).ToList();

                            if (listfiles.Count > 0)
                            {
                                foreach (var item in listfiles)
                                {
                                    _VMSContext.Remove(item);
                                }
                            }

                            if (c.tempoptionsfiles != null && c.tempoptionsfiles.Length > 0)
                            {
                                foreach (var itemf in c.tempoptionsfiles)
                                {
                                    OT_Candidates_Exam_AnswersheetDMO objfiles = new OT_Candidates_Exam_AnswersheetDMO();
                                    objfiles.OTMOEQ_Id = c.OTMOEQ_Id;
                                    objfiles.OTCANDEX_Id = oce.OTCANDEX_Id;
                                    objfiles.OTCANDEXAS_AnswerSheetFile = itemf.OTCANDEXAS_AnswerSheetFile;
                                    objfiles.OTCANDEXAS_AnswerSheetPath = itemf.OTCANDEXAS_AnswerSheetPath;
                                    objfiles.OTCANDEXAS_UpdatedDate = indiantime0;
                                    objfiles.OTCANDEXAS_CreatedDate = indiantime0;
                                    objfiles.OTCANDEXAS_CreatedBy = data.Userid;
                                    objfiles.OTCANDEXAS_UpdatedBy = data.Userid;
                                    objfiles.OTCANDEXAS_ActiveFlg = true;

                                    _VMSContext.Add(objfiles);
                                }
                            }

                        }

                        var i = _VMSContext.SaveChanges();
                        if (i > 0)
                        {

                            if (data.OTCANDEX_Id == 0)
                            {
                                var list = _VMSContext.OT_Candidates_ExamDMO.Where(w => w.OTCANDEX_Id == oce.OTCANDEX_Id).ToList();
                                if (list.Count > 0)
                                {
                                    data.OTCANDEX_Id = list.FirstOrDefault().OTCANDEX_Id;
                                }

                            }
                            data.message = "Saved";
                        }
                        else
                        {
                            data.message = "Failed";
                        }


                    }
                }

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LP_Online_Exam";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRCD_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRCD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@OTQPTYP_Id", SqlDbType.VarChar)
                    {
                        Value = data.OTQPTYP_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@OTCANDEX_Id", SqlDbType.VarChar)
                    {
                        Value = data.OTCANDEX_Id
                    });



                    cmd.Parameters.Add(new SqlParameter("@examtime", SqlDbType.VarChar)
                    {
                        Value = data.OTCANDEX_TotalTime
                    });

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
                        data.result = retObject.ToArray();
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
        #endregion





        #region Deactivate Document
        public OnlineTestCandidateDTO DeactivateActivateDocument(OnlineTestCandidateDTO data)
        {
            try
            {
                //    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //    var checkresult = _VMSContext.LP_Master_OE_Questions_FilesDMO.Where(a => a.LPMOEQF_Id == data.LPMOEQF_Id).ToList();

                //    if (checkresult.Count > 0)
                //    {
                //        var result = _VMSContext.LP_Master_OE_Questions_FilesDMO.Single(a => a.LPMOEQF_Id == data.LPMOEQF_Id);

                //        if (result.LPMOEQF_ActiveFlag == true)
                //        {
                //            result.LPMOEQF_ActiveFlag = false;
                //        }
                //        else
                //        {
                //            result.LPMOEQF_ActiveFlag = true;
                //        }
                //        result.UpdatedDate = indiantime0;
                //        _VMSContext.Update(result);

                //        var s = _VMSContext.SaveChanges();

                //        if (s > 0)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }

                //    data.getviedocarray = _VMSContext.LP_Master_OE_Questions_FilesDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion



        #region Deactivate Activate
        public OnlineTestCandidateDTO DeactivateActivateQuesOption(OnlineTestCandidateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQOA_Id == data.OTMOEQOA_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Single(a => a.OTMOEQOA_Id == data.OTMOEQOA_Id);

                    if (result.OTMOEQOA_ActiveFlg == true)
                    {
                        result.OTMOEQOA_ActiveFlg = false;
                    }
                    else
                    {
                        result.OTMOEQOA_ActiveFlg = true;
                    }
                    result.OTMOEQOA_UpdatedDate = indiantime0;
                    result.OTMOEQOA_UpdatedBy = data.Userid;
                    _VMSContext.Update(result);

                    var s = _VMSContext.SaveChanges();

                    if (s > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.getViewSavedOptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region Deactivate Activate Questions
        public OnlineTestCandidateDTO DeactivateActivateQues(OnlineTestCandidateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _VMSContext.OT_Master_OE_QuestionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _VMSContext.OT_Master_OE_QuestionsDMO.Single(a => a.OTMOEQ_Id == data.OTMOEQ_Id);

                    if (result.OTMOEQ_ActiveFlg == true)
                    {
                        result.OTMOEQ_ActiveFlg = false;
                    }
                    else
                    {
                        result.OTMOEQ_ActiveFlg = true;
                    }
                    result.OTMOEQ_UpdatedDate = indiantime0;
                    result.OTMOEQ_UpdatedBy = data.Userid;
                    _VMSContext.Update(result);

                    var s = _VMSContext.SaveChanges();

                    if (s > 0)
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
        #endregion





        //MASTER QUESTION
        #endregion

        #region  EXAM ANS PAPER  CORRECTION

        #region LOAD COMPANYWISE POSITION
        public OnlineTestCandidateDTO oncmpchange(OnlineTestCandidateDTO data)
        {
            try
            {
                // LOAD POSITION DROP DOWN

                data.positionlist = _VMSContext.HR_Master_PositionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMP_ActiveFlg == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion  
        #region LOAD DATA FOR PAPER  CORRECTION
        public OnlineTestCandidateDTO loadcorrection(OnlineTestCandidateDTO data)
        {
            try
            {
                // LOAD POSITION DROP DOWN
                data.milist = _VMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
                //  data.positionlist = _VMSContext.HR_Master_PositionDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region SEARCH CANDIDATE EXAM LIST
        public OnlineTestCandidateDTO GetSearchDetails(OnlineTestCandidateDTO data)
        {
            try
            {
                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_OTCANDIDATE_EXAM_FOR_CORRECTION";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMP_Id", SqlDbType.VarChar) { Value = data.HRMP_Id });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date) { Value = data.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date) { Value = data.todate });

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
                        data.getcandidatedetails = retObject.ToArray();
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
        #endregion

        #region VIEW ALL QUESTION/ANS FOR ENTER MARKS
        public OnlineTestCandidateDTO ViewQuestionanswers(OnlineTestCandidateDTO data)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            try
            {
                string examidss = "0";
                if (data.exmids != null)
                {

                    foreach (var item in data.exmids)
                    {
                        examidss = examidss + "," + item.OTCANDEX_Id.ToString();
                    }

                    using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_OT_GET_CNDQSNANSWER_LIST_MODIFICATION";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRCD_Id", SqlDbType.VarChar) { Value = data.HRCD_Id });
                        cmd.Parameters.Add(new SqlParameter("@OTCANDEX_Id", SqlDbType.VarChar) { Value = examidss });
                        cmd.Parameters.Add(new SqlParameter("@QSTType", SqlDbType.VarChar) { Value = data.type });

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
                            data.getcandidatequesansdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }
        #endregion

        #region SAVE  SUBJECTIVE ANS CORRECTION MARKS
        public OnlineTestCandidateDTO SaveSubjectiveMarks(OnlineTestCandidateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                if (data.savedetails != null)
                {
                    var checkhrmeid = _VMSContext.Staff_User_Login.Where(a => a.Id == data.Userid).ToList();
                    long HRME_Id = 0;
                    if (checkhrmeid.Count > 0)
                    {
                        HRME_Id = checkhrmeid.FirstOrDefault().Emp_Code;
                    }
                    //if (HRME_Id > 0)
                    //{
                    if (data.exmids != null && data.exmids.Length > 0)
                    {

                        var listexm = data.exmids.Distinct().ToList(); ;


                        foreach (var item in listexm)
                        {



                            decimal? totalsubjectivemarks = 0.00m;
                            decimal? totalmaxsubjectivemarks = 0.00m;
                            if (data.savedetails != null && data.savedetails.Length > 0)
                            {


                                var getsubjectivemarks = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Where(a => a.OTCANDEX_Id == item.OTCANDEX_Id).ToList();



                                decimal? perviousmarks = 0.00m;

                                foreach (var c in getsubjectivemarks)
                                {
                                    perviousmarks += c.OTCANDEXSANS_Marks == null ? 0 : c.OTCANDEXSANS_Marks;
                                }


                                long priviouscorect = 0;
                                long currentcorect = 0;
                                var correctcountlist = (from a in _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO
                                                        from b in _VMSContext.OT_Master_OE_QuestionsDMO
                                                        where a.OTMOEQ_Id == b.OTMOEQ_Id && b.MI_Id == data.MI_Id && a.OTCANDEXSANS_Marks == b.OTMOEQ_Marks && a.OTCANDEX_Id == item.OTCANDEX_Id
                                                        select a).Distinct().ToList();

                                if (correctcountlist.Count > 0)
                                {
                                    priviouscorect = correctcountlist.Count;
                                }


                                foreach (var c in data.savedetails)
                                {

                                    if (item.OTCANDEX_Id == c.OTCANDEX_Id)
                                    {

                                        decimal? marks = c.OTCANDEXSANS_Marks;
                                        decimal? OTMOEQ_Marks = c.OTMOEQ_Marks;
                                        // long amstid = c.AMST_Id;
                                        //long LPSTUEXSANS_Id = c.LPSTUEXSANS_Id;
                                        //long LPMOEQ_Id = c.LPMOEQ_Id;
                                        //long LPSTUEX_Id = c.LPSTUEX_Id;

                                        if (marks == OTMOEQ_Marks)
                                        {
                                            currentcorect += 1;
                                        }


                                        totalsubjectivemarks += marks;
                                        totalmaxsubjectivemarks += OTMOEQ_Marks;


                                        var checkresult = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Where(a => a.OTCANDEX_Id == item.OTCANDEX_Id && a.OTMOEQ_Id == c.OTMOEQ_Id).ToList();

                                        if (checkresult.Count > 0)
                                        {

                                            var result = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Single(a => a.OTCANDEX_Id == item.OTCANDEX_Id && a.OTMOEQ_Id == c.OTMOEQ_Id);
                                            result.OTCANDEXSANS_Marks = Convert.ToDecimal(marks);
                                            result.OTCANDEXSANS_UpdatedBy = data.Userid;
                                            result.OTCANDEXSANS_UpdatedDate = indiantime0;
                                            if (HRME_Id > 0)
                                            {
                                                result.HRME_Id = HRME_Id;
                                            }


                                            _VMSContext.Update(result);
                                        }
                                    }


                                }

                                var getobjectivecount = (from a in _VMSContext.OT_Candidates_ExamDMO
                                                         from b in _VMSContext.OT_Candidates_Exam_AnswerDMO
                                                         where (a.OTCANDEX_Id == b.OTCANDEX_Id
                                                         && a.OTCANDEX_Id == item.OTCANDEX_Id)
                                                         select b).Distinct().ToList();

                                var getsubjectivecount = (from a in _VMSContext.OT_Candidates_ExamDMO
                                                          from b in _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO
                                                          where (a.OTCANDEX_Id == b.OTCANDEX_Id && a.OTCANDEX_Id == item.OTCANDEX_Id)
                                                          select b).Distinct().ToList();



                                var resultstudent = _VMSContext.OT_Candidates_ExamDMO.Single(a => a.OTCANDEX_Id == item.OTCANDEX_Id);

                                decimal? totalmarks = 0.00m;
                                decimal? totalmaxmarks = 0.00m;
                                decimal? totalpercentage = 0.00m;
                                long totalcorrect = 0;

                                totalcorrect = (resultstudent.OTCANDEX_TotalCorrectAns - priviouscorect) + currentcorect;

                                if (totalcorrect < 0)
                                {
                                    totalcorrect = 0;
                                }

                                totalmarks = (resultstudent.OTCANDEX_TotalMarks - perviousmarks) + totalsubjectivemarks;

                                totalmaxmarks = resultstudent.OTCANDEX_TotalMaxMarks;

                                if (totalmarks > 0 && totalmaxmarks > 0)
                                {
                                    totalpercentage = (totalmarks / totalmaxmarks) * 100;
                                    totalpercentage = Math.Round(Convert.ToDecimal(totalpercentage), 0, MidpointRounding.AwayFromZero);
                                }



                                resultstudent.OTCANDEX_TotalMarks = Convert.ToDecimal(totalmarks);
                                resultstudent.OTCANDEX_TotalMaxMarks = Convert.ToDecimal(totalmaxmarks);
                                resultstudent.OTCANDEX_Percentage = Convert.ToDecimal(totalpercentage);
                                resultstudent.OTCANDEX_TotalCorrectAns = totalcorrect;
                                resultstudent.OTCANDEX_UpdatedBy = data.Userid;
                                resultstudent.OTCANDEX_UpdatedDate = indiantime0;
                                //resultstudent.LPSTUEX_PublishToStudent = getexamdetails.FirstOrDefault().LPMOEEX_AutoPublishFlg;
                                resultstudent.OTCANDEX_TotalQnsAnswered = getobjectivecount.Count() + getsubjectivecount.Count();

                                _VMSContext.Update(resultstudent);

                                var i = _VMSContext.SaveChanges();

                                if (i > 0)
                                {
                                    data.message = "Add";
                                }
                                else
                                {
                                    data.message = "Error";
                                }
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    data.message = "Emp";
                    //}
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        #endregion
        #endregion

        #region REPORT
        #region LOAD DATA FOR REPORT
        public OnlineTestCandidateDTO loadDetailsRpt(OnlineTestCandidateDTO data)
        {
            try
            {
                // LOAD POSITION DROP DOWN
                //data.positionlist = _VMSContext.HR_Master_PositionDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                data.milist = _VMSContext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region GET REPORT
        public OnlineTestCandidateDTO GetReport(OnlineTestCandidateDTO data)
        {
            try
            {
                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_OTCANDIDATE_EXAM_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMP_Id", SqlDbType.VarChar) { Value = data.HRMP_Id });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date) { Value = data.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date) { Value = data.todate });

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
                        data.getcandidatedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }
        #endregion

        #region GET RESULT REPORT 
        public OnlineTestCandidateDTO ViewResultRpt(OnlineTestCandidateDTO data)
        {
            try
            {
                string examidss = "0";
                if (data.exmids != null)
                {

                    List<long> exidss = new List<long>();

                    foreach (var item in data.exmids)
                    {
                        exidss.Add(item.OTCANDEX_Id);
                        examidss = examidss + "," + item.OTCANDEX_Id.ToString();
                    }

                    data.papertypelist = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                                          from b in _VMSContext.OT_Candidates_ExamDMO
                                          where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.OTQPTYP_Id == b.OTQPTYP_Id && exidss.Contains(b.OTCANDEX_Id)
                                          select a).Distinct().ToArray();

                    using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_OT_GET_CNDQSNANSWER_RESULT_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRCD_Id", SqlDbType.VarChar) { Value = data.HRMP_Id });
                        cmd.Parameters.Add(new SqlParameter("@OTCANDEX_Id", SqlDbType.VarChar) { Value = examidss });
                        cmd.Parameters.Add(new SqlParameter("@QSTType", SqlDbType.VarChar) { Value = "O" });

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
                            data.getcandidatequesansdetailsobj = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_OT_GET_CNDQSNANSWER_RESULT_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRCD_Id", SqlDbType.VarChar) { Value = data.HRMP_Id });
                        cmd.Parameters.Add(new SqlParameter("@OTCANDEX_Id", SqlDbType.VarChar) { Value = examidss });
                        cmd.Parameters.Add(new SqlParameter("@QSTType", SqlDbType.VarChar) { Value = "S" });

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
                            data.getcandidatequesansdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        #endregion
        #endregion

    }
}
