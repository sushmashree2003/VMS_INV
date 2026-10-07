using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using IssueManager.com.vaps.Interfaces;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_ClientProject_MappingIMPL : Interfaces.ISM_ClientProject_MappingInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;
        public ISM_ClientProject_MappingIMPL(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;
        }
        //====================================loaddata ===========================================
        public ISM_ClientProject_MappingDTO loaddata(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                data.client_list = _IssueManagercontext.clientTable.Where(t => t.MI_Id == data.MI_Id && t.ISMMCLT_ActiveFlag == true).Distinct().ToArray();

                data.project_list = _IssueManagercontext.MastersProject_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMPR_ActiveFlg == true).Distinct().ToArray();

                data.get_clentlist = (from a in _IssueManagercontext.clientTable
                                      from b in _IssueManagercontext.MastersProject_DMO
                                      from c in _IssueManagercontext.MastersModule_DMO
                                      from d in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                      from e in _IssueManagercontext.MasterModules
                                      where (a.ISMMCLT_Id == d.ISMMCLT_Id && b.ISMMPR_Id == d.ISMMPR_Id && c.ISMMMD_Id == d.ISMMMD_Id && c.IVRMM_Id == e.IVRMM_Id && a.MI_Id == data.MI_Id)
                                      select new ISM_ClientProject_MappingDTO
                                      {
                                          ISMMCLTPR_Id = d.ISMMCLTPR_Id,
                                          ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                          ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                          IVRMM_ModuleName = e.IVRMM_ModuleName,
                                          ISMMCLT_Id = d.ISMMCLT_Id,
                                          ISMMPR_Id = d.ISMMPR_Id,
                                          ISMMMD_Id = d.ISMMMD_Id,
                                          ISMMCLTPR_ActiveFlag = d.ISMMCLTPR_ActiveFlag,
                                      }).Distinct().OrderByDescending(t => t.ISMMCLTPR_Id).ToArray();

                //data.get_department = (from a in _IssueManagercontext.HR_Master_Department
                //                       from b in _IssueManagercontext.MasterEmployee
                //                       where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true && b.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id)
                //                       select new ISM_TaskCreationDTO
                //                       {
                //                           HRMD_Id = a.HRMD_Id,
                //                           HRMD_DepartmentName = a.HRMD_DepartmentName
                //                       }).Distinct().ToArray();

                data.get_department = (from a in _IssueManagercontext.HR_Master_Department
                                       where (a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                       select new ISM_TaskCreationDTO
                                       {
                                           HRMD_Id = a.HRMD_Id,
                                           HRMD_DepartmentName = a.HRMD_DepartmentName
                                       }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //==============================================project list======================================//
        public ISM_ClientProject_MappingDTO getproject(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                data.project_list = (from a in _IssueManagercontext.clientTable
                                     from b in _IssueManagercontext.MastersProject_DMO
                                     from c in _IssueManagercontext.MastersModule_DMO
                                     where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.ISMMCLT_ActiveFlag == true && b.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == c.ISMMPR_Id && c.HRMD_Id == data.HRMD_Id)
                                     select new ISM_ClientProject_MappingDTO
                                     {
                                         ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                         ISMMPR_Id = b.ISMMPR_Id
                                     }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //=================================module==================================================
        public ISM_ClientProject_MappingDTO getmodule(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                data.module_list = (from a in _IssueManagercontext.MasterModules
                                    from b in _IssueManagercontext.MastersModule_DMO
                                    from c in _IssueManagercontext.MastersProject_DMO
                                    where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMPR_Id == c.ISMMPR_Id && b.MI_Id == c.MI_Id
                                    && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == data.ISMMPR_Id && b.HRMD_Id == data.HRMD_Id)
                                    select new ISM_ClientProject_MappingDTO
                                    {
                                        IVRMM_Id = b.IVRMM_Id,
                                        ISMMMD_Id = b.ISMMMD_Id,
                                        IVRMM_ModuleName = a.IVRMM_ModuleName,
                                    }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //==========================================save data=======================================
        public ISM_ClientProject_MappingDTO savedata(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                if (data.ISMMCLTPR_Id == 0)
                {
                    for (int i = 0; i < data.moduleidss.Length; i++)
                    {

                        var tempdata = data.moduleidss[i].ISMMMD_Id;
                        {
                            //var duplicate = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id != 0 && t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.ISMMCLTPR_MOURefNo==data.ISMMCLTPR_MOURefNo && t.ISMMCLTPR_ProposalRefNo==data.ISMMCLTPR_ProposalRefNo && t.ISMMMD_Id==tempdata && t.ISMMCLTPR_NodalOfficerName==data.ISMMCLTPR_NodalOfficerName).ToArray();
                            var duplicate = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id != 0 && t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.ISMMMD_Id == tempdata).ToArray();

                            if (duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                ISM_Master_Client_ProjectDMO rrr = new ISM_Master_Client_ProjectDMO();
                                rrr.ISMMCLT_Id = data.ISMMCLT_Id;
                                rrr.ISMMPR_Id = data.ISMMPR_Id;
                                rrr.ISMMCLTPR_Id = data.ISMMCLTPR_Id;
                                rrr.ISMMMD_Id = tempdata;
                                rrr.ISMMCLTPR_ProposalRefNo = data.ISMMCLTPR_ProposalRefNo;
                                rrr.ISMMCLTPR_ProposalSentDate = data.ISMMCLTPR_ProposalSentDate;
                                rrr.ISMMCLTPR_DealClosureDate = data.ISMMCLTPR_DealClosureDate;
                                rrr.ISMMCLTPR_MOURefNo = data.ISMMCLTPR_MOURefNo;
                                rrr.ISMMCLTPR_MOUDate = data.ISMMCLTPR_MOUDate;
                                rrr.ISMMCLTPR_MOURepresentedBy = data.ISMMCLTPR_MOURepresentedBy;
                                rrr.ISMMCLTPR_MOUStartDate = data.ISMMCLTPR_MOUStartDate;
                                rrr.ISMMCLTPR_MOUEndDate = data.ISMMCLTPR_MOUEndDate;
                                rrr.ISMMCLTPR_NodalOfficerName = data.ISMMCLTPR_NodalOfficerName;
                                rrr.ISMMCLTPR_NodalOfficerContactNo = data.ISMMCLTPR_NodalOfficerContactNo;
                                rrr.ISMMCLTPR_NodalOfficerEmailId = data.ISMMCLTPR_NodalOfficerEmailId;
                                rrr.ISMMCLTPR_ProjectDuration = data.ISMMCLTPR_ProjectDuration;
                                rrr.ISMMCLTPR_TotalStudent = data.ISMMCLTPR_TotalStudent;
                                rrr.ISMMCLTPR_CostPerStudent = data.ISMMCLTPR_CostPerStudent;
                                rrr.ISMMCLTPR_EnhancementPerYr = data.ISMMCLTPR_EnhancementPerYr;
                                rrr.CreatedDate = DateTime.Now;
                                rrr.UpdatedDate = DateTime.Now;
                                rrr.ISMMCLTPR_ActiveFlag = true;
                                rrr.ISMMCLTPR_CreatedBy = data.UserId;
                                rrr.ISMMCLTPR_UpdatedBy = data.UserId;
                                rrr.ISMMCLTPR_WorkOrder = data.ISMMCLTPR_WorkOrder;
                                _IssueManagercontext.Add(rrr);
                                int y = _IssueManagercontext.SaveChanges();
                                if (y > 0)
                                {
                                    data.msg = "saved";
                                }
                                else
                                {
                                    data.msg = "Failed";
                                }
                            }
                        }
                    }
                }
                else if (data.ISMMCLTPR_Id > 0)
                {
                    for (int i = 0; i < data.moduleidss.Length; i++)
                    {
                        var tempdata = data.moduleidss[i].ISMMMD_Id;
                        {
                            var duplicate = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.ISMMMD_Id == tempdata && t.ISMMCLTPR_Id != data.ISMMCLTPR_Id).ToArray();
                            if (duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                var yy = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id == data.ISMMCLTPR_Id).SingleOrDefault();
                                yy.ISMMCLTPR_CreatedBy = data.UserId;
                                yy.ISMMCLTPR_UpdatedBy = data.UserId;
                                yy.ISMMCLT_Id = data.ISMMCLT_Id;
                                yy.ISMMPR_Id = data.ISMMPR_Id;
                                yy.ISMMMD_Id = tempdata;
                                yy.CreatedDate = DateTime.Now;
                                yy.UpdatedDate = DateTime.Now;
                                yy.ISMMCLTPR_Id = data.ISMMCLTPR_Id;
                                yy.ISMMCLTPR_ProposalRefNo = data.ISMMCLTPR_ProposalRefNo;
                                yy.ISMMCLTPR_ProposalSentDate = data.ISMMCLTPR_ProposalSentDate;
                                yy.ISMMCLTPR_DealClosureDate = data.ISMMCLTPR_DealClosureDate;
                                yy.ISMMCLTPR_MOURefNo = data.ISMMCLTPR_MOURefNo;
                                yy.ISMMCLTPR_MOUDate = data.ISMMCLTPR_MOUDate;
                                yy.ISMMCLTPR_MOURepresentedBy = data.ISMMCLTPR_MOURepresentedBy;
                                yy.ISMMCLTPR_MOUStartDate = data.ISMMCLTPR_MOUStartDate;
                                yy.ISMMCLTPR_MOUEndDate = data.ISMMCLTPR_MOUEndDate;
                                yy.ISMMCLTPR_NodalOfficerName = data.ISMMCLTPR_NodalOfficerName;
                                yy.ISMMCLTPR_NodalOfficerContactNo = data.ISMMCLTPR_NodalOfficerContactNo;
                                yy.ISMMCLTPR_NodalOfficerEmailId = data.ISMMCLTPR_NodalOfficerEmailId;
                                yy.ISMMCLTPR_ProjectDuration = data.ISMMCLTPR_ProjectDuration;
                                yy.ISMMCLTPR_TotalStudent = data.ISMMCLTPR_TotalStudent;
                                yy.ISMMCLTPR_CostPerStudent = data.ISMMCLTPR_CostPerStudent;
                                yy.ISMMCLTPR_EnhancementPerYr = data.ISMMCLTPR_EnhancementPerYr;
                                yy.ISMMCLTPR_WorkOrder = data.ISMMCLTPR_WorkOrder;
                                _IssueManagercontext.Update(yy);
                                int r = _IssueManagercontext.SaveChanges();
                                if (r > 0)
                                {
                                    data.msg = "updated";
                                }
                                else
                                {
                                    data.msg = "failed";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        //==========================================edit data======================================
        public ISM_ClientProject_MappingDTO Editdata(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                var editlist = (from a in _IssueManagercontext.MasterModules
                                from b in _IssueManagercontext.MastersModule_DMO
                                from c in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                                where (c.ISMMCLTPR_Id == data.ISMMCLTPR_Id && a.IVRMM_Id == b.IVRMM_Id && b.ISMMMD_Id == c.ISMMMD_Id)
                                select new ISM_ClientProject_MappingDTO
                                {
                                    ISMMCLTPR_Id = c.ISMMCLTPR_Id,
                                    ISMMCLT_Id = c.ISMMCLT_Id,
                                    ISMMPR_Id = c.ISMMPR_Id,
                                    IVRMM_Id = b.IVRMM_Id,
                                    ISMMMD_Id = b.ISMMMD_Id,
                                    ISMMCLTPR_ProposalRefNo = c.ISMMCLTPR_ProposalRefNo,
                                    ISMMCLTPR_ProposalSentDate = c.ISMMCLTPR_ProposalSentDate,
                                    ISMMCLTPR_DealClosureDate = c.ISMMCLTPR_DealClosureDate,
                                    ISMMCLTPR_MOURefNo = c.ISMMCLTPR_MOURefNo,
                                    ISMMCLTPR_MOUDate = c.ISMMCLTPR_MOUDate,
                                    ISMMCLTPR_MOURepresentedBy = c.ISMMCLTPR_MOURepresentedBy,
                                    ISMMCLTPR_MOUStartDate = c.ISMMCLTPR_MOUStartDate,
                                    ISMMCLTPR_MOUEndDate = c.ISMMCLTPR_MOUEndDate,
                                    ISMMCLTPR_NodalOfficerName = c.ISMMCLTPR_NodalOfficerName,
                                    ISMMCLTPR_NodalOfficerContactNo = c.ISMMCLTPR_NodalOfficerContactNo,
                                    ISMMCLTPR_NodalOfficerEmailId = c.ISMMCLTPR_NodalOfficerEmailId,
                                    ISMMCLTPR_ProjectDuration = c.ISMMCLTPR_ProjectDuration,
                                    ISMMCLTPR_TotalStudent = c.ISMMCLTPR_TotalStudent,
                                    ISMMCLTPR_CostPerStudent = c.ISMMCLTPR_CostPerStudent,
                                    ISMMCLTPR_EnhancementPerYr = c.ISMMCLTPR_EnhancementPerYr,
                                    HRMD_Id = b.HRMD_Id,
                                    ISMMCLTPR_WorkOrder = c.ISMMCLTPR_WorkOrder

                                }).Distinct().ToList();

                data.editlist = editlist.ToArray();
                if (editlist[0].ISMMMD_Id != 0)
                {
                    var ee = (from a in _IssueManagercontext.ISM_Master_Client_ProjectDMO
                              from b in _IssueManagercontext.MasterModules
                              where (a.ISMMCLTPR_Id == editlist[0].ISMMCLTPR_Id && b.IVRMM_Id == editlist[0].IVRMM_Id && a.ISMMMD_Id == editlist[0].ISMMMD_Id)
                              select new ISM_ClientProject_MappingDTO
                              {
                                  ISMMPR_Id = a.ISMMPR_Id,
                                  ISMMCLT_Id = a.ISMMCLT_Id,
                                  IVRMM_Id = b.IVRMM_Id,
                                  ISMMMD_Id = a.ISMMMD_Id

                              }).Distinct().ToList();
                    data.ISMMCLT_Id = ee[0].ISMMCLT_Id;
                    data.ISMMPR_Id = ee[0].ISMMPR_Id;
                    data.ISMMMD_Id = ee[0].ISMMMD_Id;
                    data.IVRMM_Id = ee[0].IVRMM_Id;
                    data.HRMD_Id = editlist[0].HRMD_Id;
                    data.client_list = (from a in _IssueManagercontext.clientTable
                                        where (a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                        select new ISM_ClientProject_MappingDTO
                                        {
                                            ISMMCLT_Id = a.ISMMCLT_Id,
                                            ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                        }
                                       ).Distinct().ToArray();

                    data.project_list = (from a in _IssueManagercontext.clientTable
                                         from b in _IssueManagercontext.MastersProject_DMO
                                         from c in _IssueManagercontext.MastersModule_DMO
                                         where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.ISMMCLT_ActiveFlag == true && b.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == c.ISMMPR_Id && c.HRMD_Id == data.HRMD_Id)
                                         select new ISM_ClientProject_MappingDTO
                                         {
                                             ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                             ISMMPR_Id = b.ISMMPR_Id
                                         }).Distinct().ToArray();

                    data.module_list = (from a in _IssueManagercontext.MasterModules
                                        from b in _IssueManagercontext.MastersModule_DMO
                                        from c in _IssueManagercontext.MastersProject_DMO
                                        where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMPR_Id == c.ISMMPR_Id && b.MI_Id == c.MI_Id
                                        && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == data.ISMMPR_Id && b.HRMD_Id == data.HRMD_Id)
                                        select new ISM_ClientProject_MappingDTO
                                        {
                                            IVRMM_Id = b.IVRMM_Id,
                                            ISMMMD_Id = b.ISMMMD_Id,
                                            IVRMM_ModuleName = a.IVRMM_ModuleName,
                                        }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();
                    data.ISMMCLT_Id = ee[0].ISMMCLT_Id;
                    data.ISMMPR_Id = ee[0].ISMMPR_Id;
                    data.ISMMMD_Id = ee[0].ISMMMD_Id;
                    data.IVRMM_Id = ee[0].IVRMM_Id;
                    data.HRMD_Id = ee[0].HRMD_Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        //==========================================Deactive data=====================================
        public ISM_ClientProject_MappingDTO clientDecative(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                var u = _IssueManagercontext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id == data.ISMMCLTPR_Id).SingleOrDefault();
                if (u.ISMMCLTPR_ActiveFlag == true)
                {
                    u.ISMMCLTPR_ActiveFlag = false;
                }
                else if (u.ISMMCLTPR_ActiveFlag == false)
                {
                    u.ISMMCLTPR_ActiveFlag = true;
                }

                _IssueManagercontext.Update(u);
                int o = _IssueManagercontext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        //savedataLead
        public ISM_ClientProject_MappingDTO savedataLead(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                var result = _IssueManagercontext.Database.ExecuteSqlCommand
                   ("usp_I_Post_MasterClient" +
                   " @p0," +
                   "@p1," +
                   "@p2," +
                   "@p3," +
                   "@p4," +
                   "@p5," +
                   "@p6," +
                   "@p7," +
                   "@p8," +
                   "@p9," +
                   "@p10," +
                   "@p11," +
                   "@p12," +
                   "@p13," +
                   "@p14," +
                   "@p15," +
                   "@p16," +
                   "@p17," +
                   "@p18," +
                   "@p19," +
                   "@p20," +
                   "@p21," +
                   "@p22," +
                   "@p23," +
                   "@p24," +
                   "@p25," +
                   "@p26," +
                   "@p27," +
                   "@p28", +
                   data.MI_Id,
                   data.ISMSLE_Id,
                   data.ISMMCLTPR_NodalOfficerName,
                   data.ISMMCLTPR_NodalOfficerEmailId,
                   data.ISMMCLTPR_NodalOfficerContactNo,
                   data.ISMMCLTPR_WorkOrder,
                   data.ISMMCLTPR_MOURefNo,
                   data.ISMMCLTPR_MOUDate,
                   data.UserId,
                   data.ISMMCLTPR_ProposalRefNo,
                   data.ISMMCLTPR_ProposalSentDate,
                   data.ISMMCLTPR_DealClosureDate,
                   data.ISMMCLTPR_MOUDate,
                   data.ISMMCLTPR_MOURepresentedBy,
                   data.ISMMCLTPR_MOUStartDate,
                   data.ISMMCLTPR_MOUEndDate,
                   data.ISMMCLTPR_TotalStudent,
                   data.ISMMCLTPR_CostPerStudent,
                   data.ISMMCLTPR_EnhancementPerYr,
                   data.ISMMCLTPR_WorkOrder,
                   data.ISMSLET_Id,
                   data.Template,
                   data.Contract_Year_From,
                   data.ISMMCLTPR_IEcharge,
                   data.ISMMCLTPR_OperatorCost,
                   data.Contract_Year_To,
                   data.ISMMCLTPR_Percentage,
                   data.ISMMCLT_Host,
                   data.ISMMCLTPR_CostPerYearStudent

                   );

                if (result > 0)
                {
                    data.msg = "saved";
                }
                else
                {
                    data.msg = "Failed";
                }


            }

            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }


        //public ISM_ClientProject_MappingDTO SaveApprovalVerify(ISM_ClientProject_MappingDTO data)
        //{
        //    try
        //    {
        //        foreach (var sales in data.arrayLead)
        //        {
        //            var contactExistsP = _IssueManagercontext.Database.ExecuteSqlCommand("usp_I_OrderConfirming @p0,@p1,@p2,@p3", data.arrayLead[0].ISMMCLT_Id, data.MI_Id, data.OrderConfirm_Flg, data.UserId);
        //            if (contactExistsP > 0)
        //            {
        //                data.returnval = true;
        //            }
        //            else
        //            {
        //                data.returnval = false;
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return data;
        //}

        //public ISM_ClientProject_MappingDTO SaveApprovalVerify(ISM_ClientProject_MappingDTO data)
        public ISM_ClientProject_MappingDTO SaveApprovalVerify(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                if (data.arrayLead != null && data.arrayLead.Length > 0)
                {
                    foreach (var d in data.arrayLead)
                    {
                        var result = _IssueManagercontext.Database.ExecuteSqlCommand("usp_I_OrderConfirming @p0,@p1,@p2,@p3,@p4,@p5",
                        d.ISMMCLT_Id, data.MI_Id, d.OrderConfirm_Flg, data.UserId, d.ISMCL_ID, d.ISMOC__Remark);

                    }
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public ISM_ClientProject_MappingDTO SaveOrderAcceptanceApproval(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                if (data.arrayLead != null && data.arrayLead.Length > 0)
                {
                    foreach (var d in data.arrayLead)
                    {
                        var result = _IssueManagercontext.Database.ExecuteSqlCommand("usp_I_OrderApproval @p0,@p1,@p2,@p3,@p4,@p5",
                        d.ISMMCLT_Id,
                        data.MI_Id,
                        d.ISMOAA_ApprovalFlag,
                        data.UserId,
                        d.ISMOC__Remark,
                        d.ISMOAA_Manpower);

                    }
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        //addded by praveenkumar 
        public ISM_ClientProject_MappingDTO ViewMOUCheckUpFiles(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.getviewfilesdetails = (from a in _IssueManagercontext.ISM_Client_Project_Docs_DMO_con
                                            where (a.ISMMCLTPR_Id == data.ISMMCLTPR_Id)
                                            select new ISM_ClientProject_MappingDTO
                                            {
                                                ISMMCLTPR_Id = a.ISMMCLTPR_Id,
                                                ISMCLTPRDOC_Id = a.ISMCLTPRDOC_Id,
                                                filename = a.ISMCLTPRDOC_FileName,
                                                filepath = a.ISMCLTPRDOC_FilePath
                                            }).Distinct().ToArray();


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ISM_ClientProject_MappingDTO viewtemplatedetails(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_EndUserList_SignOff";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                    {
                        Value = data.ISMMCLT_Id
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
                        data.EnduserDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (data.msg == "FetchTemp")
                {
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_MOU_TEMPLATES";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TEMPLATENAME", SqlDbType.VarChar)
                        {
                            Value = data.pending
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
                            data.templatelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "IP"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.closing_letter = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "SMS"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Sms_Quantity = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "OFFT"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Offt_letter = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "ONT"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Onnt_letter = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "IC"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Icc_letter = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "H"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Mou_details = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "P"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Patch_release = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "cu"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Custamization = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "IHC"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.It_HealthCheckup = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "MYR"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Month_yearreprot = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "MP"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Man_power = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "FS"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Feedback_System = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "SR"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Service_System = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "PAM"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Payment = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "Com"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Components_docs = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "Custom"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Custamization_report = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "HS"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.client_list = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "Att"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Attendancelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "InvDoc"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Invoicedata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "CAF"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Telecallerlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_CLosingLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "PATCH"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.patchlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    // For welcome letter template


                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "H"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.Welcomebasicdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "IC"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeImplementation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "IP"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeNonImplementation = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "P"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomePatchrelease = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "IHC"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeHealthCheckup = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "MP"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeManPower = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "PAM"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeAmount = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "SMS"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeSms = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "OFFT"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeOfflinetraining = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "ONT"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeOnlinetraining = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {   
                            Console.WriteLine(ex.Message);
                        }
                    }


                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_WelcomeLetter";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMMCLT_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                          SqlDbType.VarChar)
                        {
                            Value = "IE"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                        {
                            Value = data.Ayear
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
                            data.WelcomeIENAME = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }



                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_MOU_TEMPLATES_Proposal";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TEMPLATENAME", SqlDbType.VarChar)
                        {
                            Value = data.pending
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
                            data.get_projectdetail = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }




                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Sales_MOU_product_New";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMSBOM_Id
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
                            data.Momproductpendinglist = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    //added by praveenkumar



                    //added by praveenkumar NDA
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_Sales_MOU_product_NDA";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMSBOM_Id
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
                            data.Momproductpendinglist1 = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }


                    // IVRM PRODUCT LIST ADDED BY PRAVEENKUMAR
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "usp_s_LeadModuleList_New";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMSLE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMSBOM_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                          SqlDbType.VarChar)
                        {
                            Value = "M"
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
                            data.ModuleName = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }


                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ISM_leadwise_Project_Payment";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                        {
                            Value = data.ISMSLE_Id
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
                            data.leadprojectpayment = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public ISM_ClientProject_MappingDTO OnChangeOnChangeBOMList(ISM_ClientProject_MappingDTO dto)
        {
            try
            {
                var BOM_ids = "0";
                if (dto.BOMListt.Length > 0)
                {
                    foreach (var item in dto.BOMListt)
                    {
                        BOM_ids = BOM_ids + "," + item;
                    }

                }

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Sales_Product_package_detail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
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
                        dto.BOMTemplateList = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Sales_Product_package_detail_perstudent";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMSLE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMSBOM_Id", SqlDbType.VarChar)
                    {
                        Value = BOM_ids
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
                        dto.Sales_CostPerMonth = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }

            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return dto;
        }



        public async Task<ISM_ClientProject_MappingDTO> sendMail(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                //added  //ISES_Template_Name == "CleintInvoiceMail" //CleintInvoiceMail //MOU_Postsales
                whatsapp whtsappnew = new whatsapp(_db);
                string fileid = ""; string filename = "Order Acceptance";
                Dictionary<string, string> val = new Dictionary<string, string>();//MOU_Postsales
                var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "MOU_Postsales" && e.ISES_MailActiveFlag == true).ToList();
                //  var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "CleintInvoiceMail" && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return data;
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = data.msg;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    //  string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    var message = new SendGridMessage();
                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                    {
                        string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                        foreach (var c in ccmail)
                        {
                            if (c != "")
                            {
                                //  message.AddBcc(c);

                            }
                        }

                    }

                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] ccmail = template[0].ISES_MailCCId.Split(',');
                        foreach (var c in ccmail)
                        {
                            if (c != "")
                            {
                                message.AddCc(c);

                            }
                        }

                    }
                    //ISES_MailBCCId
                    StringBuilder sb = new StringBuilder(data.msg);
                    StringReader sr = new StringReader(sb.ToString());
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        byte[] bytess = memoryStream.ToArray();
                        var file = Convert.ToBase64String(bytess);
                        byte[] fileBytes = Convert.FromBase64String(file);
                        string emp;
                        emp = Convert.ToString(sr);
                        string c = "";
                        string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");

                        memoryStream.Close();

                        var datavalues = file.Substring(file.IndexOf(',') + 1);
                        var byteArray = Convert.FromBase64String(datavalues);
                        using (var stream = new MemoryStream(byteArray))
                        {
                            IFormFile formFile = new FormFile(stream, 0, byteArray.Length, "file", "Proposal.pdf")
                            {
                                Headers = new HeaderDictionary(),
                                ContentType = "application/pdf"
                            };
                            string accountname = "";
                            string accesskey = "";
                            var azure = _db.IVRM_Storage_path_Details.ToList();
                            if (azure.Count() > 0)
                            {
                                accountname = azure.FirstOrDefault().IVRM_SD_Access_Name;
                                accesskey = azure.FirstOrDefault().IVRM_SD_Access_Key;
                            }
                            var names = new List<string>();
                            var contentTypes = new List<string>();
                            fileid = await whtsappnew.uploadtoazure(formFile, "presales", "salesQutations", data.MI_Id, accountname, accesskey);

                        }
                    }
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = "Order Acceptance Document for Your Review";

                    //string emailid = "sanjeev@globalqtytrainig.com";
                    //string emailid = "praveenkumar.b@vapstech.com";
                    message.AddTo(data.ISMMCLTPR_NodalOfficerEmailId);



                    if (data.columnids != null && data.columnids.Length > 0)
                    {
                        foreach (var item in data.columnids)
                        {
                            if (item.coulmn != null && item.coulmn != "")
                            {
                                var webClient = new WebClient();
                                byte[] imageBytes = webClient.DownloadData(item.coulmn);
                                string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                message.AddAttachment(item.coulmn, fileContentsAsBase64, null, null, null);
                            }
                        }
                    }


                    //
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();//data.ISMMCLTPR_NodalOfficerContactNo
                    string ss = await whtsappnew.OrderQutation(data.MI_Id, data.ISMMCLTPR_NodalOfficerContactNo.ToString(), "vmsorderacceptance", filename, fileid, "Order Acceptance", institutionName[0].MI_Name);
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "CleintInvoiceMail" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = data.ISMMCLTPR_NodalOfficerEmailId
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
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

        public ISM_ClientProject_MappingDTO releaseversion(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_CLosingLetter";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                    {
                        Value = data.ISMMCLT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                    {
                        Value = "VD"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Ayear", SqlDbType.BigInt)
                    {
                        Value = data.IVRMVT_ID
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
                        data.versionreleasedata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


    }
}
//==========================================================================================


