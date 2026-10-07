using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using PreadmissionDTOs.com.vaps.VMS.TADA;
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
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Services
{
    public class TADA_Advance_TourIMPL : Interface.TADA_Advance_TourInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;
        public TADA_Advance_TourIMPL(IssueManagerContext context, DomainModelMsSqlServerContext dbcontext)
        {
            _context = context;
            _db = dbcontext;
        }
        public TADA_Advance_TourDTO getalldata(TADA_Advance_TourDTO data)
        {
            try
            {
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_Route_EmpCategory";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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

                        data.GetCategoryArray = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //GetPlanerdata
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_RoutePlanDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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

                        data.GetPlanerdata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.clientlist = _context.clientTable.Where(R => R.ISMMCLT_ActiveFlag == true).Distinct().ToArray();

                //TADA Advance Tour Lead Report
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Advance_Tour_Lead_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId",
                      SqlDbType.BigInt)
                    {
                        Value = data.UserId
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

                        data.designationArray = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_RouteDesionMakingMD";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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

                        data.Tour_Details = retObject.ToArray();


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

        //get_Wallet_load
        public TADA_Advance_TourDTO get_Wallet_load(TADA_Advance_TourDTO data)
        {
            try
            {
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                //data.GetPlanerdata = _context.ISM_Emp_RoutePlanDMO.Where(R => R.IER_EmpId == data.HRME_Id).Distinct().ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Emp_RoutePlan_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Userid",
                      SqlDbType.VarChar)
                    {
                        Value = data.UserId
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

                        data.GetPlanerdata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_WALLET_DEATILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
                    cmd.Parameters.Add(new SqlParameter("@IER_Id",
                      SqlDbType.VarChar)
                    {
                        Value = 0
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

                        data.Tour_Details = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //TourPlanDetails
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_WALLET_DEATILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                    {
                        Value = "2"
                    });
                    cmd.Parameters.Add(new SqlParameter("@IER_Id",
                      SqlDbType.VarChar)
                    {
                        Value = 0
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

                        data.TourPlanDetails = retObject.ToArray();


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
        //get_Wallet_Client
        public TADA_Advance_TourDTO get_Wallet_Client(TADA_Advance_TourDTO data)
        {
            try
            {
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                //TourPlanDetails
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_WALLET_DEATILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                    {
                        Value = "3"
                    });
                    cmd.Parameters.Add(new SqlParameter("@IER_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.TourPlanDetails = retObject.ToArray();


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
        public TADA_Advance_TourDTO saveallowence(TADA_Advance_TourDTO data)
        {
            try
            {
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.listEmployeeSales != null && data.listEmployeeSales.Length > 0)
                {
                    //@IERA_StatusFlg

                    foreach (var d in data.listEmployeeSales)
                    {
                        var contactExistsP = _context.Database.ExecuteSqlCommand("usp_i_Emp_RoutePlan @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.HRME_Id, data.PlanStartDate, data.PlanEndDate, d.ClientId, d.CategoryId, data.UserId, d.ISMSMPR_Id, data.PlanName, data.TourRemark, data.IERA_StatusFlg);
                        data.message = "save";
                    }

                    //

                    string mailcc = ""; string Mail_CC = "";
                    var checkmaxorder = (from P in _context.HR_PROCESSDMO
                                         from R in _context.HR_Process_Auth_OrderNoDMO
                                         where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "TATOUR"
                                         select new TADA_Advance_ApplyDTO
                                         {
                                             HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                             IVRMUL_Id = R.IVRMUL_Id
                                         }
                                         ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                    if (checkmaxorder.Count > 0)
                    {
                        data.UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;
                        long Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                        data.cc_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                    }
                    string emailidNew = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                    //
                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "TourMail" && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count == 0)
                    {
                        return data;
                    }

                    var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                    var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                    var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                    string Mailmsg = data.returnval;

                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string Subject = template[0].ISES_MailSubject.ToString();
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        var message = new SendGridMessage();
                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                            foreach (var c in ccmail)
                            {
                                if (c != "")
                                {
                                    message.AddBcc(c);

                                }
                            }

                        }
                        if (data.IERA_StatusFlg == "TL")
                        {
                            List<TADA_Advance_TourDTO> dtonewtemp = new List<TADA_Advance_TourDTO>();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "TADA_GETGREETINGCCBCC";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                               SqlDbType.BigInt)
                                {
                                    Value = data.HRME_Id
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
                                            dtonewtemp.Add(new TADA_Advance_TourDTO
                                            {
                                                Mail_CC = Convert.ToString(dataReader["cc"]),

                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (dtonewtemp != null && dtonewtemp.Count > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    Mail_CC = dtonewtemp[j].Mail_CC;

                                }
                                string[] ccmail = Mail_CC.Split(',');
                                foreach (var c in ccmail)
                                {
                                    if (c != "")
                                    {
                                        message.AddCc(c);

                                    }
                                }
                            }

                        }
                        else
                        {
                            if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                            {
                                string[] ccmail = template[0].ISES_MailCCId.Split(',');
                                foreach (var c in ccmail)
                                {
                                    if (c != "")
                                    {
                                        message.AddCc(c);

                                    }
                                }

                            }
                        }


                        if (data.cc_Mail != null && data.cc_Mail != "")
                        {
                            mailcc = data.cc_Mail;
                            message.AddCc(data.cc_Mail);
                        }
                        //ISES_MailBCCId

                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = "TADA TOUR  DETAILS";
                        string emailid = emailidNew;
                        message.AddTo(emailid);
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                }
                else
                {
                    var contactExistsP = _context.Database.ExecuteSqlCommand("usp_i_Emp_RoutePlan @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.HRME_Id, data.PlanStartDate, data.PlanEndDate, 0, 0, data.UserId, 0, data.PlanName, data.TourRemark, data.IERA_StatusFlg);
                    data.message = "save";
                    string mailcc = "";
                    string emailidNew = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                    //
                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "TourMail" && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count == 0)
                    {
                        return data;
                    }

                    var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                    var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                    var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                    string Mailmsg = data.returnval;

                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string Subject = template[0].ISES_MailSubject.ToString();
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        var message = new SendGridMessage();
                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                            foreach (var c in ccmail)
                            {
                                if (c != "")
                                {
                                    message.AddBcc(c);

                                }
                            }

                        }
                        if (data.IERA_StatusFlg == "TL")
                        {
                            string Mail_CC = "";
                            List<TADA_Advance_TourDTO> dtonewtemp = new List<TADA_Advance_TourDTO>();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "TADA_GETGREETINGCCBCC";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                               SqlDbType.BigInt)
                                {
                                    Value = data.HRME_Id
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
                                            dtonewtemp.Add(new TADA_Advance_TourDTO
                                            {
                                                Mail_CC = Convert.ToString(dataReader["cc"]),

                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (dtonewtemp != null && dtonewtemp.Count > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    Mail_CC = dtonewtemp[j].Mail_CC;

                                }
                                string[] ccmail = Mail_CC.Split(',');
                                foreach (var c in ccmail)
                                {
                                    if (c != "")
                                    {
                                        message.AddCc(c);

                                    }
                                }
                            }

                        }
                        else
                        {
                            if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                            {
                                string[] ccmail = template[0].ISES_MailCCId.Split(',');
                                foreach (var c in ccmail)
                                {
                                    if (c != "")
                                    {
                                        message.AddCc(c);

                                    }
                                }

                            }
                        }

                        if (data.cc_Mail != null && data.cc_Mail != "")
                        {
                            mailcc = data.cc_Mail;
                            message.AddCc(data.cc_Mail);
                        }


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = "TADA TOUR   INTIMATE";
                        string emailid = emailidNew;
                        message.AddTo(emailid);
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.message = "admin";
            }
            return data;
        }
        public TADA_Advance_TourDTO TadaAllwence(TADA_Advance_TourDTO data)
        {
            try
            {
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                //LeadList
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_Route_LeadsDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMRC_CategoryId",
                      SqlDbType.VarChar)
                    {
                        Value = data.IMRC_CategoryId
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

                        data.LeadList = retObject.ToArray();


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
        //deactive
        public TADA_Advance_TourDTO deactive(TADA_Advance_TourDTO data)
        {
            try
            {
                //IER_ID
                var resultone = _context.ISM_Emp_RoutePlanDMO.Single(t => t.IER_ID == data.IER_ID);
                if (resultone.ISM_Active == true)
                {
                    resultone.ISM_Active = false;
                    resultone.IER_UpdateDate = DateTime.Now;
                    resultone.IER_Updateby = data.UserId;
                    _context.Update(resultone);
                }
                else
                {
                    resultone.ISM_Active = true;
                    resultone.IER_UpdateDate = DateTime.Now;
                    resultone.IER_Updateby = data.UserId;
                    _context.Update(resultone);
                }
                int i = _context.SaveChanges();
                if (i > 0)
                {
                    data.message = "save";
                }
                if (i > 0)
                {
                    data.message = "notsave";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.message = "admin";
            }
            return data;
        }

        //EditData
        public TADA_Advance_TourDTO EditData(TADA_Advance_TourDTO data)
        {
            try
            {
                //IER_ID
                data.editdata = _context.ISM_Emp_RoutePlanDMO.Where(t => t.IER_ID == data.IER_ID).Distinct().ToArray();
                //editdataOthelist

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_RoutePlan_EditDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.editdataOthelist = retObject.ToArray();


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
                data.message = "admin";
            }
            return data;
        }
        //getView
        public TADA_Advance_TourDTO getView(TADA_Advance_TourDTO data)
        {
            try
            {
                //IER_ID
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_LeadRegionTourDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.editdataOthelist = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_RoutePlan_EditDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.GetPlanerdata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.clientlist = _context.clientTable.Where(R => R.ISMMCLT_ActiveFlag == true).Distinct().ToArray();
                var list = _context.TADA_AdavanceApplyDMO.Where(R => R.IER_ID == Convert.ToString(data.IER_ID)).ToList();
                if (list.Count > 0)
                {
                    data.VTADAAA_Id = list.FirstOrDefault().VTADAAA_Id;
                }
                else
                {
                    data.VTADAAA_Id = 0;
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_FoodAccomDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.Tour_Details = retObject.ToArray();


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
                data.message = "admin";
            }
            return data;
        }
        //updateallowence
        public TADA_Advance_TourDTO updateallowence(TADA_Advance_TourDTO data)
        {
            try
            {
                if (data.listEmployeeSales != null && data.listEmployeeSales.Length > 0)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                    var Routplan = _context.ISM_Emp_RoutePlan_CategoryDMO.Where(R => R.IER_ID == data.IER_ID).Distinct().ToList();
                    if (Routplan.Count > 0)
                    {
                        foreach (var d in Routplan)
                        {
                            _context.Remove(d);

                        }
                    }
                    var Routplanclient = _context.ISM_Emp_RoutePlan_ClientDetailsDMO.Where(R => R.IER_ID == data.IER_ID).Distinct().ToList();
                    if (Routplanclient.Count > 0)
                    {
                        foreach (var d in Routplanclient)
                        {
                            _context.Remove(d);

                        }
                    }
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        foreach (var d in data.listEmployeeSales)
                        {
                            var contactExistsP = _context.Database.ExecuteSqlCommand("usp_U_Emp_RoutePlan @p0, @p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.IER_ID, data.HRME_Id, data.PlanStartDate, data.PlanEndDate, d.ClientId, d.CategoryId, data.UserId, d.ISMSMPR_Id, data.PlanName, data.TourRemark);
                            data.message = "save";
                        }
                    }
                    else
                    {
                        data.message = "notsave";
                    }


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.message = "admin";
            }
            return data;
        }
        //CalculateTada
        public TADA_Advance_TourDTO CalculateTada(TADA_Advance_TourDTO data)
        {
            try
            {
                //IER_ID
                if (data.Plan_DepartureDetails != null && data.Plan_DepartureDetails.Length > 0)
                {
                    data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                    data.Travel_data = _context.VMS_TADA_Master_TravelDMO.Where(a => a.VTADAMT_ActiveFlag == true).ToArray();

                    foreach (var d in data.Plan_DepartureDetails)
                    {
                        var contactExistsP = _context.Database.ExecuteSqlCommand("usp_i_RoutePlan_DepartureDetails @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", d.IER_ID, d.IVRMMS_Id, d.IRPD_DepartureDate, d.IRPD_DepartureTime, d.IRPD_ArrivalDate, d.IRPD_ArrivalTime, data.UserId, d.IRPD_Order);
                        data.message = "save";
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_LeadRegionTourDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@IER_ID",
                          SqlDbType.VarChar)
                        {
                            Value = data.IER_ID
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

                            data.editdataOthelist = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //Tour_Details   //usp_s_FoodAccomDetails

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_FoodAccomDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@IER_ID",
                          SqlDbType.VarChar)
                        {
                            Value = data.IER_ID
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

                            data.Tour_Details = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_FoodAccomDetails_City";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@IER_ID",
                          SqlDbType.BigInt)
                        {
                            Value = data.IER_ID
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

                            data.EditArray = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_Tada_State_Names";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IER_ID",
                         SqlDbType.VarChar)
                        {
                            Value = data.IER_ID
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
                            data.StateList = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Category_Parameters";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IER_ID",
                          SqlDbType.VarChar)
                        {
                            Value = data.IER_ID
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

                            data.LeadList = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_get_Category_parameters";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IER_ID",
                          SqlDbType.VarChar)
                        {
                            Value = data.IER_ID
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

                            data.GetAdavancedata = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    data.message = "notsave";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.message = "admin";
            }
            return data;
        }

        //SaveTada
        public TADA_Advance_TourDTO SaveTada(TADA_Advance_TourDTO data)
        {
            try
            {
                data.returnvalue = false;
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                var Citylist = _context.IVRM_Master_CityDMO.Where(R => R.IVRMMCT_Name == "General").ToList();
                var listtada = _context.ISM_Emp_RoutePlanDMO.Where(R => R.IER_ID == data.IER_ID).ToList();

                var Departurelist = _context.ISM_RoutePlan_DepartureDetailsDMO.Where(R => R.IER_ID == data.IER_ID).OrderBy(R => R.IRPD_DepartureDate).ToList();
                var ArrivalTimelist = _context.ISM_RoutePlan_DepartureDetailsDMO.Where(R => R.IER_ID == data.IER_ID).OrderByDescending(R => R.IRPD_ArrivalDate).ToList();
                var Getdesignation = (from P in _context.Staff_User_Login
                                      from R in _context.MasterEmployee
                                      where (P.Emp_Code == R.HRME_Id && P.Id == data.UserId)
                                      select new TADA_Advance_TourDTO
                                      {
                                          HRMDES_Id = R.HRMDES_Id

                                      }
                                    ).ToArray();
                if (data.VTADAAA_Id == 0 && data.HRME_Id > 0 && data.IER_ID > 0)
                {

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TADA_Advance_Apply_Record_Checking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
                        string FromDate = listtada.FirstOrDefault().IER_Plan_StartDate.Value.ToString("yyyy-MM-dd");
                        string ToDate = listtada.FirstOrDefault().IER_Plan_EndDate.Value.ToString("yyyy-MM-dd");
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
                        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        data.duplicate = cmd.Parameters["@Result"].Value.ToString();
                    }
                    if (data.duplicate == "0")
                    {

                        TADA_AddvanceApplyDMO obj = new TADA_AddvanceApplyDMO();
                        obj.VTADAAA_Id = data.VTADAAA_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.VTADAAA_AppliedDate = DateTime.Now;
                        obj.VTADAAA_FromDate = listtada.FirstOrDefault().IER_Plan_StartDate;
                        obj.VTADAAA_ToDate = listtada.FirstOrDefault().IER_Plan_EndDate;
                        obj.VTADAAA_ClientId = 0;
                        obj.VTADAAA_Remarks = listtada.FirstOrDefault().IER_Remarks;
                        obj.VTADAAA_TotalAppliedAmount = data.VTADAAA_TotalAppliedAmount;
                        obj.IVRMMCT_Id = Citylist.FirstOrDefault().IVRMMCT_Id;
                        obj.VTADAAA_DepartureTime = Departurelist.FirstOrDefault().IRPD_DepartureTime;
                        obj.VTADAAA_ArrivalTime = ArrivalTimelist.FirstOrDefault().IRPD_ArrivalTime;
                        obj.VTADAAA_StatusFlg = "Pending";
                        obj.VTADAAA_ActiveFlg = true;
                        obj.VTADAAA_CreatedBy = data.UserId;
                        obj.VTADAAA_UpdatedBy = data.UserId;
                        obj.VTADAAA_CreatedDate = DateTime.Now;
                        obj.VTADAAA_UpdatedDate = DateTime.Now;
                        obj.IER_ID = data.IER_ID.ToString();
                        _context.Add(obj);
                        if (data.AllowenceArray != null && data.AllowenceArray.Length > 0)
                        {
                            for (int c = 0; c < data.AllowenceArray.Length; c++)
                            {
                                TADA_AddvanceApplyDetailsDMO dobj = new TADA_AddvanceApplyDetailsDMO();
                                dobj.VTADAAA_Id = obj.VTADAAA_Id;
                                dobj.VTADAAAD_ExpenditureHead = data.AllowenceArray[c].Type;
                                dobj.VTADAAAD_Amount = data.AllowenceArray[c].Amount;
                                dobj.VTADAAAD_Remarks = data.AllowenceArray[c].Remarks;
                                dobj.VTADAAAD_Slots = data.AllowenceArray[c].VTADAAAD_Slots;
                                dobj.VTADAAAD_TotalSlots = data.AllowenceArray[c].VTADAAAD_TotalSlots;
                                dobj.VTADAAAD_ActiveFlg = true;
                                dobj.VTADAAAD_StatusFlg = "Pending";
                                dobj.VTADAAAD_CreatedBy = data.UserId;
                                dobj.VTADAAAD_CreatedDate = DateTime.Now;
                                dobj.VTADAAAD_UpdatedBy = data.UserId;
                                dobj.VTADAAAD_UpdatedDate = DateTime.Now;
                                _context.Add(dobj);
                            }
                        }
                        if (data.multiple_client != null && data.multiple_client.Length > 0)
                        {
                            for (int x = 0; x < data.multiple_client.Length; x++)
                            {
                                VMS_TADDA_MultipleClient_FilesDMO vtdmf = new VMS_TADDA_MultipleClient_FilesDMO();
                                vtdmf.VTADAAA_Id = obj.VTADAAA_Id;
                                vtdmf.ISMMCLT_Id = data.multiple_client[x].ISMMCLT_Id;
                                vtdmf.VTADDAMCF_Remarks = data.multiple_client[x].VTADDAMCF_Remarks;
                                vtdmf.VTADDAMCF_StatusFlg = "P";
                                vtdmf.VTADDAMCF_ActiveFlag = true;
                                vtdmf.VTADDAMCF_CreatedDate = DateTime.Now;
                                vtdmf.VTADDAMCF_UpdatedDate = DateTime.Now;
                                vtdmf.VTADDAMCF_CreatedBy = data.UserId;
                                vtdmf.VTADDAMCF_UpdatedBy = data.UserId;
                                _context.Add(vtdmf);
                            }
                        }
                        if (data.VMSTADAAmountDTO != null && data.VMSTADAAmountDTO.Length > 0)
                        {
                            for (int x = 0; x < data.VMSTADAAmountDTO.Length; x++)
                            {
                                VMS_TADA_Advance_Apply_CityDMO VTAA = new VMS_TADA_Advance_Apply_CityDMO();
                                VTAA.VTADAAA_Id = obj.VTADAAA_Id;
                                VTAA.VTADAAAC_FromState = data.VMSTADAAmountDTO[x].VTADAAAC_FromState;
                                VTAA.VTADAAAC_ToState = data.VMSTADAAmountDTO[x].VTADAAAC_ToState;
                                //VTAA.HRMDES_Id = Getdesignation[0].HRMDES_Id;
                                VTAA.HRMDES_Id = Getdesignation.FirstOrDefault().HRMDES_Id;
                                VTAA.VTADAMT_Id = data.VMSTADAAmountDTO[x].VTADAMT_Id;
                                VTAA.VTADAAAC_Amount = data.VMSTADAAmountDTO[x].VTADAAAC_Amount;
                                VTAA.VTADAMD_Id = data.VMSTADAAmountDTO[x].VTADAMD_Id;
                                VTAA.IVRMMCT_Id= data.VMSTADAAmountDTO[x].IVRMMCT_Id; 
                                VTAA.VTADAAAC_CreatedDate = DateTime.Now;
                                VTAA.VTADAAAC_UpdatedDate = DateTime.Now;
                                VTAA.VTADAAAC_CreatedBy = data.UserId;
                                VTAA.VTADAAAC_UpdatedBy = data.UserId;
                                _context.Add(VTAA);
                            }
                        }
                        foreach (var p in data.filelist)
                        {
                            var Category = _context.Database.ExecuteSqlCommand("ISM_Insert_Category_Parameter @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.IER_ID, p.HRME_Id, p.ILRT_FileName, p.ILRT_FilePath, p.ILRT_Remarks, 1, DateTime.Now, DateTime.Now, data.UserId, data.UserId);
                            if (Category > 0)
                            {

                            }
                            else
                            {

                            }
                        }


                        int a = _context.SaveChanges();
                        if (a > 0)
                        {
                            data.returnvalue = true;
                            data.returnval = "Insert";
                            string mailcc = "";
                            var checkmaxorder = (from P in _context.HR_PROCESSDMO
                                                 from R in _context.HR_Process_Auth_OrderNoDMO
                                                 where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "TATOUR"
                                                 select new TADA_Advance_ApplyDTO
                                                 {
                                                     HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                                     IVRMUL_Id = R.IVRMUL_Id
                                                 }
                                                 ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                            if (checkmaxorder.Count > 0)
                            {
                                data.UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;
                                long Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                                data.cc_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            }
                            string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == obj.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            if (data.cc_Mail != null)
                            {
                                mailcc = data.cc_Mail;
                            }


                            Dictionary<string, string> val = new Dictionary<string, string>();
                            var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "TourMail" && e.ISES_MailActiveFlag == true).ToList();
                            if (template.Count == 0)
                            {
                                return data;
                            }

                            var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                            string Mailmsg = data.message;

                            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                            alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                            if (alldetails.Count > 0)
                            {
                                string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                                string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                                string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                                Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                                string Subject = template[0].ISES_MailSubject.ToString();
                                string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                                var message = new SendGridMessage();
                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                                    foreach (var c in ccmail)
                                    {
                                        if (c != "")
                                        {
                                            message.AddBcc(c);

                                        }
                                    }

                                }

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    string[] ccmail = template[0].ISES_MailCCId.Split(',');
                                    foreach (var c in ccmail)
                                    {
                                        if (c != "")
                                        {
                                            message.AddCc(c);

                                        }
                                    }

                                }
                                if (data.cc_Mail != null && data.cc_Mail != "")
                                {
                                    mailcc = data.cc_Mail;
                                    message.AddCc(data.cc_Mail);
                                }
                                //ISES_MailBCCId

                                message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                                message.Subject = "TADA TOUR  DETAILS";
                                message.AddTo(emailid);
                                message.HtmlContent = Mailmsg;
                                var client = new SendGridClient(sengridkey);
                                client.SendEmailAsync(message).Wait();

                            }
                            //  sendmail(data.MI_Id, emailid, "TADAAdavance_Apply", obj.VTADAAA_Id, mailcc);
                        }
                        else
                        {
                            data.returnvalue = true;
                            data.returnval = "Failed";
                        }
                    }
                    else
                    {
                        data.returnvalue = true;
                        data.returnval = "Duplicate";
                    }
                }
                if (data.VTADAAA_Id > 0 && data.HRME_Id > 0 && data.IER_ID > 0)
                {
                    var resultone = _context.TADA_AdavanceApplyDMO.Where(t => t.VTADAAA_Id == data.VTADAAA_Id && t.MI_Id == data.MI_Id).FirstOrDefault();
                    resultone.MI_Id = data.MI_Id;
                    resultone.HRME_Id = data.HRME_Id;
                    resultone.IVRMMCT_Id = Citylist.FirstOrDefault().IVRMMCT_Id;
                    resultone.VTADAAA_AppliedDate = DateTime.Now;
                    resultone.VTADAAA_FromDate = listtada.FirstOrDefault().IER_Plan_StartDate;
                    resultone.VTADAAA_ToDate = listtada.FirstOrDefault().IER_Plan_EndDate;
                    resultone.VTADAAA_ClientId = 0;
                    resultone.VTADAAA_Remarks = listtada.FirstOrDefault().IER_Remarks;
                    resultone.VTADAAA_TotalAppliedAmount = data.VTADAAA_TotalAppliedAmount;
                    resultone.VTADAAA_UpdatedBy = data.UserId;
                    resultone.VTADAAA_UpdatedDate = DateTime.Now;
                    _context.Update(resultone);
                    if (data.AllowenceArray != null && data.AllowenceArray.Length > 0)
                    {
                        var typedetails = _context.TADA_AdvanceApplyDetailsDMO.Where(a => a.VTADAAA_Id == data.VTADAAA_Id).ToList();
                        if (typedetails != null && typedetails.Count > 0)
                        {
                            foreach (var d in typedetails)
                            {
                                _context.Remove(d);
                            }
                        }

                        foreach (var c in data.AllowenceArray)
                        {
                            TADA_AddvanceApplyDetailsDMO dobj = new TADA_AddvanceApplyDetailsDMO();
                            dobj.VTADAAA_Id = data.VTADAAA_Id;
                            dobj.VTADAAAD_ExpenditureHead = c.Type;
                            dobj.VTADAAAD_Amount = c.Amount;
                            dobj.VTADAAAD_Remarks = c.Remarks;
                            dobj.VTADAAAD_Slots = c.VTADAAAD_Slots;
                            dobj.VTADAAAD_TotalSlots = c.VTADAAAD_TotalSlots;
                            dobj.VTADAAAD_ActiveFlg = true;
                            dobj.VTADAAAD_CreatedBy = data.UserId;
                            dobj.VTADAAAD_CreatedDate = DateTime.Now;
                            dobj.VTADAAAD_UpdatedBy = data.UserId;
                            dobj.VTADAAAD_UpdatedDate = DateTime.Now;
                            dobj.VTADAAAD_StatusFlg = "Pending";
                            _context.Add(dobj);
                        }

                    }
                    if (data.multiple_client != null && data.multiple_client.Length > 0)
                    {
                        var checkexist = _context.VMS_TADDA_MultipleClient_FilesDMO.Where(d => data.VTADAAA_Id == d.VTADAAA_Id).ToList();
                        if (checkexist.Count > 0)
                        {
                            foreach (var y in checkexist)
                            {
                                _context.Remove(y);
                            }
                        }
                        for (int x = 0; x < data.multiple_client.Length; x++)
                        {


                            VMS_TADDA_MultipleClient_FilesDMO vtdmf = new VMS_TADDA_MultipleClient_FilesDMO();
                            vtdmf.VTADAAA_Id = data.VTADAAA_Id;
                            vtdmf.ISMMCLT_Id = data.multiple_client[x].ISMMCLT_Id;
                            vtdmf.VTADDAMCF_Remarks = data.multiple_client[x].VTADDAMCF_Remarks;
                            vtdmf.VTADDAMCF_StatusFlg = "P";
                            vtdmf.VTADDAMCF_ActiveFlag = true;
                            vtdmf.VTADDAMCF_CreatedDate = DateTime.Now;
                            vtdmf.VTADDAMCF_UpdatedDate = DateTime.Now;
                            vtdmf.VTADDAMCF_CreatedBy = data.UserId;
                            vtdmf.VTADDAMCF_UpdatedBy = data.UserId;
                            _context.Add(vtdmf);
                        }
                    }
                    if (data.VMSTADAAmountDTO != null && data.VMSTADAAmountDTO.Length > 0)
                    {
                        var check = _context.VMS_TADA_Advance_Apply_CityDMO.Where(d => d.VTADAAA_Id == data.VTADAAA_Id).ToList();
                        if (check.Count > 0)
                        {
                            foreach (var y in check)
                            {
                                _context.Remove(y);
                            }
                        }
                        for (int x = 0; x < data.VMSTADAAmountDTO.Length; x++)
                        {


                            VMS_TADA_Advance_Apply_CityDMO VTAA = new VMS_TADA_Advance_Apply_CityDMO();
                            VTAA.VTADAAA_Id = data.VTADAAA_Id;
                            VTAA.VTADAAAC_FromState = data.VMSTADAAmountDTO[x].VTADAAAC_FromState;
                            VTAA.VTADAAAC_ToState = data.VMSTADAAmountDTO[x].VTADAAAC_ToState;
                            
                            VTAA.HRMDES_Id = Getdesignation.FirstOrDefault().HRMDES_Id;
                            VTAA.VTADAAAC_Amount = data.VMSTADAAmountDTO[x].VTADAAAC_Amount;
                            VTAA.VTADAMT_Id = data.VMSTADAAmountDTO[x].VTADAMT_Id;
                            VTAA.VTADAMD_Id = data.VMSTADAAmountDTO[x].VTADAMD_Id;
                            VTAA.VTADAAAC_CreatedDate = DateTime.Now;
                            VTAA.VTADAAAC_UpdatedDate = DateTime.Now;
                            VTAA.VTADAAAC_CreatedBy = data.UserId;
                            VTAA.VTADAAAC_UpdatedBy = data.UserId;
                            VTAA.IVRMMCT_Id = data.VMSTADAAmountDTO[x].IVRMMCT_Id;
                            _context.Add(VTAA);
                        }
                    }
                    foreach (var p in data.filelist)
                    {
                        var Category = _context.Database.ExecuteSqlCommand("ISM_Insert_Category_Parameter @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.IER_ID, p.HRME_Id, p.ILRT_FileName, p.ILRT_FilePath, p.ILRT_Remarks, 1, DateTime.Now, DateTime.Now, data.UserId, data.UserId);
                        if (Category > 0)
                        {

                        }
                        else
                        {

                        }
                    }

                    var iJ = _context.SaveChanges();
                    if (iJ > 0)
                    {
                        data.returnvalue = true;
                        data.returnval = "Update";
                        string mailcc = "";
                        var checkmaxorder = (from P in _context.HR_PROCESSDMO
                                             from R in _context.HR_Process_Auth_OrderNoDMO
                                             where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "TATOUR"
                                             select new TADA_Advance_ApplyDTO
                                             {
                                                 HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                                 IVRMUL_Id = R.IVRMUL_Id
                                             }
                                                  ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                        if (checkmaxorder.Count > 0)
                        {
                            data.UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;
                            long Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                            data.cc_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        }
                        string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        if (data.cc_Mail != null)
                        {
                            mailcc = data.cc_Mail;
                        }


                        Dictionary<string, string> val = new Dictionary<string, string>();
                        var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "TourMail" && e.ISES_MailActiveFlag == true).ToList();
                        if (template.Count == 0)
                        {
                            return data;
                        }

                        var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                        var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                        var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                        string Mailmsg = data.message;

                        List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                        alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                        if (alldetails.Count > 0)
                        {
                            string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                            string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                            string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                            Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                            string Subject = template[0].ISES_MailSubject.ToString();
                            string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                            var message = new SendGridMessage();
                            if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                            {
                                string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                                foreach (var c in ccmail)
                                {
                                    if (c != "")
                                    {
                                        message.AddBcc(c);

                                    }
                                }

                            }

                            if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                            {
                                string[] ccmail = template[0].ISES_MailCCId.Split(',');
                                foreach (var c in ccmail)
                                {
                                    if (c != "")
                                    {
                                        message.AddCc(c);

                                    }
                                }

                            }
                            if (data.cc_Mail != null && data.cc_Mail != "")
                            {
                                mailcc = data.cc_Mail;
                                message.AddCc(data.cc_Mail);
                            }
                            //ISES_MailBCCId

                            message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                            message.Subject = "TADA TOUR  DETAILS";
                            message.AddTo(emailid);
                            message.HtmlContent = Mailmsg;
                            var client = new SendGridClient(sengridkey);
                            client.SendEmailAsync(message).Wait();

                        }


                    }
                    else
                    {
                        data.returnvalue = true;
                        data.returnval = "UpdateFailed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Get_TourList
        //public TADA_Advance_TourDTO Get_TourList(TADA_Advance_TourDTO data)
        //{
        //    try
        //    {



        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //        data.message = "admin";
        //    }
        //    return data;
        //}

        public TADA_Advance_TourDTO Get_TourList(TADA_Advance_TourDTO data)
        {
            try
            {
                //data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                string HRME_Id = "0";

                if (data.filelist != null && data.filelist.Length > 0)
                {
                    foreach (var d in data.filelist)
                    {
                        HRME_Id = HRME_Id + "," + d.HRME_Id;
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "TourPlan_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Hrme_ID",
                      SqlDbType.VarChar)
                    {
                        Value = HRME_Id
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

                        data.Tour_Details = retObject.ToArray();
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
                data.message = "admin";
            }
            return data;
        }
        //TourPlanDates
        public TADA_Advance_TourDTO TourPlanDates(TADA_Advance_TourDTO data)
        {
            try
            {
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                string FromDate = data.PlanStartDate.Value.ToString("yyyy-MM-dd");
                string ToDate = data.PlanEndDate.Value.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TOUR_Advance_Apply_Record_Checking";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
                    cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var data1 = cmd.ExecuteNonQuery();
                    data.duplicate = cmd.Parameters["@Result"].Value.ToString();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.message = "admin";
            }
            return data;
        }
        public string sendmail(long MI_Id, string Email, string Template, long UserID, string mailcc)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        // mailcc = "";
                        string mailbcc = "";
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
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
                    }
                    else
                    {


                        // mailcc = "";
                        string mailbcc = "";
                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            // Value = Mailcontent
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public TADA_Advance_TourDTO getload(TADA_Advance_TourDTO data)
        {
            try
            {
                //load data
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISM_LeadRouteStatusRemark_New ";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId",
                      SqlDbType.BigInt)
                    {
                        Value = data.UserId
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

                        data.leadroutestatusremark = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "usp_s_LdApprvRouteDetMob";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@UserId",
                //      SqlDbType.BigInt)
                //    {
                //        Value = data.UserId
                //    });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }

                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }

                //        data.getloadarray = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
                //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_RootPlanName";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId",
                      SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                    {
                        Value = "1"
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

                        data.TourPlanDetails = retObject.ToArray();
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
        //TADA_Advance_Tour_Lead
        //Lead_Update
        public TADA_Advance_TourDTO Lead_Update(TADA_Advance_TourDTO data)
        {
            try
            {
                //load data
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISM_LeadRouteStatusRemark_New ";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserId",
                      SqlDbType.BigInt)
                    {
                        Value = data.UserId
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

                        data.leadroutestatusremark = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_TourLeadLIst";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.IER_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
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

                        data.getloadarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_Advance_TourDTO savedata(TADA_Advance_TourDTO data)
        {
            try
            {
                var contactExistsP = _context.Database.ExecuteSqlCommand("usp_LeadSelect_RoutePlan @p0,@p1,@p2,@p3,@p4,@p5", data.IER_ID, data.UserId, data.IER_Remarks, data.ILRR_Lat, data.ILRR_Long, data.ISMSLEDM_DemoExecuted);
                data.message = "save";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "not save";
            }
            return data;
        }
        public TADA_Advance_TourDTO savedta(TADA_Advance_TourDTO data)
        {
            try
            {
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                if (data.IER_ID > 0 && data.IERC_ID > 0 && data.HRME_Id > 0)
                {
                    //var Olddata = _context.ISM_Lead_RouteTADA_DMO.Where(R => R.IER_ID == data.IER_ID && R.IERC_ID == data.IERC_ID).Distinct().ToArray();
                    //if(Olddata !=null  && Olddata.Length > 0)
                    //{
                    //    foreach(var d  in Olddata)
                    //    {
                    //        _context.Remove(d);
                    //    }
                    //}

                    for (int b = 0; b < data.filelist.Length; b++)
                    {
                        ISM_Lead_RouteTADA_DMO obj = new ISM_Lead_RouteTADA_DMO();
                        obj.IER_ID = data.IER_ID;
                        obj.IERC_ID = data.IERC_ID;
                        obj.ILRT_FilePath = data.filelist[b].ILRT_FilePath;
                        obj.ILRT_FileName = data.filelist[b].ILRT_FileName;
                        obj.ILRT_Remarks = data.filelist[b].ILRT_Remarks;
                        obj.ILRT_Cost = data.filelist[b].ILRT_Cost;
                        obj.ILRT_StatusFlg = "Pending";
                        obj.ILRT_CreatedDate = DateTime.Now;
                        obj.ILRT_UpdateDate = DateTime.Now;
                        obj.ILRT_Createdby = data.UserId;
                        obj.ILRT_Updateby = data.UserId;
                        obj.ILRT_ActiveFlag = true;
                        obj.ILRT_ApprovedFlg = false;
                        _context.Add(obj);
                        var contactExistsP = _context.Database.ExecuteSqlCommand("usp_i_Route_Transaction @p0,@p1,@p2", data.HRME_Id, data.IER_ID, data.filelist[b].ILRT_Cost);

                        //usp_i_Route_Transaction
                    }
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {

                        data.message = "Saved";
                    }
                    else
                    {
                        data.message = "FailedSave";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }

        //Tadadata
        public TADA_Advance_TourDTO Tadadata(TADA_Advance_TourDTO data)
        {
            try
            {
                if (data.IER_ID > 0 && data.IERC_ID > 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TADA_ROOPt_Upload";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@IER_ID",
                          SqlDbType.BigInt)
                        {
                            Value = data.IER_ID
                        });
                        cmd.Parameters.Add(new SqlParameter("@IERC_ID",
                        SqlDbType.BigInt)
                        {
                            Value = data.IERC_ID
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

                            data.getloadarray = retObject.ToArray();
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
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }

        //FinalSubMission
        public TADA_Advance_TourDTO FinalSubMission(TADA_Advance_TourDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_ADVANCE_TOUR_FOR_APPROVAL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.VTADAAA_Id
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
                        data.EditArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_RoutePaymentTime";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
                        data.EditArrayTwo = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Checkout_Assests_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.GetArray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //TADA_RouteDetailsApproval

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_RouteDetailsApproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
                        data.TourPlanDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //getloadarray
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_AdavanceFileList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
                        data.getloadarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //LeadLists
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_RouteFinalReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
                        data.LeadLists = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //FinalSumbmission
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_FianSubmissionAmoutbyper";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
                        data.FinalSumbmission = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //ApprovalSubmission
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_FinalSubmissionAmount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
                        data.ApprovalSubmission = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //addedd

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Route_EmpCategory";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.GetCategoryArray = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //Concal Lop

                //ConcalLopdata
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_ConCalLop";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.ConcalLopdata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //GpsLocationTada
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_Tracking";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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
                        data.GpsLocationTada = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //adavanceDetail
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_TADA_ADavanceMltiple_SDetailsAmt_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.VTADAAA_Id
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

                        data.product_dd_s = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Category_Parameters";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.editdataOthelist = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_get_Category_parameters";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
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

                        data.GetAdavancedata = retObject.ToArray();


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
                data.message = "admin";
            }
            return data;
        }

        //SaveFinalSubMission
        public TADA_Advance_TourDTO SaveFinalSubMission(TADA_Advance_TourDTO data)
        {
            try
            {
                //SaveFinalSubMission
                var listtada = _context.ISM_Emp_RoutePlanDMO.Where(R => R.IER_ID == data.IER_ID).ToList();
                string FromDate = listtada.FirstOrDefault().IER_Plan_StartDate.Value.ToString("yyyy-MM-dd");
                string ToDate = listtada.FirstOrDefault().IER_Plan_EndDate.Value.ToString("yyyy-MM-dd");
                var Citylist = _context.IVRM_Master_CityDMO.Where(R => R.IVRMMCT_Name == "General").ToList();
                var Departurelist = _context.ISM_RoutePlan_DepartureDetailsDMO.Where(R => R.IER_ID == data.IER_ID).OrderBy(R => R.IRPD_DepartureDate).ToList();
                var ArrivalTimelist = _context.ISM_RoutePlan_DepartureDetailsDMO.Where(R => R.IER_ID == data.IER_ID).OrderByDescending(R => R.IRPD_ArrivalDate).ToList();

                data.returnvalue = false;
                data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                data.duplicatetwo = "0";
                if (data.VTADAAA_Id == 0 && data.VTADAA_Id == 0 && data.IER_ID > 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        data.duplicatetwo = "";
                        cmd.CommandText = "TADA_Advance_Apply_Record_Checking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });

                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
                        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        data.duplicatetwo = cmd.Parameters["@Result"].Value.ToString();
                    }
                }
                if (data.duplicatetwo == "0")
                {
                    if (data.VTADAA_Id == 0 && data.HRME_Id > 0)
                    {
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "VMS_TADA_Apply_Record_Checking";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id }); cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
                            cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
                            cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var data1 = cmd.ExecuteNonQuery();
                            data.duplicate = cmd.Parameters["@Result"].Value.ToString();
                        }

                        if (data.duplicate == "0")
                        {
                            TADA_ApplyDMO obj = new TADA_ApplyDMO();
                            obj.MI_Id = data.MI_Id;
                            if (data.VTADAAA_Id > 0)
                            {
                                obj.VTADAAA_Id = data.VTADAAA_Id;
                            }
                            obj.HRME_Id = data.HRME_Id;
                            obj.VTADAA_AppliedDate = DateTime.Now;
                            obj.VTADAA_FromDate = listtada.FirstOrDefault().IER_Plan_StartDate;
                            obj.VTADAA_ToDate = listtada.FirstOrDefault().IER_Plan_EndDate;                      
                            obj.VTADAA_Remarks = data.VTADAA_Remarks;
                            obj.VTADAA_TotalAppliedAmount = data.VTADAA_TotalAppliedAmount;
                            obj.IVRMMCT_Id = Citylist.FirstOrDefault().IVRMMCT_Id;
                            obj.VTADAA_StatusFlg = "Pending";
                            obj.VTADAA_ActiveFlg = true;
                            obj.VTADAA_ArrivalTime = ArrivalTimelist.FirstOrDefault().IRPD_ArrivalTime;
                            obj.VTADAA_DepartureTime = Departurelist.FirstOrDefault().IRPD_DepartureTime; 
                            obj.VTADAA_CreatedBy = data.UserId;
                            obj.VTADAA_UpdatedBy = data.UserId;
                            obj.VTADAA_CreatedDate = DateTime.Now;
                            obj.VTADAA_UpdatedDate = DateTime.Now;
                            obj.VTADAA_BalanceHRMS = ((Convert.ToDecimal(data.VTADAA_BalanceHRMS) + (Convert.ToDecimal(data.VTADAA_BalanceConcal)))).ToString();
                            obj.VTADAA_Finaldocument = true;
                            obj.VTADAA_ApprovedFlg = true;
                            obj.VTADAA_BalanceConcal = data.VTADAA_BalanceConcal;
                            obj.IER_ID = Convert.ToString(data.IER_ID);
                            _context.Add(obj);
                            if (data.filelistNew != null && data.filelistNew.Length > 0)
                            {
                                for (int b = 0; b < data.filelistNew.Length; b++)
                                {
                                    VMS_TADAApply_FilesDMO dobjj = new VMS_TADAApply_FilesDMO();
                                    dobjj.VTADAA_Id = obj.VTADAA_Id;
                                    dobjj.VTADAAF_FilePath = data.filelistNew[b].VTADAAF_FilePath;
                                    dobjj.VTADAAF_FileName = data.filelistNew[b].VTADAAF_FileName;
                                    dobjj.VTADAAF_Remarks = data.filelistNew[b].VTADAAF_Remarks;
                                    dobjj.VTADAAF_Filedesc = data.filelistNew[b].VTADAAF_Filedesc;
                                    dobjj.VTADAAF_Cost = data.filelistNew[b].VTADAAF_Cost;

                                    dobjj.VTADAAF_StatusFlg = "Pending";
                                    dobjj.VTADAAF_ActiveFlag = true;
                                    dobjj.VTADAAF_ApprovedFlg = false;
                                    _context.Add(dobjj);
                                }
                            }
                            foreach (var p in data.filelist)
                            {
                                var Category = _context.Database.ExecuteSqlCommand("ISM_Insert_Category_Parameter @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.IER_ID,p.HRME_Id, p.ILRT_FileName,p.ILRT_FilePath, p.ILRT_Remarks,1,DateTime.Now, DateTime.Now,data.UserId,data.UserId);
                                if (Category > 0)
                                {

                                }
                                else
                                {

                                }
                            }


                                int a = _context.SaveChanges();
                            if (a > 0)
                            {
                                data.returnvalue = true;
                                data.returnval = "Insert";
                                string mailcc = "";
                                var checkmaxorder = (from P in _context.HR_PROCESSDMO
                                                     from R in _context.HR_Process_Auth_OrderNoDMO
                                                     where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "TAAD"
                                                     select new TADA_Advance_ApplyDTO
                                                     {
                                                         HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                                         IVRMUL_Id = R.IVRMUL_Id
                                                     }
                                                     ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                                if (checkmaxorder.Count > 0)
                                {
                                    data.UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;
                                    long Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                                    data.CC_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                }
                                string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == obj.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                if (data.CC_Mail != null)
                                {
                                    mailcc = data.CC_Mail;
                                }

                                sendmail(data.MI_Id, emailid, "TADA_Apply", obj.VTADAA_Id, mailcc);

                            }
                            else
                            {
                                data.returnvalue = true;
                                data.returnval = "Failed";
                            }
                        }
                        else
                        {
                            data.returnvalue = true;
                            data.returnval = "Duplicate";
                        }
                    }
                    if (data.VTADAA_Id > 0 && data.HRME_Id > 0)
                    {
                        var resultone = _context.TADA_ApplyDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id && t.MI_Id == data.MI_Id).FirstOrDefault();
                        resultone.MI_Id = data.MI_Id;
                        resultone.HRME_Id = data.HRME_Id;
                        resultone.VTADAA_AppliedDate = DateTime.Now;
                        resultone.VTADAA_FromDate = listtada.FirstOrDefault().IER_Plan_StartDate;
                        resultone.VTADAA_ToDate = listtada.FirstOrDefault().IER_Plan_EndDate;
                        // resultone.VTADAA_ClientId = data.VTADAA_ClientId;
                        //  resultone.VTADAA_ToAddress = data.VTADAA_ToAddress;
                        resultone.VTADAA_Remarks = data.VTADAA_Remarks;
                        resultone.VTADAA_TotalAppliedAmount = data.VTADAA_TotalAppliedAmount;
                        resultone.IVRMMCT_Id = Citylist.FirstOrDefault().IVRMMCT_Id;
                        resultone.VTADAA_UpdatedBy = data.UserId;
                        resultone.VTADAA_UpdatedDate = DateTime.Now;
                        resultone.VTADAA_BalanceHRMS = ((Convert.ToDecimal(data.VTADAA_BalanceHRMS)+ (Convert.ToDecimal(data.VTADAA_BalanceConcal)))).ToString();
                        resultone.VTADAA_Finaldocument = true;
                        resultone.VTADAA_ApprovedFlg = true;
                        resultone.VTADAA_BalanceConcal = data.VTADAA_BalanceConcal;
                        _context.Update(resultone);

                        if (data.filelistNew != null && data.filelistNew.Length > 0)
                        {
                            var DetailsFiles = _context.VMS_TADAApply_FilesDMO.Where(a => a.VTADAA_Id == data.VTADAA_Id && a.VTADAAF_StatusFlg == "Pending").ToList();

                            if (DetailsFiles.Count > 0)
                            {
                                foreach (var d in DetailsFiles)
                                {
                                    _context.Remove(d);
                                }
                            }
                            if (data.filelistNew != null && data.filelistNew.Length > 0)
                            {
                                for (int b = 0; b < data.filelistNew.Length; b++)
                                {
                                    VMS_TADAApply_FilesDMO dobjj = new VMS_TADAApply_FilesDMO();
                                    dobjj.VTADAA_Id = data.VTADAA_Id;
                                    dobjj.VTADAAF_FilePath = data.filelistNew[b].VTADAAF_FilePath;
                                    dobjj.VTADAAF_FileName = data.filelistNew[b].VTADAAF_FileName;
                                    dobjj.VTADAAF_Remarks = data.filelistNew[b].VTADAAF_Remarks;
                                    dobjj.VTADAAF_Filedesc = data.filelistNew[b].VTADAAF_Filedesc;
                                    dobjj.VTADAAF_StatusFlg = "Pending";
                                    dobjj.VTADAAF_ActiveFlag = true;
                                    dobjj.VTADAAF_ApprovedFlg = false;
                                    _context.Add(dobjj);
                                }
                            }

                        }
                        foreach (var p in data.filelist)
                        {
                            var Category = _context.Database.ExecuteSqlCommand("ISM_Insert_Category_Parameter @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9", data.IER_ID, p.HRME_Id, p.ILRT_FileName, p.ILRT_FilePath, p.ILRT_Remarks, 1, DateTime.Now, DateTime.Now, data.UserId, data.UserId);
                            if (Category > 0)
                            {

                            }
                            else
                            {

                            }
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnvalue = true;
                            data.returnval = "Update";

                            string mailcc = "";
                            var checkmaxorder = (from P in _context.HR_PROCESSDMO
                                                 from R in _context.HR_Process_Auth_OrderNoDMO
                                                 where P.HRPA_Id == R.HRPA_Id && P.HRPA_TypeFlag == "TAAD"
                                                 select new TADA_Advance_ApplyDTO
                                                 {
                                                     HRPAON_SanctionLevelNo = R.HRPAON_SanctionLevelNo,
                                                     IVRMUL_Id = R.IVRMUL_Id
                                                 }
                                                 ).OrderBy(P => P.HRPAON_SanctionLevelNo).ToList();
                            if (checkmaxorder.Count > 0)
                            {
                                data.UserId = checkmaxorder.FirstOrDefault().IVRMUL_Id;

                                long Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                                data.CC_Mail = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            }
                            string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            if (data.CC_Mail != null)
                            {
                                mailcc = data.CC_Mail;
                            }
                            // emailid= "kanavisanjeev@gmail.com"; 
                            //   mailcc = "sanjusisteg@gmail.com";

                            sendmail(data.MI_Id, emailid, "TADA_Apply", data.VTADAA_Id, mailcc);

                        }
                        else
                        {
                            data.returnvalue = true;
                            data.returnval = "UpdateFailed";
                        }
                    }
                }
                else
                {
                    data.returnvalue = true;
                    data.returnval = "DuplicateAdavance";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }

        //Save_response_lead_demo
        public TADA_Advance_TourDTO Save_response_lead_demo(TADA_Advance_TourDTO dto)
        {
            try
            {
                foreach (var dat1 in dto.leaddetails)
                {

                    try
                    {

                        try
                        {
                            var leaddatahistory = _context.Database.ExecuteSqlCommand("ism_sales_records_history @p0", dat1.ISMSLE_Id);
                            if (leaddatahistory > 0)
                            {

                            }
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }



                        var leaddataupdate = _context.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == dat1.ISMSLE_Id).FirstOrDefault();
                        try
                        {
                            var leaddatahistory = _context.Database.ExecuteSqlCommand("ism_sales_records_history @p0", dat1.ISMSLE_Id);
                            if (leaddatahistory > 0)
                            {

                            }
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }

                        if (leaddataupdate != null)
                        {
                            leaddataupdate.ISMSLE_ContactNo = dat1.ISMSLE_ContactNo;
                            leaddataupdate.ISMSLE_ContactPerson = dat1.ISMSLE_ContactPerson;
                            leaddataupdate.ISMSLE_ContactDesignation = dat1.ISMSLE_ContactDesignation;
                            leaddataupdate.ISMSLE_LeadAddress1 = dat1.ISMSLE_LeadAddress1;
                            leaddataupdate.ISMSLE_LeadAddress2 = dat1.ISMSLE_LeadAddress2;
                            leaddataupdate.ISMSLE_LeadAddress3 = dat1.ISMSLE_LeadAddress3;
                            leaddataupdate.ISMSLE_LeadName = dat1.ISMSLE_LeadName;
                            leaddataupdate.ISMSLE_EmailId = dat1.ISMSLE_EmailId;
                            leaddataupdate.ISMSLE_StudentStrength = dat1.ISMSLE_StudentStrength;
                            leaddataupdate.ISMSLE_UpdatedBy = dto.UserId;
                            // leaddataupdate.UpdatedDate = DateTime.Now;
                            _context.Update(leaddataupdate);
                            var updated = _context.SaveChanges();
                            if (updated > 0)
                            {

                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }

                }

                foreach (var p in dto.product_demo_master_temp2)
                {
                    //var studentStrengthUpdate = _context.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_Id == p.ISMSLE_Id).FirstOrDefault();
                    //if (studentStrengthUpdate != null)
                    //{
                    //    studentStrengthUpdate.ISMSLE_StudentStrength = dto.ISMSLE_STUDENTSTRENGTH;
                    //    _context.Update(studentStrengthUpdate);
                    //    var updated = _context.SaveChanges();
                    //    if (updated > 0)
                    //    {

                    //    }
                    //}

                    var result = _context.ISM_Sales_Lead_Demo_Products_DMO_con.Where(a => a.ISMSLEDM_Id == p.ISMSLEDM_Id && a.ISMSMPR_Id == p.ISMSMPR_Id && a.ISMSLEDMPR_ActiveFlag == true).FirstOrDefault();

                    var contactExistsP = _context.Database.ExecuteSqlCommand("TADA_demo_response_update_proc @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16",
                                              p.ISMSLEDM_Id, result.ISMSLEDMPR_Id, dto.MI_Id, p.ISMSMST_Id, p.ISMSLEDMPR_Remarks, p.ISMSLECOM_SendQuotationFlg,
                                              p.ISMSLECOM_MoMFlg, p.ISMSLECOM_NonFlg, p.NON_Amount, p.ISMSLECOM_CancelFlg,
                                              p.SMSLEDM_CategaroyId, p.IER_ID, p.ISMSLEDMPR_ActiveFlag, dto.ILRR_Lat, dto.ILRR_Long, p.ISMSMST_StatusName, p.ISMSBOM_Id);

                    if (contactExistsP > 0)
                    {
                        dto.return_status = "Updated";

                        if (p.ISMSMST_StatusName == "HOT")
                        {
                            //sanjeev@globalqtytrainig.com
                            // var emailid = "sanjeev@globalqtytrainig.com";
                            var emailid = "mktgcoord@vapstech.com";

                            if (emailid != null && emailid != "")
                            {
                                try
                                {
                                    var Fms = _context.Database.ExecuteSqlCommand("FMS_Correspondence_Lead_Insert @p0,@p1,@p2,@p3",
                                       p.ISMSLE_Id, p.ISMSMPR_ProductName, dto.UserId, p.ISMSBOM_Id);
                                    if (Fms > 0)
                                    {

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                                //{
                                //    cmd.CommandText = "FMS_Correspondence_Lead_Insert";
                                //    cmd.CommandType = CommandType.StoredProcedure;

                                //    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                                //    SqlDbType.BigInt)
                                //    {
                                //        Value = p.ISMSLE_Id
                                //    });
                                //    cmd.Parameters.Add(new SqlParameter("@Subject",
                                //   SqlDbType.BigInt)
                                //    {
                                //        Value = p.ISMSMPR_ProductName
                                //    });
                                //    cmd.Parameters.Add(new SqlParameter("@UserId",
                                //   SqlDbType.BigInt)
                                //    {
                                //        Value = dto.UserId
                                //    });
                                //    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id",
                                //  SqlDbType.BigInt)
                                //    {
                                //        Value = p.ISMSBOM_Id
                                //    });

                                //    if (cmd.Connection.State != ConnectionState.Open)
                                //        cmd.Connection.Open();

                                //    var retObject = new List<dynamic>();
                                //    try
                                //    {
                                //        using (var dataReader = cmd.ExecuteReader())
                                //        {
                                //            while (dataReader.Read())
                                //            {
                                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                //                {
                                //                    dataRow.Add(
                                //                        dataReader.GetName(iFiled),
                                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                //                    );
                                //                }
                                //                retObject.Add((ExpandoObject)dataRow);
                                //            }
                                //        }

                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        Console.WriteLine(ex.Message);
                                //    }
                                //}


                                // sendmails(dto.MI_Id, emailid, "ConfirmedClient", p.ISMSLEDM_Id);
                            }


                        }
                    }
                    else
                    {
                        dto.return_status = "notUpdated";
                    }


                }
                _context.SaveChanges();
                dto.return_status = "Update";
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        //view_lead_demo
        public TADA_Advance_TourDTO view_lead_demo(TADA_Advance_TourDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_SALES_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLEDM_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ISMSLEDM_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                   SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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

                        data.view_lead_demo = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_SALES_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMSLEDM_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ISMSLEDM_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                   SqlDbType.VarChar)
                    {
                        Value = "2"
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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

                        data.product_dd_s = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //dto.view_lead_demo = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                //                      from b in _vmsconte.ISM_Sales_Master_Status_DMO_con
                //                      from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                      from d in _vmsconte.Hr_Master_Employee_con

                //                      where a.ISMSMST_Id == b.ISMSMST_Id && a.ISMSLE_Id == c.ISMSLE_Id && c.ISMSLEDM_Id == dto.ISMSLEDM_Id
                //                      && a.ISMSLE_Id == dto.ISMSLE_Id && c.HRME_Id == d.HRME_Id


                //                      select new ISM_Sales_Lead_Demo_DTO
                //                      {
                //                          employeename = ((d.HRME_EmployeeFirstName == null || d.HRME_EmployeeFirstName == "" ? "" : d.HRME_EmployeeFirstName)
                //                          + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == "" ? "" : " " + d.HRME_EmployeeMiddleName)
                //                          + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == "" ? "" : " " + d.HRME_EmployeeLastName)).Trim(),
                //                          HRME_Id = d.HRME_Id,
                //                          ISMSLE_LeadName = a.ISMSLE_LeadName,
                //                          ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                //                          ISMSLEDM_ContactPerson = c.ISMSLEDM_ContactPerson,
                //                          ISMSLE_ContactNo = a.ISMSLE_ContactNo,
                //                          ISMSLEDM_DemoAddress = c.ISMSLEDM_DemoAddress,
                //                          ISMSLEDM_Remarks = c.ISMSLEDM_Remarks

                //                      }).ToArray();

                //dto.product_dd_s = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                //                    from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                //                    from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                //                    where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id && c.ISMSLE_Id == dto.ISMSLE_Id
                //                    && b.ISMSMPR_Id == d.ISMSMPR_Id
                //                    //&& b.MI_Id == dto.MI_Id
                //                    select new ISM_Sales_Lead_Demo_DTO
                //                    {
                //                        ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                //                        ISMSMPR_Id = b.ISMSMPR_Id,
                //                        ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints,
                //                        ISMSLEDMPR_ActiveFlag = b.ISMSLEDMPR_ActiveFlag
                //                    }).ToArray();

                if (data.viewall == 1)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TADA_SALES_List";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMSLEDM_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.ISMSLEDM_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                       SqlDbType.VarChar)
                        {
                            Value = "3"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
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

                            data.demo_response_details = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        //Edit_response_lead_demo
        public TADA_Advance_TourDTO Edit_response_lead_demo(TADA_Advance_TourDTO dto)
        {
            try
            {
                if (dto.message == "SaveAuto")
                {
                    var resultone = _context.TADA_AdavanceApplyDMO.Where(t => t.VTADAAA_Id == dto.VTADAAA_Id).FirstOrDefault();
                    //   resultone.MI_Id = dto.MI_Id;                    
                    resultone.VTADAAA_FromDate = dto.PlanStartDate;
                    resultone.VTADAAA_ToDate = dto.PlanEndDate;

                    resultone.VTADAAA_TotalAppliedAmount = resultone.VTADAAA_TotalAppliedAmount + dto.VTADAAA_TotalAppliedAmount;
                    resultone.VTADAAA_UpdatedBy = dto.UserId;
                    resultone.VTADAAA_UpdatedDate = DateTime.Now;
                    _context.Update(resultone);
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        dto.returnvalue = true;
                        dto.returnval = "Update";
                        if (dto.VTADAAA_TotalAppliedAmount > 0)
                        {
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "VMSTADA_RoutePayment_Extenssion";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                                  SqlDbType.VarChar)
                                {
                                    Value = dto.VTADAAA_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@userid",
                                  SqlDbType.VarChar)
                                {
                                    Value = dto.UserId
                                });
                                cmd.Parameters.Add(new SqlParameter("@VTADAAA_TotalAppliedAmount",
                                SqlDbType.Decimal)
                                {
                                    Value = dto.VTADAAA_TotalAppliedAmount
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




                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        if (dto.listEmployeeSales != null && dto.listEmployeeSales.Length > 0)
                        {
                            foreach (var d in dto.listEmployeeSales)
                            {
                                var contactExistsP = _context.Database.ExecuteSqlCommand("usp_U_Emp_RoutePlan_extenssion @p0, @p1,@p2,@p3,@p4,@p5,@p6,@p7", resultone.IER_ID, resultone.HRME_Id, dto.PlanStartDate, dto.PlanEndDate, d.ClientId, d.CategoryId, dto.UserId, d.ISMSMPR_Id);

                            }
                        }

                    }
                    else
                    {
                        dto.returnvalue = true;
                        dto.returnval = "UpdateFailed";
                    }
                }
                else
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TADA_SALES_List";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMSLEDM_Id",
                          SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLEDM_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id",
                        SqlDbType.BigInt)
                        {
                            Value = dto.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                       SqlDbType.VarChar)
                        {
                            Value = "4"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
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

                            dto.demo_response_details = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    dto.status_demo_master = _context.ISM_Sales_Master_Status_DMO_con.Where(a => a.ISMSMST_ActiveFlag == true && a.MI_Id == dto.MI_Id).ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public string sendmails(long MI_Id, string Email, string Template, long UserID)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id
                && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "SMSMAILPARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
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
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }
                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;
                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                try
                {
                    List<ISM_Sales_Lead_Demo_DTO> dtonewtemp = new List<ISM_Sales_Lead_Demo_DTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_Leadccmailss";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.BigInt)
                        {
                            Value = 1
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
                                    dtonewtemp.Add(new ISM_Sales_Lead_Demo_DTO
                                    {
                                        return_status = Convert.ToString(dataReader["cc"]),
                                        ISMSLEDM_Remarks = Convert.ToString(dataReader["bcc"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (dtonewtemp.Count > 0)
                    {
                        for (int j = 0; j < dtonewtemp.Count; j++)
                        {
                            mailcc = dtonewtemp[j].return_status;
                            mailbcc = dtonewtemp[j].ISMSLEDM_Remarks;
                        }
                    }

                }
                catch (Exception error)
                {
                    //
                }
                if (alldetails.Count > 0)
                {
                    string SendingEmail = "noreply@vapstech.com";
                    //  string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }
                        if (mailcc != null && mailcc != "")
                        {
                            string[] ccmaildetails = mailcc.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddCc(c);
                                }
                            }
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            string[] bccmaildetails = mailbcc.Split(',');

                            foreach (var c in bccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddBcc(c);
                                }
                            }
                        }
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };
                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;
                            using (var emailMessage = new MailMessage())
                            {
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {
                                            foreach (var attache in img.ToList())
                                            {
                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }
                                        }
                                    }
                                }

                                if (mailcc != null && mailcc != "")
                                {
                                    string[] ccmaildetails = mailcc.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    string[] bccmaildetails = mailbcc.Split(',');

                                    foreach (var c in bccmaildetails)
                                    {
                                        emailMessage.Bcc.Add(c);
                                    }
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        //LeadComments
        public TADA_Advance_TourDTO LeadComments(TADA_Advance_TourDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_RouteFinalComments";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.IER_ID
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

                        data.view_lead_demo = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //leadsComments 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_RouteFinalComments";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.IER_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMRC_CategoryId",
                   SqlDbType.BigInt)
                    {
                        Value = data.IMRC_CategoryId
                    });
                    cmd.Parameters.Add(new SqlParameter("@StatusFlag",
                    SqlDbType.VarChar)
                    {
                        Value = data.message
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

                        data.getloadarray = retObject.ToArray();
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
        public TADA_Advance_TourDTO TravelDetails(TADA_Advance_TourDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_Travel_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VTADAMT_Id",
                         SqlDbType.VarChar)
                    {
                        Value = data.VTADAMT_Id
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
                        data.Travel_Details = retObject.ToArray();

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
        public TADA_Advance_TourDTO Getamount(TADA_Advance_TourDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TADA_City_DesgAmount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@FromState",
                        SqlDbType.VarChar)
                    {
                        Value = data.Fromstate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToState",
                        SqlDbType.VarChar)
                    {
                        Value = data.Tostate
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAMT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.VTADAMT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@VTADAMD_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.VTADAMD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ID",
                        SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    //@VRMMDT_Id
                    cmd.Parameters.Add(new SqlParameter("@VRMMDT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.VRMMDT_Id
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
                        data.TravelAmount = retObject.ToArray();

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
        public TADA_Advance_TourDTO GetCity(TADA_Advance_TourDTO data)
        {
            try
            {
                string IVRMMCT_Id = "0";
                if (data.VMSTADAAmountDTO !=null  && data.VMSTADAAmountDTO.Length >0)
                {
                    foreach(var d  in data.VMSTADAAmountDTO)
                    {
                        IVRMMCT_Id = IVRMMCT_Id + "," + d.IVRMMCT_Id;

                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Tada_City_Names";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IER_ID",
                     SqlDbType.VarChar)
                    {
                        Value = data.IER_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Tostate",
                     SqlDbType.VarChar)
                    {
                        Value = data.Tostate
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMMCT_Id",
                   SqlDbType.VarChar)
                    {
                        Value = IVRMMCT_Id
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
                        data.Travel_data = retObject.ToArray();

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
