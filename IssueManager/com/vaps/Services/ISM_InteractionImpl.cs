using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_InteractionImpl : Interfaces.ISM_InteractionInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_InteractionImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_InteractionDTO> getdetails(ISM_InteractionDTO data)
        {
            try
            {
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    data.employee_Id = empid;
                }

                var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                string role = "";
                if (userrole.Count > 0)
                {
                    role = userrole.FirstOrDefault().IVRMRT_Role;
                    data.roletype = role;
                }
                //*********************************** Employee List ********************************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_EmployeeList";
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
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_toEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** Inbox Message ********************************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_INT_InboxMessage";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMINTR_Id",
            SqlDbType.VarChar)
                    {
                        Value = data.ISMINTR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type_Id",
             SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_inboxMessages = retObject.ToArray();
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
        public ISM_InteractionDTO savedata(ISM_InteractionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _Context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "ISMInteraction").ToList();
                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }
                int level_no = 1;
                List<long> reciverids = new List<long>();
                ISM_InteractionsDMO inter = new ISM_InteractionsDMO();
                inter.MI_Id = data.MI_Id;
                inter.ISMINTR_InteractionId = data.Auto_id;
                inter.ISMINTR_ComposedByHRME_Id = empid;
                inter.ISMINTR_GroupOrIndFlg = data.ISMINTR_GroupOrIndFlg;
                inter.ISMINTR_Subject = data.ISMINTR_Subject;
                inter.ISMINTR_Interaction = data.ISMINTR_Interaction;
                inter.ISMINTR_DateTime = indianTime;
                inter.ISMINTR_ActiveFlag = true;
                inter.ISMINTR_CreatedBy = data.UserId;
                inter.ISMINTR_UpdatedBy = data.UserId;
                inter.CreatedDate = indianTime;
                inter.UpdatedDate = indianTime;
                _IssueContext.Add(inter);

                if (data.ISMINTR_GroupOrIndFlg.Equals("G", StringComparison.OrdinalIgnoreCase))
                {
                    if (data.arraymessage != null)
                    {
                        foreach (var am in data.arraymessage)
                        {
                            reciverids.Add(am.HRME_Id);
                            ISM_Interactions_DetailsDMO intDetails = new ISM_Interactions_DetailsDMO();
                            intDetails.ISMINTR_Id = inter.ISMINTR_Id;
                            intDetails.ISMINTRD_ToId = am.HRME_Id;
                            intDetails.ISMINTRD_ComposedById = empid;
                            intDetails.ISMINTRD_Interaction = data.ISMINTR_Interaction;
                            intDetails.ISMINTRD_DateTime = indianTime;
                            intDetails.ISMINTRD_InteractionOrder = level_no;
                            intDetails.ISMINTRD_ActiveFlag = true;
                            intDetails.ISMINTRD_CreatedBy = data.UserId;
                            intDetails.ISMINTRD_UpdatedBy = data.UserId;
                            intDetails.CreatedDate = DateTime.Now;
                            intDetails.UpdatedDate = DateTime.Now;
                            _IssueContext.Add(intDetails);
                        }
                    }
                }
                else
                {
                    ISM_Interactions_DetailsDMO intDetails = new ISM_Interactions_DetailsDMO();
                    intDetails.ISMINTR_Id = inter.ISMINTR_Id;
                    intDetails.ISMINTRD_ToId = data.ISMINTRD_ToId;
                    intDetails.ISMINTRD_ComposedById = empid;
                    intDetails.ISMINTRD_Interaction = data.ISMINTR_Interaction;
                    intDetails.ISMINTRD_DateTime = indianTime;
                    intDetails.ISMINTRD_InteractionOrder = level_no;
                    intDetails.ISMINTRD_ActiveFlag = true;
                    intDetails.ISMINTRD_CreatedBy = data.UserId;
                    intDetails.ISMINTRD_UpdatedBy = data.UserId;
                    intDetails.CreatedDate = DateTime.Now;
                    intDetails.UpdatedDate = DateTime.Now;
                    _IssueContext.Add(intDetails);
                }
                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                    //============================== PUSH Notification
                    var deviceidsnew = "";
                    var devicenew = "";
                    if (data.ISMINTR_GroupOrIndFlg.Equals("G", StringComparison.OrdinalIgnoreCase))
                    {
                        var devicelist = (from a in _IssueContext.MasterEmployee
                                          where (a.HRME_ActiveFlag == true && reciverids.Contains(a.HRME_Id))
                                          select new ISM_DailyReportGenerationDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                          }).Distinct().ToList();
                        if (devicelist.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";
                        }
                    }
                    else
                    {
                        var devicelist = (from a in _IssueContext.MasterEmployee
                                          where (a.HRME_ActiveFlag == true && a.HRME_Id == data.ISMINTRD_ToId)
                                          select new ISM_DailyReportGenerationDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                          }).Distinct().ToList();
                        if (devicelist.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";
                        }
                    }
                    try
                    {
                        if (data.sound == "")
                        {
                            data.sound = "default";
                        }
                        string notification = "New Message";
                        callnotification(devicenew, empid, data.MI_Id, data.sound, notification);
                        Insertnotification(data.MI_Id, empid, notification);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public string callnotification(string devicenew, long empid, long mi_id, string sound, string notification)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ISM_InteractionDTO data = new ISM_InteractionDTO();
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "style" + '"' + ":" + '"' + "inbox" + '"' + " , " + '"' + "summaryText" + '"' + ":" + '"' + "You have %n% notifications" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + notification + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + indianTime + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

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
        public string Insertnotification(long MI_Id, long empid, string notification)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                ISM_InteractionDTO data = new ISM_InteractionDTO();
                if (data.arraymessage.Length > 0)
                {
                    List<long> employeeids = new List<long>();
                    foreach (var n in data.arraymessage)
                    {
                        ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                        push.MI_Id = MI_Id;
                        push.HRME_Id = n.HRME_Id;
                        push.ISMNO_Notification = notification;
                        push.ISMNO_NoticationDate = indianTime;
                        push.ISMNO_NotificationType = "ISMINT";
                        push.ISMNO_ReadFlg = false;
                        push.ISMNO_MakeUnReadFlg = false;
                        push.ISMNO_ActiveFlag = true;
                        push.ISMNO_CreatedBy = data.UserId;
                        push.ISMNO_UpdatedBy = data.UserId;
                        push.CreatedDate = indianTime;
                        push.UpdatedDate = indianTime;
                        _IssueContext.Add(push);

                        var contactExists = _IssueContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                    push.MI_Id = MI_Id;
                    push.HRME_Id = data.ISMINTRD_ToId;
                    push.ISMNO_Notification = notification;
                    push.ISMNO_NoticationDate = indianTime;
                    push.ISMNO_NotificationType = "ISMINT";
                    push.ISMNO_ReadFlg = false;
                    push.ISMNO_MakeUnReadFlg = false;
                    push.ISMNO_ActiveFlag = true;
                    push.ISMNO_CreatedBy = data.UserId;
                    push.ISMNO_UpdatedBy = data.UserId;
                    push.CreatedDate = indianTime;
                    push.UpdatedDate = indianTime;
                    _IssueContext.Add(push);

                    var contactExists = _IssueContext.SaveChanges();
                    if (contactExists > 0)
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
                return ex.Message;
            }
            return "success";
        }

        //********************************************* Reply & Message Deatils********************************************************************//
        public async Task<ISM_InteractionDTO> get_replydetails(ISM_InteractionDTO data)
        {
            try
            {
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                string role = "";
                if (userrole.Count > 0)
                {
                    role = userrole.FirstOrDefault().IVRMRT_Role;
                    data.roletype = role;
                }
                //********************************* Message Details***************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_INT_InboxMessage";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMINTR_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.ISMINTR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type_Id",
             SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_messageDetail = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** Reply Message ********************************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_INT_ReplyMessage";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMINTR_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.ISMINTR_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_replyMessages = retObject.ToArray();
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
        public ISM_InteractionDTO savereply(ISM_InteractionDTO data)
        {
            try
            {
                int level_no = 0;
                int level_order = 0;
                List<long> replyids = new List<long>();
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var orderno = (from INT in _IssueContext.ISM_InteractionsDMO
                               from INTD in _IssueContext.ISM_Interactions_DetailsDMO
                               where (INT.ISMINTR_Id == INTD.ISMINTR_Id && INT.MI_Id == data.MI_Id && INTD.ISMINTR_Id == data.ISMINTR_Id)
                               select new ISM_InteractionDTO
                               {
                                   ISMINTRD_InteractionOrder = INTD.ISMINTRD_InteractionOrder
                               }).Distinct().ToList();

                level_no = orderno.LastOrDefault().ISMINTRD_InteractionOrder;
                if (level_no <= 0)
                {
                    level_order = 1;
                }
                else
                {
                    level_order = level_no + 1;
                }
                if (data.arrayreply != null)
                {
                    foreach (var ar in data.arrayreply)
                    {
                        replyids.Add(ar.ISMINTRD_ToId);
                    }
                }
                ISM_Interactions_DetailsDMO intDetails = new ISM_Interactions_DetailsDMO();
                intDetails.ISMINTR_Id = data.ISMINTR_Id;
                intDetails.ISMINTRD_ToId = data.ISMINTRD_ToId;
                intDetails.ISMINTRD_ComposedById = empid;
                intDetails.ISMINTRD_Interaction = data.ISMINTRD_Interaction;
                intDetails.ISMINTRD_DateTime = indianTime;
                intDetails.ISMINTRD_InteractionOrder = level_order;
                intDetails.ISMINTRD_ActiveFlag = true;
                intDetails.ISMINTRD_CreatedBy = data.UserId;
                intDetails.ISMINTRD_UpdatedBy = data.UserId;
                intDetails.CreatedDate = DateTime.Now;
                intDetails.UpdatedDate = DateTime.Now;
                _IssueContext.Add(intDetails);
                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                    //============================== PUSH Notification
                    var deviceidsnew = "";
                    var devicenew = "";
                    var devicelist = (from a in _IssueContext.MasterEmployee
                                      where (a.HRME_ActiveFlag == true && replyids.Contains(a.HRME_Id))
                                      select new ISM_DailyReportGenerationDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                      }).Distinct().ToList();
                    if (devicelist.Count > 0)
                    {
                        int k = 0;
                        foreach (var deviceid in devicelist)
                        {
                            if (k == 0)
                            {
                                deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                k = 1;
                            }
                            else
                            {
                                deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                            }
                        }
                        devicenew = "[" + deviceidsnew + "]";
                    }
                    try
                    {
                        if (data.sound == "")
                        {
                            data.sound = "default";
                        }
                        string notification = "Reply Message";
                        callnotification(devicenew, empid, data.MI_Id, data.sound, notification);
                        InsertnotificationReply(data.MI_Id, empid, notification);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public string InsertnotificationReply(long MI_Id, long empid, string notification)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                ISM_InteractionDTO data = new ISM_InteractionDTO();
                if (data.arrayreply.Length > 0)
                {
                    List<long> employeeids = new List<long>();
                    foreach (var r in data.arrayreply)
                    {
                        ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                        push.MI_Id = MI_Id;
                        push.HRME_Id = r.ISMINTRD_ToId;
                        push.ISMNO_Notification = notification;
                        push.ISMNO_NoticationDate = indianTime;
                        push.ISMNO_NotificationType = "ISMINTR";
                        push.ISMNO_ReadFlg = false;
                        push.ISMNO_MakeUnReadFlg = false;
                        push.ISMNO_ActiveFlag = true;
                        push.ISMNO_CreatedBy = data.UserId;
                        push.ISMNO_UpdatedBy = data.UserId;
                        push.CreatedDate = indianTime;
                        push.UpdatedDate = indianTime;
                        _IssueContext.Add(push);

                        var contactExists = _IssueContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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

        //================== feedback details start===============
        public ISM_InteractionDTO getfeedbac(ISM_InteractionDTO dto)
        {
            try
            {
                var hrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();

                var count = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Where(a => a.ISMDRF_RCV_HRME_Id == hrmeid[0].Emp_Code && a.ISMDRF_OpenFlg == false).ToList();
                dto.inbcount = count.Count();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_feedback_emplist_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //SqlDbType.BigInt)
                    //        {
                    //            Value = dto.MI_Id
                    //        });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
           SqlDbType.BigInt)
                    {
                        Value = hrmeid[0].Emp_Code
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
                        dto.get_feedbacklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //************************** feedback count********************************//
            ;
                dto.inbcount = count.Count();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "feedbackcount";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
           SqlDbType.BigInt)
                    {
                        Value = hrmeid[0].Emp_Code
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
                        dto.feedbackcount = retObject.ToArray();
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
            return dto;
        }

        public ISM_InteractionDTO feedback_msg_details(ISM_InteractionDTO dto)
        {
            try
            {
                var hrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_feedback_details_proc";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = hrmeid[0].Emp_Code
                    });

                    cmd.Parameters.Add(new SqlParameter("@SENDER_HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMDRF_Send_HRME_Id
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
                        dto.get_feedbackdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var count = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Where(a => a.ISMDRF_RCV_HRME_Id == hrmeid[0].Emp_Code && a.ISMDRF_Send_HRME_Id == dto.ISMDRF_Send_HRME_Id && a.ISMDRF_OpenFlg == false).ToList();
                dto.inbcount = count.Count();


                dto.from_emp_details = (from f in _IssueContext.MasterEmployee
                                        from b in _IssueContext.HR_Master_Department
                                        where f.HRMD_Id == b.HRMD_Id && f.HRME_Id == dto.ISMDRF_Send_HRME_Id
                                        select new ISM_InteractionDTO
                                        {

                                            sender_empname = f.HRME_EmployeeFirstName + " " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                            HRMD_DepartmentName = b.HRMD_DepartmentName
                                        }).Distinct().ToArray();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_feedback_emplist_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //SqlDbType.BigInt)
                    //        {
                    //            Value = dto.MI_Id
                    //        });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
           SqlDbType.BigInt)
                    {
                        Value = hrmeid[0].Emp_Code
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
                        dto.get_feedbacklist = retObject.ToArray();
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
            return dto;
        }

        public async Task<ISM_InteractionDTO> desktopnotification(ISM_InteractionDTO dto)
        {
            try
            {
                var hrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();

                if (hrmeid.Count > 0)
                {
                    var notifycount = _IssueContext.ISM_NotificationsDMO.Where(a => a.HRME_Id == hrmeid[0].Emp_Code && a.ISMNO_ReadFlg == false && a.ISMNO_ModeFlg == "WEB").ToList();
                    dto.notifycount = notifycount.Count();
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return  dto;
        }
        public ISM_InteractionDTO Sendfeedback_tfr(ISM_InteractionDTO dto)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var hrmEempid1 = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == dto.MI_Id && t.Id == dto.UserId).Distinct().ToList();
                foreach (var item in dto.feedbackdtl)
                {
                    var ss = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Single(a => a.ISMDRF_Id==item.ISMDRF_Id);
                    ss.ISMDRF_OpenFeedback = false;
                    _IssueContext.Update(ss);                        
                    
                }

                foreach (var item in dto.feedbackdtl)
                {
                    var ss = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Where(a => a.ISMDRF_Send_HRME_Id == hrmEempid1[0].Emp_Code
                    && a.ISMTCR_Id == item.ISMTCR_Id && a.ISMDRF_Feedback_DR_Date == item.ISMDRF_Feedback_DR_Date).ToList();
                    if (ss.Count == 0)
                    {
                        ISM_DailyReport_FeedBack_DMO fdm = new ISM_DailyReport_FeedBack_DMO();
                        fdm.ISMTCR_Id = item.ISMTCR_Id;
                        fdm.ISMDRF_Send_HRME_Id = hrmEempid1[0].Emp_Code;
                        fdm.ISMDRF_RCV_HRME_Id = item.ISMDRF_RCV_HRME_Id;
                        fdm.ISMDRF_FeedBack = item.ISMDRF_FeedBack;
                        fdm.ISMDRF_Feedback_DR_Date = item.ISMDRF_Feedback_DR_Date;
                        fdm.ISMDRF_Flg = false;
                        fdm.ISMDRF_OpenFlg = false;
                        fdm.ISMDRF_OpenFeedback = true;
                        fdm.CreatedDate = indianTime;
                        fdm.UpdatedDate = DateTime.Now;
                        _IssueContext.Add(fdm);

                        ISM_DailyReport_Feedback_Send_DMO fs = new ISM_DailyReport_Feedback_Send_DMO();
                        fs.MI_Id = dto.MI_Id;
                        fs.ISMTCR_Id = item.ISMTCR_Id;
                        fs.ISMDRFS_Send_HRME_Id = hrmEempid1[0].Emp_Code;
                        fs.ISMDRFS_FeedBack = item.ISMDRF_FeedBack;
                        fs.ISMDRFS_Status = "1";
                        fs.createdate = indianTime;
                        fs.updatetime = DateTime.Now;
                        _IssueContext.Add(fs);

                        ISM_DailyReport_Feedback_Receive_DMO fr = new ISM_DailyReport_Feedback_Receive_DMO();
                        fr.MI_Id = item.MI_Id_r;
                        fr.ISMTCR_Id = item.ISMTCR_Id;
                        fr.ISMDRFR_Receive_HRME_Id = item.ISMDRF_RCV_HRME_Id;
                        fr.ISMDRFR_FeedBack = item.ISMDRF_FeedBack;
                        fr.ISMDRFR_Status = "2";
                        fr.createdate = indianTime;
                        fr.updatetime = DateTime.Now;
                        _IssueContext.Add(fr);



                        dto.f_return = "Success";
                    }
                    else
                    {
                        ISM_DailyReport_Feedback_Send_DMO fs = new ISM_DailyReport_Feedback_Send_DMO();
                        fs.MI_Id = dto.MI_Id;
                        fs.ISMDRF_Id = ss[0].ISMDRF_Id;
                        fs.ISMTCR_Id = item.ISMTCR_Id;
                        fs.ISMDRFS_Send_HRME_Id = hrmEempid1[0].Emp_Code;
                        fs.ISMDRFS_FeedBack = item.ISMDRF_FeedBack;
                        fs.ISMDRFS_Status = "1";
                        fs.createdate = DateTime.Now;
                        fs.updatetime = DateTime.Now;
                        _IssueContext.Add(fs);

                        ISM_DailyReport_Feedback_Receive_DMO fr = new ISM_DailyReport_Feedback_Receive_DMO();
                        fr.MI_Id = item.MI_Id_r;
                        fr.ISMDRF_Id = ss[0].ISMDRF_Id;
                        fr.ISMTCR_Id = item.ISMTCR_Id;
                        fr.ISMDRFR_Receive_HRME_Id = item.ISMDRF_RCV_HRME_Id;
                        fr.ISMDRFR_FeedBack = item.ISMDRF_FeedBack;
                        fr.ISMDRFR_Status = "2";
                        fr.createdate = DateTime.Now;
                        fr.updatetime = DateTime.Now;
                        _IssueContext.Add(fr);

                        var ss1 = _IssueContext.ISM_DailyReport_FeedBack_DMO_con.Single(a => a.ISMDRF_RCV_HRME_Id == item.ISMDRF_RCV_HRME_Id && a.ISMTCR_Id == item.ISMTCR_Id && a.ISMDRF_Feedback_DR_Date == item.ISMDRF_Feedback_DR_Date);
                        if(ss1.ISMTCR_Id!=0)
                        {
                            ss1.ISMDRF_OpenFlg = false;
                            ss1.ISMDRF_OpenFeedback = false;
                            ss1.ISMDRF_FeedBack = item.ISMDRF_FeedBack;
                            ss1.CreatedDate = DateTime.Now;
                            ss1.UpdatedDate = DateTime.Now;
                            _IssueContext.Update(ss1);
                        }
                       


                        dto.f_return = "Success";
                    }
                }
                _IssueContext.SaveChanges();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_InteractionDTO getViewfeedback(ISM_InteractionDTO dto)
        {
            try
            {
                var hrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyFeedback_Transaction_proc";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = hrmeid[0].Emp_Code
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMTCR_Id
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
                        dto.get_feedback = retObject.ToArray();
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
            return dto;
        }
        //============== feedback detailks end====================


        //================== feedback details start===============
        public ISM_InteractionDTO getnotification(ISM_InteractionDTO dto)
        {
            try
            {
                var hrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();

                if (hrmeid.Count>0)
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_WEB_NOTIFICATION";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.BigInt)
                        {
                            Value = hrmeid[0].Emp_Code
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
                            dto.get_feedbacklist = retObject.ToArray();
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
            return dto;
        }
        public ISM_InteractionDTO clicknotification(ISM_InteractionDTO dto)
        {
            try
            {
                var updatelist = _IssueContext.ISM_NotificationsDMO.Single(e => e.ISMNO_Id == dto.ISMNO_Id);

                updatelist.ISMNO_ReadFlg = true;

                _IssueContext.Update(updatelist);
                int cxt = _IssueContext.SaveChanges();



                var hrmeid = _IssueContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();

                if (hrmeid.Count>0)
                {
                    var notifycount = _IssueContext.ISM_NotificationsDMO.Where(a => a.HRME_Id == hrmeid[0].Emp_Code && a.ISMNO_ReadFlg == false && a.ISMNO_ModeFlg == "WEB").ToList();
                    dto.notifycount = notifycount.Count();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_WEB_NOTIFICATION";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.BigInt)
                        {
                            Value = hrmeid[0].Emp_Code
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
                            dto.get_feedbacklist = retObject.ToArray();
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
            return dto;
        }
    }
}
