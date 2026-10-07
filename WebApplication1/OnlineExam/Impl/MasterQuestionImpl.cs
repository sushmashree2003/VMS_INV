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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    public class MasterQuestionImpl : Interfaces.MasterQuestionInterface
    {
        private static ConcurrentDictionary<string, MasterQuestionDTO> _login =
             new ConcurrentDictionary<string, MasterQuestionDTO>();

        ILogger<MasterQuestionImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public MasterQuestionImpl(DomainModelMsSqlServerContext dbcontext, ILogger<MasterQuestionImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public MasterQuestionDTO getloaddata(MasterQuestionDTO data)
        {
            try
            {
                data.getQuestiondetails = _dbContext.PA_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                data.getclass = _dbContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id).ToArray();
                data.getSubjects = _dbContext.MasterSubjectList.Where(a => a.MI_Id == data.MI_Id && a.ISMS_PreadmFlag==1).ToArray();
                //--------------------2nd Tab
        

                data.getAnsOptions = _dbContext.PA_Master_OE_ANS_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.PAMOEAO_OptionsFlag == "A").ToArray();

                data.getFQOptiondetails = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                           from b in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                           where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.MI_Id == data.MI_Id)
                                           select new MasterQuestionDTO
                                           {
                                               PAMOEQ_Id = a.PAMOEQ_Id,
                                               PAMOEQ_Question = a.PAMOEQ_Question,
                                               PAMOEQ_Marks = a.PAMOEQ_Marks,
                                           }).Distinct().ToArray();

                
                //data.getFQuestiondetails = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                //                            from b in _dbContext.PA_Master_OE_QNS_OptionsDMO
                //                            where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.MI_Id == data.MI_Id)
                //                            select new MasterQuestionDTO
                //                            {
                //                                PAMOEQ_Id = a.PAMOEQ_Id,
                //                                PAMOEQ_Question = a.PAMOEQ_Question,
                //                                PAMOEQ_Marks = a.PAMOEQ_Marks,
                //                                PAMOEQOA_Option = b.PAMOEQOA_Option,
                //                                PAMOEQOA_OptionCode = b.PAMOEQOA_OptionCode,
                //                                PAMOEQOA_AnswerFlag = b.PAMOEQOA_AnswerFlag
                //                            }).Distinct().ToArray();



              //  var questions = _dbContext.PA_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).ToArray().ToList();



                var questions = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                           from b in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                           where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.MI_Id == data.MI_Id)
                                           select new MasterQuestionDTO
                                           {
                                               PAMOEQ_Id = a.PAMOEQ_Id,
                                           }).Distinct().ToArray().ToList();



                List<long> PAMOEQ_Ids = new List<long>();
               
                foreach (var item in questions)
                {
                    PAMOEQ_Ids.Add(item.PAMOEQ_Id);
                }


                data.getFQuestiondetails = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                          where (a.MI_Id == data.MI_Id && ! PAMOEQ_Ids.Contains(a.PAMOEQ_Id))
                                            select new MasterQuestionDTO
                                            {
                                                PAMOEQ_Id = a.PAMOEQ_Id,
                                                PAMOEQ_Question = a.PAMOEQ_Question,
                                                PAMOEQ_Marks = a.PAMOEQ_Marks,
                                            }).Distinct().ToArray();


             





                data.result = (from a in _dbContext.School_M_Class
                               from b in _dbContext.PA_Master_OE_QuestionsDMO
                               from c in _dbContext.PA_Master_OE_Questions_ClassDMO
                               where (b.PAMOEQ_Id == c.PAMOEQ_Id && a.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_Id)
                               select new MasterQuestionDTO
                               {
                                   PAMOEQ_Id = c.PAMOEQC_Id,
                                   PAMOEQ_Question = b.PAMOEQ_Question,
                                   ASMCL_ClassName = a.ASMCL_ClassName,
                                   ASMCL_Id=a.ASMCL_Id
                               }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public MasterQuestionDTO savedetails(MasterQuestionDTO data)
        {
            try
            {
                if (data.PAMOEQ_Id != 0)
                {
                    var res = _dbContext.PA_Master_OE_QuestionsDMO.Where(t =>  t.PAMOEQ_Question.Contains(data.PAMOEQ_Question) && t.PAMOEQ_Marks == data.PAMOEQ_Marks && t.PAMOEQ_Id != data.PAMOEQ_Id && t.MI_Id==data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _dbContext.PA_Master_OE_QuestionsDMO.Single(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.MI_Id==data.MI_Id);
                        result.MI_Id = data.MI_Id;
                        result.ISMS_Id = data.ISMS_Id;
                        result.PAMOEQ_Question = data.PAMOEQ_Question;
                        result.PAMOEQ_Marks = data.PAMOEQ_Marks;
                        result.PAMOEQ_QuestionDesc = data.PAMOEQ_QuestionDesc;
                        result.CreatedDate = DateTime.Now;
                        result.UpdatedDate = DateTime.Now;
                        _dbContext.Update(result);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
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
                    var res = _dbContext.PA_Master_OE_QuestionsDMO.Where(t => t.PAMOEQ_Question == data.PAMOEQ_Question && t.PAMOEQ_Marks == data.PAMOEQ_Marks && t.PAMOEQ_Id != data.PAMOEQ_Id && t.MI_Id==data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _dbContext.PA_Master_OE_QuestionsDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.MI_Id==data.MI_Id).ToList().Count;
                        PA_Master_OE_QuestionsDMO oe = new PA_Master_OE_QuestionsDMO();
                        oe.MI_Id = data.MI_Id;
                        oe.ISMS_Id = data.ISMS_Id;
                        oe.PAMOEQ_Question = data.PAMOEQ_Question;
                        oe.PAMOEQ_Marks = data.PAMOEQ_Marks;
                        oe.PAMOEQ_QuestionDesc = data.PAMOEQ_QuestionDesc;
                        oe.CreatedDate = DateTime.Now;
                        oe.UpdatedDate = DateTime.Now;

                        _dbContext.Add(oe);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
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
                _log.LogInformation("Master Quota save data :" + ex.Message);
            }
            return data;
        }







        public MasterQuestionDTO savedataclass(MasterQuestionDTO data)
        {
            try
            {
                if (data.PAMOEQC_Id != 0)
                {
                    var res = _dbContext.PA_Master_OE_Questions_ClassDMO.Where(t => t.PAMOEQ_Id==data.PAMOEQ_Id && t.ASMCL_Id == data.ASMCL_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _dbContext.PA_Master_OE_Questions_ClassDMO.Single(t => t.PAMOEQC_Id == data.PAMOEQC_Id);
                        result.PAMOEQ_Id = data.PAMOEQ_Id;
                        result.ASMCL_Id = data.ASMCL_Id;
                        result.CreatedDate = DateTime.Now;
                        result.UpdatedDate = DateTime.Now;
                        _dbContext.Update(result);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
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
                    var res = _dbContext.PA_Master_OE_Questions_ClassDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.ASMCL_Id == data.ASMCL_Id && t.PAMOEQC_Id != data.PAMOEQC_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _dbContext.PA_Master_OE_Questions_ClassDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id).ToList().Count;
                        PA_Master_OE_Questions_ClassDMO oe = new PA_Master_OE_Questions_ClassDMO();
                     
                        oe.PAMOEQ_Id = data.PAMOEQ_Id;
                        oe.ASMCL_Id = data.ASMCL_Id;
                        oe.CreatedDate = DateTime.Now;
                        oe.UpdatedDate = DateTime.Now;

                        _dbContext.Add(oe);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
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
                _log.LogInformation("Master Quota save data :" + ex.Message);
            }
            return data;
        }

        public MasterQuestionDTO editQuestion(MasterQuestionDTO data)
        {
            try
            {
                data.editQus = _dbContext.PA_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.PAMOEQ_Id == data.PAMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question editQuestion :" + ex.Message);
            }
            return data;
        }



        //--------------------------------------2nd TAB
        public MasterQuestionDTO optionChange(MasterQuestionDTO data)
        {
            try
            {

                var logo = _dbContext.PA_Master_OE_SettingDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                data.noopt = logo.FirstOrDefault().PAMOES_NoOfOptions;

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public MasterQuestionDTO savedetails1(MasterQuestionDTO data)
        {
            try
            {

                foreach (var a in data.seleted_Ans)
                {
                    var res = _dbContext.PA_Master_OE_QNS_OptionsDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.PAMOEQOA_Option == a.PAMOEQOA_Option && t.PAMOEQOA_OptionCode == a.PAMOEQOA_OptionCode).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        PA_Master_OE_QNS_OptionsDMO option = new PA_Master_OE_QNS_OptionsDMO();

                        option.PAMOEQ_Id = data.PAMOEQ_Id;
                        option.PAMOEQOA_Option = a.PAMOEQOA_Option;
                        option.PAMOEQOA_OptionCode = a.PAMOEQOA_OptionCode;
                        option.PAMOEQOA_AnswerFlag = a.PAMOEQOA_AnswerFlag;
                        option.UpdatedDate = DateTime.Now;
                        option.CreatedDate = DateTime.Now;
                        _dbContext.Add(option);
                    }
                }

                var contactExists = _dbContext.SaveChanges();
                if (contactExists > 0)
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
                _log.LogInformation("Answer Options savedetails1 :" + ex.Message);
            }
            return data;
        }

        public MasterQuestionDTO optiondetails(MasterQuestionDTO data)
        {
            try
            {
                data.getoptiondetails = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                         from b in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                         where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.PAMOEQ_Id == data.PAMOEQ_Id && a.MI_Id == data.MI_Id)
                                         select new MasterQuestionDTO
                                         {
                                             PAMOEQOA_Id = b.PAMOEQOA_Id,
                                             PAMOEQ_Id = a.PAMOEQ_Id,
                                             PAMOEQ_Question = a.PAMOEQ_Question,
                                             PAMOEQ_Marks = a.PAMOEQ_Marks,
                                             PAMOEQOA_OptionCode = b.PAMOEQOA_OptionCode,
                                             PAMOEQOA_Option = b.PAMOEQOA_Option,
                                             PAMOEQOA_AnswerFlag = b.PAMOEQOA_AnswerFlag
                                         }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


        public MasterQuestionDTO Deletedetails(MasterQuestionDTO data)
        {
            try
            {
                var lorg1 = _dbContext.PA_Master_OE_QNS_OptionsDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id).ToList();
                if (lorg1.Any())
                {
                    for (int i = 0; lorg1.Count > i; i++)
                    {
                        _dbContext.Remove(lorg1.ElementAt(i));
                    }
                }
                
                var contactexisttransaction = 0;
                using (var dbCtxTxn = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _dbContext.SaveChanges();
                        dbCtxTxn.Commit();
                        data.returnval = true;
                    }
                    catch (Exception ex)
                    {
                        dbCtxTxn.Rollback();
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        
    }
}
