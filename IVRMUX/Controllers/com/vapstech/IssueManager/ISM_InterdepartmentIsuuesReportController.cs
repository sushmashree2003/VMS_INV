using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_InterdepartmentIsuuesReportController : Controller 
    {
        public ISM_InterdepartmentIsuuesReportDelegate _objdel = new ISM_InterdepartmentIsuuesReportDelegate();
        private readonly IssueManagerContext _IssueContext;
        public ISM_InterdepartmentIsuuesReportController(IssueManagerContext isc)
        {
            _IssueContext = isc;
        }

        [Route("getdetails/{id:int}")]
        public ISM_InterDepartmentReport getdetails(int id)
        {
            ISM_InterDepartmentReport data = new ISM_InterDepartmentReport();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getdetails(data);
        }

        [Route("getreport")]
        public ISM_InterDepartmentReport getreport([FromBody]ISM_InterDepartmentReport data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //return _objdel.getreport(data);

            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";
                data.successcount = 0;
                try
                {
                    fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
                    contodate = toatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (data.reporttype == "C")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "InterDepartment_Tasklist";
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
                            data.consolidatedissues = retObject.ToArray();
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
                        cmd.CommandText = "InterDepartment_Tasklist_View";
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
                            data.consolidatedissuesview = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            data.successcount = data.successcount + 1;
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.reporttype == "I")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "InterDepartment_I_Tasklist";
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
                            data.InterDepartmentissues = retObject.ToArray();
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
                        cmd.CommandText = "InterDepartment_I_Tasklist_View";
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
                            data.InterDepartmentissuesview = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            data.successcount = data.successcount + 1;
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if ((data.reporttype == "E") || (data.reporttype == "O" && data.emparrayexist == false))
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "InterDepartment_E_Tasklist";
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
                        cmd.CommandText = "InterDepartment_E_Tasklist_View";
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
                }
                if (data.reporttype == "O")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "OverAllDepartment_Tasklist";
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
                            data.overallempissues = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            data.successcount = data.successcount + 1;
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "OverAllDepartment_Tasklist_View";
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
                            data.overallempissuesempissuesview = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            data.successcount = data.successcount + 1;
                        }
                    }
                }
                if ((data.reporttype == "A") || (data.reporttype != "A" && data.allarrayexist == true))
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "InterDepartment_E_Tasklist_All";
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
                            data.Departmentempissuesall = retObject.ToArray();
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
                        cmd.CommandText = "Temp_InterDepartment_E_Tasklist_All";
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
                            data.temp_epartmentempissuesall = retObject.ToArray();
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
                        cmd.CommandText = "InterDepartment_E_Tasklist_View_All";
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
                            data.Departmentempissuesviewall = retObject.ToArray();
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
                        cmd.CommandText = "OverAllDepartment_Tasklist_ALL";
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
                            data.successcount = data.successcount + 1;
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "Temp_OverAllDepartment_Tasklist_All";
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
                            data.successcount = data.successcount + 1;
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "OverAllDepartment_Tasklist_View_All";
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
                            data.overallempissuesempissuesviewall = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            data.successcount = data.successcount + 1;
                        }
                    }

                    if ((data.Departmentempissuesall != null && data.Departmentempissuesall.Length > 0) && (data.overallempissuesall != null && data.overallempissuesall.Length > 0) && data.reporttype == "A")
                    {
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandText = "InterDepartment_I_Tasklist_All";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                            {
                                Value = "All"
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
                                data.InterDepartmentissues = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                data.successcount = data.successcount + 1;
                            }
                        }
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandText = "InterDepartment_I_Tasklist_All";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                            {
                                Value = "View"
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
                                data.InterDepartmentissuesview = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                data.successcount = data.successcount + 1;
                            }
                        }
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandText = "InterDepartment_Tasklist_All";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                            {
                                Value = "All"
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
                                data.consolidatedissues = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                data.successcount = data.successcount + 1;
                            }
                        }
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandText = "InterDepartment_Tasklist_All";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                            {
                                Value = "View"
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
                                data.consolidatedissuesview = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                data.successcount = data.successcount + 1;
                            }
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

        [Route("SendEmail")]
        public ISM_InterDepartmentReport SendEmail([FromBody]ISM_InterDepartmentReport data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.SendEmail(data);


        }
    }
}
