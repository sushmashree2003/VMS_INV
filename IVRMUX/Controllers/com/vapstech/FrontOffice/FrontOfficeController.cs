using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FrontOfficeController : Controller
    {
        FrontOfficeDelegate dele = new FrontOfficeDelegate();

        private readonly IssueManagerContext _IssueContext;
        public FrontOfficeController(IssueManagerContext isc)
        {
            _IssueContext = isc;
        }

        // GET: api/values
        [HttpGet("{id:int}")]
        public BirthDayDTO GetData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            BirthDayDTO data = new BirthDayDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.getdata(data);

            //return dele.getdata(id);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("consolidatedemployeemonthendreport")]
        public ISM_InterDepartmentReport consolidatedemployeemonthendreport([FromBody]ISM_InterDepartmentReport data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //return _objdel.getreport(data);

            data.HRMD_Id = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == data.HRME_Id && t.HRME_ActiveFlag == true).Select(t => t.HRMD_Id.Value).FirstOrDefault();

            data.get_department = (from a in _IssueContext.MasterEmployee
                                   where (a.HRME_Id == data.HRME_Id && a.HRME_ActiveFlag == true)
                                   select new BirthDayDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       regno = a.HRME_EmployeeCode,
                                       HRME_EmployeeFirstName = (a.HRME_EmployeeFirstName + ' ' + a.HRME_EmployeeMiddleName + ' ' + a.HRME_EmployeeLastName)

                                   }).ToArray();

            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";
                data.successcount = 0;
                try
                {


                    if (data.rdbbutton == "Month")
                    {
                        string startdate = data.yearid + "-" + data.month + "-01";
                        data.startdate = Convert.ToDateTime(startdate);
                        var year = Convert.ToDateTime(startdate).Year;
                        var mon = Convert.ToDateTime(startdate).Month;
                        var startOfMonth = new DateTime(year, mon, 1);
                        var DaysInMonth = DateTime.DaysInMonth(year, mon);
                        data.enddate = new DateTime(year, mon, DaysInMonth);

                        fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
                        confromdate = fromdatecon.ToString("yyyy-MM-dd");

                        toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
                        contodate = toatecon.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
                        confromdate = fromdatecon.ToString("yyyy-MM-dd");

                        toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
                        contodate = toatecon.ToString("yyyy-MM-dd");
                    }
                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (data.reporttype == "E")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "EmployeeMonthEndReport_E_Tasklist";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Startdate",
                 SqlDbType.VarChar)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                 SqlDbType.VarChar)
                        {
                            Value = contodate
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                 SqlDbType.BigInt)
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.Departmentempissues = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            data.successcount = data.successcount + 1;
                            Console.WriteLine(ex.Message);
                        }
                    }


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "EmployeeMonthEndReport_E_Tasklist_View";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Startdate",
                 SqlDbType.VarChar)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                 SqlDbType.VarChar)
                        {
                            Value = contodate
                        });
                        cmd.Parameters.Add(new SqlParameter("@Status",
                 SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@deprt",
                 SqlDbType.BigInt)
                        {
                            Value = data.HRMD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                 SqlDbType.BigInt)
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.Departmentempissuesview = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            data.successcount = data.successcount + 1;
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "Exit_EMployeeMonthendReport_Overall";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Startdate",
                 SqlDbType.VarChar)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                 SqlDbType.VarChar)
                        {
                            Value = contodate
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                 SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FLAG",
                 SqlDbType.Int)
                        {
                            Value = 1
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
                            data.overallempissuesall = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "Exit_EMployeeMonthendReport_Overall";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Startdate",
                 SqlDbType.VarChar)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                 SqlDbType.VarChar)
                        {
                            Value = contodate
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                 SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FLAG",
                 SqlDbType.Int)
                        {
                            Value = 2
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
                            data.temp_overallempissuesall = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "ISM_EMPLOYEE_MEMO_NOTICE_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Startdate",
                 SqlDbType.VarChar)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                 SqlDbType.VarChar)
                        {
                            Value = contodate
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                 SqlDbType.BigInt)
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.overallempmemonotice = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "HR_Management_Attendance";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                        {
                            Value = confromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                        {
                            Value = contodate
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
                            data.empmanagementdetailreport = retObject.ToArray();
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

        [Route("getmonthreport")]
        public BirthDayDTO getmonthreport([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return dele.getmonthreport(data);
        }

        [Route("getlunchreport")]
        public BirthDayDTO getlunchreport([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getlunchreport(data);
        }

        [Route("savereport")]
        public BirthDayDTO savereport([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.savereport(data);
        }

        [Route("GetFinalReport")]
        public BirthDayDTO GetFinalReport([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.GetFinalReport(data);
        }

        [Route("SendFinalMail")]
        public BirthDayDTO SendFinalMail([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.SendFinalMail(data);
        }

        [Route("SendFinalMailMD")]
        public BirthDayDTO SendFinalMailMD([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.SendFinalMailMD(data);
        }

        [Route("saveMDdata")]
        public BirthDayDTO saveMDdata([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.saveMDdata(data);
        }

        [Route("sendmailAcc")]
        public BirthDayDTO sendmailAcc([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.sendmailAcc(data);
        }

        [Route("Notapprovedstatus")]
        public BirthDayDTO Notapprovedstatus([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.Notapprovedstatus(data);
        }

        [Route("DRWaveofReport")]
        public BirthDayDTO DRWaveofReport([FromBody] BirthDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.DRWaveofReport(data);
        }

        [HttpGet]
        [Route("getdataapprover/{id:int}")]
        public BirthDayDTO getdataapprover([FromQuery] int id)
        {
            BirthDayDTO dto = new BirthDayDTO();

            //dto.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.roleId = roleidd;

            return dele.getdataapprover(dto);
        }
        [Route("getmonthreportforapproal")]
        public BirthDayDTO getmonthreportforapproal([FromBody] BirthDayDTO data)
        {
            //  data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.getmonthreportforapproal(data);
        }
        [Route("attendenceapprove")]
        public BirthDayDTO attendenceapprove([FromBody] BirthDayDTO data)
        {
            // data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.attendenceapprove(data);
        }
        [Route("attendenceapprovedlist")]
        public BirthDayDTO attendenceapprovedlist([FromBody] BirthDayDTO data)
        {
            //data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.attendenceapprovedlist(data);
        }

        //From here Unauthorised Leave Count Report (Aneeket)

        [Route("getalldetails/{id:int}")]

        public BirthDayDTO getalldetails(int id)
        {
            BirthDayDTO data = new BirthDayDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = mid;
            return dele.getdata1(data);
        }

        [HttpPost]
        [Route("unauthorizedreport")]
        public BirthDayDTO unauthorizedreport([FromBody]BirthDayDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return dele.unauthorizedreport(data);
        }
        [Route("getdepartment")]
        public BirthDayDTO getdepartment([FromBody]BirthDayDTO data)
        {
            return dele.getdepartment(data);
        }
        [Route("AdjustCompOff")]
        public BirthDayDTO AdjustCompOff([FromBody]BirthDayDTO data)
        {
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.AdjustCompOff(data);
        }

        [Route("iemonthendreport")]
        public BirthDayDTO iemonthendreport([FromBody]BirthDayDTO data)
        {
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.iemonthendreport(data);
        }

        [HttpGet]
        [Route("getalldetails1/{id:int}")]
        public BirthDayDTO.LopDetails getalldetails1(int id)
        {
            BirthDayDTO.LopDetails dto = new BirthDayDTO.LopDetails();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return dele.onloadgetdetails(dto);
        }

        [Route("Lopsave")]
        public BirthDayDTO.LopDetails Lopsave([FromBody]BirthDayDTO.LopDetails data)
        {
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.Lopsave(data);
        }

        [Route("deactivate")]
        public BirthDayDTO.LopDetails deactivate([FromBody]BirthDayDTO.LopDetails data)
        {
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.deactivate(data);
        }
        [Route("saveRc")]
        public BirthDayDTO saveRc([FromBody] BirthDayDTO data)
        {
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.saveRc(data);
        }


        [Route("proxydetails")]
        public BirthDayDTO proxydetails([FromBody] BirthDayDTO data)
        {
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.proxydetails(data);
        }

    }
}
