using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class MasterActivitiesImpl : Interfaces.MasterActivitiesInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public MasterActivitiesImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public MasterActivities_DTO getdetails(MasterActivities_DTO dTO)
        {
            try
            {
                dTO.alldata = (from a in _IssueContext.MasterActivities_DMO
                               from b in _IssueContext.HR_Master_Department
                               from c in _IssueContext.HR_Master_Designation
                               where (a.MI_Id == b.MI_Id &&  a.MI_Id==c.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                               select new MasterActivities_DTO {
                                   ISMMAC_ActivityName=a.ISMMAC_ActivityName,
                                   ISMMAC_ActiveFlag=a.ISMMAC_ActiveFlag,
                                   ISMMAC_Desc=a.ISMMAC_Desc,
                                   ISMMAC_Id=a.ISMMAC_Id,
                                   HRMD_Id=a.HRMD_Id,
                                   HRMDES_Id=a.HRMDES_Id,
                                   HRMD_DepartmentName=b.HRMD_DepartmentName,
                                   HRMDES_DesignationName=c.HRMDES_DesignationName
                               }).Distinct().ToArray();


                dTO.deptlist = _Context.HR_Master_Department.Where(t => t.MI_Id == dTO.MI_Id && t.HRMD_ActiveFlag==true).Distinct().OrderBy(t=>t.HRMD_DepartmentName).ToArray();
                dTO.designationlist = _Context.HR_Master_Designation.Where(t => t.MI_Id == dTO.MI_Id && t.HRMDES_ActiveFlag==true).Distinct().OrderBy(t=>t.HRMDES_DesignationName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }

        public MasterActivities_DTO saverecord(MasterActivities_DTO data)
        {
            try
            {
                if (data.ISMMAC_Id == 0)
                {
                    var duplicate = _IssueContext.MasterActivities_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMAC_ActivityName == data.ISMMAC_ActivityName && t.HRMD_Id==data.HRMD_Id && t.HRMDES_Id==data.HRMDES_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterActivities_DMO obj = new MasterActivities_DMO();
                        // obj.ISMMAC_Id = data.ISMMAC_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.ISMMAC_ActivityName = data.ISMMAC_ActivityName;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.HRMDES_Id = data.HRMDES_Id;
                        obj.ISMMAC_Desc = data.ISMMAC_Desc;
                        obj.ISMMAC_CreatedBy = data.UserId;
                        obj.ISMMAC_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMAC_ActiveFlag = true;

                        _IssueContext.Add(obj);
                        int a = _IssueContext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var duplicate = _IssueContext.MasterActivities_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMAC_Id != data.ISMMAC_Id && t.ISMMAC_ActivityName == data.ISMMAC_ActivityName && t.HRMD_Id == data.HRMD_Id && t.HRMDES_Id == data.HRMDES_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var obj = _IssueContext.MasterActivities_DMO.Where(t => t.ISMMAC_Id == data.ISMMAC_Id).Single();

                        obj.ISMMAC_ActivityName = data.ISMMAC_ActivityName;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.HRMDES_Id = data.HRMDES_Id;
                        obj.ISMMAC_Desc = data.ISMMAC_Desc;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMAC_UpdatedBy = data.UserId;

                        _IssueContext.Update(obj);
                        int a = _IssueContext.SaveChanges();
                        if (a > 0)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterActivities_DTO deactiveY(MasterActivities_DTO data)
        {
            try
            {
                var result = _IssueContext.MasterActivities_DMO.Single(t => t.ISMMAC_Id == data.ISMMAC_Id);
                if (result.ISMMAC_ActiveFlag == true)
                {
                    result.ISMMAC_ActiveFlag = false;
                }
                else
                {
                    result.ISMMAC_ActiveFlag = true;
                }

                result.UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);
                int a = _IssueContext.SaveChanges();
                if (a > 0)
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
