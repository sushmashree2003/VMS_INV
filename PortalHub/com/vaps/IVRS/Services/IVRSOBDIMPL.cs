using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Services
{
    public class IVRSOBDIMPL : Interfaces.IVRSOBDInterface
    {
        public DomainModelMsSqlServerContext _db;
        public PortalContext _ivrs;
        public IVRSOBDIMPL(DomainModelMsSqlServerContext db, PortalContext ivrs)
        {
            _db = db;
            _ivrs = ivrs;
        }
        public IVRSOBD getdetails(IVRSOBD data)
        {
            data.clas_list = _ivrs.School_M_Class.Where(f => f.MI_Id == data.MI_ID && f.ASMCL_ActiveFlag == true).ToArray();
            data.sect_list = _ivrs.School_M_Section.Where(f => f.MI_Id == data.MI_ID && f.ASMC_ActiveFlag == 1).ToArray();
            data.acs_lst = _ivrs.AcademicYearDMO.Where(f => f.MI_Id == data.MI_ID && f.Is_Active == true).ToArray();
            data.schoolname = _ivrs.Institution_master.Where(g => g.MI_Id == data.MI_ID && g.MI_ActiveFlag == 1).Select(b => b.MI_Name).FirstOrDefault();
            return data;
        }
        public IVRSOBD ivrgetstudetails(IVRSOBD data)
        {
            try
            {
                if (data.ASMS_ID > 0)
                {
                    data.maindata = (from a in _ivrs.AdmissionStudentDMO
                                     from b in _ivrs.School_Adm_Y_StudentDMO
                                     where (a.AMST_Id == b.AMST_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.ASMAY_Id == data.ASMAY_ID && b.ASMCL_Id == data.ASMCL_ID && b.ASMS_Id == data.ASMS_ID)
                                     select new IVRSOBD
                                     {
                                         AMST_Id = a.AMST_Id,
                                         studentName = a.AMST_FirstName,
                                         AMST_AdmNo = a.AMST_AdmNo,
                                         AMST_emailId = a.AMST_emailId,
                                         AMST_MobileNo = a.AMST_MobileNo
                                     }).ToArray();
                }
                else
                {
                    data.maindata = (from a in _ivrs.AdmissionStudentDMO
                                     from b in _ivrs.School_Adm_Y_StudentDMO
                                     where (a.AMST_Id == b.AMST_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.ASMAY_Id == data.ASMAY_ID && b.ASMCL_Id == data.ASMCL_ID)
                                     select new IVRSOBD
                                     {
                                         AMST_Id = a.AMST_Id,
                                         studentName = a.AMST_FirstName,
                                         AMST_AdmNo = a.AMST_AdmNo,
                                         AMST_emailId = a.AMST_emailId,
                                         AMST_MobileNo = a.AMST_MobileNo
                                     }).ToArray();
                }

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
    }
}
