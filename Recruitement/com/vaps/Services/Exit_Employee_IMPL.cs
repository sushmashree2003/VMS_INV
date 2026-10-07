using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.Exit;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using Recruitment.com.vaps.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Exit_Employee_IMPL : Interfaces.Exit_Employee_Interface
    {
        public HRMSContext _HRMSContext;
        public VMSContext _vmsconte;

        public Exit_Employee_IMPL(VMSContext vm, HRMSContext HRMSContext)
        {
            _vmsconte = vm;
            _HRMSContext = HRMSContext;
        }

        public ISM_Resignation_DTO get_depts(ISM_Resignation_DTO data)
        {
            try
            {
                data.department_list_R = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                          from b in _HRMSContext.HR_Master_Department
                                          where (a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && b.HRMD_ActiveFlag == true)
                                          select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_Resignation_DTO get_desig(ISM_Resignation_DTO data)
        {
            try
            {

                data.designation_list_R = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                           from b in _HRMSContext.HR_Master_Designation
                                           where (a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id && b.HRMDES_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Resignation_Master_Reasons_DTO Get_Reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                dto.get_reason_list = (from a in _vmsconte.ISM_Resignation_Master_Reasons_DMO_con
                                       from b in _vmsconte.Institution
                                       from c in _vmsconte.HR_Master_Designation_con
                                       where a.MI_Id == b.MI_Id
                                       select new ISM_Resignation_Master_Reasons_DTO
                                       {
                                           MI_Id = a.MI_Id,
                                           ISMRESGMRE_Id = a.ISMRESGMRE_Id,
                                           ISMRESGMRE_ResignationReasons = a.ISMRESGMRE_ResignationReasons,
                                           ISMRESGMRE_ActiveFlag = a.ISMRESGMRE_ActiveFlag,
                                           ISMRESGMRE_NoticePeriodDays = a.ISMRESGMRE_NoticePeriodDays,
                                           ISMRESGMRE_Template = a.ISMRESGMRE_Template,
                                           ISMRESGMRE_Flg = a.ISMRESGMRE_Flg,
                                           MI_Name = b.MI_Name,
                                       }).Distinct().OrderByDescending(p => p.CreatedDate).ToArray();

                // dto.get_reason_list = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.MI_Id == dto.MI_Id).ToList().ToArray();

                dto.company_list_dd = _vmsconte.Institution.Where(a => a.MI_ActiveFlag == 1).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_Reasons_DTO Save_Edit_Reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                if (dto.ActivitynameArray != null && dto.ActivitynameArray.Length > 0)
                {
                    dto.savecount = 0;
                    dto.duplicatecount = 0;

                    foreach (var fl in dto.ActivitynameArray)
                    {
                        if (fl.ISMRESGMRE_Id > 0)
                        {
                            var dublicate = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.ISMRESGMRE_ResignationReasons == dto.ISMRESGMRE_ResignationReasons && a.ISMRESGMRE_Flg == dto.ISMRESGMRE_Flg && a.ISMRESGMRE_Id != fl.ISMRESGMRE_Id && a.MI_Id == fl.MI_Id).Count();
                            if (dublicate > 0)
                            {
                                // dto.returnval = "Exist";
                                dto.duplicatecount = dto.duplicatecount + 1;
                            }
                            else
                            {
                                var result = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Single(a => a.ISMRESGMRE_Id == fl.ISMRESGMRE_Id);
                                result.ISMRESGMRE_ResignationReasons = dto.ISMRESGMRE_ResignationReasons;
                                result.MI_Id = fl.MI_Id;
                                result.ISMRESGMRE_Flg = dto.ISMRESGMRE_Flg;
                                result.ISMRESGMRE_NoticePeriodDays = dto.ISMRESGMRE_NoticePeriodDays;
                                result.ISMRESGMRE_Template = dto.ISMRESGMRE_Template;
                                result.UpdatedDate = DateTime.Now;
                                result.ISMRESGMRE_UpdatedBy = dto.userId;
                                _vmsconte.Update(result);
                            }


                        }
                        else
                        {
                            var dublicate = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.ISMRESGMRE_ResignationReasons == dto.ISMRESGMRE_ResignationReasons && a.ISMRESGMRE_Flg == dto.ISMRESGMRE_Flg && a.MI_Id == fl.MI_Id).Count();
                            if (dublicate > 0)
                            {
                                //  dto.returnval = "Exist";
                                dto.duplicatecount = dto.duplicatecount + 1;
                            }
                            else
                            {
                                ISM_Resignation_Master_Reasons_DMO rmr = new ISM_Resignation_Master_Reasons_DMO();
                                rmr.ISMRESGMRE_ResignationReasons = dto.ISMRESGMRE_ResignationReasons;
                                rmr.MI_Id = fl.MI_Id;
                                rmr.ISMRESGMRE_NoticePeriodDays = dto.ISMRESGMRE_NoticePeriodDays;
                                rmr.ISMRESGMRE_ActiveFlag = true;
                                rmr.ISMRESGMRE_Flg = dto.ISMRESGMRE_Flg;
                                rmr.ISMRESGMRE_Template = dto.ISMRESGMRE_Template;
                                rmr.CreatedDate = DateTime.Now;
                                rmr.UpdatedDate = DateTime.Now;
                                rmr.ISMRESGMRE_CreatedBy = dto.userId;
                                rmr.ISMRESGMRE_UpdatedBy = dto.userId;
                                _vmsconte.Add(rmr);

                            }

                        }

                    }
                    int R = _vmsconte.SaveChanges();
                    if (R > 0)
                    {
                        dto.returnval = "Add";
                        dto.savecount = R;
                    }
                    else
                    {
                        dto.returnval = "Notadd";
                    }


                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returnval = "admin";
            }
            return dto;
        }
        public ISM_Resignation_Master_Reasons_DTO active_deactive_reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                if (dto.ISMRESGMRE_ActiveFlag == true)
                {
                    var result = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Single(a => a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id);
                    result.ISMRESGMRE_ActiveFlag = false;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMRE_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "false";
                }
                else
                {
                    var result = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Single(a => a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id);
                    result.ISMRESGMRE_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMRE_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "true";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_Reasons_DTO get_details_reason(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                dto.get_details_reason_list = (from a in _vmsconte.ISM_Resignation_Master_Reasons_DMO_con
                                               where a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id
                                               select new ISM_Resignation_Master_Reasons_DTO
                                               {
                                                   ISMRESGMRE_ResignationReasons = a.ISMRESGMRE_ResignationReasons,
                                                   ISMRESGMRE_Id = a.ISMRESGMRE_Id,
                                                   ISMRESGMRE_Flg = a.ISMRESGMRE_Flg,
                                                   ISMRESGMRE_Template = a.ISMRESGMRE_Template,
                                                   ISMRESGMRE_NoticePeriodDays = a.ISMRESGMRE_NoticePeriodDays,
                                                   MI_Id = a.MI_Id,
                                               }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        //==========================================End=============================================
        public ISM_Resignation_Master_CheckLists_DTO get_ApproverDepartDesign_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chicklist_Approver_list";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMRESGMCL_Id ", SqlDbType.VarChar)
                    {
                        Value = dto.ISMRESGMCL_Id
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.get_details_Approver_list = retObject.ToArray();
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
        public ISM_Resignation_Master_CheckLists_DTO Get_Checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                //dto.Approver_lisd_dd = _vmsconte.ApplUser.Where(a => a.RoleTypeFlag == "Staff").ToList().ToArray();
                dto.company_list_dd = _vmsconte.Institution.Where(a => a.MI_ActiveFlag == 1).ToList().ToArray();



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chicklist_list";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.check_list = retObject.ToArray();
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
        public ISM_Resignation_Master_CheckLists_DTO company(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                dto.get_department_lisd_dd = _vmsconte.HR_Master_Department.Where(a => a.MI_Id == dto.MI_Id && a.HRMD_ActiveFlag == true).ToList().ToArray();
                dto.designation_list_dd = _vmsconte.HR_Master_Designation_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMDES_ActiveFlag == true).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_CheckLists_DTO get_company(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                dto.get_department_lisd_ddd = _vmsconte.HR_Master_Department.Where(a => a.MI_Id == dto.MI_Id && a.HRMD_ActiveFlag == true).ToList().ToArray();

                dto.designation_list_ddd = _vmsconte.HR_Master_Designation_con.Where(a => a.MI_Id == dto.MI_Id && a.HRMDES_ActiveFlag == true).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_CheckLists_DTO Save_Edit_Checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                if (dto.ISMRESGMCL_Id > 0)
                {
                    var result = _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con.Single(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id);
                    result.ISMRESGMCL_CheckListName = dto.ISMRESGMCL_CheckListName;
                    result.MI_Id = dto.MI_Id;
                    result.HRMD_Id = dto.HRMD_Id;
                    result.HRMDES_Id = dto.HRMDES_Id;
                    result.ISMRESGMCL_Flg = dto.ISMRESGMCL_Flg;
                    result.ISMRESGMCL_Template = dto.ISMRESGMCL_Template;
                    result.ISMRESGMCL_Remarks = dto.ISMRESGMCL_Remarks;
                    result.ISMRESGMCL_OptionalFlg = dto.ISMRESGMCL_OptionalFlg;
                    result.ISMRESGMCL_TemplateFileName = dto.ISMRESGMCL_TemplateFileName;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMCL_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "Update";


                    if (dto.ApprovalChicklist != null && dto.ApprovalChicklist.Length > 0)
                    {

                        var result2 = _vmsconte.ISM_Resignation_Master_CheckLists_AppAuthority_DMO.Where(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id).ToList(); ;


                        if (result2.Count > 0)
                        {
                            foreach (var i in result2)
                            {

                                _vmsconte.Remove(i);
                            }
                            int returnval1 = _vmsconte.SaveChanges();
                        }
                        foreach (var fl in dto.ApprovalChicklist)
                        {
                            if (dto.ISMRESGMCLAPAUT_Id > 0)
                            {
                                var result1 = _vmsconte.ISM_Resignation_Master_CheckLists_AppAuthority_DMO.Single(a => a.ISMRESGMCLAPAUT_Id == dto.ISMRESGMCLAPAUT_Id);

                                result1.ISMRESGMCL_Id = dto.ISMRESGMCL_Id;
                                result1.MI_Id = fl.MI_Id;
                                result1.HRMD_Id = fl.HRMD_Id;
                                result1.HRMDES_Id = fl.HRMDES_Id;
                                result1.ISMRESGMCLAPAUT_UpdatedDate = DateTime.Now;
                                result1.ISMRESGMCLAPAUT_UpdatedBy = dto.userId;
                                _vmsconte.Update(result);
                                dto.returnval = "Update";
                            }
                            else
                            {
                                var dublicates = _vmsconte.ISM_Resignation_Master_CheckLists_AppAuthority_DMO.Where(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id).Count();
                                if (dublicates > 0)
                                {
                                    dto.returnval = "Exist";
                                }
                                else
                                {
                                    ISM_Resignation_Master_CheckLists_AppAuthority_DMO approve = new ISM_Resignation_Master_CheckLists_AppAuthority_DMO();
                                    approve.ISMRESGMCL_Id = dto.ISMRESGMCL_Id;
                                    approve.MI_Id = fl.MI_Id;
                                    approve.HRMD_Id = fl.HRMD_Id;
                                    approve.HRMDES_Id = fl.HRMDES_Id;
                                    approve.ISMRESGMCLAPAUT_CreatedDate = DateTime.Now;
                                    approve.ISMRESGMCLAPAUT_CreatedBy = dto.userId;
                                    _vmsconte.Add(approve);

                                }
                            }

                        }
                        _vmsconte.SaveChanges();

                        //dto.returnval = "Add";

                    }

                }
                else
                {
                    var dublicate = _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con.Where(a => a.ISMRESGMCL_CheckListName == dto.ISMRESGMCL_CheckListName && a.ISMRESGMCL_Flg == dto.ISMRESGMCL_Flg && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id).Count();
                    if (dublicate > 0)
                    {
                        dto.returnval = "Exist";
                    }
                    else
                    {
                        ISM_Resignation_Master_CheckLists_DMO rmr = new ISM_Resignation_Master_CheckLists_DMO();
                        rmr.ISMRESGMCL_CheckListName = dto.ISMRESGMCL_CheckListName;
                        rmr.HRMD_Id = dto.HRMD_Id;
                        rmr.HRMDES_Id = dto.HRMDES_Id;
                        rmr.ISMRESGMCL_Flg = dto.ISMRESGMCL_Flg;
                        rmr.ISMRESGMCL_Template = dto.ISMRESGMCL_Template;
                        rmr.ISMRESGMCL_ActiveFlag = true;
                        rmr.MI_Id = dto.MI_Id;
                        rmr.ISMRESGMCL_Remarks = dto.ISMRESGMCL_Remarks;
                        rmr.ISMRESGMCL_OptionalFlg = dto.ISMRESGMCL_OptionalFlg;
                        rmr.ISMRESGMCL_TemplateFileName = dto.ISMRESGMCL_TemplateFileName;
                        rmr.CreatedDate = DateTime.Now;
                        rmr.UpdatedDate = DateTime.Now;
                        rmr.ISMRESGMCL_CreatedBy = dto.userId;
                        rmr.ISMRESGMCL_UpdatedBy = dto.userId;
                        _vmsconte.Add(rmr);

                        if (dto.ApprovalChicklist != null && dto.ApprovalChicklist.Length > 0)
                        {
                            foreach (var fl in dto.ApprovalChicklist)
                            {
                                if (dto.ISMRESGMCLAPAUT_Id > 0)
                                {
                                    var result = _vmsconte.ISM_Resignation_Master_CheckLists_AppAuthority_DMO.Single(a => a.ISMRESGMCLAPAUT_Id == dto.ISMRESGMCLAPAUT_Id);

                                    result.ISMRESGMCL_Id = rmr.ISMRESGMCL_Id;
                                    result.MI_Id = fl.MI_Id;
                                    result.HRMD_Id = fl.HRMD_Id;
                                    result.HRMDES_Id = fl.HRMDES_Id;
                                    result.ISMRESGMCLAPAUT_UpdatedDate = DateTime.Now;
                                    result.ISMRESGMCLAPAUT_UpdatedBy = dto.userId;
                                    _vmsconte.Update(result);
                                    dto.returnval = "Update";
                                }
                                else
                                {
                                    var dublicates = _vmsconte.ISM_Resignation_Master_CheckLists_AppAuthority_DMO.Where(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id).Count();
                                    if (dublicates > 0)
                                    {
                                        dto.returnval = "Exist";
                                    }
                                    else
                                    {
                                        ISM_Resignation_Master_CheckLists_AppAuthority_DMO approve = new ISM_Resignation_Master_CheckLists_AppAuthority_DMO();
                                        approve.ISMRESGMCL_Id = rmr.ISMRESGMCL_Id;
                                        approve.MI_Id = fl.MI_Id;
                                        approve.HRMD_Id = fl.HRMD_Id;
                                        approve.HRMDES_Id = fl.HRMDES_Id;
                                        approve.ISMRESGMCLAPAUT_CreatedDate = DateTime.Now;
                                        approve.ISMRESGMCLAPAUT_CreatedBy = dto.userId;
                                        _vmsconte.Add(approve);

                                    }
                                }

                            }

                        }
                        int i = _vmsconte.SaveChanges();

                        dto.returnval = "Add";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_CheckLists_DTO active_deactive_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                if (dto.ISMRESGMCL_ActiveFlag == true)
                {
                    var result = _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con.Single(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id);
                    result.ISMRESGMCL_ActiveFlag = false;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMCL_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "false";
                }
                else
                {
                    var result = _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con.Single(a => a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id);
                    result.ISMRESGMCL_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMRESGMCL_UpdatedBy = dto.userId;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "true";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_Master_CheckLists_DTO get_details_checklist(ISM_Resignation_Master_CheckLists_DTO dto)
        {
            try
            {
                dto.get_details_check_list = (from a in _vmsconte.ISM_Resignation_Master_CheckLists_DMO_con
                                              from b in _vmsconte.HR_Master_Department
                                              from c in _vmsconte.HR_Master_Designation_con
                                              from d in _vmsconte.Institution
                                              where a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.MI_Id == d.MI_Id && a.ISMRESGMCL_Id == dto.ISMRESGMCL_Id
                                              select new ISM_Resignation_Master_CheckLists_DTO
                                              {
                                                  ISMRESGMCL_Id = a.ISMRESGMCL_Id,
                                                  ISMRESGMCL_CheckListName = a.ISMRESGMCL_CheckListName,
                                                  HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                  HRMD_Id = a.HRMD_Id,
                                                  HRMDES_DesignationName = c.HRMDES_DesignationName,
                                                  HRMDES_Id = a.HRMDES_Id,
                                                  ISMRESGMCL_Flg = a.ISMRESGMCL_Flg,
                                                  ISMRESGMCL_Remarks = a.ISMRESGMCL_Remarks,
                                                  ISMRESGMCL_Template = a.ISMRESGMCL_Template,
                                                  ISMRESGMCL_OptionalFlg = a.ISMRESGMCL_OptionalFlg,
                                                  ISMRESGMCL_TemplateFileName = a.ISMRESGMCL_TemplateFileName,
                                                  MI_Id = d.MI_Id,
                                                  MI_Name = d.MI_Name,
                                              }).ToList().ToArray();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Resignation_CheckList_App_Athority";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMRESGMCL_Id", SqlDbType.VarChar)
                    {
                        Value = dto.ISMRESGMCL_Id
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.check_list = retObject.ToArray();
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
        //==================================Exit Employee Process Start=================================
        public ISM_Resignation_DTO Load_all_data(ISM_Resignation_DTO dto)
        {
            try
            {
                long userhrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();

                dto.DepartmentCodeName = (from a in _vmsconte.HR_Master_DepartmentCode_HeadDMO
                                          from b in _vmsconte.HR_Master_DepartmentCodeDMO
                                          from c in _vmsconte.HR_Master_Department
                                          from d in _vmsconte.HR_Master_Employee_DMO
                                          where (c.HRMD_Id==d.HRMD_Id && c.HRMDC_ID == b.HRMDC_ID && a.HRMDC_ID == b.HRMDC_ID && a.HRME_ID == userhrme_id)
                                          select b).Select(t => t.HRMDC_Code).FirstOrDefault();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_employee_list_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.userId)
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employee_list_dd = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ResignationApprovalGrid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = userhrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@dd", SqlDbType.BigInt)
                    {
                        Value = 1
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.ResignationapprovalGrid = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ResignationApprovalGrid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = userhrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@dd", SqlDbType.BigInt)
                    {
                        Value = 0
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.releavingapprovalGrid = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                dto.reason_list_dd = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMRESGMRE_ActiveFlag == true && a.ISMRESGMRE_Flg == "Relieving").ToArray();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Resignation_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                    {
                        Value = dto.userId
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.exit_employee_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }





                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_Releiving_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = dto.userId
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.relieving_emp_dd = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_Resignation_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = dto.userId
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.resignation_emp_dd = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "doc_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@qq", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_id", SqlDbType.BigInt)
                    {
                        Value = dto.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = "Relieving"
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.relieving_emp_dd1 = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "relieving_check_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.relieving_check_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Relieving_LetterDetails";
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.exit_print_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                //dto.exit_print_list = (from c in _vmsconte.ISM_Resignation_DMO_con
                //                       from a in _vmsconte.Hr_Master_Employee_con
                //                       from b in _vmsconte.ISM_Resignation_RelievingLetter_DMO_con
                //                       where c.HRME_Id == a.HRME_Id && c.ISMRESG_Id == b.ISMRESG_Id
                //                       select new ISM_Resignation_DTO
                //                       {
                //                           employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                //                           + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                //                           + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                //                           HRME_Id = a.HRME_Id,
                //                           ISMRESG_Id = c.ISMRESG_Id,
                //                         //  ISMRESGRL_Id=b.ISMRESGRL_Id
                //                           ISMRESGRL_RLDate = b.ISMRESGRL_RLDate,
                //                           ISMRESGRL_ActiveFlag = b.ISMRESGRL_ActiveFlag,
                //                           ISMRESG_ResignationDate = c.ISMRESG_ResignationDate,
                //                           ISMRESG_Remarks = c.ISMRESG_Remarks,
                //                           ISMRESG_ActiveFlag = c.ISMRESG_ActiveFlag
                //                       }).Distinct().ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public ISM_Resignation_DTO GetAllRelData(ISM_Resignation_DTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EMPResignation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt)
                    {
                        Value = dto.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMRESG_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMRESG_Id
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.getrelievingchecklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Checklistapproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMRESG_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.ISMRESG_Id)
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.doc_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




                var empdetails = (from a in _vmsconte.ISM_Resignation_DMO_con
                                  from b in _vmsconte.Staff_User_Login
                                  where (a.HRME_Id == b.Emp_Code && a.ISMRESG_Id == dto.ISMRESG_Id)
                                  select new ISM_Resignation_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      userId = b.Id,
                                      MI_Id = a.MI_Id
                                  }).ToList();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_PendingIssues";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = empdetails[0].MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = empdetails[0].userId });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = empdetails[0].HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMPR_Id", SqlDbType.BigInt) { Value = 0 });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.prioritylist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Rating_Employeeratingcalc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                  SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(empdetails[0].HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ovrallflag",
                       SqlDbType.BigInt)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@yearid",
                SqlDbType.VarChar)
                    {
                        Value = 0
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.ratingarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EmployeeSalaryPendingDetials";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(empdetails[0].MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(empdetails[0].HRME_Id)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.pendingsalarydetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.earningdetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true && t.HRMED_EarnDedFlag == "Earning").OrderBy(t => t.HRMED_Order).ToArray();
                dto.dectuiondetails = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_ActiveFlag == true && t.HRMED_EarnDedFlag == "Deduction").OrderBy(t => t.HRMED_Order).ToArray();



                dto.ResignationapprovalLevel = (from a in _vmsconte.HR_PROCESSDMO
                                                from b in _vmsconte.HR_Process_Auth_OrderNoDMO
                                                where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == dto.userId && a.HRPA_TypeFlag == "ExitResignation")
                                                select b).Select(t => t.HRPAON_SanctionLevelNo).FirstOrDefault();

                dto.RelivingapprovalLevel = (from a in _vmsconte.HR_PROCESSDMO
                                             from b in _vmsconte.HR_Process_Auth_OrderNoDMO
                                             where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == dto.userId && a.HRPA_TypeFlag == "ExitRelieving")
                                             select b).Select(t => t.HRPAON_SanctionLevelNo).FirstOrDefault();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO GetAllRelData1(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.getrelievingchecklist1 = (from c in _vmsconte.ISM_Resignation_DMO_con
                                              from a in _vmsconte.Hr_Master_Employee_con
                                              from b in _vmsconte.HR_Master_Department
                                              from d in _vmsconte.HR_Master_Designation_con
                                              from e in _vmsconte.Institution
                                              where a.MI_Id == dto.MI_Id && a.HRME_Id == c.HRME_Id && c.HRME_Id == dto.HRME_Id && a.HRME_Id == dto.HRME_Id && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.MI_Id == e.MI_Id
                                              select new ISM_Resignation_DTO
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  HRME_DOJ = a.HRME_DOJ,
                                                  HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                  HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                  ISMRESG_TentativeLeavingDate = Convert.ToDateTime(c.ISMRESG_TentativeLeavingDate),
                                                  ISMRESG_Id = c.ISMRESG_Id,
                                                  ISMRESG_Flg = c.ISMRESG_Flg,
                                                  MI_Name = e.MI_Name
                                              }).Distinct().ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public ISM_Resignation_DTO Exit_empl_SaveEdit(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.HRME_Id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();
                if (dto.ISMRESG_Flg == "Relieving")
                {
                    long ISMRESGMRE_Id = _vmsconte.ISM_Resignation_DMO_con.Where(a => a.HRME_Id == dto.HRME_Id && a.ISMRESG_MgmtApprRejFlg == "Approved" && a.ISMRESG_Flg == "Resignation").Select(t => t.ISMRESGMRE_Id).FirstOrDefault();

                    dto.ISMRESGMRE_Id = ISMRESGMRE_Id;
                }



                if (dto.ISMRESG_Id > 0)
                {
                    var result = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.ISMRESG_Id == dto.ISMRESG_Id);
                    result.HRME_Id = dto.HRME_Id;
                    result.ISMRESG_ResignationDate = DateTime.Now;
                    result.ISMRESGMRE_Id = dto.ISMRESGMRE_Id;
                    result.ISMRESG_NoticePeriod = dto.ISMRESG_NoticePeriod;
                    result.ISMRESG_TentativeLeavingDate = dto.ISMRESG_TentativeLeavingDate;
                    result.ISMRESG_Remarks = dto.ISMRESG_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.ISMRESG_Flg = dto.ISMRESG_Flg;
                    result.ISMRESG_MgmtApprRejFlg = "Pending";
                    result.ISMRESG_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnval = "Update";
                }
                else
                {

                    ISM_Resignation_DMO rd = new ISM_Resignation_DMO();
                    var result = _vmsconte.ISM_Resignation_DMO_con.Where(a => a.HRME_Id == dto.HRME_Id && a.ISMRESG_Flg == dto.ISMRESG_Flg && a.ISMRESG_MgmtApprRejFlg != "Rejected").ToList();
                    if (result.Count > 0)
                    {

                        if (dto.doc_list2.Length > 0)
                        {
                            dto = savechecklists(dto);
                        }
                        else
                        {
                            dto.returnval = "Exist";
                        }

                    }
                    else
                    {
                        rd.HRME_Id = dto.HRME_Id;
                        rd.ISMRESG_ResignationDate = DateTime.Now;
                        rd.ISMRESGMRE_Id = dto.ISMRESGMRE_Id;
                        rd.ISMRESG_NoticePeriod = dto.ISMRESG_NoticePeriod;
                        rd.ISMRESG_RelievingDate = DateTime.Today.AddDays(dto.ISMRESG_NoticePeriod);
                        rd.ISMRESG_Remarks = dto.ISMRESG_Remarks;
                        rd.MI_Id = dto.MI_Id;
                        rd.ISMRESG_Print_Flg = 1;
                        rd.ISMRESG_Status_Flg = 0;
                        rd.ISMRESG_MgmtApprRejFlg = "Pending";
                        rd.ISMRESG_Flg = dto.ISMRESG_Flg;
                        rd.ISMRESG_ActiveFlag = true;
                        rd.ISMRESG_CreatedBy = dto.userId;
                        rd.CreatedDate = DateTime.Now;
                        _vmsconte.Add(rd);
                        var n = _vmsconte.SaveChanges();

                        if (dto.doc_list2.Length > 0)
                        {
                            foreach (var p in dto.doc_list2)
                            {
                                var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_resignation_checklist_update_proc @p0,@p1,@p2,@p3,@p4,@p5", rd.ISMRESG_Id, p.ISMRESGMCL_Id, p.ISMRESGCL_FileName, p.document_Path, dto.MI_Id, dto.userId);
                                if (contactExistsP > 0)
                                {
                                    dto.returnval = "Add";
                                }
                                else
                                {
                                    dto.returnval = "notUpdated";
                                }
                            }
                        }
                        else
                        {
                            if (n > 0)
                            {
                                dto.returnval = "Add";
                            }
                            else
                            {
                                dto.returnval = "notUpdated";
                            }
                        }


                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO ExitApproveal(ISM_Resignation_DTO dto)
        {

            try
            {
                List<ISM_Resignation_Master_Reasons_DTO> employeedetails = new List<ISM_Resignation_Master_Reasons_DTO>();
                List<ISM_Resignation_Master_Reasons_DTO> Approvalemaildetails = new List<ISM_Resignation_Master_Reasons_DTO>();
                var AppType = "";
                if (dto.ISMRESG_Flg == "Resignation")
                {
                    AppType = "ExitResignation";
                }
                else
                {
                    AppType = "ExitRelieving";
                }


                var currentlevel = "";
                var CurrentApprovalMail = "";
                var NextLevel = "";
                var NextlevelApprovalMail = "";
                var HRMDES_DesignationName = "";
                var hremail = "";
                var ccemail = "";
                string toemailid = "";
                string employeeEmail = "";
                string FinalFlg = "";

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Reignation_SactionLevel";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                    {
                        Value = dto.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = AppType
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMRESG_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMRESG_Id
                    });


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


                                currentlevel = dataReader["currentlevel"].ToString();
                                CurrentApprovalMail = dataReader["CurrentApprovalMail"].ToString();
                                NextLevel = dataReader["NextLevel"].ToString();
                                NextlevelApprovalMail = dataReader["NextlevelApprovalMail"].ToString();
                                HRMDES_DesignationName = dataReader["HRMDES_DesignationName"].ToString();
                                hremail = dataReader["HRMail"].ToString();
                                employeeEmail = dataReader["EmployeeEmail"].ToString();
                                FinalFlg = dataReader["HRPAON_FinalFlg"].ToString();





                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




                var relievingregconditioncheck = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.ISMRESG_Id == dto.ISMRESG_Id);
                var regtable = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.ISMRESG_Id == dto.ISMRESG_Id);
                var result = 0;
                string Template = "ResignationApproval";
                long hrmeid = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();

                if (Convert.ToInt64(NextLevel) > 0)
                {
                    toemailid = NextlevelApprovalMail;

                    ccemail = hremail + "," + employeeEmail;
                }
                else
                {
                    toemailid = employeeEmail;
                    ccemail = hremail;
                }



                if (dto.ISMRESGAB_AppRejFlg == "Approved")
                {
                    ISM_Resignation_ApprovedBy_DMO rd = new ISM_Resignation_ApprovedBy_DMO();
                    rd.HRME_Id = hrmeid;
                    rd.ISMRESG_Id = dto.ISMRESG_Id;
                    rd.MI_Id = dto.MI_Id;
                    rd.ISMRESGAB_Remarks = dto.ISMRESGAB_Remarks;
                    rd.ISMRESGAB_AppRejFlg = dto.ISMRESGAB_AppRejFlg;
                    rd.ISMRESGAB_RelievingDate = dto.ISMRESGAB_RelievingDate;
                    rd.ISMRESGAB_DamageAmount = dto.damagerecord != null && dto.damagerecord.Length > 0 ? dto.damagerecord[0].dmgAmount : 0;
                    rd.ISMRESGAB_DamageRemarks = dto.damagerecord != null && dto.damagerecord.Length > 0 ? dto.damagerecord[0].dmgRemarks : "";
                    rd.ISMRESGAB_Date = DateTime.Now;
                    rd.ISMRESGAB_ActiveFlag = true;
                    rd.ISMRESGAB_CreatedBy = dto.userId;
                    rd.ISMRESGAB_UpdatedBy = dto.userId;
                    rd.ISMRESGAB_CreatedDate = DateTime.Now;
                    rd.ISMRESGAB_UpdatedDate = DateTime.Now;
                    _vmsconte.Add(rd);


                    if (relievingregconditioncheck.ISMRESG_Flg == "Resignation")
                    {
                        result = (from a in _vmsconte.HR_PROCESSDMO
                                  from b in _vmsconte.HR_Process_Auth_OrderNoDMO
                                  from c in _vmsconte.Hr_Master_Employee_con
                                  from d in _vmsconte.ISM_Resignation_DMO_con
                                  where a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRPAON_FinalFlg == true && a.HRMGT_Id == c.HRMGT_Id && a.HRMG_Id == c.HRMG_Id && c.HRME_Id == d.HRME_Id && a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == dto.userId && a.HRPA_TypeFlag == AppType
                                  select b).Count();


                        var regtable1 = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.ISMRESG_Id == dto.ISMRESG_Id && relievingregconditioncheck.ISMRESG_Flg == "Resignation");
                        if (regtable1.ISMRESG_Id > 0)
                        {
                            regtable1.ISMRESG_TentativeLeavingDate = dto.ISMRESGAB_RelievingDate;
                            regtable1.ISMRESG_RelievingDate = dto.ISMRESGAB_RelievingDate;
                            _vmsconte.Update(regtable1);
                        }
                    }
                    else
                    {

                        result = (from a in _vmsconte.HR_PROCESSDMO
                                  from b in _vmsconte.HR_Process_Auth_OrderNoDMO
                                  from c in _vmsconte.Hr_Master_Employee_con
                                  from d in _vmsconte.ISM_Resignation_DMO_con
                                  where a.HRMD_Id == c.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRPAON_FinalFlg == true && a.HRMGT_Id == c.HRMGT_Id && a.HRMG_Id == c.HRMG_Id && c.HRME_Id == d.HRME_Id && a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == dto.userId && a.HRPA_TypeFlag == AppType
                                  select b).Count();
                    }


                    if (Convert.ToBoolean(FinalFlg) == true)
                    {

                        regtable.ISMRESG_MgmtApprRejFlg = dto.ISMRESGAB_AppRejFlg;
                        regtable.ISMRESG_ManagementRemarks = dto.ISMRESGAB_Remarks;
                        regtable.ISMRESG_AccRejDate = DateTime.Now;
                        _vmsconte.Update(regtable);

                        
                        //sendEmail(dto.MI_Id, Template, dto.HRME_Id, toemailid, ccemail);

                    }
                    else
                    {
                        regtable.ISMRESG_MgmtApprRejFlg = "Partially Approved";
                        _vmsconte.Update(regtable);
                        _vmsconte.SaveChanges();

                        //sendEmail(dto.MI_Id, Template, dto.HRME_Id, toemailid, ccemail);
                    }
                    var n = _vmsconte.SaveChanges();
                    if (n > 0)
                    {

                        dto.returnval = "Approved";
                    }
                    else
                    {
                        dto.returnval = "NotAdd";
                    }
                }
                else
                {

                    ISM_Resignation_ApprovedBy_DMO rd = new ISM_Resignation_ApprovedBy_DMO();
                    rd.HRME_Id = hrmeid;
                    rd.ISMRESG_Id = dto.ISMRESG_Id;
                    rd.MI_Id = dto.MI_Id;
                    rd.ISMRESGAB_Remarks = dto.ISMRESGAB_Remarks;
                    rd.ISMRESGAB_AppRejFlg = dto.ISMRESGAB_AppRejFlg;
                    rd.ISMRESGAB_Date = DateTime.Now;
                    rd.ISMRESGAB_ActiveFlag = true;
                    rd.ISMRESGAB_DamageAmount = dto.damagerecord != null && dto.damagerecord.Length > 0 ? dto.damagerecord[0].dmgAmount : 0;
                    rd.ISMRESGAB_DamageRemarks = dto.damagerecord != null && dto.damagerecord.Length > 0 ? dto.damagerecord[0].dmgRemarks : ""; 
                    rd.ISMRESGAB_CreatedBy = dto.userId;
                    rd.ISMRESGAB_UpdatedBy = dto.userId;
                    rd.ISMRESGAB_CreatedDate = DateTime.Now;
                    rd.ISMRESGAB_UpdatedDate = DateTime.Now;
                    _vmsconte.Add(rd);
                    regtable.ISMRESG_MgmtApprRejFlg = dto.ISMRESGAB_AppRejFlg;
                    regtable.ISMRESG_ManagementRemarks = dto.ISMRESGAB_Remarks;
                    regtable.ISMRESG_AccRejDate = DateTime.Now;
                    _vmsconte.Update(regtable);

                    var n = _vmsconte.SaveChanges();
                    if (n > 0)
                    {
                        
                        //sendEmail(dto.MI_Id, Template, dto.HRME_Id, toemailid, ccemail);
                        dto.returnval = "Rejected";
                    }
                    else
                    {
                        dto.returnval = "NotAdd";
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO Exit_empl_AccRej(ISM_Resignation_DTO dto)
        {
            try
            {

                var dlt = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).ToList();
                foreach (var item in dlt)
                {
                    var res = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Single(a => a.ISMRESGCL_Id == item.ISMRESGCL_Id);
                    _vmsconte.Remove(res);
                }
                var result = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.ISMRESG_Id == dto.ISMRESG_Id);
                result.ISMRESG_MgmtApprRejFlg = dto.ISMRESG_MgmtApprRejFlg;
                result.ISMRESG_ManagementRemarks = dto.ISMRESG_ManagementRemarks;
                result.ISMRESG_AccRejDate = dto.ISMRESG_AccRejDate;
                result.UpdatedDate = DateTime.Now;
                result.ISMRESG_CreatedBy = dto.userId;
                _vmsconte.Update(result);
                _vmsconte.SaveChanges();
                dto.returnval = "Update";
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO HR_RelivingApply(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.MI_Id = _vmsconte.Hr_Master_Employee_con.Where(t => t.HRME_Id == dto.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();

                if (dto.ISMRESG_Id > 0)
                {
                    var result = _vmsconte.ISM_Resignation_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id && a.HRME_Id == dto.HRME_Id && a.MI_Id == dto.MI_Id && a.ISMRESG_Flg == dto.ISMRESG_Flg && a.ISMRESG_Remarks == dto.ISMRESG_Remarks && a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id && a.ISMRESG_MgmtApprRejFlg == "Pending").ToList();
                    if (result.Count > 0)
                    {
                        dto.returnval = "dublicate";
                    }
                    else
                    {
                        var updaterecord = _vmsconte.ISM_Resignation_DMO_con.Single(a => a.ISMRESG_Id == dto.ISMRESG_Id);
                        updaterecord.HRME_Id = dto.HRME_Id;
                        updaterecord.ISMRESGMRE_Id = dto.ISMRESGMRE_Id;
                        updaterecord.ISMRESG_NoticePeriod = 0;
                        updaterecord.ISMRESG_RelievingDate = DateTime.Today.AddDays(0);
                        updaterecord.ISMRESG_Remarks = dto.ISMRESG_Remarks;
                        updaterecord.MI_Id = dto.MI_Id;
                        updaterecord.ISMRESG_Print_Flg = 0;
                        updaterecord.ISMRESG_Status_Flg = 0;
                        updaterecord.ISMRESG_MgmtApprRejFlg = "Pending";
                        updaterecord.ISMRESG_Flg = dto.ISMRESG_Flg;
                        updaterecord.ISMRESG_ActiveFlag = true;
                        updaterecord.ISMRESG_UpdatedBy = dto.userId;
                        updaterecord.UpdatedDate = DateTime.Now;
                        _vmsconte.Update(updaterecord);
                        var n = _vmsconte.SaveChanges();


                        var removerecord = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).ToList();

                        foreach (var record in removerecord)
                        {
                            _vmsconte.Remove(record);
                        }

                        var m = _vmsconte.SaveChanges();
                        if (dto.doc_list2.Length > 0)
                        {
                            foreach (var p in dto.doc_list2)
                            {
                                var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_resignation_checklist_update_proc @p0,@p1,@p2,@p3,@p4,@p5", dto.ISMRESG_Id, p.ISMRESGMCL_Id, p.ISMRESGCL_FileName, p.document_Path, dto.MI_Id, dto.userId);
                                if (contactExistsP > 0)
                                {
                                    dto.returnval = "Add";
                                }
                                else
                                {
                                    dto.returnval = "notUpdated";
                                }
                            }
                        }
                        else
                        {
                            if (n > 0)
                            {
                                dto.returnval = "Add";
                            }
                            else
                            {
                                dto.returnval = "notUpdated";
                            }
                        }

                    }


                }
                else
                {
                    var result = _vmsconte.ISM_Resignation_DMO_con.Where(a => a.HRME_Id == dto.HRME_Id && a.MI_Id == dto.MI_Id && a.ISMRESG_Flg == dto.ISMRESG_Flg && a.ISMRESG_Remarks == dto.ISMRESG_Remarks && a.ISMRESGMRE_Id == dto.ISMRESGMRE_Id && a.ISMRESG_MgmtApprRejFlg == "Pending").ToList();
                    if (result.Count > 0)
                    {
                        dto.returnval = "dublicate";
                    }
                    else
                    {
                        ISM_Resignation_DMO rd = new ISM_Resignation_DMO();
                        rd.HRME_Id = dto.HRME_Id;
                        rd.ISMRESG_ResignationDate = DateTime.Now;
                        rd.ISMRESGMRE_Id = dto.ISMRESGMRE_Id;
                        rd.ISMRESG_NoticePeriod = 0;
                        rd.ISMRESG_RelievingDate = DateTime.Today.AddDays(0);
                        rd.ISMRESG_Remarks = dto.ISMRESG_Remarks;
                        rd.MI_Id = dto.MI_Id;
                        rd.ISMRESG_Print_Flg = 0;
                        rd.ISMRESG_Status_Flg = 0;
                        rd.ISMRESG_MgmtApprRejFlg = "Pending";
                        rd.ISMRESG_Flg = dto.ISMRESG_Flg;
                        rd.ISMRESG_ActiveFlag = true;
                        rd.ISMRESG_CreatedBy = dto.userId;
                        rd.ISMRESG_UpdatedBy = dto.userId;
                        rd.CreatedDate = DateTime.Now;
                        rd.UpdatedDate = DateTime.Now;
                        _vmsconte.Add(rd);
                        var n = _vmsconte.SaveChanges();

                        if (dto.doc_list2.Length > 0)
                        {
                            foreach (var p in dto.doc_list2)
                            {
                                var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_resignation_checklist_update_proc @p0,@p1,@p2,@p3,@p4,@p5", rd.ISMRESG_Id, p.ISMRESGMCL_Id, p.ISMRESGCL_FileName, p.document_Path, dto.MI_Id, dto.userId);
                                if (contactExistsP > 0)
                                {
                                    dto.returnval = "Add";
                                }
                                else
                                {
                                    dto.returnval = "notUpdated";
                                }
                            }
                        }
                        else
                        {
                            if (n > 0)
                            {
                                dto.returnval = "Add";
                            }
                            else
                            {
                                dto.returnval = "notUpdated";
                            }
                        }
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO HR_EmployeeDetails(ISM_Resignation_DTO data)
        {
            try
            {
                var MI_Id = _vmsconte.Hr_Master_Employee_con.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "doc_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@qq", SqlDbType.BigInt)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_id", SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = "Relieving"
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.releavingdoc_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.employeedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Empheaddetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.managmentempdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Rating_Employeeratingcalc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                  SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ovrallflag",
                       SqlDbType.BigInt)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@yearid",
                SqlDbType.VarChar)
                    {
                        Value = 0
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.ratingarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.StatusFlag = _vmsconte.ISM_Resignation_DMO_con.Where(t => t.HRME_Id == data.HRME_Id && t.ISMRESG_Flg == "NOC").Select(t => t.ISMRESG_MgmtApprRejFlg).FirstOrDefault();

                data.reason_list_dd = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMRESGMRE_ActiveFlag == true && a.ISMRESGMRE_Flg == "Resignation").ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ISM_Resignation_DTO c_approve_new(ISM_Resignation_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Resignation_RelievingLetter_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).Count();
                if (result > 0)
                {
                    dto.returnval = "Duplicate";
                }
                else
                {
                    dto.ISMRESG_Id1 = dto.ISMRESG_Id;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO Edit_Employee(ISM_Resignation_DTO data)
        {
            try
            {
                data.HRME_Id = _vmsconte.ISM_Resignation_DMO_con.Where(a => a.ISMRESG_Id == data.ISMRESG_Id).Select(t => t.HRME_Id).FirstOrDefault();
                data.exit_employee_details = _vmsconte.ISM_Resignation_DMO_con.Where(a => a.ISMRESG_Id == data.ISMRESG_Id).ToArray();

                data.relieving_check_list = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == data.ISMRESG_Id).ToArray();

                var MI_Id = _vmsconte.Hr_Master_Employee_con.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "doc_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@qq", SqlDbType.BigInt)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_id", SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = "Relieving"
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.releavingdoc_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Empheaddetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.managmentempdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.employeedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }



                data.reason_list_dd = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMRESGMRE_ActiveFlag == true && a.ISMRESGMRE_Flg == "Resignation").ToArray();


            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ISM_Resignation_DTO Savedata_td(ISM_Resignation_DTO dto)
        {
            try
            {

                foreach (var p in dto.doc_list2)
                {
                    //var result = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Single(a => a.ISMSLEDM_Id == p.ISMSLEDM_Id && a.ISMSMPR_Id == p.ISMSMPR_Id);
                    var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_resignation_checklist_update_proc @p0,@p1,@p2,@p3,@p4,@p5", dto.ISMRESG_Id, p.ISMRESGMCL_Id, p.ISMRESGCL_FileName, p.document_Path, dto.MI_Id, dto.userId);
                    if (contactExistsP > 0)
                    {
                        dto.returnval = "Add";
                    }
                    else
                    {
                        dto.returnval = "notUpdated";
                    }
                    //SqlConnection con = new SqlConnection("Data Source=demovaps.database.windows.net,1433;Initial Catalog=DevelopmentDataBase;Persist Security Info=False;User ID=demovaps;Password=vaps@123;Connection Timeout=30;");
                    // //var con = _vmsconte.Database.GetDbConnection().CreateCommand())

                }
                // _vmsconte.SaveChanges();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO edit_relieving(ISM_Resignation_DTO dto)
        {
            List<ISM_Resignation_DTO> result = new List<ISM_Resignation_DTO>();
            try
            {
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "edit_relieving_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.relieving_check_edit = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.relieving_check_edit_dd = (from a in _vmsconte.ISM_Resignation_DMO_con
                                               from b in _vmsconte.Hr_Master_Employee_con
                                               from c in _vmsconte.HR_Master_Department
                                               from d in _vmsconte.HR_Master_Designation_con
                                               where a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && b.HRMDES_Id == d.HRMDES_Id && a.HRME_Id == dto.HRME_Id
                                               select new ISM_Resignation_DTO
                                               {
                                                   employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName)
                                                 + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName)
                                                 + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_DOJ = b.HRME_DOJ,
                                                   HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                   HRMDES_DesignationName = d.HRMDES_DesignationName,
                                                   ISMRESG_TentativeLeavingDate = Convert.ToDateTime(a.ISMRESG_TentativeLeavingDate),
                                                   ISMRESG_Id = a.ISMRESG_Id
                                               }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public async Task<ISM_Resignation_DTO.LetterGenerationDTO> Savedata_printAsync(ISM_Resignation_DTO.LetterGenerationDTO dto)
        {
            try
            {
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Savedata_print_proc";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.VarChar)
                        {
                            Value = dto.userId
                        });
                        cmd.Parameters.Add(new SqlParameter("@RelievingLetterLNo", SqlDbType.VarChar)
                        {
                            Value = dto.RelievingLetterLNo
                        });
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", SqlDbType.VarChar)
                    {
                        Value = dto.companyname
                    });
                    cmd.Parameters.Add(new SqlParameter("@CompanyAddress", SqlDbType.VarChar)
                    {
                        Value = dto.companyaddress
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            var retObject1 = new List<dynamic>();
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    dto.return_p = "Update";
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO download_doc(ISM_Resignation_DTO dto)
        {
            try
            {
                var resrelivingflag = _vmsconte.ISM_Resignation_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).Select(t => t.ISMRESG_Flg).FirstOrDefault();
                if (resrelivingflag == "Resignation")
                {
                    dto.download_photo = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id).ToArray();

                }
                else
                {
                    dto.download_photo = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id && a.ISMRESGCL_Status == "Approved").ToArray();

                }

            }
            catch (Exception ee)

            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO print_exit_employee(ISM_Resignation_DTO dto)
        {
            try
            {
                //dto.exit_print_list2 = (from c in _vmsconte.ISM_Resignation_DMO_con
                //                        from a in _vmsconte.Hr_Master_Employee_con
                //                        from b in _vmsconte.ISM_Resignation_RelievingLetter_DMO_con
                //                        where c.HRME_Id == a.HRME_Id && c.ISMRESG_Id == b.ISMRESG_Id && a.HRME_Id == dto.HRME_Id
                //                        select new ISM_Resignation_DTO
                //                        {
                //                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                //                            + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                //                            + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                //                            HRME_Id = a.HRME_Id,
                //                            ISMRESG_Id = c.ISMRESG_Id,
                //                            ISMRESGRL_RLDate = b.ISMRESGRL_RLDate,
                //                            ISMRESGRL_ActiveFlag = b.ISMRESGRL_ActiveFlag,
                //                            ISMRESG_ResignationDate = c.ISMRESG_ResignationDate,
                //                            ISMRESG_Remarks = c.ISMRESG_Remarks,
                //                            ISMRESG_ActiveFlag = c.ISMRESG_ActiveFlag
                //                        }).Distinct().ToList().ToArray();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_employee_print_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });


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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.Exit_employee_print_report = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_Relieving_Latter_Rating";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.ratingarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Rating_Employeeratingcalc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                  SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ovrallflag",
                       SqlDbType.BigInt)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@yearid",
                SqlDbType.VarChar)
                    {
                        Value = 0
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.r_ismsesg_id = retObject.ToArray();
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
        public ISM_Resignation_DTO get_all_data_R(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.institutionname = _vmsconte.Institution.ToArray();

                dto.department_list_R = (from a in _vmsconte.ISM_Resignation_DMO_con
                                         from b in _vmsconte.Hr_Master_Employee_con
                                         from c in _vmsconte.HR_Master_Department
                                         where a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == dto.MI_Id

                                         select new ISM_Resignation_DTO
                                         {
                                             HRMD_DepartmentName = c.HRMD_DepartmentName,
                                             HRMD_Id = c.HRMD_Id
                                         }).Distinct().OrderBy(a => a.HRMD_Id).ToList().ToArray();

                dto.designation_list_R = (from a in _vmsconte.ISM_Resignation_DMO_con
                                          from b in _vmsconte.Hr_Master_Employee_con
                                          from c in _vmsconte.HR_Master_Designation_con
                                          where a.HRME_Id == b.HRME_Id && b.HRMDES_Id == c.HRMDES_Id && a.MI_Id == dto.MI_Id

                                          select new ISM_Resignation_DTO
                                          {
                                              HRMDES_DesignationName = c.HRMDES_DesignationName,
                                              HRMDES_Id = c.HRMDES_Id
                                          }).Distinct().OrderBy(a => a.HRMDES_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO get_all_relieving_data_R(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.department_list_R = (from a in _vmsconte.ISM_Resignation_DMO_con
                                         from b in _vmsconte.Hr_Master_Employee_con
                                         from c in _vmsconte.HR_Master_Department
                                         where a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == dto.MI_Id

                                         select new ISM_Resignation_DTO
                                         {
                                             HRMD_DepartmentName = c.HRMD_DepartmentName,
                                             HRMD_Id = c.HRMD_Id
                                         }).Distinct().OrderBy(t => t.HRMDES_Id).ToList().ToArray();

                dto.designation_list_R = (from a in _vmsconte.ISM_Resignation_DMO_con
                                          from b in _vmsconte.Hr_Master_Employee_con
                                          from c in _vmsconte.HR_Master_Designation_con
                                          where a.HRME_Id == b.HRME_Id && b.HRMDES_Id == c.HRMDES_Id && a.MI_Id == dto.MI_Id

                                          select new ISM_Resignation_DTO
                                          {
                                              HRMDES_DesignationName = c.HRMDES_DesignationName,
                                              HRMDES_Id = c.HRMDES_Id
                                          }).Distinct().OrderBy(a => a.HRMD_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public async Task<ISM_Resignation_DTO> showdetails_R(ISM_Resignation_DTO dto)
        {
            try
            {
                string dept_id = "0";
                string des_id = "0";
                List<long> dept_ids = new List<long>();
                List<long> des_ids = new List<long>();
                dto.imagepath = _vmsconte.Institution.Where(t => t.MI_Id == dto.MI_Id).Select(t => t.MI_Logo).FirstOrDefault();

                foreach (var item in dto.selectedept_list)
                {
                    dept_ids.Add(item.HRMD_Id);

                }

                foreach (var item in dto.selectedesig_list)
                {

                    des_ids.Add(item.HRMDES_Id);
                }

                for (int s = 0; s < dept_ids.Count(); s++)
                {
                    dept_id = dept_id + ',' + dept_ids[s].ToString();
                }

                for (int s = 0; s < des_ids.Count(); s++)
                {
                    des_id = des_id + ',' + des_ids[s].ToString();
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "exit_employee_report_pro";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dept_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                     SqlDbType.VarChar)
                    {
                        Value = des_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                    SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.FROMDATE)
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                   SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.TODATE)
                    });
                    cmd.Parameters.Add(new SqlParameter("@approved",
                  SqlDbType.BigInt)
                    {
                        Value = dto.ACCEPT
                    });
                    cmd.Parameters.Add(new SqlParameter("@rejected",
                  SqlDbType.BigInt)
                    {
                        Value = dto.REJECT
                    });
                    cmd.Parameters.Add(new SqlParameter("@pending",
                  SqlDbType.BigInt)
                    {
                        Value = dto.PENDING
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.exi_employee_print_list = retObject.ToArray();
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
        public async Task<ISM_Resignation_DTO> showdetails_relieving_R(ISM_Resignation_DTO dto)
        {
            try
            {
                string dept_id = "0";
                string des_id = "0";

                List<long> dept_ids = new List<long>();
                List<long> des_ids = new List<long>();

                foreach (var item in dto.selectedept_list)
                {
                    dept_ids.Add(item.HRMD_Id);

                }
                foreach (var item in dto.selectedesig_list)
                {

                    des_ids.Add(item.HRMDES_Id);
                }

                for (int s = 0; s < dept_ids.Count(); s++)
                {
                    dept_id = dept_id + ',' + dept_ids[s].ToString();
                }

                for (int s = 0; s < des_ids.Count(); s++)
                {
                    des_id = des_id + ',' + des_ids[s].ToString();
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "exit_employee_relieving_report_pro";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dept_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                     SqlDbType.VarChar)
                    {
                        Value = des_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE",
                    SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.FROMDATE)
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                   SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.TODATE)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.exi_employee_print_list = retObject.ToArray();
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
        public ISM_Resignation_DTO relieving_exit_employee_view(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.exit_print_list1 = (from c in _vmsconte.ISM_Resignation_DMO_con
                                        from a in _vmsconte.Hr_Master_Employee_con
                                        where c.HRME_Id == a.HRME_Id && c.ISMRESG_Id == dto.ISMRESG_Id
                                        select new ISM_Resignation_DTO
                                        {
                                            employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                            + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                            + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                            HRME_Id = a.HRME_Id,
                                            ISMRESG_Id = c.ISMRESG_Id,
                                            ISMRESGRL_RLDate = c.ISMRESG_TentativeLeavingDate,
                                            //ISMRESGRL_ActiveFlag = b.ISMRESGCL_ActiveFlag,
                                            ISMRESG_ResignationDate = c.ISMRESG_ResignationDate,
                                            ISMRESG_Remarks = c.ISMRESG_Remarks,
                                            ISMRESG_ActiveFlag = c.ISMRESG_ActiveFlag
                                        }).Distinct().ToList().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO loadEmployeeData(ISM_Resignation_DTO data)
        {
            try
            {
                long hrme_id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.userId).Select(t => t.Emp_Code).FirstOrDefault();

                data.companydetails = _vmsconte.Institution.Where(t => t.MI_Id == data.MI_Id && t.MI_ActiveFlag == 1).ToArray();

                data.StatusFlag = _vmsconte.ISM_Resignation_DMO_con.Where(t => t.HRME_Id == hrme_id && t.ISMRESG_Flg == "NOC").Select(t => t.ISMRESG_MgmtApprRejFlg).FirstOrDefault();

                //data.GetGriddata = _vmsconte.ISM_Resignation_DMO_con.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == hrme_id && t.ISMRESG_ActiveFlag == true ).ToArray();


                data.GetGriddata = (from a in _vmsconte.ISM_Resignation_DMO_con
                                    from b in _vmsconte.ISM_Resignation_Master_Reasons_DMO_con
                                    where (a.HRME_Id == hrme_id && a.ISMRESGMRE_Id == b.ISMRESGMRE_Id && a.MI_Id == data.MI_Id && a.ISMRESG_ActiveFlag == true && a.ISMRESG_Flg == "Resignation")
                                    select new ISM_Resignation_DTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        ISMRESG_Id = a.ISMRESG_Id,
                                        ISMRESGMRE_Id = b.ISMRESGMRE_Id,
                                        ISMRESG_ResignationDate = a.ISMRESG_ResignationDate,
                                        ISMRESGMRE_ResignationReasons = b.ISMRESGMRE_ResignationReasons,
                                        ISMRESG_MgmtApprRejFlg = a.ISMRESG_MgmtApprRejFlg,
                                        ISMRESG_Remarks = a.ISMRESG_Remarks,
                                        ISMRESG_TentativeLeavingDate = a.ISMRESG_TentativeLeavingDate,
                                    }).Distinct().ToArray();

                data.getGriddataReleaving = (from a in _vmsconte.ISM_Resignation_DMO_con
                                             from b in _vmsconte.ISM_Resignation_Master_Reasons_DMO_con
                                             where (a.HRME_Id == hrme_id && a.ISMRESGMRE_Id == b.ISMRESGMRE_Id && a.MI_Id == data.MI_Id && a.ISMRESG_ActiveFlag == true && a.ISMRESG_Flg == "Relieving")
                                             select new ISM_Resignation_DTO
                                             {
                                                 HRME_Id = a.HRME_Id,
                                                 ISMRESG_Id = a.ISMRESG_Id,
                                                 ISMRESGMRE_Id = b.ISMRESGMRE_Id,
                                                 ISMRESG_ResignationDate = a.ISMRESG_ResignationDate,
                                                 ISMRESGMRE_ResignationReasons = b.ISMRESGMRE_ResignationReasons,
                                                 ISMRESG_MgmtApprRejFlg = a.ISMRESG_MgmtApprRejFlg,
                                                 ISMRESG_Remarks = a.ISMRESG_Remarks,
                                             }).Distinct().ToArray();

                if (hrme_id > 0)
                {
                    data.employeedata = (from a in _vmsconte.HR_Master_Employee_DMO
                                         from b in _vmsconte.HR_Master_Designation
                                         from d in _vmsconte.Multiple_Email_DMO
                                         from e in _vmsconte.Multiple_Mobile_DMO
                                         where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == hrme_id && d.HRME_Id == a.HRME_Id && e.HRME_Id == a.HRME_Id && d.HRMEM_DeFaultFlag == "default" && e.HRMEMNO_DeFaultFlag == "default")
                                         select new ISM_Resignation_DTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             employeename = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                             HRME_DOJ = a.HRME_DOJ,
                                             HRME_MobileNo = e.HRMEMNO_MobileNo,
                                             HRME_EmailId = d.HRMEM_EmailId,
                                             HRMDES_DesignationName = b.HRMDES_DesignationName,
                                             HRME_PerStreet = a.HRME_PerStreet,
                                             HRME_PerArea = a.HRME_PerArea,
                                             HRME_PerCity = a.HRME_PerCity,
                                             HRME_PerStateId = a.HRME_PerStateId,
                                             HRME_PerPincode = a.HRME_PerPincode,
                                             IVRMMG_Id = a.IVRMMG_Id
                                         }).Distinct().ToArray();
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Empheaddetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = hrme_id
                    });
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.managmentempdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "doc_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = hrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@qq", SqlDbType.BigInt)
                    {
                        Value = 3
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_id", SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = "Relieving"
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.releavingdoc_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "doc_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = hrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@qq", SqlDbType.BigInt)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_id", SqlDbType.BigInt)
                    {
                        Value = data.userId
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = "Resignation"
                    });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.doc_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DailyReport_Generation_Deviation_Calculation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = hrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar)
                    {
                        Value = DateTime.Today.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = 1
                    });

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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getdeviationreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.Resignation_list_dd = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMRESGMRE_ActiveFlag == true && a.ISMRESGMRE_Flg == "Resignation").ToArray();
                data.reason_list_dd = _vmsconte.ISM_Resignation_Master_Reasons_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMRESGMRE_ActiveFlag == true && a.ISMRESGMRE_Flg == "Relieving").ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ISM_Resignation_DTO sendResignationMail(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.HRME_Id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();
                dto.ISMRESG_ResignationDate = DateTime.Today;

                var department = (from a in _vmsconte.HR_Master_Employee_DMO
                                  from b in _vmsconte.HR_Master_Department
                                  where (a.HRMD_Id == b.HRMD_Id && a.HRME_Id == dto.HRME_Id)
                                  select b).Select(t => t.HRMD_DepartmentName).FirstOrDefault();

                dto = Exit_empl_SaveEdit(dto);
                if (dto.returnval == "Add")
                {
                    string mailid = dto.ManagerMailid;
                    var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == dto.MI_Id).ToList();
                    string Mailmsg = "Please accept this letter as formal notification that I am resigning from my position as " + department + ", My resignation will be effective from " + DateTime.Now.ToString("dd/MM/yyyy") + "." +
                        "\n\n\n I would like to thank you for the opportunity to have worked in the position. I have learned a great deal during my time here and have enjoyed collaborating with my colleagues. I will take a lot of what I have learned with me in my career and look back at my time here as a valuable period of my professional life. " +
                        " \n\n\n During my Notice period, I will do what I can to make the transition as smooth as possible and will support in whatever way I can to hand over my duties to colleagues or to my replacement. Please let me know if there is anything further I can do to assist in this process.  \n\n\n Thank You";

                    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _vmsconte.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();

                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        string mailcc = alldetails[0].IVRM_mailcc;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        string Subject = "Resignation Letter";
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mailid);

                        StringBuilder sb = new StringBuilder(dto.Template);
                        StringReader sr = new StringReader(sb.ToString());
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                        message.AddCc(dto.TeamLeadMailid);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                            pdfDoc.Open();

                            htmlparser.Parse(sr);
                            pdfDoc.Close();

                            byte[] bytess = memoryStream.ToArray();
                            memoryStream.Close();

                            var file = Convert.ToBase64String(bytess);
                            string emp;
                            emp = Convert.ToString(sr);
                            string c = "";
                            string v = emp.Replace("System.IO.StringReader", "ResignationLetter.Pdf");
                            message.AddAttachment(v, file);
                            message.HtmlContent = Mailmsg;
                            var client = new SendGridClient(sengridkey);
                            client.SendEmailAsync(message).Wait();
                        }

                        using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name.Equals("Resignation_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_Email_Outgoing_1";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                            {
                                Value = mailid
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                            {
                                Value = Mailmsg
                            });
                            cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                            {
                                Value = "Resignation Letter"
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = dto.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                            {
                                Value = "Staff"
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                }
                            }
                            catch (Exception ex)
                            {
                                //return ex.Message;
                            }
                        }
                    }

                    dto.retrunMsg = "Success";
                }
                else if (dto.returnval == "AddedChecklist")
                {
                    dto.returnval = "AddedChecklist";
                }
                else
                {
                    dto.returnval = "Exist";
                }

            }
            catch (Exception ex)
            {
                dto.retrunMsg = "False";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO chcklistdocs(ISM_Resignation_DTO dto)
        {
            try
            {
                //Delete Uploaded Checklist
                if (dto.ISMRESGCL_Id > 0)
                {
                    var res = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Single(a => a.ISMRESGCL_Id == dto.ISMRESGCL_Id);
                    _vmsconte.Remove(res);

                    var ab = _vmsconte.SaveChanges();
                    if (ab > 0)
                    {
                        dto.returnval = "Update";
                    }
                    else
                    {
                        dto.returnval = "notUpdated";
                    }

                }
                else
                {

                    dto.HRME_Id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();

                    dto.uploadeddocumets = _vmsconte.ISM_Resignation_ChecKLists_DMO_con.Where(a => a.ISMRESG_Id == dto.ISMRESG_Id && a.ISMRESGCL_ActiveFlag == true).ToArray();

                    dto.apporvedrejectedperson = (from a in _vmsconte.ISM_Resignation_ApprovedBy_DMO
                                                  from b in _vmsconte.HR_Master_Employee_DMO
                                                  where (a.MI_Id == b.MI_Id && a.HRME_Id == b.HRME_Id && a.ISMRESG_Id == dto.ISMRESG_Id && a.ISMRESGAB_ActiveFlag == true)
                                                  select new ISM_Resignation_DTO
                                                  {
                                                      HRME_Id = a.HRME_Id,
                                                      employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName)
                                                   + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName)
                                                   + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                                      ISMRESGAB_Date = a.ISMRESGAB_Date,
                                                      ISMRESGAB_Remarks = a.ISMRESGAB_Remarks,
                                                      ISMRESGAB_AppRejFlg = a.ISMRESGAB_AppRejFlg,
                                                      ISMRESGAB_RelievingDate = a.ISMRESGAB_RelievingDate,
                                                  }).ToArray();

                }



            }
            catch (Exception ex)
            {
                dto.retrunMsg = "False";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO savechecklists(ISM_Resignation_DTO dto)
        {
            try
            {
                dto.HRME_Id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == dto.userId).Select(t => t.Emp_Code).FirstOrDefault();
                long ISMRESG_Id = _vmsconte.ISM_Resignation_DMO_con.Where(t => t.HRME_Id == dto.HRME_Id && t.ISMRESG_Flg == dto.ISMRESG_Flg).Select(t => t.ISMRESG_Id).FirstOrDefault();

                foreach (var p in dto.doc_list2)
                {
                    var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_resignation_checklist_update_proc @p0,@p1,@p2,@p3,@p4,@p5", ISMRESG_Id, p.ISMRESGMCL_Id, p.ISMRESGCL_FileName, p.document_Path, dto.MI_Id, dto.userId);
                    if (contactExistsP > 0)
                    {
                        dto.returnval = "AddedChecklist";
                    }
                    else
                    {
                        dto.returnval = "notUpdated";
                    }
                }
            }
            catch (Exception ex)
            {
                dto.retrunMsg = "False";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public async Task<ISM_Resignation_DTO> GetoverallReport(ISM_Resignation_DTO dto)
        {
            try
            {
                string mi_id = "0";
                string dept_id = "0";
                string des_id = "0";
                List<long> mi_ids = new List<long>();
                List<long> dept_ids = new List<long>();
                List<long> des_ids = new List<long>();
                dto.imagepath = _vmsconte.Institution.Where(t => t.MI_Id == dto.MI_Id).Select(t => t.MI_Logo).FirstOrDefault();

                foreach (var item in dto.selectinstitution_list)
                {
                    mi_ids.Add(item.MI_Id);

                }
                foreach (var item in dto.selectedept_list)
                {
                    dept_ids.Add(item.HRMD_Id);

                }

                foreach (var item in dto.selectedesig_list)
                {

                    des_ids.Add(item.HRMDES_Id);
                }
                for (int s = 0; s < mi_ids.Count(); s++)
                {
                    mi_id = mi_id + ',' + mi_ids[s].ToString();
                }

                for (int s = 0; s < dept_ids.Count(); s++)
                {
                    dept_id = dept_id + ',' + dept_ids[s].ToString();
                }

                for (int s = 0; s < des_ids.Count(); s++)
                {
                    des_id = des_id + ',' + des_ids[s].ToString();
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_ResgRelvingdetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = mi_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                   SqlDbType.VarChar)
                    {
                        Value = dept_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                     SqlDbType.VarChar)
                    {
                        Value = des_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                    SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.FROMDATE)
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                   SqlDbType.DateTime)
                    {
                        Value = Convert.ToDateTime(dto.TODATE)
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = dto.StatusFlag
                    });
                    cmd.Parameters.Add(new SqlParameter("@RRflg", SqlDbType.VarChar)
                    {
                        Value = dto.ISMRESG_Flg
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.exi_employee_print_list = retObject.ToArray();
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
        public async Task<ISM_Resignation_DTO> approverRemark(ISM_Resignation_DTO dto)
        {
            try
            {
                // dto.approvarremarks = _vmsconte.ISM_Resignation_ApprovedBy_DMO.Where(t => t.ISMRESG_Id == dto.ISMRESG_Id).ToArray();
                dto.approvarremarks = (from a in _vmsconte.ISM_Resignation_ApprovedBy_DMO
                                       from b in _vmsconte.HR_Master_Employee_DMO
                                       where (a.ISMRESG_Id == dto.ISMRESG_Id && a.HRME_Id == b.HRME_Id)
                                       select new ISM_Resignation_DTO
                                       {
                                           ISMRESGAB_Date = a.ISMRESGAB_Date,
                                           ISMRESGAB_Remarks = a.ISMRESGAB_Remarks,
                                           ISMRESGAB_RelievingDate = a.ISMRESGAB_RelievingDate,
                                           ISMRESGAB_AppRejFlg = a.ISMRESGAB_AppRejFlg,
                                           employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName)
                                              + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName)
                                              + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                       }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO AutoTranseferResignation(ISM_Resignation_DTO dto)
        {
            //try
            //{
            //    //
            //    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
            //    {
            //        cmd.CommandText = "HR_EmpMonthWiseClPlLeavesAutoCredit";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
            //        {
            //            Value = dto.MI_Id
            //        });

            //        if (cmd.Connection.State != ConnectionState.Open)
            //            cmd.Connection.Open();
            //        try
            //        {
            //            var retObject1 = new List<dynamic>();
            //            using (var dataReader1 = await cmd.ExecuteReaderAsync())
            //            {
            //                //while (dataReader1.Read())
            //                //{
            //                //    data.autonextdata.Add(new AUTONEXTDTO
            //                //    {
            //                //        MI_Id = (long)(dataReader1["MI_Id"]),
            //                //        HRELAP_Id = (long)(dataReader1["HRELAP_Id"]),
            //                //        HRME_Id = (long)(dataReader1["HRME_Id"]),
            //                //        HEAD_HRME_Id = (long)(dataReader1["HEAD_HRME_Id"]),
            //                //        HRLAON_SanctionLevelNo = (long)(dataReader1["HRLAON_SanctionLevelNo"]),
            //                //        HRME_EmailId = Convert.ToString(dataReader1["HRME_EmailId"]),
            //                //        HEAD_HRME_EmailId = Convert.ToString(dataReader1["HEAD_HRME_EmailId"]),
            //                //        LeaveStatus = Convert.ToString(dataReader1["LeaveStatus"]),
            //                //    });
            //                //}
            //                while (await dataReader1.ReadAsync())
            //                {
            //                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
            //                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
            //                    {
            //                        dataRow1.Add(
            //                            dataReader1.GetName(iFiled1),
            //                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
            //                        );
            //                    }
            //                    retObject1.Add((ExpandoObject)dataRow1);
            //                }
            //            }
            //            dto.responsemessage = "Sucess";
            //        }
            //        catch (Exception ex)
            //        {
            //            dto.responsemessage = "fail";
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    dto.retrunMsg = "False";
            //    Console.WriteLine(ex.Message);
            //}
            return dto;
        }

        //Exit Overall Report
        public ISM_Resignation_Master_Reasons_DTO getalldetails(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {

                dto.fillyear = (from a in _vmsconte.HR_Master_LeaveYearDMO
                                where (a.MI_Id == dto.MI_Id && a.HRMLY_ActiveFlag == true)
                                select new HR_Master_LeaveYearDTO
                                {
                                    HRMLY_Id = a.HRMLY_Id,
                                    HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                    HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                dto.fillmonth = (from a in _vmsconte.month
                                 select new EmployeeLogReportDTO
                                 {
                                     IVRM_Month_Id = a.IVRM_Month_Id,
                                     monthname = a.IVRM_Month_Name
                                 }).Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeData";
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
                        dto.getalldetails = retObject.ToArray();
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
        public ISM_Resignation_Master_Reasons_DTO getReport(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                string hrme_ids = "0";
                List<long> hrmeid = new List<long>();

                if (dto.EmpName.Length > 0)
                {
                    foreach (var item in dto.EmpName)
                    {
                        hrmeid.Add(item.HRME_Id);
                    }

                    for (int i = 0; i < hrmeid.Count; i++)
                    {
                        hrme_ids = hrme_ids + "," + hrmeid[i].ToString();
                    }

                }

                var d = dto.Fromdate.Value.Date.ToString("yyyy-MM-dd");

                var d1 = dto.Todate.Value.Date.ToString("yyyy-MM-dd");


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ExitOverallReportProc1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = hrme_ids });
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.VarChar) { Value = dto.month });
                    cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.VarChar) { Value = dto.year });
                    cmd.Parameters.Add(new SqlParameter("@FROM_DATE", SqlDbType.VarChar) { Value = d });
                    cmd.Parameters.Add(new SqlParameter("@TO_DATE", SqlDbType.VarChar) { Value = d1 });
                    cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.VarChar) { Value = dto.type });
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
                        dto.getReport = retObject.ToArray();
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

        //---------------------- ISM Resignation Checklist Approval------------------------

        public ISM_Resignation_ChecKLists_DTO checklistloaddata(ISM_Resignation_ChecKLists_DTO data)
        {
            try
            {
                var HRME_Id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Resignation_Checklist_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMRESG_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "ApprovalList"
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.approvallist = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;

        }
        public ISM_Resignation_ChecKLists_DTO viewchecklistdata(ISM_Resignation_ChecKLists_DTO data)
        {
            try
            {
                var HRME_Id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Resignation_Checklist_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMRESG_Id", SqlDbType.BigInt)
                    {
                        Value = data.ISMRESG_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "CheckLists"
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.viewchecklistdatas = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;

        }

        public ISM_Resignation_ChecKLists_DTO checklistapprovedrecord(ISM_Resignation_ChecKLists_DTO data)
        {
            try
            {
                var HRME_Id = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.ISMRESGCLAPP_Id > 0)
                {
                    for (int j = 0; j < data.ChecklistArray.Length; j++)
                    {
                        var result = _vmsconte.ISM_Resignation_ChecKLists_ApprovedBy_DMO.Single(a => a.ISMRESGCLAPP_Id == data.ISMRESGCLAPP_Id);
                        result.ISMRESGCL_Id = data.ChecklistArray[j].ISMRESGCL_Id;
                        result.HRME_Id = HRME_Id;
                        result.ISMRESGCLAPP_Remarks = data.ISMRESGCLAPP_Remarks;
                        result.ISMRESGCLAPP_FileName = data.ChecklistArray[j].ISMRESGCL_FileName;
                        result.ISMRESGCLAPP_FilePath = data.ChecklistArray[j].ISMRESGCL_FilePath;
                        result.ISMRESGCLAPP_UpdatedDate = DateTime.Now;
                        result.ISMRESGCLAPP_UpdatedBy = data.UserId;
                        result.ISMRESGCLAPP_Status = data.ISMRESGCLAPP_Status;
                        _vmsconte.Update(result);
                        int i = _vmsconte.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "Update";
                        }
                        else
                        {
                            data.returnval = "Exist";
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < data.ChecklistArray.Length; j++)
                    {

                        var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_Relieving_CheckList_Approval_Proc @p0,@p1,@p2,@p3", data.ChecklistArray[j].ISMRESGCL_Id, data.UserId, data.ISMRESGCLAPP_Remarks, data.ISMRESGCLAPP_Status);
                        if (contactExistsP > 0)
                        {
                            data.returnval = "Add";
                        }
                        else
                        {
                            data.returnval = "Exist";
                        }

                        //ISM_Resignation_ChecKLists_ApprovedBy_DMO rmr = new ISM_Resignation_ChecKLists_ApprovedBy_DMO();
                        //rmr.ISMRESGCL_Id = data.ChecklistArray[j].ISMRESGCL_Id;
                        //rmr.HRME_Id = HRME_Id;
                        //rmr.ISMRESGCLAPP_Remarks = data.ISMRESGCLAPP_Remarks;
                        //rmr.ISMRESGCLAPP_FileName = data.ChecklistArray[j].ISMRESGCL_FileName;
                        //rmr.ISMRESGCLAPP_FilePath = data.ChecklistArray[j].ISMRESGCL_FilePath;
                        //rmr.ISMRESGCLAPP_ActiveFlag = true;
                        //rmr.ISMRESGCLAPP_CreatedDate = DateTime.Now;
                        //rmr.ISMRESGCLAPP_UpdatedDate = DateTime.Now;
                        //rmr.ISMRESGCLAPP_CreatedBy = data.UserId;
                        //rmr.ISMRESGCLAPP_UpdatedBy = data.UserId;
                        //rmr.ISMRESGCLAPP_Status = data.ISMRESGCLAPP_Status;
                        //_vmsconte.Add(rmr);

                    }
                    //int i = _vmsconte.SaveChanges();

                    //if (i > 0)
                    //{
                    //    data.returnval = "Add";
                    //}
                    //else
                    //{
                    //    data.returnval = "Exist";
                    //}
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        //END
        //3 months before Intemation for HR of employee bond period completion.
        public ISM_Resignation_Master_Reasons_DTO HRIntemation(ISM_Resignation_Master_Reasons_DTO dto)
        {
            try
            {
                List<ISM_Resignation_Master_Reasons_DTO> employeedetails = new List<ISM_Resignation_Master_Reasons_DTO>();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Exit_Intemation";
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
                                //retObject.Add((ExpandoObject)dataRow);


                                employeedetails.Add(new ISM_Resignation_Master_Reasons_DTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_ID"].ToString()),
                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"].ToString()),
                                    HRME_EmployeeFirstName = dataReader["HRME_EmployeeFirstName"].ToString(),
                                    HRME_EmployeeCode = dataReader["HRME_EmployeeCode"].ToString(),
                                    HRME_DOJ = Convert.ToDateTime(dataReader["HRME_DOJ"].ToString()),
                                    HRME_ENDDate = Convert.ToDateTime(dataReader["HRME_ENDDate"].ToString()),
                                    HRMD_DepartmentName = dataReader["HRMD_DepartmentName"].ToString(),
                                    HRMDES_DesignationName = dataReader["HRMDES_DesignationName"].ToString(),
                                    HRName = dataReader["HRName"].ToString(),
                                    HRemail = dataReader["HRemail"].ToString(),
                                    DeptHead = dataReader["DeptHead"].ToString(),
                                    DeptheadEmail = dataReader["DeptheadEmail"].ToString(),
                                    HRMEM_EmailId = dataReader["empemail"].ToString()



                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (employeedetails != null && employeedetails.Count > 0)
                {
                    for (var i = 0; i < employeedetails.Count; i++)
                    {
                        var ccmail = "";
                        ccmail = employeedetails[i].HRemail + "," + employeedetails[i].DeptheadEmail;
                        // HR_Intemation(dto.MI_Id,"HRIntemationExit", employeedetails[i].HRME_EmployeeFirstName, employeedetails[i].HRME_EmployeeCode, employeedetails[i].HRMEM_EmailId, employeedetails[i].HRME_DOJ, employeedetails[i].HRME_ENDDate, employeedetails[i].HRMD_DepartmentName, employeedetails[i].HRMDES_DesignationName, employeedetails[i].HRName, employeedetails[i].HRemail, employeedetails[i].DeptHead, employeedetails[i].DeptheadEmail);
                        HR_Intemation(employeedetails[i].MI_Id, "HRIntemationExit", employeedetails[i].HRME_Id, employeedetails[i].HRMEM_EmailId, ccmail);

                    }
                }






            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Resignation_DTO.fullANDFinalSettlementDTO FANDFSettlement(ISM_Resignation_DTO.fullANDFinalSettlementDTO dto)
        {
            try
            {

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Leave_Encash";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try 
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.LeaveEnchashmentData = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GratuityCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.gratuitydetails = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FANDF_TADA_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.tadadetails = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EMP_Loan_PendingDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.loandetails = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_Damage_Amount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.demagedetails = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.bankdetails = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_ActiveFlag == true).ToArray();
                dto.ffdetailsarray = _vmsconte.ISM_Resignation_DMO_con.Where(t => t.ISMRESG_Id == dto.ISMRESG_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Resignation_DTO.fullANDFinalSettlementDTO DamageSave(ISM_Resignation_DTO.fullANDFinalSettlementDTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Resignation_DMO_con.Where(t => t.ISMRESG_Id == dto.ISMRESG_Id).ToList().FirstOrDefault();
                result.ISMRESG_FFTotalAmount = dto.totalffamount;
                result.ISMRESG_FFStatus = true;                
                _vmsconte.Update(result);
                var n = _vmsconte.SaveChanges();

                if (n > 0)
                {
                 
                    dto.ffdetailsarray = _vmsconte.ISM_Resignation_DMO_con.Where(t => t.ISMRESG_Id == dto.ISMRESG_Id).ToArray();
                }
               


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public string HR_Intemation(long MI_Id, string Template, long HRME_ID, string email, string ccemail)
        {
            try
            {


                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                var Paramaeters = _vmsconte.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _vmsconte.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Email_Prametars_Replace";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",
                  SqlDbType.VarChar)
                    {
                        Value = HRME_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                  SqlDbType.VarChar)
                    {
                        Value = "HRIntemationExit"
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }

                                    else if (datatype.Name == "Decimal")
                                    {
                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _vmsconte.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _vmsconte.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    string mailcc = "";
                    string mailbcc = "";

                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }



                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(email);
                    // message.AddTo("kavitajnandoor21@gmail.com");
                    //message.AddTo("sreepriyagangadharan15@gmail.com");

                    if (Attechement.Equals("1"))
                    {
                        var img = _vmsconte.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }
                    //******************* EMAIL CC DETAILS ****************//

                    if (ccemail != null && ccemail != "")
                    {
                        string[] ccmaildetails = ccemail.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }


                    if (mailcc != null && mailcc != "")
                    {
                        string[] ccmaildetails = mailcc.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }

                    //if (ccemail != null && ccemail != "")
                    //{
                    //    message.AddBcc(ccemail);
                    //}

                    //******************* EMAIL BCC DETAILS ****************//




                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    try
                    {
                        client.SendEmailAsync(message).Wait();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();

                        var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";

        }
        //END
        public string sendmail(long MI_Id, string EmailCC, string Email, string Template, long HRMEID, DateTime? LeaveDate, long userid, string status, DateTime resignationdate, string designation, string Remarks, string Level)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _vmsconte.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _vmsconte.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                var Empname = (from c in _vmsconte.HR_Master_Employee_DMO
                               where (c.HRME_Id == HRMEID)
                               select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();
                var approvarname = (from a in _vmsconte.Staff_User_Login
                                    from c in _vmsconte.HR_Master_Employee_DMO
                                    where (a.Id == userid && a.Emp_Code == c.HRME_Id)
                                    select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();

                string companyname = _vmsconte.Institution.Where(p => p.MI_Id == MI_Id && p.MI_ActiveFlag == 1).Select(p => p.MI_Name).FirstOrDefault();
                string logo = _vmsconte.Institution.Where(p => p.MI_Id == MI_Id && p.MI_ActiveFlag == 1).Select(p => p.MI_Logo).FirstOrDefault();
                Mailmsg = Mailmsg.Replace("[NAME]", Empname.HRME_EmployeeFirstName + " " + (Empname.HRME_EmployeeMiddleName == null ? "" : Empname.HRME_EmployeeMiddleName) + " " + (Empname.HRME_EmployeeLastName == null ? "" : Empname.HRME_EmployeeLastName));
                Mailmsg = Mailmsg.Replace("[EMPNAME]", approvarname.HRME_EmployeeFirstName + " " + (approvarname.HRME_EmployeeMiddleName == null ? "" : approvarname.HRME_EmployeeMiddleName) + " " + (approvarname.HRME_EmployeeLastName == null ? "" : approvarname.HRME_EmployeeLastName));
                Mailmsg = Mailmsg.Replace("[LOGO]", logo);
                Mailmsg = Mailmsg.Replace("[STATUS]", status);
                Mailmsg = Mailmsg.Replace("[Remarks]", Remarks);
                Mailmsg = Mailmsg.Replace("[Designation]", designation);
                Mailmsg = Mailmsg.Replace("[Level]", Level);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));
                Mailmsg = Mailmsg.Replace("[ResignationDATE]", resignationdate.ToString("dd/MM/yyyy"));
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _vmsconte.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";

                    //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    //{
                    //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                    //    mailcc = ccmail[0].ToString();

                    //    if (ccmail.Length > 1)
                    //    {
                    //        if (ccmail[1] != null || ccmail[1] != "")
                    //        {
                    //            mailbcc = ccmail[1].ToString();
                    //        }
                    //    }

                    //}
                    //if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    //{
                    //    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    //}

                    var message = new SendGridMessage();
                    //if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    //{
                    //    string[] mail_id = template[0].ISES_MailCCId.Split(',');
                    //    if (mail_id.Length > 0)
                    //    {
                    //        for (int i = 0; i < mail_id.Length; i++)
                    //        {
                    //            message.AddTo(mail_id[i]);

                    //        }
                    //    }
                    //}
                    //message.AddCc(EmailCC);
                    if (EmailCC != null && EmailCC != "")
                    {
                        string[] mail_id = EmailCC.Split(',');
                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddCc(mail_id[i]);

                            }
                        }
                    }
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _vmsconte.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }

                    //if (mailcc != null && mailcc != "")
                    //{
                    //    message.AddCc(mailcc);
                    //}
                    //if (mailbcc != null && mailbcc != "")
                    //{
                    //    message.AddBcc(mailbcc);
                    //}
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string sendmailToNextLevel(long MI_Id, string EmailCC, string Email, string Template, long HRMEID, DateTime? LeaveDate, long userid, string status, DateTime resignationdate, string designation, string Remarks, string Level)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _vmsconte.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _vmsconte.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                var Empname = (from c in _vmsconte.HR_Master_Employee_DMO
                               where (c.HRME_Id == HRMEID)
                               select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();
                var approvarname = (from a in _vmsconte.Staff_User_Login
                                    from c in _vmsconte.HR_Master_Employee_DMO
                                    where (a.Id == userid && a.Emp_Code == c.HRME_Id)
                                    select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();

                string companyname = _vmsconte.Institution.Where(p => p.MI_Id == MI_Id && p.MI_ActiveFlag == 1).Select(p => p.MI_Name).FirstOrDefault();
                string logo = _vmsconte.Institution.Where(p => p.MI_Id == MI_Id && p.MI_ActiveFlag == 1).Select(p => p.MI_Logo).FirstOrDefault();
                Mailmsg = Mailmsg.Replace("[NAME]", Empname.HRME_EmployeeFirstName + " " + (Empname.HRME_EmployeeMiddleName == null ? "" : Empname.HRME_EmployeeMiddleName) + " " + (Empname.HRME_EmployeeLastName == null ? "" : Empname.HRME_EmployeeLastName));
                Mailmsg = Mailmsg.Replace("[EMPNAME]", approvarname.HRME_EmployeeFirstName + " " + (approvarname.HRME_EmployeeMiddleName == null ? "" : approvarname.HRME_EmployeeMiddleName) + " " + (approvarname.HRME_EmployeeLastName == null ? "" : approvarname.HRME_EmployeeLastName));
                Mailmsg = Mailmsg.Replace("[LOGO]", logo);
                Mailmsg = Mailmsg.Replace("[STATUS]", status);
                Mailmsg = Mailmsg.Replace("[Remarks]", Remarks);
                Mailmsg = Mailmsg.Replace("[Designation]", designation);
                Mailmsg = Mailmsg.Replace("[Level]", Level);
                Mailmsg = Mailmsg.Replace("[Company]", companyname);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));
                Mailmsg = Mailmsg.Replace("[ResignationDATE]", resignationdate.ToString("dd/MM/yyyy"));
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _vmsconte.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";

                    //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    //{
                    //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                    //    mailcc = ccmail[0].ToString();

                    //    if (ccmail.Length > 1)
                    //    {
                    //        if (ccmail[1] != null || ccmail[1] != "")
                    //        {
                    //            mailbcc = ccmail[1].ToString();
                    //        }
                    //    }

                    //}
                    //if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    //{
                    //    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    //}

                    var message = new SendGridMessage();
                    //if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    //{
                    //    string[] mail_id = template[0].ISES_MailCCId.Split(',');
                    //    if (mail_id.Length > 0)
                    //    {
                    //        for (int i = 0; i < mail_id.Length; i++)
                    //        {
                    //            message.AddTo(mail_id[i]);

                    //        }
                    //    }
                    //}
                    //message.AddCc(EmailCC);
                    if (EmailCC != null && EmailCC != "")
                    {
                        string[] mail_id = EmailCC.Split(',');
                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddCc(mail_id[i]);

                            }
                        }
                    }
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _vmsconte.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string sendEmail(long MI_Id, string Template, long HRME_ID, string email, string ccemail)        {            try            {                Dictionary<string, string> val = new Dictionary<string, string>();                var template = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template                && e.ISES_MailActiveFlag == true).ToList();                if (template.Count == 0)                {                    return "Email Template not Mapped to the selected Institution";                }                var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == MI_Id).ToList();                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Resultsms = Mailcontent;                string result = Mailmsg;                var Paramaeters = _vmsconte.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();                var ParamaetersName = _vmsconte.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_Email_Prametars_Replace";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",                  SqlDbType.VarChar)                    {                        Value = HRME_ID                    });                    cmd.Parameters.Add(new SqlParameter("@Type",                  SqlDbType.VarChar)                    {                        Value = Template                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                    var datatype = dataReader.GetFieldType(iFiled);                                    if (datatype.Name == "DateTime")                                    {                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else if (datatype.Name == "Decimal")                                    {                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else                                    {                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());                                    }                                }                            }                        }                    }                    catch (Exception ex)                    {                        return ex.Message;                    }                }                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailmsg = result;                        }                    }                }                Mailmsg = result;                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {


                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailcontent = Resultsms;                        }                    }                }                Mailcontent = Resultsms;                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();                alldetails = _vmsconte.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();                string Attechement = "";                if (alldetails.Count > 0)                {                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);                    string Subject = template[0].ISES_MailSubject.ToString();                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();                    smstpdetails = _vmsconte.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();                    string mailcc = "";                    string mailbcc = "";

                    //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    //{
                    //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                    //    mailcc = ccmail[0].ToString();

                    //    if (ccmail.Length > 1)
                    //    {
                    //        if (ccmail[1] != null || ccmail[1] != "")
                    //        {
                    //            mailbcc = ccmail[1].ToString();
                    //        }
                    //    }

                    //}
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")                    {                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();                    }                    var message = new SendGridMessage();                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);                    message.Subject = Subject;                    message.AddTo(email);

                    // mailto:message.addto("kavitajnandoor21@gmail.com");
                    //message.AddTo("sreepriyagangadharan15@gmail.com");

                    if (Attechement.Equals("1"))                    {                        var img = _vmsconte.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();                        if (img.Count > 0)                        {                            for (int i = 0; i < img.Count; i++)                            {                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;                                Stream stream = response.GetResponseStream();                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);                            }                        }                    }
                    //****************** EMAIL CC DETAILS ***************//

                    if (ccemail != null && ccemail != "")                    {                        string[] ccmaildetails = ccemail.Split(',');                        foreach (var c in ccmaildetails)                        {                            message.AddCc(c);                        }                    }                    //if (mailcc != null && mailcc != "")                    //{                    //    string[] ccmaildetails = mailcc.Split(',');                    //    foreach (var c in ccmaildetails)                    //    {                    //        message.AddCc(c);                    //    }                    //}

                    //if (ccemail != null && ccemail != "")
                    //{
                    //    message.AddBcc(ccemail);
                    //}

                    //****************** EMAIL BCC DETAILS ***************//




                    if (mailcc != null && mailcc != "")                    {
                        //message.AddCc(mailcc);
                    }                    if (mailbcc != null && mailbcc != "")                    {                        message.AddBcc(mailbcc);                    }                    message.HtmlContent = Mailmsg;                    var client = new SendGridClient(sengridkey);                    try                    {                        client.SendEmailAsync(message).Wait();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())                    {                        var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();                        var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();                        var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();                        cmd.CommandText = "IVRM_Email_Outgoing";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@EmailId",                            SqlDbType.NVarChar)                        {                            Value = email                        });                        cmd.Parameters.Add(new SqlParameter("@Message",                           SqlDbType.NVarChar)                        {                            Value = Mailcontent                        });                        cmd.Parameters.Add(new SqlParameter("@module",                        SqlDbType.VarChar)                        {                            Value = modulename[0]                        });                        cmd.Parameters.Add(new SqlParameter("@MI_Id",                       SqlDbType.BigInt)                        {                            Value = MI_Id                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        try                        {                            using (var dataReader = cmd.ExecuteReader())                            {                            }                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return "success";        }



    }
}



