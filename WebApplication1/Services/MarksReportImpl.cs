using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class MarksReportImpl : Interfaces.MarksReportInterface
    {
        //public DomainModelMsSqlServerContext _Context;

        public MarksReportContext _MarksReportContext;
        public MarksReportImpl(MarksReportContext DomainModelContext)
        {
            _MarksReportContext = DomainModelContext;
        }
        public MarksReportDTO getdetails(MarksReportDTO data)
        {
            try
            {
               
               


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public MarksReportDTO schedulelist(MarksReportDTO data)
        {
            try
            {
                var Acdemic_preadmission = _MarksReportContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.mid).Select(d => d.ASMAY_Id).FirstOrDefault();
                data.asmay_id = Acdemic_preadmission;
                if (data.oralwrittenscheduleflag == "oral")
                {
                    List<OralTestScheduleDMO> oral = new List<OralTestScheduleDMO>();
                    data.writentestlist = (from a in _MarksReportContext.oraltest
                                            where (a.MI_Id == data.mid && a.ASMAY_Id==data.asmay_id)
                                            select new MarksReportDTO
                                            {
                                                disid = a.PAOTS_Id,
                                                disname = a.PAOTS_ScheduleName
                                            }).ToArray();

                    List<School_M_Class> classname = new List<School_M_Class>();
                    classname = _MarksReportContext.admissioncls.Where(t => t.MI_Id == data.mid && t.ASMCL_ActiveFlag == true).ToList();
                    data.fillclass = classname.ToArray();
                }
                else
                {
                    List<WrittenTestScheduleDMO> written = new List<WrittenTestScheduleDMO>();
                    data.writentestlist = (from a in _MarksReportContext.writentest
                                           where (a.MI_Id == data.mid && a.ASMAY_Id == data.asmay_id)
                                           select new MarksReportDTO
                                           {
                                               disid = a.PAWTS_Id,
                                               disname = a.PAWTS_ScheduleName
                                           }).ToArray();
                    List<IVRM_Master_SubjectsDMO> sublist = new List<IVRM_Master_SubjectsDMO>();
                    sublist = _MarksReportContext.allSubject.Where(t => t.MI_Id == data.mid && t.ISMS_ActiveFlag == 1 && t.ISMS_PreadmFlag==1).ToList();
                    data.fillsub = sublist.ToArray();

                    List<School_M_Class> classname = new List<School_M_Class>();
                    classname = _MarksReportContext.admissioncls.Where(t => t.MI_Id == data.mid && t.ASMCL_ActiveFlag == true).ToList();
                    data.fillclass = classname.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public MarksReportDTO Getreportdetails(MarksReportDTO data)
        {
            try
            {
                var Acdemic_preadmission = _MarksReportContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.mid).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.yearid =Convert.ToInt32(Acdemic_preadmission);

                if (data.flagows == "oral")
                {
                    data.fillhead = (from a in _MarksReportContext.MasterConfiguration
                                     where (a.MI_Id == data.mid && a.ASMAY_Id== data.yearid)
                                     select new MarksReportDTO
                                     {
                                         hid =Convert.ToInt64(a.ISPAC_OralTestBy),
                                         //hhead = a.PAMSU_SubjectName,
                                         hmaxmarks = a.ISPAC_OralByMax_Marks
                                     }).OrderByDescending(t => t.asmay_id).ToArray();
                    if (data.schids == 0)
                    {
                        data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                           from b in _MarksReportContext.StudentDetailsDMO
                                           from c in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                           from d in _MarksReportContext.admissioncls
                                           from e in _MarksReportContext.oraltest
                                           where (a.PASR_Id == b.PASR_Id && b.PASR_Id == c.PASR_Id && a.PASR_Id == c.PASR_Id && e.PAOTS_Id==c.PAOTS_Id && b.MI_Id == data.mid && b.ASMCL_Id==d.ASMCL_Id && e.ASMAY_Id==data.yearid)
                                           select new MarksReportDTO
                                           {
                                               PASR_Id=b.PASR_Id,
                                               PASR_FirstName = b.PASR_FirstName,
                                               PASR_MiddleName = b.PASR_MiddleName,
                                               PASR_LastName = b.PASR_LastName,
                                               // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                               regno = b.PASR_RegistrationNo,
                                               ISMS_Id = a.PAOTM_Id,
                                               PASWMS_MarksScored = a.PAOTMS_Marks,
                                               classname=d.ASMCL_ClassName

                                           }).OrderBy(t=>t.PASR_Id).ToArray();
                    }
                    else
                    {
                        data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                           from b in _MarksReportContext.StudentDetailsDMO
                                           from c in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                           from d in _MarksReportContext.admissioncls
                                           where (a.PASR_Id == b.PASR_Id && b.PASR_Id == c.PASR_Id && a.PASR_Id == c.PASR_Id && b.MI_Id == data.mid && c.PAOTS_Id == data.schids && b.ASMCL_Id == d.ASMCL_Id)
                                           select new MarksReportDTO
                                           {
                                               PASR_Id = b.PASR_Id,
                                               PASR_FirstName = b.PASR_FirstName,
                                               PASR_MiddleName = b.PASR_MiddleName,
                                               PASR_LastName = b.PASR_LastName,
                                               // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                               regno = b.PASR_RegistrationNo,
                                               ISMS_Id = a.PAOTM_Id,
                                               PASWMS_MarksScored = a.PAOTMS_Marks,
                                               classname = d.ASMCL_ClassName
                                           }).OrderBy(t => t.PASR_Id).ToArray();
                      
                    }
                }
                else
                {
                    data.fillhead = (from a in _MarksReportContext.allSubject
                                     where (a.MI_Id == data.mid && a.ISMS_PreadmFlag == 1 && a.ISMS_ActiveFlag==1)
                                     select new MarksReportDTO
                                     {
                                         hid = a.ISMS_Id,
                                         hhead = a.ISMS_SubjectName,
                                         hmaxmarks = a.ISMS_Max_Marks
                                     }).ToArray();
                    if (data.schids == 0)
                    {
                        if(data.ISMS_Id == 0)
                        {
                            data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               from f in _MarksReportContext.writentest
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id==c.PAWTS_Id && b.MI_Id == data.mid && b.ASMCL_Id==e.ASMCL_Id)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).ToArray();
                        }
                        else
                        {
                          
                            data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid  && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).ToArray();
                        }
                       
                    }
                    else
                    {
                        if (data.ISMS_Id == 0)
                        {

                      //      Preadmission_Subjectwise_Written_Marks_Students(WrittenTestStudentSubjectWiseMarksDMO)INNER JOIN
                      //Preadmission_Subjectwise_Written_Marks ON
                      //Preadmission_Subjectwise_Written_Marks_Students.PASWM_Id = Preadmission_Subjectwise_Written_Marks.PASWM_Id INNER JOIN
                      //Preadmission_School_Registration(StudentDetailsDMO) ON Preadmission_Subjectwise_Written_Marks_Students.PASR_Id = Preadmission_School_Registration.PASR_Id INNER JOIN
                      //Preadmission_WrittenTest_Schedule_Student(WrittenTestScheduleStudentInsertDMO) ON Preadmission_School_Registration.PASR_Id = Preadmission_WrittenTest_Schedule_Student.PASR_Id


                            data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id  && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && b.ASMCL_Id == e.ASMCL_Id)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).OrderBy(t => t.PASWMS_Id).ToArray();
                        }
                        else
                        {
                            data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).ToArray();
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
    }
}
