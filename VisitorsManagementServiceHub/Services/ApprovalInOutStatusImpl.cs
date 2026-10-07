using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class ApprovalInOutStatusImpl:Interfaces.ApprovalInOutStatusInterface
    {

        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _dbcontext;
        public ApprovalInOutStatusImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext ctxt)
        {
            visctxt = context;
            _dbcontext = ctxt;
        }


        public ApprovalInOutStatus_DTO getDetails(ApprovalInOutStatus_DTO data)
        {
            try
            {                
                long roletypeid = 0;
                string roletype = "";

                if (data.UserId > 0)
                {
                    roletypeid = visctxt.appUserRole.Where(t => t.UserId == data.UserId).Distinct().FirstOrDefault().RoleTypeId;
                    if (roletypeid > 0)
                    {
                        roletype = visctxt.applicationRole.Where(t => t.Id == roletypeid).Distinct().FirstOrDefault().roleType;
                    }
                    if (roletype != "" && roletype != null)
                    {
                        if (roletype.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                        {
                            var institutionlist1 = (from a in visctxt.Institute
                                                    from b in visctxt.UserRoleWithInstituteDMO
                                                    where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                                    select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                            data.institutionlist = institutionlist1.ToArray();

                            if (data.MI_Id == 0)
                            {
                                if (institutionlist1.Count > 0)
                                {
                                    data.MI_Id = institutionlist1.FirstOrDefault().MI_Id;
                                }
                            }
                        }
                        else if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            var institutionlist2 = (from a in visctxt.Institute
                                                    from b in visctxt.UserRoleWithInstituteDMO
                                                    where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                                    select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                            data.institutionlist = institutionlist2.ToArray();

                            if (data.MI_Id == 0)
                            {
                                if (institutionlist2.Count > 0)
                                {
                                    data.MI_Id = institutionlist2.FirstOrDefault().MI_Id;
                                }
                            }
                        }
                        else if (roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                        {
                            var institutionlist3 = (from a in visctxt.Institute
                                                    from b in visctxt.UserRoleWithInstituteDMO
                                                    where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                                    select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                            data.institutionlist = institutionlist3.ToArray();

                            if (data.MI_Id == 0)
                            {
                                if (institutionlist3.Count > 0)
                                {
                                    data.MI_Id = institutionlist3.FirstOrDefault().MI_Id;
                                }
                            }
                        }
                        else
                        {
                            var institutionlist = (from a in visctxt.Institute
                                                   from b in visctxt.UserRoleWithInstituteDMO
                                                   where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                                   select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                            data.institutionlist = institutionlist.ToArray();
                            if (data.MI_Id == 0)
                            {
                                if (institutionlist.Count > 0)
                                {
                                    data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                                }
                            }
                        }
                    }
                    else
                    {
                        var institutionlist = (from a in visctxt.Institute
                                               from b in visctxt.UserRoleWithInstituteDMO
                                               where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                               select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                        data.institutionlist = institutionlist.ToArray();
                        if (data.MI_Id == 0)
                        {
                            if (institutionlist.Count > 0)
                            {
                                data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                            }
                        }
                    }
                }
                data.visitorlist = (from a in visctxt.Visitor_Management_Appointment_DMO
                                    //from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                                    where ( a.MI_Id == data.MI_Id && a.VMAP_Status == "Approved"
                                    && a.VMAP_ChekInOutStatus != "Checked Out" )
                                    select new ApprovalInOutStatus_DTO
                                    {
                                        VMAP_Id = a.VMAP_Id,
                                        VMAP_VisitorName = a.VMAP_VisitorName,
                                        MI_Id = a.MI_Id,
                                    }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public ApprovalInOutStatus_DTO saveData(ApprovalInOutStatus_DTO data)
        {
            try
            {
                if (data.VMAP_Id > 0)
                {
                    var update = visctxt.Visitor_Management_Appointment_DMO.Single(d => d.VMAP_Id == data.VMAP_Id && d.MI_Id == data.MI_Id);
                    update.UpdatedDate = DateTime.Now;
                    update.VMAP_ExitDateTime = data.VMAP_ExitDateTime;
                    update.VMAP_ChekInOutStatus = data.VMAP_ChekInOutStatus;
                    update.VMAP_Remarks = data.VMAP_Remarks;

                    update.VMAP_UpdatedBy = data.UserId;
                    visctxt.Update(update);
                    int s = visctxt.SaveChanges();
                    if (s > 0)
                    {
                        data.returnVal = "updated";
                        var Template = "VisitorCheckInOutStatus";
                        var contactno = visctxt.Visitor_Management_Appointment_DMO.Single(t => t.VMAP_Id == data.VMAP_Id).VMAP_VisitorContactNo;
                        if (contactno > 0)
                        {
                            SMS sms = new SMS(_dbcontext);
                            sms.sendSmsCheckINOUT(data.MI_Id, contactno, Template, data.VMAP_Id);

                            //sendSmsCheckINOUT(data.MI_Id, contactno, Template, data.VMAP_Id);

                            visitornotification("fDOUfTEYDfU:APA91bHLkVeYnCRvxh4oXYLoEf-VmJOx0u3TEg6mr8V7oDDlCbCcXGoNA02xr3TMji3VaYRlTPObS7uOh4487KkWi7ldtYu1_Uxr1xvwkVKDyZzQw1wv4g4KOdADD392ook426aUttTj", data);
                        }
                        var mailId = visctxt.Visitor_Management_Appointment_DMO.Single(t => t.VMAP_Id == data.VMAP_Id).VMAP_VisitorEmailid;

                        if (mailId!="")
                        {
                            Email email = new Email(_dbcontext);
                            email.sendmailCheckINOUT(data.MI_Id, mailId, Template, data.VMAP_Id);

                            //sendmailCheckINOUT(data.MI_Id, mailId, Template, data.VMAP_Id);
                        }

                        //if (data.type == "int_dept")
                        //{
                        //    SMS sms = new SMS(_dbcontext);
                        //    sms.sendSms(data.MI_Id, contactno, Template, data.VMAP_Id);

                        //    Email Email = new Email(_dbcontext);
                        //    Email.sendmail(data.MI_Id, mailId, Template, data.VMAP_Id);
                        //}
                        //else if (data.type == "out_dept")
                        //{
                        //    SMS sms = new SMS(_dbcontext);
                        //    sms.sendSms(data.MI_Id, contactno, Template, data.VMAP_Id);

                        //    Email Email = new Email(_dbcontext);
                        //    Email.sendmail(data.MI_Id, mailId, Template, data.VMAP_Id);
                        //}

                    }
                    else
                    {
                        data.returnVal = "updateFailed";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public ApprovalInOutStatus_DTO EditDetails(ApprovalInOutStatus_DTO id)
        {
            ApprovalInOutStatus_DTO resp = new ApprovalInOutStatus_DTO();
            try
            {

                var editData = (from a in visctxt.Visitor_Management_Appointment_DMO
                                from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                                from e in visctxt.MasterEmployee
                                where (a.MI_Id == b.MI_Id && a.VMAP_Id == b.VMAP_Id && e.HRME_Id == a.VMAP_HRME_Id && a.MI_Id == id.MI_Id && a.VMAP_Id == id.VMAP_Id)
                                select new ApprovalInOutStatus_DTO
                                {
                                    VMAP_Id = a.VMAP_Id,
                                    VMAP_VisitorName = a.VMAP_VisitorName,
                                    empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName),
                                    VMAP_MeetingDateTime = a.VMAP_MeetingDateTime,
                                    VMAP_Remarks = a.VMAP_Remarks,
                                    VMAP_EntryDateTime = a.VMAP_EntryDateTime,
                                    MI_Id = a.MI_Id,

                                }).Distinct().ToList();
                resp.editDetails = editData.ToArray();


                //if (id.type == "int_dept")
                //{
                //    var editData = (from a in visctxt.Visitor_Management_Appointment_DMO
                //                    from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                //                    from e in visctxt.MasterEmployee
                //                    where (a.MI_Id == b.MI_Id && a.VMAP_Id == b.VMAP_Id && e.HRME_Id == a.VMMV_ToMeet && a.MI_Id == id.MI_Id && a.VMAP_Id == id.VMAP_Id)
                //                    select new ApprovalInOutStatus_DTO
                //                    {
                //                        VMAP_Id = a.VMAP_Id,
                //                        VMAP_VisitorName = a.VMAP_VisitorName,
                //                        empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName),
                //                        VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                //                        VMMV_Remarks = a.VMMV_Remarks,
                //                        VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                //                        MI_Id = a.MI_Id,

                //                    }).Distinct().ToList();
                //    resp.editDetails = editData.ToArray();
                //}
                //else if (id.type == "out_dept")
                //{
                //    var editData = (from a in visctxt.Visitor_Management_Appointment_DMO
                //                    from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                //                        //from e in visctxt.MasterEmployee
                //                    where (a.MI_Id == b.MI_Id && a.VMAP_Id == b.VMAP_Id && a.MI_Id == id.MI_Id && a.VMAP_Id == id.VMAP_Id)
                //                    select new ApprovalInOutStatus_DTO
                //                    {
                //                        VMAP_Id = a.VMAP_Id,
                //                        VMAP_VisitorName = a.VMAP_VisitorName,
                //                        empname = a.VMMV_PersonToMeet,
                //                        VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                //                        VMMV_Remarks = a.VMMV_Remarks,
                //                        VMMV_EntryDateTime = a.VMMV_EntryDateTime,
                //                        MI_Id = a.MI_Id,
                //                    }).Distinct().ToList();
                //    resp.editDetails = editData.ToArray();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public ApprovalInOutStatus_DTO change_radiobutton(ApprovalInOutStatus_DTO data)
        {
            try
            {
                //if (data.type == "int_dept")
                //{
                //    data.visitorlist = (from a in visctxt.Visitor_Management_Appointment_DMO
                //                        from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                //                        where (a.MI_Id == b.MI_Id && a.VMAP_Id == b.VMAP_Id && a.MI_Id == data.MI_Id && a.VMAP_DeptFlag == false
                //                        && a.VMAP_ChekInOutStatus != "Checked Out")
                //                        select new ApprovalInOutStatus_DTO
                //                        {
                //                            VMAP_Id = a.VMAP_Id,
                //                            VMAP_VisitorName = a.VMAP_VisitorName,
                //                            MI_Id = a.MI_Id,
                //                        }).Distinct().ToArray();
                //}
                //else if (data.type == "out_dept")
                //{
                //    data.visitorlist = (from a in visctxt.Visitor_Management_Appointment_DMO
                //                        from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                //                        where (a.MI_Id == b.MI_Id && a.VMAP_Id == b.VMAP_Id && a.MI_Id == data.MI_Id && a.VMAP_DeptFlag == true
                //                        && a.VMAP_ChekInOutStatus != "Checked Out")
                //                        select new ApprovalInOutStatus_DTO
                //                        {
                //                            VMAP_Id = a.VMAP_Id,
                //                            VMAP_VisitorName = a.VMAP_VisitorName,
                //                            MI_Id = a.MI_Id,
                //                        }).Distinct().ToArray();
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        #region sms
        //public async Task<string> sendSmsCheckINOUT(long MI_Id, long mobileNo, string Template, long UserID)
        //{

        //    try
        //    {

        //        Dictionary<string, string> val = new Dictionary<string, string>();

        //        var template = _dbcontext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

        //        if (template.Count == 0)
        //        {
        //            return "SMS Template not Mapped to the selected Institution";
        //        }


        //        var institutionName = _dbcontext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

        //        var Paramaeters = _dbcontext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

        //        var ParamaetersName = _dbcontext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

        //        string sms = template.FirstOrDefault().ISES_SMSMessage;

        //        string result = sms;

        //        List<Match> variables = new List<Match>();

        //        foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
        //        {
        //            variables.Add(match);
        //        }

        //        if (Template == "EMAILOTP")
        //        {
        //            result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
        //            sms = result;
        //        }
        //        else
        //        {
        //            using (var cmd = _dbcontext.Database.GetDbConnection().CreateCommand())
        //            {


        //                cmd.CommandText = "SMSMAILPARAMETER";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@UserID",
        //                    SqlDbType.BigInt)
        //                {
        //                    Value = UserID
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@template",
        //                   SqlDbType.VarChar)
        //                {
        //                    Value = Template
        //                });


        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                                );
        //                                var datatype = dataReader.GetFieldType(iFiled);
        //                                if (datatype.Name == "DateTime")
        //                                {
        //                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
        //                                    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
        //                                }
        //                                else
        //                                {
        //                                    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    return ex.Message;
        //                }

        //            }

        //            for (int j = 0; j < ParamaetersName.Count; j++)
        //            {
        //                for (int p = 0; p < val.Count; p++)
        //                {
        //                    if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
        //                    {
        //                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
        //                        sms = result;
        //                    }
        //                }
        //            }

        //            sms = result;
        //        }


        //        List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
        //        alldetails = _dbcontext.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

        //        if (alldetails.Count > 0)
        //        {
        //            string url = alldetails[0].IVRMSD_URL.ToString();

        //            string PHNO = mobileNo.ToString();

        //            url = url.Replace("PHNO", PHNO);

        //            url = url.Replace("MESSAGE", sms);

        //            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
        //            System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
        //            Stream stream = response.GetResponseStream();

        //            StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
        //            string responseparameters = readStream.ReadToEnd();
        //            var myContent = JsonConvert.SerializeObject(responseparameters);

        //            dynamic responsedata = JsonConvert.DeserializeObject(myContent);
        //            string messageid = responsedata;

        //            using (var cmd = _dbcontext.Database.GetDbConnection().CreateCommand())
        //            {
        //                var template1010 = _dbcontext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

        //                var moduleid = _dbcontext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

        //                var modulename = _dbcontext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

        //                cmd.CommandText = "IVRM_SMS_Outgoing";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@MobileNo",
        //                    SqlDbType.NVarChar)
        //                {
        //                    Value = PHNO
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Message",
        //                   SqlDbType.NVarChar)
        //                {
        //                    Value = sms
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@module",
        //                SqlDbType.VarChar)
        //                {
        //                    Value = modulename[0]
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //               SqlDbType.BigInt)
        //                {
        //                    Value = MI_Id
        //                });

        //                cmd.Parameters.Add(new SqlParameter("@status",
        //           SqlDbType.VarChar)
        //                {
        //                    Value = "Delivered"
        //                });

        //                cmd.Parameters.Add(new SqlParameter("@Message_id",
        //        SqlDbType.VarChar)
        //                {
        //                    Value = messageid
        //                });


        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    return ex.Message;
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return ex.Message;
        //    }
        //    return "success";
        //}
        #endregion

        #region mail
        //public string sendmailCheckINOUT(long MI_Id, string Email, string Template, long UserID)
        //{
        //    try
        //    {
        //        Dictionary<string, string> val = new Dictionary<string, string>();

        //        var template = _dbcontext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

        //        if (template.Count == 0)
        //        {
        //            return "";
        //        }


        //        var institutionName = _dbcontext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

        //        var Paramaeters = _dbcontext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

        //        var ParamaetersName = _dbcontext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

        //        string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
        //        string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

        //        string Resultsms = Mailcontent;
        //        string result = Mailmsg;

        //        List<Match> variables = new List<Match>();

        //        foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
        //        {
        //            variables.Add(match);
        //        }

        //        if (Template == "EMAILOTP")
        //        {
        //            result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
        //            Mailmsg = result;
        //            Mailcontent = result;
        //        }
        //        else
        //        {
        //            using (var cmd = _dbcontext.Database.GetDbConnection().CreateCommand())
        //            {

        //                cmd.CommandText = "SMSMAILPARAMETER";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@UserID",
        //                    SqlDbType.BigInt)
        //                {
        //                    Value = UserID
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@template",
        //                   SqlDbType.VarChar)
        //                {
        //                    Value = Template
        //                });


        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                                );
        //                                var datatype = dataReader.GetFieldType(iFiled);
        //                                if (datatype.Name == "DateTime")
        //                                {
        //                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
        //                                    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
        //                                }
        //                                else
        //                                {
        //                                    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    return ex.Message;
        //                }

        //            }


        //            for (int j = 0; j < ParamaetersName.Count; j++)
        //            {
        //                for (int p = 0; p < val.Count; p++)
        //                {
        //                    if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
        //                    {
        //                        result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
        //                        Mailmsg = result;
        //                    }
        //                }
        //            }
        //            Mailmsg = result;

        //            for (int j = 0; j < ParamaetersName.Count; j++)
        //            {
        //                for (int p = 0; p < val.Count; p++)
        //                {
        //                    if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
        //                    {
        //                        Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
        //                        Mailcontent = Resultsms;
        //                    }
        //                }
        //            }
        //            Mailcontent = Resultsms;
        //        }

        //        List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
        //        alldetails = _dbcontext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

        //        string Attechement = "";
        //        if (alldetails.Count > 0)
        //        {
        //            string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
        //            string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
        //            string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
        //            Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
        //            string Subject = template[0].ISES_MailSubject.ToString();
        //            string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
        //            List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
        //            smstpdetails = _dbcontext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

        //            if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
        //            {
        //                string mailcc = "";
        //                string mailbcc = "";
        //                if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
        //                {
        //                    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

        //                    mailcc = ccmail[0].ToString();

        //                    if (ccmail.Length > 1)
        //                    {
        //                        if (ccmail[1] != null || ccmail[1] != "")
        //                        {
        //                            mailbcc = ccmail[1].ToString();
        //                        }
        //                    }

        //                }
        //                if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
        //                {
        //                    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
        //                }


        //                //Sending mail using SendGrid API.
        //                //Date:07-02-2017.

        //                var message = new SendGridMessage();
        //                message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
        //                message.Subject = Subject;
        //                message.AddTo(Email);

        //                if (Attechement.Equals("1"))
        //                {
        //                    var img = _dbcontext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

        //                    if (img.Count > 0)
        //                    {
        //                        for (int i = 0; i < img.Count; i++)
        //                        {
        //                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
        //                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
        //                            Stream stream = response.GetResponseStream();
        //                            message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
        //                        }
        //                    }
        //                }

        //                if (mailcc != null && mailcc != "")
        //                {
        //                    message.AddCc(mailcc);
        //                }
        //                if (mailbcc != null && mailbcc != "")
        //                {
        //                    message.AddBcc(mailbcc);
        //                }

        //                message.HtmlContent = Mailmsg;
        //                // var client = new Web("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");

        //                //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
        //                //{
        //                //    message.AddAttachment(template.FirstOrDefault().ISES_MailHTMLTemplate);

        //                //}
        //                var client = new SendGridClient(sengridkey);

        //                client.SendEmailAsync(message).Wait();
        //            }
        //            else
        //            {


        //                string mailcc = "";
        //                string mailbcc = "";
        //                if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
        //                {
        //                    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

        //                    mailcc = ccmail[0].ToString();

        //                    if (ccmail.Length > 1)
        //                    {
        //                        if (ccmail[1] != null || ccmail[1] != "")
        //                        {
        //                            mailbcc = ccmail[1].ToString();
        //                        }
        //                    }

        //                }
        //                if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
        //                {
        //                    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
        //                }


        //                using (var clientsmtp = new SmtpClient())
        //                {
        //                    var credential = new NetworkCredential
        //                    {
        //                        UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
        //                        Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
        //                    };

        //                    clientsmtp.Credentials = credential;
        //                    clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
        //                    clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
        //                    clientsmtp.EnableSsl = true;

        //                    using (var emailMessage = new MailMessage())
        //                    {


        //                        emailMessage.To.Add(new MailAddress(Email));
        //                        emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
        //                        emailMessage.Subject = Subject;
        //                        emailMessage.Body = Mailmsg;
        //                        emailMessage.IsBodyHtml = true;


        //                        if (Attechement.Equals("1"))
        //                        {
        //                            var img = _dbcontext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

        //                            if (img.Count > 0)
        //                            {
        //                                for (int i = 0; i < img.Count; i++)
        //                                {

        //                                    foreach (var attache in img.ToList())
        //                                    {

        //                                        //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

        //                                        System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
        //                                        System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
        //                                        Stream stream = response.GetResponseStream();
        //                                        emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
        //                                    }


        //                                    //var attachment = new MimePart("image", "gif")
        //                                    //{
        //                                    //    ContentObject = new ContentObject(File.OpenRead(img[i].IVRM_Att_Path), ContentEncoding.Default),
        //                                    //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
        //                                    //    ContentTransferEncoding = ContentEncoding.Base64,
        //                                    //    FileName = Path.GetFileName(img[i].IVRM_Att_Path)
        //                                    //};
        //                                }
        //                            }
        //                        }


        //                        if (mailcc != null && mailcc != "")
        //                        {
        //                            emailMessage.CC.Add(mailcc);
        //                        }
        //                        if (mailbcc != null && mailbcc != "")
        //                        {
        //                            emailMessage.Bcc.Add(mailbcc);
        //                        }
        //                        clientsmtp.Send(emailMessage);
        //                    }
        //                }

        //            }


        //            using (var cmd = _dbcontext.Database.GetDbConnection().CreateCommand())
        //            {
        //                var template1010 = _dbcontext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

        //                var moduleid = _dbcontext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

        //                var modulename = _dbcontext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

        //                cmd.CommandText = "IVRM_Email_Outgoing";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@EmailId",
        //                    SqlDbType.NVarChar)
        //                {
        //                    Value = Email
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Message",
        //                   SqlDbType.NVarChar)
        //                {
        //                    Value = Mailcontent
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@module",
        //                SqlDbType.VarChar)
        //                {
        //                    Value = modulename[0]
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //               SqlDbType.BigInt)
        //                {
        //                    Value = MI_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    return ex.Message;
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return ex.Message;
        //    }
        //    return "success";
        //}
        #endregion

        public string visitornotification(string devicenew, ApprovalInOutStatus_DTO dto)
        {
            try
            {
                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                string sound = "default";
                long notId = 4;
                string empname = "TEST";
                DateTime ISMDRPT_Date = System.DateTime.Today;

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Daily Report " + ISMDRPT_Date + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}
