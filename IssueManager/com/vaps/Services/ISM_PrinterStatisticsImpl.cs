using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace IssueManager.com.vaps.Services
{
    public class ISM_PrinterStatisticsImpl:Interfaces.ISM_PrinterStatisticsInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_PrinterStatisticsImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
       // public ISM_PrinterStatisticsDTO getdetails(ISM_PrinterStatisticsDTO data)
        public async Task<ISM_PrinterStatisticsDTO> getdetails(ISM_PrinterStatisticsDTO data)
        {
            try
            {
                //var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                //long empid = 0;
                //if (hrmEempid.Length > 0)
                //{
                //    empid = hrmEempid.FirstOrDefault().Emp_Code;
                //    data.HRME_Id = empid;
                //}
                var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                string role = "";
                if (userrole.Count > 0)
                {
                    role = userrole.FirstOrDefault().IVRMRT_Role;
                    data.roletype = role;
                }

              
               
                    try
                    {

                        data.get_department = (from a in _IssueContext.HR_Master_Department
                                               where (a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                               select new ISM_PrinterStatisticsDTO
                                               {
                                                   HRMD_Id = a.HRMD_Id,
                                                   HRMD_DepartmentName = a.HRMD_DepartmentName
                                               }).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                    data.get_printers = (from a in _IssueContext.PrinterDMO

                                         where (a.VMSMPR_ActiveFlag == true)
                                                             select new ISM_PrinterStatisticsDTO
                                                             {
                                                                 VMSMPR_Id = a.VMSMPR_Id,
                                                                 VMSMPR_Name = a.VMSMPR_Name,
                                                             }).ToArray();

                    var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
                    long empid = 0;
                    if (hrmEempid.Count > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                    }
                    var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                    long? dept_id = 0;
                    if (hrmeDepartment_id.Count > 0)
                    {
                        dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    }
                   // data.dept_Id =Convert.ToInt32(dept_id);
                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                             from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id &&  c.VMSMPR_Id == a.VMSMPR_Id)
                                             select new ISM_PrinterStatisticsDTO
                                             {
                                                 HRMD_Id = a.HRMD_Id,
                                                 HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                 VMSPOD_PODate = a.VMSPOD_PODate,
                                                 VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                 VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                 VMSPOD_Type=a.VMSPOD_Type,
                                                 VMSPOD_Id = a.VMSPOD_Id,
                                                 VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                 VMSMPR_Name=c.VMSMPR_Name




                                             }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();



                }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                

  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_PrinterStatisticsDTO savedata(ISM_PrinterStatisticsDTO data)
        {
            try
            {
                string FromDate = data.VMSPOD_FromDate.Value.ToString("yyyy-MM-dd");
                string ToDate = data.VMSPOD_ToDate.Value.ToString("yyyy-MM-dd");
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
              
                var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                string role = "";
                if (userrole.Count > 0)
                {
                    role = userrole.FirstOrDefault().IVRMRT_Role;
                    data.roletype = role;

                }

                    if (data.VMSPOD_Id > 0)
                {

         

                        var result = _IssueContext.ISM_PrinterStatisticsDMO.Single(t => t.VMSPOD_Id == data.VMSPOD_Id);
                        var res0 = _IssueContext.ISM_PrinterStatisticsDMO.Where(a => a.VMSPOD_Id == data.VMSPOD_Id).ToList();
                        if (res0.Count > 0)
                        {
                            var res01 = _IssueContext.ISM_PrinterStatisticsDMO.Single(a => a.VMSPOD_Id == data.VMSPOD_Id);
                            res01.VMSMPR_Id = data.VMSMPR_Id;
                            res01.HRMD_Id = data.HRMD_Id;
                        res01.VMSPOD_PODate = data.VMSPOD_PODate;
                            res01.VMSPOD_ActiveFlag = true;
                            res01.VMSPOD_CreatedBy = data.UserId;
                            res01.VMSPOD_UpdatedBy = data.UserId;
                            res01.VMSPOD_Remarks = data.VMSPOD_Remarks;
                            res01.VMSPOD_UpdatedDate = indianTime;
                            res01.VMSPOD_NoOfPages = data.VMSPOD_NoOfPages;
                            res01.VMSPOD_Type = data.VMSPOD_Type;
                            res01.VMSPOD_PODate = DateTime.Now;
                        res01.VMSPOD_FromDate = data.VMSPOD_FromDate;
                        res01.VMSPOD_ToDate = data.VMSPOD_ToDate;
                        _IssueContext.Update(res01);
                        //sendmails("EMAILOTP", data);
                        //PrinterEmail(data);
                    }
                        else
                        {
                            ISM_PrinterStatisticsDMO uptaskclt = new ISM_PrinterStatisticsDMO();
                            uptaskclt.VMSPOD_Id = result.VMSPOD_Id;
                            uptaskclt.VMSPOD_PODate = DateTime.Now;
                            uptaskclt.VMSPOD_ActiveFlag = true;
                            uptaskclt.VMSPOD_CreatedBy = data.UserId;
                            uptaskclt.VMSPOD_UpdatedBy = data.UserId;
                            uptaskclt.VMSPOD_CreatedDate = indianTime;
                            uptaskclt.VMSPOD_UpdatedDate = indianTime;
                            uptaskclt.VMSPOD_Remarks = data.VMSPOD_Remarks;
                            uptaskclt.VMSPOD_NoOfPages = data.VMSPOD_NoOfPages;
                            uptaskclt.VMSPOD_Type = data.VMSPOD_Type;
                            uptaskclt.HRMD_Id = data.HRMD_Id;
                            uptaskclt.VMSPOD_FromDate = data.VMSPOD_FromDate;
                            uptaskclt.VMSPOD_ToDate = data.VMSPOD_ToDate;
                        uptaskclt.VMSMPR_Id = data.VMSMPR_Id;
                       

                        _IssueContext.Add(uptaskclt);
                        //sendmails("EMAILOTP", data);
                    }

                

                }

                //if (data.VMSPOD_Id != 0)

                else{

                    //else
                    //{
                    //var result = _IssueContext.ISM_PrinterStatisticsDMO.Single(t => t.VMSPOD_Id == data.VMSPOD_Id);
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "printerdatechecking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt) { Value = data.HRMD_Id });
                        cmd.Parameters.Add(new SqlParameter("@VMSMPR_Id", SqlDbType.BigInt) { Value = data.VMSMPR_Id });
                        cmd.Parameters.Add(new SqlParameter("@VMSPOD_Type", SqlDbType.VarChar) { Value = data.VMSPOD_Type });
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
                        ISM_PrinterStatisticsDMO result = new ISM_PrinterStatisticsDMO();

                    result.MI_Id = data.MI_Id;
                        result.HRMD_Id = data.HRMD_Id;
                        //result.VMSPOD_Id = data.VMSPOD_Id;
                        result.VMSPOD_PODate = DateTime.Now;
                        result.VMSPOD_Remarks = data.VMSPOD_Remarks;
                        result.VMSPOD_NoOfPages = data.VMSPOD_NoOfPages;
                        result.VMSPOD_Type = data.VMSPOD_Type;

                    result.VMSPOD_CreatedBy = data.UserId;
                        result.VMSPOD_CreatedDate = indianTime;
                    result.VMSPOD_UpdatedBy = data.UserId;
                    result.VMSPOD_UpdatedDate = indianTime;
                    result.VMSPOD_ActiveFlag = true;
                        result.VMSPOD_FromDate = data.VMSPOD_FromDate;
                        result.VMSPOD_ToDate = data.VMSPOD_ToDate;
                        result.VMSMPR_Id = data.VMSMPR_Id;

                        _IssueContext.Add(result);
                        //sendmails("EMAILOTP", data);
                        


                    }
                    else
                    {
                        data.returnval = "duplicate";
                    }


                    //}
                }

                    if (data.returnval!= "duplicate")
                {
                    var contactExists = _IssueContext.SaveChanges();
                    sendmails("Printdata", data);


                    if (contactExists > 0)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
                }
                }
                data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                         from b in _IssueContext.HR_Master_Department
                                         from c in _IssueContext.PrinterDMO
                                         where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id && a.VMSPOD_CreatedBy == data.UserId && c.VMSMPR_Id==a.VMSMPR_Id)
                                         select new ISM_PrinterStatisticsDTO
                                         {
                                             HRMD_Id = a.HRMD_Id,
                                             HRMD_DepartmentName = b.HRMD_DepartmentName,
                                             VMSPOD_PODate = a.VMSPOD_PODate,
                                             VMSPOD_Remarks = a.VMSPOD_Remarks,
                                             VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                             VMSPOD_Type = a.VMSPOD_Type,
                                             VMSPOD_Id = a.VMSPOD_Id,
                                             VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                             VMSMPR_Name=c.VMSMPR_Name,




                                         }).Distinct().OrderByDescending(a=>a.VMSPOD_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_PrinterStatisticsDTO deactive(ISM_PrinterStatisticsDTO data)
        {
            try
            {
                var result = _IssueContext.ISM_PrinterStatisticsDMO.Single(t => t.VMSPOD_Id == data.VMSPOD_Id);

                if (result.VMSPOD_ActiveFlag == true)
                {
                    result.VMSPOD_ActiveFlag = false;
                }
                else if (result.VMSPOD_ActiveFlag == false)
                {
                    result.VMSPOD_ActiveFlag = true;
                }
                result.VMSPOD_UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);

                int returnval = _IssueContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_PrinterStatisticsDTO gettaskAttachment(ISM_PrinterStatisticsDTO data)
        {
            try
            {

                data.get_printdetails=(from a in _IssueContext.ISM_PrinterStatisticsDMO
                                       from b in _IssueContext.HR_Master_Department
           
           where (a.HRMD_Id==b.HRMD_Id && a.MI_Id == data.MI_Id &&  a.VMSPOD_CreatedBy == data.UserId && a.VMSPOD_Id == data.VMSPOD_Id)
                 select new ISM_PrinterStatisticsDTO
                 {
                     HRMD_Id = a.HRMD_Id,
                     HRMD_DepartmentName=b.HRMD_DepartmentName,
                
                     VMSPOD_PODate = a.VMSPOD_PODate,   
                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                     VMSPOD_Type = a.VMSPOD_Type,
                     VMSPOD_Id = a.VMSPOD_Id,
                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                     VMSMPR_Id=a.VMSMPR_Id,
                     VMSPOD_FromDate=a.VMSPOD_FromDate,
                     VMSPOD_ToDate=a.VMSPOD_ToDate



                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public ISM_PrinterStatisticsDTO deptchange(ISM_PrinterStatisticsDTO data)
        {
            try
            {
                data.get_printers =(from a in _IssueContext.PrinterDMO
                                    from b in _IssueContext.PrinterDeptMapping
                                    where (a.VMSMPR_Id==b.VMSMPR_Id && b.HRMD_Id==data.HRMD_Id && a.VMSMPR_ActiveFlag == true)
                                    select new ISM_PrinterStatisticsDTO
                                    {
                                        VMSMPR_Id=a.VMSMPR_Id,
                                        VMSMPR_Name=a.VMSMPR_Name,
                                    }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ISM_PrinterStatisticsDTO PrinterEmail(ISM_PrinterStatisticsDTO data)
        {
            try
            {

                decimal total = 0;
                string str = "";
                List<ISM_PrinterStatisticsDTO> Tasklist = new List<ISM_PrinterStatisticsDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Printerdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@printDate", SqlDbType.DateTime) { Value =DateTime.Now });
                 
                 
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                
                                    Tasklist.Add(new ISM_PrinterStatisticsDTO
                                    {
                                        VMSPOD_NoOfPages = Convert.ToInt32(dataReader["VMSPOD_NoOfPages"]),
                                        VMSPOD_Type = Convert.ToString(dataReader["VMSPOD_Type"]),
                                        VMSPOD_Remarks = Convert.ToString(dataReader["VMSPOD_Remarks"]),
                                        VMSPOD_FromDate = Convert.ToDateTime(dataReader["VMSPOD_FromDate"]),
                                        VMSPOD_ToDate = Convert.ToDateTime(dataReader["VMSPOD_ToDate"]),
                                        HRMD_DepartmentName = Convert.ToString(dataReader["HRMD_DepartmentName"]),
                                    });
                                
                                //retObject.Add((ExpandoObject)dataRow);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                string hrmdid = "0";
           
                if (Tasklist.Count > 0)
                {
                    str += "<table width='100%' border=" + "'1'" + "    id=" + "'tablepaging'" + " class=" + "'table table-hover table-responsive table-bordered'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                   "<th style=" + "width:20px'" + "> SL.NO. </th>";
                    str += "<th>" + "Department Name" + "</th>";
                    str += "<th>" + "Printer Name" + "</th>";
                    str += "<th>" + "Period" + "</th>";
                    str += "<th>" + "No Of Prints" + "</th>";
                    str += "</tr>";
                    int rownumber = 0;
                    for (int j = 0; j < Tasklist.Count; j++)
                    {
                        rownumber += 1;
                      
                        str += "<tr>" +
                      "<td" + " >" + rownumber + "</td> ";
                        str += "<td >" + Tasklist[j].HRMD_DepartmentName + "</td>";
                        str += "<td >" + Tasklist[j].VMSMPR_Name + "</td>";
                        str += "<td >" + Tasklist[j].VMSPOD_FromDate +" - " + Tasklist[j].VMSPOD_ToDate + "</td>";
                   
                        str += "<td>" + Tasklist[j].VMSPOD_NoOfPages + "</th>";
               
                        str += "</tr>";
                        hrmdid = hrmdid + "," + Tasklist[j].HRMD_Id;
                    }

                    str += "</table>";


                  

                  
                        var suss = sendmail( "PrinterTemplate", str, hrmdid);
           



                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }

        public string sendmail(string Template, string items, string hrmdid)
        {
          
                try
                {
                var MI_Id = 17;

                string emailccdetails = "";

                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _Context.smsEmailSetting.Where(e =>  e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count == 0)
                    {
                        return "";
                    }
                    var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                    var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                    var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                    string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                    string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                    string Resultsms = Mailcontent;
                    string result = Mailmsg;
                    List<Match> variables = new List<Match>();
                    foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                    {
                        variables.Add(match);
                    }
              

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
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
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;
                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
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
                        smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    
                            string heademail = "";
                            if (hrmdid != "")
                            {
                                List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Fetch_DepartmentHeadmultiple";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                                    SqlDbType.VarChar)
                                    {
                                        Value = hrmdid
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
                                                dtonewtemp.Add(new ISM_PlannerReportsDTO
                                                {
                                                    Tomail = Convert.ToString(dataReader["Head"])
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                if (dtonewtemp.Count() > 0)
                                {
                                    for (int j = 0; j < dtonewtemp.Count; j++)
                                    {
                                        heademail = dtonewtemp[j].Tomail;
                                    }
                                }
                            }

                            if (heademail != "")
                            {
                                emailccdetails = heademail ;
                            }
                          
                            //emailbccdetails = "rakesh.reddy@vapstech.com";
                       

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }
                        if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                        {
                            var message = new SendGridMessage();
                            message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                            message.Subject = Subject;
                            message.AddTo(" ");
                            if (Attechement.Equals("1"))
                            {
                                var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
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
                        
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                                var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                                var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                if (modulename.Count == 0)
                                {
                                    modulename[0] = "Issue Manager";
                                }

                                cmd.CommandText = "IVRM_Email_Outgoing";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@EmailId",
                                    SqlDbType.NVarChar)
                                {
                                    Value = ""
                                });
                                cmd.Parameters.Add(new SqlParameter("@Message",
                                   SqlDbType.NVarChar)
                                {
                                    Value = items
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
                                    emailMessage.To.Add(new MailAddress(""));
                                    emailMessage.From = new MailAddress(SendingEmail, institutionName[0].MI_Name);
                                    emailMessage.Subject = Subject;
                                    emailMessage.Body = Mailmsg;
                                    emailMessage.IsBodyHtml = true;
                                    if (Attechement.Equals("1"))
                                    {
                                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
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


                                  
                                    clientsmtp.Send(emailMessage);

                                 
                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                        if (modulename.Count == 0)
                                        {
                                            modulename[0] = "Issue Manager";
                                        }

                                        cmd.CommandText = "IVRM_Email_Outgoing";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                                            SqlDbType.NVarChar)
                                        {
                                            Value = ""
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@Message",
                                           SqlDbType.NVarChar)
                                        {
                                            Value =items
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

        public string sendmails(string Template, ISM_PrinterStatisticsDTO data)
        {
            try
            {
                var HRMD_Id = data.HRMD_Id;
                string Emails = "";
                string Email = "manohara.nm@vapstech.com";
                var MI_Id = 17;
                string mailcc = "";
                string mailbcc = "";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }

                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id
                && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "VMS_Printer_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.Add(new SqlParameter("@Date",
                       SqlDbType.DateTime)
                    {
                        Value = DateTime.Now
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                       SqlDbType.BigInt)
                    {
                        Value = HRMD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMSPOD_Remarks",
                       SqlDbType.VarChar)
                    {
                        Value = data.VMSPOD_Remarks
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMSPOD_FromDate",
                       SqlDbType.DateTime)
                    {
                        Value = data.VMSPOD_FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMSPOD_ToDate ",
                       SqlDbType.DateTime)
                    {
                        Value = data.VMSPOD_ToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMSPOD_NoOfPages",
                       SqlDbType.BigInt)
                    {
                        Value = data.VMSPOD_NoOfPages
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMSPOD_Type ",
                       SqlDbType.VarChar)
                    {
                        Value = data.VMSPOD_Type
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
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                try
                {
                    string heademail = "";

                    List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Department_Head_Email";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                        SqlDbType.VarChar)
                        {
                            Value = HRMD_Id
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
                                    dtonewtemp.Add(new ISM_PlannerReportsDTO
                                    {
                                        Tomail = Convert.ToString(dataReader["Head"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (dtonewtemp.Count() > 0)
                    {
                        for (int j = 0; j < dtonewtemp.Count; j++)
                        {
                            heademail = dtonewtemp[j].Tomail;
                        }
                    }


                    if (heademail != "")
                    {
                        Emails = heademail;
                    }

                }
                catch (Exception error)
                {
                    //
                }
                Emails = "manohara1999nm@gmail.com";
              
                //Email = $"{Email} {Emails}";
                mailcc = "nagaraja.mv@vapstech.com";
                //mailbcc = "manohara1999nm@gmail.com";
                if (alldetails.Count > 0)
                {
                    // string SendingEmail = "noreply@vapstech.com";
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
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
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
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
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
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



    }
    }
