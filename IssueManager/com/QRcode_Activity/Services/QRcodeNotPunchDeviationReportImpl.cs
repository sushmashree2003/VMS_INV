using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager.HRMasterArea;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity;
using IssueManager.com.QRcode_Activity.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
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

namespace IssueManager.com.QRcode_Activity.Services
{
    public class QRcodeNotPunchDeviationReportImpl : Interfaces.QRcodeNotPunchDeviationReportInterface
    {

        public HRMasterAreaContext _INVContext;
        ILogger<QRcodeNotPunchDeviationReportImpl> _logInv;
        public DomainModelMsSqlServerContext _db;

        public QRcodeNotPunchDeviationReportImpl(HRMasterAreaContext InvContext, ILogger<QRcodeNotPunchDeviationReportImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public QRcodeNotPunchDeviationReportDTO getloaddata(QRcodeNotPunchDeviationReportDTO data)
        {


            data.fill_buildings = _INVContext.HR_Master_Building_con.Where(a => a.MI_Id == data.MI_Id).ToArray();

            List<MasterRoleType> allroles = new List<MasterRoleType>();
            allroles = _INVContext.masterRoleType.Where(t => t.IVRMRT_Role == "House Keeping" || t.IVRMRT_Role == "Security" || t.IVRMRT_Role == "Hardware Asset Keeping").ToList();
            data.fillroletype = allroles.ToArray();


            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_empnamedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = data.IVRMRT_Id });


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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.emp_deatils = retObject.ToArray();
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



        public QRcodeNotPunchDeviationReportDTO get_Floors(QRcodeNotPunchDeviationReportDTO data)
        {
          
            List<long> list = new List<long>();
            if (data.multipletype!=null)
            {
                foreach(var i in data.multipletype)
                {
                    list.Add(i.HRMB_Id);
                }
            }
            data.fill_Floors = (from a in _INVContext.HR_Master_Floor_con
                                from b in _INVContext.HR_Master_Building_con

                                where (a.HRMF_ActiveFlag == true && a.HRMB_Id == b.HRMB_Id && a.MI_Id==b.MI_Id && list.Contains(a.HRMB_Id))
                                   select new QRcodeNotPunchDeviationReportDTO
                                   {
                                       HRMB_Id = b.HRMB_Id,
                                       HRMF_Id = a.HRMF_Id,
                                       HRMB_BuildingName = b.HRMB_BuildingName,
                                       HRMF_FloorName = a.HRMF_FloorName,

                                   }
                     ).Distinct().ToArray();

            return data;
        }




        public QRcodeNotPunchDeviationReportDTO get_Areas(QRcodeNotPunchDeviationReportDTO data)
        {

            List<long> list = new List<long>();
            if (data.multipletypea != null)
            {
                foreach (var i in data.multipletypea)
                {
                    list.Add(i.HRMF_Id);
                }
            }
            data.fill_Areas = (from c in _INVContext.HRMasterAreaDMO


                               where (c.HRMAR_ActiveFlag == true && list.Contains(c.HRMF_Id))
                                select new QRcodeNotPunchDeviationReportDTO
                                {
                                    HRMAR_Id = c.HRMAR_Id,
                                    HRMAR_AreaName = c.HRMAR_AreaName,
                                }
                     ).Distinct().ToArray();

            return data;
        }





        public QRcodeNotPunchDeviationReportDTO getEmployeeName(QRcodeNotPunchDeviationReportDTO data)
        {

           

            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_empnamedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = data.IVRMRT_Id });


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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.emp_deatils = retObject.ToArray();
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

        
        public QRcodeNotPunchDeviationReportDTO QRcodedeviation(QRcodeNotPunchDeviationReportDTO data)
        {
            try
            {
                
                string hrmeids = "0";
                string HRMAR_Id = "0";
                string HRMB_Id = "0";
                string HRMF_Id = "0";
                if (data.arrayuserEmp !=null && data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrmeids = hrmeids + "," + ue.UserId;
                    }
                }
                if(data.multitypesAreaee !=null)
                {
                    foreach (var ue in data.multitypesAreaee)
                    {
                        HRMAR_Id = HRMAR_Id + "," + ue.HRMAR_ID;
                    }
                }
                if(data.multipletype != null)
                {
                    foreach (var ue in data.multipletype)
                    {
                        HRMB_Id = HRMB_Id + "," + ue.HRMB_Id;
                    }
                }
                if(data.multipletypea !=null)
                {
                    foreach (var ue in data.multipletypea)
                    {
                        HRMF_Id = HRMF_Id + "," + ue.HRMF_Id;
                    }
                }
                string fromDate = "";
                string Todate = "";

                if (data.startdate != null && data.enddate != null)
                {
                    fromDate = data.startdate.Value.ToString("yyyy-MM-dd");
                    Todate = data.enddate.Value.ToString("yyyy-MM-dd");
                }

                //******************************Deviation Remarks & Deviation Calculation Report ************************************//

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRcode_NotPunch_Deviationreportdetails";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@userid",
                    SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });
                 //   cmd.Parameters.Add(new SqlParameter("@Flag",
                 //SqlDbType.VarChar)
                 //   {
                 //       Value = "nonpunch"
                 //   });

                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                   SqlDbType.DateTime)
                    {
                        // Value = data.startdate
                        Value = fromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.DateTime)
                    {
                        //Value = data.enddate
                        Value = Todate
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMAR_ID",
                  SqlDbType.VarChar)
                    {
                        Value = HRMAR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMB_Id",
                    SqlDbType.VarChar)
                    {
                        Value = HRMB_Id
                    });

                    //HRMF_Id
                    cmd.Parameters.Add(new SqlParameter("@HRMF_Id",
                    SqlDbType.VarChar)
                    {
                        Value = HRMF_Id
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }

                        data.get_deviationreport = retObject.ToArray();
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

        public QRcodeNotPunchDeviationReportDTO SendMail(QRcodeNotPunchDeviationReportDTO data)
        {



            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "Notpunched" && e.ISES_MailActiveFlag == true).ToList();
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
                    string emailid = "rajkumarraj960522@gmail.com";
                   // string emailid = _db.Multiple_Email_DMO.Where(L => L.HRME_Id == data.HRME_Id && L.HRMEM_DeFaultFlag == "default").Select(L => L.HRMEM_EmailId).FirstOrDefault();

                    message.AddTo(emailid);
                  //  message.AddTo(To_Mail);
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



                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "RTOvialtion" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = ""
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }
    }
}
