using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
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
    public class ISM_ComplaintsTaskListImpl : Interfaces.ISM_ComplaintsTaskListInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_ComplaintsTaskListImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
        public async Task<ISM_ComplaintsTaskListDTO> getdetails(ISM_ComplaintsTaskListDTO data)
        {
            try
            {

                data.arrlistchk = (from a in _Context.HR_Master_DepartmentCodeDMO
                                   select new ISM_ComplaintsTaskListDTO
                                   {
                                       HRMDC_ID = a.HRMDC_ID,
                                       HRMDC_Name = a.HRMDC_Name,
                                       HRMDC_Order = a.HRMDC_Order

                                   }).Distinct().ToArray();

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                List<long?> department_ids = new List<long?>();
                string statuslist = "Open,ReOpen,Inprogress,In progress";
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    data.HRME_Id = empid;
                }

                var hrmeDepartment_head = _IssueContext.HR_Master_DepartmentCode_Head.Where(t => t.HRME_ID == data.HRME_Id).ToList();
                if (hrmeDepartment_head.Count > 0)
                {
                    data.roletype = "Head";
                }

                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid).Distinct().ToArray();
                string departmentid = "0";
                if (hrmeDepartment_id.Length > 0)
                {
                    foreach (var ei in hrmeDepartment_id)
                    {
                        departmentid = departmentid + "," + ei.HRMD_Id;
                        department_ids.Add(ei.HRMD_Id);
                    }
                }
                List<long?> institute_ids = new List<long?>();
                string mi_ids = "0";
                var inst_id = _IssueContext.IVRM_User_Login_InstitutionwiseDMO.Where(m => m.Id == data.UserId && m.Activeflag == 1).Distinct().ToList();
                if (inst_id.Count > 0)
                {
                    foreach (var inst in inst_id)
                    {
                        mi_ids = mi_ids + "," + inst.MI_Id;
                        institute_ids.Add(inst.MI_Id);
                    }
                }
                data.get_Status = _IssueContext.MasterStatus_DMO.Where(t => t.MI_Id == data.MI_Id && department_ids.Contains(t.HRMD_Id) && t.ISMMISTS_ActiveFlag == true).Distinct().OrderBy(t => t.ISMMISTS_Order).ToArray();

                //*********************************** MY PENDING Task List ************************************************//           
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Pending_AssignedBy_To_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@RoleFlg",
                          SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
                    });
                    cmd.Parameters.Add(new SqlParameter("@countflag",
           SqlDbType.VarChar)
                    {
                        Value = "MyPending"
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.VarChar)
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
                        data.getmy_comptasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //*********************************** Employee List ********************************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_EmployeeList_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Userid",
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
                        data.get_employeelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Employee Mapped List ********************************// 

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList_CmplaintTask";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@user_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
          SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
          SqlDbType.BigInt)
                    {
                        Value = empid
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


                List<long?> transfer_ids = new List<long?>();
                var transfercount = _IssueContext.ISM_TaskCreation_TransferredToDMO.Where(t => t.ISMTCRTRTO_ActiveFlg == true && t.ISMTCRTRTO_TransferredBy == empid).Distinct().ToList();
                if (transfercount.Count > 0)
                {
                    foreach (var tr in transfercount)
                    {
                        transfer_ids.Add(tr.ISMTCR_Id);
                    }
                }
                //****************************************************** Assigned By Me Task Count                
                var byMe = (from a in _IssueContext.ISM_TaskCreationDMO
                            from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                            where (a.ISMTCR_Id == b.ISMTCR_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCRASTO_ActiveFlg == true && b.ISMTCRASTO_AssignedBy == empid)
                            select new ISM_ComplaintsTaskListDTO
                            {
                                ISMTCR_Id = b.ISMTCR_Id
                            }).ToList();
                if (byMe.Count > 0)
                {
                    data.byMecount = byMe.Count();
                }
                else
                {
                    data.byMecount = 0;
                }
                //****************************************************** Assigned To Me Task Count    
                var toMe = (from a in _IssueContext.ISM_TaskCreationDMO
                            from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                            where (a.ISMTCR_Id == b.ISMTCR_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCRASTO_ActiveFlg == true && b.HRME_Id == empid)
                            select new ISM_ComplaintsTaskListDTO
                            {
                                ISMTCR_Id = b.ISMTCR_Id
                            }).ToList();
                if (toMe.Count > 0)
                {
                    data.toMecount = toMe.Count();
                }
                else
                {
                    data.toMecount = 0;
                }


                var toMeonly = (from a in _IssueContext.ISM_TaskCreationDMO
                                from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                where (a.ISMTCR_Id == b.ISMTCR_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCRASTO_ActiveFlg == true && b.HRME_Id == empid && (a.ISMTCR_Status == "Open" || a.ISMTCR_Status == "ReOpen" || a.ISMTCR_Status == "Inprogress" || a.ISMTCR_Status == "In progress"))
                                select new ISM_ComplaintsTaskListDTO
                                {
                                    ISMTCR_Id = b.ISMTCR_Id
                                }).ToList();
                if (toMeonly.Count > 0)
                {
                    data.Mycountonly = toMeonly.Count();
                }
                else
                {
                    data.Mycountonly = 0;
                }
                //To get (COUNT) Uncomment Start

                var transferMe = (from a in _IssueContext.ISM_TaskCreationDMO
                                  from b in _IssueContext.ISM_TaskCreation_TransferredToDMO
                                  where (a.ISMTCR_Id == b.ISMTCR_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCRTRTO_ActiveFlg == true && b.HRME_Id == empid)
                                  select new ISM_ComplaintsTaskListDTO
                                  {
                                      ISMTCR_Id = b.ISMTCR_Id
                                  }).ToList();
                if (transferMe.Count > 0)
                {
                    data.transferMecount = transferMe.Count();
                }
                else
                {
                    data.transferMecount = 0;
                }

                var transferMeonly = (from a in _IssueContext.ISM_TaskCreationDMO
                                      from b in _IssueContext.ISM_TaskCreation_TransferredToDMO
                                      where (a.ISMTCR_Id == b.ISMTCR_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCRTRTO_ActiveFlg == true && b.HRME_Id == empid && (a.ISMTCR_Status == "Open" || a.ISMTCR_Status == "ReOpen" || a.ISMTCR_Status == "Inprogress" || a.ISMTCR_Status == "In progress"))
                                      select new ISM_ComplaintsTaskListDTO
                                      {
                                          ISMTCR_Id = b.ISMTCR_Id
                                      }).ToList();
                if (transferMeonly.Count > 0)
                {
                    data.transferMecountonly = transferMeonly.Count();
                }
                else
                {
                    data.transferMecountonly = 0;
                }

                var transferbycount = (from a in _IssueContext.ISM_TaskCreationDMO
                                       from b in _IssueContext.ISM_TaskCreation_TransferredToDMO
                                       where (a.ISMTCR_Id == b.ISMTCR_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCRTRTO_ActiveFlg == true && b.ISMTCRTRTO_TransferredBy == empid)
                                       select new ISM_ComplaintsTaskListDTO
                                       {
                                           ISMTCR_Id = b.ISMTCR_Id
                                       }).ToList();
                if (transferMe.Count > 0)
                {
                    data.transferbycount = transferbycount.Count();
                }
                else
                {
                    data.transferbycount = 0;
                }

                data.Mycountonly = data.Mycountonly + data.transferMecountonly;

                //To get (COUNT) Uncomment End

                //*********************************** Pending By & To Me task Count ************************************************//   
                var cnt = "NotAssign";
                //ISM_Pending_AssignedBy_Approved_Details
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                   // cmd.CommandText = "ISM_Pending_AssignedBy_Approved_Details";
                     cmd.CommandText = "ISM_Pending_AssignedBy_To_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@RoleFlg",
           SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
                    });
                    cmd.Parameters.Add(new SqlParameter("@countflag",
           SqlDbType.VarChar)
                    {
                        Value = cnt
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.VarChar)
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
                        data.get_tasks = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** Completed tasks created by me task Count ************************************************//   

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PendingCloseTask_List_DR_Dashboard_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE",
            SqlDbType.VarChar)
                    {
                        Value = DateTime.Now
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
                        data.completedcreatedbyme = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** Completed tasks created assigned By  Me task Count **************************************//                   
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CompletedTaskCount_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.BigInt)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@falg",
            SqlDbType.VarChar)
                    {
                        Value = "C"
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
                        data.get_tasks_completed = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //get_comptasklist
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Pending_AssignedBy_Approved_Details";
                    // cmd.CommandText = "ISM_Pending_AssignedBy_To_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@RoleFlg",
           SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
                    });
                    cmd.Parameters.Add(new SqlParameter("@countflag",
           SqlDbType.VarChar)
                    {
                        Value = "Rejecttask"
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.VarChar)
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
                        data.get_comptasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.get_priority = _IssueContext.MastersPriorities_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMP_ActiveFlag == true).Distinct().OrderBy(t => t.HRMP_Order).ToArray();

                //added By sanjeev
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_RejectedDailyReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.VarChar)
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
                        data.Rejected_comptasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //get_pendingcount
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Pending_AssignedBy_Approved_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@RoleFlg",
           SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
                    });
                    cmd.Parameters.Add(new SqlParameter("@countflag",
           SqlDbType.VarChar)
                    {
                        Value = "Notapproved"
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.VarChar)
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
                        data.get_pendingcount = retObject.ToArray();
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
        public async Task<ISM_ComplaintsTaskListDTO> deptfilter(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                if (data.savegroup.Length > 0)
                {
                    string pidsss = "100000";
                    foreach (var item in data.savegroup)
                    {
                        pidsss = pidsss + "," + item.HRMDC_ID;
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_OnChangeofDept";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRMD_IDsss",
                                  SqlDbType.VarChar)
                        {
                            Value = pidsss
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
                            data.get_employeelist = retObject.ToArray();
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
                        cmd.CommandText = "ISM_EmployeeList_New";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Userid",
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
                            data.get_employeelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }



            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<ISM_ComplaintsTaskListDTO> getsearchtask(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                string hrmeids = "0";
                if (data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrmeids = hrmeids + "," + ue.HRME_Id;
                    }
                    hrmeids = hrmeids.Substring(2);
                }
                else
                {
                    if (hrmeids == "" || hrmeids == "0")
                    {
                        hrmeids = empid.ToString();
                    }
                }
                string statuslist = "";
                if (data.arrayStatus.Length > 0)
                {
                    foreach (var sts in data.arrayStatus)
                    {
                        statuslist = statuslist + "," + sts.ISMMISTS_StatusName;
                    }
                }
                else
                {
                    if (statuslist == "")
                    {
                        statuslist = "Open,ReOpen,Inprogress,In progress";
                    }
                }
                //*********************************** Task List ************************************************//           
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_Task_Cart_bkp";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@RoleFlg",
           SqlDbType.VarChar)
                    {
                        Value = rolet.FirstOrDefault().IVRMRT_Role
                    });
                    cmd.Parameters.Add(new SqlParameter("@TypeFlg",
           SqlDbType.VarChar)
                    {
                        Value = data.typeFlg
                    });
                    cmd.Parameters.Add(new SqlParameter("@status",
           SqlDbType.VarChar)
                    {
                        Value = statuslist
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMM_Id",
            SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.VarChar)
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
                        data.get_comptasklist = retObject.ToArray();
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
        public async Task<ISM_ComplaintsTaskListDTO> getsearchtaskdetails(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                string hrmeids = "0";
                if (data.arrayuserEmp != null && data.arrayuserEmp.Length > 0)
                {
                    foreach (var ue in data.arrayuserEmp)
                    {
                        hrmeids = hrmeids + "," + ue.HRME_Id;
                    }
                    hrmeids = hrmeids.Substring(2);
                }
                else
                {
                    if (hrmeids == "" || hrmeids == "0")
                    {
                        hrmeids = empid.ToString();
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_Task_Search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TaskNo",
           SqlDbType.VarChar)
                    {
                        Value = data.ISMTCR_TaskNo
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value = hrmeids
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
                        data.get_comptasklist = retObject.ToArray();
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
        public async Task<ISM_ComplaintsTaskListDTO> getcount(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                if (data.countflag != "Completed" && data.countflag != "Close" && data.countflag != "RejectedTask")
                {
                    //*********************************** Pending By & To Me task Count ************************************************//   
                    string flags = "";
                    //if (data.countflag == "NotAssign")
                    //{
                    //    flags = "ISM_Pending_AssignedBy_Approved_Details";
                    //}
                    //else
                    //{
                    //    flags = "ISM_Pending_AssignedBy_To_Details_New";
                    //}
                    flags = "ISM_Pending_AssignedBy_To_Details_New";
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = flags;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@RoleFlg",
               SqlDbType.VarChar)
                        {
                            Value = rolet.FirstOrDefault().IVRMRT_Role
                        });
                        cmd.Parameters.Add(new SqlParameter("@countflag",
               SqlDbType.VarChar)
                        {
                            Value = data.countflag
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.VarChar)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@userid",
                SqlDbType.VarChar)
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
                            data.get_tasks = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.countflag == "Completed")
                {
                    //*********************************** Completed tasks created assigned By  Me task Count **************************************//                   
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CompletedTaskCount_List";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.BigInt)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@userid",
                SqlDbType.BigInt)
                        {
                            Value = data.UserId
                        });
                        cmd.Parameters.Add(new SqlParameter("@falg",
            SqlDbType.VarChar)
                        {
                            Value = "ALL"
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
                            data.get_tasks = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.countflag == "Close")
                {
                    //*********************************** Completed tasks created assigned By  Me task Count **************************************//                   
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PendingCloseTask_List";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.BigInt)
                        {
                            Value = empid
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
                            data.get_tasks = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.countflag == "RejectedTask")
                {
                    //*********************************** Completed tasks created assigned By  Me task Count **************************************//                   
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "RejectedCloseTask_List";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.BigInt)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_ID",
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
                            data.get_tasks = retObject.ToArray();
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
        public async Task<ISM_ComplaintsTaskListDTO> gettaskdetails(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                data.Role_flag = "";
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    data.HRME_Id = empid;
                    var checkhead = _IssueContext.HR_Master_DepartmentCode_Head.Where(t => t.HRME_ID == empid).ToList();
                    if (checkhead.Count > 0)
                    {
                        data.Role_flag = "Head";
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_TaskDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
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
                        data.get_taskdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_TaskResponse";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
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
                        data.get_taskresponse = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_TaskAttachements";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
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
                        data.get_taskattachments = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_ResponseAttachements";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
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
                        data.get_responseattachments = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Check_Task_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.BigInt)
                    {
                        Value = empid
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
                        data.Temparray = retObject.ToArray();
                        if (data.Temparray.Length > 0)
                        {
                            data.taskeditable = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Check_Assign_Task_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.BigInt)
                    {
                        Value = empid
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
                        data.Temparray2 = retObject.ToArray();
                        if (data.Temparray2.Length > 0)
                        {
                            data.assigntaskeditable = true;
                        }
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
        public ISM_ComplaintsTaskListDTO savePlanner(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                if (data.plannerArray != null)
                {
                    foreach (var pa in data.plannerArray)
                    {
                        foreach (var te in data.taskEmpArray)
                        {
                            if (pa.ISMTCR_Id == te.ISMTCR_Id)
                            {
                                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(pa.ISMTCRASTO_StartDate), INDIAN_ZONE);
                                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(pa.ISMTCRASTO_EndDate), INDIAN_ZONE);

                                string confromdate = "";
                                string contodate = "";

                                try
                                {
                                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                    contodate = todatecon.ToString("yyyy-MM-dd");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                                taskasign.ISMTCR_Id = pa.ISMTCR_Id;
                                taskasign.HRME_Id = te.HRME_Id;
                                taskasign.ISMTCRASTO_AssignedDate = indianTime;
                                taskasign.ISMTCRASTO_AssignedBy = empid;
                                taskasign.ISMTCRASTO_Remarks = pa.ISMTCRASTO_Remarks;
                                taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(confromdate);
                                taskasign.ISMTCRASTO_EndDate = Convert.ToDateTime(contodate);
                                taskasign.ISMTCRASTO_EffortInHrs = pa.ISMTCRASTO_EffortInHrs;
                                taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                                taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                                taskasign.ISMTCRASTO_ActiveFlg = true;
                                taskasign.CreatedDate = indianTime;
                                taskasign.UpdatedDate = indianTime;
                                _IssueContext.Add(taskasign);

                                if (te.HRMPR_Id > 0)
                                {
                                    var checktasktr = _IssueContext.ISM_TaskCreationDMO.Where(a => a.ISMTCR_Id == te.ISMTCR_Id).ToList();
                                    //----
                                    var checktasktransferresult = _IssueContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == te.ISMTCR_Id);

                                    checktasktransferresult.HRMPR_Id = Convert.ToInt64(te.HRMPR_Id);
                                    checktasktransferresult.UpdatedDate = indianTime;
                                    _IssueContext.Update(checktasktransferresult);
                         
                                    if (te.HRMPR_Id != checktasktr.FirstOrDefault().HRMPR_Id)
                                    {
                                        ISM_TaskCreation_PriorityHistoryDMO priority = new ISM_TaskCreation_PriorityHistoryDMO();
                                        priority.ISMTCR_Id = te.ISMTCR_Id;
                                        priority.HRMPR_Id = checktasktr.FirstOrDefault().HRMPR_Id;
                                        priority.ISMTCRPH_Remarks = pa.ISMTCRASTO_Remarks;
                                        priority.ISMTCRPH_ActiveFlg = true;
                                        priority.ISMTCRPH_CreatedDate = DateTime.Now;
                                        priority.ISMTCRPH_UpdatedDate = DateTime.Now;
                                        priority.ISMTCRPH_CreatedBy = data.UserId;
                                        priority.ISMTCRPH_UpdatedBy = data.UserId;
                                        _IssueContext.Add(priority);
                                    }

                                }
                            }
                        }
                    }
                }
                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                    Saveplannernotification(data);
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
        public void Saveplannernotification(ISM_ComplaintsTaskListDTO data)
        {
            var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
            long empid = 0;
            if (hrmEempid.Length > 0)
            {
                empid = hrmEempid.FirstOrDefault().Emp_Code;
            }
            PushNotification pb = new PushNotification(_Context);
            data.returnval = true;
            // ============================================== Push Notification Assigned                    
            var deviceidsnew = "";
            var devicenew = "";
            foreach (var pa in data.plannerArray)
            {
                foreach (var te in data.taskEmpArray)
                {
                    if (pa.ISMTCR_Id == te.ISMTCR_Id)
                    {
                        if (te.HRME_Id != empid)
                        {
                            pb.savewebnotificationasy(data.MI_Id, "TASKASSIGNEDWEB", te.ISMTCR_Id, te.ISMTCR_Id, te.HRME_Id, data.UserId, "Assigned");
                            // assignto.Add(te.HRME_Id);
                            var devicelist = (from a in _IssueContext.MasterEmployee
                                              where (a.HRME_ActiveFlag == true && a.HRME_Id == te.HRME_Id)
                                              select new ISM_ComplaintsTaskListDTO
                                              {
                                                  HRME_To = a.HRME_Id,
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToList();
                            if (devicelist.Count > 0)
                            {
                                data.HRME_To = devicelist.FirstOrDefault().HRME_To;
                                data.deviceArray = devicelist.ToArray();
                            }
                            if (devicelist.Count > 0)
                            {
                                int k = 0;
                                foreach (var deviceid in devicelist)
                                {
                                    if (k == 0)
                                    {
                                        deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                        k = 1;
                                    }
                                    else
                                    {
                                        deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                    }
                                }
                                devicenew = "[" + deviceidsnew + "]";
                            }
                            var pushemployee = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_Id == pa.ISMTCR_Id).ToList();
                            if (te.HRME_Id != empid)
                            {
                                try
                                {
                                    string subject = "New Task Assigned";
                                    string titletext = pushemployee.FirstOrDefault().ISMTCR_TaskNo + " - " + pushemployee.FirstOrDefault().ISMTCR_Title;
                                    string tasktype = "Assigned";
                                    if (data.sound == "" || data.sound == null)
                                    {
                                        data.sound = "default";
                                    }
                                    callnotificationasy(devicenew, data.HRME_To, pa.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                                    Insertnotificationasy(data.MI_Id, data.HRME_To, pa.ISMTCR_Id, titletext, tasktype, data.UserId);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            try
                            {
                                if (pushemployee.Count > 0)
                                {
                                    if (te.HRME_Id != empid)
                                    {
                                        var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                                             from a in _IssueContext.Staff_User_Login
                                                             where (e.ISMPNU_Userid == a.Id && a.Emp_Code == te.HRME_Id)
                                                             select new ISM_ComplaintsTaskListDTO
                                                             {
                                                                 UserId = e.ISMPNU_Userid
                                                             }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                                        if (webpushtocken.Count > 0)
                                        {
                                            foreach (var webtockenid in webpushtocken)
                                            {
                                                PushNotification ObjPushNotification = new PushNotification(_Context);
                                                ObjPushNotification.callpushnotificationasy(pushemployee.FirstOrDefault().ISMTCR_Title, "New Task Assigned!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + pushemployee.FirstOrDefault().ISMTCR_TaskNo + "&taskid=" + pushemployee.FirstOrDefault().ISMTCR_Id, webtockenid.UserId);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
        }
        public ISM_ComplaintsTaskListDTO saveTransfer(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                PushNotification pb = new PushNotification(_Context);
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                long oldassiemployee = 0;
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                if (data.transferArray != null)
                {
                    foreach (var ta in data.transferArray)
                    {
                        data.ISMTCR_Id = ta.ISMTCR_Id;
                        foreach (var id in data.hrme_list)
                        {
                            var checkplanner = (from a in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                from b in _IssueContext.MasterEmployee
                                                from c in _IssueContext.ISM_Task_PlannerDMO
                                                from d in _IssueContext.ISM_Task_Planner_TasksDMO
                                                where (a.HRME_Id == b.HRME_Id && b.HRME_Id == c.HRME_Id && c.ISMTPL_Id == d.ISMTPL_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && a.ISMTCR_Id == ta.ISMTCR_Id && d.ISMTCR_Id == ta.ISMTCR_Id)
                                                select new ISM_ComplaintsTaskListDTO
                                                {
                                                    ISMTCR_Id = d.ISMTCR_Id
                                                }).Distinct().ToList();

                            //var checkplanner = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == ta.ISMTCR_Id).ToList();
                            if (checkplanner.Count() == 0 || data.transfer == true)
                            {
                                var checktasktransfer = (from a in _IssueContext.ISM_TaskCreationDMO
                                                         from b in _IssueContext.ISM_TaskCreation_TransferredToDMO
                                                         where (a.ISMTCR_Id == b.ISMTCR_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCRTRTO_ActiveFlg == true && b.ISMTCR_Id == ta.ISMTCR_Id)
                                                         select new ISM_ComplaintsTaskListDTO
                                                         {
                                                             ISMTCR_Id = b.ISMTCR_Id
                                                         }).Distinct().ToList();

                                if (checktasktransfer.Count() > 0)
                                {
                                    var checktasktransferresult = _IssueContext.ISM_TaskCreation_TransferredToDMO.Single(a => a.ISMTCR_Id == ta.ISMTCR_Id);
                                    checktasktransferresult.ISMTCRTRTO_TransferredBy = empid;

                                    checktasktransferresult.HRME_Id = id.HRME_Id;

                                    checktasktransferresult.ISMTCRTRTO_Remarks = ta.ISMTCRTRTO_Remarks;
                                    checktasktransferresult.ISMTCRTRTO_StartDate = ta.ISMTCRTRTO_StartDate;
                                    checktasktransferresult.ISMTCRTRTO_EndDate = ta.ISMTCRTRTO_EndDate;
                                    checktasktransferresult.ISMTCRTRTO_EffortInHrs = ta.ISMTCRTRTO_EffortInHrs;
                                    checktasktransferresult.UpdatedDate = DateTime.UtcNow;
                                    checktasktransferresult.UpdatedDate = DateTime.UtcNow;
                                    _IssueContext.Update(checktasktransferresult);


                                    var updateassign = _IssueContext.ISM_TaskCreation_AssignedToDMO.Single(a => a.ISMTCR_Id == ta.ISMTCR_Id);
                                    var oldassigndate = updateassign.ISMTCRASTO_AssignedDate.ToString("dd/MM/yyyy");
                                    oldassiemployee = updateassign.HRME_Id;
                                    var oldassignemployee = ((from a in _IssueContext.MasterEmployee
                                                              where (a.HRME_Id == updateassign.HRME_Id)
                                                              select new ISM_ComplaintsTaskListDTO
                                                              {
                                                                  HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName)
                                                              }).FirstOrDefault()).HRME_EmployeeFirstName;

                                    updateassign.ISMTCRASTO_AssignedBy = empid;
                                    updateassign.ISMTCRASTO_ActiveFlg = true;
                                    updateassign.HRME_Id = id.HRME_Id;
                                    updateassign.ISMTCRASTO_Remarks = ta.ISMTCRTRTO_Remarks;
                                    updateassign.ISMTCRASTO_StartDate = ta.ISMTCRTRTO_StartDate;
                                    updateassign.ISMTCRASTO_EndDate = ta.ISMTCRTRTO_EndDate;
                                    updateassign.ISMTCRASTO_EffortInHrs = ta.ISMTCRTRTO_EffortInHrs;
                                    updateassign.UpdatedDate = indianTime;
                                    updateassign.ISMTCRASTO_AssignedDate = indianTime;
                                    _IssueContext.Update(updateassign);

                                    ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();
                                    taskresponse.ISMTCR_Id = ta.ISMTCR_Id;
                                    taskresponse.HRME_Id = empid;
                                    taskresponse.ISMTCRRES_ResponseDate = indianTime;
                                    taskresponse.ISMTCRRES_Response = "Previously Assigned To " + oldassignemployee + " On Date: " + oldassigndate + ". \nTransfer Remarks: " + ta.ISMTCRTRTO_Remarks;
                                    taskresponse.ISMTCRRES_ResponseAttachment = "";
                                    taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                                    taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                                    taskresponse.ISMTCRRES_Status = "Open";
                                    taskresponse.ISMTCRRES_ResponsePageName = "Transfer Page";
                                    taskresponse.ISMTCRRES_ActiveFlg = true;
                                    taskresponse.CreatedDate = indianTime;
                                    taskresponse.UpdatedDate = indianTime;
                                    _IssueContext.Add(taskresponse);
                                }
                                else
                                {
                                    ISM_TaskCreation_TransferredToDMO tasktransfer = new ISM_TaskCreation_TransferredToDMO();
                                    tasktransfer.ISMTCR_Id = ta.ISMTCR_Id;


                                    tasktransfer.HRME_Id = id.HRME_Id;

                                    // tasktransfer.HRME_Id = ta.HRME_Id;
                                    tasktransfer.ISMTCRTRTO_TransferredDate = data.ISMTCRTRTO_TransferredDate;
                                    tasktransfer.ISMTCRTRTO_TransferredBy = empid;
                                    tasktransfer.ISMTCRTRTO_Remarks = ta.ISMTCRTRTO_Remarks;
                                    tasktransfer.ISMTCRTRTO_StartDate = ta.ISMTCRTRTO_StartDate;
                                    tasktransfer.ISMTCRTRTO_EndDate = ta.ISMTCRTRTO_EndDate;
                                    tasktransfer.ISMTCRTRTO_EffortInHrs = ta.ISMTCRTRTO_EffortInHrs;
                                    tasktransfer.ISMTCRTRTO_CreatedBy = data.UserId;
                                    tasktransfer.ISMTCRTRTO_UpdatedBy = data.UserId;
                                    tasktransfer.ISMTCRTRTO_ActiveFlg = true;
                                    tasktransfer.CreatedDate = indianTime;
                                    tasktransfer.UpdatedDate = indianTime;
                                    _IssueContext.Add(tasktransfer);

                                    var updateassign = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.ISMTCR_Id == ta.ISMTCR_Id).FirstOrDefault();
                                    //var updateassign = _IssueContext.ISM_TaskCreation_AssignedToDMO.Single(a => a.ISMTCR_Id == ta.ISMTCR_Id);
                                    var oldassigndate = updateassign.ISMTCRASTO_AssignedDate.ToString("dd/MM/yyyy");
                                    oldassiemployee = updateassign.HRME_Id;
                                    var oldassignemployee = ((from a in _IssueContext.MasterEmployee
                                                              where (a.HRME_Id == updateassign.HRME_Id)
                                                              select new ISM_ComplaintsTaskListDTO
                                                              {
                                                                  HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName)
                                                              }).FirstOrDefault()).HRME_EmployeeFirstName;

                                    updateassign.ISMTCRASTO_AssignedBy = empid;
                                    updateassign.ISMTCRASTO_ActiveFlg = true;
                                    updateassign.HRME_Id = id.HRME_Id;
                                    updateassign.ISMTCRASTO_Remarks = ta.ISMTCRTRTO_Remarks;
                                    updateassign.ISMTCRASTO_StartDate = ta.ISMTCRTRTO_StartDate;
                                    updateassign.ISMTCRASTO_EndDate = ta.ISMTCRTRTO_EndDate;
                                    updateassign.ISMTCRASTO_EffortInHrs = ta.ISMTCRTRTO_EffortInHrs;
                                    updateassign.UpdatedDate = indianTime;
                                    updateassign.ISMTCRASTO_AssignedDate = indianTime;
                                    _IssueContext.Update(updateassign);

                                    ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();
                                    taskresponse.ISMTCR_Id = ta.ISMTCR_Id;
                                    taskresponse.HRME_Id = empid;
                                    taskresponse.ISMTCRRES_ResponseDate = indianTime;
                                    taskresponse.ISMTCRRES_Response = "Previously Assigned To " + oldassignemployee + " On Date: " + oldassigndate + ". \nTransfer Remarks: " + ta.ISMTCRTRTO_Remarks;
                                    taskresponse.ISMTCRRES_ResponseAttachment = "";
                                    taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                                    taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                                    taskresponse.ISMTCRRES_Status = "Open";
                                    taskresponse.ISMTCRRES_ResponsePageName = "Transfer Page";
                                    taskresponse.ISMTCRRES_ActiveFlg = true;
                                    taskresponse.CreatedDate = indianTime;
                                    taskresponse.UpdatedDate = indianTime;
                                    _IssueContext.Add(taskresponse);
                                }
                            }
                            else
                            {
                                data.message = "Planner";
                                return data;
                            }
                        }
                    }
                }

                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                    if (data.transfer == true)
                    {
                        try
                        {
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_Transfer_Update_Planner";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@Hrme_Id",
                         SqlDbType.BigInt)
                                {
                                    Value = oldassiemployee
                                });
                                cmd.Parameters.Add(new SqlParameter("@ISMTCR_ID",
                        SqlDbType.BigInt)
                                {
                                    Value = data.ISMTCR_Id
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
                                    data.transfer_plannerUpdate = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    Savetransfernotification(data);
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
        public void Savetransfernotification(ISM_ComplaintsTaskListDTO data)
        {
            PushNotification pb = new PushNotification(_Context);
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
            long empid = 0;
            if (hrmEempid.Length > 0)
            {
                empid = hrmEempid.FirstOrDefault().Emp_Code;
            }

            long taskid = 0;
            List<long> transferto = new List<long>();
            foreach (var at in data.transferArray)
            {

                taskid = at.ISMTCR_Id;

                if (data.hrme_list != null)
                {
                    foreach (var item in data.hrme_list)
                    {
                        transferto.Add(item.HRME_Id);

                        pb.savewebnotificationasy(data.MI_Id, "NPTRANSFER", at.ISMTCR_Id, at.ISMTCR_Id, item.HRME_Id, data.UserId, "Transferred");
                    }
                }

            }
            var deviceidsnew = "";
            var devicenew = "";
            var devicelist = (from a in _IssueContext.MasterEmployee
                              where (a.HRME_ActiveFlag == true && transferto.Contains(a.HRME_Id))
                              select new ISM_ComplaintsTaskListDTO
                              {
                                  HRME_To = a.HRME_Id,
                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                              }).Distinct().ToList();
            if (devicelist.Count > 0)
            {
                data.HRME_To = devicelist.FirstOrDefault().HRME_To;
            }

            data.deviceArray = devicelist.ToArray();

            if (devicelist.Count > 0)
            {
                int k = 0;
                foreach (var deviceid in devicelist)
                {
                    if (k == 0)
                    {
                        deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                        k = 1;
                    }
                    else
                    {
                        deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                    }
                }
                devicenew = "[" + deviceidsnew + "]";
            }
            var pushemployee = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_Id == taskid).ToList();

            try
            {
                string subject = "New Task Transferred";
                string titletext = pushemployee.FirstOrDefault().ISMTCR_TaskNo + " - " + pushemployee.FirstOrDefault().ISMTCR_Title;
                string tasktype = "Transferred";
                if (data.sound == "")
                {
                    data.sound = "default";
                }
                callnotificationasy(devicenew, data.HRME_To, taskid, titletext, data.MI_Id, data.sound, subject);
                Insertnotificationasy(data.MI_Id, data.HRME_To, taskid, titletext, tasktype, data.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {

                if (pushemployee.Count > 0)
                {
                    if (data.HRME_To != empid)
                    {
                        var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                             from a in _IssueContext.Staff_User_Login
                                             where (e.ISMPNU_Userid == a.Id && a.Emp_Code == data.HRME_To)
                                             select new ISM_ComplaintsTaskListDTO
                                             {
                                                 UserId = e.ISMPNU_Userid
                                             }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                        if (webpushtocken.Count > 0)
                        {
                            foreach (var webtockenid in webpushtocken)
                            {
                                PushNotification ObjPushNotification = new PushNotification(_Context);
                                ObjPushNotification.callpushnotificationasy(pushemployee.FirstOrDefault().ISMTCR_Title, "New Task Transffered!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + pushemployee.FirstOrDefault().ISMTCR_TaskNo + "&taskid=" + pushemployee.FirstOrDefault().ISMTCR_Id, webtockenid.UserId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        public ISM_ComplaintsTaskListDTO saveResponse(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                Array checkupdate = new Array[10];
                Array plannertaskcheckupdate = new Array[10];
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Task_Update_Check";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@taskid",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTCR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
            SqlDbType.BigInt)
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        checkupdate = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (checkupdate.Length == 0)
                {
                    PushNotification pb = new PushNotification(_Context);
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                    long empid = 0;
                    if (hrmEempid.Length > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                    }
                    if (data.attachmentArray != null)
                    {
                        foreach (var taskatt in data.attachmentArray)
                        {
                            ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();
                            taskresponse.ISMTCR_Id = data.ISMTCR_Id;
                            taskresponse.HRME_Id = empid;
                            taskresponse.ISMTCRRES_ResponseDate = indianTime;
                            taskresponse.ISMTCRRES_Response = data.ISMTCRRES_Response;
                            taskresponse.ISMTCRRES_ResponseAttachment = taskatt;
                            taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                            taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                            taskresponse.ISMTCRRES_Status = data.ISMMISTS_StatusName;
                            taskresponse.ISMTCRRES_ActiveFlg = true;
                            taskresponse.ISMTCRRES_ResponsePageName = "Complaints/Task List";
                            taskresponse.CreatedDate = indianTime;
                            taskresponse.UpdatedDate = indianTime;
                            _IssueContext.Add(taskresponse);
                        }
                    }
                    else
                    {
                        ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();
                        taskresponse.ISMTCR_Id = data.ISMTCR_Id;
                        taskresponse.HRME_Id = empid;
                        taskresponse.ISMTCRRES_ResponseDate = indianTime;
                        taskresponse.ISMTCRRES_Response = data.ISMTCRRES_Response;
                        taskresponse.ISMTCRRES_ResponseAttachment = "";
                        taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                        taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                        taskresponse.ISMTCRRES_Status = data.ISMMISTS_StatusName;
                        taskresponse.ISMTCRRES_ResponsePageName = "Complaints/Task List";
                        taskresponse.ISMTCRRES_ActiveFlg = true;
                        taskresponse.CreatedDate = indianTime;
                        taskresponse.UpdatedDate = indianTime;
                        _IssueContext.Add(taskresponse);
                    }
                    //***************************************UPDATE TASK CREATION STATUS***********************************************//
                    var taskesult = _IssueContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == data.ISMTCR_Id);
                    taskesult.ISMTCR_Status = data.ISMMISTS_StatusName;
                    if (data.ISMMISTS_StatusName.Equals("ReOpen", StringComparison.OrdinalIgnoreCase))
                    {
                        taskesult.ISMTCR_ReOpenFlg = true;
                        taskesult.ISMTCR_ReOpenDate = indianTime;
                    }
                    taskesult.UpdatedDate = indianTime;
                    taskesult.ISMTCR_Desc = data.taskdescription;
                    taskesult.ISMTCR_UpdatedBy = data.UserId;
                    _IssueContext.Update(taskesult);

                    var contactExists = _IssueContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        Responsenotification(data);
                    }
                    else
                    {
                        data.returnval = false;
                    }
                    //***************************************UPDATE PLANNER TASK STATUS***********************************************//
                    try
                    {
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Planner_Task_Status_Update";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@taskid",
                    SqlDbType.BigInt)
                            {
                                Value = data.ISMTCR_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@updatedby",
                    SqlDbType.BigInt)
                            {
                                Value = data.UserId
                            });
                            cmd.Parameters.Add(new SqlParameter("@status",
                    SqlDbType.VarChar)
                            {
                                Value = data.ISMMISTS_StatusName
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
                                plannertaskcheckupdate = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    if (data.assigntaskeditable == true)
                    {
                        var Assignedto = _IssueContext.ISM_TaskCreation_AssignedToDMO.Single(a => a.ISMTCR_Id == data.ISMTCR_Id);
                        Assignedto.UpdatedDate = indianTime;
                        Assignedto.ISMTCRASTO_TimeRequiredFlg = data.durationFlg;
                        Assignedto.ISMTCRASTO_EffortInHrs = data.eachTaskMaxDuration;
                        _IssueContext.Update(Assignedto);
                        var AssignedToupdate = _IssueContext.SaveChanges();
                    }
                }
                else
                {
                    data.returnduplicatestatus = "Notupdate";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_ComplaintsTaskListDTO saveMultiResponse(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                if (data.savemultipletaskupdatearray != null && data.savemultipletaskupdatearray.Count() > 0)
                {
                    foreach (var task in data.savemultipletaskupdatearray)
                    {
                        Array checkupdate = new Array[10];
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Task_Update_Check";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@taskid",
                    SqlDbType.BigInt)
                            {
                                Value = task.ISMTCR_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@userid",
                    SqlDbType.BigInt)
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
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                checkupdate = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (checkupdate.Length == 0)
                        {
                            PushNotification pb = new PushNotification(_Context);
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                            var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                            long empid = 0;
                            if (hrmEempid.Length > 0)
                            {
                                empid = hrmEempid.FirstOrDefault().Emp_Code;
                            }

                            ISM_TaskCreation_ResponseDMO taskresponse = new ISM_TaskCreation_ResponseDMO();
                            taskresponse.ISMTCR_Id = task.ISMTCR_Id;
                            taskresponse.HRME_Id = empid;
                            taskresponse.ISMTCRRES_ResponseDate = indianTime;
                            taskresponse.ISMTCRRES_Response = task.ISMTCRRES_Response;
                            taskresponse.ISMTCRRES_ResponseAttachment = "";
                            taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                            taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                            taskresponse.ISMTCRRES_Status = task.ISMMISTS_StatusName;
                            taskresponse.ISMTCRRES_ResponsePageName = "Complaints/Task List";
                            taskresponse.ISMTCRRES_ActiveFlg = true;
                            taskresponse.CreatedDate = indianTime;
                            taskresponse.UpdatedDate = indianTime;
                            _IssueContext.Add(taskresponse);

                            //***************************************UPDATE TASK CREATION STATUS***********************************************//
                            var taskesult = _IssueContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == task.ISMTCR_Id);
                            taskesult.ISMTCR_Status = task.ISMMISTS_StatusName;
                            if (task.ISMMISTS_StatusName.Equals("ReOpen", StringComparison.OrdinalIgnoreCase))
                            {
                                taskesult.ISMTCR_ReOpenFlg = true;
                                taskesult.ISMTCR_ReOpenDate = indianTime;
                            }
                            taskesult.UpdatedDate = indianTime;
                            taskesult.ISMTCR_UpdatedBy = data.UserId;
                            _IssueContext.Update(taskesult);

                            var contactExists = _IssueContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;
                                try
                                {
                                    Responsenotification(data);
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                            }
                            else
                            {
                                data.returnval = false;
                            }
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
        public void Responsenotification(ISM_ComplaintsTaskListDTO data)
        {
            PushNotification pb = new PushNotification(_Context);
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
            long empid = 0;
            if (hrmEempid.Length > 0)
            {
                empid = hrmEempid.FirstOrDefault().Emp_Code;
            }

            var resplist = _IssueContext.ISM_TaskCreation_ResponseDMO.Where(e => e.ISMTCR_Id == data.ISMTCR_Id && e.ISMTCRRES_CreatedBy == data.UserId && e.ISMTCRRES_Response == data.ISMTCRRES_Response).Distinct().OrderByDescending(r => r.CreatedDate).Take(1).ToList();

            if (resplist.Count > 0)
            {
                var responseto1 = _IssueContext.ISM_TaskCreationDMO.Where(r => r.ISMTCR_Id == data.ISMTCR_Id && r.ISMTCR_ActiveFlg == true).Distinct().ToList();
                long response_id1 = 0;
                if (responseto1.Count > 0)
                {
                    response_id1 = responseto1.FirstOrDefault().HRME_Id;
                }
                if (empid == response_id1)
                {
                    var hrmeidlist = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO

                                      where b.ISMTCR_Id == data.ISMTCR_Id
                                      select new ISM_ComplaintsTaskListDTO
                                      {
                                          HRME_Id = b.HRME_Id,
                                      }).Distinct().ToList();
                    if (hrmeidlist.Count > 0)
                    {
                        pb.savewebnotificationasy(data.MI_Id, "NPRESPONSE", resplist.FirstOrDefault().ISMTCRRES_Id, data.ISMTCR_Id, hrmeidlist.FirstOrDefault().HRME_Id, data.UserId, "Response");
                    }
                }
                else
                {
                    pb.savewebnotificationasy(data.MI_Id, "NPRESPONSE", resplist.FirstOrDefault().ISMTCRRES_Id, data.ISMTCR_Id, response_id1, data.UserId, "Response");
                }

            }

            data.returnval = true;

            // ============================================== Push Notification Response 
            var responseto = _IssueContext.ISM_TaskCreationDMO.Where(r => r.ISMTCR_Id == data.ISMTCR_Id && r.ISMTCR_ActiveFlg == true).Distinct().ToList();
            long response_id = 0;
            if (responseto.Count > 0)
            {
                response_id = responseto.FirstOrDefault().HRME_Id;
            }

            var deviceidsnew = "";
            var devicenew = "";
            var devicelist = (from a in _IssueContext.MasterEmployee
                              where (a.HRME_ActiveFlag == true && a.HRME_Id == response_id)
                              select new ISM_DailyReportGenerationDTO
                              {
                                  HRME_Id = a.HRME_Id,
                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                              }).Distinct().ToList();
            if (devicelist.Count > 0)
            {
                data.HRME_To = devicelist.FirstOrDefault().HRME_To;
                data.deviceArray = devicelist.ToArray();
            }

            if (devicelist.Count > 0)
            {
                int k = 0;
                foreach (var deviceid in devicelist)
                {
                    if (k == 0)
                    {
                        deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                        k = 1;
                    }
                    else
                    {
                        deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                    }
                }
                devicenew = "[" + deviceidsnew + "]";
            }
            var pushemployee = _IssueContext.ISM_TaskCreationDMO.Where(t => t.ISMTCR_Id == data.ISMTCR_Id).ToList();
            var assigned = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(t => t.ISMTCR_Id == data.ISMTCR_Id).ToList();
            if (pushemployee.Count > 0)
            {
                if (pushemployee.FirstOrDefault().HRME_Id != empid)
                {
                    try
                    {
                        string subject = "Task Response :";
                        string titletext = pushemployee.FirstOrDefault().ISMTCR_TaskNo + "\n" + pushemployee.FirstOrDefault().ISMTCR_Title + "\n Response:" + data.ISMTCRRES_Response;
                        string tasktype = "Response";
                        if (data.sound == "")
                        {
                            data.sound = "default";
                        }
                        callnotificationasy(devicenew, response_id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                        Insertnotificationasy(data.MI_Id, response_id, data.ISMTCR_Id, titletext, tasktype, data.UserId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            var devicelistto = (from b in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                from e in _IssueContext.MasterEmployee
                                where (b.ISMTCR_Id == data.ISMTCR_Id && b.ISMTCRASTO_AssignedBy == e.HRME_Id
                             && e.HRME_AppDownloadedDeviceId != null && e.HRME_AppDownloadedDeviceId != "")
                                select new ISM_ComplaintsTaskListDTO
                                {
                                    HRME_Id = b.HRME_Id,
                                    HRME_AppDownloadedDeviceId = e.HRME_AppDownloadedDeviceId
                                }).Distinct().OrderBy(t => t.HRME_Id).ToList();

            if (devicelistto.Count > 0)
            {
                foreach (var deviceid in devicelistto)
                {
                    if (assigned.FirstOrDefault().ISMTCRASTO_AssignedBy != deviceid.HRME_Id && pushemployee.FirstOrDefault().HRME_Id != deviceid.HRME_Id)
                    {
                        deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';

                        devicenew = "[" + deviceidsnew + "]";

                        try
                        {
                            string subject = "Task Response :";
                            string titletext = pushemployee.FirstOrDefault().ISMTCR_TaskNo + "\n" + pushemployee.FirstOrDefault().ISMTCR_Title + "\nResponse:" + data.ISMTCRRES_Response;
                            string tasktype = "Response";
                            if (data.sound == "")
                            {
                                data.sound = "default";
                            }
                            callnotificationasy(devicenew, deviceid.HRME_Id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound, subject);
                            Insertnotificationasy(data.MI_Id, deviceid.HRME_Id, data.ISMTCR_Id, titletext, tasktype, data.UserId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            try
            {
                if (pushemployee.Count > 0)
                {

                    if (pushemployee.FirstOrDefault().HRME_Id != empid)
                    {
                        var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                             from a in _IssueContext.Staff_User_Login
                                             where (e.ISMPNU_Userid == a.Id && a.Emp_Code == pushemployee.FirstOrDefault().HRME_Id)
                                             select new ISM_ComplaintsTaskListDTO
                                             {
                                                 UserId = e.ISMPNU_Userid
                                             }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                        if (webpushtocken.Count > 0)
                        {
                            foreach (var webtockenid in webpushtocken)
                            {
                                PushNotification ObjPushNotification = new PushNotification(_Context);
                                ObjPushNotification.callpushnotificationasy(pushemployee.FirstOrDefault().ISMTCR_TaskNo + "- " + pushemployee.FirstOrDefault().ISMTCR_Title + "- Response:" + data.ISMTCRRES_Response, "Task Response!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + pushemployee.FirstOrDefault().ISMTCR_TaskNo + "&taskid=" + pushemployee.FirstOrDefault().ISMTCR_Id, webtockenid.UserId);
                            }
                        }
                    }
                    if (assigned.FirstOrDefault().ISMTCRASTO_AssignedBy != empid && pushemployee.FirstOrDefault().HRME_Id != assigned.FirstOrDefault().ISMTCRASTO_AssignedBy)
                    {
                        var webpushtocken = (from e in _IssueContext.ISM_PUSH_NOTIFICATION_USERDMO
                                             from a in _IssueContext.Staff_User_Login
                                             where (e.ISMPNU_Userid == a.Id && a.Emp_Code == assigned.FirstOrDefault().ISMTCRASTO_AssignedBy)
                                             select new ISM_ComplaintsTaskListDTO
                                             {
                                                 UserId = e.ISMPNU_Userid
                                             }).Distinct().OrderBy(t => t.HRME_Id).ToList();
                        if (webpushtocken.Count > 0)
                        {
                            foreach (var webtockenid in webpushtocken)
                            {
                                PushNotification ObjPushNotification = new PushNotification(_Context);
                                ObjPushNotification.callpushnotificationasy(pushemployee.FirstOrDefault().ISMTCR_TaskNo + "- " + pushemployee.FirstOrDefault().ISMTCR_Title + "- Response:" + data.ISMTCRRES_Response, "Task Response!!", "http://localhost:57606/#/app/ISM_ComplaintsTaskList/729?taskno=" + pushemployee.FirstOrDefault().ISMTCR_TaskNo + "&taskid=" + pushemployee.FirstOrDefault().ISMTCR_Id, webtockenid.UserId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            //return  data;
        }
        public async Task callnotificationasy(string devicenew, long empid, long task_id, string titletext, long mi_id, string sound, string subject)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";
                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
           "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + subject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + titletext + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";
                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public async Task Insertnotificationasy(long MI_Id, long empid, long task_id, string titletext, string tasktype, long userid)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ISM_PlannerCreationDTO data = new ISM_PlannerCreationDTO();
                ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                push.MI_Id = MI_Id;
                push.HRME_Id = empid;
                push.ISMNO_Notification = titletext;
                push.ISMNO_NoticationDate = indianTime;
                push.ISMNO_NotificationType = tasktype;
                push.ISMNO_ReadFlg = false;
                push.ISMNO_MakeUnReadFlg = false;
                push.ISMNO_ActiveFlag = true;
                push.ISMNO_CreatedBy = userid;
                push.ISMNO_UpdatedBy = userid;
                push.CreatedDate = indianTime;
                push.UpdatedDate = indianTime;
                _IssueContext.Add(push);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public ISM_ComplaintsTaskListDTO deactive(ISM_ComplaintsTaskListDTO data)
        {
            try
            {

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_ComplaintsTaskListDTO gettaskAttachment(ISM_ComplaintsTaskListDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_ComplaintsTaskListDTO GetTaskSearchDetails(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_Task_Global_Search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TaskNo", SqlDbType.VarChar)
                    {
                        Value = data.ISMTCR_TaskNo
                    });

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar)
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_globalsearchtask = retObject.ToArray();
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
        //
        public ISM_ComplaintsTaskListDTO SaveApprovaldata(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.savegroup != null && data.savegroup.Length > 0)
                {
                    foreach (var te in data.savegroup)
                    {
                        if (te.typeFlg == "R")
                        {

                            ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                            taskasign.ISMTCR_Id = te.ISMTCR_Id;
                            taskasign.HRME_Id = te.HRME_Id;
                            taskasign.ISMTCRASTO_AssignedDate = indianTime;
                            taskasign.ISMTCRASTO_AssignedBy = empid;
                            taskasign.ISMTCRASTO_Remarks = te.ISMTCRTRTO_Remarks;
                            taskasign.ISMTCRASTO_StartDate = DateTime.Now;
                            taskasign.ISMTCRASTO_EndDate = DateTime.Now;
                            taskasign.ISMTCRASTO_EffortInHrs = te.ISMTCRASTO_EffortInHrs;
                            taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                            taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                            taskasign.ISMTCRASTO_ActiveFlg = true;
                            taskasign.CreatedDate = indianTime;
                            taskasign.UpdatedDate = indianTime;
                            _IssueContext.Add(taskasign);

                        }
                        if (te.HRMPR_Id > 0 && te.ISMMAC_Id > 0)
                        {
                            var checktasktransferresult = _IssueContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == te.ISMTCR_Id);
                            checktasktransferresult.HRMPR_Id = Convert.ToInt64(te.HRMPR_Id);
                            checktasktransferresult.UpdatedDate = indianTime;
                            checktasktransferresult.ISMTCR_VerifiedBy = empid;
                            checktasktransferresult.ISMTCR_VerifierRemarks = te.ISMTCRTRTO_Remarks;
                            _IssueContext.Update(checktasktransferresult);

                            if (te.HRMPR_Id != te.ISMMAC_Id)
                            {
                                ISM_TaskCreation_PriorityHistoryDMO priority = new ISM_TaskCreation_PriorityHistoryDMO();
                                priority.ISMTCR_Id = te.ISMTCR_Id;
                                priority.HRMPR_Id = te.ISMMAC_Id;
                                priority.ISMTCRPH_Remarks = te.ISMTCRTRTO_Remarks;
                                priority.ISMTCRPH_ActiveFlg = true;
                                priority.ISMTCRPH_CreatedDate = DateTime.Now;
                                priority.ISMTCRPH_UpdatedDate = DateTime.Now;
                                priority.ISMTCRPH_CreatedBy = data.UserId;
                                priority.ISMTCRPH_UpdatedBy = data.UserId;
                                _IssueContext.Add(priority);
                            }
                        }

                    }
                    int i = _IssueContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "save";
                    }
                    else
                    {
                        data.message = "Notsave";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }
        public string sendnotifymail(long ISMTCR_Id, long empid, long UserId, long MI_Id, long ToHRME_Id, string Tomail)
        {
            string message = "";
            try
            {


                var getresponseid = _IssueContext.ISM_TaskCreation_ResponseDMO.Where(e => e.ISMTCR_Id == ISMTCR_Id && e.HRME_Id == empid && e.ISMTCRRES_CreatedBy == UserId).OrderByDescending(e => e.ISMTCRRES_Id).Distinct().Take(1).ToList();
                List<EmailDeviationDTO> Tasklist = new List<EmailDeviationDTO>();

                var checktemplate = _IssueContext.smsEmailSetting.Where(a => a.MI_Id == MI_Id && a.ISES_Template_Name == "TASKRESPONSETEMPL"
                                && a.ISES_MailActiveFlag == true).ToList();

                if (checktemplate.Count == 0)
                {
                    message = "Email Template not Mapped to the selected Institution";
                }
                else
                {

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_RESPONSE_NOTIFICATION";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                 SqlDbType.BigInt)
                        {
                            Value = empid
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
                 SqlDbType.BigInt)
                        {
                            Value = ISMTCR_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@UserId",
                 SqlDbType.BigInt)
                        {
                            Value = UserId
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
                                    Tasklist.Add(new EmailDeviationDTO
                                    {
                                        MI_Name = Convert.ToString(dataReader["MI_Name"]),
                                        ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                        ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                        ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                        ISMTPLTA_Status = Convert.ToString(dataReader["ISMTCRRES_Status"]),
                                        responseby = Convert.ToString(dataReader["EmpName"]),
                                        ISMTCR_CreationDate = Convert.ToDateTime(dataReader["ISMTCR_CreationDate"]),
                                        ISMTCRRES_Response = Convert.ToString(dataReader["ISMTCRRES_Response"]),
                                        ISMTCRRES_ResponseDate = Convert.ToDateTime(dataReader["ISMTCRRES_ResponseDate"])

                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    string stra = "";


                    if (Tasklist != null && Tasklist.Count > 0)
                    {
                        int rownumber = 0;
                        stra = stra + "<table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Company Name</th>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Task No.</th>" +
                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Tittle </th>" +
                                    "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Description </th>" +
                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Created Date </th>" +
                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Response By </th>"
                                     +
                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Response</th>" +
                                      "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Response Date</th>" +

                                     "</tr>";
                        for (int j = 0; j < Tasklist.Count; j++)
                        {
                            rownumber += 1;
                            stra = stra + "<tr> " +
                                    "<td style=" + "'border: 1px solid black; text-align:center;font-size:13px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-weight:bold;font-family: sans-serif;'" + " >" + Tasklist[j].MI_Name + "</td>" +
                                    "<td align=" + "left " + "style=" + "'font-size:13px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + Tasklist[j].ISMTCR_TaskNo + "</td>" +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_Title + "</td> " +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_Desc + "</td> " +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_CreationDate.ToString("dd/MM/yyyy") + "</td> " +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].responseby + "</td> " +
                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCRRES_Response + "</td> " +
                                     "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCRRES_ResponseDate.ToString("dd/MM/yyyy") + "</td> " +
                                     "</tr>";
                        }
                        stra = stra + "</table>";



                        string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;


                        templatedetails = templatedetails.Replace("[MSG]", stra);

                        try
                        {

                            sendmailnotassign(MI_Id, "TASKRESPONSETEMPL", Tomail, templatedetails, ISMTCR_Id, empid, ToHRME_Id);


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

                throw ex;
            }

            return message;
        }
        //EMAIL
        #region   email
        public string sendmailnotassign(long MI_Id, string Template, string emailid, string msgg, long ISMTCR_Id, long empid, long ToHRME_Id)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _Context.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                Mailmsg = msgg;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TASKRESPONSE_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMTCR_Id",
                           SqlDbType.BigInt)
                        {
                            Value = ISMTCR_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToHRME_Id",
                           SqlDbType.BigInt)
                        {
                            Value = ToHRME_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

                //Mailmsg = msgg;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;


                        message.AddTo(emailid);

                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }
                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {

                                emailMessage.To.Add(new MailAddress(emailid));



                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        //cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = "Inventory"
                        //});

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion

        #region DuplicateTaskClose
        public async Task<ISM_ComplaintsTaskListDTO> getDuplicateTasksList(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    data.HRME_Id = empid;
                }

                var hrmeDepartment_head = _IssueContext.HR_Master_DepartmentCode_Head.Where(t => t.HRME_ID == data.HRME_Id).ToList();
                if (hrmeDepartment_head.Count > 0)
                {
                    data.roletype = "Head";
                }

                //get_DuplicateTaskList
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Employee_Open_Task_Report";
                    // cmd.CommandText = "ISM_Pending_AssignedBy_To_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
            SqlDbType.VarChar)
                    {
                        Value =empid //data.UserId
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
                        data.get_comptasklist = retObject.ToArray();
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
        
        public ISM_ComplaintsTaskListDTO saveDuplicateTask(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var duplicate = _IssueContext.ISM_DuplicateTask_Save_RemarksDMO.Where(t => t.ISMTCR_Id == data.ISMTCR_Id).Distinct().ToArray();
                
                if (duplicate.Length > 0)
                {
                    data.message = "Duplicate";
                }
                else
                {
                    for (int i = 0; i < data.plannerArray.Length; i++)
                    {
                        var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("ISM_DupicateTask_Save_Remarks_details @p0,@p1",
                            data.plannerArray[i].ISMTCR_Id,
                            data.plannerArray[i].ISMTCRASTO_Remarks);
                        if (contactExistsP > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "Admin";
            }
            return data;
        }

        public async Task<ISM_ComplaintsTaskListDTO> getDuplicateApprovalTasksList(ISM_ComplaintsTaskListDTO data)
        {
            try
            {

                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                    data.HRME_Id = empid;
                }

                var hrmeDepartment_head = _IssueContext.HR_Master_DepartmentCode_Head.Where(t => t.HRME_ID == data.HRME_Id).ToList();
                if (hrmeDepartment_head.Count > 0)
                {
                    data.roletype = "Head";
                }

                //get_DuplicateTaskList
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Employee_Open_Approval_Task_Report";
                    // cmd.CommandText = "ISM_Pending_AssignedBy_To_Details";
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
                        data.get_comptasklist = retObject.ToArray();
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
        //assigntask_ids             
        public ISM_ComplaintsTaskListDTO approveOrRejectDuplicateTask(ISM_ComplaintsTaskListDTO data)
        {
            try
            {
                var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("ISM_DuplicateTask_Approve @p0,@p1,@p2", data.ISMTCR_Ids, data.flag == "Close" ? 0 : 1, data.UserId);
                if (contactExistsP > 0)
                {
                    data.returnval = true;
                    data.message = "Add";
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "Admin";
            }
            return data;
        }
        #endregion
    }
}
