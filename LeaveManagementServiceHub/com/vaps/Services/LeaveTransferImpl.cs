using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveTransferImpl : LeaveTransferInterface
    {
        public LMContext _lmContext;
        public LeaveTransferImpl(LMContext ttcategory)
        {
            _lmContext = ttcategory;
        }

        public LeaveCreditDTO getBasicData(LeaveCreditDTO dto)
        {
            try
            {
                List<HR_Master_Leave_DMO> leaveyeardropdown = new List<HR_Master_Leave_DMO>();
                leaveyeardropdown = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == dto.MI_Id && t.HRML_LeaveCreditFlg == true).ToList(); //
                dto.leave_name = leaveyeardropdown.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public LeaveCreditDTO getLeaveOB(LeaveCreditDTO data)
        {
            try
            {
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Page_Privilege_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@pageurl", SqlDbType.VarChar)
                    {
                        Value = "app.LeaveTransfer"
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
                        data.pagePrivilege = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                //Only it will work once page will mapp to that perticular employee
                if (data.pagePrivilege.Length > 0)
                {
                    List<HR_Master_GroupType_DMO> staf_types = new List<HR_Master_GroupType_DMO>();
                    staf_types = _lmContext.HR_Master_GroupType_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag == true).ToList();
                    data.stf_types = staf_types.Distinct().ToArray();

                    List<HR_Master_Department_DMO> Department_types = new List<HR_Master_Department_DMO>();
                    Department_types = _lmContext.HR_Master_Department_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
                    data.Department_types = Department_types.Distinct().ToArray();

                    List<HR_Master_Designation_DMO> Designation_types = new List<HR_Master_Designation_DMO>();
                    Designation_types = _lmContext.HR_Master_Designation_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToList();
                    data.Designation_types = Designation_types.Distinct().ToArray();


                    List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
                    leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveCreditFlg == true).ToList();
                    data.leave_name = leave_name.Distinct().ToArray();

                    List<HR_Master_LeaveYear_DMO> leavearrayyear = new List<HR_Master_LeaveYear_DMO>();
                    leavearrayyear = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag == true).ToList();
                    data.leavearrayyear = leavearrayyear.Distinct().OrderByDescending(a => a.HRMLY_LeaveYearOrder).ToArray();



                    data.result = (from c in _lmContext.HR_Emp_Leave_StatusDMO
                                   from b in _lmContext.HR_Master_Leave_DMO
                                   from a in _lmContext.HR_Master_Employee_DMO
                                   from d in _lmContext.HR_Master_LeaveYear_DMO
                                   where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && b.HRML_Id == c.HRML_Id && a.HRME_Id == c.HRME_Id && c.HRMLY_Id == d.HRMLY_Id)
                                   select new LeaveCreditDTO
                                   {
                                       HRELS_Id = c.HRELS_Id,
                                       HRME_Id = c.HRME_Id,
                                       HRML_Id = c.HRML_Id,
                                       HRMLY_Id = c.HRMLY_Id,
                                       HRML_LeaveName = b.HRML_LeaveName,
                                       HRELS_CBLeaves = c.HRELS_CBLeaves,
                                       HRELS_TotalLeaves = c.HRELS_TotalLeaves,
                                       HRMLY_LeaveYear = d.HRMLY_LeaveYear,
                                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                                   }
                       ).Distinct().ToArray();





                    data.leave_year_id = (from x in _lmContext.HR_Master_LeaveYear_DMO
                                          where (x.MI_Id == data.MI_Id)
                                          select new HR_Master_LeaveYear_DMO
                                          {
                                              HRMLY_Id = x.HRMLY_Id
                                          }
                            ).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        public LeaveCreditDTO get_departments(LeaveCreditDTO data)
        {
            try
            {
                List<long> selected_emp_types = new List<long>();

                foreach (var itm in data.emptypes)
                {
                    selected_emp_types.Add(itm.HRMGT_Id);
                }

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        public LeaveCreditDTO get_designation(LeaveCreditDTO data)
        {
            try
            {
                List<long?> selected_desg_types = new List<long?>();

                foreach (var itm in data.emptypes)
                {
                    selected_desg_types.Add(itm.HRMD_Id);
                }

                data.Designation_types = (from a in _lmContext.HRGroupDeptDessgDMO
                                          from b in _lmContext.HR_Master_Designation_DMO
                                          where (a.HRMDES_Id == b.HRMDES_Id
                                          && b.HRMDES_ActiveFlag == true
                                           && a.MI_Id == b.MI_Id & a.MI_Id == data.MI_Id && selected_desg_types.Contains(a.HRMD_Id))
                                          select new LeaveCreditDTO
                                          {
                                              HRMDES_Id = b.HRMDES_Id,
                                              HRMDES_DesignationName = b.HRMDES_DesignationName,
                                          }
                         ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        public LeaveCreditDTO get_ob_Details(LeaveCreditDTO data)
        {


            return data;
        }

        public LeaveCreditDTO get_Employe_ob(LeaveCreditDTO data)
        {
            try
            {

                List<long> selected_typ = new List<long>();

                foreach (var itm in data.emptypes)
                {
                    selected_typ.Add(itm.HRMGT_Id);
                }
                List<long?> selected_dep = new List<long?>();

                foreach (var itm in data.empdept)
                {
                    selected_dep.Add(itm.HRMD_Id);
                }
                List<long> selected_des = new List<long>();

                foreach (var itm in data.empdesg)
                {
                    selected_des.Add(itm.HRMDES_Id);
                }

                data.get_emp = (from a in _lmContext.HR_Master_Employee_DMO
                                from b in _lmContext.HR_Master_Designation_DMO
                                from c in _lmContext.HR_Master_Department_DMO
                                from d in _lmContext.HR_Master_GroupType_DMO
                                where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRMGT_Id == d.HRMGT_Id && d.HRMGT_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == d.MI_Id && selected_typ.Contains(d.HRMGT_Id) && selected_dep.Contains(c.HRMD_Id) && selected_des.Contains(b.HRMDES_Id))
                                select new LeaveCreditDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        public LeaveCreditDTO SaveDetails(LeaveCreditDTO _category)
        {
            //HR_Emp_Leave_StatusDMO objpge = Mapper.Map<HR_Emp_Leave_StatusDMO>(_category);          
            try
            {
                if (_category.HRELS_Id > 0)
                {
                    var resultCount = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRELS_OBLeaves == _category.HRELS_OBLeaves && t.HRELS_CreditedLeaves == _category.HRELS_CreditedLeaves && t.HRELS_TotalLeaves == _category.HRELS_TotalLeaves && t.HRELS_TransLeaves == _category.HRELS_TransLeaves && t.MI_Id == _category.MI_Id && t.HRML_Id == _category.HRML_Id && t.HRME_Id == _category.HRME_Id && t.HRMLY_Id == _category.HRMLY_Id && t.HRELS_EncashedLeaves == _category.HRELS_EncashedLeaves && t.HRELS_CBLeaves == _category.HRELS_CBLeaves).Count();
                    if (resultCount > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                    else
                    {
                        if (resultCount == 0)
                        {
                            var result = _lmContext.HR_Emp_Leave_StatusDMO.Single(t => t.HRELS_Id == _category.HRELS_Id);
                            result.HRELS_CBLeaves = _category.HRELS_CBLeaves;
                            result.MI_Id = _category.MI_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.HRELS_UpdatedBy = _category.UserId;
                            _lmContext.Update(result);
                        }
                        var contactExists = _lmContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _category.emplist.Length; i++)
                    {
                        HR_Emp_Leave_StatusDMO objpge1 = Mapper.Map<HR_Emp_Leave_StatusDMO>(_category);
                        var result = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRELS_CreditedLeaves == _category.emplist[i].HRELS_CreditedLeaves && t.MI_Id == objpge1.MI_Id && t.HRML_Id == objpge1.HRML_Id && t.HRME_Id == _category.emplist[i].HRME_Id && t.HRML_Id == objpge1.HRML_Id && t.HRMLY_Id == objpge1.HRMLY_Id).Count();
                        if (result > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else if (result == 0)
                        {
                            objpge1.MI_Id = _category.MI_Id;
                            objpge1.HRME_Id = _category.emplist[i].HRME_Id;
                            objpge1.HRML_Id = _category.HRML_Id;
                            objpge1.HRMLY_Id = _category.HRMLY_Id;
                            objpge1.HRELS_OBLeaves = 0;
                            objpge1.HRELS_CreditedLeaves = _category.emplist[i].HRELS_CreditedLeaves;
                            objpge1.HRELS_TotalLeaves = objpge1.HRELS_CreditedLeaves;
                            objpge1.HRELS_TransLeaves = 0;
                            objpge1.HRELS_EncashedLeaves = 0;
                            objpge1.HRELS_CBLeaves = _category.emplist[i].HRELS_CreditedLeaves;

                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.UpdatedDate = DateTime.Now;
                            objpge1.HRELS_UpdatedBy = _category.UserId;
                            objpge1.HRELS_CreatedBy = _category.UserId;
                            _lmContext.Add(objpge1);
                        }

                    }
                    var contactExists = _lmContext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        _category.returnval = true;
                    }
                    else
                    {
                        _category.returnval = false;
                    }

                }
                List<HR_Emp_Leave_StatusDMO> m_events = new List<HR_Emp_Leave_StatusDMO>();
                m_events = _lmContext.HR_Emp_Leave_StatusDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                _category.master_eventlist = m_events.ToArray();
                //  }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public LeaveCreditDTO SaveDetails11(LeaveCreditDTO data)
        {
            try
            {
                for (int i = 0; i < data.emplist.Length; i++)
                {
                    var result1 = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.emplist[i].HRME_Id && t.HRMLY_Id == data.HRMLY_Id && t.HRML_Id == data.HRML_Id).ToList();
                    if (result1.Count > 0)
                    {
                        //var resultwo = _lmContext.HR_Emp_Leave_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.emplist[i].HRME_Id && t.HRMLY_Id == data.HRMLY_Id && t.HRML_Id == data.HRML_Id);
                        //resultwo.HRELS_CreditedLeaves = resultwo.HRELS_CreditedLeaves + data.emplist[i].HRELS_CreditedLeaves;
                        //resultwo.HRELS_TotalLeaves = resultwo.HRELS_TotalLeaves + data.emplist[i].HRELS_CreditedLeaves;
                        //resultwo.HRELS_CBLeaves = resultwo.HRELS_CBLeaves + data.emplist[i].HRELS_CreditedLeaves;
                        //resultwo.UpdatedDate = DateTime.Now;
                        //resultwo.HRELS_UpdatedBy = data.UserId;
                        //_lmContext.Update(resultwo);
                        var contactExistsP = _lmContext.Database.ExecuteSqlCommand("HR_Leave_Additiondeduction @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, data.emplist[i].HRME_Id, data.HRML_Id, data.HRMLY_Id, data.emplist[i].HRELS_CreditedLeaves, data.emplist[i].Type, data.emplist[i].Reason, data.UserId);
                        if (contactExistsP > 0)
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

        public LeaveCreditDTO leavecarryforward(LeaveCreditDTO dto)
        {
            try
            {
                for (int i = 0; i < dto.emplist.Length; i++)
                {
                    var contactExistsP = _lmContext.Database.ExecuteSqlCommand("HR_EMPLEAVE_CARRYFORWARD_DETAILS @p0,@p1", dto.MI_Id, dto.emplist[i].HRME_Id);
                    if (contactExistsP > 0)
                    {
                        dto.retrunMsg = "Add";
                    }

                    else
                    {
                        dto.retrunMsg = "notUpdated";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public LeaveCreditDTO deletepages(int id)
        {
            LeaveCreditDTO page = new LeaveCreditDTO();
            try
            {
                List<HR_Emp_Leave_StatusDMO> lorg = new List<HR_Emp_Leave_StatusDMO>();
                lorg = _lmContext.HR_Emp_Leave_StatusDMO.Where(t => t.HRELS_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _lmContext.Remove(lorg.ElementAt(0));
                    var contactExists = _lmContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
    }
}
