using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;
using SendGrid;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.LeaveManagement;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace FrontOfficeHub.com.vaps.Services
{
    public class FrontOfficeMonthEndReportImpl : Interfaces.FrontOfficeMonthEndReportInterface
    {
        private static ConcurrentDictionary<string, BirthDayDTO> _login = new ConcurrentDictionary<string, BirthDayDTO>();
        public DomainModelMsSqlServerContext _db;
        public FOContext _FOContext;
        public HRMSContext _HRMSContext;

        public FrontOfficeMonthEndReportImpl(DomainModelMsSqlServerContext db, FOContext fOContext, HRMSContext HRMSContext)
        {
            _db = db;
            _FOContext = fOContext;
            _HRMSContext = HRMSContext;
        }
        public async Task<BirthDayDTO> getdata(BirthDayDTO obj)
        {
            BirthDayDTO dto = new BirthDayDTO();
            try
            {

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Page_Privilege_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = 17
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                    {
                        Value = 60050
                    });
                    cmd.Parameters.Add(new SqlParameter("@pageurl", SqlDbType.VarChar)
                    {
                        Value = "app.FrontOfficemonthreport"
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
                        dto.pagePrivilege = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                //Only it will work once page will mapp to that perticular employee
                if (dto.pagePrivilege.Length > 0)
                {



                    List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                    staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMGT_ActiveFlag == true).ToList();
                    dto.filltypes = staf_types.Distinct().ToArray();

                    //dto.fillyear = (from a in _db.HR_Master_LeaveYearDMO
                    //                where (a.MI_Id == dto.MI_Id && a.HRMLY_ActiveFlag == true)
                    //                select new HR_Master_LeaveYearDTO
                    //                {
                    //                    HRMLY_Id = a.HRMLY_Id,
                    //                    HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                    //                    HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                    //                }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                    //dto.fillmonth = (from a in _db.month
                    //                 select new EmployeeLogReportDTO
                    //                 {
                    //                     IVRM_Month_Id = a.IVRM_Month_Id,
                    //                     monthname = a.IVRM_Month_Name
                    //                 }).Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();

                    dto.getmonth = _FOContext.Month.Where(t => t.Is_Active == true).ToArray();

                    dto.getyear = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id == 17).ToArray();

                    dto.institutionlist = _FOContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public async Task<BirthDayDTO> getmonthreport(BirthDayDTO rpt)
        {
            try
            {

                var instdetails = _FOContext.Institution.Where(t => t.MI_Id == rpt.MI_Id).FirstOrDefault();
                if (instdetails != null)
                {
                    rpt.institutename = instdetails.MI_Subdomain;
                }
                var singleorall = 0;
                //var employeelist = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.MI_Id == rpt.MI_Id).Select(t => t.HRME_Id).ToArray();
                if (rpt.rdbbutton == "Month")
                {
                    string startdate = rpt.yearid + "-" + rpt.month + "-01";
                    rpt.Fromdate = Convert.ToDateTime(startdate);
                    var year = Convert.ToDateTime(startdate).Year;
                    var mon = Convert.ToDateTime(startdate).Month;
                    var startOfMonth = new DateTime(year, mon, 1);
                    var DaysInMonth = DateTime.DaysInMonth(year, mon);
                    rpt.Todate = new DateTime(year, mon, DaysInMonth);
                }
                var MonthName = _db.month.Where(t => t.IVRM_Month_Id == rpt.month).Select(t => t.IVRM_Month_Name).FirstOrDefault();
                string Year = rpt.yearid.ToString();
                rpt.salaryGenerated = _HRMSContext.HR_Employee_Salary.Where(c => c.MI_Id == rpt.MI_Id && c.HRES_Year == Year && c.HRES_Month == MonthName).Distinct().ToArray();


                if (rpt.flag == "Single")
                {
                    singleorall = 1;
                    var emp_Id = _FOContext.Staff_User_Login.Where(c => c.Id == rpt.Id).Distinct().ToList();
                    if (emp_Id.Count > 0)
                    {
                        rpt.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;
                    }
                    if (rpt.flagl == "1")
                    {
                        var getmonth = rpt.Fromdate.Value.Month;
                        var getyear = rpt.Fromdate.Value.Year;
                        rpt.yearid = getyear;
                        rpt.month = getmonth;

                        string startdate = getyear + "-" + getmonth + "-01";
                        rpt.Fromdate = Convert.ToDateTime(startdate);
                        rpt.Todate = Convert.ToDateTime(rpt.Todate).AddDays(-1);
                    }
                    else if (rpt.flagl == "0")
                    {
                        // var getmonth = (rpt.Fromdate.Value.Month) - 1;
                        var getmonth = DateTime.Now.AddMonths(-1).Month;


                        if (getmonth >= 1)
                        {
                            getmonth = Convert.ToInt32(getmonth);
                            var getyear = rpt.Fromdate.Value.Year;
                            rpt.yearid = getyear;
                            rpt.month = getmonth;
                            var today = DateTime.Today;
                            var month = new DateTime(today.Year, today.Month, 1);
                            string startdate = getyear + "-" + getmonth + "-01";
                            //rpt.Fromdate = Convert.ToDateTime(startdate);
                            rpt.Fromdate = month.AddMonths(-1);
                            var mon = Convert.ToDateTime(rpt.Fromdate).Month;
                            var startOfMonth = new DateTime(getyear, getmonth, 1);
                            var DaysInMonth = DateTime.DaysInMonth(getyear, mon);
                            //rpt.Todate = new DateTime(getyear, mon, DaysInMonth);
                            rpt.Todate = month.AddDays(-1);

                            //var Fromdate = month.AddMonths(-1);
                            //var Todate = month.AddDays(-1);
                        }
                        else
                        {
                            var getyear = Convert.ToInt32((rpt.Fromdate.Value.Year)) - 1;
                            string startdate = getyear + "-" + 12 + "-01";
                            rpt.yearid = getyear;
                            rpt.month = getmonth;
                            var today = DateTime.Today;
                            var month = new DateTime(today.Year, today.Month, 1);
                            // rpt.Fromdate = Convert.ToDateTime(startdate);
                            rpt.Fromdate = month.AddMonths(-1);
                            var mon = Convert.ToDateTime(rpt.Fromdate).Month;
                            var startOfMonth = new DateTime(getyear, getmonth, 1);
                            var DaysInMonth = DateTime.DaysInMonth(getyear, mon);
                            //rpt.Todate = new datetime(getyear, mon, daysinmonth);
                            rpt.Todate = month.AddDays(-1);


                        }
                    }

                    rpt.SelectedActivityDetails1_stf = (from a in _FOContext.HR_Master_Employee_DMO
                                                        from b in _FOContext.HR_Master_Designation_DMO
                                                        where (a.MI_Id == rpt.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == rpt.HRME_Id && a.HRME_DOJ <= rpt.Todate && ((a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true) || (a.HRME_DOL.Value.Month == rpt.Todate.Value.Month && a.HRME_DOL.Value.Year == rpt.Todate.Value.Year)))
                                                        select new MasterEmployeeDTO
                                                        {
                                                            HRME_Id = a.HRME_Id,
                                                            HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                                            HRME_PerStreet = b.HRMDES_DesignationName,
                                                            HRME_DOJ = a.HRME_DOJ,
                                                            HRME_ExcPunch = a.HRME_ExcPunch,
                                                            HRME_ExcDR = a.HRME_ExcDR,
                                                            HRME_LeftFlag = a.HRME_LeftFlag,
                                                            HRME_DOL = a.HRME_DOL,
                                                            HRME_Relaxtime = a.HRME_Relaxtime
                                                        }).OrderBy(t => t.HRME_EmployeeFirstName).ToArray();
                }
                else
                {
                    singleorall = 0;
                    rpt.SelectedActivityDetails1_stf = (from a in _FOContext.HR_Master_Employee_DMO
                                                        from b in _FOContext.HR_Master_Designation_DMO
                                                        where (a.MI_Id == rpt.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_DOJ <= rpt.Todate && ((a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true) || (a.HRME_DOL.Value.Month == rpt.Todate.Value.Month && a.HRME_DOL.Value.Year == rpt.Todate.Value.Year)))
                                                        select new MasterEmployeeDTO
                                                        {
                                                            HRME_Id = a.HRME_Id,
                                                            HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                                            HRME_PerStreet = b.HRMDES_DesignationName,
                                                            HRME_DOJ = a.HRME_DOJ,
                                                            HRME_ExcPunch = a.HRME_ExcPunch,
                                                            HRME_ExcDR = a.HRME_ExcDR,
                                                            HRME_LeftFlag = a.HRME_LeftFlag,
                                                            HRME_DOL = a.HRME_DOL,
                                                            HRME_Relaxtime = a.HRME_Relaxtime
                                                        }).OrderBy(t => t.HRME_EmployeeFirstName).ToArray();
                }


                string hrmeids = "0";
                string LIEOhrmeids = "0";

                for (int i = 0; i < rpt.SelectedActivityDetails1_stf.Length; i++)
                {
                    if (i == 0) { hrmeids = rpt.SelectedActivityDetails1_stf[i].HRME_Id.ToString(); }
                    else { hrmeids = hrmeids + "," + rpt.SelectedActivityDetails1_stf[i].HRME_Id.ToString(); }
                    if (rpt.SelectedActivityDetails1_stf[i].HRME_PerStreet.ToUpper() != "DRIVER")
                    {
                        if (i == 0) { LIEOhrmeids = rpt.SelectedActivityDetails1_stf[i].HRME_Id.ToString(); }
                        else { LIEOhrmeids = LIEOhrmeids + "," + rpt.SelectedActivityDetails1_stf[i].HRME_Id.ToString(); }
                    }
                }

                long hrmly_id = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id == rpt.MI_Id && Convert.ToDateTime(t.HRMLY_FromDate) <= rpt.Fromdate && Convert.ToDateTime(t.HRMLY_ToDate) >= rpt.Todate && t.HRMLY_ActiveFlag == true).Select(t => t.HRMLY_Id).FirstOrDefault();

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_AllEmpsAttendanceMissingPunchUpdate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                    {
                        Value = singleorall
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.BigInt)
                    {
                        Value = rpt.HRME_Id
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
                        rpt.workinghourDetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Punch_Dates"; //"Fo_Emp_Log_Report"
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        //rpt.HRELT_FromDate = Convert.ToDateTime(rpt.Fromdate);
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
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
                        rpt.punchdates = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fo_Emp_Log_InDetails_Report"; //"Fo_Emp_Log_Report"
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                    {
                        Value = ""
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        //rpt.HRELT_FromDate = Convert.ToDateTime(rpt.Fromdate);
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar)
                    {
                        Value = LIEOhrmeids
                    });
                    cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar)
                    {
                        Value = "LIEO"
                    });
                    cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                    {
                        Value = singleorall
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
                        rpt.staffDetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmpDrReportsNotSentDates";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                    {
                        Value = hrmly_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
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
                        rpt.Dailyreptdetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DR_Reject_with_Punch_Based";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    //cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                    //{
                    //    Value = rpt.HRME_Id
                    //});
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = "Date"
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.Bit)
                    {
                        Value = singleorall
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
                        rpt.rejectedDrList = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

               // New CompOff Adjest
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_CompOff_Adjest_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.VarChar)
                    {
                        Value = hrmly_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
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
                        rpt.compOffAdjest = retObject1.ToArray(); //concallextention
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //con call lop A
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_CONCALL_LopDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "withlop"
                    });

                    cmd.Parameters.Add(new SqlParameter("@Empflg", SqlDbType.Bit)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = 0
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
                        rpt.lunchdata = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }               
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_CONCALL_LopDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "withlopemp"
                    });

                    cmd.Parameters.Add(new SqlParameter("@Empflg", SqlDbType.Bit)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = 0
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
                        rpt.employeeTypeList = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_CONCALL_LopDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "nolop"
                    });

                    cmd.Parameters.Add(new SqlParameter("@Empflg", SqlDbType.Bit)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = 0
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
                        rpt.sms_mail_count = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //con call end

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EmployeeDRLOP_Calculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@STARTDATE", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@ENDDATE", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
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
                        rpt.drrjectedreport = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Emp_PunchMissing_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.Id
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
                        rpt.punchmissingapproval = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Evaluation_Rating";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.VarChar)
                    {
                        Value = rpt.yearid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.BigInt)
                    {
                        Value = rpt.month
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = "R"
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
                        rpt.employeeRetingdata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                //    rpt.lunchdata = _HRMSContext.Lop_DetailsDMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRALD_Date >= rpt.Fromdate && t.HRALD_Date <= rpt.Todate && t.HRALD_Activeflag == true).ToArray();
                rpt.Halfdaydrdata = (from t in _FOContext.ISM_DailyReportGenerationDMO
                                     where (t.MI_Id == rpt.MI_Id && t.ISMDRPT_Date >= rpt.Fromdate && t.ISMDRPT_Date <= rpt.Todate && t.ISMDRPT_ActiveFlg == true && t.ISMDRPT_HalfDayFlag == true)
                                     select new BirthDayDTO
                                     {
                                         HRME_Id = t.HRME_Id,
                                         ISMDRPT_Date = t.ISMDRPT_Date
                                     }).Distinct().ToArray();

                rpt.Blockeddrdata = (from t in _FOContext.ISM_DailyReportGenerationDMO
                                     where (t.MI_Id == rpt.MI_Id && t.ISMDRPT_Date >= rpt.Fromdate && t.ISMDRPT_Date <= rpt.Todate && t.ISMDRPT_ActiveFlg == true && t.ISMDRPT_BlockedFlag == true)
                                     select new BirthDayDTO
                                     {
                                         HRME_Id = t.HRME_Id,
                                         ISMDRPT_Date = t.ISMDRPT_Date
                                     }).OrderBy(t => t.ISMDRPT_Date).Distinct().ToArray();

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_EmpUnAuthLeaveDates";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                    {
                        Value = hrmly_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                    {
                        Value = singleorall
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.BigInt)
                    {
                        Value = rpt.HRME_Id
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
                        rpt.staffleaveDetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_ReconsiderLeave_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt)
                    {
                        Value = rpt.yearid
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt)
                    {
                        Value = rpt.month
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = rpt.rdbbutton
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
                        rpt.get_mi_list1 = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_EmpAuthLeaveDates";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                    {
                        Value = hrmly_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                    {
                        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                    {
                        Value = singleorall
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.BigInt)
                    {
                        Value = rpt.HRME_Id
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
                        rpt.LeaveDetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Emp_CompoffAdjested_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = rpt.Fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime)
                    {
                        Value = rpt.Todate
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
                        rpt.Compoffdetails = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Auto_CompOff_Adjest_eligibility";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 800000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt)
                    {
                        Value = rpt.yearid
                    });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt)
                    {
                        Value = rpt.month
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.HRME_Id
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
                        rpt.compoffadjtelig = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
               
                //DR Not Approved Count
                rpt.drnotapprovedcount = _db.ISM_DailyReportGenerationDMO.Where(t => t.HRME_Id == rpt.SelectedActivityDetails1_stf[0].HRME_Id && t.ISMDRPT_Date >= rpt.Fromdate && t.ISMDRPT_Date <= rpt.Todate && t.ISMDRPT_ApprovedFlg == null).Count();

                rpt.holidaylist = (from a in _FOContext.holidayWorkingDayType
                                   from b in _FOContext.holidaydate
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == rpt.MI_Id && b.HRMLY_Id == hrmly_id && a.FOHTWD_HolidayFlag == true && a.FOHWDT_ActiveFlg == true && a.FOHWDT_Id == b.FOHWDT_Id && b.FOMHWDD_FromDate >= rpt.Fromdate && b.FOMHWDD_FromDate <= rpt.Todate)
                                   select new BirthDayDTO
                                   {
                                       FOMHWDD_FromDate = b.FOMHWDD_FromDate,
                                       FOMHWDD_ToDate = b.FOMHWDD_ToDate
                                   }).ToArray();

                List<long> leaveids = new List<long>();
                leaveids = _FOContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRML_LeaveCreditFlg == true).Select(t => t.HRML_Id).ToList();
                string leaveidstring = String.Join(",", leaveids);

                rpt.monthname = _FOContext.Month.Where(t => t.IVRM_Month_Id == rpt.month).Select(t => t.IVRM_Month_Name).FirstOrDefault();
                //List<HR_Employee_Salary_DetailsDTO> currentsalary = new List<HR_Employee_Salary_DetailsDTO>();
                rpt.currentsalary = (from HRES in _HRMSContext.HR_Employee_Salary
                                     from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                     from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                     where (HRESD.HRES_Id == HRES.HRES_Id && HRESD.HRMED_Id == HRMED.HRMED_Id && HRES.HRES_Year == rpt.yearid.ToString() && HRES.HRES_Month == rpt.monthname && hrmeids.Contains(HRES.HRME_Id.ToString()) && HRMED.HRMED_EarnDedFlag == "Earning" && HRMED.HRMED_ActiveFlag == true)
                                     select new HR_Employee_Salary_DetailsDTO
                                     {
                                         HRME_Id = HRES.HRME_Id,
                                         HRESD_Id = HRESD.HRESD_Id,
                                         HRES_Id = HRES.HRES_Id,
                                         HRMED_Id = HRESD.HRMED_Id,
                                         HRMED_Name = HRMED.HRMED_Name,
                                         HRESD_Amount = HRESD.HRESD_Amount,
                                         HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                     }).ToArray();

                //rpt.HRCOMMENTS = _FOContext.VMSConsolidateReportTempDMO.Where(t => t.VMSCRT_Year == rpt.yearid.ToString() && t.VMSCRT_Month == rpt.month.ToString() && employeelist.Contains(t.HRME_Id)).ToArray();
                var duplicate = (from a in _FOContext.VMSConsolidateReportTempDMO
                                 from b in _FOContext.HR_Master_Employee_DMO
                                 from c in _FOContext.Multiple_Email_DMO
                                 where (a.VMSCRT_Year == rpt.yearid.ToString() && a.VMSCRT_Month == rpt.month.ToString() && a.HRME_Id == b.HRME_Id && b.MI_Id == rpt.MI_Id && b.HRME_Id == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                                 select new VMSConsolidateReportTempDTO
                                 {
                                     VMSCRT_Id = a.VMSCRT_Id,
                                     VMSCRT_Year = a.VMSCRT_Year,
                                     VMSCRT_Month = a.VMSCRT_Month,
                                     VMSCRT_EMPCode = a.VMSCRT_EMPCode,
                                     VMSCRT_EName = a.VMSCRT_EName,
                                     VMSCRT_Earlyout = a.VMSCRT_Earlyout,
                                     VMSCRT_DailyReportnotsend = a.VMSCRT_DailyReportnotsend,
                                     VMSCRT_Latecoming = a.VMSCRT_Latecoming,
                                     VMSCRT_unauthorizedleave = a.VMSCRT_unauthorizedleave,
                                     VMSCRT_DesigCode = a.VMSCRT_DesigCode,
                                     VMSCRT_DRDetails = a.VMSCRT_DRDetails,
                                     VMSCRT_ULDetails = a.VMSCRT_ULDetails,
                                     VMSCRT_EODetails = a.VMSCRT_EODetails,
                                     VMSCRT_LCDetails = a.VMSCRT_LCDetails,
                                     VMSCRT_EOU180 = a.VMSCRT_EOU180,
                                     VMSCRT_LCU180 = a.VMSCRT_LCU180,
                                     VMSCRT_HREO = a.VMSCRT_HREO,
                                     VMSCRT_HRLC = a.VMSCRT_HRLC,
                                     VMSCRT_HRUL = a.VMSCRT_HRUL,
                                     VMSCRT_HRDR = a.VMSCRT_HRDR,
                                     VMSCRT_HRTLOP = a.VMSCRT_HRTLOP,
                                     VMSCRT_Remarks = a.VMSCRT_Remarks,
                                     VMSCRT_DOJ = b.HRME_DOJ.ToString(),
                                     HRME_Id = a.HRME_Id,
                                     VMSCRT_TLOP = a.VMSCRT_TLOP,
                                     emailid = c.HRMEM_EmailId
                                 }).ToArray();
                rpt.HRCOMMENTS = duplicate.ToArray();

                rpt.monthendreportclientwise = _FOContext.ISM_Client_MERDMO.Where(a => a.Month_Id == rpt.month && a.ISMCLMER_Year == rpt.yearid && a.ISMCLMER_ActiveFlag == true).ToArray();


                rpt.staffList = (from a in _FOContext.ISM_Master_Client_IEMapping_DMO
                                 from b in _FOContext.HR_Master_Employee_DMO
                                 where (a.ISMCIM_IEList == b.HRME_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && a.ISMMCLTIE_ActiveFlag == true)
                                 select b).ToArray();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                try
                {
                    var typ = rpt.flag;
                    if (typ != "Single")
                    {
                        typ = "All";
                    }
                    _FOContext.Database.ExecuteSqlCommand("AttendanceLogInsert @p0,@p1,@p2,@p3,@p4,@p5", rpt.MI_Id, rpt.Id, typ, indianTime, rpt.Fromdate, rpt.Todate);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.sms_text = e.Message;
                Console.WriteLine(rpt.sms_text);
            }
            return rpt;
        }

        public async Task<BirthDayDTO> getlunchreport(BirthDayDTO rpt)
        {
            try
            {
                var employeelist = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.MI_Id == rpt.MI_Id && rpt.employeetpList.Contains(t.HRMGT_Id.ToString()) && rpt.employeeDeptList.Contains(t.HRMD_Id.ToString()) && rpt.employeeDesgList.Contains(t.HRMDES_Id.ToString())).Select(t => t.HRME_Id).ToArray();

                string hrmeids = string.Join(",", employeelist);
                string startdate = rpt.yearid + "-" + rpt.month + "-1";
                int monthend = _FOContext.Month.Where(t => t.IVRM_Month_Id == rpt.month).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault();
                string enddate = rpt.yearid + "-" + rpt.month + "-" + monthend;
                rpt.Fromdate = Convert.ToDateTime(startdate);
                rpt.Todate = Convert.ToDateTime(enddate);

                rpt.columnnames = (from a in Enumerable.Range(0, (Convert.ToDateTime(enddate) - Convert.ToDateTime(startdate)).Days + 1)
                                   let columndate = Convert.ToDateTime(startdate).AddDays(a)
                                   select columndate).Distinct().ToArray();

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmpLunchDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.NVarChar)
                    {
                        Value = hrmeids
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                    {
                        Value = enddate
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
                        rpt.lunchdata = retObject1.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }

        public BirthDayDTO savereport(BirthDayDTO rpt)
        {
            try
            {
                if (rpt.ConsolidateReportTempDTO.Length > 0)
                {
                    if (rpt.Fromdate != null)
                    {
                        rpt.yearid = rpt.Fromdate.Value.Year;
                        rpt.months = rpt.Fromdate.Value.Month.ToString();
                    }

                    var datalwp = _FOContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRML_LeaveType == "LWP").ToList();
                    long leaveyearid = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRMLY_LeaveYear == rpt.yearid.ToString() && t.HRMLY_ActiveFlag == true).Select(t => t.HRMLY_Id).FirstOrDefault();
                    long compoffid = _FOContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRML_LeaveCode == "COMPOFF").Select(t => t.HRML_Id).FirstOrDefault();

                    for (int i = 0; i < rpt.ConsolidateReportTempDTO.Length; i++)
                    {
                        var duplicate = _FOContext.VMSConsolidateReportTempDMO.Where(t => t.VMSCRT_Year == rpt.yearid.ToString() && t.VMSCRT_Month == rpt.months && t.HRME_Id == rpt.ConsolidateReportTempDTO[i].HRME_Id).ToList();

                        if (duplicate.Count > 0)
                        {
                            //var duplicatedd = _FOContext.VMSConsolidateReportTempDMO.Single(t => t.VMSCRT_Year == rpt.yearid.ToString() && t.VMSCRT_Month == rpt.months && t.HRME_Id == rpt.ConsolidateReportTempDTO[i].HRME_Id);
                            var duplicatedd = _FOContext.VMSConsolidateReportTempDMO.Where(t => t.VMSCRT_Year == rpt.yearid.ToString() && t.VMSCRT_Month == rpt.months && t.HRME_Id == rpt.ConsolidateReportTempDTO[i].HRME_Id).FirstOrDefault();
                            duplicatedd.VMSCRT_Year = rpt.yearid.ToString();
                            duplicatedd.VMSCRT_Month = rpt.months;
                            duplicatedd.VMSCRT_EMPCode = rpt.ConsolidateReportTempDTO[i].ecode;
                            duplicatedd.VMSCRT_EName = rpt.ConsolidateReportTempDTO[i].ename;
                            duplicatedd.VMSCRT_Earlyout = rpt.ConsolidateReportTempDTO[i].empdate[0].earlyout;
                            duplicatedd.VMSCRT_DailyReportnotsend = rpt.ConsolidateReportTempDTO[i].drcount;
                            duplicatedd.VMSCRT_Latecoming = rpt.ConsolidateReportTempDTO[i].empdate[0].latecoming;
                            duplicatedd.VMSCRT_unauthorizedleave = rpt.ConsolidateReportTempDTO[i].leavecount;
                            duplicatedd.VMSCRT_DesigCode = rpt.ConsolidateReportTempDTO[i].desgname;
                            duplicatedd.VMSCRT_TLOP = rpt.ConsolidateReportTempDTO[i].totlop;
                            duplicatedd.VMSCRT_DRDetails = rpt.ConsolidateReportTempDTO[i].drdates;
                            duplicatedd.VMSCRT_ULDetails = rpt.ConsolidateReportTempDTO[i].leavedates;
                            duplicatedd.VMSCRT_EODetails = rpt.ConsolidateReportTempDTO[i].empdate[0].earlydates;
                            duplicatedd.VMSCRT_LCDetails = rpt.ConsolidateReportTempDTO[i].empdate[0].latedates;
                            duplicatedd.VMSCRT_EOU180 = rpt.ConsolidateReportTempDTO[i].empdate[0].earlydateafter;
                            duplicatedd.VMSCRT_LCU180 = rpt.ConsolidateReportTempDTO[i].empdate[0].latedateafter;
                            duplicatedd.VMSCRT_HREO = rpt.ConsolidateReportTempDTO[i].HR_EO;
                            duplicatedd.VMSCRT_HRLC = rpt.ConsolidateReportTempDTO[i].HR_LC;
                            duplicatedd.VMSCRT_HRUL = rpt.ConsolidateReportTempDTO[i].HR_UL;
                            duplicatedd.VMSCRT_HRDR = rpt.ConsolidateReportTempDTO[i].HR_DR;
                            duplicatedd.VMSCRT_HRTLOP = rpt.ConsolidateReportTempDTO[i].HR_LOP;
                            duplicatedd.VMSCRT_Remarks = rpt.ConsolidateReportTempDTO[i].HR_REM;
                            duplicatedd.VMSCRT_HDailyReport = rpt.ConsolidateReportTempDTO[i].halfdrcount;
                            duplicatedd.VMSCRT_HDRDetails = rpt.ConsolidateReportTempDTO[i].halfdrdates;
                            duplicatedd.VMSCRT_HRHDR = rpt.ConsolidateReportTempDTO[i].HR_HDR;
                            duplicatedd.VMSCRT_BDailyReport = rpt.ConsolidateReportTempDTO[i].blkdrcount;
                            duplicatedd.VMSCRT_BDRDetails = rpt.ConsolidateReportTempDTO[i].blkdrdates;
                            duplicatedd.VMSCRT_HRBDR = rpt.ConsolidateReportTempDTO[i].HR_BDR;
                            duplicatedd.VMSCRT_CompoffAdjustCount = rpt.ConsolidateReportTempDTO[i].nooffcompoffadjested;
                            _FOContext.Update(duplicatedd);
                            _FOContext.SaveChanges();

                            //Leave Transactions
                            if (duplicatedd.VMSCRT_Id > 0 && duplicatedd.VMSCRT_HRTLOP > 0)
                            {
                                var leaveduplicate = (from a in _FOContext.HR_Emp_Leave_Trans_DMO
                                                      from b in _FOContext.HR_Master_Leave_DMO
                                                      where (a.HRELT_LeaveId == b.HRML_Id && a.MI_Id == b.MI_Id && b.MI_Id == rpt.MI_Id && a.HRELT_FromDate.Year == rpt.yearid && a.HRELT_FromDate.Month == Convert.ToInt32(rpt.months) && a.HRME_Id == rpt.ConsolidateReportTempDTO[i].HRME_Id && b.HRML_LeaveType == "LWP" && a.HRELT_ActiveFlag == true)
                                                      select new VMSConsolidateReportTempDTO
                                                      {
                                                          HRELT_Id = a.HRELT_Id,
                                                          HRELT_TotDays = a.HRELT_TotDays
                                                      }).ToArray();

                                if (leaveduplicate.Length > 0)
                                {
                                    for (int count = 0; count < leaveduplicate.Length; count++)
                                    {
                                        HR_Emp_Leave_Trans_DMO obj = new HR_Emp_Leave_Trans_DMO();
                                        obj = _FOContext.HR_Emp_Leave_Trans_DMO.Where(t => t.HRELT_Id == leaveduplicate[count].HRELT_Id).FirstOrDefault();
                                        obj.HRELT_ActiveFlag = false;
                                        _FOContext.Update(obj);
                                        _FOContext.SaveChanges();
                                    }
                                }

                                string startdate = rpt.yearid + "-" + rpt.months + "-1";
                                if ((Convert.ToDateTime(rpt.ConsolidateReportTempDTO[i].VMSCRT_DOJ).Month == Convert.ToDateTime(startdate).Month) && (Convert.ToDateTime(rpt.ConsolidateReportTempDTO[i].VMSCRT_DOJ).Year == Convert.ToDateTime(startdate).Year))
                                {
                                    startdate = rpt.ConsolidateReportTempDTO[i].VMSCRT_DOJ;
                                }

                                double enddate = duplicatedd.VMSCRT_HRTLOP;
                                if (duplicatedd.VMSCRT_HRTLOP.ToString().IndexOf(".") > 0)
                                {
                                    enddate = duplicatedd.VMSCRT_HRTLOP + 0.5;
                                }
                                enddate = (Convert.ToDateTime(startdate).Day + enddate) - 1;

                                string lastdate = rpt.yearid + "-" + rpt.months + "-" + enddate;
                                rpt.HRELT_FromDate = Convert.ToDateTime(startdate);
                                if (enddate > DateTime.DaysInMonth(Convert.ToInt32(rpt.yearid), Convert.ToInt32(rpt.months)))
                                { lastdate = rpt.yearid + "-" + rpt.months + "-" + DateTime.DaysInMonth(Convert.ToInt32(rpt.yearid), Convert.ToInt32(rpt.months)); }
                                rpt.HRELT_ToDate = Convert.ToDateTime(lastdate);
                                HR_Emp_Leave_Trans_DMO empltrans = new HR_Emp_Leave_Trans_DMO();
                                empltrans.MI_Id = rpt.MI_Id;
                                empltrans.HRME_Id = rpt.ConsolidateReportTempDTO[i].HRME_Id;
                                empltrans.HRMLY_Id = leaveyearid;
                                empltrans.HRELT_LeaveId = datalwp.FirstOrDefault().HRML_Id;
                                empltrans.HRELT_FromDate = rpt.HRELT_FromDate;
                                empltrans.HRELT_ToDate = rpt.HRELT_ToDate;
                                empltrans.HRELT_TotDays = Convert.ToDecimal(duplicatedd.VMSCRT_HRTLOP);
                                empltrans.HRELT_Reportingdate = rpt.HRELT_ToDate.AddDays(1);
                                empltrans.HRELT_LeaveReason = "LWP Cumulative Report";
                                empltrans.HRELT_Status = "Approved";
                                empltrans.CreatedDate = DateTime.Now;
                                empltrans.UpdatedDate = DateTime.Now;
                                empltrans.HRELT_ActiveFlag = true;
                                _FOContext.Add(empltrans);
                                var flag = _FOContext.SaveChanges();
                                if (flag > 0)
                                {
                                    HR_Emp_Leave_Trans_Details_DMO objpge = new HR_Emp_Leave_Trans_Details_DMO();
                                    objpge.HRELT_Id = empltrans.HRELT_Id;
                                    objpge.HRML_Id = datalwp.FirstOrDefault().HRML_Id;
                                    objpge.HRME_Id = rpt.ConsolidateReportTempDTO[i].HRME_Id;
                                    objpge.MI_Id = rpt.MI_Id;
                                    objpge.HRELTD_FromDate = rpt.HRELT_FromDate;
                                    objpge.HRELTD_ToDate = rpt.HRELT_ToDate;
                                    objpge.HRELTD_TotDays = Convert.ToDecimal(duplicatedd.VMSCRT_HRTLOP);
                                    objpge.HRELTD_LWPFlag = true;
                                    objpge.CreatedDate = DateTime.Now;
                                    objpge.UpdatedDate = DateTime.Now;
                                    _FOContext.Add(objpge);
                                    _FOContext.SaveChanges();
                                }

                            }
                            else
                            {
                                var leaveduplicate = (from a in _FOContext.HR_Emp_Leave_Trans_DMO
                                                      from b in _FOContext.HR_Master_Leave_DMO
                                                      where (a.HRELT_LeaveId == b.HRML_Id && a.MI_Id == b.MI_Id && b.MI_Id == rpt.MI_Id && a.HRELT_FromDate.Year == rpt.yearid && a.HRELT_FromDate.Month == Convert.ToInt32(rpt.months) && a.HRME_Id == rpt.ConsolidateReportTempDTO[i].HRME_Id && b.HRML_LeaveType == "LWP" && a.HRELT_ActiveFlag == true)
                                                      select new VMSConsolidateReportTempDTO
                                                      {
                                                          HRELT_Id = a.HRELT_Id,
                                                          HRELT_TotDays = a.HRELT_TotDays
                                                      }).ToArray();

                                if (leaveduplicate.Length > 0)
                                {
                                    for (int count = 0; count < leaveduplicate.Length; count++)
                                    {
                                        HR_Emp_Leave_Trans_DMO obj = new HR_Emp_Leave_Trans_DMO();
                                        obj = _FOContext.HR_Emp_Leave_Trans_DMO.Where(t => t.HRELT_Id == leaveduplicate[count].HRELT_Id).FirstOrDefault();
                                        obj.HRELT_ActiveFlag = false;
                                        _FOContext.Update(obj);
                                        _FOContext.SaveChanges();
                                    }
                                }
                            }
                            //Leave Transactions
                        }
                        else
                        {

                            VMSConsolidateReportTempDMO objdmo = new VMSConsolidateReportTempDMO();
                            objdmo.HRME_Id = rpt.ConsolidateReportTempDTO[i].HRME_Id;
                            objdmo.VMSCRT_Year = rpt.yearid.ToString();
                            objdmo.VMSCRT_Month = rpt.months;
                            objdmo.VMSCRT_EMPCode = rpt.ConsolidateReportTempDTO[i].ecode;
                            objdmo.VMSCRT_EName = rpt.ConsolidateReportTempDTO[i].ename;
                            objdmo.VMSCRT_Earlyout = rpt.ConsolidateReportTempDTO[i].empdate[0].earlyout;
                            objdmo.VMSCRT_DailyReportnotsend = rpt.ConsolidateReportTempDTO[i].drcount;
                            objdmo.VMSCRT_Latecoming = rpt.ConsolidateReportTempDTO[i].empdate[0].latecoming;
                            objdmo.VMSCRT_unauthorizedleave = rpt.ConsolidateReportTempDTO[i].leavecount;
                            objdmo.VMSCRT_DesigCode = rpt.ConsolidateReportTempDTO[i].desgname;
                            objdmo.VMSCRT_TLOP = rpt.ConsolidateReportTempDTO[i].totlop;
                            objdmo.VMSCRT_DRDetails = rpt.ConsolidateReportTempDTO[i].drdates;
                            objdmo.VMSCRT_ULDetails = rpt.ConsolidateReportTempDTO[i].leavedates;
                            objdmo.VMSCRT_EODetails = rpt.ConsolidateReportTempDTO[i].empdate[0].earlydates;
                            objdmo.VMSCRT_LCDetails = rpt.ConsolidateReportTempDTO[i].empdate[0].latedates;
                            objdmo.VMSCRT_EOU180 = rpt.ConsolidateReportTempDTO[i].empdate[0].earlydateafter;
                            objdmo.VMSCRT_LCU180 = rpt.ConsolidateReportTempDTO[i].empdate[0].latedateafter;
                            objdmo.VMSCRT_HREO = rpt.ConsolidateReportTempDTO[i].HR_EO;
                            objdmo.VMSCRT_HRLC = rpt.ConsolidateReportTempDTO[i].HR_LC;
                            objdmo.VMSCRT_HRUL = rpt.ConsolidateReportTempDTO[i].HR_UL;
                            objdmo.VMSCRT_HRDR = rpt.ConsolidateReportTempDTO[i].HR_DR;
                            objdmo.VMSCRT_HRTLOP = rpt.ConsolidateReportTempDTO[i].HR_LOP;
                            objdmo.VMSCRT_Remarks = rpt.ConsolidateReportTempDTO[i].HR_REM;

                            objdmo.VMSCRT_HDailyReport = rpt.ConsolidateReportTempDTO[i].halfdrcount;
                            objdmo.VMSCRT_HDRDetails = rpt.ConsolidateReportTempDTO[i].halfdrdates;
                            objdmo.VMSCRT_HRHDR = rpt.ConsolidateReportTempDTO[i].HR_HDR;

                            objdmo.VMSCRT_BDailyReport = rpt.ConsolidateReportTempDTO[i].blkdrcount;
                            objdmo.VMSCRT_BDRDetails = rpt.ConsolidateReportTempDTO[i].blkdrdates;
                            objdmo.VMSCRT_HRBDR = rpt.ConsolidateReportTempDTO[i].HR_BDR;
                            objdmo.VMSCRT_CompoffAdjustCount = rpt.ConsolidateReportTempDTO[i].nooffcompoffadjested;

                            _FOContext.Add(objdmo);
                            _FOContext.SaveChanges();

                            //Leave Transactions
                            if (objdmo.VMSCRT_Id > 0 && objdmo.VMSCRT_HRTLOP > 0)
                            {
                                var leaveduplicate = (from a in _FOContext.HR_Emp_Leave_Trans_DMO
                                                      from b in _FOContext.HR_Master_Leave_DMO
                                                      where (a.HRELT_LeaveId == b.HRML_Id && a.MI_Id == b.MI_Id && b.MI_Id == rpt.MI_Id && a.HRELT_FromDate.Year == rpt.yearid && a.HRELT_FromDate.Month == Convert.ToInt32(rpt.months) && a.HRME_Id == rpt.ConsolidateReportTempDTO[i].HRME_Id && b.HRML_LeaveType == "LWP" && a.HRELT_ActiveFlag == true)
                                                      select new VMSConsolidateReportTempDTO
                                                      {
                                                          HRELT_Id = a.HRELT_Id,
                                                          HRELT_TotDays = a.HRELT_TotDays
                                                      }).ToArray();

                                if (leaveduplicate.Length > 0)
                                {
                                    for (int count = 0; count < leaveduplicate.Length; count++)
                                    {
                                        HR_Emp_Leave_Trans_DMO obj = new HR_Emp_Leave_Trans_DMO();
                                        obj = _FOContext.HR_Emp_Leave_Trans_DMO.Where(t => t.HRELT_Id == leaveduplicate[count].HRELT_Id).FirstOrDefault();
                                        obj.HRELT_ActiveFlag = false;
                                        _FOContext.Update(obj);
                                        _FOContext.SaveChanges();
                                    }
                                }

                                string startdate = rpt.yearid + "-" + rpt.months + "-1";
                                if ((Convert.ToDateTime(rpt.ConsolidateReportTempDTO[i].VMSCRT_DOJ).Month == Convert.ToDateTime(startdate).Month) && (Convert.ToDateTime(rpt.ConsolidateReportTempDTO[i].VMSCRT_DOJ).Year == Convert.ToDateTime(startdate).Year))
                                {
                                    startdate = rpt.ConsolidateReportTempDTO[i].VMSCRT_DOJ;
                                }

                                double enddate = objdmo.VMSCRT_HRTLOP;
                                if (objdmo.VMSCRT_HRTLOP.ToString().IndexOf(".") > 0)
                                {
                                    enddate = objdmo.VMSCRT_HRTLOP + 0.5;
                                }
                                enddate = (Convert.ToDateTime(startdate).Day + enddate) - 1;

                                string lastdate = rpt.yearid + "-" + rpt.months + "-" + enddate;
                                rpt.HRELT_FromDate = Convert.ToDateTime(startdate);

                                if (enddate > DateTime.DaysInMonth(Convert.ToInt32(rpt.yearid), Convert.ToInt32(rpt.months)))
                                { lastdate = rpt.yearid + "-" + rpt.months + "-" + DateTime.DaysInMonth(Convert.ToInt32(rpt.yearid), Convert.ToInt32(rpt.months)); }

                                rpt.HRELT_ToDate = Convert.ToDateTime(lastdate);

                                HR_Emp_Leave_Trans_DMO empltrans = new HR_Emp_Leave_Trans_DMO();
                                empltrans.MI_Id = rpt.MI_Id;
                                empltrans.HRME_Id = rpt.ConsolidateReportTempDTO[i].HRME_Id;
                                empltrans.HRMLY_Id = leaveyearid;
                                empltrans.HRELT_LeaveId = datalwp.FirstOrDefault().HRML_Id;
                                empltrans.HRELT_FromDate = rpt.HRELT_FromDate;
                                empltrans.HRELT_ToDate = rpt.HRELT_ToDate;
                                empltrans.HRELT_TotDays = Convert.ToDecimal(objdmo.VMSCRT_HRTLOP);
                                empltrans.HRELT_Reportingdate = rpt.HRELT_ToDate.AddDays(1);
                                empltrans.HRELT_LeaveReason = "LWP Cumulative Report";
                                empltrans.HRELT_Status = "Approved";
                                empltrans.CreatedDate = DateTime.Now;
                                empltrans.UpdatedDate = DateTime.Now;
                                empltrans.HRELT_ActiveFlag = true;
                                _FOContext.Add(empltrans);
                                var flag = _FOContext.SaveChanges();
                                if (flag > 0)
                                {
                                    HR_Emp_Leave_Trans_Details_DMO objpge = new HR_Emp_Leave_Trans_Details_DMO();
                                    objpge.HRELT_Id = empltrans.HRELT_Id;
                                    objpge.HRML_Id = datalwp.FirstOrDefault().HRML_Id;
                                    objpge.HRME_Id = rpt.ConsolidateReportTempDTO[i].HRME_Id;
                                    objpge.MI_Id = rpt.MI_Id;
                                    objpge.HRELTD_FromDate = rpt.HRELT_FromDate;
                                    objpge.HRELTD_ToDate = rpt.HRELT_ToDate;
                                    objpge.HRELTD_TotDays = Convert.ToDecimal(objdmo.VMSCRT_HRTLOP);
                                    objpge.HRELTD_LWPFlag = true;
                                    objpge.CreatedDate = DateTime.Now;
                                    objpge.UpdatedDate = DateTime.Now;
                                    _FOContext.Add(objpge);
                                    _FOContext.SaveChanges();
                                }
                            }
                            //Leave Transactions
                        }


                        if (rpt.ConsolidateReportTempDTO[i].compoffadjestdate != null && rpt.ConsolidateReportTempDTO[i].compoffadjestdate.Length > 0)
                        {
                            for (int j = 0; j < rpt.ConsolidateReportTempDTO[i].compoffadjestdate.Length; j++)
                            {
                                //DateTime Compoffdate = Convert.ToDateTime(rpt.ConsolidateReportTempDTO[i].compoffadjestdate[j].Date);


                                // dd-MM-yyyy  
                                DateTime Compoffdate;
                                string dateString = rpt.ConsolidateReportTempDTO[i].compoffadjestdate[j].Date;
                                
                                try
                                {
                                    Compoffdate = Convert.ToDateTime(dateString,
                                    System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

                                }
                                catch (Exception e)
                                {
                                    Compoffdate = DateTime.ParseExact(dateString, @"dd/MM/yyyy",
                                     System.Globalization.CultureInfo.InvariantCulture);
                                }



                                _FOContext.Database.ExecuteSqlCommand("FO_Auto_CompOff_Adjest @p0,@p1,@p2,@p3,@p4", Compoffdate, rpt.ConsolidateReportTempDTO[i].compoffadjestdate[j].HRME_Id, rpt.ConsolidateReportTempDTO[i].compoffadjestdate[j].LOP, rpt.ConsolidateReportTempDTO[i].compoffadjestdate[j].Type, rpt.UserId);
                            }

                        }

                    }
                }
                rpt.retrunMsg = "True";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
                rpt.sms_text = e.Message;
                Console.WriteLine(rpt.sms_text);
            }
            return rpt;
        }

        public async Task<BirthDayDTO> GetFinalReport(BirthDayDTO rpt)
        {
            try
            {
                var instdetails = _FOContext.Institution.Where(t => t.MI_Id == rpt.MI_Id).FirstOrDefault();
                if (instdetails != null)
                {
                    rpt.institutename = instdetails.MI_Subdomain;
                }

                string startdate = rpt.yearid + "-" + rpt.month + "-01";
                int monthend = _FOContext.Month.Where(t => t.IVRM_Month_Id == rpt.month).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault();
                string enddate = rpt.yearid + "-" + rpt.month + "-" + monthend;
                rpt.Fromdate = Convert.ToDateTime(startdate);
                rpt.Todate = Convert.ToDateTime(enddate);

                List<long> ids = new List<long>();
                ids = (from a in _FOContext.VMSConsolidateReportTempDMO
                       from b in _FOContext.HR_Master_Employee_DMO
                       where (b.MI_Id == rpt.MI_Id && a.HRME_Id == b.HRME_Id && a.VMSCRT_Year == rpt.yearid.ToString() && a.VMSCRT_Month == rpt.month.ToString())
                       select a.HRME_Id).ToList();
                string hrmeids = String.Join(",", ids);

                rpt.monthname = _FOContext.Month.Where(t => t.IVRM_Month_Id == rpt.month).Select(t => t.IVRM_Month_Name).FirstOrDefault();

                var duplicate = (from a in _FOContext.VMSConsolidateReportTempDMO
                                 from b in _FOContext.HR_Master_Employee_DMO
                                 where (a.VMSCRT_Year == rpt.yearid.ToString() && a.VMSCRT_Month == rpt.month.ToString() && a.HRME_Id == b.HRME_Id && b.MI_Id == rpt.MI_Id)
                                 select new VMSConsolidateReportTempDTO
                                 {
                                     VMSCRT_Id = a.VMSCRT_Id,
                                     VMSCRT_Year = a.VMSCRT_Year,
                                     VMSCRT_Month = a.VMSCRT_Month,
                                     VMSCRT_EMPCode = a.VMSCRT_EMPCode,
                                     VMSCRT_EName = a.VMSCRT_EName,
                                     VMSCRT_Earlyout = a.VMSCRT_Earlyout,
                                     VMSCRT_DailyReportnotsend = a.VMSCRT_DailyReportnotsend,
                                     VMSCRT_Latecoming = a.VMSCRT_Latecoming,
                                     VMSCRT_unauthorizedleave = a.VMSCRT_unauthorizedleave,
                                     VMSCRT_DesigCode = a.VMSCRT_DesigCode,
                                     VMSCRT_DRDetails = a.VMSCRT_DRDetails,
                                     VMSCRT_ULDetails = a.VMSCRT_ULDetails,
                                     VMSCRT_EODetails = a.VMSCRT_EODetails,
                                     VMSCRT_LCDetails = a.VMSCRT_LCDetails,
                                     VMSCRT_EOU180 = a.VMSCRT_EOU180,
                                     VMSCRT_LCU180 = a.VMSCRT_LCU180,
                                     VMSCRT_HREO = a.VMSCRT_HREO,
                                     VMSCRT_HRLC = a.VMSCRT_HRLC,
                                     VMSCRT_HRUL = a.VMSCRT_HRUL,
                                     VMSCRT_HRDR = a.VMSCRT_HRDR,
                                     VMSCRT_HRTLOP = a.VMSCRT_HRTLOP,
                                     VMSCRT_Remarks = a.VMSCRT_Remarks,
                                     VMSCRT_DOJ = b.HRME_DOJ.ToString(),
                                     HRME_Id = a.HRME_Id,
                                     VMSCRT_TLOP = a.VMSCRT_TLOP,
                                     VMSCRT_Approval = a.VMSCRT_Approval,
                                     VMSCRT_MDComment = a.VMSCRT_MDComment,
                                     VMSCRT_HDailyReport = a.VMSCRT_HDailyReport,
                                     VMSCRT_HDRDetails = a.VMSCRT_HDRDetails,
                                     VMSCRT_HRHDR = a.VMSCRT_HRHDR,
                                     VMSCRT_BDailyReport = a.VMSCRT_BDailyReport,
                                     VMSCRT_BDRDetails = a.VMSCRT_BDRDetails,
                                     VMSCRT_HRBDR = a.VMSCRT_HRBDR
                                 }).ToArray();
                rpt.ConsolidateReportTempDTO = duplicate.ToArray();

                //var salaryreport = (from a in _HRMSContext.VMSConsolidateReportTempDMO
                //                    from b in _HRMSContext.HR_Employee_Salary
                //                    from c in _HRMSContext.HR_Employee_Salary_Details
                //                    from d in _HRMSContext.HR_Master_EarningsDeductions
                //                 where (a.VMSCRT_Year == rpt.yearid.ToString() && a.VMSCRT_Month == rpt.months && a.HRME_Id == b.HRME_Id && b.HRES_Year == a.VMSCRT_Year && b.HRES_Month == rpt.monthname && b.MI_Id == rpt.MI_Id && b.HRES_Id == c.HRES_Id && c.HRMED_Id == d.HRMED_Id)
                //                 select new VMSConsolidateReportTempDTO
                //                 {
                //                     HRME_Id = a.HRME_Id,
                //                     HRMED_Id = d.HRMED_Id,
                //                     HRMED_Name = d.HRMED_Name,
                //                     HRMED_EarnDedFlag = d.HRMED_EarnDedFlag,
                //                     HRESD_Amount = c.HRESD_Amount
                //                 }).ToArray();

                //rpt.ConsolidateSalaryReport = salaryreport.ToArray();

                List<long> leaveids = new List<long>();
                leaveids = _FOContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRML_LeaveCreditFlg == true).Select(t => t.HRML_Id).ToList();
                string leaveidstring = String.Join(",", leaveids);

                List<LeaveCreditDTO> result = new List<LeaveCreditDTO>();
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employees_Bal_Leaves";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                    {
                        Value = rpt.Fromdate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                    {
                        Value = rpt.Todate.ToString()
                    });
                    cmd.Parameters.Add(new SqlParameter("@Leaveid", SqlDbType.VarChar)
                    {
                        Value = leaveidstring
                    });
                    cmd.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.VarChar)
                    {
                        Value = hrmeids
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        rpt.LeaveDetails = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                rpt.currentsalary = (from HRES in _HRMSContext.HR_Employee_Salary
                                     from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                     from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                     where (HRESD.HRES_Id == HRES.HRES_Id && HRESD.HRMED_Id == HRMED.HRMED_Id && HRES.HRES_Year == rpt.yearid.ToString() && HRES.HRES_Month == rpt.monthname && hrmeids.Contains(HRES.HRME_Id.ToString()) && HRMED.HRMED_EarnDedFlag == "Earning" && HRMED.HRMED_ActiveFlag == true)
                                     select new HR_Employee_Salary_DetailsDTO
                                     {
                                         HRME_Id = HRES.HRME_Id,
                                         HRESD_Id = HRESD.HRESD_Id,
                                         HRES_Id = HRES.HRES_Id,
                                         HRMED_Id = HRESD.HRMED_Id,
                                         HRMED_Name = HRMED.HRMED_Name,
                                         HRESD_Amount = HRESD.HRESD_Amount,
                                         HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                     }).ToArray();

                if (rpt.ConsolidateReportTempDTO.Length == 0)
                {
                    rpt.retrunMsg = "False";
                }
                else
                {
                    rpt.retrunMsg = "True";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }

        public BirthDayDTO SendFinalMail(BirthDayDTO rpt)
        {
            try
            {

                string monthname = _FOContext.Month.Where(t => t.IVRM_Month_Id.ToString() == rpt.months && t.Is_Active == true).Select(t => t.IVRM_Month_Name).FirstOrDefault();

                if (rpt.ConsolidateReportTempDTO.Length > 0)
                {
                    for (int i = 0; i < rpt.ConsolidateReportTempDTO.Length; i++)
                    {
                        string html = "";
                        string emailid = _FOContext.Multiple_Email_DMO.Where(t => t.HRME_Id == rpt.ConsolidateReportTempDTO[i].HRME_Id && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                        html = rpt.ConsolidateReportTempDTO[i].html;
                        html = html.Replace(@"\", "");
                        sendmail(rpt.MI_Id, emailid, "MonthEndReportMail", rpt.ConsolidateReportTempDTO[i], rpt.yearid.ToString(), monthname, html);
                    }
                }
                rpt.retrunMsg = "True";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }

        public string sendmail(long MI_Id, string Email, string Template, VMSConsolidateReportTempDTO obj, string year, string month, string html)
        {
            try
            {
                if (Email != "" && Email != null)
                {
                    Dictionary<string, string> val = new Dictionary<string, string>();
                    var template = _FOContext.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count == 0)
                    {
                        return "Email Template not Mapped to the selected Institution";
                    }
                    var institutionName = _FOContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                    var Paramaeters = _FOContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                    var ParamaetersName = _FOContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                    string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                    List<Match> variables = new List<Match>();

                    foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                    {
                        variables.Add(match);
                    }

                    //TEMPLATE EDITING
                    Mailmsg = Templatemaking(obj, Mailmsg, MI_Id, year, month, html);
                    //TEMPLATE EDITING

                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _FOContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                    string Attechement = "";
                    if (alldetails.Count > 0)
                    {
                        string Subject = "";
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        try
                        {
                            var Empnames = _FOContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == obj.HRME_Id && p.MI_Id == MI_Id).Select(p => new { p.HRME_EmployeeFirstName, p.HRME_EmployeeMiddleName, p.HRME_EmployeeLastName }).FirstOrDefault();
                            string Empname = Empnames.HRME_EmployeeFirstName + " " + (Empnames.HRME_EmployeeMiddleName + " " + Empnames.HRME_EmployeeLastName).Trim();
                            if (Empname != null && Empname != "")
                            {
                                Subject = Empname.ToUpper().Trim() + "-" + month.ToUpper().Trim() + "-" + year + "-" + "ATTENDANCE REPORT";
                            }
                        }
                        catch
                        {
                            Subject = template[0].ISES_MailSubject.ToString();
                        }
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        string mailcc = "";
                        string mailbcc = "";
                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmail = template[0].ISES_MailCCId.Split(',');

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
                        message.AddTo(Email);

                        if (Attechement.Equals("1"))
                        {
                            var img = _FOContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                        using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _FOContext.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _FOContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _FOContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                                Value = Mailmsg
                            });
                            cmd.Parameters.Add(new SqlParameter("@module",
                            SqlDbType.VarChar)
                            {
                                Value = "Front Office"
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public String Templatemaking(VMSConsolidateReportTempDTO obj, string Mailmsg, long MI_Id, string year, string month, string html)
        {
            try
            {
                //var Empnames = _FOContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == obj.HRME_Id && p.MI_Id == MI_Id).Select(p => new { p.HRME_EmployeeFirstName, p.HRME_EmployeeMiddleName, p.HRME_EmployeeLastName }).FirstOrDefault();
                //string Empname = Empnames.HRME_EmployeeFirstName + " " + (Empnames.HRME_EmployeeMiddleName + " " + Empnames.HRME_EmployeeLastName).Trim();

                //Mailmsg = Mailmsg.Replace("[NAME]", Empname);
                //Mailmsg = Mailmsg.Replace("[EMPCODE]", obj.ecode);
                //Mailmsg = Mailmsg.Replace("[MONTH]", month);
                //Mailmsg = Mailmsg.Replace("[YEAR]", year);
                //Mailmsg = Mailmsg.Replace("[EARLYOUTCNT]", obj.empdate[0].earlyout.ToString());
                //Mailmsg = Mailmsg.Replace("[LATEINCNT]", obj.empdate[0].latecoming.ToString());
                //Mailmsg = Mailmsg.Replace("[LEAVECNT]", obj.leavecount.ToString());
                //Mailmsg = Mailmsg.Replace("[DAILYRPTCNT]", obj.drcount.ToString());
                //Mailmsg = Mailmsg.Replace("[EARLYOUTDATES]", obj.empdate[0].earlydates);
                //Mailmsg = Mailmsg.Replace("[LATEINDATES]", obj.empdate[0].latedates);
                //Mailmsg = Mailmsg.Replace("[LATEIN180DATES]", obj.empdate[0].latedateafter);
                //Mailmsg = Mailmsg.Replace("[EARLYOUT180DATES]", obj.empdate[0].earlydateafter);
                //Mailmsg = Mailmsg.Replace("[DAILYRPTDATES]", obj.VMSCRT_DRDetails);
                //Mailmsg = Mailmsg.Replace("[LEAVEDATES]", obj.leavedates);
                //Mailmsg = Mailmsg.Replace("[TOTLOP]", obj.HR_LOP.ToString());
                //Mailmsg = Mailmsg.Replace("[RMK]", obj.HR_REM);

                Mailmsg = Mailmsg.Replace("[BODY]", html);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Mailmsg;
        }

        public BirthDayDTO SendFinalMailMD(BirthDayDTO rpt)
        {
            try
            {
                string monthname = _FOContext.Month.Where(t => t.IVRM_Month_Id.ToString() == rpt.months && t.Is_Active == true).Select(t => t.IVRM_Month_Name).FirstOrDefault();
                string Template = TemplatemakingMD(rpt.ConsolidateReportTempDTO, "", rpt.MI_Id, rpt.yearid.ToString(), monthname);

                sendmailMD("goutamkumar@vapstech.com", "goutamkumar@vapstech.com", "Attendance Cummulative Report", Template, rpt.MI_Id);

                rpt.retrunMsg = "True";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }

        public void sendmailMD(string mailid, string mailccid, string subject, string body, long id)
        {
            //mailid = "goutamkumar@vapstech.com";
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_Template_Name.Equals("FO", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _db.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = mailccid;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);

                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
            // return "success";
        }

        public String TemplatemakingMD(VMSConsolidateReportTempDTO[] ConsolidateReport, string Mailmsg, long MI_Id, string year, string month)
        {
            string Templatemsg = "";
            try
            {
                Templatemsg = " <p><span style='font-weight:bold'>Cummulative report for " + month + " - " + year + " </span></p>   <table width='100%' id='tablepaging' class='yui' style='border-collapse: collapse; width: 100%;'><tr>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> EMP CODE </th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> NAME</th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> EARLY OUT LOP</th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> LATE COMING LOP </th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> UNAUTHORIZED LEAVE LOP </th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> DAILY REPORT NOT SENT LOP </th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> TOTAL LOP </th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> REMARKS </th></tr>";

                for (int count = 0; count < ConsolidateReport.Length; count++)
                {
                    var Empnames = _FOContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == ConsolidateReport[count].HRME_Id && p.MI_Id == MI_Id).Select(p => new { p.HRME_EmployeeFirstName, p.HRME_EmployeeMiddleName, p.HRME_EmployeeLastName }).FirstOrDefault();
                    string Empname = Empnames.HRME_EmployeeFirstName + " " + (Empnames.HRME_EmployeeMiddleName + " " + Empnames.HRME_EmployeeLastName).Trim();

                    Templatemsg = Templatemsg + "<tr> " +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;'> " + ConsolidateReport[count].VMSCRT_EMPCode + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'>" + Empname + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" + (ConsolidateReport[count].VMSCRT_EOU180 == "" ? 0 : Convert.ToInt16(ConsolidateReport[count].VMSCRT_EOU180) * 0.5) + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" +
                        (ConsolidateReport[count].VMSCRT_LCU180 == "" ? 0 : Convert.ToInt16(ConsolidateReport[count].VMSCRT_LCU180) * 0.5) + "</td> " +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" + ConsolidateReport[count].VMSCRT_unauthorizedleave + "</td> " +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" +
                        ConsolidateReport[count].VMSCRT_DailyReportnotsend * 0.5 + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" + ConsolidateReport[count].VMSCRT_TLOP + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" + ConsolidateReport[count].VMSCRT_Remarks + "</td>" +
                        "</tr>";
                }

                Templatemsg = Templatemsg + "</table>";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Templatemsg;
        }

        public BirthDayDTO saveMDdata(BirthDayDTO rpt)
        {
            try
            {
                if (rpt.ConsolidateReportTempDTO.Length > 0)
                {
                    for (int i = 0; i < rpt.ConsolidateReportTempDTO.Length; i++)
                    {
                        var duplicate = _FOContext.VMSConsolidateReportTempDMO.Where(t => t.VMSCRT_Id == rpt.ConsolidateReportTempDTO[i].VMSCRT_Id).FirstOrDefault();

                        if (duplicate != null)
                        {
                            duplicate.VMSCRT_Approval = rpt.ConsolidateReportTempDTO[i].VMSCRT_Approval;
                            duplicate.VMSCRT_MDComment = rpt.ConsolidateReportTempDTO[i].VMSCRT_MDComment;
                            _FOContext.Update(duplicate);
                            _FOContext.SaveChanges();
                        }
                    }
                }
                rpt.retrunMsg = "True";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }

        public BirthDayDTO sendmailAcc(BirthDayDTO rpt)
        {
            try
            {
                string monthname = _FOContext.Month.Where(t => t.IVRM_Month_Id.ToString() == rpt.months && t.Is_Active == true).Select(t => t.IVRM_Month_Name).FirstOrDefault();
                string Template = TemplatemakingAcc(rpt.ConsolidateReportTempDTO, "", rpt.MI_Id, rpt.yearid.ToString(), monthname);

                sendmailMD("goutamkumar@vapstech.com", "goutamkumar@vapstech.com", "Attendance Cummulative Report", Template, rpt.MI_Id);

                rpt.retrunMsg = "True";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }

        public String TemplatemakingAcc(VMSConsolidateReportTempDTO[] ConsolidateReport, string Mailmsg, long MI_Id, string year, string month)
        {
            string Templatemsg = "";
            try
            {
                Templatemsg = " <p><span style='font-weight:bold'>Cummulative report for " + month + " - " + year + " </span></p>   <table width='100%' id='tablepaging' class='yui' style='border-collapse: collapse; width: 100%;'><tr>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> EMP CODE </th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> NAME</th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> TOTAL LOP </th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> MD APPROVAL </th>" +
                    "<th style='border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'> MD COMMENT </th>" +
                    "</tr>";

                for (int count = 0; count < ConsolidateReport.Length; count++)
                {
                    var Empnames = _FOContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == ConsolidateReport[count].HRME_Id && p.MI_Id == MI_Id).Select(p => new { p.HRME_EmployeeFirstName, p.HRME_EmployeeMiddleName, p.HRME_EmployeeLastName }).FirstOrDefault();
                    string Empname = Empnames.HRME_EmployeeFirstName + " " + (Empnames.HRME_EmployeeMiddleName + " " + Empnames.HRME_EmployeeLastName).Trim();

                    Templatemsg = Templatemsg + "<tr> " +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;'> " + ConsolidateReport[count].VMSCRT_EMPCode + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'>" + Empname + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" + ConsolidateReport[count].VMSCRT_TLOP + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" + (ConsolidateReport[count].VMSCRT_Approval == true ? "YES" : "NO") + "</td>" +
                        "<td style='border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'>" + ConsolidateReport[count].VMSCRT_MDComment + "</td>" +
                        "</tr>";
                }

                Templatemsg = Templatemsg + "</table>";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Templatemsg;
        }

        public async Task<BirthDayDTO> deleteduplicatepunch(BirthDayDTO rpt)
        {
            try
            {
                int s = _FOContext.Database.ExecuteSqlCommand("FO_EPDayWiseDupRcd_Delete");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rpt;
        }

        public BirthDayDTO Notapprovedstatus(BirthDayDTO rpt)
        {
            try
            {


                string startdate = rpt.yearid + "-" + rpt.month + "-01";
                rpt.Fromdate = Convert.ToDateTime(startdate);
                var year = Convert.ToDateTime(startdate).Year;
                var mon = Convert.ToDateTime(startdate).Month;
                var startOfMonth = new DateTime(year, mon, 1);
                var DaysInMonth = DateTime.DaysInMonth(year, mon);
                rpt.Todate = new DateTime(year, mon, DaysInMonth);
                string fromdate = "";
                string todate = "";
                if (rpt.Fromdate != null && rpt.Todate != null)
                {
                    fromdate = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd");
                    todate = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd");
                }
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Leaves_PendingStatus";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar)
                    {
                        Value = todate
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
                        rpt.Leavepending = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }
        public BirthDayDTO DRWaveofReport(BirthDayDTO rpt)
        {
            try
            {

                string Monthid = "0";
                List<long> month = new List<long>();
                foreach (var item in rpt.Monthlist)
                {
                    month.Add(item.IVRM_Month_Id);
                }
                for (int a = 0; a < month.Count(); a++)
                {
                    Monthid = Monthid + ',' + month[a].ToString();
                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Blocked_Days_ReportTemp";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar)
                    {
                        Value = rpt.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.NVarChar)
                    {
                        Value = Monthid
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
                        rpt.drblockedReport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }

        //=====================attendence approval ============================
        public BirthDayDTO getdataapprover(BirthDayDTO rpt)
        {


            List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
            staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRMGT_ActiveFlag == true).ToList();
            rpt.filltypes = staf_types.Distinct().ToArray();
            rpt.companylist = _db.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();


            var athorisation = (from a in _db.HR_PROCESSDMO
                                from b in _db.HR_Process_Auth_OrderNoDMO
                                where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == rpt.Id && a.HRPA_TypeFlag == "Attendence")
                                select a).Count();

            if (athorisation > 0)
            {

                rpt.fillyear = (from a in _db.HR_Master_LeaveYearDMO
                                where (a.MI_Id == rpt.MI_Id && a.HRMLY_ActiveFlag == true)
                                select new HR_Master_LeaveYearDTO
                                {
                                    HRMLY_Id = a.HRMLY_Id,
                                    HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                    HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                rpt.fillmonth = (from a in _db.month
                                 select new EmployeeLogReportDTO
                                 {
                                     IVRM_Month_Id = a.IVRM_Month_Id,
                                     monthname = a.IVRM_Month_Name
                                 }).Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();

                rpt.mappeduser = 1;
            }
            else
            {
                rpt.fillyear = (from a in _db.HR_Master_LeaveYearDMO
                                where (a.MI_Id == rpt.MI_Id && a.HRMLY_ActiveFlag == true)
                                select new HR_Master_LeaveYearDTO
                                {
                                    HRMLY_Id = a.HRMLY_Id,
                                    HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                    HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                rpt.fillmonth = (from a in _db.month
                                 select new EmployeeLogReportDTO
                                 {
                                     IVRM_Month_Id = a.IVRM_Month_Id,
                                     monthname = a.IVRM_Month_Name
                                 }).Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();
                rpt.mappeduser = 0;
            }





            using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_Emp_PunchMissing_Approval";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                {
                    Value = rpt.Id
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
                    rpt.punchmissingapproval = retObject1.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }



            return rpt;
        }

        public async Task<BirthDayDTO> getmonthreportforapproal(BirthDayDTO rpt)
        {
            try
            {

                var instdetails = _FOContext.Institution.Where(t => t.MI_Id == rpt.MI_Id).FirstOrDefault();
                if (instdetails != null)
                {
                    rpt.institutename = instdetails.MI_Subdomain;
                }
                var singleorall = 0;
                //var employeelist = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.MI_Id == rpt.MI_Id).Select(t => t.HRME_Id).ToArray();
                if (rpt.rdbbutton == "Month")
                {
                    string startdate = rpt.yearid + "-" + rpt.month + "-01";
                    rpt.Fromdate = Convert.ToDateTime(startdate);
                    var year = Convert.ToDateTime(startdate).Year;
                    var mon = Convert.ToDateTime(startdate).Month;
                    var startOfMonth = new DateTime(year, mon, 1);
                    var DaysInMonth = DateTime.DaysInMonth(year, mon);
                    rpt.Todate = new DateTime(year, mon, DaysInMonth);
                }


                var MonthName = _db.month.Where(t => t.IVRM_Month_Id == rpt.month).Select(t => t.IVRM_Month_Name).FirstOrDefault();
                string Year = rpt.yearid.ToString();
                rpt.salaryGenerated = _HRMSContext.HR_Employee_Salary.Where(c => c.MI_Id == rpt.MI_Id && c.HRES_Year == Year && c.HRES_Month == MonthName).Distinct().ToArray();


                if (rpt.flag == "Single")
                {
                    singleorall = 1;
                    var emp_Id = _FOContext.Staff_User_Login.Where(c => c.Id == rpt.Id).Distinct().ToList();
                    if (emp_Id.Count > 0)
                    {
                        rpt.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;
                    }
                    if (rpt.flagl == "1")
                    {
                        var getmonth = rpt.Fromdate.Value.Month;
                        var getyear = rpt.Fromdate.Value.Year;
                        rpt.yearid = getyear;
                        rpt.month = getmonth;

                        string startdate = getyear + "-" + getmonth + "-01";
                        rpt.Fromdate = Convert.ToDateTime(startdate);
                        rpt.Todate = Convert.ToDateTime(rpt.Todate).AddDays(-1);
                    }
                    else if (rpt.flagl == "0")
                    {
                        // var getmonth = (rpt.Fromdate.Value.Month) - 1;
                        var getmonth = DateTime.Now.AddMonths(-1).Month;


                        if (getmonth >= 1)
                        {
                            getmonth = Convert.ToInt32(getmonth);
                            var getyear = rpt.Fromdate.Value.Year;
                            rpt.yearid = getyear;
                            rpt.month = getmonth;
                            var today = DateTime.Today;
                            var month = new DateTime(today.Year, today.Month, 1);
                            string startdate = getyear + "-" + getmonth + "-01";
                            //rpt.Fromdate = Convert.ToDateTime(startdate);
                            rpt.Fromdate = month.AddMonths(-1);
                            var mon = Convert.ToDateTime(rpt.Fromdate).Month;
                            var startOfMonth = new DateTime(getyear, getmonth, 1);
                            var DaysInMonth = DateTime.DaysInMonth(getyear, mon);
                            //rpt.Todate = new DateTime(getyear, mon, DaysInMonth);
                            rpt.Todate = month.AddDays(-1);

                            //var Fromdate = month.AddMonths(-1);
                            //var Todate = month.AddDays(-1);
                        }
                        else
                        {
                            var getyear = Convert.ToInt32((rpt.Fromdate.Value.Year)) - 1;
                            string startdate = getyear + "-" + 12 + "-01";
                            rpt.yearid = getyear;
                            rpt.month = getmonth;
                            var today = DateTime.Today;
                            var month = new DateTime(today.Year, today.Month, 1);
                            // rpt.Fromdate = Convert.ToDateTime(startdate);
                            rpt.Fromdate = month.AddMonths(-1);
                            var mon = Convert.ToDateTime(rpt.Fromdate).Month;
                            var startOfMonth = new DateTime(getyear, getmonth, 1);
                            var DaysInMonth = DateTime.DaysInMonth(getyear, mon);
                            //rpt.Todate = new datetime(getyear, mon, daysinmonth);
                            rpt.Todate = month.AddDays(-1);


                        }
                    }

                    rpt.SelectedActivityDetails1_stf = (from a in _FOContext.HR_Master_Employee_DMO
                                                        from b in _FOContext.HR_Master_Designation_DMO
                                                        where (a.MI_Id == rpt.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == rpt.HRME_Id && a.HRME_DOJ <= rpt.Todate && ((a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true) || (a.HRME_DOL.Value.Month == rpt.Todate.Value.Month && a.HRME_DOL.Value.Year == rpt.Todate.Value.Year)))
                                                        select new MasterEmployeeDTO
                                                        {
                                                            HRME_Id = a.HRME_Id,
                                                            HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                                            HRME_PerStreet = b.HRMDES_DesignationName,
                                                            HRME_DOJ = a.HRME_DOJ,
                                                            HRME_ExcPunch = a.HRME_ExcPunch,
                                                            HRME_ExcDR = a.HRME_ExcDR,
                                                            HRME_LeftFlag = a.HRME_LeftFlag,
                                                            HRME_DOL = a.HRME_DOL,
                                                            HRME_Relaxtime = a.HRME_Relaxtime
                                                        }).OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

                    List<MasterEmployeeDTO> Tasklist = new List<MasterEmployeeDTO>();

                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "SelectedActivityDetails1_stf";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //    {
                    //        Value = rpt.MI_Id
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    //    {
                    //        Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id", SqlDbType.Bit)
                    //    {
                    //        Value = rpt.Id
                    //    });
                    //    cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.BigInt)
                    //    {
                    //        Value = rpt.HRME_Id
                    //    });
                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

                    //    var retObject = new List<dynamic>();
                    //    try
                    //    {
                    //        using (var dataReader = cmd.ExecuteReader())
                    //        {
                    //            while (dataReader.Read())
                    //            {

                    //                Tasklist.Add(new MasterEmployeeDTO
                    //                {
                    //                    HRME_Id = Convert.ToInt64(dataReader["hrmE_Id"]),
                    //                    HRME_EmployeeCode = Convert.ToString(dataReader["hrmE_EmployeeCode"]),
                    //                    HRME_EmployeeFirstName = Convert.ToString(dataReader["hrmE_EmployeeFirstName"]),
                    //                    HRME_PerStreet = Convert.ToString(dataReader["hrmE_PerStreet"]),
                    //                    HRME_DOJ = Convert.ToDateTime(dataReader["hrmE_DOJ"]),
                    //                    HRME_ExcPunch = Convert.ToBoolean(dataReader["hrmE_ExcPunch"]),
                    //                    HRME_ExcDR = Convert.ToBoolean(dataReader["hrmE_ExcDR"]),
                    //                    HRME_LeftFlag = Convert.ToBoolean(dataReader["hrmE_LeftFlag"]),
                    //                    HRME_DOL = Convert.ToDateTime(dataReader["hrmE_DOL"]),
                    //                    HRME_Relaxtime = Convert.ToInt64(dataReader["hrmE_Relaxtime"]),

                    //                });

                    //                rpt.SelectedActivityDetails1_stf = Tasklist.ToArray();
                    //            }

                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.Write(ex.Message);
                    //    }
                    //}







                }
                else
                {
                    singleorall = 0;
                    rpt.SelectedActivityDetails1_stf = (from a in _FOContext.HR_Master_Employee_DMO
                                                        from b in _FOContext.HR_Master_Designation_DMO
                                                        from c in _FOContext.VMSConsolidateReportTempDMO
                                                        where (c.VMSCRT_Year == rpt.yearid.ToString() && c.VMSCRT_Month == rpt.month.ToString() && a.HRME_Id == c.HRME_Id && a.MI_Id == rpt.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_DOJ <= rpt.Todate && ((a.HRME_LeftFlag == false && a.HRME_ActiveFlag == true) || (a.HRME_DOL.Value.Month == rpt.Todate.Value.Month && a.HRME_DOL.Value.Year == rpt.Todate.Value.Year)))
                                                        select new MasterEmployeeDTO
                                                        {
                                                            VMSCRT_Id = c.VMSCRT_Id,
                                                            HRME_Id = a.HRME_Id,
                                                            HRME_EmployeeCode = a.HRME_EmployeeCode,
                                                            HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                                            HRME_PerStreet = b.HRMDES_DesignationName,
                                                            HRME_DOJ = a.HRME_DOJ,
                                                            HRME_ExcPunch = a.HRME_ExcPunch,
                                                            HRME_ExcDR = a.HRME_ExcDR,
                                                            HRME_LeftFlag = a.HRME_LeftFlag,
                                                            HRME_DOL = a.HRME_DOL,
                                                            HRME_Relaxtime = a.HRME_Relaxtime
                                                        }).OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

                    List<MasterEmployeeDTO> Tasklist = new List<MasterEmployeeDTO>();


                    //rpt.approvedlist = (from a in _FOContext.VMSConsolidateReportTempDMO
                    //                    from b in _FOContext.HR_Emp_Attendance_ApprovalDMO
                    //                    where (a.VMSCRT_Year == rpt.yearid.ToString() && a.VMSCRT_Month == rpt.month.ToString() && a.VMSCRT_Id == b.VMSCRT_Id && b.HREAA_ApprovalFlg== "Hold")
                    //                    select b).ToArray();


                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Employee_SalaryAttandanceHoldList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
                          SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(rpt.MI_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                      SqlDbType.VarChar)
                        {
                            Value = "attandence"
                        });
                        cmd.Parameters.Add(new SqlParameter("@year",
                      SqlDbType.BigInt)
                        {
                            Value = rpt.yearid
                        });
                        cmd.Parameters.Add(new SqlParameter("@month",
                      SqlDbType.BigInt)
                        {
                            Value = rpt.month
                        });
                        cmd.Parameters.Add(new SqlParameter("@userid",
                      SqlDbType.BigInt)
                        {
                            Value = 0
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
                            rpt.approvedlist = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    string hrmeids = "0";
                    string LIEOhrmeids = "0";

                    for (int i = 0; i < rpt.SelectedActivityDetails1_stf.Length; i++)
                    {
                        if (i == 0) { hrmeids = rpt.SelectedActivityDetails1_stf[i].HRME_Id.ToString(); }
                        else { hrmeids = hrmeids + "," + rpt.SelectedActivityDetails1_stf[i].HRME_Id.ToString(); }
                        if (rpt.SelectedActivityDetails1_stf[i].HRME_PerStreet.ToUpper() != "DRIVER")
                        {
                            if (i == 0) { LIEOhrmeids = rpt.SelectedActivityDetails1_stf[i].HRME_Id.ToString(); }
                            else { LIEOhrmeids = LIEOhrmeids + "," + rpt.SelectedActivityDetails1_stf[i].HRME_Id.ToString(); }
                        }
                    }

                    long hrmly_id = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id == rpt.MI_Id && Convert.ToDateTime(t.HRMLY_FromDate) <= rpt.Fromdate && Convert.ToDateTime(t.HRMLY_ToDate) >= rpt.Todate && t.HRMLY_ActiveFlag == true).Select(t => t.HRMLY_Id).FirstOrDefault();

                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Rating_EMPLOYEEMonthly_Overallrating";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MONTH_ID", SqlDbType.BigInt)
                        {
                            Value = rpt.month
                        });
                        cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.BigInt)
                        {
                            Value = Year
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
                            rpt.employeeRetingdata = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_AllEmpsAttendanceMissingPunchUpdate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.BigInt)
                        {
                            Value = rpt.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                        {
                            Value = singleorall
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
                            rpt.workinghourDetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_AllEmpsAttendanceMissingPunchUpdate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                        {
                            Value = singleorall
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.BigInt)
                        {
                            Value = rpt.HRME_Id
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
                            rpt.workinghourDetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fo_Emp_Log_InDetails_Report"; //"Fo_Emp_Log_Report"
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                        {
                            //rpt.HRELT_FromDate = Convert.ToDateTime(rpt.Fromdate);
                            Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@multiplehrmeid", SqlDbType.VarChar)
                        {
                            Value = LIEOhrmeids
                        });
                        cmd.Parameters.Add(new SqlParameter("@punchtype", SqlDbType.VarChar)
                        {
                            Value = "LIEO"
                        });
                        cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                        {
                            Value = singleorall
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
                            rpt.staffDetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmpDrReportsNotSentDates";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                        {
                            Value = hrmly_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
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
                            rpt.Dailyreptdetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_DR_NotApprovedCount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                        {
                            Value = hrmly_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
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
                            rpt.columnnames = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    rpt.lunchdata = _HRMSContext.Lop_DetailsDMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRALD_Date >= rpt.Fromdate && t.HRALD_Date <= rpt.Todate && t.HRALD_Activeflag == true).ToArray();

                    rpt.Halfdaydrdata = (from t in _FOContext.ISM_DailyReportGenerationDMO
                                         where (t.MI_Id == rpt.MI_Id && t.ISMDRPT_Date >= rpt.Fromdate && t.ISMDRPT_Date <= rpt.Todate && t.ISMDRPT_ActiveFlg == true && t.ISMDRPT_HalfDayFlag == true)
                                         select new BirthDayDTO
                                         {
                                             HRME_Id = t.HRME_Id,
                                             ISMDRPT_Date = t.ISMDRPT_Date
                                         }).Distinct().ToArray();

                    rpt.Blockeddrdata = (from t in _FOContext.ISM_DailyReportGenerationDMO
                                         where (t.MI_Id == rpt.MI_Id && t.ISMDRPT_Date >= rpt.Fromdate && t.ISMDRPT_Date <= rpt.Todate && t.ISMDRPT_ActiveFlg == true && t.ISMDRPT_BlockedFlag == true)
                                         select new BirthDayDTO
                                         {
                                             HRME_Id = t.HRME_Id,
                                             ISMDRPT_Date = t.ISMDRPT_Date
                                         }).OrderBy(t => t.ISMDRPT_Date).Distinct().ToArray();

                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_EmpUnAuthLeaveDates";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                        {
                            Value = hrmly_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                        {
                            Value = singleorall
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.BigInt)
                        {
                            Value = rpt.HRME_Id
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
                            rpt.staffleaveDetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_EmpAuthLeaveDates";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                        {
                            Value = hrmly_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@IndORAllFlg", SqlDbType.Bit)
                        {
                            Value = singleorall
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMEId", SqlDbType.BigInt)
                        {
                            Value = rpt.HRME_Id
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
                            rpt.LeaveDetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_PunchMissing_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 800000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(rpt.Todate).ToString("yyyy-MM-dd")
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
                            rpt.punchmissingapproval = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    rpt.holidaylist = (from a in _FOContext.holidayWorkingDayType
                                       from b in _FOContext.holidaydate
                                       where (a.MI_Id == b.MI_Id && a.MI_Id == rpt.MI_Id && b.HRMLY_Id == hrmly_id && a.FOHTWD_HolidayFlag == true && a.FOHWDT_ActiveFlg == true && a.FOHWDT_Id == b.FOHWDT_Id && b.FOMHWDD_FromDate >= rpt.Fromdate && b.FOMHWDD_FromDate <= rpt.Todate)
                                       select new BirthDayDTO
                                       {
                                           FOMHWDD_FromDate = b.FOMHWDD_FromDate,
                                           FOMHWDD_ToDate = b.FOMHWDD_ToDate
                                       }).ToArray();

                    List<long> leaveids = new List<long>();
                    leaveids = _FOContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == rpt.MI_Id && t.HRML_LeaveCreditFlg == true).Select(t => t.HRML_Id).ToList();
                    string leaveidstring = String.Join(",", leaveids);



                    rpt.monthname = _FOContext.Month.Where(t => t.IVRM_Month_Id == rpt.month).Select(t => t.IVRM_Month_Name).FirstOrDefault();
                    //List<HR_Employee_Salary_DetailsDTO> currentsalary = new List<HR_Employee_Salary_DetailsDTO>();
                    rpt.currentsalary = (from HRES in _HRMSContext.HR_Employee_Salary
                                         from HRESD in _HRMSContext.HR_Employee_Salary_Details
                                         from HRMED in _HRMSContext.HR_Master_EarningsDeductions
                                         where (HRESD.HRES_Id == HRES.HRES_Id && HRESD.HRMED_Id == HRMED.HRMED_Id && HRES.HRES_Year == rpt.yearid.ToString() && HRES.HRES_Month == rpt.monthname && hrmeids.Contains(HRES.HRME_Id.ToString()) && HRMED.HRMED_EarnDedFlag == "Earning" && HRMED.HRMED_ActiveFlag == true)
                                         select new HR_Employee_Salary_DetailsDTO
                                         {
                                             HRME_Id = HRES.HRME_Id,
                                             HRESD_Id = HRESD.HRESD_Id,
                                             HRES_Id = HRES.HRES_Id,
                                             HRMED_Id = HRESD.HRMED_Id,
                                             HRMED_Name = HRMED.HRMED_Name,
                                             HRESD_Amount = HRESD.HRESD_Amount,
                                             HRMED_EarnDedFlag = HRMED.HRMED_EarnDedFlag
                                         }).ToArray();

                    //rpt.HRCOMMENTS = _FOContext.VMSConsolidateReportTempDMO.Where(t => t.VMSCRT_Year == rpt.yearid.ToString() && t.VMSCRT_Month == rpt.month.ToString() && employeelist.Contains(t.HRME_Id)).ToArray();
                    var duplicate = (from a in _FOContext.VMSConsolidateReportTempDMO
                                     from b in _FOContext.HR_Master_Employee_DMO
                                     from c in _FOContext.Multiple_Email_DMO
                                     where (a.VMSCRT_Year == rpt.yearid.ToString() && a.VMSCRT_Month == rpt.month.ToString() && a.HRME_Id == b.HRME_Id && b.MI_Id == rpt.MI_Id && b.HRME_Id == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                                     select new VMSConsolidateReportTempDTO
                                     {
                                         VMSCRT_Id = a.VMSCRT_Id,
                                         VMSCRT_Year = a.VMSCRT_Year,
                                         VMSCRT_Month = a.VMSCRT_Month,
                                         VMSCRT_EMPCode = a.VMSCRT_EMPCode,
                                         VMSCRT_EName = a.VMSCRT_EName,
                                         VMSCRT_Earlyout = a.VMSCRT_Earlyout,
                                         VMSCRT_DailyReportnotsend = a.VMSCRT_DailyReportnotsend,
                                         VMSCRT_Latecoming = a.VMSCRT_Latecoming,
                                         VMSCRT_unauthorizedleave = a.VMSCRT_unauthorizedleave,
                                         VMSCRT_DesigCode = a.VMSCRT_DesigCode,
                                         VMSCRT_DRDetails = a.VMSCRT_DRDetails,
                                         VMSCRT_ULDetails = a.VMSCRT_ULDetails,
                                         VMSCRT_EODetails = a.VMSCRT_EODetails,
                                         VMSCRT_LCDetails = a.VMSCRT_LCDetails,
                                         VMSCRT_EOU180 = a.VMSCRT_EOU180,
                                         VMSCRT_LCU180 = a.VMSCRT_LCU180,
                                         VMSCRT_HREO = a.VMSCRT_HREO,
                                         VMSCRT_HRLC = a.VMSCRT_HRLC,
                                         VMSCRT_HRUL = a.VMSCRT_HRUL,
                                         VMSCRT_HRDR = a.VMSCRT_HRDR,
                                         VMSCRT_HRTLOP = a.VMSCRT_HRTLOP,
                                         VMSCRT_Remarks = a.VMSCRT_Remarks,
                                         VMSCRT_DOJ = b.HRME_DOJ.ToString(),
                                         HRME_Id = a.HRME_Id,
                                         VMSCRT_TLOP = a.VMSCRT_TLOP,
                                         emailid = c.HRMEM_EmailId
                                     }).ToArray();
                    rpt.HRCOMMENTS = duplicate.ToArray();


                    rpt.monthendreportclientwise = _FOContext.ISM_Client_MERDMO.Where(a => a.Month_Id == rpt.month && a.ISMCLMER_Year == rpt.yearid && a.ISMCLMER_ActiveFlag == true).ToArray();


                    rpt.staffList = (from a in _FOContext.ISM_Master_Client_IEMapping_DMO
                                     from b in _FOContext.HR_Master_Employee_DMO
                                     where (a.ISMCIM_IEList == b.HRME_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && a.ISMMCLTIE_ActiveFlag == true)
                                     select b).ToArray();

                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    try
                    {
                        var typ = rpt.flag;
                        if (typ != "Single")
                        {
                            typ = "All";
                        }
                        _FOContext.Database.ExecuteSqlCommand("AttendanceLogInsert @p0,@p1,@p2,@p3,@p4,@p5", rpt.MI_Id, rpt.Id, typ, indianTime, rpt.Fromdate, rpt.Todate);
                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine(Ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }

        public BirthDayDTO attendenceapprove(BirthDayDTO rpt)
        {
            try
            {
                if (rpt.AttendenceApprovedDTO.Length > 0)
                {
                    var approvedresult = 0;
                    for (int i = 0; i < rpt.AttendenceApprovedDTO.Length; i++)
                    {
                        //var duplicate = _FOContext.VMSConsolidateReportTempDMO.Where(t => t.VMSCRT_Year == rpt.yearid.ToString() && t.VMSCRT_Month == rpt.months && t.HRME_Id == rpt.Id).ToList();
                        approvedresult = _HRMSContext.Database.ExecuteSqlCommand("Employee_Attendence_Approval @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", rpt.MI_Id, rpt.AttendenceApprovedDTO[i].ecode, rpt.AttendenceApprovedDTO[i].hrmE_Id, rpt.Id, rpt.AttendenceApprovedDTO[i].HREAA_Remarks, rpt.AttendenceApprovedDTO[i].HREAA_LOP, rpt.months, rpt.yearid, rpt.AttendenceApprovedDTO[i].status);

                    }

                    if (approvedresult > 0)
                    {
                        rpt.message = "Saved";
                    }
                    else
                    {
                        rpt.message = "NotSaved";
                    }


                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }
        public BirthDayDTO attendenceapprovedlist(BirthDayDTO rpt)
        {
            try
            {
                if (rpt.rdbbutton == "Month")
                {
                    string startdate = rpt.yearid + "-" + rpt.month + "-01";
                    rpt.Fromdate = Convert.ToDateTime(startdate);
                    var year = Convert.ToDateTime(startdate).Year;
                    var mon = Convert.ToDateTime(startdate).Month;
                    var startOfMonth = new DateTime(year, mon, 1);
                    var DaysInMonth = DateTime.DaysInMonth(year, mon);
                    rpt.Todate = new DateTime(year, mon, DaysInMonth);
                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Attendence_Approved_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(rpt.Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMSCRT_Year", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(rpt.yearid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@VMSCRT_Month", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(rpt.months)
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
                        rpt.griddatalist = retObject.Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                rpt.retrunMsg = "False";
            }
            return rpt;
        }

        //Unauthorized Leave Count Report

        public BirthDayDTO getdata1(BirthDayDTO data)
        {
            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
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
                        Value = "app.UnauthorizedLeaveCountReport"
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
                        data.pagePrivilege = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                //Only it will work once page will mapp to that perticular employee
                if (data.pagePrivilege.Length > 0)
                {
                    data.getmonth = _FOContext.Month.Where(t => t.Is_Active == true).ToArray();

                    data.getyear = _FOContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                    data.institutionlist = _FOContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BirthDayDTO unauthorizedreport(BirthDayDTO data)
        {
            try
            {
                if (data.Datetype == "Month")
                {
                    string startdate = data.HRMLY_LeaveYear + "-" + data.IVRM_Month_Id + "-01";
                    data.from_date = Convert.ToDateTime(startdate);
                    var year = Convert.ToDateTime(startdate).Year;
                    var mon = Convert.ToDateTime(startdate).Month;
                    var startOfMonth = new DateTime(year, mon, 1);
                    var DaysInMonth = DateTime.DaysInMonth(year, mon);
                    data.to_date = new DateTime(year, mon, DaysInMonth);
                }



                string mi_ids = "0";
                List<long> mid = new List<long>();

                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                    for (int i = 0; i < mid.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }

                string hrmd_ids = "0";
                List<long> dept_id = new List<long>();

                if (data.selected_Dept.Length > 0)
                {
                    foreach (var item in data.selected_Dept)
                    {
                        dept_id.Add(item.HRMD_Id);
                    }

                    for (int i = 0; i < dept_id.Count; i++)
                    {
                        hrmd_ids = hrmd_ids + "," + dept_id[i].ToString();
                    }
                }

                var from_date = data.from_date.Value.Date.ToString("yyyy-MM-dd");
                var to_date = data.to_date.Value.Date.ToString("yyyy-MM-dd");

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {//HR_Unauthorizedleave_report
                    cmd.CommandText = "HR_Unauthorizedleave_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID1", SqlDbType.VarChar) { Value = mi_ids });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = to_date });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = data.flag });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_ID2", SqlDbType.VarChar) { Value = hrmd_ids });
                    cmd.Parameters.Add(new SqlParameter("@MI_DeptFlag", SqlDbType.VarChar) { Value = data.radioflag });
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.gettodate = retObject.ToArray();
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


        public BirthDayDTO getdepartment(BirthDayDTO data)
        {
            try
            {

                data.columnnames = (from a in _FOContext.HR_Master_Department_DMO
                                    from b in _FOContext.Institution
                                    where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                    select new HR_Master_DepartmentDTO
                                    {
                                        HRMD_Id = a.HRMD_Id,
                                        MI_Id = a.MI_Id,
                                        HRMD_DepartmentName = a.HRMD_DepartmentName
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //=============End==================

        //Montly Attandence Auto Mail Trigger For All Employee.
        public BirthDayDTO MonthlyAutoMailTrigger(BirthDayDTO data)
        {
            try
            {
                List<BirthDayDTO> employeedetails = new List<BirthDayDTO>();

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Attandence_Monthend_AutoMail_Trigger";
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                //retObject.Add((ExpandoObject)dataRow);


                                employeedetails.Add(new BirthDayDTO
                                {
                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"].ToString()),
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_ID"].ToString()),
                                    Id = Convert.ToInt64(dataReader["VMSCRT_Id"].ToString()),
                                    HRME_EmailId = dataReader["HRMEM_EmailId"].ToString(),
                                    ccmail = dataReader["ccmail"].ToString(),

                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (employeedetails != null && employeedetails.Count > 0)
                {
                    for (var i = 0; i < employeedetails.Count; i++)
                    {

                        sendEmail(employeedetails[i].MI_Id, "MonthEndReportAutoMailTrigger", employeedetails[i].Id, employeedetails[i].HRME_EmailId, employeedetails[i].ccmail);

                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public BirthDayDTO AdjustCompOff(BirthDayDTO data)
        {
            try
            {
                data.month = data.Fromdate.Value.Month;
                data.yearid = data.Fromdate.Value.Year;
                var HRME_ID = _FOContext.Staff_User_Login.Where(t => t.Id == data.Id).Select(t => t.Emp_Code).FirstOrDefault();

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {//FO_Auto_CompOff_Adjest
                    cmd.CommandText = "FO_Auto_CompOff_Adjest";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = data.yearid });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.Int) { Value = data.month });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = HRME_ID });
                    cmd.Parameters.Add(new SqlParameter("@LOPadjest", SqlDbType.VarChar) { Value = data.LOP });
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.gettodate = retObject.ToArray();
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

        public string sendEmail(long MI_Id, string Template, long HRME_ID, string email, string ccemail)        {            try            {                Dictionary<string, string> val = new Dictionary<string, string>();                var template = _FOContext.smsEmailSetting.Where(e => e.ISES_Template_Name == Template                && e.ISES_MailActiveFlag == true).ToList();                if (template.Count == 0)                {                    return "Email Template not Mapped to the selected Institution";                }                var institutionName = _FOContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Resultsms = Mailcontent;                string result = Mailmsg;                var Paramaeters = _FOContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();                var ParamaetersName = _FOContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_Email_Prametars_Replace";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",                  SqlDbType.VarChar)                    {                        Value = HRME_ID                    });                    cmd.Parameters.Add(new SqlParameter("@Type",                  SqlDbType.VarChar)                    {                        Value = Template                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                    var datatype = dataReader.GetFieldType(iFiled);                                    if (datatype.Name == "DateTime")                                    {                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else if (datatype.Name == "Decimal")                                    {                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else                                    {                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());                                    }                                }                            }                        }                    }                    catch (Exception ex)                    {                        return ex.Message;                    }                }                if (val.Count > 0)
                {



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
                    alldetails = _FOContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();

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
                        //smstpdetails = _FOContext.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                        string mailcc = "";
                        string mailbcc = "";

                        //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        //{
                        //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        //    mailcc = ccmail[0].ToString();

                        //    if (ccmail.Length > 1)
                        //    {
                        //        if (ccmail[1] != null || ccmail[1] != "")
                        //        {
                        //            mailbcc = ccmail[1].ToString();
                        //        }
                        //    }

                        //}
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }



                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email);

                        // mailto:message.addto("kavitajnandoor21@gmail.com");
                        //message.AddTo("sreepriyagangadharan15@gmail.com");

                        if (Attechement.Equals("1"))
                        {
                            var img = _FOContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                        //****************** EMAIL CC DETAILS ***************//

                        if (ccemail != null && ccemail != "")
                        {
                            string[] ccmaildetails = ccemail.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }


                        //if (mailcc != null && mailcc != "")
                        //{
                        //    string[] ccmaildetails = mailcc.Split(',');

                        //    foreach (var c in ccmaildetails)
                        //    {
                        //        message.AddCc(c);
                        //    }
                        //}

                        //if (ccemail != null && ccemail != "")
                        //{
                        //    message.AddBcc(ccemail);
                        //}

                        //****************** EMAIL BCC DETAILS ***************//




                        if (mailcc != null && mailcc != "")
                        {
                            //message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

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



                        using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _FOContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();

                            var moduleid = _FOContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _FOContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_Email_Outgoing";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmailId",
                                SqlDbType.NVarChar)
                            {
                                Value = email
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
                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return "success";        }


        public BirthDayDTO iemonthendreport(BirthDayDTO data)
        {
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Client_MERReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMCLMER_Id", SqlDbType.VarChar) { Value = data.ISMCLMER_Id });
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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.monthendreportotherdata = retObject.ToArray();
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

        public BirthDayDTO.LopDetails getBasicData(BirthDayDTO.LopDetails dto)
        {

            try
            {

                dto.employees = (from b in _HRMSContext.MasterEmployee
                                 from c in _HRMSContext.HR_Master_Department
                                 where (b.HRME_ActiveFlag == true && b.HRMD_Id == c.HRMD_Id)
                                 select new BirthDayDTO.LopDetails
                                 {

                                     HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                     HRME_Id = b.HRME_Id,
                                     DepartmentName = c.HRMD_DepartmentName
                                 }).Distinct().ToArray();

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Additional_LOPGridDetails";
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.griddata = retObject.ToArray();
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
        public BirthDayDTO.LopDetails Lopsave(BirthDayDTO.LopDetails dto)
        {

            try
            {

                var MI_Id = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == dto.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();

                if (dto.HRALD_ID > 0)
                {
                    var update = _HRMSContext.Lop_DetailsDMO.Where(t => t.HRALD_ID == dto.HRALD_ID).ToList().FirstOrDefault();
                    update.HRME_Id = dto.HRME_Id;
                    update.HRALD_Type = dto.HRALD_Type;
                    update.HRALD_Remarks = dto.HRALD_Remarks;
                    update.HRALD_Date = dto.lopdate;
                    update.HRALD_File = dto.HRALD_File;
                    update.HRALD_Activeflag = true;
                    update.Createddate = DateTime.Now;
                    update.Updateddate = DateTime.Now;
                    update.Createdby = dto.userid;
                    update.Updatedby = dto.userid;
                    _HRMSContext.Add(update);
                    var contactExistsP = _HRMSContext.SaveChanges();
                    if (contactExistsP > 0)
                    {
                        dto.returnmsg = "Update";
                    }

                    else
                    {
                        dto.returnmsg = "false";
                    }
                }
                else
                {
                    Lop_DetailsDMO objlop = new Lop_DetailsDMO();
                    objlop.HRME_Id = dto.HRME_Id;
                    objlop.MI_Id = MI_Id;
                    objlop.HRALD_Type = dto.HRALD_Type;
                    objlop.HRALD_Remarks = dto.HRALD_Remarks;
                    objlop.HRALD_Date = dto.lopdate;
                    objlop.HRALD_File = dto.HRALD_File;
                    objlop.HRALD_Activeflag = true;
                    objlop.Createddate = DateTime.Now;
                    objlop.Updateddate = DateTime.Now;
                    objlop.Createdby = dto.userid;
                    objlop.Updatedby = dto.userid;
                    _HRMSContext.Add(objlop);
                    var contactExistsP = _HRMSContext.SaveChanges();
                    if (contactExistsP > 0)
                    {
                        dto.returnmsg = "Add";
                    }

                    else
                    {
                        dto.returnmsg = "false";
                    }

                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public BirthDayDTO.LopDetails deactivate(BirthDayDTO.LopDetails dto)
        {
            try
            {
                var result = _HRMSContext.Lop_DetailsDMO.Where(t => t.HRALD_ID == dto.HRALD_ID).FirstOrDefault();
                if (result.HRALD_Activeflag == true)
                {
                    result.HRALD_Activeflag = false;
                    result.Updatedby = dto.Id;
                    result.Updateddate = DateTime.Now;
                    _HRMSContext.Update(result);
                    dto.returnmsg = "true";
                }
                else
                {
                    result.HRALD_Activeflag = true;
                    result.Updatedby = dto.Id;
                    result.Updateddate = DateTime.Now;
                    _HRMSContext.Update(result);
                    dto.returnmsg = "true";
                }
                _HRMSContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }



        public BirthDayDTO saveRc(BirthDayDTO rpt)
        {
            try
            {
                foreach (var item in rpt.rcLeaveRecords)
                {
                    var contactExistsP = _FOContext.Database.ExecuteSqlCommand("HR_ReconsiderLeave_Approval @p0,@p1,@p2,@p3,@p4,@p5,@p6", rpt.MI_Id, item.HRERLAP_Id, item.HRME_Id, item.HRERLAP_Remarks, item.HRERLAP_ApplicationStatus, item.HRERLAP_FromDate.ToString("yyyy-MM-dd"), item.HRERLAP_ToDate.ToString("yyyy-MM-dd"));
                    if (contactExistsP > 0) 
                    {
                        rpt.retrunMsg = "Success";      
                    }
                    else
                    {
                        rpt.retrunMsg = "NotSuccess";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }

        public BirthDayDTO proxydetails(BirthDayDTO rpt)
        {
            try
            {
                if(rpt.flags == "E")
                {
                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HR_Employee_Evaluation_Rating";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = rpt.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                        {
                            Value = rpt.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.VarChar)
                        {
                            Value = rpt.yearid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.BigInt)
                        {
                            Value = rpt.month
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = rpt.flags
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
                            rpt.ConsolidateSalaryReport = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                }

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_MonthEnd_Report_Proxy_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = rpt.HRME_Id
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
                        rpt.proxylist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }


    }

}
