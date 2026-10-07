using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class HR_Planner_ManagementImpl : Interfaces.HR_Planner_ManagementInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public HR_Planner_ManagementImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
        public HR_Planner_ManagementDTO getdetails(HR_Planner_ManagementDTO data)
        {
            try
            {
                string userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).Select(t => t.IVRMRT_Role).FirstOrDefault();
                data.employeelist = _IssueContext.MasterEmployee.Where(e => e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false).ToArray();
                data.employeelistdata = (from a in _IssueContext.MasterEmployee
                                         from b in _IssueContext.Staff_User_Login
                                         where (a.MI_Id == data.MI_Id && a.HRME_Id == b.Emp_Code && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                         select new HR_Planner_ManagementDTO
                                         {
                                             UserId = b.Id,
                                             HRME_Id = a.HRME_Id,
                                             HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),

                                         }).ToArray();
                data.HRME_Id = _IssueContext.Staff_User_Login.Where(t => t.Id == data.User_Id).Select(t => t.Emp_Code).FirstOrDefault();

                //  userrole = "COORDINATOR";
                if (userrole.ToUpper() == "COORDINATOR")
                {
                   
                    //Employees_Planner_Management

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Employees_Planner_Management";
                        cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                        {
                            Value = data.User_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "1"
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value =0
                        });
                        cmd.CommandType = CommandType.StoredProcedure;
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.griddata = retObject.ToArray(); // existing DTO used 
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                }
                else if (userrole.ToUpper() == "STAFF")
                {                 
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Employees_Planner_Management";
                        cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                        {
                            Value = data.User_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "2"
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.CommandType = CommandType.StoredProcedure;
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.griddata = retObject.ToArray(); // existing DTO used 
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                //ADMIN
                //clientList
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Master_Client_Details";
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.CommandType = CommandType.StoredProcedure;
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.ClientNames = retObject.ToArray(); // existing DTO used 
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
        public HR_Planner_ManagementDTO getdetailsreminder(HR_Planner_ManagementDTO data)
        {
            try
            {
                string userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).Select(t => t.IVRMRT_Role).FirstOrDefault();
                data.employeelist = _IssueContext.MasterEmployee.Where(e => e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false).ToArray();

                data.HRME_Id = _IssueContext.Staff_User_Login.Where(t => t.Id == data.User_Id).Select(t => t.Emp_Code).FirstOrDefault();

                
                if (userrole.ToUpper() == "STAFF")
                {
                   
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Employees_Planner_Management";
                        cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                        {
                            Value = data.User_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "3"
                        });

                        cmd.CommandType = CommandType.StoredProcedure;
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.griddata = retObject.ToArray(); // existing DTO used 
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                }
                else
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Employees_Planner_Management";
                        cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar)
                        {
                            Value = data.User_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = 0
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = "4"
                        });

                        cmd.CommandType = CommandType.StoredProcedure;
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.griddata = retObject.ToArray(); // existing DTO used 
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

         
                }
                //ADMIN
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Planner_ManagementDTO savePlanner(HR_Planner_ManagementDTO data)
        {
            try
            {
                string mob = "";
                string eml = "";
                if (data.reminderflag == false)
                {
                    for (int c = 0; c < data.emp_array.Length; c++)
                    {
                        var duplicate = _IssueContext.HR_Planner_ManagementDMO.Where(t => t.HRME_Id == data.emp_array[c] && t.HRPLMG_PlannerName == data.HRPLMG_PlannerName && t.HRPLMG_PlannerDetails == data.HRPLMG_PlannerDetails && t.HRPLMG_MeetingPersonName == data.HRPLMG_MeetingPersonName && t.HRPLMG_ContactNo == data.HRPLMG_ContactNo && t.HRPLMG_MeetingAddress == data.HRPLMG_MeetingAddress && t.HRPLMG_MeetingDateTime == data.HRPLMG_MeetingDateTime && t.HRPLMG_MeetingPurpose == data.HRPLMG_MeetingPurpose && t.HRPLMG_PersonsAccompanying == data.HRPLMG_PersonsAccompanying && t.HRPLMG_EntryDateTime == data.HRPLMG_EntryDateTime && t.HRPLMG_ReminderSchedule == data.HRPLMG_ReminderSchedule && t.HRPLMG_ReminderFlag == data.HRPLMG_ReminderFlag && t.HRPLMG_RepeatFlag == data.HRPLMG_RepeatFlag).ToArray();

                        if (duplicate.Length > 0)
                        {
                            data.returnval = "Duplicate";
                        }
                        else
                        {
                            if (data.HRPLMG_Id == 0)
                            {
                                HR_Planner_ManagementDMO dobj = new HR_Planner_ManagementDMO();
                                dobj.MI_Id = data.MI_Id;
                                dobj.ISMMCLT_Id = data.ISMMCLT_Id;
                                dobj.ISMPRINC_Id = data.ISMPRINC_Id;
                                dobj.HRME_Id = data.emp_array[c];
                                dobj.HRPLMG_PlannerName = data.HRPLMG_PlannerName;
                                dobj.HRPLMG_ActiveFlag = true;
                                dobj.HRPLMG_RemainderNotificationFlag = false;
                                dobj.HRPLMG_PlannerDetails = data.HRPLMG_PlannerDetails;
                                dobj.HRPLMG_MeetingPersonName = data.HRPLMG_MeetingPersonName;
                                dobj.HRPLMG_ContactNo = data.HRPLMG_ContactNo;
                                dobj.HRPLMG_MeetingAddress = data.HRPLMG_MeetingAddress;
                                dobj.HRPLMG_MeetingDateTime = data.HRPLMG_MeetingDateTime;
                                dobj.HRPLMG_MeetingPurpose = data.HRPLMG_MeetingPurpose;
                                dobj.HRPLMG_PersonsAccompanying = data.HRPLMG_PersonsAccompanying;
                                dobj.HRPLMG_EntryDateTime = data.HRPLMG_EntryDateTime;
                                dobj.HRPLMG_Remarks = data.HRPLMG_Remarks;
                                dobj.HRPLMG_ReminderSchedule = data.HRPLMG_ReminderSchedule;
                                dobj.HRPLMG_ReminderFlag = data.HRPLMG_ReminderFlag;
                                dobj.HRPLMG_RepeatFlag = data.HRPLMG_RepeatFlag;
                                dobj.HRPLMG_MeetingStartTime = data.HRPLMG_MeetingStartTime;
                                dobj.HRPLMG_MeetingEndTime = data.HRPLMG_MeetingEndTime;
                                dobj.HRPLMG_CreatedDate = DateTime.Now;
                                dobj.HRPLMG_CreatedBy = data.User_Id;
                                dobj.HRPLMG_UpdatedBy = data.User_Id;
                                _IssueContext.Add(dobj);



                                var deleteexisting = _IssueContext.HR_Planner_Management_ApprovalDMO.Where(r => r.HRPLMG_Id == data.HRPLMG_Id).ToList();
                                if (deleteexisting.Count > 0)
                                {
                                    foreach (var item in deleteexisting)
                                    {
                                        _IssueContext.Remove(item);
                                    }
                                }
                                if (data.Multipleuserlvl.Length > 0)
                                {
                                    foreach (var item in data.Multipleuserlvl)
                                    {
                                        HR_Planner_Management_ApprovalDMO obj = new HR_Planner_Management_ApprovalDMO();
                                        obj.HRPLMG_Id = dobj.HRPLMG_Id;
                                        obj.UserId = item.userId;
                                        obj.HRPLMGAPP_ApprovalAlevel = item.HRPLMGAPP_ApprovalAlevel;
                                        obj.HRPLMGAPP_MaxApprovalDays = item.HRPLMGAPP_MaxApprovalDays;
                                        obj.HRPLMGAPP_ActiveFlag = true;
                                        obj.HRPLMGAPP_CreatedBy = data.User_Id;
                                        obj.HRPLMGAPP_UpdatedBy = data.User_Id;
                                        obj.HRPLMGAPP_CreatedDate = DateTime.Now;
                                        obj.HRPLMGAPP_Updateddate = DateTime.Now;
                                        _IssueContext.Add(obj);

                                    }
                                }

                                var filelidt = _IssueContext.HR_Planner_Management_FileDMO.Where(r => r.HRPLMG_Id == data.HRPLMG_Id).ToList();

                                if (filelidt.Count > 0)
                                {
                                    foreach (var item in filelidt)
                                    {
                                        _IssueContext.Remove(item);
                                    }
                                }

                                if (data.filelist.Length > 0)
                                {
                                    foreach (var item in data.filelist)
                                    {

                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            HR_Planner_Management_FileDMO obb = new HR_Planner_Management_FileDMO();


                                            obb.HRPLMG_Id = dobj.HRPLMG_Id;
                                            obb.HRPLMGFL_FileName = item.cfilename;
                                            obb.HRPLMGFL_FileLocation = item.cfilepath;
                                            obb.HRPLMGFL_ActiveFlag = true;

                                            obb.HRPLMGFL_CreatedBy = data.User_Id;
                                            obb.HRPLMGFL_UpdatedBy = data.User_Id;
                                            obb.HRPLMGFL_CreatedDate = DateTime.Now;
                                            obb.HRPLMGFL_Updateddate = DateTime.Now;
                                            _IssueContext.Add(obb);
                                        }


                                    }
                                }




                                var contactExists = _IssueContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.HRPLMG_Id = dobj.HRPLMG_Id;
                                    data.returnval = "Add";
                                }
                                else
                                {
                                    data.returnval = "false";
                                }
                            }
                            else
                            {
                                HR_Planner_ManagementDMO dobj = new HR_Planner_ManagementDMO();
                                dobj = _IssueContext.HR_Planner_ManagementDMO.Where(t => t.HRPLMG_Id == data.HRPLMG_Id).FirstOrDefault();
                                dobj.MI_Id = data.MI_Id;
                                dobj.ISMMCLT_Id = data.ISMMCLT_Id;
                                dobj.ISMPRINC_Id = data.ISMPRINC_Id;
                                dobj.HRME_Id = data.emp_array[c];
                                dobj.HRPLMG_PlannerName = data.HRPLMG_PlannerName;
                                dobj.HRPLMG_ActiveFlag = true;
                                dobj.HRPLMG_RemainderNotificationFlag = false;
                                dobj.HRPLMG_PlannerDetails = data.HRPLMG_PlannerDetails;
                                dobj.HRPLMG_MeetingPersonName = data.HRPLMG_MeetingPersonName;
                                dobj.HRPLMG_ContactNo = data.HRPLMG_ContactNo;
                                dobj.HRPLMG_MeetingAddress = data.HRPLMG_MeetingAddress;
                                dobj.HRPLMG_MeetingDateTime = data.HRPLMG_MeetingDateTime;
                                dobj.HRPLMG_MeetingPurpose = data.HRPLMG_MeetingPurpose;
                                dobj.HRPLMG_PersonsAccompanying = data.HRPLMG_PersonsAccompanying;
                                dobj.HRPLMG_EntryDateTime = data.HRPLMG_EntryDateTime;
                                dobj.HRPLMG_Remarks = data.HRPLMG_Remarks;
                                dobj.HRPLMG_ReminderSchedule = data.HRPLMG_ReminderSchedule;
                                dobj.HRPLMG_ReminderFlag = data.HRPLMG_ReminderFlag;
                                dobj.HRPLMG_RepeatFlag = data.HRPLMG_RepeatFlag;
                                dobj.HRPLMG_MeetingStartTime = data.HRPLMG_MeetingStartTime;
                                dobj.HRPLMG_MeetingEndTime = data.HRPLMG_MeetingEndTime;
                                dobj.HRPLMG_Updateddate = DateTime.Now;
                                dobj.HRPLMG_UpdatedBy = data.User_Id;
                                _IssueContext.Update(dobj);

                                var deleteexisting = _IssueContext.HR_Planner_Management_ApprovalDMO.Where(r => r.HRPLMG_Id == data.HRPLMG_Id).ToList();
                                if (deleteexisting.Count > 0)
                                {
                                    foreach (var item in deleteexisting)
                                    {
                                        _IssueContext.Remove(item);
                                    }
                                }
                                if (data.Multipleuserlvl.Length > 0)
                                {
                                    foreach (var item in data.Multipleuserlvl)
                                    {
                                        HR_Planner_Management_ApprovalDMO obj = new HR_Planner_Management_ApprovalDMO();
                                        obj.HRPLMG_Id = dobj.HRPLMG_Id;
                                        obj.UserId = item.userId;
                                        obj.HRPLMGAPP_ApprovalAlevel = item.HRPLMGAPP_ApprovalAlevel;
                                        obj.HRPLMGAPP_MaxApprovalDays = item.HRPLMGAPP_MaxApprovalDays;
                                        obj.HRPLMGAPP_ActiveFlag = true;
                                        obj.HRPLMGAPP_CreatedBy = data.User_Id;
                                        obj.HRPLMGAPP_UpdatedBy = data.User_Id;
                                        obj.HRPLMGAPP_CreatedDate = DateTime.Now;
                                        obj.HRPLMGAPP_Updateddate = DateTime.Now;
                                        _IssueContext.Add(obj);

                                    }
                                }

                                var filelidt = _IssueContext.HR_Planner_Management_FileDMO.Where(r => r.HRPLMG_Id == data.HRPLMG_Id).ToList();

                                if (filelidt.Count > 0)
                                {
                                    foreach (var item in filelidt)
                                    {
                                        _IssueContext.Remove(item);
                                    }
                                }
                                if (data.filelist.Length > 0)
                                {
                                    foreach (var item in data.filelist)
                                    {

                                        if (item.cfilepath != null && item.cfilepath != "")
                                        {
                                            HR_Planner_Management_FileDMO obb = new HR_Planner_Management_FileDMO();


                                            obb.HRPLMG_Id = data.HRPLMG_Id;
                                            obb.HRPLMGFL_FileName = item.cfilename;
                                            obb.HRPLMGFL_FileLocation = item.cfilepath;
                                            obb.HRPLMGFL_ActiveFlag = true;

                                            obb.HRPLMGFL_CreatedBy = data.User_Id;
                                            obb.HRPLMGFL_UpdatedBy = data.User_Id;
                                            obb.HRPLMGFL_CreatedDate = DateTime.Now;
                                            obb.HRPLMGFL_Updateddate = DateTime.Now;
                                            _IssueContext.Add(obb);
                                        }


                                    }
                                }
                                var contactExists = _IssueContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnval = "Update";
                                }
                                else
                                {
                                    data.returnval = "false";
                                }
                            }
                        }
                    }
                }
                else
                {
                    data.HRME_Id = _IssueContext.Staff_User_Login.Where(t => t.Id == data.User_Id).Select(t => t.Emp_Code).FirstOrDefault();

                    for (int i = 0; i < data.mobile_list_dto.Length; i++)
                    {
                        if (i == 0)
                        {
                            mob = data.mobile_list_dto[i].HRMEMNO_MobileNo.ToString();
                        }
                        else
                        {
                            mob = mob + "," + data.mobile_list_dto[i].HRMEMNO_MobileNo.ToString();
                        }
                    }

                    for (int i = 0; i < data.email_list_dto.Length; i++)
                    {
                        if (i == 0)
                        {
                            eml = data.email_list_dto[i].HRMEM_EmailId.ToString();
                        }
                        else
                        {
                            eml = eml + "," + data.email_list_dto[i].HRMEM_EmailId.ToString();
                        }
                    }


                    if (data.HRPLMG_Id == 0)
                    {
                        var duplicate = _IssueContext.HR_Planner_ManagementDMO.Where(t => t.HRPLMG_PlannerName == data.HRPLMG_PlannerName).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.returnval = "Duplicate";
                        }
                        else
                        {
                            HR_Planner_ManagementDMO dobj = new HR_Planner_ManagementDMO();
                            dobj.MI_Id = data.MI_Id;
                            dobj.ISMMCLT_Id = data.ISMMCLT_Id;
                            dobj.ISMPRINC_Id = data.ISMPRINC_Id;
                            dobj.HRME_Id = data.HRME_Id;
                            dobj.HRPLMG_PlannerName = data.HRPLMG_PlannerName;

                            dobj.HRPLMG_ActiveFlag = true;
                            dobj.HRPLMG_RemainderNotificationFlag = true;
                            dobj.HRPLMG_PlannerDetails = data.HRPLMG_PlannerDetails;
                            dobj.HRPLMG_MeetingPersonName = data.HRPLMG_MeetingPersonName;
                            dobj.HRPLMG_ContactNo = data.HRPLMG_ContactNo;
                            dobj.HRPLMG_MeetingAddress = data.HRPLMG_MeetingAddress;
                            dobj.HRPLMG_MeetingDateTime = data.HRPLMG_MeetingDateTime;
                            dobj.HRPLMG_MeetingPurpose = data.HRPLMG_MeetingPurpose;
                            dobj.HRPLMG_PersonsAccompanying = data.HRPLMG_PersonsAccompanying;
                            dobj.HRPLMG_EntryDateTime = data.HRPLMG_EntryDateTime;
                            dobj.HRPLMG_Remarks = data.HRPLMG_Remarks;
                            dobj.HRPLMG_ReminderSchedule = data.remiderday.ToString();
                            dobj.HRPLMG_ReminderFlag = data.HRPLMG_ReminderFlag;
                            dobj.HRPLMG_RepeatFlag = data.HRPLMG_RepeatFlag;
                            dobj.HRPLMG_MeetingStartTime = data.HRPLMG_MeetingStartTime;
                            dobj.HRPLMG_MeetingEndTime = data.HRPLMG_MeetingEndTime;
                            dobj.HRPLMG_CreatedDate = DateTime.Now;
                            dobj.HRPLMG_CreatedBy = data.User_Id;
                            dobj.HRPLMG_UpdatedBy = data.User_Id;
                            dobj.HRPLMG_ReminderContent = data.HRPLMG_ReminderContent;
                            dobj.HRPLMG_ReminderToEmailid = eml;
                            dobj.HRPLMG_ReminderToMobileNo = mob;
                            dobj.HRPLMG_PaidDate = data.HRPLMG_PaidDate;
                            _IssueContext.Add(dobj);
                            var contactExists = _IssueContext.SaveChanges();

                            var deleteexisting = _IssueContext.HR_Planner_Management_ApprovalDMO.Where(r => r.HRPLMG_Id == dobj.HRPLMG_Id).ToList();
                            if (deleteexisting.Count > 0)
                            {
                                foreach (var item in deleteexisting)
                                {
                                    _IssueContext.Remove(item);
                                }
                            }
                            if (data.Multipleuserlvl.Length > 0)
                            {
                                foreach (var item in data.Multipleuserlvl)
                                {
                                    HR_Planner_Management_ApprovalDMO obj = new HR_Planner_Management_ApprovalDMO();
                                    obj.HRPLMG_Id = dobj.HRPLMG_Id;
                                    obj.UserId = item.userId;
                                    obj.HRPLMGAPP_ApprovalAlevel = item.HRPLMGAPP_ApprovalAlevel;
                                    obj.HRPLMGAPP_MaxApprovalDays = item.HRPLMGAPP_MaxApprovalDays;
                                    obj.HRPLMGAPP_ActiveFlag = true;
                                    obj.HRPLMGAPP_CreatedBy = data.User_Id;
                                    obj.HRPLMGAPP_UpdatedBy = data.User_Id;
                                    obj.HRPLMGAPP_CreatedDate = DateTime.Now;
                                    obj.HRPLMGAPP_Updateddate = DateTime.Now;
                                    _IssueContext.Add(obj);
                                    _IssueContext.SaveChanges();
                                }
                            }
                            if (contactExists > 0)
                            {
                                data.HRPLMG_Id = dobj.HRPLMG_Id;
                                data.returnval = "Add";
                            }
                            else
                            {
                                data.returnval = "false";
                            }
                        }
                    }
                    else
                    {
                        var duplicate = _IssueContext.HR_Planner_ManagementDMO.Where(t => t.HRPLMG_PlannerName == data.HRPLMG_PlannerName && t.HRPLMG_Id != data.HRPLMG_Id).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.returnval = "Duplicate";
                        }
                        else
                        {
                            HR_Planner_ManagementDMO dobj = new HR_Planner_ManagementDMO();
                            dobj = _IssueContext.HR_Planner_ManagementDMO.Where(t => t.HRPLMG_Id == data.HRPLMG_Id).FirstOrDefault();
                            dobj.MI_Id = data.MI_Id;
                            dobj.ISMMCLT_Id = data.ISMMCLT_Id;
                            dobj.ISMPRINC_Id = data.ISMPRINC_Id;
                            dobj.HRME_Id = data.HRME_Id;
                            dobj.HRPLMG_PlannerName = data.HRPLMG_PlannerName;
                            dobj.HRPLMG_ActiveFlag = true;
                            dobj.HRPLMG_RemainderNotificationFlag = true;
                            dobj.HRPLMG_PlannerDetails = data.HRPLMG_PlannerDetails;
                            dobj.HRPLMG_MeetingPersonName = data.HRPLMG_MeetingPersonName;
                            dobj.HRPLMG_ContactNo = data.HRPLMG_ContactNo;
                            dobj.HRPLMG_MeetingAddress = data.HRPLMG_MeetingAddress;
                            dobj.HRPLMG_MeetingDateTime = data.HRPLMG_MeetingDateTime;
                            dobj.HRPLMG_MeetingPurpose = data.HRPLMG_MeetingPurpose;
                            dobj.HRPLMG_PersonsAccompanying = data.HRPLMG_PersonsAccompanying;
                            dobj.HRPLMG_EntryDateTime = data.HRPLMG_EntryDateTime;
                            dobj.HRPLMG_Remarks = data.HRPLMG_Remarks;
                            dobj.HRPLMG_ReminderSchedule = data.remiderday.ToString();
                            dobj.HRPLMG_ReminderFlag = data.HRPLMG_ReminderFlag;
                            dobj.HRPLMG_RepeatFlag = data.HRPLMG_RepeatFlag;
                            dobj.HRPLMG_MeetingStartTime = data.HRPLMG_MeetingStartTime;
                            dobj.HRPLMG_MeetingEndTime = data.HRPLMG_MeetingEndTime;
                            dobj.HRPLMG_ReminderContent = data.HRPLMG_ReminderContent;
                            dobj.HRPLMG_Updateddate = DateTime.Now;
                            dobj.HRPLMG_UpdatedBy = data.User_Id;
                            dobj.HRPLMG_ReminderToEmailid = eml;
                            dobj.HRPLMG_ReminderToMobileNo = mob;
                            dobj.HRPLMG_PaidDate = data.HRPLMG_PaidDate;
                            _IssueContext.Update(dobj);
                            var contactExists = _IssueContext.SaveChanges();


                            var deleteexisting = _IssueContext.HR_Planner_Management_ApprovalDMO.Where(r => r.HRPLMG_Id == dobj.HRPLMG_Id).ToList();
                            if (deleteexisting.Count > 0)
                            {
                                foreach (var item in deleteexisting)
                                {
                                    _IssueContext.Remove(item);
                                }
                            }
                            if (data.Multipleuserlvl.Length > 0)
                            {
                                foreach (var item in data.Multipleuserlvl)
                                {
                                    HR_Planner_Management_ApprovalDMO obj = new HR_Planner_Management_ApprovalDMO();
                                    obj.HRPLMG_Id = dobj.HRPLMG_Id;
                                    obj.UserId = item.userId;
                                    obj.HRPLMGAPP_ApprovalAlevel = item.HRPLMGAPP_ApprovalAlevel;
                                    obj.HRPLMGAPP_MaxApprovalDays = item.HRPLMGAPP_MaxApprovalDays;
                                    obj.HRPLMGAPP_ActiveFlag = true;
                                    obj.HRPLMGAPP_CreatedBy = data.User_Id;
                                    obj.HRPLMGAPP_UpdatedBy = data.User_Id;
                                    obj.HRPLMGAPP_CreatedDate = DateTime.Now;
                                    obj.HRPLMGAPP_Updateddate = DateTime.Now;
                                    _IssueContext.Add(obj);
                                    _IssueContext.SaveChanges();

                                }
                            }

                            if (contactExists > 0)
                            {
                                data.returnval = "Update";
                            }
                            else
                            {
                                data.returnval = "false";
                            }
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
        public HR_Planner_ManagementDTO edit(HR_Planner_ManagementDTO data)
        {
            try
            {
                var editdata = _IssueContext.HR_Planner_ManagementDMO.Where(t => t.HRPLMG_Id == data.HRPLMG_Id).ToList();

                data.editdata = editdata.ToArray();

                //
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Performa_invoice_Records_Details";
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar)
                    {
                        Value = editdata[0].ISMMCLT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
                    });

                    cmd.CommandType = CommandType.StoredProcedure;
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Performadetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Performa_invoice_Records_Details";
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar)
                    {
                        Value = editdata[0].ISMMCLT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "2"
                    });

                    cmd.CommandType = CommandType.StoredProcedure;
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Templatedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.editfiles = (from a in _IssueContext.HR_Planner_Management_FileDMO

                                  where (a.HRPLMG_Id == data.HRPLMG_Id)
                                  select new MasterplannerfiledocDTO
                                  {
                                      cfilename = a.HRPLMGFL_FileName,
                                      cfilepath = a.HRPLMGFL_FileLocation,
                                      //  cfiledesc = a.TRMVDO_DocumentFileDesc,
                                      //   name = a.TRMVDO_DocumentName,

                                  }).Distinct().ToArray();

              

                data.editedlevelcheck = _IssueContext.HR_Planner_Management_ApprovalDMO.Where(t => t.HRPLMG_Id == data.HRPLMG_Id).ToArray();


                var geteditdata = _IssueContext.HR_Planner_ManagementDMO.Where(t => t.HRPLMG_Id == data.HRPLMG_Id).ToList();

                if (geteditdata.Count > 0)
                {
                    if (geteditdata[0].HRPLMG_ReminderToMobileNo != null && geteditdata[0].HRPLMG_ReminderToMobileNo != "")
                    {
                        List<string> mobilevv = new List<string>(geteditdata[0].HRPLMG_ReminderToMobileNo.Split(','));
                        mobilevv.Reverse();




                        data.mobilenolist = mobilevv.ToArray();


                    }

                    if (geteditdata[0].HRPLMG_ReminderToEmailid != null && geteditdata[0].HRPLMG_ReminderToEmailid != "")
                    {
                        List<string> eevv = new List<string>(geteditdata[0].HRPLMG_ReminderToEmailid.Split(','));
                        eevv.Reverse();




                        data.emailist = eevv.ToArray();


                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Planner_ManagementDTO activedeactive(HR_Planner_ManagementDTO data)
        {
            try
            {
                var resdata = _IssueContext.HR_Planner_ManagementDMO.Single(t => t.HRPLMG_Id == data.HRPLMG_Id);
                if (resdata.HRPLMG_ActiveFlag == false)
                {
                    resdata.HRPLMG_ActiveFlag = true;
                    resdata.HRPLMG_UpdatedBy = data.User_Id;
                    resdata.HRPLMG_Updateddate = DateTime.Now;
                    _IssueContext.Update(resdata);
                }
                else
                {
                    resdata.HRPLMG_ActiveFlag = false;
                    resdata.HRPLMG_UpdatedBy = data.User_Id;
                    resdata.HRPLMG_Updateddate = DateTime.Now;
                    _IssueContext.Update(resdata);
                }
                int ctx = _IssueContext.SaveChanges();
                if (ctx > 0)
                {
                    data.resrnval = true;
                }
                else
                {
                    data.resrnval = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //DeleteYY
        public HR_Planner_ManagementDTO DeleteYY(HR_Planner_ManagementDTO data)
        {
            try
            {


                var result = _IssueContext.HR_Planner_ManagementDMO.Single(t => t.HRPLMG_Id == data.HRPLMG_Id);
                if (result.HRPLMG_ActiveFlag == true)
                {
                    result.HRPLMG_ActiveFlag = false;
                    result.HRPLMG_UpdatedBy = data.User_Id;
                    result.HRPLMG_Updateddate = DateTime.Now;
                    _IssueContext.Update(result);
                }
                else
                {
                    result.HRPLMG_ActiveFlag = true;

                    //  _IssueContext.Update(result);

                }
                int ctx = _IssueContext.SaveChanges();
                if (ctx > 0)
                {
                    data.resrnval = true;
                }
                else
                {
                    data.resrnval = false;
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Planner_ManagementDTO viewuploadflies(HR_Planner_ManagementDTO data)
        {
            try
            {
                data.editfiles = (from a in _IssueContext.HR_Planner_Management_FileDMO

                                  where (a.HRPLMG_Id == data.HRPLMG_Id)
                                  select new MasterplannerfiledocDTO
                                  {
                                      gridid = a.HRPLMG_Id,
                                      cfileid = a.HRPLMGFL_Id,
                                      cfilename = a.HRPLMGFL_FileName,
                                      cfilepath = a.HRPLMGFL_FileLocation,
                                      //cfiledesc = a.TRMVDO_DocumentFileDesc,
                                      //name = a.TRMVDO_DocumentName,

                                  }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Planner_ManagementDTO getempmailid(HR_Planner_ManagementDTO data)
        {
            try
            {
                data.getempmailid = (from a in _IssueContext.Staff_User_Login
                                     from b in _IssueContext.Multiple_Email_DMO
                                  where (a.Id == data.UserId && a.Emp_Code==b.HRME_Id)
                                  select new HR_Planner_ManagementDTO
                                  {
                                      HRMEM_EmailId=b.HRMEM_EmailId

                                  }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Planner_ManagementDTO deleteuploadfile(HR_Planner_ManagementDTO data)
        {
            try
            {
                if (data.HRPLMGFL_Id > 0)
                {
                    var deletefile = _IssueContext.HR_Planner_Management_FileDMO.Where(e => e.HRPLMGFL_Id == data.HRPLMGFL_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            _IssueContext.Remove(item);
                        }


                        int y = _IssueContext.SaveChanges();
                        if (y > 0)
                        {
                            data.resrnval = true;
                        }
                        else
                        {
                            data.resrnval = false;
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
        public HR_Planner_ManagementDTO generalreminderemail(HR_Planner_ManagementDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var reminderlistonce = _IssueContext.HR_Planner_ManagementDMO.Where(a => a.HRPLMG_ActiveFlag == true && a.HRPLMG_RemainderNotificationFlag == true && a.HRPLMG_RepeatFlag == "Never" && Convert.ToDateTime(a.HRPLMG_MeetingDateTime).Date >= indianTime.Date).ToList();

                if (reminderlistonce.Count > 0)
                {
                    foreach (var item in reminderlistonce)
                    {
                        var days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                        DateTime newdate = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).AddDays(-days);

                        if (indianTime.Date >= newdate.Date && indianTime.Date <= Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Date)
                        {
                            if (item.HRPLMG_PaidDate != null)
                            {
                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                {
                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                    {
                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                        eevv.Reverse();
                                        string type = "REMINDER";
                                        if (indianTime.Date > Convert.ToDateTime(item.HRPLMG_MeetingDateTime))
                                        {
                                            type = "OVER-DUE REMINDER";
                                        }
                                        try
                                        {
                                            //Save To Temp Table For Daily Report
                                            Save_Planner_Temp_Details(type, item, indianTime, item.HRPLMG_MeetingDateTime);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        //foreach (var emll in eevv)
                                        //{
                                        //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                        //}


                                        if (item.ISMMCLT_Id > 0)
                                        {
                                            sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, Convert.ToDateTime(item.HRPLMG_MeetingDateTime), type, item.HRPLMG_ReminderFlag);

                                        }
                                        else
                                        {
                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, Convert.ToDateTime(item.HRPLMG_MeetingDateTime), type, item.HRPLMG_ReminderFlag);

                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                    eevv.Reverse();
                                    string type = "REMINDER";
                                    if (indianTime.Date > Convert.ToDateTime(item.HRPLMG_MeetingDateTime))
                                    {
                                        type = "OVER-DUE REMINDER";
                                    }

                                    try
                                    {
                                        //Save To Temp Table For Daily Report
                                        Save_Planner_Temp_Details(type, item, indianTime, item.HRPLMG_MeetingDateTime);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }


                                    if (item.ISMMCLT_Id > 0)
                                    {
                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, Convert.ToDateTime(item.HRPLMG_MeetingDateTime), type, item.HRPLMG_ReminderFlag);

                                    }
                                    else
                                    {
                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, Convert.ToDateTime(item.HRPLMG_MeetingDateTime), type, item.HRPLMG_ReminderFlag);

                                    }                                    //foreach (var emll in eevv)
                                    //{
                                    //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                    //}

                                }
                            }

                            //if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                            //{
                            //    List<string> mbbb = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                            //    mbbb.Reverse();
                            //    foreach (var emll in mbbb)
                            //    {

                            //        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id,item.HRPLMG_PlannerDetails);
                            //    }

                            //}

                        }


                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }

        public HR_Planner_ManagementDTO generalreminderemail_temp(HR_Planner_ManagementDTO data)
        {//added by praveen
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var reminderlistonce = _IssueContext.HR_Planner_ManagementDMO.Where(a => a.HRPLMG_ActiveFlag == true && a.HRPLMG_RemainderNotificationFlag == true && a.HRPLMG_RepeatFlag == "Repeat").Distinct().ToList();

                if (reminderlistonce.Count > 0)
                {
                    foreach (var item in reminderlistonce)
                    {
                        if (item.HRPLMG_MeetingDateTime != null)
                        {
                            DateTime ppdate = Convert.ToDateTime(item.HRPLMG_MeetingDateTime);
                            ppdate = ppdate.Date;
                            if (Convert.ToDateTime(ppdate).Date > indianTime.Date)
                            {
                                var days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                                DateTime newdate = Convert.ToDateTime(ppdate).AddDays(-days);

                                if (indianTime.Date >= newdate.Date && indianTime.Date <= Convert.ToDateTime(ppdate).Date)
                                {
                                    if (item.HRPLMG_PaidDate != null)
                                    {
                                        if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > ppdate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }

                                                //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                //eevv.Reverse();

                                                List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "Get_EmpMailIds";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();

                                                    var retObject = new List<dynamic>();
                                                    try
                                                    {
                                                        using (var dataReader = cmd.ExecuteReader())
                                                        {

                                                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                            while (dataReader.Read())
                                                            {
                                                                Tasklist.Add(new HR_Planner_ManagementDTO
                                                                {
                                                                    HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                });
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.Write(ex.Message);
                                                    }
                                                }


                                                foreach (var obj in Tasklist)
                                                {

                                                    List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                    eevv.Reverse();

                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, ppdate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }

                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                        {
                                            string type = "REMINDER";
                                            if (indianTime.Date > ppdate)
                                            {
                                                type = "OVER-DUE REMINDER";
                                            }

                                            List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmd.CommandText = "Get_EmpMailIds";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });                                               
                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();

                                                var retObject = new List<dynamic>();
                                                try
                                                {
                                                    using (var dataReader = cmd.ExecuteReader())
                                                    {
                                                       
                                                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                            while (dataReader.Read())
                                                            {
                                                                Tasklist.Add(new HR_Planner_ManagementDTO
                                                                {
                                                                    HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),                                                                  
                                                                });
                                                            }                                                            
                                                        }                                                    
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.Write(ex.Message);
                                                }
                                            }

                                            
                                            foreach (var obj in Tasklist)
                                            {
                                              
                                                List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                eevv.Reverse();
                                            

                                           

                                            //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                            //eevv.Reverse();
                                            
                                            try
                                            {
                                                //Save To Temp Table For Daily Report
                                                Save_Planner_Temp_Details(type, item, indianTime, ppdate);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }

                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);
                                        }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (item.HRPLMG_ReminderFlag == "Monthly")
                                {
                                    int currentyear = indianTime.Year;
                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);

                                    string getremindermonth = indianTime.Month.ToString();
                                    string getreminderday = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (getremindermonth.ToString().Length == 1)
                                    {
                                        getremindermonth = "0" + getremindermonth;
                                    }
                                    if (getreminderday.Length == 1)
                                    {
                                        getreminderday = "0" + getreminderday;
                                    }

                                    int monthdays = DateTime.DaysInMonth(currentyear, Convert.ToInt32(getremindermonth));
                                    string expecteddate = "";
                                    if (Convert.ToInt32(getreminderday) > monthdays)
                                    {
                                        expecteddate = monthdays + "/" + getremindermonth + "/" + currentyear;
                                    }
                                    else
                                    {
                                        expecteddate = getreminderday + "/" + getremindermonth + "/" + currentyear;
                                    }


                                    DateTime addreminderdays = DateTime.ParseExact(expecteddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                                    DateTime prevscheduledate = addreminderdays.AddMonths(-1);
                                    DateTime currentscheduledate = addreminderdays;
                                    DateTime nextscheduledate = addreminderdays.AddMonths(1);

                                    DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);
                                    int predeductmonth = Convert.ToDateTime(prevscheduledate).Month;
                                    int prededcuctyear = Convert.ToDateTime(prevscheduledate).Year;
                                    int prededucctdays = DateTime.DaysInMonth(prededcuctyear, predeductmonth);
                                    DateTime prereminderendate = Convert.ToDateTime(prevscheduledate).AddDays(prededucctdays - (days + 1));
                                    if (indianTime.Date >= newdate.Date && indianTime.Date <= prereminderendate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();

                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }

                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);
                                                }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > prevscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                //eevv.Reverse();
                                                List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "Get_EmpMailIds";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();

                                                    var retObject = new List<dynamic>();
                                                    try
                                                    {
                                                        using (var dataReader = cmd.ExecuteReader())
                                                        {

                                                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                            while (dataReader.Read())
                                                            {
                                                                Tasklist.Add(new HR_Planner_ManagementDTO
                                                                {
                                                                    HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                });
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.Write(ex.Message);
                                                    }
                                                }


                                                foreach (var obj in Tasklist)
                                                {

                                                    List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }

                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);
                                            }
                                            }
                                        }
                                    }

                                    DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                    int curdeductmonth = Convert.ToDateTime(currentscheduledate).Month;
                                    int curdedcuctyear = Convert.ToDateTime(currentscheduledate).Year;
                                    int curdeducctdays = DateTime.DaysInMonth(curdedcuctyear, curdeductmonth);
                                    DateTime curreminderendate = Convert.ToDateTime(currentscheduledate).AddDays(curdeducctdays - (days + 1));
                                    if (indianTime.Date >= newdate1.Date && indianTime.Date <= curreminderendate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }

                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                            }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > currentscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                //eevv.Reverse();
                                                List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "Get_EmpMailIds";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();

                                                    var retObject = new List<dynamic>();
                                                    try
                                                    {
                                                        using (var dataReader = cmd.ExecuteReader())
                                                        {

                                                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                            while (dataReader.Read())
                                                            {
                                                                Tasklist.Add(new HR_Planner_ManagementDTO
                                                                {
                                                                    HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                });
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.Write(ex.Message);
                                                    }
                                                }


                                                foreach (var obj in Tasklist)
                                                {

                                                    List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                            }
                                            }
                                        }
                                    }

                                    DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                    int nextdeductmonth = Convert.ToDateTime(nextscheduledate).Month;
                                    int nextdedcuctyear = Convert.ToDateTime(nextscheduledate).Year;
                                    int nextdeducctdays = DateTime.DaysInMonth(nextdedcuctyear, nextdeductmonth);
                                    DateTime nextreminderendate = Convert.ToDateTime(nextscheduledate).AddDays(nextdeducctdays - (days + 1));
                                    if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextreminderendate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                                }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > nextscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                //eevv.Reverse();
                                                List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "Get_EmpMailIds";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();

                                                    var retObject = new List<dynamic>();
                                                    try
                                                    {
                                                        using (var dataReader = cmd.ExecuteReader())
                                                        {

                                                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                            while (dataReader.Read())
                                                            {
                                                                Tasklist.Add(new HR_Planner_ManagementDTO
                                                                {
                                                                    HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                });
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.Write(ex.Message);
                                                    }
                                                }


                                                foreach (var obj in Tasklist)
                                                {

                                                    List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                            }
                                        }
                                        }
                                    }
                                }

                                if (item.HRPLMG_ReminderFlag == "Quarterly")
                                {
                                    int currentyear = indianTime.Year;
                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);

                                    string getremindermonth = Convert.ToDateTime(ppdate).Month.ToString();
                                    string getreminderday = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (getremindermonth.ToString().Length == 1)
                                    {
                                        getremindermonth = "0" + getremindermonth;
                                    }
                                    if (getreminderday.Length == 1)
                                    {
                                        getreminderday = "0" + getreminderday;
                                    }

                                    int monthdays = DateTime.DaysInMonth(currentyear, Convert.ToInt32(getremindermonth));
                                    string expecteddate = "";
                                    if (Convert.ToInt32(getreminderday) > monthdays)
                                    {
                                        expecteddate = monthdays + "/" + getremindermonth + "/" + currentyear;
                                    }
                                    else
                                    {
                                        expecteddate = getreminderday + "/" + getremindermonth + "/" + currentyear;
                                    }

                                    DateTime addreminderdays = DateTime.ParseExact(expecteddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                                    int[] a_array = new int[] { 3, 6, 9, 12 };

                                    foreach (int items in a_array)
                                    {
                                        addreminderdays = addreminderdays.AddMonths(3);

                                        DateTime prevscheduledate = addreminderdays.AddMonths(-3);
                                        DateTime currentscheduledate = addreminderdays;
                                        DateTime nextscheduledate = addreminderdays.AddMonths(3);

                                        DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);
                                        DateTime prereminderendate = Convert.ToDateTime(prevscheduledate).AddMonths(3).AddDays(-days);
                                        if (indianTime.Date >= newdate.Date && indianTime.Date <= prereminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > prevscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        //eevv.Reverse();
                                                        List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Get_EmpMailIds";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();

                                                            var retObject = new List<dynamic>();
                                                            try
                                                            {
                                                                using (var dataReader = cmd.ExecuteReader())
                                                                {

                                                                    //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                    while (dataReader.Read())
                                                                    {
                                                                        Tasklist.Add(new HR_Planner_ManagementDTO
                                                                        {
                                                                            HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.Write(ex.Message);
                                                            }
                                                        }


                                                        foreach (var obj in Tasklist)
                                                        {

                                                            List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                            eevv.Reverse();
                                                            try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);
                                                }
                                                }
                                            }
                                            break;
                                        }

                                        DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                        DateTime curreminderendate = Convert.ToDateTime(currentscheduledate).AddMonths(3).AddDays(-days);
                                        if (indianTime.Date >= newdate1.Date && indianTime.Date <= curreminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > currentscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        //eevv.Reverse();
                                                        List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Get_EmpMailIds";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();

                                                            var retObject = new List<dynamic>();
                                                            try
                                                            {
                                                                using (var dataReader = cmd.ExecuteReader())
                                                                {

                                                                    //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                    while (dataReader.Read())
                                                                    {
                                                                        Tasklist.Add(new HR_Planner_ManagementDTO
                                                                        {
                                                                            HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.Write(ex.Message);
                                                            }
                                                        }


                                                        foreach (var obj in Tasklist)
                                                        {

                                                            List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                            eevv.Reverse();
                                                            try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    }
                                                }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                }
                                                }
                                            }
                                            break;
                                        }

                                        DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                        DateTime nextreminderendate = Convert.ToDateTime(nextscheduledate).AddMonths(3).AddDays(-days);
                                        if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextreminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > nextscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        //eevv.Reverse();
                                                        List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Get_EmpMailIds";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();

                                                            var retObject = new List<dynamic>();
                                                            try
                                                            {
                                                                using (var dataReader = cmd.ExecuteReader())
                                                                {

                                                                    //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                    while (dataReader.Read())
                                                                    {
                                                                        Tasklist.Add(new HR_Planner_ManagementDTO
                                                                        {
                                                                            HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.Write(ex.Message);
                                                            }
                                                        }


                                                        foreach (var obj in Tasklist)
                                                        {

                                                            List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                            eevv.Reverse();
                                                            try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }

                                if (item.HRPLMG_ReminderFlag == "Half Yearly")
                                {
                                    int currentyear = indianTime.Year;
                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);

                                    string getremindermonth = Convert.ToDateTime(ppdate).Month.ToString();
                                    string getreminderday = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (getremindermonth.ToString().Length == 1)
                                    {
                                        getremindermonth = "0" + getremindermonth;
                                    }
                                    if (getreminderday.Length == 1)
                                    {
                                        getreminderday = "0" + getreminderday;
                                    }

                                    int monthdays = DateTime.DaysInMonth(currentyear, Convert.ToInt32(getremindermonth));
                                    string expecteddate = "";
                                    if (Convert.ToInt32(getreminderday) > monthdays)
                                    {
                                        expecteddate = monthdays + "/" + getremindermonth + "/" + currentyear;
                                    }
                                    else
                                    {
                                        expecteddate = getreminderday + "/" + getremindermonth + "/" + currentyear;
                                    }

                                    DateTime addreminderdays = DateTime.ParseExact(expecteddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                                    int[] a_array = new int[] { 6, 12 };
                                    foreach (int items in a_array)
                                    {
                                        addreminderdays = addreminderdays.AddMonths(6);

                                        DateTime prevscheduledate = addreminderdays.AddMonths(-6);
                                        DateTime currentscheduledate = addreminderdays;
                                        DateTime nextscheduledate = addreminderdays.AddMonths(6);

                                        DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);
                                        DateTime prereminderendate = Convert.ToDateTime(prevscheduledate).AddMonths(6).AddDays(-days);
                                        if (indianTime.Date >= newdate.Date && indianTime.Date <= prereminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > prevscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        //eevv.Reverse();
                                                        List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Get_EmpMailIds";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();

                                                            var retObject = new List<dynamic>();
                                                            try
                                                            {
                                                                using (var dataReader = cmd.ExecuteReader())
                                                                {

                                                                    //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                    while (dataReader.Read())
                                                                    {
                                                                        Tasklist.Add(new HR_Planner_ManagementDTO
                                                                        {
                                                                            HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.Write(ex.Message);
                                                            }
                                                        }


                                                        foreach (var obj in Tasklist)
                                                        {

                                                            List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                            eevv.Reverse();
                                                            try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                                }
                                            }
                                            break;
                                        }

                                        DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                        DateTime curreminderendate = Convert.ToDateTime(currentscheduledate).AddMonths(6).AddDays(-days);
                                        if (indianTime.Date >= newdate1.Date && indianTime.Date <= curreminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > currentscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        //eevv.Reverse();
                                                        List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Get_EmpMailIds";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();

                                                            var retObject = new List<dynamic>();
                                                            try
                                                            {
                                                                using (var dataReader = cmd.ExecuteReader())
                                                                {

                                                                    //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                    while (dataReader.Read())
                                                                    {
                                                                        Tasklist.Add(new HR_Planner_ManagementDTO
                                                                        {
                                                                            HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.Write(ex.Message);
                                                            }
                                                        }


                                                        foreach (var obj in Tasklist)
                                                        {

                                                            List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                            eevv.Reverse();
                                                            try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();

                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                                }
                                            }
                                            break;
                                        }

                                        DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                        DateTime nextreminderendate = Convert.ToDateTime(nextscheduledate).AddMonths(6).AddDays(-days);
                                        if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextreminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > nextscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        //eevv.Reverse();
                                                        List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Get_EmpMailIds";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();

                                                            var retObject = new List<dynamic>();
                                                            try
                                                            {
                                                                using (var dataReader = cmd.ExecuteReader())
                                                                {

                                                                    //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                    while (dataReader.Read())
                                                                    {
                                                                        Tasklist.Add(new HR_Planner_ManagementDTO
                                                                        {
                                                                            HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Console.Write(ex.Message);
                                                            }
                                                        }


                                                        foreach (var obj in Tasklist)
                                                        {

                                                            List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                            eevv.Reverse();
                                                            try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }

                                if (item.HRPLMG_ReminderFlag == "Daily")
                                {
                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                    {
                                        string type = "REMINDER";
                                        //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                        //eevv.Reverse();
                                        List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "Get_EmpMailIds";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();

                                            var retObject = new List<dynamic>();
                                            try
                                            {
                                                using (var dataReader = cmd.ExecuteReader())
                                                {

                                                    //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                    while (dataReader.Read())
                                                    {
                                                        Tasklist.Add(new HR_Planner_ManagementDTO
                                                        {
                                                            HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.Write(ex.Message);
                                            }
                                        }


                                        foreach (var obj in Tasklist)
                                        {

                                            List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                            eevv.Reverse();
                                            try
                                        {
                                            //Save To Temp Table For Daily Report
                                            Save_Planner_Temp_Details(type, item, indianTime, indianTime);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, indianTime, type, item.HRPLMG_ReminderFlag);
                                    }
                                    }
                                }

                                if (item.HRPLMG_ReminderFlag == "Yearly")
                                {
                                    int currentyear = indianTime.Year;
                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);

                                    string getremindermonth = Convert.ToDateTime(ppdate).Month.ToString();
                                    string getreminderday = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (getremindermonth.ToString().Length == 1)
                                    {
                                        getremindermonth = "0" + getremindermonth;
                                    }
                                    if (getreminderday.Length == 1)
                                    {
                                        getreminderday = "0" + getreminderday;
                                    }

                                    int monthdays = DateTime.DaysInMonth(currentyear, Convert.ToInt32(getremindermonth));
                                    string expecteddate = "";
                                    if (Convert.ToInt32(getreminderday) > monthdays)
                                    {
                                        expecteddate = monthdays + "/" + getremindermonth + "/" + currentyear;
                                    }
                                    else
                                    {
                                        expecteddate = getreminderday + "/" + getremindermonth + "/" + currentyear;
                                    }

                                    DateTime addreminderdays = DateTime.ParseExact(expecteddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                                    int scheduleyear = Convert.ToDateTime(ppdate).Year;

                                    DateTime currentscheduledate = addreminderdays;
                                    DateTime nextscheduledate = addreminderdays.AddYears(1);
                                    nextscheduledate = nextscheduledate.AddDays(-days);

                                    DateTime newdate = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate.Date && indianTime.Date <= nextscheduledate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    //eevv.Reverse();
                                                    List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        cmd.CommandText = "Get_EmpMailIds";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();

                                                        var retObject = new List<dynamic>();
                                                        try
                                                        {
                                                            using (var dataReader = cmd.ExecuteReader())
                                                            {

                                                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                                while (dataReader.Read())
                                                                {
                                                                    Tasklist.Add(new HR_Planner_ManagementDTO
                                                                    {
                                                                        HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }
                                                    }


                                                    foreach (var obj in Tasklist)
                                                    {

                                                        List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > currentscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                //eevv.Reverse();
                                                List<HR_Planner_ManagementDTO> Tasklist = new List<HR_Planner_ManagementDTO>();

                                                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "Get_EmpMailIds";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@HRPLMGId", SqlDbType.VarChar) { Value = item.HRPLMG_Id });
                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();

                                                    var retObject = new List<dynamic>();
                                                    try
                                                    {
                                                        using (var dataReader = cmd.ExecuteReader())
                                                        {

                                                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                                            while (dataReader.Read())
                                                            {
                                                                Tasklist.Add(new HR_Planner_ManagementDTO
                                                                {
                                                                    HRMEM_EmailId = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                                                });
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.Write(ex.Message);
                                                    }
                                                }


                                                foreach (var obj in Tasklist)
                                                {

                                                    List<string> eevv = new List<string>(obj.HRMEM_EmailId.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                            }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;


        }
        public HR_Planner_ManagementDTO generalremindersms(HR_Planner_ManagementDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var reminderlistonce = _IssueContext.HR_Planner_ManagementDMO.Where(a => a.HRPLMG_ActiveFlag == true && a.HRPLMG_RemainderNotificationFlag == true && a.HRPLMG_RepeatFlag == "Never" && Convert.ToDateTime(a.HRPLMG_MeetingDateTime).Date >= indianTime.Date).ToList();

                if (reminderlistonce.Count > 0)
                {
                    foreach (var item in reminderlistonce)
                    {
                        var days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                        DateTime newdate = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).AddDays(-days);

                        if (indianTime.Date >= newdate.Date && indianTime.Date <= Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Date)
                        {

                            if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                            {
                                List<string> mbbb = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                mbbb.Reverse();
                                foreach (var emll in mbbb)
                                {

                                    sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                }

                            }

                        }


                    }
                }



            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }
        public HR_Planner_ManagementDTO generalreminderemailtemp(HR_Planner_ManagementDTO data)
        {
            try
            {
                generalreminderrepeatEmail(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HR_Planner_ManagementDTO generalreminderrepeatEmail(HR_Planner_ManagementDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //HR_Planner_Daily_SchedulerDetailsDMO hR_Planner_Daily_SchedulerDetailsDMO = new HR_Planner_Daily_SchedulerDetailsDMO();

                //var Temp_PlannerDetails = _IssueContext.HR_Planner_Daily_SchedulerDetailsDMO.Where(a => a.HRPDSD_Date.Value.Date == indianTime.Date).ToArray();

                //if (Temp_PlannerDetails.Length > 0)
                //{
                //    foreach (var d in Temp_PlannerDetails)
                //    {
                //        _IssueContext.Remove(d);
                //    }
                //    _IssueContext.SaveChanges();
                //}

                var reminderlistonce = _IssueContext.HR_Planner_ManagementDMO.Where(a => a.HRPLMG_ActiveFlag == true && a.HRPLMG_RemainderNotificationFlag == true && a.HRPLMG_RepeatFlag == "Repeat").Distinct().ToList();

                if (reminderlistonce.Count > 0)
                {
                    foreach (var item in reminderlistonce)
                    {
                        if (item.HRPLMG_MeetingDateTime != null)
                        {
                            DateTime ppdate = Convert.ToDateTime(item.HRPLMG_MeetingDateTime);
                            ppdate = ppdate.Date;
                            if (Convert.ToDateTime(ppdate).Date > indianTime.Date)
                            {
                                var days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                                DateTime newdate = Convert.ToDateTime(ppdate).AddDays(-days);

                                if (indianTime.Date >= newdate.Date && indianTime.Date <= Convert.ToDateTime(ppdate).Date)
                                {
                                    if (item.HRPLMG_PaidDate != null)
                                    {
                                        if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > ppdate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }

                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, ppdate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }

                                                if (item.ISMMCLT_Id > 0)
                                                {
                                                    sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);

                                                }
                                                else
                                                {
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                        {
                                            string type = "REMINDER";
                                            if (indianTime.Date > ppdate)
                                            {
                                                type = "OVER-DUE REMINDER";
                                            }
                                            List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                            eevv.Reverse();
                                            try
                                            {
                                                //Save To Temp Table For Daily Report
                                                Save_Planner_Temp_Details(type, item, indianTime, ppdate);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }

                                            if (item.ISMMCLT_Id > 0)
                                            {
                                                sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);

                                            }
                                            else
                                            {
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (item.HRPLMG_ReminderFlag == "Monthly")
                                {
                                    int currentyear = indianTime.Year;
                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);

                                    string getremindermonth = indianTime.Month.ToString();
                                    string getreminderday = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (getremindermonth.ToString().Length == 1)
                                    {
                                        getremindermonth = "0" + getremindermonth;
                                    }
                                    if (getreminderday.Length == 1)
                                    {
                                        getreminderday = "0" + getreminderday;
                                    }

                                    int monthdays = DateTime.DaysInMonth(currentyear, Convert.ToInt32(getremindermonth));
                                    string expecteddate = "";
                                    if (Convert.ToInt32(getreminderday) > monthdays)
                                    {
                                        expecteddate = monthdays + "/" + getremindermonth + "/" + currentyear;
                                    }
                                    else
                                    {
                                        expecteddate = getreminderday + "/" + getremindermonth + "/" + currentyear;
                                    }


                                    DateTime addreminderdays = DateTime.ParseExact(expecteddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                                    DateTime prevscheduledate = addreminderdays.AddMonths(-1);
                                    DateTime currentscheduledate = addreminderdays;
                                    DateTime nextscheduledate = addreminderdays.AddMonths(1);

                                    DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);
                                    int predeductmonth = Convert.ToDateTime(prevscheduledate).Month;
                                    int prededcuctyear = Convert.ToDateTime(prevscheduledate).Year;
                                    int prededucctdays = DateTime.DaysInMonth(prededcuctyear, predeductmonth);
                                    DateTime prereminderendate = Convert.ToDateTime(prevscheduledate).AddDays(prededucctdays - (days + 1));
                                    if (indianTime.Date >= newdate.Date && indianTime.Date <= prereminderendate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }

                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);


                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }




                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > prevscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }

                                                //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);
                                                if (item.ISMMCLT_Id > 0)
                                                {
                                                    sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);


                                                }
                                                else
                                                {
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }

                                            }
                                        }
                                    }

                                    DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                    int curdeductmonth = Convert.ToDateTime(currentscheduledate).Month;
                                    int curdedcuctyear = Convert.ToDateTime(currentscheduledate).Year;
                                    int curdeducctdays = DateTime.DaysInMonth(curdedcuctyear, curdeductmonth);
                                    DateTime curreminderendate = Convert.ToDateTime(currentscheduledate).AddDays(curdeducctdays - (days + 1));
                                    if (indianTime.Date >= newdate1.Date && indianTime.Date <= curreminderendate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }

                                                    //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);


                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }


                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > currentscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                                //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                if (item.ISMMCLT_Id > 0)
                                                {
                                                    sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                                else
                                                {
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }


                                            }
                                        }
                                    }

                                    DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                    int nextdeductmonth = Convert.ToDateTime(nextscheduledate).Month;
                                    int nextdedcuctyear = Convert.ToDateTime(nextscheduledate).Year;
                                    int nextdeducctdays = DateTime.DaysInMonth(nextdedcuctyear, nextdeductmonth);
                                    DateTime nextreminderendate = Convert.ToDateTime(nextscheduledate).AddDays(nextdeducctdays - (days + 1));
                                    if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextreminderendate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }


                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > nextscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                            }
                                        }
                                    }
                                }

                                if (item.HRPLMG_ReminderFlag == "Quarterly")
                                {
                                    int currentyear = indianTime.Year;
                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);

                                    string getremindermonth = Convert.ToDateTime(ppdate).Month.ToString();
                                    string getreminderday = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (getremindermonth.ToString().Length == 1)
                                    {
                                        getremindermonth = "0" + getremindermonth;
                                    }
                                    if (getreminderday.Length == 1)
                                    {
                                        getreminderday = "0" + getreminderday;
                                    }

                                    int monthdays = DateTime.DaysInMonth(currentyear, Convert.ToInt32(getremindermonth));
                                    string expecteddate = "";
                                    if (Convert.ToInt32(getreminderday) > monthdays)
                                    {
                                        expecteddate = monthdays + "/" + getremindermonth + "/" + currentyear;
                                    }
                                    else
                                    {
                                        expecteddate = getreminderday + "/" + getremindermonth + "/" + currentyear;
                                    }

                                    DateTime addreminderdays = DateTime.ParseExact(expecteddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                                    int[] a_array = new int[] { 3, 6, 9, 12 };

                                    foreach (int items in a_array)
                                    {
                                        addreminderdays = addreminderdays.AddMonths(3);

                                        DateTime prevscheduledate = addreminderdays.AddMonths(-3);
                                        DateTime currentscheduledate = addreminderdays;
                                        DateTime nextscheduledate = addreminderdays.AddMonths(3);

                                        DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);
                                        DateTime prereminderendate = Convert.ToDateTime(prevscheduledate).AddMonths(3).AddDays(-days);
                                        if (indianTime.Date >= newdate.Date && indianTime.Date <= prereminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > prevscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);
                                                        if (item.ISMMCLT_Id > 0)
                                                        {
                                                            sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }
                                                        else
                                                        {
                                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }


                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }

                                                }
                                            }
                                            break;
                                        }

                                        DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                        DateTime curreminderendate = Convert.ToDateTime(currentscheduledate).AddMonths(3).AddDays(-days);
                                        if (indianTime.Date >= newdate1.Date && indianTime.Date <= curreminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > currentscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);


                                                        if (item.ISMMCLT_Id > 0)
                                                        {
                                                            sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }
                                                        else
                                                        {
                                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }

                                                }
                                            }
                                            break;
                                        }

                                        DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                        DateTime nextreminderendate = Convert.ToDateTime(nextscheduledate).AddMonths(3).AddDays(-days);
                                        if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextreminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > nextscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);


                                                        if (item.ISMMCLT_Id > 0)
                                                        {
                                                            sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }
                                                        else
                                                        {
                                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }


                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                   // sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }



                                                }
                                            }
                                            break;
                                        }
                                    }
                                }

                                if (item.HRPLMG_ReminderFlag == "Half Yearly")
                                {
                                    int currentyear = indianTime.Year;
                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);

                                    string getremindermonth = Convert.ToDateTime(ppdate).Month.ToString();
                                    string getreminderday = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (getremindermonth.ToString().Length == 1)
                                    {
                                        getremindermonth = "0" + getremindermonth;
                                    }
                                    if (getreminderday.Length == 1)
                                    {
                                        getreminderday = "0" + getreminderday;
                                    }

                                    int monthdays = DateTime.DaysInMonth(currentyear, Convert.ToInt32(getremindermonth));
                                    string expecteddate = "";
                                    if (Convert.ToInt32(getreminderday) > monthdays)
                                    {
                                        expecteddate = monthdays + "/" + getremindermonth + "/" + currentyear;
                                    }
                                    else
                                    {
                                        expecteddate = getreminderday + "/" + getremindermonth + "/" + currentyear;
                                    }

                                    DateTime addreminderdays = DateTime.ParseExact(expecteddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                                    int[] a_array = new int[] { 6, 12 };
                                    foreach (int items in a_array)
                                    {
                                        addreminderdays = addreminderdays.AddMonths(6);

                                        DateTime prevscheduledate = addreminderdays.AddMonths(-6);
                                        DateTime currentscheduledate = addreminderdays;
                                        DateTime nextscheduledate = addreminderdays.AddMonths(6);

                                        DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);
                                        DateTime prereminderendate = Convert.ToDateTime(prevscheduledate).AddMonths(6).AddDays(-days);
                                        if (indianTime.Date >= newdate.Date && indianTime.Date <= prereminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > prevscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        if (item.ISMMCLT_Id > 0)
                                                        {
                                                            sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }
                                                        else
                                                        {
                                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }


                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, prevscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }


                                                }
                                            }
                                            break;
                                        }

                                        DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                        DateTime curreminderendate = Convert.ToDateTime(currentscheduledate).AddMonths(6).AddDays(-days);
                                        if (indianTime.Date >= newdate1.Date && indianTime.Date <= curreminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > currentscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                        if (item.ISMMCLT_Id > 0)
                                                        {
                                                            sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }
                                                        else
                                                        {
                                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }


                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                }
                                            }
                                            break;
                                        }

                                        DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                        DateTime nextreminderendate = Convert.ToDateTime(nextscheduledate).AddMonths(6).AddDays(-days);
                                        if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextreminderendate.Date)
                                        {
                                            if (item.HRPLMG_PaidDate != null)
                                            {
                                                if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                                {
                                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                    {
                                                        string type = "REMINDER";
                                                        if (indianTime.Date > nextscheduledate)
                                                        {
                                                            type = "OVER-DUE REMINDER";
                                                        }
                                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                        eevv.Reverse();
                                                        try
                                                        {
                                                            //Save To Temp Table For Daily Report
                                                            Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message);
                                                        }
                                                        //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                                        if (item.ISMMCLT_Id > 0)
                                                        {
                                                            sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }
                                                        else
                                                        {
                                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, nextscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }

                                if (item.HRPLMG_ReminderFlag == "Daily")
                                {
                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                    {
                                        string type = "REMINDER";
                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                        eevv.Reverse();
                                        try
                                        {
                                            //Save To Temp Table For Daily Report
                                            Save_Planner_Temp_Details(type, item, indianTime, indianTime);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                        if (item.ISMMCLT_Id > 0)
                                        {
                                            sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);

                                        }
                                        else
                                        {
                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);

                                        }
                                    }
                                }

                                if (item.HRPLMG_ReminderFlag == "Yearly")
                                {
                                    int currentyear = indianTime.Year;
                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);

                                    string getremindermonth = Convert.ToDateTime(ppdate).Month.ToString();
                                    string getreminderday = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (getremindermonth.ToString().Length == 1)
                                    {
                                        getremindermonth = "0" + getremindermonth;
                                    }
                                    if (getreminderday.Length == 1)
                                    {
                                        getreminderday = "0" + getreminderday;
                                    }

                                    int monthdays = DateTime.DaysInMonth(currentyear, Convert.ToInt32(getremindermonth));
                                    string expecteddate = "";
                                    if (Convert.ToInt32(getreminderday) > monthdays)
                                    {
                                        expecteddate = monthdays + "/" + getremindermonth + "/" + currentyear;
                                    }
                                    else
                                    {
                                        expecteddate = getreminderday + "/" + getremindermonth + "/" + currentyear;
                                    }

                                    DateTime addreminderdays = DateTime.ParseExact(expecteddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                                    int scheduleyear = Convert.ToDateTime(ppdate).Year;

                                    DateTime currentscheduledate = addreminderdays;
                                    DateTime nextscheduledate = addreminderdays.AddYears(1);
                                    nextscheduledate = nextscheduledate.AddDays(-days);

                                    DateTime newdate = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate.Date && indianTime.Date <= nextscheduledate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    try
                                                    {
                                                        //Save To Temp Table For Daily Report
                                                        Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    if (item.ISMMCLT_Id > 0)
                                                    {
                                                        sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                    else
                                                    {
                                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                string type = "REMINDER";
                                                if (indianTime.Date > currentscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                try
                                                {
                                                    //Save To Temp Table For Daily Report
                                                    Save_Planner_Temp_Details(type, item, indianTime, currentscheduledate);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                                //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                if (item.ISMMCLT_Id > 0)
                                                {
                                                    sendclientreminder(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                                else
                                                {
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }

        public HR_Planner_ManagementDTO clientDetails(HR_Planner_ManagementDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Performa_invoice_Records_Details";
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMMCLT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "1"
                    });

                    cmd.CommandType = CommandType.StoredProcedure;
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Performadetails = retObject.ToArray(); 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Performa_invoice_Records_Details";
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMMCLT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "2"
                    });

                    cmd.CommandType = CommandType.StoredProcedure;
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Templatedetails = retObject.ToArray();
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

        public void Save_Planner_Temp_Details(string type, HR_Planner_ManagementDMO item, DateTime? indianTime, DateTime? currentscheduledate)
        {
            try
            {
                HR_Planner_Daily_SchedulerDetailsDMO hR_Planner_Daily_SchedulerDetailsDMO = new HR_Planner_Daily_SchedulerDetailsDMO();
                hR_Planner_Daily_SchedulerDetailsDMO.HRPLMG_Id = item.HRPLMG_Id;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_Date = indianTime;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_PlannerName = item.HRPLMG_PlannerName;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_PlannerDetails = item.HRPLMG_PlannerDetails;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_PlannerContent = item.HRPLMG_ReminderContent;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_RepeatFlag = type;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPLMG_ReminderFlag = item.HRPLMG_ReminderFlag;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_DueDate = currentscheduledate;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_CreatedDate = indianTime;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_UpdatedDate = indianTime;
                hR_Planner_Daily_SchedulerDetailsDMO.HRPDSD_Email = item.HRPLMG_ReminderToEmailid;
                _IssueContext.Add(hR_Planner_Daily_SchedulerDetailsDMO);
                _IssueContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public HR_Planner_ManagementDTO rptmail(HR_Planner_ManagementDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var reminderlistonce = _IssueContext.HR_Planner_ManagementDMO.Where(a => a.HRPLMG_ActiveFlag == true && a.HRPLMG_RemainderNotificationFlag == true && a.HRPLMG_RepeatFlag == "Repeat").Distinct().ToList();

                if (reminderlistonce.Count > 0)
                {
                    foreach (var item in reminderlistonce)
                    {
                        if (item.HRPLMG_MeetingDateTime != null)
                        {

                            DateTime ppdate = Convert.ToDateTime(item.HRPLMG_MeetingDateTime);


                            ppdate = ppdate.Date;

                            if (Convert.ToDateTime(ppdate).Date > indianTime.Date)
                            {
                                var days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                                DateTime newdate = Convert.ToDateTime(ppdate).AddDays(-days);

                                if (indianTime.Date >= newdate.Date && indianTime.Date <= Convert.ToDateTime(ppdate).Date)
                                {
                                    if (item.HRPLMG_PaidDate != null)
                                    {
                                        if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > ppdate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                        {
                                            List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                            eevv.Reverse();
                                            string type = "REMINDER";
                                            if (indianTime.Date > ppdate)
                                            {
                                                type = "OVER-DUE REMINDER";
                                            }
                                            sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, ppdate, type, item.HRPLMG_ReminderFlag);
                                        }
                                    }


                                }
                            }
                            else
                            {
                                //Monthly
                                if (item.HRPLMG_ReminderFlag == "Monthly")
                                {
                                    int currentmonth = indianTime.Month;
                                    int previousmont = indianTime.AddMonths(-1).Month;
                                    int Nextmonth = indianTime.AddMonths(1).Month;

                                    int dayss = Convert.ToDateTime(ppdate).Day;



                                    int scheduleyear = Convert.ToDateTime(ppdate).Year;
                                    int currentyear = indianTime.Year;
                                    string cdate1 = "";
                                    string cdate2 = "";
                                    string cdate3 = "";

                                    string cm = "";
                                    string pm = "";
                                    string nm = "";
                                    string dddt = "";

                                    cm = currentmonth.ToString();
                                    pm = previousmont.ToString();
                                    nm = Nextmonth.ToString();
                                    if (cm.Length == 1)
                                    {
                                        cm = "0" + cm;
                                    }
                                    if (pm.Length == 1)
                                    {
                                        pm = "0" + pm;
                                    }
                                    if (nm.Length == 1)
                                    {
                                        nm = "0" + nm;
                                    }
                                    dddt = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (dddt.Length == 1)
                                    {
                                        dddt = "0" + dddt;
                                    }


                                    if (currentmonth == 2)
                                    {
                                        if (dayss > 28)
                                        {

                                            cdate1 = "28" + "/" + cm + "/" + currentyear;
                                        }
                                        else
                                        {
                                            cdate1 = dddt + "/" + cm + "/" + currentyear;
                                        }
                                    }
                                    else if (currentmonth == 1 || currentmonth == 3 || currentmonth == 5 || currentmonth == 7 || currentmonth == 8 | currentmonth == 10 || currentmonth == 12)
                                    {
                                        if (dayss > 30)
                                        {

                                            cdate1 = "30" + "/" + cm + "/" + currentyear;
                                        }
                                        else
                                        {
                                            cdate1 = dddt + "/" + cm + "/" + currentyear;
                                        }
                                    }
                                    else
                                    {
                                        cdate1 = dddt + "/" + cm + "/" + currentyear;
                                    }



                                    if (Nextmonth < currentmonth)
                                    {
                                        int currentyear1 = currentyear + 1;

                                        if (Nextmonth == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate3 = "28" + "/" + nm + "/" + currentyear1;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                            }
                                        }
                                        else if (Nextmonth == 1 || Nextmonth == 3 || Nextmonth == 5 || Nextmonth == 7 || Nextmonth == 8 | Nextmonth == 10 || Nextmonth == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate3 = "30" + "/" + nm + "/" + currentyear1;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                            }
                                        }
                                        else
                                        {
                                            cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                        }

                                    }
                                    else
                                    {

                                        if (Nextmonth == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate3 = "28" + "/" + nm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear;
                                            }
                                        }
                                        else if (Nextmonth == 1 || Nextmonth == 3 || Nextmonth == 5 || Nextmonth == 7 || Nextmonth == 8 | Nextmonth == 10 || Nextmonth == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate3 = "30" + "/" + nm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear;
                                            }
                                        }
                                        else
                                        {
                                            cdate3 = dddt + "/" + nm + "/" + currentyear;
                                        }


                                    }
                                    if (previousmont > currentmonth)
                                    {
                                        int currentyear2 = currentyear - 1;

                                        if (previousmont == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate2 = "28" + "/" + pm + "/" + currentyear2;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                            }
                                        }
                                        else if (previousmont == 1 || previousmont == 3 || previousmont == 5 || previousmont == 7 || previousmont == 8 | previousmont == 10 || previousmont == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate2 = "30" + "/" + pm + "/" + currentyear2;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                            }
                                        }
                                        else
                                        {
                                            cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                        }

                                    }
                                    else
                                    {

                                        if (previousmont == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate2 = "28" + "/" + pm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear;
                                            }
                                        }
                                        else if (previousmont == 1 || previousmont == 3 || previousmont == 5 || previousmont == 7 || previousmont == 8 | previousmont == 10 || previousmont == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate2 = "30" + "/" + pm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear;
                                            }
                                        }
                                        else
                                        {
                                            cdate2 = dddt + "/" + pm + "/" + currentyear;
                                        }
                                    }

                                    DateTime DT1 = DateTime.ParseExact(cdate1.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    DateTime DT2 = DateTime.ParseExact(cdate2.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat); DateTime DT3 = DateTime.ParseExact(cdate3.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);


                                    DateTime prevscheduledate = DT2;
                                    DateTime currentscheduledate = DT1;
                                    DateTime nextscheduledate = DT3;


                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                                    DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);

                                    if (indianTime.Date >= newdate.Date && indianTime.Date <= prevscheduledate.Date)
                                    {


                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);


                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > prevscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);


                                            }
                                        }










                                    }
                                    DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate1.Date && indianTime.Date <= currentscheduledate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > currentscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                            }
                                        }



                                    }
                                    DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextscheduledate.Date)
                                    {

                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > nextscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                            }
                                        }

                                    }


                                }


                                //Quatrally

                                if (item.HRPLMG_ReminderFlag == "Quarterly")
                                {
                                    int currentmonth = indianTime.Month;
                                    int previousmont = indianTime.AddMonths(-4).Month;
                                    int Nextmonth = indianTime.AddMonths(4).Month;

                                    int scheduleyear = Convert.ToDateTime(ppdate).Year;
                                    int currentyear = indianTime.Year;
                                    string cdate1 = "";
                                    string cdate2 = "";
                                    string cdate3 = "";
                                    int dayss = Convert.ToDateTime(ppdate).Day;
                                    string cm = "";
                                    string pm = "";
                                    string nm = "";
                                    string dddt = "";

                                    cm = currentmonth.ToString();
                                    pm = previousmont.ToString();
                                    nm = Nextmonth.ToString();
                                    if (cm.Length == 1)
                                    {
                                        cm = "0" + cm;
                                    }
                                    if (pm.Length == 1)
                                    {
                                        pm = "0" + pm;
                                    }
                                    if (nm.Length == 1)
                                    {
                                        nm = "0" + nm;
                                    }
                                    dddt = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (dddt.Length == 1)
                                    {
                                        dddt = "0" + dddt;
                                    }





                                    if (currentmonth == 2)
                                    {
                                        if (dayss > 28)
                                        {

                                            cdate1 = "28" + "/" + cm + "/" + currentyear;
                                        }
                                        else
                                        {
                                            cdate1 = dddt + "/" + cm + "/" + currentyear;
                                        }
                                    }
                                    else if (currentmonth == 1 || currentmonth == 3 || currentmonth == 5 || currentmonth == 7 || currentmonth == 8 | currentmonth == 10 || currentmonth == 12)
                                    {
                                        if (dayss > 30)
                                        {

                                            cdate1 = "30" + "/" + cm + "/" + currentyear;
                                        }
                                        else
                                        {
                                            cdate1 = dddt + "/" + cm + "/" + currentyear;
                                        }
                                    }
                                    else
                                    {
                                        cdate1 = dddt + "/" + cm + "/" + currentyear;
                                    }




                                    if (Nextmonth < currentmonth)
                                    {
                                        int currentyear1 = currentyear + 1;

                                        if (Nextmonth == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate3 = "28" + "/" + nm + "/" + currentyear1;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                            }
                                        }
                                        else if (Nextmonth == 1 || Nextmonth == 3 || Nextmonth == 5 || Nextmonth == 7 || Nextmonth == 8 | Nextmonth == 10 || Nextmonth == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate3 = "30" + "/" + nm + "/" + currentyear1;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                            }
                                        }
                                        else
                                        {
                                            cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                        }
                                    }
                                    else
                                    {

                                        if (Nextmonth == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate3 = "20" + "/" + nm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear;
                                            }
                                        }
                                        else if (Nextmonth == 1 || Nextmonth == 3 || Nextmonth == 5 || Nextmonth == 7 || Nextmonth == 8 | Nextmonth == 10 || Nextmonth == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate3 = "30" + "/" + nm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear;
                                            }
                                        }
                                        else
                                        {
                                            cdate3 = dddt + "/" + nm + "/" + currentyear;
                                        }

                                    }
                                    if (previousmont > currentmonth)
                                    {
                                        int currentyear2 = currentyear - 1;



                                        if (previousmont == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate2 = "28" + "/" + pm + "/" + currentyear2;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                            }
                                        }
                                        else if (previousmont == 1 || previousmont == 3 || previousmont == 5 || previousmont == 7 || previousmont == 8 | previousmont == 10 || previousmont == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate2 = "30" + "/" + pm + "/" + currentyear2;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                            }
                                        }
                                        else
                                        {
                                            cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                        }

                                    }
                                    else
                                    {

                                        if (previousmont == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate2 = "28" + "/" + pm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear;
                                            }
                                        }
                                        else if (previousmont == 1 || previousmont == 3 || previousmont == 5 || previousmont == 7 || previousmont == 8 | previousmont == 10 || previousmont == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate2 = "30" + "/" + pm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear;
                                            }
                                        }
                                        else
                                        {
                                            cdate2 = dddt + "/" + pm + "/" + currentyear;
                                        }
                                    }




                                    DateTime DT1 = DateTime.ParseExact(cdate1.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    DateTime DT2 = DateTime.ParseExact(cdate2.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat); DateTime DT3 = DateTime.ParseExact(cdate3.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);


                                    DateTime prevscheduledate = DT2;
                                    DateTime currentscheduledate = DT1;
                                    DateTime nextscheduledate = DT3;



                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                                    DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);

                                    if (indianTime.Date >= newdate.Date && indianTime.Date <= prevscheduledate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);


                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > prevscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);


                                            }
                                        }


                                    }
                                    DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate1.Date && indianTime.Date <= currentscheduledate.Date)
                                    {

                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > currentscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);

                                            }
                                        }

                                    }
                                    DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextscheduledate.Date)
                                    {

                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    //foreach (var emll in eevv)
                                                    //{
                                                    //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                                    //}

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > nextscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                                //foreach (var emll in eevv)
                                                //{
                                                //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                                //}

                                            }
                                        }

                                    }

                                }
                                if (item.HRPLMG_ReminderFlag == "Half Yearly")
                                {
                                    int currentmonth = indianTime.Month;
                                    int previousmont = indianTime.AddMonths(-6).Month;
                                    int Nextmonth = indianTime.AddMonths(6).Month;
                                    int dayss = Convert.ToDateTime(ppdate).Day;
                                    int scheduleyear = Convert.ToDateTime(ppdate).Year;
                                    int currentyear = indianTime.Year;
                                    string cdate1 = "";
                                    string cdate2 = "";
                                    string cdate3 = "";
                                    string cm = "";
                                    string pm = "";
                                    string nm = "";
                                    string dddt = "";

                                    cm = currentmonth.ToString();
                                    pm = previousmont.ToString();
                                    nm = Nextmonth.ToString();
                                    if (cm.Length == 1)
                                    {
                                        cm = "0" + cm;
                                    }
                                    if (pm.Length == 1)
                                    {
                                        pm = "0" + pm;
                                    }
                                    if (nm.Length == 1)
                                    {
                                        nm = "0" + nm;
                                    }
                                    dddt = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (dddt.Length == 1)
                                    {
                                        dddt = "0" + dddt;
                                    }

                                    //cdate1 = dddt + "/" + cm + "/" + currentyear;


                                    if (currentmonth == 2)
                                    {
                                        if (dayss > 28)
                                        {

                                            cdate1 = "28" + "/" + cm + "/" + currentyear;
                                        }
                                        else
                                        {
                                            cdate1 = dddt + "/" + cm + "/" + currentyear;
                                        }
                                    }
                                    else if (currentmonth == 1 || currentmonth == 3 || currentmonth == 5 || currentmonth == 7 || currentmonth == 8 | currentmonth == 10 || currentmonth == 12)
                                    {
                                        if (dayss > 30)
                                        {

                                            cdate1 = "30" + "/" + cm + "/" + currentyear;
                                        }
                                        else
                                        {
                                            cdate1 = dddt + "/" + cm + "/" + currentyear;
                                        }
                                    }
                                    else
                                    {
                                        cdate1 = dddt + "/" + cm + "/" + currentyear;
                                    }




                                    if (Nextmonth < currentmonth)
                                    {
                                        int currentyear1 = currentyear + 1;

                                        if (Nextmonth == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate3 = "28" + "/" + nm + "/" + currentyear1;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                            }
                                        }
                                        else if (Nextmonth == 1 || Nextmonth == 3 || Nextmonth == 5 || Nextmonth == 7 || Nextmonth == 8 | Nextmonth == 10 || Nextmonth == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate3 = "30" + "/" + nm + "/" + currentyear1;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                            }
                                        }
                                        else
                                        {
                                            cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                        }
                                    }
                                    else
                                    {

                                        if (Nextmonth == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate3 = "20" + "/" + nm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear;
                                            }
                                        }
                                        else if (Nextmonth == 1 || Nextmonth == 3 || Nextmonth == 5 || Nextmonth == 7 || Nextmonth == 8 | Nextmonth == 10 || Nextmonth == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate3 = "30" + "/" + nm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear;
                                            }
                                        }
                                        else
                                        {
                                            cdate3 = dddt + "/" + nm + "/" + currentyear;
                                        }

                                    }
                                    if (previousmont > currentmonth)
                                    {
                                        int currentyear2 = currentyear - 1;



                                        if (previousmont == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate2 = "28" + "/" + pm + "/" + currentyear2;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                            }
                                        }
                                        else if (previousmont == 1 || previousmont == 3 || previousmont == 5 || previousmont == 7 || previousmont == 8 | previousmont == 10 || previousmont == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate2 = "30" + "/" + pm + "/" + currentyear2;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                            }
                                        }
                                        else
                                        {
                                            cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                        }

                                    }
                                    else
                                    {

                                        if (previousmont == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate2 = "28" + "/" + pm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear;
                                            }
                                        }
                                        else if (previousmont == 1 || previousmont == 3 || previousmont == 5 || previousmont == 7 || previousmont == 8 | previousmont == 10 || previousmont == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate2 = "30" + "/" + pm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear;
                                            }
                                        }
                                        else
                                        {
                                            cdate2 = dddt + "/" + pm + "/" + currentyear;
                                        }
                                    }




                                    DateTime DT1 = DateTime.ParseExact(cdate1.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    DateTime DT2 = DateTime.ParseExact(cdate2.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat); DateTime DT3 = DateTime.ParseExact(cdate3.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);


                                    DateTime prevscheduledate = DT2;
                                    DateTime currentscheduledate = DT1;
                                    DateTime nextscheduledate = DT3;


                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                                    DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);

                                    if (indianTime.Date >= newdate.Date && indianTime.Date <= prevscheduledate.Date)
                                    {

                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    //foreach (var emll in eevv)
                                                    //{
                                                    //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                                    //}

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > prevscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);
                                                //foreach (var emll in eevv)
                                                //{
                                                //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                                //}

                                            }
                                        }

                                    }
                                    DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate1.Date && indianTime.Date <= currentscheduledate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate1.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > currentscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    //foreach (var emll in eevv)
                                                    //{
                                                    //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                                    //}

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > currentscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, currentscheduledate, type, item.HRPLMG_ReminderFlag);
                                                //foreach (var emll in eevv)
                                                //{
                                                //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                                //}

                                            }
                                        }
                                    }
                                    DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextscheduledate.Date)
                                    {
                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate2.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > nextscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                                    //foreach (var emll in eevv)
                                                    //{
                                                    //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                                    //}

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > nextscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, nextscheduledate, type, item.HRPLMG_ReminderFlag);
                                                //foreach (var emll in eevv)
                                                //{
                                                //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                                //}

                                            }
                                        }



                                    }



                                }


                                if (item.HRPLMG_ReminderFlag == "Daily")
                                {

                                    if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                    {
                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                        eevv.Reverse();
                                        string type = "REMINDER";

                                        sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, indianTime, type, item.HRPLMG_ReminderFlag);
                                        //foreach (var emll in eevv)
                                        //{
                                        //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                        //}

                                    }


                                }

                                if (item.HRPLMG_ReminderFlag == "Yearly")
                                {
                                    int currentmonth = indianTime.Month;
                                    int previousmont = indianTime.AddMonths(-12).Month;
                                    int Nextmonth = indianTime.AddMonths(12).Month;

                                    int scheduleyear = Convert.ToDateTime(ppdate).Year;
                                    int currentyear = indianTime.Year;
                                    string cdate1 = "";
                                    string cdate2 = "";
                                    string cdate3 = "";
                                    string cm = "";
                                    string pm = "";
                                    string nm = "";
                                    string dddt = "";
                                    int dayss = Convert.ToDateTime(ppdate).Day;
                                    cm = currentmonth.ToString();
                                    pm = previousmont.ToString();
                                    nm = Nextmonth.ToString();
                                    if (cm.Length == 1)
                                    {
                                        cm = "0" + cm;
                                    }
                                    if (pm.Length == 1)
                                    {
                                        pm = "0" + pm;
                                    }
                                    if (nm.Length == 1)
                                    {
                                        nm = "0" + nm;
                                    }
                                    dddt = Convert.ToDateTime(ppdate).Day.ToString();

                                    if (dddt.Length == 1)
                                    {
                                        dddt = "0" + dddt;
                                    }



                                    if (currentmonth == 2)
                                    {
                                        if (dayss > 28)
                                        {

                                            cdate1 = "28" + "/" + cm + "/" + currentyear;
                                        }
                                        else
                                        {
                                            cdate1 = dddt + "/" + cm + "/" + currentyear;
                                        }
                                    }
                                    else if (currentmonth == 1 || currentmonth == 3 || currentmonth == 5 || currentmonth == 7 || currentmonth == 8 | currentmonth == 10 || currentmonth == 12)
                                    {
                                        if (dayss > 30)
                                        {

                                            cdate1 = "30" + "/" + cm + "/" + currentyear;
                                        }
                                        else
                                        {
                                            cdate1 = dddt + "/" + cm + "/" + currentyear;
                                        }
                                    }
                                    else
                                    {
                                        cdate1 = dddt + "/" + cm + "/" + currentyear;
                                    }




                                    if (Nextmonth < currentmonth)
                                    {
                                        int currentyear1 = currentyear + 1;

                                        if (Nextmonth == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate3 = "28" + "/" + nm + "/" + currentyear1;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                            }
                                        }
                                        else if (Nextmonth == 1 || Nextmonth == 3 || Nextmonth == 5 || Nextmonth == 7 || Nextmonth == 8 | Nextmonth == 10 || Nextmonth == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate3 = "30" + "/" + nm + "/" + currentyear1;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                            }
                                        }
                                        else
                                        {
                                            cdate3 = dddt + "/" + nm + "/" + currentyear1;
                                        }
                                    }
                                    else
                                    {

                                        if (Nextmonth == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate3 = "20" + "/" + nm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear;
                                            }
                                        }
                                        else if (Nextmonth == 1 || Nextmonth == 3 || Nextmonth == 5 || Nextmonth == 7 || Nextmonth == 8 | Nextmonth == 10 || Nextmonth == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate3 = "30" + "/" + nm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate3 = dddt + "/" + nm + "/" + currentyear;
                                            }
                                        }
                                        else
                                        {
                                            cdate3 = dddt + "/" + nm + "/" + currentyear;
                                        }

                                    }
                                    if (previousmont > currentmonth)
                                    {
                                        int currentyear2 = currentyear - 1;



                                        if (previousmont == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate2 = "28" + "/" + pm + "/" + currentyear2;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                            }
                                        }
                                        else if (previousmont == 1 || previousmont == 3 || previousmont == 5 || previousmont == 7 || previousmont == 8 | previousmont == 10 || previousmont == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate2 = "30" + "/" + pm + "/" + currentyear2;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                            }
                                        }
                                        else
                                        {
                                            cdate2 = dddt + "/" + pm + "/" + currentyear2;
                                        }

                                    }
                                    else
                                    {

                                        if (previousmont == 2)
                                        {
                                            if (dayss > 28)
                                            {

                                                cdate2 = "28" + "/" + pm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear;
                                            }
                                        }
                                        else if (previousmont == 1 || previousmont == 3 || previousmont == 5 || previousmont == 7 || previousmont == 8 | previousmont == 10 || previousmont == 12)
                                        {
                                            if (dayss > 30)
                                            {

                                                cdate2 = "30" + "/" + pm + "/" + currentyear;
                                            }
                                            else
                                            {
                                                cdate2 = dddt + "/" + pm + "/" + currentyear;
                                            }
                                        }
                                        else
                                        {
                                            cdate2 = dddt + "/" + pm + "/" + currentyear;
                                        }
                                    }





                                    DateTime DT1 = DateTime.ParseExact(cdate1.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    DateTime DT2 = DateTime.ParseExact(cdate2.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat); DateTime DT3 = DateTime.ParseExact(cdate3.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);


                                    DateTime prevscheduledate = DT2;
                                    DateTime currentscheduledate = DT1;
                                    DateTime nextscheduledate = DT3;


                                    int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                                    DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);

                                    if (indianTime.Date >= newdate.Date && indianTime.Date <= prevscheduledate.Date)
                                    {

                                        if (item.HRPLMG_PaidDate != null)
                                        {
                                            if (Convert.ToDateTime(item.HRPLMG_PaidDate).Date < newdate.Date)
                                            {
                                                if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                                {
                                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                    eevv.Reverse();
                                                    string type = "REMINDER";
                                                    if (indianTime.Date > prevscheduledate)
                                                    {
                                                        type = "OVER-DUE REMINDER";
                                                    }
                                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);


                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                            {
                                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                                eevv.Reverse();
                                                string type = "REMINDER";
                                                if (indianTime.Date > prevscheduledate)
                                                {
                                                    type = "OVER-DUE REMINDER";
                                                }
                                                sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, prevscheduledate, type, item.HRPLMG_ReminderFlag);


                                            }
                                        }

                                    }
                                    DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate1.Date && indianTime.Date <= currentscheduledate.Date)
                                    {
                                        if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                        {
                                            //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                            //eevv.Reverse();
                                            //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv);
                                            //foreach (var emll in eevv)
                                            //{
                                            //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                            //}

                                        }
                                    }
                                    DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                    if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextscheduledate.Date)
                                    {
                                        if (item.HRPLMG_ReminderToEmailid != "" && item.HRPLMG_ReminderToEmailid != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                        {
                                            //List<string> eevv = new List<string>(item.HRPLMG_ReminderToEmailid.Split(','));
                                            //eevv.Reverse();
                                            //sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv);
                                            //foreach (var emll in eevv)
                                            //{
                                            //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                            //}

                                        }
                                    }



                                }

                            }


                        }

                    }
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public HR_Planner_ManagementDTO generalreminderrepeatsms(HR_Planner_ManagementDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var reminderlistonce = _IssueContext.HR_Planner_ManagementDMO.Where(a => a.HRPLMG_ActiveFlag == true && a.HRPLMG_RemainderNotificationFlag == true && a.HRPLMG_RepeatFlag == "Repeat").Distinct().ToList();

                if (reminderlistonce.Count > 0)
                {
                    foreach (var item in reminderlistonce)
                    {


                        if (Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Date > indianTime.Date)
                        {
                            var days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                            DateTime newdate = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).AddDays(-days);

                            if (indianTime.Date >= newdate.Date && indianTime.Date <= Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Date)
                            {

                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    string type = "REMINDER";

                                    sendremindermail(item.MI_Id, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id, eevv, indianTime, type, item.HRPLMG_ReminderFlag);
                                    //foreach (var emll in eevv)
                                    //{
                                    //    sendremindermail(item.MI_Id, emll, item.HRPLMG_ReminderContent, item.HRPLMG_PlannerDetails, item.HRPLMG_Id);
                                    //}

                                }
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> mbbb = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    mbbb.Reverse();
                                    foreach (var emll in mbbb)
                                    {

                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }

                            }
                        }
                        else
                        {
                            if (item.HRPLMG_ReminderFlag == "Monthly")
                            {


                                int currentmonth = indianTime.Month;
                                int previousmont = indianTime.AddMonths(-1).Month;
                                int Nextmonth = indianTime.AddMonths(1).Month;

                                int scheduleyear = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Year;
                                int currentyear = indianTime.Year;
                                string cdate1 = "";
                                string cdate2 = "";
                                string cdate3 = "";


                                cdate1 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + currentmonth + "/" + currentyear;

                                if (Nextmonth < currentmonth)
                                {
                                    int currentyear1 = currentyear + 1;
                                    cdate3 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + Nextmonth + "/" + currentyear1;
                                }
                                else
                                {
                                    cdate3 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + Nextmonth + "/" + currentyear;
                                }
                                if (previousmont > currentmonth)
                                {
                                    int currentyear2 = currentyear - 1;
                                    cdate2 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + previousmont + "/" + currentyear2;
                                }
                                else
                                {
                                    cdate2 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + previousmont + "/" + currentyear;
                                }




                                DateTime prevscheduledate = Convert.ToDateTime(cdate2);
                                DateTime currentscheduledate = Convert.ToDateTime(cdate1);
                                DateTime nextscheduledate = Convert.ToDateTime(cdate3);


                                int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                                DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);

                                if (indianTime.Date >= newdate.Date && indianTime.Date <= prevscheduledate.Date)
                                {
                                    if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                    {
                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                        eevv.Reverse();
                                        foreach (var emll in eevv)
                                        {
                                            sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                        }

                                    }
                                }
                                DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                                if (indianTime.Date >= newdate1.Date && indianTime.Date <= currentscheduledate.Date)
                                {
                                    if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                    {
                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                        eevv.Reverse();
                                        foreach (var emll in eevv)
                                        {
                                            sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                        }

                                    }
                                }
                                DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                                if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextscheduledate.Date)
                                {
                                    if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                    {
                                        List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                        eevv.Reverse();
                                        foreach (var emll in eevv)
                                        {
                                            sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                        }

                                    }
                                }



                            }
                        }


                        if (item.HRPLMG_ReminderFlag == "Quarterly")
                        {
                            int currentmonth = indianTime.Month;
                            int previousmont = indianTime.AddMonths(-4).Month;
                            int Nextmonth = indianTime.AddMonths(4).Month;

                            int scheduleyear = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Year;
                            int currentyear = indianTime.Year;
                            string cdate1 = "";
                            string cdate2 = "";
                            string cdate3 = "";


                            cdate1 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + currentmonth + "/" + currentyear;

                            if (Nextmonth < currentmonth)
                            {
                                int currentyear1 = currentyear + 1;
                                cdate3 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + Nextmonth + "/" + currentyear1;
                            }
                            else
                            {
                                cdate3 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + Nextmonth + "/" + currentyear;
                            }
                            if (previousmont > currentmonth)
                            {
                                int currentyear2 = currentyear - 1;
                                cdate2 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + previousmont + "/" + currentyear2;
                            }
                            else
                            {
                                cdate2 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + previousmont + "/" + currentyear;
                            }




                            DateTime prevscheduledate = Convert.ToDateTime(cdate2);
                            DateTime currentscheduledate = Convert.ToDateTime(cdate1);
                            DateTime nextscheduledate = Convert.ToDateTime(cdate3);


                            int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                            DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);

                            if (indianTime.Date >= newdate.Date && indianTime.Date <= prevscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }
                            DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                            if (indianTime.Date >= newdate1.Date && indianTime.Date <= currentscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }
                            DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                            if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }



                        }


                        if (item.HRPLMG_ReminderFlag == "Half Yearly")
                        {
                            int currentmonth = indianTime.Month;
                            int previousmont = indianTime.AddMonths(-6).Month;
                            int Nextmonth = indianTime.AddMonths(6).Month;

                            int scheduleyear = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Year;
                            int currentyear = indianTime.Year;
                            string cdate1 = "";
                            string cdate2 = "";
                            string cdate3 = "";


                            cdate1 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + currentmonth + "/" + currentyear;

                            if (Nextmonth < currentmonth)
                            {
                                int currentyear1 = currentyear + 1;
                                cdate3 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + Nextmonth + "/" + currentyear1;
                            }
                            else
                            {
                                cdate3 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + Nextmonth + "/" + currentyear;
                            }
                            if (previousmont > currentmonth)
                            {
                                int currentyear2 = currentyear - 1;
                                cdate2 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + previousmont + "/" + currentyear2;
                            }
                            else
                            {
                                cdate2 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + previousmont + "/" + currentyear;
                            }




                            DateTime prevscheduledate = Convert.ToDateTime(cdate2);
                            DateTime currentscheduledate = Convert.ToDateTime(cdate1);
                            DateTime nextscheduledate = Convert.ToDateTime(cdate3);


                            int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                            DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);

                            if (indianTime.Date >= newdate.Date && indianTime.Date <= prevscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }
                            DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                            if (indianTime.Date >= newdate1.Date && indianTime.Date <= currentscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }
                            DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                            if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }



                        }
                        if (item.HRPLMG_ReminderFlag == "Daily")
                        {

                            if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                            {
                                List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                eevv.Reverse();
                                foreach (var emll in eevv)
                                {
                                    sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                }

                            }


                        }
                        if (item.HRPLMG_ReminderFlag == "Yearly")
                        {
                            int currentmonth = indianTime.Month;
                            int previousmont = indianTime.AddMonths(-12).Month;
                            int Nextmonth = indianTime.AddMonths(12).Month;

                            int scheduleyear = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Year;
                            int currentyear = indianTime.Year;
                            string cdate1 = "";
                            string cdate2 = "";
                            string cdate3 = "";


                            cdate1 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + currentmonth + "/" + currentyear;

                            if (Nextmonth < currentmonth)
                            {
                                int currentyear1 = currentyear + 1;
                                cdate3 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + Nextmonth + "/" + currentyear1;
                            }
                            else
                            {
                                cdate3 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + Nextmonth + "/" + currentyear;
                            }
                            if (previousmont > currentmonth)
                            {
                                int currentyear2 = currentyear - 1;
                                cdate2 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + previousmont + "/" + currentyear2;
                            }
                            else
                            {
                                cdate2 = Convert.ToDateTime(item.HRPLMG_MeetingDateTime).Day + "/" + previousmont + "/" + currentyear;
                            }




                            DateTime prevscheduledate = Convert.ToDateTime(cdate2);
                            DateTime currentscheduledate = Convert.ToDateTime(cdate1);
                            DateTime nextscheduledate = Convert.ToDateTime(cdate3);


                            int days = Convert.ToInt32(item.HRPLMG_ReminderSchedule);
                            DateTime newdate = Convert.ToDateTime(prevscheduledate).AddDays(-days);

                            if (indianTime.Date >= newdate.Date && indianTime.Date <= prevscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }
                            DateTime newdate1 = Convert.ToDateTime(currentscheduledate).AddDays(-days);
                            if (indianTime.Date >= newdate1.Date && indianTime.Date <= currentscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }
                            DateTime newdate2 = Convert.ToDateTime(nextscheduledate).AddDays(-days);
                            if (indianTime.Date >= newdate2.Date && indianTime.Date <= nextscheduledate.Date)
                            {
                                if (item.HRPLMG_ReminderToMobileNo != "" && item.HRPLMG_ReminderToMobileNo != null && item.HRPLMG_ReminderContent != null && item.HRPLMG_ReminderContent != "")
                                {
                                    List<string> eevv = new List<string>(item.HRPLMG_ReminderToMobileNo.Split(','));
                                    eevv.Reverse();
                                    foreach (var emll in eevv)
                                    {
                                        sendremindersms(item.MI_Id, Convert.ToInt64(emll), item.HRPLMG_ReminderContent, item.HRPLMG_Id, item.HRPLMG_PlannerDetails);
                                    }

                                }
                            }



                        }
                    }
                }



            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }
        public string sendremindermail(long MI_Id, string message1, string subject, long HRPLMG_Id, List<string> ToEmail, DateTime remdate, string type, string reminder)
        {
            try
            {
                string mailids = "";
                string Subject = "";
                string background = "#1e78eb";
                if (type != "REMINDER")
                {
                    background = "red";
                }
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == "REMINDER").ToList();

                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = message1;
                string htmltemplet = template.FirstOrDefault().ISES_MailHTMLTemplate.ToString();
                string Mailmsg = message1;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                Mailcontent = Resultsms;
                String dateofrem = remdate.ToString("dd/MM/yyyy");
                htmltemplet = htmltemplet.Replace("[DATE]", dateofrem);
                htmltemplet = htmltemplet.Replace("[TYPE]", type);
                htmltemplet = htmltemplet.Replace("[REMINDER]", subject);
                htmltemplet = htmltemplet.Replace("[DESCRIPTION]", Mailmsg);
                htmltemplet = htmltemplet.Replace("[CHANGECOLOR]", background);

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    if (reminder != "None")
                    {
                        Subject = reminder.ToUpper() + "-" + type + "-" + subject;
                    }
                    else
                    {
                        Subject = type + "-" + subject;
                    }

                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _IssueContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;

                        foreach (var item in ToEmail)
                        {
                            message.AddTo(item);
                            mailids = mailids + "," + item;
                        }

                        message.HtmlContent = htmltemplet;
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

                                foreach (var item in ToEmail)
                                {
                                    emailMessage.To.Add(new MailAddress(item));
                                    mailids = mailids + "," + item;
                                }

                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }

                    ////================smtp complited===========

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mailids
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Reminder"
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

        public string sendclientreminder(long MI_Id, string message1, string subject, long HRPLMG_Id, List<string> ToEmail, DateTime remdate, string type, string reminder)
        {
            try
            {
                string mailids = "";
                string Subject = "";
                string background = "#1e78eb";
                if (type != "REMINDER")
                {
                    background = "red";
                }
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == "roadshow").ToList();

                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = message1;
                //string htmltemplet = template.FirstOrDefault().ISES_MailHTMLTemplate.ToString();
                string Mailmsg = message1;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                Mailcontent = Resultsms;
                //String dateofrem = remdate.ToString("dd/MM/yyyy");
                //htmltemplet = htmltemplet.Replace("[DATE]", dateofrem);
                //htmltemplet = htmltemplet.Replace("[TYPE]", type);
                //htmltemplet = htmltemplet.Replace("[REMINDER]", subject);
                //htmltemplet = htmltemplet.Replace("[DESCRIPTION]", Mailmsg);
                //htmltemplet = htmltemplet.Replace("[CHANGECOLOR]", background);

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    if (reminder != "None")
                    {
                        Subject = reminder.ToUpper() + "-" + type + "-" + subject;
                    }
                    else
                    {
                        Subject = type + "-" + subject;
                    }

                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _IssueContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;

                        foreach (var item in ToEmail)
                        {
                            message.AddTo(item);
                            mailids = mailids + "," + item;
                        }

                        message.HtmlContent = Mailcontent;
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

                                foreach (var item in ToEmail)
                                {
                                    emailMessage.To.Add(new MailAddress(item));
                                    mailids = mailids + "," + item;
                                }

                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }

                    ////================smtp complited===========

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mailids
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Reminder"
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



        public string sendremindersms(long MI_Id, long mobileNo, string message, long ID, string type)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();




                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();



                string sms = message;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _IssueContext.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;




                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Reminder"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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