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
   
    public class VMS_ReportService : Interfaces.VMS_ReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public VMS_ReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }

        public VMS_ReportDTO onloadgetdetails(VMS_ReportDTO dto)
        {
            dto.companydetails = _HRMSContext.Institution.Where(t => t.MI_ActiveFlag == 1).ToArray();
            
            return dto;
        }
        
        public VMS_ReportDTO report(VMS_ReportDTO dto)
        {
            try
            {
                string mi_ids = "0";
                List<long> mid = new List<long>();

                if (dto.selected_Inst.Length > 0)
                {
                    foreach (var item in dto.selected_Inst)
                    {
                        mid.Add(item.MI_Id);
                    }

                    for (int i = 0; i < mid.Count; i++)
                    {
                        mi_ids = mi_ids + "," + mid[i].ToString();
                    }
                }
                
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_ClientIE_Developerwise_Moduledetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                      SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = dto.allind
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
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
                        dto.GetReport = retObject.ToArray();
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
            return dto;
        }


        public VMS_ReportDTO get_Employee(VMS_ReportDTO dto)
        {
            
            var dept = dto.multipledep.Split(',');
            List<long> list2 = new List<long>();
            for (int i = 0; i < dept.Length; i++)
            {
                list2.Add(Convert.ToInt64(dept[i]));
            }

            dto.fillemployee = (from a in _HRMSContext.MasterEmployee
                                where (list2.Contains(a.MI_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                 select new VMS_ReportDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                     HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                     HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                     HRME_EmployeeCode = a.HRME_EmployeeCode
                                 }
                     ).Distinct().ToArray();

            dto.projectName = (from a in _HRMSContext.ISM_Master_ProjectDMO
                               where (list2.Contains(a.MI_Id) && a.ISMMPR_ActiveFlg == true)
                               select new VMS_ReportDTO
                               {
                                   ISMMPR_Id = a.ISMMPR_Id,
                                   ISMMPR_ProjectName = a.ISMMPR_ProjectName,
                               }
                              ).Distinct().ToArray();
                

            return dto;
        }


    }
}
