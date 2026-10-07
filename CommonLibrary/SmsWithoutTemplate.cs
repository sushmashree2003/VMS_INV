using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class SmsWithoutTemplate
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;

        public SmsWithoutTemplate(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public async Task<string> sendSms(long MI_Id, long mobileNo,string userName,string pwd)
        {
            
            try
            {
                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string url = alldetails[0].IVRMSD_URL.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", "Login Credential.....Your UserName is: "+userName+" and"+" Password Is: "+pwd);

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string responseparameters = readStream.ReadToEnd();
                return "Success";
               
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
            return "";
        }
        public async Task<string> sendsmsfromPortal(long MI_Id, long mobileNo, string message)
        {

            try
            {
                message = message.Replace("&", "%26");
                message = message.Replace("#", "%23");
                message = message.Replace("'", "%27");
                message = message.Replace("$", "%24");
                message = message.Replace("+", "%2B");
                message = message.Replace("@", "%40");
                message = message.Replace("?", "%3F");
                message = message.Replace(">", "%3E");
                message = message.Replace("=", "%3D");
                message = message.Replace(";", "%3B");
                message = message.Replace("<", "%3C");

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string url = alldetails[0].IVRMSD_URL.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", message);

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string responseparameters = readStream.ReadToEnd();

                if (responseparameters != null)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = message
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "PRINCIPAL DASHBOARD"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = responseparameters
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
                            return ex.Message;
                        }
                    }
                }


            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }


        public async Task<string> sendsmsfromPortal_new(long MI_Id, long mobileNo, string message,string Template, string module_name)
        {

            try
            {
                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "PRINCIPAL" || Template == "MANAGER")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, message.ToString());
                    sms = result;
                }

                string url = alldetails[0].IVRMSD_URL.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", sms);

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string responseparameters = readStream.ReadToEnd();

                if (responseparameters != null)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = module_name
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = responseparameters
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
                            return ex.Message;
                        }
                    }
                }


            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }
    }
}

