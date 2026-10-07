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
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;

using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using Microsoft.EntityFrameworkCore;

using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IssueManager.com.Tenants.Implimentation
{
    public class TenantUploadNoticeImpl : Interface.TenantUploadNoticeInterface
    {

        public DomainModelMsSqlServerContext _db;

        public IssueManagerContext _context;

        public TenantUploadNoticeImpl(IssueManagerContext _cont, DomainModelMsSqlServerContext db)
        {
            _context = _cont;
            _db = db;
        }
        public TenantUploadNoticeDTO Getdetails(TenantUploadNoticeDTO data)
        {
            try
            {

                data.building = _context.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id).ToArray();

               // data.tenants = _context.TenantAgreement.Where(t => t.MI_Id == data.MI_Id && t.TENANTAGR_LeftFlg != true).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tenant_NoticeBoard_List";
                    cmd.CommandType = CommandType.StoredProcedure;
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.notice_details = retObject.ToArray();
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

        public TenantUploadNoticeDTO gettenant(TenantUploadNoticeDTO data)
        {
            try
            {
                data.tenants = _context.TenantAgreement.Where(t => t.MI_Id == data.MI_Id && t.HRMB_Id == data.HRMB_Id && t.TENANTAGR_LeftFlg != true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TenantUploadNoticeDTO savedetail(TenantUploadNoticeDTO data)
        {
            try
            {


                int returnval;

                List<long> cls_ids = new List<long>();
                if (data.TENENTNTB_Id > 0)
                {

                    var resultobj = _context.TenantNoticeBoardDMO.Single(t => t.TENENTNTB_Id.Equals(data.TENENTNTB_Id) && t.MI_Id.Equals(data.MI_Id));

                    resultobj.TENENTNTB_Title = data.TENENTNTB_Title;
                    resultobj.TENENTNTB_Description = data.TENENTNTB_Description;
                    resultobj.TENENTNTB_StartDate = data.TENENTNTB_StartDate;
                    resultobj.TENENTNTB_EndDate = data.TENENTNTB_EndDate;
                    resultobj.TENENTNTB_DisplayDate = data.TENENTNTB_DisplayDate;
                    resultobj.TENENTNTB_AllOrIndFlg = data.TENENTNTB_AllOrIndFlg;
                    resultobj.TENENTNTB_ActiveFlag = true;
                    resultobj.TENENTNTB_UpdatedDate = DateTime.Now;
                    resultobj.HRMB_Id = data.dd;

                    _context.Update(resultobj);


                    if (data.FilePath_Array.Length > 0)
                    {
                        var resultstf = _context.TenantNoticeBoardFilesDMO.Where(a => a.TENENTNTB_Id == data.TENENTNTB_Id).ToList();
                        foreach (var item in resultstf)
                        {
                            var dltstf = _context.TenantNoticeBoardFilesDMO.Single(a => a.TENENTNTB_Id == data.TENENTNTB_Id && a.TENENTNTBFL_Id == item.TENENTNTBFL_Id);
                            _context.Remove(dltstf);
                        }
                        foreach (var fl in data.FilePath_Array)
                        {

                            TenantNoticeBoardFilesDMO em = new TenantNoticeBoardFilesDMO();
                            em.TENENTNTB_Id = resultobj.TENENTNTB_Id;

                            em.TENENTNTBFL_FileName = fl.TENENTNTBFL_FileName;
                            em.TENENTNTBFL_FilePath = fl.TENENTNTBFL_FilePath;
                            em.TENENTNTBFL_ActiveFlag = true;
                            em.TENENTNTBFL_CreatedDate = DateTime.Today;
                            em.TENENTNTBFL_UpdatedDate = DateTime.Now;
                            em.TENENTNTBFL_CreatedBy = data.UserId;
                            em.TENENTNTBFL_UpdatedBy = data.UserId;
                            _context.Add(em);

                        }
                    }





                    var contactExists = _context.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        data.message = "Add";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Update";
                    }


                }

                else
                {

                    TenantNoticeBoardDMO obj = new TenantNoticeBoardDMO();

                    obj.MI_Id = data.MI_Id;
                    obj.TENENTNTB_Title = data.TENENTNTB_Title;
                    obj.HRMB_Id = data.dd;

                    obj.TENENTNTB_Description = data.TENENTNTB_Description;
                    obj.TENENTNTB_StartDate = data.TENENTNTB_StartDate;
                    obj.TENENTNTB_EndDate = data.TENENTNTB_EndDate;
                    obj.TENENTNTB_DisplayDate = data.TENENTNTB_DisplayDate;
                    obj.TENENTNTB_AllOrIndFlg = data.TENENTNTB_AllOrIndFlg;
                    obj.TENENTNTB_ActiveFlag = true;
                    obj.TENENTNTB_UpdatedDate = DateTime.Now;


                    obj.TENENTNTB_CreatedDate = DateTime.Now;


                    _context.Add(obj);
                    //returnval = _context.SaveChanges();
                    // data.TENENTNTB_Id = obj.TENENTNTB_Id;
                    if (data.FilePath_Array.Length > 0)
                    {
                        foreach (var fl in data.FilePath_Array)
                        {

                            TenantNoticeBoardFilesDMO em = new TenantNoticeBoardFilesDMO();
                            em.TENENTNTB_Id = obj.TENENTNTB_Id;

                            em.TENENTNTBFL_FileName = fl.TENENTNTBFL_FileName;
                            em.TENENTNTBFL_FilePath = fl.TENENTNTBFL_FilePath;
                            em.TENENTNTBFL_ActiveFlag = true;
                            em.TENENTNTBFL_CreatedDate = DateTime.Today;
                            em.TENENTNTBFL_UpdatedDate = DateTime.Now;
                            em.TENENTNTBFL_CreatedBy = data.UserId;
                            em.TENENTNTBFL_UpdatedBy = data.UserId;
                            _context.Add(em);

                        }
                    }



                    foreach (var stu in data.studentarray)
                    {

                        TenantNoticeBoardTenantDMO st = new TenantNoticeBoardTenantDMO();
                        st.TENENTNTB_Id = obj.TENENTNTB_Id;
                        st.TENANTAGR_Id = stu.TENANTAGR_Id;
                        st.TENANTNTBTNT_ActiveFlag = true;
                        st.TENANTNTBTNT_CreatedDate = DateTime.Now;
                        st.TENANTNTBTNT_UpdatedDate = DateTime.Now;
                        st.TENANTNTBTNT_CreatedBy = data.UserId;
                        st.TENANTNTBTNT_UpdatedBy = data.UserId;
                        _context.Add(st);
                    }




                    returnval = _context.SaveChanges();


                    var contactExists = _context.SaveChanges();

                    if (returnval > 0)
                    {
                        data.returnval = true;
                        data.message = "Add";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Failed";
                    }





                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public TenantUploadNoticeDTO sendmail(TenantUploadNoticeDTO data)
        {
            try
            {
                foreach (var i in data.TENANTNOTICE_Ids)
                {

                    var tenantnotice = (from a in _context.TenantNoticeBoardDMO
                                        from b in _context.TenantNoticeBoardTenantDMO
                                        where (a.TENENTNTB_Id == b.TENENTNTB_Id && a.MI_Id == data.MI_Id && b.TENENTNTB_Id == i.TENENTNTB_Id && a.TENENTNTB_ActiveFlag == true)
                                        select b).Distinct().ToArray();

                    foreach (var j in tenantnotice)
                    {

                        var tenantdetails = (from b in _context.TenantNoticeBoardTenantDMO
                                             from g in _context.TenantNoticeBoardDMO
                                             from d in _context.TenantNoticeBoardFilesDMO
                                             from a in _context.TenantAgreement
                                             from c in _context.HR_Master_Building
                                             from e in _context.HR_Master_Room
                                             where (a.MI_Id == data.MI_Id && b.TENENTNTB_Id == j.TENENTNTB_Id && d.TENENTNTB_Id == g.TENENTNTB_Id && b.TENANTAGR_Id == a.TENANTAGR_Id && a.TENANTAGR_Id == j.TENANTAGR_Id && a.TENANTAGR_ActiveFlg == true && b.TENENTNTB_Id == g.TENENTNTB_Id &&
                                             c.HRMB_Id == a.HRMB_Id && g.HRMB_Id == c.HRMB_Id && e.HRMR_Id == a.HRMR_Id)
                                             select new TenantAgreementDTO
                                             {
                                                 TENANTAGR_Id = a.TENANTAGR_Id,
                                                 TENANTAGR_TenantName = a.TENANTAGR_TenantName,
                                                 TENENTNTB_Id = b.TENENTNTB_Id,
                                                 TENANTAGR_Remarks = g.TENENTNTB_Title,
                                                 TENANTAGRREN_Remarks = g.TENENTNTB_Description,
                                                 returnval = b.TENANTNTBTNT_ActiveFlag,
                                                 TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                                 HRMB_BuildingName = c.HRMB_BuildingName,
                                                 HRMR_RoomName = e.HRMR_RoomName,
                                                 TENANTAGR_AgrFromDate = a.TENANTAGR_AgrFromDate,
                                                 TENANTAGR_AgrToDate = a.TENANTAGR_AgrToDate,
                                                 TENANTAGR_Address = a.TENANTAGR_Address,
                                                 TENENTNTBFL_FilePath = d.TENENTNTBFL_FilePath


                                             }).ToArray();



                        string emailccdetails = "";
                        string emailbccdetails = "";


                        var checktemplate = _context.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "TENANTNOTICEBOARD"
                        && a.ISES_MailActiveFlag == true).ToList();

                        DateTime now = DateTime.Now;
                        string strDate = now.ToString("dd-MM-yyyy");

                        if (checktemplate.Count > 0)
                        {
                            emailccdetails = checktemplate.FirstOrDefault().ISES_MailCCId;
                            emailbccdetails = checktemplate.FirstOrDefault().ISES_MailBCCId;
                        }
                        else
                        {
                            emailccdetails = "";
                            emailbccdetails = "";
                        }

                        if (checktemplate.Count == 0)
                        {
                            data.message = "Email Template not Mapped to the selected Institution";
                        }
                        else
                        {

                            string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                            if (templatedetails.Contains("[TENANTNAME]"))
                            {
                                templatedetails = templatedetails.Replace("[TENANTNAME]", tenantdetails[0].TENANTAGR_TenantName.ToUpper());
                            }
                            if (templatedetails.Contains(" [TITLE]"))
                            {
                                templatedetails = templatedetails.Replace(" [TITLE]", tenantdetails[0].TENANTAGR_Remarks.ToUpper());
                            }
                            if (templatedetails.Contains("[DESCRIPTION]"))
                            {
                                templatedetails = templatedetails.Replace("[DESCRIPTION]", tenantdetails[0].TENANTAGRREN_Remarks);
                            }
                            if (templatedetails.Contains("[FLAT]"))
                            {
                                templatedetails = templatedetails.Replace("[FLAT]", tenantdetails[0].HRMR_RoomName.ToString());
                            }
                            if (templatedetails.Contains("[BUILDING]"))
                            {
                                templatedetails = templatedetails.Replace("[BUILDING]", tenantdetails[0].HRMB_BuildingName.ToString());
                            }
                            if (templatedetails.Contains("[ADDRESS]"))
                            {
                                templatedetails = templatedetails.Replace("[ADDRESS]", tenantdetails[0].TENANTAGR_Address.ToString());
                            }
                            if (templatedetails.Contains("[STARTDATE]"))
                            {
                                templatedetails = templatedetails.Replace("[STARTDATE]", tenantdetails[0].TENANTAGR_AgrFromDate.ToString("dd-MM-yyyy"));
                            }
                            if (templatedetails.Contains("[DATE1]"))
                            {

                                templatedetails = templatedetails.Replace("[DATE1]", strDate);
                            }

                            try
                            {
                                data.message = sendmailexamtt(data.MI_Id, tenantdetails[0].TENANTAGR_EmailId.ToString(), templatedetails, emailccdetails, emailbccdetails, tenantdetails[0].TENENTNTBFL_FilePath, "TENANTNOTICEBOARD");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }


                    }
                }


            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                data.message = ex.Message;
            }



            return data;

        }

        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails,string files ,string temp)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == temp
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = url;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    string mailcc = "";
                    string mailbcc = "";

                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }

                    
                          
                   
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(mail_id);
                    //  message.AddTo("kavitajnandoor21@gmail.com");

                    if (files != null && files != "")
                    {
                        var webClient = new WebClient();
                        byte[] imageBytes = webClient.DownloadData(files);

                        string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);

                        message.AddAttachment(files, fileContentsAsBase64, null, null, null);
                    }

                    if (Attechement.Equals("1"))
                    {
                        var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                    //******************* EMAIL CC DETAILS ****************//

                    if (emailccdetails != null && emailccdetails != "")
                    {
                        string[] ccmaildetails = emailccdetails.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }

                    //******************** Attachment ******************* //

                   
                  


                    //******************* EMAIL BCC DETAILS ****************//




                    //if (mailcc != null && mailcc != "")
                    //{
                    //    message.AddCc(mailcc);
                    //}
                    //if (mailbcc != null && mailbcc != "")
                    //{
                    //    message.AddBcc(mailbcc);
                    //}

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    try
                    {
                        client.SendEmailAsync(message).Wait();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "TENANTNOTICEBOARD" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";

        }


        public TenantUploadNoticeDTO getsection(TenantUploadNoticeDTO dto)
        {
            try
            {

                var rolet = _context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.IVRMRT_Id).ToList();

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {

                    dto.HRME_Id = _context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                }
                else
                {
                    dto.HRME_Id = 0;
                }
                //added by roopa//

                var asmcl_ids = "0";
                if (dto.classlsttwo.Length > 0)
                {
                    foreach (var ue in dto.classlsttwo)
                    {
                        asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;

                    }

                }



                //added by roopa
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_StaffwiseSectionStdata_new";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.VarChar)
                    {
                        Value = asmcl_ids
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
                        dto.sectionlist = retObject.ToArray();
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
            return dto;
        }
        public TenantUploadNoticeDTO editdetails(TenantUploadNoticeDTO data)
        {
            try
            {
                var edit = (from a in _context.TenantNoticeBoardDMO
                            where (a.TENENTNTB_Id == data.TENENTNTB_Id && a.MI_Id == data.MI_Id)
                            select new TenantUploadNoticeDTO
                            {
                                TENENTNTB_Id = a.TENENTNTB_Id,
                                TENENTNTB_Title = a.TENENTNTB_Title,
                                TENENTNTB_Description = a.TENENTNTB_Description,
                                TENENTNTB_StartDate = a.TENENTNTB_StartDate,
                                TENENTNTB_EndDate = a.TENENTNTB_EndDate,
                                TENENTNTB_DisplayDate = a.TENENTNTB_DisplayDate,
                                HRMB_Id = a.HRMB_Id,
                                TENENTNTB_AllOrIndFlg = a.TENENTNTB_AllOrIndFlg,

                            }).Distinct().OrderBy(t => t.TENENTNTB_Id).ToArray();
                if (edit.Length > 0)
                {
                    data.editdetails = edit;
                }

                data.attachementlist = (from a in _context.TenantNoticeBoardDMO
                                        from b in _context.TenantNoticeBoardFilesDMO
                                        where a.TENENTNTB_Id == data.TENENTNTB_Id && a.TENENTNTB_Id == b.TENENTNTB_Id
                                        select new TenantUploadNoticeDTO
                                        {
                                            TENENTNTBFL_FileName = b.TENENTNTBFL_FileName,
                                            TENENTNTBFL_FilePath = b.TENENTNTBFL_FilePath,

                                            TENENTNTB_Id = a.TENENTNTB_Id
                                        }).ToArray();

                var check = _context.TenantNoticeBoardDMO.Single(a => a.MI_Id == data.MI_Id && a.TENENTNTB_Id == data.TENENTNTB_Id);
                if (check.TENENTNTB_AllOrIndFlg == "Individual")
                {
                    // int asmsid = 0;

                    var cls = _context.TenantNoticeBoardTenantDMO.Where(a => a.TENENTNTB_Id == data.TENENTNTB_Id).ToList();
                    data.editclasslist = cls.ToArray();

                    List<long> INTBC_Ids = new List<long>();
                    List<long> TENANTAGR_Id = new List<long>();
                    foreach (var item in cls)
                    {

                        TENANTAGR_Id.Add(item.TENANTAGR_Id);
                    }

                    string asmcl_id = "0";
                    foreach (var item in cls)
                    {

                        asmcl_id = asmcl_id + "," + item.TENANTAGR_Id;
                    }









                }
                //================= staff======

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TenantUploadNoticeDTO deactivate(TenantUploadNoticeDTO data)
        {
            try
            {
                var query = _context.TenantNoticeBoardDMO.Single(s => s.MI_Id == data.MI_Id && s.TENENTNTB_Id == data.TENENTNTB_Id);

                if (query.TENENTNTB_ActiveFlag == true)
                {
                    query.TENENTNTB_ActiveFlag = false;
                }
                else
                {
                    query.TENENTNTB_ActiveFlag = true;
                }
                query.TENENTNTB_UpdatedDate = DateTime.Now;

                _context.Update(query);
                var contactExists = _context.SaveChanges();
                if (contactExists > 0)
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



        public TenantUploadNoticeDTO getstudent(TenantUploadNoticeDTO data)
        {
            try
            {
                //added by roopa//
                string asmcl_ids = "0", asmsid = "0", fmg_ids = "0", fmt_ids = "0", flag = "";
                if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                {
                    if (data.classlsttwo.Length > 0)
                    {
                        foreach (var ue in data.classlsttwo)
                        {
                            asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;
                            // asmsid = asmsid + "," + ue.ASMS_Id;
                        }

                    }
                }

                if (data.sectionlistarray != null && data.sectionlistarray.Length > 0)
                {
                    foreach (var item in data.sectionlistarray)
                    {
                        asmsid = asmsid + "," + item.ASMS_Id;
                    }
                }
                if (data.fee_def == true)
                {
                    flag = "F";
                }
                else
                {
                    flag = "S";
                }
                if (data.fee_def == true)
                {

                    if (data.defgrparray != null && data.defgrparray.Length > 0)
                    {
                        foreach (var item in data.defgrparray)
                        {
                            fmg_ids = fmg_ids + "," + item.FMG_Id;
                        }
                    }

                    if (data.defarray != null && data.defarray.Length > 0)
                    {
                        foreach (var item in data.defarray)
                        {
                            fmt_ids = fmt_ids + "," + item.FMT_Id;
                        }
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FEE_TERMS_STUDENTS_list";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                    {
                        // Value = dto.ASMCL_Id
                        Value = asmcl_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                 SqlDbType.VarChar)
                    {
                        Value = fmg_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                 SqlDbType.VarChar)
                    {
                        Value = fmt_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = flag
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentlist = retObject.ToArray();
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

        public TenantUploadNoticeDTO Deptselectiondetails(TenantUploadNoticeDTO dto)
        {
            try
            {
                string departments = "0";
                if (dto.departmentlist.Length > 0)
                {
                    foreach (var ue in dto.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_DepartmentChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
                    {
                        Value = departments
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.designation = retObject.ToArray();
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
            return dto;
        }

        public TenantUploadNoticeDTO Desgselectiondetails(TenantUploadNoticeDTO dto)
        {
            try
            {
                string departments = "0";
                if (dto.designationlist.Length > 0)
                {
                    foreach (var ue in dto.designationlist)
                    {
                        departments = departments + "," + ue.HRMDES_Id;
                    }

                }
                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DesignationChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
                    {
                        Value = departments
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.get_userEmplist = retObject.ToArray();
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
            return dto;
        }

        public TenantUploadNoticeDTO viewData(TenantUploadNoticeDTO dto)
        {
            try
            {
                dto.attachementlist = (from a in _context.TenantNoticeBoardDMO
                                       from b in _context.TenantNoticeBoardFilesDMO
                                       where a.TENENTNTB_Id == dto.TENENTNTB_Id && a.TENENTNTB_Id == b.TENENTNTB_Id
                                       select new TenantUploadNoticeDTO
                                       {
                                           TENENTNTBFL_FileName = b.TENENTNTBFL_FileName,
                                           TENENTNTBFL_FilePath = b.TENENTNTBFL_FilePath,
                                           // INTB_Attachment = a.INTB_Attachment,
                                           TENENTNTB_Id = a.TENENTNTB_Id
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


        public TenantUploadNoticeDTO viewrecords(TenantUploadNoticeDTO dto)
        {

            if (dto.INTB_ToStudentFlg == true)
            {
                dto.flag = "Student";
            }
            if (dto.INTB_ToStaffFlg == true)
            {
                dto.flag = "Staff";
            }

            dto.notice_Name = dto.TENENTNTB_Title;

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "NoticeView_list";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id",
              SqlDbType.BigInt)
                {
                    Value = dto.MI_Id
                });


                cmd.Parameters.Add(new SqlParameter("@TENENTNTB_Id",
             SqlDbType.BigInt)
                {
                    Value = dto.TENENTNTB_Id
                });


                cmd.Parameters.Add(new SqlParameter("@flag",
               SqlDbType.VarChar)
                {
                    Value = dto.flag
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
                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    dto.viewlist = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return dto;
        }

    }
}
