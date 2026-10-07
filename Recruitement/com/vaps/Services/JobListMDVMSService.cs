using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class JobListMDVMSService : Interfaces.JobListMDVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public JobListMDVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public HR_MRFRequisitionDTO getBasicData(HR_MRFRequisitionDTO dto)
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

        public HR_MRFRequisitionDTO SaveUpdate(HR_MRFRequisitionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //HR_MRFRequisitionDMO dmoObj = Mapper.Map<HR_MRFRequisitionDMO>(dto);

                var result = _VMSContext.HR_MRFRequisitionDMO.Single(t => t.HRMRFR_Id == dto.HRMRFR_Id);
                //dto.HRMRFR_ActiveFlag =true;
                //Mapper.Map(dto, result);

                result.HRMRFR_UpdatedBy = dto.HRMRFR_UpdatedBy;
                result.UpdatedDate = DateTime.Now;
                result.HRMRFR_MDFlag = dto.HRMRFR_MDFlag;
                result.HRMRFR_PayScaleFrom = dto.HRMRFR_PayScaleFrom;
                result.HRMRFR_PayScaleTo = dto.HRMRFR_PayScaleTo;
                result.HRMRFR_MDComment = dto.HRMRFR_MDComment;
                result.HRMRFR_Status = dto.HRMRFR_Status;
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
                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_MRFRequisitionDTO getRecordById(int id)
        {
            HR_MRFRequisitionDTO dto = new HR_MRFRequisitionDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_MRFRequisitionDMO> lorg = new List<HR_MRFRequisitionDMO>();

                //dto.VMSEditValue = (from a in _VMSContext.HR_MRFRequisitionDMO
                //                    from b in _VMSContext.HR_Master_PositionDMO
                //                    from c in _VMSContext.HR_Master_Department
                //                    from d in _VMSContext.HR_Master_PriorityDMO
                //                    from e in _VMSContext.HR_Master_PostionTypeDMO
                //                    from i in _VMSContext.ApplicationUserDMO
                //                    where (a.HRMRFR_Id.Equals(id) && a.HRMP_Id == b.HRMP_Id && a.MI_Id == b.MI_Id && a.HRMD_Id == c.HRMD_Id && a.MI_Id == c.MI_Id && a.HRMPR_Id == d.HRMPR_Id && a.MI_Id == d.MI_Id && a.HRMPT_Id == e.HRMPT_Id && a.MI_Id == e.MI_Id  && a.HRMRFR_CreatedBy == i.Id)
                //                    select new HR_MRFRequisitionDTO
                //                    {
                //                        HRMRFR_Id = a.HRMRFR_Id,
                //                        HRMP_Id = a.HRMP_Id,
                //                        HRMP_Position = b.HRMP_Position,
                //                        HRMD_Id = a.HRMD_Id,
                //                        HRMD_DepartmentName = c.HRMD_DepartmentName,
                //                        HRMPR_Id = a.HRMPR_Id,
                //                        HRMP_Name = d.HRMP_Name,
                //                        HRMPT_Id = a.HRMPT_Id,
                //                        HRMPT_Name = e.HRMPT_Name,
                //                        HRMC_Id = a.HRMC_Id,
                //                        HRMRFR_Qualification = a.HRMRFR_Qualification,
                //                        HRMRFR_NoofPosition = a.HRMRFR_NoofPosition,
                //                        HRMRFR_Skills = a.HRMRFR_Skills,
                //                        HRMRFR_JobDesc = a.HRMRFR_JobDesc,
                //                        HRMRFR_ExpFrom = a.HRMRFR_ExpFrom,
                //                        HRMRFR_ExpTo = a.HRMRFR_ExpTo,
                //                        HRMRFR_Age = a.HRMRFR_Age,
                //                        HRMRFR_MRFNO = a.HRMRFR_MRFNO,
                //                        IVRMMG_Id = a.IVRMMG_Id,
                //                        HRMRFR_Gender = a.HRMRFR_Gender,
                //                        HRMRFR_Reason = a.HRMRFR_Reason,
                //                        HRMRFR_Remark = a.HRMRFR_Remark,
                //                        HRMRFR_WrittenTestFlg = a.HRMRFR_WrittenTestFlg,
                //                        HRMRFR_OnlineTestFlg = a.HRMRFR_OnlineTestFlg,
                //                        HRMRFR_TechnicalInterviewFlg = a.HRMRFR_TechnicalInterviewFlg,
                //                        HRMRFR_Attachment = a.HRMRFR_Attachment,
                //                        HRMRFR_CreatedBy = a.HRMRFR_CreatedBy,
                //                        UserName = i.UserName,
                //                        CreatedDate = a.CreatedDate,
                //                        HRMRFR_PayScaleFrom = a.HRMRFR_PayScaleFrom,
                //                        HRMRFR_PayScaleTo = a.HRMRFR_PayScaleTo,
                //                        HRMRFR_PositionFilled = a.HRMRFR_PositionFilled,
                //                        HRMRFR_HRComment = a.HRMRFR_HRComment,
                //                        HRMRFR_JobLocation = a.HRMRFR_JobLocation,
                //                        HRMRFR_MDComment = a.HRMRFR_MDComment
                //                    }).ToArray();



                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_VMSEditValue";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRMRFR_Id", SqlDbType.BigInt) { Value = id });

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
                        dto.VMSEditValue = retObject.ToArray();
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
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_MRFRequisitionDTO GetAllDropdownAndDatatableDetails(HR_MRFRequisitionDTO dto)
        {
            List<HR_MRFRequisitionDMO> datalist = new List<HR_MRFRequisitionDMO>();
            try
            {
                //var mrfdatalist = from a in _VMSContext.HR_MRFRequisitionDMO
                //                  from b in _VMSContext.HR_Master_PositionDMO
                //                  from d in _VMSContext.HR_Master_PriorityDMO
                //                  from e in _VMSContext.ApplicationUserDMO
                //                  from f in _VMSContext.HR_Master_Department
                //                  from g in _VMSContext.ApplicationUserDMO
                //                  where (a.HRMP_Id == b.HRMP_Id && a.HRMPR_Id == d.HRMPR_Id && a.HRMRFR_ManagerFlag == true && a.HRMRFR_CreatedBy == e.Id && a.HRMD_Id == f.HRMD_Id && a.HRMRFR_Status == "Pending" && a.HRMRFR_CreatedBy == g.Id && a.HRMRFR_CreatedBy == g.Id)
                //                  select new HR_MRFRequisitionDTO
                //                  {
                //                      UserName = g.UserName,
                //                      HRMRFR_Id = a.HRMRFR_Id,
                //                      HRMP_Id = a.HRMP_Id,
                //                      HRMP_Position = b.HRMP_Position,
                //                      HRMPR_Id = a.HRMPR_Id,
                //                      HRMP_Name = d.HRMP_Name,
                //                      HRMRFR_MRFNO = a.HRMRFR_MRFNO,
                //                      HRMRFR_NoofPosition = a.HRMRFR_NoofPosition,
                //                      HRMRFR_Status = a.HRMRFR_Status,
                //                      HRMRFR_JobLocation = a.HRMRFR_JobLocation,
                //                      HRMD_DepartmentName = f.HRMD_DepartmentName,
                //                      CreatedDate = a.CreatedDate
                //                  };
                //dto.VMSMRFList = mrfdatalist.ToArray();



                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_mrfdatalist";
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
                        dto.VMSMRFList = retObject.ToArray();
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

    }
}
