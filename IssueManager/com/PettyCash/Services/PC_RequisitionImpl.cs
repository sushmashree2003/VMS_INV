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
    public class PC_RequisitionImpl : Interface.PC_RequisitionInterface
    {
        public PettyCashContext _context;
        public DomainModelMsSqlServerContext _Contextdb;
        public PC_RequisitionImpl(PettyCashContext _con, DomainModelMsSqlServerContext _Contextdbsql)
        {
            _context = _con;
            _Contextdb = _Contextdbsql;
        }
        public PC_RequisitionDTO onloaddata(PC_RequisitionDTO data)
        {
            try
            {

                data.getdept = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).OrderBy(a => a.HRMD_DepartmentName).ToArray();
                data.getparticulars = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id && a.PCMPART_ActiveFlg == true).ToArray();

                var rolet = _context.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleid).ToList();
                var roletype = "";

                //var getloaddata = (from a in _context.PC_RequisitionDMO
                //                   from b in _context.MasterEmployee
                //                   from c in _context.HR_Master_Department
                //                   where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                //                   select new PC_RequisitionDTO
                //                   {
                //                       departmentname = c.HRMD_DepartmentName,
                //                       employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                //                       || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                //                       || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                //                       PCREQTN_ActiveFlg = a.PCREQTN_ActiveFlg,
                //                       PCREQTN_Date = a.PCREQTN_Date,
                //                       PCREQTN_Purpose = a.PCREQTN_Purpose,
                //                       PCREQTN_Id = a.PCREQTN_Id,
                //                       PCREQTN_RequisitionNo = a.PCREQTN_RequisitionNo,
                //                       PCREQTN_CreatedDate = a.PCREQTN_CreatedDate,
                //                       PCREQTN_CreatedBy = a.PCREQTN_CreatedBy,
                //                   }).OrderByDescending(a => a.PCREQTN_CreatedDate).ToArray();

                var getloaddata = (from a in _context.PC_RequisitionDMO
                                   from b in _context.MasterEmployee
                                   from c in _context.HR_Master_Department
                                   from d in _context.PC_Requisition_DetailsDMO
                                   from e in _context.PC_Master_ParticularsDMO
                                   where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && d.PCREQTN_Id == a.PCREQTN_Id && e.PCMPART_Id == d.PCMPART_Id && a.MI_Id == data.MI_Id)
                                   select new PC_RequisitionDTO
                                   {
                                       departmentname = c.HRMD_DepartmentName,
                                       employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                       || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                       || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                       PCREQTN_ActiveFlg = a.PCREQTN_ActiveFlg,
                                       PCREQTN_Date = a.PCREQTN_Date,
                                       PCREQTN_Purpose = a.PCREQTN_Purpose,
                                       PCREQTN_Id = a.PCREQTN_Id,
                                       PCREQTN_RequisitionNo = a.PCREQTN_RequisitionNo,
                                       PCREQTN_CreatedDate = a.PCREQTN_CreatedDate,
                                       PCREQTN_CreatedBy = a.PCREQTN_CreatedBy,
                                       PCMPART_Id = e.PCMPART_Id,
                                   }).OrderByDescending(a => a.PCREQTN_CreatedDate).ToArray();


                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (!roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) && !roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        getloaddata = getloaddata.Where(a => a.PCREQTN_CreatedBy == data.Userid).ToArray();
                    }
                }

                data.getloaddata = getloaddata;

                data.getapprovedindent = (from a in _context.PC_IndentDMO
                                          from b in _context.PC_Indent_DetailsDMO
                                          where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id)
                                          select new PC_RequisitionDTO
                                          {
                                              PCREQTN_Id = b.PCREQTN_Id
                                          }).Distinct().ToArray();


                data.getapprovedindentcashapproved = (from a in _context.PC_RequisitionDMO
                                          from b in _context.PC_Indent_DetailsDMO
                                          from c in _context.PC_IndentDMO
                                          from d in _context.PC_Indent_ApprovedDMO
                                          from e in _context.PC_Indent_Approved_DetailsDMO
                                          where (a.PCREQTN_Id == b.PCREQTN_Id && a.MI_Id == data.MI_Id && b.PCINDENT_Id == c.PCINDENT_Id && c.PCINDENT_Id == d.PCINDENT_Id && e.PCINDENTAP_Id == d.PCINDENTAP_Id)
                                          select new PC_RequisitionDTO
                                          {
                                              PCREQTN_Id = b.PCREQTN_Id
                                          }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_RequisitionDTO onchangedept(PC_RequisitionDTO data)
        {

            try
            {
                data.getemp = (from a in _context.MasterEmployee
                               from b in _context.HR_Master_Department
                               where (a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id
                               && a.HRMD_Id == data.HRMD_Id)
                               select new PC_RequisitionDTO
                               {
                                   HRME_Id = a.HRME_Id,
                                   empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                   (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                                    (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                               }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public PC_RequisitionDTO saverecord(PC_RequisitionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.PCREQTN_Id > 0)
                {
                    var checkid = _context.PC_RequisitionDMO.Where(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id).ToList();

                    if (checkid.Count > 0)
                    {
                        var checkidresult = _context.PC_RequisitionDMO.Single(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id);
                        checkidresult.PCREQTN_Date = data.PCREQTN_Date;
                        checkidresult.PCREQTN_Purpose = data.PCREQTN_Purpose;
                        checkidresult.PCREQTN_UpdatedDate = indiantime0;
                        checkidresult.PCREQTN_UpdatedBy = data.Userid;
                        checkidresult.PCREQTN_TotAmount = data.PCREQTN_TotAmount;

                        _context.Update(checkidresult);

                        List<long> temparr = new List<long>();
                        List<long> temparr1 = new List<long>();

                        //GETTING ALL FILES
                        foreach (PC_Requisition_DetailsDTO ph in data.PC_Requisition_DetailsDTO)
                        {
                            temparr.Add(ph.PCREQTNDET_Id);
                        }

                        //REMOVING FILES
                        Array files_Noresultremove = _context.PC_Requisition_DetailsDMO.Where(t => !temparr.Contains(t.PCREQTNDET_Id)
                        && t.PCREQTN_Id == data.PCREQTN_Id).ToArray();
                        foreach (PC_Requisition_DetailsDMO ph1 in files_Noresultremove)
                        {
                            _context.Remove(ph1);
                        }


                        foreach (PC_Requisition_DetailsDTO ph in data.PC_Requisition_DetailsDTO)
                        {

                            if (ph.PCREQTNDET_Id > 0)
                            {
                                var Files_Noresult = _context.PC_Requisition_DetailsDMO.Single(t => t.PCREQTNDET_Id == ph.PCREQTNDET_Id);
                                Files_Noresult.PCREQTNDET_Remarks = ph.PCREQTNDET_Remarks;
                                Files_Noresult.PCREQTNDET_Amount = ph.PCREQTNDET_Amount;
                                Files_Noresult.PCREQTNDET_UpdatedBy = data.Userid;
                                Files_Noresult.PCREQTNDET_UpdatedDate = indiantime0;
                                _context.Update(Files_Noresult);
                            }
                            else
                            {
                                PC_Requisition_DetailsDMO pC_Requisition_DetailsDMO = new PC_Requisition_DetailsDMO();

                                pC_Requisition_DetailsDMO.PCREQTN_Id = data.PCREQTN_Id;
                                pC_Requisition_DetailsDMO.PCMPART_Id = ph.PCMPART_Id;
                                pC_Requisition_DetailsDMO.PCREQTNDET_Amount = ph.PCREQTNDET_Amount;
                                pC_Requisition_DetailsDMO.PCREQTNDET_ActiveFlg = true;
                                pC_Requisition_DetailsDMO.PCREQTNDET_Remarks = ph.PCREQTNDET_Remarks;
                                pC_Requisition_DetailsDMO.PCREQTNDET_CreatedDate = indiantime0;
                                pC_Requisition_DetailsDMO.PCREQTNDET_UpdatedDate = indiantime0;
                                pC_Requisition_DetailsDMO.PCREQTNDET_CreatedBy = data.Userid;
                                pC_Requisition_DetailsDMO.PCREQTNDET_UpdatedBy = data.Userid;

                                _context.Add(pC_Requisition_DetailsDMO);
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
                    var refernceno = pettyCash_AutoRefernceNo_Generation.GenerateReferenceNo(data.MI_Id, acd_Id, "Requisition");

                    if (refernceno == "" || refernceno == "Error")
                    {
                        data.saveorupdate = "ReferenceNotGenerated";
                        return data;
                    }

                    PC_RequisitionDMO pC_RequisitionDMO = new PC_RequisitionDMO
                    {
                        MI_Id = data.MI_Id,
                        HRME_Id = data.HRME_Id,
                        PCREQTN_Purpose = data.PCREQTN_Purpose,
                        PCREQTN_TotAmount = data.PCREQTN_TotAmount,
                        PCREQTN_Date = data.PCREQTN_Date,
                        PCREQTN_RequisitionNo = refernceno,
                        PCREQTN_ActiveFlg = true,
                        PCREQTN_CreatedDate = indiantime0,
                        PCREQTN_UpdatedDate = indiantime0,
                        PCREQTN_CreatedBy = data.Userid,
                        PCREQTN_UpdatedBy = data.Userid
                    };

                    _context.Add(pC_RequisitionDMO);

                    foreach (var c in data.PC_Requisition_DetailsDTO)
                    {
                        PC_Requisition_DetailsDMO pC_Requisition_DetailsDMO = new PC_Requisition_DetailsDMO
                        {
                            PCREQTN_Id = pC_RequisitionDMO.PCREQTN_Id,
                            PCMPART_Id = c.PCMPART_Id,
                            PCREQTNDET_Amount = c.PCREQTNDET_Amount,
                            PCREQTNDET_ActiveFlg = true,
                            PCREQTNDET_Remarks = c.PCREQTNDET_Remarks,
                            PCREQTNDET_CreatedDate = indiantime0,
                            PCREQTNDET_UpdatedDate = indiantime0,
                            PCREQTNDET_CreatedBy = data.Userid,
                            PCREQTNDET_UpdatedBy = data.Userid
                        };
                        _context.Add(pC_Requisition_DetailsDMO);
                    }

                    var i = _context.SaveChanges();

                    if (i > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                        var PCREQTN_Id = pC_RequisitionDMO.PCREQTN_Id;
                        var str = "";

                        var itemlist = (from a in _context.PC_RequisitionDMO
                                        from b in _context.PC_Requisition_DetailsDMO
                                        from c in _context.PC_Master_ParticularsDMO
                                        where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCREQTN_Id == PCREQTN_Id)
                                        select new PC_RequisitionDTO
                                        {
                                            pcreqtndeT_Id = b.PCREQTNDET_Id,
                                            PCMPART_Id = b.PCMPART_Id,
                                            PCREQTN_Id = b.PCREQTN_Id,
                                            PCMPART_ParticularName = c.PCMPART_ParticularName,
                                            PCREQTNDET_Amount = b.PCREQTNDET_Amount,
                                            PCREQTNDET_Remarks = b.PCREQTNDET_Remarks,
                                            PCREQTNDET_ActiveFlg = b.PCREQTNDET_ActiveFlg,

                                        }).Distinct().ToList();


                        if (itemlist.Count > 0)
                        {
                            str += "<table width=" + "'550px'" + " style=" + "'text-align: center;line-height:2;border:0;cellspacing:0;cellpadding: 0;background-color:#F5F5F5'" + "><tr>";

                            str += "<th width=" + "'5%'" + " style='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'> SL.NO. </th>";
                            str += "<th width=" + "'30%'" + "style ='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Particular Name" + "</th>";
                            str += "<th width=" + "'20%'" + " style ='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Amount" + "</th>";
                            str += "<th width=" + "'45%'" + " style ='background-color:#d2973b;color:aliceblue;border-right:1px solid #fff;text-align:center;'>" + "Remarks" + "</th>";
                            str += "</tr>";
                            int rownumber = 0;

                            for (int j = 0; j < itemlist.Count; j++)
                            {
                                rownumber += 1;

                                str += "<tr>" +
                              "<td style=" + "'vertical-align:top'" + "" + " >" + rownumber + "</td> ";
                                str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCMPART_ParticularName + "</td>";
                                str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCREQTNDET_Amount + "</td>";
                                str += "<td style=" + "'vertical-align:top'" + ">" + itemlist[j].PCREQTNDET_Remarks + "</td>";
                                ;
                                str += "</tr>";
                            }

                            str += "</table>";
                        }

                        try
                        {
                            if (itemlist.Count > 0)
                            {
                                string emp_email = pettyCash_AutoRefernceNo_Generation.GetEmailIdForPettyCash(data.MI_Id, "PC_Requisition_Notification", "Email");
                                if (emp_email != "")
                                {
                                    //Email email = new Email(_Contextdb);
                                    //email.PettyCash_Email_Notification_Send(data.MI_Id, emp_email, "PC_Requisition", PCREQTN_Id,str);
                                    var email = PettyCash_Email_Notification_Send(data.MI_Id, emp_email, "PC_Requisition", PCREQTN_Id, str);

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

        //public PC_RequisitionDTO saverecord(PC_RequisitionDTO data)
        //{
        //    try
        //    {
        //        TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //        DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

        //        if (data.PCREQTN_Id > 0)
        //        {
        //            var checkid = _context.PC_RequisitionDMO.Where(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id).ToList();

        //            if (checkid.Count > 0)
        //            {
        //                var checkidresult = _context.PC_RequisitionDMO.Single(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id);
        //                checkidresult.PCREQTN_Date = data.PCREQTN_Date;
        //                checkidresult.PCREQTN_Purpose = data.PCREQTN_Purpose;
        //                checkidresult.PCREQTN_UpdatedDate = indiantime0;
        //                checkidresult.PCREQTN_UpdatedBy = data.Userid;
        //                checkidresult.PCREQTN_TotAmount = data.PCREQTN_TotAmount;

        //                _context.Update(checkidresult);

        //                List<long> temparr = new List<long>();
        //                List<long> temparr1 = new List<long>();

        //                //GETTING ALL FILES
        //                foreach (PC_Requisition_DetailsDTO ph in data.PC_Requisition_DetailsDTO)
        //                {
        //                    temparr.Add(ph.PCREQTNDET_Id);
        //                }

        //                //REMOVING FILES
        //                Array files_Noresultremove = _context.PC_Requisition_DetailsDMO.Where(t => !temparr.Contains(t.PCREQTNDET_Id)
        //                && t.PCREQTN_Id == data.PCREQTN_Id).ToArray();
        //                foreach (PC_Requisition_DetailsDMO ph1 in files_Noresultremove)
        //                {
        //                    _context.Remove(ph1);
        //                }


        //                foreach (PC_Requisition_DetailsDTO ph in data.PC_Requisition_DetailsDTO)
        //                {

        //                    if (ph.PCREQTNDET_Id > 0)
        //                    {
        //                        var Files_Noresult = _context.PC_Requisition_DetailsDMO.Single(t => t.PCREQTNDET_Id == ph.PCREQTNDET_Id);
        //                        Files_Noresult.PCREQTNDET_Remarks = ph.PCREQTNDET_Remarks;
        //                        Files_Noresult.PCREQTNDET_Amount = ph.PCREQTNDET_Amount;
        //                        Files_Noresult.PCREQTNDET_UpdatedBy = data.Userid;
        //                        Files_Noresult.PCREQTNDET_UpdatedDate = indiantime0;
        //                        _context.Update(Files_Noresult);
        //                    }
        //                    else
        //                    {
        //                        PC_Requisition_DetailsDMO pC_Requisition_DetailsDMO = new PC_Requisition_DetailsDMO();

        //                        pC_Requisition_DetailsDMO.PCREQTN_Id = data.PCREQTN_Id;
        //                        pC_Requisition_DetailsDMO.PCMPART_Id = ph.PCMPART_Id;
        //                        pC_Requisition_DetailsDMO.PCREQTNDET_Amount = ph.PCREQTNDET_Amount;
        //                        pC_Requisition_DetailsDMO.PCREQTNDET_ActiveFlg = true;
        //                        pC_Requisition_DetailsDMO.PCREQTNDET_Remarks = ph.PCREQTNDET_Remarks;
        //                        pC_Requisition_DetailsDMO.PCREQTNDET_CreatedDate = indiantime0;
        //                        pC_Requisition_DetailsDMO.PCREQTNDET_UpdatedDate = indiantime0;
        //                        pC_Requisition_DetailsDMO.PCREQTNDET_CreatedBy = data.Userid;
        //                        pC_Requisition_DetailsDMO.PCREQTNDET_UpdatedBy = data.Userid;

        //                        _context.Add(pC_Requisition_DetailsDMO);
        //                    }
        //                }

        //                var i = _context.SaveChanges();

        //                if (i > 0)
        //                {
        //                    data.message = "Update";
        //                    data.returnval = true;
        //                }
        //                else
        //                {
        //                    data.message = "Update";
        //                    data.returnval = false;
        //                }

        //            }
        //        }
        //        else
        //        {

        //            var acd_Id = _context.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(indiantime0.Date) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(indiantime0.Date)).Select(d => d.IMFY_Id).FirstOrDefault();

        //            PettyCash_AutoRefernceNo_Generation pettyCash_AutoRefernceNo_Generation = new PettyCash_AutoRefernceNo_Generation(_context);
        //            var refernceno = pettyCash_AutoRefernceNo_Generation.GenerateReferenceNo(data.MI_Id, acd_Id, "Requisition");

        //            if (refernceno == "" || refernceno == "Error")
        //            {
        //                data.saveorupdate = "ReferenceNotGenerated";
        //                return data;
        //            }

        //            PC_RequisitionDMO pC_RequisitionDMO = new PC_RequisitionDMO
        //            {
        //                MI_Id = data.MI_Id,
        //                HRME_Id = data.HRME_Id,
        //                PCREQTN_Purpose = data.PCREQTN_Purpose,
        //                PCREQTN_TotAmount = data.PCREQTN_TotAmount,
        //                PCREQTN_Date = data.PCREQTN_Date,
        //                PCREQTN_RequisitionNo = refernceno,
        //                PCREQTN_ActiveFlg = true,
        //                PCREQTN_CreatedDate = indiantime0,
        //                PCREQTN_UpdatedDate = indiantime0,
        //                PCREQTN_CreatedBy = data.Userid,
        //                PCREQTN_UpdatedBy = data.Userid
        //            };

        //            _context.Add(pC_RequisitionDMO);

        //            foreach (var c in data.PC_Requisition_DetailsDTO)
        //            {
        //                PC_Requisition_DetailsDMO pC_Requisition_DetailsDMO = new PC_Requisition_DetailsDMO
        //                {
        //                    PCREQTN_Id = pC_RequisitionDMO.PCREQTN_Id,
        //                    PCMPART_Id = c.PCMPART_Id,
        //                    PCREQTNDET_Amount = c.PCREQTNDET_Amount,
        //                    PCREQTNDET_ActiveFlg = true,
        //                    PCREQTNDET_Remarks = c.PCREQTNDET_Remarks,
        //                    PCREQTNDET_CreatedDate = indiantime0,
        //                    PCREQTNDET_UpdatedDate = indiantime0,
        //                    PCREQTNDET_CreatedBy = data.Userid,
        //                    PCREQTNDET_UpdatedBy = data.Userid
        //                };
        //                _context.Add(pC_Requisition_DetailsDMO);
        //            }

        //            var i = _context.SaveChanges();

        //            if (i > 0)
        //            {
        //                data.message = "Add";
        //                data.returnval = true;
        //                var PCREQTN_Id = pC_RequisitionDMO.PCREQTN_Id;

        //                try
        //                {
        //                    string emp_email = pettyCash_AutoRefernceNo_Generation.GetEmailIdForPettyCash(data.MI_Id, "PC_Requisition_Notification", "Email");
        //                    if (emp_email != "")
        //                    {
        //                        Email email = new Email(_Contextdb);
        //                        email.PettyCash_Email_Notification_Send(data.MI_Id, emp_email, "PC_Requisition_Notification", PCREQTN_Id);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.returnval = true;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            else
        //            {
        //                data.message = "Add";
        //                data.returnval = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        data.message = "Error";
        //        data.returnval = false;
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        public PC_RequisitionDTO EditData(PC_RequisitionDTO data)
        {
            try
            {
                var getdetails = _context.PC_RequisitionDMO.Where(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id).ToList();

                data.geteditdetails = getdetails.ToArray();

                data.geteditdept = (from a in _context.HR_Master_Department
                                    from b in _context.MasterEmployee
                                    where (a.HRMD_Id == b.HRMD_Id && b.HRME_Id == getdetails.FirstOrDefault().HRME_Id)
                                    select a).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.geteditsavedemp = (from a in _context.MasterEmployee
                                        from b in _context.HR_Master_Department
                                        where (a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id
                                        && a.HRME_Id == getdetails.FirstOrDefault().HRME_Id)
                                        select new PC_RequisitionDTO
                                        {
                                            HRME_Id = a.HRME_Id,
                                            empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                            (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                                             (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                                        }).Distinct().ToArray();

                var getdeptid = _context.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == getdetails.FirstOrDefault().HRME_Id).ToList();

                data.geteditemp = (from a in _context.MasterEmployee
                                   from b in _context.HR_Master_Department
                                   where (a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id
                                   && a.HRMD_Id == getdeptid.FirstOrDefault().HRMD_Id)
                                   select new PC_RequisitionDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                       (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                                        (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                                   }).Distinct().ToArray();


                data.geteditsavedparticulars = (from a in _context.PC_RequisitionDMO
                                                from b in _context.PC_Requisition_DetailsDMO
                                                from c in _context.PC_Master_ParticularsDMO
                                                where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCREQTN_Id == data.PCREQTN_Id)
                                                select new PC_RequisitionDTO
                                                {
                                                    pcreqtndeT_Id = b.PCREQTNDET_Id,
                                                    PCMPART_Id = b.PCMPART_Id,
                                                    PCREQTN_Id = b.PCREQTN_Id,
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
        public PC_RequisitionDTO deactiveY(PC_RequisitionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getresult = _context.PC_RequisitionDMO.Where(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id).ToList();

                if (getresult.Count > 0)
                {
                    var checkresult = _context.PC_RequisitionDMO.Single(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id);
                    checkresult.PCREQTN_ActiveFlg = checkresult.PCREQTN_ActiveFlg == true ? false : true;
                    checkresult.PCREQTN_UpdatedDate = indiantime0;
                    checkresult.PCREQTN_UpdatedBy = data.Userid;

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
        public PC_RequisitionDTO Viewdata(PC_RequisitionDTO data)
        {
            try
            {
                data.getviewdata = (from a in _context.PC_RequisitionDMO
                                    from b in _context.PC_Requisition_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCREQTN_Id == data.PCREQTN_Id)
                                    select new PC_RequisitionDTO
                                    {
                                        pcreqtndeT_Id = b.PCREQTNDET_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCREQTN_Id = b.PCREQTN_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCREQTNDET_Amount = b.PCREQTNDET_Amount,
                                        PCREQTNDET_Remarks = b.PCREQTNDET_Remarks,
                                        PCREQTNDET_ActiveFlg = b.PCREQTNDET_ActiveFlg,

                                    }).Distinct().ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PC_Requisition_ApprovalLevel";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PCREQTN_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.PCREQTN_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@PCMPART_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.PCMPART_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }
                        data.modalview = retObject.ToArray();

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
        public PC_RequisitionDTO deactiveparticulars(PC_RequisitionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getresult = _context.PC_Requisition_DetailsDMO.Where(a => a.PCREQTNDET_Id == data.pcreqtndeT_Id).ToList();

                if (getresult.Count > 0)
                {
                    var checkresult = _context.PC_Requisition_DetailsDMO.Single(a => a.PCREQTNDET_Id == data.pcreqtndeT_Id);
                    checkresult.PCREQTNDET_ActiveFlg = checkresult.PCREQTNDET_ActiveFlg == true ? false : true;
                    checkresult.PCREQTNDET_UpdatedDate = indiantime0;
                    checkresult.PCREQTNDET_UpdatedBy = data.Userid;

                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        decimal? totamount = 0;
                        var updateamount = _context.PC_Requisition_DetailsDMO.Where(a => a.PCREQTN_Id == data.PCREQTN_Id && a.PCREQTNDET_ActiveFlg == true).ToList();

                        if (updateamount.Count > 0)
                        {
                            foreach (var c in updateamount)
                            {
                                totamount += c.PCREQTNDET_Amount;
                            }

                            var checkresult_main = _context.PC_RequisitionDMO.Single(a => a.PCREQTN_Id == data.PCREQTN_Id);
                            checkresult_main.PCREQTN_TotAmount = totamount;
                            checkresult_main.PCREQTN_UpdatedBy = data.Userid;
                            checkresult_main.PCREQTN_UpdatedDate = indiantime0;

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

                data.getviewdata = (from a in _context.PC_RequisitionDMO
                                    from b in _context.PC_Requisition_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCREQTN_Id == data.PCREQTN_Id)
                                    select new PC_RequisitionDTO
                                    {
                                        pcreqtndeT_Id = b.PCREQTNDET_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCREQTN_Id = b.PCREQTN_Id,
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
    }
}