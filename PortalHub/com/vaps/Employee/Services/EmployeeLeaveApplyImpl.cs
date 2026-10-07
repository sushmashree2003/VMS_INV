using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.LeaveManagement;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeLeaveApplyImpl:Interfaces.EmployeeLeaveApplyInterface 
    {
        public LMContext _lmContext;
        public ExamContext _exm;
        public EmployeeLeaveApplyImpl(LMContext ttcategory, ExamContext exm)
        {
            _lmContext = ttcategory;
            _exm = exm;
        }

        public EmployeeDashboardDTO getonlineLeave(EmployeeDashboardDTO data)
        {
            data.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

            data.leave_name = (from x in _lmContext.HR_Master_Leave_DMO
                               from y in _lmContext.HR_Emp_Leave_StatusDMO

                               where (x.HRML_Id == y.HRML_Id && x.MI_Id == data.MI_Id && x.HRML_LeaveCreditFlg == true && y.HRME_Id == 15 )
                               select new EmployeeDashboardDTO
                               {

                                   HRELS_Id = y.HRELS_Id,
                                   HRML_LeaveName = x.HRML_LeaveName,
                                   HRELS_TotalLeaves = y.HRELS_TotalLeaves,
                                   HRELS_CreditedLeaves = y.HRELS_CreditedLeaves,
                                   HRELS_TransLeaves = y.HRELS_TransLeaves,
                                   HRELS_CBLeaves = y.HRELS_CBLeaves,
                                   HRML_Id = y.HRML_Id

                               }
                     ).Distinct().ToArray();


            data.online_leave = (from a in _lmContext.HR_Master_Employee_DMO
                                 from b in _lmContext.HR_Master_Designation_DMO
                                // from c in _lmContext.HR_Emp_Leave_StatusDMO
                                 where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == 15 && a.MI_Id == data.MI_Id)
                                 select new EmployeeDashboardDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                     HRME_DOJ = a.HRME_DOB,
                                     HRME_MobileNo = a.HRME_MobileNo,
                                     HRME_EmailId = a.HRME_EmailId,
                                     HRMDES_DesignationName = b.HRMDES_DesignationName
                                 }
                     ).Distinct().ToArray();

            return data;
        }
        public EmployeeDashboardDTO saveonlineLeave(EmployeeDashboardDTO _category)
        {
            HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(_category);
            try
            {
                _category.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == _category.UserId && c.MI_Id == _category.MI_Id).Emp_Code;

                if (objpge.HRELAP_Id > 0)
                {
                   

                    var resultCount = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_FromDate <= objpge.HRELAP_FromDate && t.HRELAP_ToDate >= objpge.HRELAP_ToDate).Count();

                    if (resultCount == 0)
                    {
                        var result = _lmContext.HR_Emp_Leave_ApplicationDMO.Single(t => t.MI_Id == objpge.MI_Id);

                        result.MI_Id = objpge.MI_Id;
                        //result.HRME_Id = objpge.HRME_Id;
                        result.HRME_Id = 15;
                        result.HRELAP_ApplicationDate = objpge.HRELAP_ApplicationDate;
                        result.HRELAP_FromDate = objpge.HRELAP_FromDate;
                        result.HRELAP_ToDate = objpge.HRELAP_ToDate;
                        result.HRELAP_TotalDays = objpge.HRELAP_TotalDays;
                        result.HRELAP_LeaveReason = objpge.HRELAP_LeaveReason;
                        result.HRELAP_ContactNoOnLeave = objpge.HRELAP_ContactNoOnLeave;
                        result.HRELAP_ReportingDate = objpge.HRELAP_ReportingDate;
                        result.HRELAP_UpdatedBy = _category.UserId;


                        result.UpdatedDate = DateTime.Now;
                        _lmContext.Update(result);
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
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                }
                else
                {


                    var checkbalanceleave = _lmContext.HR_Emp_Leave_StatusDMO.Where(d => d.MI_Id == _category.MI_Id && d.HRME_Id == _category.HRME_Id && d.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id).ToList();
                    var bal_leave = checkbalanceleave.FirstOrDefault().HRELS_CBLeaves - _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                    var fromdate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate).ToString("d");
                    var todate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate).ToString("d");
                    if (checkbalanceleave.FirstOrDefault().HRELS_CBLeaves > 0)
                    {
                        var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                              from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                              where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == _category.HRME_Id 
                                              && m.HRELAP_FromDate.Value.Date == _category.frmToDates.FirstOrDefault().HRELAP_FromDate.Value.Date
                                              && m.HRELAP_ToDate.Value.Date == _category.frmToDates.FirstOrDefault().HRELAP_ToDate.Value.Date
                                              && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                              select new EmployeeDashboardDTO
                                              {
                                                  HRELAP_Id = m.HRELAP_Id
                                              }
                                              ).ToList();
                        if (checkduplicate.Count == 0)
                        {
                            objpge.HRME_Id = 15;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.HRELAP_CreatedBy = _category.UserId;
                            objpge.HRELAP_UpdatedBy = _category.UserId;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.HRELAP_ActiveFlag = true;
                            objpge.HRELAP_ApplicationID = _category.temp_table_data.FirstOrDefault().HRELAP_ApplicationID;
                            objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                            objpge.HRELAP_ApplicationStatus = "Applied";
                            objpge.HRELAP_SanctioningLevel = "1";
                            objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                            objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);


                            _lmContext.Add(objpge);
                            var contactExists = _lmContext.SaveChanges();


                            HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);


                            if (objpge.HRELAP_Id > 0)
                            {
                                var resultempltransDetails = _lmContext.HR_Emp_Leave_Appl_DetailsDMO.Where(t => t.HRELAP_Id == objpge1.HRELAP_Id && t.HRELAPD_FromDate <= objpge1.HRELAPD_FromDate && t.HRELAPD_ToDate == objpge1.HRELAPD_ToDate).ToList();


                                Mapper.Map(objpge1, resultempltransDetails);
                                _lmContext.Update(resultempltransDetails);

                            }


                            else
                            {


                            }
                            if (contactExists > 0)
                            {
                                _category.returnval = true;

                                var statusUpdate = _lmContext.HR_Emp_Leave_StatusDMO.Single(d => d.HRELS_Id == _category.temp_table_data.FirstOrDefault().HRELS_Id);
                                statusUpdate.HRELS_TransLeaves = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                statusUpdate.HRELS_CBLeaves = bal_leave;
                                _lmContext.Update(statusUpdate);
                                _lmContext.SaveChanges();
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        }
                        else
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }

                    }
                    else
                    {
                        _category.message = "You Crossed Your Leave Limits";
                    }

                }
                // }

                List<HR_Emp_Leave_ApplicationDMO> m_events = new List<HR_Emp_Leave_ApplicationDMO>();
                m_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                _category.master_eventlist = m_events.ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }
    }
}
