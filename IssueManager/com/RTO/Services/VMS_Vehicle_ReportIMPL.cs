using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Services
{
    public class VMS_Vehicle_ReportIMPL : Interfaces.VMS_Vehicle_ReportInterface
    {
        public RTOContext _RTOContext;
        public DomainModelMsSqlServerContext _db;
        public VMS_Vehicle_ReportIMPL(RTOContext hh, DomainModelMsSqlServerContext db)
        {
            _RTOContext = hh;
            _db = db;
        }
        public VMS_Vehicle_ReportDTO loaddata(VMS_Vehicle_ReportDTO data)
        {
            try
            {
                data.vehicales = _RTOContext.Master_VehicleDMO.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //RecieptReport
        public VMS_Vehicle_ReportDTO RecieptReport(VMS_Vehicle_ReportDTO data)
        {
            try
            {
                //RTO_VehicleKeys_Handoverreceived_report
                string VMSVKEYS_Id = "0";
                if (data.Vehicalist !=null)
                {
                    foreach(var d in data.Vehicalist)
                    {
                        VMSVKEYS_Id = VMSVKEYS_Id + ',' + d.VMSVKEYS_Id;
                    }
                }
                string FromDate = "";
                string To_Date = "";
                if(data.FromDate !=null && data.ToDate != null)
                {
                    FromDate = data.FromDate.Value.ToString("yyyy-MM-dd");
                    To_Date = data.ToDate.Value.ToString("yyyy-MM-dd");
                }

                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RTO_VehicleKeys_Handoverreceived_report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@VMSVKEYS_Id",
                      SqlDbType.VarChar)
                    {
                        Value = VMSVKEYS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                     SqlDbType.VarChar)
                    {
                        Value = FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                    SqlDbType.VarChar)
                    {
                        Value = To_Date
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
                        data.GetReport = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.milogo = _db.Institute.Where(t => t.MI_Id == data.MI_Id).Select(t => t.MI_Logo).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VMS_Vehicle_ReportDTO savedata(VMS_Vehicle_ReportDTO data)
        {
            try
            {
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            return data;
        }
        //onchange
        public VMS_Vehicle_ReportDTO onchange(VMS_Vehicle_ReportDTO data)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            return data;
        }
        public VMS_Vehicle_ReportDTO savedataSource(VMS_Vehicle_ReportDTO data)
        {
            try
            {

             
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
               
            }
            return data;
        }

        public VMS_Vehicle_ReportDTO clientDecative(VMS_Vehicle_ReportDTO data)
        {
            try
            {
               
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
               
            }
            return data;
        }
    }
}
