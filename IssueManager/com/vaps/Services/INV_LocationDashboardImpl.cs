using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class INV_LocationDashboardImpl : Interfaces.INV_LocationDashboardInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public INV_LocationDashboardImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public async Task<INV_LocationDashboardDTO> getfloordetail(INV_LocationDashboardDTO data)
        {
            try
            {
                List<INV_LocationDashboardDTO> Tasklist = new List<INV_LocationDashboardDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FloorRoom_Deatils";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt) { Value = data.MI_ID });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.floordetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //INV_Dashboard_Assets
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Dashboard_Assets";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = 0 });
                    cmd.Parameters.Add(new SqlParameter("@INVMLO_Id", SqlDbType.VarChar) { Value = 0 });

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
                                Tasklist.Add(new INV_LocationDashboardDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),


                                });
                            }
                        }
                        data.employeedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "GetEmployeeLists";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.checkoutemployeelist = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                //if (Tasklist != null && Tasklist.Count > 0)
                //{
                //    List<long> HRME_Id = new List<long>();
                //    foreach (var d in Tasklist)
                //    {
                //        HRME_Id.Add(d.HRME_Id);
                //    }

                //    data.checkoutemployeelist = _IssueContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && !HRME_Id.Contains(t.HRME_Id)).Distinct().ToArray();
                //    //data.checkoutemployeelist = _IssueContext.MasterEmployee.Where(t => t.HRME_LeftFlag == false && !HRME_Id.Contains(t.HRME_Id)).Distinct().ToArray();

                //}
                //else
                //{
                //    data.checkoutemployeelist = _IssueContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToArray();
                //}

                data.checkoutemployeelist = _IssueContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToArray();

                //INV_VendorDashboard Voucher_Details
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_VendorDashboard";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Onload" });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id", SqlDbType.VarChar) { Value = 0 });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Voucher_Details = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                // data.Supplier_Details
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_VendorDashboard";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Suplier" });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id", SqlDbType.VarChar) { Value = 0 });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Supplier_Details = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_LocationDashboardSP";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.inv_locationdashboardsp_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;

        }


        //ITTeams Dashboard
        public INV_LocationDashboardDTO getemployeedetail(INV_LocationDashboardDTO data)
        {
            try
            {
                List<INV_LocationDashboardDTO> Tasklist = new List<INV_LocationDashboardDTO>();
                if (data.Flag == "Dashboard")
                {

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_AseetsDashboard";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "1" });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@INVMLO_Id", SqlDbType.VarChar) { Value = 0 });

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
                                    Tasklist.Add(new INV_LocationDashboardDTO
                                    {
                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                        User_Id = Convert.ToInt64(dataReader["INVMLO_Id"]),


                                    });
                                }
                            }
                            // data.employeedetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (Tasklist != null && Tasklist.Count > 0)
                    {
                        data.User_Id = Tasklist[0].User_Id;
                    }
                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Dashboard_Assets";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@INVMLO_Id", SqlDbType.VarChar) { Value = data.User_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }

                        // Check whether all items are already confirmed or if any pending confirmation exists
                        bool hasPendingConfirmation = false;
                        var filteredEmployeeDetails = new List<dynamic>();

                        if (retObject != null && retObject.Count > 0)
                        {
                            var invatcoIds = new List<long>();
                            foreach (IDictionary<string, object> row in retObject)
                            {
                                long? coId = GetInvatcoId(row);
                                if (coId.HasValue && coId.Value > 0)
                                {
                                    invatcoIds.Add(coId.Value);
                                }
                            }

                            if (invatcoIds.Count > 0)
                            {
                                var checkoutRecords = _IssueContext.INV_AssetTag_CheckOutDMO.Where(r => invatcoIds.Contains(r.INVATCO_Id)).ToList();
                                foreach (IDictionary<string, object> row in retObject)
                                {
                                    long? coId = GetInvatcoId(row);
                                    if (coId.HasValue && coId.Value > 0)
                                    {
                                        long id = coId.Value;
                                        var rec = checkoutRecords.FirstOrDefault(r => r.INVATCO_Id == id);
                                        if (rec != null)
                                        {
                                            string remarks = rec.INVATCO_CheckOutRemarks ?? "";
                                            SetOrUpdateKey(row, "INVATCO_CheckOutRemarks", remarks);

                                            bool isConfirmed = remarks.Trim().Equals("Confirmed by Employee", StringComparison.OrdinalIgnoreCase);
                                            bool isUnselected = remarks.Trim().Equals("Unselected by Employee", StringComparison.OrdinalIgnoreCase)
                                                               || remarks.Trim().Equals("Unselected by IT", StringComparison.OrdinalIgnoreCase);

                                            SetOrUpdateKey(row, "isConfirmed", isConfirmed);

                                            // If called for Employee Dashboard, only include items assigned to this employee (and not unselected)
                                            if (data.Flag == "Dashboard")
                                            {
                                                if (isUnselected || rec.HRME_Id == null || rec.HRME_Id != data.HRME_Id)
                                                {
                                                    continue;
                                                }

                                                // If assigned to employee and not yet confirmed, pending confirmation is needed
                                                if (!isConfirmed && rec.HRME_Id == data.HRME_Id)
                                                {
                                                    hasPendingConfirmation = true;
                                                }

                                                filteredEmployeeDetails.Add(row);
                                            }
                                            else
                                            {
                                                // For Admin / Location dashboard, include all items
                                                if (!isConfirmed && !isUnselected && rec.HRME_Id == data.HRME_Id)
                                                {
                                                    hasPendingConfirmation = true;
                                                }
                                                filteredEmployeeDetails.Add(row);
                                            }
                                        }
                                        else
                                        {
                                            if (data.Flag != "Dashboard")
                                            {
                                                filteredEmployeeDetails.Add(row);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        data.hasPendingConfirmation = hasPendingConfirmation;
                        data.employeedetails = (data.Flag == "Dashboard") ? filteredEmployeeDetails.ToArray() : retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;

        }

        private long? GetInvatcoId(IDictionary<string, object> row)
        {
            if (row == null) return null;
            foreach (var key in row.Keys)
            {
                if (string.Equals(key, "INVATCO_Id", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(key, "AssetTagCheckOutId", StringComparison.OrdinalIgnoreCase))
                {
                    var val = row[key];
                    if (val != null && val != DBNull.Value && long.TryParse(val.ToString(), out long parsedId))
                    {
                        return parsedId;
                    }
                }
            }
            return null;
        }

        private void SetOrUpdateKey(IDictionary<string, object> row, string keyName, object value)
        {
            if (row == null) return;
            string matchedKey = null;
            foreach (var k in row.Keys)
            {
                if (string.Equals(k, keyName, StringComparison.OrdinalIgnoreCase))
                {
                    matchedKey = k;
                    break;
                }
            }
            if (matchedKey != null)
            {
                row[matchedKey] = value;
            }
            else
            {
                row.Add(keyName, value);
            }
        }

        public INV_LocationDashboardDTO OnChangeLocation(INV_LocationDashboardDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Dashboard_Assets";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = "4" });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@INVMLO_Id", SqlDbType.VarChar) { Value = 0 });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.locationdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;

        }

        public INV_LocationDashboardDTO SaveDetails(INV_LocationDashboardDTO data)
        {
            try
            {
                // Unassign unselected assets
                if (data.unselectedlocationlistarrays != null && data.unselectedlocationlistarrays.Length > 0)
                {
                    foreach (var d in data.unselectedlocationlistarrays)
                    {
                        if (d != null && d.INVATCO_Id.HasValue && d.INVATCO_Id.Value > 0)
                        {
                            var updates = _IssueContext.INV_AssetTag_CheckOutDMO.Where(R => R.INVATCO_Id == d.INVATCO_Id.Value).FirstOrDefault();
                            if (updates != null)
                            {
                                updates.HRME_Id = null;
                                updates.INVATCO_ReceivedBy = null;
                                updates.INVATCO_CheckOutRemarks = "Unselected by IT";
                                updates.UpdatedDate = DateTime.Now;
                                updates.INVATCO_UpdatedBy = data.User_Id;
                                _IssueContext.Update(updates);
                            }
                        }
                    }
                }

                // Assign selected assets
                if (data.locationlistarrays != null && data.locationlistarrays.Length > 0)
                {
                    long defaultHrmeId = data.HRME_Id;
                    var defaultEmp = defaultHrmeId > 0 ? _IssueContext.MasterEmployee.Where(a => a.HRME_Id == defaultHrmeId).FirstOrDefault() : null;

                    foreach (var d in data.locationlistarrays)
                    {
                        if (d != null && d.INVATCO_Id.HasValue && d.INVATCO_Id.Value > 0)
                        {
                            long hrmeId = (d.HRME_Id.HasValue && d.HRME_Id.Value > 0) ? d.HRME_Id.Value : defaultHrmeId;
                            var emp = (defaultEmp != null && defaultEmp.HRME_Id == hrmeId)
                                ? defaultEmp
                                : _IssueContext.MasterEmployee.Where(a => a.HRME_Id == hrmeId).FirstOrDefault();

                            var updates = _IssueContext.INV_AssetTag_CheckOutDMO.Where(R => R.INVATCO_Id == d.INVATCO_Id.Value).FirstOrDefault();

                            if (updates != null && emp != null)
                            {
                                updates.MI_Id = emp.MI_Id;
                                updates.HRME_Id = hrmeId;
                                updates.INVATCO_ReceivedBy = (emp.HRME_EmployeeFirstName + " " + (emp.HRME_EmployeeMiddleName ?? "") + " " + (emp.HRME_EmployeeLastName ?? "")).Trim();
                                updates.INVATCO_CheckOutRemarks = "Transfer";
                                updates.UpdatedDate = DateTime.Now;
                                updates.INVATCO_UpdatedBy = data.User_Id;
                                _IssueContext.Update(updates);
                            }
                        }
                    }
                }

                int i = _IssueContext.SaveChanges();
                data.Flag = "save";
                data.returnval = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.Flag = "admin";
                data.returnval = false;
            }
            return data;
        }

        //suplier
        public INV_LocationDashboardDTO OnChange_Suplier(INV_LocationDashboardDTO data)
        {
            try
            {

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_VendorDashboard";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Suplier_Details" });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Voucher_Details = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_VendorDashboard";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "GRN_Details" });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Supplier_Details = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //locationdetails
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_VendorDashboard";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Suplier_files" });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id", SqlDbType.VarChar) { Value = data.HRME_Id });
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.locationdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //employeelist
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.Flag = "admin";
            }
            return data;
        }
        public INV_LocationDashboardDTO Details_supplier(INV_LocationDashboardDTO data)
        {
            try
            {


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.Flag = "admin";
            }
            return data;
        }
        public INV_LocationDashboardDTO Suplier_Details(INV_LocationDashboardDTO data)
        {
            try
            {


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.Flag = "admin";
            }
            return data;
        }

        //removelocationemployee
        public INV_LocationDashboardDTO removelocationemployee(INV_LocationDashboardDTO data)
        {
            try
            {
                var location = "";
                if (data.locationlistarrays != null && data.locationlistarrays.Length > 0)
                {
                    foreach (var x in data.locationlistarrays)
                    {
                        if (x != null && x.INVMLO_Id.HasValue && x.INVMLO_Id.Value > 0)
                        {
                            location += x.INVMLO_Id.Value + ",";
                        }
                    }
                }

                var outputval = _Context.Database.ExecuteSqlCommand("Employee_Asset_map_removing  @p0,@p1,@p2", location, data.HRME_Id, data.Flag);

                if (data.Flag == "REMOVE" && data.HRME_Id > 0)
                {
                    var coList = _IssueContext.INV_AssetTag_CheckOutDMO.Where(r => r.HRME_Id == data.HRME_Id).ToList();
                    foreach (var item in coList)
                    {
                        item.HRME_Id = null;
                        item.INVATCO_ReceivedBy = null;
                        item.INVATCO_CheckOutRemarks = "Unassigned by IT";
                        item.UpdatedDate = DateTime.Now;
                        _IssueContext.Update(item);
                    }
                    _IssueContext.SaveChanges();
                }

                if (outputval > 0)
                {
                    data.message = "Success";
                }
                else
                {
                    data.message = "Failed";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // by sushma
        public INV_LocationDashboardDTO getemployeedetailCommon(INV_LocationDashboardDTO data)
        {
            long commonlocationid = 0;
            var retObject = new List<dynamic>();

            try
            {
                // 1. Try EmployeeCommon stored procedure
                try
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmployeeCommon";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });

                        var outputParam = new SqlParameter("@INVMLOCATIONID", SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }

                        commonlocationid = outputParam.Value != DBNull.Value && outputParam.Value != null
                            ? Convert.ToInt64(outputParam.Value)
                            : 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EmployeeCommon SP error: " + ex.Message);
                }

                if (retObject.Count > 0)
                {
                    data.employeedetailsCommon = retObject.ToArray();
                }
                //else
                //{
                //    // 2. If EmployeeCommon returned no rows, fallback to INV_Dashboard_Assets_Common
                //    if (commonlocationid > 0)
                //    {
                //        data.INVMLO_Id = commonlocationid;
                //    }

                //    try
                //    {
                //        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                //        {
                //            cmd.CommandText = "INV_Dashboard_Assets_Common";
                //            cmd.CommandType = CommandType.StoredProcedure;
                //            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });
                //            cmd.Parameters.Add(new SqlParameter("@INVMLO_Id", SqlDbType.BigInt) { Value = data.INVMLO_Id });
                //            cmd.Parameters.Add(new SqlParameter("@hrme_id", SqlDbType.BigInt) { Value = data.HRME_Id });

                //            if (cmd.Connection.State != ConnectionState.Open)
                //                cmd.Connection.Open();

                //            var retObjectCommon = new List<dynamic>();
                //            using (var dataReader = cmd.ExecuteReader())
                //            {
                //                while (dataReader.Read())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                //                    }
                //                    retObjectCommon.Add((ExpandoObject)dataRow);
                //                }
                //            }
                //            if (retObjectCommon.Count > 0)
                //            {
                //                data.employeedetailsCommon = retObjectCommon.ToArray();
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine("INV_Dashboard_Assets_Common SP error: " + ex.Message);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("getemployeedetailCommon error: " + ex.Message);
            }
            return data;
        }

        public INV_LocationDashboardDTO SaveEmployeeAssets(INV_LocationDashboardDTO data)
        {
            try
            {
                // Process unselected assets (unassign from employee)
                if (data.unselectedlocationlistarrays != null && data.unselectedlocationlistarrays.Length > 0)
                {
                    foreach (var d in data.unselectedlocationlistarrays)
                    {
                        if (d != null && d.INVATCO_Id.HasValue && d.INVATCO_Id.Value > 0)
                        {
                            var updates = _IssueContext.INV_AssetTag_CheckOutDMO.Where(R => R.INVATCO_Id == d.INVATCO_Id.Value).FirstOrDefault();
                            if (updates != null)
                            {
                                updates.HRME_Id = null;
                                updates.INVATCO_ReceivedBy = null;
                                updates.INVATCO_CheckOutRemarks = "Unselected by Employee";
                                updates.UpdatedDate = DateTime.Now;
                                updates.INVATCO_UpdatedBy = data.User_Id;
                                _IssueContext.Update(updates);
                            }
                        }
                    }
                }

                // Process selected assets (confirm employee assignment)
                if (data.locationlistarrays != null && data.locationlistarrays.Length > 0)
                {
                    var emp = _IssueContext.MasterEmployee.Where(a => a.HRME_Id == data.HRME_Id).FirstOrDefault();
                    string empName = "";
                    if (emp != null)
                    {
                        empName = (emp.HRME_EmployeeFirstName + " " + (emp.HRME_EmployeeMiddleName ?? "") + " " + (emp.HRME_EmployeeLastName ?? "")).Trim();
                    }

                    foreach (var d in data.locationlistarrays)
                    {
                        if (d != null && d.INVATCO_Id.HasValue && d.INVATCO_Id.Value > 0)
                        {
                            var updates = _IssueContext.INV_AssetTag_CheckOutDMO.Where(R => R.INVATCO_Id == d.INVATCO_Id.Value).FirstOrDefault();
                            if (updates != null)
                            {
                                long targetHrmeId = (d.HRME_Id.HasValue && d.HRME_Id.Value > 0) ? d.HRME_Id.Value : data.HRME_Id;
                                if (targetHrmeId > 0)
                                {
                                    updates.HRME_Id = targetHrmeId;
                                    if (emp == null || emp.HRME_Id != targetHrmeId)
                                    {
                                        var itemEmp = _IssueContext.MasterEmployee.Where(a => a.HRME_Id == targetHrmeId).FirstOrDefault();
                                        if (itemEmp != null)
                                        {
                                            updates.INVATCO_ReceivedBy = (itemEmp.HRME_EmployeeFirstName + " " + (itemEmp.HRME_EmployeeMiddleName ?? "") + " " + (itemEmp.HRME_EmployeeLastName ?? "")).Trim();
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(empName))
                                    {
                                        updates.INVATCO_ReceivedBy = empName;
                                    }
                                }
                                updates.INVATCO_CheckOutRemarks = "Confirmed by Employee";
                                updates.UpdatedDate = DateTime.Now;
                                updates.INVATCO_UpdatedBy = data.User_Id;
                                _IssueContext.Update(updates);
                            }
                        }
                    }
                }

                int i = _IssueContext.SaveChanges();
                data.returnval = true;
                data.Flag = "save";
                data.message = "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
                data.Flag = "admin";
                data.message = "Error: " + ex.Message;
            }
            return data;
        }

        //Sanjai
        public INV_LocationDashboardDTO GetPendingAcknowledgementReport(INV_LocationDashboardDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT 
                            co.INVATCO_Id,
                            co.MI_Id,
                            ISNULL(mi.MI_Name, '') AS MI_Name,
                            co.INVAAT_Id,
                            ISNULL(aat.INVAAT_AssetId, '') AS INVAAT_AssetId,
                            ISNULL(aat.INVAAT_SerialNo, '') AS INVAAT_SerialNo,
                            ISNULL(aat.INVAAT_FilePath, '') AS INVAAT_FilePath,
                            ISNULL(aat.INVAAT_ModelNo, '') AS INVAAT_ModelNo,
                            ISNULL(aat.INVAAT_ManufacturerName, '') AS INVAAT_ManufacturerName,
                            ISNULL(aat.INVAAT_AssetDescription, '') AS INVAAT_AssetDescription,
                            co.INVMI_Id,
                            ISNULL(item.INVMI_ItemName, '') AS INVMI_ItemName,
                            co.INVMLO_Id,
                            ISNULL(mlo.INVMLO_LocationRoomName, '') AS INVMLO_LocationRoomName,
                            ISNULL(mlo.INVMLO_LocationRemarks, '') AS INVMLO_LocationRemarks,
                            ISNULL(co.INVATCO_CheckOutQty, 1) AS INVATCO_CheckOutQty,
                            ISNULL(co.INVATCO_CheckOutRemarks, 'Pending Acknowledgement') AS INVATCO_CheckOutRemarks,
                            ISNULL(co.INVATCO_CheckoutDate, co.CreatedDate) AS AssignedDate,
                            co.UpdatedDate AS UnselectedDate,
                            DATEDIFF(day, ISNULL(co.INVATCO_CheckoutDate, co.CreatedDate), GETDATE()) AS PendingDays,
                            CASE 
                                WHEN co.INVATCO_CheckOutRemarks = 'Unselected by Employee' THEN 'Unselected by Employee'
                                ELSE 'Pending Acknowledgement'
                            END AS AcknowledgementStatus,
                            co.INVATCO_UpdatedBy,
                            emp.HRME_Id,
                            COALESCE(
                                NULLIF(LTRIM(RTRIM(CONCAT(emp.HRME_EmployeeFirstName, ' ', ISNULL(emp.HRME_EmployeeMiddleName, ''), ' ', ISNULL(emp.HRME_EmployeeLastName, '')))), ''),
                                NULLIF(LTRIM(RTRIM(CONCAT(emp_upd.HRME_EmployeeFirstName, ' ', ISNULL(emp_upd.HRME_EmployeeMiddleName, ''), ' ', ISNULL(emp_upd.HRME_EmployeeLastName, '')))), ''),
                                co.INVATCO_ReceivedBy,
                                'Unassigned / Open'
                            ) AS EmployeeName,
                            ISNULL(emp.HRME_EmployeeCode, emp_upd.HRME_EmployeeCode) AS HRME_EmployeeCode,
                            ISNULL(dept.HRMD_DepartmentName, '') AS HRMD_DepartmentName,
                            ISNULL(desig.HRMDES_DesignationName, '') AS HRMDES_DesignationName,
                            ISNULL(tgrn.INVTGRN_PurchaseRate, 0) AS INVTGRN_PurchaseRate
                        FROM [INV].[INV_AssetTag_CheckOut] co
                        LEFT JOIN [INV].[INV_Asset_AssetTag] aat ON co.INVAAT_Id = aat.INVAAT_Id
                        LEFT JOIN [INV].[INV_Master_Item] item ON co.INVMI_Id = item.INVMI_Id
                        LEFT JOIN [INV].[INV_Master_Location] mlo ON co.INVMLO_Id = mlo.INVMLO_Id
                        LEFT JOIN [dbo].[Master_Institution] mi ON co.MI_Id = mi.MI_Id
                        LEFT JOIN [dbo].[HR_Master_Employee] emp ON co.HRME_Id IS NOT NULL AND co.HRME_Id = emp.HRME_Id
                        LEFT JOIN [dbo].[IVRM_User_Login_Faculty] ulf ON co.INVATCO_UpdatedBy IS NOT NULL AND co.INVATCO_UpdatedBy = ulf.IVRMUL_Id
                        LEFT JOIN [dbo].[HR_Master_Employee] emp_upd ON ulf.Emp_Code IS NOT NULL AND ulf.Emp_Code = emp_upd.HRME_Id
                        LEFT JOIN [dbo].[HR_Master_Department] dept ON ISNULL(emp.HRMD_Id, emp_upd.HRMD_Id) = dept.HRMD_Id
                        LEFT JOIN [dbo].[HR_Master_Designation] desig ON ISNULL(emp.HRMDES_Id, emp_upd.HRMDES_Id) = desig.HRMDES_Id
                        OUTER APPLY (
                            SELECT TOP 1 INVTGRN_PurchaseRate 
                            FROM [INV].[INV_T_GRN] 
                            WHERE INVMI_Id = co.INVMI_Id 
                            ORDER BY INVTGRN_Id DESC
                        ) tgrn
                        WHERE (co.INVATCO_CheckOutRemarks IS NULL 
                               OR LTRIM(RTRIM(co.INVATCO_CheckOutRemarks)) = ''
                               OR LTRIM(RTRIM(co.INVATCO_CheckOutRemarks)) <> 'Confirmed by Employee')
                          AND (@MI_Id = 0 OR co.MI_Id = @MI_Id OR co.MI_Id IS NULL OR co.MI_Id = 0)
                        ORDER BY co.UpdatedDate DESC, co.CreatedDate DESC";

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
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

                    data.pendingAssetList = retObject.ToArray();

                    // Load active location rooms for dropdown using ExpandoObject (avoids cross-assembly anonymous type serialization error)
                    using (var locCmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        locCmd.CommandText = @"
                            SELECT 
                                INVMLO_Id, 
                                INVMLO_LocationRoomName 
                            FROM [INV].[INV_Master_Location] 
                            WHERE (INVMLO_ActiveFlg = 1 OR INVMLO_ActiveFlg IS NULL) 
                              AND (@MI_Id = 0 OR MI_Id = @MI_Id OR MI_Id IS NULL OR MI_Id = 0)
                            ORDER BY INVMLO_LocationRoomName";
                        locCmd.CommandType = CommandType.Text;
                        locCmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_ID });

                        if (locCmd.Connection.State != ConnectionState.Open)
                            locCmd.Connection.Open();

                        var locObject = new List<dynamic>();
                        using (var locReader = locCmd.ExecuteReader())
                        {
                            while (locReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < locReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        locReader.GetName(iFiled),
                                        locReader.IsDBNull(iFiled) ? null : locReader[iFiled]
                                    );
                                }
                                locObject.Add((ExpandoObject)dataRow);
                            }
                        }

                        data.locationRoomList = locObject.ToArray();
                        data.locationdetails = locObject.ToArray();
                    }

                    data.returnval = true;
                    data.message = "Success";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
                data.message = ex.Message;
            }
            return data;
        }

        public INV_LocationDashboardDTO SavePendingAssetLocationTransfer(INV_LocationDashboardDTO data)
        {
            try
            {
                if (data.To_INVMLO_Id.HasValue && data.To_INVMLO_Id.Value > 0)
                {
                    var itemsToUpdate = data.selectedPendingAssets ?? data.locationlistarrays;
                    if (itemsToUpdate != null && itemsToUpdate.Length > 0)
                    {
                        foreach (var item in itemsToUpdate)
                        {
                            if (item != null && item.INVATCO_Id.HasValue && item.INVATCO_Id.Value > 0)
                            {
                                var coRecord = _IssueContext.INV_AssetTag_CheckOutDMO.FirstOrDefault(r => r.INVATCO_Id == item.INVATCO_Id.Value);
                                if (coRecord != null)
                                {
                                    coRecord.INVMLO_Id = data.To_INVMLO_Id.Value;
                                    coRecord.UpdatedDate = DateTime.Now;
                                    coRecord.INVATCO_UpdatedBy = data.User_Id;
                                    _IssueContext.Update(coRecord);
                                }
                            }
                        }
                        _IssueContext.SaveChanges();
                        data.returnval = true;
                        data.message = "Location Transferred Successfully";
                        data.Flag = "save";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "No assets selected for transfer.";
                    }
                }
                else
                {
                    data.returnval = false;
                    data.message = "Please select a destination Location/Room.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
                data.message = "Error: " + ex.Message;
            }
            return data;
        }

    }
}

