using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Services
{
    public class ISM_CommonTaskImpl : Interfaces.ISM_CommonTaskInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_CommonTaskImpl(IssueManagerContext para1, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para1;
            _Context = para2;
        }

        public ISM_CommonTask_DTO Loaddata(ISM_CommonTask_DTO data)
        {
            try
            {
                data.daylist = (from a in _IssueContext.masterday
                                where (a.MI_Id == data.MI_Id && a.FOMD_ActiveFlag == true)
                                select new ISM_CommonTask_DTO
                                {
                                    FOMD_Id = a.FOMD_Id,
                                    FOMD_DayName = a.FOMD_DayName,
                                }).Distinct().OrderBy(a=>a.FOMD_Id).ToArray();

                data.venuelsit = (from a in _IssueContext.MasterVenues_DMO
                                  where (a.MI_Id == data.MI_Id && a.ISMMVNE_ActiveFlag == true)
                                  select new ISM_CommonTask_DTO
                                  {
                                      ISMMVNE_Id = a.ISMMVNE_Id,
                                      ISMMVNE_VenueName = a.ISMMVNE_VenueName,
                                  }).Distinct().OrderBy(t => t.ISMMVNE_Id).ToArray();

                data.mettingroomlist = (from a in _IssueContext.ISM_Master_MeetingRoom_DMO
                                        where (a.MI_Id == data.MI_Id && a.ISMMMER_ActiveFlag == true)
                                        select new ISM_CommonTask_DTO
                                        {
                                            ISMMMER_Id = a.ISMMMER_Id,
                                            ISMMMER_MeetingRoomName = a.ISMMMER_MeetingRoomName
                                        }).Distinct().OrderBy(t => t.ISMMMER_Id).ToArray();

                data.departmentlist = (from a in _IssueContext.HR_Master_Department
                                       where (a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
                                       select new ISM_CommonTask_DTO
                                       {
                                           HRMD_Id = a.HRMD_Id,
                                           HRMD_DepartmentName = a.HRMD_DepartmentName,
                                       }).Distinct().OrderBy(a=>a.HRMD_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_CommonTask_DTO get_Designation(ISM_CommonTask_DTO data)
        {
            try
            {
                List<long> dept_ids = new List<long>();
                if (data.selectedDept.Length > 0)
                {
                    foreach (var deptid in data.selectedDept)
                    {
                        dept_ids.Add(deptid.HRMD_Id);
                    }
                }

                data.designationlist = (from a in _IssueContext.HR_Master_Department
                                        from b in _IssueContext.MasterEmployee
                                        from c in _IssueContext.HR_Master_Designation
                                       where (dept_ids.Contains(a.HRMD_Id) && a.MI_Id == data.MI_Id && b.HRMD_Id==a.HRMD_Id
                                       && b.HRMDES_Id==c.HRMDES_Id && a.HRMD_ActiveFlag == true)
                                       select new ISM_CommonTask_DTO
                                       {
                                           HRMDES_Id = c.HRMDES_Id,
                                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                                       }).Distinct().OrderBy(a => a.HRMD_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_CommonTask_DTO get_Employee(ISM_CommonTask_DTO data)
        {
            try
            {
                List<long> dept_ids = new List<long>();
                List<long> hrmedes_ids = new List<long>();
                if (data.selectedDept.Length > 0)
                {
                    foreach (var deptid in data.selectedDept)
                    {
                        dept_ids.Add(deptid.HRMD_Id);
                    }
                }
                if (data.selectedDesg.Length > 0)
                {
                    foreach (var desgid in data.selectedDesg)
                    {
                        hrmedes_ids.Add(desgid.HRMDES_Id);
                    }
                }

                data.employeelist = (from a in _IssueContext.HR_Master_Department
                                        from b in _IssueContext.MasterEmployee
                                        from c in _IssueContext.HR_Master_Designation
                                        where (hrmedes_ids.Contains(c.HRMDES_Id) && dept_ids.Contains(a.HRMD_Id) && a.MI_Id == data.MI_Id && b.HRMD_Id == a.HRMD_Id
                                       && b.HRMDES_Id == c.HRMDES_Id && a.HRMD_ActiveFlag == true)
                                     select new ISM_CommonTask_DTO
                                        {
                                            HRME_Id = b.HRME_Id,
                                            HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                         HRME_EmployeeCode = b.HRME_EmployeeCode,
                                        }).Distinct().OrderBy(a => a.HRMD_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
