using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class VmsAuditReportsIMPL : Interfaces.VmsAuditReportsInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;

        public VmsAuditReportsIMPL(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }


        public VmsAuditReportsDTO getalldata(VmsAuditReportsDTO data)
        {
            try
            {
                var rolet = _context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = false;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase) || roletype.Equals("Accountant", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                 
                }
                else
                {
                    roletype = "";
                    data.roleflag = false;
                }
                
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_AuditReports";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
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
                        data.MI_List = retObject.ToArray();

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

        // Getdata
        public VmsAuditReportsDTO Getdata(VmsAuditReportsDTO data)
        {
            try
            {
                string comapanylist = "0";
                if (data.comapanylist.Length > 0)
                {
                    foreach (var ue in data.comapanylist)
                    {
                        comapanylist = comapanylist + "," + ue.MI_Id;
                    }
                }
                var rolet = _context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = false;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase) || roletype.Equals("Accountant", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }

                }
                else
                {
                    roletype = "";
                    data.roleflag = false;
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_AuditReports_Department";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = comapanylist
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                    {
                        Value = comapanylist
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
                        data.departmentdropdown = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                // VMS_AuditReports Department

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public VmsAuditReportsDTO TadaAllwence(VmsAuditReportsDTO data)
        {
            return data;
        }


        public VmsAuditReportsDTO savedta(VmsAuditReportsDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        //edit
        public VmsAuditReportsDTO edit(VmsAuditReportsDTO data)

        {
            try
            {
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public VmsAuditReportsDTO deactive(VmsAuditReportsDTO data)
        {




            return data;
        }

    }
}
