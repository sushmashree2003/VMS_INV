using AssetTrackingServiceHub.com.vaps.Master.Interface;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Master.Implementation
{
    public class MasterSystemImpl : IMasterSystem
    {
        private readonly InventoryContext _inventoryContext;

        public MasterSystemImpl(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }

        public async Task<MasterSystemDTO> GetDetails(MasterSystemDTO request)
        {
            try
            {
              var getData = await _inventoryContext.MasterSysteDMO
                    .Where(mi => mi.MI_Id == request.MI_Id && mi.ActiveFlag== true)                    
                    .ToArrayAsync();
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
                if(request.MS_Id >0)
                {
                    var update= await _inventoryContext.MasterSysteDMO.Where(ms => ms.MS_Id == request.MS_Id).FirstOrDefaultAsync();
                    if(update != null)
                    {
                        update.System = request.System;
                        update.UpdatedDate = DateTime.Now;
                        update.UpdatedBy = request.UserId;
                        update.ActiveFlag = request.ActiveFlag;
                        update.MI_Id = request.MI_Id;
                        _inventoryContext.Update(update);
                        var updatedData = await _inventoryContext.SaveChangesAsync();
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
                    _inventoryContext.Add(result);
                    var savedData = await _inventoryContext.SaveChangesAsync();
                    if (savedData > 0)
                    {
                        request.returnValue = true;

                    }
                    else { request.returnValue = false; }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return request;
        }
        public async Task<MasterSystemDTO> DeleteRecord(MasterSystemDTO request)
        {
            try
            {
                var result = await _inventoryContext.MasterSysteDMO
                    .Where(ms => ms.MS_Id == request.MS_Id)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    result.ActiveFlag = !result.ActiveFlag;
                    result.UpdatedDate = DateTime.Now;
                    result.UpdatedBy = request.UserId;

                    _inventoryContext.Update(result);
                    var updatedData = await _inventoryContext.SaveChangesAsync();
                    request.returnValue = updatedData > 0;
                }
                else
                {
                    request.returnValue = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MasterSystem DeleteRecord: " + ex.Message);
                request.returnValue = false;
            }

            return request;
        }
    }
}
