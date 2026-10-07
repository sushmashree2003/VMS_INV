using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_ImplTaskMappingImpl : Interfaces.ISM_ImplTaskMappingInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_ImplTaskMappingImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_ImplTaskMappingDTO> getdetails(ISM_ImplTaskMappingDTO data)
        {
            try
            {

                //*********************************** Implimentation Planner  List ********************************************//
                data.implplannerlist = _IssueContext.ISM_Master_Implementation_PlannerDMO.Where(e => e.ISMMIMPPL_ActiveFlag == true).Distinct().ToArray();
                
                
                //*********************************** Implimentation Planner List ********************************************//
                //*********************************** Employee List ********************************************// 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_EmployeeList_impl";
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
                        data.get_employeelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //*********************************** User Employee Mapped List ********************************// 
          //      using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
          //      {
          //          cmd.CommandText = "ISM_User_EmployeeList";
          //          cmd.CommandType = CommandType.StoredProcedure;

          //          cmd.Parameters.Add(new SqlParameter("@user_Id",
          //   SqlDbType.BigInt)
          //          {
          //              Value = data.UserId
          //          });
          //          cmd.Parameters.Add(new SqlParameter("@MI_Id",
          //SqlDbType.BigInt)
          //          {
          //              Value = data.MI_Id
          //          });
          //          if (cmd.Connection.State != ConnectionState.Open)
          //              cmd.Connection.Open();

          //          var retObject = new List<dynamic>();
          //          try
          //          {
          //              using (var dataReader = await cmd.ExecuteReaderAsync())
          //              {
          //                  while (await dataReader.ReadAsync())
          //                  {
          //                      var dataRow = new ExpandoObject() as IDictionary<string, object>;
          //                      for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
          //                      {
          //                          dataRow.Add(
          //                              dataReader.GetName(iFiled),
          //                             dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
          //                          );
          //                      }
          //                      retObject.Add((ExpandoObject)dataRow);
          //                  }
          //              }
          //              data.get_userEmplist = retObject.ToArray();
          //          }
          //          catch (Exception ex)
          //          {
          //              Console.WriteLine(ex.Message);
          //          }
          //      }

                           
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<ISM_ImplTaskMappingDTO> deptfilter(ISM_ImplTaskMappingDTO data)
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
                        cmd.CommandText = "ISM_EmployeeList";
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

        public async Task<ISM_ImplTaskMappingDTO> getsearchtask(ISM_ImplTaskMappingDTO data)
        {
            try
            {
                var mi_id = _IssueContext.MasterEmployee.Single(e => e.HRME_Id == data.HRME_Id).MI_Id;
                var dept_id = _IssueContext.MasterEmployee.Single(e => e.HRME_Id == data.HRME_Id).HRMD_Id;

                

                //*********************************** Task List ************************************************//           
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_IMPLEMENTATIONPLANNERLIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMMIMPPL_Id",
           SqlDbType.VarChar)
                    {
                        Value = data.ISMMIMPPL_Id
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


                var catlist = (from a in _IssueContext.ISM_Master_TaskCategory_DMO
                               from b in _IssueContext.ISM_Master_TaskCategory_Employee_DMO
                               where a.ISMMTCAT_Id == b.ISMMTCAT_Id && a.ISMMTCAT_ActiveFlag == true && b.ISMMTCATE_ActiveFlag == true
                               && b.HRME_Id == data.HRME_Id && a.MI_Id== mi_id && a.HRMD_Id== dept_id
                               select a
                             ).Distinct().ToList();
                if (catlist.Count>0)
                {
                    data.categorylist = catlist.ToArray();
                }
                else
                {
                    data.categorylist = _IssueContext.ISM_Master_TaskCategory_DMO.Where(e => e.ISMMTCAT_ActiveFlag == true && e.MI_Id== mi_id && e.HRMD_Id== dept_id).Distinct().ToArray();
                }

                data.projectlist = _IssueContext.MastersProject_DMO.Where(w => w.ISMMPR_ActiveFlg == true && w.MI_Id== mi_id)
                    .Distinct().OrderBy(e => e.ISMMPR_ProjectName).ToArray();

                data.clientlist = _IssueContext.clientTable.Where(w => w.ISMMCLT_ActiveFlag == true && w.MI_Id == mi_id)
                    .Distinct().OrderBy(e => e.ISMMCLT_ClientName).ToArray();
                data.get_priority = _IssueContext.MastersPriorities_DMO.Where(e => e.HRMP_ActiveFlag == true && e.MI_Id==mi_id).Distinct().ToArray();

                data.modulelist = _IssueContext.MasterModules.Where(e => e.Module_ActiveFlag == 1).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_ImplTaskMappingDTO> getcount(ISM_ImplTaskMappingDTO data)
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

                //*********************************** Pending By & To Me task Count ************************************************//           
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ISM_ImplTaskMappingDTO> gettaskdetails(ISM_ImplTaskMappingDTO data)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_ImplTaskMappingDTO savePlanner(ISM_ImplTaskMappingDTO data)
        {
            try
            {
               
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                    long empid = 0;
                    if (hrmEempid.Length > 0)
                    {
                        empid = hrmEempid.FirstOrDefault().Emp_Code;
                    }
                    var userrole = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                    string role = "";
                    if (userrole.Count > 0)
                    {
                        role = userrole.FirstOrDefault().IVRMRT_Role;
                        data.roletype = role;
                    }
                var mi_id = _IssueContext.MasterEmployee.Single(e => e.HRME_Id == data.HRME_Id).MI_Id;
                var dept_id = _IssueContext.MasterEmployee.Single(e => e.HRME_Id == data.HRME_Id).HRMD_Id;

            

                if (data.implplannerarr.Length > 0)
                    {
                        foreach (var item in data.implplannerarr)
                        {

                            Master_NumberingDTO check = new Master_NumberingDTO();
                            data.transnumbconfigurationsettingsss = check;
                            List<Master_Numbering> MM = new List<Master_Numbering>();
                            MM = _Context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "ISMTASK").ToList();
                            if (MM.Count() > 0)
                            {
                                data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                                data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                                data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                                data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                                data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                                data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                                data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                                data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                                data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                                data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                                data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                                data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                                data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                                data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                                data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                                data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                                data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                            }

                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Context);
                                data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }


                            ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                            task.ISMTCR_TaskNo = data.Auto_id;
                            task.MI_Id = mi_id;
                        task.HRMD_Id = Convert.ToInt64(dept_id);
                            task.ISMMPR_Id = item.ISMMPR_Id;
                            task.IVRMM_Id = item.IVRMM_Id;
                            task.ISMTCR_BugOREnhancementFlg = item.ISMTCR_BugOREnhancementFlg;
                            task.ISMTCR_CreationDate = indianTime;
                            task.ISMTCR_Title = item.ISMIMPPL_ActivityName;
                            task.HRMPR_Id = item.HRMPR_Id;
                            task.HRME_Id = empid;
                            task.ISMTCR_Desc = item.ISMIMPPL_Remarks;
                            task.ISMTCR_Status = "Open";
                            task.ISMMTCAT_Id = item.ISMMTCAT_Id;
                            task.ISMMCLT_Id = item.ISMMCLT_Id;
                            task.ISMTCR_ReOpenFlg = false;
                            task.ISMTCR_ActiveFlg = true;
                            task.ISMTCR_CreatedBy = data.UserId;
                            task.ISMTCR_UpdatedBy = data.UserId;
                            task.CreatedDate = indianTime;
                            task.UpdatedDate = indianTime;
                            _IssueContext.Add(task);
                        //if (item.ISMMCLT_Id > 0)
                        //{
                        //    ISM_TaskCreation_ClientDMO taskclt = new ISM_TaskCreation_ClientDMO();
                        //    taskclt.ISMTCR_Id = task.ISMTCR_Id;
                        //    taskclt.ISMMCLT_Id = item.ISMMCLT_Id;
                        //    taskclt.ISMTCRCL_ActiveFlg = true;
                        //    taskclt.ISMTCRCL_CreatedBy = data.UserId;
                        //    taskclt.ISMTCRCL_UpdatedBy = data.UserId;
                        //    taskclt.CreatedDate = indianTime;
                        //    taskclt.UpdatedDate = indianTime;
                        //    _IssueContext.Add(taskclt);
                        //}
                        decimal ttm = 0;
                        if (item.ISMIMPPL_TimeRequiredFlg.ToLower()=="mins")
                        {
                            ttm = item.ISMIMPPL_TimeRequired / 60;

                        }
                     else   if (item.ISMIMPPL_TimeRequiredFlg.ToLower() == "days")
                        {
                            ttm = item.ISMIMPPL_TimeRequired * 8;

                        }
                        else
                        {
                            ttm = item.ISMIMPPL_TimeRequired;
                        }


                        ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();

                        taskasign.ISMTCR_Id = task.ISMTCR_Id;
                        taskasign.HRME_Id = data.HRME_Id;
                        taskasign.ISMTCRASTO_AssignedDate = indianTime;
                        taskasign.ISMTCRASTO_AssignedBy = empid;
                        taskasign.ISMTCRASTO_Remarks = item.ISMIMPPL_Remarks;
                        taskasign.ISMTCRASTO_StartDate = item.ismtcrastO_StartDate;
                        taskasign.ISMTCRASTO_EndDate = item.ismtcrastO_EndDate;
                        taskasign.ISMTCRASTO_EffortInHrs = ttm;
                        taskasign.ISMTCRASTO_ActiveFlg = true;
                        taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                        taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                        _IssueContext.Add(taskasign);

                        ISM_Task_Advance_PlannerDMO aplan = new ISM_Task_Advance_PlannerDMO();
                        aplan.ISMTCR_Id = task.ISMTCR_Id;
                        aplan.MI_Id = mi_id;
                        aplan.HRME_Id = data.HRME_Id;
                        aplan.ISMTAPL_Periodicity = item.ISMIMPPL_Periodicity;
                        aplan.ISMTAPL_Remarks = item.ISMIMPPL_Remarks;
                        aplan.ISMTAPL_ActiveFlg = true;
                        aplan.ISMTAPL_Year = data.ASMAY_Id;
                        aplan.ISMTAPL_FromDate = item.ismtcrastO_StartDate;
                        aplan.ISMTAPL_ToDate = item.ismtcrastO_EndDate;
                        aplan.HRME_Id = empid;
                        aplan.ISMTAPL_TimeRequired = item.ISMIMPPL_TimeRequired;
                        aplan.ISMTAPL_OFFDate = item.ISMTAPL_OFFDate;
                        aplan.ISMTAPL_Day = item.ISMTAPL_Day;
                        aplan.ISMTAPL_TimeRequiredFlg = item.ISMIMPPL_TimeRequiredFlg;
                        aplan.CreatedDate = indianTime;
                        aplan.UpdatedDate = indianTime;
                        aplan.ISMTAPL_CreatedBy = data.UserId;
                        aplan.ISMTAPL_UpdatedBy = data.UserId;
                        _IssueContext.Add(aplan);

                        ISM_Task_Advance_Planner_DatesDMO dobj = new ISM_Task_Advance_Planner_DatesDMO();
                        dobj.ISMTAPL_Id = aplan.ISMTAPL_Id;
                        dobj.ISMTAPLD_StartDate=item.ismtcrastO_StartDate;
                        dobj.ISMTAPLD_EndDate = item.ismtcrastO_EndDate;
                        dobj.ISMTAPLD_ActiveFlg = true;
                        dobj.ISMTAPLD_CreatedDate = indianTime;
                        dobj.ISMTAPLD_UpdatedDate = indianTime;
                        dobj.ISMTAPLD_CreatedBy =data.UserId;
                        dobj.ISMTAPLD_UpdatedBy = data.UserId;
                        _IssueContext.Add(dobj);
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
                


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_ImplTaskMappingDTO saveTransfer(ISM_ImplTaskMappingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

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
                        foreach (var id in data.hrme_list)
                        {
                            var checktasktransfer = (from a in _IssueContext.ISM_TaskCreationDMO
                                                     from b in _IssueContext.ISM_TaskCreation_TransferredToDMO
                                                     where (a.ISMTCR_Id == b.ISMTCR_Id && a.ISMTCR_ActiveFlg == true && b.ISMTCRTRTO_ActiveFlg == true && b.ISMTCR_Id == ta.ISMTCR_Id)
                                                     select new ISM_ImplTaskMappingDTO
                                                     {
                                                         ISMTCR_Id = b.ISMTCR_Id
                                                     }).Distinct().ToList();

                            if (checktasktransfer.Count() > 0)
                            {
                                var checktasktransferresult = _IssueContext.ISM_TaskCreation_TransferredToDMO.Single(a => a.ISMTCR_Id == ta.ISMTCR_Id);
                                checktasktransferresult.ISMTCRTRTO_TransferredBy = empid;

                                checktasktransferresult.HRME_Id = id.HRME_Id;

                                //checktasktransferresult.HRME_Id = ta.HRME_Id;
                                checktasktransferresult.UpdatedDate = DateTime.UtcNow;
                                _IssueContext.Update(checktasktransferresult);
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
                            }
                            //   var result = _IssueContext.ISM_TaskCreationDMO.Where(a => a.ISMTCR_Id == ta.ISMTCR_Id).ToList();
                            //long dept_id = 0;
                            //var hrmdid = (from a in _IssueContext.HR_Master_Department
                            //              from b in _IssueContext.MasterEmployee
                            //              where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && b.HRME_Id == ta.HRME_Id)
                            //              select new ISM_ImplTaskMappingDTO
                            //              {
                            //                  HRMD_Id = a.HRMD_Id
                            //              }).Distinct().ToList();
                            //if (hrmdid.Count > 0)
                            //{
                            //    dept_id = hrmdid.FirstOrDefault().HRMD_Id;
                            //}

                            //foreach (var r in result)
                            //{
                            //    var res11 = _IssueContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == r.ISMTCR_Id);
                            //    res11.HRMD_Id = dept_id;
                            //    res11.HRME_Id = ta.HRME_Id;                          
                            //    res11.UpdatedDate = DateTime.UtcNow;;
                            //    _IssueContext.Update(res11);
                            //}
                        }
                    }
                }

                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnval = true;
                    // ============================================== Push Notification Transfer
                    long taskid = 0;
                    List<long> transferto = new List<long>();
                    foreach (var at in data.transferArray)
                    {
                        transferto.Add(at.HRME_Id);
                        taskid = at.ISMTCR_Id;
                    }
                    var deviceidsnew = "";
                    var devicenew = "";
                    var devicelist = (from a in _IssueContext.MasterEmployee
                                      where (a.HRME_ActiveFlag == true && transferto.Contains(a.HRME_Id))
                                      select new ISM_ImplTaskMappingDTO
                                      {
                                          HRME_To = a.HRME_Id,
                                          HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                      }).Distinct().ToList();
                    data.HRME_To = devicelist.FirstOrDefault().HRME_To;
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
                    try
                    {
                        string titletext = "New Task Transferred";
                        string tasktype = "Transferred";
                        if (data.sound == "")
                        {
                            data.sound = "default";
                        }
                        callnotification(devicenew, data.HRME_To, taskid, titletext, data.MI_Id, data.sound);
                        Insertnotification(data.MI_Id, data.HRME_To, taskid, titletext, tasktype, data.UserId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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
        public ISM_ImplTaskMappingDTO saveResponse(ISM_ImplTaskMappingDTO data)
        {
            try
            {
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
                        taskresponse.ISMTCRRES_ResponseDate = data.ISMTCRRES_ResponseDate;
                        taskresponse.ISMTCRRES_Response = data.ISMTCRRES_Response;
                        taskresponse.ISMTCRRES_ResponseAttachment = taskatt;
                        taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                        taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
                        taskresponse.ISMTCRRES_ActiveFlg = true;
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
                    taskresponse.ISMTCRRES_ResponseDate = data.ISMTCRRES_ResponseDate;
                    taskresponse.ISMTCRRES_Response = data.ISMTCRRES_Response;
                    taskresponse.ISMTCRRES_ResponseAttachment = "";
                    taskresponse.ISMTCRRES_CreatedBy = data.UserId;
                    taskresponse.ISMTCRRES_UpdatedBy = data.UserId;
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
                _IssueContext.Update(taskesult);

                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
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
                    data.HRME_To = devicelist.FirstOrDefault().HRME_To;
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
                    try
                    {
                        string titletext = "Task Response";
                        string tasktype = "Response";
                        if (data.sound == "")
                        {
                            data.sound = "default";
                        }
                        callnotification(devicenew, response_id, data.ISMTCR_Id, titletext, data.MI_Id, data.sound);
                        Insertnotification(data.MI_Id, response_id, data.ISMTCR_Id, titletext, tasktype, data.UserId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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
        public string callnotification(string devicenew, long empid, long task_id, string titletext, long mi_id, string sound)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                //     "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + indianTime + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + indianTime + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + indianTime + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

                notificationparams.Add(daata.ToString());
                // var mycontent = JsonConvert.SerializeObject(notificationparams);
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
                return ex.Message;
            }
            return "success";
        }
        public string Insertnotification(long MI_Id, long empid, long task_id, string titletext, string tasktype, long userid)
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
                return ex.Message;
            }
            return "success";
        }

        public ISM_ImplTaskMappingDTO deactive(ISM_ImplTaskMappingDTO data)
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
        public ISM_ImplTaskMappingDTO gettaskAttachment(ISM_ImplTaskMappingDTO data)
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


    }
}
