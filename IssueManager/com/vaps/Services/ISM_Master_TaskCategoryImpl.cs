using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_Master_TaskCategoryImpl : Interfaces.ISM_Master_TaskCategoryInterface
    {

        public IssueManagerContext _context;
        public ISM_Master_TaskCategoryImpl(IssueManagerContext w)
        {
            _context = w;
        }
        public ISM_Master_TaskCategory_DTO loaddata(ISM_Master_TaskCategory_DTO data)
        {
            try
            {

                data.departmentlist = (from a in _context.HR_Master_Department
                                       from b in _context.Institution
                                       where (a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true && b.MI_ActiveFlag == 1)
                                       select new ISM_Master_TaskCategory_DTO
                                       {
                                           HRMD_Id = a.HRMD_Id,
                                           HRMD_DepartmentName = a.HRMD_DepartmentName,
                                           MI_Name = b.MI_Name,
                                       }).Distinct().OrderByDescending(a => a.HRMD_Order).ToArray();

                data.alldata1 = (from a in _context.HR_Master_Department
                                 from b in _context.ISM_Master_TaskCategory_DMO
                                 from c in _context.Institution
                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == c.MI_Id && b.MI_Id == c.MI_Id)
                                 select new ISM_Master_TaskCategory_DTO
                                 {
                                     ISMMTCAT_Id = b.ISMMTCAT_Id,
                                     ISMMTCAT_TaskCategoryName = b.ISMMTCAT_TaskCategoryName,
                                     ISMMTCAT_TaskPercentage = b.ISMMTCAT_TaskPercentage,
                                     ISMMTCAT_TCRemarks = b.ISMMTCAT_TCRemarks,
                                     HRMD_Id = b.HRMD_Id,
                                     ISMMTCAT_EachTaskMaxDuration = b.ISMMTCAT_EachTaskMaxDuration,
                                     ISMMTCAT_DurationFlg = b.ISMMTCAT_DurationFlg,
                                     HRMD_DepartmentName = a.HRMD_DepartmentName,
                                     ISMMTCAT_ActiveFlag = b.ISMMTCAT_ActiveFlag,
                                     ISMMTCAT_CompulsoryFlg = b.ISMMTCAT_CompulsoryFlg,
                                     MI_Id = b.MI_Id,
                                     MI_Name = c.MI_Subdomain
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ISM_Master_TaskCategory_DTO save(ISM_Master_TaskCategory_DTO data)
        {
            try
            {


                if (data.ISMMTCAT_Id == 0)
                {
                    var mi_iddept = _context.HR_Master_Department.Where(t => t.HRMD_Id == data.HRMD_Id).ToList();

                    data.MI_Id = mi_iddept[0].MI_Id;

                    var duplicate = _context.ISM_Master_TaskCategory_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMTCAT_TaskCategoryName == data.ISMMTCAT_TaskCategoryName && t.HRMD_Id == data.HRMD_Id && t.ISMMTCAT_Id != 0).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        var exist1 = _context.ISM_Master_TaskCategory_DMO.Where(t => t.HRMD_Id == data.HRMD_Id && t.ISMMTCAT_CompulsoryFlg == true).ToList();
                        decimal yy = 0;

                        for (int f = 0; f < exist1.Count(); f++)
                        {
                            yy = exist1[f].ISMMTCAT_TaskPercentage + yy;
                        }
                        //if (yy <= 100)
                        //{
                        var zz = yy + data.ISMMTCAT_TaskPercentage;
                        if (data.ISMMTCAT_CompulsoryFlg != true)
                        {


                            ISM_Master_TaskCategory_DMO u = new ISM_Master_TaskCategory_DMO();
                            u.MI_Id = data.MI_Id;
                            u.ISMMTCAT_TaskCategoryName = data.ISMMTCAT_TaskCategoryName;
                            u.ISMMTCAT_TaskPercentage = data.ISMMTCAT_TaskPercentage;
                            u.ISMMTCAT_TCRemarks = data.ISMMTCAT_TCRemarks;
                            u.ISMMTCAT_EachTaskMaxDuration = data.ISMMTCAT_EachTaskMaxDuration;
                            u.ISMMTCAT_CompulsoryFlg = data.ISMMTCAT_CompulsoryFlg;
                            u.ISMMTCAT_DurationFlg = data.ISMMTCAT_DurationFlg;
                            u.ISMMTCAT_CreatedBy = data.UserId;
                            u.ISMMTCAT_UpdatedBy = data.UserId;
                            u.ISMMTCAT_CreatedDate = DateTime.Now;
                            u.ISMMTCAT_UpdatedDate = DateTime.Now;
                            u.HRMD_Id = data.HRMD_Id;
                            u.ISMMTCAT_ActiveFlag = true;
                            _context.Add(u);

                            var w = _context.SaveChanges();
                            if (w > 0)
                            {
                                data.msg = "saved";
                            }
                            else
                            {
                                data.msg = "failed";
                            }
                        }
                        else if (data.ISMMTCAT_CompulsoryFlg == true)
                        {
                            if (zz <= 100)
                            {
                                ISM_Master_TaskCategory_DMO u = new ISM_Master_TaskCategory_DMO();
                                u.MI_Id = data.MI_Id;
                                u.ISMMTCAT_TaskCategoryName = data.ISMMTCAT_TaskCategoryName;
                                u.ISMMTCAT_TaskPercentage = data.ISMMTCAT_TaskPercentage;
                                u.ISMMTCAT_TCRemarks = data.ISMMTCAT_TCRemarks;
                                u.ISMMTCAT_EachTaskMaxDuration = data.ISMMTCAT_EachTaskMaxDuration;
                                u.ISMMTCAT_CompulsoryFlg = data.ISMMTCAT_CompulsoryFlg;
                                u.ISMMTCAT_DurationFlg = data.ISMMTCAT_DurationFlg;
                                u.ISMMTCAT_CreatedBy = data.UserId;
                                u.ISMMTCAT_UpdatedBy = data.UserId;
                                u.ISMMTCAT_CreatedDate = DateTime.Now;
                                u.ISMMTCAT_UpdatedDate = DateTime.Now;
                                u.HRMD_Id = data.HRMD_Id;
                                u.ISMMTCAT_ActiveFlag = true;
                                _context.Add(u);
                                var w = _context.SaveChanges();
                                if (w > 0)
                                {
                                    data.msg = "saved";
                                }
                                else
                                {
                                    data.msg = "failed";
                                }
                            }
                            else if (zz > 100)
                            {
                                data.msg = "taskpercentagegreater";
                            }
                        }
                        //}
                    }
                }

                else if (data.ISMMTCAT_Id > 0)
                {
                    var exist1 = _context.ISM_Master_TaskCategory_DMO.Where(t => t.HRMD_Id == data.HRMD_Id && t.ISMMTCAT_CompulsoryFlg == true && t.ISMMTCAT_Id != data.ISMMTCAT_Id).ToList();
                    decimal yy = 0;

                    for (int f = 0; f < exist1.Count(); f++)
                    {
                        yy = exist1[f].ISMMTCAT_TaskPercentage + yy;
                    }
                    //if (yy <= 100)
                    //{
                    var zz = yy + data.ISMMTCAT_TaskPercentage;
                    if (data.ISMMTCAT_CompulsoryFlg != true)
                    {
                        var u = _context.ISM_Master_TaskCategory_DMO.Where(t => t.ISMMTCAT_Id == data.ISMMTCAT_Id).SingleOrDefault();
                        u.ISMMTCAT_TaskPercentage = data.ISMMTCAT_TaskPercentage;
                        u.ISMMTCAT_TCRemarks = data.ISMMTCAT_TCRemarks;
                        u.ISMMTCAT_EachTaskMaxDuration = data.ISMMTCAT_EachTaskMaxDuration;
                        u.ISMMTCAT_CompulsoryFlg = data.ISMMTCAT_CompulsoryFlg;
                        u.ISMMTCAT_DurationFlg = data.ISMMTCAT_DurationFlg;
                        u.ISMMTCAT_UpdatedBy = data.UserId;
                        u.ISMMTCAT_UpdatedDate = DateTime.Now;
                        u.HRMD_Id = data.HRMD_Id;
                        _context.Update(u);
                        var w = _context.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }

                    }
                    else if (data.ISMMTCAT_CompulsoryFlg == true)
                    {
                        if (zz <= 100)
                        {
                            var u = _context.ISM_Master_TaskCategory_DMO.Where(t => t.ISMMTCAT_Id == data.ISMMTCAT_Id).SingleOrDefault();
                            u.ISMMTCAT_TaskPercentage = data.ISMMTCAT_TaskPercentage;
                            u.ISMMTCAT_TCRemarks = data.ISMMTCAT_TCRemarks;
                            u.ISMMTCAT_EachTaskMaxDuration = data.ISMMTCAT_EachTaskMaxDuration;
                            u.ISMMTCAT_CompulsoryFlg = data.ISMMTCAT_CompulsoryFlg;
                            u.ISMMTCAT_DurationFlg = data.ISMMTCAT_DurationFlg;
                            u.ISMMTCAT_UpdatedBy = data.UserId;
                            u.ISMMTCAT_UpdatedDate = DateTime.Now;
                            u.HRMD_Id = data.HRMD_Id;
                            _context.Update(u);
                            var w = _context.SaveChanges();
                            if (w > 0)
                            {
                                data.msg = "updated";
                            }
                            else
                            {
                                data.msg = "Failed";
                            }
                        }
                        else if (zz > 100)
                        {
                            data.msg = "taskpercentagegreater";
                        }
                    }
                    //}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ISM_Master_TaskCategory_DTO deactive(ISM_Master_TaskCategory_DTO data)
        {
            try
            {
                var g = _context.ISM_Master_TaskCategory_DMO.Where(t => t.ISMMTCAT_Id == data.ISMMTCAT_Id).SingleOrDefault();
                if (g.ISMMTCAT_ActiveFlag == true)
                {
                    g.ISMMTCAT_ActiveFlag = false;
                }
                else
                {
                    g.ISMMTCAT_ActiveFlag = true;
                }
                g.ISMMTCAT_UpdatedDate = DateTime.Now;
                g.ISMMTCAT_UpdatedBy = data.UserId;
                g.MI_Id = data.MI_Id;
                _context.Update(g);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ISM_Master_TaskCategory_DTO EditData(ISM_Master_TaskCategory_DTO data)
        {
            try
            {

                data.editlist = (from a in _context.HR_Master_Department
                                 from b in _context.ISM_Master_TaskCategory_DMO
                                 from c in _context.Institution
                                 where (b.ISMMTCAT_Id == data.ISMMTCAT_Id && a.HRMD_Id == b.HRMD_Id && a.MI_Id == c.MI_Id && a.HRMD_ActiveFlag == true)
                                 select new ISM_Master_TaskCategory_DTO
                                 {
                                     ISMMTCAT_Id = b.ISMMTCAT_Id,
                                     ISMMTCAT_TaskCategoryName = b.ISMMTCAT_TaskCategoryName,
                                     ISMMTCAT_TaskPercentage = b.ISMMTCAT_TaskPercentage,
                                     ISMMTCAT_CompulsoryFlg = b.ISMMTCAT_CompulsoryFlg,
                                     ISMMTCAT_TCRemarks = b.ISMMTCAT_TCRemarks,
                                     HRMD_Id = b.HRMD_Id,
                                     ISMMTCAT_EachTaskMaxDuration = b.ISMMTCAT_EachTaskMaxDuration,
                                     ISMMTCAT_DurationFlg = b.ISMMTCAT_DurationFlg,
                                     MI_Id = b.MI_Id,
                                     HRMD_DepartmentName = a.HRMD_DepartmentName,
                                     MI_Name = c.MI_Name,
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        //============== Master Task Categary Check List===========
        public ISM_Master_TaskCategory_DTO loaddata_checklist(ISM_Master_TaskCategory_DTO dto)
        {
            try
            {
                var getmiids = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == dto.UserId).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getmiids)
                {
                    miids.Add(c.MI_Id);
                }

                dto.departmentlist = (from a in _context.HR_Master_Department
                                      from b in _context.Institution
                                      where (a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true && b.MI_ActiveFlag == 1 && miids.Contains(b.MI_Id))
                                      select new ISM_Master_TaskCategory_DTO
                                      {
                                          HRMD_Id = a.HRMD_Id,
                                          HRMD_DepartmentName = a.HRMD_DepartmentName,
                                          MI_Name = b.MI_Name,
                                          MI_Id = b.MI_Id,
                                      }).Distinct().OrderBy(a => a.MI_Name).ThenBy(a => a.HRMD_DepartmentName).ToArray();

                dto.checklist = (from a in _context.ISM_Master_TaskCategory_ChecklistDMO_con
                                 from b in _context.ISM_Master_TaskCategory_DMO
                                 from c in _context.HR_Master_Department
                                 from d in _context.Institution
                                 where a.ISMMTCAT_Id == b.ISMMTCAT_Id && b.HRMD_Id == c.HRMD_Id && c.MI_Id == d.MI_Id && d.MI_Id == b.MI_Id
                                 && miids.Contains(b.MI_Id)
                                 select new ISM_Master_TaskCategory_DTO
                                 {
                                     ISMMTCATCL_Id = a.ISMMTCATCL_Id,
                                     ISMMTCAT_TaskCategoryName = b.ISMMTCAT_TaskCategoryName,
                                     ISMMTCATCL_CheckListName = a.ISMMTCATCL_CheckListName,
                                     ISMMTCATCL_UploadEnterFlg = a.ISMMTCATCL_UploadEnterFlg,
                                     ISMMTCATCL_CheckListFlg = a.ISMMTCATCL_CheckListFlg,
                                     ISMMTCATCL_ActiveFlag = a.ISMMTCATCL_ActiveFlag,
                                     ISMMTCAT_CreatedDate = Convert.ToDateTime(a.ISMMTCATCL_CreatedDate),
                                     HRMD_DepartmentName = c.HRMD_DepartmentName,
                                     MI_Name = d.MI_Name
                                 }).Distinct().OrderBy(a => a.MI_Name).OrderBy(a => a.HRMD_DepartmentName).OrderByDescending(a => a.ISMMTCAT_CreatedDate).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public ISM_Master_TaskCategory_DTO getcategory(ISM_Master_TaskCategory_DTO dto)
        {
            try
            {
                dto.categorylist = _context.ISM_Master_TaskCategory_DMO.Where(t => t.HRMD_Id == dto.HRMD_Id && t.ISMMTCAT_ActiveFlag == true && t.MI_Id == dto.MI_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public ISM_Master_TaskCategory_DTO savechecklist(ISM_Master_TaskCategory_DTO dto)
        {
            try
            {
                if (dto.ISMMTCATCL_Id > 0)
                {
                    foreach (var item in dto.checklistarray)
                    {
                        var result = _context.ISM_Master_TaskCategory_ChecklistDMO_con.Single(a => a.ISMMTCATCL_Id == dto.ISMMTCATCL_Id);
                        result.ISMMTCATCL_CheckListName = item.ISMMTCATCL_CheckListName;
                        result.ISMMTCATCL_UploadEnterFlg = item.ISMMTCATCL_UploadEnterFlg;
                        result.ISMMTCAT_Id = dto.ISMMTCAT_Id;
                        result.ISMMTCATCL_UpdatedBy = dto.UserId;
                        result.ISMMTCATCL_UpdatedDate = DateTime.Today;
                        _context.Update(result);
                    }
                    var chck = _context.SaveChanges();
                    if (chck > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    foreach (var item in dto.checklistarray)
                    {
                        ISM_Master_TaskCategory_ChecklistDMO TC = new ISM_Master_TaskCategory_ChecklistDMO();
                        TC.ISMMTCATCL_CheckListName = item.ISMMTCATCL_CheckListName;
                        TC.ISMMTCATCL_UploadEnterFlg = item.ISMMTCATCL_UploadEnterFlg;
                        TC.ISMMTCAT_Id = dto.ISMMTCAT_Id;
                        TC.ISMMTCATCL_ActiveFlag = true;
                        TC.ISMMTCATCL_CreatedBy = dto.UserId;
                        TC.ISMMTCATCL_CreatedDate = DateTime.Today;
                        _context.Add(TC);
                    }
                    var chck = _context.SaveChanges();
                    if (chck > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public ISM_Master_TaskCategory_DTO EditData_checklist(ISM_Master_TaskCategory_DTO dto)
        {
            try
            {
                var getmiids = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == dto.UserId).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getmiids)
                {
                    miids.Add(c.MI_Id);
                }

                var editdata = _context.ISM_Master_TaskCategory_ChecklistDMO_con.Where(a => a.ISMMTCATCL_Id == dto.ISMMTCATCL_Id).ToArray();
                if (editdata.Length > 0)
                {
                    dto.checklistedit = editdata;
                }

                var ed = (from a in _context.HR_Master_Department
                          from b in _context.Institution
                          from c in _context.ISM_Master_TaskCategory_DMO
                          where (a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true && b.MI_ActiveFlag == 1 && a.HRMD_Id == c.HRMD_Id
                          && c.ISMMTCAT_Id == editdata[0].ISMMTCAT_Id)
                          select new ISM_Master_TaskCategory_DTO
                          {
                              HRMD_Id = a.HRMD_Id,

                          }).Distinct().OrderByDescending(a => a.HRMD_Order).ToArray();

                if (ed.Length > 0)
                {
                    dto.departmentlistedit = ed;
                }
                dto.categorylist = _context.ISM_Master_TaskCategory_DMO.Where(t => t.HRMD_Id == ed[0].HRMD_Id && t.ISMMTCAT_ActiveFlag == true
                && miids.Contains(t.MI_Id)).Distinct().ToArray();

                dto.departmentlist = (from a in _context.HR_Master_Department
                                      from b in _context.Institution
                                      where (a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true && b.MI_ActiveFlag == 1 && miids.Contains(b.MI_Id))
                                      select new ISM_Master_TaskCategory_DTO
                                      {
                                          HRMD_Id = a.HRMD_Id,
                                          HRMD_DepartmentName = a.HRMD_DepartmentName,
                                          MI_Name = b.MI_Name,
                                          MI_Id = b.MI_Id
                                      }).Distinct().OrderBy(a => a.MI_Name).ThenBy(a => a.HRMD_DepartmentName).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public ISM_Master_TaskCategory_DTO deactive_checklist(ISM_Master_TaskCategory_DTO dto)
        {
            try
            {
                if (dto.ISMMTCATCL_ActiveFlag == true)
                {
                    var result = _context.ISM_Master_TaskCategory_ChecklistDMO_con.Single(a => a.ISMMTCATCL_Id == dto.ISMMTCATCL_Id);
                    result.ISMMTCATCL_ActiveFlag = false;
                    result.ISMMTCATCL_UpdatedBy = dto.UserId;
                    result.ISMMTCATCL_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var chck = _context.SaveChanges();
                    if (chck > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    var result = _context.ISM_Master_TaskCategory_ChecklistDMO_con.Single(a => a.ISMMTCATCL_Id == dto.ISMMTCATCL_Id);
                    result.ISMMTCATCL_ActiveFlag = true;
                    result.ISMMTCATCL_UpdatedBy = dto.UserId;
                    result.ISMMTCATCL_UpdatedDate = DateTime.Today;
                    _context.Update(result);
                    var chck = _context.SaveChanges();
                    if (chck > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
    }
}