using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class MasterStatusImpl : Interfaces.MasterStatusInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public MasterStatusImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }


        public MasterStatus_DTO getdetails(MasterStatus_DTO dTO)
        {
            try
            {
                dTO.deptlist = _Context.HR_Master_Department.Where(t => t.MI_Id == dTO.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_DepartmentName).ToArray();
                dTO.getStatusdetails = (from a in _IssueContext.MasterStatus_DMO
                                        from b in _IssueContext.HR_Master_Department
                                        where (a.MI_Id == b.MI_Id && a.HRMD_Id == b.HRMD_Id && a.MI_Id == dTO.MI_Id && b.HRMD_ActiveFlag == true)
                                        select new MasterStatus_DTO
                                        {
                                            HRMD_Id = a.HRMD_Id,
                                            ISMMISTS_ActiveFlag = a.ISMMISTS_ActiveFlag,
                                            ISMMISTS_Color = a.ISMMISTS_Color,
                                            ISMMISTS_Order = a.ISMMISTS_Order,
                                            ISMMISTS_StatusFlag = a.ISMMISTS_StatusFlag,
                                            ISMMISTS_Id = a.ISMMISTS_Id,
                                            ISMMISTS_StatusName = a.ISMMISTS_StatusName,
                                            HRMD_DepartmentName = b.HRMD_DepartmentName
                                        }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }

        public MasterStatus_DTO saverecord(MasterStatus_DTO data)
        {
            try
            {
                if (data.ISMMISTS_Id == 0)
                {
                    var duplicate = _IssueContext.MasterStatus_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMISTS_StatusName == data.ISMMISTS_StatusName && t.HRMD_Id == data.HRMD_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterStatus_DMO obj = new MasterStatus_DMO();
                        // obj.ISMMISTS_Id = data.ISMMISTS_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.ISMMISTS_Order = data.ISMMISTS_Order;
                        obj.ISMMISTS_StatusName = data.ISMMISTS_StatusName;
                        obj.ISMMISTS_Color = data.ISMMISTS_Color;
                        obj.ISMMISTS_ActiveFlag = true;
                        obj.ISMMISTS_CreatedBy = data.UserId;
                        obj.ISMMISTS_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMISTS_StatusFlag = data.ISMMISTS_StatusFlag;


                        _IssueContext.Add(obj);
                        int a = _IssueContext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var duplicate = _IssueContext.MasterStatus_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMISTS_Id != data.ISMMISTS_Id && t.HRMD_Id == data.HRMD_Id && t.ISMMISTS_StatusName == data.ISMMISTS_StatusName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var obj = _IssueContext.MasterStatus_DMO.Where(t => t.ISMMISTS_Id == data.ISMMISTS_Id).Single();

                        obj.HRMD_Id = data.HRMD_Id;
                        obj.ISMMISTS_Order = data.ISMMISTS_Order;
                        obj.ISMMISTS_StatusName = data.ISMMISTS_StatusName;
                        obj.ISMMISTS_StatusFlag = data.ISMMISTS_StatusFlag;
                        obj.ISMMISTS_Color = data.ISMMISTS_Color;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMISTS_UpdatedBy = data.UserId;

                        _IssueContext.Update(obj);
                        int a = _IssueContext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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

        public MasterStatus_DTO deactiveY(MasterStatus_DTO data)
        {
            try
            {
                var result = _IssueContext.MasterStatus_DMO.Single(t => t.MI_Id == data.MI_Id && t.ISMMISTS_Id == data.ISMMISTS_Id);
                if (result.ISMMISTS_ActiveFlag == true)
                {
                    result.ISMMISTS_ActiveFlag = false;
                }
                else if (result.ISMMISTS_ActiveFlag == false)
                {
                    result.ISMMISTS_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);
                int rowAffected = _IssueContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterStatus_DTO save_ack(MasterStatus_DTO dto)
        {
            try
            {
                ISM_ACKN_Status_DMO dmo = new ISM_ACKN_Status_DMO();
                DateTime dt = DateTime.Now;
                DateTime t_date = dt.Date;
                dmo.ISM_AS_Remarks = dto.ISM_AS_Remarks;
                dmo.User_Id = dto.UserId;
                dmo.ISM_Date = t_date;
                dmo.ISM_Created_Date = DateTime.Now;
                dmo.MI_Id = dto.MI_Id;
                _Context.Add(dmo);
                _Context.SaveChanges();
                dto.returnval = true;
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }



    }
}
