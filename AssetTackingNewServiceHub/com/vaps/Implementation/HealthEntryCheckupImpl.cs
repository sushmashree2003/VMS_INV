using AssetTrackingServiceHub.com.vaps.Interface;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class HealthEntryCheckupImpl : IHealthCheckupEntry
    {
        private readonly AssetTrackingContext _ATContext;

        public HealthEntryCheckupImpl(AssetTrackingContext ATContext)
        {
            _ATContext = ATContext;
        }
        public async Task<Health_Entry_CheckupDTO> GetDetails(Health_Entry_CheckupDTO request)
        {
            try
            {
                // 1. Fetch Active Projects
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

                // 2. Fetch Clients
                request.ClientList = await _ATContext.ISM_Master_Client_DMO
                    .Where(c => c.MI_Id == request.MI_Id)
                    .OrderBy(c => c.ISMMCLT_ClientName)
                    .Select(c => new ClientDropDownDTO
                    {
                        ISMMCLT_Id = c.ISMMCLT_Id,
                        ISMMCLT_ClientName = c.ISMMCLT_ClientName
                    })
                    .Distinct()
                    .ToArrayAsync();

                // 3. Fetch Status Master Data
                try
                {
                    var statusList = new List<Health_Entry_StatusDTO>();
                    using (var cmdStatus = _ATContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmdStatus.CommandText = "SELECT Health_Entry_Status_Id, StatusName FROM Health_Entry_Status";
                        if (cmdStatus.Connection.State != ConnectionState.Open)
                            await cmdStatus.Connection.OpenAsync();

                        using (var rStatus = await cmdStatus.ExecuteReaderAsync())
                        {
                            while (await rStatus.ReadAsync())
                            {
                                statusList.Add(new Health_Entry_StatusDTO
                                {
                                    Health_Entry_Status_Id = Convert.ToInt64(rStatus["Health_Entry_Status_Id"]),
                                    StatusName = rStatus["StatusName"]?.ToString()
                                });
                            }
                        }
                    }

                    if (statusList.Count == 0)
                    {
                        statusList = await _ATContext.Health_Entry_Status
                            .Select(s => new Health_Entry_StatusDTO
                            {
                                Health_Entry_Status_Id = s.Health_Entry_Status_Id,
                                StatusName = s.StatusName
                            })
                            .ToListAsync();
                    }

                    if (statusList.Count == 0)
                    {
                        statusList = new List<Health_Entry_StatusDTO>
                        {
                            new Health_Entry_StatusDTO { Health_Entry_Status_Id = 1, StatusName = "OK" },
                            new Health_Entry_StatusDTO { Health_Entry_Status_Id = 2, StatusName = "Attention" },
                            new Health_Entry_StatusDTO { Health_Entry_Status_Id = 3, StatusName = "Fail" },
                            new Health_Entry_StatusDTO { Health_Entry_Status_Id = 4, StatusName = "In Progress" },
                            new Health_Entry_StatusDTO { Health_Entry_Status_Id = 5, StatusName = "Not Checked" }
                        };
                    }

                    request.StatusList = statusList.ToArray();
                }
                catch (Exception exStatus)
                {
                    Console.WriteLine("Error in fetching StatusList: " + exStatus.Message);
                    request.StatusList = new Health_Entry_StatusDTO[]
                    {
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 1, StatusName = "OK" },
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 2, StatusName = "Attention" },
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 3, StatusName = "Fail" },
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 4, StatusName = "In Progress" },
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 5, StatusName = "Not Checked" }
                    };
                }

                // 5. Fetch Summary Grid Entries
                var gridList = new List<Health_Checkup_Grid_DTO>();
                using (var command2 = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    command2.CommandText = "SP_Get_Health_Checkup_Entry_Grid";
                    command2.CommandType = CommandType.StoredProcedure;

                    var paramGrid = command2.CreateParameter();
                    paramGrid.ParameterName = "@MI_Id";
                    paramGrid.Value = request.MI_Id;
                    command2.Parameters.Add(paramGrid);

                    if (command2.Connection.State != ConnectionState.Open)
                        await command2.Connection.OpenAsync();

                    using (var reader2 = await command2.ExecuteReaderAsync())
                    {
                        var colNames2 = Enumerable.Range(0, reader2.FieldCount).Select(reader2.GetName).ToList();
                        while (await reader2.ReadAsync())
                        {
                            gridList.Add(new Health_Checkup_Grid_DTO
                            {
                                HCUE_Id = Convert.ToInt64(reader2["HCUE_Id"]),
                                ProjectName = reader2["ISMMPR_ProjectName"]?.ToString(),
                                CreatedDate = reader2["created_date"] != DBNull.Value ? Convert.ToDateTime(reader2["created_date"]) : (DateTime?)null,
                                NextDueDate = reader2["Next_Due_Date"] != DBNull.Value ? Convert.ToDateTime(reader2["Next_Due_Date"]) : (DateTime?)null,
                                TechnicianName = reader2["Technician_Name"]?.ToString(),
                                StatusName = reader2["StatusName"]?.ToString() ?? "Pending",
                                ISMMPR_Id = Convert.ToInt64(reader2["ISMMPR_Id"]),
                                ClientName = colNames2.Contains("ISMMCLT_ClientName") && reader2["ISMMCLT_ClientName"] != DBNull.Value
                                    ? reader2["ISMMCLT_ClientName"].ToString()
                                    : (colNames2.Contains("ClientName") && reader2["ClientName"] != DBNull.Value ? reader2["ClientName"].ToString() : string.Empty)
                            });
                        }
                    }
                }
                request.gridDataList = gridList.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetDetails: " + ex.Message);
            }

            return request;
        }

        public async Task<Health_Entry_CheckupDTO> GetCheckpoints(Health_Entry_CheckupDTO request)
        {
            try
            {
                var list = new List<SystemCategoryCheckpointDto>();
                using (var command = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "dbo.SP_GetSystemCategoryCheckpoints";
                    command.CommandType = CommandType.StoredProcedure;

                    var param1 = command.CreateParameter();
                    param1.ParameterName = "@MI_Id";
                    param1.Value = request.MI_Id;
                    command.Parameters.Add(param1);

                    var param2 = command.CreateParameter();
                    param2.ParameterName = "@ISMMPR_Id";
                    param2.Value = request.ISMMPR_Id;
                    command.Parameters.Add(param2);

                    var param3 = command.CreateParameter();
                    param3.ParameterName = "@HCUE_Id";
                    param3.Value = request.HCUE_Id;
                    command.Parameters.Add(param3);

                    if (command.Connection.State != ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new SystemCategoryCheckpointDto
                            {
                                SystemCategoryHeader = reader["SystemCategoryHeader"]?.ToString(),
                                System = reader["System"]?.ToString(),
                                Category = reader["Category"]?.ToString(),
                                CheckPoint = reader["CheckPoint"]?.ToString(),
                                SCCC_Id = reader["SCCC_Id"] != DBNull.Value ? Convert.ToInt64(reader["SCCC_Id"]) : 0,
                                MS_Id = reader["MS_Id"] != DBNull.Value ? Convert.ToInt64(reader["MS_Id"]) : 0,
                                MC_Id = reader["MC_Id"] != DBNull.Value ? Convert.ToInt64(reader["MC_Id"]) : 0,

                                // Map status, remarks, and priority from the mapping table
                                Health_Entry_Status_Id = reader["Health_Entry_Status_Id"] != DBNull.Value ? Convert.ToInt64(reader["Health_Entry_Status_Id"]) : (long?)null,
                                Remarks = reader["Remarks"]?.ToString(),
                                Priority = reader["Priority"]?.ToString() ?? "Medium",
                                Approval_Status = reader["Approval_Status"]?.ToString()
                            });
                        }
                    }
                }
                request.systemCategoryCheckpointDtos = list.ToArray();

                // Ensure StatusList is always populated when loading checkpoints
                try
                {
                    request.StatusList = await _ATContext.Health_Entry_Status
                        .Select(s => new Health_Entry_StatusDTO
                        {
                            Health_Entry_Status_Id = s.Health_Entry_Status_Id,
                            StatusName = s.StatusName
                        })
                        .ToArrayAsync();
                }
                catch
                {
                    request.StatusList = new Health_Entry_StatusDTO[]
                    {
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 1, StatusName = "OK" },
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 2, StatusName = "Attention" },
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 3, StatusName = "Fail" },
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 4, StatusName = "In Progress" },
                        new Health_Entry_StatusDTO { Health_Entry_Status_Id = 5, StatusName = "Not Checked" }
                    };
                }

                request.returnval = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetCheckpoints: " + ex.Message);
            }
            return request;
        }

        public async Task<Health_Entry_CheckupDTO> SaveData(Health_Entry_CheckupDTO request)
        {
            var connection = _ATContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    long hcueId = 0;

                    // Sanitize Contact Number
                    if (!string.IsNullOrWhiteSpace(request.Contact_Number))
                    {
                        request.Contact_Number = request.Contact_Number.Trim();
                        if (request.Contact_Number.Length > 10)
                        {
                            request.Contact_Number = request.Contact_Number.Substring(0, 10);
                        }
                    }

                    // -------------------------------------------------------------
                    // STEP 1: Insert into Header Table (Health_Checkup_Entry)
                    // -------------------------------------------------------------
                    using (var cmd1 = connection.CreateCommand())
                    {
                        cmd1.Transaction = transaction;
                        cmd1.CommandText = "dbo.SP_Insert_Health_Checkup_Entry";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@MI_Id", request.MI_Id));
                        cmd1.Parameters.Add(new SqlParameter("@Technician_Name", (object)request.Technician_Name ?? DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@Company_Name", (object)request.Company_Name ?? DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@Contact_Number", (object)request.Contact_Number ?? DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@Client_Id", (object)request.ISMMCLT_Id ?? DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@Createdby", request.UserId));
                        cmd1.Parameters.Add(new SqlParameter("@Updatedby", request.UserId));
                        cmd1.Parameters.Add(new SqlParameter("@ActiveFlag", true));
                        cmd1.Parameters.Add(new SqlParameter("@ISMMPR_Id", request.ISMMPR_Id));
                        cmd1.Parameters.Add(new SqlParameter("@Status_Id", DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@Designation", (object)request.Designation ?? DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@Next_Due_Date", request.Next_Due_Date));
                        cmd1.Parameters.Add(new SqlParameter("@Emailid", (object)request.Emailid ?? DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@Remarks", (object)request.Remarks ?? DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@Priority", (object)request.Priority ?? DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@MS_Id", DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@MC_Id", DBNull.Value));
                        cmd1.Parameters.Add(new SqlParameter("@MCP_Id", DBNull.Value));


                        var result = await cmd1.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            hcueId = Convert.ToInt64(result);
                        }
                    }

                    // -------------------------------------------------------------
                    // STEP 2: Loop & Insert into Mapping Table (Health_Entry_System_Category_Checkpoint_Mapping)
                    // -------------------------------------------------------------
                    if (hcueId > 0 && request.systemCategoryCheckpointDtos != null && request.systemCategoryCheckpointDtos.Length > 0)
                    {
                        using (var cmd2 = connection.CreateCommand())
                        {
                            cmd2.Transaction = transaction;
                            cmd2.CommandText = "dbo.SP_Insert_Health_Entry_System_Category_Checkpoint_Mapping";
                            cmd2.CommandType = CommandType.StoredProcedure;

                            foreach (var item in request.systemCategoryCheckpointDtos)
                            {
                                cmd2.Parameters.Clear(); // Clear parameters for each row iteration

                                cmd2.Parameters.Add(new SqlParameter("@HCUE_Id", hcueId));
                                cmd2.Parameters.Add(new SqlParameter("@SCCC_Id", item.SCCC_Id));
                                cmd2.Parameters.Add(new SqlParameter("@Health_Entry_Status_Id", (object)item.Health_Entry_Status_Id ?? DBNull.Value));
                                cmd2.Parameters.Add(new SqlParameter("@Priority", (object)item.Priority ?? DBNull.Value));
                                cmd2.Parameters.Add(new SqlParameter("@Remarks", (object)item.Remarks ?? DBNull.Value));
                                cmd2.Parameters.Add(new SqlParameter("@ActiveFlag", true));
                                cmd2.Parameters.Add(new SqlParameter("@CreatedBy", request.UserId));
                                cmd2.Parameters.Add(new SqlParameter("@UpdatedBy", request.UserId));
                                cmd2.Parameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                                cmd2.Parameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
                                cmd2.Parameters.Add(new SqlParameter("@Approval_Status", "FIRST LEVEL APPROVAL"));

                                await cmd2.ExecuteNonQueryAsync();
                            }
                        }
                    }

                    transaction.Commit();
                    request.returnval = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error in SaveData: " + ex.Message);
                    request.returnval = false;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return request;
        }

        public async Task<Health_Entry_CheckupDTO> GetApprovalDetails(Health_Entry_CheckupDTO request)
        {
            try
            {
                var list = new List<ApprovalDetailsDto>();

                using (var command = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_Get_Health_Checkup_Approval_Details";
                    command.CommandType = CommandType.StoredProcedure;

                    // Parameter 1: @MI_Id
                    var param1 = command.CreateParameter();
                    param1.ParameterName = "@MI_Id";
                    param1.Value = request.MI_Id;
                    command.Parameters.Add(param1);

                    // Parameter 2: @ISMMPR_Id
                    var param2 = command.CreateParameter();
                    param2.ParameterName = "@ISMMPR_Id";
                    param2.Value = request.ISMMPR_Id;
                    command.Parameters.Add(param2);

                    // Parameter 3: @Approval_Status
                    var param3 = command.CreateParameter();
                    param3.ParameterName = "@Approval_Status";
                    param3.Value = request.Approval_Status;
                    command.Parameters.Add(param3);

                  

                    if (command.Connection.State != ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new ApprovalDetailsDto
                            {
                                HESCCM_Id = Convert.ToInt64(reader["HESCCM_Id"]),
                                SystemCategoryHeader = reader["SystemCategoryHeader"]?.ToString(),
                                System = reader["System"]?.ToString(),
                                Category = reader["Category"]?.ToString(),
                                CheckPoint = reader["CheckPoint"]?.ToString(),
                                StatusName = reader["StatusName"]?.ToString(),
                                Remarks = reader["Remarks"]?.ToString(),
                                Priority = reader["Priority"]?.ToString(),
                                CheckedBy = reader["NormalizedUserName"]?.ToString(),
                                CheckedDate = reader["checkdate"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["checkdate"]).ToString("dd-MM-yyyy")
                                    : ""
                            });
                        }
                    }
                }

                request.approvalDetailsList = list.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetApprovalDetails: " + ex.Message);
            }

            return request;
        }

        public async Task<Health_Entry_CheckupDTO> SaveApproval(Health_Entry_CheckupDTO request)
        {
            try
            {
                var connection = _ATContext.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                int totalAffectedRows = 0;

                if (request.selectedHesccmIds != null && request.selectedHesccmIds.Length > 0)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "dbo.SP_Update_Health_Checkup_Approval_Status";
                        command.CommandType = CommandType.StoredProcedure;

                        foreach (var hesccmId in request.selectedHesccmIds)
                        {
                            command.Parameters.Clear();

                            var param1 = command.CreateParameter();
                            param1.ParameterName = "@MI_Id";
                            param1.Value = request.MI_Id;
                            command.Parameters.Add(param1);

                            var param2 = command.CreateParameter();
                            param2.ParameterName = "@HESCCM_Id";
                            param2.Value = hesccmId;
                            command.Parameters.Add(param2);

                            var param3 = command.CreateParameter();
                            param3.ParameterName = "@ISMMPR_Id";
                            param3.Value = request.ISMMPR_Id;
                            command.Parameters.Add(param3);

                            var param4 = command.CreateParameter();
                            param4.ParameterName = "@Approval_Status";
                            param4.Value = (object)request.Approval_Status ?? DBNull.Value;
                            command.Parameters.Add(param4);

                            var param5 = command.CreateParameter();
                            param5.ParameterName = "@UpdatedBy";
                            param5.Value = request.UserId;
                            command.Parameters.Add(param5);

                            int rows = await command.ExecuteNonQueryAsync();
                            totalAffectedRows += rows;
                        }
                    }

                    request.returnval = true;
                }
                else
                {
                    request.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SaveApproval: " + ex.Message);
                request.returnval = false;
            }

            return request;
        }

        public async Task<Health_Entry_CheckupDTO> GetProjectSummaryReport(Health_Entry_CheckupDTO request)
        {
            var connection = _ATContext.Database.GetDbConnection();
            try
            {
                var summaryList = new List<HealthCheckupSummaryDTO>();

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // 1. Fetch Header Details (ProjectName, TechnicianName, InspectionDate, NextDueDate, ClientName)
                if (request.HCUE_Id > 0)
                {
                    try
                    {
                        using (var cmdHeader = connection.CreateCommand())
                        {
                            cmdHeader.CommandText = @"
                                SELECT TOP 1 
                                    h.HCUE_Id,
                                    h.ISMMPR_Id,
                                    p.ISMMPR_ProjectName,
                                    c.ISMMCLT_ClientName,
                                    h.Technician_Name,
                                    h.created_date,
                                    h.Next_Due_Date
                                FROM Health_Checkup_Entry h
                                LEFT JOIN ISM_Master_Project p ON h.ISMMPR_Id = p.ISMMPR_Id
                                LEFT JOIN ISM_Master_Client c ON CAST(h.Client_Id AS VARCHAR(50)) = CAST(c.ISMMCLT_Id AS VARCHAR(50))
                                WHERE h.HCUE_Id = @HCUE_Id";
                            cmdHeader.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                            using (var readerHeader = await cmdHeader.ExecuteReaderAsync())
                            {
                                if (await readerHeader.ReadAsync())
                                {
                                    request.projectName = readerHeader["ISMMPR_ProjectName"] != DBNull.Value ? readerHeader["ISMMPR_ProjectName"].ToString() : string.Empty;
                                    request.Technician_Name = readerHeader["Technician_Name"] != DBNull.Value ? readerHeader["Technician_Name"].ToString() : string.Empty;
                                    request.CreatedDate = readerHeader["created_date"] != DBNull.Value ? Convert.ToDateTime(readerHeader["created_date"]) : (DateTime?)null;

                                    if (readerHeader["Next_Due_Date"] != DBNull.Value)
                                    {
                                        request.Next_Due_Date = Convert.ToDateTime(readerHeader["Next_Due_Date"]);
                                    }

                                    request.clientName = readerHeader["ISMMCLT_ClientName"] != DBNull.Value ? readerHeader["ISMMCLT_ClientName"].ToString() : string.Empty;
                                }
                            }
                        }
                    }
                    catch (Exception exHeader)
                    {
                        Console.WriteLine("Error fetching header in GetProjectSummaryReport: " + exHeader.Message);
                    }

                    // 2. Fetch Systems Summary ONLY for the checkpoints actually saved for this specific HCUE_Id
                    try
                    {
                        using (var cmdSummary = connection.CreateCommand())
                        {
                            cmdSummary.CommandText = @"
                                SELECT 
                                    ms.System AS SystemName,
                                    mc.Category AS CategoryName,
                                    COUNT(m.HESCCM_Id) AS TotalItems,
                                    SUM(CASE WHEN st.StatusName LIKE '%OK%' OR st.StatusName LIKE '%Pass%' THEN 1 ELSE 0 END) AS OK_Count,
                                    SUM(CASE WHEN st.StatusName LIKE '%Attention%' THEN 1 ELSE 0 END) AS Attention_Count,
                                    SUM(CASE WHEN st.StatusName LIKE '%Fail%' OR st.StatusName LIKE '%Critical%' THEN 1 ELSE 0 END) AS Fail_Count,
                                    SUM(CASE WHEN st.StatusName LIKE '%Progress%' THEN 1 ELSE 0 END) AS InProgress_Count,
                                    SUM(CASE WHEN m.Health_Entry_Status_Id IS NULL OR st.StatusName IS NULL OR st.StatusName LIKE '%Not Checked%' THEN 1 ELSE 0 END) AS NotChecked_Count
                                FROM Health_Entry_System_Category_Checkpoint_Mapping m
                                INNER JOIN System_Category_Checkpoint_Mapping sccc ON m.SCCC_Id = sccc.SCCC_Id
                                INNER JOIN Master_System ms ON sccc.MS_Id = ms.MS_Id
                                INNER JOIN Master_Category mc ON sccc.MC_Id = mc.MC_Id
                                LEFT JOIN Health_Entry_Status st ON m.Health_Entry_Status_Id = st.Health_Entry_Status_Id
                                WHERE m.HCUE_Id = @HCUE_Id AND (m.ActiveFlag = 1 OR m.ActiveFlag IS NULL)
                                GROUP BY ms.System, mc.Category
                                ORDER BY ms.System, mc.Category";
                            cmdSummary.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                            using (var readerSummary = await cmdSummary.ExecuteReaderAsync())
                            {
                                while (await readerSummary.ReadAsync())
                                {
                                    summaryList.Add(new HealthCheckupSummaryDTO
                                    {
                                        SystemName = readerSummary["SystemName"] != DBNull.Value ? readerSummary["SystemName"].ToString() : string.Empty,
                                        CategoryName = readerSummary["CategoryName"] != DBNull.Value ? readerSummary["CategoryName"].ToString() : string.Empty,
                                        TotalItems = readerSummary["TotalItems"] != DBNull.Value ? Convert.ToInt32(readerSummary["TotalItems"]) : 0,
                                        OkCount = readerSummary["OK_Count"] != DBNull.Value ? Convert.ToInt32(readerSummary["OK_Count"]) : 0,
                                        AttentionCount = readerSummary["Attention_Count"] != DBNull.Value ? Convert.ToInt32(readerSummary["Attention_Count"]) : 0,
                                        FailCount = readerSummary["Fail_Count"] != DBNull.Value ? Convert.ToInt32(readerSummary["Fail_Count"]) : 0,
                                        InProgressCount = readerSummary["InProgress_Count"] != DBNull.Value ? Convert.ToInt32(readerSummary["InProgress_Count"]) : 0,
                                        NotCheckedCount = readerSummary["NotChecked_Count"] != DBNull.Value ? Convert.ToInt32(readerSummary["NotChecked_Count"]) : 0
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception exSum)
                    {
                        Console.WriteLine("Error calculating specific summary in GetProjectSummaryReport: " + exSum.Message);
                    }
                }

                // Fallback to SP if summaryList is empty and HCUE_Id was not provided or had no records
                if (summaryList.Count == 0)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "dbo.SP_Get_Health_Checkup_Dashboard_Template";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@MI_Id", request.MI_Id));
                        command.Parameters.Add(new SqlParameter("@ISMMPR_Id", request.ISMMPR_Id));
                        command.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                if (string.IsNullOrEmpty(request.projectName))
                                    request.projectName = reader["ProjectName"] != DBNull.Value ? reader["ProjectName"].ToString() : string.Empty;
                                if (string.IsNullOrEmpty(request.Technician_Name))
                                    request.Technician_Name = reader["TechnicianName"] != DBNull.Value ? reader["TechnicianName"].ToString() : string.Empty;
                                if (!request.CreatedDate.HasValue)
                                    request.CreatedDate = reader["InspectionDate"] != DBNull.Value ? Convert.ToDateTime(reader["InspectionDate"]) : (DateTime?)null;
                                if (request.Next_Due_Date == default(DateTime) && reader["NextDueDate"] != DBNull.Value)
                                    request.Next_Due_Date = Convert.ToDateTime(reader["NextDueDate"]);
                                if (string.IsNullOrEmpty(request.clientName))
                                    request.clientName = reader["ClientName"] != DBNull.Value ? reader["ClientName"].ToString() : string.Empty;
                            }

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    summaryList.Add(new HealthCheckupSummaryDTO
                                    {
                                        SystemName = reader["SystemName"] != DBNull.Value ? reader["SystemName"].ToString() : string.Empty,
                                        CategoryName = reader["CategoryName"] != DBNull.Value ? reader["CategoryName"].ToString() : string.Empty,
                                        TotalItems = reader["TotalItems"] != DBNull.Value ? Convert.ToInt32(reader["TotalItems"]) : 0,
                                        OkCount = reader["OK_Count"] != DBNull.Value ? Convert.ToInt32(reader["OK_Count"]) : 0,
                                        AttentionCount = reader["Attention_Count"] != DBNull.Value ? Convert.ToInt32(reader["Attention_Count"]) : 0,
                                        FailCount = reader["Fail_Count"] != DBNull.Value ? Convert.ToInt32(reader["Fail_Count"]) : 0,
                                        InProgressCount = reader["InProgress_Count"] != DBNull.Value ? Convert.ToInt32(reader["InProgress_Count"]) : 0,
                                        NotCheckedCount = reader["NotChecked_Count"] != DBNull.Value ? Convert.ToInt32(reader["NotChecked_Count"]) : 0
                                    });
                                }
                            }
                        }
                    }
                }
                    
                request.summaryReportList = summaryList.ToArray();
                request.returnval = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing GetProjectSummaryReport: " + ex.Message);
                request.returnval = false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return request;
        }

        public async Task<Health_Entry_CheckupDTO> GetDetailedCheckupReport(Health_Entry_CheckupDTO request)
        {
            var connection = _ATContext.Database.GetDbConnection();
            try
            {
                var detailedList = new List<SystemCategoryCheckpointDto>();

                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // 1. Fetch Header Details (ProjectName, TechnicianName, InspectionDate, NextDueDate, ClientName)
                if (request.HCUE_Id > 0)
                {
                    try
                    {
                        using (var cmdFallback = connection.CreateCommand())
                        {
                            cmdFallback.CommandText = @"
                                SELECT TOP 1 
                                    h.HCUE_Id,
                                    h.ISMMPR_Id,
                                    p.ISMMPR_ProjectName,
                                    c.ISMMCLT_ClientName,
                                    h.Technician_Name,
                                    h.created_date,
                                    h.Next_Due_Date
                                FROM Health_Checkup_Entry h
                                LEFT JOIN ISM_Master_Client c ON CAST(h.Client_Id AS VARCHAR(50)) = CAST(c.ISMMCLT_Id AS VARCHAR(50))
                                LEFT JOIN ISM_Master_Project p ON h.ISMMPR_Id = p.ISMMPR_Id
                                WHERE h.HCUE_Id = @HCUE_Id";
                            cmdFallback.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                            using (var readerFallback = await cmdFallback.ExecuteReaderAsync())
                            {
                                if (await readerFallback.ReadAsync())
                                {
                                    if (readerFallback["ISMMCLT_ClientName"] != DBNull.Value)
                                        request.clientName = readerFallback["ISMMCLT_ClientName"].ToString();

                                    if (readerFallback["ISMMPR_ProjectName"] != DBNull.Value)
                                        request.projectName = readerFallback["ISMMPR_ProjectName"].ToString();

                                    if (readerFallback["Technician_Name"] != DBNull.Value)
                                        request.Technician_Name = readerFallback["Technician_Name"].ToString();

                                    if (readerFallback["created_date"] != DBNull.Value)
                                        request.CreatedDate = Convert.ToDateTime(readerFallback["created_date"]);

                                    if (readerFallback["Next_Due_Date"] != DBNull.Value)
                                        request.Next_Due_Date = Convert.ToDateTime(readerFallback["Next_Due_Date"]);
                                }
                            }
                        }
                    }
                    catch (Exception exFallback)
                    {
                        Console.WriteLine("Error in header lookup: " + exFallback.Message);
                    }
                }

                // If header fields still empty, fallback to SP
                if (string.IsNullOrEmpty(request.clientName) || string.IsNullOrEmpty(request.projectName))
                {
                    try
                    {
                        using (var cmdHeader = connection.CreateCommand())
                        {
                            cmdHeader.CommandText = "dbo.SP_Get_Health_Checkup_Dashboard_Template";
                            cmdHeader.CommandType = CommandType.StoredProcedure;

                            cmdHeader.Parameters.Add(new SqlParameter("@MI_Id", request.MI_Id));
                            cmdHeader.Parameters.Add(new SqlParameter("@ISMMPR_Id", request.ISMMPR_Id));
                            cmdHeader.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                            using (var readerHeader = await cmdHeader.ExecuteReaderAsync())
                            {
                                if (await readerHeader.ReadAsync())
                                {
                                    if (string.IsNullOrEmpty(request.projectName))
                                        request.projectName = readerHeader["ProjectName"] != DBNull.Value ? readerHeader["ProjectName"].ToString() : string.Empty;
                                    if (string.IsNullOrEmpty(request.Technician_Name))
                                        request.Technician_Name = readerHeader["TechnicianName"] != DBNull.Value ? readerHeader["TechnicianName"].ToString() : string.Empty;
                                    if (!request.CreatedDate.HasValue)
                                        request.CreatedDate = readerHeader["InspectionDate"] != DBNull.Value ? Convert.ToDateTime(readerHeader["InspectionDate"]) : (DateTime?)null;

                                    if (readerHeader["NextDueDate"] != DBNull.Value)
                                    {
                                        request.Next_Due_Date = Convert.ToDateTime(readerHeader["NextDueDate"]);
                                    }

                                    if (string.IsNullOrEmpty(request.clientName))
                                        request.clientName = readerHeader["ClientName"] != DBNull.Value ? readerHeader["ClientName"].ToString() : string.Empty;
                                }
                            }
                        }
                    }
                    catch (Exception exHeader)
                    {
                        Console.WriteLine("Error in SP header lookup: " + exHeader.Message);
                    }
                }

                // 2. Fetch EXACT Checkpoints saved for this HCUE_Id
                if (request.HCUE_Id > 0)
                {
                    try
                    {
                        using (var cmdSpecificCheckpoints = connection.CreateCommand())
                        {
                            cmdSpecificCheckpoints.CommandText = @"
                                SELECT 
                                    (ms.System + ' - ' + mc.Category) AS SystemCategoryHeader,
                                    ms.System,
                                    mc.Category,
                                    ISNULL(mcp.CheckPoint, 'Check Point') AS CheckPoint,
                                    sccc.SCCC_Id,
                                    sccc.MS_Id,
                                    sccc.MC_Id,
                                    m.Health_Entry_Status_Id,
                                    ISNULL(st.StatusName, 'OK') AS StatusName,
                                    ISNULL(m.Approval_Status, 'Pending') AS Approval_Status,
                                    ISNULL(m.Remarks, '') AS Remarks,
                                    ISNULL(m.Priority, 'Medium') AS Priority,
                                    ISNULL(h.Technician_Name, '-') AS CheckedBy,
                                    '-' AS VerifiedBy,
                                    ISNULL(m.CreatedDate, h.created_date) AS CheckedDate
                                FROM Health_Entry_System_Category_Checkpoint_Mapping m
                                INNER JOIN Health_Checkup_Entry h ON m.HCUE_Id = h.HCUE_Id
                                INNER JOIN System_Category_Checkpoint_Mapping sccc ON m.SCCC_Id = sccc.SCCC_Id
                                INNER JOIN Master_System ms ON sccc.MS_Id = ms.MS_Id
                                INNER JOIN Master_Category mc ON sccc.MC_Id = mc.MC_Id
                                LEFT JOIN Master_CheckPoint mcp ON (
                                    TRY_CAST(sccc.MCP_Id AS BIGINT) = mcp.MCP_Id 
                                    OR CAST(sccc.MCP_Id AS VARCHAR(50)) = CAST(mcp.MCP_Id AS VARCHAR(50))
                                )
                                LEFT JOIN Health_Entry_Status st ON m.Health_Entry_Status_Id = st.Health_Entry_Status_Id
                                WHERE m.HCUE_Id = @HCUE_Id AND (m.ActiveFlag = 1 OR m.ActiveFlag IS NULL)
                                ORDER BY m.HESCCM_Id ASC";
                            cmdSpecificCheckpoints.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                            using (var readerSpecific = await cmdSpecificCheckpoints.ExecuteReaderAsync())
                            {
                                while (await readerSpecific.ReadAsync())
                                {
                                    detailedList.Add(new SystemCategoryCheckpointDto
                                    {
                                        SystemCategoryHeader = readerSpecific["SystemCategoryHeader"] != DBNull.Value ? readerSpecific["SystemCategoryHeader"].ToString() : string.Empty,
                                        System = readerSpecific["System"] != DBNull.Value ? readerSpecific["System"].ToString() : string.Empty,
                                        Category = readerSpecific["Category"] != DBNull.Value ? readerSpecific["Category"].ToString() : string.Empty,
                                        CheckPoint = readerSpecific["CheckPoint"] != DBNull.Value ? readerSpecific["CheckPoint"].ToString() : string.Empty,
                                        SCCC_Id = readerSpecific["SCCC_Id"] != DBNull.Value ? Convert.ToInt64(readerSpecific["SCCC_Id"]) : 0,
                                        MS_Id = readerSpecific["MS_Id"] != DBNull.Value ? Convert.ToInt64(readerSpecific["MS_Id"]) : 0,
                                        MC_Id = readerSpecific["MC_Id"] != DBNull.Value ? Convert.ToInt64(readerSpecific["MC_Id"]) : 0,
                                        Health_Entry_Status_Id = readerSpecific["Health_Entry_Status_Id"] != DBNull.Value ? Convert.ToInt64(readerSpecific["Health_Entry_Status_Id"]) : (long?)null,
                                        Approval_Status = readerSpecific["Approval_Status"] != DBNull.Value ? readerSpecific["Approval_Status"].ToString() : "Pending",
                                        Remarks = readerSpecific["Remarks"] != DBNull.Value ? readerSpecific["Remarks"].ToString() : string.Empty,
                                        Priority = readerSpecific["Priority"] != DBNull.Value ? readerSpecific["Priority"].ToString() : "Medium",
                                        StatusName = readerSpecific["StatusName"] != DBNull.Value ? readerSpecific["StatusName"].ToString() : string.Empty,
                                        CheckedBy = readerSpecific["CheckedBy"] != DBNull.Value ? readerSpecific["CheckedBy"].ToString() : (!string.IsNullOrEmpty(request.Technician_Name) ? request.Technician_Name : "-"),
                                        VerifiedBy = readerSpecific["VerifiedBy"] != DBNull.Value ? readerSpecific["VerifiedBy"].ToString() : "-",
                                        CheckedDate = readerSpecific["CheckedDate"] != DBNull.Value ? Convert.ToDateTime(readerSpecific["CheckedDate"]) : (request.CreatedDate.HasValue ? request.CreatedDate : (DateTime?)null)
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception exSpecific)
                    {
                        Console.WriteLine("Error in specific checkpoint query in GetDetailedCheckupReport: " + exSpecific.Message);
                    }
                }

                // Fallback to SP if detailedList is still empty
                if (detailedList.Count == 0)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "dbo.SP_GetSystemCategoryCheckpoints";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@MI_Id", request.MI_Id));
                        command.Parameters.Add(new SqlParameter("@ISMMPR_Id", request.ISMMPR_Id));
                        command.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var colNames = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                            while (await reader.ReadAsync())
                            {
                                detailedList.Add(new SystemCategoryCheckpointDto
                                {
                                    SystemCategoryHeader = reader["SystemCategoryHeader"] != DBNull.Value ? reader["SystemCategoryHeader"].ToString() : string.Empty,
                                    System = reader["System"] != DBNull.Value ? reader["System"].ToString() : string.Empty,
                                    Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : string.Empty,
                                    CheckPoint = reader["CheckPoint"] != DBNull.Value ? reader["CheckPoint"].ToString() : string.Empty,
                                    SCCC_Id = reader["SCCC_Id"] != DBNull.Value ? Convert.ToInt64(reader["SCCC_Id"]) : 0,
                                    MS_Id = reader["MS_Id"] != DBNull.Value ? Convert.ToInt64(reader["MS_Id"]) : 0,
                                    MC_Id = reader["MC_Id"] != DBNull.Value ? Convert.ToInt64(reader["MC_Id"]) : 0,
                                    Health_Entry_Status_Id = reader["Health_Entry_Status_Id"] != DBNull.Value ? Convert.ToInt64(reader["Health_Entry_Status_Id"]) : (long?)null,
                                    Approval_Status = reader["Approval_Status"] != DBNull.Value ? reader["Approval_Status"].ToString() : "Pending",
                                    Remarks = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString() : string.Empty,
                                    Priority = reader["Priority"] != DBNull.Value ? reader["Priority"].ToString() : "Medium",
                                    StatusName = colNames.Contains("StatusName") && reader["StatusName"] != DBNull.Value ? reader["StatusName"].ToString() : string.Empty,
                                    CheckedBy = colNames.Contains("CheckedBy") && reader["CheckedBy"] != DBNull.Value ? reader["CheckedBy"].ToString() : string.Empty,
                                    VerifiedBy = colNames.Contains("VerifiedBy") && reader["VerifiedBy"] != DBNull.Value ? reader["VerifiedBy"].ToString() : string.Empty,
                                    CheckedDate = colNames.Contains("CheckedDate") && reader["CheckedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CheckedDate"]) : (DateTime?)null
                                });
                            }
                        }
                    }

                    if (request.HCUE_Id > 0)
                    {
                        detailedList = detailedList.Where(x => x.Health_Entry_Status_Id.HasValue && x.Health_Entry_Status_Id.Value > 0).ToList();
                    }
                }

                // BIND ARRAY TO DTO PROPERTY
                request.detailedReportList = detailedList.ToArray();
                request.returnval = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetDetailedCheckupReport: " + ex.Message);
                request.returnval = false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return request;
        }

        public async Task<Health_Entry_CheckupDTO> GetSignOffReportDetails(Health_Entry_CheckupDTO request)
        {
            var connection = _ATContext.Database.GetDbConnection();
            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // =========================================================================
                // 1. If HCUE_Id > 0, fetch EXACT record details directly for this Entry
                // =========================================================================
                if (request.HCUE_Id > 0)
                {
                    try
                    {
                        using (var cmdSpecific = connection.CreateCommand())
                        {
                            cmdSpecific.CommandText = @"
                                SELECT TOP 1 
                                    h.HCUE_Id,
                                    h.ISMMPR_Id,
                                    h.Client_Id,
                                    p.ISMMPR_ProjectName,
                                    p.ISMMPR_Desc AS ProjectAddress,
                                    ISNULL(c.ISMMCLT_ClientName, ISNULL(c2.ISMMCLT_ClientName, '')) AS ISMMCLT_ClientName,
                                    h.Technician_Name,
                                    h.Company_Name,
                                    h.Contact_Number,
                                    h.Emailid,
                                    h.Designation,
                                    h.created_date,
                                    h.Next_Due_Date,
                                    h.Remarks
                                FROM Health_Checkup_Entry h
                                LEFT JOIN ISM_Master_Project p ON h.ISMMPR_Id = p.ISMMPR_Id
                                LEFT JOIN ISM_Master_Client c ON (
                                    TRY_CAST(h.Client_Id AS BIGINT) = c.ISMMCLT_Id 
                                    OR CAST(h.Client_Id AS VARCHAR(50)) = CAST(c.ISMMCLT_Id AS VARCHAR(50))
                                )
                                LEFT JOIN ISM_Master_Client c2 ON CAST(h.Client_Id AS VARCHAR(50)) = c2.ISMMCLT_ClientName
                                WHERE h.HCUE_Id = @HCUE_Id";
                            cmdSpecific.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                            using (var readerSpecific = await cmdSpecific.ExecuteReaderAsync())
                            {
                                if (await readerSpecific.ReadAsync())
                                {
                                    // 1. Technical Sign-Off
                                    request.technicalSignOff = new TechnicalSignOffDTO
                                    {
                                        ISMMPR_Id = readerSpecific["ISMMPR_Id"] != DBNull.Value ? Convert.ToInt64(readerSpecific["ISMMPR_Id"]) : request.ISMMPR_Id,
                                        TechnicianName = readerSpecific["Technician_Name"] != DBNull.Value ? readerSpecific["Technician_Name"].ToString() : string.Empty,
                                        CompanyName = readerSpecific["Company_Name"] != DBNull.Value ? readerSpecific["Company_Name"].ToString() : string.Empty,
                                        ContactNumber = readerSpecific["Contact_Number"] != DBNull.Value ? readerSpecific["Contact_Number"].ToString() : string.Empty,
                                        Emailid = readerSpecific["Emailid"] != DBNull.Value ? readerSpecific["Emailid"].ToString() : string.Empty,
                                        Date = readerSpecific["created_date"] != DBNull.Value ? Convert.ToDateTime(readerSpecific["created_date"]) : (DateTime?)null
                                    };

                                    // Resolve Client Name
                                    string resolvedClientName = readerSpecific["ISMMCLT_ClientName"] != DBNull.Value ? readerSpecific["ISMMCLT_ClientName"].ToString() : string.Empty;
                                    if (string.IsNullOrWhiteSpace(resolvedClientName) && readerSpecific["Client_Id"] != DBNull.Value)
                                    {
                                        resolvedClientName = readerSpecific["Client_Id"].ToString();
                                    }

                                    // 2. Client Owner Verification
                                    request.clientOwnerVerification = new ClientOwnerVerificationDTO
                                    {
                                        ISMMPR_Id = readerSpecific["ISMMPR_Id"] != DBNull.Value ? Convert.ToInt64(readerSpecific["ISMMPR_Id"]) : request.ISMMPR_Id,
                                        ClientName = resolvedClientName,
                                        Designation = readerSpecific["Designation"] != DBNull.Value ? readerSpecific["Designation"].ToString() : string.Empty,
                                        Date = readerSpecific["created_date"] != DBNull.Value ? Convert.ToDateTime(readerSpecific["created_date"]) : (DateTime?)null,
                                        CurrentStatus = "Pending"
                                    };

                                    // 3. Inspection Details
                                    request.inspectionDetailsSignOff = new InspectionDetailsSignOffDTO
                                    {
                                        ReportRefNo = "RPT-" + request.HCUE_Id,
                                        ProjectName = readerSpecific["ISMMPR_ProjectName"] != DBNull.Value ? readerSpecific["ISMMPR_ProjectName"].ToString() : string.Empty,
                                        Address = readerSpecific["ProjectAddress"] != DBNull.Value && !string.IsNullOrWhiteSpace(readerSpecific["ProjectAddress"].ToString()) ? readerSpecific["ProjectAddress"].ToString() : "N/A",
                                        InspectionDate = readerSpecific["created_date"] != DBNull.Value ? Convert.ToDateTime(readerSpecific["created_date"]) : (DateTime?)null,
                                        InspectionTime = readerSpecific["created_date"] != DBNull.Value ? Convert.ToDateTime(readerSpecific["created_date"]).ToString("hh:mm tt") : string.Empty,
                                        NextDueDate = readerSpecific["Next_Due_Date"] != DBNull.Value ? Convert.ToDateTime(readerSpecific["Next_Due_Date"]) : (DateTime?)null
                                    };

                                    // Root fields
                                    request.clientName = resolvedClientName;
                                    request.projectName = request.inspectionDetailsSignOff.ProjectName;
                                    request.Technician_Name = request.technicalSignOff.TechnicianName;
                                    request.CreatedDate = request.inspectionDetailsSignOff.InspectionDate;
                                    if (request.inspectionDetailsSignOff.NextDueDate.HasValue)
                                    {
                                        request.Next_Due_Date = request.inspectionDetailsSignOff.NextDueDate.Value;
                                    }
                                }
                            }
                        }

                        // Fetch real Approval Status from checkpoints mapping table
                        using (var cmdAppr = connection.CreateCommand())
                        {
                            cmdAppr.CommandText = "SELECT TOP 1 Approval_Status FROM Health_Entry_System_Category_Checkpoint_Mapping WHERE HCUE_Id = @HCUE_Id AND Approval_Status IS NOT NULL";
                            cmdAppr.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));
                            var apprResult = await cmdAppr.ExecuteScalarAsync();
                            if (apprResult != null && apprResult != DBNull.Value && !string.IsNullOrWhiteSpace(apprResult.ToString()))
                            {
                                var st = apprResult.ToString().Trim();
                                if (st.Equals("APPROVED", StringComparison.OrdinalIgnoreCase))
                                {
                                    request.clientOwnerVerification.CurrentStatus = "APPROVED";
                                }
                                else if (st.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
                                {
                                    request.clientOwnerVerification.CurrentStatus = "REJECTED";
                                }
                                else
                                {
                                    request.clientOwnerVerification.CurrentStatus = "Pending";
                                }
                            }
                            else
                            {
                                request.clientOwnerVerification.CurrentStatus = "Pending";
                            }
                        }

                        // 4. Calculate Inspection Summary specifically for this entry
                        using (var cmdSum = connection.CreateCommand())
                        {
                            cmdSum.CommandText = @"
                                SELECT 
                                    COUNT(DISTINCT sccc.MS_Id) AS Total_Systems_Inspected,
                                    COUNT(m.HESCCM_Id) AS Total_Checkpoints,
                                    SUM(CASE WHEN st.StatusName LIKE '%OK%' OR st.StatusName LIKE '%Pass%' THEN 1 ELSE 0 END) AS OK,
                                    SUM(CASE WHEN st.StatusName LIKE '%Attention%' THEN 1 ELSE 0 END) AS Attention,
                                    SUM(CASE WHEN st.StatusName LIKE '%Fail%' THEN 1 ELSE 0 END) AS Fail,
                                    SUM(CASE WHEN st.StatusName LIKE '%Progress%' THEN 1 ELSE 0 END) AS [In Progress],
                                    SUM(CASE WHEN m.Health_Entry_Status_Id IS NULL OR st.StatusName IS NULL OR st.StatusName LIKE '%Not Checked%' THEN 1 ELSE 0 END) AS Not_Checked
                                FROM Health_Entry_System_Category_Checkpoint_Mapping m
                                LEFT JOIN Health_Entry_Status st ON m.Health_Entry_Status_Id = st.Health_Entry_Status_Id
                                LEFT JOIN AT_System_Category_Checkpoint_Mapping sccc ON m.SCCC_Id = sccc.SCCC_Id
                                WHERE m.HCUE_Id = @HCUE_Id";
                            cmdSum.Parameters.Add(new SqlParameter("@HCUE_Id", request.HCUE_Id));

                            using (var rSum = await cmdSum.ExecuteReaderAsync())
                            {
                                if (await rSum.ReadAsync())
                                {
                                    int totalCheckpoints = rSum["Total_Checkpoints"] != DBNull.Value ? Convert.ToInt32(rSum["Total_Checkpoints"]) : 0;
                                    int okCount = rSum["OK"] != DBNull.Value ? Convert.ToInt32(rSum["OK"]) : 0;
                                    int attentionCount = rSum["Attention"] != DBNull.Value ? Convert.ToInt32(rSum["Attention"]) : 0;
                                    int failCount = rSum["Fail"] != DBNull.Value ? Convert.ToInt32(rSum["Fail"]) : 0;
                                    int inProgressCount = rSum["In Progress"] != DBNull.Value ? Convert.ToInt32(rSum["In Progress"]) : 0;
                                    int notCheckedCount = rSum["Not_Checked"] != DBNull.Value ? Convert.ToInt32(rSum["Not_Checked"]) : 0;
                                    int totalSystems = rSum["Total_Systems_Inspected"] != DBNull.Value ? Convert.ToInt32(rSum["Total_Systems_Inspected"]) : 0;

                                    decimal score = totalCheckpoints > 0 ? Math.Round(((decimal)okCount / totalCheckpoints) * 100, 1) : 0;
                                    request.inspectionSummarySignOff = new InspectionSummarySignOffDTO
                                    {
                                        ISMMPR_Id = request.ISMMPR_Id,
                                        TotalSystemsInspected = totalSystems,
                                        TotalCheckpoints = totalCheckpoints,
                                        OkCount = okCount,
                                        AttentionCount = attentionCount,
                                        FailCount = failCount,
                                        InProgressCount = inProgressCount,
                                        NotCheckedCount = notCheckedCount,
                                        OverallHealthScorePercent = score
                                    };
                                }
                            }
                        }
                    }
                    catch (Exception exSpecific)
                    {
                        Console.WriteLine("Error in specific record lookup for SignOff: " + exSpecific.Message);
                    }
                }
                else
                {
                    // =========================================================================
                    // Fallback to project-level SPs only if HCUE_Id == 0
                    // =========================================================================
                    try
                    {
                        using (var cmd1 = connection.CreateCommand())
                        {
                            cmd1.CommandText = "Inpection_Details";
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add(new SqlParameter("@ISMMPR_Id", request.ISMMPR_Id));

                            using (var reader1 = await cmd1.ExecuteReaderAsync())
                            {
                                if (await reader1.ReadAsync())
                                {
                                    request.inspectionDetailsSignOff = new InspectionDetailsSignOffDTO
                                    {
                                        ReportRefNo = reader1["REPORT_REF_NO"] != DBNull.Value ? reader1["REPORT_REF_NO"].ToString() : string.Empty,
                                        ProjectName = reader1["ISMMPR_ProjectName"] != DBNull.Value ? reader1["ISMMPR_ProjectName"].ToString() : string.Empty,
                                        Address = reader1["Address"] != DBNull.Value ? reader1["Address"].ToString() : "N/A",
                                        InspectionDate = reader1["InspectionDate"] != DBNull.Value ? Convert.ToDateTime(reader1["InspectionDate"]) : (DateTime?)null,
                                        InspectionTime = reader1["InspectionTime"] != DBNull.Value ? reader1["InspectionTime"].ToString() : string.Empty,
                                        NextDueDate = reader1["Next_Due_Date"] != DBNull.Value ? Convert.ToDateTime(reader1["Next_Due_Date"]) : (DateTime?)null
                                    };
                                }
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine("Error in Inpection_Details SP: " + ex1.Message);
                    }

                    try
                    {
                        using (var cmd2 = connection.CreateCommand())
                        {
                            cmd2.CommandText = "Inspection_Summary";
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Add(new SqlParameter("@ISMMPR_Id", request.ISMMPR_Id));

                            using (var reader2 = await cmd2.ExecuteReaderAsync())
                            {
                                if (await reader2.ReadAsync())
                                {
                                    request.inspectionSummarySignOff = new InspectionSummarySignOffDTO
                                    {
                                        ISMMPR_Id = reader2["ISMMPR_Id"] != DBNull.Value ? Convert.ToInt64(reader2["ISMMPR_Id"]) : 0,
                                        TotalSystemsInspected = reader2["Total_Systems_Inspected"] != DBNull.Value ? Convert.ToInt32(reader2["Total_Systems_Inspected"]) : 0,
                                        TotalCheckpoints = reader2["Total_Checkpoints"] != DBNull.Value ? Convert.ToInt32(reader2["Total_Checkpoints"]) : 0,
                                        OkCount = reader2["OK"] != DBNull.Value ? Convert.ToInt32(reader2["OK"]) : 0,
                                        AttentionCount = reader2["Attention"] != DBNull.Value ? Convert.ToInt32(reader2["Attention"]) : 0,
                                        FailCount = reader2["Fail"] != DBNull.Value ? Convert.ToInt32(reader2["Fail"]) : 0,
                                        InProgressCount = reader2["In Progress"] != DBNull.Value ? Convert.ToInt32(reader2["In Progress"]) : 0,
                                        NotCheckedCount = reader2["Not_Checked"] != DBNull.Value ? Convert.ToInt32(reader2["Not_Checked"]) : 0,
                                        OverallHealthScorePercent = reader2["Overall_Health_Score_Percent"] != DBNull.Value ? Convert.ToDecimal(reader2["Overall_Health_Score_Percent"]) : (decimal?)null
                                    };
                                }
                            }
                        }
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Error in Inspection_Summary SP: " + ex2.Message);
                    }

                    try
                    {
                        using (var cmd3 = connection.CreateCommand())
                        {
                            cmd3.CommandText = "Technical_sign_off";
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.Add(new SqlParameter("@ISMMPR_Id", request.ISMMPR_Id));

                            using (var reader3 = await cmd3.ExecuteReaderAsync())
                            {
                                if (await reader3.ReadAsync())
                                {
                                    request.technicalSignOff = new TechnicalSignOffDTO
                                    {
                                        ISMMPR_Id = reader3["ISMMPR_Id"] != DBNull.Value ? Convert.ToInt64(reader3["ISMMPR_Id"]) : 0,
                                        TechnicianName = reader3["Technician_Name"] != DBNull.Value ? reader3["Technician_Name"].ToString() : string.Empty,
                                        CompanyName = reader3["Company_Name"] != DBNull.Value ? reader3["Company_Name"].ToString() : string.Empty,
                                        ContactNumber = reader3["Contact_Number"] != DBNull.Value ? reader3["Contact_Number"].ToString() : string.Empty,
                                        Emailid = reader3["Emailid"] != DBNull.Value ? reader3["Emailid"].ToString() : string.Empty,
                                        Date = reader3["date"] != DBNull.Value ? Convert.ToDateTime(reader3["date"]) : (DateTime?)null
                                    };
                                }
                            }
                        }
                    }
                    catch (Exception ex3)
                    {
                        Console.WriteLine("Error in Technical_sign_off SP: " + ex3.Message);
                    }

                    try
                    {
                        using (var cmd4 = connection.CreateCommand())
                        {
                            cmd4.CommandText = "client_owner_verification";
                            cmd4.CommandType = CommandType.StoredProcedure;
                            cmd4.Parameters.Add(new SqlParameter("@ISMMPR_Id", request.ISMMPR_Id));

                            using (var reader4 = await cmd4.ExecuteReaderAsync())
                            {
                                if (await reader4.ReadAsync())
                                {
                                    request.clientOwnerVerification = new ClientOwnerVerificationDTO
                                    {
                                        ISMMPR_Id = reader4["ISMMPR_Id"] != DBNull.Value ? Convert.ToInt64(reader4["ISMMPR_Id"]) : 0,
                                        ClientName = reader4["Client_Name"] != DBNull.Value ? reader4["Client_Name"].ToString() : string.Empty,
                                        Designation = reader4["Designation"] != DBNull.Value ? reader4["Designation"].ToString() : string.Empty,
                                        Date = reader4["date"] != DBNull.Value ? Convert.ToDateTime(reader4["date"]) : (DateTime?)null,
                                        CurrentStatus = "Pending"
                                    };
                                }
                            }
                        }
                    }
                    catch (Exception ex4)
                    {
                        Console.WriteLine("Error in client_owner_verification SP: " + ex4.Message);
                    }
                }

                if (request.clientOwnerVerification != null && string.IsNullOrEmpty(request.clientOwnerVerification.CurrentStatus))
                {
                    request.clientOwnerVerification.CurrentStatus = "Pending";
                }

                request.returnval = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetSignOffReportDetails: " + ex.Message);
                request.returnval = false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return request;
        }

    }
}
