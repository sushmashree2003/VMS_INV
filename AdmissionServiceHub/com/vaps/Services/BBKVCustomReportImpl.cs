using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AdmissionServiceHub.com.vaps.Services
{

    public class BBKVCustomReportImpl : Interfaces.BBKVCustomReportInterface
    {
        private static ConcurrentDictionary<string, BBKVCustomReportDTO> _tcreport =
      new ConcurrentDictionary<string, BBKVCustomReportDTO>();

        public readonly HHSTCCustomReportContext _reporttc;
        private object c;
        ILogger<BBKVCustomReportImpl> _reportimpl;

        public BBKVCustomReportImpl(HHSTCCustomReportContext reporttc, ILogger<BBKVCustomReportImpl> reportimpl)
        {
            _reporttc = reporttc;
            _reportimpl = reportimpl;
        }

        public BBKVCustomReportDTO getdetails(int id)
        {
            BBKVCustomReportDTO data = new BBKVCustomReportDTO();

            data.accyear = _reporttc.accyear.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToArray();
            data.accclass = _reporttc.accclass.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).OrderBy(t=>t.ASMCL_Order).ToArray();
            data.accsection = _reporttc.accsection.Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).OrderBy(t=>t.ASMC_Order).ToArray();

            return data;
        }

        public BBKVCustomReportDTO getnameregno(BBKVCustomReportDTO data)
        {
            try
            {
                var flag = "";
                if (data.tctemporper == "temp")
                {
                    flag = "T";
                }
                else if (data.tctemporper == "per")
                {
                    flag = "L";
                }
                if (data.admnoorname == "regno")
                {
                    data.studentlist = (from a in _reporttc.studenttc
                                        from b in _reporttc.yearwisestudent
                                        from c in _reporttc.student
                                        where (a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_SOL == flag && b.ASMAY_Id == data.ASMAY_Id)
                                        select new BBKVCustomReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            studentname = ((c.AMST_AdmNo == null ? " " : c.AMST_AdmNo) + ':' + (c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                        }
                                     ).Distinct().ToArray();
                }
                else if (data.admnoorname == "stdname")
                {
                    data.studentlist = (from a in _reporttc.studenttc
                                        from b in _reporttc.yearwisestudent
                                        from c in _reporttc.student
                                        where (a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_SOL == flag && b.ASMAY_Id == data.ASMAY_Id)
                                        select new BBKVCustomReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            studentname = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName) + ':' + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                        }
                                     ).Distinct().ToArray();
                }



            }
            catch (Exception ex)
            {
                _reportimpl.LogInformation("HHSTcCustomReportError:" + ex.Message);
            }
            return data;
        }

        public BBKVCustomReportDTO stdnamechange(BBKVCustomReportDTO data)
        {
            try
            {
                data.classsecregno = (from a in _reporttc.yearwisestudent
                                      from b in _reporttc.student
                                      from c in _reporttc.accclass
                                      from d in _reporttc.accsection
                                      where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new BBKVCustomReportDTO
                                      {
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          AMST_RegistrationNo = b.AMST_RegistrationNo
                                      }
                                   ).ToArray();
            }
            catch (Exception ex)
            {
                _reportimpl.LogInformation("NamesearchBBKVCustomReportError:" + ex.Message);
            }
            return data;
        }

        public BBKVCustomReportDTO onclicktcperortemo(BBKVCustomReportDTO data)
        {
            try
            {
                var flag = "";
                if (data.tctemporper == "temp")
                {
                    flag = "T";
                }
                else if (data.tctemporper == "per")
                {
                    flag = "L";
                }


                if (data.admnoorname == "regno")
                {
                    data.studentlist = (from a in _reporttc.studenttc
                                        from b in _reporttc.yearwisestudent
                                        from c in _reporttc.student
                                        where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == flag && b.ASMAY_Id == data.ASMAY_Id)
                                        select new BBKVCustomReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            studentname = ((c.AMST_AdmNo == null ? " " : c.AMST_AdmNo) + ':' + (c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                        }).Distinct().ToArray();
                }
                else if (data.admnoorname == "stdname")
                {
                    data.studentlist = (from a in _reporttc.studenttc
                                        from b in _reporttc.yearwisestudent
                                        from c in _reporttc.student
                                        where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == flag && b.ASMAY_Id == data.ASMAY_Id)
                                        select new BBKVCustomReportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            studentname = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName) + ':' + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                        }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                _reportimpl.LogInformation("onclicktempHHSTCreportError:" + ex.Message);
            }
            return data;

        }

        public BBKVCustomReportDTO getTcdetails(BBKVCustomReportDTO data)
        {
            try
            {
                var flag = "";
                if (data.tctemporper == "temp")
                {
                    flag = "T";
                }
                else if (data.tctemporper == "per")
                {
                    flag = "L";
                }

                data.studentTCList = (from a in _reporttc.student
                                      from b in _reporttc.studenttc
                                      from c in _reporttc.accsection
                                      from d in _reporttc.accclass
                                      from r in _reporttc.religion
                                      from cas in _reporttc.caste
                                      where (a.AMST_Id == b.AMST_Id
                                      && b.ASMS_Id == c.ASMS_Id
                                      && a.AMST_Id == data.AMST_Id
                                      && b.ASMCL_Id == d.ASMCL_Id
                                      && a.IVRMMR_Id== r.IVRMMR_Id
                                      && a.IC_Id == cas.IMC_Id && a.AMST_SOL == flag)
                                      select new BBKVCustomReportDTO
                                      {
                                          studentname = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? " " : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? " " : " " + a.AMST_LastName)).Trim(),
                                          AMST_AdmNo = a.AMST_AdmNo,
                                          AMST_RegistrationNo = a.AMST_RegistrationNo,
                                          AMST_FatherName = a.AMST_FatherName,
                                          AMST_MotherName = a.AMST_MotherName,
                                          Nationality = a.AMST_Nationality != 0 ? _reporttc.Country.Single(v => v.IVRMMC_Id == a.AMST_Nationality).IVRMMC_Nationality : "",
                                          AMST_BirthPlace = a.AMST_BirthPlace,
                                          ASTC_LastAttendedDate = b.ASTC_LastAttendedDate,
                                          AMST_Sex = a.AMST_Sex,
                                          AMST_DOB = a.AMST_DOB.Date,
                                          AMST_DOB_Words = a.AMST_DOB_Words,
                                          AMST_Date = a.AMST_Date.Date,
                                          astC_TCNO = b.ASTC_TCNO,
                                          AMST_emailId = a.AMST_emailId,
                                          AMST_AadharNo = a.AMST_AadharNo,
                                          AMST_MobileNo = a.AMST_MobileNo,
                                          ASMCL_Id = d.ASMCL_Id,
                                          Last_Class_Studied = d.ASMCL_ClassName,
                                          astC_LeavingReason = b.ASTC_LeavingReason,
                                          astC_TCIssueDate = b.ASTC_TCDate,
                                          AMST_PerCity = a.AMST_PerCity,
                                          AMST_PerStreet = a.AMST_PerStreet,
                                          AMST_PerArea = a.AMST_PerArea,
                                          AMST_ConStreet = a.AMST_ConStreet,
                                          AMST_ConArea = a.AMST_ConArea,
                                          AMST_ConCity = a.AMST_ConCity,
                                          ASTC_Remarks = b.ASTC_Remarks,
                                          Religion = r.IVRMMR_Name,
                                          caste = cas.IMC_CasteName,
                                          ASTC_Conduct = b.ASTC_Conduct,
                                          ASMC_SectionName = c.ASMC_SectionName,
                                          ASTC_Qual_PromotionFlag = b.ASTC_Qual_Class,
                                          Fee_Due_Amnt = b.Fee_Due_Amnt,
                                          Library_Due_Amnt = b.Library_Due_Amnt,
                                          Store_Canteen_Due = b.Store_Canteen_Due,
                                          PDA_Due = b.PDA_Due,
                                          classname = d.ASMCL_ClassName,
                                          qualificlass = b.ASTC_Qual_Class,
                                          tcdatess = b.ASTC_TCDate,
                                          langustudies = b.ASTC_LanguageStudied,
                                          electivestudies = b.ASTC_ElectivesStudied,
                                          promotedflag = b.ASTC_Qual_PromotionFlag,
                                          feedue = b.ASTC_FeePaid,
                                          feeconcession = b.ASTC_FeeConcession,
                                          totalworkingdays = b.ASTC_WorkingDays,
                                          noworkingdays = b.ASTC_AttendedDays,
                                          govtadmno = a.AMST_GovtAdmno,
                                          ASTC_TCApplicationDate = b.ASTC_TCApplicationDate,
                                          subcaste=a.AMST_SubCasteIMC_Id,
                                          medium=b.ASTC_MediumOfINStruction

                                      }).ToArray();

                data.MasterCompany = (from a in _reporttc.Institution
                                      where (a.MI_Id == data.MI_Id)
                                      select new BBKVCustomReportDTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          MI_Id = a.MI_Id,
                                      }).ToArray();

                data.academicList1 = _reporttc.accyear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.previousschool = _reporttc.StudentPrevSchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToArray();
                var getnextclass1 = (from a in _reporttc.studenttc
                                     where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id)
                                     select new BBKVCustomReportDTO
                                     {
                                         classid = a.ASMCL_Id,
                                     }).Distinct().ToArray();

                var classnext = getnextclass1.FirstOrDefault().classid + 1;
                data.getnextclass = _reporttc.accclass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == Convert.ToInt64(classnext.ToString())).ToArray();

                data.classnamejoin = (from a in _reporttc.student
                                      from b in _reporttc.accclass
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id)
                                      select new BBKVCustomReportDTO
                                      {
                                          joinclassid = a.ASMCL_Id,
                                          classjoinname = b.ASMCL_ClassName
                                      }).ToArray();
            }
            catch (Exception ex)
            {
                _reportimpl.LogInformation("getTcdetailsHHSTCError:" + ex.Message);
            }
            return data;
        }


    }

}
