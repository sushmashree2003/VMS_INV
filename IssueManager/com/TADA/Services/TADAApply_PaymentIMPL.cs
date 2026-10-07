using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using Microsoft.EntityFrameworkCore;
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
    public class TADAApply_PaymentIMPL : Interface.TADAApply_PaymentInterFace
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;

        public TADAApply_PaymentIMPL(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }

        public TADAApply_PaymentDTO getalldata(TADAApply_PaymentDTO data)
        {
            try
            {
                var Adavance_Detail = _context.TADAApply_PaymentDMO.ToList().Select(R => R.VTADAA_Id);

                data.Adavance = (from p in _context.TADAApply_PaymentDMO
                                 from Q in _context.TADA_ApplyDMO
                                 from t in _context.MasterEmployee
                                 from u in _context.IVRM_Master_CityDMO
                                 where (Q.HRME_Id == t.HRME_Id && p.VTADAA_Id == Q.VTADAA_Id && Q.IVRMMCT_Id == u.IVRMMCT_Id && Q.MI_Id == data.MI_Id
                                 )
                                 select new TADAApply_PaymentDTO
                                 {
                                     HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                     HRME_EmployeeCode = t.HRME_EmployeeCode,
                                     VTADAAP_AmountPaid = p.VTADAAP_AmountPaid,
                                     VTADAAP_Date = p.VTADAAP_Date,
                                     VTADAAP_PaymentMode = p.VTADAAP_PaymentMode,
                                     VTADAAP_Id = p.VTADAAP_Id,
                                     IVRMMCT_Name = u.IVRMMCT_Name,
                                     VTADAA_Id = p.VTADAA_Id,
                                     VTADAAP_ActiveFlg = p.VTADAAP_ActiveFlg,
                                     VTADAAP_Remarks = p.VTADAAP_Remarks,
                                     VTADAAP_CreatedDate = p.VTADAAP_CreatedDate

                                 }).Distinct().OrderByDescending(R => R.VTADAAP_CreatedDate).ToArray();
                data.GetReport = (from a in _context.TADA_ApplyDMO
                                  from c in _context.IVRM_Master_CityDMO
                                  from t in _context.MasterEmployee
                                  where (a.VTADAA_ActiveFlg == true && a.MI_Id == data.MI_Id && a.IVRMMCT_Id == c.IVRMMCT_Id
                                  && a.VTADAA_StatusFlg == "Approved" && a.HRME_Id == t.HRME_Id
                                  && t.HRME_ActiveFlag == true && !Adavance_Detail.Contains(a.VTADAA_Id))
                                  select new TADA_ApplyDTO
                                  {
                                      VTADAA_FromDate = a.VTADAA_FromDate,
                                      VTADAA_ToDate = a.VTADAA_ToDate,
                                      VTADAA_TotalAppliedAmount = a.VTADAA_TotalAppliedAmount,
                                      HRME_EmployeeCode = t.HRME_EmployeeCode,
                                      VTADAA_Id = a.VTADAA_Id,
                                      VTADAAA_Id = a.VTADAAA_Id,
                                      IVRMMCT_Name = c.IVRMMCT_Name,
                                      VTADAA_StatusFlg = a.VTADAA_StatusFlg,
                                      VTADAA_TotalSactionedAmount = a.VTADAA_TotalSactionedAmount,
                                      VTADAA_BalanceHRMS = a.VTADAA_BalanceHRMS,
                                      HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                      VTADAA_CreatedDate = a.VTADAA_CreatedDate

                                  }).Distinct().OrderByDescending(p => p.VTADAA_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //edittt
        public TADAApply_PaymentDTO edittt(TADAApply_PaymentDTO data)
        {
            try
            {
                var result = _context.TADAApply_PaymentDMO.Where(p => p.VTADAAP_Id == data.VTADAAP_Id).FirstOrDefault();
                if (result.VTADAAP_ActiveFlg == true)
                {
                    data.EditArray = (from a in _context.TADA_ApplyDMO
                                      from c in _context.IVRM_Master_CityDMO
                                      from t in _context.MasterEmployee
                                      where (a.VTADAA_ActiveFlg == true && a.IVRMMCT_Id == c.IVRMMCT_Id
                                      && a.VTADAA_StatusFlg == "Approved" && a.HRME_Id == t.HRME_Id
                                      && t.HRME_ActiveFlag == true && a.VTADAA_Id == data.VTADAA_Id)
                                      select new TADA_ApplyDTO
                                      {
                                          VTADAA_FromDate = a.VTADAA_FromDate,
                                          VTADAA_ToDate = a.VTADAA_ToDate,

                                          VTADAA_TotalAppliedAmount = a.VTADAA_TotalAppliedAmount,

                                          IVRMMCT_Name = c.IVRMMCT_Name,
                                          VTADAA_StatusFlg = a.VTADAA_StatusFlg,
                                          VTADAA_TotalSactionedAmount = a.VTADAA_TotalSactionedAmount,

                                          HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                          VTADAA_CreatedDate = a.VTADAA_CreatedDate

                                      }).Distinct().OrderByDescending(p => p.VTADAA_CreatedDate).ToArray();

                    data.Edit_Two = (from p in _context.TADAApply_PaymentDMO
                                     from Q in _context.TADA_ApplyDMO
                                     where (p.VTADAA_Id == Q.VTADAA_Id && p.VTADAAP_Id == data.VTADAAP_Id
                                     )
                                     select new TADAApply_PaymentDTO
                                     {

                                         VTADAAP_AmountPaid = p.VTADAAP_AmountPaid,
                                         VTADAAP_Date = p.VTADAAP_Date,
                                         VTADAAP_PaymentMode = p.VTADAAP_PaymentMode,
                                         VTADAAP_Id = p.VTADAAP_Id,
                                         VTADAAP_PaymentRefNo = p.VTADAAP_PaymentRefNo,
                                         VTADAA_Id = p.VTADAA_Id,
                                         VTADAAP_Remarks = p.VTADAAP_Remarks

                                     }).Distinct().ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADAApply_PaymentDTO Viewdetails(TADAApply_PaymentDTO data)
        {
            try
            {
                if (data.VTADAA_Id > 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADAMltiple_SDetailsAmt_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@VTADAA_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.VTADAA_Id
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

                            data.Veiwdetails = retObject.ToArray();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                if (data.VTADAAA_Id > 0)
                {
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

                            data.Veiwdetailstt = retObject.ToArray();


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
            return data;
        }
        //data.VTADAA_Id = 1;
        public TADAApply_PaymentDTO savedta(TADAApply_PaymentDTO data)
        {
            try
            {
                data.returnvalue = false;
                var result = _context.TADAApply_PaymentDMO.Where(p => p.VTADAA_Id == data.VTADAA_Id && p.VTADAAP_ActiveFlg == true).ToList();
                var resultone = _context.TADA_ApplyDMO.Where(t => t.VTADAA_Id == data.VTADAA_Id && t.VTADAA_ActiveFlg == true).FirstOrDefault();

                if (data.VTADAA_Id > 0 && data.VTADAAP_Id == 0)
                {
                    if (result.Count == 0)
                    {
                        TADAApply_PaymentDMO obj = new TADAApply_PaymentDMO();
                        obj.VTADAA_Id = data.VTADAA_Id;
                        obj.VTADAAP_PaymentMode = data.VTADAAP_PaymentMode;
                        obj.VTADAAP_AmountPaid = data.VTADAAP_AmountPaid;
                        obj.VTADAAP_PaymentRefNo = data.VTADAAP_PaymentRefNo;
                        obj.VTADAAP_Remarks = data.VTADAAP_Remarks;
                        obj.VTADAAP_Date = data.VTADAAP_Date;
                        obj.VTADAAP_ActiveFlg = true;
                        obj.VTADAAP_CreatedDate = DateTime.Now;
                        obj.VTADAAP_CreatedBy = data.UserId;
                        obj.VTADAAP_UpdatedBy = data.UserId;
                        obj.VTADAAP_UpdatedDate = DateTime.Now;


                        _context.Add(obj);

                        if (resultone.VTADAA_Id > 0)
                        {

                            resultone.VTADAA_TotalPaidAmount = data.VTADAAP_AmountPaid;
                            if (data.VTADAA_BalanceHRMS != null && data.VTADAA_BalanceHRMS != "")
                            {
                                resultone.VTADAA_BalanceHRMS = data.VTADAA_BalanceHRMS;
                            }
                            _context.Update(resultone);
                        }

                        int a = _context.SaveChanges();
                        if (a > 0)
                        {
                            data.returnvalue = true;
                            data.returnval = "Insert";
                            string mailcc = "";
                            data.HRME_Id = _context.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                            string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            var chkk = (from P in _context.TADAApply_PaymentDMO
                                        from R in _context.TADA_ApplyDMO
                                        where (R.VTADAA_Id == P.VTADAA_Id && R.VTADAA_ActiveFlg == true && P.VTADAA_Id == obj.VTADAA_Id)
                                        select R
                                      ).ToList();
                            if (chkk.Count > 0)
                            {
                                data.UserId = chkk.FirstOrDefault().HRME_Id;
                                mailcc = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == data.UserId && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                            }
                            sendmail(data.MI_Id, emailid, "TADA_Payment", obj.VTADAAP_Id, mailcc);

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
                else
                {
                    if (data.VTADAAP_Id > 0 && data.VTADAA_Id > 0)
                    {
                        var resultupdate = _context.TADAApply_PaymentDMO.Where(p => p.VTADAAP_Id == data.VTADAAP_Id).FirstOrDefault();
                        if (resultupdate.VTADAAP_ActiveFlg == true)
                        {

                            resultupdate.VTADAAP_PaymentMode = data.VTADAAP_PaymentMode;
                            resultupdate.VTADAAP_AmountPaid = data.VTADAAP_AmountPaid;
                            resultupdate.VTADAAP_PaymentRefNo = data.VTADAAP_PaymentRefNo;
                            resultupdate.VTADAAP_Remarks = data.VTADAAP_Remarks;
                            resultupdate.VTADAAP_Date = data.VTADAAP_Date;
                            resultupdate.VTADAAP_UpdatedBy = data.UserId;
                            resultupdate.VTADAAP_UpdatedDate = DateTime.Now;
                            _context.Update(resultupdate);
                        }
                        if (resultone.VTADAA_Id > 0)
                        {

                            resultone.VTADAA_TotalPaidAmount = data.VTADAAP_AmountPaid;
                            _context.Update(resultone);
                        }
                        int a = _context.SaveChanges();
                        if (a > 0)
                        {
                            data.returnvalue = true;
                            data.returnval = "Update";

                        }
                        else
                        {
                            data.returnvalue = true;
                            data.returnval = "UpdateFailed";
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
        //AutoSchedule
        public TADAApply_PaymentDTO AutoSchedule(TADAApply_PaymentDTO data)
        {
            try
            {
                List<TADAApply_PaymentDTO> Tasklist = new List<TADAApply_PaymentDTO>();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_EmployeeTADA_AdvancePayment_Automail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
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
                                Tasklist.Add(new TADAApply_PaymentDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                    VTADAAA_Id = Convert.ToInt64(dataReader["vtadaaA_Id"]),
                                    VTADAAP_Id = Convert.ToInt64(dataReader["VTADAAAP_Id"]),

                                });
                            }
                        }
                        //  data.alldata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                if (Tasklist != null && Tasklist.Count > 0)
                {

                    foreach (var d in Tasklist)
                    {
                        string mailcc = "";

                        string emailid = _context.Multiple_Email_DMO.Where(p => p.HRME_Id == d.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();

                        var chkk = (from P in _context.TADA_AdavanceApply_PaymentDMO
                                    from R in _context.TADA_AdavanceApplyDMO
                                    where (R.VTADAAA_Id == P.VTADAAA_Id && R.VTADAAA_ActiveFlg == true && P.VTADAAA_Id == d.VTADAAA_Id)
                                    select R
                                  ).ToList();

                        if (chkk.Count > 0)
                        {
                            var countemail = _context.ApplicationUser.Where(R => R.Id == chkk.FirstOrDefault().VTADAAA_UpdatedBy).ToList();
                            if (countemail.Count > 0 && countemail.FirstOrDefault().Email != "")
                            {
                                mailcc = countemail.FirstOrDefault().Email;

                            }


                        }
                        if (emailid != "")
                        {

                            sendmail(data.MI_Id, emailid, "TADA_AdvancePayment", d.VTADAAP_Id, mailcc);
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
        //deactive
        public TADAApply_PaymentDTO deactive(TADAApply_PaymentDTO data)
        {
            try
            {
                data.returnvalue = false;
                var resultone = _context.TADAApply_PaymentDMO.Where(t => t.VTADAAP_Id == data.VTADAAP_Id).FirstOrDefault();
                if (resultone.VTADAAP_ActiveFlg == true)
                {
                    resultone.VTADAAP_ActiveFlg = false;
                    resultone.VTADAAP_UpdatedDate = DateTime.Now;
                    resultone.VTADAAP_UpdatedBy = data.UserId;
                    _context.Update(resultone);

                }
                else
                {
                    if (resultone.VTADAAP_ActiveFlg == false)
                    {
                        resultone.VTADAAP_ActiveFlg = true;
                        resultone.VTADAAP_UpdatedDate = DateTime.Now;
                        resultone.VTADAAP_UpdatedBy = data.UserId;
                        _context.Update(resultone);

                    }
                }

                int returnval = _context.SaveChanges();

                if (returnval > 0)
                {
                    data.returnvalue = true;
                    data.returnval = "Delete";
                }
                else
                {
                    data.returnval = "NotDelete";
                    data.returnvalue = true;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        //email
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
    }
}
