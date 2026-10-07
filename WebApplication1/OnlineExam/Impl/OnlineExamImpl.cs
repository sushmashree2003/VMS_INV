using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.OnlineExam;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineExam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    public class OnlineExamImpl : Interfaces.OnlineExamInterface
    {
        private static ConcurrentDictionary<string, OnlineExamDTO> _login =
             new ConcurrentDictionary<string, OnlineExamDTO>();

        ILogger<OnlineExamImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public OnlineExamImpl(DomainModelMsSqlServerContext dbcontext, ILogger<OnlineExamImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public OnlineExamDTO getloaddata(OnlineExamDTO data)
        {
            try
            {
               // data.getclass = _dbContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id).ToArray();

               var PASR_ID = _dbContext.StudentApplication.Where(a => a.MI_Id == data.MI_Id && a.Id==data.userid).FirstOrDefault().pasr_id;

                var ASMCL_ID = _dbContext.StudentApplication.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).FirstOrDefault().ASMCL_Id;

                data.Amst_ID = PASR_ID;
                data.ASMCL_Id = ASMCL_ID;

                data.getSubjects = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.MasterSubjectList
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && c.ISMS_PreadmFlag == 1)
                                    select new OnlineExamDTO
                                    {
                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        ASMCL_Id = b.ASMCL_Id,
                                    }).Distinct().ToArray();



                data.getQdetails = _dbContext.PA_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public OnlineExamDTO getSubjects(OnlineExamDTO data)
        {
            try
            {
                data.getSubjects = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.MasterSubjectList
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.ISMS_Id==c.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id==data.ASMCL_Id && c.ISMS_PreadmFlag == 1)
                                    select new OnlineExamDTO
                                    {
                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        ASMCL_Id = b.ASMCL_Id,
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public OnlineExamDTO getQuestion(OnlineExamDTO data)
        {
            try
            {
                data.getQuestion = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id==data.ASMCL_Id && a.ISMS_Id==data.ISMS_Id)
                                    select new OnlineExamDTO
                                    {
                                        PAMOEQ_Id = a.PAMOEQ_Id,
                                        PAMOEQOA_Id=c.PAMOEQOA_Id,
                                        PAMOEQ_Question = a.PAMOEQ_Question,
                                        ASMCL_Id = b.ASMCL_Id,
                                        PAMOEQOA_OptionCode = c.PAMOEQOA_OptionCode,
                                        PAMOEQOA_Option = c.PAMOEQOA_Option,
                                        PAMOEQOA_AnswerFlag=c.PAMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        
          public OnlineExamDTO savedanswers(OnlineExamDTO data)
          {
            try
            {
                data.savedanswer = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                    from d in _dbContext.PA_Students_ExamDMO
                                    from e in _dbContext.PA_Students_Exam_AnswerDMO
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.MI_Id == data.MI_Id && e.PAMOEQOA_Id == c.PAMOEQOA_Id && d.PASTE_Id == e.PASTE_Id && d.AMST_Id==data.Amst_ID)
                                    select new OnlineExamDTO
                                    {
                                        PAMOEQ_Id = e.PAMOEQ_Id,
                                        PAMOEQOA_Id = e.PAMOEQOA_Id,
                                        PAMOEQ_Question = a.PAMOEQ_Question,
                                        PAMOEQOA_OptionCode = c.PAMOEQOA_OptionCode,
                                        PAMOEQOA_Option = c.PAMOEQOA_Option,
                                        PAMOEQOA_AnswerFlag = c.PAMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public OnlineExamDTO Saveanswer(OnlineExamDTO data)
        {
            try
            {
                var res = _dbContext.PA_Students_ExamDMO.Where(t => t.AMST_Id==data.Amst_ID).ToList();
                if (res.Count > 0)
                {
                    try
                    {
                        //PA_Students_Exam_AnswerDMO oe = new PA_Students_Exam_AnswerDMO();
                        //oe.PASTE_Id = res.FirstOrDefault().PASTE_Id;
                        //oe.PAMOEQOA_Id = data.saveanswerlst[0].PAMOEQOA_Id;
                        //oe.CreatedDate = DateTime.Now;
                        //oe.UpdatedDate = DateTime.Now;
                        //_dbContext.Add(oe);
                        //var contactExists = _dbContext.SaveChanges();


                        var check = (from a in _dbContext.PA_Students_ExamDMO
                                     from b in _dbContext.PA_Students_Exam_AnswerDMO
                                     where (a.PASTE_Id == b.PASTE_Id && a.AMST_Id == data.Amst_ID && a.MI_Id == data.MI_Id && a.PASTE_Id == res.FirstOrDefault().PASTE_Id  && b.PAMOEQ_Id== data.saveanswerlst[0].PAMOEQ_Id)
                                     select new OnlineExamDTO
                                     {
                                         PASTE_Id = b.PASTE_Id,
                                         PAMOEQOA_Id = b.PAMOEQOA_Id,
                                     }).ToList();

                        if (check.Count > 0)
                        {
                            var update1 = _dbContext.PA_Students_Exam_AnswerDMO.Where(t => t.PASTE_Id == check.FirstOrDefault().PASTE_Id && t.PAMOEQ_Id== data.saveanswerlst[0].PAMOEQ_Id).SingleOrDefault();


                            update1.PAMOEQOA_Id = data.saveanswerlst[0].PAMOEQOA_Id;
                            _dbContext.Update(update1);
                            _dbContext.SaveChanges();
                        }

                        else
                        {
                            PA_Students_Exam_AnswerDMO oe = new PA_Students_Exam_AnswerDMO();
                            oe.PASTE_Id = res.FirstOrDefault().PASTE_Id;
                            oe.PAMOEQOA_Id = data.saveanswerlst[0].PAMOEQOA_Id;
                            oe.PAMOEQ_Id = data.saveanswerlst[0].PAMOEQ_Id;
                            oe.CreatedDate = DateTime.Now;
                            oe.UpdatedDate = DateTime.Now;
                            _dbContext.Add(oe);
                            var contactExists = _dbContext.SaveChanges();
                        }



                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                else
                {
                    try
                    {
                        PA_Students_ExamDMO oe = new PA_Students_ExamDMO();
                        
                        //oe.PASTE_Id = data.userid;
                       // oe.PAMOEQ_Id = data.saveanswerlst[0].PAMOEQ_Id;
                        oe.MI_Id = data.MI_Id;
                        oe.AMST_Id = data.Amst_ID;
                        oe.PASTE_Date = DateTime.Now;
                        oe.PASTE_TotalTime =null;
                        oe.PASTE_TotalQnsAnswered = null;
                        oe.PASTE__TotalCorrectAns = null;
                        oe.PASTE_TotalMaxMarks = null;
                        oe.PASTE_TotalMarks = null;
                        oe.PASTE_Percentage = null;
                        oe.CreatedDate = DateTime.Now;
                        oe.UpdatedDate = DateTime.Now;
                        _dbContext.Add(oe);

                        
                            PA_Students_Exam_AnswerDMO oe1 = new PA_Students_Exam_AnswerDMO();
                            oe1.PASTE_Id = oe.PASTE_Id;
                            oe1.PAMOEQOA_Id = data.saveanswerlst[0].PAMOEQOA_Id;
                            oe1.PAMOEQ_Id = data.saveanswerlst[0].PAMOEQ_Id;
                            oe1.CreatedDate = DateTime.Now;
                            oe1.UpdatedDate = DateTime.Now;
                            _dbContext.Add(oe1);
                            var contactExists1 = _dbContext.SaveChanges();
                        
                       

                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.savedanswer = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                    from d in _dbContext.PA_Students_ExamDMO
                                    from e in _dbContext.PA_Students_Exam_AnswerDMO
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.MI_Id == data.MI_Id && e.PAMOEQOA_Id == c.PAMOEQOA_Id && d.PASTE_Id == e.PASTE_Id && d.AMST_Id == data.Amst_ID)
                                    select new OnlineExamDTO
                                    {
                                        PAMOEQ_Id = e.PAMOEQ_Id,
                                        PAMOEQOA_Id = e.PAMOEQOA_Id,
                                        PAMOEQ_Question = a.PAMOEQ_Question,
                                        PAMOEQOA_OptionCode = c.PAMOEQOA_OptionCode,
                                        PAMOEQOA_Option = c.PAMOEQOA_Option,
                                        PAMOEQOA_AnswerFlag = c.PAMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


        public async Task<OnlineExamDTO> submitexam(OnlineExamDTO data)
        {
            try
            {
                using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Online_Exam";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@examtime",
                        SqlDbType.VarChar)
                    {
                        Value = data.PASTE_TotalTime
                    });
                    cmd.Parameters.Add(new SqlParameter("@Amst_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.Amst_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


    }
}
