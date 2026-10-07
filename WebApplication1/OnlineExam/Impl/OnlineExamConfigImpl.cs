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
    public class OnlineExamConfigImpl : Interfaces.OnlineExamConfigInterface
    {
        private static ConcurrentDictionary<string, MasterQuestionDTO> _login =
             new ConcurrentDictionary<string, MasterQuestionDTO>();

        ILogger<OnlineExamConfigImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public OnlineExamConfigImpl(DomainModelMsSqlServerContext dbcontext, ILogger<OnlineExamConfigImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public MasterQuestionDTO getloaddata(MasterQuestionDTO data)
        {
            try
            {
                data.getQdetails = _dbContext.PA_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
               
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
                if (data.PAMOES_Id != 0)
                {
                    var res = _dbContext.PA_Master_OE_SettingDMO.Where(t => t.PAMOES_NoofQns == data.Noofques && t.PAMOES_TotalMarks == data.totmrks && t.PAMOES_TotalDuration == data.totdur && t.PAMOES_EachQnsMarks==data.echmrkques && t.PAMOES_EachQnsDuration==data.echquesdur && t.PAMOES_NoOfOptions==data.noopt && t.MI_Id==data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _dbContext.PA_Master_OE_SettingDMO.Single(t => t.PAMOES_Id == data.PAMOES_Id);
                        result.MI_Id = data.MI_Id;
                        result.PAMOES_NoofQns = data.Noofques;
                        result.PAMOES_TotalMarks = data.totmrks;
                        result.PAMOES_TotalDuration = data.totdur;
                        result.PAMOES_EachQnsMarks = data.echmrkques;
                        result.PAMOES_EachQnsDuration = data.echquesdur;
                        result.PAMOES_NoOfOptions = data.noopt;
                        //result.CreatedDate = DateTime.Now;
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
                  
                    var res = _dbContext.PA_Master_OE_SettingDMO.Where(t => t.PAMOES_NoofQns == data.Noofques && t.PAMOES_TotalMarks == data.totmrks && t.PAMOES_TotalDuration == data.totdur && t.PAMOES_EachQnsMarks == data.echmrkques && t.PAMOES_EachQnsDuration == data.echquesdur && t.PAMOES_NoOfOptions == data.noopt && t.MI_Id == data.MI_Id && t.PAMOES_Id != data.PAMOES_Id).ToList();


                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                       
                        PA_Master_OE_SettingDMO result1 = new PA_Master_OE_SettingDMO();
                        result1.MI_Id = data.MI_Id;
                        result1.PAMOES_NoofQns = data.Noofques;
                        result1.PAMOES_TotalMarks = data.totmrks;
                        result1.PAMOES_TotalDuration = data.totdur;
                        result1.PAMOES_EachQnsMarks = data.echmrkques;
                        result1.PAMOES_EachQnsDuration = data.echquesdur;
                        result1.PAMOES_NoOfOptions = data.noopt;
                        result1.CreatedDate = DateTime.Now;
                        result1.UpdatedDate = DateTime.Now;

                        _dbContext.Add(result1);
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
                data.editQus = _dbContext.PA_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id && a.PAMOES_Id == data.PAMOES_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question editQuestion :" + ex.Message);
            }
            return data;
        }


        public MasterQuestionDTO Deletedetails(MasterQuestionDTO data)
        {
            bool returnresult = false;
            MasterQuestionDTO del = new MasterQuestionDTO();
            try
            {

                var delete = _dbContext.PA_Master_OE_SettingDMO.Where(t => t.PAMOES_Id == data.PAMOES_Id).ToList();
                try
                {
                    if (delete.Any())
                    {
                        _dbContext.Remove(delete.ElementAt(0));

                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            del.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            del.returnval = returnresult;
                        }
                    }
                }
                catch (Exception ee)
                {
                
                    Console.WriteLine(ee.Message);
                }
            }
            catch (Exception ee)
            {
              
                Console.WriteLine(ee.Message);
            }
            return del;

        }
        
    }
}
