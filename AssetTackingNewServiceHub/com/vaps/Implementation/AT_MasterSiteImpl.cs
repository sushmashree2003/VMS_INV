using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class AT_MasterSiteImpl : Interface.AT_MasterSiteInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AT_MasterSiteImpl> _logAT;
        public AT_MasterSiteImpl(AssetTrackingContext ATContext, ILogger<AT_MasterSiteImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public AT_MasterSiteDTO getloaddata(AT_MasterSiteDTO data)
        {
            try
            {
                data.get_sites = _ATContext.INV_Master_SiteDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMSI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Master Site load Page:" + ex.Message);
            }
            return data;
        }

        public AT_MasterSiteDTO savedetails(AT_MasterSiteDTO data)
        {
            try
            {
                if (data.INVMSI_Id != 0)
                {
                    var res = _ATContext.INV_Master_SiteDMO.Where(t => t.INVMSI_SiteBuildingName == data.INVMSI_SiteBuildingName  && t.MI_Id == data.MI_Id && t.INVMSI_Id != data.INVMSI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ATContext.INV_Master_SiteDMO.Single(t => t.INVMSI_Id == data.INVMSI_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMSI_SiteBuildingName = data.INVMSI_SiteBuildingName;
                        result.INVMSI_SiteRemarks = data.INVMSI_SiteRemarks;                       
                        result.INVMSI_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _ATContext.Update(result);

                        var contactExists = _ATContext.SaveChanges();
                        if (contactExists > 0)
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
                    var res = _ATContext.INV_Master_SiteDMO.Where(t => t.INVMSI_SiteBuildingName == data.INVMSI_SiteBuildingName && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_SiteDMO site = new INV_Master_SiteDMO();
                        site.MI_Id = data.MI_Id;
                        site.INVMSI_SiteBuildingName = data.INVMSI_SiteBuildingName;
                        site.INVMSI_SiteRemarks = data.INVMSI_SiteRemarks;                       
                        site.INVMSI_ActiveFlg = true;
                        site.CreatedDate = DateTime.Now;
                        site.UpdatedDate = DateTime.Now;
                        _ATContext.Add(site);

                        var contactExists = _ATContext.SaveChanges();
                        if (contactExists > 0)
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
                data.message = "Error";
                _logAT.LogInformation("Master Site savedata :" + ex.Message);
            }
            return data;
        }

        public AT_MasterSiteDTO deactive(AT_MasterSiteDTO data)
        {
            try
            {
                var result = _ATContext.INV_Master_SiteDMO.Single(t => t.INVMSI_Id == data.INVMSI_Id);

                if (result.INVMSI_ActiveFlg == true)
                {
                    result.INVMSI_ActiveFlg = false;
                }
                else if (result.INVMSI_ActiveFlg == false)
                {
                    result.INVMSI_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public async Task<MasterSystemDTO> GetDetails(MasterSystemDTO request)
        {
            try
            {
                var getData = await _ATContext.MasterSystemDMO
                      .Where(mi => mi.MI_Id == request.MI_Id && mi.ActiveFlag == true).ToArrayAsync();
                if (getData != null)
                {
                    request.returnValue = true;
                    request.alldata = getData;
                    request.successMessage = "Data fetched successfully";
                }
                else
                {
                    request.returnValue = false;
                    request.successMessage = "Data not Found!";
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return request;
        }


        public async Task<MasterSystemDTO> SaveOrUpdateRecord(MasterSystemDTO request)
        {
            try
            {
                if (request.MS_Id > 0)
                {
                    var update = await _ATContext.MasterSystemDMO.Where(ms => ms.MS_Id == request.MS_Id).FirstOrDefaultAsync();
                    if (update != null)
                    {
                        update.System = request.System;
                        update.UpdatedDate = DateTime.Now;
                        update.UpdatedBy = request.UserId;
                        update.ActiveFlag = true;
                        update.MI_Id = request.MI_Id;
                        _ATContext.Update(update);
                        var updatedData = await _ATContext.SaveChangesAsync();
                        if (updatedData > 0)
                        {

                            request.returnValue = true;

                        }
                        else
                        {
                            request.returnValue = false;
                        }
                    }
                    else
                    {
                        request.returnValue = false;
                    }

                }
                else
                {
                    MasterSystemDMO result = new MasterSystemDMO();
                    result.MI_Id = request.MI_Id;
                    result.System = request.System;
                    result.CreatedBy = request.UserId;
                    result.UpdatedBy = request.UserId;
                    result.CreatedDate = DateTime.Now;
                    result.UpdatedDate = DateTime.Now;
                    result.ActiveFlag = true;
                    _ATContext.Add(result);
                    var savedData = await _ATContext.SaveChangesAsync();
                    if (savedData > 0)
                    {
                        request.returnValue = true;

                    }
                    else { request.returnValue = false; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return request;
        }

        public async Task<MasterSystemDTO> DeleteRecord(MasterSystemDTO request)
        {
            try
            {
                var result = await _ATContext.MasterSystemDMO
                    .Where(ms => ms.MS_Id == request.MS_Id)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    // Toggle the active flag (if true becomes false, if false becomes true)
                    result.ActiveFlag = !result.ActiveFlag;
                    result.UpdatedDate = DateTime.Now;
                    result.UpdatedBy = request.UserId;

                    _ATContext.Update(result);
                    var updatedData = await _ATContext.SaveChangesAsync();

                    // Return true if record successfully updated in DB
                    request.returnValue = updatedData > 0;
                }
                else
                {
                    request.returnValue = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteRecord/ToggleActive: " + ex.Message);
                request.returnValue = false;
            }

            return request;
        }

    }
}
