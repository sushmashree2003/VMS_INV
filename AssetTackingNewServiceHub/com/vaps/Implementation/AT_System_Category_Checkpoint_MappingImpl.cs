using AssetTrackingServiceHub.com.vaps.Interface;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class AT_System_Category_Checkpoint_MappingImpl : IAT_System_Category_Checkpoint_Mapping
    {
        private readonly AssetTrackingContext _ATContext;

        public AT_System_Category_Checkpoint_MappingImpl(AssetTrackingContext ATContext)
        {
            _ATContext = ATContext;
        }

        public async Task<AT_System_Category_Checkpoint_MappingDTO> GetDetails(AT_System_Category_Checkpoint_MappingDTO request)
        {
            try
            {
                // 1. Projects Query
                request.projectlist = await _ATContext.ISM_Master_ProjectDMO
                    .Where(p => p.ISMMPR_ActiveFlg == true && p.MI_Id == request.MI_Id)
                    .OrderBy(p => p.ISMMPR_ProjectName)
                    .Select(p => new ProjectDropdownDTO
                    {
                        ISMMPR_Id = p.ISMMPR_Id,
                        ISMMPR_ProjectName = p.ISMMPR_ProjectName
                    })
                    .Distinct()
                    .ToArrayAsync();

                // 2. Systems Query
                request.systemlist = await _ATContext.MasterSystemDMO
                    .Where(s => s.MI_Id == request.MI_Id && s.ActiveFlag == true)
                    .Select(s => new SystemDropdownDTO
                    {
                        MS_Id = s.MS_Id,
                        System = s.System
                    })
                    .ToArrayAsync();

                // 3. Categories Query
                request.categorylist = await _ATContext.AT_Master_CategoryDMO
                    .Where(c => c.MI_Id == request.MI_Id && c.ActiveFlag == true)
                    .Select(c => new CategoryDropdownDTO
                    {
                        MC_Id = c.MC_Id,
                        Category = c.Category
                    })
                    .ToArrayAsync();

                // 4. Checkpoints Query
                request.checkpointlist = await _ATContext.AT_Master_CheckPointDMO
                    .Where(cp => cp.MI_Id == request.MI_Id && cp.ActiveFlag == true)
                    .Select(cp => new CheckpointDropdownDTO
                    {
                        MCP_Id = cp.MCP_Id,
                        CheckPoint = cp.CheckPoint
                    })
                    .ToArrayAsync();
                var gridData = await _ATContext.AT_System_Category_Checkpoint_MappingDMO
                                         .Where(main => main.MI_Id == request.MI_Id)
                                         .Join(
                                             _ATContext.ISM_Master_ProjectDMO,
                                             main => main.ISMMPR_Id,
                                             pro => pro.ISMMPR_Id,
                                             (main, pro) => new { main, pro }
                                         )
                                         .Join(
                                             _ATContext.MasterSystemDMO,
                                             x => x.main.MS_Id,
                                             sys => sys.MS_Id,
                                             (x, sys) => new { x.main, x.pro, sys }
                                         )
                                         .Join(
                                             _ATContext.AT_Master_CategoryDMO,
                                             x => x.main.MC_Id,
                                             cat => cat.MC_Id,
                                             (x, cat) => new { x.main, x.pro, x.sys, cat }
                                         )
                                         .GroupBy(x => new
                                         {
                                             x.main.ISMMPR_Id,
                                             x.main.MS_Id,
                                             x.main.MC_Id,
                                             x.pro.ISMMPR_ProjectName,
                                             x.sys.System,
                                             x.cat.Category,
                                             x.main.ActiveFlag
                                         })
                                         .Select(g => new GridMappingDTO
                                         {
                                             ISMMPR_Id = g.Key.ISMMPR_Id,
                                             MS_Id = g.Key.MS_Id,
                                             MC_Id = g.Key.MC_Id,
                                             ProjectName = g.Key.ISMMPR_ProjectName,
                                             SystemName = g.Key.System,
                                             CategoryName = g.Key.Category,
                                             TotalItems = g.Count(),
                                             ActiveFlag = g.Key.ActiveFlag
                                         })
                                         .OrderByDescending(x => x.ISMMPR_Id)
                                         .ToArrayAsync();

                request.alldata = gridData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetDetails: " + ex.Message);
            }

            return request;
        }

        //public async Task<AT_System_Category_Checkpoint_MappingDTO> SaveRecord(AT_System_Category_Checkpoint_MappingDTO request)
        //{
        //    try
        //    {
        //        if (request.selectedCheckpoints != null && request.selectedCheckpoints.Count > 0)
        //        {
        //            // 1. Convert selected checkpoint IDs into a comma-separated string (e.g., "1,2,5")
        //            string mcpIds = string.Join(",", request.selectedCheckpoints
        //                .Select(c => c.MCP_Id)
        //                .Where(id => id > 0));

        //            // 2. Prepare SQL Parameters matching the SP signature
        //            var msIdParam = new SqlParameter("@MS_Id", SqlDbType.BigInt) { Value = request.MS_Id };
        //            var mcIdParam = new SqlParameter("@MC_Id", SqlDbType.BigInt) { Value = request.MC_Id };
        //            var mcpIdParam = new SqlParameter("@MCP_Id", SqlDbType.VarChar) { Value = mcpIds };
        //            var miIdParam = new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = (object)request.MI_Id ?? DBNull.Value };
        //            var ismmprIdParam = new SqlParameter("@ISMMPR_Id", SqlDbType.BigInt) { Value = (object)request.ISMMPR_Id ?? DBNull.Value };
        //            var userIdParam = new SqlParameter("@user_Id", SqlDbType.BigInt) { Value = request.UserId };

        //            // OUTPUT parameter for SCCC_Id
        //            var scccIdParam = new SqlParameter("@SCCC_Id", SqlDbType.BigInt)
        //            {
        //                Direction = ParameterDirection.InputOutput,
        //                Value = request.ATSCCM_Id > 0 ? (object)request.ATSCCM_Id : DBNull.Value
        //            };

        //            // 3. Execute Stored Procedure
        //            await _ATContext.Database.ExecuteSqlCommandAsync(
        //                    "EXEC [dbo].[System_Category_Checkpoint_Mapping_Insert] @MS_Id, @MC_Id, @MCP_Id, @MI_Id, @ISMMPR_Id, @user_Id, @SCCC_Id OUTPUT",
        //                    msIdParam, mcIdParam, mcpIdParam, miIdParam, ismmprIdParam, userIdParam, scccIdParam
        //                );

        //            // 4. Read back the generated or updated Output ID
        //            if (scccIdParam.Value != DBNull.Value && scccIdParam.Value != null)
        //            {
        //                request.ATSCCM_Id = Convert.ToInt64(scccIdParam.Value);
        //            }

        //            request.returnValue = true;
        //        }
        //        else
        //        {
        //            request.returnValue = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error saving mapping via SP: " + ex.Message);
        //        request.returnValue = false;
        //    }

        //    return request;
        //}
        public async Task<AT_System_Category_Checkpoint_MappingDTO> SaveRecord(AT_System_Category_Checkpoint_MappingDTO request)
        {
            try
            {
                if (request.selectedCheckpoints != null && request.selectedCheckpoints.Count > 0)
                {
                    int totalSelected = request.selectedCheckpoints.Count;

                    var recordsToSave = new List<AT_System_Category_Checkpoint_MappingDMO>();

                    foreach (var item in request.selectedCheckpoints)
                    {
                        if (item.MCP_Id > 0)
                        {
                            //AnyAsync return TRUE or FALSE .It doesn't load the record from the database.
                            // Check whether the same Project + System + Category + Checkpoint already exists
                            bool exists = await _ATContext.AT_System_Category_Checkpoint_MappingDMO
                                .AnyAsync(x =>
                                    x.MI_Id == request.MI_Id &&
                                    x.ISMMPR_Id == request.ISMMPR_Id &&
                                    x.MS_Id == request.MS_Id &&
                                    x.MC_Id == request.MC_Id &&
                                    x.MCP_Id == item.MCP_Id.ToString());

                            // Save only if it does not already exist
                            if (!exists)
                            {
                                recordsToSave.Add(new AT_System_Category_Checkpoint_MappingDMO
                                {
                                    MI_Id = request.MI_Id,
                                    ISMMPR_Id = request.ISMMPR_Id,
                                    MS_Id = request.MS_Id,
                                    MC_Id = request.MC_Id,
                                    MCP_Id = item.MCP_Id.ToString(),
                                    Total_Item = totalSelected,
                                    CreatedBy = request.UserId,
                                    CreatedDate = DateTime.Now,
                                    UpdatedBy = request.UserId,
                                    UpdatedDate = DateTime.Now,
                                    ActiveFlag = true
                                });
                            }
                        }
                    }

                    if (recordsToSave.Any())
                    {
                        await _ATContext.AT_System_Category_Checkpoint_MappingDMO.AddRangeAsync(recordsToSave);
                        await _ATContext.SaveChangesAsync();

                        request.returnValue = true;
                    }
                    else
                    {
                        // All selected checkpoints already exist
                        request.returnValue = false;
                        request.message = "Selected checkpoint(s) already mapped.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving mapping: " + ex.Message);
                request.returnValue = false;
            }

            return request;
        }
       
    }
}
