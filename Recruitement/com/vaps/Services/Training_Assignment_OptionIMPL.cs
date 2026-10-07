using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Training_Assignment_OptionIMPL : Interfaces.Training_Assignment_OptionInterface
    {
        private static ConcurrentDictionary<string, HR_Master_Floor_DTO> _HrMFD = new ConcurrentDictionary<string, HR_Master_Floor_DTO>();

        public VMSContext _vmsconte;
       

      
        public Training_Assignment_OptionIMPL(VMSContext vmsContext, HRMSContext hrms)
        {
            _vmsconte = vmsContext;
           

        }

        //get
        public HR_Training_Assignment_Option_DTO getdata_MFO(HR_Training_Assignment_Option_DTO dto)
        {
            try
            {
                dto.question_list = _vmsconte.HR_Training_Assignment_Option_DMO.Where(a => a.MI_Id == dto.MI_Id).OrderByDescending(a => a.HRTAOPT_Id).ToList().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public HR_Training_Assignment_Option_DTO SaveEdit_MFO(HR_Training_Assignment_Option_DTO dto)
        {

            try
            {
                if (dto.HRTAOPT_Id > 0)
                {
                    var result = _vmsconte.HR_Training_Assignment_Option_DMO.Single(a => a.HRTAOPT_Id == dto.HRTAOPT_Id && a.MI_Id == dto.MI_Id);
                    result.HRTAOPT_OptionName = dto.HRTAOPT_OptionName;
                    result.HRTAOPT_OptionOrder = dto.HRTAOPT_OptionOrder;
                    result.HRTAOPT_UpdatedBy = dto.userId;
                    result.HRTAOPT_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";

                }
                else
                {
                    HR_Training_Assignment_Option_DMO t = new HR_Training_Assignment_Option_DMO();
                    t.HRTAOPT_OptionName = dto.HRTAOPT_OptionName;
                    t.HRTAOPT_OptionOrder = dto.HRTAOPT_OptionOrder;
                    t.HRTAOPT_ActiveFlg = true;                 
                    t.HRTAOPT_CreatedBy = dto.userId;
                    t.HRTAOPT_UpdatedDate = DateTime.Now;
                    t.HRTAOPT_UpdatedBy = dto.userId;
                    t.HRTAOPT_CreatedDate = DateTime.Now;
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
        public HR_Training_Assignment_Option_DTO deactivate_MFO(HR_Training_Assignment_Option_DTO dto)
        {
            try
            {
                var result = _vmsconte.HR_Training_Assignment_Option_DMO.Single(a => a.HRTAOPT_Id == dto.HRTAOPT_Id && a.MI_Id == dto.MI_Id);
                if (dto.HRTAOPT_ActiveFlg == true)
                {
                    result.HRTAOPT_ActiveFlg = false;
                    result.HRTAOPT_UpdatedBy = dto.userId;
                    result.HRTAOPT_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = false;
                }
                else
                {
                    result.HRTAOPT_ActiveFlg = true;
                    result.HRTAOPT_UpdatedBy = dto.userId;
                    result.HRTAOPT_UpdatedDate = DateTime.Now;
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

        public HR_Training_Assignment_Option_DTO details_MFO(HR_Training_Assignment_Option_DTO dto)
        {
            try
            {
                dto.question_list = _vmsconte.HR_Training_Assignment_Option_DMO.Where(a => a.MI_Id == dto.MI_Id && a.HRTAOPT_Id == dto.HRTAOPT_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}


