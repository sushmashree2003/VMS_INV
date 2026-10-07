using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace IssueManager.com.vaps.Services
{
    public class ISM_Task_DeviationImpl : Interfaces.ISM_Task_DeviationInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_Task_DeviationImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public ISM_Task_DeviationDTO GetDetails(ISM_Task_DeviationDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                long? dept_id = 0;
                long? design_id = 0;

                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true
                && t.HRME_LeftFlag == false).Distinct().ToList();

                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

                    data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                }

                var hrmeDepartment = _IssueContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == dept_id
                && t.HRMD_ActiveFlag == true).Distinct().ToList();

                if (hrmeDepartment.Count > 0)
                {
                    data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                }

                

                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_admin_DR_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Department List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt) { Value = dept_id });
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
                        data.DepartmentList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Designation List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DesignationList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt) { Value = data.UserId });
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
                        data.Designation = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                

                //***********************************  ********************************// 

                data.monthlist = _IssueContext.IVRM_Month_DMO.ToArray();

                data.yearlist = _IssueContext.HR_Master_LeaveYearDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true).OrderByDescending(a => a.HRMLY_LeaveYear).ToArray();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Names";
                   
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
                        data.filldata = retObject.ToArray();
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

        public async Task<ISM_Task_DeviationDTO> getdeviationReport(ISM_Task_DeviationDTO data)
        {
            try
            {
                string departments = "0";
                if (data.deptlist.Length > 0)
                {
                    foreach (var ue in data.deptlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }

                string desgination = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.desglist)
                    {
                        desgination = desgination + "," + ue.HRMDES_Id;
                    }
                }

                string emplist = "0";
                if (data.desglist.Length > 0)
                {
                    foreach (var ue in data.emplist)
                    {
                        emplist = emplist + "," + ue.HRME_Id;
                    }
                }

                string confromdate = "";
                string contodate = "";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Fromdate), INDIAN_ZONE);
                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.Todate), INDIAN_ZONE);

                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                contodate = todatecon.ToString("yyyy-MM-dd");

                var GET_EMPID = _IssueContext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                long empid = GET_EMPID.Count > 0 ? GET_EMPID.FirstOrDefault().Emp_Code : 0;


                //******************************Deviation Remarks & Deviation Calculation Report ************************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DeviationTask_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //SqlDbType.VarChar)
                    //{
                    //    Value = data.MI_Id
                    //});
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",
                           SqlDbType.DateTime)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
                    SqlDbType.DateTime)
                    {
                        Value = contodate
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = emplist
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
                        data.get_deviationRemarksReport = retObject.ToArray();
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

        public ISM_Task_DeviationDTO DeptSelectionDetails(ISM_Task_DeviationDTO data)
        {
            try
            {
                string departments = "10000";
                if (data.departmentlist.Length > 0)
                {
                    foreach (var ue in data.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }
                //if (data.DRApprovalTypeFlag.Equals("DRMultiApproval"))
                //{
                //    roletype = "Admin";
                //    data.roleflag = false;
                //}


                        
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DesignationList_DR_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.VarChar) { Value = data.UserId });
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
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
                        data.Designation = retObject.ToArray();
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
        public ISM_Task_DeviationDTO DesgSelectionDetails(ISM_Task_DeviationDTO data)
        {
            try
            {
                string departments = "0";
                if (data.designationlist.Length > 0)
                {
                    foreach (var ue in data.designationlist)
                    {
                        departments = departments + "," + ue.HRMDES_Id;
                    }
                }

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }

                //if (data.DRApprovalTypeFlag.Equals("DRMultiApproval"))
                //{
                //    roletype = "Admin";
                //    data.roleflag = false;
                //}


                //******************************Deviation Remarks & Deviation Calculation Report ************************************//         
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DesignationChange_DR_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments", SqlDbType.VarChar) { Value = departments });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar) { Value = roletype });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.VarChar) { Value = data.UserId });
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
                        data.get_userEmplist = retObject.ToArray();
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


        public ISM_Task_DeviationDTO SaveDateExtention(ISM_Task_DeviationDTO dto)
        {
            try
            {
                dto.DateextStatus = "Pending";
               


                if (dto.DeviationtaskExtention.Length > 0)
                {
                    

                    foreach (var x in dto.DeviationtaskExtention)
                    {

                        try
                        {
                            string ISMTDE_TaskStartDate = x.ISMTDE_TaskStartDate.ToString("yyyy-MM-dd");
                            string ISMTDE_TaskEndDate = x.ISMTDE_TaskEndDate.ToString("yyyy-MM-dd");
                            string ISMTDE_TaskExtendDate = x.ISMTDE_TaskExtendDate.ToString("yyyy-MM-dd");

                            var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("ISM_Task_Deviation_DateExtend_Insert @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7",
                                     x.ISMTCR_Id, ISMTDE_TaskStartDate, ISMTDE_TaskEndDate, ISMTDE_TaskExtendDate,dto.DateextStatus,x.ISMTDE_Remarks, dto.UserId,x.HRME_Id);
                            if (contactExistsP > 0)
                            {
                                dto.message = "save";
                            }
                            else
                            {
                                dto.message = "notsave";
                            }
                        }
                        catch (Exception ee)
                        {
                            dto.message = "failed";
                            Console.WriteLine(ee.Message);
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


        public ISM_Task_DeviationDTO OnloadApproval(ISM_Task_DeviationDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Task_Deviation_DateExtend_Request";
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
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Task_date_Extend_Details1";
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
                        data.empwise_statuslist = retObject.ToArray();
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



        //==========Request Details Approval=================
        public ISM_Task_DeviationDTO Getrequestdetails(ISM_Task_DeviationDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Task_Deviation_DateExtend_Requested_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });

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
                        data.get_taskdetails = retObject.ToArray();
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

        public ISM_Task_DeviationDTO SaveApprovalExtention(ISM_Task_DeviationDTO dto)
        {
            try
            {
                
                if (dto.TaskExtentionApproval.Length > 0)
                {

                    foreach (var x in dto.TaskExtentionApproval)
                    {
                        try
                        { 
                            string ISMTDE_TaskExtendDate = x.ISMTDE_TaskExtendDate.ToString("yyyy-MM-dd");
                            var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("Task_Enddate_Extend_Approval @p0,@p1,@p2,@p3,@p4",
                                     x.ISMTDE_Id, dto.UserId, x.ISMTDEA_Remarks, ISMTDE_TaskExtendDate,dto.Appstatus);
                            if (contactExistsP > 0)
                            {
                                dto.message = "save";
                            }
                            else
                            {
                                dto.message = "notsave";
                            }
                        }
                        catch (Exception ee)
                        {
                            dto.message = "failed";
                            Console.WriteLine(ee.Message);
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


        public ISM_Task_DeviationDTO ViewApproveDetails(ISM_Task_DeviationDTO dto)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Task_date_Extend_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value = dto.HRME_Id });

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
                        dto.getsaveddetails = retObject.ToArray();
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

       


    }
}
