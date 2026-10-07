using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_Master_TaskCategory_EmployeeImpl : Interfaces.ISM_Master_TaskCategory_EmployeeInterface
    {
        public IssueManagerContext _context;
        public ISM_Master_TaskCategory_EmployeeImpl(IssueManagerContext w)
        {
            _context = w;
        }
        public ISM_Master_TaskCategory_Employee_DTO loaddata(ISM_Master_TaskCategory_Employee_DTO data)
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

                data.alldata1 = (from a in _context.ISM_Master_TaskCategory_Employee_DMO
                                 from b in _context.MasterEmployee
                                 from c in _context.ISM_Master_TaskCategory_DMO
                                 from d in _context.HR_Master_Department
                                 from e in _context.Institution
                                 where (a.HRME_Id == b.HRME_Id && a.ISMMTCAT_Id == c.ISMMTCAT_Id && d.HRMD_Id==c.HRMD_Id && b.MI_Id==e.MI_Id && d.MI_Id==e.MI_Id && b.HRME_ActiveFlag==true && b.HRME_LeftFlag==false)
                                 select new ISM_Master_TaskCategory_Employee_DTO
                                 {
                                     ISMMTCATE_Id = a.ISMMTCATE_Id,
                                     HRME_Id = a.HRME_Id,
                                     ISMMTCAT_Id = a.ISMMTCAT_Id,
                                     HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName)
                                     + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" :
                                                           " " + b.HRME_EmployeeMiddleName)
                                                           + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" :
                                                           " " + b.HRME_EmployeeLastName)
                                                           + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" :
                                                           " : " + b.HRME_EmployeeCode)).Trim(),
                                     ISMMTCAT_TaskCategoryName = c.ISMMTCAT_TaskCategoryName,
                                     ISMMTCATE_TaskPercentage = a.ISMMTCATE_TaskPercentage,
                                     HRMD_Id = c.HRMD_Id,
                                     ISMMTCAT_CompulsoryFlg = c.ISMMTCAT_CompulsoryFlg,
                                     ISMMTCATE_ActiveFlag = a.ISMMTCATE_ActiveFlag,
                                     ISMMTCATE_CompulsoryFlg = a.ISMMTCATE_CompulsoryFlg,
                                     HRMD_DepartmentName=d.HRMD_DepartmentName,
                                     MI_Name=e.MI_Subdomain
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ISM_Master_TaskCategory_Employee_DTO save(ISM_Master_TaskCategory_Employee_DTO FGpage)
        {
            string retval = "";
            try
            {
                if (FGpage.ISMMTCATE_Id > 0)
                {
                    for (int q = 0; q < FGpage.emplcategorylist.Length; q++)
                    {
                        var removelist1 = _context.ISM_Master_TaskCategory_Employee_DMO.Where(t => t.HRME_Id == FGpage.emplcategorylist[q].HRME_Id&&t.ISMMTCAT_Id!= FGpage.emplcategorylist[q].ISMMTCAT_Id).ToList();
                        if (removelist1.Count > 0)
                        {
                            foreach (var item in removelist1)
                            {
                                _context.Remove(item);
                            }
                        }
                    }
                        for (int q = 0; q < FGpage.emplcategorylist.Length; q++)
                    {
                        var exist11 = _context.ISM_Master_TaskCategory_Employee_DMO.Where(t => t.ISMMTCAT_Id == FGpage.emplcategorylist[q].ISMMTCAT_Id && t.HRME_Id == FGpage.emplcategorylist[q].HRME_Id).ToList();

                        if (exist11.Count> 0)
                        {
                            
                            var updating = _context.ISM_Master_TaskCategory_Employee_DMO.Where(t => t.HRME_Id == FGpage.emplcategorylist[q].HRME_Id && t.ISMMTCAT_Id == FGpage.emplcategorylist[q].ISMMTCAT_Id).SingleOrDefault();
                            updating.ISMMTCATE_CompulsoryFlg = FGpage.emplcategorylist[q].ISMMTCATE_CompulsoryFlg;
                            updating.ISMMTCATE_UpdatedDate = DateTime.Now;
                            updating.ISMMTCATE_UpdatedBy = FGpage.UserId;                            
                            _context.Update(updating);
                            var contactExists2 = _context.SaveChanges();
                            if (contactExists2 == 1)
                            {
                                FGpage.message = "update";
                                retval = "save";
                                FGpage.returnduplicatestatus = retval;
                            }
                            else
                            {
                                FGpage.message = "noupdate";
                                retval = "notsave";
                                FGpage.returnduplicatestatus = retval;
                            }

                        }
                        else
                        {
                            ISM_Master_TaskCategory_Employee_DMO newfeepge1 = new ISM_Master_TaskCategory_Employee_DMO();
                            var task1 = _context.ISM_Master_TaskCategory_DMO.Where(t => t.ISMMTCAT_Id == FGpage.emplcategorylist[q].ISMMTCAT_Id).ToList();
                            newfeepge1.ISMMTCATE_TaskPercentage = task1[0].ISMMTCAT_TaskPercentage;
                            newfeepge1.ISMMTCATE_UpdatedDate = DateTime.Now;
                            newfeepge1.ISMMTCATE_CreatedDate = DateTime.Now;
                            newfeepge1.HRME_Id = FGpage.emplcategorylist[q].HRME_Id;
                            newfeepge1.ISMMTCATE_CompulsoryFlg = FGpage.emplcategorylist[q].ISMMTCATE_CompulsoryFlg;
                            newfeepge1.ISMMTCATE_UpdatedBy = FGpage.UserId;
                            newfeepge1.ISMMTCATE_CreatedBy = FGpage.UserId;
                            newfeepge1.ISMMTCATE_ActiveFlag = true;
                            newfeepge1.ISMMTCAT_Id = FGpage.emplcategorylist[q].ISMMTCAT_Id;
                            _context.Add(newfeepge1);
                            var contactExists2 = _context.SaveChanges();
                            if (contactExists2 == 1)
                            {
                                FGpage.message = "update";
                                retval = "save";
                                FGpage.returnduplicatestatus = retval;
                            }
                            else
                            {
                                FGpage.message = "noupdate";
                                retval = "notsave";
                                FGpage.returnduplicatestatus = retval;
                            }
                        }
                    }
                }
                if (FGpage.ISMMTCATE_Id == 0)
                {
                   for(int rr = 0; rr < FGpage.emplcategorylist.Length; rr++)
                    {
                        var duplicate = _context.ISM_Master_TaskCategory_Employee_DMO.Where(t => t.HRME_Id == FGpage.emplcategorylist[rr].HRME_Id && t.ISMMTCAT_Id == FGpage.emplcategorylist[rr].ISMMTCAT_Id).ToList();
                        if (duplicate.Count > 0)
                        {
                            FGpage.message = "duplicate";
                        }
                        else
                        {
                            ISM_Master_TaskCategory_Employee_DMO newfeepge3 = new ISM_Master_TaskCategory_Employee_DMO();
                            var task1 = _context.ISM_Master_TaskCategory_DMO.Where(t => t.ISMMTCAT_Id == FGpage.emplcategorylist[rr].ISMMTCAT_Id).ToList();
                            newfeepge3.ISMMTCATE_TaskPercentage = task1[0].ISMMTCAT_TaskPercentage;
                            newfeepge3.ISMMTCATE_UpdatedDate = DateTime.Now;
                            newfeepge3.ISMMTCATE_CreatedDate = DateTime.Now;
                            newfeepge3.HRME_Id = FGpage.emplcategorylist[rr].HRME_Id;
                            newfeepge3.ISMMTCATE_UpdatedBy = FGpage.UserId;
                            newfeepge3.ISMMTCATE_CreatedBy = FGpage.UserId;
                            newfeepge3.ISMMTCATE_CompulsoryFlg = FGpage.emplcategorylist[rr].ISMMTCATE_CompulsoryFlg;
                            newfeepge3.ISMMTCATE_ActiveFlag = true;
                            newfeepge3.ISMMTCAT_Id = FGpage.emplcategorylist[rr].ISMMTCAT_Id;
                            _context.Add(newfeepge3);
                            var contactExists2 = _context.SaveChanges();
                            if (contactExists2 == 1)
                            {
                                FGpage.message = "save";
                                retval = "save";
                                FGpage.returnduplicatestatus = retval;
                            }
                            else
                            {
                                FGpage.message = "nosave";
                                retval = "notsave";
                                FGpage.returnduplicatestatus = retval;
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }
        public ISM_Master_TaskCategory_Employee_DTO deactive(ISM_Master_TaskCategory_Employee_DTO data)
        {
            try
            {
                var g = _context.ISM_Master_TaskCategory_Employee_DMO.Where(t => t.ISMMTCATE_Id == data.ISMMTCATE_Id).SingleOrDefault();
                if (g.ISMMTCATE_ActiveFlag == true)
                {
                    g.ISMMTCATE_ActiveFlag = false;
                }
                else
                {
                    g.ISMMTCATE_ActiveFlag = true;
                }
                g.ISMMTCATE_UpdatedDate = DateTime.Now;
                g.ISMMTCATE_UpdatedBy = data.UserId;

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
        public ISM_Master_TaskCategory_Employee_DTO EditData(ISM_Master_TaskCategory_Employee_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.MasterEmployee
                                 from b in _context.ISM_Master_TaskCategory_DMO
                                 from c in _context.ISM_Master_TaskCategory_Employee_DMO
                                 from d in _context.HR_Master_Department
                                 from e in _context.Institution

                                 where (c.ISMMTCATE_Id == data.ISMMTCATE_Id && c.ISMMTCAT_Id == b.ISMMTCAT_Id && a.HRME_Id == c.HRME_Id&&d.HRMD_Id==b.HRMD_Id&&b.HRMD_Id==a.HRMD_Id&&a.HRMD_Id==b.HRMD_Id&& d.MI_Id == e.MI_Id)
                                 select new ISM_Master_TaskCategory_Employee_DTO
                                 {
                                     ISMMTCATE_Id = c.ISMMTCATE_Id,
                                     ISMMTCAT_TaskCategoryName = b.ISMMTCAT_TaskCategoryName,
                                     ISMMTCAT_Id = c.ISMMTCAT_Id,
                                     HRMD_Id = b.HRMD_Id,
                                     HRMD_DepartmentName = d.HRMD_DepartmentName,
                                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                     + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" :
                                                           " " + a.HRME_EmployeeMiddleName)
                                                           + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" :
                                                           " " + a.HRME_EmployeeLastName)
                                                           + (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" :
                                                           " : " + a.HRME_EmployeeCode)).Trim(),

                                     HRME_Id = c.HRME_Id,
                                     ISMMTCATE_TaskPercentage = c.ISMMTCATE_TaskPercentage,
                                     MI_Name = e.MI_Name,
                                 }).Distinct().ToArray();
                data.categorylist = _context.ISM_Master_TaskCategory_DMO.Where(t =>  t.ISMMTCAT_ActiveFlag == true&&t.HRMD_Id==data.HRMD_Id).Distinct().ToArray();

                data.employeelist = (from a in _context.MasterEmployee
                                     where (a.HRMD_Id == data.HRMD_Id && a.HRME_ActiveFlag == true&& a.HRME_LeftFlag == false&&a.HRME_Id==data.HRME_Id)
                                     select new ISM_Master_TaskCategory_Employee_DTO
                                     {
                                         HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                         HRME_Id = a.HRME_Id,
                                     }).Distinct().ToArray();
                data.departmentlist = _context.HR_Master_Department.Where(t => t.HRMD_ActiveFlag == true).Distinct().OrderByDescending(t => t.HRMD_Order).ToArray();

                data.catlistselected = _context.ISM_Master_TaskCategory_Employee_DMO.Where(t =>t.HRME_Id==data.HRME_Id).Distinct().ToArray();
                data.selectflg = _context.ISM_Master_TaskCategory_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id && t.ISMMTCATE_CompulsoryFlg == true).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public ISM_Master_TaskCategory_Employee_DTO getcategory(ISM_Master_TaskCategory_Employee_DTO data)
        {
            try
            {
                data.employeelist = (from a in _context.MasterEmployee
                                     where (a.HRMD_Id == data.HRMD_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag==false)
                                     select new ISM_Master_TaskCategory_Employee_DTO
                                     {
                                         HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + "" + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : " "+a.HRME_EmployeeMiddleName) + "" + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : " "+a.HRME_EmployeeLastName)).Trim(),
                                         HRME_Id = a.HRME_Id,
                                     }).Distinct().ToArray();

                data.categorylist = _context.ISM_Master_TaskCategory_DMO.Where(t =>  t.HRMD_Id == data.HRMD_Id && t.ISMMTCAT_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
