using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Alumni;

namespace TransportServiceHub.Services
{
    public class UsrPwdImpl : Interfaces.UsrPwdInterface
    {

        public TransportContext _context;
        ILogger<SMSEmailSendImpl> _areaimpl;
        public DomainModelMsSqlServerContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _UserManager;

        public UsrPwdImpl(ILogger<SMSEmailSendImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> UserManager)
        {
            _areaimpl = areaimpl;
            _context = context;
            _db = db;
            _UserManager = UserManager;
            _signInManager = signInManager;
        }

        public SMSEmailSendDTO getdata(int id)
        {
            SMSEmailSendDTO data = new SMSEmailSendDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _context.School_M_Class.Where(y => y.ASMCL_ActiveFlag == true && y.MI_Id == id).ToList();
                data.classlist = allclass.Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                List<School_M_Section> allsec = new List<School_M_Section>();
                allsec = _context.School_M_Section.Where(s => s.MI_Id == id).ToList();
                data.sectionlist = allsec.Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }

        public SMSEmailSendDTO Getreportdetails(SMSEmailSendDTO data)
        {
            try
            {
                //if (data.filterdata == "stureg")
                //{
                //    if (data.ASMCL_Id == 0 && data.ASMS_Id == 0)
                //    {
                //        data.messagelist = (from b in _context.Adm_M_Student
                //                            from c in _context.School_M_Class
                //                            from d in _context.School_M_Section
                //                            from e in _context.School_Adm_Y_StudentDMO
                //                            from f in _context.ApplUser
                //                            from a in _context.StudentAppUserLoginDMO
                //                            where (a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id)
                //                            select new SMSEmailSendDTO
                //                            {
                //                                ASMCL_ClassName = c.ASMCL_ClassName,
                //                                ASMC_SectionName = d.ASMC_SectionName,
                //                                username = f.UserName,
                //                                password = "Password@123",
                //                                studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                //                                AMST_MobileNo = b.AMST_MobileNo,
                //                                AMST_emailId = b.AMST_emailId,
                //                                AMST_Id = b.AMST_Id

                //                            }).Distinct().ToArray();
                //    }
                //    else if (data.ASMCL_Id != 0 && data.ASMS_Id != 0)
                //    {
                //        data.messagelist = (from b in _context.Adm_M_Student
                //                            from c in _context.School_M_Class
                //                            from d in _context.School_M_Section
                //                            from e in _context.School_Adm_Y_StudentDMO
                //                            from f in _context.ApplUser
                //                            from a in _context.StudentAppUserLoginDMO
                //                            where (a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id && e.ASMS_Id == data.ASMS_Id)
                //                            select new SMSEmailSendDTO
                //                            {
                //                                ASMCL_ClassName = c.ASMCL_ClassName,
                //                                ASMC_SectionName = d.ASMC_SectionName,
                //                                username = f.UserName,
                //                                password = "Password@123",
                //                                studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                //                                AMST_MobileNo = b.AMST_MobileNo,
                //                                AMST_emailId = b.AMST_emailId,
                //                                AMST_Id = b.AMST_Id

                //                            }).Distinct().ToArray();
                //    }
                //    else if (data.ASMCL_Id == 0 && data.ASMS_Id != 0)
                //    {
                //        data.messagelist = (from b in _context.Adm_M_Student
                //                            from c in _context.School_M_Class
                //                            from d in _context.School_M_Section
                //                            from e in _context.School_Adm_Y_StudentDMO
                //                            from f in _context.ApplUser
                //                            from a in _context.StudentAppUserLoginDMO
                //                            where (a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMS_Id == data.ASMS_Id)
                //                            select new SMSEmailSendDTO
                //                            {
                //                                ASMCL_ClassName = c.ASMCL_ClassName,
                //                                ASMC_SectionName = d.ASMC_SectionName,
                //                                username = f.UserName,
                //                                password = "Password@123",
                //                                studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                //                                AMST_MobileNo = b.AMST_MobileNo,
                //                                AMST_emailId = b.AMST_emailId,
                //                                AMST_Id = b.AMST_Id


                //                            }).Distinct().ToArray();
                //    }
                //    else if (data.ASMCL_Id != 0 && data.ASMS_Id == 0)
                //    {
                //        data.messagelist = (from b in _context.Adm_M_Student
                //                            from c in _context.School_M_Class
                //                            from d in _context.School_M_Section
                //                            from e in _context.School_Adm_Y_StudentDMO
                //                            from f in _context.ApplUser
                //                            from a in _context.StudentAppUserLoginDMO
                //                            where (a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id)
                //                            select new SMSEmailSendDTO
                //                            {
                //                                ASMCL_ClassName = c.ASMCL_ClassName,
                //                                ASMC_SectionName = d.ASMC_SectionName,
                //                                username = f.UserName,
                //                                password = "Password@123",
                //                                studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                                AMST_AdmNo = b.AMST_AdmNo == null ? " " : b.AMST_AdmNo,
                //                                AMST_MobileNo = b.AMST_MobileNo,
                //                                AMST_emailId = b.AMST_emailId,
                //                                AMST_Id = b.AMST_Id

                //                            }).Distinct().ToArray();
                //    }
                //}
                //else if (data.filterdata == "stutra")
                //{
                //    if (data.ASMCL_Id == 0 && data.ASMS_Id == 0)
                //    {
                //        data.messagelist = (from b in _context.Adm_M_Student
                //                            from c in _context.School_M_Class
                //                            from d in _context.School_M_Section
                //                            from e in _context.School_Adm_Y_StudentDMO
                //                            from f in _context.ApplUser
                //                            from a in _context.StudentAppUserLoginDMO
                //                            from g in _context.TR_Student_RouteDMO
                //                            where (g.AMST_Id == b.AMST_Id && b.MI_Id == g.MI_Id && a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id)
                //                            select new SMSEmailSendDTO
                //                            {
                //                                ASMCL_ClassName = c.ASMCL_ClassName,
                //                                ASMC_SectionName = d.ASMC_SectionName,
                //                                username = f.UserName,
                //                                password = "Password@123",
                //                                studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                //                                AMST_MobileNo = b.AMST_MobileNo,
                //                                AMST_emailId = b.AMST_emailId,
                //                                AMST_Id = b.AMST_Id

                //                            }).Distinct().ToArray();
                //    }
                //    else if (data.ASMCL_Id != 0 && data.ASMS_Id != 0)
                //    {
                //        data.messagelist = (from b in _context.Adm_M_Student
                //                            from c in _context.School_M_Class
                //                            from d in _context.School_M_Section
                //                            from e in _context.School_Adm_Y_StudentDMO
                //                            from f in _context.ApplUser
                //                            from a in _context.StudentAppUserLoginDMO
                //                            from g in _context.TR_Student_RouteDMO
                //                            where (g.AMST_Id == b.AMST_Id && b.MI_Id == g.MI_Id && a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id && e.ASMS_Id == data.ASMS_Id)
                //                            select new SMSEmailSendDTO
                //                            {
                //                                ASMCL_ClassName = c.ASMCL_ClassName,
                //                                ASMC_SectionName = d.ASMC_SectionName,
                //                                username = f.UserName,
                //                                password = "Password@123",
                //                                studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                //                                AMST_MobileNo = b.AMST_MobileNo,
                //                                AMST_emailId = b.AMST_emailId,
                //                                AMST_Id = b.AMST_Id

                //                            }).Distinct().ToArray();
                //    }
                //    else if (data.ASMCL_Id == 0 && data.ASMS_Id != 0)
                //    {
                //        data.messagelist = (from b in _context.Adm_M_Student
                //                            from c in _context.School_M_Class
                //                            from d in _context.School_M_Section
                //                            from e in _context.School_Adm_Y_StudentDMO
                //                            from f in _context.ApplUser
                //                            from a in _context.StudentAppUserLoginDMO
                //                            from g in _context.TR_Student_RouteDMO
                //                            where (g.AMST_Id == b.AMST_Id && b.MI_Id == g.MI_Id && a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMS_Id == data.ASMS_Id)
                //                            select new SMSEmailSendDTO
                //                            {
                //                                ASMCL_ClassName = c.ASMCL_ClassName,
                //                                ASMC_SectionName = d.ASMC_SectionName,
                //                                username = f.UserName,
                //                                password = "Password@123",
                //                                studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                //                                AMST_MobileNo = b.AMST_MobileNo,
                //                                AMST_emailId = b.AMST_emailId,
                //                                AMST_Id = b.AMST_Id


                //                            }).Distinct().ToArray();
                //    }
                //    else if (data.ASMCL_Id != 0 && data.ASMS_Id == 0)
                //    {
                //        data.messagelist = (from b in _context.Adm_M_Student
                //                            from c in _context.School_M_Class
                //                            from d in _context.School_M_Section
                //                            from e in _context.School_Adm_Y_StudentDMO
                //                            from f in _context.ApplUser
                //                            from a in _context.StudentAppUserLoginDMO
                //                            from g in _context.TR_Student_RouteDMO
                //                            where (g.AMST_Id == b.AMST_Id && b.MI_Id == g.MI_Id && a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id)
                //                            select new SMSEmailSendDTO
                //                            {
                //                                ASMCL_ClassName = c.ASMCL_ClassName,
                //                                ASMC_SectionName = d.ASMC_SectionName,
                //                                username = f.UserName,
                //                                password = "Password@123",
                //                                studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                                AMST_AdmNo = b.AMST_AdmNo == null ? " " : b.AMST_AdmNo,
                //                                AMST_MobileNo = b.AMST_MobileNo,
                //                                AMST_emailId = b.AMST_emailId,
                //                                AMST_Id = b.AMST_Id

                //                            }).Distinct().ToArray();
                //    }
                //}




                //data.messagelist = (from a in _context.AcademicYear
                //                    from b in _context.School_M_Class
                //                    from c in _context.School_M_Section
                //                    from d in _context.Adm_M_Student
                //                    from e in _context.School_Adm_Y_StudentDMO
                //                    where (d.AMST_Id == e.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && d.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id && e.ASMS_Id == data.ASMS_Id && d.AMST_SOL == "S"
                //                    && d.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1)
                //                    select new SMSEmailSendDTO
                //                    {
                //                        ASMCL_ClassName = b.ASMCL_ClassName,
                //                        ASMC_SectionName = c.ASMC_SectionName,
                //                        studentname = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                //                        AMST_AdmNo = d.AMST_AdmNo == null ? " " : d.AMST_AdmNo,
                //                        AMST_Id = d.AMST_Id
                //                    }).OrderBy(dd => dd.studentname).ToArray();
                List<long> classid = new List<long>();
                List<long> sectionid = new List<long>();
                List<long> alumniids = new List<long>();
                if (data.ASMS_Id != 0)
                {
                    var sectionidlist = (from a in _db.AcademicYear
                                         from b in _db.Masterclasscategory
                                         from c in _db.School_M_Class
                                         from d in _db.AdmSchoolMasterClassCatSec
                                         from e in _db.School_M_Section
                                         where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.Is_Active == true
                                         && d.ASMCC_Id == b.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && e.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true
                                         && b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && d.ASMS_Id == data.ASMS_Id)
                                         select e).Distinct().ToList();
                    foreach (var y in sectionidlist)
                    {
                        sectionid.Add(y.ASMS_Id);
                    }
                }
                else
                {
                    var sectionidlist = (from a in _db.AcademicYear
                                         from b in _db.Masterclasscategory
                                         from c in _db.School_M_Class
                                         from d in _db.AdmSchoolMasterClassCatSec
                                         from e in _db.School_M_Section
                                         where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.Is_Active == true
                                         && d.ASMCC_Id == b.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && e.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true
                                         && b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id)
                                         select e).Distinct().ToList();
                    foreach (var y in sectionidlist)
                    {
                        sectionid.Add(y.ASMS_Id);
                    }
                }

                if (data.filterdata == "reg")
                {
                    data.messagelist = (from a in _context.AcademicYear
                                        from b in _context.School_M_Class
                                        from c in _context.School_M_Section
                                        from d in _context.Adm_M_Student
                                        from e in _context.School_Adm_Y_StudentDMO
                                        where (d.AMST_Id == e.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && c.ASMS_Id == e.ASMS_Id
                                        && d.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id && sectionid.Contains(e.ASMS_Id)
                                        && d.AMST_SOL == "S" && d.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1)
                                        select new SMSEmailSendDTO
                                        {
                                            ASMCL_ClassName = b.ASMCL_ClassName,
                                            ASMC_SectionName = c.ASMC_SectionName,
                                            studentname = ((d.AMST_FirstName == null ? "" : d.AMST_FirstName) +
                                            (d.AMST_MiddleName == null ? "" : " " + d.AMST_MiddleName) +
                                            (d.AMST_LastName == null ? "" : " " + d.AMST_LastName)).Trim(),
                                            AMST_AdmNo = d.AMST_AdmNo == null ? " " : d.AMST_AdmNo,
                                            AMST_Id = d.AMST_Id
                                        }).OrderBy(dd => dd.studentname).ToArray();
                }
                else if (data.filterdata == "new")
                {
                    data.messagelist = (from a in _context.AcademicYear
                                        from b in _context.School_M_Class
                                        from c in _context.School_M_Section
                                        from d in _context.Adm_M_Student
                                        from e in _context.School_Adm_Y_StudentDMO
                                        where (d.AMST_Id == e.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && c.ASMS_Id == e.ASMS_Id
                                        && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id && sectionid.Contains(e.ASMS_Id)
                                        && d.AMST_SOL == "S" && d.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1)
                                        select new SMSEmailSendDTO
                                        {
                                            ASMCL_ClassName = b.ASMCL_ClassName,
                                            ASMC_SectionName = c.ASMC_SectionName,
                                            studentname = ((d.AMST_FirstName == null ? "" : d.AMST_FirstName) +
                                            (d.AMST_MiddleName == null ? "" : " " + d.AMST_MiddleName) +
                                            (d.AMST_LastName == null ? "" : " " + d.AMST_LastName)).Trim(),
                                            AMST_AdmNo = d.AMST_AdmNo == null ? " " : d.AMST_AdmNo,
                                            AMST_Id = d.AMST_Id
                                        }).OrderBy(dd => dd.studentname).ToArray();
                }
                else
                {

                    var alumnilist = (from a in _db.AlumniUserRegistrationDMO
                                      from b in _db.Alumni_User_LoginDMO
                                      from d in _db.Alumni_M_StudentDMO
                                      where (a.ALMST_Id == d.ALMST_Id && a.ALSREG_Id == b.ALSREG_Id
                                      && d.MI_Id == data.MI_Id)
                                      select new SMSEmailSendDTO
                                      {
                                          AMST_Id = d.ALMST_Id
                                      }).ToList();

                    foreach (var y in alumnilist)
                    {
                        alumniids.Add(y.AMST_Id);
                    }

                    data.messagelist = (from a in _context.AcademicYear
                                        from b in _context.School_M_Class
                                        from d in _context.Alumni_M_StudentDMO
                                        where (a.ASMAY_Id == d.ASMAY_Id_Left && b.ASMCL_Id == d.ASMCL_Id_Left
                                        && d.MI_Id == data.MI_Id && d.ASMAY_Id_Left == data.ASMAY_Id && d.ASMCL_Id_Left == data.ASMCL_Id && !alumniids.Contains(d.ALMST_Id))
                                        select new SMSEmailSendDTO
                                        {
                                            ASMCL_ClassName = b.ASMCL_ClassName,
                                            studentname = ((d.ALMST_FirstName == null ? "" : d.ALMST_FirstName) +
                                            (d.ALMST_MiddleName == null ? "" : " " + d.ALMST_MiddleName) +
                                            (d.ALMST_LastName == null ? "" : " " + d.ALMST_LastName)).Trim(),
                                            AMST_AdmNo = d.ALMST_AdmNo == null ? " " : d.ALMST_AdmNo,
                                            AMST_Id = d.ALMST_Id
                                        }).OrderBy(dd => dd.studentname).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }

        public async Task<SMSEmailSendDTO> smssend(SMSEmailSendDTO data)
        {
            SMS sms = new SMS(_db);

            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].AMST_MobileNo != 0)
                {
                    data.success = await sms.sendSms(data.MI_Id, data.data_array[i].AMST_MobileNo, "ADMINISTRATOR", data.data_array[i].AMST_Id);
                }
            }

            //data.success = await sms.sendSms(data.MI_Id, data.data_array[i].AMST_MobileNo, "ADMINISTRATOR", data.data_array[i].AMST_Id);

            return data;
        }
        public SMSEmailSendDTO emailsend(SMSEmailSendDTO data)
        {

            Email Email = new Email(_db);
            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].AMST_emailId != "null" && data.data_array[i].AMST_emailId != null && data.data_array[i].AMST_emailId != "")
                {
                    data.success = Email.sendmail(data.MI_Id, data.data_array[i].AMST_emailId, "ADMINISTRATOR", data.data_array[i].AMST_Id);
                }
            }
            return data;
        }

        public SMSEmailSendDTO creusrnme(SMSEmailSendDTO data)
        {
            try
            {

                data.success = createusername(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string createusername(SMSEmailSendDTO data)
        {
            string failname = "";
            string admno = "";
            int counts = 0;
            int countf = 0;
            string otpadmno = "";

            try
            {
                var checkotporadm = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).ToList();

                var virtualcode = _db.VirtualSchool.Where(t => t.IVRM_MI_Id == data.MI_Id).Select(t => t.ivrm_school_code).FirstOrDefault();

                otpadmno = checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO;
                if (data.filterdata != "Alumni")
                {
                    if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "Admno")
                    {
                        for (int kk = 0; kk < data.data_array.Count(); kk++)
                        {
                            long countincrementstudnet = 0;
                            long countincrementfather = 0;
                            long countincrementmother = 0;


                            try
                            {

                                var amst_id = data.data_array[kk].AMST_Id;
                                var checkstudent = (from a in _db.StudentAppUserLoginDMO
                                                    where a.AMST_ID == amst_id
                                                    select new SMSEmailSendDTO
                                                    {
                                                        AMST_Id = a.AMST_ID
                                                    }).ToList();
                                if (checkstudent.Count() == 0)
                                {
                                    //generateOTP otp = new generateOTP();
                                    //string studotp = otp.GeneratePassword();

                                    string studotp = otpadmno;

                                    var studDet = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amst_id).ToList();

                                    admno = studDet.FirstOrDefault().AMST_AdmNo;
                                    long stduserid = 0;
                                    long fatuserid = 0;
                                    long motuserid = 0;
                                    string res = "";
                                    Dictionary<string, long> temp = new Dictionary<string, long>();

                                    if (studDet.FirstOrDefault().AMST_emailId != "" && studDet.FirstOrDefault().AMST_emailId != null)
                                    {
                                        string StudentName = virtualcode + "S" + admno.ToString();

                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                        stduserid = response.useridapp;
                                        res = response.resp;
                                        if (stduserid == 0)
                                        {
                                            countincrementstudnet += 1;
                                            StudentName = virtualcode + "S" + admno.ToString() + countincrementstudnet;

                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                            stduserid = response1.useridapp;
                                            res = response1.resp;
                                            temp.Add("studentid", stduserid);
                                        }
                                        else
                                        {
                                            temp.Add("studentid", stduserid);

                                        }
                                        bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMST_Id);
                                        if (res == "Success" && val == true)
                                        {
                                        }

                                    }
                                    else
                                    {
                                        temp.Add("studentid", 0);
                                    }


                                    if (studDet.FirstOrDefault().AMST_FatheremailId != "" && studDet.FirstOrDefault().AMST_FatheremailId != null)
                                    {
                                        string fathrotp = admno;

                                        string fathName = virtualcode + "F" + fathrotp.ToString();

                                        fathName = Regex.Replace(fathName, @"\s+", "");
                                        if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
                                        {
                                            data.AMST_FatherMobleNo = studDet.FirstOrDefault().AMST_FatherMobleNo;
                                        }
                                        else
                                        {
                                            data.AMST_FatherMobleNo = 0;
                                        }
                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                        fatuserid = response.useridapp;
                                        res = response.resp;
                                        if (fatuserid == 0)
                                        {
                                            // fathrotp = otp.GeneratePassword();

                                            //fathName = virtualcode + data.MI_Id.ToString() + "F" + fathrotp.ToString();

                                            countincrementfather += 1;

                                            fathName = virtualcode + "F" + fathrotp.ToString() + countincrementfather;

                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                            fatuserid = response1.useridapp;
                                            res = response1.resp;
                                            temp.Add("Fatherid", fatuserid);
                                        }
                                        else
                                        {
                                            temp.Add("Fatherid", fatuserid);
                                        }
                                        bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMST_Id);
                                        if (res == "Success" && val == true)
                                        {
                                        }
                                        fathrotp = "";
                                    }
                                    else
                                    {
                                        temp.Add("Fatherid", 0);
                                    }
                                    if (studDet.FirstOrDefault().AMST_MotherEmailId != "" && studDet.FirstOrDefault().AMST_MotherEmailId != null)
                                    {

                                        //string motherotp = otp.getFourDigitOTPMother();
                                        //string motherotp = otp.GeneratePassword_Mother();

                                        string motherotp = admno;
                                        // string MotherName = virtualcode + data.MI_Id.ToString() + "M" + motherotp.ToString();
                                        string MotherName = virtualcode + "M" + motherotp.ToString();

                                        // string MotherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0,4) + motherotp;
                                        MotherName = Regex.Replace(MotherName, @"\s+", "");
                                        if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "")
                                        {
                                            data.AMST_MotherMobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo;
                                        }
                                        else
                                        {
                                            data.AMST_MotherMobileNo = 0;
                                        }
                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                        motuserid = response.useridapp;
                                        res = response.resp;
                                        if (motuserid == 0)
                                        {
                                            countincrementmother += 1;

                                            MotherName = virtualcode + "M" + motherotp.ToString() + countincrementmother;

                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                            motuserid = response1.useridapp;
                                            res = response1.resp;
                                            temp.Add("motherid", motuserid);
                                        }
                                        else
                                        {
                                            temp.Add("motherid", motuserid);
                                        }
                                        bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMST_Id);
                                        if (res == "Success" && val == true)
                                        {
                                        }
                                        motherotp = "";
                                    }
                                    else
                                    {
                                        temp.Add("motherid", 0);
                                    }
                                    if (temp.Count != 0)
                                    {
                                        long uid = 0;
                                        long fid = 0;
                                        long mid = 0;
                                        if (temp["studentid"] != 0)
                                        {
                                            uid = temp["studentid"];
                                        }
                                        if (temp["Fatherid"] != 0)
                                        {
                                            fid = temp["Fatherid"];
                                        }
                                        if (temp["motherid"] != 0)
                                        {
                                            mid = temp["motherid"];
                                        }

                                        bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
                                        if (vall == true)
                                        {
                                            counts = counts + 1;
                                        }
                                        else
                                        {
                                            countf = countf + 1;
                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                            catch (Exception ex)
                            {
                                failname += "," + admno;
                                countf = countf + 1;
                                Console.WriteLine(ex.Message);
                                continue;
                            }
                        }
                    }
                    else if (checkotporadm.FirstOrDefault().IVRM_OTP_ADMNO == "OTP")
                    {
                        for (int kk = 0; kk < data.data_array.Count(); kk++)
                        {
                            try
                            {

                                var amst_id = data.data_array[kk].AMST_Id;
                                var checkstudent = (from a in _db.StudentAppUserLoginDMO
                                                    where a.AMST_ID == amst_id
                                                    select new SMSEmailSendDTO
                                                    {
                                                        AMST_Id = a.AMST_ID
                                                    }).ToList();
                                if (checkstudent.Count() == 0)
                                {
                                    generateOTP otp = new generateOTP();
                                    string studotp = otp.GeneratePassword();

                                    var studDet = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amst_id).ToList();
                                    admno = studDet.FirstOrDefault().AMST_AdmNo;
                                    long stduserid = 0;
                                    long fatuserid = 0;
                                    long motuserid = 0;
                                    string res = "";
                                    Dictionary<string, long> temp = new Dictionary<string, long>();

                                    if (studDet.FirstOrDefault().AMST_emailId != "" && studDet.FirstOrDefault().AMST_emailId != null)
                                    {
                                        //string studotp = otp.getFourDigitOTPFather();
                                        //string studotp = otp.GeneratePassword();
                                        //string StudentName = virtualcode + data.MI_Id.ToString() + "S" + studotp.ToString();

                                        string StudentName = virtualcode + "S" + studotp.ToString();

                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                        stduserid = response.useridapp;
                                        res = response.resp;
                                        if (stduserid == 0)
                                        {
                                            studotp = otp.GeneratePassword();
                                            // studotp = otp.getFourDigitOTPFather();
                                            //StudentName = virtualcode + data.MI_Id.ToString() + "S" + studotp.ToString();

                                            StudentName = virtualcode + "S" + studotp.ToString();

                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_emailId, StudentName, data.MI_Id, "STUDENT", "STUDENT", studDet.FirstOrDefault().AMST_MobileNo.ToString()).Result;
                                            stduserid = response1.useridapp;
                                            res = response1.resp;
                                            temp.Add("studentid", stduserid);
                                        }
                                        else
                                        {
                                            temp.Add("studentid", stduserid);

                                        }
                                        bool val = AddStudentUserLogin(data.MI_Id, StudentName, studDet.FirstOrDefault().AMST_Id);
                                        if (res == "Success" && val == true)
                                        {
                                        }

                                    }
                                    else
                                    {
                                        temp.Add("studentid", 0);
                                    }


                                    if (studDet.FirstOrDefault().AMST_FatheremailId != "" && studDet.FirstOrDefault().AMST_FatheremailId != null)
                                    {

                                        // string fathrotp = otp.getFourDigitOTPFather();
                                        //string fathrotp = otp.GeneratePassword();
                                        string fathrotp = studotp;

                                        // string fathName = virtualcode + data.MI_Id.ToString() + "F" + fathrotp.ToString();
                                        string fathName = virtualcode + "F" + fathrotp.ToString();

                                        fathName = Regex.Replace(fathName, @"\s+", "");
                                        if (studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != null && studDet.FirstOrDefault().AMST_FatherMobleNo.ToString() != "")
                                        {
                                            data.AMST_FatherMobleNo = studDet.FirstOrDefault().AMST_FatherMobleNo;
                                        }
                                        else
                                        {
                                            data.AMST_FatherMobleNo = 0;
                                        }
                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                        fatuserid = response.useridapp;
                                        res = response.resp;
                                        if (fatuserid == 0)
                                        {
                                            fathrotp = otp.GeneratePassword();

                                            //fathName = virtualcode + data.MI_Id.ToString() + "F" + fathrotp.ToString();
                                            fathName = virtualcode + "F" + fathrotp.ToString();

                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_FatheremailId, fathName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_FatherMobleNo.ToString()).Result;
                                            fatuserid = response1.useridapp;
                                            res = response1.resp;
                                            temp.Add("Fatherid", fatuserid);
                                        }
                                        else
                                        {
                                            temp.Add("Fatherid", fatuserid);
                                        }
                                        bool val = AddStudentUserLogin(data.MI_Id, fathName, studDet.FirstOrDefault().AMST_Id);
                                        if (res == "Success" && val == true)
                                        {
                                        }
                                        fathrotp = "";
                                    }
                                    else
                                    {
                                        temp.Add("Fatherid", 0);
                                    }
                                    if (studDet.FirstOrDefault().AMST_MotherEmailId != "" && studDet.FirstOrDefault().AMST_MotherEmailId != null)
                                    {

                                        //string motherotp = otp.getFourDigitOTPMother();
                                        //string motherotp = otp.GeneratePassword_Mother();

                                        string motherotp = studotp;
                                        // string MotherName = virtualcode + data.MI_Id.ToString() + "M" + motherotp.ToString();
                                        string MotherName = virtualcode + "M" + motherotp.ToString();

                                        // string MotherName = studDet.FirstOrDefault().AMST_FatherName.Substring(0,4) + motherotp;
                                        MotherName = Regex.Replace(MotherName, @"\s+", "");
                                        if (studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != null && studDet.FirstOrDefault().AMST_MotherMobileNo.ToString() != "")
                                        {
                                            data.AMST_MotherMobileNo = studDet.FirstOrDefault().AMST_MotherMobileNo;
                                        }
                                        else
                                        {
                                            data.AMST_MotherMobileNo = 0;
                                        }
                                        ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                        motuserid = response.useridapp;
                                        res = response.resp;
                                        if (motuserid == 0)
                                        {
                                            motherotp = otp.GeneratePassword_Mother();
                                            //  MotherName = virtualcode + data.MI_Id.ToString() + "M" + motherotp.ToString();

                                            MotherName = virtualcode + "M" + motherotp.ToString();
                                            ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().AMST_MotherEmailId, MotherName, data.MI_Id, "PARENTS", "PARENTS", data.AMST_MotherMobileNo.ToString()).Result;
                                            motuserid = response1.useridapp;
                                            res = response1.resp;
                                            temp.Add("motherid", motuserid);
                                        }
                                        else
                                        {
                                            temp.Add("motherid", motuserid);
                                        }
                                        bool val = AddStudentUserLogin(data.MI_Id, MotherName, studDet.FirstOrDefault().AMST_Id);
                                        if (res == "Success" && val == true)
                                        {
                                        }
                                        motherotp = "";
                                    }
                                    else
                                    {
                                        temp.Add("motherid", 0);
                                    }
                                    if (temp.Count != 0)
                                    {
                                        long uid = 0;
                                        long fid = 0;
                                        long mid = 0;
                                        if (temp["studentid"] != 0)
                                        {
                                            uid = temp["studentid"];
                                        }
                                        if (temp["Fatherid"] != 0)
                                        {
                                            fid = temp["Fatherid"];
                                        }
                                        if (temp["motherid"] != 0)
                                        {
                                            mid = temp["motherid"];
                                        }

                                        bool vall = AddStudentApplogin(uid, fid, mid, studDet.FirstOrDefault().AMST_Id);
                                        if (vall == true)
                                        {
                                            counts = counts + 1;
                                        }
                                        else
                                        {
                                            countf = countf + 1;
                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                            catch (Exception ex)
                            {
                                failname += "," + admno;
                                countf = countf + 1;
                                Console.WriteLine(ex.Message);
                                continue;
                            }
                        }
                    }
                }
                else if (data.filterdata == "Alumni")
                {
                    for (int kk = 0; kk < data.data_array.Count(); kk++)
                    {
                        try
                        {
                            var amst_id = data.data_array[kk].AMST_Id;

                            generateOTP otp = new generateOTP();
                            string studotp = otp.GeneratePassword();

                            var studDet = _db.Alumni_M_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.ALMST_Id == amst_id).ToList();
                            admno = studDet.FirstOrDefault().ALMST_AdmNo;



                            long stduserid = 0;
                            string res = "";
                            Dictionary<string, long> temp = new Dictionary<string, long>();
                            if (studDet.FirstOrDefault().ALMST_emailId != "" && studDet.FirstOrDefault().ALMST_emailId != null)
                            {
                                string StudentName = "ALU" + studotp.ToString();
                                ImportStudentWrapperDTO response = Createlogins(studDet.FirstOrDefault().ALMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALMST_MobileNo.ToString()).Result;
                                stduserid = response.useridapp;
                                res = response.resp;
                                if (stduserid == 0)
                                {
                                    studotp = otp.GeneratePassword();
                                    StudentName = "ALU" + studotp.ToString();
                                    ImportStudentWrapperDTO response1 = Createlogins(studDet.FirstOrDefault().ALMST_emailId, StudentName, data.MI_Id, "ALUMNI", "ALUMNI", studDet.FirstOrDefault().ALMST_MobileNo.ToString()).Result;
                                    stduserid = response1.useridapp;
                                    res = response1.resp;
                                    temp.Add("studentid", stduserid);
                                }
                                else
                                {
                                    temp.Add("studentid", stduserid);

                                    AlumniUserRegistrationDMO Alumni = new AlumniUserRegistrationDMO();
                                    Alumni.MI_Id = studDet.FirstOrDefault().MI_Id;
                                    Alumni.ALSREG_Photo = studDet.FirstOrDefault().ALMST_StudentPhoto;
                                    Alumni.ALSREG_ApprovedFlag = true;
                                    Alumni.ALSREG_MemberName = studDet.FirstOrDefault().ALMST_FirstName;
                                    Alumni.ALSREG_EmailId = studDet.FirstOrDefault().ALMST_emailId;
                                    Alumni.ALSREG_MobileNo = Convert.ToInt64(studDet.FirstOrDefault().ALMST_MobileNo);
                                    Alumni.ALSREG_AdmittedYear = Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id_Join);
                                    Alumni.ALSREG_LeftYear = Convert.ToInt64(studDet.FirstOrDefault().ASMAY_Id_Left);
                                    Alumni.ALSREG_LeftClass = Convert.ToInt64(studDet.FirstOrDefault().ASMCL_Id_Left);
                                    Alumni.ALSREG_AdmittedClass = Convert.ToInt64(studDet.FirstOrDefault().ASMCL_Id_Join);
                                    Alumni.ALSREG_Date = DateTime.Now;
                                    Alumni.CreatedDate = DateTime.Now;
                                    Alumni.UpdatedDate = DateTime.Now;
                                    Alumni.ALMST_Id = studDet.FirstOrDefault().ALMST_Id;
                                    Alumni.ALSREG_CreatedBy = stduserid;
                                    Alumni.ALSREG_UpdatedBy = stduserid;
                                    Alumni.ALSREG_ActiveFlg = true;
                                    _db.Add(Alumni);
                                    _db.SaveChanges();

                                    Alumni_User_LoginDMO alumniluser = new Alumni_User_LoginDMO();
                                    alumniluser.ALSREG_Id = Alumni.ALSREG_Id;
                                    alumniluser.IVRMUL_Id = stduserid;
                                    _db.Add(alumniluser);
                                    _db.SaveChanges();
                                }
                            }
                            else
                            {
                                temp.Add("studentid", 0);
                            }
                        }
                        catch (Exception ex)
                        {
                            failname += "," + admno;
                            countf = countf + 1;
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                    }

                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (counts == data.data_array.Count())
            {
                data.message = "Record Saved";
            }
            else
            {
                if (failname != "")
                {
                    data.message = "Failed Record " + admno + "";
                }
                else
                {
                    data.message = "Record Saved";
                }
            }

            return data.message;
        }
        public bool AddStudentApplogin(long userid, long fatherid, long motherid, long amstId)
        {
            StudentAppUserLoginDMO dmo = new StudentAppUserLoginDMO();
            dmo.AMST_ID = amstId;
            dmo.STD_APP_ID = Convert.ToInt32(userid);
            dmo.FAT_APP_ID = Convert.ToInt32(fatherid);
            dmo.MOT_APP_ID = Convert.ToInt32(motherid);

            _db.Add(dmo);
            var flag = _db.SaveChanges();
            if (flag > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddStudentUserLogin(long mi_id, string username, long amstId)
        {
            StudentUserLoginDMO dmo = new StudentUserLoginDMO();
            dmo.AMST_Id = amstId;
            dmo.CreatedDate = DateTime.Now;
            dmo.IVRMSTUUL_ActiveFlag = 1;
            dmo.IVRMSTUUL_Password = "Password@123";
            dmo.IVRMSTUUL_UserName = username;
            dmo.MI_Id = mi_id;
            dmo.UpdatedDate = DateTime.Now;
            _db.Add(dmo);
            var flag = _db.SaveChanges();
            if (flag > 0)
            {
                StudentUserLogin_Institutionwise inst = new StudentUserLogin_Institutionwise();
                inst.AMST_Id = amstId;
                inst.CreatedDate = DateTime.Now;
                inst.IVRMSTUULI_ActiveFlag = 1;
                inst.IVRMSTUUL_Id = dmo.IVRMSTUUL_Id;
                inst.UpdatedDate = DateTime.Now;
                _db.Add(inst);
                var flag1 = _db.SaveChanges();
                if (flag1 > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public async Task<ImportStudentWrapperDTO> Createlogins(string emailid, string name, long mi_id, string roles, string roletype, string mobile)
        {
            ImportStudentWrapperDTO respdto = new ImportStudentWrapperDTO();
            //string resp = ""; 
            //Creating Student and parents login as well as Sending user name and password code starts.
            try
            {
                ApplicationUser user = new ApplicationUser();
                user = await _UserManager.FindByNameAsync(name);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = name, Email = emailid, PhoneNumber = mobile };
                    user.Entry_Date = DateTime.Now;
                    user.EmailConfirmed = true;
                    var result = await _UserManager.CreateAsync(user, "Password@123");
                    if (result.Succeeded)
                    {
                        // Student Roles
                        string studentRole = roles;
                        var id = _db.applicationRole.Single(d => d.Name == studentRole);
                        //

                        // Student Role Type
                        string studentRoleType = roletype;
                        var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
                        //

                        // Save role
                        var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.IVRMRT_Id) };
                        role.CreatedDate = DateTime.Now;
                        role.UpdatedDate = DateTime.Now;
                        _db.appUserRole.Add(role);
                        _db.SaveChanges();
                        respdto.useridapp = role.UserId;
                        UserRoleWithInstituteDMO mas1 = new UserRoleWithInstituteDMO();
                        mas1.Id = user.Id;
                        mas1.MI_Id = mi_id;
                        mas1.Activeflag = 1;
                        _db.Add(mas1);
                        var res = _db.SaveChanges();
                        if (res > 0)
                        {
                            respdto.resp = "Success";
                        }
                        else
                        {
                            respdto.resp = "";
                        }

                    }
                    else
                    {
                        respdto.resp = result.Errors.FirstOrDefault().Description.ToString();
                    }
                }

            }
            catch (Exception e)
            {
                _areaimpl.LogInformation("Student Admission form error");
                _areaimpl.LogDebug(e.Message);
            }
            return respdto;

            //Creating Student and parents login as well as Sending user name and password code Ends.
        }

    }
}
