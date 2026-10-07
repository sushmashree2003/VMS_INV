using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Implementation
{
    public class INV_ItemReportImpl : Interface.INV_ItemReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_ItemReportImpl> _logInv;
        public INV_ItemReportImpl(InventoryContext InvContext, ILogger<INV_ItemReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_Master_ItemDTO> getloaddata(INV_Master_ItemDTO data)
        {
            try
            {
                var stfck1 = _INVContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                var stfcheck1 = _INVContext.applicationRole.Where(a => a.Id == stfck1[0].RoleId).ToList();
                data.staffcheck = stfcheck1[0].Name;

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
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


                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    var stfck2 = _INVContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                    var stfcheck2 = _INVContext.applicationRole.Where(a => a.Id == stfck1[0].RoleId).ToList();
                    data.staffcheck = stfcheck2[0].Name;


                    if (stfcheck2[0].Name != "Staff")
                    {

                        data.MI_Id = data.MI_Id_new;


                    }
                    else
                    {
                        data.MI_Id = data.MI_Id;
                    }
                    cmd.CommandText = "INV_ItemReport_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.get_itemreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Item Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<INV_Master_ItemDTO> onreport(INV_Master_ItemDTO data)
        {
            try
            {
                string itemids = "0";
                string grnids = "0";
                if (data.optionflag == "Item")
                {
                    if (data.itemsArray != null)
                    {
                        foreach (var i in data.itemsArray)
                        {
                            itemids = itemids + "," + i.INVMI_Id;
                        }
                    }
                }
                if (data.optionflag == "Group")
                {
                    if (data.grnArray != null)
                    {
                        foreach (var i in data.grnArray)
                        {
                            grnids = grnids + "," + i.INVMG_Id;
                        }
                    }
                }
                var stfck1 = _INVContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                var stfcheck1 = _INVContext.applicationRole.Where(a => a.Id == stfck1[0].RoleId).ToList();
                data.staffcheck = stfcheck1[0].Name;

                data.MI_Id = data.MI_Id_new;
                //if (stfcheck1[0].Name != "Staff")
                //{

                //    data.MI_Id = data.MI_Id_new;


                //}
                //else
                //{
                //    data.MI_Id = data.MI_Id;
                //}


                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_ItemReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = itemids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMG_Id",
                  SqlDbType.VarChar)
                    {
                        //Value = data.INVMG_Id
                        Value = grnids
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
                        data.get_itemreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Item Report:" + ex.Message);
            }
            return data;
        }


    }
}
