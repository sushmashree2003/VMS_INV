using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class CandidateListVMSService : Interfaces.CandidateListVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public CandidateListVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext) 
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public HR_Candidate_DetailsDTO getBasicData(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                var institutionlist = (from a in _VMSContext.Institution
                                       where a.MI_ActiveFlag == 1
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                dto.institutionlist = institutionlist.ToArray();
                if (dto.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        dto.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Candidate_DetailsDTO SaveUpdate(HR_Candidate_DetailsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Candidate_DetailsDMO dmoObj = Mapper.Map<HR_Candidate_DetailsDMO>(dto);
                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Candidate_DetailsDTO editData(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Candidate_DetailsDMO> lorg = new List<HR_Candidate_DetailsDMO>();

                dto.VMSEditValue = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == id).ToArray();
                dto.qualificationlist = _VMSContext.HR_Candidate_QualificationsDMO.Where(t => t.HRCD_Id == id).ToArray();
                dto.experiencelist = _VMSContext.HR_Candidate_ExperienceDMO.Where(t => t.HRCD_Id == id).ToArray();
                dto.languagelist = _VMSContext.HR_Candidate_LanguagesDMO.Where(t => t.HRCD_Id == id).ToArray();
                dto.familylist = _VMSContext.HR_Candidate_FamilyDMO.Where(t => t.HRCD_Id == id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Candidate_DetailsDTO GetAllDropdownAndDatatableDetails(HR_Candidate_DetailsDTO dto)
        {
            List<HR_Candidate_DetailsDMO> datalist = new List<HR_Candidate_DetailsDMO>();
            try
            {
                var mrfdatalist = from a in _VMSContext.HR_Candidate_DetailsDMO
                                  from b in _VMSContext.HR_Master_JobsDMO
                                  where (a.HRMJ_Id == b.HRMJ_Id && a.MI_Id == dto.MI_Id)
                                  select new HR_Candidate_DetailsDTO
                                  {
                                      HRCD_Id = a.HRCD_Id,
                                      HRCD_FullName = a.HRCD_FirstName + " " + (a.HRCD_MiddleName == null ? "" : a.HRCD_MiddleName) + " " + (a.HRCD_LastName == null ? "" : a.HRCD_LastName),
                                      HRCD_FirstName = a.HRCD_FirstName,
                                      HRCD_MiddleName = a.HRCD_MiddleName,
                                      HRCD_LastName = a.HRCD_LastName,
                                      HRMJ_Id = a.HRMJ_Id,
                                      HRCD_ExpFrom = a.HRCD_ExpFrom,
                                      HRCD_ExpTo = a.HRCD_ExpTo,
                                      applydate = a.HRCD_AppDate.ToString("dd-MM-yyyy"),
                                      HRCD_RecruitmentStatus = a.HRCD_RecruitmentStatus,
                                      HRMJ_JobTiTle = b.HRMJ_JobTiTle
                                  };
                dto.VMSMRFList = mrfdatalist.ToArray();

                dto.MasterPosTypeList = (from emp in _VMSContext.HR_Master_PostionTypeDMO
                                         where emp.HRMPT_ActiveFlg == true && emp.MI_Id == dto.MI_Id
                                         select new HR_Candidate_DetailsDTO
                                         {
                                             HRMPT_Id = emp.HRMPT_Id,
                                             HRMPT_Name = emp.HRMPT_Name
                                         }).ToArray();

                dto.MasterPositionList = (from emp in _VMSContext.HR_Master_PositionDMO
                                          where emp.HRMP_ActiveFlg == true
                                          select new HR_Candidate_DetailsDTO
                                          {
                                              HRMP_Id = emp.HRMP_Id,
                                              HRMP_Position = emp.HRMP_Position
                                          }).ToArray();

                dto.MasterQualification = (from emp in _VMSContext.HR_Master_Course
                                           where emp.HRMC_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                           select new HR_Candidate_DetailsDTO
                                           {
                                               HRMC_Id = emp.HRMC_Id,
                                               HRMC_QulaificationName = emp.HRMC_QulaificationName
                                           }).ToArray();

                dto.MasterGender = (from emp in _VMSContext.IVRM_Master_Gender
                                    where emp.IVRMMG_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                    select new HR_Candidate_DetailsDTO
                                    {
                                        IVRMMG_Id = emp.IVRMMG_Id,
                                        IVRMMG_GenderName = emp.IVRMMG_GenderName
                                    }).ToArray();

                dto.masterjob = (from a in _VMSContext.HR_Master_JobsDMO
                                 where a.HRMJ_ActiveFlg == true
                                 select new HR_Candidate_DetailsDTO
                                 {
                                     HRMJ_Id = a.HRMJ_Id,
                                     HRMJ_JobCode = a.HRMJ_JobCode,
                                     HRMJ_JobTiTle = a.HRMJ_JobTiTle
                                 }).ToArray();

                dto.mastercountry = _VMSContext.IVRM_Master_Country.ToArray();
                dto.masterReligion = _VMSContext.MasterReligionDMO.Where(t => t.Is_Active == true).ToArray();
                dto.masterCaste = _VMSContext.mastercasteDMO.Where(t => t.MI_Id == dto.MI_Id).ToArray();
                dto.mastermaritalstatus = _VMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id == dto.MI_Id && t.IVRMMMS_ActiveFlag == true).ToArray();
                dto.cousultencydetail = _VMSContext.Mastert_ConsultancyDMO.Where(t => t.HRMCON_ActiveFlg == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
