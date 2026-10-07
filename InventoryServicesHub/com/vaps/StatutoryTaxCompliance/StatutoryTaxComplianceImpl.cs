using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Inventory.StatutoryTaxCompliance;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Inventory;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.StatutoryTaxCompliance
{
    public class StatutoryTaxComplianceImpl : Interface.StatutoryTaxComplianceInterface
    {
        public DomainModelMsSqlServerContext _db;
        public InventoryContext _context;
        public StatutoryTaxComplianceImpl(InventoryContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;

        }
        public StatutoryTaxComplianceDTO getdetails(StatutoryTaxComplianceDTO data)
        {
            data.AllCompany = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
            data.Year = _context.IVRM_Master_FinancialYearDMO.Distinct().ToArray();

            return data;
        }
        public StatutoryTaxComplianceDTO SaveTabOne(StatutoryTaxComplianceDTO data)
        {
            try
            {
                long VMSSC_Id = 0;
                VMS_Statutory_ComplianceDMO VMS_Statutory_ComplianceDMO = new VMS_Statutory_ComplianceDMO();
                VMS_Statutory_consultantDMO VMS_Statutory_consultantDMO = new VMS_Statutory_consultantDMO();

                if (data.VMSSC_Id > 0)
                {
                    VMSSC_Id = data.VMSSC_Id;
                    var obj = _context.VMS_Statutory_ComplianceDMO.Single(a => a.VMSSC_Id == data.VMSSC_Id);

                    obj.MI_Id = data.MI_Id;
                    obj.VMSSC_Status = data.VMSSC_Status;
                    obj.IMFY_Id = data.IMFY_Id;
                    obj.VMSSC_Flag = data.VMSSC_Flag;
                    obj.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    obj.VMSSC_Remarks = data.VMSSC_Remarks;
                    obj.VMSSC_FromDate = DateTime.Now;
                    obj.VMSSC_ToDate = DateTime.Now;
                    obj.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    obj.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    obj.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    obj.VMSSC_CreatedDate = DateTime.Now;
                    obj.VMSSC_UpdatedDate = DateTime.Now;
                    obj.VMSSC_CreatedBy = data.UserId;
                    obj.VMSSC_UpdatedBy = data.UserId;
                    _context.Update(obj);

                    var obj1 = _context.VMS_Statutory_consultantDMO.Single(a => a.VMSSC_Id == data.VMSSC_Id);
                    obj1.VMSSC_Id = data.VMSSC_Id;
                    obj1.VMSSCT_Flag = data.VMSSCT_Flag;
                    obj1.VMSSCT_consultantName = data.VMSSCT_consultantName;
                    obj1.VMSSCT_Address = data.VMSSCT_Address;
                    obj1.VMSSCT_Remarks = data.VMSSCT_Remarks;
                    obj1.VMSSCT_ActiveFlag = true;
                    obj1.VMSSCT_CreatedDate = DateTime.Now;
                    obj1.VMSSCT_UpdatedDate = DateTime.Now;
                    obj1.VMSSCT_CreatedBy = data.UserId;
                    obj1.VMSSCT_UpdatedBy = data.UserId;
                    _context.Update(obj1);
                }
                else
                {

                    VMS_Statutory_ComplianceDMO.MI_Id = data.MI_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Status = data.VMSSC_Status;
                    VMS_Statutory_ComplianceDMO.IMFY_Id = data.IMFY_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Flag = data.VMSSC_Flag;
                    VMS_Statutory_ComplianceDMO.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    VMS_Statutory_ComplianceDMO.VMSSC_Remarks = data.VMSSC_Remarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_FromDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_ToDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedBy = data.UserId;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedBy = data.UserId;
                    _context.Add(VMS_Statutory_ComplianceDMO);
                    VMSSC_Id = VMS_Statutory_ComplianceDMO.VMSSC_Id;


                    VMS_Statutory_consultantDMO.VMSSC_Id = VMSSC_Id;
                    VMS_Statutory_consultantDMO.VMSSCT_Flag = data.VMSSCT_Flag;
                    VMS_Statutory_consultantDMO.VMSSCT_consultantName = data.VMSSCT_consultantName;
                    VMS_Statutory_consultantDMO.VMSSCT_Address = data.VMSSCT_Address;
                    VMS_Statutory_consultantDMO.VMSSCT_Remarks = data.VMSSCT_Remarks;
                    VMS_Statutory_consultantDMO.VMSSCT_ActiveFlag = true;
                    VMS_Statutory_consultantDMO.VMSSCT_CreatedDate = DateTime.Now;
                    VMS_Statutory_consultantDMO.VMSSCT_UpdatedDate = DateTime.Now;
                    VMS_Statutory_consultantDMO.VMSSCT_CreatedBy = data.UserId;
                    VMS_Statutory_consultantDMO.VMSSCT_UpdatedBy = data.UserId;
                    _context.Add(VMS_Statutory_consultantDMO);


                }
                if (data.VMSSC_Id > 0)
                {
                    Array GetFiles = _context.VMS_Statutory_Compliance_DocumentsDMO.Where(a => a.VMSSC_Id == data.VMSSC_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.FILESLISTdata.Length > 0 && data.FILESLISTdata != null)
                {
                    foreach (var STC in data.FILESLISTdata)
                    {
                        VMS_Statutory_Compliance_DocumentsDMO obj11 = new VMS_Statutory_Compliance_DocumentsDMO();
                        obj11.VMSSC_Id = VMSSC_Id;
                        obj11.VMSSCD_Filename = STC.VMSSCD_Filename;
                        obj11.VMSSCD_Filepath = STC.VMSSCD_Filepath;
                        obj11.VMSSCD_ActiveFlag = true;
                        obj11.VMSSCD_CreatedDate = DateTime.Now;
                        obj11.VMSSCD_UpdatedDate = DateTime.Now;
                        obj11.VMSSCD_CreatedBy = data.UserId;
                        obj11.VMSSCD_UpdatedBy = data.UserId;
                        _context.Add(obj11);

                    }
                }
                var returnval1 = _context.SaveChanges();
                if (returnval1 > 0)
                {
                    data.message = "saved";
                }
                else
                {
                    data.message = "notsaved";
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public StatutoryTaxComplianceDTO GetReference(StatutoryTaxComplianceDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_ReferenceNo_Generation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.IMFY_Id
                    });
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
                            }
                        }
                        data.ReferenceNo = retObject.ToArray();

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
        public StatutoryTaxComplianceDTO Getincome(StatutoryTaxComplianceDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Statutory_Tax_Complaince_Data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });

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
                            }
                        }
                        data.Flagdata = retObject.ToArray();

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
        public StatutoryTaxComplianceDTO viewData(StatutoryTaxComplianceDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Statutory_Tax_Document";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Id
                    });

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
                            }
                        }
                        data.viewData = retObject.ToArray();

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
        public StatutoryTaxComplianceDTO SaveTabtwo(StatutoryTaxComplianceDTO data)
        {
            try
            {
                long VMSSC_Id = 0;
                VMS_Statutory_ComplianceDMO VMS_Statutory_ComplianceDMO = new VMS_Statutory_ComplianceDMO();
                if (data.VMSSC_Id > 0)
                {
                    VMSSC_Id = data.VMSSC_Id;
                    var obj = _context.VMS_Statutory_ComplianceDMO.Single(a => a.VMSSC_Id == data.VMSSC_Id);

                    obj.MI_Id = data.MI_Id;
                    obj.VMSSC_Status = data.VMSSC_Status;

                    obj.IMFY_Id = data.IMFY_Id;
                    obj.VMSSC_Flag = data.VMSSC_Flag;
                    obj.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    obj.VMSSC_Remarks = data.VMSSC_Remarks;
                    obj.VMSSC_FromDate = DateTime.Now;
                    obj.VMSSC_ToDate = DateTime.Now;
                    obj.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    obj.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    obj.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    obj.VMSSC_CreatedDate = DateTime.Now;
                    obj.VMSSC_UpdatedDate = DateTime.Now;
                    obj.VMSSC_CreatedBy = data.UserId;
                    obj.VMSSC_UpdatedBy = data.UserId;
                    _context.Update(obj);
                }
                else
                {
                    VMS_Statutory_ComplianceDMO.MI_Id = data.MI_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Status = data.VMSSC_Status;
                    VMS_Statutory_ComplianceDMO.IMFY_Id = data.IMFY_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Flag = data.VMSSC_Flag;
                    VMS_Statutory_ComplianceDMO.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    VMS_Statutory_ComplianceDMO.VMSSC_Remarks = data.VMSSC_Remarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_FromDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_ToDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedBy = data.UserId;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedBy = data.UserId;
                    _context.Add(VMS_Statutory_ComplianceDMO);
                    VMSSC_Id = VMS_Statutory_ComplianceDMO.VMSSC_Id;
                }
                if (data.VMSSC_Id > 0)
                {
                    Array GetFiles = _context.VMS_Statutory_Compliance_DocumentsDMO.Where(a => a.VMSSC_Id == data.VMSSC_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.FILESLISTdata.Length > 0 && data.FILESLISTdata != null)
                {
                    foreach (var STC in data.FILESLISTdata)
                    {
                        VMS_Statutory_Compliance_DocumentsDMO obj11 = new VMS_Statutory_Compliance_DocumentsDMO();
                        obj11.VMSSC_Id = VMSSC_Id;
                        obj11.VMSSCD_Filename = STC.VMSSCD_Filename;
                        obj11.VMSSCD_Filepath = STC.VMSSCD_Filepath;
                        obj11.VMSSCD_ActiveFlag = true;
                        obj11.VMSSCD_CreatedDate = DateTime.Now;
                        obj11.VMSSCD_UpdatedDate = DateTime.Now;
                        obj11.VMSSCD_CreatedBy = data.UserId;
                        obj11.VMSSCD_UpdatedBy = data.UserId;
                        _context.Add(obj11);

                    }
                }
                var returnval1 = _context.SaveChanges();
                if (returnval1 > 0)
                {
                    data.message = "saved";
                }
                else
                {
                    data.message = "notsaved";
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public StatutoryTaxComplianceDTO SaveTabthree(StatutoryTaxComplianceDTO data)
        {
            try
            {
                long VMSSC_Id = 0;
                VMS_Statutory_ComplianceDMO VMS_Statutory_ComplianceDMO = new VMS_Statutory_ComplianceDMO();
                if (data.VMSSC_Id > 0)
                {
                    VMSSC_Id = data.VMSSC_Id;
                    var obj = _context.VMS_Statutory_ComplianceDMO.Single(a => a.VMSSC_Id == data.VMSSC_Id);

                    obj.MI_Id = data.MI_Id;
                    obj.VMSSC_Status = data.VMSSC_Status;
                    obj.IMFY_Id = data.IMFY_Id;
                    obj.VMSSC_Flag = data.VMSSC_Flag;
                    obj.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    obj.VMSSC_Remarks = data.VMSSC_Remarks;
                    obj.VMSSC_FromDate = DateTime.Now;
                    obj.VMSSC_ToDate = DateTime.Now;
                    obj.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    obj.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    obj.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    obj.VMSSC_CreatedDate = DateTime.Now;
                    obj.VMSSC_UpdatedDate = DateTime.Now;
                    obj.VMSSC_CreatedBy = data.UserId;
                    obj.VMSSC_UpdatedBy = data.UserId;
                    _context.Update(obj);
                }
                else
                {
                    VMS_Statutory_ComplianceDMO.MI_Id = data.MI_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Status = data.VMSSC_Status;
                    VMS_Statutory_ComplianceDMO.IMFY_Id = data.IMFY_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Flag = data.VMSSC_Flag;
                    VMS_Statutory_ComplianceDMO.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    VMS_Statutory_ComplianceDMO.VMSSC_Remarks = data.VMSSC_Remarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_FromDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_ToDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedBy = data.UserId;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedBy = data.UserId;
                    _context.Add(VMS_Statutory_ComplianceDMO);
                    VMSSC_Id = VMS_Statutory_ComplianceDMO.VMSSC_Id;
                }
                if (data.VMSSC_Id > 0)
                {
                    Array GetFiles = _context.VMS_Statutory_Compliance_DocumentsDMO.Where(a => a.VMSSC_Id == data.VMSSC_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.FILESLISTdata.Length > 0 && data.FILESLISTdata != null)
                {
                    foreach (var STC in data.FILESLISTdata)
                    {
                        VMS_Statutory_Compliance_DocumentsDMO obj11 = new VMS_Statutory_Compliance_DocumentsDMO();
                        obj11.VMSSC_Id = VMSSC_Id;
                        obj11.VMSSCD_Filename = STC.VMSSCD_Filename;
                        obj11.VMSSCD_Filepath = STC.VMSSCD_Filepath;
                        obj11.VMSSCD_ActiveFlag = true;
                        obj11.VMSSCD_CreatedDate = DateTime.Now;
                        obj11.VMSSCD_UpdatedDate = DateTime.Now;
                        obj11.VMSSCD_CreatedBy = data.UserId;
                        obj11.VMSSCD_UpdatedBy = data.UserId;
                        _context.Add(obj11);

                    }
                }
                var returnval1 = _context.SaveChanges();
                if (returnval1 > 0)
                {
                    data.message = "saved";
                }
                else
                {
                    data.message = "notsaved";
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public StatutoryTaxComplianceDTO SaveTabFour(StatutoryTaxComplianceDTO data)
        {
            try
            {
                long VMSSC_Id = 0;
                VMS_Statutory_ComplianceDMO VMS_Statutory_ComplianceDMO = new VMS_Statutory_ComplianceDMO();
                if (data.VMSSC_Id > 0)
                {
                    VMSSC_Id = data.VMSSC_Id;
                    var obj = _context.VMS_Statutory_ComplianceDMO.Single(a => a.VMSSC_Id == data.VMSSC_Id);

                    obj.MI_Id = data.MI_Id;
                    obj.VMSSC_Status = data.VMSSC_Status;

                    obj.IMFY_Id = data.IMFY_Id;
                    obj.VMSSC_Flag = data.VMSSC_Flag;
                    obj.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    obj.VMSSC_Remarks = data.VMSSC_Remarks;
                    obj.VMSSC_FromDate = data.VMSSC_FromDate;
                    obj.VMSSC_ToDate = data.VMSSC_ToDate;
                    obj.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    obj.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    obj.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    obj.VMSSC_CreatedDate = DateTime.Now;
                    obj.VMSSC_UpdatedDate = DateTime.Now;
                    obj.VMSSC_CreatedBy = data.UserId;
                    obj.VMSSC_UpdatedBy = data.UserId;
                    _context.Update(obj);
                }
                else
                {
                    VMS_Statutory_ComplianceDMO.MI_Id = data.MI_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Status = data.VMSSC_Status;
                    VMS_Statutory_ComplianceDMO.IMFY_Id = data.IMFY_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Flag = data.VMSSC_Flag;
                    VMS_Statutory_ComplianceDMO.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    VMS_Statutory_ComplianceDMO.VMSSC_Remarks = data.VMSSC_Remarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_FromDate = data.VMSSC_FromDate;
                    VMS_Statutory_ComplianceDMO.VMSSC_ToDate = data.VMSSC_ToDate;
                    VMS_Statutory_ComplianceDMO.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedBy = data.UserId;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedBy = data.UserId;
                    _context.Add(VMS_Statutory_ComplianceDMO);
                    VMSSC_Id = VMS_Statutory_ComplianceDMO.VMSSC_Id;
                }
                if (data.VMSSC_Id > 0)
                {
                    Array GetFiles = _context.VMS_Statutory_Compliance_DocumentsDMO.Where(a => a.VMSSC_Id == data.VMSSC_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.FILESLISTdata.Length > 0 && data.FILESLISTdata != null)
                {
                    foreach (var STC in data.FILESLISTdata)
                    {
                        VMS_Statutory_Compliance_DocumentsDMO obj11 = new VMS_Statutory_Compliance_DocumentsDMO();
                        obj11.VMSSC_Id = VMSSC_Id;
                        obj11.VMSSCD_Filename = STC.VMSSCD_Filename;
                        obj11.VMSSCD_Filepath = STC.VMSSCD_Filepath;
                        obj11.VMSSCD_ActiveFlag = true;
                        obj11.VMSSCD_CreatedDate = DateTime.Now;
                        obj11.VMSSCD_UpdatedDate = DateTime.Now;
                        obj11.VMSSCD_CreatedBy = data.UserId;
                        obj11.VMSSCD_UpdatedBy = data.UserId;
                        _context.Add(obj11);

                    }
                }
                var returnval1 = _context.SaveChanges();
                if (returnval1 > 0)
                {
                    data.message = "saved";
                }
                else
                {
                    data.message = "notsaved";
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public StatutoryTaxComplianceDTO SaveTabFive(StatutoryTaxComplianceDTO data)
        {
            try
            {
                long VMSSC_Id = 0;
                VMS_Statutory_ComplianceDMO VMS_Statutory_ComplianceDMO = new VMS_Statutory_ComplianceDMO();
                if (data.VMSSC_Id > 0)
                {
                    VMSSC_Id = data.VMSSC_Id;
                    var obj = _context.VMS_Statutory_ComplianceDMO.Single(a => a.VMSSC_Id == data.VMSSC_Id);

                    obj.MI_Id = data.MI_Id;
                    obj.IMFY_Id = data.IMFY_Id;
                    obj.VMSSC_Status = data.VMSSC_Status;
                    obj.VMSSC_Flag = data.VMSSC_Flag;
                    obj.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    obj.VMSSC_Remarks = data.VMSSC_Remarks;
                    obj.VMSSC_FromDate = data.VMSSC_FromDate;
                    obj.VMSSC_ToDate = data.VMSSC_ToDate;
                    obj.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    obj.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    obj.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    obj.VMSSC_CreatedDate = DateTime.Now;
                    obj.VMSSC_UpdatedDate = DateTime.Now;
                    obj.VMSSC_CreatedBy = data.UserId;
                    obj.VMSSC_UpdatedBy = data.UserId;
                    _context.Update(obj);
                }
                else
                {
                    VMS_Statutory_ComplianceDMO.MI_Id = data.MI_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Status = data.VMSSC_Status;
                    VMS_Statutory_ComplianceDMO.IMFY_Id = data.IMFY_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Flag = data.VMSSC_Flag;
                    VMS_Statutory_ComplianceDMO.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    VMS_Statutory_ComplianceDMO.VMSSC_Remarks = data.VMSSC_Remarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_FromDate = data.VMSSC_FromDate;
                    VMS_Statutory_ComplianceDMO.VMSSC_ToDate = data.VMSSC_ToDate;
                    VMS_Statutory_ComplianceDMO.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedBy = data.UserId;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedBy = data.UserId;
                    _context.Add(VMS_Statutory_ComplianceDMO);
                    VMSSC_Id = VMS_Statutory_ComplianceDMO.VMSSC_Id;
                }
                if (data.VMSSC_Id > 0)
                {
                    Array GetFiles = _context.VMS_Statutory_Compliance_DocumentsDMO.Where(a => a.VMSSC_Id == data.VMSSC_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.FILESLISTdata.Length > 0 && data.FILESLISTdata != null)
                {
                    foreach (var STC in data.FILESLISTdata)
                    {
                        VMS_Statutory_Compliance_DocumentsDMO obj11 = new VMS_Statutory_Compliance_DocumentsDMO();
                        obj11.VMSSC_Id = VMSSC_Id;
                        obj11.VMSSCD_Filename = STC.VMSSCD_Filename;
                        obj11.VMSSCD_Filepath = STC.VMSSCD_Filepath;
                        obj11.VMSSCD_ActiveFlag = true;
                        obj11.VMSSCD_CreatedDate = DateTime.Now;
                        obj11.VMSSCD_UpdatedDate = DateTime.Now;
                        obj11.VMSSCD_CreatedBy = data.UserId;
                        obj11.VMSSCD_UpdatedBy = data.UserId;
                        _context.Add(obj11);

                    }
                }
                var returnval1 = _context.SaveChanges();
                if (returnval1 > 0)
                {
                    data.message = "saved";
                }
                else
                {
                    data.message = "notsaved";
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StatutoryTaxComplianceDTO SaveTabSix(StatutoryTaxComplianceDTO data)
        {
            try
            {
                long VMSSC_Id = 0;
                VMS_Statutory_ComplianceDMO VMS_Statutory_ComplianceDMO = new VMS_Statutory_ComplianceDMO();
                if (data.VMSSC_Id > 0)
                {
                    VMSSC_Id = data.VMSSC_Id;
                    var obj = _context.VMS_Statutory_ComplianceDMO.Single(a => a.VMSSC_Id == data.VMSSC_Id);

                    obj.MI_Id = data.MI_Id;
                    obj.VMSSC_Status = data.VMSSC_Status;
                    obj.IMFY_Id = data.IMFY_Id;
                    obj.VMSSC_Flag = data.VMSSC_Flag;
                    obj.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    obj.VMSSC_Remarks = data.VMSSC_Remarks;
                    obj.VMSSC_FromDate = data.VMSSC_FromDate;
                    obj.VMSSC_ToDate = data.VMSSC_ToDate;
                    obj.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    obj.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    obj.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    obj.VMSSC_CreatedDate = DateTime.Now;
                    obj.VMSSC_UpdatedDate = DateTime.Now;
                    obj.VMSSC_CreatedBy = data.UserId;
                    obj.VMSSC_UpdatedBy = data.UserId;
                    _context.Update(obj);
                }
                else
                {
                    VMS_Statutory_ComplianceDMO.MI_Id = data.MI_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Status = data.VMSSC_Status;
                    VMS_Statutory_ComplianceDMO.IMFY_Id = data.IMFY_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Flag = data.VMSSC_Flag;
                    VMS_Statutory_ComplianceDMO.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    VMS_Statutory_ComplianceDMO.VMSSC_Remarks = data.VMSSC_Remarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_FromDate = data.VMSSC_FromDate;
                    VMS_Statutory_ComplianceDMO.VMSSC_ToDate = data.VMSSC_ToDate;
                    VMS_Statutory_ComplianceDMO.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedBy = data.UserId;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedBy = data.UserId;
                    _context.Add(VMS_Statutory_ComplianceDMO);
                    VMSSC_Id = VMS_Statutory_ComplianceDMO.VMSSC_Id;
                }
                if (data.VMSSC_Id > 0)
                {
                    Array GetFiles = _context.VMS_Statutory_Compliance_DocumentsDMO.Where(a => a.VMSSC_Id == data.VMSSC_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.FILESLISTdata.Length > 0 && data.FILESLISTdata != null)
                {
                    foreach (var STC in data.FILESLISTdata)
                    {
                        VMS_Statutory_Compliance_DocumentsDMO obj11 = new VMS_Statutory_Compliance_DocumentsDMO();
                        obj11.VMSSC_Id = VMSSC_Id;
                        obj11.VMSSCD_Filename = STC.VMSSCD_Filename;
                        obj11.VMSSCD_Filepath = STC.VMSSCD_Filepath;
                        obj11.VMSSCD_ActiveFlag = true;
                        obj11.VMSSCD_CreatedDate = DateTime.Now;
                        obj11.VMSSCD_UpdatedDate = DateTime.Now;
                        obj11.VMSSCD_CreatedBy = data.UserId;
                        obj11.VMSSCD_UpdatedBy = data.UserId;
                        _context.Add(obj11);

                    }
                }
                var returnval1 = _context.SaveChanges();
                if (returnval1 > 0)
                {
                    data.message = "saved";
                }
                else
                {
                    data.message = "notsaved";
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public StatutoryTaxComplianceDTO SaveTabSeven(StatutoryTaxComplianceDTO data)
        {
            try
            {
                long VMSSC_Id = 0;
                VMS_Statutory_ComplianceDMO VMS_Statutory_ComplianceDMO = new VMS_Statutory_ComplianceDMO();
                if (data.VMSSC_Id > 0)
                {
                    VMSSC_Id = data.VMSSC_Id;
                    var obj = _context.VMS_Statutory_ComplianceDMO.Single(a => a.VMSSC_Id == data.VMSSC_Id);

                    obj.MI_Id = data.MI_Id;
                    obj.IMFY_Id = data.IMFY_Id;
                    obj.VMSSC_Flag = data.VMSSC_Flag;
                    obj.VMSSC_Status = data.VMSSC_Status;

                    obj.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    obj.VMSSC_Remarks = data.VMSSC_Remarks;
                    obj.VMSSC_FromDate = data.VMSSC_FromDate;
                    obj.VMSSC_ToDate = data.VMSSC_ToDate;
                    obj.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    obj.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    obj.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    obj.VMSSC_CreatedDate = DateTime.Now;
                    obj.VMSSC_UpdatedDate = DateTime.Now;
                    obj.VMSSC_CreatedBy = data.UserId;
                    obj.VMSSC_UpdatedBy = data.UserId;
                    _context.Update(obj);
                }
                else
                {
                    VMS_Statutory_ComplianceDMO.MI_Id = data.MI_Id;
                    VMS_Statutory_ComplianceDMO.IMFY_Id = data.IMFY_Id;
                    VMS_Statutory_ComplianceDMO.VMSSC_Status = data.VMSSC_Status;
                    VMS_Statutory_ComplianceDMO.VMSSC_Flag = data.VMSSC_Flag;
                    VMS_Statutory_ComplianceDMO.VMSSC_ReferenceNo = data.VMSSC_ReferenceNo;
                    VMS_Statutory_ComplianceDMO.VMSSC_Remarks = data.VMSSC_Remarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_FromDate = data.VMSSC_FromDate;
                    VMS_Statutory_ComplianceDMO.VMSSC_ToDate = data.VMSSC_ToDate;
                    VMS_Statutory_ComplianceDMO.VMSSC_OtherRemarks = data.VMSSC_OtherRemarks;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseFlag = data.VMSSC_CaseFlag;
                    VMS_Statutory_ComplianceDMO.VMSSC_CaseDtails = data.VMSSC_CaseDtails;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedDate = DateTime.Now;
                    VMS_Statutory_ComplianceDMO.VMSSC_CreatedBy = data.UserId;
                    VMS_Statutory_ComplianceDMO.VMSSC_UpdatedBy = data.UserId;
                    _context.Add(VMS_Statutory_ComplianceDMO);
                    VMSSC_Id = VMS_Statutory_ComplianceDMO.VMSSC_Id;
                }
                if (data.VMSSC_Id > 0)
                {
                    Array GetFiles = _context.VMS_Statutory_Compliance_DocumentsDMO.Where(a => a.VMSSC_Id == data.VMSSC_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.FILESLISTdata.Length > 0 && data.FILESLISTdata != null)
                {
                    foreach (var STC in data.FILESLISTdata)
                    {
                        VMS_Statutory_Compliance_DocumentsDMO obj11 = new VMS_Statutory_Compliance_DocumentsDMO();
                        obj11.VMSSC_Id = VMSSC_Id;
                        obj11.VMSSCD_Filename = STC.VMSSCD_Filename;
                        obj11.VMSSCD_Filepath = STC.VMSSCD_Filepath;
                        obj11.VMSSCD_ActiveFlag = true;
                        obj11.VMSSCD_CreatedDate = DateTime.Now;
                        obj11.VMSSCD_UpdatedDate = DateTime.Now;
                        obj11.VMSSCD_CreatedBy = data.UserId;
                        obj11.VMSSCD_UpdatedBy = data.UserId;
                        _context.Add(obj11);

                    }
                }
                var returnval1 = _context.SaveChanges();
                if (returnval1 > 0)
                {
                    data.message = "saved";
                }
                else
                {
                    data.message = "notsaved";
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public StatutoryTaxComplianceDTO ViewFiles(StatutoryTaxComplianceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.getviewfilesdetails = (from a in _context.VMS_Statutory_Compliance_DocumentsDMO
                                            where (a.VMSSC_Id == data.VMSSC_Id)
                                            select new StatutoryTaxComplianceDTO
                                            {
                                                VMSSCD_Id = a.VMSSCD_Id,
                                                VMSSC_Id = a.VMSSC_Id,
                                                VMSSCD_Filename = a.VMSSCD_Filename,
                                                VMSSCD_Filepath = a.VMSSCD_Filepath
                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StatutoryTaxComplianceDTO Edit(StatutoryTaxComplianceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                try
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EDit_Statutory_Data";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@VMSSC_Id",
                         SqlDbType.VarChar)
                        {
                            Value = data.VMSSC_Id
                        });

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
                                }
                            }
                            data.geteditdetails = retObject.ToArray();

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

                data.geteditfilesdetails = (from a in _context.VMS_Statutory_Compliance_DocumentsDMO
                                            where (a.VMSSC_Id == data.VMSSC_Id)
                                            select new StatutoryTaxComplianceDTO
                                            {
                                                VMSSCD_Id = a.VMSSCD_Id,
                                                VMSSC_Id = a.VMSSC_Id,
                                                VMSSCD_Filename = a.VMSSCD_Filename,
                                                VMSSCD_Filepath = a.VMSSCD_Filepath
                                            }).Distinct().ToArray();

                data.geteditfilesdetailstwo = (from a in _context.VMS_Statutory_Compliance_DocumentsDMO
                                               where (a.VMSSC_Id == data.VMSSC_Id)
                                               select new StatutoryTaxComplianceDTO
                                               {
                                                   VMSSCD_Id = a.VMSSCD_Id,
                                                   VMSSC_Id = a.VMSSC_Id,
                                                   VMSSCD_Filenametwo = a.VMSSCD_Filename,
                                                   VMSSCD_Filepathtwo = a.VMSSCD_Filepath
                                               }).Distinct().ToArray();

                data.geteditfilesdetailsthree = (from a in _context.VMS_Statutory_Compliance_DocumentsDMO
                                                 where (a.VMSSC_Id == data.VMSSC_Id)
                                                 select new StatutoryTaxComplianceDTO
                                                 {
                                                     VMSSCD_Id = a.VMSSCD_Id,
                                                     VMSSC_Id = a.VMSSC_Id,
                                                     VMSSCD_Filenamethree = a.VMSSCD_Filename,
                                                     VMSSCD_Filepaththree = a.VMSSCD_Filepath
                                                 }).Distinct().ToArray();

                data.geteditfilesdetailsfour = (from a in _context.VMS_Statutory_Compliance_DocumentsDMO
                                                where (a.VMSSC_Id == data.VMSSC_Id)
                                                select new StatutoryTaxComplianceDTO
                                                {
                                                    VMSSCD_Id = a.VMSSCD_Id,
                                                    VMSSC_Id = a.VMSSC_Id,
                                                    VMSSCD_Filenamefour = a.VMSSCD_Filename,
                                                    VMSSCD_Filepathfour = a.VMSSCD_Filepath
                                                }).Distinct().ToArray();

                data.geteditfilesdetailsfive = (from a in _context.VMS_Statutory_Compliance_DocumentsDMO
                                                where (a.VMSSC_Id == data.VMSSC_Id)
                                                select new StatutoryTaxComplianceDTO
                                                {
                                                    VMSSCD_Id = a.VMSSCD_Id,
                                                    VMSSC_Id = a.VMSSC_Id,
                                                    VMSSCD_Filenamefive = a.VMSSCD_Filename,
                                                    VMSSCD_Filepathfive = a.VMSSCD_Filepath
                                                }).Distinct().ToArray();

                data.geteditfilesdetailssix = (from a in _context.VMS_Statutory_Compliance_DocumentsDMO
                                               where (a.VMSSC_Id == data.VMSSC_Id)
                                               select new StatutoryTaxComplianceDTO
                                               {
                                                   VMSSCD_Id = a.VMSSCD_Id,
                                                   VMSSC_Id = a.VMSSC_Id,
                                                   VMSSCD_Filenamesix = a.VMSSCD_Filename,
                                                   VMSSCD_Filepathsix = a.VMSSCD_Filepath
                                               }).Distinct().ToArray();

                data.geteditfilesdetailsseven = (from a in _context.VMS_Statutory_Compliance_DocumentsDMO
                                                 where (a.VMSSC_Id == data.VMSSC_Id)
                                                 select new StatutoryTaxComplianceDTO
                                                 {
                                                     VMSSCD_Id = a.VMSSCD_Id,
                                                     VMSSC_Id = a.VMSSC_Id,
                                                     VMSSCD_Filenameseven = a.VMSSCD_Filename,
                                                     VMSSCD_Filepathseven = a.VMSSCD_Filepath
                                                 }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StatutoryTaxComplianceDTO Consultantreport(StatutoryTaxComplianceDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Consultant_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });

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
                            }
                        }
                        data.Consultant_Report = retObject.ToArray();

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
        public StatutoryTaxComplianceDTO Casereports(StatutoryTaxComplianceDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
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
                            }
                        }
                        data.Income_CaseList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "2"
                    });
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
                            }
                        }
                        data.Income_CaseListtwo = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "3"
                    });
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
                            }
                        }
                        data.Income_CaseListthree = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "4"
                    });
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
                            }
                        }
                        data.Income_CaseListfour = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "5"
                    });
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
                            }
                        }
                        data.Income_CaseListfive = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "6"
                    });
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
                            }
                        }
                        data.Income_CaseListsix = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "7"
                    });
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
                            }
                        }
                        data.Income_CaseListseven = retObject.ToArray();

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
        public StatutoryTaxComplianceDTO ClosedCasereports(StatutoryTaxComplianceDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_Closed_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
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
                            }
                        }
                        data.Income_ClosedCaseList = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_Closed_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "2"
                    });
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
                            }
                        }
                        data.Income_ClosedCaseListtwo = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_Closed_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "3"
                    });
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
                            }
                        }
                        data.Income_ClosedCaseListthree = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_Closed_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "4"
                    });
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
                            }
                        }
                        data.Income_ClosedCaseListfour = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_Closed_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "5"
                    });
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
                            }
                        }
                        data.Income_ClosedCaseListfive = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_Closed_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "6"
                    });
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
                            }
                        }
                        data.Income_ClosedCaseListsix = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_Statutory_Closed_CaseList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@VMSSC_Flag",
                     SqlDbType.VarChar)
                    {
                        Value = data.VMSSC_Flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                  SqlDbType.VarChar)
                    {
                        Value = "7"
                    });
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
                            }
                        }
                        data.Income_ClosedCaseListseven = retObject.ToArray();

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
        public StatutoryTaxComplianceDTO SendmailStatutory(StatutoryTaxComplianceDTO data)
        {
            try
            {
                var PTdate = _context.VMS_Statutory_ComplianceDMO.Where(a => a.VMSSC_Flag == "PT").Distinct().ToArray();
                var SEACdate = _context.VMS_Statutory_ComplianceDMO.Where(a => a.VMSSC_Flag == "SEAC").Distinct().ToArray();
                var OrderNoticedate = _context.VMS_Statutory_ComplianceDMO.Where(a => a.VMSSC_Flag == "OrderNotice").Distinct().ToArray();
                var TLCdate = _context.VMS_Statutory_ComplianceDMO.Where(a => a.VMSSC_Flag == "TLC").Distinct().ToArray();

                foreach (var c in PTdate)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Statutory_Mail_Remainder";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Date",
                         SqlDbType.VarChar)
                        {
                            Value = c.VMSSC_ToDate.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                        {
                            Value = "PT"
                        });
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
                                }
                            }
                            data.GetReminderdetails1 = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    if (data.GetReminderdetails1 != null && data.GetReminderdetails1.Length > 0)
                    {
                        sendmail(data.MI_Id, "ganesh.p@vapstech.com", "StatutoryTaxComplaince", c.VMSSC_ToDate.ToString("yyyy-MM-dd"), "Professional Tax (PT) Compliance", c.VMSSC_ReferenceNo);

                    }

                }
                foreach (var d in SEACdate)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Statutory_Mail_Remainder";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Date",
                         SqlDbType.VarChar)
                        {
                            Value = d.VMSSC_ToDate.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                        {
                            Value = "SEAC"
                        });
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
                                }
                            }
                            data.GetReminderdetails2 = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    if (data.GetReminderdetails2 != null && data.GetReminderdetails2.Length > 0)
                    {
                        sendmail(data.MI_Id, "ganesh.p@vapstech.com", "StatutoryTaxComplaince", d.VMSSC_ToDate.ToString("yyyy-MM-dd"), "Shops and Establishments Act Compliance", d.VMSSC_ReferenceNo);

                    }

                }
                foreach (var f in OrderNoticedate)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Statutory_Mail_Remainder";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Date",
                         SqlDbType.VarChar)
                        {
                            Value = f.VMSSC_ToDate.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                        {
                            Value = "OrderNotice"
                        });
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
                                }
                            }
                            data.GetReminderdetails3 = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    if (data.GetReminderdetails3 != null && data.GetReminderdetails3.Length > 0)
                    {
                        sendmail(data.MI_Id, "ganesh.p@vapstech.com", "StatutoryTaxComplaince", f.VMSSC_ToDate.ToString("yyyy-MM-dd"), "Order Notice", f.VMSSC_ReferenceNo);

                    }

                }
                foreach (var g in TLCdate)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Statutory_Mail_Remainder";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Date",
                         SqlDbType.VarChar)
                        {
                            Value = g.VMSSC_ToDate.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                      SqlDbType.VarChar)
                        {
                            Value = "TLC"
                        });
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
                                }
                            }
                            data.GetReminderdetails4 = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    if (data.GetReminderdetails4 != null && data.GetReminderdetails4.Length > 0)
                    {
                        sendmail(data.MI_Id, "", "StatutoryTaxComplaince", g.VMSSC_ToDate.ToString("yyyy-MM-dd"), "Trade License Compliance", g.VMSSC_ReferenceNo);

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public string sendmail(long MI_Id, string Email, string Template, string date, string Type, string Reference)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                Mailmsg = Mailmsg.Replace("[DATE1]", date);
                Mailmsg = Mailmsg.Replace("[REFNO]", Reference);
                Mailmsg = Mailmsg.Replace("[Type]", Type);
                //Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

    }

}
