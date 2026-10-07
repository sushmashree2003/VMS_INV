using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class SMSReportImpl : Interfaces.SMSReportInterface
    {
        public ScheduleReportContext _SReportContext;
        public DomainModelMsSqlServerContext _SSReportContext;
        public SMSReportImpl(ScheduleReportContext DomainModelContext, DomainModelMsSqlServerContext DomainModelContext1)
        {
            _SReportContext = DomainModelContext; _SSReportContext = DomainModelContext1;
        }

        public SMSReportDTO getdetails(SMSReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _SReportContext.AcademicYear.Where(t => t.MI_Id == data.mid && t.Is_Active == true).ToList();
                data.yeardropDown = year.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<SMSReportDTO> Getreportdetails(SMSReportDTO data)
        {
            List<SMSReportDTO> AllInOne = new List<SMSReportDTO>();
            List<SMSReportDTO> AllInOne1 = new List<SMSReportDTO>();
            List<SMSReportDTO> stulist = new List<SMSReportDTO>();
          
            SMSReportDTO temp1 = new SMSReportDTO();
            try
            {

                List<SMSReportDTO> result = new List<SMSReportDTO>();
                List<SMSReportDTO> result1 = new List<SMSReportDTO>();
                // for sms  count
                using (var cmd = _SReportContext.Database.GetDbConnection().CreateCommand())
                {
                    //changed by suryan
                    cmd.CommandText = "GET_SMS_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = data.asmayid });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = data.from_date });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = data.to_date });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.mid });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new SMSReportDTO
                                {
                                    Name = (dataReader["Name"]).ToString(),
                                    smscount = (dataReader["SMSCount"]).ToString(),
                                    emailcount = (dataReader["EMAILCount"]).ToString()
                                });
                                data.sms_listarray = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // for email  count
                using (var cmd = _SReportContext.Database.GetDbConnection().CreateCommand())
                {
                    //changed by suryan
                    cmd.CommandText = "GET_EMAIL_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = data.asmayid });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = data.from_date });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = data.to_date });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.mid });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result1.Add(new SMSReportDTO
                                {
                                    Name = (dataReader["Name"]).ToString(),
                                    smscount = (dataReader["SMSCount"]).ToString(),
                                    emailcount = (dataReader["EMAILCount"]).ToString()
                                });
                                data.mial_listarray = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                if (data.sms_listarray.Count() > 0)
                {
                  
                    for (int i = 0; i < data.sms_listarray.Count(); i++)
                    {
                        SMSReportDTO temp = new SMSReportDTO();
                        int where = 0;
                        temp.Name = data.sms_listarray[i].Name;
                        temp.smscount = data.sms_listarray[i].smscount;
                        if (data.mial_listarray.Count() > 0)
                        {
                            for (int j = 0; j < data.mial_listarray.Count(); j++)
                            {
                                if (where == 0)
                                {
                                    if (data.sms_listarray[i].Name == data.mial_listarray[j].Name)
                                    {
                                        temp.emailcount = data.mial_listarray[j].emailcount;
                                        where = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            temp.emailcount = "0";

                        }
                        if(where==0)
                        {
                            temp.emailcount = "0";
                        }                       
                        AllInOne.Add(temp);
                    }

                    for (int i = 0; i < data.mial_listarray.Count(); i++)
                    {
                        SMSReportDTO temp = new SMSReportDTO();
                        int where = 0;
                        temp.Name = data.mial_listarray[i].Name;
                        temp.emailcount = data.mial_listarray[i].emailcount;
                        if (data.sms_listarray.Count() > 0)
                        {
                            for (int j = 0; j < data.sms_listarray.Count(); j++)
                            {
                                if (where == 0)
                                {
                                    if (data.mial_listarray[i].Name == data.sms_listarray[j].Name)
                                    {
                                        temp.smscount = data.sms_listarray[j].smscount;
                                        where = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            temp.smscount = "0";

                        }
                        if (where == 0)
                        {
                            temp.smscount = "0";
                            AllInOne.Add(temp);
                        }
                       
                    }
                }
                else
                {
                    for (int i = 0; i < data.mial_listarray.Count(); i++)
                    {
                        SMSReportDTO temp = new SMSReportDTO();
                        temp.Name = data.mial_listarray[i].Name;
                        temp.smscount = data.mial_listarray[i].smscount;
                        temp.emailcount = data.mial_listarray[i].emailcount;
                        AllInOne.Add(temp);
                    }
                }

                data.sms_mial_listarray = AllInOne.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;

        }
    }
}
