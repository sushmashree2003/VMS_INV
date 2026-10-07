using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class EmployeeMonthEndImpl : Interfaces.EmployeeMonthEndInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public EmployeeMonthEndImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public EmployeeMonthEndDTO getemplist(EmployeeMonthEndDTO data)
        {
            try
            {
                data.employeelist = (from a in _IssueContext.MasterEmployee
                                     where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new EmployeeMonthEndDTO {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null? "": a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "":a.HRME_EmployeeLastName)
                                     }).ToArray();

                data.monthlist = _IssueContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();

                data.yearlist = _IssueContext.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public async Task<EmployeeMonthEndDTO> getempdetailsAsync(EmployeeMonthEndDTO data)
        {
            try
            {
                
                string startdate = data.HRMLY_Id + "-" + data.IVRM_Month_Id + "-01";
                int monthend = _IssueContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == data.IVRM_Month_Id).Select(t => t.IVRM_Month_Max_Days).FirstOrDefault();
                string enddate = data.HRMLY_Id + "-" + data.IVRM_Month_Id + "-" + monthend;
                data.Fromdate = Convert.ToDateTime(startdate);
                data.Todate = Convert.ToDateTime(enddate);

                data.monthname = _IssueContext.IVRM_Month_DMO.Where(t => t.IVRM_Month_Id == data.IVRM_Month_Id).Select(t => t.IVRM_Month_Name).FirstOrDefault();

                data.MI_Id = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();

                data.employeedetails = (from a in _IssueContext.MasterEmployee
                                        from b in _IssueContext.HR_Master_Designation
                                        from c in _IssueContext.HR_Master_Department
                                       // from d in _IssueContext.HR_Master_DepartmentCode_Head
                                        where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == data.HRME_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id)
                                     select new EmployeeMonthEndDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                         HRMDES_DesignationName = b.HRMDES_DesignationName,
                                         HRMD_DepartmentName = c.HRMD_DepartmentName
                                     }).ToArray();

                data.companydetails = _IssueContext.Institution.Where(t => t.MI_ActiveFlag == 1 && t.MI_Id == data.MI_Id).ToArray();

                data.HRMDC_ID = (from a in _IssueContext.MasterEmployee
                                 from b in _IssueContext.HR_Master_Department
                                 where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == data.HRME_Id)
                                 select b.HRMDC_ID).FirstOrDefault();

               

                if (_IssueContext.VMS_EmpWiseMonthEndRemarksDMO.Where(t=>t.HRME_Id == data.HRME_Id && t.VMSEWMER_Year == data.HRMLY_Id && t.VMSEWMER_Month == data.IVRM_Month_Id).ToArray().Length > 0)
                {
                    data.allheadvalues = (from a in _IssueContext.VMS_Master_MonthEndDepartmentDMO
                                         from b in _IssueContext.VMS_EmpWiseMonthEndRemarksDMO
                                         where(a.VMSMMEDPT_Id == b.VMSMMEDPT_Id && b.HRME_Id == data.HRME_Id && b.VMSEWMER_Year == data.HRMLY_Id && b.VMSEWMER_Month == data.IVRM_Month_Id)
                                         select new EmployeeMonthEndDTO
                                         {
                                             VMSMMEDPT_Id = b.VMSMMEDPT_Id,
                                             VMSMMEDPT_Description = a.VMSMMEDPT_Description,
                                             VMSEWMER_Remarks = b.VMSEWMER_Remarks,
                                             VMSEWMER_Value = b.VMSEWMER_Value,
                                             VMSMMEDPT_ActiveFlag = a.VMSMMEDPT_ActiveFlag,
                                             VMSMMEDPT_ManualFlag = a.VMSMMEDPT_ManualFlag
                                         }).OrderBy(t=>t.VMSMMEDPT_Id).ToArray();

                    data.valueexist = true;
                }
                else
                {
                    data.allheadvalues = _IssueContext.VMS_Master_MonthEndDepartmentDMO.Where(t => t.HRMDC_Id == data.HRMDC_ID).OrderBy(t => t.VMSMMEDPT_Id).ToArray();

                    data.valueexist = false;

                   

                    //No of days DR not generated
                    long hrmly_id = _IssueContext.HR_Master_LeaveYearDMO.Where(t => t.MI_Id == data.MI_Id && Convert.ToDateTime(t.HRMLY_FromDate) <= data.Fromdate && Convert.ToDateTime(t.HRMLY_ToDate) >= data.Todate && t.HRMLY_ActiveFlag == true).Select(t => t.HRMLY_Id).FirstOrDefault();

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmpDrReportsNotSentDates";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                        {
                            Value = hrmly_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(data.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(data.Todate).ToString("yyyy-MM-dd")
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.Dailyreptnotsent = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //No of day Absent
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmpUnAuthLeaveDates";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                        {
                            Value = hrmly_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(data.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(data.Todate).ToString("yyyy-MM-dd")
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.staffleaveDetails = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //OVERALL
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_EmpMonthEndRpt";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(data.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(data.Todate).ToString("yyyy-MM-dd")
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.OverallDeveloperCounts = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //OVERALL

                    //names
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_EmpMonthEndRptNames";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                        {
                            Value = Convert.ToDateTime(data.Fromdate).ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar)
                        {
                            Value = Convert.ToDateTime(data.Todate).ToString("yyyy-MM-dd")
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                        );
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.Names = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //
                }

                //added by roopa
                // var dname = (from a in _IssueContext.HR_Master_Department
                //              from b in _IssueContext.MasterEmployee
                //              where (a.HRMD_Id == b.HRMD_Id && b.HRME_Id == data.HRME_Id)
                //              select new EmployeeMonthEndDTO
                //              {
                //                  HRMD_DepartmentName = a.HRMD_DepartmentName
                //              }).FirstOrDefault();
                //data.str_team_lead = (from a in _IssueContext.HR_Master_DepartmentCode_Head
                //                      from b in _IssueContext.MasterEmployee
                //                      from c in _IssueContext.Institution
                //                      from d in _IssueContext.HR_Master_Department
                //                      where (a.HRMDC_ID == d.HRMDC_ID
                //                    && d.MI_Id == c.MI_Id && a.HRME_ID == b.HRME_Id && d.MI_Id == data.MI_Id && d.HRMD_DepartmentName== dname.HRMD_DepartmentName)
                //                      select new EmployeeMonthEndDTO
                //                      {
                //                          HRME_EmployeeFirstNametwo = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                //                      }).Distinct().ToArray();

                //var namarray = (from a in _IssueContext.ISM_User_Employees_MappingDMO
                //                      from b in _IssueContext.Staff_User_Login
                //                      from c in _IssueContext.MasterEmployee
                //                      where (a.HRME_Id == data.HRME_Id && a.ISMUSEMM_Order == 1 && c.HRME_Id == b.Emp_Code)
                //                      select c).ToList();
                //if(namarray.Count > 0)
                //{
                //    data.rpt_name = namarray.FirstOrDefault().HRME_EmployeeFirstName;
                //}


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public EmployeeMonthEndDTO savereport(EmployeeMonthEndDTO data)
        {
            try
            {
                VMS_EmpWiseMonthEndRemarksDMO obj = new VMS_EmpWiseMonthEndRemarksDMO();
                var duplicate = _IssueContext.VMS_EmpWiseMonthEndRemarksDMO.Where(t => t.HRME_Id == data.HRME_Id && t.VMSEWMER_Year == data.VMSEWMER_Year && t.VMSEWMER_Month == data.VMSEWMER_Month).ToArray();

                if(duplicate.Length > 0)
                {
                    for(int x = 0; x <= data.TempDTO.Length; x++)
                    {
                        obj = _IssueContext.VMS_EmpWiseMonthEndRemarksDMO.Where(t => t.HRME_Id == data.HRME_Id && t.VMSEWMER_Year == data.VMSEWMER_Year && t.VMSEWMER_Month == data.VMSEWMER_Month && t.VMSMMEDPT_Id == data.TempDTO[x].VMSMMEDPT_Id).FirstOrDefault();
                        obj.VMSEWMER_Remarks = data.TempDTO[x].remark;
                        obj.VMSEWMER_Value = data.TempDTO[x].values;
                        obj.CreatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Today;
                        _IssueContext.Update(obj);
                    }                    
                    var count = _IssueContext.SaveChanges();
                    if (count > 0)
                    {
                        data.message = "Update";
                    }
                    else
                    {
                        data.message = "False";
                    }
                }
                else
                {
                    for(int x = 0; x < data.TempDTO.Length; x++)
                    {
                        VMS_EmpWiseMonthEndRemarksDMO obj2 = new VMS_EmpWiseMonthEndRemarksDMO();
                        obj2.HRME_Id = data.HRME_Id;
                        obj2.VMSEWMER_Month = data.VMSEWMER_Month;
                        obj2.VMSEWMER_Year = data.VMSEWMER_Year;
                        obj2.VMSMMEDPT_Id = data.TempDTO[x].VMSMMEDPT_Id;
                        obj2.VMSEWMER_Remarks = data.TempDTO[x].remark;
                        obj2.VMSEWMER_Value = data.TempDTO[x].values;
                        obj2.CreatedBy = data.UserId;
                        obj2.CreatedDate = DateTime.Today;
                        _IssueContext.Add(obj2);
                    }
                    var count = _IssueContext.SaveChanges();
                    if(count > 0)
                    {
                        data.message = "Add";
                    }
                    else
                    {
                        data.message = "False";
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
