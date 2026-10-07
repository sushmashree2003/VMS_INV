using CommonLibrary;
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
    public class ISM_PlannerListImpl : Interfaces.ISM_PlannerListInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISM_PlannerListImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<ISM_PlannerCreationDTO> getdetails(ISM_PlannerCreationDTO data)
        {
            try
            {
                List<long?> department_ids = new List<long?>();
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;

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
                data.get_Status = _IssueContext.MasterStatus_DMO.Where(t => t.MI_Id == data.MI_Id && department_ids.Contains(t.HRMD_Id) && t.ISMMISTS_ActiveFlag == true).Distinct().OrderBy(t => t.ISMMISTS_Order).ToArray();
                //================ Assigned Task List
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Planner_TaskList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
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
                        data.get_Assignedtasklist = retObject.ToArray();
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

        public async Task<ISM_PlannerCreationDTO> gettaskdetails(ISM_PlannerCreationDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Complaint_TaskDeatils";
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
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                if (data.addTaskArray != null)
                {
                    foreach (var pa in data.addTaskArray)
                    {
                        ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                        taskasign.ISMTCR_Id = pa.ISMTCR_Id;
                        taskasign.HRME_Id = empid;
                        taskasign.ISMTCRASTO_AssignedDate = data.ISMTCRASTO_AssignedDate;
                        taskasign.ISMTCRASTO_AssignedBy = empid;
                        taskasign.ISMTCRASTO_Remarks = pa.ISMTCRASTO_Remarks;
                        taskasign.ISMTCRASTO_StartDate = pa.ISMTCRASTO_StartDate;
                        taskasign.ISMTCRASTO_EndDate = pa.ISMTCRASTO_EndDate;
                        taskasign.ISMTCRASTO_EffortInHrs = pa.ISMTCRASTO_EffortInHrs;
                        taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                        taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                        taskasign.ISMTCRASTO_ActiveFlg = true;
                        taskasign.CreatedDate = DateTime.Now;
                        taskasign.UpdatedDate = DateTime.Now;
                        _IssueContext.Add(taskasign);
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_PlannerCreationDTO savePlanner(ISM_PlannerCreationDTO data)
        {
            try
            {
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                ISM_Task_PlannerDMO plnr = new ISM_Task_PlannerDMO();
                plnr.MI_Id = data.MI_Id;
                plnr.HRME_Id = empid;
                plnr.ISMTPL_PlannedBy = empid;
                plnr.ISMTPL_PlannerName = data.ISMTPL_PlannerName;
                plnr.ISMTPL_Remarks = data.ISMTPL_Remarks;
                plnr.ISMTPL_StartDate = data.ISMTPL_StartDate;
                plnr.ISMTPL_EndDate = data.ISMTPL_EndDate;
                plnr.ISMTPL_TotalHrs = data.ISMTPL_TotalHrs;
                plnr.ISMTPL_ApprovalFlg = false;
                plnr.ISMTPL_ActiveFlg = true;
                plnr.ISMTPL_CreatedBy = data.UserId;
                plnr.ISMTPL_UpdatedBy = data.UserId;
                plnr.CreatedDate = DateTime.Now;
                plnr.UpdatedDate = DateTime.Now;
                _IssueContext.Add(plnr);

                if (data.taskplannerArray != null)
                {
                    foreach (var pl in data.taskplannerArray)
                    {
                        ISM_Task_Planner_TasksDMO plnrtask = new ISM_Task_Planner_TasksDMO();
                        plnrtask.ISMTPL_Id = plnr.ISMTPL_Id;
                        plnrtask.ISMTCR_Id = pl.ISMTCR_Id;
                        plnrtask.ISMTPLTA_Remarks = pl.ISMTPLTA_Remarks;
                        plnrtask.ISMTPLTA_StartDate = pl.ISMTPLTA_StartDate;
                        plnrtask.ISMTPLTA_EndDate = pl.ISMTPLTA_EndDate;
                        plnrtask.ISMTPLTA_EffortInHrs = pl.ISMTPLTA_EffortInHrs;
                        plnrtask.ISMTPLTA_StartDate = pl.ISMTPLTA_StartDate;
                        plnrtask.ISMTPLTA_EndDate = pl.ISMTPLTA_EndDate;
                        plnrtask.ISMTPLTA_EffortInHrs = pl.ISMTPLTA_EffortInHrs;
                        plnrtask.ISMTPLTA_Status = "Open";
                        plnrtask.ISMTPLTA_DeviationFlg = false;
                        plnrtask.ISMTPLTA_ExtraFlg = false;
                        //plnrtask.ISMTPLAPTA_RejectedFlg = false;
                        //plnrtask.ISMTPLAPTA_UnPlannedFlg = false;
                        plnrtask.ISMTPLTA_ActiveFlg = true;
                        plnrtask.ISMTPLTA_CreatedBy = data.UserId;
                        plnrtask.ISMTPLTA_UpdatedBy = data.UserId;
                        plnrtask.CreatedDate = DateTime.Now;
                        plnrtask.UpdatedDate = DateTime.Now;
                        _IssueContext.Add(plnrtask);
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
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

   

    }
}
