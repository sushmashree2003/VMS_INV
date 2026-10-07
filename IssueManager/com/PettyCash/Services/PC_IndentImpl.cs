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
    public class PC_IndentImpl : Interface.PC_IndentInterface
    {
        public PettyCashContext _context;
        public DomainModelMsSqlServerContext _Contextdb;
        public PC_IndentImpl(PettyCashContext _con, DomainModelMsSqlServerContext _Contextdbsql)
        {
            _context = _con;
            _Contextdb = _Contextdbsql;
        }
        public PC_IndentDTO onloaddata(PC_IndentDTO data)
        {
            try
            {
                var rolet = _context.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleid).ToList();
                var roletype = "";

                var getuserinstitution = _context.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                var getloaddata = (from a in _context.PC_IndentDMO
                                    from b in _context.MasterEmployee
                                    from c in _context.HR_Master_Department
                                    where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                    select new PC_IndentDTO
                                    {
                                        departmentname = c.HRMD_DepartmentName,
                                        employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " "
                                        + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == ""
                                        || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null
                                        || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)
                                        + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                        PCINDENT_ActiveFlg = a.PCINDENT_ActiveFlg,
                                        PCINDENT_Date = a.PCINDENT_Date,
                                        PCINDENT_Desc = a.PCINDENT_Desc,
                                        PCINDENT_Id = a.PCINDENT_Id,
                                        PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                        createdate = a.PCINDENT_CreatedDate,
                                        PCINDENT_CreatedBy = a.PCINDENT_CreatedBy
                                    }).OrderByDescending(a => a.createdate).ToArray();

                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;

                    if (!roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) && !roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        getloaddata = getloaddata.Where(a => a.PCINDENT_CreatedBy == data.Userid).ToArray();
                    }
                }
                

                data.getloaddata = getloaddata;

                data.getapproveddata = _context.PC_Indent_ApprovedDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();

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
        public PC_IndentDTO OnChangeInstitution(PC_IndentDTO data)
        {
            try
            {
                var rolet = _context.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleid).ToList();
                var roletype = "";

                var getloaddata = (from a in _context.PC_IndentDMO
                                   from b in _context.MasterEmployee
                                   from c in _context.HR_Master_Department
                                   where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                   select new PC_IndentDTO
                                   {
                                       departmentname = c.HRMD_DepartmentName,
                                       employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " "
                                       + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == ""
                                       || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null
                                       || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)
                                       + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                       PCINDENT_ActiveFlg = a.PCINDENT_ActiveFlg,
                                       PCINDENT_Date = a.PCINDENT_Date,
                                       PCINDENT_Desc = a.PCINDENT_Desc,
                                       PCINDENT_Id = a.PCINDENT_Id,
                                       PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                       createdate = a.PCINDENT_CreatedDate,
                                       PCINDENT_CreatedBy = a.PCINDENT_CreatedBy
                                   }).OrderByDescending(a => a.createdate).ToArray();
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (!roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) && !roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        getloaddata = getloaddata.Where(a => a.PCINDENT_CreatedBy == data.Userid).ToArray();
                    }
                }

                data.getapproveddata = _context.PC_Indent_ApprovedDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

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
        public PC_IndentDTO onchangedate(PC_IndentDTO data)
        {
            try
            {
                List<long> PCREQTN_Ids = new List<long>();

                var getrequisitionids = (from a in _context.PC_IndentDMO
                                         from b in _context.PC_Indent_DetailsDMO
                                         where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id)
                                         select new PC_IndentDTO
                                         {
                                             PCREQTN_Id = b.PCREQTN_Id
                                         }).Distinct().ToList();

                if (getrequisitionids.Count > 0)
                {
                    foreach (var c in getrequisitionids)
                    {
                        PCREQTN_Ids.Add(c.PCREQTN_Id);
                    }
                }
                else
                {
                    PCREQTN_Ids.Add(0);
                }

                data.requisitiondetais = (from a in _context.PC_RequisitionDMO
                                          from b in _context.MasterEmployee
                                          from c in _context.HR_Master_Department
                                          where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && !PCREQTN_Ids.Contains(a.PCREQTN_Id) && a.MI_Id == data.MI_Id
                                          && (a.PCREQTN_Date >= data.PCINDENT_Date_From && a.PCREQTN_Date <= data.PCINDENT_Date_To)
                                          && a.PCREQTN_ActiveFlg == true)
                                          select new PC_IndentDTO
                                          {
                                              departmentname = c.HRMD_DepartmentName,
                                              employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName)
                                              + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" :
                                              " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                              || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                              || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                              PCREQTN_Date = a.PCREQTN_Date,
                                              PCREQTN_Purpose = a.PCREQTN_Purpose,
                                              PCREQTN_Id = a.PCREQTN_Id,
                                              PCREQTN_RequisitionNo = a.PCREQTN_RequisitionNo,
                                              PCREQTN_TotAmount = a.PCREQTN_TotAmount,
                                              createdate = a.PCREQTN_CreatedDate
                                          }).Distinct().OrderByDescending(a => a.createdate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO getrequisitiondetails(PC_IndentDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                foreach (var c in data.temp_requisitionid)
                {
                    ids.Add(c.PCREQTN_Id);
                }

                data.requisitionparticulardetais = (from a in _context.PC_RequisitionDMO
                                                    from b in _context.PC_Requisition_DetailsDMO
                                                    from c in _context.PC_Master_ParticularsDMO
                                                    where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && ids.Contains(b.PCREQTN_Id)
                                                    && a.PCREQTN_ActiveFlg == true && b.PCREQTNDET_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                                    select new PC_IndentDTO
                                                    {
                                                        pcreqtndeT_Id = b.PCREQTNDET_Id,
                                                        PCMPART_Id = b.PCMPART_Id,
                                                        PCREQTN_Id = b.PCREQTN_Id,
                                                        PCREQTN_RequisitionNo = a.PCREQTN_RequisitionNo,
                                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                        PCREQTNDET_Amount = b.PCREQTNDET_Amount,
                                                        PCREQTNDET_Remarks = b.PCREQTNDET_Remarks,
                                                        PCREQTNDET_ActiveFlg = b.PCREQTNDET_ActiveFlg,
                                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO saverecord(PC_IndentDTO data)
        {
            try
            {
                var str = "";
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == data.Userid).Select(a => a.Emp_Code).FirstOrDefault();

                if (data.PCINDENT_Id > 0)
                {
                    var checkid = _context.PC_IndentDMO.Where(a => a.PCINDENT_Id == data.PCINDENT_Id).ToList();

                    if (checkid.Count > 0)
                    {
                        var checkidresult = _context.PC_IndentDMO.Single(a => a.PCINDENT_Id == data.PCINDENT_Id);
                        checkidresult.PCINDENT_Date = data.PCINDENT_Date;
                        checkidresult.PCINDENT_Desc = data.PCINDENT_Desc;
                        checkidresult.PCINDENT_UpdatedDate = indiantime0;
                        checkidresult.PCINDENT_UpdatedBy = data.Userid;
                        checkidresult.PCINDENT_SanctionedAmt = data.PCINDENT_ApprovedAmt;

                        _context.Update(checkidresult);

                        List<long> temparr = new List<long>();
                        List<long> temparr1 = new List<long>();

                        //GETTING ALL FILES
                        foreach (PC_Indent_DetailsDTO ph in data.PC_Indent_DetailsDTO)
                        {
                            temparr.Add(ph.PCINDENTDET_Id);
                        }

                        //REMOVING FILES
                        Array files_Noresultremove = _context.PC_Indent_DetailsDMO.Where(t => !temparr.Contains(t.PCINDENTDET_Id)
                        && t.PCREQTN_Id == data.PCREQTN_Id).ToArray();
                        foreach (PC_Indent_DetailsDMO ph1 in files_Noresultremove)
                        {
                            _context.Remove(ph1);
                        }

                        foreach (PC_Indent_DetailsDTO ph in data.PC_Indent_DetailsDTO)
                        {
                            if (ph.PCINDENTDET_Id > 0)
                            {
                                var Files_Noresult = _context.PC_Indent_DetailsDMO.Single(t => t.PCINDENTDET_Id == ph.PCINDENTDET_Id);
                                Files_Noresult.PCINDENTDET_Remarks = ph.PCINDENTDET_Remarks;
                                Files_Noresult.PCINDENTDET_SanctionedAmt = ph.PCINDENTDET_ApprovedAmt;
                                Files_Noresult.PCINDENTDET_UpdatedBy = data.Userid;
                                Files_Noresult.PCINDENTDET_UpdatedDate = indiantime0;
                                _context.Update(Files_Noresult);
                            }
                        }

                        var i = _context.SaveChanges();

                        if (i > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    var acd_Id = _context.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(indiantime0.Date) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(indiantime0.Date)).Select(d => d.IMFY_Id).FirstOrDefault();

                    PettyCash_AutoRefernceNo_Generation pettyCash_AutoRefernceNo_Generation = new PettyCash_AutoRefernceNo_Generation(_context);
                    var refernceno = pettyCash_AutoRefernceNo_Generation.GenerateReferenceNo(data.MI_Id, acd_Id, "Indent");

                    if (refernceno == "" || refernceno == "Error")
                    {
                        data.saveorupdate = "ReferenceNotGenerated";
                        return data;
                    }

                    PC_IndentDMO pC_IndentDMO = new PC_IndentDMO
                    {
                        MI_Id = data.MI_Id,
                        HRME_Id = data.HRME_Id,
                        PCINDENT_IndentNo = refernceno,
                        PCINDENT_Date = data.PCINDENT_Date,
                        PCINDENT_Desc = data.PCINDENT_Desc,
                        PCINDENT_RequestedAmount = data.PCINDENT_RequestedAmount,
                        PCINDENT_SanctionedAmt = data.PCINDENT_ApprovedAmt,
                       
                        PCINDENT_ActiveFlg = true,
                        PCINDENT_CreatedDate = indiantime0,
                        PCINDENT_UpdatedDate = indiantime0,
                        PCINDENT_CreatedBy = data.Userid,
                        PCINDENT_UpdatedBy = data.Userid
                    };
                    _context.Add(pC_IndentDMO);

                    foreach (var c in data.PC_Indent_DetailsDTO)
                    {
                        PC_Indent_DetailsDMO pC_Indent_DetailsDMO = new PC_Indent_DetailsDMO
                        {
                            PCINDENT_Id = pC_IndentDMO.PCINDENT_Id,
                            PCMPART_Id = c.PCMPART_Id,
                            PCREQTN_Id = c.PCREQTN_Id,
                            PCINDENTDET_RequestedAmount = c.PCINDENTDET_Amount,
                            PCINDENTDET_Remarks = c.PCINDENTDET_Remarks,
                            PCINDENTDET_SanctionedAmt = c.PCINDENTDET_ApprovedAmt,
                            PCINDENTDET_ActiveFlg = true,
                            PCINDENTDET_CreatedDate = indiantime0,
                            PCINDENTDET_UpdatedDate = indiantime0,
                            PCINDENTDET_CreatedBy = data.Userid,
                            PCINDENTDET_UpdatedBy = data.Userid
                        };
                        _context.Add(pC_Indent_DetailsDMO);
                    }

                    var i = _context.SaveChanges();

                    if (i > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                        var PCINDENT_Id = pC_IndentDMO.PCINDENT_Id;
                        try
                        {
                            var itemlist = (from a in _context.PC_IndentDMO
                                            from b in _context.PC_Indent_DetailsDMO
                                            from c in _context.PC_Master_ParticularsDMO
                                            where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENT_Id == PCINDENT_Id && a.MI_Id == data.MI_Id)
                                            select new PC_IndentDTO
                                            {
                                                PCINDENTDET_Id = b.PCINDENTDET_Id,
                                                PCMPART_Id = b.PCMPART_Id,
                                                PCINDENT_Id = b.PCINDENT_Id,
                                                PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                                PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                                PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                                PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,

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
                                    str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCINDENTDET_Amount + "</td>";
                                    str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCINDENTDET_ApprovedAmt + "</td>";
                                    str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCINDENTDET_Remarks + "</td>";


                                    str += "</tr>";
                                }

                                str += "</table>";
                            }


                            if (itemlist.Count > 0)
                            {
                                string emp_email = pettyCash_AutoRefernceNo_Generation.GetEmailIdForPettyCash(data.MI_Id, "PC_Indent_Notification", "Email");
                                if (emp_email != "")
                                {
                                    //Email email = new Email(_Contextdb);
                                    var email = PettyCash_Email_Notification_Send(data.MI_Id, emp_email, "PC_Indent", PCINDENT_Id, str);

                                }
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
        public PC_IndentDTO EditData(PC_IndentDTO data)
        {
            try
            {
                data.geteditdata = _context.PC_IndentDMO.Where(a => a.PCINDENT_Id == data.PCINDENT_Id && a.MI_Id == data.MI_Id).ToArray();

                data.geteditparticularsdata = (from a in _context.PC_IndentDMO
                                               from b in _context.PC_Indent_DetailsDMO
                                               from c in _context.PC_Master_ParticularsDMO
                                               where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENT_Id == data.PCINDENT_Id
                                                && a.MI_Id == data.MI_Id)
                                               select new PC_IndentDTO
                                               {
                                                   PCINDENTDET_Id = b.PCINDENTDET_Id,
                                                   PCMPART_Id = b.PCMPART_Id,
                                                   PCINDENT_Id = b.PCINDENT_Id,
                                                   PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                   PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                                   PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                                   PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                                   PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,
                                                   PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                                   PCINDENT_Desc = a.PCINDENT_Desc

                                               }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO deactiveY(PC_IndentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getresult = _context.PC_IndentDMO.Where(a => a.PCINDENT_Id == data.PCINDENT_Id && a.MI_Id == data.MI_Id).ToList();

                if (getresult.Count > 0)
                {
                    var checkresult = _context.PC_IndentDMO.Single(a => a.PCINDENT_Id == data.PCINDENT_Id && a.MI_Id == data.MI_Id);
                    checkresult.PCINDENT_ActiveFlg = checkresult.PCINDENT_ActiveFlg == true ? false : true;
                    checkresult.PCINDENT_UpdatedDate = indiantime0;
                    checkresult.PCINDENT_UpdatedBy = data.Userid;
                    _context.Update(checkresult);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO Viewdata(PC_IndentDTO data)
        {
            try
            {
                data.getviewdata = (from a in _context.PC_IndentDMO
                                    from b in _context.PC_Indent_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENT_Id == data.PCINDENT_Id
                                     && a.MI_Id == data.MI_Id)
                                    select new PC_IndentDTO
                                    {
                                        PCINDENTDET_Id = b.PCINDENTDET_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCINDENT_Id = b.PCINDENT_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                        PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                        PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                        PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO Viewdataprint(PC_IndentDTO data)
        {
            try
            {
                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PC_IndentDetails_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PCINDENTAP_Id", SqlDbType.VarChar)
                    { Value = data.PCINDENTAP_Id });
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
                        data.getviewdata = retObject.ToArray();
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
        public PC_IndentDTO deactiveparticulars(PC_IndentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getresult = _context.PC_Indent_DetailsDMO.Where(a => a.PCINDENTDET_Id == data.PCINDENTDET_Id).ToList();

                if (getresult.Count > 0)
                {
                    var checkresult = _context.PC_Indent_DetailsDMO.Single(a => a.PCINDENTDET_Id == data.PCINDENTDET_Id);
                    checkresult.PCINDENTDET_ActiveFlg = checkresult.PCINDENTDET_ActiveFlg == true ? false : true;
                    checkresult.PCINDENTDET_UpdatedDate = indiantime0;
                    checkresult.PCINDENTDET_UpdatedBy = data.Userid;

                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        decimal? totamount = 0;
                        var updateamount = _context.PC_Indent_DetailsDMO.Where(a => a.PCINDENT_Id == data.PCINDENT_Id && a.PCINDENTDET_ActiveFlg == true).ToList();

                        if (updateamount.Count > 0)
                        {
                            foreach (var c in updateamount)
                            {
                                totamount += c.PCINDENTDET_SanctionedAmt;
                            }

                            var checkresult_main = _context.PC_IndentDMO.Single(a => a.PCINDENT_Id == data.PCINDENT_Id);
                            checkresult_main.PCINDENT_SanctionedAmt = totamount;
                            checkresult_main.PCINDENT_UpdatedBy = data.Userid;
                            checkresult_main.PCINDENT_UpdatedDate = indiantime0;

                            _context.Update(checkresult_main);

                            var i1 = _context.SaveChanges();
                            if (i1 > 0)
                            {
                                data.returnval = true;
                            }
                        }
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.getviewdata = (from a in _context.PC_IndentDMO
                                    from b in _context.PC_Indent_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENT_Id == data.PCINDENT_Id && a.MI_Id == data.MI_Id)
                                    select new PC_IndentDTO
                                    {
                                        PCINDENTDET_Id = b.PCINDENTDET_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCINDENT_Id = b.PCINDENT_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                        PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                        PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                        PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,

                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
