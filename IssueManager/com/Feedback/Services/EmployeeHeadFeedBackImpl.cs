 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.FeedBack;

namespace IssueManager.com.Feedback.Services
    {
        public class EmployeeHeadFeedBackImpl : Interface.EmployeeHeadFeedBackInterface
    {
            public IssueManagerContext _context;
            public EmployeeHeadFeedBackImpl(IssueManagerContext _cont)
            {
                _context = _cont;
            }

       

        public async Task<EmployeeHeadFeedBackDTO> getdetails(EmployeeHeadFeedBackDTO data)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeFeedback";
                    cmd.CommandType = CommandType.StoredProcedure;
          
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
                        data.feedbackgrid = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*******************************************************************// 



                var rolet = _context.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
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
                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_admin";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                        data.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //*********************************** User Department List ********************************// 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DepartmentList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
SqlDbType.BigInt)
                    {
                        Value = dept_id
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
                    cmd.CommandText = "DesignationList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@role",
      SqlDbType.VarChar)
                    {
                        Value = roletype
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                        data.Designation = retObject.ToArray();
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




        public EmployeeHeadFeedBackDTO savedetails(EmployeeHeadFeedBackDTO data)
        {
            try
            {
                foreach (var c in data.totalgrid)
                {
                    EmployeeHeadFeedBackDMO feed = new EmployeeHeadFeedBackDMO();
                    
                    feed.MI_Id = data.MI_Id;
                    // feed.ASMAY_Id = data.ASMAY_Id;
                    feed.HRME_Id = c.HRME_Id;
                    feed.HRGFE_FeedBack = c.HRGFE_FeedBack;
                    feed.HRGFE_FeedbackDate = data.HRGFE_FeedbackDate;
                    feed.HRGFE_ActiveFlag = true;
                    feed.HRGFE_CreatedBy = data.UserId;
                    feed.HRGFE_UpdatedBy = data.UserId;
                    feed.CreatedDate = DateTime.Now;
                    feed.UpdatedDate = DateTime.Now;
                    _context.Add(feed);
                };
                    
                        
                        var contactExists = _context.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        





      
                       







public EmployeeHeadFeedBackDTO delete_data(int id)
        {
            EmployeeHeadFeedBackDTO delete = new EmployeeHeadFeedBackDTO();
            try
            {
                List<EmployeeHeadFeedBackDMO> lorg = new List<EmployeeHeadFeedBackDMO>();
                lorg = _context.EmployeeHeadFeedBackDMO.Where(t => t.HRGFE_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _context.Remove(lorg.ElementAt(0));
                    var contactExists = _context.SaveChanges();
                    if (contactExists == 1)
                    {
                        delete.returnval = true;
                    }
                    else
                    {
                        delete.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return delete;
        }

    }
    }