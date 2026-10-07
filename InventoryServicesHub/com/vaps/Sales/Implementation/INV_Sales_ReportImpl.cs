using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class INV_Sales_ReportImpl : Interface.INV_Sales_ReprtInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_Sales_ReportImpl> _logInv;
        public INV_Sales_ReportImpl(InventoryContext InvContext, ILogger<INV_Sales_ReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_Sales_ReportDTO> getloaddata(INV_Sales_ReportDTO data)
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
                    string miid = "0";
                    var stfck = _INVContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                    var stfcheck = _INVContext.applicationRole.Where(a => a.Id == stfck[0].RoleId).ToList();

                    if (stfcheck[0].Name != "Staff")
                    {
                        foreach (var ii in data.get_mi_arraylist)
                        {
                            miid = miid + "," + ii.MI_Id.ToString();
                        }
                    }
                    else
                    {
                        miid = data.MI_Id.ToString();
                    }
                    cmd.CommandText = "INV_Sales_Details";
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
                        data.get_Salesdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PO Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<INV_Sales_ReportDTO> onreport(INV_Sales_ReportDTO data)
        {
            try
            {
                string poids = "0";
                string invmids = "0";
                string invmcids = "0";
                string invmgrnids = "0";
                string miid = "0";
                var stfck = _INVContext.ApplicationUserRole_con.Where(a => a.UserId == data.UserId).ToList();
                var stfcheck = _INVContext.applicationRole.Where(a => a.Id == stfck[0].RoleId).ToList();

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
              if (data.optionflag == "Item-Wise")
                {
                    if (data.SalesitemArray != null)
                    {
                        foreach (var i in data.SalesitemArray)
                        {
                            invmids = invmids + "," + i.INVMI_Id;
                        }
                    }
                }
                else if (data.optionflag == "Customer-Wise")
                {
                    if (data.SalescustomerArray != null)
                    {
                        foreach (var i in data.SalescustomerArray)
                        {
                            invmcids = invmcids + "," + i.INVMC_Id;
                        }
                    }
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_AdSalesReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = miid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                 SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMC_Id",
              SqlDbType.VarChar)
                    {
                        Value = invmcids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                SqlDbType.VarChar)
                    {
                        Value = invmids
                    });

                    cmd.Parameters.Add(new SqlParameter("@StartDate",
                  SqlDbType.VarChar)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.enddate
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
                        data.get_Salesreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales Report:" + ex.Message);
            }
            return data;
        }
            //    try
            //    {
            //        if (data.optionflag == "Itm")
            //        {
            //            string poids = "0";
            //            string invmids = "0";
            //            string invmsids = "0";
            //           if (data.optionflag == "Itm")
            //            {
            //                if (data.SalesitemArray != null)
            //                {
            //                    foreach (var i in data.SalesitemArray)
            //                    {
            //                        invmids = invmids + "," + i.INVMI_Id;
            //                    }
            //                }
            //            }
            //            else if (data.optionflag == "Cus")
            //            {
            //                if (data.SalescustomerArray != null)
            //                {
            //                    foreach (var i in data.SalescustomerArray)
            //                    {
            //                        invmsids = invmsids + "," + i.INVMC_Id;
            //                    }
            //                }
            //            }
            //            using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
            //            {
            //                cmd.CommandText = "INV_Sales_Report";
            //                cmd.CommandType = CommandType.StoredProcedure;

            //                cmd.Parameters.Add(new SqlParameter("@MI_Id",
            //         SqlDbType.BigInt)
            //                {
            //                    Value = data.MI_Id
            //                });
            //                cmd.Parameters.Add(new SqlParameter("@startdate",
            //              SqlDbType.VarChar)
            //                {
            //                    Value = data.startdate
            //                });
            //                cmd.Parameters.Add(new SqlParameter("@enddate",
            //                SqlDbType.VarChar)
            //                {
            //                    Value = data.enddate
            //                });

            //                cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
            //              SqlDbType.VarChar)
            //                {
            //                    Value = invmids
            //                });
            //                cmd.Parameters.Add(new SqlParameter("@INVMS_Ids",
            //            SqlDbType.VarChar)
            //                {
            //                    Value = invmsids
            //                });

            //                cmd.Parameters.Add(new SqlParameter("@optionflag",
            //              SqlDbType.VarChar)
            //                {
            //                    Value = data.optionflag
            //                });

            //                if (cmd.Connection.State != ConnectionState.Open)
            //                    cmd.Connection.Open();

            //                var retObject = new List<dynamic>();
            //                try
            //                {
            //                    using (var dataReader =  cmd.ExecuteReader())
            //                    {
            //                        while ( dataReader.Read())
            //                        {
            //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                            {
            //                                dataRow.Add(
            //                                    dataReader.GetName(iFiled),
            //                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                                );
            //                            }
            //                            retObject.Add((ExpandoObject)dataRow);
            //                        }
            //                    }
            //                    data.get_Salesreport = retObject.ToArray();
            //                }
            //                catch (Exception ex)
            //                {
            //                    Console.WriteLine(ex.Message);
            //                }
            //            }


            //        }
            //        else
            //        {
            //            string poids = "0";
            //            string invmids = "0";
            //            string invmsids = "0";
            //           if (data.optionflag == "Item")
            //            {
            //                if (data.SalesitemArray != null)
            //                {
            //                    foreach (var i in data.SalesitemArray)
            //                    {
            //                        invmids = invmids + "," + i.INVMI_Id;
            //                    }
            //                }
            //            }
            //            else if (data.optionflag == "Customer")
            //            {
            //                if (data.SalescustomerArray != null)
            //                {
            //                    foreach (var i in data.SalescustomerArray)
            //                    {
            //                        invmsids = invmsids + "," + i.INVMC_Id;
            //                    }
            //                }
            //            }

            //            using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
            //            {
            //                cmd.CommandText = "INV_SalesRept";
            //                cmd.CommandType = CommandType.StoredProcedure;

            //                cmd.Parameters.Add(new SqlParameter("@MI_Id",
            //         SqlDbType.BigInt)
            //                {
            //                    Value = data.MI_Id
            //                });
            //                cmd.Parameters.Add(new SqlParameter("@startdate",
            //              SqlDbType.VarChar)
            //                {
            //                    Value = data.startdate
            //                });
            //                cmd.Parameters.Add(new SqlParameter("@enddate",
            //                SqlDbType.VarChar)
            //                {
            //                    Value = data.enddate
            //                });

            //                cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
            //              SqlDbType.VarChar)
            //                {
            //                    Value = invmids
            //                });
            //                cmd.Parameters.Add(new SqlParameter("@INVMC_Ids",
            //            SqlDbType.VarChar)
            //                {
            //                    Value = invmsids
            //                });

            //                cmd.Parameters.Add(new SqlParameter("@optionflag",
            //              SqlDbType.VarChar)
            //                {
            //                    Value = data.optionflag
            //                });

            //                if (cmd.Connection.State != ConnectionState.Open)
            //                    cmd.Connection.Open();

            //                var retObject = new List<dynamic>();
            //                try
            //                {
            //                    using (var dataReader = await cmd.ExecuteReaderAsync())
            //                    {
            //                        while (await dataReader.ReadAsync())
            //                        {
            //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                            {
            //                                dataRow.Add(
            //                                    dataReader.GetName(iFiled),
            //                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                                );
            //                            }
            //                            retObject.Add((ExpandoObject)dataRow);
            //                        }
            //                    }
            //                    data.get_Salesreport = retObject.ToArray();
            //                }
            //                catch (Exception ex)
            //                {
            //                    Console.WriteLine(ex.Message);
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        _logInv.LogInformation("Sales Report:" + ex.Message);
            //    }
            //    return data;
            //}


        }
}
