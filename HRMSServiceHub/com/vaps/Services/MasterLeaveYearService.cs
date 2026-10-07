using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class MasterLeaveYearService : Interfaces.MasterLeaveYearInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterLeaveYearService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public HR_Master_LeaveYearDTO getBasicData(HR_Master_LeaveYearDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }


            return dto;
        }
        public HR_Master_LeaveYearDTO SaveUpdate(HR_Master_LeaveYearDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_LeaveYearDMO dmoObj = Mapper.Map<HR_Master_LeaveYearDMO>(dto);

                var duplicatecountresult = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && t.HRMLY_FromDate == dto.HRMLY_FromDate && t.HRMLY_ToDate == dto.HRMLY_ToDate).Count();

                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMLY_Id > 0)
                    {
                        var duplicateyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && t.HRMLY_LeaveYear == dto.HRMLY_LeaveYear && t.HRMLY_Id != dto.HRMLY_Id).Count();
                        if (duplicateyear == 0)
                        {
                            var result = _HRMSContext.HR_MasterLeaveYear.Single(t => t.HRMLY_Id == dmoObj.HRMLY_Id);

                            dto.UpdatedDate = DateTime.Now;
                            result.HRMLY_UpdatedBy = dto.LogInUserId;
                            Mapper.Map(dto, result);
                            _HRMSContext.Update(result);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else if (duplicateyear > 0)
                        {
                            dto.retrunMsg = "Year";
                            return dto;
                        }
                    }
                    else
                    {
                        var duplicateyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id && t.HRMLY_LeaveYear == dto.HRMLY_LeaveYear).Count();
                        if (duplicateyear == 0)
                        {
                            var Countorder = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id == dto.MI_Id).Count();

                            dmoObj.HRMLY_ActiveFlag = true;
                            dmoObj.HRMLY_LeaveYear = dto.HRMLY_LeaveYear;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRMLY_LeaveYearOrder = Countorder + 1;
                            dmoObj.HRMLY_FromDate = dto.HRMLY_FromDate;
                            dmoObj.HRMLY_ToDate = dto.HRMLY_ToDate;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            dmoObj.HRMLY_UpdatedBy = dto.LogInUserId;
                            dmoObj.HRMLY_CreatedBy = dto.LogInUserId;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.HRMLY_Id = dmoObj.HRMLY_Id;
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else if (duplicateyear > 0)
                        {
                            dto.retrunMsg = "Year";
                            return dto;
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
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
        public HR_Master_LeaveYearDTO editData(int id)
        {

            HR_Master_LeaveYearDTO dto = new HR_Master_LeaveYearDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_LeaveYearDMO> lorg = new List<HR_Master_LeaveYearDMO>();
                lorg = _HRMSContext.HR_MasterLeaveYear.AsNoTracking().Where(t => t.HRMLY_Id.Equals(id)).ToList();
                dto.leaveYearList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_LeaveYearDTO deactivate(HR_Master_LeaveYearDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMLY_Id > 0)
                {
                    var result = _HRMSContext.HR_MasterLeaveYear.Single(t => t.HRMLY_Id == dto.HRMLY_Id);

                    if (result.HRMLY_ActiveFlag == true)
                    {
                        result.HRMLY_ActiveFlag = false;
                    }
                    else if (result.HRMLY_ActiveFlag == false)
                    {
                        result.HRMLY_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    result.HRMLY_UpdatedBy = dto.LogInUserId;
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMLY_ActiveFlag == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_LeaveYearDTO GetAllDropdownAndDatatableDetails(HR_Master_LeaveYearDTO dto)
        {
            List<HR_Master_LeaveYearDMO> datalist = new List<HR_Master_LeaveYearDMO>();
            try
            {
                datalist = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.leaveYearList = datalist.OrderByDescending(a=>a.CreatedDate).ToArray();

                dto.leaveYearListorderwise= _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id)).OrderBy(a=>a.HRMLY_LeaveYearOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_Master_LeaveYearDTO SetLeaveOrder(HR_Master_LeaveYearDTO data)
        {
            try
            {
                data.retrunMsg = "Failed";
                if (data.LeaveYearOrder!=null && data.LeaveYearOrder.Length > 0)
                {
                    var countorder = 0;
                    foreach(var c in data.LeaveYearOrder)
                    {
                        countorder += 1;

                        var Result = _HRMSContext.HR_MasterLeaveYear.Single(a => a.HRMLY_Id == c.HRMLY_Id);
                        Result.HRMLY_LeaveYearOrder = countorder;
                        Result.UpdatedDate = DateTime.Now;
                        _HRMSContext.Update(Result);
                    }

                    var i = _HRMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.retrunMsg = "Update";
                    }

                    data.leaveYearListorderwise = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(data.MI_Id)).OrderBy(a => a.HRMLY_LeaveYearOrder).ToArray();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
