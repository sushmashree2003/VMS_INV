using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class EmployeeLogReportImpl:Interfaces.EmployeeLogReportInterface
    {
        public FOContext _FOContext;
        private object multipletype;


        public EmployeeLogReportImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }

        public EmployeeLogReportDTO getdata(EmployeeLogReportDTO data)
        {
            try
            {
                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); //
                data.filltypes = staf_types.Distinct().ToArray();

                var q = (from a in _FOContext.holidaydate
                         where (a.MI_Id == data.MI_Id && a.FOMHWD_ActiveFlg == true)
                         select new
                         {
                             monthid = a.FOMHWDD_FromDate.Value.Month,
                             monthname = Convert.ToDateTime(a.FOMHWDD_FromDate).ToString("MMMMM").ToString()
                         }).Distinct().ToArray();
                var query = q.Distinct().ToArray();

                data.fillmonth = (from a in query
                                  select new EmployeeLogReportDTO
                                  {
                                      monthid = a.monthid,
                                      monthname = a.monthname
                                  }).Distinct().OrderBy(t => t.monthid).ToArray();


                data.fillyear = (from a in _FOContext.HR_Master_LeaveYearDMO
                                 where (a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                 select new HR_Master_LeaveYearDTO
                                 {
                                     HRMLY_Id = Convert.ToInt32(a.HRMLY_Id),
                                     HRMLY_LeaveYear=a.HRMLY_LeaveYear,
                                     HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                 }).Distinct().OrderBy(t=>t.HRMLY_LeaveYearOrder).ToArray();

                //ie log report
                data.hrmeid = (from a in _FOContext.ApplicationUserDMO
                               from b in _FOContext.Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.userId)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.employeename = (from a in _FOContext.HR_Master_DepartmentCode_Head
                                     from c in _FOContext.HR_Master_Department_DMO
                                     from d in _FOContext.HR_Master_Employee_DMO
                                     where (a.HRME_ID == data.hrmeid && a.HRMDC_ID == c.HRMDC_ID && c.HRMD_Id == d.HRMD_Id && d.HRME_ActiveFlag == true )
                                     select new EmployeeLogReportDTO
                                     {
                                         HRME_Id = d.HRME_Id,
                                         EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName)
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeLogReportDTO get_departments(EmployeeLogReportDTO data)
        {
            var dd = data.multipletype.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                   from b in _FOContext.HR_Master_Department_DMO
                                   from c in _FOContext.HR_Master_GroupType_DMO
                                   where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
                                       && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMGT_Id))
                                   select new EmployeeLogReportDTO
                                   {
                                       HRMD_Id = b.HRMD_Id,
                                       HRMD_DepartmentName = b.HRMD_DepartmentName,
                                   }
                      ).Distinct().ToArray();

            return data;
        }

        public EmployeeLogReportDTO get_designation(EmployeeLogReportDTO data)
        {
            var dd = data.multipledep.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }

            data.filldesignation = (from a in _FOContext.HR_Master_Employee_DMO
                                    from b in _FOContext.HR_Master_Designation_DMO
                                    from c in _FOContext.HR_Master_Department_DMO
                                    where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                    && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                    && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMD_Id))
                                    select new EmployeeLogReportDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
        public EmployeeLogReportDTO get_employee(EmployeeLogReportDTO data)
        {
            var desig = data.multipledes.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < desig.Length; i++)
            {
                list.Add(Convert.ToInt64(desig[i]));
            }
            var dept = data.multipledep.Split(',');
            List<long> list2 = new List<long>();
            for (int i = 0; i < dept.Length; i++)
            {
                list2.Add(Convert.ToInt64(dept[i]));
            }
            var typ = data.multipletype.Split(',');
            List<long> list3 = new List<long>();
            for (int i = 0; i < typ.Length; i++)
            {
                list3.Add(Convert.ToInt64(typ[i]));
            }
            data.fillemployee = (from a in _FOContext.HR_Master_Employee_DMO
                                 where (a.MI_Id == data.MI_Id && list.Contains(a.HRMDES_Id) && list3.Contains(a.HRMGT_Id) && list2.Contains(a.HRMD_Id) && a.HRME_ActiveFlag == true )
                                 select new EmployeeLogReportDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' '
                                                + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                     //HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                     //HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                     HRME_EmployeeCode = a.HRME_EmployeeCode,
                                     HRME_LeftFlag = a.HRME_LeftFlag,
                                 }
                     ).Distinct().ToArray();

            return data;
        }
        public async Task<EmployeeLogReportDTO> getreport(EmployeeLogReportDTO data)
        {

            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "Fo_Emp_Log_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.VarChar)
                    {
                        Value = data.selectdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                       SqlDbType.VarChar)
                    {
                        Value = data.selectmonth
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                   SqlDbType.VarChar)
                    {
                        Value = data.selectyear
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                   SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.NVarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                  SqlDbType.VarChar)
                    {
                        Value = data.multiplehrmeid
                    });

                    cmd.Parameters.Add(new SqlParameter("@punchtype",
                  SqlDbType.VarChar)
                    {
                        Value = data.punchtype
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
                                        dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.filldata = retObject1.ToArray();

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


        public EmployeeLogReportDTO IEeLearningLogreport(EmployeeLogReportDTO data)
        {
            try
            {
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "staff_login_report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@hrme_id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@month_id", SqlDbType.BigInt)
                    {
                        Value = data.monthid
                    });
                    cmd.Parameters.Add(new SqlParameter("@year_id", SqlDbType.BigInt)
                    {
                        Value = data.HRMLY_Id
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
                        data.logindetails = retObject.ToArray();
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



        public EmployeeLogReportDTO getrptSave(EmployeeLogReportDTO data)
        {
            try
            {


                    var contactExistsP = _FOContext.Database.ExecuteSqlCommand("insertvmsattendancefileupload @p0,@p1,@p2,@p3,@p4",
                          data.afu_filename, data.afu_filepath, data.afu_date, data.userId,data.HRRemarks);
                    if (contactExistsP > 0)
                    {
                        data.Message = "Add";
                    }
                    else
                    {
                        data.Message = "Error";
                    }



                //using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "insertvmsattendancefileupload";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@afu_filename", SqlDbType.VarChar)
                //    {
                //        Value = data.afu_filename
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@afu_filepath", SqlDbType.VarChar)
                //    {
                //        Value = data.afu_filepath
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@afu_date", SqlDbType.DateTime)
                //    {
                //        Value = data.afu_date
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt)
                //    {
                //        Value = data.userId
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
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.AllFOHRAttendence = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return data;
        }


        public EmployeeLogReportDTO getDailyAttendanceRprt(EmployeeLogReportDTO data)
        {
            try
            {

                DateTime? startDate = data.START_DATE;
                var sdate = startDate?.ToString("yyyy-MM-dd") ?? "NA";

                DateTime? EndDate = data.END_DATE;
                var edate = EndDate?.ToString("yyyy-MM-dd") ?? "NA";

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Attendance_File_Upload_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = sdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                    {
                        Value = edate
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
                        data.AllFOHRAttendenceReportMD = retObject.ToArray();
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


    }
}
