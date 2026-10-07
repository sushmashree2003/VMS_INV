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
    public class FO_Vehicle_Ob_reportIMPL : Interfaces.FO_Vehicle_Ob_reportInterface
    {
        public RTOContext _RTOContext;
        public DomainModelMsSqlServerContext _db;
        public FO_Vehicle_Ob_reportIMPL(RTOContext hh, DomainModelMsSqlServerContext db)
        {
            _RTOContext = hh;
            _db = db;
        }
        public FO_Vehicle_Ob_reportDTO loaddata(FO_Vehicle_Ob_reportDTO data)
        {
            try
            {
                data.fovehicales = _RTOContext.Master_VehicleDMO.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //RecieptReport
        public FO_Vehicle_Ob_reportDTO RecieptReport1(FO_Vehicle_Ob_reportDTO data)
        {
            try
            {
                //fovechicle_report
                string VMSFOVOB_Id = "0";
                if (data.fo_ob_report_list != null)
                {
                    foreach(var d in data.fo_ob_report_list)
                    {
                        VMSFOVOB_Id = VMSFOVOB_Id + ',' + d.VMSFOVOB_Id;
                    }
                }
                string fromDate = "";
                string ToDate = "";
                if (data.FromDate != null && data.ToDate != null)
                {
                    fromDate = data.FromDate.Value.ToString("yyyy-MM-dd");
                    ToDate = data.ToDate.Value.ToString("yyyy-MM-dd");
                }
                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@VMSVKEYS_Id",
                      SqlDbType.VarChar)
                    {
                        Value = VMSFOVOB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                     SqlDbType.VarChar)
                    {
                        Value = fromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                    SqlDbType.VarChar)
                    {
                        Value = ToDate
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
                        data.GetReportt = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FO_Vehicle_Ob_reportDTO onchange(FO_Vehicle_Ob_reportDTO data)
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
