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
    public class SiteLocationsReportImpl : Interface.SiteLocationsReportInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<SiteLocationsReportImpl> _logAT;
        public SiteLocationsReportImpl(AssetTrackingContext ATContext, ILogger<SiteLocationsReportImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public AT_MasterSiteDTO getloaddata(AT_MasterSiteDTO data)
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
                data.get_sites = _ATContext.INV_Master_SiteDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMSI_ActiveFlg == true).OrderBy(m => m.INVMSI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Site Report load Page:" + ex.Message);
            }
            return data;
        }

        public AT_MasterSiteDTO getreport(AT_MasterSiteDTO data)
        {
            try
            {
                data.MI_logo = _ATContext.Institution.Where(R => R.MI_Id == data.MI_Id_new).Select(P => P.MI_Logo).FirstOrDefault();
                List<long> ids = new List<long>();
                if (data.sitearray != null)
                {
                    foreach (var s in data.sitearray)
                    {
                        ids.Add(s.INVMSI_Id);
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

                data.get_sitereport = (from a in _ATContext.INV_Master_SiteDMO
                                       from b in _ATContext.INV_Master_LocationDMO
                                       where (a.INVMSI_Id == b.INVMSI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && ids.Contains(a.INVMSI_Id))
                                       select new AT_MasterLocationDTO
                                       {
                                           INVMSI_Id = a.INVMSI_Id,
                                           INVMLO_Id = b.INVMLO_Id,
                                           INVMSI_SiteBuildingName = a.INVMSI_SiteBuildingName,
                                           INVMLO_LocationRoomName = b.INVMLO_LocationRoomName,
                                           INVMLO_LocationRemarks = b.INVMLO_LocationRemarks,
                                           INVMLO_InchargeName = b.INVMLO_InchargeName,
                                           INVMLO_ActiveFlg = b.INVMLO_ActiveFlg,
                                       }).Distinct().OrderBy(m => m.INVMSI_Id).ToArray();

                data.MI_Logo = _ATContext.Institution.Where(r => r.MI_Id == data.MI_Id && r.MI_ActiveFlag == 1).Select(p => p.MI_Logo).FirstOrDefault();

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Site Report :" + ex.Message);
            }
            return data;
        }



    }
}
