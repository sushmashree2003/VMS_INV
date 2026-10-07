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
    public class MasterCategoryImpl : IMasterCategory
    {
        private readonly AssetTrackingContext _ATContext;

        public MasterCategoryImpl(AssetTrackingContext ATContext)
        {
            _ATContext = ATContext;
        }

        public async Task<AT_Master_CategoryDTO> GetDetails(AT_Master_CategoryDTO request)
        {
            try
            {
                var getData = await _ATContext.AT_Master_CategoryDMO
                    .Where(mi => mi.MI_Id == request.MI_Id && mi.ActiveFlag==true)
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
                Console.WriteLine("Error in MasterCategory GetDetails: " + ex.Message);
            }

            return request;
        }

        public async Task<AT_Master_CategoryDTO> SaveOrUpdateRecord(AT_Master_CategoryDTO request)
        {
            try
            {
                // Check Duplicate Category Name
                var checkDup = await _ATContext.AT_Master_CategoryDMO
                    .Where(m => m.MI_Id == request.MI_Id
                             && m.Category.Equals(request.Category, StringComparison.OrdinalIgnoreCase)
                             && m.MC_Id != request.MC_Id)
                    .ToListAsync();

                if (checkDup.Count > 0)
                {
                    request.duplicate = true;
                    request.returnValue = false;
                    return request;
                }

                request.duplicate = false;

                if (request.MC_Id > 0)
                {
                    // Update Record
                    var update = await _ATContext.AT_Master_CategoryDMO
                        .Where(ms => ms.MC_Id == request.MC_Id)
                        .FirstOrDefaultAsync();

                    if (update != null)
                    {
                        update.Category = request.Category;
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
                    AT_Master_CategoryDMO result = new AT_Master_CategoryDMO
                    {
                        MI_Id = request.MI_Id,
                        Category = request.Category,
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
                Console.WriteLine("Error in MasterCategory SaveOrUpdateRecord: " + ex.Message);
                request.returnValue = false;
            }

            return request;
        }

        public async Task<AT_Master_CategoryDTO> DeleteRecord(AT_Master_CategoryDTO request)
        {
            try
            {
                var result = await _ATContext.AT_Master_CategoryDMO
                    .Where(ms => ms.MC_Id == request.MC_Id)
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
                Console.WriteLine("Error in MasterCategory DeleteRecord: " + ex.Message);
                request.returnValue = false;
            }

            return request;
        }
    }
}

