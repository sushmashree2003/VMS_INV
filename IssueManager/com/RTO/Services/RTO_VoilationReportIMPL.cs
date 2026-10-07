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
    public class RTO_VoilationReportIMPL : Interfaces.RTO_VoilationReportInterface
    {
        public RTOContext _RTOContext;
        public DomainModelMsSqlServerContext _db;
        public RTO_VoilationReportIMPL(RTOContext hh, DomainModelMsSqlServerContext db)
        {
            _RTOContext = hh;
            _db = db;
        }
        public RTO_VoilationReportDTO loaddata(RTO_VoilationReportDTO data)
        {
            try
            {
                data.Rtoviolations = _RTOContext.Master_VehicleDMO.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //RecieptReport
        public RTO_VoilationReportDTO RecieptReport1(RTO_VoilationReportDTO data)
        {
            try
            {
                //fovechicle_report
                string VMSFOVOB_Id = "0";
                if (data.rto_ob_report_list != null)
                {
                    foreach (var d in data.rto_ob_report_list)
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
                    cmd.CommandText = "RTO_VOILATIONPROCD";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TRMV_Id",
                      SqlDbType.VarChar)
                    {
                        Value = VMSFOVOB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROM_Date",
                     SqlDbType.VarChar)
                    {
                        Value = fromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TO_DATE",
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


        public RTO_VoilationReportDTO onchange(RTO_VoilationReportDTO data)
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