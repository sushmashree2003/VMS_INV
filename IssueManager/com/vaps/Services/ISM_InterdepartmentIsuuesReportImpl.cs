using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace IssueManager.com.vaps.Services
{
    public class ISM_InterdepartmentIsuuesReportImpl : Interfaces.ISM_InterdepartmentIsuuesReportInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_InterdepartmentIsuuesReportImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;

        }
        public async Task<ISM_InterDepartmentReport> getdetails(ISM_InterDepartmentReport data)
        {
            try
            {
                data.get_department = (from a in _IssueContext.HR_Master_DepartmentCodeDMO                                    
                                       select new ISM_TaskCreationDTO
                                       {
                                           HRMD_Id = a.HRMDC_ID,
                                           HRMD_DepartmentName = a.HRMDC_Name
                                       }).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //public ISM_InterDepartmentReport getreport(ISM_InterDepartmentReport data)
        //{
        //    try
        //    {
        //        DateTime fromdatecon = DateTime.Now;
        //        DateTime toatecon = DateTime.Now;
        //        string confromdate = "";
        //        string contodate = "";
        //        data.successcount = 0;
        //        try
        //        {
        //            fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
        //            confromdate = fromdatecon.ToString("yyyy-MM-dd");

        //            toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
        //            contodate = toatecon.ToString("yyyy-MM-dd");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        if (data.reporttype == "C")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.consolidatedissues= retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount+1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.consolidatedissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }   
        //        else if (data.reporttype == "I")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_I_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //        SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.InterDepartmentissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_I_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //        SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.InterDepartmentissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }
        //        else if ((data.reporttype == "E") || (data.reporttype == "O" && data.emparrayexist == false))
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }
        //        if (data.reporttype == "O")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesempissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //        }
        //        if ((data.reporttype == "A") ||(data.reporttype != "A" && data.allarrayexist == true))
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "Temp_InterDepartment_E_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.temp_epartmentempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_View_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesviewall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_ALL";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "Temp_OverAllDepartment_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.temp_overallempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_View_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesempissuesviewall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }

        //            if ((data.Departmentempissuesall != null && data.Departmentempissuesall.Length > 0) && (data.overallempissuesall != null && data.overallempissuesall.Length > 0) && data.reporttype == "A")
        //            {
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_I_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "All"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.InterDepartmentissues = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_I_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "View"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.InterDepartmentissuesview = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "All"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.consolidatedissues = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "View"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.consolidatedissuesview = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}       

        public ISM_InterDepartmentReport SendEmail(ISM_InterDepartmentReport data)
        {
            try
            {
                var res = sendmail(data.MI_Id, "pstomd@vapstech.com", "DEVIATIONMAIL", data.Emailcontent);
                data.result = res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string sendmail(long MI_Id, string Email, string Template, string msg)
        {
            try
            {
               
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
       
                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
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
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;

           
                        result = Mailmsg.Replace("[MSG]", msg);
                        Mailmsg = result;
                   

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
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
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string emailccdetails = "";
                    string emailbccdetails = "";
                  

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        if (Attechement.Equals("1"))
                        {
                            var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
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

                        
                            if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                            {
                                string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                                foreach (var c in ccmaildetails)
                                {
                                    if (c != Email)
                                    {
                                        message.AddCc(c);
                                    }

                                }
                            }

                            if (emailbccdetails != null && emailbccdetails != "")
                            {
                                string[] bccmaildetails = emailbccdetails.Split(',');

                                foreach (var bc in bccmaildetails)
                                {
                                    if (bc != Email)
                                    {
                                        message.AddBcc(bc);
                                    }
                                }
                            }
                  

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();

                        
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            if (modulename.Count == 0)
                            {
                                modulename[0] = "Issue Manager";
                            }

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
                                Value =msg
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
                                return ex.Message;
                            }
                        }
                    }
                    else
                    {

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }
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
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(SendingEmail, institutionName[0].MI_Name);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                if (Attechement.Equals("1"))
                                {
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {
                                            foreach (var attache in img.ToList())
                                            {
                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }
                                        }
                                    }
                                }


                                    if (emailccdetails != null && emailccdetails != "")
                                    {
                                        string[] ccmaildetails = emailccdetails.Split(',');

                                        foreach (var c in ccmaildetails)
                                        {
                                            if (c != Email)
                                            {
                                                emailMessage.CC.Add(c);
                                            }
                                        }
                                    }

                                    if (emailbccdetails != null && emailbccdetails != "")
                                    {
                                        string[] bccmaildetails = emailbccdetails.Split(',');

                                        foreach (var bc in bccmaildetails)
                                        {
                                            if (bc != Email)
                                            {
                                                emailMessage.Bcc.Add(bc);
                                            }
                                        }
                                    }
                               

                                clientsmtp.Send(emailMessage);

                            
                                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                {
                                    var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                                    var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                                    var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                    if (modulename.Count == 0)
                                    {
                                        modulename[0] = "Issue Manager";
                                    }

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
                                        Value = msg
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
                                        return ex.Message;
                                    }
                                }
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


        //public ISM_InterDepartmentReport getreport(ISM_InterDepartmentReport data)
        //{
        //    try
        //    {
        //        DateTime fromdatecon = DateTime.Now;
        //        DateTime toatecon = DateTime.Now;
        //        string confromdate = "";
        //        string contodate = "";
        //        data.successcount = 0;
        //        try
        //        {
        //            fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
        //            confromdate = fromdatecon.ToString("yyyy-MM-dd");

        //            toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
        //            contodate = toatecon.ToString("yyyy-MM-dd");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        if (data.reporttype == "C")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.consolidatedissues= retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount+1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.consolidatedissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }   
        //        else if (data.reporttype == "I")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_I_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //        SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.InterDepartmentissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_I_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //        SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.InterDepartmentissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }
        //        else if ((data.reporttype == "E") || (data.reporttype == "O" && data.emparrayexist == false))
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }
        //        if (data.reporttype == "O")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesempissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //        }
        //        if ((data.reporttype == "A") ||(data.reporttype != "A" && data.allarrayexist == true))
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "Temp_InterDepartment_E_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.temp_epartmentempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_View_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesviewall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_ALL";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "Temp_OverAllDepartment_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.temp_overallempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_View_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesempissuesviewall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }

        //            if ((data.Departmentempissuesall != null && data.Departmentempissuesall.Length > 0) && (data.overallempissuesall != null && data.overallempissuesall.Length > 0) && data.reporttype == "A")
        //            {
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_I_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "All"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.InterDepartmentissues = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_I_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "View"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.InterDepartmentissuesview = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "All"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.consolidatedissues = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "View"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.consolidatedissuesview = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}       

        //public ISM_InterDepartmentReport getreport(ISM_InterDepartmentReport data)
        //{
        //    try
        //    {
        //        DateTime fromdatecon = DateTime.Now;
        //        DateTime toatecon = DateTime.Now;
        //        string confromdate = "";
        //        string contodate = "";
        //        data.successcount = 0;
        //        try
        //        {
        //            fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
        //            confromdate = fromdatecon.ToString("yyyy-MM-dd");

        //            toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
        //            contodate = toatecon.ToString("yyyy-MM-dd");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        if (data.reporttype == "C")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.consolidatedissues= retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount+1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.consolidatedissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }   
        //        else if (data.reporttype == "I")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_I_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //        SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.InterDepartmentissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_I_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //        SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.InterDepartmentissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }
        //        else if ((data.reporttype == "E") || (data.reporttype == "O" && data.emparrayexist == false))
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //        }
        //        if (data.reporttype == "O")
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissues = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_View";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@deprt",
        //         SqlDbType.BigInt)
        //                {
        //                    Value = data.HRMD_Id
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesempissuesview = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //        }
        //        if ((data.reporttype == "A") ||(data.reporttype != "A" && data.allarrayexist == true))
        //        {
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "Temp_InterDepartment_E_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.temp_epartmentempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "InterDepartment_E_Tasklist_View_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.Departmentempissuesviewall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    data.successcount = data.successcount + 1;
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_ALL";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "Temp_OverAllDepartment_Tasklist_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.temp_overallempissuesall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }
        //            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandText = "OverAllDepartment_Tasklist_View_All";
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@Startdate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = confromdate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@EndDate",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = contodate
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@Status",
        //         SqlDbType.VarChar)
        //                {
        //                    Value = ""
        //                });
        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var retObject = new List<dynamic>();
        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                );
        //                            }
        //                            retObject.Add((ExpandoObject)dataRow);
        //                        }
        //                    }
        //                    data.overallempissuesempissuesviewall = retObject.ToArray();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    data.successcount = data.successcount + 1;
        //                }
        //            }

        //            if ((data.Departmentempissuesall != null && data.Departmentempissuesall.Length > 0) && (data.overallempissuesall != null && data.overallempissuesall.Length > 0) && data.reporttype == "A")
        //            {
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_I_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "All"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.InterDepartmentissues = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_I_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "View"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.InterDepartmentissuesview = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "All"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.consolidatedissues = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandTimeout = 0;
        //                    cmd.CommandText = "InterDepartment_Tasklist_All";
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    cmd.Parameters.Add(new SqlParameter("@Type",
        //             SqlDbType.VarChar)
        //                    {
        //                        Value = "View"
        //                    });
        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var retObject = new List<dynamic>();
        //                    try
        //                    {
        //                        using (var dataReader = cmd.ExecuteReader())
        //                        {
        //                            while (dataReader.Read())
        //                            {
        //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                {
        //                                    dataRow.Add(
        //                                        dataReader.GetName(iFiled),
        //                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
        //                                    );
        //                                }
        //                                retObject.Add((ExpandoObject)dataRow);
        //                            }
        //                        }
        //                        data.consolidatedissuesview = retObject.ToArray();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine(ex.Message);
        //                        data.successcount = data.successcount + 1;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}       


    }
}
