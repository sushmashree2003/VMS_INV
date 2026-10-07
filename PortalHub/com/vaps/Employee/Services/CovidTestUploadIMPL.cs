using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.VMS.Training;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PortalHub.com.vaps.Employee.Services
{
    public class CovidTestUploadIMPL : Interfaces.CovidTestUploadInterface
    {
        public PortalContext _context;
        public CovidTestUploadIMPL(PortalContext _con)
        {
            _context = _con;
        }

        public CovidTestUploadDTO onloaddata(CovidTestUploadDTO data)
        {
            try
            {
                data.hrmeid = (from a in _context.ApplicationUser
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.stdid = (from a in _context.ApplicationUser
                              from b in _context.StudentUserLoginDMO
                              where (a.Id == b.IVRMSTUUL_Id && a.Id == data.Userid)
                              select b).Select(t => t.AMST_Id).FirstOrDefault();

                data.RoleType = (from a in _context.ApplicationRole_con
                                 where (a.Id == data.roleid)
                                 select a).Select(t => t.roleType).FirstOrDefault();

                
                    data.getloaddetails = _context.Staff_CovidTestDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == data.hrmeid).ToArray();
                
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CovidTestUploadDTO saverecord(CovidTestUploadDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.RoleType = (from a in _context.ApplicationRole_con
                                 where (a.Id == data.roleid)
                                 select a).Select(t => t.roleType).FirstOrDefault();

                data.hrmeid = (from a in _context.ApplicationUser
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.stdid = (from a in _context.ApplicationUser
                              from b in _context.StudentUserLoginDMO
                              where (a.Id == b.IVRMSTUUL_Id && a.Id == data.Userid)
                              select b).Select(t => t.AMST_Id).FirstOrDefault();

               
                    if (data.ISTCOVTST_Id > 0)
                    {
                       
                            var checkresult = _context.Staff_CovidTestDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTCOVTST_Id == data.ISTCOVTST_Id);
                            checkresult.ISTCOVTST_Id = data.ISTCOVTST_Id;
                            checkresult.ISTCOVTST_TestDate = data.ISTCOVTST_TestDate;
                            checkresult.HRME_Id = data.hrmeid;
                            checkresult.ISTCOVTST_TestResult = data.ISTCOVTST_TestResult;
                            checkresult.ISTCOVTST_FileName = data.ISTCOVTST_FileName;
                            checkresult.ISTCOVTST_FilePath = data.ISTCOVTST_FilePath;
                            checkresult.ISTCOVTST_UpdatedBy = data.Userid;
                            checkresult.ISTCOVTST_UpdatedDate = indiantime0;
                            _context.Update(checkresult);
                            var kk = _context.SaveChanges();
                            if (kk > 0)
                            {
                                data.returnval = true;
                                data.message = "Update";
                            }
                            else
                            {
                                data.returnval = false;
                                data.message = "Update";
                            }
                        
                    }
                    else
                    {
                        var checkduplicate = _context.Staff_CovidTestDMO.Where(a => a.MI_Id == data.MI_Id && a.ISTCOVTST_Id == data.ISTCOVTST_Id).ToList();
                        if (checkduplicate.Count > 0)
                        {
                            data.message = "Duplicate";
                        }
                        else
                        {
                            Staff_CovidTestDMO obj = new Staff_CovidTestDMO();
                            obj.MI_Id = data.MI_Id;
                            obj.ISTCOVTST_TestDate = data.ISTCOVTST_TestDate;
                            obj.HRME_Id = data.hrmeid;
                            obj.ISTCOVTST_TestResult = data.ISTCOVTST_TestResult;
                            obj.ISTCOVTST_FileName = data.ISTCOVTST_FileName;
                            obj.ISTCOVTST_FilePath = data.ISTCOVTST_FilePath;
                            obj.ISTCOVTST_ActiveFlag = true;
                            obj.ISTCOVTST_CreatedBy = data.Userid;
                            obj.ISTCOVTST_UpdatedBy = data.Userid;
                            obj.ISTCOVTST_CreatedDate = indiantime0;
                            obj.ISTCOVTST_UpdatedDate = indiantime0;
                            _context.Add(obj);
                            var i = _context.SaveChanges();
                            if (i > 0)
                            {
                                data.message = "Add";
                                data.returnval = true;
                            }
                            else
                            {
                                data.message = "Add";
                                data.returnval = false;
                            }
                        }
                    }
                
              

            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CovidTestUploadDTO deactiveY(CovidTestUploadDTO data)
        {
            try
            {

                data.RoleType = (from a in _context.ApplicationRole_con
                                 where (a.Id == data.roleid)
                                 select a).Select(t => t.roleType).FirstOrDefault();

                data.hrmeid = (from a in _context.ApplicationUser
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.stdid = (from a in _context.ApplicationUser
                              from b in _context.StudentUserLoginDMO
                              where (a.Id == b.IVRMSTUUL_Id && a.Id == data.Userid)
                              select b).Select(t => t.AMST_Id).FirstOrDefault();
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
               
                    var checkactivestatus = _context.Staff_CovidTestDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTCOVTST_Id == data.ISTCOVTST_Id);

                    if (checkactivestatus.ISTCOVTST_ActiveFlag == true)
                    {
                        var resultdeactive = _context.Staff_CovidTestDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTCOVTST_Id == data.ISTCOVTST_Id);

                        if (resultdeactive.ISTCOVTST_ActiveFlag == true)
                        {
                            resultdeactive.ISTCOVTST_ActiveFlag = false;
                        }
                        else
                        {
                            resultdeactive.ISTCOVTST_ActiveFlag = true;
                        }

                        resultdeactive.ISTCOVTST_UpdatedDate = indiantime0;
                        resultdeactive.ISTCOVTST_UpdatedBy = data.Userid;
                        _context.Update(resultdeactive);

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                    else
                    {
                        var resultdeactive = _context.Staff_CovidTestDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTCOVTST_Id == data.ISTCOVTST_Id);

                        if (resultdeactive.ISTCOVTST_ActiveFlag == true)
                        {
                            resultdeactive.ISTCOVTST_ActiveFlag = false;
                        }
                        else
                        {
                            resultdeactive.ISTCOVTST_ActiveFlag = true;
                        }

                        resultdeactive.ISTCOVTST_UpdatedDate = indiantime0;
                        resultdeactive.ISTCOVTST_UpdatedBy = data.Userid;
                        _context.Update(resultdeactive);

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
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

