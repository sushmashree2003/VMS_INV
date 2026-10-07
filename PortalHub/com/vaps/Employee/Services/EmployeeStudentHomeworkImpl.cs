using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Portals.Employee;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeStudentHomeworkImpl : Interfaces.EmployeeStudentHomeworkInterface
    {

        public PortalContext _PrincipalDashboardContext;
        public ExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;

        public EmployeeStudentHomeworkImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, ExamContext exm)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _examcontext = exm;
        }


        public IVRM_Homework_DTO Getdetails(IVRM_Homework_DTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = _examcontext.Staff_User_Login.Single(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).Emp_Code;
                }
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                data.yearlist = _PrincipalDashboardContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_ActiveFlag == 1).OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.classwork = (from m in _PrincipalDashboardContext.IVRM_Homework_DMO
                                  from n in _PrincipalDashboardContext.School_M_Class
                                  from o in _PrincipalDashboardContext.School_M_Section
                                  from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                  from q in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                  from z in _PrincipalDashboardContext.Staff_User_Login
                                  where (m.ASMCL_Id == q.ASMCL_Id && m.ASMS_Id == q.ASMS_Id && q.ASMCL_Id == n.ASMCL_Id && q.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id && q.ASMAY_Id == m.ASMAY_Id && m.IVRMUL_Id == z.Id && m.IVRMUL_Id == data.Login_Id && m.MI_Id == data.MI_Id)
                                  select new IVRM_Homework_DTO
                                  {
                                      ASMCL_ClassName = n.ASMCL_ClassName,
                                      ASMC_SectionName = o.ASMC_SectionName,
                                      ISMS_SubjectName = p.ISMS_SubjectName,
                                      IHW_AssignmentNo = m.IHW_AssignmentNo,
                                      IHW_Attachment = m.IHW_Attachment,
                                      IHW_Assignment = m.IHW_Assignment,
                                      IHW_Date = m.IHW_Date,
                                      IHW_Topic = m.IHW_Topic,
                                      IHW_ActiveFlag = m.IHW_ActiveFlag,
                                      IHW_Id = m.IHW_Id,
                                      ASMAY_Id = m.ASMAY_Id,
                                      IVRMUL_Id = m.IVRMUL_Id
                                  }).Distinct().OrderBy(t => t.IHW_Date).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public async Task<IVRM_Homework_DTO> get_classes(IVRM_Homework_DTO data)
        {
            try
            {

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    data.classlist = (from a in _PrincipalDashboardContext.School_M_Class
                                      from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                      from c in _PrincipalDashboardContext.AcademicYearDMO
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true)
                                      select new IVRM_ClassWorkDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,
                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _PrincipalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_StaffwiseClassStdata";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
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
                            data.classlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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

        public IVRM_Homework_DTO getsectiondata(IVRM_Homework_DTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;

                List<long> asmclids = new List<long>();
                foreach (var item in data.clsarray)
                {
                    asmclids.Add(item.ASMCL_Id);
                }

                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    data.sectionlist = (from a in _PrincipalDashboardContext.School_M_Section
                                        from b in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                                        from c in _PrincipalDashboardContext.AcademicYearDMO
                                        from e in _PrincipalDashboardContext.School_M_Class
                                        where (a.MI_Id == c.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMC_ActiveFlag == 1 && asmclids.Contains(b.ASMCL_Id))
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = a.ASMC_SectionName,
                                            ASMC_Order = a.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && asmclids.Contains(t.ASMCL_Id)).Select(t => t.ASMCC_Id).ToArray();

                    var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();
                    var sectionexamid = (from e in _examcontext.Staff_User_Login
                                         from f in _examcontext.Exm_Login_PrivilegeDMO
                                         from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                         where (e.Id == data.Login_Id &&
                                           f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && asmclids.Contains(i.ASMCL_Id) && secid.Contains(i.ASMS_Id)
                                           && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                         select new IVRM_Homework_DTO
                                         {
                                             ASMS_Id = i.ASMS_Id
                                         }).Distinct().Select(t => t.ASMS_Id).ToArray();

                    List<School_M_Section> seclist = new List<School_M_Section>();
                    seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
                    data.sectionlist = seclist.Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_Homework_DTO getsubject(IVRM_Homework_DTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                List<long> asmclids = new List<long>();
                foreach (var cls in data.clsarray)
                {
                    asmclids.Add(cls.ASMCL_Id);
                }
                List<long> asmsids = new List<long>();
                foreach (var sec in data.secarray)
                {
                    asmsids.Add(sec.ASMS_Id);
                }


                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    data.subjectlist = (from a in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                        from b in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                        from c in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                        from d in _PrincipalDashboardContext.School_Adm_Y_StudentDMO

                                        where (a.ISMS_Id == b.ISMS_Id && b.ELP_Id == c.ELP_Id && c.ASMAY_Id == d.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == d.ASMS_Id &&
                                        a.MI_Id == data.MI_Id && asmclids.Contains(b.ASMCL_Id) && asmsids.Contains(b.ASMS_Id) && c.ASMAY_Id == data.ASMAY_Id && a.ISMS_ActiveFlag == 1 && b.ELPs_ActiveFlg == true && d.AMAY_ActiveFlag == 1)
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ISMS_Id = b.ISMS_Id,
                                            ISMS_SubjectName = a.ISMS_SubjectName,
                                            ISMS_SubjectCode = a.ISMS_SubjectCode,
                                            ISMS_order = a.ISMS_OrderFlag
                                        }).Distinct().OrderBy(t => t.ISMS_order).ToArray();
                }
                else if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.subjectlist = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                        from f in _PrincipalDashboardContext.School_M_Section
                                        from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                        from h in _PrincipalDashboardContext.Staff_User_Login
                                        from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                        from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                        from k in _PrincipalDashboardContext.School_M_Class
                                        where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                                        && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && asmclids.Contains(d.ASMCL_Id) && asmsids.Contains(d.ASMS_Id)
                                        && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true && i.HRME_LeftFlag == false
                                        && h.Emp_Code == data.HRME_Id)
                                        select new IVRM_ClassWorkDTO
                                        {
                                            ISMS_Id = g.ISMS_Id,
                                            ISMS_SubjectName = g.ISMS_SubjectName,
                                            ISMS_SubjectCode = g.ISMS_SubjectCode,
                                            ISMS_order = g.ISMS_OrderFlag

                                        }).Distinct().OrderBy(t => t.ISMS_order).ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_Homework_DTO savedetail(IVRM_Homework_DTO data)
        {
            try
            {
                if (data.IHW_Id > 0)
                {
                    var Duplicate = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.IHW_Id != data.IHW_Id && t.IHW_Topic == data.IHW_Topic && t.IHW_Assignment == data.IHW_Assignment && t.ISMS_Id == data.ISMS_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IHW_Date == data.IHW_Date).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        var update = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.IHW_Id == data.IHW_Id).SingleOrDefault();


                        update.ASMAY_Id = data.ASMAY_Id;
                        update.IHW_AssignmentNo = data.IHW_AssignmentNo;
                        update.ISMS_Id = data.ISMS_Id;
                        update.ASMCL_Id = data.ASMCL_Id;
                        update.ASMS_Id = data.ASMS_Id;
                        update.IHW_Date = data.IHW_Date;
                        update.IHW_Topic = data.IHW_Topic;
                        update.IHW_Assignment = data.IHW_Assignment;
                        update.IHW_Attachment = data.IHW_Attachment;
                        update.IHW_FilePath = data.IHW_FilePath;
                        update.UpdatedDate = DateTime.Now;


                        _PrincipalDashboardContext.Update(update);
                        int rowaffected = _PrincipalDashboardContext.SaveChanges();
                        if (rowaffected > 0)
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
                    var Duplicate = _PrincipalDashboardContext.IVRM_Homework_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_Id == data.ISMS_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IHW_Date == data.IHW_Date).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {

                        IVRM_Homework_DMO obj = new IVRM_Homework_DMO();

                        obj.IHW_Id = data.IHW_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.IHW_AssignmentNo = data.IHW_AssignmentNo;
                        obj.ISMS_Id = data.ISMS_Id;
                        obj.IVRMUL_Id = data.Login_Id;
                        obj.ASMCL_Id = data.ASMCL_Id;
                        obj.ASMS_Id = data.ASMS_Id;
                        obj.IHW_Date = data.IHW_Date;
                        obj.IHW_Topic = data.IHW_Topic;
                        obj.IHW_Assignment = data.IHW_Assignment;
                        obj.IHW_Attachment = data.IHW_Attachment;
                        obj.IHW_FilePath = data.IHW_FilePath;
                        obj.IHW_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _PrincipalDashboardContext.Add(obj);
                        int rowAffected = _PrincipalDashboardContext.SaveChanges();
                        if (rowAffected > 0)
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

        public IVRM_Homework_DTO deactivate(IVRM_Homework_DTO data)
        {
            try
            {
                var query = _PrincipalDashboardContext.IVRM_Homework_DMO.Single(s => s.MI_Id == data.MI_Id && s.IHW_Id == data.IHW_Id);

                if (query.IHW_ActiveFlag == true)
                {
                    query.IHW_ActiveFlag = false;
                }
                else
                {
                    query.IHW_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _PrincipalDashboardContext.Update(query);
                var contactExists = _PrincipalDashboardContext.SaveChanges();
                if (contactExists == 1)
                {
                    data.returnval = true;
                    //data.returnsavestatus = "saved";
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
        public IVRM_Homework_DTO editData(IVRM_Homework_DTO data)
        {
            try
            {
                data.editlist = (from m in _PrincipalDashboardContext.IVRM_Homework_DMO
                                 from a in _PrincipalDashboardContext.AcademicYearDMO
                                 from n in _PrincipalDashboardContext.School_M_Class
                                 from o in _PrincipalDashboardContext.School_M_Section
                                 from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                 where (m.IHW_Id == data.IHW_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.MI_Id == n.MI_Id && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id && m.ASMAY_Id == a.ASMAY_Id && m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id)
                                 select new IVRM_Homework_DTO
                                 {
                                     ASMCL_Id = n.ASMCL_Id,
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMS_Id = o.ASMS_Id,
                                     ISMS_Id = p.ISMS_Id,
                                     IHW_AssignmentNo = m.IHW_AssignmentNo,
                                     IHW_Attachment = m.IHW_Attachment,
                                     IHW_Assignment = m.IHW_Assignment,
                                     IHW_FilePath = m.IHW_FilePath,
                                     IHW_Date = m.IHW_Date,
                                     IHW_Topic = m.IHW_Topic,
                                     IVRMUL_Id = m.IVRMUL_Id,
                                     IHW_Id = m.IHW_Id,

                                 }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
