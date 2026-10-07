using DataAccessMsSqlServerProvider;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class Employee_WeekOffImp:Employee_WeekOffInterface
    {
        private readonly VMSContext _context;
        private readonly DomainModelMsSqlServerContext _db;

        public Employee_WeekOffImp(VMSContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }

        public Employee_DataDto OnLoadData(Employee_DataDto dto)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_Employee_Names";
                cmd.CommandType = CommandType.StoredProcedure;

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
                    dto.empData = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return dto;
        }

        //save
        public Employee_DataDto Save(Employee_DataDto dto)
        {
            try
            {
                Employee_DataDto result1 = new Employee_DataDto();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_WeekoffInsert";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                      SqlDbType.VarChar, 255) 
                    {
                        Value = dto.HRME_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Dates",
                      SqlDbType.VarChar, -1) 
                    {
                        Value = dto.Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
                      SqlDbType.BigInt)
                    {
                        Value = dto.User_Id
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
                               
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                dto.returnData = ex.ToString();
            }
            return dto;
        }

    }
}
