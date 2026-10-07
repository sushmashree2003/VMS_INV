using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_Master_TaskGroupImpl : Interfaces.ISM_Master_TaskGroupInterface
    {

        public IssueManagerContext _IssueManagercontext;
        public ISM_Master_TaskGroupImpl(IssueManagerContext hh)
        {
            _IssueManagercontext = hh;
        }

        public ISM_Master_TaskGroupDTO loaddata(ISM_Master_TaskGroupDTO data)
        {
            try
            {
                var hrmeid = _IssueManagercontext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                if (hrmeid.Count() > 0)
                {
                    data.department_list = _IssueManagercontext.HR_Master_Department.Where(t => t.HRMD_ActiveFlag == true).Distinct().ToArray();

                    data.alldata = (from a in _IssueManagercontext.HR_Master_Department
                                    from b in _IssueManagercontext.ISM_Master_TaskGroupDMO
                                    where (b.MI_Id == data.MI_Id && a.HRMD_Id == b.ISMMTGRP_CreatedByDeptId)
                                    select new ISM_Master_TaskGroupDTO
                                    {
                                        ISMMTGRP_Id = b.ISMMTGRP_Id,
                                        ISMMTGRP_ActiveFlag = b.ISMMTGRP_ActiveFlag,
                                        HRMD_DepartmentName = a.HRMD_DepartmentName,
                                        ISMMTGRP_TaskGroupName = b.ISMMTGRP_TaskGroupName,
                                        ISMMTGRP_TGRemarks = b.ISMMTGRP_TGRemarks,
                                        HRMD_Id = a.HRMD_Id
                                    }).Distinct().ToArray();
            }
                else
                {
                data.msg = "You Dont Have Permission To Save Data";
            }
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Master_TaskGroupDTO savedata(ISM_Master_TaskGroupDTO data)
        {
            try
            {
                var hrmeidd = _IssueManagercontext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                if (hrmeidd.Count() == 0)
                {
                    data.msg = "You Dont Have Permission To Save Data";
                    return data;
                }

                if (data.ISMMTGRP_Id == 0)
                {
                    var duplicate = _IssueManagercontext.ISM_Master_TaskGroupDMO.Where(t => t.ISMMTGRP_Id != 0 && t.ISMMTGRP_TaskGroupName == data.ISMMTGRP_TaskGroupName && t.ISMMTGRP_TGRemarks == data.ISMMTGRP_TGRemarks).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        var hrmeid = _IssueManagercontext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();
                        var deptid1 = _IssueManagercontext.MasterEmployee.
                            Where(a => a.HRME_Id == hrmeid[0].Emp_Code).ToList();
                        ISM_Master_TaskGroupDMO dmo = new ISM_Master_TaskGroupDMO();
                        dmo.ISMMTGRP_TaskGroupName = data.ISMMTGRP_TaskGroupName;
                        dmo.MI_Id = data.MI_Id;
                        dmo.ISMMTGRP_CreatedByDeptId = deptid1[0].HRMD_Id;
                        dmo.ISMMTGRP_TGRemarks = data.ISMMTGRP_TGRemarks;
                        dmo.ISMMTGRP_ActiveFlag = true;
                        dmo.ISMMTGRP_CreatedBy = data.UserId;
                        dmo.ISMMTGRP_UpdatedBy = data.UserId;
                        dmo.ISMMTGRP_CreatedDate = DateTime.Now;
                        dmo.ISMMTGRP_UpdatedDate = DateTime.Now;
                        _IssueManagercontext.Add(dmo);

                        foreach (var item in data.deptid)
                        {
                            ISM_Master_TaskGroup_DeptDMO dpt = new ISM_Master_TaskGroup_DeptDMO();
                            dpt.ISMMTGRP_Id = dmo.ISMMTGRP_Id;
                            dpt.HRMD_Id = item.HRMD_Id;
                            dpt.ISMMTGRPD_ActiveFlag = true;
                            dpt.ISMMTGRPD_CreatedBy = data.UserId;
                            dpt.ISMMTGRPD_UpdatedBy = data.UserId;
                            dpt.ISMMTGRPD_CreatedDate = DateTime.Now;
                            dpt.ISMMTGRPD_UpdatedDate = DateTime.Now;
                            _IssueManagercontext.Add(dpt);
                        }

                        int y = _IssueManagercontext.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }

                else if (data.ISMMTGRP_Id > 0)
                {
                    var duplicate = _IssueManagercontext.ISM_Master_TaskGroupDMO.Where(t => t.ISMMTGRP_TaskGroupName == data.ISMMTGRP_TaskGroupName && t.ISMMTGRP_TGRemarks == data.ISMMTGRP_TGRemarks && t.ISMMTGRP_CreatedByDeptId == data.ISMMTGRP_CreatedByDeptId && t.ISMMTGRP_Id != data.ISMMTGRP_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var hrmeid = _IssueManagercontext.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();
                        var deptid1 = _IssueManagercontext.MasterEmployee.Where(a => a.HRME_Id == hrmeid[0].Emp_Code).ToList();

                        var dmo = _IssueManagercontext.ISM_Master_TaskGroupDMO.Single(t => t.ISMMTGRP_Id == data.ISMMTGRP_Id && t.MI_Id == data.MI_Id);
                        dmo.ISMMTGRP_TaskGroupName = data.ISMMTGRP_TaskGroupName;
                        dmo.MI_Id = data.MI_Id;
                        dmo.ISMMTGRP_CreatedByDeptId = deptid1[0].HRMD_Id;
                        dmo.ISMMTGRP_TGRemarks = data.ISMMTGRP_TGRemarks;
                        dmo.ISMMTGRP_UpdatedBy = data.UserId;
                        dmo.ISMMTGRP_UpdatedDate = DateTime.Now;

                        _IssueManagercontext.Update(dmo);

                        var removelist = _IssueManagercontext.ISM_Master_TaskGroup_DeptDMO.Where(t => t.ISMMTGRP_Id == data.ISMMTGRP_Id).ToList();
                        if (removelist.Count > 0)
                        {
                            foreach (var item in removelist)
                            {
                                _IssueManagercontext.Remove(item);
                            }

                        }
                        foreach (var item in data.deptid)
                        {
                            ISM_Master_TaskGroup_DeptDMO dpt = new ISM_Master_TaskGroup_DeptDMO();
                            dpt.ISMMTGRP_Id = dmo.ISMMTGRP_Id;
                            dpt.HRMD_Id = item.HRMD_Id;
                            dpt.ISMMTGRPD_ActiveFlag = true;
                            dpt.ISMMTGRPD_CreatedBy = data.UserId;
                            dpt.ISMMTGRPD_UpdatedBy = data.UserId;
                            dpt.ISMMTGRPD_CreatedDate = DateTime.Now;
                            dpt.ISMMTGRPD_UpdatedDate = DateTime.Now;
                            _IssueManagercontext.Add(dpt);
                        }

                        int r = _IssueManagercontext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
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

        public ISM_Master_TaskGroupDTO Editdata(ISM_Master_TaskGroupDTO data)
        {
            try
            {
                var editlist = (from a in _IssueManagercontext.ISM_Master_TaskGroupDMO
                                from c in _IssueManagercontext.ISM_Master_TaskGroup_DeptDMO
                                where (a.ISMMTGRP_Id == data.ISMMTGRP_Id && c.ISMMTGRP_Id == a.ISMMTGRP_Id
                                       && a.MI_Id == data.MI_Id)
                                select new ISM_Master_TaskGroupDTO
                                {
                                    ISMMTGRP_Id = a.ISMMTGRP_Id,
                                    ISMMTGRP_TaskGroupName = a.ISMMTGRP_TaskGroupName,
                                    ISMMTGRP_TGRemarks = a.ISMMTGRP_TGRemarks,
                                    ISMMTGRP_CreatedByDeptId = a.ISMMTGRP_CreatedByDeptId,
                                    HRMD_Id = c.HRMD_Id,
                                    MI_Id = data.MI_Id,
                                }).Distinct().ToArray();
                data.editlist = editlist.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ISM_Master_TaskGroupDTO clientDecative(ISM_Master_TaskGroupDTO data)
        {
            try
            {
                var u = _IssueManagercontext.ISM_Master_TaskGroup_DeptDMO.Where(t => t.ISMMTGRP_Id == data.ISMMTGRP_Id).ToList();
                foreach (var item in u)
                {
                    if (item.ISMMTGRPD_ActiveFlag == true)
                    {
                        var ss1 = _IssueManagercontext.ISM_Master_TaskGroup_DeptDMO.Single(a => a.ISMMTGRP_Id == data.ISMMTGRP_Id && a.HRMD_Id == item.HRMD_Id);
                        ss1.ISMMTGRPD_ActiveFlag = false;
                        _IssueManagercontext.Update(ss1);
                        data.returnval = true;
                    }
                    else
                    {
                        var ss1 = _IssueManagercontext.ISM_Master_TaskGroup_DeptDMO.Single(a => a.ISMMTGRP_Id == data.ISMMTGRP_Id && a.HRMD_Id == item.HRMD_Id);
                        ss1.ISMMTGRPD_ActiveFlag = true;
                        _IssueManagercontext.Update(ss1);
                        data.returnval = true;
                    }
                }

                var v = _IssueManagercontext.ISM_Master_TaskGroupDMO.Single(a => a.ISMMTGRP_Id == data.ISMMTGRP_Id);

                if (data.ISMMTGRP_ActiveFlag == true)
                {

                    v.ISMMTGRP_ActiveFlag = false;
                    _IssueManagercontext.Update(v);
                    data.returnval = true;
                }
                else
                {
                    v.ISMMTGRP_ActiveFlag = true;
                    _IssueManagercontext.Update(v);
                    data.returnval = true;
                }
                _IssueManagercontext.SaveChanges();

            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public ISM_Master_TaskGroupDTO DeptDeactive(ISM_Master_TaskGroupDTO data)
        {
            try
            {
                var u = _IssueManagercontext.ISM_Master_TaskGroup_DeptDMO.Where(t => t.ISMMTGRPD_Id == data.ISMMTGRPD_Id).SingleOrDefault();
                if (u.ISMMTGRPD_ActiveFlag == true)
                {
                    u.ISMMTGRPD_ActiveFlag = false;
                }
                else if (u.ISMMTGRPD_ActiveFlag == false)
                {
                    u.ISMMTGRPD_ActiveFlag = true;
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

        public ISM_Master_TaskGroupDTO viewuploaddept(ISM_Master_TaskGroupDTO data)
        {
            try
            {
                data.viewuploaddept = (from a in _IssueManagercontext.HR_Master_Department
                                       from b in _IssueManagercontext.ISM_Master_TaskGroup_DeptDMO
                                       from c in _IssueManagercontext.ISM_Master_TaskGroupDMO
                                       where (c.ISMMTGRP_Id == data.ISMMTGRP_Id && c.ISMMTGRP_Id == b.ISMMTGRP_Id
                                       && b.HRMD_Id == a.HRMD_Id)
                                       select new ISM_Master_TaskGroupDTO
                                       {
                                           ISMMTGRPD_Id = b.ISMMTGRPD_Id,
                                           HRMD_Id = a.HRMD_Id,
                                           HRMD_DepartmentName = a.HRMD_DepartmentName,
                                           ISMMTGRPD_ActiveFlag = b.ISMMTGRPD_ActiveFlag,
                                       }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
