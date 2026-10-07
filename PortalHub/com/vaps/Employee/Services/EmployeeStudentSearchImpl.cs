using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeStudentSearchImpl:Interfaces.EmployeeStudentSearchInterface 
    {
        public FeeGroupContext _fees;
        public ExamContext _exam;
        public DomainModelMsSqlServerContext _db;

        public EmployeeStudentSearchImpl(FeeGroupContext fees,ExamContext exm, DomainModelMsSqlServerContext db)
        {
            _fees = fees;
            _exam = exm;
            _db = db;
        }

        public EmployeeDashboardDTO getdatastuacadgrp(EmployeeDashboardDTO data)
        {
            try
            {
                 data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                //data.fillstudent = (from a in _fees.AdmissionStudentDMO
                //                        from b in _fees.School_Adm_Y_StudentDMO
                //                        from c in _fees.admissioncls
                //                        from d in _fees.school_M_Section
                //                        where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id )
                //                        select new EmployeeDashboardDTO
                //                        {
                //                            Amst_Id = a.AMST_Id,
                //                            AMST_FirstName = a.AMST_FirstName,
                //                            AMST_MiddleName = a.AMST_MiddleName,
                //                            AMST_LastName = a.AMST_LastName,

                //                        }
                //   ).Distinct().ToArray();

                //var clstchname = (from a in _db.Adm_SchAttLoginUserClass
                //                    from b in _db.Adm_SchAttLoginUser
                //                    from c in _db.School_M_Class
                //                    where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                //                    && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                //                    && b.HRME_Id == data.HRME_Id
                //                    && c.ASMCL_ActiveFlag == true)
                //                    select new EmployeeDashboardDTO
                //                    {
                //                        ASMCL_Id = c.ASMCL_Id,
                //                        ASMCL_ClassName = c.ASMCL_ClassName,
                //                        ASMS_Id=a.ASMS_Id,
                //                    }
                //                    ).Distinct().ToArray();


                var clstchname = (from a in _exam.ClassTeacherMappingDMO
                                  from e in _exam.HR_Master_Employee_DMO
                                  where (a.HRME_Id == e.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == data.HRME_Id)
                                  select new EmployeeDashboardDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      ASMCL_Id = a.ASMCL_Id,
                                      ASMS_Id = a.ASMS_Id,

                                      HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                  }).Distinct().ToList();

                data.fillstudent = (from a in _fees.AdmissionStudentDMO
                                    from b in _fees.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == clstchname[0].ASMCL_Id && b.ASMS_Id == clstchname[0].ASMS_Id)
                                    select new EmployeeDashboardDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                        amst_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
            ).OrderBy(t => t.AMST_FirstName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public EmployeeDashboardDTO getstudentdetails(EmployeeDashboardDTO data)
        {
            try
            {
                data.fillstudentalldetails = (from a in _fees.AdmissionStudentDMO
                                    from b in _fees.School_Adm_Y_StudentDMO
                                    from c in _fees.admissioncls
                                    from d in _fees.school_M_Section
                                    from e in _fees.AcademicYear 
                                    where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && e.ASMAY_Id == data.ASMAY_Id)
                                    select new EmployeeDashboardDTO
                                    {
                                     // amst_Id = a.AMST_Id,
                                        amst_FirstName = a.AMST_FirstName,
                                        amst_MiddleName = a.AMST_MiddleName,
                                        amst_LastName = a.AMST_LastName,
                                        amst_RegistrationNo = a.AMST_RegistrationNo,
                                        amst_AdmNo = a.AMST_AdmNo,
                                        amay_RollNo = b.AMAY_RollNo,
                                        classname = c.ASMCL_ClassName,
                                        sectionname = d.ASMC_SectionName,
                                        fathername = a.AMST_FatherName,
                                        mothername = a.AMST_MotherName,
                                        bloodgroup = a.AMST_BloodGroup,
                                        address1 =a.AMST_PerStreet,
                                        address2 =a.AMST_PerArea ,
                                        address3 =a.AMST_PerCity,
                                        fathermobileno = a.AMST_FatherMobleNo,
                                        studentdob = a.AMST_DOB,
                                        amst_mobile = a.AMST_MobileNo,
                                        amst_sex =a.AMST_Sex,
                                        amst_dob =a.AMST_DOB ,
                                        amst_emailid =a.AMST_emailId,
                                        asma_year =e.ASMAY_Year 

                                    }
                    ).Distinct().ToArray();

                data.examlist=(from a in _exam.ExmStudentMarksProcessDMO 
                               from b in _exam.exammasterDMO 
                               where(a.MI_Id==data.MI_Id && b.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.AMST_Id==data.Amst_Id && a.EME_Id==b.EME_Id && b.EME_ActiveFlag==true )
                               select new EmployeeDashboardDTO {
                                   exam_name =b.EME_ExamName ,
                                   totalmarks =a.ESTMP_TotalMaxMarks ,
                                   obtainmarks =a.ESTMP_TotalObtMarks ,
                                   persentage =a.ESTMP_Percentage,
                                   result = a.ESTMP_Result
                               }
                               ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


    }
}
