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
    public class ISM_TaskGroup_ReportImpl : Interfaces.ISM_TaskGroup_ReportInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_TaskGroup_ReportImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
        public async Task<ISM_TaskGroup_ReportDTO> getdetails(ISM_TaskGroup_ReportDTO data)
        {
            try
            {
                data.grouplist = (from a in _IssueContext.ISM_Master_TaskGroupDMO
                                  where (a.MI_Id == data.MI_Id && a.ISMMTGRP_ActiveFlag == true)
                                  select new ISM_TaskGroup_ReportDTO
                                  {
                                      ISMMTGRP_TaskGroupName = a.ISMMTGRP_TaskGroupName,
                                      ISMMTGRP_Id = a.ISMMTGRP_Id
                                  }).Distinct().ToArray();

                data.department_list = (from a in _IssueContext.HR_Master_Department
                                        from b in _IssueContext.ISM_TaskCreationDMO
                                        where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true)
                                        select new ISM_TaskGroup_ReportDTO
                                        {
                                            HRMD_Id = a.HRMD_Id,
                                            HRMD_DepartmentName = a.HRMD_DepartmentName
                                        }).Distinct().ToArray();
                data.projectlist = (from a in _IssueContext.MastersProject_DMO
                                    where (a.MI_Id == data.MI_Id && a.ISMMPR_ActiveFlg == true)
                                    select new ISM_TaskGroup_ReportDTO
                                    {
                                        ISMMPR_Id = a.ISMMPR_Id,
                                        ISMMPR_ProjectName = a.ISMMPR_ProjectName
                                    }).Distinct().ToArray();
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //public async Task<ISM_TaskGroup_ReportDTO> getdepartments(ISM_TaskGroup_ReportDTO data)
        //{
        //    try
        //    {
        //        data.department_list = (from a in _IssueContext.HR_Master_Department
        //                                from b in _IssueContext.ISM_TaskCreationDMO
        //                                where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true)
        //                                select new ISM_TaskGroup_ReportDTO
        //                                {
        //                                    HRMD_Id = a.HRMD_Id,
        //                                    HRMD_DepartmentName = a.HRMD_DepartmentName
        //                                }).Distinct().ToArray();
        //    }
        //    catch(Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return data;
        //}
        public async Task<ISM_TaskGroup_ReportDTO> getplannerReport(ISM_TaskGroup_ReportDTO data)
        {
            try
            {
                //group
                string group_ids = "0";
                List<long> group1_ids = new List<long>();
                foreach (var item in data.selectedgrouplist)
                {
                    group1_ids.Add(item.ISMMTGRP_Id);
                }
                for (int s = 0; s < group1_ids.Count(); s++)
                {
                    group_ids = group_ids + ',' + group1_ids[s].ToString();
                }

                // department
                string dept_ids = "0";
                List<long> dept1_ids = new List<long>();
                foreach (var item in data.selecteddepartment_list)
                {
                    dept1_ids.Add(item.HRMD_Id);
                }

                for (int s = 0; s < dept1_ids.Count(); s++)
                {
                    dept_ids = dept_ids + ',' + dept1_ids[s].ToString();
                }

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";
                //fromdatecon = Convert.ToDateTime("2020-02-17");
                try
                {
                    fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
                    contodate = toatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_TASKGROUP_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMMTGRP_Id",SqlDbType.VarChar)
                    {
                        Value = group_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",SqlDbType.VarChar)
                    {
                        Value = dept_ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                    {
                        Value = contodate
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
                        data.userdetails = retObject.ToArray();
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
        public async Task<ISM_TaskGroup_ReportDTO> getReport(ISM_TaskGroup_ReportDTO data)
        {
            try
            {
                string project_ids = "0";
                List<long> project1_ids = new List<long>();
                foreach (var item in data.selectedprojectlist)
                {
                    project1_ids.Add(item.ISMMPR_Id);
                }
                for (int s = 0; s < project1_ids.Count(); s++)
                {
                    project_ids = project_ids + ',' + project1_ids[s].ToString();
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_CLIENTPROJECTPAYMENT_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMMPR_Id",
                      SqlDbType.VarChar)
                    {
                        Value = project_ids
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
                        data.reportdetails = retObject.ToArray();
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

        /*CATEGORY REPORT*/
        public ISM_TaskGroup_ReportDTO getcategorydetails(ISM_TaskGroup_ReportDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                long? dept_id = 0;
                long? design_id = 0;
                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                    design_id = hrmeDepartment_id.FirstOrDefault().HRMDES_Id;
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentList";
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
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

                //department_list
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentList_CallCenter";
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.department_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //
                data.userdetails = _IssueContext.ISM_Task_CallCenter_RemarkDMO.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_TaskGroup_ReportDTO onchangedept(ISM_TaskGroup_ReportDTO data)
        {
            try
            {
                data.getcategory = (from a in _IssueContext.ISM_Master_TaskCategory_DMO
                                    from c in _IssueContext.HR_Master_Department
                                    where (a.HRMD_Id == c.HRMD_Id && a.ISMMTCAT_ActiveFlag == true && c.HRMDC_ID == data.HRMDC_ID)
                                    select new ISM_TaskGroup_ReportDTO
                                    {
                                        ISMMTCAT_Id = a.ISMMTCAT_Id,
                                        ISMMTCAT_TaskCategoryName = a.ISMMTCAT_TaskCategoryName
                                    }).Distinct().ToArray();


                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.roleflag = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.roleflag = false;
                    }
                }
                else
                {
                    roletype = "";
                    data.roleflag = true;
                }

                if(data.ISMMPR_ProjectName=="Call_Center")
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_User_EmployeeList_CallCenter";
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
                        cmd.Parameters.Add(new SqlParameter("@Dept_Id", SqlDbType.VarChar)
                        {
                            Value = data.HRMDC_ID
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
                            data.getemployee = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_User_EmployeeList_admin_Dept_Wise";
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
                        cmd.Parameters.Add(new SqlParameter("@Dept_Id", SqlDbType.BigInt)
                        {
                            Value = data.HRMDC_ID
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
                            data.getemployee = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_TaskGroup_ReportDTO categorygroupReport(ISM_TaskGroup_ReportDTO data)
        {
            try
            {
                string HRME_Id = "0";
                foreach (var c in data.categoryemptempdto)
                {
                    HRME_Id = HRME_Id + "," + c.HRME_Id;
                }

                string categoryid = "0";
                foreach (var c in data.categorytempdto)
                {
                    categoryid = categoryid + "," + c.ISMMTCAT_Id;
                }

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";                 
                try
                {
                    fromdatecon = Convert.ToDateTime(data.startdate.Value.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    toatecon = Convert.ToDateTime(data.enddate.Value.ToString("yyyy-MM-dd"));
                    contodate = toatecon.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Detailed_Category_Group_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDC_ID", SqlDbType.VarChar)
                    {
                        Value = data.HRMDC_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@Category_Id", SqlDbType.VarChar)
                    {
                        Value = categoryid
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
                        data.get_report = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //ISM_Task_CallCenter_RemarkDMO
        //  SaveFeedBack
        public ISM_TaskGroup_ReportDTO SaveFeedBack(ISM_TaskGroup_ReportDTO data)
        {
            try
            {
                if(data.selecteddepartment_list !=null  && data.selecteddepartment_list.Length >0)
                {
                    foreach(var d in data.selecteddepartment_list)
                    {
                        ISM_Task_CallCenter_RemarkDMO  obj= new ISM_Task_CallCenter_RemarkDMO();
                        obj.ISMTCR_Id = d.ISMTCR_Id;
                        obj.ISMTCCR_Remarks = d.ISMTPLAPTA_Remarks;
                        obj.ISMTCCR_CreatedDate = DateTime.Now;
                        obj.ISMTCCR_UpdatedDate = DateTime.Now;
                        obj.ISMTCCR_CreatedBy = data.UserId;
                        obj.ISMTCCR_UpdateBy = data.UserId;
                        _IssueContext.Add(obj);
                    }
                    int i=   _IssueContext.SaveChanges();
                    if(i > 0)
                    {
                        data.ISMTPLAPTA_Status = "save";
                    }

                }
                
            }
            catch (Exception ex)
            {
                data.ISMTPLAPTA_Status = "admin";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
