using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class EmployeeMonthlyReportImpl : Interfaces.EmployeeMonthlyReportInterface
    {
        public FOContext _FOContext;
        private object multipletype;

        public EmployeeMonthlyReportImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }

        public EmployeeMonthlyReportDTO getdata(EmployeeMonthlyReportDTO data)
        {
            try
            {
                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList();
                data.filltypes = staf_types.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public EmployeeMonthlyReportDTO get_departments(EmployeeMonthlyReportDTO data)
        {
            var dept = data.multipletype.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dept.Length; i++)
            {
                list.Add(Convert.ToInt64(dept[i]));
            }
            data.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                   from b in _FOContext.HR_Master_Department_DMO
                                   from c in _FOContext.HR_Master_GroupType_DMO
                                   where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
                                       && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMGT_Id))
                                   select new EmployeeMonthlyReportDTO
                                   {
                                       HRMD_Id = b.HRMD_Id,
                                       HRMD_DepartmentName = b.HRMD_DepartmentName,
                                   }
                     ).Distinct().ToArray();

            return data;
        }
        public EmployeeMonthlyReportDTO get_designation(EmployeeMonthlyReportDTO data)
        {
            var dd = data.multipledep.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }
            data.filldesignation = (from a in _FOContext.HR_Master_Employee_DMO
                                    from b in _FOContext.HR_Master_Designation_DMO
                                    from c in _FOContext.HR_Master_Department_DMO
                                    where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                    && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                    && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && list.Contains(c.HRMD_Id))
                                    select new EmployeeMonthlyReportDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
        public EmployeeMonthlyReportDTO get_employee(EmployeeMonthlyReportDTO data)
        {
            var desig = data.multipledes.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < desig.Length; i++)
            {
                list.Add(Convert.ToInt64(desig[i]));
            }
            var dept = data.multipledep.Split(',');
            List<long> list2 = new List<long>();
            for (int i = 0; i < dept.Length; i++)
            {
                list2.Add(Convert.ToInt64(dept[i]));
            }
            var typ = data.multipletype.Split(',');
            List<long> list3 = new List<long>();
            for (int i = 0; i < typ.Length; i++)
            {
                list3.Add(Convert.ToInt64(typ[i]));
            }


            data.fillemployee = (from a in _FOContext.HR_Master_Employee_DMO
                                 where (a.MI_Id == data.MI_Id && list.Contains(a.HRMDES_Id) && list3.Contains(a.HRMGT_Id) && list2.Contains(a.HRMD_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                 select new EmployeeMonthlyReportDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' '
                                                + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                     //HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                     //HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                     HRME_EmployeeCode = a.HRME_EmployeeCode
                                 }
                ).Distinct().ToArray();

            return data;
        }
        
        public async Task<EmployeeMonthlyReportDTO> getreport(EmployeeMonthlyReportDTO data)
        {

            try
            {


                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Emp_Monthly_yearly_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                        SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                       SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",
                   SqlDbType.NVarChar)
                    {
                        Value = data.multiplehrmeid
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    if (data.type == null || data.type == "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@type",
               SqlDbType.VarChar)
                        {
                            Value = "monthly"
                        });
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@type",
              SqlDbType.VarChar)
                        {
                            Value = data.type
                        });
                    }

                    cmd.Parameters.Add(new SqlParameter("@cols",
                 SqlDbType.NVarChar, 2000)
                    {
                        Direction = ParameterDirection.Output
                    });
                    cmd.Parameters.Add(new SqlParameter("@totalpresent",
                 SqlDbType.VarChar, 10)
                    {
                        Direction = ParameterDirection.Output
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}

                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.filldata = retObject11.ToArray();

                        data.columnnames = cmd.Parameters["@cols"].Value.ToString();
                        data.totalworkingdays = cmd.Parameters["@totalpresent"].Value.ToString();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
               


                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Emps_Monthly_Report_VMS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",
                     SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
                       SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Multiplehrmeid",
                   SqlDbType.NVarChar)
                    {
                        Value = data.multiplehrmeid
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.filldataS = retObject.ToArray();
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

        public EmployeeMonthlyReportDTO get_employeeselected(EmployeeMonthlyReportDTO data)
        {
            try
            {
                var desig = data.multipledes.Split(',');
                List<long> list = new List<long>();
                for (int i = 0; i < desig.Length; i++)
                {
                    list.Add(Convert.ToInt64(desig[i]));
                }
                var dept = data.multipledep.Split(',');
                List<long> list2 = new List<long>();
                for (int i = 0; i < dept.Length; i++)
                {
                    list2.Add(Convert.ToInt64(dept[i]));
                }
                var typ = data.multipletype.Split(',');
                List<long> list3 = new List<long>();
                for (int i = 0; i < typ.Length; i++)
                {
                    list3.Add(Convert.ToInt64(typ[i]));
                }

                data.LoginId = _FOContext.Staff_User_Login.Where(t => t.Id == data.LoginId).Select(t => t.Emp_Code).FirstOrDefault();

                data.fillemployee = (from a in _FOContext.HR_Master_Employee_DMO
                                     from b in _FOContext.HR_Leave_Auth_OrderNo_DMO
                                     where (a.HRME_Id == b.HRME_Id && b.IVRMUL_Id == data.LoginId && list.Contains(a.HRMDES_Id) && list3.Contains(a.HRMGT_Id) && list2.Contains(a.HRMD_Id) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new EmployeeMonthlyReportDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                         HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                         HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                         HRME_EmployeeCode = a.HRME_EmployeeCode
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public EmployeeMonthlyReportDTO getalldetailsselected(EmployeeMonthlyReportDTO data)
        {
            try
            {
                data.LoginId = _FOContext.Staff_User_Login.Where(t => t.Id == data.LoginId).Select(t => t.Emp_Code).FirstOrDefault();
                List<HR_Master_GroupType> staf_types = new List<HR_Master_GroupType>();
                staf_types = (from a in _FOContext.HR_Master_GroupType_DMO
                              from b in _FOContext.HR_Leave_Auth_OrderNo_DMO
                              from c in _FOContext.HR_Leave_Authorisation_DMO
                              from d in _FOContext.Institution
                              where (a.HRMGT_Id == c.HRMGT_Id && b.HRLA_Id == c.HRLA_Id && b.IVRMUL_Id == data.LoginId && a.HRMGT_ActiveFlag == true && d.MI_ActiveFlag == 1 && a.MI_Id == d.MI_Id)
                              select new HR_Master_GroupType
                              {
                                  HRMGT_Id = a.HRMGT_Id,
                                  HRMGT_EmployeeGroupType = d.MI_Subdomain + "- " + a.HRMGT_EmployeeGroupType,
                                  HRMGT_Code = a.HRMGT_Code
                              }).Distinct().ToList();

                //staf_types = _FOContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList();
                data.filltypes = staf_types.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public EmployeeMonthlyReportDTO get_departmentselected(EmployeeMonthlyReportDTO data)
        {
            data.LoginId = _FOContext.Staff_User_Login.Where(t => t.Id == data.LoginId).Select(t => t.Emp_Code).FirstOrDefault();
            var dept = data.multipletype.Split(',');
            List<long> list = new List<long>();
            for (int i = 0; i < dept.Length; i++)
            {
                list.Add(Convert.ToInt64(dept[i]));
            }

            data.filldepartment = (from a in _FOContext.HR_Master_Department_DMO
                                   from b in _FOContext.HR_Leave_Auth_OrderNo_DMO
                                    from c in _FOContext.HR_Leave_Authorisation_DMO
                                   from d in _FOContext.Institution
                                   where (a.HRMD_Id == c.HRMD_Id && b.HRLA_Id == c.HRLA_Id && b.IVRMUL_Id == data.LoginId && a.HRMD_ActiveFlag == true && list.Contains(c.HRMGT_Id) && d.MI_ActiveFlag == 1 && a.MI_Id == d.MI_Id)
                          select new HR_Master_Department
                          {
                              HRMD_Id = a.HRMD_Id,
                              HRMD_DepartmentName = d.MI_Subdomain + "- " + a.HRMD_DepartmentName
                          }).Distinct().ToArray();

            return data;
        }

        public EmployeeMonthlyReportDTO get_designationselected(EmployeeMonthlyReportDTO data)
        {
            data.LoginId = _FOContext.Staff_User_Login.Where(t => t.Id == data.LoginId).Select(t => t.Emp_Code).FirstOrDefault();
            var dd = data.multipledep.Split(',');
            List<long?> list = new List<long?>();
            for (int i = 0; i < dd.Length; i++)
            {
                list.Add(Convert.ToInt64(dd[i]));
            }

            data.filldesignation = (from a in _FOContext.HR_Master_Designation_DMO
                                    from b in _FOContext.HR_Leave_Auth_OrderNo_DMO
                                   from c in _FOContext.HR_Leave_Authorisation_DMO
                                    from d in _FOContext.Institution
                                    where (a.HRMDES_Id == c.HRMDES_Id && b.HRLA_Id == c.HRLA_Id && b.IVRMUL_Id == data.LoginId && a.HRMDES_ActiveFlag == true && list.Contains(c.HRMD_Id) && d.MI_ActiveFlag == 1 && a.MI_Id == d.MI_Id)
                                   select new HR_Master_Designation
                                   {
                                       HRMDES_Id = a.HRMDES_Id,
                                       HRMDES_DesignationName = d.MI_Subdomain + "- " + a.HRMDES_DesignationName
                                   }).Distinct().ToArray();

            return data;
        }
    }
}

   