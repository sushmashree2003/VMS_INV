using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;

namespace VisitorsManagementServiceHub.Services
{
    public class GetVisitorReportImpl : GetVisitorReportInterface
    {
        public VisitorsManagementContext visctxt;

        public GetVisitorReportImpl(VisitorsManagementContext context)
        {
            visctxt = context;
        }
        public async Task<GetVisitorReportDTO> report(GetVisitorReportDTO data)
        {

            try
            {
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

                var rolelist = visctxt.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).ToList();

                data.IVRMRT_Role = rolelist.FirstOrDefault().IVRMRT_Role.ToLower();

                if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    data.userName = "admin";
                }
                else if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    data.userName = "admin";
                }
                else if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("HR", StringComparison.OrdinalIgnoreCase))
                {
                    data.userName = "admin";
                }
                else
                {
                    data.userName = "";
                }

                if ((data.all1 == "1") || (data.all1 == "0"))
                {

                    if (data.all1 == "1")
                    {
                        data.month_id = "";
                        data.yearid = "";
                    }
                    else
                    {
                        data.fromdate = "";
                        data.todate = "";
                    }

                    using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Vistors_Report_VMS_Test"; //  Vistors_Report_New -- Vistors_Report_VMS
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                        {
                            Value = mi_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                      SqlDbType.VarChar)
                        {
                            Value = data.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                      SqlDbType.VarChar)
                        {
                            Value = data.todate
                        });
                        cmd.Parameters.Add(new SqlParameter("@year",
                     SqlDbType.VarChar)
                        {
                            Value = data.yearid
                        });
                        cmd.Parameters.Add(new SqlParameter("@months",
                     SqlDbType.VarChar)
                        {
                            Value = data.month_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@column",
                    SqlDbType.VarChar)
                        {
                            Value = data.txtbox
                        });
                        cmd.Parameters.Add(new SqlParameter("@value",
                    SqlDbType.VarChar)
                        {
                            Value = data.searchby
                        });
                        cmd.Parameters.Add(new SqlParameter("@userName",
                   SqlDbType.VarChar)
                        {
                            Value = data.userName
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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.viewlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    List<GetVisitorReportDTO> result = new List<GetVisitorReportDTO>();

                    using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "visitor_details_VMS"; //visitor_details_New
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = mi_ids });
                        cmd.Parameters.Add(new SqlParameter("@column", SqlDbType.VarChar) { Value = data.searchby });
                        cmd.Parameters.Add(new SqlParameter("@value", SqlDbType.VarChar) { Value = data.txtbox });
                        cmd.Parameters.Add(new SqlParameter("@userName", SqlDbType.VarChar) { Value = data.userName });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new GetVisitorReportDTO
                                    {
                                        AMVM_Name = dataReader["VMMV_VisitorName"].ToString(),
                                        AMVM_Contact_No = dataReader["VMMV_VisitorContactNo"].ToString(),
                                        AMVM_Emailid = dataReader["VMMV_VisitorEmailid"].ToString(),
                                        Date_Visit = Convert.ToDateTime(dataReader["VMMV_MeetingDateTime"].ToString()),
                                        Time_Visit = dataReader["VMMV_EntryDateTime"].ToString(),
                                        AMVM_Out_Time = dataReader["VMMV_ExitDateTime"].ToString(),
                                        AMVM_Status = dataReader["VMMV_CkeckedInOutStatus"].ToString(),
                                        AMVM_Purpose = dataReader["VMMV_MeetingPurpose"].ToString(),
                                    });
                                    data.viewlist = result.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<GetVisitorReportDTO> getGpsloaddata(GetVisitorReportDTO data)
        {

            try
            {
                var emp = (from a in visctxt.MasterEmployee
                           from b in visctxt.VMS_Attendance_GPSLocationDMO_con
                           where a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id
                           select new GetVisitorReportDTO
                           {
                               employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                  + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                  + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                               HRME_Id=a.HRME_Id
                           }).Distinct().ToArray();
                if(emp.Length>0)
                {
                    data.employeelist = emp;
                }
            }

            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public async Task<GetVisitorReportDTO> gpsreport(GetVisitorReportDTO dto)
        {

            try
            {
                string hrmeid = "0";
                if (dto.optionflag == "Employee")
                {

                   
                    foreach (var item in dto.employeearray)
                    {
                        hrmeid = hrmeid + "," + item.HRME_Id;
                    }
                   
                }
              
                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AttendanceGPSLocationReport"; //  Vistors_Report_New
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                 SqlDbType.VarChar)
                    {
                        Value = hrmeid
                    });

                     cmd.Parameters.Add(new SqlParameter("@optionflag",
                 SqlDbType.VarChar)
                    {
                        Value = dto.optionflag
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.attendancegpslist = retObject.ToArray();
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

        public async Task<GetVisitorReportDTO> getTenantVisitorReport(GetVisitorReportDTO dto)
        {

            try
            {
                string miids = "0";

                foreach (var item in dto.selected_Inst)
                {
                    miids = miids + "," + item.MI_Id;
                }
                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tenant_Visitor_report"; //  Vistors_Report_New
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = miids
                    });
                    cmd.Parameters.Add(new SqlParameter("@year",
                   SqlDbType.VarChar)
                    {
                        Value = dto.yearid
                    });
                    cmd.Parameters.Add(new SqlParameter("@months",
              SqlDbType.VarChar)
                    {
                        Value = dto.month_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.VarChar)
                    {
                        Value = dto.todate
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.attendancegpslist = retObject.ToArray();
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
