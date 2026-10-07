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
    public class CheckInReportImpl : Interface.CheckInReportInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<CheckInReportImpl> _logAT;
        public CheckInReportImpl(AssetTrackingContext ATContext, ILogger<CheckInReportImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public CheckInAssetsDTO getloaddata(CheckInAssetsDTO data)
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

                if (stfcheck1[0].Name != "Staff")
                {

                    data.MI_Id = data.MI_Id_new;


                }
                else
                {
                    data.MI_Id = data.MI_Id;
                }

                data.get_items = (from a in _ATContext.INV_Master_ItemDMO
                                  from b in _ATContext.INV_Asset_CheckInDMO
                                  where (a.INVMI_Id == b.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMI_ActiveFlg == true)
                                  select new CheckInAssetsDTO
                                  {
                                      INVMI_Id = a.INVMI_Id,
                                      INVMI_ItemName = a.INVMI_ItemName,
                                  }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();

                data.get_locations = (from a in _ATContext.INV_Master_LocationDMO
                                      from b in _ATContext.INV_Asset_CheckInDMO
                                      where (a.INVMLO_Id == b.INVMLO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_ActiveFlg == true)
                                      select new CheckInAssetsDTO
                                      {
                                          INVMLO_Id = a.INVMLO_Id,
                                          INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                      }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();

                data.get_Financialyear = (from a in _ATContext.IVRM_Master_FinancialYear
                                          from b in _ATContext.INV_StockDMO
                                          where (a.IMFY_Id == b.IMFY_Id && b.MI_Id == data.MI_Id)
                                          select new CheckOutAssetsDTO
                                          {
                                              IMFY_Id = b.IMFY_Id,
                                              IMFY_FinancialYear = a.IMFY_FinancialYear,
                                              IMFY_AssessmentYear = a.IMFY_AssessmentYear,
                                              IMFY_FromDate = a.IMFY_FromDate,
                                              IMFY_ToDate = a.IMFY_ToDate,
                                              IMFY_OrderBy = a.IMFY_OrderBy
                                          }).Distinct().OrderBy(o => o.IMFY_OrderBy).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Check-In Report load Page:" + ex.Message);
            }
            return data;
        }

        public CheckInAssetsDTO getreport(CheckInAssetsDTO data)
        {
            try
            {
                string itemids = "0";
                string locationids = "0";
                data.MI_logo = _ATContext.Institution.Where(R => R.MI_Id == data.MI_Id_new).Select(P => P.MI_Logo).FirstOrDefault();
                if (data.selectionflag == "Item")
                {
                    if (data.itemarray != null)
                    {
                        foreach (var i in data.itemarray)
                        {
                            itemids = itemids + "," + i.INVMI_Id;
                        }
                    }
                }
                else if (data.selectionflag == "Location")
                {
                    if (data.locationarray != null)
                    {
                        foreach (var l in data.locationarray)
                        {
                            locationids = locationids + "," + l.INVMLO_Id;
                        }
                    }
                }

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
                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AT_CheckIn_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.IMFY_Id
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

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
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
                        data.get_checkInreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   
                }
                data.MI_Logo = _ATContext.Institution.Where(r => r.MI_Id == data.MI_Id && r.MI_ActiveFlag == 1).Select(p => p.MI_Logo).FirstOrDefault();
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Check-In Report :" + ex.Message);
            }
            return data;
        }



    }
}
