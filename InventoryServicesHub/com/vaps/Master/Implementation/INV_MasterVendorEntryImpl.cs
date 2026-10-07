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
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace InventoryServicesHub.com.vaps.Master.Implementation
{
    public class INV_MasterVendorEntryImpl : INV_MasterVendorEntryInterface
    {

        private readonly InventoryContext _INVContext;
        private readonly ILogger<INV_MasterVendorEntryImpl> _logInv;
        public INV_MasterVendorEntryImpl(InventoryContext InvContext, ILogger<INV_MasterVendorEntryImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }


        public async Task<INV_MasterVendorEntryDTO> SaveData(INV_MasterVendorEntryDTO data)
        {
            try
            {
                // Duplicate Check (Based on PAN Number)
                string panNumber = (data.MVE_PAN_Number ?? "").Trim();
                if (!string.IsNullOrEmpty(panNumber))
                {
                    var duplicate = await _INVContext.INV_MasterVendorEntryDMO
                        .Where(a => a.MVE_PAN_Number.ToLower().Trim() == panNumber.ToLower()
                             && a.MVE_Id != data.MVE_Id)
                    .FirstOrDefaultAsync();

                    if (duplicate != null)
                {
                    data.returnval = false;
                    data.saveorupdate = "Vendor PAN Number Already Exists";
                    return data;
                    }
                }

                // UPDATE
                if (data.MVE_Id > 0)
                {
                    var entity = await _INVContext.INV_MasterVendorEntryDMO
                        .Where(a => a.MVE_Id == data.MVE_Id).FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        data.returnval = false;
                        data.saveorupdate = "Record not found";
                        return data;
                    }

                    entity.VCM_Id = data.VCM_Id;
                    entity.MI_Id = data.MI_Id;
                    entity.MVE_VendorName = data.MVE_VendorName;
                    entity.MVE_CompanyName = data.MVE_CompanyName;
                    entity.MVE_PAN_Number = data.MVE_PAN_Number;
                    entity.MVE_MSME_Register = data.MVE_MSME_Register;
                    entity.MVE_MobileNumber = data.MVE_MobileNumber;
                    entity.MVE_Register_OfficeAddress = data.MVE_Register_OfficeAddress;
                    entity.MVE_Account_name = data.MVE_Account_name;
                    entity.MVE_Account_Number = data.MVE_Account_Number;
                    entity.MVE_Service_Offered = data.MVE_Service_Offered;
                    entity.MVE_GST_Number = data.MVE_GST_Number;
                    entity.MVE_CIN_Number = data.MVE_CIN_Number;
                    entity.MVE_Contact_Person = data.MVE_Contact_Person;
                    entity.MVE_Email_Address = data.MVE_Email_Address;
                    entity.MVE_Bank_Name = data.MVE_Bank_Name;
                    entity.MVE_Branch_Name = data.MVE_Branch_Name;
                    entity.MVE_IFSC_Code = data.MVE_IFSC_Code;
                    entity.MVE_Payment_Terms = data.MVE_Payment_Terms;
                    entity.MVE_Agreement_Startdate = data.MVE_Agreement_Startdate;
                    entity.MVE_Agreement_Enddate = data.MVE_Agreement_Enddate;

                    entity.MVE_GST_Certificate_filepath = data.MVE_GST_Certificate_filepath;
                    entity.MVE_PAN_Upload_filepath = data.MVE_PAN_Upload_filepath;
                    entity.MVE_Bank_Details_filepath = data.MVE_Bank_Details_filepath;
                    entity.MVE_MOU_filepath = data.MVE_MOU_filepath;
                    entity.MVE_Company_Register_Certificate_filepath = data.MVE_Company_Register_Certificate_filepath;
                    entity.MVE_MSME_Certificate_filepath = data.MVE_MSME_Certificate_filepath;
                    entity.MVE_ISO_Certificate_filepath = data.MVE_ISO_Certificate_filepath;
                    entity.MVE_Workorder_filepath = data.MVE_Workorder_filepath;
                    entity.ApprovalStatus = data.ApprovalStatus;

                    entity.MVE_Updatedby = data.UserId;
                    entity.MVE_Updateddate = DateTime.Now;

                    var result = await _INVContext.SaveChangesAsync();

                    data.returnval = result > 0;
                    data.saveorupdate = result > 0 ? "Updated Successfully" : "Update Failed";
                }
                // Insert
                else
                {
                    var entity = new INV_MasterVendorEntryDMO
                    {
                        VCM_Id = data.VCM_Id,
                        MI_Id = data.MI_Id,
                        MVE_VendorName = data.MVE_VendorName,
                        MVE_CompanyName = data.MVE_CompanyName,
                        MVE_PAN_Number = data.MVE_PAN_Number,
                        MVE_MSME_Register = data.MVE_MSME_Register,
                        MVE_MobileNumber = data.MVE_MobileNumber,
                        MVE_Register_OfficeAddress = data.MVE_Register_OfficeAddress,
                        MVE_Account_name = data.MVE_Account_name,
                        MVE_Account_Number = data.MVE_Account_Number,
                        MVE_Service_Offered = data.MVE_Service_Offered,
                        MVE_GST_Number = data.MVE_GST_Number,
                        MVE_CIN_Number = data.MVE_CIN_Number,
                        MVE_Contact_Person = data.MVE_Contact_Person,
                        MVE_Email_Address = data.MVE_Email_Address,
                        MVE_Bank_Name = data.MVE_Bank_Name,
                        MVE_Branch_Name = data.MVE_Branch_Name,
                        MVE_IFSC_Code = data.MVE_IFSC_Code,
                        MVE_Payment_Terms = data.MVE_Payment_Terms,
                        MVE_Agreement_Startdate = data.MVE_Agreement_Startdate,
                        MVE_Agreement_Enddate = data.MVE_Agreement_Enddate,

                        MVE_GST_Certificate_filepath = data.MVE_GST_Certificate_filepath,
                        MVE_PAN_Upload_filepath = data.MVE_PAN_Upload_filepath,
                        MVE_Bank_Details_filepath = data.MVE_Bank_Details_filepath,
                        MVE_MOU_filepath = data.MVE_MOU_filepath,
                        MVE_Company_Register_Certificate_filepath = data.MVE_Company_Register_Certificate_filepath,
                        MVE_MSME_Certificate_filepath = data.MVE_MSME_Certificate_filepath,
                        MVE_ISO_Certificate_filepath = data.MVE_ISO_Certificate_filepath,
                        MVE_Workorder_filepath = data.MVE_Workorder_filepath,
                        ApprovalStatus = "pending",

                        MVE_Activeflag = true,
                        MVE_Createdby = data.UserId,
                        MVE_Updatedby = data.UserId,
                        MVE_Createddate = DateTime.Now,
                        MVE_Updateddate = DateTime.Now
                    };

                    _INVContext.INV_MasterVendorEntryDMO.Add(entity);

                    var result = await _INVContext.SaveChangesAsync();

                    data.returnval = result > 0;
                    data.saveorupdate = result > 0 ? "Saved Successfully" : "Save Failed";
                }
            }
            catch (Exception ex)
            {
                _logInv.LogError("Error in SaveData: " + ex.Message + " | InnerException: " + ex.InnerException?.Message);
                data.returnval = false;
                data.saveorupdate = ex.InnerException?.Message ?? ex.Message;
            }

            return data;
        }


        public async Task<INV_MasterVendorEntryDTO> Toggle_button(INV_MasterVendorEntryDTO data)
        {
            try
            {
                var ToggleButton = await _INVContext.INV_MasterVendorEntryDMO
                    .Where(a => a.MVE_Id == data.MVE_Id && a.MI_Id == data.MI_Id)
                    .FirstOrDefaultAsync();

                if (ToggleButton == null)
                {
                    data.returnval = false;
                    data.saveorupdate = "No record found";
                    return data;
                }

                if (ToggleButton.MVE_Activeflag == true)
                {
                    ToggleButton.MVE_Activeflag = false;
                    data.saveorupdate = "Successfully Deactivated";
                }
                else
                {
                    ToggleButton.MVE_Activeflag = true;
                    data.saveorupdate = "Successfully Activated";
                }

                ToggleButton.MVE_Createddate = DateTime.Now;
                ToggleButton.MVE_Updatedby = data.UserId;
                _INVContext.Update(ToggleButton);
                var existing = await _INVContext.SaveChangesAsync();

                if (existing > 0)
                {
                    data.returnval = true;
                    data.MVE_Activeflag = ToggleButton.MVE_Activeflag;
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


        public async Task<INV_MasterVendorEntryDTO> GridData(INV_MasterVendorEntryDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MasterVendorEntry_GridData";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;

                            for (var iField = 0; iField < dataReader.FieldCount; iField++)
                            {
                                if (!dataRow.ContainsKey(dataReader.GetName(iField)))
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iField),
                                        dataReader.IsDBNull(iField) ? null : dataReader[iField]
                                    );
                                }
                            }
                            retObject.Add(dataRow);
                        }
                    }
                    data.VendorEntryArray = retObject.ToArray();
                }

                var categoryList = await _INVContext.INV_VendorCategoryMasterDMO
                                    .Where(a => a.MI_Id == data.MI_Id && a.VCM_Activeflag == true)
                                    .OrderBy(a => a.VCM_Vendorname)
                                    .ToArrayAsync();

                data.VendorCategoryList = categoryList;

                if (data.VendorEntryArray != null && data.VendorEntryArray.Length > 0)
                {
                    data.returnval = true;
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
                _logInv.LogError("Error in GridData: " + ee.Message);
                data.returnval = false;
                data.saveorupdate = ee.Message;
            }
            return data;
        }

        public async Task<INV_MasterVendorEntryDTO> CategoryDropDown(INV_MasterVendorEntryDTO data)
        {
            try
            {
                var categoryList = await _INVContext.INV_VendorCategoryMasterDMO
                                    .Where(a => a.MI_Id == data.MI_Id && a.VCM_Activeflag == true)
                                    .OrderBy(a => a.VCM_Vendorname)
                                    .ToArrayAsync();

                if (categoryList != null && categoryList.Length > 0)
                {
                    data.VendorCategoryList = categoryList;
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                _logInv.LogError("Error in CategoryDropDown: " + ee.Message);
                data.returnval = false;
            }
            return data;
        }

    }
}
