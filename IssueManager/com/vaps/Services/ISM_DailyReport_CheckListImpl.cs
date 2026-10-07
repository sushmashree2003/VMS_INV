using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
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
    public class ISM_DailyReport_CheckListImpl:Interfaces.ISM_DailyReport_CheckListInterface
    {
        public IssueManagerContext _IssueContext;
        public ISM_DailyReport_CheckListImpl(IssueManagerContext issues)
        {
            _IssueContext = issues;
        }
        public ISM_DailyReport_CheckListDTO loaddata(ISM_DailyReport_CheckListDTO dto)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_EmployeeList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@user_Id",
             SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                dto.mi_list = _IssueContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_DailyReport_CheckListDTO getdata(ISM_DailyReport_CheckListDTO dto)
        {
            try
            {
                string hrme = "0";

                foreach (var item in dto.arrayuserEmp)
                {
                    hrme = hrme + "," + item.HRME_Id;
                }
            
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_GetDailyrCheckReport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;


                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = dto.MI_IdNew
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_IdS", SqlDbType.VarChar)
                        {
                            Value = hrme
                        });

                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)
                        {
                            Value = dto.startdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)
                        {
                            Value = dto.enddate
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = dto.selectionflag
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            dto.getdailyreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
             
               
               
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
