using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class AddJobVMSService : Interfaces.AddJobVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public AddJobVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public HR_MRFRequisitionDTO getBasicData(HR_MRFRequisitionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //if (dto.HRMRFR_Id == 0)
                dto = GetAllDropdownAndDatatableDetails(dto);
                //if (dto.HRMRFR_Id != 0)
                //{
                //    HR_MRFRequisitionDTO objMRF = new HR_MRFRequisitionDTO();
                //    objMRF = editData(Convert.ToInt32(dto.HRMRFR_Id));
                //    dto.VMSMRFList = objMRF.VMSMRFList;
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_MRFRequisitionDTO SaveUpdate(HR_MRFRequisitionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_MRFRequisitionDMO dmoObj = Mapper.Map<HR_MRFRequisitionDMO>(dto);
                HR_MRF_ListDMO mrfListObj = new HR_MRF_ListDMO();

                //var duplicatecountresult = _VMSContext.HR_MRFRequisitionDMO.Where(t => t.HRMP_Id == dto.HRMP_Id && t.HRMD_Id == dto.HRMD_Id && t.HRMPR_Id == dto.HRMPR_Id && t.HRMPT_Id == dto.HRMPT_Id && t.HRMC_Id == dto.HRMC_Id && t.HRMRFR_MRFNO == dto.HRMRFR_MRFNO && t.HRMRFR_NoofPosition == dto.HRMRFR_NoofPosition && t.HRMRFR_Skills == dto.HRMRFR_Skills && t.HRMRFR_JobDesc == dto.HRMRFR_JobDesc && t.HRMRFR_ExpFrom == dto.HRMRFR_ExpFrom && t.HRMRFR_ExpTo == dto.HRMRFR_ExpTo && t.HRMRFR_Age == dto.HRMRFR_Age && t.IVRMMG_Id == dto.IVRMMG_Id && t.HRMRFR_Reason == dto.HRMRFR_Reason && t.HRMRFR_Remark == dto.HRMRFR_Remark && t.HRMRFR_WrittenTestFlg == dto.HRMRFR_WrittenTestFlg && t.HRMRFR_OnlineTestFlg == dto.HRMRFR_OnlineTestFlg && t.HRMRFR_TechnicalInterviewFlg == dto.HRMRFR_TechnicalInterviewFlg && t.HRMRFR_Attachment == dto.HRMRFR_Attachment && t.HRMRFR_Status == dto.HRMRFR_Status && t.HRMRFR_PayScaleFrom == dto.HRMRFR_PayScaleFrom && t.HRMRFR_PayScaleTo == dto.HRMRFR_PayScaleTo && t.HRMRFR_PositionFilled == dto.HRMRFR_PositionFilled && t.HRMRFR_HRComment == dto.HRMRFR_HRComment && t.HRMRFR_MDComment == dto.HRMRFR_MDComment && t.HRMRFR_JobLocation == dto.HRMRFR_JobLocation).Count();
                //if (duplicatecountresult == 0)
                //{
                    if (dmoObj.HRMRFR_Id > 0)
                    {
                        var result = _VMSContext.HR_MRFRequisitionDMO.Single(t => t.HRMRFR_Id == dmoObj.HRMRFR_Id);
                    //dto.HRMRFR_ActiveFlag =true;
                    //Mapper.Map(dto, result);

                    result.HRMRFR_ActiveFlag = true;
                    result.MI_Id = dto.MI_Id;
                    result.HRMP_Id = dto.HRMP_Id;
                    result.HRMD_Id = dto.HRMD_Id;
                    result.HRMPR_Id = dto.HRMPR_Id;
                    result.HRMPT_Id = dto.HRMPT_Id;
                    //dmoObj.HRMRFR_MRFNO = dto.HRMRFR_MRFNO;
                    dmoObj.HRMC_Id = dto.HRMC_Id;
                    result.HRMRFR_NoofPosition = dto.HRMRFR_NoofPosition;
                    result.HRMRFR_Skills = dto.HRMRFR_Skills;
                    result.HRMRFR_JobDesc = dto.HRMRFR_JobDesc;
                    result.HRMRFR_ExpFrom = dto.HRMRFR_ExpFrom;
                    result.HRMRFR_ExpTo = dto.HRMRFR_ExpTo;
                    result.HRMRFR_Age = dto.HRMRFR_Age;
                    result.IVRMMS_Id = dto.IVRMMS_Id;
                    result.IVRMMC_Id = dto.IVRMMC_Id;
                    dmoObj.IVRMMG_Id = dto.IVRMMG_Id;
                    result.HRMRFR_Gender = dto.HRMRFR_Gender;
                    result.HRMRFR_Qualification = dto.HRMRFR_Qualification;
                    result.HRMRFR_Reason = dto.HRMRFR_Reason;
                    result.HRMRFR_WrittenTestFlg = dto.HRMRFR_WrittenTestFlg;
                    result.HRMRFR_OnlineTestFlg = dto.HRMRFR_OnlineTestFlg;
                    result.HRMRFR_TechnicalInterviewFlg = dto.HRMRFR_TechnicalInterviewFlg;
                    result.HRMRFR_Remark = dto.HRMRFR_Remark;
                    result.HRMRFR_Attachment = dto.HRMRFR_Attachment;
                    result.HRMRFR_CreatedBy = dto.HRMRFR_UpdatedBy;
                    result.HRMRFR_UpdatedBy = dto.HRMRFR_UpdatedBy;
                    result.UpdatedDate = DateTime.Now;
                    result.HRMRFR_Status = dto.HRMRFR_Status;
                    result.HRMRFR_ManagerFlag = dto.HRMRFR_ManagerFlag;
                    result.HRMRFR_HRFlag = dto.HRMRFR_HRFlag;
                    result.HRMRFR_MDFlag = dto.HRMRFR_MDFlag;
                    result.HRMRFR_PayScaleFrom = dto.HRMRFR_PayScaleFrom;
                    result.HRMRFR_PayScaleTo = dto.HRMRFR_PayScaleTo;
                    result.HRMRFR_PositionFilled = dto.HRMRFR_PositionFilled;
                    result.HRMRFR_HRComment = dto.HRMRFR_HRComment;
                    result.HRMRFR_MDComment = dto.HRMRFR_MDComment;
                    result.HRMRFR_JobLocation = dto.HRMRFR_JobLocation;
                    _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRMRFR_ActiveFlag = true;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.HRMP_Id = dto.HRMP_Id;
                        dmoObj.HRMD_Id = dto.HRMD_Id;
                        dmoObj.HRMPR_Id = dto.HRMPR_Id;
                        dmoObj.HRMPT_Id = dto.HRMPT_Id;
                        //dmoObj.HRMRFR_MRFNO = dto.HRMRFR_MRFNO;
                        dmoObj.HRMC_Id = dto.HRMC_Id;
                        dmoObj.HRMRFR_NoofPosition = dto.HRMRFR_NoofPosition;
                        dmoObj.HRMRFR_Skills = dto.HRMRFR_Skills;
                        dmoObj.HRMRFR_JobDesc = dto.HRMRFR_JobDesc;
                        dmoObj.HRMRFR_ExpFrom = dto.HRMRFR_ExpFrom;
                        dmoObj.HRMRFR_ExpTo = dto.HRMRFR_ExpTo;
                        dmoObj.HRMRFR_Age = dto.HRMRFR_Age;
                        dmoObj.IVRMMS_Id = dto.IVRMMS_Id;
                        dmoObj.IVRMMC_Id = dto.IVRMMC_Id;
                        dmoObj.IVRMMG_Id = dto.IVRMMG_Id;
                        dmoObj.HRMRFR_Gender = dto.HRMRFR_Gender;
                        dmoObj.HRMRFR_Qualification = dto.HRMRFR_Qualification;
                        dmoObj.HRMRFR_Reason = dto.HRMRFR_Reason;
                        dmoObj.HRMRFR_WrittenTestFlg = dto.HRMRFR_WrittenTestFlg;
                        dmoObj.HRMRFR_OnlineTestFlg = dto.HRMRFR_OnlineTestFlg;
                        dmoObj.HRMRFR_TechnicalInterviewFlg = dto.HRMRFR_TechnicalInterviewFlg;
                        dmoObj.HRMRFR_Remark = dto.HRMRFR_Remark;
                        dmoObj.HRMRFR_Attachment = dto.HRMRFR_Attachment;
                        dmoObj.HRMRFR_CreatedBy = dto.HRMRFR_UpdatedBy;
                        dmoObj.HRMRFR_UpdatedBy = dto.HRMRFR_UpdatedBy;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;

                        dmoObj.HRMRFR_Status = dto.HRMRFR_Status;
                        dmoObj.HRMRFR_ManagerFlag = dto.HRMRFR_ManagerFlag;
                        dmoObj.HRMRFR_HRFlag = dto.HRMRFR_HRFlag;
                        dmoObj.HRMRFR_MDFlag = dto.HRMRFR_MDFlag;
                        dmoObj.HRMRFR_PayScaleFrom = dto.HRMRFR_PayScaleFrom;
                        dmoObj.HRMRFR_PayScaleTo = dto.HRMRFR_PayScaleTo;
                        dmoObj.HRMRFR_PositionFilled = dto.HRMRFR_PositionFilled;
                        dmoObj.HRMRFR_HRComment = dto.HRMRFR_HRComment;
                        dmoObj.HRMRFR_MDComment = dto.HRMRFR_MDComment;
                        dmoObj.HRMRFR_JobLocation = dto.HRMRFR_JobLocation;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                //}
                //else
                //{
                //    dto.retrunMsg = "Duplicate";
                //    return dto;
                //}

                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_MRFRequisitionDTO editData(int id)
        {
            HR_MRFRequisitionDTO dto = new HR_MRFRequisitionDTO();
            dto.retrunMsg = "";
            try
            {
                dto.VMSMRFList = (from a in _VMSContext.HR_MRFRequisitionDMO
                                  from b in _VMSContext.HR_Master_PositionDMO
                                  from c in _VMSContext.HR_Master_Department
                                  from d in _VMSContext.HR_Master_PriorityDMO
                                  from e in _VMSContext.HR_Master_PostionTypeDMO
                                  from g in _VMSContext.HR_Master_Course
                                  from h in _VMSContext.IVRM_Master_Gender
                                  where (a.HRMRFR_Id.Equals(id) && a.HRMP_Id == b.HRMP_Id && a.HRMD_Id == c.HRMD_Id && a.HRMPR_Id == d.HRMPR_Id && a.HRMPT_Id == e.HRMPT_Id && a.HRMC_Id == g.HRMC_Id)
                                  select new HR_MRFRequisitionDTO
                                  {
                                      HRMRFR_Id = a.HRMRFR_Id,
                                      HRMP_Id = a.HRMP_Id,
                                      HRMP_Position = b.HRMP_Position,
                                      HRMD_Id = a.HRMD_Id,
                                      HRMD_DepartmentName = c.HRMD_DepartmentName,
                                      HRMPR_Id = a.HRMPR_Id,
                                      HRMP_Name = d.HRMP_Name,
                                      HRMPT_Id = a.HRMPT_Id,
                                      HRMPT_Name = e.HRMPT_Name,
                                      HRMC_Id = a.HRMC_Id,
                                      HRMC_QulaificationName = g.HRMC_QulaificationName,
                                      HRMRFR_NoofPosition = a.HRMRFR_NoofPosition,
                                      HRMRFR_Skills = a.HRMRFR_Skills,
                                      HRMRFR_JobDesc = a.HRMRFR_JobDesc,
                                      HRMRFR_ExpFrom = a.HRMRFR_ExpFrom,
                                      HRMRFR_ExpTo = a.HRMRFR_ExpTo,
                                      HRMRFR_Age = a.HRMRFR_Age,
                                      HRMRFR_MRFNO = a.HRMRFR_MRFNO,
                                      IVRMMG_Id = a.IVRMMG_Id,
                                      IVRMMG_GenderName = h.IVRMMG_GenderName,
                                      HRMRFR_Reason = a.HRMRFR_Reason,
                                      HRMRFR_Remark = a.HRMRFR_Remark,
                                      HRMRFR_WrittenTestFlg = a.HRMRFR_WrittenTestFlg,
                                      HRMRFR_OnlineTestFlg = a.HRMRFR_OnlineTestFlg,
                                      HRMRFR_TechnicalInterviewFlg = a.HRMRFR_TechnicalInterviewFlg,
                                      HRMRFR_Attachment = a.HRMRFR_Attachment,
                                      HRMRFR_JobLocation = a.HRMRFR_JobLocation
                                  }).ToArray();



                //using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "HR_MRFrequisitiondetails";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@HRMRFR_Id", SqlDbType.BigInt) { Value = id });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        dto.VMSEditValue = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_MRFRequisitionDTO deactivate(HR_MRFRequisitionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMRFR_Id > 0)
                {
                    var result = _VMSContext.HR_MRFRequisitionDMO.Single(t => t.HRMRFR_Id == dto.HRMRFR_Id);

                    if (result.HRMRFR_ActiveFlag == true)
                    {
                        result.HRMRFR_ActiveFlag = false;
                    }
                    else if (result.HRMRFR_ActiveFlag == false)
                    {
                        result.HRMRFR_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMRFR_ActiveFlag == true)
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

        public HR_MRFRequisitionDTO GetAllDropdownAndDatatableDetails(HR_MRFRequisitionDTO dto)
        {
            List<HR_MRFRequisitionDMO> datalist = new List<HR_MRFRequisitionDMO>();
            try
            {
                //datalist = _VMSContext.HR_MRFRequisitionDMO.ToList();
                //dto.VMSMRFList = datalist.ToArray();

                dto.MasterPositionList = (from emp in _VMSContext.HR_Master_PositionDMO
                                          where emp.HRMP_ActiveFlg == true && emp.MI_Id == dto.MI_Id
                                          select new HR_MRFRequisitionDTO
                                          {
                                              HRMP_Id = emp.HRMP_Id,
                                              HRMP_Position = emp.HRMP_Position,
                                              HRMP_Skills = emp.HRMP_Skills,
                                              HRMP_Desc = emp.HRMP_Desc
                                          }).ToArray();

                dto.MasterLocation = (from emp in _VMSContext.HR_Master_LocationDMO
                                      where (emp.HRMLO_ActiveFlg == true && emp.MI_Id == dto.MI_Id)
                                      select new HR_MRFRequisitionDTO
                                      {
                                          HRMLO_Id = emp.HRMLO_Id,
                                          HRMLO_LocationName = emp.HRMLO_LocationName
                                      }).ToArray();

                dto.MasterDepartmentList = (from emp in _VMSContext.HR_Master_Department
                                            where (emp.HRMD_ActiveFlag == true && emp.MI_Id == dto.MI_Id)
                                            select new HR_MRFRequisitionDTO
                                            {
                                                HRMD_Id = emp.HRMD_Id,
                                                HRMD_DepartmentName = emp.HRMD_DepartmentName
                                            }).ToArray();

                dto.MasterPriorityList = (from emp in _VMSContext.HR_Master_PriorityDMO
                                          where emp.HRMP_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                          select new HR_MRFRequisitionDTO
                                          {
                                              HRMPR_Id = emp.HRMPR_Id,
                                              HRMP_Name = emp.HRMP_Name
                                          }).ToArray();

                dto.MasterPosTypeList = (from emp in _VMSContext.HR_Master_PostionTypeDMO
                                         where emp.HRMPT_ActiveFlg == true && emp.MI_Id == dto.MI_Id
                                         select new HR_MRFRequisitionDTO
                                         {
                                             HRMPT_Id = emp.HRMPT_Id,
                                             HRMPT_Name = emp.HRMPT_Name
                                         }).ToArray();

                dto.MasterQualification = (from emp in _VMSContext.HR_Master_Course
                                           where emp.HRMC_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                           select new HR_MRFRequisitionDTO
                                           {
                                               HRMC_Id = emp.HRMC_Id,
                                               HRMC_QulaificationName = emp.HRMC_QulaificationName
                                           }).ToArray();

                dto.MasterGender = (from emp in _VMSContext.IVRM_Master_Gender
                                    where emp.IVRMMG_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                    select new HR_MRFRequisitionDTO
                                    {
                                        IVRMMG_Id = emp.IVRMMG_Id,
                                        IVRMMG_GenderName = emp.IVRMMG_GenderName
                                    }).ToArray();

                dto.clientlist = (from emp in _VMSContext.ISM_Master_Client_DMO
                                  where emp.ISMMCLT_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                  select new HR_MRFRequisitionDTO
                                  {
                                      ISMMCLT_Id = emp.ISMMCLT_Id,
                                      ISMMCLT_ClientName = emp.ISMMCLT_ClientName
                                  }).ToArray();


                dto.countrylist = _VMSContext.IVRM_Master_Country.ToArray();
                dto.consulancygriddata = _VMSContext.Mastert_ConsultancyDMO.Where(t=>t.HRMCON_ActiveFlg==true).ToArray();


                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_MRFrequisitionGridDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = dto.HRMRFR_UpdatedBy });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.gridrecordMRF = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_MRFRequisitionDTO staterecord(HR_MRFRequisitionDTO dto)
        {
            try
            {
                dto.statelist = _VMSContext.IVRM_Master_State.Where(t => t.IVRMMC_Id == dto.IVRMMC_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_MRFRequisitionDTO deleterecord(HR_MRFRequisitionDTO dto)
        {
            try
            {
                try
                {
                    if (dto.HRMRFR_Id > 0)
                    {
                        var result = _VMSContext.HR_MRFRequisitionDMO.Single(t => t.HRMRFR_Id == dto.HRMRFR_Id);

                        if (result.HRMRFR_ActiveFlag == true)
                        {
                            result.HRMRFR_ActiveFlag = false;
                        }
                        else if (result.HRMRFR_ActiveFlag == false)
                        {
                            result.HRMRFR_ActiveFlag = true;
                        }
                        result.UpdatedDate = DateTime.Now;

                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {

                            dto.retrunMsg = "Deleted";
                        }
                        else
                        {
                            dto.retrunMsg = "Failed";
                        }

                        dto = GetAllDropdownAndDatatableDetails(dto);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                    dto.retrunMsg = "Error occured";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_MRFRequisitionDTO JobDisplay(HR_MRFRequisitionDTO dto)
        {
            try
            {

                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HRF_PostingForWeb";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.webjobposting = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HR_MRFRequisitionDTO saverecord(HR_MRFRequisitionDTO dto)
        {
            try
            {
                if (dto.HRMCON_Id != 0)
                {
                    var res = _VMSContext.Mastert_ConsultancyDMO.Where(t => t.HRMCON_ConsultancyName == dto.HRMCON_ConsultancyName && t.HRMCON_Id != dto.HRMCON_Id).ToList();
                    if (res.Count > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                    }
                    else
                    {
                        var result = _VMSContext.Mastert_ConsultancyDMO.Single(t => t.HRMCON_Id == dto.HRMCON_Id);
                        result.HRMCON_ConsultancyName = dto.HRMCON_ConsultancyName;
                        result.HRMCON_Address = dto.HRMCON_Address;
                        result.HRMCON_City = dto.HRMCON_City;
                        result.HRMCON_State = dto.HRMCON_State;
                        result.HRMCON_ContactNo = dto.HRMCON_ContactNo;
                        result.HRMCON_EmailId = dto.HRMCON_EmailId;
                        result.HRMCON_PINCode = dto.HRMCON_PINCode;
                        result.HRMCON_ContactPersonName = dto.HRMCON_ContactPersonName;
                        //result.HRMCON_CreatedBy = dto.HRMCON_CreatedBy;
                        //result.HRMCON_UpdatedBy = dto.HRMCON_UpdatedBy;
                        result.HRMCON_ActiveFlg = true;
                        result.HRMCON_UpdatedDate = DateTime.Now;
                        result.HRMCON_CreatedDate = DateTime.Now;
                        _VMSContext.Update(result);

                        var contactExists = _VMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            dto.returnval = true;
                        }
                        else
                        {
                            dto.returnval = false;
                        }
                    }
                }

                else
                {

                    var contactExistsP = _VMSContext.Database.ExecuteSqlCommand("SaveValue_Master_Consultancy @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", dto.HRMCON_ConsultancyName, dto.HRMCON_Address, dto.HRMCON_City, dto.HRMCON_State, dto.HRMCON_ContactNo, dto.HRMCON_EmailId, dto.HRMCON_PINCode, dto.HRMCON_ContactPersonName, dto.userid);
                    if (contactExistsP > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "notUpdated";
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_MRFRequisitionDTO activedeactive(HR_MRFRequisitionDTO dto)
        {
            try
            {
                var result = _VMSContext.Mastert_ConsultancyDMO.Single(t => t.HRMCON_Id == dto.HRMCON_Id);

                if (result.HRMCON_ActiveFlg == true)
                {
                    result.HRMCON_ActiveFlg = false;
                }
                else if (result.HRMCON_ActiveFlg == false)
                {
                    result.HRMCON_ActiveFlg = true;
                }
                result.HRMCON_UpdatedDate = DateTime.Now;
                _VMSContext.Update(result);
                int returnval = _VMSContext.SaveChanges();
                if (returnval > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


    }
}
