using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.Rating;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IssueManager.com.Tenants.Implimentation
{
    public class TenantMasterNocChecklistImpl : Interface.TenantMasterNocChecklistInterface
    {
        public IssueManagerContext _context;


        public TenantMasterNocChecklistImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }
        //Master Type
        public TenantMasterNocChecklistDTO getloaddata(TenantMasterNocChecklistDTO data)
        {
            try
            {
                data.get_tenant = _context.TenantAgreement.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.get_amenities = _context.HR_Master_Amenities_DMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.staffmappinglist = (from a in _context.TenantAgreement
                                         from b in _context.HR_Master_Amenities_DMO
                                         from c in _context.TenantMasterNocChecklistDMO
                                         where (a.MI_Id == b.MI_Id && b.HRMAM_Id == c.HRMAM_Id && a.TENANTAGR_Id == c.TENANTAGR_Id)
                                         select new TenantMasterNocChecklistDTO
                                         {
                                             name = a.TENANTAGR_TenantName,
                                             area = b.HRMAM_AmenitiesName,
                                             remarks = c.TENANTNOCCL_Remarks,
                                             date = c.TENANTNOCCL_CheckedDate,
                                             damount = c.TENANTNOCCL_DeductingAmount,
                                             TENANTNOCCL_Id = c.TENANTNOCCL_Id,
                                             TENANTAGR_Id = a.TENANTAGR_Id,
                                             HRMAM_Id = b.HRMAM_Id,
                                             TENANTNOCCL_WorkingFlg = c.TENANTNOCCL_WorkingFlg,
                                             TENANTNOCCL_ActiveFlag = c.TENANTNOCCL_ActiveFlag,
                                             TENANTNOCCL_FilePath = c.TENANTNOCCL_FilePath,
                                             TENANTNOCCL_FileName = c.TENANTNOCCL_FileName

                                         }).Distinct().ToArray();



            }

            catch (Exception ex)
            {
                //db.LogInformation("Tax load Page:" + ex.Message);
            }
            return data;
        }

        public TenantMasterNocChecklistDTO getamenity(TenantMasterNocChecklistDTO data)
        {
            try
            {
                var get_amenity = _context.TenantAgreement.Where(a => a.TENANTAGR_Id == data.TENANTAGR_Id && a.MI_Id == data.MI_Id).ToList();

                data.get_amenities = (from A in _context.HR_Master_Amenities_DMO
                                      from B in _context.HR_Master_Room_AmenitiesDMO
                                      where (A.HRMAM_Id == B.HRMAM_Id && B.HRMRAM_ActiveFlag == true && A.MI_Id == data.MI_Id && B.HRMR_Id == get_amenity[0].HRMR_Id)

                                      select new TenantMasterNocChecklistDTO
                                      {
                                          HRMAM_Id = A.HRMAM_Id,
                                          HRMAM_AmenitiesName = A.HRMAM_AmenitiesName

                                      }).ToArray();
                //data.get_amenity = _context.HR_Master_Amenities_DMO.Where(a => a.HRMAM_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMAM_Id == data.HRMAM_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TenantMasterNocChecklistDTO savedetails(TenantMasterNocChecklistDTO data)
        {
            try
            {
                //var template = _context.TenantMasterNocChecklistDMO.ToList();

                //var Mailcontent = template.FirstOrDefault().HRME_Id;
                //var IDDD = template.FirstOrDefault().TENANTAGR_Id;
                data.HRME_Id = _context.Staff_User_Login.Where(R => R.Id == data.User_Id).Select(P => P.Emp_Code).FirstOrDefault();

                if (data.TENANTNOCCL_Id != 0)
                {
                    var dublicatecheck = _context.TenantMasterNocChecklistDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTNOCCL_WorkingFlg == data.TENANTNOCCL_WorkingFlg && t.TENANTNOCCL_CheckedDate == data.TENANTNOCCL_CheckedDate && t.TENANTNOCCL_Remarks == data.TENANTNOCCL_Remarks && t.TENANTNOCCL_DeductingAmount == data.TENANTNOCCL_DeductingAmount).ToList();
                    if (dublicatecheck.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }

                    else
                    {
                        var result = _context.TenantMasterNocChecklistDMO.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).FirstOrDefault();
                        //result.TENANTAGR_Id = data.TENANTAGR_Id;

                        result.TENANTAGR_Id = data.TENANTAGR_Id;
                        result.TENANTNOCCL_DeductingAmount = data.TENANTNOCCL_DeductingAmount;

                        result.TENANTNOCCL_CheckedDate = data.TENANTNOCCL_CheckedDate;
                        result.TENANTNOCCL_Remarks = data.TENANTNOCCL_Remarks;
                        //   result.TENANTAGR_Id = IDDD;
                        result.HRME_Id = data.HRME_Id;
                        result.TENANTNOCCL_CreatedDate = DateTime.Now;
                        result.TENANTNOCCL_UpdatedDate = DateTime.Now;
                        result.TENANTNOCCL_CreatedBy = data.User_Id;
                        result.TENANTNOCCL_UpdatedBy = data.User_Id;
                        result.TENANTNOCCL_FileName = data.TENANTNOCCL_FileName;
                        result.TENANTNOCCL_FilePath = data.TENANTNOCCL_FilePath;

                        _context.Update(result);

                        var contactExists = _context.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnduplicatestatus = "Update";
                        }
                        else
                        {
                            data.returnduplicatestatus = "Not Update";
                        }
                    }
                }
                else
                {
                    var res = _context.TenantMasterNocChecklistDMO.Where(t => (t.TENANTNOCCL_CheckedDate == data.TENANTNOCCL_CheckedDate && t.TENANTNOCCL_Id == data.TENANTNOCCL_Id && t.TENANTNOCCL_Remarks == data.TENANTNOCCL_Remarks)).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        TenantMasterNocChecklistDMO area = new TenantMasterNocChecklistDMO();

                        // area.HRME_Id = 1;
                        //area.TENANTNOCCL_Id = data.TENANTNOCCL_Id;
                        area.TENANTNOCCL_DeductingAmount = data.TENANTNOCCL_DeductingAmount;
                        area.TENANTAGR_Id = data.TENANTAGR_Id;
                        area.TENANTNOCCL_Remarks = data.TENANTNOCCL_Remarks;
                        area.TENANTNOCCL_WorkingFlg = true;
                        area.TENANTNOCCL_ActiveFlag = true;
                        area.HRME_Id = data.HRME_Id;
                        area.HRMAM_Id = data.HRMAM_Id;
                        area.TENANTNOCCL_CreatedDate = DateTime.Now;
                        area.TENANTNOCCL_UpdatedDate = DateTime.Now;
                        area.TENANTNOCCL_CreatedBy = data.User_Id;
                        area.TENANTNOCCL_UpdatedBy = data.User_Id;
                        area.TENANTNOCCL_CheckedDate = DateTime.Now;
                        area.TENANTNOCCL_FileName = data.TENANTNOCCL_FileName;
                        area.TENANTNOCCL_FilePath = data.TENANTNOCCL_FilePath;
                        _context.Update(area);

                        var contactExists = _context.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnduplicatestatus = "Saved";
                        }
                        else
                        {
                            data.returnduplicatestatus = "Not Saved";
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

        public TenantMasterNocChecklistDTO activedeactive(TenantMasterNocChecklistDTO dto)        {            try            {                var result = _context.TenantMasterNocChecklistDMO.Single(t => t.TENANTNOCCL_Id == dto.TENANTNOCCL_Id);                if (result.TENANTNOCCL_ActiveFlag == true)                {                    result.TENANTNOCCL_ActiveFlag = false;                }                else if (result.TENANTNOCCL_ActiveFlag == false)                {                    result.TENANTNOCCL_ActiveFlag = true;                }                result.TENANTNOCCL_UpdatedDate = DateTime.Now;                _context.Update(result);                int returnval = _context.SaveChanges();                if (returnval > 0)                {                    dto.returnval = true;                }                else                {                    dto.returnval = false;                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return dto;        }
    }
}