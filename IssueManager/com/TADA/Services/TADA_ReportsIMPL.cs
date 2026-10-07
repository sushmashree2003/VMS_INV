using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Services
{
    public class TADA_ReportsIMPL : Interface.TADA_ReportsInterface
    {
        public IssueManagerContext _context;
        public TADA_ReportsIMPL(IssueManagerContext context)
        {
            _context = context;
        }
        public TADA_ApplyReportsDTO getalldata(TADA_ApplyReportsDTO data)
        {
            try
            {

                var rolet = _context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase) || roletype.Equals("Accountant", StringComparison.OrdinalIgnoreCase) || roletype.Equals("Front Office Executive", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                    //IVRMRT_Role
                   
                    
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }

                var hrmEempid = _context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                long? dept_id = 0;
                long? design_id = 0;
                var hrmeDepartment_id = _context.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;

                    data.empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                }

                var hrmeDepartment = _context.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == dept_id && t.HRMD_ActiveFlag == true).Distinct().ToList();

                if (hrmeDepartment.Count > 0)
                {
                    data.empdepartment = hrmeDepartment.FirstOrDefault().HRMD_DepartmentName;
                }
                //**********************************user Department****************//
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentListTADA";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt)
                    {
                        Value = dept_id
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
                        data.DepartmentList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Designation List ********************************// 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DesignationListTADA";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
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

                        data.Designation = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Emp List ********************************// 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_adminTADA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id", SqlDbType.BigInt)
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


                        data.get_userEmplist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //*********************************** User Employee Mapped List ********************************// 


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        // <summary>       //getMasterReport
        public TADA_ApplyReportsDTO Deptselectiondetails(TADA_ApplyReportsDTO data)
        {
            try
            {
                string departments = "10000";
                if (data.departmentlist.Length > 0)
                {
                    foreach (var ue in data.departmentlist)
                    {
                        departments = departments + "," + ue.HRMDC_ID;
                    }
                }
                //******************************Deviation Remarks & Deviation Calculation Report 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentChangeTADA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                    SqlDbType.VarChar)
                    {
                        Value = departments
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
                        data.Designation = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //************************************//        
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TADA_ApplyReportsDTO Desgselectiondetails(TADA_ApplyReportsDTO data)
        {
            try
            {
                string departments = "0";
                if (data.designationlist.Length > 0)
                {
                    foreach (var ue in data.designationlist)
                    {
                        departments = departments + "," + ue.HRMDES_Id;
                    }

                }
                //******************************Deviation Remarks & Deviation Calculation Report 

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DesignationChangeTADA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@departments",
                      SqlDbType.VarChar)
                    {
                        Value = departments
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

                        data.get_userEmplist = retObject.ToArray();


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
        }       //Viewdetails
        public TADA_ApplyReportsDTO Viewdetails(TADA_ApplyReportsDTO data)
        {
            try
            {
                if (data.RdoPunch == "consolidatereport" || data.RdoPunch == "TADAApply")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADAMltiple_SDetailsAmt_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@VTADAA_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.VTADAA_Id
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

                            data.Veiwdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                if (data.RdoPunch == "TADAAdavance" || data.RdoPunch == "consolidatereport")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_TADA_ADavanceMltiple_SDetailsAmt_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@VTADAAA_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.VTADAAA_Id
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

                            data.Veiwdetailstt = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.consolited = (from a in _context.TADA_ApplyDMO
                                       from b in _context.TADA_AdavanceApplyDMO
                                       where (a.VTADAAA_Id == b.VTADAAA_Id && a.VTADAA_ActiveFlg == true && a.VTADAAA_Id == data.VTADAAA_Id)
                                       select new TADA_ApplyReportsDTO
                                       {
                                           VTADAA_TotalAppliedAmount = a.VTADAA_TotalAppliedAmount,
                                           VTADAA_TotalSactionedAmount = a.VTADAA_TotalSactionedAmount,
                                           VTADAA_TotalPaidAmount = a.VTADAA_TotalPaidAmount,
                                           //adavance
                                           VTADAAA_TotalAppliedAmount = b.VTADAAA_TotalAppliedAmount,
                                           VTADAAA_TotalSactionedAmount = b.VTADAAA_TotalSactionedAmount,
                                           VTADAAA_TotalPaidAmount = b.VTADAAA_TotalPaidAmount
                                       }
                                     ).Distinct().ToArray();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TADA_ApplyReportsDTO GetReport(TADA_ApplyReportsDTO data)
        {
            try
            {
                
                List<long> HRME_Id = new List<long>();
                foreach (var c in data.emplist)
                {
                    HRME_Id.Add(c.HRME_Id);
                }
                
               string StatusFlg = "";
                if (data.optionflag=="All")
                {
                    StatusFlg = "Approved,Rejected,Pending";


                }
                else
                {
                    StatusFlg = data.optionflag;
                }
                if (data.RdoPunch == "TADAAdavance")
                {
                    data.GetReport = (from a in _context.TADA_AdavanceApplyDMO
                                      from c in _context.IVRM_Master_CityDMO
                                      from d in _context.MasterEmployee
                                      from e in _context.Institution
                                      where (a.VTADAAA_ActiveFlg == true && a.IVRMMCT_Id == c.IVRMMCT_Id
                                  && a.HRME_Id == d.HRME_Id && HRME_Id.Contains(a.HRME_Id)
                                && a.MI_Id == e.MI_Id && e.MI_ActiveFlag == 1 && StatusFlg.Contains(a.VTADAAA_StatusFlg) &&
                              (a.VTADAAA_FromDate >= data.From_Date && a.VTADAAA_ToDate <= data.To_Date)
                                      )
                                      //&& HRME_Id.Contains(a.HRME_Id) //&& a.VTADAAA_StatusFlg==data.optionflag
                                      select new TADA_Advance_ApplyDTO
                                      {
                                          VTADAAA_Id = a.VTADAAA_Id,
                                          VTADAAA_FromDate = a.VTADAAA_FromDate,
                                          VTADAAA_ToDate = a.VTADAAA_ToDate,
                                          VTADAAA_ToAddress = a.VTADAAA_ToAddress,
                                          VTADAAA_Remarks = a.VTADAAA_Remarks,
                                          VTADAAA_TotalAppliedAmount = a.VTADAAA_TotalAppliedAmount,
                                          VTADAAA_TotalSactionedAmount = a.VTADAAA_TotalSactionedAmount,
                                          VTADAAA_TotalPaidAmount = a.VTADAAA_TotalPaidAmount,
                                          IVRMMCT_Name = c.IVRMMCT_Name,
                                          VTADAAA_StatusFlg = a.VTADAAA_StatusFlg,
                                          EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                          MI_Name = e.MI_Name,
                                          HRME_EmployeeCode = d.HRME_EmployeeCode
                                      }
                                ).Distinct().ToArray();
                }
                if (data.RdoPunch == "TADAApply")
                {
                    data.GetReport = (from a in _context.TADA_ApplyDMO
                                      from c in _context.IVRM_Master_CityDMO
                                      from d in _context.MasterEmployee
                                      from e in _context.Institution
                                      where (HRME_Id.Contains(a.HRME_Id) && a.HRME_Id == d.HRME_Id && a.VTADAA_ActiveFlg == true && a.IVRMMCT_Id == c.IVRMMCT_Id && a.MI_Id == e.MI_Id && e.MI_ActiveFlag == 1 && StatusFlg.Contains(a.VTADAA_StatusFlg) &&
                              (a.VTADAA_FromDate >= data.From_Date && a.VTADAA_ToDate <= data.To_Date)
                                     )
                                      select new TADA_ApplyReportsDTO
                                      {
                                          VTADAA_Id = a.VTADAA_Id,
                                          VTADAA_FromDate = a.VTADAA_FromDate,
                                          VTADAA_ToDate = a.VTADAA_ToDate,
                                          VTADAA_ToAddress = a.VTADAA_ToAddress,
                                          VTADAA_Remarks = a.VTADAA_Remarks,
                                          IVRMMCT_Name = c.IVRMMCT_Name,
                                          VTADAA_TotalAppliedAmount = a.VTADAA_TotalAppliedAmount,
                                          VTADAA_TotalSactionedAmount = a.VTADAA_TotalSactionedAmount,
                                          VTADAA_TotalPaidAmount = a.VTADAA_TotalPaidAmount,
                                          VTADAA_StatusFlg = a.VTADAA_StatusFlg,
                                          EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                          MI_Name = e.MI_Name,
                                          HRME_EmployeeCode = d.HRME_EmployeeCode
                                      }
                                    ).Distinct().ToArray();

                }
                if (data.RdoPunch == "consolidatereport")
                {
                    data.GetReport = (from a in _context.TADA_ApplyDMO
                                      from b in _context.TADA_AdavanceApplyDMO
                                      from c in _context.IVRM_Master_CityDMO
                                      from d in _context.MasterEmployee
                                      from e in _context.Institution
                                      where (HRME_Id.Contains(a.HRME_Id) && a.HRME_Id == d.HRME_Id && a.VTADAA_ActiveFlg == true && a.IVRMMCT_Id == c.IVRMMCT_Id &&
                              (a.VTADAA_FromDate >= data.From_Date && a.VTADAA_ToDate <= data.To_Date && a.VTADAAA_Id == b.VTADAAA_Id && a.MI_Id == e.MI_Id && e.MI_ActiveFlag == 1 && b.VTADAAA_StatusFlg==data.optionflag)
                                     )
                                      
                                      select new TADA_ApplyReportsDTO
                                      {
                                          VTADAA_Id = a.VTADAA_Id,
                                          VTADAA_FromDate = a.VTADAA_FromDate,
                                          VTADAA_ToDate = a.VTADAA_ToDate,
                                          VTADAA_ToAddress = a.VTADAA_ToAddress,
                                          VTADAA_Remarks = a.VTADAA_Remarks,
                                          IVRMMCT_Name = c.IVRMMCT_Name,
                                          VTADAA_TotalAppliedAmount = a.VTADAA_TotalAppliedAmount,
                                          VTADAA_TotalSactionedAmount = a.VTADAA_TotalSactionedAmount,

                                          VTADAA_TotalPaidAmount = a.VTADAA_TotalPaidAmount,
                                          VTADAA_StatusFlg = a.VTADAA_StatusFlg,
                                          EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                          VTADAAA_Id = b.VTADAAA_Id,
                                          MI_Name = e.MI_Name,
                                          HRME_EmployeeCode = d.HRME_EmployeeCode,
                                          VTADAAA_TotalAppliedAmount = b.VTADAAA_TotalAppliedAmount,
                                          VTADAAA_TotalSactionedAmount = b.VTADAAA_TotalSactionedAmount,
                                          VTADAAA_TotalPaidAmount = b.VTADAAA_TotalPaidAmount,
                                      }
                                   ).Distinct().ToArray();

                }             
               data.MI_Name= _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Select(R => R.MI_Logo).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
