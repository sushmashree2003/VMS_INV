using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.Training;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Recruitment.com.vaps.Services
{
    public class Training_Assignment_QuestionsIMPL : Interfaces.Training_Assignment_QuestionsInterface
    {
        private static ConcurrentDictionary<string, HR_Training_Assignment_Questions_DTO> _HrMFD = new ConcurrentDictionary<string, HR_Training_Assignment_Questions_DTO>();

        public VMSContext _vmsconte;

        public Training_Assignment_QuestionsIMPL(VMSContext vmsContext)
        {
            _vmsconte = vmsContext;
            

        }
        public HR_Training_Assignment_Questions_DTO getdata_question(HR_Training_Assignment_Questions_DTO dto)
        {
        
            try
            {
                dto.question_list = _vmsconte.HR_Training_Assignment_Questions_DMO.Where(a => a.MI_Id == dto.MI_Id).OrderByDescending(a => a.HRTAQNS_Id).ToList().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_Training_Assignment_Questions_DTO SaveEdit_question(HR_Training_Assignment_Questions_DTO dto)
        {

            try
            {
                if (dto.HRTAQNS_Id > 0)
                {
                    var result = _vmsconte.HR_Training_Assignment_Questions_DMO.Single(a => a.HRTAQNS_Id == dto.HRTAQNS_Id && a.MI_Id == dto.MI_Id);

                    result.HRTAQNS_QuestionName = dto.HRTAQNS_QuestionName;
                    result.HRTAQNS_QuestionTypeFlg = dto.HRTAQNS_QuestionName;
                    result.HRTAQNS_QuestionOrder = dto.HRTAQNS_QuestionOrder;
                    result.HRTAQNS_UpdatedBy = dto.userId;
                    result.HRTAQNS_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else
                {
                    HR_Training_Assignment_Questions_DMO t = new HR_Training_Assignment_Questions_DMO();               
                    t.HRTAQNS_QuestionName = dto.HRTAQNS_QuestionName;
                    t.HRTAQNS_QuestionTypeFlg = dto.HRTAQNS_QuestionTypeFlg;
                    t.HRTAQNS_QuestionOrder = dto.HRTAQNS_QuestionOrder;
                    t.HRMTT_Id = 1;
                    t.HRTAQNS_ActiveFlg = true;
                    t.HRTAQNS_CreatedBy = dto.userId;
                    t.HRTAQNS_CreatedDate = DateTime.Now;
                    t.HRTAQNS_UpdatedBy = dto.userId;
                    t.HRTAQNS_UpdatedDate = DateTime.Now;
                    t.MI_Id = dto.MI_Id;
                    _vmsconte.Add(t);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public HR_Training_Assignment_Questions_DTO details_question(HR_Training_Assignment_Questions_DTO dto)
        {
            try
            {
                dto.question_list = _vmsconte.HR_Training_Assignment_Questions_DMO.Where(a => a.MI_Id == dto.MI_Id && a.HRTAQNS_Id == dto.HRTAQNS_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Training_Assignment_Questions_DTO deactivate_question(HR_Training_Assignment_Questions_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Training_Assignment_Questions_DMO.Single(a => a.HRTAQNS_Id == dto.HRTAQNS_Id && a.MI_Id == dto.MI_Id);
                if (dto.HRTAQNS_ActiveFlg == true)
                {
                    result.HRTAQNS_ActiveFlg = false;
                    result.HRTAQNS_UpdatedBy = dto.userId;
                    result.HRTAQNS_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = false;
                }
                else
                {
                    result.HRTAQNS_ActiveFlg = true;
                    result.HRTAQNS_UpdatedBy = dto.userId;
                    result.HRTAQNS_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = true;
                }

            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

    }
}
