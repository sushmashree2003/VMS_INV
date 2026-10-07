using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;

namespace PortalHub.com.vaps.Student.Services
{
    public class COEImpl : Interfaces.COEInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _coecontext;
        public COEImpl(PortalContext coecontext)
        {
            _coecontext = coecontext;
        }
        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
        {
            try
            {
                //data.coeyearlist = (from a in _coecontext.School_Adm_Y_StudentDMO
                //                    from b in _coecontext.AcademicYearDMO
                //                    where (b.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id)
                //                    select new StudentDashboardDTO
                //                    {
                //                        ASMAY_Id = b.ASMAY_Id,
                //                        ASMAY_Year = b.ASMAY_Year
                //                    }
                //             ).ToArray();

                data.currentyear = (from a in _coecontext.School_Adm_Y_StudentDMO
                                    from b in _coecontext.AcademicYearDMO
                                    where (b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                    select new StudentDashboardDTO
                                    {
                                        ASMAY_Id = b.ASMAY_Id,
                                        ASMAY_Year = b.ASMAY_Year
                                    }
                             ).ToArray();
                data.coeyearlist = (from d in _coecontext.AcademicYearDMO
                                    from a in _coecontext.School_M_Class
                                    from b in _coecontext.School_M_Section
                                    from c in _coecontext.School_Adm_Y_StudentDMO
                                    where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                    a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                    select new ExamDTO
                                    {
                                        ASMCL_Id = c.ASMCL_Id,
                                        ASMCL_ClassName = a.ASMCL_ClassName,
                                        ASMS_Id = c.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                        ASMAY_Id = c.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year
                                    }
                             ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentDashboardDTO getcoedata(StudentDashboardDTO data)
        {
            try
            {
                data.coereportlist = (from a in _coecontext.COE_Master_EventsDMO
                                      from b in _coecontext.COE_EventsDMO
                                      from c in _coecontext.School_Adm_Y_StudentDMO
                                      from d in _coecontext.Adm_M_Student
                                      where (a.COEME_Id == b.COEME_Id && b.ASMAY_Id == c.ASMAY_Id && c.AMST_Id == d.AMST_Id && c.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == 4 && b.COEE_EStartDate.Value.Month == data.month)

                                      select new StudentDashboardDTO
                                      {
                                          COEME_EventName = a.COEME_EventName,
                                          COEME_EventDesc = a.COEME_EventDesc,
                                          COEE_EStartDate = b.COEE_EStartDate,
                                          COEE_EEndDate = b.COEE_EEndDate,
                                          ASMAY_Id = b.ASMAY_Id
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
