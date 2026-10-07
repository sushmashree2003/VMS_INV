using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
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
    public class INV_LocationReportIMPL : Interface.INV_LocationReportInterface
    {
        public InventoryContext _INVContext;

        ILogger<INV_LocationReportIMPL> _logInv;
        public INV_LocationReportIMPL(InventoryContext InvContext, ILogger<INV_LocationReportIMPL> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }
        public async Task<INV_LocationReportDTO> getloaddata(INV_LocationReportDTO data)
        {
            try
            {
                data.milist = _INVContext.Institution.Where(p => p.MI_ActiveFlag == 1).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public async Task<INV_LocationReportDTO> onreport(INV_LocationReportDTO data)
        {
            try
            {
                if (data.optionflag != null && data.optionflag!="")
                {
                    string itemids = ""; string INVMG_IdS = ""; string INVMLO_IdS = ""; string INVMST_IdS = "";
                    if (data.optionflag == "Item")
                    {
                        if (data.itemsArray != null)
                        {
                            foreach (var i in data.itemsArray)
                            {
                                itemids = itemids + "0," + i.INVMI_Id;
                            }
                        }
                    }
                    if (data.optionflag == "Group")
                    {
                        if (data.itemsArray != null)
                        {
                            foreach (var i in data.itemsArray)
                            {
                                INVMG_IdS = INVMG_IdS + "0," + i.INVMG_Id;
                            }
                        }
                    }
                    if (data.optionflag == "Location")
                    {
                        if (data.itemsArray != null)
                        {
                            foreach (var i in data.itemsArray)
                            {
                                INVMLO_IdS = INVMLO_IdS + "0," + i.INVMLO_Id;
                            }
                        }
                    }
                    if (data.optionflag == "Store")
                    {
                        if (data.itemsArray != null)
                        {
                            foreach (var i in data.itemsArray)
                            {
                                INVMST_IdS = INVMST_IdS + "0," + i.INVMST_Id;
                            }
                        }
                    }                  
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_AllLocationReport";
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
                        cmd.Parameters.Add(new SqlParameter("@INVMG_Ids",
                      SqlDbType.VarChar)
                        {
                            Value = INVMG_IdS
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMLO_Ids",
                    SqlDbType.VarChar)
                        {
                            Value = INVMLO_IdS
                        });                      
                        cmd.Parameters.Add(new SqlParameter("@INVMST_Ids",
                   SqlDbType.VarChar)
                        {
                            Value = INVMST_IdS
                        });
                        //@AsOnDate
                        cmd.Parameters.Add(new SqlParameter("@AsOnDate",
                   SqlDbType.Bit)
                        {
                            Value = 0
                        });
                        //optionflagtwo
                        cmd.Parameters.Add(new SqlParameter("@optionflagtwo",
                  SqlDbType.VarChar)
                        {
                            Value = data.optionflagtwo
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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //onchangeloaddata
        public async Task<INV_LocationReportDTO> onchangeloaddata(INV_LocationReportDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Item_LocationReport_Details";
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
