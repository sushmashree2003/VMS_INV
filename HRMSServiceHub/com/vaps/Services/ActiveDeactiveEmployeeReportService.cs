using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class ActiveDeactiveEmployeeReportService : Interfaces.ActiveDeactiveEmployeeReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public ActiveDeactiveEmployeeReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public ActiveDeactiveEmployeeReportDTO getalldetails(ActiveDeactiveEmployeeReportDTO data)
        {
            try
            {
                data.companydetails = _HRMSContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ActiveDeactiveEmployeeReportDTO report(ActiveDeactiveEmployeeReportDTO data)
        {
            try
            {
                string mi_ids = "0";                List<long> mid = new List<long>();                if (data.selected_Inst.Length > 0)                {                    foreach (var item in data.selected_Inst)                    {                        mid.Add(item.MI_Id);                    }                    for (int i = 0; i < mid.Count; i++)                    {                        mi_ids = mi_ids + "," + mid[i].ToString();                    }                }


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employeedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_ID",
                      SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                   SqlDbType.VarChar)
                    {
                        Value = data.radiotype
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
