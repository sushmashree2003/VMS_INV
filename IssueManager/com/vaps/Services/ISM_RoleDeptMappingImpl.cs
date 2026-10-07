using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IssueManager.com.vaps.Services
{
    public class ISM_RoleDeptMappingImpl:Interfaces.ISM_RoleDeptMappingInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public ISM_RoleDeptMappingImpl(IssueManagerContext hh)
        {
            _IssueManagercontext = hh;
        }
        public ISM_RoleDeptMappingDTO loaddata(ISM_RoleDeptMappingDTO data)
        {
            try
            {
                data.role_list = _IssueManagercontext.IVRM_Role_Type.Where(t => t.IVRMRT_Id!=0).Distinct().ToArray();

               data.department_list = _IssueManagercontext.HR_Master_Department.Where(t => t.HRMD_Id != 0 && t.MI_Id == data.MI_Id &&  t.HRMD_ActiveFlag == true).Distinct().ToArray();

                data.get_deptlist = (from a in _IssueManagercontext.IVRM_Role_Type
                                     from b in _IssueManagercontext.HR_Master_Department
                                     from c in _IssueManagercontext.ISM_RoleDeptMappingDMO
                                     where (a.IVRMRT_Id == c.IVRMRT_Id && b.HRMD_Id == c.HRMD_Id && c.MI_Id == data.MI_Id)
                                     select new ISM_RoleDeptMappingDTO
                                     {
                                         ISMRTDEM_Id = c.ISMRTDEM_Id,
                                         IVRMRT_Id = a.IVRMRT_Id,
                                         IVRMRT_Role = a.IVRMRT_Role,
                                         HRMD_Id = b.HRMD_Id,
                                         HRMD_DepartmentName = b.HRMD_DepartmentName,
                                         ISMRTDEM_ActiveFlag = c.ISMRTDEM_ActiveFlag,
                                     }
                                   ).Distinct().ToArray();
                  }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
 //  ==================================================save data=======================================================//
        public ISM_RoleDeptMappingDTO savedata(ISM_RoleDeptMappingDTO data)
        {
            try
            {
                if (data.ISMRTDEM_Id == 0)
                {
                    //var duplicate = _IssueManagercontext.ISM_RoleDeptMappingDMO.Where(t => t.HRMD_Id == data.HRMD_Id && t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                    
                    //if (duplicate.Count() > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    foreach (var ee in data.departmentids)
                    {
                        var duplicate = _IssueManagercontext.ISM_RoleDeptMappingDMO.Where(t => t.HRMD_Id == ee.HRMD_Id && t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                        if (duplicate.Count() == 0)
                        {
                            ISM_RoleDeptMappingDMO vvv = new ISM_RoleDeptMappingDMO();
                            vvv.IVRMRT_Id = data.IVRMRT_Id;
                            vvv.HRMD_Id = ee.HRMD_Id;
                            vvv.MI_Id = data.MI_Id;
                            vvv.CreatedDate = DateTime.Now;
                            vvv.ISMRTDEM_CreatedBy = data.UserId;
                            vvv.ISMRTDEM_ActiveFlag = true;
                            _IssueManagercontext.Add(vvv);
                            int y = _IssueManagercontext.SaveChanges();
                            if (y > 0)
                            {
                                data.msg = "Saved";
                            }
                            else
                            {
                                data.msg = "Failed";
                            }
                        }
                    }
                }
                else if (data.ISMRTDEM_Id > 0)
                {
                    var duplicate = _IssueManagercontext.ISM_RoleDeptMappingDMO.Where(t => t.ISMRTDEM_Id != data.ISMRTDEM_Id && t.IVRMRT_Id == data.IVRMRT_Id && t.HRMD_Id == data.HRMD_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var ss = _IssueManagercontext.ISM_RoleDeptMappingDMO.Single(t => t.ISMRTDEM_Id == data.ISMRTDEM_Id);
                        ss.IVRMRT_Id = data.IVRMRT_Id;
                        ss.HRMD_Id = data.HRMD_Id;
                        ss.MI_Id = data.MI_Id;
                        ss.UpdatedDate = DateTime.Now;
                        ss.ISMRTDEM_UpdatedBy = data.UserId;
                        _IssueManagercontext.Update(ss);
                        int r = _IssueManagercontext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "Updated";
                        }
                        else
                        {
                            data.msg = "Failed";
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
        //==================================deptDecative==================================================//
        public ISM_RoleDeptMappingDTO deptDecative(ISM_RoleDeptMappingDTO data)
        {
            try
            {
                var u = _IssueManagercontext.ISM_RoleDeptMappingDMO.Single(t => t.ISMRTDEM_Id==data.ISMRTDEM_Id);
                if (u.ISMRTDEM_ActiveFlag == true)
                {
                    u.ISMRTDEM_ActiveFlag = false;
                }
                else if (u.ISMRTDEM_ActiveFlag == false)
                {
                    u.ISMRTDEM_ActiveFlag = true;
                }

                _IssueManagercontext.Update(u);
                int o = _IssueManagercontext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
     }
}
