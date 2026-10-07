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
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.PettyCash;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IssueManager.com.PettyCash.Services
{
    public class PC_Indent_ApprovalImpl : Interface.PC_Indent_ApprovalInterface
    {
        public PettyCashContext _context;
        public DomainModelMsSqlServerContext _Contextdb;
        public PC_Indent_ApprovalImpl(PettyCashContext _con, DomainModelMsSqlServerContext _Contextdbsql)
        {
            _context = _con;
            _Contextdb = _Contextdbsql;
        }

        public PC_Indent_ApprovalDTO onloaddata(PC_Indent_ApprovalDTO data)
        {
            try
            {
                var getuserinstitution = _context.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();

                data.getloaddata = (from a in _context.PC_Indent_ApprovedDMO
                                    from b in _context.MasterEmployee
                                    from c in _context.HR_Master_Department
                                    where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                    select new PC_Indent_ApprovalDTO
                                    {
                                        departmentname = c.HRMD_DepartmentName,
                                        employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName)
                                        + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " "
                                        + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                        || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                        || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                        PCINDENTAPDT_ActiveFlg = a.PCINDENTAPT_ActiveFlg,
                                        PCINDENTAPT_Date = a.PCINDENTAPT_Date,
                                        PCINDENTAPT_Desc = a.PCINDENTAPT_Desc,
                                        PCINDENTAP_Id = a.PCINDENTAP_Id,
                                        PCINDENTAPT_IndentNo = a.PCINDENTAPT_IndentNo,
                                        PCINDENTAPT_SanctionedAmt = a.PCINDENTAPT_SanctionedAmt,
                                        createdate = a.PCINDENTAPT_CreatedDate,
                                        PCINDENT_Id = a.PCINDENT_Id,
                                    }).OrderByDescending(a => a.createdate).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PettyCash_Indent_Balance_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getbalancedetails = retObject.ToArray();
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
        public PC_Indent_ApprovalDTO OnChangeInstitution(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getloaddata = (from a in _context.PC_Indent_ApprovedDMO
                                    from b in _context.MasterEmployee
                                    from c in _context.HR_Master_Department
                                    where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                    select new PC_Indent_ApprovalDTO
                                    {
                                        departmentname = c.HRMD_DepartmentName,
                                        employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                        || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                        || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                        PCINDENTAPDT_ActiveFlg = a.PCINDENTAPT_ActiveFlg,
                                        PCINDENTAPT_Date = a.PCINDENTAPT_Date,
                                        PCINDENTAPT_Desc = a.PCINDENTAPT_Desc,
                                        PCINDENTAP_Id = a.PCINDENTAP_Id,
                                        PCINDENTAPT_IndentNo = a.PCINDENTAPT_IndentNo,
                                        PCINDENTAPT_SanctionedAmt = a.PCINDENTAPT_SanctionedAmt,
                                        createdate = a.PCINDENTAPT_CreatedDate,
                                        PCINDENT_Id = a.PCINDENT_Id,
                                    }).OrderByDescending(a => a.createdate).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PettyCash_Indent_Balance_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
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
                                    dataRow.Add(dataReader.GetName(iFiled),dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getbalancedetails = retObject.ToArray();
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
        public PC_Indent_ApprovalDTO onchangedate(PC_Indent_ApprovalDTO data)
        {
            try
            {
                List<string> indentno = new List<string>();
                List<long> indentid = new List<long>();

                var getsaveindent = _context.PC_Indent_ApprovedDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                if (getsaveindent.Count > 0)
                {
                    foreach (var c in getsaveindent)
                    {
                        indentid.Add(c.PCINDENT_Id);
                    }
                }

                data.indentdetails = (from a in _context.PC_IndentDMO
                                      from b in _context.MasterEmployee
                                      from c in _context.HR_Master_Department
                                      where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id
                                      && (a.PCINDENT_Date >= data.PCINDENT_Date_From && a.PCINDENT_Date <= data.PCINDENT_Date_To)
                                      && a.PCINDENT_ActiveFlg == true && !indentid.Contains(a.PCINDENT_Id))
                                      select new PC_Indent_ApprovalDTO
                                      {
                                          departmentname = c.HRMD_DepartmentName,
                                          employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                          || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                          || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                          PCINDENT_Date = a.PCINDENT_Date,
                                          PCINDENT_Desc = a.PCINDENT_Desc,
                                          PCINDENT_Id = a.PCINDENT_Id,
                                          PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                          PCINDENT_RequestedAmount = a.PCINDENT_RequestedAmount,
                                          PCINDENT_ApprovedAmt = a.PCINDENT_SanctionedAmt,
                                          createdate = a.PCINDENT_CreatedDate
                                      }).Distinct().OrderByDescending(a => a.createdate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO getindentdetails(PC_Indent_ApprovalDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                foreach (var c in data.temp_indentid)
                {
                    ids.Add(c.PCINDENT_Id);
                }

                data.indentparticulardetails = (from a in _context.PC_IndentDMO
                                                from b in _context.PC_Indent_DetailsDMO
                                                from c in _context.PC_Master_ParticularsDMO
                                                from d in _context.PC_RequisitionDMO
                                                from e in _context.MasterEmployee
                                                where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && ids.Contains(b.PCINDENT_Id)
                                                && d.PCREQTN_Id == b.PCREQTN_Id && e.HRME_Id == d.HRME_Id && a.PCINDENT_ActiveFlg == true && b.PCINDENTDET_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                                select new PC_Indent_ApprovalDTO
                                                {
                                                    pcindentdeT_Id = b.PCINDENTDET_Id,
                                                    PCMPART_Id = b.PCMPART_Id,
                                                    PCINDENT_Id = b.PCINDENT_Id,
                                                    PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                                    PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                    PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                                    PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                                    PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,
                                                    PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                                    employeename = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : " " + e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == ""
                                                    || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName) + (e.HRME_EmployeeCode == null
                                                    || e.HRME_EmployeeCode == "" ? "" : " : " + e.HRME_EmployeeCode)).Trim()
                                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO saverecord(PC_Indent_ApprovalDTO data)
        {
            try
            {
                var str = "";
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == data.Userid).Select(a => a.Emp_Code).FirstOrDefault();

                if (data.PCINDENTAP_Id > 0)
                {

                }
                else
                {
                    PC_Indent_ApprovedDMO pC_Indent_ApprovedDMO = new PC_Indent_ApprovedDMO
                    {
                        MI_Id = data.MI_Id,
                        HRME_Id = data.HRME_Id,
                        PCINDENTAPT_IndentNo = data.PCINDENTAPT_IndentNo,
                        PCINDENT_Id = data.PCINDENT_Id,
                        PCINDENTAPT_Date = data.PCINDENTAPT_Date,
                        PCINDENTAPT_Desc = data.PCINDENTAPT_Desc,
                        PCINDENTAPT_RequestedAmount = data.PCINDENTAPT_RequestedAmount,
                        PCINDENTAPT_SanctionedAmt = data.PCINDENTAPT_SanctionedAmt,
                        PCINDENTAPT_ActiveFlg = true,
                        PCINDENTAPT_CreatedDate = indiantime0,
                        PCINDENTAPT_UpdatedDate = indiantime0,
                        PCINDENTAPT_CreatedBy = data.Userid,
                        PCINDENTAPT_UpdatedBy = data.Userid
                    };

                    _context.Add(pC_Indent_ApprovedDMO);

                    if (data.PC_Indent_Approved_Details_DTO.Length > 0)
                    {
                        foreach (var c in data.PC_Indent_Approved_Details_DTO)
                        {
                            PC_Indent_Approved_DetailsDMO pC_Indent_Approved_DetailsDMO = new PC_Indent_Approved_DetailsDMO
                            {
                                PCINDENTAP_Id = pC_Indent_ApprovedDMO.PCINDENTAP_Id,
                                PCMPART_Id = c.PCMPART_Id,
                                PCINDENTAPDT_RequestedAmount = c.PCINDENTAPDT_RequestedAmount,
                                PCINDENTAPDT_Remarks = c.PCINDENTAPDT_Remarks,
                                PCINDENTAPDT_SanctionedAmt = c.PCINDENTAPDT_SanctionedAmt,
                                PCINDENTAPDT_ActiveFlg = true,
                                PCINDENTAPDT_CreatedDate = indiantime0,
                                PCINDENTAPDT_UpdatedDate = indiantime0,
                                PCINDENTAPDT_CreatedBy = data.Userid,
                                PCINDENTAPDT_UpdatedBy = data.Userid
                            };
                            _context.Add(pC_Indent_Approved_DetailsDMO);
                        }
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;

                        // Email Notification
                        var PCINDENTAP_Id = pC_Indent_ApprovedDMO.PCINDENTAP_Id;
                        try
                        {

                            var itemlist = (from a in _context.PC_Indent_ApprovedDMO
                                            from b in _context.PC_Indent_Approved_DetailsDMO
                                            from c in _context.PC_Master_ParticularsDMO
                                            where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENTAP_Id == PCINDENTAP_Id
                                            && a.MI_Id == data.MI_Id)
                                            select new PC_Indent_ApprovalDTO
                                            {
                                                PCINDENTAPDT_Id = b.PCINDENTAPDT_Id,
                                                PCMPART_Id = b.PCMPART_Id,
                                                PCINDENTAP_Id = b.PCINDENTAP_Id,
                                                PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                PCINDENTAPDT_RequestedAmount = b.PCINDENTAPDT_RequestedAmount,
                                                PCINDENTAPT_SanctionedAmt = b.PCINDENTAPDT_SanctionedAmt,
                                                PCINDENTAPDT_ActiveFlg = b.PCINDENTAPDT_ActiveFlg,
                                                PCINDENTAPDT_Remarks = b.PCINDENTAPDT_Remarks,

                                            }).Distinct().ToList();


                            if (itemlist.Count > 0)
                            {
                                str += "<table width=" + "'550px'" + " style=" + "'text-align: center;line-height:2;border:0;cellspacing:0;cellpadding: 0;background-color:#F5F5F5'" + "><tr>";

                                str += "<th width=" + "'5%'" + " style ='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'> SL.NO. </th>";
                                str += "<th  width=" + "'25%'" + " style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Particular Name" + "</th>";
                                str += "<th  width=" + "'15%'" + " style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Requested Amount" + "</th>";
                                str += "<th  width=" + "'15%'" + "  style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Sanctioned Amount" + "</th>";
                                str += "<th  width=" + "'40%'" + " style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Remarks" + "</th>";
                                str += "</tr>";
                                int rownumber = 0;

                                for (int j = 0; j < itemlist.Count; j++)
                                {
                                    rownumber += 1;

                                    str += "<tr>" +
                                  "<td style=" + "'vertical-align:top'" + "" + " >" + rownumber + "</td> ";
                                    str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCMPART_ParticularName + "</td>";
                                    str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCINDENTAPDT_RequestedAmount + "</td>";
                                    str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCINDENTAPT_SanctionedAmt + "</td>";
                                    str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCINDENTAPDT_Remarks + "</td>";


                                    str += "</tr>";
                                }

                                str += "</table>";
                            }


                            PettyCash_AutoRefernceNo_Generation pettyCash_AutoRefernceNo_Generation = new PettyCash_AutoRefernceNo_Generation(_context);
                            string emp_email = pettyCash_AutoRefernceNo_Generation.GetEmailIdForPettyCash(data.MI_Id, "PC_Indent_Approved_Notification", "Email");

                            if (itemlist.Count > 0)
                            {
                                //Email email = new Email(_Contextdb);
                                //email.PettyCash_Email_Notification_Send(data.MI_Id, emp_email, "PC_IndentApp", PCINDENTAP_Id, str);
                                var email = PettyCash_Email_Notification_Send(data.MI_Id, emp_email, "PC_IndentApp", PCINDENTAP_Id, str);

                            }
                        }
                        catch (Exception ex)
                        {
                            data.returnval = true;
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string PettyCash_Email_Notification_Send(long MI_Id, string Email, string Template, long UserID, string templatecontent)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Contextdb.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _Contextdb.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Contextdb.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _Contextdb.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

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


                    using (var cmd = _Contextdb.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "PettyCash_EmailNotification_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@template", SqlDbType.VarChar) { Value = Template });
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar) { Value = UserID });

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
                                        dataRow.Add(dataReader.GetName(iFiled),
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

                        if (ParamaetersName[j].ISMP_NAME.Equals("[MSG]"))
                        {
                            result = Mailmsg.Replace("[MSG]", templatecontent);
                            Mailmsg = result;
                        }

                    }


                    var ParamaetersName1 = _Contextdb.SMS_MAIL_PARAMETER_DMO.Where(i => (Paramaeters.Contains(i.ISMP_ID) && i.ISMP_NAME != "[MSG]")).ToList();

                    for (int j = 0; j < ParamaetersName1.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName1[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]) && !ParamaetersName1[j].ISMP_NAME.Equals("[MSG]"))
                            {
                                result = Mailmsg.Replace(ParamaetersName1[j].ISMP_NAME, val[ParamaetersName1[p].ISMP_NAME]);
                                Mailmsg = result;
                            }
                        }
                    }


                    Mailmsg = result;

                    //    for (int j = 0; j < ParamaetersName.Count; j++)
                    //    {
                    //        for (int p = 0; p < val.Count; p++)
                    //        {
                    //            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                    //            {
                    //                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                    //                Mailcontent = Resultsms;
                    //            }
                    //        }
                    //    }
                    //    Mailcontent = Resultsms;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Contextdb.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _Contextdb.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        //message.AddTo("punithnmsd2@gmail.com");
                        // CC Mail
                        if (template[0].ISES_EnableMailCCFlg == true)
                        {
                            if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                            {
                                string[] ccmail = template[0].ISES_MailCCId.Split(',');

                                foreach (var d in ccmail)
                                {
                                    message.AddCc(d);
                                }
                            }
                        }

                        // BCC Mail
                        if (template[0].ISES_EnableMailBCCFlg == true)
                        {
                            if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "'")
                            {
                                string[] bccmail = template[0].ISES_MailBCCId.Split(',');

                                foreach (var d in bccmail)
                                {
                                    message.AddBcc(d);
                                }
                            }
                        }

                        if (Attechement.Equals("1"))
                        {
                            var img = _Contextdb.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

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
                                    var img = _Contextdb.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }
                                        }
                                    }
                                }

                                // CC Mail
                                if (template[0].ISES_EnableMailCCFlg == true)
                                {
                                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                    {
                                        string[] ccmail = template[0].ISES_MailCCId.Split(',');

                                        foreach (var d in ccmail)
                                        {
                                            emailMessage.CC.Add(d);
                                        }
                                    }
                                }

                                // BCC Mail
                                if (template[0].ISES_EnableMailBCCFlg == true)
                                {
                                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                    {
                                        string[] bccmail = template[0].ISES_MailBCCId.Split(',');

                                        foreach (var d in bccmail)
                                        {
                                            emailMessage.Bcc.Add(d);
                                        }
                                    }
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }
                    }


                    using (var cmd = _Contextdb.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Contextdb.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Contextdb.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Contextdb.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
        public PC_Indent_ApprovalDTO Viewdata(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getviewdata = (from a in _context.PC_Indent_ApprovedDMO
                                    from b in _context.PC_Indent_Approved_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENTAP_Id == data.PCINDENTAP_Id
                                    && a.MI_Id == data.MI_Id)
                                    select new PC_Indent_ApprovalDTO
                                    {
                                        PCINDENTAPDT_Id = b.PCINDENTAPDT_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCINDENTAP_Id = b.PCINDENTAP_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCINDENTAPDT_RequestedAmount = b.PCINDENTAPDT_RequestedAmount,
                                        PCINDENTAPT_SanctionedAmt = b.PCINDENTAPDT_SanctionedAmt,
                                        PCINDENTAPDT_ActiveFlg = b.PCINDENTAPDT_ActiveFlg,
                                        PCINDENTAPDT_Remarks = b.PCINDENTAPDT_Remarks,

                                    }).Distinct().ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Expenditure
        public PC_Indent_ApprovalDTO ExpenditureLoaddata(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getindentapprovaldetails = (from a in _context.PC_Indent_ApprovedDMO
                                                 from b in _context.PC_IndentDMO
                                                 where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                                 && a.PCINDENTAPT_ActiveFlg == true && b.PCINDENT_ActiveFlg == true)
                                                 select new PC_Indent_ApprovalDTO
                                                 {
                                                     PCINDENTAP_Id = a.PCINDENTAP_Id,
                                                     PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                                     createdate = a.PCINDENTAPT_CreatedDate
                                                 }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                data.getparticularsdetails = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id && a.PCMPART_ActiveFlg == true).ToArray();

                var getuserinstitution = _context.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).ToArray();

                data.getexpenditureloaddata = (from a in _context.PC_ExpenditureDMO
                                               from b in _context.PC_Master_ParticularsDMO
                                               from c in _context.MasterEmployee
                                               where (a.PCMPART_Id == b.PCMPART_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id)
                                               select new PC_Indent_ApprovalDTO
                                               {
                                                   PCMPART_ParticularName = b.PCMPART_ParticularName,
                                                   PCEXPTR_Amount = a.PCEXPTR_Amount,
                                                   PCEXPTR_ExpenditureNo = a.PCEXPTR_ExpenditureNo,
                                                   PCEXPTR_Date = a.PCEXPTR_Date,
                                                   PCEXPTR_ModeOfPayment = a.PCEXPTR_ModeOfPayment,
                                                   PCEXPTR_ReferenceNo = a.PCEXPTR_ReferenceNo,
                                                   PCEXPTR_Id = a.PCEXPTR_Id,
                                                   PCEXPTR_Desc = a.PCEXPTR_Desc,
                                                   PCEXPTR_ActiveFlg = a.PCEXPTR_ActiveFlg,
                                                   employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " "
                                                   + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                   || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null
                                                   || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)
                                                   + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : " + c.HRME_EmployeeCode)).Trim(),
                                                   PCINDENTAPT_IndentNo = a.PCINDENTAP_Id > 0 ? _context.PC_Indent_ApprovedDMO.Where(k => k.PCINDENTAP_Id == a.PCINDENTAP_Id).FirstOrDefault().PCINDENTAPT_IndentNo : "",
                                                   createdate = a.PCEXPTR_CreatedDate
                                               }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                //data.getemp = (from a in _context.MasterEmployee
                //               where (a.MI_Id == data.MI_Id)
                //               select new PC_Indent_ApprovalDTO
                //               {
                //                   HRME_Id = a.HRME_Id,
                //                   empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                //                   (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                //                    (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                //               }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureInstitution(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getindentapprovaldetails = (from a in _context.PC_Indent_ApprovedDMO
                                                 from b in _context.PC_IndentDMO
                                                 where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                                 && a.PCINDENTAPT_ActiveFlg == true && b.PCINDENT_ActiveFlg == true)
                                                 select new PC_Indent_ApprovalDTO
                                                 {
                                                     PCINDENTAP_Id = a.PCINDENTAP_Id,
                                                     PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                                     createdate = a.PCINDENTAPT_CreatedDate
                                                 }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                data.getparticularsdetails = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id && a.PCMPART_ActiveFlg == true).ToArray();

                data.getexpenditureloaddata = (from a in _context.PC_ExpenditureDMO
                                               from b in _context.PC_Master_ParticularsDMO
                                               from c in _context.MasterEmployee
                                               where (a.PCMPART_Id == b.PCMPART_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id)
                                               select new PC_Indent_ApprovalDTO
                                               {
                                                   PCMPART_ParticularName = b.PCMPART_ParticularName,
                                                   PCEXPTR_Amount = a.PCEXPTR_Amount,
                                                   PCEXPTR_ExpenditureNo = a.PCEXPTR_ExpenditureNo,
                                                   PCEXPTR_Date = a.PCEXPTR_Date,
                                                   PCEXPTR_ModeOfPayment = a.PCEXPTR_ModeOfPayment,
                                                   PCEXPTR_ReferenceNo = a.PCEXPTR_ReferenceNo,
                                                   PCEXPTR_Id = a.PCEXPTR_Id,
                                                   PCEXPTR_Desc = a.PCEXPTR_Desc,
                                                   PCEXPTR_ActiveFlg = a.PCEXPTR_ActiveFlg,
                                                   employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " "
                                                   + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                   || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null
                                                   || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)
                                                   + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : " + c.HRME_EmployeeCode)).Trim(),
                                                   createdate = a.PCEXPTR_CreatedDate,
                                                   PCINDENTAPT_IndentNo = a.PCINDENTAP_Id > 0 ? _context.PC_Indent_ApprovedDMO.Where(k => k.PCINDENTAP_Id == a.PCINDENTAP_Id).FirstOrDefault().PCINDENTAPT_IndentNo : "",
                                               }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                data.getemp = (from a in _context.MasterEmployee
                               where (a.MI_Id == data.MI_Id)
                               select new PC_Indent_ApprovalDTO
                               {
                                   HRME_Id = a.HRME_Id,
                                   empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                   (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                                    (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                               }).Distinct().ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureIndent(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getparticularsdetails = (from a in _context.PC_Indent_ApprovedDMO
                                              from b in _context.PC_Indent_Approved_DetailsDMO
                                              from c in _context.PC_Master_ParticularsDMO
                                              where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id
                                              && b.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                              && b.PCINDENTAPDT_ActiveFlg == true)
                                              select c).Distinct().ToArray();

                data.getemp = (from a in _context.PC_Indent_ApprovedDMO
                               from b in _context.PC_Indent_Approved_DetailsDMO
                               from c in _context.PC_IndentDMO
                               from d in _context.PC_Indent_DetailsDMO
                               from e in _context.PC_RequisitionDMO
                               from f in _context.PC_Requisition_DetailsDMO
                               from g in _context.MasterEmployee
                               where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && a.PCINDENT_Id == c.PCINDENT_Id && c.PCINDENT_Id == d.PCINDENT_Id && d.PCREQTN_Id == e.PCREQTN_Id && f.PCREQTN_Id == e.PCREQTN_Id && e.HRME_Id == g.HRME_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id)
                               select new PC_Indent_ApprovalDTO
                               {
                                   HRME_Id = g.HRME_Id,
                                   empname = ((g.HRME_EmployeeFirstName == null || g.HRME_EmployeeFirstName == "" ? "" : " " + g.HRME_EmployeeFirstName) + (g.HRME_EmployeeMiddleName == null || g.HRME_EmployeeMiddleName == "" || g.HRME_EmployeeMiddleName == "0" ? "" : " " + g.HRME_EmployeeMiddleName) +
                                   (g.HRME_EmployeeLastName == null || g.HRME_EmployeeLastName == "" || g.HRME_EmployeeLastName == "0" ? "" : " " + g.HRME_EmployeeLastName) +
                                    (g.HRME_EmployeeCode == null || g.HRME_EmployeeCode == "" ? "" : " : " + g.HRME_EmployeeCode)).Trim(),

                               }).Distinct().ToArray();

                data.getviewdata = (from a in _context.PC_Indent_ApprovedDMO
                                    from b in _context.PC_Indent_Approved_DetailsDMO
                                    from c in _context.PC_IndentDMO
                                    from d in _context.PC_Indent_DetailsDMO
                                    from e in _context.PC_Master_ParticularsDMO
                                    from f in _context.PC_RequisitionDMO
                                    from g in _context.PC_Requisition_DetailsDMO
                                    from h in _context.MasterEmployee
                                    where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && a.PCINDENT_Id == c.PCINDENT_Id && c.PCINDENT_Id == d.PCINDENT_Id && d.PCMPART_Id == e.PCMPART_Id && b.PCMPART_Id == e.PCMPART_Id && d.PCREQTN_Id == f.PCREQTN_Id && f.PCREQTN_Id == g.PCREQTN_Id && f.HRME_Id == h.HRME_Id &&
                                   a.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id)
                                    select new PC_Indent_ApprovalDTO
                                    {
                                        PCINDENTAPDT_Id = b.PCINDENTAPDT_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCINDENTAP_Id = b.PCINDENTAP_Id,
                                        PCMPART_ParticularName = e.PCMPART_ParticularName,
                                        PCINDENTAPDT_RequestedAmount = b.PCINDENTAPDT_RequestedAmount,
                                        PCINDENTAPT_SanctionedAmt = b.PCINDENTAPDT_SanctionedAmt,
                                        PCINDENTAPDT_ActiveFlg = b.PCINDENTAPDT_ActiveFlg,
                                        PCINDENTAPDT_Remarks = b.PCINDENTAPDT_Remarks,
                                        empname = ((h.HRME_EmployeeFirstName == null || h.HRME_EmployeeFirstName == "" ? "" : " " + h.HRME_EmployeeFirstName) + (h.HRME_EmployeeMiddleName == null || h.HRME_EmployeeMiddleName == "" || h.HRME_EmployeeMiddleName == "0" ? "" : " " + h.HRME_EmployeeMiddleName) +
                                   (h.HRME_EmployeeLastName == null || h.HRME_EmployeeLastName == "" || h.HRME_EmployeeLastName == "0" ? "" : " " + h.HRME_EmployeeLastName) +
                                    (h.HRME_EmployeeCode == null || h.HRME_EmployeeCode == "" ? "" : " : " + h.HRME_EmployeeCode)).Trim(),

                                    }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureParticular(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getparticularsindentdetails = (from a in _context.PC_Indent_ApprovedDMO
                                                    from b in _context.PC_Indent_Approved_DetailsDMO
                                                    from c in _context.PC_Master_ParticularsDMO
                                                    where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id
                                                    && b.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                                    && b.PCINDENTAPDT_ActiveFlg == true && b.PCMPART_Id == data.PCMPART_Id)
                                                    select new PC_Indent_ApprovalDTO
                                                    {
                                                        PCINDENTAPDT_RequestedAmount = b.PCINDENTAPDT_RequestedAmount,
                                                        PCINDENTAPDT_SanctionedAmt = b.PCINDENTAPDT_SanctionedAmt,
                                                        PCINDENTAPDT_AmountSpent = b.PCINDENTAPDT_AmountSpent,
                                                        PCINDENTAPDT_BalanceAmount = b.PCINDENTAPDT_BalanceAmount,
                                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                        PCMPART_Id = c.PCMPART_Id,
                                                        PCINDENTAPDT_Id = b.PCINDENTAPDT_Id,
                                                    }).Distinct().ToArray();

                data.getindentdetails = (from a in _context.PC_Indent_ApprovedDMO
                                         from b in _context.PC_IndentDMO
                                         where (a.PCINDENT_Id == b.PCINDENT_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id)
                                         select new PC_Indent_ApprovalDTO
                                         {
                                             PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                             PCINDENTAPT_RequestedAmount = a.PCINDENTAPT_RequestedAmount,
                                             PCINDENTAPT_SanctionedAmt = a.PCINDENTAPT_SanctionedAmt,
                                             PCINDENTAPT_AmountSpent = a.PCINDENTAPT_AmountSpent,
                                             PCINDENTAPT_BalanceAmount = a.PCINDENTAPT_BalanceAmount,
                                             PCINDENTAPDT_Remarks = b.PCINDENT_Desc

                                         }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO SaveExpenditure(PC_Indent_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //data.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == data.Userid).Select(a => a.Emp_Code).FirstOrDefault();

                var acd_Id = _context.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(indiantime0.Date) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(indiantime0.Date)).Select(d => d.IMFY_Id).FirstOrDefault();

                if (data.PCEXPTR_Id > 0)
                {
                    data.saveorupdate = "Update";

                    var count = 0;
                    if (data.PCINDENTAP_Id > 0)
                    {
                        var updatedcount = _context.Database.ExecuteSqlCommand("PC_Expenditure_Update_Modify @p0,@p1,@p2,@p3,@p4,@p4", data.MI_Id, data.PCINDENTAP_Id, data.PCMPART_Id, data.PCEXPTR_Amount, data.Userid, data.PCEXPTR_Id);

                        if (updatedcount > 0)
                        {
                            count = 1;
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    else
                    {
                        count = 1;
                    }
                    if (count > 0)
                    {
                        var result = _context.PC_ExpenditureDMO.Single(a => a.PCEXPTR_Id == data.PCEXPTR_Id);
                        result.HRME_Id = data.HRME_Id;
                        result.PCEXPTR_Amount = data.PCEXPTR_Amount;
                        result.PCEXPTR_Date = data.PCEXPTR_Date;
                        result.PCEXPTR_ModeOfPayment = data.PCEXPTR_ModeOfPayment;
                        result.PCEXPTR_Desc = data.PCEXPTR_Desc;
                        result.PCEXPTR_UpdatedDate = indiantime0;
                        result.PCEXPTR_UpdatedBy = data.Userid;
                        _context.Update(result);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.saveorupdate = "Error";
                    }
                }
                else
                {
                    PettyCash_AutoRefernceNo_Generation pettyCash_AutoRefernceNo_Generation = new PettyCash_AutoRefernceNo_Generation(_context);
                    var refernceno = pettyCash_AutoRefernceNo_Generation.GenerateReferenceNo(data.MI_Id, acd_Id, "Expenditure");

                    if (refernceno == "" || refernceno == "Error")
                    {
                        data.saveorupdate = "ReferenceNotGenerated";
                        return data;
                    }

                    var count = 0;
                    if (data.PCINDENTAP_Id > 0)
                    {
                        var updatedcount = _context.Database.ExecuteSqlCommand("PC_Expenditure_Update @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.PCINDENTAP_Id, data.PCMPART_Id, data.PCEXPTR_Amount, data.Userid);

                        if (updatedcount > 0)
                        {
                            count = 1;
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    else
                    {
                        count = 1;
                    }

                    if (count > 0)
                    {
                        data.saveorupdate = "Add";
                        PC_ExpenditureDMO pC_ExpenditureDMO = new PC_ExpenditureDMO();
                        pC_ExpenditureDMO.MI_Id = data.MI_Id;
                        pC_ExpenditureDMO.HRME_Id = data.HRME_Id;
                        pC_ExpenditureDMO.PCMPART_Id = data.PCMPART_Id;
                        pC_ExpenditureDMO.PCEXPTR_Amount = data.PCEXPTR_Amount;
                        pC_ExpenditureDMO.PCEXPTR_ExpenditureNo = refernceno;

                        if (data.PCINDENTAP_Id > 0)
                        {
                            pC_ExpenditureDMO.PCINDENTAP_Id = data.PCINDENTAP_Id;
                        }
                        pC_ExpenditureDMO.PCEXPTR_Date = data.PCEXPTR_Date;
                        pC_ExpenditureDMO.PCEXPTR_Desc = data.PCEXPTR_Desc;
                        pC_ExpenditureDMO.PCEXPTR_ModeOfPayment = data.PCEXPTR_ModeOfPayment;
                        pC_ExpenditureDMO.PCEXPTR_ReferenceNo = data.PCEXPTR_ReferenceNo;
                        pC_ExpenditureDMO.PCEXPTR_ActiveFlg = true;
                        pC_ExpenditureDMO.PCEXPTR_CreatedDate = indiantime0;
                        pC_ExpenditureDMO.PCEXPTR_UpdatedDate = indiantime0;
                        pC_ExpenditureDMO.PCEXPTR_CreatedBy = data.Userid;
                        pC_ExpenditureDMO.PCEXPTR_UpdatedBy = data.Userid;

                        _context.Add(pC_ExpenditureDMO);

                        var i = _context.SaveChanges();

                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.saveorupdate = "Error";
                    }
                }

                data.getexpenditureloaddata = (from a in _context.PC_ExpenditureDMO
                                               from b in _context.PC_Master_ParticularsDMO
                                               from c in _context.MasterEmployee
                                               where (a.PCMPART_Id == b.PCMPART_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id)
                                               select new PC_Indent_ApprovalDTO
                                               {
                                                   PCMPART_ParticularName = b.PCMPART_ParticularName,
                                                   PCEXPTR_Amount = a.PCEXPTR_Amount,
                                                   PCEXPTR_ExpenditureNo = a.PCEXPTR_ExpenditureNo,
                                                   PCEXPTR_Date = a.PCEXPTR_Date,
                                                   PCEXPTR_ModeOfPayment = a.PCEXPTR_ModeOfPayment,
                                                   PCEXPTR_ReferenceNo = a.PCEXPTR_ReferenceNo,
                                                   PCEXPTR_Id = a.PCEXPTR_Id,
                                                   PCEXPTR_Desc = a.PCEXPTR_Desc,
                                                   PCEXPTR_ActiveFlg = a.PCEXPTR_ActiveFlg,
                                                   employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " "
                                                   + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                   || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null
                                                   || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)
                                                   + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : " + c.HRME_EmployeeCode)).Trim(),
                                                   PCINDENTAPT_IndentNo = a.PCINDENTAP_Id > 0 ? _context.PC_Indent_ApprovedDMO.Where(k => k.PCINDENTAP_Id == a.PCINDENTAP_Id).FirstOrDefault().PCINDENTAPT_IndentNo : "",
                                                   createdate = a.PCEXPTR_CreatedDate,
                                               }).Distinct().OrderByDescending(a => a.createdate).ToArray();
            }
            catch (Exception ex)
            {
                data.saveorupdate = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO EditExpenditure(PC_Indent_ApprovalDTO data)
        {
            try
            {
                var EditExpenditureDetails = _context.PC_ExpenditureDMO.Where(a => a.MI_Id == data.MI_Id && a.PCEXPTR_Id == data.PCEXPTR_Id).ToList();

                data.PCINDENTAP_Id = Convert.ToInt64(EditExpenditureDetails.FirstOrDefault().PCINDENTAP_Id);
                data.PCMPART_Id = EditExpenditureDetails.FirstOrDefault().PCMPART_Id;

                data.Editexpendituredetails = EditExpenditureDetails.ToArray();

                data.getparticularsdetails = (from a in _context.PC_Indent_ApprovedDMO
                                              from b in _context.PC_Indent_Approved_DetailsDMO
                                              from c in _context.PC_Master_ParticularsDMO
                                              where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id
                                              && b.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                              && b.PCINDENTAPDT_ActiveFlg == true)
                                              select c).Distinct().ToArray();

                data.getparticularsindentdetails = (from a in _context.PC_Indent_ApprovedDMO
                                                    from b in _context.PC_Indent_Approved_DetailsDMO
                                                    from c in _context.PC_Master_ParticularsDMO
                                                    where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id
                                                    && b.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                                    && b.PCINDENTAPDT_ActiveFlg == true && b.PCMPART_Id == data.PCMPART_Id)
                                                    select new PC_Indent_ApprovalDTO
                                                    {
                                                        PCINDENTAPDT_RequestedAmount = b.PCINDENTAPDT_RequestedAmount,
                                                        PCINDENTAPDT_SanctionedAmt = b.PCINDENTAPDT_SanctionedAmt,
                                                        PCINDENTAPDT_AmountSpent = b.PCINDENTAPDT_AmountSpent,
                                                        PCINDENTAPDT_BalanceAmount = b.PCINDENTAPDT_BalanceAmount,
                                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                        PCMPART_Id = c.PCMPART_Id,
                                                        PCINDENTAPDT_Id = b.PCINDENTAPDT_Id,
                                                    }).Distinct().ToArray();

                data.getindentdetails = (from a in _context.PC_Indent_ApprovedDMO
                                         from b in _context.PC_IndentDMO
                                         where (a.PCINDENT_Id == b.PCINDENT_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id)
                                         select new PC_Indent_ApprovalDTO
                                         {
                                             PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                             PCINDENTAPT_RequestedAmount = a.PCINDENTAPT_RequestedAmount,
                                             PCINDENTAPT_SanctionedAmt = a.PCINDENTAPT_SanctionedAmt,
                                             PCINDENTAPT_AmountSpent = a.PCINDENTAPT_AmountSpent,
                                             PCINDENTAPT_BalanceAmount = a.PCINDENTAPT_BalanceAmount,

                                         }).Distinct().ToArray();

                data.geteditemp = (from a in _context.MasterEmployee
                                   where (a.MI_Id == data.MI_Id && a.HRME_Id == EditExpenditureDetails.FirstOrDefault().HRME_Id)
                                   select new PC_Indent_ApprovalDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                       (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                                        (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                                   }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                data.saveorupdate = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO DeleteExpenditure(PC_Indent_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.PC_ExpenditureDMO.Where(a => a.MI_Id == data.MI_Id && a.PCEXPTR_Id == data.PCEXPTR_Id).ToList();

                if (checkresult.Count > 0)
                {
                    long? indentid = 0;
                    if (checkresult.FirstOrDefault().PCINDENTAP_Id > 0)
                    {
                        indentid = checkresult.FirstOrDefault().PCINDENTAP_Id;
                    }

                    var updatedcount = _context.Database.ExecuteSqlCommand("PC_Expenditure_Delete @p0,@p1,@p2,@p3,@p4,@p5,@p6", data.MI_Id,
                        indentid, checkresult.FirstOrDefault().PCMPART_Id, checkresult.FirstOrDefault().PCEXPTR_Amount, data.Userid, data.PCEXPTR_Id,
                        data.PCEXPTR_DeletedRemarks);

                    if (updatedcount > 0)
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
                data.saveorupdate = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Expenditure Indent Mapping
        public PC_Indent_ApprovalDTO ExpenditureIndentMappingLoaddata(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getindentapprovaldetails = (from a in _context.PC_Indent_ApprovedDMO
                                                 from b in _context.PC_IndentDMO
                                                 where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                                 && a.PCINDENTAPT_ActiveFlg == true && b.PCINDENT_ActiveFlg == true)
                                                 select new PC_Indent_ApprovalDTO
                                                 {
                                                     PCINDENTAP_Id = a.PCINDENTAP_Id,
                                                     PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                                     createdate = a.PCINDENTAPT_CreatedDate
                                                 }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                data.getparticularsdetails = (from a in _context.PC_ExpenditureDMO
                                              from b in _context.PC_Master_ParticularsDMO
                                              where (a.PCMPART_Id == b.PCMPART_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                              && (a.PCINDENTAP_Id == null || a.PCINDENTAP_Id == 0) && a.PCEXPTR_ActiveFlg == true)
                                              select new PC_Indent_ApprovalDTO
                                              {
                                                  PCEXPTR_Id = a.PCEXPTR_Id,
                                                  PCMPART_ParticularName = b.PCMPART_ParticularName + " : " +
                                                  Convert.ToDateTime(a.PCEXPTR_Date.Value.Date.ToString("dd/MM/yyyy")).ToString("dd/MM/yyyy"),
                                                  PCMPART_Id = b.PCMPART_Id,
                                                  createdate = a.PCEXPTR_CreatedDate

                                              }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                var getuserinstitution = _context.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureIndentMappingInstitution(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getindentapprovaldetails = (from a in _context.PC_Indent_ApprovedDMO
                                                 from b in _context.PC_IndentDMO
                                                 where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                                 && a.PCINDENTAPT_ActiveFlg == true && b.PCINDENT_ActiveFlg == true)
                                                 select new PC_Indent_ApprovalDTO
                                                 {
                                                     PCINDENTAP_Id = a.PCINDENTAP_Id,
                                                     PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                                     createdate = a.PCINDENTAPT_CreatedDate
                                                 }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                data.getparticularsdetails = (from a in _context.PC_ExpenditureDMO
                                              from b in _context.PC_Master_ParticularsDMO
                                              where (a.PCMPART_Id == b.PCMPART_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                              && (a.PCINDENTAP_Id == null || a.PCINDENTAP_Id == 0))
                                              select new PC_Indent_ApprovalDTO
                                              {
                                                  PCEXPTR_Id = a.PCEXPTR_Id,
                                                  PCMPART_ParticularName = b.PCMPART_ParticularName + " : " +
                                                  Convert.ToDateTime(a.PCEXPTR_Date.Value.Date.ToString("dd/MM/yyyy")),
                                                  PCMPART_Id = b.PCMPART_Id,
                                                  createdate = a.PCEXPTR_CreatedDate

                                              }).Distinct().OrderByDescending(a => a.createdate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO SearchExpenditureMappingDetails(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getselectedexpenparticularsdetails = (from a in _context.PC_ExpenditureDMO
                                                           from b in _context.PC_Master_ParticularsDMO
                                                           from c in _context.MasterEmployee
                                                           where (a.PCMPART_Id == b.PCMPART_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id
                                                           && a.PCEXPTR_Id == data.PCEXPTR_Id && a.PCMPART_Id == data.PCMPART_Id
                                                           && b.PCMPART_Id == data.PCMPART_Id)
                                                           select new PC_Indent_ApprovalDTO
                                                           {
                                                               PCEXPTR_Id = a.PCEXPTR_Id,
                                                               PCMPART_Id = a.PCMPART_Id,
                                                               PCEXPTR_ExpenditureNo = a.PCEXPTR_ExpenditureNo,
                                                               PCMPART_ParticularName = b.PCMPART_ParticularName,
                                                               PCEXPTR_Amount = a.PCEXPTR_Amount,
                                                               PCEXPTR_Date = a.PCEXPTR_Date,
                                                               PCEXPTR_ReferenceNo = a.PCEXPTR_ReferenceNo,
                                                               employeename = ((c.HRME_EmployeeFirstName == null
                                                               || c.HRME_EmployeeFirstName == "" ? "" : " " + c.HRME_EmployeeFirstName) +
                                                               (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" ||
                                                               c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) +
                                                               (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == ""
                                                               || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName) +
                                                               (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : " +
                                                               c.HRME_EmployeeCode)).Trim(),
                                                           }).Distinct().ToArray();

                data.getselectedindentdetails = _context.PC_Indent_ApprovedDMO.Where(a => a.PCINDENTAP_Id == data.PCINDENTAP_Id).ToArray();

                data.getselectedindentparticulardetails = (from a in _context.PC_Indent_ApprovedDMO
                                                           from b in _context.PC_Indent_Approved_DetailsDMO
                                                           from c in _context.PC_Master_ParticularsDMO
                                                           where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id
                                                           && a.PCINDENTAP_Id == data.PCINDENTAP_Id && b.PCMPART_Id == data.PCMPART_Id)
                                                           select new PC_Indent_ApprovalDTO
                                                           {
                                                               PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                               PCINDENTAPDT_SanctionedAmt = b.PCINDENTAPDT_SanctionedAmt,
                                                               PCINDENTAPDT_AmountSpent = b.PCINDENTAPDT_AmountSpent,
                                                               PCINDENTAPDT_BalanceAmount = b.PCINDENTAPDT_BalanceAmount,
                                                               PCINDENTAPDT_Remarks = b.PCINDENTAPDT_Remarks,
                                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO SaveExpenditureMappingDetails(PC_Indent_ApprovalDTO data)
        {
            try
            {
                var getexpenditure = _context.PC_ExpenditureDMO.Where(a => a.MI_Id == data.MI_Id && a.PCEXPTR_Id == data.PCEXPTR_Id).ToList();


                var updatedcount = _context.Database.ExecuteSqlCommand("PC_Expenditure_IndentMapping_Update @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id,
                     data.PCINDENTAP_Id, data.PCMPART_Id, getexpenditure.FirstOrDefault().PCEXPTR_Amount, data.Userid, data.PCEXPTR_Id);
                if (updatedcount > 0)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}