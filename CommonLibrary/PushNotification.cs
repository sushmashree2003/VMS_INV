using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Dynamic;
using DomainModel.Model.com.vapstech.IssueManager;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace CommonLibrary
{
   public class PushNotification
    {
        
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;

        public PushNotification(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
       //(long? MobileNo, string HRME_AppDownloadedDeviceId, long HRME_Id)
        public string Insert_PushNotification(long MI_Id, string Header_Name, string Message, long? MobileNo, string HRME_AppDownloadedDeviceId, long HRME_Id, long AMST_Id)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
               var time = indianTime.ToString("hh:mm tt");


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = MI_Id;
                sdd.PNSD_HeaderName = Header_Name;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = "Staff";
                sdd.PNSD_SMSMessage = Message;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _db.Add(sdd);

                PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();
                ss.PNSD_Id = sdd.PNSD_Id;
                ss.PNSDDE_MobileNo = MobileNo;
                ss.PNSDDE_DeviceId = HRME_AppDownloadedDeviceId;
                ss.PNSDDE_ReadStatus = "1";
                ss.PNSDDE_DeliveryDate = indianTime;
                ss.PNSDDE_DeliveryTime = time;
                ss.PNSDDE_MakeUnreadFlg = false;
                ss.PNSDDE_ApprovalLevel = "0";
                _db.Add(ss);

                PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                sds.PNSD_Id = sdd.PNSD_Id;
                sds.HRME_Id = HRME_Id;
                _db.Add(sds);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return "";
        }
        public string Insert_PushNotificationLeave(string id, long mi_id, string devicenew, LeaveCreditDTO dto, string header_flg, string headerName)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");
                
                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = headerName;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = "Staff";
                if(headerName == "Leave Apply")
                {
                    var leavedetails = _db.HR_Emp_Leave_ApplicationDMO.Where(h => h.MI_Id == mi_id && h.HRELAP_ActiveFlag == true && h.HRELAP_Id == Convert.ToInt64(id)).FirstOrDefault();
                    DateTime fromdate = DateTime.Parse(leavedetails.HRELAP_FromDate.ToString());
                    DateTime todate = DateTime.Parse(leavedetails.HRELAP_ToDate.ToString());
                    sdd.PNSD_SMSMessage = "Leave applied successfully from " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy");
                }
                else if(headerName == "Leave Approved")
                {
                    var leavedetails = _db.HR_Emp_Leave_ApplicationDMO.Where(h => h.MI_Id == mi_id && h.HRELAP_ActiveFlag == true && h.HRELAP_ApplicationID == id).FirstOrDefault();
                    DateTime fromdate = DateTime.Parse(leavedetails.HRELAP_FromDate.ToString());
                    DateTime todate = DateTime.Parse(leavedetails.HRELAP_ToDate.ToString());
                    sdd.PNSD_SMSMessage = "Leave from " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy") + " approved for one level.";
                }
                else if (headerName == "Leave Rejected")
                {
                    var leavedetails = _db.HR_Emp_Leave_ApplicationDMO.Where(h => h.MI_Id == mi_id && h.HRELAP_ActiveFlag == true && h.HRELAP_ApplicationID == id).FirstOrDefault();
                    DateTime fromdate = DateTime.Parse(leavedetails.HRELAP_FromDate.ToString());
                    DateTime todate = DateTime.Parse(leavedetails.HRELAP_ToDate.ToString());
                    sdd.PNSD_SMSMessage = "Leave from " + fromdate.ToString("dd/MM/yyyy") + " to " + todate.ToString("dd/MM/yyyy") + " is Rejected";
                }

                sdd.PNSD_Header_Flg = header_flg;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _db.Add(sdd);

                foreach (var item in dto.devicelist12)
                {
                    PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();
                    ss.PNSD_Id = sdd.PNSD_Id;
                    ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                    ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                    ss.PNSDDE_ReadStatus = "1";
                    ss.PNSDDE_DeliveryDate = indianTime;
                    ss.PNSDDE_DeliveryTime = time;
                    ss.PNSDDE_MakeUnreadFlg = false;
                    ss.PNSDDE_ApprovalLevel = "0";
                    _db.Add(ss);
                }
                foreach (var item1 in dto.devicelist12)
                {
                    PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.HRME_Id = item1.HRME_Id;
                    _db.Add(sds);
                }
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }
        public string Insert_PushNotificationIndent(long id, long mi_id, string devicenew, ISM_Driver_IndentDTO dto, string header_flg, string headerName)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = headerName;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = "Staff";
                if (headerName == "Indent Apply")
                {
                    sdd.PNSD_SMSMessage = "Indent Applied";
                }
                else if (headerName == "Indent Approved")
                {
                    sdd.PNSD_SMSMessage = "Indent Approved";
                }

                sdd.PNSD_Header_Flg = header_flg;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                _db.Add(sdd);

                foreach (var item in dto.devicelist12)
                {
                    PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();
                    ss.PNSD_Id = sdd.PNSD_Id;
                    ss.PNSDDE_MobileNo = item.HRME_MobileNo;
                    ss.PNSDDE_DeviceId = item.AMST_AppDownloadedDeviceId;
                    ss.PNSDDE_ReadStatus = "1";
                    ss.PNSDDE_DeliveryDate = indianTime;
                    ss.PNSDDE_DeliveryTime = time;
                    ss.PNSDDE_MakeUnreadFlg = false;
                    ss.PNSDDE_ApprovalLevel = "0";
                    _db.Add(ss);
                }
                foreach (var item1 in dto.devicelist12)
                {
                    PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.HRME_Id = item1.HRME_Id;
                    _db.Add(sds);
                }
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }
        public string savewebnotification(long MI_Id,string templates, long primaryid, long taskid,long emp_id ,long userid,string type)
        {
            try
            {
                    Dictionary<string, string> val = new Dictionary<string, string>();

                    var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == templates && e.ISES_PNActiveFlg == true).ToList();

                    if (template.Count == 0)
                    {
                        return "no template";
                    }

                    var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                    var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                    var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                    var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                    string Mailmsg = template.FirstOrDefault().ISES_PNMessage;
                    string Url = template.FirstOrDefault().ISES_LandingURLHTML;

                    string result = Mailmsg;

                    List<Match> variables = new List<Match>();

                    foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                    {
                        variables.Add(match);
                    }

                    if (variables.Count == 0)
                    {
                        Mailmsg = result;
                    }
                    else
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GET_PUSH_NOTIFICATION_PARAMETER";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@Primayid",
                                SqlDbType.BigInt)
                            {
                                Value = primaryid
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                               SqlDbType.BigInt)
                            {
                                Value = MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@taskid",
                               SqlDbType.BigInt)
                            {
                                Value = taskid
                            });
                            cmd.Parameters.Add(new SqlParameter("@Template",
                               SqlDbType.VarChar)
                            {
                                Value = templates
                            });
                          cmd.Parameters.Add(new SqlParameter("@emp_id",
                               SqlDbType.BigInt)
                            {
                                Value = emp_id
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
                    }



                Insertnotification(MI_Id,emp_id,taskid, Mailmsg, type, userid, Url);



            }
            catch (Exception ex)
            {

                throw ex;
            }

            return "";
        }
        public async Task savewebnotificationasy(long MI_Id, string templates, long primaryid, long taskid, long emp_id, long userid, string type)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == templates && e.ISES_PNActiveFlg == true).ToList();

                //if (template.Count == 0)
                //{
                //    return "no template";
                //}

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_PNMessage;
                string Url = template.FirstOrDefault().ISES_LandingURLHTML;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_PUSH_NOTIFICATION_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Primayid",
                            SqlDbType.BigInt)
                        {
                            Value = primaryid
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@taskid",
                           SqlDbType.BigInt)
                        {
                            Value = taskid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Template",
                           SqlDbType.VarChar)
                        {
                            Value = templates
                        });
                        cmd.Parameters.Add(new SqlParameter("@emp_id",
                             SqlDbType.BigInt)
                        {
                            Value = emp_id
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
                            //return ex.Message;
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
                }



                Insertnotification(MI_Id, emp_id, taskid, Mailmsg, type, userid, Url);



            }
            catch (Exception ex)
            {

                throw ex;
            }

            //return "";
        }
        public string Insertnotification(long MI_Id, long empid, long task_id, string titletext, string tasktype, long userid,string url)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                push.MI_Id = MI_Id;
                push.HRME_Id = empid;
                push.ISMNO_Notification = titletext;
                push.ISMNO_NoticationDate = indianTime;
                push.ISMNO_NotificationType = tasktype;
                push.ISMNO_ReadFlg = false;
                push.ISMNO_MakeUnReadFlg = false;
                push.ISMNO_ActiveFlag = true;
                push.ISMNO_CreatedBy = userid;
                push.ISMNO_UpdatedBy = userid;
                push.CreatedDate = indianTime;
                push.UpdatedDate = indianTime;
                push.ISMNO_ModeFlg = "WEB";
                push.ISMNO_RedirectURL = url;
                _db.Add(push);
                var contactExists = _db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string callpushnotification(string body,string title,string click_action,long userid)
        {

            List<ISM_NotificationDTO> dtonewtemp = new List<ISM_NotificationDTO>();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ISM_GET_PUSHNOTIFICATION_TOCKEN";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userid",
                SqlDbType.BigInt)
                {
                    Value = userid
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
                            dtonewtemp.Add(new ISM_NotificationDTO
                            {
                                pushtocken = Convert.ToString(dataReader["Tocken"]),
                                servicekey = Convert.ToString(dataReader["Servicekey"])
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
                for (int i = 0; i < dtonewtemp.Count; i++)
                {

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", dtonewtemp[0].servicekey));
                //Sender Id - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", "XXXXX.."));
                tRequest.ContentType = "application/json";

                var payload = new
                {
                    to = dtonewtemp[i].pushtocken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = body.ToString(),
                        title = title.ToString(),
                        click_action = click_action.ToString(),
                        icon = "https://dcampusstrg.blob.core.windows.net/files/0/VMSLogo-1.png",
                        badge = 1
                    },
                    data = new
                    {
                        key1 = click_action.ToString(),
                        key2 = "https://dcampusstrg.blob.core.windows.net/files/0/VMSLogo-1.png",
                    }

                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    //result.Response = sResponseFromServer;
                                }
                        }
                    }
                }
            }
                
            }
            return "success";
        }
        public async Task callpushnotificationasy(string body, string title, string click_action, long userid)
        {

            List<ISM_NotificationDTO> dtonewtemp = new List<ISM_NotificationDTO>();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ISM_GET_PUSHNOTIFICATION_TOCKEN";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userid",
                SqlDbType.BigInt)
                {
                    Value = userid
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
                            dtonewtemp.Add(new ISM_NotificationDTO
                            {
                                pushtocken = Convert.ToString(dataReader["Tocken"]),
                                servicekey = Convert.ToString(dataReader["Servicekey"])
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
                for (int i = 0; i < dtonewtemp.Count; i++)
                {

                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    //serverKey - Key from Firebase cloud messaging server  
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", dtonewtemp[0].servicekey));
                    //Sender Id - From firebase project setting  
                    tRequest.Headers.Add(string.Format("Sender: id={0}", "XXXXX.."));
                    tRequest.ContentType = "application/json";

                    var payload = new
                    {
                        to = dtonewtemp[i].pushtocken,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {
                            body = body.ToString(),
                            title = title.ToString(),
                            click_action = click_action.ToString(),
                            icon = "https://dcampusstrg.blob.core.windows.net/files/0/VMSLogo-1.png",
                            badge = 1
                        },
                        data = new
                        {
                            key1 = click_action.ToString(),
                            key2 = "https://dcampusstrg.blob.core.windows.net/files/0/VMSLogo-1.png",
                        }

                    };

                    string postbody = JsonConvert.SerializeObject(payload).ToString();
                    Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                    tRequest.ContentLength = byteArray.Length;
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                    {
                                        String sResponseFromServer = tReader.ReadToEnd();
                                        //result.Response = sResponseFromServer;
                                    }
                            }
                        }
                    }
                }

            }
           
        }

    }
}
