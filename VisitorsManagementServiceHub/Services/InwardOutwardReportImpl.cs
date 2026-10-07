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
    public class InwardOutwardReportImpl : InwardOutwardReportInterface
    {
        public VisitorsManagementContext visctxt;
        public InwardOutwardReportImpl(VisitorsManagementContext context)
        {
            visctxt = context;
        }
        public async Task<InwardOutwardReportDTO> report(InwardOutwardReportDTO data)
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
                    cmd.CommandText = "Inward_Outward_Report_VMS_TEST";//  Inward_Outward_Report_New // Inward_Outward_Report_VMS
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@radiotype",
                   SqlDbType.VarChar)
                    {
                        Value = data.radiotype
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
                    cmd.Parameters.Add(new SqlParameter("@yearid",
                SqlDbType.VarChar)
                    {
                        Value = data.yearid
                    });
                    cmd.Parameters.Add(new SqlParameter("@months",
                 SqlDbType.VarChar)
                    {
                        Value = data.month_id
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
