using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
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
    public class ISM_UserEmployeeMappingImpl : Interfaces.ISM_UserEmployeeMappingInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_UserEmployeeMappingImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }
        public async Task<Clients_DTO> getdetails(Clients_DTO data)
        {
            try
            {
                //data.get_departmentlist = _IssueContext.HR_Master_Department.Where(t => t.HRMD_ActiveFlag == true).Distinct().ToArray();
                //using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "ISM_Company_DepartmentList";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await dataReader.ReadAsync())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.get_departmentlist = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                data.get_employeelist = (from b in _IssueContext.MasterEmployee
                                         from c in _IssueContext.HR_Master_Designation
                                         where (b.HRME_ActiveFlag == true && b.HRMDES_Id == c.HRMDES_Id && b.HRME_LeftFlag == false)
                                         select new Clients_DTO
                                         {
                                             HRME_Id = b.HRME_Id,
                                             employeename = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                             HRME_EmployeeCode = b.HRME_EmployeeCode,
                                             designationName = c.HRMDES_DesignationName,

                                         }).Distinct().OrderBy(t => t.employeename).ToArray();

                data.get_userslist = (from a in _IssueContext.MasterEmployee
                                      from b in _IssueContext.Staff_User_Login
                                      where (a.HRME_Id == b.Emp_Code && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && b.IVRMSTAUL_ActiveFlag == 1)
                                      select new Clients_DTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          User_Id = b.Id,
                                          HRME_EmployeeCode = a.HRME_EmployeeCode,
                                          usersname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                      }).Distinct().OrderBy(t => t.employeename).ToArray();

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeCount";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_userEmpcount = retObject.ToArray();
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
        public Clients_DTO getemployeelist(Clients_DTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.User_Id).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                List<long?> mappedEmp_ids = new List<long?>();
                var mappedEmplist = (from a in _IssueContext.MasterEmployee
                                     from b in _IssueContext.ISM_User_Employees_MappingDMO
                                     where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && b.ISMUSEMM_ActiveFlag == true && b.User_Id == data.User_Id)
                                     select new Clients_DTO
                                     {
                                         HRME_Id = b.HRME_Id
                                     }).Distinct().OrderBy(t => t.HRME_Id).ToList();

                string mappedEmpid = "0";
                if (mappedEmplist.Count > 0)
                {
                    foreach (var mel in mappedEmplist)
                    {
                        mappedEmpid = mappedEmpid + "," + mel.HRME_Id;
                        mappedEmp_ids.Add(mel.HRME_Id);
                    }
                }

                data.get_employeelist = (from b in _IssueContext.MasterEmployee
                                         where (b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && b.HRMD_Id == data.HRMD_Id && !mappedEmp_ids.Contains(b.HRME_Id))
                                         select new Clients_DTO
                                         {
                                             HRME_Id = b.HRME_Id,
                                             employeename = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                             HRME_EmployeeCode = b.HRME_EmployeeCode
                                         }).Distinct().OrderBy(t => t.employeename).ToArray();

                data.get_userEmpDetails = (from a in _IssueContext.MasterEmployee
                                           from b in _IssueContext.Staff_User_Login
                                           from c in _IssueContext.ISM_User_Employees_MappingDMO
                                           where (a.HRME_Id == c.HRME_Id && b.Id == c.User_Id && c.User_Id == data.User_Id && a.HRME_ActiveFlag==true && a.HRME_LeftFlag==false)
                                           select new Clients_DTO
                                           {
                                               HRME_Id = c.HRME_Id,
                                               employeename = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                               HRME_EmployeeCode = a.HRME_EmployeeCode,
                                               ISMUSEMM_ActiveFlag = c.ISMUSEMM_ActiveFlag,
                                               ISMUSEMM_Id = c.ISMUSEMM_Id,
                                               ISMUSEMM_Order=c.ISMUSEMM_Order
                                           }).Distinct().OrderBy(t => t.employeename).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Clients_DTO savedata(Clients_DTO data)
        {
            try
            {

                if (data.ISMUSEMM_Id != 0)
                {
                    var res = _IssueContext.ISM_User_Employees_MappingDMO.Where(t => t.User_Id == data.User_Id && t.ISMUSEMM_Id != data.ISMUSEMM_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        if (data.employeeList.Length > 0)
                        {
                            foreach (var x in data.employeeList)
                            {
                                var res1 = _IssueContext.ISM_User_Employees_MappingDMO.Where(a => a.ISMUSEMM_Id == x.ISMUSEMM_Id).ToList();
                                if (res1.Count > 0)
                                {
                                    var res11 = _IssueContext.ISM_User_Employees_MappingDMO.Single(a => a.ISMUSEMM_Id == x.ISMUSEMM_Id);
                                    res11.HRME_Id = x.HRME_Id;
                                    res11.ISMUSEMM_UpdatedBy = data.UserId;
                                    res11.UpdatedDate = DateTime.Now;
                                    _IssueContext.Update(res11);
                                }
                                else
                                {
                                    ISM_User_Employees_MappingDMO uemp = new ISM_User_Employees_MappingDMO();
                                    uemp.User_Id = data.User_Id;
                                    uemp.HRME_Id = x.HRME_Id;
                                    uemp.ISMUSEMM_Order = x.ISMUSEMM_Order;
                                    uemp.ISMUSEMM_ActiveFlag = true;
                                    uemp.ISMUSEMM_CreatedBy = data.UserId;
                                    uemp.ISMUSEMM_UpdatedBy = data.UserId;
                                    uemp.ISMUSEMM_Proxyflag = x.proxyflag;
                                    uemp.CreatedDate = DateTime.Now;
                                    uemp.UpdatedDate = DateTime.Now;
                                    _IssueContext.Add(uemp);
                                }

                            }
                        }

                        var contactExists = _IssueContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    if (data.employeeList.Length > 0)
                    {
                        foreach (var x in data.employeeList)
                        {
                            var res = _IssueContext.ISM_User_Employees_MappingDMO.Where(t => t.User_Id == data.User_Id && t.HRME_Id == Convert.ToInt64(x.HRME_Id)).ToList();
                            if (res.Count > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                ISM_User_Employees_MappingDMO uemp = new ISM_User_Employees_MappingDMO();
                                uemp.User_Id = data.User_Id;
                                uemp.HRME_Id = x.HRME_Id;
                                uemp.ISMUSEMM_Order = x.ISMUSEMM_Order;
                                uemp.ISMUSEMM_ActiveFlag = true;
                                uemp.ISMUSEMM_CreatedBy = data.UserId;
                                uemp.ISMUSEMM_UpdatedBy = data.UserId;
                                uemp.ISMUSEMM_Proxyflag = x.proxyflag;
                                uemp.CreatedDate = DateTime.Now;
                                uemp.UpdatedDate = DateTime.Now;
                                _IssueContext.Add(uemp);
                            }                           
                        }
                    }
                    var contactExists = _IssueContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Clients_DTO deactive(Clients_DTO data)
        {
            try
            {
                var result = _IssueContext.ISM_User_Employees_MappingDMO.Single(t => t.ISMUSEMM_Id == data.ISMUSEMM_Id);

                if (result.ISMUSEMM_ActiveFlag == true)
                {
                    result.ISMUSEMM_ActiveFlag = false;
                }
                else if (result.ISMUSEMM_ActiveFlag == false)
                {
                    result.ISMUSEMM_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);
                int returnval = _IssueContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public Clients_DTO updateorder(Clients_DTO data)
        {
            try
            {
                if (data.employeeList.Length > 0)
                {
                    foreach (var x in data.employeeList)
                    {
                        var result = _IssueContext.ISM_User_Employees_MappingDMO.Single(t => t.ISMUSEMM_Id == x.ISMUSEMM_Id);

                        result.ISMUSEMM_Order = x.ISMUSEMM_Order;
                        result.UpdatedDate = DateTime.Now;
                        _IssueContext.Update(result);
                        int returnval = _IssueContext.SaveChanges();
                        if (returnval > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;           
        }

        //nagaraj added for report purpose
        public async Task<Clients_DTO> getdetailsNew(Clients_DTO data)
        {
            try
            {

                using (var cmdd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdd.CommandText = "HR_Employees_Head";
                    cmdd.CommandType = CommandType.StoredProcedure;

                    cmdd.Parameters.Add(new SqlParameter("@Flag",
             SqlDbType.VarChar)
                    {
                        Value = data.get_OnLoadEmpDetailsFlag
                    });

                    if (cmdd.Connection.State != ConnectionState.Open)
                        cmdd.Connection.Open();

                    var retObj = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmdd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObj.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_OnLoadEmpDetails = retObj.ToArray();
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

        public async Task<Clients_DTO> getUserEmpMappedReport(Clients_DTO data)
        {
            try
            {
                string hrmeIds = string.Join(",", data.HRMEIDS.Select(h => h.HRME_Id.ToString()).ToArray());
                using (var cmdd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmdd.CommandText = "HR_Employees_HeadWise_Employees";
                    cmdd.CommandType = CommandType.StoredProcedure;

                    cmdd.Parameters.Add(new SqlParameter("@Hrme_id",
             SqlDbType.VarChar)
                    {
                        //  Value = data.HRME_Id
                        Value = hrmeIds
                    });

                    if (cmdd.Connection.State != ConnectionState.Open)
                        cmdd.Connection.Open();

                    var retObj = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmdd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObj.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getUserEmpMappedReport = retObj.ToArray();
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
