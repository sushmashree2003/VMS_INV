using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Portals.Student;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Portals.Employee;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vaps.admission;

namespace PortalHub.com.vaps.Employee.Services
{
    public class IVRM_ClassWorkImpl : Interfaces.IVRM_ClassWorkInterface
    {
        public PortalContext _PrincipalDashboardContext;
        public ExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        //public DomainModelMsSqlServerContext _db;
        public IVRM_ClassWorkImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, ExamContext exm)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _examcontext = exm;
        }

     
        public IVRM_ClassWorkDTO Getdetails(IVRM_ClassWorkDTO data)
        {
            try
            {
                data.HRME_Id = _examcontext.Staff_User_Login.Single(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).Emp_Code;

                data.yearlist = _PrincipalDashboardContext.AcademicYearDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_ActiveFlag == 1).Distinct().OrderBy(t=>t.ASMAY_Order).ToArray();
               
               // data.classlist = _PrincipalDashboardContext.School_M_Section.Where(s => s.MI_Id == data.MI_Id && s.ASMC_ActiveFlag == 1).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
              
                data.classwork = (from m in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                            from n in _PrincipalDashboardContext.School_M_Class
                            from o in _PrincipalDashboardContext.School_M_Section
                            from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                            from z in _PrincipalDashboardContext.Staff_User_Login
                            where (m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id && m.Login_Id==z.Id && m.MI_Id==z.MI_Id && m.MI_Id==n.MI_Id && m.MI_Id == data.MI_Id && m.Login_Id==data.Login_Id)
                            select new IVRM_ClassWorkDTO
                            {
                                ASMCL_ClassName = n.ASMCL_ClassName,
                                ASMCL_Id = n.ASMCL_Id,
                                ASMC_SectionName = o.ASMC_SectionName,
                                ASMS_Id=o.ASMS_Id,
                                ISMS_SubjectName = p.ISMS_SubjectName,
                                ISMS_Id=p.ISMS_Id,
                                ICW_Id=m.ICW_Id,
                                ICW_Content = m.ICW_Content,
                                ICW_Topic = m.ICW_Topic,
                                ICW_SubTopic = m.ICW_SubTopic,
                                ICW_FromDate = m.ICW_FromDate,
                                ICW_ToDate = m.ICW_ToDate,
                                ICW_ActiveFlag = m.ICW_ActiveFlag,
                                ICW_Assignment=m.ICW_Assignment,
                                ICW_TeachingAid=m.ICW_TeachingAid,
                                ICW_Evaluation=m.ICW_Evaluation,
                                ICW_Attachment=m.ICW_Attachment,
                                ICW_FilePath=m.ICW_FilePath,
                                MI_Id=m.MI_Id,
                                ASMAY_Id=m.ASMAY_Id

                            }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

     
        public async Task<IVRM_ClassWorkDTO> get_classes(IVRM_ClassWorkDTO data)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_ClassWorkDTO getsectiondata(IVRM_ClassWorkDTO data)
        {
            try
            {

                //data.sectionlist = (from a in _db.Adm_SchAttLoginUserClass
                //                    from b in _db.Adm_SchAttLoginUser
                //                    from c in _db.School_M_Section
                //                    where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                //                    && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                //                    && b.HRME_Id == data.HRME_Id
                //                    && c.ASMC_ActiveFlag == 1)
                //                    select new IVRM_ClassWorkDTO
                //                    {
                //                        ASMS_Id = c.ASMS_Id,
                //                        ASMC_SectionName = c.ASMC_SectionName,
                //                    }).Distinct().ToArray();

                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

                var secid = _examcontext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();
                var sectionexamid = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     where (e.Id == data.Login_Id && //data.MI_Id == data.MI_Id &&
                                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMCL_Id == data.ASMCL_Id && secid.Contains(i.ASMS_Id)
                                       && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                     select new IVRM_Homework_DTO
                                     {
                                         ASMS_Id = i.ASMS_Id
                                     }).Distinct().Select(t => t.ASMS_Id).ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _examcontext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
                data.sectionlist = seclist.Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                //data.sectionlist = (from d in _PrincipalDashboardContext.School_M_Section
                //                   from e in _PrincipalDashboardContext.School_Adm_Y_StudentDMO
                //                    where (d.ASMS_Id==e.ASMS_Id && d.ASMC_ActiveFlag == 1 && e.AMAY_ActiveFlag==1 && e.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id &&  e.ASMCL_Id == data.ASMCL_Id  )
                //                    select new IVRM_ClassWorkDTO
                //                    {
                //                        ASMS_Id = d.ASMS_Id,
                //                        ASMC_SectionName = d.ASMC_SectionName
                //                    }
                //            ).Distinct().OrderBy(t=>t.ASMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_ClassWorkDTO getsubject(IVRM_ClassWorkDTO data)
        {
            try
            {
                //data.subjectlist = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                //                     from f in _PrincipalDashboardContext.School_M_Section
                //                     from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                //                     where (d.ASMS_Id==f.ASMS_Id && d.ISMS_Id==g.ISMS_Id && g.MI_Id==data.MI_Id && d.ASMS_Id==data.ASMS_Id)
                //                     select new IVRM_ClassWorkDTO
                //                     {
                //                         ISMS_Id = g.ISMS_Id,
                //                         ISMS_SubjectName = g.ISMS_SubjectName,
                //                         ISMS_SubjectCode = g.ISMS_SubjectCode
                //                     }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                data.subjectlist = (from d in _PrincipalDashboardContext.Exm_Login_Privilege_SubjectsDMO
                                    from f in _PrincipalDashboardContext.School_M_Section
                                    from g in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                    from h in _PrincipalDashboardContext.Staff_User_Login
                                    from i in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                    from j in _PrincipalDashboardContext.Exm_Login_PrivilegeDMO
                                    from k in _PrincipalDashboardContext.School_M_Class
                                    where (d.ELP_Id == j.ELP_Id && h.IVRMSTAUL_Id == j.Login_Id && i.HRME_Id == h.Emp_Code && k.ASMCL_Id == d.ASMCL_Id
                                    && d.ASMS_Id == f.ASMS_Id && d.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && d.ASMS_Id == data.ASMS_Id
                                    && d.ELPs_ActiveFlg == true && j.ELP_ActiveFlg == true && i.HRME_ActiveFlag == true && i.HRME_LeftFlag == false
                                    && h.Emp_Code == data.HRME_Id)
                                    select new IVRM_ClassWorkDTO
                                    {
                                        ISMS_Id = g.ISMS_Id,
                                        ISMS_SubjectName = g.ISMS_SubjectName,
                                        ISMS_SubjectCode = g.ISMS_SubjectCode,
                                        ASMCL_Order = k.ASMCL_Order,
                                        ISMS_order = g.ISMS_OrderFlag

                                    }).Distinct().OrderBy(t => t.ASMCL_Order).ThenBy(a => a.ISMS_order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
        public IVRM_ClassWorkDTO savedetail(IVRM_ClassWorkDTO data)
        {
            try
            {

                if (data.ICW_Id > 0)
                {
                    var Duplicate = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ICW_Id!= data.ICW_Id  && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ISMS_Id && t.Login_Id == data.Login_Id && t.ICW_Topic == data.ICW_Topic && t.ICW_ToDate == data.ICW_ToDate).ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        var update = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ICW_Id == data.ICW_Id).SingleOrDefault();
                       
                        update.ISMS_Id = data.ISMS_Id;
                        update.ASMCL_Id = data.ASMCL_Id;
                        update.ASMS_Id = data.ASMS_Id;
                        update.ICW_FromDate = data.ICW_FromDate;
                        update.ICW_ToDate = data.ICW_ToDate;
                        update.ICW_Topic = data.ICW_Topic;
                        update.ICW_SubTopic = data.ICW_SubTopic;
                        update.ICW_Content = data.ICW_Content;
                        update.ICW_TeachingAid = data.ICW_TeachingAid;
                        update.ICW_Assignment = data.ICW_Assignment;
                        update.ICW_Evaluation = data.ICW_Evaluation;
                        update.ICW_Attachment = data.ICW_Attachment;
                        update.ICW_FilePath = data.ICW_FilePath;
                       
                        update.UpdatedDate = DateTime.Now;

                        _PrincipalDashboardContext.Update(update);
                        var contactExists = _PrincipalDashboardContext.SaveChanges();
                        if (contactExists > 0)
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
                    var Duplicate = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ISMS_Id && t.Login_Id == data.Login_Id && t.ICW_Topic == data.ICW_Topic && t.ICW_ToDate == data.ICW_ToDate).ToList();
                    if(Duplicate.Count()>0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        IVRM_ClassWorkDMO obj = new IVRM_ClassWorkDMO();

                        obj.MI_Id = data.MI_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.ISMS_Id = data.ISMS_Id;
                        obj.Login_Id = data.Login_Id;
                        obj.ASMCL_Id = data.ASMCL_Id;
                        obj.ASMS_Id = data.ASMS_Id;
                        obj.ICW_FromDate = data.ICW_FromDate;
                        obj.ICW_ToDate = data.ICW_ToDate;
                        obj.ICW_Topic = data.ICW_Topic;
                        obj.ICW_SubTopic = data.ICW_SubTopic;
                        obj.ICW_Content = data.ICW_Content;
                        obj.ICW_TeachingAid = data.ICW_TeachingAid;
                        obj.ICW_Assignment = data.ICW_Assignment;
                        obj.ICW_Evaluation = data.ICW_Evaluation;
                        obj.ICW_Attachment = data.ICW_Attachment;
                        obj.ICW_FilePath = data.ICW_FilePath;
                        obj.ICW_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _PrincipalDashboardContext.Add(obj);
                        var contactExists = _PrincipalDashboardContext.SaveChanges();
                        if (contactExists > 0)
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
        
        public IVRM_ClassWorkDTO editData(IVRM_ClassWorkDTO data)
        {
            try
            {
                data.editlist = (from m in _PrincipalDashboardContext.IVRM_ClassWorkDMO
                                 from a in _PrincipalDashboardContext.AcademicYearDMO
                                 from n in _PrincipalDashboardContext.School_M_Class
                                 from o in _PrincipalDashboardContext.School_M_Section
                                 from p in _PrincipalDashboardContext.IVRM_Master_SubjectsDMO
                                 where (m.MI_Id == n.MI_Id && m.ASMAY_Id == a.ASMAY_Id && m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == p.ISMS_Id && m.ICW_Id == data.ICW_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id)
                                 select new IVRM_ClassWorkDTO
                                 {
                                     ASMCL_Id = n.ASMCL_Id,
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMS_Id = o.ASMS_Id,
                                     ISMS_Id = p.ISMS_Id,
                                     ICW_Topic = m.ICW_Topic,
                                     ICW_SubTopic = m.ICW_SubTopic,
                                     ICW_Assignment = m.ICW_Assignment,
                                     ICW_Attachment = m.ICW_Attachment,
                                     ICW_FilePath = m.ICW_FilePath,
                                     ICW_FromDate = m.ICW_FromDate,
                                     ICW_ToDate = m.ICW_ToDate,
                                     ICW_Id = m.ICW_Id,
                                     ICW_Content=m.ICW_Content,
                                     ICW_TeachingAid=m.ICW_TeachingAid,
                                     ICW_Evaluation=m.ICW_Evaluation,
                                    
                                 }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRM_ClassWorkDTO deactivate(IVRM_ClassWorkDTO data)
        {
            try
            {
                var query = _PrincipalDashboardContext.IVRM_ClassWorkDMO.Single(s => s.MI_Id == data.MI_Id && s.ICW_Id == data.ICW_Id);

                if (query.ICW_ActiveFlag == true)
                {
                    query.ICW_ActiveFlag = false;
                }
                else
                {
                    query.ICW_ActiveFlag = true;
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

    }
}
