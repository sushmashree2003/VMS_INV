using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using InventoryServicesHub.com.vaps.Master.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Implementation
{
    public class INV_VendorCategoryMasterImpl : INV_VendorCategoryMasterInterface
    {
        private readonly InventoryContext _INVContext;
        private readonly ILogger<INV_VendorCategoryMasterImpl> _logInv;
        public INV_VendorCategoryMasterImpl(InventoryContext InvContext, ILogger<INV_VendorCategoryMasterImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }


        public async Task<INV_VendorCategoryMasterDTO> SaveData(INV_VendorCategoryMasterDTO data)
        {
            try
            {
                // Duplicate Check
                var duplicate = await _INVContext.INV_VendorCategoryMasterDMO
                    .Where(a => a.VCM_Vendorname.ToLower().Trim() == data.VCM_Vendorname.ToLower().Trim()
                             && a.VCM_Id != data.VCM_Id)
                    .FirstOrDefaultAsync();

                if (duplicate != null)
                {
                    data.returnval = false;
                    data.saveorupdate = "Vendor Name Already Exists";
                    return data;
                }

                // Update
                if (data.VCM_Id > 0)
                {
                    var entity = await _INVContext.INV_VendorCategoryMasterDMO
                        .Where(a => a.VCM_Id == data.VCM_Id)
                        .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        data.returnval = false;
                        data.saveorupdate = "Record not found";
                        return data;
                    }

                    entity.VCM_Vendorname = data.VCM_Vendorname;
                    entity.MI_Id = data.MI_Id;
                    //entity.VCM_Activeflag = true;
                    entity.VCM_Updatedby = data.UserId;
                    entity.VCM_Updateddate = DateTime.Now;

                    var result = await _INVContext.SaveChangesAsync();

                    data.returnval = result > 0;
                    data.saveorupdate = result > 0
                        ? "Updated Successfully"
                        : "Update Failed";
                }

                // Insert
                else
                {
                    var entity = new INV_VendorCategoryMasterDMO
                    {
                        VCM_Vendorname = data.VCM_Vendorname,
                        MI_Id = data.MI_Id,
                        VCM_Activeflag = true,
                        VCM_Createdby = data.UserId,
                        VCM_Updatedby = data.UserId,
                        VCM_Createddate = DateTime.Now,
                        VCM_Updateddate = DateTime.Now
                    };

                    _INVContext.INV_VendorCategoryMasterDMO.Add(entity);

                    var result = await _INVContext.SaveChangesAsync();

                    data.returnval = result > 0;
                    data.saveorupdate = result > 0
                        ? "Saved Successfully"
                        : "Save Failed";
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.saveorupdate = ex.InnerException?.Message ?? ex.Message;
            }

            return data;
        }


        public async Task<INV_VendorCategoryMasterDTO> Toggle_button(INV_VendorCategoryMasterDTO data)
        {
            try
            {
                var ToggleButton = await _INVContext.INV_VendorCategoryMasterDMO
                    .Where(a => a.VCM_Id == data.VCM_Id && a.MI_Id == data.MI_Id)
                    .FirstOrDefaultAsync();

                if (ToggleButton == null)
                {
                    data.returnval = false;
                    data.saveorupdate = "No record found";
                    return data;
                }

                if (ToggleButton.VCM_Activeflag == true)
                {
                    ToggleButton.VCM_Activeflag = false;
                    data.saveorupdate = "Successfully Deactivated";
                }
                else
                {
                    ToggleButton.VCM_Activeflag = true;
                    data.saveorupdate = "Successfully Activated";
                }

                ToggleButton.VCM_Createddate = DateTime.Now;
                ToggleButton.VCM_Updatedby = data.UserId;
                _INVContext.Update(ToggleButton);
                var existing = await _INVContext.SaveChangesAsync();

                if (existing > 0)
                {
                    data.returnval = true;
                    data.VCM_Activeflag = ToggleButton.VCM_Activeflag;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.returnval = false;
                data.saveorupdate = "Exception occurred";
            }
            return data;
        }


        public async Task<INV_VendorCategoryMasterDTO> GridData(INV_VendorCategoryMasterDTO data)
        {
            try
            {
                var getdata = await _INVContext.INV_VendorCategoryMasterDMO
                                .Where(a => a.MI_Id == data.MI_Id)
                                .OrderByDescending(a => a.VCM_Updateddate)
                                .ToArrayAsync();


                if (getdata != null && getdata.Length > 0)
                {
                    data.returnval = true;
                    data.VendorCategoryMasterArray = getdata;
                    data.saveorupdate = "Data fetched successfully";
                }
                else
                {
                    data.returnval = false;
                    data.saveorupdate = "No records found";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}
