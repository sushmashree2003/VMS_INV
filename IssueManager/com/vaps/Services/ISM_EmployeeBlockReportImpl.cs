using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
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
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_EmployeeBlockReportImpl : Interfaces.ISM_EmployeeBlockReportInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_EmployeeBlockReportImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_BlockEmployeeDTO> getempblockReport(ISM_BlockEmployeeDTO data)
        {
            try
            {

                string empids = "0";
                if (data.temp_Employee != null)
                {
                    foreach (var e in data.temp_Employee)
                    {
                        empids = empids + "," + e.HRME_Id;
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_EmployeeBlock_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TypeFlg",
                    SqlDbType.VarChar)
                    {
                        Value = data.TypeFlg
                    });
                    cmd.Parameters.Add(new SqlParameter("@SelectionFlag",
                    SqlDbType.VarChar)
                    {
                        Value = data.SelectionFlag
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate",
                   SqlDbType.DateTime)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate",
                    SqlDbType.DateTime)
                    {
                        Value = data.enddate
                    });
                  
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.HRMD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = empids
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
                    SqlDbType.VarChar)
                    {
                        Value = data.userid
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
                        data.get_empblockReport = retObject.ToArray();
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
        //getempblockNewReport
        public async Task<ISM_BlockEmployeeDTO> getempblockNewReport(ISM_BlockEmployeeDTO data)
        {
            try
            {
               
                string empids = "0";
                if (data.temp_Employee != null)
                {
                    foreach (var e in data.temp_Employee)
                    {
                        empids = empids + "," + e.HRME_Id;
                    }
                }
                string department = "0";

                if (data.temp_Employee != null)
                {
                    foreach (var e in data.dept)
                    {
                        department = department + "," + e.HRMDC_ID;
                    }
                }
                string FromDate = ""; string ToDate = "";
                if (data.startdate != null && data.enddate != null)
                {
                    FromDate = data.startdate.Value.ToString("yyyy-MM-dd");
                    ToDate = data.enddate.Value.ToString("yyyy-MM-dd");

                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_EMPLOYEES_BLOCKED_TASKS_LISTTWO";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@department",
                    SqlDbType.VarChar)
                    {
                        Value = department
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = empids
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate",
                   SqlDbType.VarChar)
                    {
                        Value = FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate",
                    SqlDbType.VarChar)
                    {
                        Value = ToDate
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
                        data.get_empblockReport = retObject.ToArray();
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
