using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Inventory;
using InventoryServicesHub.com.vaps.Master.Interface;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Inventory;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Implementation
{
    public class INV_VendorSignOffImpl : INV_VendorSignOffInterface
    {
        public InventoryContext _context;

        public INV_VendorSignOffImpl(InventoryContext context)
        {
            _context = context;
        }

        public async Task<INV_VendorSignOffDTO> getloaddata(INV_VendorSignOffDTO dto)
        {
            try
            {
                // Fetch active vendors dropdown list
                var vendorList = await (
                                from a in _context.INV_MasterVendorEntryDMO
                                join b in _context.INV_VendorApprovalStatusDMO
                                    on a.MVE_Id equals b.MVE_Id
                                where a.MI_Id == dto.MI_Id
                                      && a.MVE_Activeflag == true
                                      && b.ApprovalStatus == "Approved"
                                      && !_context.INV_Master_Vendor_SignOffDMO
                                            .Any(x => x.MVE_Id == a.MVE_Id && x.MI_ID == dto.MI_Id)
                                orderby a.MVE_VendorName
                                select new INV_VendorSignOffDTO
                                {
                                    MVE_Id = a.MVE_Id,
                                    MVE_VendorName = a.MVE_VendorName,
                                    MVE_CompanyName = a.MVE_CompanyName,
                                    MVE_Email_Address = a.MVE_Email_Address
                                }
                            ).Distinct().ToArrayAsync();

                // Fetch Vendor Sign Off grid data
                var signOffData = await (from a in _context.INV_Master_Vendor_SignOffDMO
                                         join b in _context.INV_MasterVendorEntryDMO
                                                  on a.MVE_Id equals b.MVE_Id
                                         where a.MI_ID == dto.MI_Id
                                         select new INV_VendorSignOffDTO
                                         {
                                             INVMSO_Id = a.INVMSO_Id,
                                             MI_Id = a.MI_ID,
                                             MVE_Id = a.MVE_Id,
                                             MVE_VendorName = b.MVE_VendorName,
                                             MVE_CompanyName = b.MVE_CompanyName,
                                             MVE_Email_Address = b.MVE_Email_Address,
                                             INVMSO_SignOffDate = a.INVMSO_SignOffDate,
                                             INVMSO_MOU_FilePath = a.INVMSO_MOU_FilePath,
                                             INVMSO_WorkOrder_FilePath = a.INVMSO_WorkOrder_FilePath,
                                             INVMSO_ActiveFlg = a.INVMSO_ActiveFlg,
                                             INVMSO_CreatedBy = a.INVMSO_CreatedBy,
                                             INVMSO_UpdatedBy = a.INVMSO_UpdatedBy
                                         }).OrderByDescending(a => a.INVMSO_Id).ToArrayAsync();

                dto.vendorList = vendorList;
                dto.VendroNameDropdownArray = vendorList;
                dto.signOffList = signOffData;
                dto.VendrosignoffGridArray = signOffData;

                if ((vendorList != null && vendorList.Length > 0) || (signOffData != null && signOffData.Length > 0))
                {
                    dto.returnval = true;
                    dto.saveorupdate = "Data fetched successfully";
                }
                else
                {
                    dto.returnval = false;
                    dto.saveorupdate = "No records found";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public async Task<INV_VendorSignOffDTO> savedetails(INV_VendorSignOffDTO data)
        {
            try
            {
                // Duplicate Check
                var duplicate = await _context.INV_Master_Vendor_SignOffDMO
                    .Where(a => a.MVE_Id == data.MVE_Id
                             && a.MI_ID == data.MI_Id
                             && a.INVMSO_Id != data.INVMSO_Id)
                    .FirstOrDefaultAsync();

                if (duplicate != null)
                {
                    data.returnval = false;
                    data.saveorupdate = "Vendor Sign-Off Already Exists";
                    return data;
                }

                // Update
                if (data.INVMSO_Id > 0)
                {
                    var result = await _context.INV_Master_Vendor_SignOffDMO
                        .Where(a => a.INVMSO_Id == data.INVMSO_Id)
                        .FirstOrDefaultAsync();

                    if (result == null)
                    {
                        data.returnval = false;
                        data.saveorupdate = "Record not found";
                        return data;
                    }

                    result.MI_ID = data.MI_Id;
                    result.MVE_Id = data.MVE_Id;
                    result.INVMSO_SignOffDate = data.INVMSO_SignOffDate;
                    result.INVMSO_MOU_FilePath = data.INVMSO_MOU_FilePath;
                    result.INVMSO_WorkOrder_FilePath = data.INVMSO_WorkOrder_FilePath;
                    result.INVMSO_UpdatedBy = data.UserId;
                    result.INVMSO_UpdatedDate = DateTime.Now;

                    _context.Update(result);

                    var i = await _context.SaveChangesAsync();

                    if (i > 0)
                    {
                        data.returnval = true;

                        bool mailStatus = SendRequestMail(data);

                        data.saveorupdate = "Record Updated Successfully";
                    }
                    else
                    {
                        data.returnval = false;
                        data.saveorupdate = "Update Failed";
                    }
                }
                // Insert
                else
                {
                    // Validate request
                    if (data == null || data.MVE_Id <= 0 || data.MI_Id <= 0)
                    {
                        data.returnval = false;
                        data.saveorupdate = "Invalid Request. Required values are missing.";
                        return data;
                    }

                    INV_Master_Vendor_SignOffDMO result = new INV_Master_Vendor_SignOffDMO();

                    result.MI_ID = data.MI_Id;
                    result.MVE_Id = data.MVE_Id;
                    result.INVMSO_SignOffDate = data.INVMSO_SignOffDate;
                    result.INVMSO_MOU_FilePath = data.INVMSO_MOU_FilePath ?? "";
                    result.INVMSO_WorkOrder_FilePath = data.INVMSO_WorkOrder_FilePath ?? "";

                    result.INVMSO_ActiveFlg = true;
                    result.INVMSO_CreatedBy = data.UserId;
                    result.INVMSO_CreatedDate = DateTime.Now;
                    result.INVMSO_UpdatedBy = data.UserId;
                    result.INVMSO_UpdatedDate = DateTime.Now;

                    _context.INV_Master_Vendor_SignOffDMO.Add(result);

                    var i = await _context.SaveChangesAsync();

                    if (i > 0)
                    {
                        data.returnval = true;

                        bool mailStatus = SendRequestMail(data);

                        data.saveorupdate = "Record Saved Successfully";
                    }
                    else
                    {
                        data.returnval = false;
                        data.saveorupdate = "Failed to Save Data";
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.saveorupdate = ex.InnerException?.Message ?? ex.Message;
            }

            return data;
        }

        private bool SendRequestMail(INV_VendorSignOffDTO dto)
        {
            try
            {
                // Get vendor's registered email address from INV_MasterVendorEntry
                var vendor = _context.INV_MasterVendorEntryDMO
                    .FirstOrDefault(v => v.MVE_Id == dto.MVE_Id && v.MI_Id == dto.MI_Id);

                if (vendor == null || string.IsNullOrEmpty(vendor.MVE_Email_Address))
                {
                    return false;
                }

                string emailId = vendor.MVE_Email_Address;

                var institution = _context.Institution
                    .FirstOrDefault(i => i.MI_Id == dto.MI_Id);

                var emailConfig = _context.EMAIL_DETAILS_DMO
                    .FirstOrDefault(e => e.MI_ID == dto.MI_Id);

                if (emailConfig == null)
                {
                    return false;
                }

                var client = new SendGridClient(emailConfig.IVRM_sendgridkey);

                var message = new SendGridMessage();

                message.From = new EmailAddress(
                    emailConfig.IVRMMD_Mail_ID,
                    institution != null ? institution.MI_Name : "VAPS");

                message.Subject = "Vendor Sign Off Details";

                message.AddTo(new EmailAddress(emailId));

                message.HtmlContent = dto.msg;

                var response = client.SendEmailAsync(message).Result;

                return ((int)response.StatusCode >= 200 &&
                        (int)response.StatusCode < 300);
            }
            catch
            {
                return false;
            }
        }

        public async Task<INV_VendorSignOffDTO> deactive(INV_VendorSignOffDTO dto)
        {
            try
            {
                var record = await _context.INV_Master_Vendor_SignOffDMO
                    .Where(a => a.MI_ID == dto.MI_Id).FirstOrDefaultAsync();

                if (record != null)
                {
                    if (record.INVMSO_ActiveFlg == true)
                    {
                        record.INVMSO_ActiveFlg = false;
                    }
                    else
                    {
                        record.INVMSO_ActiveFlg = true;
                    }

                    record.INVMSO_UpdatedBy = dto.UserId;
                    record.INVMSO_CreatedDate = DateTime.Now;

                    _context.Update(record);
                    int count = _context.SaveChanges();
                    if (count > 0)
                    {
                        dto.returnval = true;
                    }
                    else
                    {
                        dto.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = false;
            }
            return dto;
        }
    }
}
