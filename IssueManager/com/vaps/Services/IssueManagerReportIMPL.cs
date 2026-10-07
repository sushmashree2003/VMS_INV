using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class IssueManagerReportIMPL : Interfaces.IssueManagerReportInterface
    {
        public IssueManagerContext _IssueContext;
        ILogger<IssueManagerReportDTO> _logger;
        public IssueManagerReportIMPL(IssueManagerContext _cont, ILogger<IssueManagerReportDTO> _log)
        {
            _IssueContext = _cont;
            _logger = _log;
        }
        public IssueManagerReportDTO getdetails(IssueManagerReportDTO data)
        {
            try
            {
                //data.floorList = _IssueContext.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id && t.HRMF_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //getReport
        public IssueManagerReportDTO getReport(IssueManagerReportDTO data)
        {
            try
            {
                string FromDate = ""; string ToDate = "";

                if(data.FromDate !=null && data.ToDate != null)
                {
                    FromDate = data.FromDate.Value.ToString("yyyy-MM-dd");
                    ToDate = data.ToDate.Value.ToString("yyyy-MM-dd");
                }
                string HRMF_FloorName = "";
                if (data.HRMF_FloorNamelist.Length > 0)
                {
                    foreach (var ue in data.HRMF_FloorNamelist)
                    {
                        HRMF_FloorName = HRMF_FloorName + "0," + ue.hrmF_FloorName;
                    }
                }
                else
                {
                    if(data.FloarName !=null && data.FloarName !="")
                    {
                        HRMF_FloorName = data.FloarName;
                    }
                  
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISSUEMANGER_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)
                    {
                        Value = FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)
                    {
                        Value = ToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@optionflag", SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMF_FloorName", SqlDbType.VarChar)
                    {
                        Value = HRMF_FloorName
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead 
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getReport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }       
    }
}
