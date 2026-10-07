using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.RTO;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Services
{
    public class RTOViolationIMPL : Interfaces.RTOViolationInterface
    {
        public RTOContext _RTOContext;
        public DomainModelMsSqlServerContext _db;
        public RTOViolationIMPL(RTOContext hh, DomainModelMsSqlServerContext db)
        {
            _RTOContext = hh;
            _db = db;
        }
        public RTO_Violation_ObservationDTO loaddata(RTO_Violation_ObservationDTO data)
        {
            try
            {
                //fineArray
                data.fineArray = _RTOContext.RTO_Master_Fine_ParameterDMO.Where(R => R.RTOMFP_ActiveFlag == true).Distinct().ToArray();
                data.vehicalArray = _RTOContext.Master_VehicleDMO.Where(R=>R.TRMV_ActiveFlag==true).Distinct().ToArray();
                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RTO_EMPLOYEE_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                    {
                        Value = "emplist"
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
                                var dataRow = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
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
                        data.emplist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                //RTO_VialationList
                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RTO_EMPLOYEE_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                    {
                        Value = "ViolationList"
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
                                var dataRow = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
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
                        data.RTO_VialationList = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                //StaffUserLogin
                data.CreatedBy = _db.ApplicationUser.Where(R => R.Id == data.UserId).Select(R => R.NormalizedUserName).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //RecieptReport
        public RTO_Violation_ObservationDTO RecieptReport(RTO_Violation_ObservationDTO data)
        {
            try
            {
                data.editlist = _RTOContext.RTO_Violation_ObservationDMO.Where(R => R.RTOVOB_Id == data.RTOVOB_Id).Distinct().ToArray();
                data.filist = _RTOContext.RTO_Violation_Observation_FileDMO.Where(R => R.RTOVOB_Id == data.RTOVOB_Id).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public RTO_Violation_ObservationDTO savedata(RTO_Violation_ObservationDTO data)
        {
            try
            {
                if (data.RTOVOB_Id > 0)
                {
                    var duplicate = _RTOContext.RTO_Violation_ObservationDMO.Where(R => R.RTOVOB_DateTimeOfViolation == data.RTOVOB_DateTimeOfViolation && R.RTOVOB_Date == data.RTOVOB_Date && R.RTOMFP_Id == data.RTOMFP_Id && R.RTOVOB_Id != data.RTOVOB_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _RTOContext.RTO_Violation_ObservationDMO.Where(R => R.RTOVOB_Id == data.RTOVOB_Id).FirstOrDefault();                        // var                        
                        obj.HRME_Id = data.HRME_Id;
                        obj.TRMV_Id = data.TRMV_Id;
                        obj.RTOVOB_Date = data.RTOVOB_Date;
                        obj.RTOVOB_AnyViolationFlg = data.RTOVOB_AnyViolationFlg;
                        obj.RTOVOB_DateTimeOfViolation = data.RTOVOB_DateTimeOfViolation;
                        obj.RTOMFP_Id = data.RTOMFP_Id;
                        obj.RTOVOB_ViolationDescription = data.RTOVOB_ViolationDescription;
                        obj.RTOVOB_FineAmount = data.RTOVOB_FineAmount;
                        obj.RTOVOB_UpdatedDate = DateTime.Now;
                        obj.RTOVOB_UpdatedBy = data.UserId;
                        obj.RTOVOB_FineAmountPaidFlg = data.RTOVOB_FineAmountPaidFlg;
                        _RTOContext.Update(obj);
                        if (data.ViolationFiles != null && data.ViolationFiles.Length > 0)
                        {
                            var filelist = _RTOContext.RTO_Violation_Observation_FileDMO.Where(R => R.RTOVOB_Id == data.RTOVOB_Id).ToList();
                            if (filelist.Count > 0)
                            {
                                foreach (var file in filelist)
                                {
                                    _RTOContext.Remove(file);
                                }
                            }
                            for (int p = 0; p < data.ViolationFiles.Length; p++)
                            {
                                RTO_Violation_Observation_FileDMO db = new RTO_Violation_Observation_FileDMO();
                                db.RTOVOB_Id = data.RTOVOB_Id;
                                db.RTOVOBFL_FileName = data.ViolationFiles[p].RTOVOBFL_FileName;
                                db.RTOVOBFL_FilePath = data.ViolationFiles[p].RTOVOBFL_FilePath;
                                db.RTOVOBFL_Remarks = data.ViolationFiles[p].RTOVOBFL_Remarks;
                                db.RTOVOBFL_ActiveFlag = true;
                                db.RTOVOBFL_CreatedBy = data.UserId;
                                db.RTOVOBFL_UpdatedBy = data.UserId;
                                db.RTOVOBFL_CreatedDate = DateTime.Now;
                                db.RTOVOBFL_UpdatedDate = DateTime.Now;
                                _RTOContext.Add(db);
                            }


                        }
                        int W = _RTOContext.SaveChanges();
                        if (W > 0)
                        {
                            data.returnval = "update";
                            Dictionary<string, string> val = new Dictionary<string, string>();
                            var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "RTOvialtion" && e.ISES_MailActiveFlag == true).ToList();
                            if (template.Count == 0)
                            {

                                return data;
                            }
                            var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                            string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                            List<Match> variables = new List<Match>();

                            foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                            {
                                variables.Add(match);
                            }
                            Mailmsg = data.Template;
                            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                            alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();

                            string Attechement = "";
                            if (alldetails.Count > 0)
                            {
                                string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                                string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                                string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                                Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                                string Subject = template[0].ISES_MailSubject.ToString();
                                string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                                string mailcc = "";
                                string mailbcc = "";

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    //Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                                }

                                var message = new SendGridMessage();
                                message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                                message.Subject = Subject;
                                string emailid = _db.Multiple_Email_DMO.Where(L => L.HRME_Id == data.HRME_Id && L.HRMEM_DeFaultFlag == "default").Select(L => L.HRMEM_EmailId).FirstOrDefault();

                                long Id = _db.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                                string To_Mail = _db.Multiple_Email_DMO.Where(A => A.HRME_Id == Id && A.HRMEM_DeFaultFlag == "default").Select(A => A.HRMEM_EmailId).FirstOrDefault();
                                // string To_Mail = "punith@unnathimarketing.com";
                                message.AddTo(emailid);
                                message.AddTo(To_Mail);
                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {
                                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                            Stream stream = response.GetResponseStream();
                                            message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                        }
                                    }
                                }
                              
                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        message.AddCc(c);
                                    }
                                }
                              
                                if (mailbcc != null && mailbcc != "")
                                {
                                    message.AddBcc(mailbcc);
                                }
                                message.HtmlContent = Mailmsg;
                                var client = new SendGridClient(sengridkey);
                                client.SendEmailAsync(message).Wait();
                                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                                {
                                    var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "RTOvialtion" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                                    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                                    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                    cmd.CommandText = "IVRM_Email_Outgoing";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                                    {
                                        Value = emailid
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                                    {
                                        Value = Mailmsg
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                                    {
                                        Value = modulename[0]
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = data.MI_Id
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

                                    }
                                }
                            }
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _RTOContext.RTO_Violation_ObservationDMO.Where(R => R.RTOVOB_DateTimeOfViolation == data.RTOVOB_DateTimeOfViolation && R.RTOVOB_Date == data.RTOVOB_Date && R.RTOMFP_Id == data.RTOMFP_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        RTO_Violation_ObservationDMO obj = new RTO_Violation_ObservationDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.TRMV_Id = data.TRMV_Id;
                        obj.RTOVOB_Date = data.RTOVOB_Date;
                        obj.RTOVOB_AnyViolationFlg = data.RTOVOB_AnyViolationFlg;
                        obj.RTOVOB_DateTimeOfViolation = data.RTOVOB_DateTimeOfViolation;
                        obj.RTOMFP_Id = data.RTOMFP_Id;
                        obj.RTOVOB_ViolationDescription = data.RTOVOB_ViolationDescription;
                        obj.RTOVOB_FineAmount = data.RTOVOB_FineAmount;
                        obj.RTOVOB_ActiveFlag = true;
                        obj.RTOVOB_CreatedDate = DateTime.Now;
                        obj.RTOVOB_UpdatedDate = DateTime.Now;
                        obj.RTOVOB_CreatedBy = data.UserId;
                        obj.RTOVOB_UpdatedBy = data.UserId;
                        obj.RTOVOB_FineAmountPaidFlg = data.RTOVOB_FineAmountPaidFlg;
                        _RTOContext.Add(obj);
                        if (data.ViolationFiles != null && data.ViolationFiles.Length > 0)
                        {
                            for (int K = 0; K < data.ViolationFiles.Length; K++)
                            {
                                RTO_Violation_Observation_FileDMO db = new RTO_Violation_Observation_FileDMO();
                                db.RTOVOB_Id = obj.RTOVOB_Id;
                                db.RTOVOBFL_FileName = data.ViolationFiles[K].RTOVOBFL_FileName;
                                db.RTOVOBFL_FilePath = data.ViolationFiles[K].RTOVOBFL_FilePath;
                                db.RTOVOBFL_Remarks = data.ViolationFiles[K].RTOVOBFL_Remarks;
                                db.RTOVOBFL_ActiveFlag = true;
                                db.RTOVOBFL_CreatedBy = data.UserId;
                                db.RTOVOBFL_UpdatedBy = data.UserId;
                                db.RTOVOBFL_CreatedDate = DateTime.Now;
                                db.RTOVOBFL_UpdatedDate = DateTime.Now;
                                _RTOContext.Add(db);

                            }


                        }
                        int p = _RTOContext.SaveChanges();
                        if (p > 0)
                        {
                            data.returnval = "save";
                            Dictionary<string, string> val = new Dictionary<string, string>();
                            var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "RTOvialtion" && e.ISES_MailActiveFlag == true).ToList();
                            if (template.Count == 0)
                            {
                                return data;
                            }
                            var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                            string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                            List<Match> variables = new List<Match>();

                            foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                            {
                                variables.Add(match);
                            }
                            Mailmsg = data.Template;
                            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                            alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();

                            string Attechement = "";
                            if (alldetails.Count > 0)
                            {
                                string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                                string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                                string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                                Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                                string Subject = template[0].ISES_MailSubject.ToString();
                                string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                                string mailcc = "";
                                string mailbcc = "";
                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    string[] ccmail = template[0].ISES_MailCCId.Split(',');
                                    mailcc = ccmail[0].ToString();
                                    if (ccmail.Length > 1)
                                    {
                                        if (ccmail[1] != null || ccmail[1] != "")
                                        {
                                            mailbcc = ccmail[1].ToString();
                                        }
                                    }
                                }
                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    //Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                                }

                                var message = new SendGridMessage();
                                message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                                message.Subject = Subject;
                                string emailid = _db.Multiple_Email_DMO.Where(L => L.HRME_Id == data.HRME_Id && L.HRMEM_DeFaultFlag == "default").Select(L => L.HRMEM_EmailId).FirstOrDefault();

                                long Id = _db.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                                string To_Mail = _db.Multiple_Email_DMO.Where(A => A.HRME_Id == Id && A.HRMEM_DeFaultFlag == "default").Select(A => A.HRMEM_EmailId).FirstOrDefault();
                                //  string To_Mail = "punithnmsd2@gmail.com";
                                message.AddTo(emailid);
                                message.AddTo(To_Mail);
                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {
                                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                            Stream stream = response.GetResponseStream();
                                            message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                        }
                                    }
                                }

                                if (mailcc != null && mailcc != "")
                                {
                                    message.AddCc(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    message.AddBcc(mailbcc);
                                }
                                message.HtmlContent = Mailmsg;
                                var client = new SendGridClient(sengridkey);
                                client.SendEmailAsync(message).Wait();
                                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                                {
                                    var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "RTOvialtion" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                                    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                                    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                    cmd.CommandText = "IVRM_Email_Outgoing";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                                    {
                                        Value = emailid
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                                    {
                                        Value = Mailmsg
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                                    {
                                        Value = modulename[0]
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = data.MI_Id
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

                                    }
                                }
                            }
                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }
                }
                //RTO_Violation_ObservationDMO
                //RTO_Violation_ObservationDMO

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
              //  data.returnval = "admin";
            }
            return data;
        }
        //onchange
        public RTO_Violation_ObservationDTO onchange(RTO_Violation_ObservationDTO data)
        {
            try
            {
                if (data.RTOVOB_Id > 0)
                {
                    var obj = _RTOContext.RTO_Violation_ObservationDMO.Where(R => R.RTOVOB_Id == data.RTOVOB_Id).FirstOrDefault();
                    if(obj.RTOVOB_ActiveFlag==true)
                    {
                        obj.RTOVOB_ActiveFlag = false;
                    }
                    else
                    {
                        obj.RTOVOB_ActiveFlag = true;
                    }
                    _RTOContext.Update(obj);
                    int i = _RTOContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
                    }
                }
                else
                {
                    data.returnval = "admin";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public RTO_Violation_ObservationDTO savedataSource(RTO_Violation_ObservationDTO data)
        {
            try
            {



            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public RTO_Violation_ObservationDTO clientDecative(RTO_Violation_ObservationDTO data)
        {
            try
            {

            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
                data.returnval = "admin";
            }
            return data;
        }
    }
}
