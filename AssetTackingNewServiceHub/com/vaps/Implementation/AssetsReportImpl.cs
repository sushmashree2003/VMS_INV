using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class AssetsReportImpl : Interface.AssetsReportInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetsReportImpl> _logAT;
        public AssetsReportImpl(AssetTrackingContext ATContext, ILogger<AssetsReportImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public async Task<CheckOutAssetsDTO> getloaddata(CheckOutAssetsDTO data)
        {
            try
            {
                var stfck1 = _ATContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                var stfcheck1 = _ATContext.applicationRole.Where(a => a.Id == stfck1[0].RoleId).ToList();
                data.staffcheck = stfcheck1[0].Name;

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_master_institution_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Userid",
            SqlDbType.VarChar)
                    {
                        Value = data.UserId
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
                        data.get_mi_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    if (stfcheck1[0].Name != "Staff")
                    {

                        data.MI_Id = data.MI_Id_new;


                    }
                    else
                    {
                        data.MI_Id = data.MI_Id;
                    }
                    cmd.CommandText = "AT_AssetsReport_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@selectionflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.selectionflag
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
                        data.get_AssetsReportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Check-In Report load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<CheckOutAssetsDTO> getreport(CheckOutAssetsDTO data)
        {
            try
            {
                var stfck1 = _ATContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                var stfcheck1 = _ATContext.applicationRole.Where(a => a.Id == stfck1[0].RoleId).ToList();
                data.staffcheck = stfcheck1[0].Name;


                if (stfcheck1[0].Name != "Staff")
                {

                    data.MI_Id = data.MI_Id_new;


                }
                else
                {
                    data.MI_Id = data.MI_Id;
                }

                if (data.selectionflag == "year")
                {
                  
                    using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "AT_Location";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });                       
                        cmd.Parameters.Add(new SqlParameter("@coyear",
                      SqlDbType.VarChar)
                        {
                            Value = data.IMFY_FinancialYear
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
                            data.get_locations = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    data.get_locations = null;
                    string itemids = "0";
                    string locationids = "0";

                    if (data.selectionflag == "Location")
                    {
                        if (data.locationarray != null)
                        {
                            foreach (var l in data.locationarray)
                            {
                                locationids = locationids + "," + l.INVMLO_Id;
                            }
                        }
                    }
                    else if (data.selectionflag == "Item")
                    {
                        if (data.itemarray != null)
                        {
                            foreach (var i in data.itemarray)
                            {
                                itemids = itemids + "," + i.INVMI_Id;
                            }
                        }
                    }
                    if(locationids=="0" ||locationids == "")
                    {
                        locationids = "a";
                    }
                    using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "AT_AssetsReport";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@selectionflag",
                      SqlDbType.VarChar)
                        {
                            Value = data.selectionflag
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                    SqlDbType.VarChar)
                        {
                            Value = itemids
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMLO_Id",
                                        SqlDbType.VarChar)
                        {
                            Value = locationids
                        });
                        cmd.Parameters.Add(new SqlParameter("@coyear",
                   SqlDbType.VarChar)
                        {
                            Value = data.IMFY_FinancialYear
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
                            data.get_AssetsReport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "AT_LocationDetail";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });                       
                  
                        cmd.Parameters.Add(new SqlParameter("@INVMLO_Id",
                                        SqlDbType.VarChar)
                        {
                            Value = locationids
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
                            data.get_locationDetails = retObject.ToArray();
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
                data.message = "Error";
                _logAT.LogInformation("Assets Report :" + ex.Message);
            }
            return data;
        }



    }
}
