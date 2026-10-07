using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class ClassWiseDailyAttendanceImpl : Interfaces.ClassWiseDailyAttendanceInterface
    {
        private static ConcurrentDictionary<string, castecategoryDTO> _login =
        new ConcurrentDictionary<string, castecategoryDTO>();

        private readonly ClassWiseDailyAttendanceContext _classWiseDailyAttendanceContext;
        public ClassWiseDailyAttendanceImpl(ClassWiseDailyAttendanceContext castecategoryContext)
        {
            _classWiseDailyAttendanceContext = castecategoryContext;
        }
        public SchoolYearWiseStudentDTO GetddlDatabind(SchoolYearWiseStudentDTO mas)
        {

            var check_rolename = (from a in _classWiseDailyAttendanceContext.MasterRoleType
                                  where (a.IVRMRT_Id == mas.roleId)
                                  select new StudentAttendanceEntryDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }
                                  ).ToList();

            var empcode_check = (from a in _classWiseDailyAttendanceContext.Staff_User_Login
                                 where (a.MI_Id == mas.MI_Id && a.IVRMSTAUL_UserName.Equals(mas.username.Trim()))
                                 select new StudentAttendanceEntryDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.Equals("ADMIN") || check_rolename.FirstOrDefault().rolename.Equals("admin"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> aya3 = new List<School_M_Section>();
                aya3 = _classWiseDailyAttendanceContext.section.Where(d => d.MI_Id == mas.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                mas.sectionList = aya3.OrderBy(s => s.ASMC_Order).ToArray();
            }
            else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> aya3 = new List<School_M_Section>();
                aya3 = _classWiseDailyAttendanceContext.section.Where(d => d.MI_Id == mas.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                mas.sectionList = aya3.OrderBy(s => s.ASMC_Order).ToArray();
            }
            else if (check_rolename.FirstOrDefault().rolename.Equals("HOD") || check_rolename.FirstOrDefault().rolename.Equals("Hod"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> aya3 = new List<School_M_Section>();
                aya3 = _classWiseDailyAttendanceContext.section.Where(d => d.MI_Id == mas.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                mas.sectionList = aya3.OrderBy(s => s.ASMC_Order).ToArray();
            }
            else if (check_rolename.FirstOrDefault().rolename.Equals("Super Admin"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> aya3 = new List<School_M_Section>();
                aya3 = _classWiseDailyAttendanceContext.section.Where(d => d.MI_Id == mas.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                mas.sectionList = aya3.OrderBy(s => s.ASMC_Order).ToArray();
            }

            else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User")
                || check_rolename.FirstOrDefault().rolename.Equals("COORDINATOR"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> aya3 = new List<School_M_Section>();
                aya3 = _classWiseDailyAttendanceContext.section.Where(d => d.MI_Id == mas.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                mas.sectionList = aya3.OrderBy(s => s.ASMC_Order).ToArray();
            }

            else
            {
                if (empcode_check.Count > 0)
                {
                    mas.classList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                     from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                     from c in _classWiseDailyAttendanceContext.classs
                                     where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                     && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                     && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                     && c.ASMCL_ActiveFlag == true)
                                     select new StudentAttendanceEntryDTO
                                     {
                                         ASMCL_Id = c.ASMCL_Id,
                                         asmcL_ClassName = c.ASMCL_ClassName,
                                     }
                              ).Distinct().ToArray();


                    mas.sectionList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                       from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                       from c in _classWiseDailyAttendanceContext.section
                                       where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                       && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                       && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                       && c.ASMC_ActiveFlag == 1)
                                       select new StudentAttendanceEntryDTO
                                       {
                                           ASMS_Id = c.ASMS_Id,
                                           asmC_SectionName = c.ASMC_SectionName,
                                       }
                                        ).Distinct().ToArray();



                }
                else
                {
                    //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                }
            }

            List<AcademicYear> aya = new List<AcademicYear>();
            aya = _classWiseDailyAttendanceContext.academicyr.Where(d => d.MI_Id == mas.MI_Id && d.Is_Active == true).ToList();
            mas.YearList = aya.OrderByDescending(a => a.ASMAY_Order).ToArray();

            List<AcademicYear> defaultyear = new List<AcademicYear>();
            defaultyear = _classWiseDailyAttendanceContext.academicyr.Where(d => d.MI_Id == mas.MI_Id && d.ASMAY_Id == mas.ASMAY_Id && d.Is_Active == true).ToList();
            mas.defalutYearList = defaultyear.OrderByDescending(a => a.ASMAY_Order).ToArray();

            return mas;
        }
        public SchoolYearWiseStudentDTO getsection(SchoolYearWiseStudentDTO mas)
        {

            var check_rolename = (from a in _classWiseDailyAttendanceContext.MasterRoleType
                                  where (a.IVRMRT_Id == mas.roleId)
                                  select new StudentAttendanceEntryDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }
                                  ).ToList();

            var empcode_check = (from a in _classWiseDailyAttendanceContext.Staff_User_Login
                                 where (a.MI_Id == mas.MI_Id && a.IVRMSTAUL_UserName.Equals(mas.username.Trim()))
                                 select new StudentAttendanceEntryDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.Equals("Staff") || check_rolename.FirstOrDefault().rolename.Equals("STAFF"))
            {

                if (empcode_check.Count > 0)
                {
                    mas.sectionList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                       from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                       from c in _classWiseDailyAttendanceContext.section
                                       from d in _classWiseDailyAttendanceContext.Masterclasscategory
                                       from e in _classWiseDailyAttendanceContext.AdmSchoolMasterClassCatSec
                                       where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                       && d.ASMCC_Id == e.ASMCC_Id && d.ASMAY_Id == mas.ASMAY_Id && d.ASMCL_Id == mas.ASMCL_Id
                                       && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                       && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                       && c.ASMC_ActiveFlag == 1)
                                       select new StudentAttendanceEntryDTO
                                       {
                                           ASMS_Id = c.ASMS_Id,
                                           asmC_SectionName = c.ASMC_SectionName,

                                       }).Distinct().ToArray();
                }
                else
                {
                    var sectiondata = (from a in _classWiseDailyAttendanceContext.classs
                                       from b in _classWiseDailyAttendanceContext.section
                                       from c in _classWiseDailyAttendanceContext.academicyr
                                       from d in _classWiseDailyAttendanceContext.Masterclasscategory
                                       from e in _classWiseDailyAttendanceContext.AdmSchoolMasterClassCatSec
                                       where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                                       && d.MI_Id == mas.MI_Id && d.ASMAY_Id == mas.ASMAY_Id && d.ASMCL_Id == mas.ASMCL_Id)
                                       select b
                                ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();


                    if (sectiondata.Length > 0)
                    {
                        mas.sectionList = sectiondata.ToArray();
                    }
                    else
                    {
                        List<School_M_Section> aya3 = new List<School_M_Section>();
                        aya3 = _classWiseDailyAttendanceContext.section.Where(d => d.MI_Id == mas.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                        mas.sectionList = aya3.OrderBy(s => s.ASMC_Order).ToArray();
                    }

                }

            }
            else
            {
                var sectiondata = (from a in _classWiseDailyAttendanceContext.classs
                                   from b in _classWiseDailyAttendanceContext.section
                                   from c in _classWiseDailyAttendanceContext.academicyr
                                   from d in _classWiseDailyAttendanceContext.Masterclasscategory
                                   from e in _classWiseDailyAttendanceContext.AdmSchoolMasterClassCatSec
                                   where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                                   && d.MI_Id == mas.MI_Id && d.ASMAY_Id == mas.ASMAY_Id && d.ASMCL_Id == mas.ASMCL_Id)
                                   select b
                                ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();


                if (sectiondata.Length > 0)
                {
                    mas.sectionList = sectiondata.ToArray();
                }
                else
                {
                    List<School_M_Section> aya3 = new List<School_M_Section>();
                    aya3 = _classWiseDailyAttendanceContext.section.Where(d => d.MI_Id == mas.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                    mas.sectionList = aya3.OrderBy(s => s.ASMC_Order).ToArray();
                }

            }

            return mas;

        }
        public SchoolYearWiseStudentDTO setfromdate(SchoolYearWiseStudentDTO mas)
        {
            var check_rolename = (from a in _classWiseDailyAttendanceContext.MasterRoleType
                                  where (a.IVRMRT_Id == mas.roleId)
                                  select new StudentAttendanceEntryDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }
                                              ).ToList();

            var empcode_check = (from a in _classWiseDailyAttendanceContext.Staff_User_Login
                                 where (a.MI_Id == mas.MI_Id && a.IVRMSTAUL_UserName.Equals(mas.username.Trim()))
                                 select new StudentAttendanceEntryDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.Equals("ADMIN") || check_rolename.FirstOrDefault().rolename.Equals("admin"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();
            }
            else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();
            }
            else if (check_rolename.FirstOrDefault().rolename.Equals("HOD") || check_rolename.FirstOrDefault().rolename.Equals("Hod"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();
            }

            else if (check_rolename.FirstOrDefault().rolename.Equals("COORDINATOR"))
            {
                List<School_M_Class> aya1 = new List<School_M_Class>();
                aya1 = _classWiseDailyAttendanceContext.classs.Where(d => d.MI_Id == mas.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                mas.classList = aya1.OrderBy(c => c.ASMCL_Order).ToArray();
            }

            else
            {
                if (empcode_check.Count > 0)
                {
                    mas.classList = (from a in _classWiseDailyAttendanceContext.Adm_SchAttLoginUserClass
                                     from b in _classWiseDailyAttendanceContext.Adm_SchAttLoginUser
                                     from c in _classWiseDailyAttendanceContext.classs
                                     where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                     && b.MI_Id == mas.MI_Id && b.ASMAY_Id == mas.ASMAY_Id
                                     && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                     && c.ASMCL_ActiveFlag == true)
                                     select new StudentAttendanceEntryDTO
                                     {
                                         ASMCL_Id = c.ASMCL_Id,
                                         asmcL_ClassName = c.ASMCL_ClassName,
                                     }
                              ).Distinct().ToArray();
                }
                else
                {
                    //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                }
            }

            return mas;

        }

    }
}
