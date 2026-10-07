using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
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
    public class ISM_NoticeIssueImpl : Interfaces.ISM_NoticeIssueInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_NoticeIssueImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_PlannerCreationDTO> getdetails(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                List<ISM_PlannerCreationDTO> dtonewtemp1 = new List<ISM_PlannerCreationDTO>();
                //*********************************** Month List ********************************// 
                data.monthlist = _IssueContext.IVRM_Month_DMO.Where(m => m.Is_Active == true).ToArray();
                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList";
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
                                dtonewtemp1.Add(new ISM_PlannerCreationDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"])

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                var hrmEempidnew = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empidnew = 0;
                if (hrmEempidnew.Length > 0)
                {
                    empidnew = hrmEempidnew.FirstOrDefault().Emp_Code;
                }
                var roletn = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletypen = "";
                if (roletn.Count > 0)
                {
                    roletypen = roletn.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletypen = "";
                }
                string hrmeids = "0";
                if (dtonewtemp1.Count() > 0)
                {
                    foreach (var ue in dtonewtemp1)
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

                //**************************************** Pending  Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Peding_Memo_List";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        data.get_Memolist = retObject.ToArray();
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

        public async Task<ISM_PlannerCreationDTO> getplannerlist(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                List<ISM_PlannerCreationDTO> dtonewtemp1 = new List<ISM_PlannerCreationDTO>();
                //*********************************** Month List ********************************// 
                data.monthlist = _IssueContext.IVRM_Month_DMO.Where(m => m.Is_Active == true).ToArray();
                //*********************************** User Employee Mapped List ********************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_User_EmployeeList";
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
                                dtonewtemp1.Add(new ISM_PlannerCreationDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"])

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                var hrmEempidnew = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empidnew = 0;
                if (hrmEempidnew.Length > 0)
                {
                    empidnew = hrmEempidnew.FirstOrDefault().Emp_Code;
                }
                var roletn = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletypen = "";
                if (roletn.Count > 0)
                {
                    roletypen = roletn.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletypen = "";
                }
                string hrmeids = "0";
                if (dtonewtemp1.Count() > 0)
                {
                    foreach (var ue in dtonewtemp1)
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

                //**************************************** Pending  Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Peding_Planner_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
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
                        data.get_plannerlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //**************************************** Max Date Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_MaxDate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
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
                        data.get_plannermax = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //**************************************** Approved Planner *************************//
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Approved_Planner_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
               SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });

                    //       cmd.Parameters.Add(new SqlParameter("@Month_Id",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = data.IVRM_Month_Id
                    //       });

                    cmd.Parameters.Add(new SqlParameter("@roletype",
            SqlDbType.VarChar)
                    {
                        Value = roletypen
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
                        data.get_updatedplannerlist = retObject.ToArray();
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
        public async Task<ISM_PlannerCreationDTO> getplannerdetails(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;

                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_TaskDeatils";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
          SqlDbType.VarChar)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
            SqlDbType.VarChar)
                    {
                        Value = data.ISMTPL_Id
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
                        data.get_plannerdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_Category";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTPL_Id
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
                        data.get_plannercategory = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_ApprovalStatus_Deatils";
                    cmd.CommandType = CommandType.StoredProcedure;

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

                    cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ISMTPL_Id
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
                        data.get_plannerApprovaldetails = retObject.ToArray();
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
        public async Task<ISM_PlannerCreationDTO> getextratask(ISM_PlannerCreationDTO data)
        {
            try
            {
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                }
                else
                {
                    roletype = "";
                }
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PlannerApproval_ExtraTask";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
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
                        data.get_extratasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //     using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //     {
                //         cmd.CommandText = "ISM_Pending_AssignedBy_To_Details";
                //         cmd.CommandType = CommandType.StoredProcedure;

                //         cmd.Parameters.Add(new SqlParameter("@RoleFlg",
                //SqlDbType.VarChar)
                //         {
                //             Value = rolet.FirstOrDefault().IVRMRT_Role
                //         });
                //         cmd.Parameters.Add(new SqlParameter("@countflag",
                //SqlDbType.VarChar)
                //         {
                //             Value = "NotAssign"
                //         });

                //         cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                // SqlDbType.VarChar)
                //         {
                //             Value = empid
                //         });
                //         cmd.Parameters.Add(new SqlParameter("@userid",
                // SqlDbType.VarChar)
                //         {
                //             Value = data.UserId
                //         });

                //         if (cmd.Connection.State != ConnectionState.Open)
                //             cmd.Connection.Open();

                //         var retObject = new List<dynamic>();
                //         try
                //         {
                //             using (var dataReader = await cmd.ExecuteReaderAsync())
                //             {
                //                 while (await dataReader.ReadAsync())
                //                 {
                //                     var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                     for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                     {
                //                         dataRow.Add(
                //                             dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                         );
                //                     }
                //                     retObject.Add((ExpandoObject)dataRow);
                //                 }
                //             }
                //             data.get_extratasklist = retObject.ToArray();
                //         }
                //         catch (Exception ex)
                //         {
                //             Console.WriteLine(ex.Message);
                //         }
                //     }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data)
        {
            try
            {

                decimal totalhrs = 0;
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                if (data.addExtraTaskArray != null)
                {
                    foreach (var pa in data.addExtraTaskArray)
                    {
                        pa.ISMTPLTA_ExtraFlg = true;
                        totalhrs += pa.ISMTPLTA_EffortInHrs;
                        var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == pa.ISMTCR_Id
                          && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();
                        if (updateplannertask.Count > 0)
                        {
                            var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == pa.ISMTCR_Id
                            && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true);

                            result.ISMTPLTA_Status = pa.ISMTPLTA_Status;
                            result.ISMTPLTA_Remarks = pa.ISMTPLTA_Remarks;
                            result.UpdatedDate = DateTime.UtcNow;
                            result.ISMTPLTA_UpdatedBy = data.UserId;
                            _IssueContext.Update(result);
                        }
                        else
                        {
                            ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();
                            dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                            dmo_task.ISMTCR_Id = pa.ISMTCR_Id;
                            dmo_task.ISMTPLTA_Remarks = pa.ISMTPLTA_Remarks;
                            dmo_task.ISMTPLTA_StartDate = pa.ISMTPLTA_StartDate;
                            dmo_task.ISMTPLTA_EndDate = pa.ISMTPLTA_EndDate;
                            dmo_task.ISMTPLTA_EffortInHrs = pa.ISMTPLTA_EffortInHrs;
                            dmo_task.ISMTPLTA_Status = "Open";
                            dmo_task.ISMTPLTA_DeviationFlg = false;
                            dmo_task.ISMTPLTA_ExtraFlg = pa.ISMTPLTA_ExtraFlg;
                            dmo_task.ISMTPLTA_TimeTakenInHrs = pa.ISMTPLTA_TimeTakenInHrs;
                            dmo_task.ISMTPLTA_ActiveFlg = true;
                            dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                            dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                            dmo_task.CreatedDate = DateTime.UtcNow;
                            dmo_task.UpdatedDate = DateTime.UtcNow;
                            _IssueContext.Add(dmo_task);
                        }
                    }

                    var plannerresultlist = _IssueContext.ISM_Task_PlannerDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMTPL_Id == data.ISMTPL_Id).ToList();
                    decimal totalhrslist = plannerresultlist.FirstOrDefault().ISMTPL_TotalHrs;
                    totalhrslist = totalhrslist + totalhrs;

                    var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.MI_Id == data.MI_Id && a.ISMTPL_Id == data.ISMTPL_Id);
                    plannerresult.ISMTPL_TotalHrs = totalhrslist;
                    _IssueContext.Update(plannerresult);
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_PlannerCreationDTO updateplannerStatus(ISM_PlannerCreationDTO data)
        {
            long empid = 0;
            long plannedby_id = 0;
            long counttrue = 0;
            long approvedCount = 0;
            long rejectedCount = 0;
            decimal finaleffort = 0;
            try
            {
                if (data.ISMTPLAP_Id > 0)
                {
                    var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                    if (hrmEempid.Length > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                    }

                    var planned_id = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.ISMTPL_Id == data.ISMTPL_Id).Distinct().ToArray();
                    if (planned_id.Length > 0)
                    {
                        plannedby_id = planned_id.FirstOrDefault().HRME_Id;
                    }

                    var dmoresult = _IssueContext.ISM_Task_Planner_ApprovedDMO.Single(a => a.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPLAP_ActiveFlg == true);

                    dmoresult.HRME_Id = empid;
                    dmoresult.ISMTPLAP_Remarks = data.ISMTPLAP_Remarks;
                    dmoresult.UpdatedDate = DateTime.UtcNow;
                    dmoresult.ISMTPLAP_UpdatedBy = data.UserId;
                    dmoresult.ISMTPLAP_TotalHrs = data.totaleffort;
                    _IssueContext.Update(dmoresult);
                    decimal totalhrs = 0;

                    foreach (var planner in data.approveplannerArray)
                    {
                        data.ISMTPLAPTA_DeviationFlg = false;
                        if (planner.extraflag == 1)
                        {
                            data.ISMTPLAPTA_ExtraTaskFlg = true;
                            totalhrs += data.ISMTPLAPTA_EffortInHrs;
                        }
                        else
                        {
                            data.ISMTPLAPTA_ExtraTaskFlg = false;
                            totalhrs += 0;
                        }

                        if (planner.plannerStatus == 1)
                        {
                            data.ISMTPLAPTA_ApprovalFlg = true;
                            data.ISMTPLAPTA_RejectedFlg = false;
                        }
                        else
                        {
                            data.ISMTPLAPTA_ApprovalFlg = false;
                            data.ISMTPLAPTA_RejectedFlg = true;
                        }

                        /***************************** Planner Task Approval  **************************/

                        var updateapprovaltask = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                       && a.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPLAPTA_ActiveFlg == true).ToList();

                        if (updateapprovaltask.Count > 0)
                        {
                            var resulttask = _IssueContext.ISM_Task_Planner_Approved_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                             && a.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPLAPTA_ActiveFlg == true);

                            resulttask.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            resulttask.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            resulttask.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            resulttask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            resulttask.ISMTPLAPTA_Status = data.ISMTPLAPTA_Status;
                            resulttask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                            resulttask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                            resulttask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            resulttask.UpdatedDate = DateTime.UtcNow;
                            resulttask.ISMTPLAPTA_UpdatedBy = data.UserId;

                            _IssueContext.Update(resulttask);
                            if (data.ISMTPLAPTA_ApprovalFlg == true)
                            {
                                finaleffort += planner.ISMTPLAPTA_EffortInHrs;
                            }
                        }
                        else
                        {
                            ISM_Task_Planner_Approved_TasksDMO approvedtask = new ISM_Task_Planner_Approved_TasksDMO();
                            approvedtask.ISMTPLAP_Id = Convert.ToInt64(data.ISMTPLAP_Id);
                            approvedtask.ISMTCR_Id = planner.ISMTCR_Id;
                            approvedtask.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            approvedtask.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            approvedtask.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            approvedtask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            approvedtask.ISMTPLAPTA_Status = planner.ISMTPLAPTA_Status;
                            approvedtask.ISMTPLAPTA_DeviationFlg = data.ISMTPLAPTA_DeviationFlg;
                            approvedtask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                            approvedtask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                            approvedtask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            approvedtask.ISMTPLTA_Id = planner.ISMTPLTA_Id;
                            approvedtask.ISMTPLAPTA_ActiveFlg = true;

                            approvedtask.CreatedDate = DateTime.UtcNow;
                            approvedtask.UpdatedDate = DateTime.UtcNow;
                            approvedtask.ISMTPLAPTA_CreatedBy = data.UserId;
                            approvedtask.ISMTPLAPTA_UpdatedBy = data.UserId;
                            _IssueContext.Add(approvedtask);
                            //finaleffort += Convert.ToInt64(planner.ISMTPLAPTA_EffortInHrs);
                            if (data.ISMTPLAPTA_ApprovalFlg == true)
                            {
                                finaleffort += planner.ISMTPLAPTA_EffortInHrs;
                            }
                        }

                        if (data.ISMTPLAPTA_ApprovalFlg == true)
                        {
                            approvedCount = approvedCount + 1;
                        }
                        else if (data.ISMTPLAPTA_RejectedFlg == true)
                        {
                            rejectedCount = rejectedCount + 1;
                        }

                        // ************ Checking Planner Task Is Created Or Not ************ //

                        var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                        && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();

                        if (updateplannertask.Count > 0)
                        {
                            var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                            && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_Id == planner.ISMTPLTA_Id);

                            result.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            result.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            result.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            result.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            result.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            result.UpdatedDate = DateTime.UtcNow;
                            result.ISMTPLTA_UpdatedBy = data.UserId;

                            _IssueContext.Update(result);
                        }
                        else
                        {
                            ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                            dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                            dmo_task.ISMTCR_Id = planner.ISMTCR_Id;
                            dmo_task.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            dmo_task.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            dmo_task.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            dmo_task.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            dmo_task.ISMTPLTA_Status = planner.ISMTPLAPTA_Status;
                            dmo_task.ISMTPLTA_ExtraFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                            dmo_task.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            dmo_task.ISMTPLTA_ActiveFlg = true;
                            dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                            dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                            dmo_task.CreatedDate = DateTime.UtcNow;
                            dmo_task.UpdatedDate = DateTime.UtcNow;
                            _IssueContext.Add(dmo_task);
                        }

                        // ***************** Adding Task To Assigned Task ***************** //
                        var checktaskisassigned = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == plannedby_id
                        && a.ISMTCR_Id == planner.ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                        if (checktaskisassigned.Count() == 0)
                        {
                            ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                            dmo_task_assigned.ISMTCR_Id = data.ISMTCR_Id;
                            dmo_task_assigned.HRME_Id = plannedby_id;
                            dmo_task_assigned.ISMTCRASTO_AssignedDate = DateTime.UtcNow;
                            dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                            dmo_task_assigned.ISMTCRASTO_Remarks = planner.ISMTPLAPTA_Remarks;
                            dmo_task_assigned.ISMTCRASTO_StartDate = planner.ISMTPLAPTA_StartDate;
                            dmo_task_assigned.ISMTCRASTO_EndDate = planner.ISMTPLAPTA_EndDate;
                            dmo_task_assigned.ISMTCRASTO_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                            dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                            dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                            dmo_task_assigned.CreatedDate = DateTime.UtcNow;
                            dmo_task_assigned.UpdatedDate = DateTime.UtcNow;

                            _IssueContext.Add(dmo_task_assigned);
                        }
                    }


                    //***************** Planner Task Overall Status ***************//
                    long totalcount = 0;

                    var plannertasklist = (from a in _IssueContext.ISM_Task_Planner_ApprovedDMO
                                           from b in _IssueContext.ISM_Task_Planner_Approved_TasksDMO
                                           where (a.ISMTPLAP_Id == b.ISMTPLAP_Id && b.ISMTPLAP_Id == data.ISMTPLAP_Id && a.ISMTPL_Id == data.ISMTPL_Id)
                                           select new ISM_PlannerCreationDTO
                                           {
                                               ISMTPL_Id = a.ISMTPL_Id,
                                               ISMTPLAPTA_Id = b.ISMTPLAPTA_Id,
                                           }).Distinct().ToList();

                    totalcount = plannertasklist.Count();

                    if (totalcount == approvedCount)
                    {
                        data.ISMTPL_ApprovalFlg = true;
                    }
                    else if (totalcount == rejectedCount)
                    {
                        data.ISMTPL_ApprovalFlg = false;
                    }
                    else
                    {
                        data.ISMTPL_ApprovalFlg = true;
                    }

                    decimal totalhrslist = 0;
                    totalhrslist = finaleffort + totalhrs;

                    var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                    plannerresult.ISMTPL_TotalHrs = totalhrslist;
                    plannerresult.ISMTPL_ApprovalFlg = data.ISMTPL_ApprovalFlg;
                    plannerresult.ISMTPL_ApprovedBy = empid;
                    _IssueContext.Update(plannerresult);
                    var i = _IssueContext.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                        //****************** EMAIL *********************** //                      
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                    if (hrmEempid.Length > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                    }

                    var planned_id = _IssueContext.ISM_Task_PlannerDMO.Where(t => t.ISMTPL_Id == data.ISMTPL_Id).Distinct().ToArray();
                    if (planned_id.Length > 0)
                    {
                        plannedby_id = planned_id.FirstOrDefault().HRME_Id;
                    }

                    ISM_Task_Planner_ApprovedDMO dmo = new ISM_Task_Planner_ApprovedDMO();
                    dmo.MI_Id = data.MI_Id;
                    dmo.HRME_Id = empid;
                    dmo.ISMTPL_Id = data.ISMTPL_Id;
                    dmo.ISMTPLAP_Remarks = data.ISMTPLAP_Remarks;
                    dmo.ISMTPLAP_ActiveFlg = true;
                    dmo.CreatedDate = DateTime.UtcNow;
                    dmo.UpdatedDate = DateTime.UtcNow;
                    dmo.ISMTPLAP_TotalHrs = data.totaleffort;
                    dmo.ISMTPLAP_CreatedBy = data.UserId;
                    dmo.ISMTPLAP_UpdatedBy = data.UserId;
                    _IssueContext.Add(dmo);
                    decimal totalhrs = 0;


                    foreach (var planner in data.approveplannerArray)
                    {
                        data.ISMTPLAPTA_DeviationFlg = false;
                        if (planner.extraflag == 1)
                        {
                            data.ISMTPLAPTA_ExtraTaskFlg = true;
                            totalhrs += data.ISMTPLAPTA_EffortInHrs;
                        }
                        else
                        {
                            data.ISMTPLAPTA_ExtraTaskFlg = false;
                            totalhrs += 0;
                        }

                        if (planner.plannerStatus == 1)
                        {
                            data.ISMTPLAPTA_ApprovalFlg = true;
                            data.ISMTPLAPTA_RejectedFlg = false;
                        }
                        else
                        {
                            data.ISMTPLAPTA_ApprovalFlg = false;
                            data.ISMTPLAPTA_RejectedFlg = true;
                        }

                        /***************************** Planner Task Approval  **************************/
                        ISM_Task_Planner_Approved_TasksDMO approvedtask = new ISM_Task_Planner_Approved_TasksDMO();
                        approvedtask.ISMTPLAP_Id = dmo.ISMTPLAP_Id;
                        approvedtask.ISMTCR_Id = planner.ISMTCR_Id;
                        approvedtask.ISMTPLAPTA_Remarks = planner.ISMTPLAPTA_Remarks;
                        approvedtask.ISMTPLAPTA_StartDate = planner.ISMTPLAPTA_StartDate;
                        approvedtask.ISMTPLAPTA_EndDate = planner.ISMTPLAPTA_EndDate;
                        approvedtask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                        approvedtask.ISMTPLAPTA_Status = planner.ISMTPLAPTA_Status;
                        approvedtask.ISMTPLAPTA_DeviationFlg = data.ISMTPLAPTA_DeviationFlg;
                        approvedtask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                        approvedtask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                        approvedtask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                        approvedtask.ISMTPLTA_Id = planner.ISMTPLTA_Id;
                        approvedtask.ISMTPLAPTA_ActiveFlg = true;

                        approvedtask.CreatedDate = DateTime.UtcNow;
                        approvedtask.UpdatedDate = DateTime.UtcNow;
                        approvedtask.ISMTPLAPTA_CreatedBy = data.UserId;
                        approvedtask.ISMTPLAPTA_UpdatedBy = data.UserId;

                        _IssueContext.Add(approvedtask);
                        if (data.ISMTPLAPTA_ApprovalFlg == true)
                        {
                            finaleffort += planner.ISMTPLAPTA_EffortInHrs;
                        }
                        if (data.ISMTPLAPTA_ApprovalFlg == true)
                        {
                            approvedCount = approvedCount + 1;
                        }
                        else if (data.ISMTPLAPTA_RejectedFlg == true)
                        {
                            rejectedCount = rejectedCount + 1;
                        }
                        // ************ Checking Planner Task Is Created Or Not ************ //

                        var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                        && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();

                        if (updateplannertask.Count > 0)
                        {
                            var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                            && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_Id == planner.ISMTPLTA_Id);

                            result.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            result.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            result.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            result.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            result.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            result.UpdatedDate = DateTime.UtcNow;
                            result.ISMTPLTA_UpdatedBy = data.UserId;

                            _IssueContext.Update(result);
                        }
                        else
                        {
                            ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                            dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                            dmo_task.ISMTCR_Id = planner.ISMTCR_Id;
                            dmo_task.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                            dmo_task.ISMTPLTA_StartDate = planner.ISMTPLAPTA_StartDate;
                            dmo_task.ISMTPLTA_EndDate = planner.ISMTPLAPTA_EndDate;
                            dmo_task.ISMTPLTA_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            dmo_task.ISMTPLTA_Status = planner.ISMTPLAPTA_Status;
                            dmo_task.ISMTPLTA_ExtraFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                            dmo_task.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            dmo_task.ISMTPLTA_ActiveFlg = true;
                            dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                            dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                            dmo_task.CreatedDate = DateTime.UtcNow;
                            dmo_task.UpdatedDate = DateTime.UtcNow;
                            _IssueContext.Add(dmo_task);
                        }

                        // ***************** Adding Task To Assigned Task ***************** //

                        var checktaskisassigned = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == plannedby_id
                        && a.ISMTCR_Id == planner.ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                        if (checktaskisassigned.Count() == 0)
                        {
                            ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                            dmo_task_assigned.ISMTCR_Id = planner.ISMTCR_Id;
                            dmo_task_assigned.HRME_Id = plannedby_id;
                            dmo_task_assigned.ISMTCRASTO_AssignedDate = DateTime.UtcNow;
                            dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                            dmo_task_assigned.ISMTCRASTO_Remarks = planner.ISMTPLAPTA_Remarks;
                            dmo_task_assigned.ISMTCRASTO_StartDate = planner.ISMTPLAPTA_StartDate;
                            dmo_task_assigned.ISMTCRASTO_EndDate = planner.ISMTPLAPTA_EndDate;
                            dmo_task_assigned.ISMTCRASTO_EffortInHrs = planner.ISMTPLAPTA_EffortInHrs;
                            dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                            dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                            dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                            dmo_task_assigned.CreatedDate = DateTime.UtcNow;
                            dmo_task_assigned.UpdatedDate = DateTime.UtcNow;

                            _IssueContext.Add(dmo_task_assigned);
                        }
                    }

                    var i = _IssueContext.SaveChanges();


                    //***************** Planner Task Overall Status ***************//
                    long totalcount = 0;

                    var plannertasklist = (from a in _IssueContext.ISM_Task_Planner_ApprovedDMO
                                           from b in _IssueContext.ISM_Task_Planner_Approved_TasksDMO
                                           where (a.ISMTPLAP_Id == b.ISMTPLAP_Id && a.ISMTPL_Id == data.ISMTPL_Id)
                                           select new ISM_PlannerCreationDTO
                                           {
                                               ISMTPL_Id = a.ISMTPL_Id,
                                               ISMTPLAPTA_Id = b.ISMTPLAPTA_Id,
                                           }).Distinct().ToList();

                    totalcount = plannertasklist.Count();

                    if (totalcount == approvedCount)
                    {
                        data.ISMTPL_ApprovalFlg = true;
                    }
                    else if (totalcount == rejectedCount)
                    {
                        data.ISMTPL_ApprovalFlg = false;
                    }
                    else
                    {
                        data.ISMTPL_ApprovalFlg = true;
                    }
                    decimal totalhrslist = 0;
                    totalhrslist = finaleffort + totalhrs;

                    var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                    plannerresult.ISMTPL_TotalHrs = totalhrslist;
                    plannerresult.ISMTPL_ApprovalFlg = data.ISMTPL_ApprovalFlg;
                    plannerresult.ISMTPL_ApprovedBy = empid;
                    _IssueContext.Update(plannerresult);
                    var j = _IssueContext.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                        //****************** EMAIL *********************** //

                        /*     string str = "";
                             DateTime fromdatecon = DateTime.Now;
                             TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                             DateTime toatecon = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                             List<ISM_PlannerCreationDTO> result = new List<ISM_PlannerCreationDTO>();
                             string confromdate = "";
                             string contodate = "";
                             string datetime = "";
                             try
                             {
                                 contodate = toatecon.ToString("dd/MM/yyyy");
                                 datetime = toatecon.ToString("dd/MM/yyyy hh:mm tt");
                             }
                             catch (Exception ex)
                             {
                                 Console.WriteLine(ex.Message);
                             }

                             List<ISM_PlannerCreationDTO> dtonewtemp = new List<ISM_PlannerCreationDTO>();

                             using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                             {
                                 cmd.CommandText = "ISM_PlannerApproval_Search";
                                 cmd.CommandType = CommandType.StoredProcedure;

                                 cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                                 {
                                     Value = data.MI_Id
                                 });

                                 cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                                 {
                                     Value = plannedby_id
                                 });
                                 cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id", SqlDbType.VarChar)
                                 {
                                     Value = data.ISMTPL_Id
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
                                             dtonewtemp.Add(new ISM_PlannerCreationDTO
                                             {
                                                 ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                                 ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                                 ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                                 ISMMCLT_ClientName = Convert.ToString(dataReader["ISMMCLT_ClientName"]),
                                                 HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                                 ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                                 ISMTPLAPTA_Statusnew = Convert.ToString(dataReader["ISMTPLAPTA_Status"]),
                                                 approvedby = Convert.ToString(dataReader["approvedby"]),
                                                 ISMTPLAPTA_StartDate = Convert.ToDateTime(dataReader["ISMTPLAPTA_StartDate"]),
                                                 ISMTPLAPTA_EndDate = Convert.ToDateTime(dataReader["ISMTPLAPTA_EndDate"]),
                                                 ISMTPLAPTA_EffortInHrsnew = Convert.ToString(dataReader["ISMTPLAPTA_EffortInHrs"]),
                                             });
                                         }
                                     }
                                 }
                                 catch (Exception ex)
                                 {
                                     Console.WriteLine(ex.Message);
                                 }
                             }


                             var getemployeedetails = (from a in _IssueContext.MasterEmployee
                                                       from b in _IssueContext.HR_Master_Department
                                                       from c in _IssueContext.HR_Master_Designation
                                                       from d in _IssueContext.Multiple_Email_DMO
                                                       from e in _IssueContext.HR_Master_Grade
                                                       where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_Id == d.HRME_Id && a.HRMG_Id == e.HRMG_Id
                                                       && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && d.HRMEM_DeFaultFlag == "default"
                                                       && a.MI_Id == data.MI_Id && a.HRME_Id == empid)
                                                       select new ISM_PlannerCreationDTO
                                                       {
                                                           employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                           + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                           + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                                           departmnt = b.HRMD_DepartmentName,
                                                           desgination = c.HRMDES_DesignationName,
                                                           gradename = e.HRMG_GradeName,
                                                           emailid = d.HRMEM_EmailId,
                                                           HRMD_Id = Convert.ToInt64(a.HRMD_Id)
                                                       }).Distinct().ToList();


                             var emaildetails = _IssueContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == getemployeedetails.FirstOrDefault().HRMD_Id).ToList();

                             string emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                             string emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;

                             var checktemplate = _IssueContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "Planner_Status"
                            && a.ISES_MailActiveFlag == true).ToList();
                             if (checktemplate.Count == 0)
                             {
                                 data.message = "Email Template not Mapped to the selected Institution";
                             }
                             else
                             {
                                 var emplname = getemployeedetails.FirstOrDefault().employeename;
                                 var deptname = getemployeedetails.FirstOrDefault().departmnt;
                                 var desginame = getemployeedetails.FirstOrDefault().desgination;
                                 var gradname = getemployeedetails.FirstOrDefault().gradename;
                                 var email = getemployeedetails.FirstOrDefault().emailid;
                                 var date = contodate;
                                 var newdate = datetime;

                                 var completeEffort = data.ISMTPL_TotalHrs;

                                 string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                                 templatedetails = templatedetails.Replace("[DeveloperName]", emplname);
                                 templatedetails = templatedetails.Replace("[DATE]", date);
                                 templatedetails = templatedetails.Replace("[Department]", deptname);
                                 templatedetails = templatedetails.Replace("[Designation]", desginame);
                                 templatedetails = templatedetails.Replace("[CompletedEffort]", completeEffort.ToString());
                                 templatedetails = templatedetails.Replace("[DATENAME]", newdate);

                                 int rownumber = 0;

                                 if (dtonewtemp.Count > 0)
                                 {

                                     for (int j = 0; j < dtonewtemp.Count; j++)
                                     {
                                         rownumber += 1;
                                         str += "<tr>" +
                                             "<td align=" + "center" + " style=" + ">" + rownumber + "</td> " +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_TaskNo + "</td>" +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_Title + "</td>" +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].ISMTCR_BugOREnhancementFlg + "</td> " +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].ISMMCLT_ClientName + "</td> " +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].HRMP_Name + "</td> " +
                                             "<td align=" + "left" + "style=" + "word-break: break-word;" + ">" + dtonewtemp[j].ISMTCR_Desc + "</td> " +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLAPTA_Statusnew + "</td> " +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].approvedby + "</td> " +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLAPTA_StartDate + "</td> " +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLAPTA_EndDate + "</td> " +
                                             "<td align=" + "left" + ">" + dtonewtemp[j].ISMTPLAPTA_EffortInHrsnew + "</td> " +
                                             "</tr>";
                                     }
                                 }

                                 templatedetails = templatedetails.Replace("[MSG]", str);
                                 try
                                 {
                                     // sendmailexamtt(data.MI_Id, email, templatedetails, emailccdetails, emailbccdetails);
                                 }
                                 catch (Exception ex)
                                 {
                                     Console.WriteLine(ex.Message);
                                 }
                             }
                             */
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (counttrue == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_PlannerCreationDTO allupdateplannerStatus(ISM_PlannerCreationDTO data)
        {
            long empid = 0;
            long plannedby_id = 0;
            long counttrue = 0;
            long approvedCount = 0;
            long rejectedCount = 0;
            decimal finaleffort = 0;
            var empname = "";
            try
            {
                List<long> Emplist = new List<long>();
                if (data.Emplist.Length > 0)
                {
                    foreach (var ue in data.Emplist)
                    {

                        var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                        if (hrmEempid.Length > 0)
                        {
                            empid = hrmEempid.FirstOrDefault().Emp_Code;
                        }

                        var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                        if (hrmeDepartment_id.Count > 0)
                        {

                            empname = ((hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName == "" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeFirstName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeMiddleName) + (hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == null || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "" || hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName == "0" ? "" : " " + hrmeDepartment_id.FirstOrDefault().HRME_EmployeeLastName)).Trim();
                        }

                        plannedby_id = ue.HRME_Id;
                        data.ISMTPLAP_Remarks = "Approved by - " + empname;
                        data.ISMTPL_Id = ue.ISMTPL_Id;
                        data.MI_Id = ue.MI_Id;

                        ISM_Task_Planner_ApprovedDMO dmo = new ISM_Task_Planner_ApprovedDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.HRME_Id = empid;
                        dmo.ISMTPL_Id = data.ISMTPL_Id;
                        dmo.ISMTPLAP_Remarks = data.ISMTPLAP_Remarks;
                        dmo.ISMTPLAP_ActiveFlg = true;
                        dmo.CreatedDate = DateTime.UtcNow;
                        dmo.UpdatedDate = DateTime.UtcNow;
                        dmo.ISMTPLAP_TotalHrs = ue.ISMTPL_TotalHrs;
                        dmo.ISMTPLAP_CreatedBy = data.UserId;
                        dmo.ISMTPLAP_UpdatedBy = data.UserId;
                        _IssueContext.Add(dmo);
                        decimal totalhrs = 0;

                        List<ISM_PlannerCreationDTO> dtonewtemp = new List<ISM_PlannerCreationDTO>();
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Planner_TaskDeatils";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                  SqlDbType.VarChar)
                            {
                                Value = ue.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISMTPL_Id",
                    SqlDbType.VarChar)
                            {
                                Value = data.ISMTPL_Id
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
                                        dtonewtemp.Add(new ISM_PlannerCreationDTO
                                        {
                                            ISMTPL_Id = Convert.ToInt64(dataReader["ISMTPL_Id"]),
                                            ISMTPLTA_Id = Convert.ToInt64(dataReader["ISMTPLTA_Id"]),
                                            ISMTCR_Id = Convert.ToInt64(dataReader["ISMTCR_Id"]),
                                            HRMPR_Id = Convert.ToInt64(dataReader["HRMPR_Id"]),
                                            HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                            // ISMTPLAP_Id = Convert.ToInt64(dataReader["ISMTPLAP_Id"]),
                                            ISMTCR_BugOREnhancement = Convert.ToString(dataReader["ISMTCR_BugOREnhancement"]),
                                            ISMTCR_CreationDate = Convert.ToDateTime(dataReader["ISMTCR_CreationDate"]),
                                            ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                            ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                            ISMTCR_Status = Convert.ToString(dataReader["ISMTCR_Status"]),
                                            ISMTCR_ReOpenFlg = Convert.ToBoolean(dataReader["ISMTCR_ReOpenFlg"]),
                                            //ISMTCR_ReOpenDate = Convert.ToDateTime(dataReader["ISMTCR_ReOpenDate"]),
                                            ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                            //ISMMCLT_Id = Convert.ToInt64(dataReader["ISMMCLT_Id"]),
                                            // ISMMCLT_ClientName = Convert.ToString(dataReader["ISMMCLT_ClientName"]),
                                            ISMTPL_CreatedBy = Convert.ToInt64(dataReader["ISMTPL_CreatedBy"]),
                                            plannerDate = Convert.ToDateTime(dataReader["plannerDate"]),
                                            ISMTPLTA_Status = Convert.ToString(dataReader["ISMTPLTA_Status"]),
                                            ISMTPL_PlannedBy = Convert.ToInt64(dataReader["ISMTPL_PlannedBy"]),
                                            ISMTPL_PlannerName = Convert.ToString(dataReader["ISMTPL_PlannerName"]),
                                            // ISMTPL_Remarks = Convert.ToString(dataReader["ISMTPL_Remarks"]),
                                            ISMTPL_StartDate = Convert.ToDateTime(dataReader["ISMTPL_StartDate"]),
                                            ISMTPL_EndDate = Convert.ToDateTime(dataReader["ISMTPL_EndDate"]),
                                            ISMTPL_TotalHrs = Convert.ToDecimal(dataReader["ISMTPL_TotalHrs"]),
                                            ISMTPL_ApprovalFlg = Convert.ToBoolean(dataReader["ISMTPL_ApprovalFlg"]),
                                            ISMTPL_ApprovedBy = Convert.ToInt64(dataReader["ISMTPL_ApprovedBy"]),
                                            ISMTPL_ActiveFlg = Convert.ToBoolean(dataReader["ISMTPL_ActiveFlg"]),
                                            ISMTPLTA_ApprovalFlg = Convert.ToBoolean(dataReader["ISMTPLTA_ApprovalFlg"]),
                                            ISMTPLTA_StartDate = Convert.ToDateTime(dataReader["ISMTPLTA_StartDate"]),
                                            ISMTPLTA_EndDate = Convert.ToDateTime(dataReader["ISMTPLTA_EndDate"]),
                                            ISMTPLTA_EffortInHrs = Convert.ToDecimal(dataReader["ISMTPLTA_EffortInHrs"]),
                                            // ISMTPLTA_Remarks = Convert.ToString(dataReader["ISMTPLTA_Remarks"]),
                                            plannedby = Convert.ToString(dataReader["plannedby"])
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        foreach (var planner in dtonewtemp)
                        {
                            data.ISMTPLAPTA_DeviationFlg = false;

                            data.ISMTPLAPTA_ExtraTaskFlg = false;
                            totalhrs += 0;
                            data.ISMTPLAPTA_ApprovalFlg = true;
                            data.ISMTPLAPTA_RejectedFlg = false;


                            /***************************** Planner Task Approval  **************************/
                            ISM_Task_Planner_Approved_TasksDMO approvedtask = new ISM_Task_Planner_Approved_TasksDMO();
                            approvedtask.ISMTPLAP_Id = dmo.ISMTPLAP_Id;
                            approvedtask.ISMTCR_Id = planner.ISMTCR_Id;
                            approvedtask.ISMTPLAPTA_Remarks = data.ISMTPLAP_Remarks;
                            approvedtask.ISMTPLAPTA_StartDate = planner.ISMTPLTA_StartDate;
                            approvedtask.ISMTPLAPTA_EndDate = planner.ISMTPLTA_EndDate;
                            approvedtask.ISMTPLAPTA_EffortInHrs = planner.ISMTPLTA_EffortInHrs;
                            approvedtask.ISMTPLAPTA_Status = planner.ISMTPLTA_Status;
                            approvedtask.ISMTPLAPTA_DeviationFlg = data.ISMTPLTA_DeviationFlg;
                            approvedtask.ISMTPLAPTA_ExtraTaskFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                            approvedtask.ISMTPLAPTA_RejectedFlg = data.ISMTPLAPTA_RejectedFlg;
                            approvedtask.ISMTPLAPTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                            approvedtask.ISMTPLTA_Id = planner.ISMTPLTA_Id;
                            approvedtask.ISMTPLAPTA_ActiveFlg = true;

                            approvedtask.CreatedDate = DateTime.UtcNow;
                            approvedtask.UpdatedDate = DateTime.UtcNow;
                            approvedtask.ISMTPLAPTA_CreatedBy = data.UserId;
                            approvedtask.ISMTPLAPTA_UpdatedBy = data.UserId;

                            _IssueContext.Add(approvedtask);
                            if (data.ISMTPLAPTA_ApprovalFlg == true)
                            {
                                finaleffort += planner.ISMTPLAPTA_EffortInHrs;
                            }
                            if (data.ISMTPLAPTA_ApprovalFlg == true)
                            {
                                approvedCount = approvedCount + 1;
                            }
                            else if (data.ISMTPLAPTA_RejectedFlg == true)
                            {
                                rejectedCount = rejectedCount + 1;
                            }
                            // ************ Checking Planner Task Is Created Or Not ************ //

                            var updateplannertask = _IssueContext.ISM_Task_Planner_TasksDMO.Where(a => a.ISMTCR_Id == planner.ISMTCR_Id
                            && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true).ToList();

                            if (updateplannertask.Count > 0)
                            {
                                var result = _IssueContext.ISM_Task_Planner_TasksDMO.Single(a => a.ISMTCR_Id == planner.ISMTCR_Id
                                && a.ISMTPL_Id == data.ISMTPL_Id && a.ISMTPLTA_ActiveFlg == true && a.ISMTPLTA_Id == planner.ISMTPLTA_Id);

                                result.ISMTPLTA_StartDate = planner.ISMTPLTA_StartDate;
                                result.ISMTPLTA_EndDate = planner.ISMTPLTA_EndDate;
                                result.ISMTPLTA_EffortInHrs = planner.ISMTPLTA_EffortInHrs;
                                result.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                result.ISMTPLTA_Remarks = planner.ISMTPLAPTA_Remarks;
                                result.UpdatedDate = DateTime.UtcNow;
                                result.ISMTPLTA_UpdatedBy = data.UserId;

                                _IssueContext.Update(result);
                            }
                            else
                            {
                                ISM_Task_Planner_TasksDMO dmo_task = new ISM_Task_Planner_TasksDMO();

                                dmo_task.ISMTPL_Id = data.ISMTPL_Id;
                                dmo_task.ISMTCR_Id = planner.ISMTCR_Id;
                                dmo_task.ISMTPLTA_Remarks = planner.ISMTPLTA_Remarks;
                                dmo_task.ISMTPLTA_StartDate = planner.ISMTPLTA_StartDate;
                                dmo_task.ISMTPLTA_EndDate = planner.ISMTPLTA_EndDate;
                                dmo_task.ISMTPLTA_EffortInHrs = planner.ISMTPLTA_EffortInHrs;
                                dmo_task.ISMTPLTA_Status = planner.ISMTPLTA_Status;
                                dmo_task.ISMTPLTA_ExtraFlg = data.ISMTPLAPTA_ExtraTaskFlg;
                                dmo_task.ISMTPLTA_ApprovalFlg = data.ISMTPLAPTA_ApprovalFlg;
                                dmo_task.ISMTPLTA_ActiveFlg = true;
                                dmo_task.ISMTPLTA_CreatedBy = data.UserId;
                                dmo_task.ISMTPLTA_UpdatedBy = data.UserId;
                                dmo_task.CreatedDate = DateTime.UtcNow;
                                dmo_task.UpdatedDate = DateTime.UtcNow;
                                _IssueContext.Add(dmo_task);
                            }

                            // ***************** Adding Task To Assigned Task ***************** //

                            var checktaskisassigned = _IssueContext.ISM_TaskCreation_AssignedToDMO.Where(a => a.HRME_Id == plannedby_id
                            && a.ISMTCR_Id == planner.ISMTCR_Id && a.ISMTCRASTO_ActiveFlg == true).ToList();

                            if (checktaskisassigned.Count() == 0)
                            {
                                ISM_TaskCreation_AssignedToDMO dmo_task_assigned = new ISM_TaskCreation_AssignedToDMO();
                                dmo_task_assigned.ISMTCR_Id = planner.ISMTCR_Id;
                                dmo_task_assigned.HRME_Id = plannedby_id;
                                dmo_task_assigned.ISMTCRASTO_AssignedDate = DateTime.UtcNow;
                                dmo_task_assigned.ISMTCRASTO_AssignedBy = empid;
                                dmo_task_assigned.ISMTCRASTO_Remarks = planner.ISMTPLTA_Remarks;
                                dmo_task_assigned.ISMTCRASTO_StartDate = planner.ISMTPLTA_StartDate;
                                dmo_task_assigned.ISMTCRASTO_EndDate = planner.ISMTPLTA_EndDate;
                                dmo_task_assigned.ISMTCRASTO_EffortInHrs = planner.ISMTPLTA_EffortInHrs;
                                dmo_task_assigned.ISMTCRASTO_ActiveFlg = true;
                                dmo_task_assigned.ISMTCRASTO_CreatedBy = data.UserId;
                                dmo_task_assigned.ISMTCRASTO_UpdatedBy = data.UserId;
                                dmo_task_assigned.CreatedDate = DateTime.UtcNow;
                                dmo_task_assigned.UpdatedDate = DateTime.UtcNow;

                                _IssueContext.Add(dmo_task_assigned);
                            }
                        }


                        //***************** Planner Task Overall Status ***************//
                        long totalcount = 0;

                        var plannertasklist = (from a in _IssueContext.ISM_Task_Planner_ApprovedDMO
                                               from b in _IssueContext.ISM_Task_Planner_Approved_TasksDMO
                                               where (a.ISMTPLAP_Id == b.ISMTPLAP_Id && a.ISMTPL_Id == data.ISMTPL_Id)
                                               select new ISM_PlannerCreationDTO
                                               {
                                                   ISMTPL_Id = a.ISMTPL_Id,
                                                   ISMTPLAPTA_Id = b.ISMTPLAPTA_Id,
                                               }).Distinct().ToList();

                        totalcount = plannertasklist.Count();

                        data.ISMTPL_ApprovalFlg = true;

                        decimal totalhrslist = 0;
                        totalhrslist = finaleffort + totalhrs;

                        var plannerresult = _IssueContext.ISM_Task_PlannerDMO.Single(a => a.ISMTPL_Id == data.ISMTPL_Id);
                        plannerresult.ISMTPL_TotalHrs = totalhrslist;
                        plannerresult.ISMTPL_ApprovalFlg = data.ISMTPL_ApprovalFlg;
                        plannerresult.ISMTPL_ApprovedBy = empid;
                        _IssueContext.Update(plannerresult);
                        var i = _IssueContext.SaveChanges();

                        if (i > 0)
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
            catch (Exception ex)
            {
                if (counttrue == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Planner_Status" && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = Resultsms;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _IssueContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mail_id);

                        if (Attechement.Equals("1"))
                        {
                            var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        //******************* EMAIL CC DETAILS ****************//

                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        ////******************* EMAIL BCC DETAILS ****************//

                        //if (emailbccdetails != null && emailbccdetails != "")
                        //{
                        //    string[] bccmaildetails = emailbccdetails.Split(',');

                        //    foreach (var c in bccmaildetails)
                        //    {
                        //        message.AddBcc(c.ToString());
                        //    }
                        //}


                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
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


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }
                                //******************* EMAIL CC DETAILS ****************//

                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }

                                ////******************* EMAIL BCC DETAILS ****************//

                                //if (emailbccdetails != null && emailbccdetails != "")
                                //{
                                //    string[] bccmaildetails = emailbccdetails.Split(',');

                                //    foreach (var c in bccmaildetails)
                                //    {
                                //        emailMessage.Bcc.Add(c.ToString());
                                //    }
                                //}


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Planner_Report_Generation" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _IssueContext.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
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
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }

        public async Task<ISM_PlannerCreationDTO> getApprovedPlannerDetail(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Approved_Planner_Detail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.BigInt)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMTPLAP_Id",
          SqlDbType.BigInt)
                    {
                        Value = data.ISMTPLAP_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
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
                        data.get_ApprovedPlannerDetail = retObject.ToArray();
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

        public ISM_PlannerCreationDTO removetask(ISM_PlannerCreationDTO data)
        {
            try
            {
                var result = _IssueContext.ISM_TaskCreation_AssignedToDMO.Single(t => t.ISMTCRASTO_Id == data.ISMTCRASTO_Id);

                if (result.ISMTCRASTO_ActiveFlg == true)
                {
                    result.ISMTCRASTO_ActiveFlg = false;
                }
                else if (result.ISMTCRASTO_ActiveFlg == false)
                {
                    result.ISMTCRASTO_ActiveFlg = true;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ISM_PlannerReportsDTO> sendschedularnotice(ISM_PlannerReportsDTO data)
        {
            try
            {
                data.communicationfalg = "Notice";
                data.type = true;
                data.Deviationreporttype = "Emp";
                List<EmplistforMNNEDTO> datalist = new List<EmplistforMNNEDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PEDING_MEMO_LIST_NOTICE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                                datalist.Add(new EmplistforMNNEDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                    comp_Date = (String.IsNullOrEmpty(dataReader["comp_Date"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dataReader["comp_Date"].ToString())),
                                    start_Date = (String.IsNullOrEmpty(dataReader["ISMTPL_StartDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dataReader["ISMTPL_StartDate"].ToString())),
                                    end_Date = (String.IsNullOrEmpty(dataReader["ISMTPL_EndDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dataReader["ISMTPL_EndDate"].ToString())),
                                    ISMEMN_Date = Convert.ToString(dataReader["ISMEMN_Date"]),
                                    ISMEMN_CompleByDate = Convert.ToString(dataReader["ISMEMN_CompleByDate"]),
                                    ISMEMN_No = Convert.ToString(dataReader["ISMEMN_No"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                string TransNo = "";
                string taskdetails = "";
                if (datalist.Count > 0)
                {
                    foreach (var ue in datalist)
                    {
                        var Employee = (from a in _IssueContext.MasterEmployee
                                        from b in _Context.Multiple_Email_DMO
                                        where a.HRME_Id == b.HRME_Id && a.HRME_Id == ue.HRME_Id && b.HRME_Id == ue.HRME_Id
                                        select new ISM_PlannerReportsDTO
                                        {
                                            HRMEM_EmailId = b.HRMEM_EmailId,
                                            HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                            HRMD_Id = Convert.ToInt64(a.HRMD_Id),
                                            MI_Id = a.MI_Id,
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim()

                                        }).ToList();
                        if (Employee.Count > 0)
                        {
                            if (datalist.Count > 0)
                            {
                                decimal deviationper = 0;
                                foreach (var plan in datalist)
                                {
                                    if (plan.plannerdetails != null && plan.plannerdetails.Length > 1)
                                    {
                                        foreach (var planD in plan.plannerdetails)
                                        {
                                            deviationper = deviationper + Convert.ToDecimal(planD.Deviation_Percentage);
                                        }
                                        deviationper = (deviationper / plan.plannerdetails.Length);
                                    }
                                    else if (plan.plannerdetails != null && plan.plannerdetails.Length == 0)
                                    {
                                        deviationper = Convert.ToDecimal(plan.plannerdetails.FirstOrDefault().Deviation_Percentage);
                                    }
                                    data.deviation = deviationper.ToString();
                                }
                            }
                            data.MI_Id = Employee.FirstOrDefault().MI_Id;
                            //if (ue.comp_Date == null)
                            //{
                            var compdays = completebydays(1, "Notice");
                            data.comp_Date = DateTime.Now.AddDays(compdays);
                            //}
                            //else
                            //{
                            //    data.comp_Date = ue.comp_Date;
                            //}
                            data.ASMAY_Id = _IssueContext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                            data.HRME_Id = ue.HRME_Id;
                            data.employeename = Employee.FirstOrDefault().employeename;
                            data.HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                            if (Employee.FirstOrDefault().HRMEM_EmailId != null && Employee.FirstOrDefault().HRMEM_EmailId != "")
                            {
                                var EmployeeEmail = Employee.FirstOrDefault().HRMEM_EmailId;
                                data.EmployeeEmail = EmployeeEmail;
                                if (data.communicationfalg == "Notice")
                                {
                                    data.startdate = ue.start_Date;
                                    data.enddate = ue.end_Date;
                                    data.ISMEMN_No = ue.ISMEMN_No;
                                    data.ISMEMN_Date = ue.ISMEMN_Date;
                                    data.ISMEMN_CompleByDate = ue.ISMEMN_CompleByDate;

                                    TransNo = gettransactionno(data);
                                    taskdetails = gettaskregisterReportnotice(data);
                                    data.message = sendmail(data.MI_Id, EmployeeEmail, "NOTICE", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, data.communicationfalg, TransNo, data.communicationmsg, taskdetails, data, data.comp_Date);
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public async Task<ISM_PlannerReportsDTO> sendschedularplannernotice(ISM_PlannerReportsDTO data)
        {
            try
            {
                ISM_PlannerReportsDTO dates = new ISM_PlannerReportsDTO();
                dates = startenddays(data);

                //System.Globalization.CultureInfo ci =
                //  System.Threading.Thread.CurrentThread.CurrentCulture;
                //DayOfWeek firstdayofweek = ci.DateTimeFormat.FirstDayOfWeek;

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                DayOfWeek today = indiantime0.DayOfWeek;

                //DateTime weekstartdate = DateTime.Now.AddDays(-(today - firstdayofweek)).Date;
                //DateTime weekenddate = weekstartdate.AddDays(6).Date;
                data.startdate = dates.startdate;
                data.enddate = dates.enddate;

                if (dates.message == today.ToString())
                {
                    data.communicationfalg = "Notice";
                    data.type = false;
                    data.Deviationreporttype = "Emp";
                    List<EmplistforMNNEDTO> datalist = new List<EmplistforMNNEDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_PlannerNotgeneratedALL";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@StartDate",
                                   SqlDbType.DateTime)
                        {
                            Value = data.startdate
                        });

                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                        SqlDbType.DateTime)
                        {
                            Value = data.enddate
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
                                    datalist.Add(new EmplistforMNNEDTO
                                    {
                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    string TransNo = "";
                    string taskdetails = "";
                    if (datalist.Count > 0)
                    {
                        foreach (var ue in datalist)
                        {
                            var Employee = (from a in _IssueContext.MasterEmployee
                                            from b in _Context.Multiple_Email_DMO
                                            where a.HRME_Id == b.HRME_Id && a.HRME_Id == ue.HRME_Id && b.HRME_Id == ue.HRME_Id
                                            select new ISM_PlannerReportsDTO
                                            {
                                                HRMEM_EmailId = b.HRMEM_EmailId,
                                                HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                                HRMD_Id = Convert.ToInt64(a.HRMD_Id),
                                                MI_Id = a.MI_Id,
                                                employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim()

                                            }).ToList();
                            if (Employee.Count > 0)
                            {

                                data.MI_Id = Employee.FirstOrDefault().MI_Id;
                                data.comp_Date = DateTime.Now.AddDays(3);
                                data.ASMAY_Id = _IssueContext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                                data.HRME_Id = ue.HRME_Id;
                                data.employeename = Employee.FirstOrDefault().employeename;
                                data.HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                                if (Employee.FirstOrDefault().HRMEM_EmailId != null && Employee.FirstOrDefault().HRMEM_EmailId != "")
                                {
                                    var EmployeeEmail = Employee.FirstOrDefault().HRMEM_EmailId;
                                    data.EmployeeEmail = EmployeeEmail;
                                    data.communicationfalg = "Notice";
                                    if (data.communicationfalg == "Notice")
                                    {
                                        TransNo = gettransactionno(data);
                                        //taskdetails = gettaskregisterReportnotice(data);
                                        data.communicationfalg = "Planner-Notice";
                                        data.message = sendmail(data.MI_Id, EmployeeEmail, "PlannerNotGeneratedAll", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, "Notice", TransNo, data.communicationmsg, taskdetails, data, data.comp_Date);
                                    }

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public async Task<ISM_PlannerReportsDTO> empcommunication(ISM_PlannerReportsDTO data)
        {
            try
            {
                string TransNo = "";
                string taskdetails = "";
                if (data.EmplistforMNNE.Length > 0)
                {
                    foreach (var ue in data.EmplistforMNNE)
                    {
                        var Employee = (from a in _IssueContext.MasterEmployee
                                        from b in _Context.Multiple_Email_DMO
                                        where a.HRME_Id == b.HRME_Id && a.HRME_Id == ue.HRME_Id && b.HRME_Id == ue.HRME_Id
                                        select new ISM_PlannerReportsDTO
                                        {
                                            HRMEM_EmailId = b.HRMEM_EmailId,
                                            HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                            HRMD_Id = Convert.ToInt64(a.HRMD_Id),
                                            MI_Id = a.MI_Id,
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim()

                                        }).ToList();
                        if (Employee.Count > 0)
                        {
                            if (data.EmplistforMNNE.Length > 0)
                            {
                                decimal deviationper = 0;
                                foreach (var plan in data.EmplistforMNNE)
                                {
                                    if (plan.plannerdetails != null && plan.plannerdetails.Length > 1)
                                    {
                                        foreach (var planD in plan.plannerdetails)
                                        {
                                            deviationper = deviationper + Convert.ToDecimal(planD.Deviation_Percentage);
                                        }
                                        deviationper = (deviationper / plan.plannerdetails.Length);
                                    }
                                    else if (plan.plannerdetails != null && plan.plannerdetails.Length == 0)
                                    {
                                        deviationper = Convert.ToDecimal(plan.plannerdetails.FirstOrDefault().Deviation_Percentage);
                                    }
                                    data.deviation = deviationper.ToString();
                                }
                            }
                            data.MI_Id = Employee.FirstOrDefault().MI_Id;
                            data.comp_Date = ue.comp_Date;
                            data.ASMAY_Id = _IssueContext.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                            data.HRME_Id = ue.HRME_Id;
                            data.employeename = Employee.FirstOrDefault().employeename;
                            data.HRMD_Id = Employee.FirstOrDefault().HRMD_Id;
                            if (Employee.FirstOrDefault().HRMEM_EmailId != null && Employee.FirstOrDefault().HRMEM_EmailId != "")
                            {
                                var EmployeeEmail = Employee.FirstOrDefault().HRMEM_EmailId;
                                data.EmployeeEmail = EmployeeEmail;
                                //data.EmployeeEmail = "rakesh.reddy@vapstech.com";
                                //EmployeeEmail = data.EmployeeEmail;
                                if (data.communicationfalg == "Notice")
                                {
                                    data.startdate = ue.start_Date;
                                    data.enddate = ue.end_Date;
                                    data.ISMEMN_No = ue.ISMEMN_No;
                                    data.ISMEMN_Date = ue.ISMEMN_Date;
                                    data.ISMEMN_CompleByDate = ue.ISMEMN_CompleByDate;

                                    TransNo = gettransactionno(data);
                                    taskdetails = gettaskregisterReportnotice(data);
                                    data.message = sendmail(data.MI_Id, EmployeeEmail, "NOTICE", ue.HRME_Id, Employee.FirstOrDefault().HRMD_Id, data.UserId, data.communicationfalg, TransNo, data.communicationmsg, taskdetails, data, data.comp_Date);
                                }

                            }

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

        public string gettransactionno(ISM_PlannerReportsDTO data)
        {
            string TransNo = "";
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == data.MI_Id && t.ISMEMN_Type == data.communicationfalg && t.ASMAY_ID == data.ASMAY_Id).OrderByDescending(t => t.ISMEMN_ID).ToList();
            if (checktransno.Count > 0)
            {
                string[] Newtransno = checktransno.FirstOrDefault().ISMEMN_No.Split('/');
                var Clinet = Newtransno[0].ToString();
                var Type = Newtransno[1].ToString();
                var Year = Newtransno[2].ToString();
                var NewTransNo = Newtransno[3].ToString();
                TransNo = (Convert.ToInt32(NewTransNo) + 1).ToString();
                TransNo = (Clinet + "/" + Type + "/" + Year + "/" + TransNo).ToString();
            }
            else
            {
                var transano = _Context.Master_Numbering.Where(imp => imp.MI_Id == data.MI_Id && imp.IMN_Flag == data.communicationfalg).ToList();
                if (transano.Count > 0)
                {
                    var acyr = _Context.AcademicYear.Where(t => t.ASMAY_Id.Equals(data.ASMAY_Id)).FirstOrDefault();
                    string AcadYear = acyr.ASMAY_Year;

                    var Clinet = transano.FirstOrDefault().IMN_PrefixParticular;
                    var Type = data.communicationfalg.ToUpper();
                    var Year = AcadYear;
                    var NewTransNo = transano.FirstOrDefault().IMN_StartingNo;
                    //TransNo = (Convert.ToInt32(NewTransNo) + 1).ToString();
                    TransNo = (Clinet + "/" + Type + "/" + Year + "/" + NewTransNo).ToString();
                }
            }
            if (TransNo != "")
            {
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_MEMO_NOTICE_Outgoing";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                       SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_No",
                    SqlDbType.NVarChar)
                    {
                        Value = TransNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_Type",
                       SqlDbType.NVarChar)
                    {
                        Value = data.communicationfalg
                    });
                    if (data.ISMEMN_No != null && data.ISMEMN_No != "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@ISMEMN_Description",
                    SqlDbType.NVarChar)
                        {
                            Value = data.ISMEMN_No
                        });
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@ISMEMN_Description",
                      SqlDbType.NVarChar)
                        {
                            Value = "Planner Not Generated"
                        });

                    }
                    cmd.Parameters.Add(new SqlParameter("@ISMEMN_CreatedBy",
                       SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@INDDate",
                      SqlDbType.DateTime)
                    {
                        Value = indiantime0
                    });
                    cmd.Parameters.Add(new SqlParameter("@COMPDate",
                     SqlDbType.DateTime)
                    {
                        Value = data.comp_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@startdate",
                      SqlDbType.DateTime)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Enddate",
                     SqlDbType.DateTime)
                    {
                        Value = data.enddate
                    });

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
                        return ex.Message;
                    }
                }
            }

            return TransNo;
        }

        public string gettaskregisterReportnotice(ISM_PlannerReportsDTO data)
        {
            try
            {
                int rownumber = 0;
                string str = "";
                string str1 = "";
                string str2 = "";
                if (data.Deviationreporttype == "Dept")
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
                    long empuserid = 0;
                    var useridofemp = _IssueContext.Staff_User_Login.Where(t => t.Emp_Code == data.HRME_Id).ToList();
                    if (useridofemp.Count > 0)
                    {
                        empuserid = Convert.ToInt64(useridofemp.FirstOrDefault().Id);
                    }
                    List<ISM_PlannerReportsDTO> emplist = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
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
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@user_Id",
                 SqlDbType.BigInt)
                        {
                            Value = empuserid
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
                                    emplist.Add(new ISM_PlannerReportsDTO
                                    {
                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    string hrmeids = "0";
                    if (emplist.Count > 0)
                    {
                        for (int j = 0; j < emplist.Count; j++)
                        {
                            if (emplist.Count > 0)
                            {
                                foreach (var ue in emplist)
                                {
                                    hrmeids = hrmeids + "," + ue.HRME_Id;
                                }
                            }
                            else
                            {
                                if (hrmeids == "" || hrmeids == "0")
                                {
                                    hrmeids = data.HRME_Id.ToString();
                                }
                            }
                        }
                    }

                    List<ISM_PlannerReportsDTO> empdevlist = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "departmentWise_Dev_Report_Employee";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@StartDate",
                       SqlDbType.DateTime)
                        {
                            Value = data.startdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@EndDate",
                        SqlDbType.DateTime)
                        {
                            Value = data.enddate
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
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    empdevlist.Add(new ISM_PlannerReportsDTO
                                    {
                                        employeename = Convert.ToString(dataReader["employeename"]),
                                        deviation = Convert.ToString(dataReader["DevPer"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (empdevlist.Count > 0)
                    {
                        str += "<p> <span style=" + "'font-weight:bold'" + ">Developer Wise Deviation Lists :</span></p> <table width='100%' style=" + "'border-collapse: collapse; width: 100%;'" + " class=" + "'table1'" + "><tr>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > EMPLOYEE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STARTDATE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ENDDATE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > DEVIATION </th>" +
                                 "</tr>";
                        for (int j = 0; j < empdevlist.Count; j++)
                        {

                            if (Convert.ToDecimal(empdevlist[j].deviation) >= Convert.ToDecimal(data.NoOfDays))
                            {
                                rownumber += 1;
                                str1 += "<tr> " +
                               "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].employeename + "</td>" +
                               "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-family: sans-serif;'" + ">" + data.startdate.Value.ToShortDateString() + "</td>" +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + data.enddate.Value.ToShortDateString() + "</td> " +
                               "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;;font-weight:bold;font-family: sans-serif;'" + " >" + empdevlist[j].deviation + "%</td> " +
                               "</tr>";
                            }
                        }
                        str2 = "</table>";
                    }
                    data.message = str + str1 + str2;
                }
                else
                {
                    List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Peding_Memo_Task_List";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Memo",
                        SqlDbType.VarChar)
                        {
                            Value = data.ISMEMN_No
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
                                    dtonewtemp.Add(new ISM_PlannerReportsDTO
                                    {
                                        ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                        ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                        ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                        ISMTCR_Status = Convert.ToString(dataReader["ISMTCR_Status"]),
                                        AssignedBy = Convert.ToString(dataReader["AssignedBy"]),
                                        taskstartdate = Convert.ToString(dataReader["StartDate"]),
                                        taskatenddate = Convert.ToString(dataReader["EndDate"]),
                                        ISMTPLTA_Id = Convert.ToInt64(dataReader["ISMTPLTA_Id"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (dtonewtemp.Count > 0)
                    {
                        //Deviation: " + data.deviation + " %</ span > < span style = " + "'float: center;padding-left:5%;'" + " > Total Open Tasks Tilldate: " + dtonewtemp.Count + " </ span ></ span ></ p >
                        var stdate = Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy");
                        var eddate = Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy");
                        str += "<p> <span style=" + "'font-weight:bold'" + ">Pending Issue Lists :<span style=" + "'float: center;padding-left:10%;'" + "></span> </span></p> <table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK NO.</th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK NAME</th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > TASK TYPE </th>" +
                               "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STATUS </th>" +
                                "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ASSIGNEDBY </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > STARTDATE </th>" +
                                 "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > ENDDATE </th>" +
                                 "</tr>";
                        for (int j = 0; j < dtonewtemp.Count; j++)
                        {
                            rownumber += 1;
                            str1 += "<tr> " +
                                "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + dtonewtemp[j].ISMTCR_TaskNo + "</td>" +
                                "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + dtonewtemp[j].ISMTCR_Title + "</td>" +
                                "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].ISMTCR_BugOREnhancementFlg + "</td> " +
                                "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].ISMTCR_Status + "</td> " +
                                "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].AssignedBy + "</td> " +
                                "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].taskstartdate + "</td> " +
                                "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-family: sans-serif;'" + " >" + dtonewtemp[j].taskatenddate + "</td> " +
                                 "</tr>";
                        }
                        str2 = "</table>";

                    }
                    data.message = str + str1 + str2;

                    var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == data.MI_Id && t.ISMEMN_Type == data.communicationfalg && t.ASMAY_ID == data.ASMAY_Id).OrderByDescending(t => t.ISMEMN_ID).ToList();
                    long memonoticeid = 0;
                    if (checktransno.Count() > 0)
                    {
                        memonoticeid = checktransno.FirstOrDefault().ISMEMN_ID;
                    }
                    if (data.type == true)
                    {
                        if (dtonewtemp.Count() > 0)
                        {
                            foreach (var item in dtonewtemp)
                            {
                                ISM_EMPLOYEE_MEMO_NOTICE_TASKS res = new ISM_EMPLOYEE_MEMO_NOTICE_TASKS();

                                res.ISMTPLTA_Id = item.ISMTPLTA_Id;
                                res.ISMEMN_ID = memonoticeid;
                                res.Createddate = DateTime.Now;
                                res.Updateddate = DateTime.Now;
                                _IssueContext.Add(res);
                                _IssueContext.SaveChanges();
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.message;
        }

        public string sendmail(long MI_Id, string Email, string Template, long UserID, long HRMD_Id, long id, string flag, string transno, string msg, string taskdetails, ISM_PlannerReportsDTO data, DateTime? compdate)
        {
            try
            {
                long HRMEID = 0;
                string mailcc = "";
                string mailbcc = "";
                var Employee = (from a in _IssueContext.MasterEmployee
                                from b in _Context.Staff_User_Login
                                where a.HRME_Id == b.Emp_Code && b.Id == id
                                select new ISM_PlannerReportsDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    MI_Id = a.MI_Id
                                }).ToList();
                if (Employee.Count() > 0)
                {
                    HRMEID = Convert.ToInt64(Employee.FirstOrDefault().HRME_Id);
                }

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }
                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_MEMO_NOTICE_TEMPLET_PARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@EMP_HRMEID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                        //Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                       SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                        SqlDbType.VarChar)
                    {
                        Value = flag
                        //Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.VarChar)
                    {
                        Value = MI_Id
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

                val.Add("[MSG]", taskdetails);
                val.Add("[STARTDATE]", Convert.ToDateTime(data.startdate).ToString("dd/MM/yyyy"));
                val.Add("[ENDDATE]", Convert.ToDateTime(data.enddate).ToString("dd/MM/yyyy"));
                val.Add("[COMPDATE]", Convert.ToDateTime(compdate).ToString("dd/MM/yyyy"));

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;
                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
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
                    string emailccdetails = "";
                    string emailbccdetails = "";
                    var emaildetails = _Context.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == MI_Id && a.HRMD_Id == HRMD_Id).ToList();
                    if (emaildetails.Count > 0)
                    {
                        emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                        emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                    }
                    else
                    {
                        string heademail = "";
                        if (HRMD_Id != 0)
                        {
                            List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Fetch_DepartmentHead";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = HRMD_Id
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
                                            dtonewtemp.Add(new ISM_PlannerReportsDTO
                                            {
                                                Tomail = Convert.ToString(dataReader["Head"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (dtonewtemp.Count() > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    heademail = dtonewtemp[j].Tomail;
                                }
                            }
                        }

                        if (heademail != "")
                        {
                            emailccdetails = heademail + ",hr@vapstech.com,pstomd@vapstech.com";
                        }
                        else
                        {
                            emailccdetails = "hr@vapstech.com,pstomd@vapstech.com";
                        }
                        //emailbccdetails = "rakesh.reddy@vapstech.com";
                    }

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        if (Attechement.Equals("1"))
                        {
                            var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (data.Deviationreporttype == "Dept")
                        {
                            try
                            {
                                List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Fetch_MemoNoticeccmails";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                    SqlDbType.BigInt)
                                    {
                                        Value = 1
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
                                                dtonewtemp.Add(new ISM_PlannerReportsDTO
                                                {
                                                    MailCc = Convert.ToString(dataReader["cc"]),
                                                    MailBCc = Convert.ToString(dataReader["bcc"])
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                if (dtonewtemp.Count > 0)
                                {
                                    for (int j = 0; j < dtonewtemp.Count; j++)
                                    {
                                        mailcc = dtonewtemp[j].MailCc;
                                        mailbcc = dtonewtemp[j].MailBCc;
                                    }
                                    if (mailcc != null && mailcc != "")
                                    {
                                        string[] ccmaildetails = mailcc.Split(',');

                                        foreach (var c in ccmaildetails)
                                        {
                                            if (c != Email)
                                            {
                                                message.AddCc(c);
                                            }
                                        }
                                    }
                                    if (mailbcc != null && mailbcc != "")
                                    {
                                        string[] bccmaildetails = mailbcc.Split(',');

                                        foreach (var c in bccmaildetails)
                                        {
                                            if (c != Email)
                                            {
                                                message.AddBcc(c);
                                            }
                                        }
                                    }
                                }

                            }
                            catch (Exception error)
                            {
                                //
                            }
                        }
                        else
                        {
                            if (emailccdetails != null && emailccdetails != "")
                            {
                                string[] ccmaildetails = emailccdetails.Split(',');

                                foreach (var c in ccmaildetails)
                                {
                                    if (c != Email)
                                    {
                                        message.AddCc(c);
                                    }

                                }
                            }

                            if (emailbccdetails != null && emailbccdetails != "")
                            {
                                string[] bccmaildetails = emailbccdetails.Split(',');

                                foreach (var bc in bccmaildetails)
                                {
                                    if (bc != Email)
                                    {
                                        message.AddBcc(bc);
                                    }
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();

                        var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == MI_Id && t.ISMEMN_No == transno).ToList();
                        if (checktransno.Count() > 0)
                        {
                            var Resultupdate = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Single(a => a.ISMEMN_ID == checktransno.FirstOrDefault().ISMEMN_ID);
                            Resultupdate.ISMEMN_EmailSentFlag = true;
                            _IssueContext.Update(Resultupdate);
                            _IssueContext.SaveChanges();
                        }

                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            if (modulename.Count == 0)
                            {
                                modulename[0] = "Issue Manager";
                            }

                            cmd.CommandText = "IVRM_Email_Outgoing";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmailId",
                                SqlDbType.NVarChar)
                            {
                                Value = Email
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message",
                               SqlDbType.NVarChar)
                            {
                                Value = data.communicationfalg + " - " + transno + " - " + data.employeename
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
                                return ex.Message;
                            }
                        }
                    }
                    else
                    {

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
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(SendingEmail, institutionName[0].MI_Name);
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


                                if (data.Deviationreporttype == "Dept")
                                {
                                    //emailMessage.CC.Add("siddesh@vapstech.com");
                                    //emailMessage.CC.Add("pstomd@vapstech.com");
                                    //emailMessage.CC.Add("hr@vapstech.com");
                                }
                                else
                                {
                                    if (emailccdetails != null && emailccdetails != "")
                                    {
                                        string[] ccmaildetails = emailccdetails.Split(',');

                                        foreach (var c in ccmaildetails)
                                        {
                                            if (c != Email)
                                            {
                                                emailMessage.CC.Add(c);
                                            }
                                        }
                                    }

                                    if (emailbccdetails != null && emailbccdetails != "")
                                    {
                                        string[] bccmaildetails = emailbccdetails.Split(',');

                                        foreach (var bc in bccmaildetails)
                                        {
                                            if (bc != Email)
                                            {
                                                emailMessage.Bcc.Add(bc);
                                            }
                                        }
                                    }
                                }

                                clientsmtp.Send(emailMessage);

                                var checktransno = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Where(t => t.MI_ID == MI_Id && t.ISMEMN_No == transno).ToList();
                                if (checktransno.Count() > 0)
                                {
                                    var Resultupdate = _IssueContext.ISM_EMPLOYEE_MEMO_NOTICE.Single(a => a.ISMEMN_ID == checktransno.FirstOrDefault().ISMEMN_ID);
                                    Resultupdate.ISMEMN_EmailSentFlag = true;
                                    _IssueContext.Update(Resultupdate);
                                    _IssueContext.SaveChanges();
                                }
                                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                {
                                    var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template).Select(d => d.IVRMIM_Id).ToList();

                                    var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                                    var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                    if (modulename.Count == 0)
                                    {
                                        modulename[0] = "Issue Manager";
                                    }

                                    cmd.CommandText = "IVRM_Email_Outgoing";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@EmailId",
                                        SqlDbType.NVarChar)
                                    {
                                        Value = Email
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Message",
                                       SqlDbType.NVarChar)
                                    {
                                        Value = data.communicationfalg + " - " + transno + " - " + data.employeename
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
                                        return ex.Message;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public ISM_PlannerReportsDTO startenddays(ISM_PlannerReportsDTO data)
        {
            try
            {
                string day = "";
                List<ISM_PlannerReportsDTO> empdevlist = new List<ISM_PlannerReportsDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "START_END_DAY_OF_WEEK";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
            SqlDbType.VarChar)
                    {
                        Value = 1
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
                                empdevlist.Add(new ISM_PlannerReportsDTO
                                {
                                    startdate = Convert.ToDateTime(dataReader["START_OF_WEEK"]),
                                    enddate = Convert.ToDateTime(dataReader["END_OF_WEEK"]),
                                    message = Convert.ToString(dataReader["START_DAY_OF_WEEK"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (empdevlist.Count() > 0)
                {
                    for (int j = 0; j < empdevlist.Count; j++)
                    {
                        data.startdate = empdevlist[j].startdate;
                        data.enddate = empdevlist[j].enddate;
                        data.message = empdevlist[j].message;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public long completebydays(long hrmd_id, string flag)
        {
            long days = 0;
            try
            {
                List<ISM_PlannerReportsDTO> empdevlist = new List<ISM_PlannerReportsDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MEMO_NOTICE_COMPLETEBY_DATE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRMD_ID",
            SqlDbType.BigInt)
                    {
                        Value = hrmd_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
            SqlDbType.VarChar)
                    {
                        Value = flag
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
                                empdevlist.Add(new ISM_PlannerReportsDTO
                                {
                                    day_count = Convert.ToInt64(dataReader["Days"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (empdevlist.Count() > 0)
                {
                    days = empdevlist[0].day_count;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return days;
        }
    }
}
