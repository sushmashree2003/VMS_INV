using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Concurrent;
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
using CommonLibrary;
using SendGrid.Helpers.Mail;
using SendGrid;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;

namespace FrontOffice.com.vaps.Services
{
    public class Employee_Add_logs_ManualImpl : Interfaces.Employee_Add_logs_ManualInterface
    {
        int MI_ID = 0;
        private static ConcurrentDictionary<string, FO_Emp_PunchDTO> _login = new ConcurrentDictionary<string, FO_Emp_PunchDTO>();
        public FOContext _FOContext;
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;



        public Employee_Add_logs_ManualImpl(FOContext foctx, HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
            _FOContext = foctx;
        }


        public FO_Emp_PunchDTO getdata(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO TTMC = new FO_Emp_PunchDTO();
            try
            {

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Page_Privilege_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@pageurl", SqlDbType.VarChar)
                    {
                        Value = "app.FrontOfficeAddlogs"
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
                        TTMC.pagePrivilege = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                //Only it will work once page will mapp to that perticular employee
                if (TTMC.pagePrivilege.Length > 0)
                {


                    TTMC.griddata = (from a in _FOContext.FO_Emp_Punch_Alter_Request_DetailsDMO
                                     from b in _FOContext.fO_Emp_Punch_Alter_Requests
                                     from c in _FOContext.HR_Master_Employee_DMO
                                     from d in _FOContext.ApplicationUserDMO
                                     where (a.FOEPAR_Id == b.FOEPAR_Id && c.HRME_Id == b.HRME_Id && b.FOEPAR_CreatedBy == d.Id)
                                     select new FO_Emp_PunchDTO
                                     {
                                         HRME_Id = b.HRME_Id,
                                         HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + (string.IsNullOrEmpty(c.HRME_EmployeeMiddleName) ? "" : ' ' + c.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(c.HRME_EmployeeLastName) ? "" : ' ' + c.HRME_EmployeeLastName),
                                         FOEPAR_PunchDate = b.FOEPAR_PunchDate,
                                         FOEPARD_Time = a.FOEPARD_Time,
                                         FOEPARD_Remarks = a.FOEPARD_Remarks,
                                         FOEPARD_InOutFlg = a.FOEPARD_InOutFlg,
                                         username = d.UserName,
                                         FOEPARD_FilePath = a.FOEPARD_FilePath,
                                         Status = b.FOEPAR_ApprovalFlg
                                     }).OrderByDescending(a => a.FOEPAR_PunchDate).ToArray();

                    var emp = (from m in _FOContext.HR_Master_Employee_DMO
                               where m.HRME_ActiveFlag == true && m.HRME_LeftFlag == false
                               select new HR_Master_Employee_DMO
                               {
                                   HRME_Id = m.HRME_Id,
                                   HRME_EmployeeFirstName = m.HRME_EmployeeFirstName + (string.IsNullOrEmpty(m.HRME_EmployeeMiddleName) ? "" : ' ' + m.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(m.HRME_EmployeeLastName) ? "" : ' ' + m.HRME_EmployeeLastName),
                               }).ToList();
                    TTMC.employeelist = emp.ToArray();



                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;
        }

        public FO_Emp_PunchDTO empname(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO TTMC = new FO_Emp_PunchDTO();
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.emplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                var que1 = _FOContext.FO_Emp_Punch.Where(a => a.MI_Id == data.MI_Id && a.FOEP_PunchDate.Value.Date == data.empdate.Date && a.HRME_Id == data.HRME_Id).Select(t => t.FOEP_Id).FirstOrDefault();

                if (que1 > 0)
                {
                    var que2 = _FOContext.FO_Emp_Punch_Details.Where(b => b.MI_Id == data.MI_Id && b.FOEP_Id == que1).ToList();
                    TTMC.employeelist = que2.ToArray();
                    TTMC.returnval = true;
                }
                else
                {
                    TTMC.returnval = false;
                }
            }
            catch (Exception ex)
            {
                TTMC.returnval = false;
            }
            return TTMC;
        }

        public FO_Emp_PunchDTO savedetail(FO_Emp_PunchDTO data)
        {
            try
            {

                FO_Emp_Punch_Alter_Request objpunch = new FO_Emp_Punch_Alter_Request();
                objpunch.MI_Id = data.MI_Id;
                objpunch.HRME_Id = data.HRME_Id;
                objpunch.FOEPAR_PunchDate = data.FOEP_PunchDate;
                objpunch.FOEPAR_Remarks = data.FOEPAR_Remarks;
                objpunch.FOEPAR_ActiveFlg = true;
                objpunch.FOEPAR_ApprovalFlg = "Requested";
                objpunch.FOEPAR_CreatedDate = DateTime.Now;
                objpunch.FOEPAR_UpdatedDate = DateTime.Now;
                objpunch.FOEPAR_CreatedBy = data.UserId;
                objpunch.FOEPAR_UpdatedBy = data.UserId;
                _FOContext.Add(objpunch);
                _FOContext.SaveChanges();
                for (int i = 0; i < data.temp1.Length; i++)
                {
                    if (objpunch.FOEPAR_Id > 0)
                    {
                        FO_Emp_Punch_Alter_Request_DetailsDMO fopunch = new FO_Emp_Punch_Alter_Request_DetailsDMO();
                        fopunch.FOEPAR_Id = objpunch.FOEPAR_Id;
                        fopunch.FOEPARD_Time = data.temp1[i].FOEPD_PunchTime;
                        fopunch.FOEPARD_InOutFlg = data.temp1[i].InOutFlg;
                        fopunch.FOEPARD_ActiveFlg = true;
                        fopunch.FOEPARD_CreatedDate = DateTime.Now;
                        fopunch.FOEPARD_UpdatedDate = DateTime.Now;
                        fopunch.FOEPARD_ApprvedFlg = true;
                        fopunch.FOEPARD_FilePath = data.temp1[i].FOEPARD_FilePath;
                        fopunch.FOEPARD_Remarks = data.temp1[i].FOEPAR_Remarks;
                        fopunch.FOEPARD_CreatedBy = data.UserId;
                        fopunch.FOEPARD_UpdatedBy = data.UserId;
                        _FOContext.Add(fopunch);

                    }
                }



                var contactExists = _FOContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnmsg = "Saved";
                }
                else
                {
                    data.returnmsg = "Failed";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FO_Emp_PunchDTO editdata(FO_Emp_PunchDTO data)
        {
            try
            {
                data.editvalues = _FOContext.FO_Emp_Punch_Details.Where(t => t.FOEPD_Id == data.FOEPD_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FO_Emp_PunchDTO approvalloaddata(FO_Emp_PunchDTO data)
        {
            try
            {


                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Emp_PunchMissing_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.punchapproveddetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_EMP_PunchMissingEntry_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.punchdetails = retObject1.ToArray();
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
            return data;
        }

        public async Task<FO_Emp_PunchDTO> FindDataAsync(FO_Emp_PunchDTO data)
        {
            try
            {

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_EMP_PunchMissingTimeEntry_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FOEPAR_Id", SqlDbType.BigInt)
                    {
                        Value = data.FOEPAR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FOEPAR_Idall", SqlDbType.VarChar)
                    {
                        Value = data.json
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.logapprovallist = retObject1.ToArray();
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

        public FO_Emp_PunchDTO saveapproval(FO_Emp_PunchDTO data)
        {
            try
            {


                for (int i = 0; i < data.temp1.Length; i++)
                {
                    //FO_Emp_Punch_DetailsDMO obj = new FO_Emp_Punch_DetailsDMO();
                    //obj = _FOContext.FO_Emp_Punch_Details.Where(t => t.FOEPD_Id == data.temp1[i].FOEPD_Id).FirstOrDefault();
                    //obj.FOEPD_CheckedBySPFlg = data.UserId;
                    //_FOContext.Update(obj);
                    //_FOContext.SaveChanges();

                    var contactExistsP = _FOContext.Database.ExecuteSqlCommand("FO_Missing_Logs_Approval_Save @p0,@p1,@p2,@p3,@p4,@p5", data.temp1[i].FOEPARD_Id, data.UserId, data.temp1[i].FOEPD_PunchTime, data.temp1[i].FOEPARD_InOutFlg, data.temp1[i].FOEPAR_Remarks, data.temp1[i].Status);
                    if (contactExistsP > 0)
                    {
                        data.returnmsg = "Success";
                    }
                    else
                    {
                        data.returnmsg = "NotSuccess";
                    }

                    if (data.HRMLY_LeaveYear > 0)
                    {

                        string startdate = data.HRMLY_LeaveYear + "-" + data.IVRM_Month_Id + "-01";
                        var Fromdate = Convert.ToDateTime(startdate);
                        var year = Convert.ToDateTime(startdate).Year;
                        var mon = Convert.ToDateTime(startdate).Month;
                        var startOfMonth = new DateTime(year, mon, 1);
                        var DaysInMonth = DateTime.DaysInMonth(year, mon);
                        var Todate = new DateTime(year, mon, DaysInMonth);


                        using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "FO_PunchMissing_Details";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 800000000;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                            {
                                Value = Convert.ToDateTime(Fromdate).ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                            {
                                Value = Convert.ToDateTime(Todate).ToString("yyyy-MM-dd")
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            try
                            {
                                var retObject1 = new List<dynamic>();
                                using (var dataReader1 = cmd.ExecuteReader())
                                {
                                    while (dataReader1.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader1.GetName(iFiled1),
                                                dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                            );
                                        }
                                        retObject1.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.punchdetails = retObject1.ToArray();
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
                data.returnmsg = "Failed";
            }
            return data;
        }

        public FO_Emp_PunchDTO getBasicData(FO_Emp_PunchDTO dto)
        {

            try
            {
                //employee
                dto.employees = (
                                 from mm in _HRMSContext.MasterEmployee
                                 where mm.HRME_ActiveFlag == true
                                 orderby mm.HRME_EmployeeOrder
                                 select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                //dto.griddata = (from a in _HRMSContext.HRAluminiEmployeeBGVerificationDMO
                //                from b in _HRMSContext.MasterEmployee
                //                where a.HRME_Id == b.HRME_Id
                //                select new HRAluminiEmployeeBGVerificationDTO
                //                {
                //                    HRMEVER_Id = a.HRMEVER_Id,
                //                    HRMEVER_EmailId = a.HRMEVER_EmailId,
                //                    HRME_Id = a.HRME_Id,
                //                    HRMEVER_RequestedCompanyName = a.HRMEVER_RequestedCompanyName,
                //                    HRMEVER_ContactNo = a.HRMEVER_ContactNo,
                //                    HRMEVER_ActiveFlg = a.HRMEVER_ActiveFlg,
                //                    HRMEMNO_RequestedBy = a.HRMEMNO_RequestedBy,
                //                    HRMEVER_VerificationCode = a.HRMEVER_VerificationCode,
                //                    HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                //                    HRMEVER_GeneratedDate = a.HRMEVER_GeneratedDate,
                //                    HRMEVER_ExpiryDate = a.HRMEVER_ExpiryDate,
                //                }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
