using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AttendanceReportImpl : Interfaces.AttendanceReportInterface
    {
        public StudentAttendanceReportContext _db;
        private object c;
        ILogger<AttendanceReportImpl> _acdimpl;
        public AttendanceReportImpl(StudentAttendanceReportContext db, ILogger<AttendanceReportImpl> acdimpl)
        {
            _db = db;
            _acdimpl = acdimpl;
        }
        public async Task<StudentAttendanceReportDTO> getInitailData(StudentAttendanceReportDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(d => d.MI_Id == ctdo.miid && d.Is_Active == true).ToListAsync();
                ctdo.academicList = allyear.OrderByDescending(y => y.ASMAY_Order).ToArray();

                List<MasterAcademic> defaultyear = new List<MasterAcademic>();
                defaultyear = _db.academicYear.Where(t => t.MI_Id == ctdo.miid && t.Is_Active == true && t.ASMAY_Id == ctdo.ASMAY_Id).ToList();
                ctdo.academicListdefault = defaultyear.OrderByDescending(a => a.ASMAY_Order).ToArray();

                var photopath = _db.standarad.Where(t => t.MI_Id == ctdo.miid).ToList();
                if (photopath.Count > 0)
                {
                    ctdo.photopathname = photopath.FirstOrDefault().ASC_Logo_Path;
                }


                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == ctdo.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                 ).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == ctdo.miid && a.IVRMSTAUL_UserName.Equals(ctdo.username.Trim()))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                if (check_rolename.FirstOrDefault().rolename.Equals("ADMIN") || check_rolename.FirstOrDefault().rolename.Equals("admin"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Super Admin"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }

                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("COORDINATOR"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }

                else
                {
                    if (empcode_check.Count > 0)
                    {
                        ctdo.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }
                                  ).Distinct().ToArray();


                        ctdo.SectionList = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new StudentAttendanceReportDTO
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                            }
                                            ).Distinct().ToArray();



                    }
                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }


                //List<School_M_Class> allclass = new List<School_M_Class>();
                //allclass = await _db.admissionClass.Where(d=>d.MI_Id== ctdo.miid && d.ASMCL_ActiveFlag==true).ToListAsync();
                //ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                //List<School_M_Section> allsection = new List<School_M_Section>();
                //allsection = await _db.masterSection.Where(d=>d.MI_Id== ctdo.miid && d.ASMC_ActiveFlag==1).ToListAsync();
                //ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public async Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO ctdo)
        {
            var asmclid = "";
            var asmsid = "";
            int k = 0;
            var check_rolename = (from a in _db.MasterRoleType
                                  where (a.IVRMRT_Id == ctdo.roleId)
                                  select new StudentAttendanceReportDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }
                                 ).ToList();

            var empcode_check = (from a in _db.Staff_User_Login
                                 where (a.MI_Id == ctdo.miid && a.IVRMSTAUL_UserName.Equals(ctdo.username.Trim()))
                                 select new StudentAttendanceReportDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.Equals("STAFF") || check_rolename.FirstOrDefault().rolename.Equals("staff") || check_rolename.FirstOrDefault().rolename.Equals("Staff"))
            {
                k = 1;
                if (ctdo.type == '1' || ctdo.type == 1)
                {
                    if (empcode_check.Count > 0)
                    {
                        var classlist1 = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }
                                  ).Distinct().ToList();

                        for (int i = 0; i < classlist1.Count; i++)
                        {
                            if (i == 0)
                            {
                                asmclid = classlist1[i].ASMCL_Id.ToString();
                            }
                            else
                            {
                                asmclid = asmclid + ',' + classlist1[i].ASMCL_Id.ToString();
                            }
                        }

                        var sectionlist1 = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new StudentAttendanceReportDTO
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                            }
                                        ).Distinct().ToList();

                        for (int i = 0; i < sectionlist1.Count; i++)
                        {
                            if (i == 0)
                            {
                                asmsid = sectionlist1[i].ASMS_Id.ToString();
                            }
                            else
                            {
                                asmsid = asmsid + ',' + sectionlist1[i].ASMS_Id.ToString();
                            }
                        }
                    }

                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
                else
                {
                    if (ctdo.ASMC_Id == 0)
                    {
                        asmclid = ctdo.ASMCL_Id.ToString();

                        var sectiondata = (from a in _db.admissionClass
                                           from b in _db.masterSection
                                           from c in _db.academicYear
                                           from d in _db.Masterclasscategory
                                           from e in _db.AdmSchoolMasterClassCatSec
                                           from f in _db.Adm_SchAttLoginUser
                                           from g in _db.Adm_SchAttLoginUserClass
                                           where (f.ASALU_Id == g.ASALU_Id && a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id && d.MI_Id == ctdo.miid && d.ASMAY_Id == ctdo.ASMAY_Id && d.ASMCL_Id == ctdo.ASMCL_Id && f.ASMAY_Id == ctdo.ASMAY_Id && g.ASMCL_Id == ctdo.ASMCL_Id)
                                           select b
                                 ).Distinct().OrderBy(g => g.ASMC_Order).ToList();


                        if (sectiondata.Count > 0)
                        {
                            for (int i = 0; i < sectiondata.Count; i++)
                            {
                                if (i == 0)
                                {
                                    asmsid = sectiondata[i].ASMS_Id.ToString();
                                }
                                else
                                {
                                    asmsid = asmsid + ',' + sectiondata[i].ASMS_Id.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        asmclid = ctdo.ASMCL_Id.ToString();
                        asmsid = ctdo.ASMC_Id.ToString();
                    }
                }
            }
            else
            {
                if (ctdo.ASMC_Id == 0)
                {
                    asmclid = ctdo.ASMCL_Id.ToString();

                    var sectiondata = (from a in _db.admissionClass
                                       from b in _db.masterSection
                                       from c in _db.academicYear
                                       from d in _db.Masterclasscategory
                                       from e in _db.AdmSchoolMasterClassCatSec                                       
                                       where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id && d.MI_Id == ctdo.miid && d.ASMAY_Id == ctdo.ASMAY_Id && d.ASMCL_Id == ctdo.ASMCL_Id)
                                       select b
                             ).Distinct().OrderBy(g => g.ASMC_Order).ToList();


                    if (sectiondata.Count > 0)
                    {
                        for (int i = 0; i < sectiondata.Count; i++)
                        {
                            if (i == 0)
                            {
                                asmsid = sectiondata[i].ASMS_Id.ToString();
                            }
                            else
                            {
                                asmsid = asmsid + ',' + sectiondata[i].ASMS_Id.ToString();
                            }
                        }
                    }
                }
                else
                {
                    asmclid = ctdo.ASMCL_Id.ToString();
                    asmsid = ctdo.ASMC_Id.ToString();
                }
                //asmclid = ctdo.ASMCL_Id.ToString();
                //asmsid = ctdo.ASMC_Id.ToString();
            }


            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "AttendanceReport_NEW";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(ctdo.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.NVarChar)
                {
                    Value = asmclid
                });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.NVarChar)
                {
                    Value = asmsid
                });
                cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(ctdo.type)
                });

                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(ctdo.miid)
                });
                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt)
                {
                    Value = k
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
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
                    ctdo.studentAttendanceList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("Error In Attendance Report 100% :" + ex.Message);
                }
                return ctdo;
            }
        }
        public async Task<StudentAttendanceReportDTO> getsection(StudentAttendanceReportDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {

                var photopath = _db.standarad.Where(t => t.MI_Id == ctdo.miid).ToList();
                if (photopath.Count > 0)
                {
                    ctdo.photopathname = photopath.FirstOrDefault().ASC_Logo_Path;
                }


                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == ctdo.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                 ).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == ctdo.miid && a.IVRMSTAUL_UserName.Equals(ctdo.username.Trim()))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                if (check_rolename.FirstOrDefault().rolename.Equals("ADMIN") || check_rolename.FirstOrDefault().rolename.Equals("admin"))
                {
                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
                {
                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }

                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("COORDINATOR"))
                {
                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                    ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
                }

                else
                {
                    if (empcode_check.Count > 0)
                    {
                        ctdo.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }
                                  ).Distinct().ToArray();


                        ctdo.SectionList = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new StudentAttendanceReportDTO
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                            }
                                            ).Distinct().ToArray();



                    }
                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public async Task<StudentAttendanceReportDTO> getclass(StudentAttendanceReportDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {

                var photopath = _db.standarad.Where(t => t.MI_Id == ctdo.miid).ToList();
                if (photopath.Count > 0)
                {
                    ctdo.photopathname = photopath.FirstOrDefault().ASC_Logo_Path;
                }


                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == ctdo.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }
                                 ).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == ctdo.miid && a.IVRMSTAUL_UserName.Equals(ctdo.username.Trim()))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                if (check_rolename.FirstOrDefault().rolename.Equals("ADMIN") || check_rolename.FirstOrDefault().rolename.Equals("admin"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();
                }

                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("COORDINATOR"))
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                    ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();
                }

                else
                {
                    if (empcode_check.Count > 0)
                    {
                        ctdo.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == ctdo.miid && b.ASMAY_Id == ctdo.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
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
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

    }
}
