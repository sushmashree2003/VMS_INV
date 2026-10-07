using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveCreditImpl : LeaveCreditInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveCreditImpl> _log;

        public LMContext _lmContext;
        public LeaveCreditImpl(LMContext ttcategory, DomainModelMsSqlServerContext db)
        {
            _lmContext = ttcategory;
            _db = db;
        }

        public LeaveCreditDTO getleave(LeaveCreditDTO data)
        {
            List<HR_Master_GroupType_DMO> staf_types = new List<HR_Master_GroupType_DMO>();
            staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList(); //
            data.stf_types = staf_types.Distinct().ToArray();

            List<HR_Master_Department_DMO> Department_types = new List<HR_Master_Department_DMO>();
            Department_types = _lmContext.HR_Master_Department_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList(); //
            data.Department_types = Department_types.Distinct().ToArray();


            List<HR_Master_Designation_DMO> Designation_types = new List<HR_Master_Designation_DMO>();
            Designation_types = _lmContext.HR_Master_Designation_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToList(); //
            data.Designation_types = Designation_types.Distinct().ToArray();

            List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
            leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveCreditFlg == true).ToList(); //
            data.leave_name = leave_name.Distinct().ToArray();

            List<HR_Master_Grade_DMO> gradename = new List<HR_Master_Grade_DMO>();
            gradename = _lmContext.HR_Master_Grade_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMG_ActiveFlag == true).ToList(); //
            data.grade_name = gradename.Distinct().ToArray();

            List<IVRM_Month_DMO> credit_month = new List<IVRM_Month_DMO>();
            credit_month = _lmContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToList(); //
            data.credit_month = credit_month.Distinct().ToArray();

            data.result = (from c in _lmContext.HR_Master_Leave_Details_DMO
                           from b in _lmContext.HR_Master_Leave_DMO
                           from d in _lmContext.HR_Master_Designation_DMO
                           from e in _lmContext.HR_Master_Department_DMO
                           from f in _lmContext.HR_Master_GroupType_DMO
                           where (c.MI_Id == data.MI_Id && f.HRMGT_Id == c.HRMGT_Id && c.HRMD_Id == e.HRMD_Id && d.HRMDES_Id == c.HRMDES_Id && b.MI_Id == data.MI_Id && b.HRML_Id == c.HRML_Id && c.HRMDES_Id == d.HRMDES_Id && d.MI_Id == data.MI_Id && e.MI_Id == data.MI_Id)
                           select new LeaveCreditDTO
                           {
                               HRMLD_Id = c.HRMLD_Id,
                               HRMLD_NoOfDays = c.HRMLD_NoOfDays,
                               HRMLD_MaxLeaveApplicable = c.HRMLD_MaxLeaveApplicable,
                               HRMDES_DesignationName = d.HRMDES_DesignationName,
                               HRMD_DepartmentName = e.HRMD_DepartmentName,
                               HRML_LeaveName = b.HRML_LeaveName,
                               HRMLD_CarryForFlg = c.HRMLD_CarryForFlg,
                               HRMLD_EncashFlg = c.HRMLD_EncashFlg,
                               HRMGT_EmployeeGroupType = f.HRMGT_EmployeeGroupType
                           }).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO get_leavecode(LeaveCreditDTO data)
        {
            data.leave_code = (from a in _lmContext.HR_Master_Leave_DMO
                               where (a.HRML_Id == data.leavecode)
                               select new LeaveCreditDTO
                               {
                                   HRML_Id = a.HRML_Id,
                                   HRML_LeaveCode = a.HRML_LeaveCode,
                               }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO viewmonthwiseleave(LeaveCreditDTO data)
        {

            data.edit_lvblnce = (from a in _lmContext.HR_Master_Leave_Details_DMO
                                 from b in _lmContext.HR_Master_Leave_Details_CreditMonth_DMO
                                 from c in _lmContext.IVRM_Month_DMO
                                 where (a.HRMLD_Id == b.HRMLD_Id && a.HRMLD_Id == data.HRMLD_Id && b.HRMLDCM_LCMonthCode == c.IVRM_Month_Id)
                                 select new LeaveCreditDTO
                                 {
                                     HRML_Id = a.HRML_Id,
                                     HRMLDCM_NoOfLeaves = b.HRMLDCM_NoOfLeaves,
                                     IVRM_Month_Name = c.IVRM_Month_Name
                                 }
                     ).Distinct().ToArray();
            return data;

        }
        public LeaveCreditDTO get_departments(LeaveCreditDTO data)
        {
            List<long> selected_emp_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_emp_types.Add(itm.HRMGT_Id);
            }

            //data.Department_types = (from a in _lmContext.HR_Master_Employee_DMO
            //                         from b in _lmContext.HR_Master_Department_DMO
            //                         from c in _lmContext.HR_Master_GroupType_DMO
            //                         where (a.HRMD_Id == b.HRMD_Id && a.HRMGT_Id == c.HRMGT_Id && c.HRMGT_ActiveFlag == true
            //                              && b.HRMD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id
            //                              && selected_emp_types.Contains(c.HRMGT_Id))
            //                         select new LeaveCreditDTO
            //                         {
            //                             HRMD_Id = b.HRMD_Id,
            //                             HRMD_DepartmentName = b.HRMD_DepartmentName,
            //                         }
            //         ).Distinct().ToArray();




            data.Department_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                     from b in _lmContext.HR_Master_Department_DMO
                                     where (a.HRMD_Id == b.HRMD_Id
                                          && b.HRMD_ActiveFlag == true && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id
                                          && selected_emp_types.Contains(a.HRMGT_Id))
                                     select new LeaveCreditDTO
                                     {
                                         HRMD_Id = b.HRMD_Id,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName,
                                     }
                     ).Distinct().ToArray();

            return data;



        }
        public LeaveCreditDTO get_designation(LeaveCreditDTO data)
        {
            List<long?> selected_desg_types = new List<long?>();

            foreach (var itm in data.emptypes)
            {
                selected_desg_types.Add(itm.HRMD_Id);
            }

            data.Designation_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                      from b in _lmContext.HR_Master_Designation_DMO
                                      where (a.HRMDES_Id == b.HRMDES_Id && b.HRMDES_ActiveFlag == true
                                       && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && selected_desg_types.Contains(a.HRMD_Id))
                                      select new LeaveCreditDTO
                                      {
                                          HRMDES_Id = b.HRMDES_Id,
                                          HRMDES_DesignationName = b.HRMDES_DesignationName,
                                      }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO get_grade(LeaveCreditDTO data)
        {
            List<long> selected_grade_types = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_grade_types.Add(itm.HRMG_Id);
            }

            data.grade_name = (from a in _lmContext.HR_Master_Employee_DMO
                               from b in _lmContext.HR_Master_Designation_DMO
                               from c in _lmContext.HR_Master_Department_DMO
                               from d in _lmContext.HR_Master_Grade_DMO
                               where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && a.HRMG_Id == d.HRMG_Id && c.HRMD_ActiveFlag == true
                               && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && d.HRMG_ActiveFlag == true
                                && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && selected_grade_types.Contains(d.HRMG_Id))
                               select new LeaveCreditDTO
                               {
                                   HRMG_Id = d.HRMG_Id,
                                   HRMG_GradeName = d.HRMG_GradeName,
                               }
                     ).Distinct().ToArray();

            return data;
        }
        public LeaveCreditDTO SaveData(LeaveCreditDTO data)
        {
            HR_Master_Leave_Details_DMO objpge = Mapper.Map<HR_Master_Leave_Details_DMO>(data);
            try
            {

                if (data.HRMLD_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_DMO.Single(t => t.MI_Id == data.MI_Id && t.HRML_Id == data.HRML_Id);
                    // data.HRMLD_Id = result.HRMLD_Id;
                    data.HRMLD_NoOfDays = result.HRMLD_NoOfDays;
                    data.HRMLD_MaxLeaveApplicable = result.HRMLD_MaxLeaveApplicable;
                    data.UpdatedDate = result.CreatedDate;
                    data.CreatedDate = result.CreatedDate;
                    result.HRMLD_UpdatedBy = data.UserId;
                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }
                else
                {

                    for (int i = 0; i < data.Emp_types.Length; i++)
                    {
                        for (int j = 0; j < data.dept_types.Length; j++)
                        {
                            for (int k = 0; k < data.desig_types.Length; k++)
                            {
                                var result = _lmContext.HRGroupDeptDessgDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMGT_Id == data.Emp_types[i].HRMGT_Id &&
                                a.HRMD_Id == data.dept_types[j].HRMD_Id && a.HRMDES_Id == data.desig_types[k].HRMDES_Id).ToList();

                                if (result.Count > 0)
                                {

                                    LeaveCreditDTO ttlldto = new LeaveCreditDTO();
                                    HR_Master_Leave_Details_DMO MM = Mapper.Map<HR_Master_Leave_Details_DMO>(ttlldto);
                                    MM.HRML_Id = data.HRML_Id;
                                    MM.MI_Id = data.MI_Id;
                                    MM.HRMLD_NoOfDays = data.HRMLD_NoOfDays;
                                    MM.HRMLD_MaxLeaveApplicable = data.HRMLD_MaxLeaveApplicable;
                                    MM.CreatedDate = DateTime.Now;
                                    MM.UpdatedDate = DateTime.Now;
                                    MM.HRMLD_CreatedBy = data.UserId;
                                    MM.HRMLD_UpdatedBy = data.UserId;
                                    MM.HRMGT_Id = Convert.ToInt64(data.Emp_types[i].HRMGT_Id);
                                    MM.HRMD_Id = Convert.ToInt64(data.dept_types[j].HRMD_Id);
                                    MM.HRMDES_Id = Convert.ToInt64(data.desig_types[k].HRMDES_Id);
                                    // MM.HRMG_Id = Convert.ToInt64(data.grade_types[l].HRMG_Id);
                                    MM.HRMLD_CarryForFlg = data.HRMLD_CarryForFlg;
                                    MM.HRMLD_EncashFlg = data.HRMLD_EncashFlg;

                                    _lmContext.Add(MM);

                                    if (data.monthwisecredit == false)
                                    {
                                        HR_Master_Leave_Details_CreditMonth_DMO cm = Mapper.Map<HR_Master_Leave_Details_CreditMonth_DMO>(ttlldto);
                                        cm.HRMLD_Id = MM.HRMLD_Id;
                                        cm.HRMLDCM_LCMonthCode = 1;
                                        cm.CreatedDate = DateTime.Now;
                                        cm.UpdatedDate = DateTime.Now;
                                        cm.HRMLDCM_CreatedBy = data.UserId;
                                        cm.HRMLDCM_UpdatedBy = data.UserId;
                                        cm.HRMLDCM_NoOfLeaves = Convert.ToInt64(data.HRMLD_NoOfDays);
                                        _lmContext.Add(cm);
                                    }
                                    else
                                    {
                                        for (int m = 0; m < data.selectedMonthList.Length; m++)
                                        {
                                            HR_Master_Leave_Details_CreditMonth_DMO cm = Mapper.Map<HR_Master_Leave_Details_CreditMonth_DMO>(ttlldto);
                                            cm.HRMLD_Id = MM.HRMLD_Id;
                                            cm.HRMLDCM_LCMonthCode = data.selectedMonthList[m].IVRM_Month_Id;
                                            cm.CreatedDate = DateTime.Now;
                                            cm.UpdatedDate = DateTime.Now;
                                            cm.HRMLDCM_CreatedBy = data.UserId;
                                            cm.HRMLDCM_UpdatedBy = data.UserId;
                                            cm.HRMLDCM_NoOfLeaves = data.selectedMonthList[m].HRMLDCM_NoOfLeaves;
                                            _lmContext.Add(cm);
                                        }
                                    }

                                    var flag_sucess = _lmContext.SaveChanges();
                                    if (flag_sucess > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                    }
                                }
                                //else
                                //{
                                //    data.message = "dublicate";
                                //}
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _log.LogInformation("Leave Details error");
                _log.LogDebug(ex.Message);
            }

            return data;
        }


        public LeaveCreditDTO EditData(LeaveCreditDTO data)
        {

            using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Leave_details_Edit";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HRMLD_Id", SqlDbType.BigInt)
                {
                    Value = data.HRMLD_Id
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
                    data.report_details = retObject.ToArray();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            return data;
        }



        public LeaveCreditDTO LeaveMaster(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDCM_Id > 0)
                {
                    var result = _lmContext.HR_Master_Leave_DMO.Single(t => t.MI_Id == data.MI_Id && t.HRML_Id == data.HRMLDCM_Id);
                    data.HRML_Id = result.HRML_Id;
                    data.HRML_LeaveName = result.HRML_LeaveName;
                    data.HRML_LeaveCode = result.HRML_LeaveCode;
                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;


                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_DMO MM1 = new HR_Master_Leave_DMO();
                    MM1.MI_Id = data.MI_Id;
                    MM1.HRML_LeaveCode = data.HRML_LeaveCode;
                    MM1.CreatedDate = DateTime.Now;
                    MM1.UpdatedDate = DateTime.Now;
                    //MM1.HRML_UpdatedBy = data.UserId;
                    //MM1.HRML_CreatedBy = data.UserId;

                    _lmContext.Add(MM1);

                    data.HRML_Id = MM1.HRML_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public LeaveCreditDTO LeaveDetailsCreditMonth(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDCM_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_CreditMonth_DMO.Single(t => t.HRMLDCM_Id == data.HRMLDCM_Id);
                    data.HRMLDCM_Id = result.HRMLDCM_Id;
                    data.HRMLDCM_LCMonthCode = result.HRMLDCM_LCMonthCode;
                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;
                    data.HRMLDCM_Id = result.HRMLDCM_Id;

                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_Details_CreditMonth_DMO MM2 = new HR_Master_Leave_Details_CreditMonth_DMO();
                    // MM2.MI_Id = data.MI_Id;
                    MM2.HRMLDCM_Id = data.HRMLDCM_Id;
                    MM2.HRMLDCM_LCMonthCode = data.HRMLDCM_LCMonthCode;
                    MM2.CreatedDate = DateTime.Now;
                    MM2.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM2);

                    data.HRMLDCM_Id = MM2.HRMLDCM_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public LeaveCreditDTO LeaveDetailsCF(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDCF_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_CF_DMO.Single(t => t.HRMLDCF_Id == data.HRMLDCF_Id);
                    data.HRMLDCF_Id = result.HRMLDCF_Id;
                    data.HRMLDCF_MaxLeaveAplFlg = result.HRMLDCF_MaxLeaveAplFlg;
                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;


                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_Details_CF_DMO MM3 = new HR_Master_Leave_Details_CF_DMO();
                    MM3.HRMLDCF_Id = data.HRMLDCF_Id;
                    MM3.HRMLDCF_MaxLeaveAplFlg = data.HRMLDCF_MaxLeaveAplFlg;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM3);

                    data.HRMLDCF_Id = MM3.HRMLDCF_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public LeaveCreditDTO LeaveDetailsCFmonth(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDCFM_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_CFMonth_DMO.Single(t => t.HRMLDCFM_Id == data.HRMLDCFM_Id);
                    data.HRMLDCF_Id = result.HRMLDCF_Id;

                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;


                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_Details_CFMonth_DMO MM4 = new HR_Master_Leave_Details_CFMonth_DMO();
                    MM4.HRMLDCFM_MonthId = data.HRMLDCFM_MonthId;
                    MM4.CreatedDate = DateTime.Now;
                    MM4.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM4);

                    data.HRMLDCFM_Id = MM4.HRMLDCFM_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }
        public LeaveCreditDTO LeaveDetailsEnCash(LeaveCreditDTO data)
        {
            try
            {
                //add & update Activity details
                if (data.HRMLDEC_Id > 0)
                {

                    var result = _lmContext.HR_Master_Leave_Details_EnCash_DMO.Single(t => t.HRMLDEC_Id == data.HRMLDEC_Id && t.HRMLDEC_ServiceAplFlg == data.HRMLDEC_ServiceAplFlg && t.HRMLDEC_MaxLeaveFlg == data.HRMLDEC_MaxLeaveFlg && t.HRMLDEC_MinLeaveFlg == data.HRMLDEC_MinLeaveFlg && t.HRMLDEC_ScheduleFlg == data.HRMLDEC_ScheduleFlg);
                    data.HRMLDEC_Id = result.HRMLDEC_Id;

                    data.HRMLDEC_ServiceAplFlg = result.HRMLDEC_ServiceAplFlg;
                    data.HRMLDEC_ServiceYear = result.HRMLDEC_ServiceYear;
                    data.HRMLDEC_ServiceMonth = result.HRMLDEC_ServiceMonth;
                    data.HRMLDEC_ServiceDays = result.HRMLDEC_ServiceDays;
                    data.HRMLDEC_MaxLeaveFlg = result.HRMLDEC_MaxLeaveFlg;
                    data.HRMLDEC_MaxLeaves = result.HRMLDEC_MaxLeaves;
                    data.HRMLDEC_MinLeaveFlg = result.HRMLDEC_MinLeaveFlg;
                    data.HRMLDEC_MinLeaves = result.HRMLDEC_MinLeaves;
                    data.HRMLDEC_ScheduleFlg = result.HRMLDEC_ScheduleFlg;
                    data.HRMLDEC_ScheduleYear = result.HRMLDEC_ScheduleYear;
                    data.HRMLDEC_ScheduleMonth = result.HRMLDEC_ScheduleMonth;

                    data.UpdatedDate = DateTime.Now;
                    data.CreatedDate = result.CreatedDate;

                    Mapper.Map(data, result);
                    _lmContext.Update(result);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.Edit_flag = true;
                    }
                    else
                    {
                        data.Edit_flag = false;
                    }
                }

                else
                {
                    HR_Master_Leave_Details_EnCash_DMO MM5 = new HR_Master_Leave_Details_EnCash_DMO();
                    MM5.HRMLDEC_ServiceAplFlg = data.HRMLDEC_ServiceAplFlg;
                    MM5.HRMLDEC_ServiceYear = data.HRMLDEC_ServiceYear;
                    MM5.HRMLDEC_ServiceMonth = data.HRMLDEC_ServiceMonth;
                    MM5.HRMLDEC_ServiceDays = data.HRMLDEC_ServiceDays;
                    MM5.HRMLDEC_MaxLeaveFlg = data.HRMLDEC_MaxLeaveFlg;
                    MM5.HRMLDEC_MaxLeaves = data.HRMLDEC_MaxLeaves;
                    MM5.HRMLDEC_MinLeaveFlg = data.HRMLDEC_MinLeaveFlg;
                    MM5.HRMLDEC_MinLeaves = data.HRMLDEC_MinLeaves;
                    MM5.HRMLDEC_ScheduleFlg = data.HRMLDEC_ScheduleFlg;
                    MM5.HRMLDEC_ScheduleYear = data.HRMLDEC_ScheduleYear;
                    MM5.HRMLDEC_ScheduleMonth = data.HRMLDEC_ScheduleMonth;
                    MM5.CreatedDate = DateTime.Now;
                    MM5.UpdatedDate = DateTime.Now;

                    _lmContext.Add(MM5);

                    data.HRMLDEC_Id = MM5.HRMLDEC_Id;
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Leave details error");
                _log.LogDebug(e.Message);
            }

            return data;
        }

        //Manual Leave Credit
        public LeaveCreditDTO OnLoadManualLeaveCredit(LeaveCreditDTO data)
        {
            try
            {
                if (data.OnLoadOrChangeInstFlag.Equals("OnLoad"))
                {
                    var getuserinstitution = _lmContext.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    List<long> miids = new List<long>();

                    foreach (var c in getuserinstitution)
                    {
                        miids.Add(c.MI_Id);
                    }

                    data.getuserinstitution = _lmContext.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();
                }

                data.leavearrayyear = _lmContext.HR_Master_LeaveYear_DMO.Where(a => a.MI_Id == data.MI_Id
                 && a.HRMLY_ActiveFlag == true).Distinct().OrderByDescending(a => a.HRMLY_LeaveYearOrder).ToArray();

                data.get_leaveDetails = _lmContext.HR_Master_Leave_DMO.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LeaveCreditDTO SearchManualLeaveCreditDetails(LeaveCreditDTO data)
        {
            try
            {
                data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                select new LeaveCreditDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) +
                                    (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                     (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                     (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)),
                                    HRME_DOJ = a.HRME_DOJ
                                }).Distinct().OrderBy(a => a.HRME_DOJ).ToArray();

                data.get_leavestatus = _lmContext.HR_Emp_Leave_StatusDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMLY_Id == data.HRMLY_Id
                && data.selectedreport.Contains(a.HRML_Id)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LeaveCreditDTO SaveManualLeaveCreditDetails(LeaveCreditDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                if (data.SelectedEmployees != null && data.SelectedEmployees.Length > 0)
                {
                    foreach (var emp in data.SelectedEmployees)
                    {
                        foreach (var emp_leaves in emp.leavedetails)
                        {
                            if (emp_leaves.HRELS_Id > 0)
                            {
                                var Result = _lmContext.HR_Emp_Leave_StatusDMO.Single(a => a.HRELS_Id == emp_leaves.HRELS_Id);
                                Result.HRELS_CBLeaves = emp_leaves.leavecount;
                                Result.UpdatedDate = indiantime0;
                                _lmContext.Update(Result);
                            }
                            else
                            {
                                HR_Emp_Leave_StatusDMO hR_Emp_Leave_StatusDMO = new HR_Emp_Leave_StatusDMO();
                                hR_Emp_Leave_StatusDMO.MI_Id = data.MI_Id;
                                hR_Emp_Leave_StatusDMO.HRME_Id = emp.HRME_Id;
                                hR_Emp_Leave_StatusDMO.HRML_Id = emp_leaves.HRML_Id;
                                hR_Emp_Leave_StatusDMO.HRMLY_Id = data.HRMLY_Id;
                                hR_Emp_Leave_StatusDMO.HRELS_OBLeaves = 0.00m;
                                hR_Emp_Leave_StatusDMO.HRELS_CreditedLeaves = emp_leaves.leavecount;
                                hR_Emp_Leave_StatusDMO.HRELS_TotalLeaves = emp_leaves.leavecount;
                                hR_Emp_Leave_StatusDMO.HRELS_TransLeaves = emp_leaves.leavecount;
                                hR_Emp_Leave_StatusDMO.HRELS_EncashedLeaves = 0;
                                hR_Emp_Leave_StatusDMO.HRELS_CBLeaves = emp_leaves.leavecount;
                                hR_Emp_Leave_StatusDMO.CreatedDate = indiantime0;
                                hR_Emp_Leave_StatusDMO.UpdatedDate = indiantime0;
                                _lmContext.Add(hR_Emp_Leave_StatusDMO);
                            }
                        }
                    }

                    var i = _lmContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Auto Leave Cretid API (Schedular)
        public LeaveCreditDTO.CreditLeaveDTO AutoCompOffCredit(LeaveCreditDTO.CreditLeaveDTO data)
        {
            try
            {
                var contactExistsP = _lmContext.Database.ExecuteSqlCommand("HR_Auto_CompOff_Credit");
                if (contactExistsP > 0)
                {
                    data.returnmsg = "Add";
                }

                else
                {
                    data.returnmsg = "notUpdated";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LeaveCreditDTO.CreditLeaveDTO AutoLeaveCredit(LeaveCreditDTO.CreditLeaveDTO data)
        {
            try
            {

                var contactExistsP = _lmContext.Database.ExecuteSqlCommand("HR_AutoLeave_Credit_proc");
                if (contactExistsP > 0)
                {
                    data.returnmsg = "Add";
                }
                else
                {
                    data.returnmsg = "notUpdated";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //END
        public LeaveCreditDTO LeavePendingMailTrigger(LeaveCreditDTO data)
        {
            try
            {
                List<LeaveCreditDTO> employeelist = new List<LeaveCreditDTO>();
                List<LeaveCreditDTO> leavelist = new List<LeaveCreditDTO>();

                employeelist = (from a in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                from b in _lmContext.Staff_User_Login
                                from c in _lmContext.Emp_Email_Id
                                where (a.HRME_Id == b.Emp_Code && a.HRME_Id == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                                select new LeaveCreditDTO
                                {
                                    HRME_EmailId = c.HRMEM_EmailId,
                                    UserId = b.Id,
                                }).Distinct().ToList();

                if (employeelist != null && employeelist.Count() > 0)
                {
                    

                    foreach (var itm in employeelist)
                    {
                        leavelist.Clear();
                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "VMS_LeaveApproval";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                            {
                                Value = itm.UserId
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
                                        leavelist.Add(new LeaveCreditDTO
                                        {

                                            HRME_EmployeeFirstName = Convert.ToString(dataReader["HRME_EmployeeFirstName"]),
                                            HRML_LeaveName = Convert.ToString(dataReader["HRML_LeaveName"]),
                                            HRELAP_FromDate = Convert.ToDateTime(dataReader["HRELAP_FromDate"]),
                                            HRELAP_ToDate = Convert.ToDateTime(dataReader["HRELAP_ToDate"]),
                                            HRELAP_TotalDays = Convert.ToDecimal(dataReader["HRELAP_TotalDays"]),
                                            HRELAP_LeaveReason = Convert.ToString(dataReader["HRELAP_LeaveReason"]),
                                        });
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (leavelist.Count > 0)
                        {

                            var rownumber = 0;
                            var str = "";

                            str += "<strong>Dear sir/madam</strong></br>" +
                                "<p><strong>You have below employee leaves to approve, Kindly check VMS Portal.</strong></p></br>";


                            str += "<table  style='border: 1px solid black;  border-collapse: collapse;'>" +
                                                                          "<thead style='border: 1px solid black; border - collapse: collapse; background-color: #0463c2;color: #fff !important;text-align: center;'>" +
                                                            "<tr >" +
                                                            "<th align=" + "left" + " style=' border: 1px solid black;  border - collapse: collapse; '>" + "Serial No" + "</td>" +
                                                            "<th align=" + "left" + " style=' border: 1px solid black;  border - collapse: collapse; '>" + "Emploee Name" + "</td> " +
                                                              "<th align=" + "left" + " style=' border: 1px solid black;  border - collapse: collapse; '>" + "Leave Name" + "</td> " +
                                                                "<th align=" + "left" + " style=' border: 1px solid black;  border - collapse: collapse; '>" + "From Date" + "</td> " +
                                                                "<th align=" + "left" + " style=' border: 1px solid black;  border - collapse: collapse; '>" + "To Date" + "</td> " +
                                                                "<th align=" + "left" + " style=' border: 1px solid black;  border - collapse: collapse; '>" + "Total Days" + "</td> " +
                                                            "</tr>" +
                                                              "</thead>" +
                                                              "<tbody>";


                            for (int i = 0; i < leavelist.Count; i++)
                            {

                                rownumber = rownumber + 1;
                                str += "<tr>" +
                             "<td align=" + "left" + " style=' border: 1px solid black;  border-collapse: collapse; '>" + rownumber + "</td>" +
                             "<td align=" + "left" + " style=' border: 1px solid black;  border-collapse: collapse; '>" + leavelist[i].HRME_EmployeeFirstName + "</td> " +
                               "<td align=" + "left" + " style=' border: 1px solid black;  border-collapse: collapse; '>" + leavelist[i].HRML_LeaveName + "</td> " +
                                 "<td align=" + "left" + " style=' border: 1px solid black;  border-collapse: collapse; '>" + leavelist[i].HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") + " </td> " +
                                   "<td align=" + "left" + " style=' border: 1px solid black;  border-collapse: collapse; '>" + leavelist[i].HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") + "</td> " +
                                     "<td align=" + "left" + " style=' border: 1px solid black;  border-collapse: collapse; '>" + leavelist[i].HRELAP_TotalDays + "</td> " +
                             "</tr>";
                            }

                            str += "</tbody>" +
                       "</table>";

                            if (itm.HRME_EmailId.Length>0)
                            {
                                //sendmailToManagement(17, str, itm.HRME_EmailId);
                                data.retrunMsg = "Email Sent Succefully ";
                            }
                            sendmailToManagement(17, str, "drishya@unnathimarketing.com");
                            // sendmailToManagement(17, str, "praveenkumar@globalqtytrainig.com");

                        }
                    }
                }
                else
                {
                    data.retrunMsg = "Authority is not mapped OR Email is not setted for Approval Person ";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public string sendmailToManagement(long MI_Id, string TableDetails, string emailccdetails)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();

                //var template = _db.smsEmailSetting.Where(e => e.MI_Id==MI_Id && e.ISES_Template_Name == templatesetting
                //&& e.ISES_MailActiveFlag == true).ToList();
                //if (template.Count == 0)
                //{
                //    return "Email Template not Mapped to the selected Institution";
                //}
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = TableDetails;
                string Mailmsg = TableDetails;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                Mailmsg = Mailmsg.Replace("[MSG]", TableDetails);
                //Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = "Leave Approval Pending Details";
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
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
                        message.AddTo(emailccdetails);


                        //if (Attechement.Equals("1"))
                        //{
                        //    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        //    if (img.Count > 0)
                        //    {
                        //        for (int i = 0; i < img.Count; i++)
                        //        {
                        //            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                        //            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                        //            Stream stream = response.GetResponseStream();
                        //            message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                        //        }
                        //    }
                        //}
                        //******************* EMAIL CC DETAILS ****************//

                        //if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        //{
                        //    string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                        //    foreach (var c in ccmaildetails)
                        //    {
                        //        message.AddCc(c);
                        //    }
                        //}

                        //******************* EMAIL BCC DETAILS ****************//

                        //if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        //{
                        //    string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                        //    foreach (var d in ccmaildetails)
                        //    {
                        //        message.AddCc(d);
                        //    }
                        //}




                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(emailccdetails);
                        //******************* EMAIL CC DETAILS ****************//

                        //if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        //{
                        //    string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                        //    foreach (var c in ccmaildetails)
                        //    {
                        //        message.AddCc(c);
                        //    }
                        //}

                        //******************* EMAIL BCC DETAILS ****************//

                        //if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        //{
                        //    string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                        //    foreach (var d in ccmaildetails)
                        //    {
                        //        message.AddCc(d);
                        //    }
                        //}

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
                            clientsmtp.UseDefaultCredentials = false;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(emailccdetails));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                //if (Attechement.Equals("1"))
                                //{
                                //    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                //    if (img.Count > 0)
                                //    {
                                //        for (int i = 0; i < img.Count; i++)
                                //        {

                                //            foreach (var attache in img.ToList())
                                //            {
                                //                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                //                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                //                Stream stream = response.GetResponseStream();
                                //                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                //            }

                                //        }
                                //    }
                                //}

                                //******************* EMAIL CC DETAILS ****************//

                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }

                                //******************* EMAIL BCC DETAILS ****************//

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


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "LeaveApprovalPendingDetails" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailccdetails
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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";
        }



    }
}
