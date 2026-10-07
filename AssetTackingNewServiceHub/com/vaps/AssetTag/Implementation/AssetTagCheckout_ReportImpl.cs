using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Implementation
{
    public class AssetTagCheckout_ReportImpl : Interface.AssetTagCheckout_ReportInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagCheckout_ReportImpl> _ATINV;
        public AssetTagCheckout_ReportImpl(AssetTrackingContext ATContext, ILogger<AssetTagCheckout_ReportImpl> log)
        {
            _ATContext = ATContext;
            _ATINV = log;
        }

        public async Task<AssetTagCheckOutDTO> getloaddata(AssetTagCheckOutDTO data)
        {
            try
            {
                var stfck1 = _ATContext.ApplicationUserRole_con.Where(a => a.UserId == data.userId).ToList();
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
                        Value = data.userId
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
                    string miid = "0";
                    var stfck = _ATContext.ApplicationUserRole_con.Where(a => a.UserId == data.userId).ToList();
                    var stfcheck = _ATContext.applicationRole.Where(a => a.Id == stfck[0].RoleId).ToList();

                    if (stfcheck[0].Name != "Staff")
                    {
                        foreach (var ii in data.get_mi_arraylist)
                        {
                            miid = miid + "," + ii.MI_Id;
                        }
                    }
                    else
                    {
                        miid = data.MI_Id.ToString();
                    }

                    cmd.CommandText = "AssetTagCheckout_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = miid
                    });

                    cmd.Parameters.Add(new SqlParameter("@optionflag",
            SqlDbType.VarChar)
                    {
                        Value = data.optionflag
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
                        data.get_ckoutdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _ATINV.LogInformation("Asset tag Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<AssetTagCheckOutDTO> onreport(AssetTagCheckOutDTO data)
        {
            try
            {
                string invmids = "0";
                string invmstids = "0";
                string invmloids = "0";
                string invaatids = "0";
                string hrmeid = "0";
                string miid = "0";
                var stfck = _ATContext.ApplicationUserRole_con.Where(a => a.UserId == data.userId).ToList();
                var stfcheck = _ATContext.applicationRole.Where(a => a.Id == stfck[0].RoleId).ToList();
                data.staffcheck = stfcheck[0].Name;
             
                if (stfcheck[0].Name != "Staff")
                {
                    foreach (var ii in data.get_mi_arraylist)
                    {
                        miid = miid + "," + ii.MI_Id;
                    }
                }
                else
                {
                    miid = data.MI_Id.ToString();
                }
                if (data.optionflag == "Item")
                {
                    if (data.coItemArray != null)
                    {
                        foreach (var i in data.coItemArray)
                        {
                            invmids = invmids + "," + i.INVMI_Id;
                        }
                    }
                }
                else if (data.optionflag == "Store")
                {
                    if (data.coStoreArray != null)
                    {
                        foreach (var i in data.coStoreArray)
                        {
                            invmstids = invmstids + "," + i.INVMST_Id;
                        }
                    }
                }
                else if (data.optionflag == "Location")
                {
                    if (data.coLocationArray != null)
                    {
                        foreach (var i in data.coLocationArray)
                        {
                            invmloids = invmloids + "," + i.INVMLO_Id;
                        }
                    }
                }
                else if (data.optionflag == "Tag")
                {
                    if (data.coTagArray != null)
                    {
                        foreach (var i in data.coTagArray)
                        {
                            invaatids = invaatids + "," + i.INVAAT_Id;
                        }
                    }
                }
                else if (data.optionflag == "Employee")
                {
                    if (data.employeeArray != null)
                    {
                        foreach (var i in data.employeeArray)
                        {
                            hrmeid = hrmeid + "," + i.HRME_Id;
                        }
                    }
                }

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagCheckout_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = miid
                    });
                    cmd.Parameters.Add(new SqlParameter("@startdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate",
                    SqlDbType.VarChar)
                    {
                        Value = data.enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVAAT_Ids",
                SqlDbType.VarChar)
                    {
                        Value = invaatids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = invmids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Ids",
                SqlDbType.VarChar)
                    {
                        Value = invmstids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMLO_Ids",
                SqlDbType.VarChar)
                    {
                        Value = invmloids
                    });

                    cmd.Parameters.Add(new SqlParameter("@optionflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.VarChar)
                    {
                        Value = hrmeid
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
                        data.get_ckoutreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _ATINV.LogInformation("Asset Tag Report:" + ex.Message);
            }
            return data;
        }


    }
}
