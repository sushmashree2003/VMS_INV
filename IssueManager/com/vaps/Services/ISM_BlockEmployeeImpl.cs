using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Services
{
    public class ISM_BlockEmployeeImpl : Interfaces.ISM_BlockEmployeeInterface
    {
        public IssueManagerContext _IssueContext;
        ILogger<ISM_BlockEmployeeDTO> _logger;

        public ISM_BlockEmployeeImpl(IssueManagerContext _cont, ILogger<ISM_BlockEmployeeDTO> _log)
        {
            _IssueContext = _cont;
            _logger = _log;
        }


        public async Task<ISM_BlockEmployeeDTO> getloaddata(ISM_BlockEmployeeDTO data)
        {
            try
            {
                //*********************************** All Employee List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_All_EmployeeList";
                    cmd.CommandType = CommandType.StoredProcedure;
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
                        data.get_Emplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //*********************************** Blocked Employee List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Blocked_EmployeeList";
                    cmd.CommandType = CommandType.StoredProcedure;
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
                        data.get_blockedEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Failed";
                _logger.LogInformation("Block Employee getloaddata : " + ex.Message);
            }
            return data;
        }

        public ISM_BlockEmployeeDTO savedata(ISM_BlockEmployeeDTO data)
        {
            try
            {
                long empid = 0;
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.userid).Distinct().ToArray();
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.ISMBE_Id > 0)
                {
                    //var result = _IssueContext.ISM_Block_EmployeeDMO.Single(a => a.MI_Id == data.MI_Id && a.ISMBE_Id == data.ISMBE_Id);
                    //result.ISEBE_UnblockDate = data.ISEBE_UnblockDate;
                    //result.ISMBE_BlockFlg = false;
                    //result.ISM_PlannerBlockFlg = false;
                    //result.UpdatedDate = indianTime;
                    //result.ISMBE_UpdatedBy = data.userid;
                    //_IssueContext.Update(result);
                    //var i = _IssueContext.SaveChanges();
                    var result = _IssueContext.ISM_Block_EmployeeDMO.Single(a => a.MI_Id == data.MI_Id && a.ISMBE_Id == data.ISMBE_Id);
                    var result1 = _IssueContext.Database.ExecuteSqlCommand("EMP_DR_Unblock @p0,@p1,@p2,@p3,@p4", data.ISEBE_UnblockDate, indianTime, data.userid, data.MI_Id, data.ISMBE_Id);

                    if (result1 > 0)
                    {
                        data.message = "unblock";
                        data.returnval = true;
                        /*********************************** Push Notification*********************************************/
                        var getemployeedetails = (from a in _IssueContext.MasterEmployee
                                                  from b in _IssueContext.HR_Master_Department
                                                  from c in _IssueContext.HR_Master_Designation
                                                  from e in _IssueContext.HR_Master_Grade
                                                  where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                                                  && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
                                                  select new ISM_DailyReportGenerationDTO
                                                  {
                                                      employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                      + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                      department = b.HRMD_DepartmentName,
                                                      desgination = c.HRMDES_DesignationName,
                                                      gradename = e.HRMG_GradeName,
                                                      HRMD_Id = a.HRMD_Id
                                                  }).Distinct().ToList();
                        var deviceidsnew = "";
                        var devicenew = "";
                        var devicelist = (from a in _IssueContext.MasterEmployee
                                          where (a.HRME_ActiveFlag == true && a.HRME_Id == result.HRME_Id)
                                          select new ISM_BlockEmployeeDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                          }).Distinct().ToList();
                        data.deviceArray = devicelist.ToArray();
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
                            string notificationtype = "Unblock";
                            string notificationtitle = "Unblocked";
                            callnotification(devicenew, result.HRME_Id, getemployeedetails.FirstOrDefault().employeename, data.MI_Id, data.sound, notificationtitle);
                            Insertnotification(data.MI_Id, empid, getemployeedetails.FirstOrDefault().employeename, data.userid, notificationtype, data.message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        data.message = "unblock";
                        data.returnval = true;
                    }
                }
                else
                {
                    if (data.temp_blockEmployee.Length > 0)
                    {
                        foreach (var block in data.temp_blockEmployee)
                        {
                            ISM_Block_EmployeeDMO dmo = new ISM_Block_EmployeeDMO();

                            dmo.MI_Id = data.MI_Id;
                            dmo.HRME_Id = block.HRME_Id;
                            dmo.ISMBE_BlockDate = data.ISMBE_BlockDate;
                            dmo.ISMBE_Reason = data.ISMBE_Reason;
                            dmo.ISMBE_BlockFlg = data.ISMBE_BlockFlg;
                            dmo.ISMBE_ActiveFlg = true;
                            dmo.ISMBE_CreatedBy = data.userid;
                            dmo.ISMBE_UpdatedBy = data.userid;
                            dmo.CreatedDate = indianTime;
                            dmo.UpdatedDate = indianTime;
                            _IssueContext.Add(dmo);
                        }
                        var i = _IssueContext.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Save";
                            data.returnval = true;
                            /*********************************** Push Notification*********************************************/
                            var getemployeedetails = (from a in _IssueContext.MasterEmployee
                                                      from b in _IssueContext.HR_Master_Department
                                                      from c in _IssueContext.HR_Master_Designation
                                                      from e in _IssueContext.HR_Master_Grade
                                                      where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRMG_Id == e.HRMG_Id
                                                      && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
                                                      select new ISM_DailyReportGenerationDTO
                                                      {
                                                          employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                          + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                          + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                          department = b.HRMD_DepartmentName,
                                                          desgination = c.HRMDES_DesignationName,
                                                          gradename = e.HRMG_GradeName,
                                                          HRMD_Id = a.HRMD_Id
                                                      }).Distinct().ToList();
                            var deviceidsnew = "";
                            var devicenew = "";
                            if (data.temp_blockEmployee.Length > 0)
                            {
                                List<long> employeeids = new List<long>();
                                foreach (var n in data.temp_blockEmployee)
                                {
                                    employeeids.Add(n.HRME_Id);
                                }
                                var devicelist = (from a in _IssueContext.MasterEmployee
                                                  where (a.HRME_ActiveFlag == true && employeeids.Contains(a.HRME_Id))
                                                  select new ISM_BlockEmployeeDTO
                                                  {
                                                      HRME_Id = a.HRME_Id,
                                                      HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                                  }).Distinct().ToList();
                                data.deviceArray = devicelist.ToArray();
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
                                string notificationtype = "Block";
                                string notificationtitle = "Blocked";
                                callnotification(devicenew, empid, getemployeedetails.FirstOrDefault().employeename, data.MI_Id, data.sound, notificationtitle);
                                Insertnotification(data.MI_Id, empid, getemployeedetails.FirstOrDefault().employeename, data.userid, notificationtype, data.message);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            data.message = "Save";
                            data.returnval = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                data.message = "Failed";
                _logger.LogInformation("Block Employee savedata : " + ex.Message);
            }
            return data;
        }

        public string callnotification(string devicenew, long empid, string empname, long mi_id, string sound, string notificationtitle)
        {
            try
            {
                ISM_BlockEmployeeDTO data = new ISM_BlockEmployeeDTO();
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "style" + '"' + ":" + '"' + "inbox" + '"' + " , " + '"' + "summaryText" + '"' + ":" + '"' + "You have %n% notifications" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + notificationtitle + empname + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + "Daily Report " + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";


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

        public string Insertnotification(long MI_Id, long blockempid, string empname, long userid, string notificationtype, string message)
        {
            try
            {


                var notification = "Daily Report of " + empname + "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ISM_BlockEmployeeDTO data = new ISM_BlockEmployeeDTO();
                if (message == "unblock")
                {
                    ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                    push.MI_Id = MI_Id;
                    push.HRME_Id = blockempid;
                    push.ISMNO_Notification = notification;
                    push.ISMNO_NoticationDate = indianTime;
                    push.ISMNO_NotificationType = notificationtype;
                    push.ISMNO_ReadFlg = false;
                    push.ISMNO_MakeUnReadFlg = false;
                    push.ISMNO_ActiveFlag = true;
                    push.ISMNO_CreatedBy = userid;
                    push.ISMNO_UpdatedBy = userid;
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
                else
                {
                    if (data.temp_blockEmployee.Length > 0)
                    {
                        List<long> employeeids = new List<long>();
                        foreach (var n in data.temp_blockEmployee)
                        {
                            ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                            push.MI_Id = MI_Id;
                            push.HRME_Id = n.HRME_Id;
                            push.ISMNO_Notification = notification;
                            push.ISMNO_NoticationDate = indianTime;
                            push.ISMNO_NotificationType = notificationtype;
                            push.ISMNO_ReadFlg = false;
                            push.ISMNO_MakeUnReadFlg = false;
                            push.ISMNO_ActiveFlag = true;
                            push.ISMNO_CreatedBy = userid;
                            push.ISMNO_UpdatedBy = userid;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public ISM_BlockEmployeeDTO getblockedEmpDeatils(ISM_BlockEmployeeDTO data)
        {
            try
            {
                //*********************************** Blocked Employee Detail ********************************//

                data.get_blockedEmpDeatils = (from a in _IssueContext.ISM_Block_EmployeeDMO
                                              from b in _IssueContext.MasterEmployee
                                              where (a.HRME_Id == b.HRME_Id && a.ISMBE_Id == data.ISMBE_Id)
                                              select new ISM_BlockEmployeeDTO
                                              {
                                                  ISMBE_Id = a.ISMBE_Id,
                                                  HRME_Id = a.HRME_Id,
                                                  empname = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                                  HRME_EmployeeCode = b.HRME_EmployeeCode,
                                                  ISMBE_BlockDate = a.ISMBE_BlockDate,
                                                  ISEBE_UnblockDate = a.ISEBE_UnblockDate,
                                                  ISMBE_Reason = a.ISMBE_Reason,
                                                  ISMBE_BlockFlg = a.ISMBE_BlockFlg,
                                              }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Block Employee Details : " + ex.Message);
            }
            return data;
        }
        public ISM_BlockEmployeeDTO deactivate(ISM_BlockEmployeeDTO data)
        {
            try
            {
                var result = _IssueContext.ISM_Block_EmployeeDMO.Single(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.HRMD_Id && a.ISMBE_Id == data.ISMBE_Id);
                if (result.ISMBE_ActiveFlg == true)
                {
                    result.ISMBE_ActiveFlg = false;
                }
                else
                {
                    result.ISMBE_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.UtcNow;
                result.ISMBE_UpdatedBy = data.userid;
                _IssueContext.Update(result);

                var i = _IssueContext.SaveChanges();
                if (i > 0)
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
                _logger.LogInformation("Block Employee deactivate : " + ex.Message);
            }
            return data;
        }
    }
}
