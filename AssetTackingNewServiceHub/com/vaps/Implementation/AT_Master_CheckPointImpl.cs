using AssetTrackingServiceHub.com.vaps.Interface;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class AT_Master_CheckPointImpl : IAT_Master_CheckPoint
    {
        private readonly AssetTrackingContext _ATContext;

        public AT_Master_CheckPointImpl(AssetTrackingContext ATContext)
        {
            _ATContext = ATContext;
        }

        public async Task<AT_Master_CheckPointDTO> GetDetails(AT_Master_CheckPointDTO request)
        {
            try
            {
                var getData = await _ATContext.AT_Master_CheckPointDMO
                    .Where(mi => mi.MI_Id == request.MI_Id)
                    .ToArrayAsync();

                if (getData != null && getData.Length > 0)
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
                Console.WriteLine("Error in AT_Master_CheckPoint GetDetails: " + ex.Message);
            }

            return request;
        }

        public async Task<AT_Master_CheckPointDTO> SaveOrUpdateRecord(AT_Master_CheckPointDTO request)
        {
            try
            {
                // Check Duplicate CheckPoint Name
                var checkDup = await _ATContext.AT_Master_CheckPointDMO
                    .Where(m => m.MI_Id == request.MI_Id
                             && m.CheckPoint.Equals(request.CheckPoint, StringComparison.OrdinalIgnoreCase)
                             && m.MCP_Id != request.MCP_Id)
                    .ToListAsync();

                if (checkDup.Count > 0)
                {
                    request.duplicate = true;
                    request.returnValue = false;
                    return request;
                }

                request.duplicate = false;

                if (request.MCP_Id > 0)
                {
                    // Update Record
                    var update = await _ATContext.AT_Master_CheckPointDMO
                        .Where(ms => ms.MCP_Id == request.MCP_Id)
                        .FirstOrDefaultAsync();

                    if (update != null)
                    {
                        update.CheckPoint = request.CheckPoint;
                        update.UpdatedDate = DateTime.Now;
                        update.UpdatedBy = request.UserId;
                        update.MI_Id = request.MI_Id;

                        _ATContext.Update(update);
                        var updatedData = await _ATContext.SaveChangesAsync();
                        request.returnValue = updatedData > 0;
                    }
                    else
                    {
                        request.returnValue = false;
                    }
                }
                else
                {
                    // Insert Record
                    AT_Master_CheckPointDMO result = new AT_Master_CheckPointDMO
                    {
                        MI_Id = request.MI_Id,
                        CheckPoint = request.CheckPoint,
                        CreatedBy = request.UserId,
                        UpdatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        ActiveFlag = true
                    };

                    _ATContext.Add(result);
                    var savedData = await _ATContext.SaveChangesAsync();
                    request.returnValue = savedData > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AT_Master_CheckPoint SaveOrUpdateRecord: " + ex.Message);
                request.returnValue = false;
            }

            return request;
        }

        public async Task<AT_Master_CheckPointDTO> DeleteRecord(AT_Master_CheckPointDTO request)
        {
            try
            {
                var result = await _ATContext.AT_Master_CheckPointDMO
                    .Where(ms => ms.MCP_Id == request.MCP_Id)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    result.ActiveFlag = !result.ActiveFlag;
                    result.UpdatedDate = DateTime.Now;
                    result.UpdatedBy = request.UserId;

                    _ATContext.Update(result);
                    var updatedData = await _ATContext.SaveChangesAsync();
                    request.returnValue = updatedData > 0;
                }
                else
                {
                    request.returnValue = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AT_Master_CheckPoint DeleteRecord: " + ex.Message);
                request.returnValue = false;
            }

            return request;
        }
    }
}
