using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.FMS;
using DomainModel.Model.com.vapstech.FMS;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace FMS.Com.Services
{
    public class FMS_CorrespondenceImpl : Interface.FMS_CorrespondenceInterface
    {
        public FMSContext _FMSContext;
        public FMS_CorrespondenceImpl(FMSContext FMS)
        {
            _FMSContext = FMS;
        }

        /* FIRST TAB*/
        public FMS_CorrespondenceDTO onfirsttabcorrespondence(FMS_CorrespondenceDTO data)
        {
            try
            {
                var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == data.userid).ToList();

                if (getempid.Count > 0)
                {
                    var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();

                    data.HRMD_Id = Convert.ToInt64(empdetails.FirstOrDefault().HRMD_Id);

                    var getprimary_deptdetails = (from a in _FMSContext.HR_Master_Department
                                                  from b in _FMSContext.Institution
                                                  where (a.MI_Id == b.MI_Id && a.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id)
                                                  select new FMS_CorrespondenceDTO
                                                  {
                                                      MI_Id = a.MI_Id,
                                                      HRMD_Id = a.HRMD_Id,
                                                      HRMD_DepartmentName = a.HRMD_DepartmentName + " : " + b.MI_Name
                                                  }).Distinct().ToList();

                    var getsecondary_deptdetails = (from a in _FMSContext.ISM_PDpnt_SDpnt_MappingDMO
                                                    from b in _FMSContext.Institution
                                                    from c in _FMSContext.HR_Master_Department
                                                    where (a.ISMPDSDMAP_SMI_Id == b.MI_Id && c.HRMD_Id == a.ISMPDSDMAP_SHRMD_Id && a.ISMPDSDMAP_ActiveFlag == true
                                                    && a.MI_Id == data.MI_Id && a.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id)
                                                    select new FMS_CorrespondenceDTO
                                                    {
                                                        MI_Id = a.ISMPDSDMAP_SMI_Id,
                                                        HRMD_Id = a.ISMPDSDMAP_SHRMD_Id,
                                                        HRMD_DepartmentName = c.HRMD_DepartmentName + " : " + b.MI_Name

                                                    }).Distinct().ToList();

                    foreach (var c in getsecondary_deptdetails)
                    {
                        getprimary_deptdetails.Add(c);
                    }

                    data.institutiondeptdetails = getprimary_deptdetails.ToArray();


                    data.getfilecategory = (from a in _FMSContext.FMS_Master_FileCategoryDMO
                                            from b in _FMSContext.FMS_Department_FileCategoryDMO
                                            where (a.FMSMFC_Id == b.FMSMFC_Id && a.MI_Id == data.MI_Id && b.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id
                                            && a.FMSMFC_ActiveFlg == true && b.FMSDFC_ActiveFlg == true)
                                            select a).Distinct().ToArray();

                    if (data.getfilecategory.Length > 0)
                    {
                        data.messageval = "Mapped";
                    }
                    else
                    {
                        data.messageval = "NotMappeed";
                    }

                    data.getfirstgriddata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                             from b in _FMSContext.FMS_Master_FileCategoryDMO
                                             from c in _FMSContext.IVRM_Master_FinancialYear
                                             from d in _FMSContext.FMS_Department_FileCategoryDMO
                                             where (a.FMSMFC_Id == b.FMSMFC_Id && b.FMSMFC_Id == d.FMSMFC_Id && a.IMFY_Id == c.IMFY_Id && a.MI_Id == data.MI_Id
                                             && d.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id && d.FMSDFC_ActiveFlg == true)
                                             select new FMS_CorrespondenceDTO
                                             {
                                                 FMSCOR_Id = a.FMSCOR_Id,
                                                 FMSCOR_Date = a.FMSCOR_Date,
                                                 FMSCOR_ClientSupplierFlg = a.FMSCOR_ClientSupplierFlg,
                                                 FMSCOR_Subject = a.FMSCOR_Subject,
                                                 FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                 FMSCOR_ActiveFlg = a.FMSCOR_ActiveFlg,
                                                 FMSMFC_FileCategoryName = b.FMSMFC_FileCategoryName,
                                                 IMFY_FinancialYear = c.IMFY_FinancialYear,
                                                 FMSCOR_LetterHeadNo = a.FMSCOR_LetterHeadNo,
                                                 FMSCOR_Description = a.FMSCOR_Description,
                                                 FMSCOR_CreatedDate = a.FMSCOR_CreatedDate,
                                                 FMSCOR_ToName = a.FMSCOR_ToName,
                                                 username = a.FMSCOR_CreatedBy > 0 ? _FMSContext.ApplicationUser.
                                                 Where(k => k.Id == a.FMSCOR_CreatedBy).FirstOrDefault().UserName : "",

                                                 countflag = a.FMSCOR_ClientSupplierFlg == "Others" ? 1 : 0,
                                             }).Distinct().OrderByDescending(a => a.FMSCOR_CreatedDate).ToArray();
                }
                else
                {
                    data.messageval = "NoPrivileges";
                }

                data.getgeneralconfig = _FMSContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getclient = _FMSContext.ISM_Master_Client_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true).OrderBy(a => a.ISMMCLT_ClientName).Distinct().ToArray();

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_confirmList_FMS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.getsaleslead = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //  data.getsaleslead = _FMSContext.ISM_Sales_Lead_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_ActiveFlag == true).OrderBy(a => a.ISMSLE_LeadName).Distinct().ToArray();

                data.getsuppliers = _FMSContext.INV_Master_SupplierDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMS_ActiveFlg == true).OrderBy(a => a.INVMS_SupplierName).Distinct().ToArray();

                data.getemployeelist = (from b in _FMSContext.MasterEmployee
                                        from a in _FMSContext.HR_Master_Department
                                        where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && b.HRME_LeftFlag == false && b.HRME_ActiveFlag == true)
                                        select new FMS_CorrespondenceDTO
                                        {
                                            HRME_Id = b.HRME_Id,
                                            HRME_EmployeeName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : ""
                                            + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == ""
                                            || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName)
                                            + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " "
                                            + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " : "
                                            + b.HRME_EmployeeCode)).Trim(),
                                        }).Distinct().OrderBy(a => a.HRME_EmployeeName).ToArray();

                data.getproject = _FMSContext.MastersProject_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMPR_ActiveFlg == true).OrderBy(a => a.ISMMPR_ProjectName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO onchangeclient(FMS_CorrespondenceDTO data)
        {
            try
            {
                data.getproject = (from a in _FMSContext.ISM_Master_Client_ProjectDMO
                                   from b in _FMSContext.ISM_Master_Client_DMO
                                   from c in _FMSContext.MastersProject_DMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && c.ISMMPR_Id == a.ISMMPR_Id
                                   && a.ISMMCLTPR_ActiveFlag == true && a.ISMMCLT_Id == data.ISMMCLT_Id && b.MI_Id == data.MI_Id)
                                   select new FMS_CorrespondenceDTO
                                   {
                                       ISMMPR_Id = c.ISMMPR_Id,
                                       ISMMPR_ProjectName = c.ISMMPR_ProjectName
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FMS_CorrespondenceDTO getleaddetails(FMS_CorrespondenceDTO data)
        {
            try
            {

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_confirmList_send_FMS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.BigInt)
                    {
                        Value = data.@ISMSLE_Id
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
                        data.get_lead_details = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO checkrefnoduplicate(FMS_CorrespondenceDTO data)
        {
            try
            {
                var checkduplicaterefno = _FMSContext.FMS_CorrespondenceDMO.Where(a => a.MI_Id == data.MI_Id
                && a.FMSCOR_RefernceNo.Equals(data.FMSCOR_RefernceNo)).ToList();

                if (checkduplicaterefno.Count > 0)
                {
                    data.messageval = "Duplicate";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO saveonfirsttabcorrespondence(FMS_CorrespondenceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.FMSCOR_Id > 0)
                {
                    var result = _FMSContext.FMS_CorrespondenceDMO.Single(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == data.FMSCOR_Id);

                    result.FMSCOR_Subject = data.FMSCOR_Subject;
                    result.FMSCOR_Description = data.FMSCOR_Description;
                    result.FMSCOR_LetterHeadNo = data.FMSCOR_LetterHeadNo;
                    result.FMSCOR_ToName = data.FMSCOR_ToName;
                    result.FMSCOR_Date = data.FMSCOR_Date;
                    result.FMSCOR_UpdatedDate = indiantime0;
                    result.FMSCOR_UpdatedBy = data.userid;
                    _FMSContext.Update(result);

                    if (data.FMSCOR_ClientSupplierFlg == "Client")
                    {
                        var clientresult = _FMSContext.FMS_Correspondence_ClientDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id && a.ISMMCLT_Id == data.ISMMCLT_Id).ToList();

                        if (clientresult.Count > 0)
                        {
                            var resultclient = _FMSContext.FMS_Correspondence_ClientDMO.Single(a => a.ISMMCLT_Id == data.ISMMCLT_Id && a.FMSCOR_Id == data.FMSCOR_Id);
                            resultclient.ISMMCLT_Id = data.ISMMCLT_Id;
                            if (data.ISMMPR_Id > 0)
                            {
                                resultclient.ISMMPR_Id = data.ISMMPR_Id;
                            }
                            resultclient.FMSCORCL_UpdatedDate = indiantime0;
                            resultclient.FMSCORCL_UpdatedBy = data.userid;
                            _FMSContext.Update(resultclient);
                        }
                    }

                    else if (data.FMSCOR_ClientSupplierFlg == "Supplier")
                    {
                        var supplierresult = _FMSContext.FMS_Correspondence_SupplierDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id && a.INVMS_Id == data.INVMS_Id).ToList();

                        if (supplierresult.Count > 0)
                        {
                            var resultsupplier = _FMSContext.FMS_Correspondence_SupplierDMO.Single(a => a.FMSCOR_Id == data.FMSCOR_Id && a.INVMS_Id == data.INVMS_Id);
                            resultsupplier.INVMS_Id = data.INVMS_Id;
                            if (data.ISMMPR_Id > 0)
                            {
                                resultsupplier.ISMMPR_Id = data.ISMMPR_Id;
                            }
                            resultsupplier.FMSCORSUP_UpdatedDate = indiantime0;
                            resultsupplier.FMSCORSUP_UpdatedBy = data.userid;
                            _FMSContext.Update(resultsupplier);
                        }
                    }

                    else if (data.FMSCOR_ClientSupplierFlg == "Sales Lead")
                    {
                        var salesleadresult = _FMSContext.FMS_Correspondence_SalesLeadDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id
                        && a.ISMSLE_Id == data.ISMSLE_Id).ToList();

                        if (salesleadresult.Count > 0)
                        {
                            var resultsaleslead = _FMSContext.FMS_Correspondence_SalesLeadDMO.Single(a => a.FMSCOR_Id == data.FMSCOR_Id
                            && a.ISMSLE_Id == data.ISMSLE_Id);
                            resultsaleslead.ISMSLE_Id = data.ISMSLE_Id;
                            resultsaleslead.FMSCORSL_UpdatedDate = indiantime0;
                            resultsaleslead.FMSCORSL_UpdatedBy = data.userid;
                            _FMSContext.Update(resultsaleslead);
                        }
                    }

                    else if (data.FMSCOR_ClientSupplierFlg == "Internal")
                    {
                        List<long> hrmeids = new List<long>();
                        List<long> pkids = new List<long>();


                        foreach (var c in data.employeelist)
                        {
                            hrmeids.Add(c.HRME_Id);
                            pkids.Add(c.FMSCOREMP_Id);
                        }

                        Array mappedids = _FMSContext.FMS_Correspondence_EmployeeDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id
                        && !hrmeids.Contains(a.HRME_Id)).ToArray();

                        foreach (var d in mappedids)
                        {
                            _FMSContext.Remove(d);
                        }

                        foreach (var e in data.employeelist)
                        {
                            if (e.FMSCOREMP_Id > 0)
                            {
                                var resultemp = _FMSContext.FMS_Correspondence_EmployeeDMO.Single(a => a.HRME_Id == e.HRME_Id && a.FMSCOREMP_Id == e.FMSCOREMP_Id
                                 && a.FMSCOR_Id == data.FMSCOR_Id);

                                resultemp.FMSCOREMP_UpdatedDate = indiantime0;
                                resultemp.FMSCOREMP_UpdatedBy = data.userid;

                                _FMSContext.Update(resultemp);
                            }
                            else
                            {
                                FMS_Correspondence_EmployeeDMO fMS_Correspondence_EmployeeDMO = new FMS_Correspondence_EmployeeDMO();

                                fMS_Correspondence_EmployeeDMO.FMSCOR_Id = data.FMSCOR_Id;
                                fMS_Correspondence_EmployeeDMO.HRME_Id = e.HRME_Id;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_ActiveFlg = true;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_CreatedDate = indiantime0;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_UpdatedDate = indiantime0;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_CreatedBy = data.userid;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_UpdatedBy = data.userid;

                                _FMSContext.Add(fMS_Correspondence_EmployeeDMO);
                            }
                        }

                    }

                    var i = _FMSContext.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                        data.saveorupdate = "Update";
                    }
                    else
                    {
                        data.returnval = false;
                        data.saveorupdate = "Update";
                    }
                }

                else
                {
                    var acd_Id = _FMSContext.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(data.FMSCOR_Date.Value.Date) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(data.FMSCOR_Date.Value.Date)).Select(d => d.IMFY_Id).FirstOrDefault();

                    if (data.FMSCOR_LetterHeadNo != null && data.FMSCOR_LetterHeadNo != "")
                    {
                        using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "LetterheadsNUmberChecking";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = data.HRMD_Id });
                            cmd.Parameters.Add(new SqlParameter("@Rnnum ", SqlDbType.VarChar) { Value = data.FMSCOR_LetterHeadNo });
                            cmd.Parameters.Add(new SqlParameter("@LHNo", SqlDbType.VarChar, Int32.MaxValue) { Direction = ParameterDirection.Output });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            if (cmd.Parameters["@LHNo"].Value.ToString() == "Letter Head Number Already Used")
                            {
                                data.saveorupdate = "Letter Head Number Already Used";
                                return data;
                            }
                            else if (cmd.Parameters["@LHNo"].Value.ToString() == "Letter Head Number Not Exists")
                            {
                                data.saveorupdate = "Letter Head Number Not Exists";
                                return data;
                            }
                        }

                        // Letter Head Tracking Saving

                        FMS_Department_LetterHeads_TrackingDMO fMS_Department_LetterHeads_TrackingDMO = new FMS_Department_LetterHeads_TrackingDMO();
                        fMS_Department_LetterHeads_TrackingDMO.MI_Id = data.MI_Id;
                        fMS_Department_LetterHeads_TrackingDMO.HRMD_Id = data.HRMD_Id;
                        fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_LetterheadNo = data.FMSCOR_LetterHeadNo;
                        fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_CancelledFlag = false;
                        fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_Reason = data.FMSCOR_Subject;
                        fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_CreatedBy = data.userid;
                        fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_UpdatedBy = data.userid;
                        fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_CreatedDate = indiantime0;
                        fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_UpdatedDate = indiantime0;
                        _FMSContext.Add(fMS_Department_LetterHeads_TrackingDMO);
                    }

                    var dup = _FMSContext.FMS_CorrespondenceDMO.Where(t => t.MI_Id == data.MI_Id && t.FMSCOR_Id == data.FMSCOR_Id);
                    
                    FMS_CorrespondenceDMO fMS_CorrespondenceDMO = new FMS_CorrespondenceDMO();
                    fMS_CorrespondenceDMO.MI_Id = data.MI_Id;
                    fMS_CorrespondenceDMO.FMSMFC_Id = data.FMSMFC_Id;
                    fMS_CorrespondenceDMO.IMFY_Id = acd_Id;
                    fMS_CorrespondenceDMO.FMSCOR_Date = data.FMSCOR_Date;
                    fMS_CorrespondenceDMO.FMSCOR_ClientSupplierFlg = data.FMSCOR_ClientSupplierFlg;
                    fMS_CorrespondenceDMO.FMSCOR_Subject = data.FMSCOR_Subject;
                    fMS_CorrespondenceDMO.FMSCOR_Description = data.FMSCOR_Description;
                    fMS_CorrespondenceDMO.FMSCOR_LetterHeadNo = data.FMSCOR_LetterHeadNo;
                    fMS_CorrespondenceDMO.FMSCOR_ToName = data.FMSCOR_ToName;

                    if (data.manualorautorefno == "Manual")
                    {
                        fMS_CorrespondenceDMO.FMSCOR_RefernceNo = data.FMSCOR_RefernceNo;
                    }
                    else if (data.manualorautorefno == "Auto")
                    {
                        using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "FMS_Correspondence_AutoGenerateNo";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = data.HRMD_Id });
                            cmd.Parameters.Add(new SqlParameter("@FMSMFC_Id", SqlDbType.VarChar) { Value = data.FMSMFC_Id });
                            cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar) { Value = data.ISMMCLT_Id });
                            cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar) { Value = data.ISMSLE_Id });
                            cmd.Parameters.Add(new SqlParameter("@INVMS_Id ", SqlDbType.VarChar) { Value = data.INVMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.VarChar) { Value = acd_Id });
                            cmd.Parameters.Add(new SqlParameter("@ClientSupplierFlg ", SqlDbType.VarChar) { Value = data.FMSCOR_ClientSupplierFlg });
                            cmd.Parameters.Add(new SqlParameter("@FileGenNo", SqlDbType.VarChar, Int32.MaxValue) { Direction = ParameterDirection.Output });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            if (cmd.Parameters["@FileGenNo"].Value.ToString() == "")
                            {
                                data.saveorupdate = "0";
                                return data;
                            }

                            if (cmd.Parameters["@FileGenNo"].Value.ToString() == "Codes")
                            {
                                data.saveorupdate = "RefNoErrorCodes";
                                return data;
                            }

                            fMS_CorrespondenceDMO.FMSCOR_RefernceNo = cmd.Parameters["@FileGenNo"].Value.ToString();
                        }
                    }

                    fMS_CorrespondenceDMO.FMSCOR_ActiveFlg = true;
                    fMS_CorrespondenceDMO.FMSCOR_CreatedDate = indiantime0;
                    fMS_CorrespondenceDMO.FMSCOR_UpdatedDate = indiantime0;
                    fMS_CorrespondenceDMO.FMSCOR_CreatedBy = data.userid;
                    fMS_CorrespondenceDMO.FMSCOR_UpdatedBy = data.userid;

                    _FMSContext.Add(fMS_CorrespondenceDMO);

                    if (data.FMSCOR_ClientSupplierFlg == "Client")
                    {
                        FMS_Correspondence_ClientDMO fMS_Correspondence_Client = new FMS_Correspondence_ClientDMO();
                        fMS_Correspondence_Client.FMSCOR_Id = fMS_CorrespondenceDMO.FMSCOR_Id;
                        fMS_Correspondence_Client.ISMMCLT_Id = data.ISMMCLT_Id;
                        if (data.ISMMPR_Id > 0)
                        {
                            fMS_Correspondence_Client.ISMMPR_Id = data.ISMMPR_Id;
                        }
                        fMS_Correspondence_Client.FMSCORCL_ActiveFlg = true;
                        fMS_Correspondence_Client.FMSCORCL_CreatedDate = indiantime0;
                        fMS_Correspondence_Client.FMSCORCL_UpdatedDate = indiantime0;
                        fMS_Correspondence_Client.FMSCORCL_CreatedBy = data.userid;
                        fMS_Correspondence_Client.FMSCORCL_UpdatedBy = data.userid;

                        _FMSContext.Add(fMS_Correspondence_Client);
                    }

                    else if (data.FMSCOR_ClientSupplierFlg == "Supplier")
                    {
                        FMS_Correspondence_SupplierDMO fMS_Correspondence_SupplierDMO = new FMS_Correspondence_SupplierDMO();
                        fMS_Correspondence_SupplierDMO.FMSCOR_Id = fMS_CorrespondenceDMO.FMSCOR_Id;
                        fMS_Correspondence_SupplierDMO.INVMS_Id = data.INVMS_Id;
                        if (data.ISMMPR_Id > 0)
                        {
                            fMS_Correspondence_SupplierDMO.ISMMPR_Id = data.ISMMPR_Id;
                        }
                        fMS_Correspondence_SupplierDMO.FMSCORSUP_ActiveFlg = true;
                        fMS_Correspondence_SupplierDMO.FMSCORSUP_CreatedDate = indiantime0;
                        fMS_Correspondence_SupplierDMO.FMSCORSUP_UpdatedDate = indiantime0;
                        fMS_Correspondence_SupplierDMO.FMSCORSUP_CreatedBy = data.userid;
                        fMS_Correspondence_SupplierDMO.FMSCORSUP_UpdatedBy = data.userid;

                        _FMSContext.Add(fMS_Correspondence_SupplierDMO);
                    }

                    else if (data.FMSCOR_ClientSupplierFlg == "Sales Lead")
                    {
                        var duplicate = _FMSContext.FMS_Correspondence_SalesLeadDMO.Where(R => R.ISMSLE_Id == data.ISMSLE_Id && R.FMSCORSL_ActiveFlg==true).Distinct().ToList();
                        if(duplicate.Count > 0)
                        {

                        }
                        else
                        {
                            FMS_Correspondence_SalesLeadDMO fMS_Correspondence_SalesLeadDMO = new FMS_Correspondence_SalesLeadDMO();
                            fMS_Correspondence_SalesLeadDMO.FMSCOR_Id = fMS_CorrespondenceDMO.FMSCOR_Id;
                            fMS_Correspondence_SalesLeadDMO.ISMSLE_Id = data.ISMSLE_Id;
                            fMS_Correspondence_SalesLeadDMO.FMSCORSL_ActiveFlg = true;
                            fMS_Correspondence_SalesLeadDMO.FMSCORSL_CreatedDate = indiantime0;
                            fMS_Correspondence_SalesLeadDMO.FMSCORSL_UpdatedDate = indiantime0;
                            fMS_Correspondence_SalesLeadDMO.FMSCORSL_CreatedBy = data.userid;
                            fMS_Correspondence_SalesLeadDMO.FMSCORSL_UpdatedBy = data.userid;
                            _FMSContext.Add(fMS_Correspondence_SalesLeadDMO);
                        }

                       
                    }

                    else if (data.FMSCOR_ClientSupplierFlg == "Internal")
                    {
                        if (data.employeelist != null && data.employeelist.Length > 0)
                        {
                            foreach (var c in data.employeelist)
                            {
                                FMS_Correspondence_EmployeeDMO fMS_Correspondence_EmployeeDMO = new FMS_Correspondence_EmployeeDMO();

                                fMS_Correspondence_EmployeeDMO.FMSCOR_Id = fMS_CorrespondenceDMO.FMSCOR_Id;
                                fMS_Correspondence_EmployeeDMO.HRME_Id = c.HRME_Id;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_ActiveFlg = true;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_CreatedDate = indiantime0;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_UpdatedDate = indiantime0;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_CreatedBy = data.userid;
                                fMS_Correspondence_EmployeeDMO.FMSCOREMP_UpdatedBy = data.userid;

                                _FMSContext.Add(fMS_Correspondence_EmployeeDMO);
                            }
                        }
                    }

                    var i = _FMSContext.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                        data.saveorupdate = "Add";
                    }
                    else
                    {
                        data.returnval = false;
                        data.saveorupdate = "Add";
                    }
                }
            }
            catch (Exception ex)
            {
                data.saveorupdate = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO activedeactivefirsttab(FMS_CorrespondenceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresultdetails = _FMSContext.FMS_CorrespondenceDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id).ToList();

                if (checkresultdetails.FirstOrDefault().FMSCOR_ActiveFlg == true)
                {
                    var checkidrefernceclient = _FMSContext.FMS_Correspondence_ClientDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id && a.FMSCORCL_ActiveFlg == true).Count();

                    var checkidreferncesales = _FMSContext.FMS_Correspondence_SalesLeadDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id
                    && a.FMSCORSL_ActiveFlg == true).Count();

                    var checkidreferncesup = _FMSContext.FMS_Correspondence_SupplierDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id && a.FMSCORSUP_ActiveFlg == true).Count();

                    var checkidreferncefile = _FMSContext.FMS_Correspondence_FilesDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id && a.FMSCORFL_ActiveFlg == true).Count();

                    var checkidreferncemode = _FMSContext.FMS_Correspondence_ModeDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id && a.FMSCORMD_ActiveFlg == true).Count();

                    if (checkidrefernceclient == 0 && checkidreferncesales == 0 && checkidreferncesup == 0 && checkidreferncefile == 0 && checkidreferncemode == 0)
                    {
                        var resultdeactive = _FMSContext.FMS_CorrespondenceDMO.Single(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == data.FMSCOR_Id);

                        if (resultdeactive.FMSCOR_ActiveFlg == true)
                        {
                            resultdeactive.FMSCOR_ActiveFlg = false;
                        }
                        else
                        {
                            resultdeactive.FMSCOR_ActiveFlg = true;
                        }

                        resultdeactive.FMSCOR_UpdatedDate = indiantime0;
                        resultdeactive.FMSCOR_UpdatedBy = data.userid;
                        _FMSContext.Update(resultdeactive);

                        var i = _FMSContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.messageval = "Mapped";
                    }
                }
                else
                {
                    var resultdeactive = _FMSContext.FMS_CorrespondenceDMO.Single(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == data.FMSCOR_Id);

                    if (resultdeactive.FMSCOR_ActiveFlg == true)
                    {
                        resultdeactive.FMSCOR_ActiveFlg = false;
                    }
                    else
                    {
                        resultdeactive.FMSCOR_ActiveFlg = true;
                    }

                    resultdeactive.FMSCOR_UpdatedDate = indiantime0;
                    resultdeactive.FMSCOR_UpdatedBy = data.userid;
                    _FMSContext.Update(resultdeactive);

                    var i = _FMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO Editdatafirsttab(FMS_CorrespondenceDTO data)
        {
            try
            {
                var geteditdatafirsttabd = _FMSContext.FMS_CorrespondenceDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == data.FMSCOR_Id).ToList();

                data.geteditdatafirsttab = geteditdatafirsttabd.ToArray();

                string FMSCOR_ClientSupplierFlg = geteditdatafirsttabd.FirstOrDefault().FMSCOR_ClientSupplierFlg;

                if (FMSCOR_ClientSupplierFlg == "Client")
                {
                    var geteditdataclient = (from a in _FMSContext.FMS_Correspondence_ClientDMO
                                             from b in _FMSContext.ISM_Master_Client_DMO
                                             where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                             select new FMS_CorrespondenceDTO
                                             {
                                                 ISMMCLT_Id = a.ISMMCLT_Id,
                                                 ISMMPR_Id = a.ISMMPR_Id,
                                                 FMSCORCL_Id = a.FMSCORCL_Id,
                                                 ISMMCLT_ClientName = b.ISMMCLT_ClientName
                                             }).Distinct().ToList();

                    data.geteditclientdata = geteditdataclient.ToArray();

                    data.getproject = (from a in _FMSContext.ISM_Master_Client_ProjectDMO
                                       from b in _FMSContext.ISM_Master_Client_DMO
                                       from c in _FMSContext.MastersProject_DMO
                                       where (a.ISMMCLT_Id == b.ISMMCLT_Id && c.ISMMPR_Id == a.ISMMPR_Id
                                       && a.ISMMCLTPR_ActiveFlag == true && a.ISMMCLT_Id == geteditdataclient.FirstOrDefault().ISMMCLT_Id && b.MI_Id == data.MI_Id)
                                       select new FMS_CorrespondenceDTO
                                       {
                                           ISMMPR_Id = c.ISMMPR_Id,
                                           ISMMPR_ProjectName = c.ISMMPR_ProjectName
                                       }).Distinct().ToArray();
                }

                else if (FMSCOR_ClientSupplierFlg == "Sales Lead")
                {
                    var geteditsalesleaddata = (from a in _FMSContext.FMS_Correspondence_SalesLeadDMO
                                                from b in _FMSContext.ISM_Sales_Lead_DMO
                                                where (a.ISMSLE_Id == b.ISMSLE_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                                select new FMS_CorrespondenceDTO
                                                {
                                                    ISMSLE_Id = a.ISMSLE_Id,
                                                    FMSCORSL_Id = a.FMSCORSL_Id,
                                                    ISMSLE_LeadName = b.ISMSLE_LeadName
                                                }).Distinct().ToArray();

                    data.geteditsalesdata = geteditsalesleaddata.ToArray();
                }

                else if (FMSCOR_ClientSupplierFlg == "Supplier")
                {
                    var geteditsupplierdata = (from a in _FMSContext.FMS_Correspondence_SupplierDMO
                                               from b in _FMSContext.INV_Master_SupplierDMO
                                               where (a.INVMS_Id == b.INVMS_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                               select new FMS_CorrespondenceDTO
                                               {
                                                   INVMS_Id = a.INVMS_Id,
                                                   ISMMPR_Id = a.ISMMPR_Id,
                                                   FMSCORSUP_Id = a.FMSCORSUP_Id,
                                                   INVMS_SupplierName = b.INVMS_SupplierName

                                               }).Distinct().ToArray();

                    data.geteditsupplierdata = geteditsupplierdata.ToArray();
                }

                else if (FMSCOR_ClientSupplierFlg == "Internal")
                {
                    data.geteditemployeedetails = _FMSContext.FMS_Correspondence_EmployeeDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO viewfirsttabdetails(FMS_CorrespondenceDTO data)
        {
            try
            {
                var getdetails = _FMSContext.FMS_CorrespondenceDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == data.FMSCOR_Id).ToList();

                string FMSCOR_ClientSupplierFlg = getdetails.FirstOrDefault().FMSCOR_ClientSupplierFlg;
                data.FMSCOR_ClientSupplierFlg = FMSCOR_ClientSupplierFlg;

                if (FMSCOR_ClientSupplierFlg == "Client")
                {
                    var geteditdataclient = (from a in _FMSContext.FMS_Correspondence_ClientDMO
                                             from b in _FMSContext.ISM_Master_Client_DMO
                                             where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                             select new FMS_CorrespondenceDTO
                                             {
                                                 ISMMCLT_Id = a.ISMMCLT_Id,
                                                 detailsid = a.FMSCORCL_Id,
                                                 ISMMCLT_ClientName = b.ISMMCLT_ClientName,
                                                 activeflag = a.FMSCORCL_ActiveFlg,
                                                 FMSCOR_Id = a.FMSCOR_Id,
                                                 ISMMPR_ProjectName = a.ISMMPR_Id > 0 ? _FMSContext.MastersProject_DMO.Where(a => a.ISMMPR_Id == a.ISMMPR_Id).FirstOrDefault().ISMMPR_ProjectName : "--"

                                             }).Distinct().ToList();

                    data.getviewclientdata = geteditdataclient.ToArray();

                }

                else if (FMSCOR_ClientSupplierFlg == "Sales Lead")
                {
                    var geteditsalesleaddata = (from a in _FMSContext.FMS_Correspondence_SalesLeadDMO
                                                from b in _FMSContext.ISM_Sales_Lead_DMO
                                                where (a.ISMSLE_Id == b.ISMSLE_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                                select new FMS_CorrespondenceDTO
                                                {
                                                    ISMSLE_Id = a.ISMSLE_Id,
                                                    detailsid = a.FMSCORSL_Id,
                                                    ISMSLE_LeadName = b.ISMSLE_LeadName,
                                                    activeflag = a.FMSCORSL_ActiveFlg,
                                                    FMSCOR_Id = a.FMSCOR_Id
                                                }).Distinct().ToArray();

                    data.getviewsalesdata = geteditsalesleaddata.ToArray();
                }

                else if (FMSCOR_ClientSupplierFlg == "Supplier")
                {
                    var geteditsupplierdata = (from a in _FMSContext.FMS_Correspondence_SupplierDMO
                                               from b in _FMSContext.INV_Master_SupplierDMO
                                               where (a.INVMS_Id == b.INVMS_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                               select new FMS_CorrespondenceDTO
                                               {
                                                   INVMS_Id = a.INVMS_Id,
                                                   detailsid = a.FMSCORSUP_Id,
                                                   INVMS_SupplierName = b.INVMS_SupplierName,
                                                   activeflag = a.FMSCORSUP_ActiveFlg,
                                                   FMSCOR_Id = a.FMSCOR_Id,
                                                   ISMMPR_ProjectName = a.ISMMPR_Id > 0 ? _FMSContext.MastersProject_DMO.Where(a => a.ISMMPR_Id == a.ISMMPR_Id).FirstOrDefault().ISMMPR_ProjectName : "--"
                                               }).Distinct().ToArray();

                    data.getviewsupplierdata = geteditsupplierdata.ToArray();
                }

                else if (FMSCOR_ClientSupplierFlg == "Internal")
                {
                    data.getviewemployeedetails = (from a in _FMSContext.FMS_CorrespondenceDMO
                                                   from b in _FMSContext.FMS_Correspondence_EmployeeDMO
                                                   from c in _FMSContext.MasterEmployee
                                                   where (a.FMSCOR_Id == b.FMSCOR_Id && b.HRME_Id == c.HRME_Id && a.FMSCOR_Id == data.FMSCOR_Id
                                                   && b.FMSCOR_Id == data.FMSCOR_Id)
                                                   select new FMS_CorrespondenceDTO
                                                   {
                                                       HRME_Id = b.HRME_Id,
                                                       FMSCOR_Id = b.FMSCOR_Id,
                                                       detailsid = b.FMSCOREMP_Id,
                                                       activeflag = b.FMSCOREMP_ActiveFlg,
                                                       HRME_EmployeeName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : ""
                                                       + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                       || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName)
                                                       + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " "
                                                       + c.HRME_EmployeeLastName) + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : "
                                                       + c.HRME_EmployeeCode)).Trim(),
                                                   }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO activedeactivesubdetails(FMS_CorrespondenceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                string FMSCOR_ClientSupplierFlg = data.FMSCOR_ClientSupplierFlg;
                data.FMSCOR_ClientSupplierFlg = FMSCOR_ClientSupplierFlg;

                if (FMSCOR_ClientSupplierFlg == "Client")
                {
                    var checkresultclient = _FMSContext.FMS_Correspondence_ClientDMO.Single(a => a.FMSCORCL_Id == data.details_pkid);
                    if (checkresultclient.FMSCORCL_ActiveFlg == true)
                    {
                        checkresultclient.FMSCORCL_ActiveFlg = false;
                    }
                    else
                    {
                        checkresultclient.FMSCORCL_ActiveFlg = true;
                    }
                    checkresultclient.FMSCORCL_UpdatedBy = data.userid;
                    checkresultclient.FMSCORCL_UpdatedDate = indiantime0;
                    _FMSContext.Update(checkresultclient);
                    var i = _FMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    var geteditdataclient = (from a in _FMSContext.FMS_Correspondence_ClientDMO
                                             from b in _FMSContext.ISM_Master_Client_DMO
                                             where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                             select new FMS_CorrespondenceDTO
                                             {
                                                 ISMMCLT_Id = a.ISMMCLT_Id,
                                                 detailsid = a.FMSCORCL_Id,
                                                 ISMMCLT_ClientName = b.ISMMCLT_ClientName,
                                                 activeflag = a.FMSCORCL_ActiveFlg,
                                                 FMSCOR_Id = a.FMSCOR_Id,
                                                 ISMMPR_ProjectName = a.ISMMPR_Id > 0 ? _FMSContext.MastersProject_DMO.Single(a => a.ISMMPR_Id == a.ISMMPR_Id).ISMMPR_ProjectName : "--"

                                             }).Distinct().ToList();

                    data.getviewclientdata = geteditdataclient.ToArray();

                }

                else if (FMSCOR_ClientSupplierFlg == "Sales Lead")
                {
                    var checkresultsaleslead = _FMSContext.FMS_Correspondence_SalesLeadDMO.Single(a => a.FMSCORSL_Id == data.details_pkid);
                    if (checkresultsaleslead.FMSCORSL_ActiveFlg == true)
                    {
                        checkresultsaleslead.FMSCORSL_ActiveFlg = false;
                    }
                    else
                    {
                        checkresultsaleslead.FMSCORSL_ActiveFlg = true;
                    }
                    checkresultsaleslead.FMSCORSL_UpdatedBy = data.userid;
                    checkresultsaleslead.FMSCORSL_UpdatedDate = indiantime0;
                    _FMSContext.Update(checkresultsaleslead);
                    var i = _FMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    var geteditsalesleaddata = (from a in _FMSContext.FMS_Correspondence_SalesLeadDMO
                                                from b in _FMSContext.ISM_Sales_Lead_DMO
                                                where (a.ISMSLE_Id == b.ISMSLE_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                                select new FMS_CorrespondenceDTO
                                                {
                                                    ISMSLE_Id = a.ISMSLE_Id,
                                                    detailsid = a.FMSCORSL_Id,
                                                    ISMSLE_LeadName = b.ISMSLE_LeadName,
                                                    activeflag = a.FMSCORSL_ActiveFlg,
                                                    FMSCOR_Id = a.FMSCOR_Id
                                                }).Distinct().ToArray();

                    data.getviewsalesdata = geteditsalesleaddata.ToArray();

                }

                else if (FMSCOR_ClientSupplierFlg == "Supplier")
                {
                    var checkresultsupplier = _FMSContext.FMS_Correspondence_SupplierDMO.Single(a => a.FMSCORSUP_Id == data.details_pkid);
                    if (checkresultsupplier.FMSCORSUP_ActiveFlg == true)
                    {
                        checkresultsupplier.FMSCORSUP_ActiveFlg = false;
                    }
                    else
                    {
                        checkresultsupplier.FMSCORSUP_ActiveFlg = true;
                    }
                    checkresultsupplier.FMSCORSUP_UpdatedBy = data.userid;
                    checkresultsupplier.FMSCORSUP_UpdatedDate = indiantime0;
                    _FMSContext.Update(checkresultsupplier);
                    var i = _FMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


                    var geteditsupplierdata = (from a in _FMSContext.FMS_Correspondence_SupplierDMO
                                               from b in _FMSContext.INV_Master_SupplierDMO
                                               where (a.INVMS_Id == b.INVMS_Id && a.FMSCOR_Id == data.FMSCOR_Id)
                                               select new FMS_CorrespondenceDTO
                                               {
                                                   INVMS_Id = a.INVMS_Id,
                                                   detailsid = a.FMSCORSUP_Id,
                                                   INVMS_SupplierName = b.INVMS_SupplierName,
                                                   activeflag = a.FMSCORSUP_ActiveFlg,
                                                   FMSCOR_Id = a.FMSCOR_Id
                                               }).Distinct().ToArray();

                    data.getviewsupplierdata = geteditsupplierdata.ToArray();
                }

                else if (FMSCOR_ClientSupplierFlg == "Internal")
                {
                    var checkresultemp = _FMSContext.FMS_Correspondence_EmployeeDMO.Single(a => a.FMSCOREMP_Id == data.details_pkid);
                    if (checkresultemp.FMSCOREMP_ActiveFlg == true)
                    {
                        checkresultemp.FMSCOREMP_ActiveFlg = false;
                    }
                    else
                    {
                        checkresultemp.FMSCOREMP_ActiveFlg = true;
                    }
                    checkresultemp.FMSCOREMP_UpdatedBy = data.userid;
                    checkresultemp.FMSCOREMP_UpdatedDate = indiantime0;
                    _FMSContext.Update(checkresultemp);
                    var i = _FMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    data.getviewemployeedetails = (from a in _FMSContext.FMS_CorrespondenceDMO
                                                   from b in _FMSContext.FMS_Correspondence_EmployeeDMO
                                                   from c in _FMSContext.MasterEmployee
                                                   where (a.FMSCOR_Id == b.FMSCOR_Id && b.HRME_Id == c.HRME_Id && a.FMSCOR_Id == data.FMSCOR_Id
                                                   && b.FMSCOR_Id == data.FMSCOR_Id)
                                                   select new FMS_CorrespondenceDTO
                                                   {
                                                       HRME_Id = b.HRME_Id,
                                                       FMSCOR_Id = b.FMSCOR_Id,
                                                       detailsid = b.FMSCOREMP_Id,
                                                       activeflag = b.FMSCOREMP_ActiveFlg,
                                                       HRME_EmployeeName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : ""
                                                       + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                       || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName)
                                                       + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " "
                                                       + c.HRME_EmployeeLastName) + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : "
                                                       + c.HRME_EmployeeCode)).Trim(),
                                                   }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /*GENERAL DETILS FOR SEARCH REFERNCE NO. */
        public FMS_CorrespondenceDTO searchrefernceno(FMS_CorrespondenceDTO data)
        {
            try
            {
                //var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == data.userid && a.MI_Id == data.MI_Id).ToList();

                //var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();

                if (data.TabNo == "Tab1")
                {
                    data.getfirstgriddata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                             from b in _FMSContext.FMS_Master_FileCategoryDMO
                                             from c in _FMSContext.IVRM_Master_FinancialYear
                                             from h in _FMSContext.FMS_Department_FileCategoryDMO
                                             where (a.FMSMFC_Id == b.FMSMFC_Id && b.FMSMFC_Id == h.FMSMFC_Id && a.IMFY_Id == c.IMFY_Id && a.MI_Id == data.MI_Id
                                             && a.FMSCOR_RefernceNo.Contains(data.FMSCOR_RefernceNo) && h.HRMD_Id == data.HRMD_Id)
                                             select new FMS_CorrespondenceDTO
                                             {
                                                 FMSCOR_Id = a.FMSCOR_Id,
                                                 FMSCOR_Date = a.FMSCOR_Date,
                                                 FMSCOR_ClientSupplierFlg = a.FMSCOR_ClientSupplierFlg,
                                                 FMSCOR_Subject = a.FMSCOR_Subject,
                                                 FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                 FMSCOR_ActiveFlg = a.FMSCOR_ActiveFlg,
                                                 FMSMFC_FileCategoryName = b.FMSMFC_FileCategoryName,
                                                 IMFY_FinancialYear = c.IMFY_FinancialYear,
                                                 FMSCOR_CreatedDate = a.FMSCOR_CreatedDate,
                                                 FMSCOR_LetterHeadNo = a.FMSCOR_LetterHeadNo,
                                                 FMSCOR_Description = a.FMSCOR_Description,
                                                 FMSCOR_ToName = a.FMSCOR_ToName,
                                                 countflag = a.FMSCOR_ClientSupplierFlg == "Others" || a.FMSCOR_ClientSupplierFlg == "Internal" ? 1 : 0,
                                                 username = a.FMSCOR_CreatedBy > 0 ? _FMSContext.ApplicationUser.
                                             Where(k => k.Id == a.FMSCOR_CreatedBy).FirstOrDefault().UserName : "",
                                             }).Distinct().OrderByDescending(a => a.FMSCOR_CreatedDate).ToArray();
                }

                else if (data.TabNo == "Tab2")
                {
                    data.getsecondtabdata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                             from b in _FMSContext.FMS_Correspondence_FilesDMO
                                             from c in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                             from d in _FMSContext.FMS_Master_FileCabinetDMO
                                             from e in _FMSContext.FMS_Master_FileCabinet_RowDMO
                                             from f in _FMSContext.FMS_Master_FileCabinet_ColumnDMO
                                             from g in _FMSContext.FMS_Master_FileCategoryDMO
                                             from h in _FMSContext.FMS_Department_FileCategoryDMO
                                             from i in _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO
                                             where (a.FMSCOR_Id == b.FMSCOR_Id && b.FMSMPFN_Id == c.FMSMPFN_Id && i.FMSMFCBT_Id == d.FMSMFCBT_Id
                                             && i.FMSMFCR_Id == e.FMSMFCR_Id && i.FMSMFCC_Id == f.FMSMFCC_Id && a.FMSMFC_Id == g.FMSMFC_Id
                                             && g.FMSMFC_Id == h.FMSMFC_Id && a.FMSCOR_ActiveFlg == true && a.MI_Id == data.MI_Id
                                             && h.HRMD_Id == data.HRMD_Id && a.FMSCOR_RefernceNo.Contains(data.FMSCOR_RefernceNo) && h.FMSDFC_ActiveFlg == true)
                                             select new FMS_CorrespondenceDTO
                                             {
                                                 FMSCOR_Id = a.FMSCOR_Id,
                                                 FMSMPFN_Id = b.FMSMPFN_Id,
                                                 FMSMFCBT_Id = i.FMSMFCBT_Id,
                                                 FMSMFCR_Id = i.FMSMFCR_Id,
                                                 FMSMFCC_Id = i.FMSMFCC_Id,
                                                 physicalfilename = c.FMSMPFN_PhysicalFileName,
                                                 cabinetname = d.FMSMFCBT_FileCabinetName,
                                                 cabinetrow = e.FMSMFCR_CabinetRowName,
                                                 cabinetcolumn = f.FMSMFCC_CabinetColumnName,
                                                 FMSCORFL_Id = b.FMSCORFL_Id,
                                                 FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                 FMSCOR_CreatedDate = b.FMSCORFL_CreatedDate,
                                                 FMSCORFL_ActiveFlg = b.FMSCORFL_ActiveFlg,
                                             }).Distinct().OrderByDescending(a => a.FMSCOR_CreatedDate).ToArray();
                }

                else if (data.TabNo == "Tab3")
                {
                    data.getthirdtabdata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                            from b in _FMSContext.FMS_Correspondence_ModeDMO
                                            from c in _FMSContext.FMS_Master_DispatchModeDMO
                                            from d in _FMSContext.FMS_Master_FileCategoryDMO
                                            from e in _FMSContext.FMS_Department_FileCategoryDMO
                                            where (a.FMSCOR_Id == b.FMSCOR_Id && b.FMSMDMD_Id == c.FMSMDMD_Id && a.FMSMFC_Id == d.FMSMFC_Id
                                            && d.FMSMFC_Id == e.FMSMFC_Id && a.MI_Id == data.MI_Id && a.FMSCOR_ActiveFlg == true && e.FMSDFC_ActiveFlg == true
                                            && e.HRMD_Id == data.HRMD_Id && a.FMSCOR_RefernceNo.Contains(data.FMSCOR_RefernceNo))
                                            select new FMS_CorrespondenceDTO
                                            {
                                                FMSCORMD_Id = b.FMSCORMD_Id,
                                                FMSCOR_Id = b.FMSCOR_Id,
                                                FMSCORMD_ActiveFlg = b.FMSCORMD_ActiveFlg,
                                                FMSMTMD_DispatchMode = c.FMSMTMD_DispatchMode,
                                                FMSCORMD_FileName = b.FMSCORMD_FileName,
                                                FMSCORMD_FilePath = b.FMSCORMD_FilePath,
                                                FMSCOR_CreatedDate = b.FMSCORMD_CreatedDate,
                                                FMSCORMD_SentDate = b.FMSCORMD_SentDate,
                                                FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                FMSCORMD_ReferenceNo = b.FMSCORMD_ReferenceNo,
                                                FMSMDMD_Id = b.FMSMDMD_Id
                                            }).Distinct().OrderByDescending(a => a.FMSCOR_CreatedDate).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* Search Reference Nos Based On Selection */
        public FMS_CorrespondenceDTO searchreferncenosecondtab(FMS_CorrespondenceDTO data)
        {
            try
            {
                var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == data.userid).ToList();
                long? hrmdid = 0;
                if (getempid.Count > 0)
                {
                    var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();
                    hrmdid = empdetails.FirstOrDefault().HRMD_Id;
                }

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";


                fromdatecon = Convert.ToDateTime(data.fromdate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(data.todate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                List<string> flag = new List<string>();

                foreach (var c in data.flagtypes)
                {
                    flag.Add(c.FMSCOR_ClientSupplierFlg);
                }


                data.getreferncenolist = (from a in _FMSContext.FMS_CorrespondenceDMO
                                          from b in _FMSContext.FMS_Master_FileCategoryDMO
                                          from c in _FMSContext.FMS_Department_FileCategoryDMO
                                          where (a.FMSMFC_Id == b.FMSMFC_Id && b.FMSMFC_Id == c.FMSMFC_Id && c.FMSDFC_ActiveFlg == true
                                          && a.FMSCOR_ActiveFlg == true && a.MI_Id == data.MI_Id && c.HRMD_Id == data.HRMD_Id
                                          && flag.Contains(a.FMSCOR_ClientSupplierFlg) && (a.FMSCOR_Date >= data.fromdate && a.FMSCOR_Date <= data.todate))
                                          select new FMS_CorrespondenceDTO
                                          {
                                              FMSCOR_Id = a.FMSCOR_Id,
                                              FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                              FMSCOR_Date = a.FMSCOR_Date,

                                          }).Distinct().OrderByDescending(a => a.FMSCOR_Date).ToArray();

                //using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "FMS_Search_RefernceNo_Details";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FMSCOR_ClientSupplierFlg", SqlDbType.VarChar)
                //    {
                //        Value = data.FMSCOR_ClientSupplierFlg
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                //    {
                //        Value = confromdate
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                //    {
                //        Value = contodate
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@FMSMFC_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.FMSMFC_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.ISMMCLT_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@INVMS_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.INVMS_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.ISMSLE_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar)
                //    {
                //        Value = hrmdid
                //    });

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
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.getreferncenolist = retObject.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /* Get The File Category , Client , Sales Lead , Suppliers From Selected Dept */
        public FMS_CorrespondenceDTO OnChangeDept(FMS_CorrespondenceDTO data)
        {
            try
            {
                data.getfilecategory = (from a in _FMSContext.FMS_Master_FileCategoryDMO
                                        from b in _FMSContext.FMS_Department_FileCategoryDMO
                                        where (a.FMSMFC_Id == b.FMSMFC_Id && a.MI_Id == data.MI_Id && b.HRMD_Id == data.HRMD_Id
                                        && a.FMSMFC_ActiveFlg == true && b.FMSDFC_ActiveFlg == true)
                                        select a).Distinct().ToArray();

                if (data.getfilecategory.Length > 0)
                {
                    data.messageval = "Mapped";
                }
                else
                {
                    data.messageval = "NotMappeed";
                }

                if (data.TabNo == "Tab1")
                {
                    data.getfirstgriddata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                             from b in _FMSContext.FMS_Master_FileCategoryDMO
                                             from c in _FMSContext.IVRM_Master_FinancialYear
                                             from d in _FMSContext.FMS_Department_FileCategoryDMO
                                             where (a.FMSMFC_Id == b.FMSMFC_Id && b.FMSMFC_Id == d.FMSMFC_Id && a.IMFY_Id == c.IMFY_Id && a.MI_Id == data.MI_Id
                                             && d.HRMD_Id == data.HRMD_Id && d.FMSDFC_ActiveFlg == true)
                                             select new FMS_CorrespondenceDTO
                                             {
                                                 FMSCOR_Id = a.FMSCOR_Id,
                                                 FMSCOR_Date = a.FMSCOR_Date,
                                                 FMSCOR_ClientSupplierFlg = a.FMSCOR_ClientSupplierFlg,
                                                 FMSCOR_Subject = a.FMSCOR_Subject,
                                                 FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                 FMSCOR_ActiveFlg = a.FMSCOR_ActiveFlg,
                                                 FMSMFC_FileCategoryName = b.FMSMFC_FileCategoryName,
                                                 IMFY_FinancialYear = c.IMFY_FinancialYear,
                                                 FMSCOR_LetterHeadNo = a.FMSCOR_LetterHeadNo,
                                                 FMSCOR_Description = a.FMSCOR_Description,
                                                 FMSCOR_CreatedDate = a.FMSCOR_CreatedDate,
                                                 FMSCOR_ToName = a.FMSCOR_ToName,
                                                 countflag = a.FMSCOR_ClientSupplierFlg == "Others" || a.FMSCOR_ClientSupplierFlg == "Internal" ? 1 : 0,
                                                 username = a.FMSCOR_CreatedBy > 0 ? _FMSContext.ApplicationUser.
                                                 Where(k => k.Id == a.FMSCOR_CreatedBy).FirstOrDefault().UserName : "",
                                             }).Distinct().ToArray();
                }

                else if (data.TabNo == "Tab2")
                {
                    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                    var startDate = new DateTime(indiantime0.Year, indiantime0.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    data.fromdate = startDate;
                    data.todate = endDate;

                    var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == data.userid).ToList();

                    if (getempid.Count > 0)
                    {
                        var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();

                        data.getphysicalfilelist = (from e in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                                    from d in _FMSContext.FMS_Department_PhysicalFileNameDMO
                                                    where (e.FMSMPFN_Id == d.FMSMPFN_Id && e.MI_Id == data.MI_Id && e.FMSMPFN_ActiveFlg == true
                                                    && d.HRMD_Id == data.HRMD_Id && d.FMSDPFN_ActiveFlg == true)
                                                    select new FMS_Master_PhysicalFileNameDTO
                                                    {
                                                        FMSMPFN_Id = e.FMSMPFN_Id,
                                                        FMSMPFN_PhysicalFileName = ((e.FMSMPFN_PhysicalFileName == null || e.FMSMPFN_PhysicalFileName == "" ? "" : " " + e.FMSMPFN_PhysicalFileName) +
                                                        (e.FMSMPFN_CreatedYear == null || e.FMSMPFN_CreatedYear == "" ? "" : " - " + e.FMSMPFN_CreatedYear) +
                                                        (e.FMSMPFN_FileNo == null || e.FMSMPFN_FileNo == "" ? "" : " - " + e.FMSMPFN_FileNo)).Trim(),
                                                    }).Distinct().OrderBy(a => a.FMSMPFN_PhysicalFileName).ToArray();

                        data.getcabinetlist = _FMSContext.FMS_Master_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMFCBT_ActiveFlg == true).ToArray();

                        data.getsecondtabdata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                                 from b in _FMSContext.FMS_Correspondence_FilesDMO
                                                 from c in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                                 from d in _FMSContext.FMS_Master_FileCabinetDMO
                                                 from e in _FMSContext.FMS_Master_FileCabinet_RowDMO
                                                 from f in _FMSContext.FMS_Master_FileCabinet_ColumnDMO
                                                 from g in _FMSContext.FMS_Master_FileCategoryDMO
                                                 from h in _FMSContext.FMS_Department_FileCategoryDMO
                                                 from i in _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO
                                                 where (a.FMSCOR_Id == b.FMSCOR_Id && b.FMSMPFN_Id == c.FMSMPFN_Id && b.FMSMPFN_Id == i.FMSMPFN_Id
                                                 && i.FMSMFCBT_Id == d.FMSMFCBT_Id && i.FMSMFCR_Id == e.FMSMFCR_Id && i.FMSMFCC_Id == f.FMSMFCC_Id
                                                 && a.FMSMFC_Id == g.FMSMFC_Id && g.FMSMFC_Id == h.FMSMFC_Id
                                                 && a.FMSCOR_ActiveFlg == true && a.MI_Id == data.MI_Id && h.HRMD_Id == data.HRMD_Id
                                                 && h.FMSDFC_ActiveFlg == true && d.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                                 select new FMS_CorrespondenceDTO
                                                 {
                                                     FMSCOR_Id = a.FMSCOR_Id,
                                                     FMSMPFN_Id = b.FMSMPFN_Id,
                                                     FMSMFCBT_Id = i.FMSMFCBT_Id,
                                                     FMSMFCR_Id = i.FMSMFCR_Id,
                                                     FMSMFCC_Id = i.FMSMFCC_Id,
                                                     physicalfilename = c.FMSMPFN_PhysicalFileName,
                                                     cabinetname = d.FMSMFCBT_FileCabinetName,
                                                     cabinetrow = e.FMSMFCR_CabinetRowName,
                                                     cabinetcolumn = f.FMSMFCC_CabinetColumnName,
                                                     FMSCORFL_Id = b.FMSCORFL_Id,
                                                     FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                     FMSCOR_CreatedDate = b.FMSCORFL_CreatedDate,
                                                     FMSCORFL_ActiveFlg = b.FMSCORFL_ActiveFlg
                                                 }).Distinct().OrderByDescending(a => a.FMSCOR_CreatedDate).ToArray();

                        if (data.getphysicalfilelist.Length > 0)
                        {
                            data.messageval = "Mapped";
                        }
                        else
                        {
                            data.messageval = "NotMappeed";
                        }
                    }
                }

                else if (data.TabNo == "Tab3")
                {
                    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                    var startDate = new DateTime(indiantime0.Year, indiantime0.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    data.fromdate = startDate;
                    data.todate = endDate;

                    data.getdispatchmode = _FMSContext.FMS_Master_DispatchModeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMDMD_ActiveFlg == true).ToArray();

                    data.getthirdtabdata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                            from b in _FMSContext.FMS_Correspondence_ModeDMO
                                            from c in _FMSContext.FMS_Master_DispatchModeDMO
                                            from d in _FMSContext.FMS_Master_FileCategoryDMO
                                            from e in _FMSContext.FMS_Department_FileCategoryDMO
                                            where (a.FMSCOR_Id == b.FMSCOR_Id && b.FMSMDMD_Id == c.FMSMDMD_Id && a.FMSMFC_Id == d.FMSMFC_Id
                                            && d.FMSMFC_Id == e.FMSMFC_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                            && a.FMSCOR_ActiveFlg == true && e.FMSDFC_ActiveFlg == true
                                            && e.HRMD_Id == data.HRMD_Id)
                                            select new FMS_CorrespondenceDTO
                                            {
                                                FMSCORMD_Id = b.FMSCORMD_Id,
                                                FMSCOR_Id = b.FMSCOR_Id,
                                                FMSCORMD_ActiveFlg = b.FMSCORMD_ActiveFlg,
                                                FMSMTMD_DispatchMode = c.FMSMTMD_DispatchMode,
                                                FMSCORMD_FileName = b.FMSCORMD_FileName,
                                                FMSCORMD_FilePath = b.FMSCORMD_FilePath,
                                                FMSCOR_CreatedDate = b.FMSCORMD_CreatedDate,
                                                FMSCORMD_SentDate = b.FMSCORMD_SentDate,
                                                FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                FMSCORMD_ReferenceNo = b.FMSCORMD_ReferenceNo,
                                                FMSMDMD_Id = b.FMSMDMD_Id
                                            }).Distinct().OrderByDescending(a => a.FMSCOR_CreatedDate).ToArray();
                }

                data.getgeneralconfig = _FMSContext.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getclient = _FMSContext.ISM_Master_Client_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true).OrderBy(a => a.ISMMCLT_ClientName).Distinct().ToArray();

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISm_Sales_Lead_confirmList_FMS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.getsaleslead = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // data.getsaleslead = _FMSContext.ISM_Sales_Lead_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_ActiveFlag == true).OrderBy(a => a.ISMSLE_LeadName).Distinct().ToArray();

                data.getsuppliers = _FMSContext.INV_Master_SupplierDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMS_ActiveFlg == true).OrderBy(a => a.INVMS_SupplierName).Distinct().ToArray();

                data.getproject = _FMSContext.MastersProject_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMPR_ActiveFlg == true).OrderBy(a => a.ISMMPR_ProjectName).ToArray();

                data.getemployeelist = (from b in _FMSContext.MasterEmployee
                                        from a in _FMSContext.HR_Master_Department
                                        where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && b.HRME_LeftFlag == false && b.HRME_ActiveFlag == true)
                                        select new FMS_CorrespondenceDTO
                                        {
                                            HRME_Id = b.HRME_Id,
                                            HRME_EmployeeName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : ""
                                            + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == ""
                                            || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName)
                                            + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " "
                                            + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " : "
                                            + b.HRME_EmployeeCode)).Trim(),
                                        }).Distinct().OrderBy(a => a.HRME_EmployeeName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /*SECOND TAB */
        public FMS_CorrespondenceDTO onsecondtabcorrespondence(FMS_CorrespondenceDTO data)
        {
            try
            {
                var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == data.userid).ToList();

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                
                var startDate = new DateTime(indiantime0.Year, indiantime0.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                data.fromdate = startDate;
                data.todate = endDate;

                if (getempid.Count > 0)
                {
                    var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();

                    data.HRMD_Id = Convert.ToInt64(empdetails.FirstOrDefault().HRMD_Id);

                    var getprimary_deptdetails = (from a in _FMSContext.HR_Master_Department
                                                  from b in _FMSContext.Institution
                                                  where (a.MI_Id == b.MI_Id && a.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id)
                                                  select new FMS_CorrespondenceDTO
                                                  {
                                                      MI_Id = a.MI_Id,
                                                      HRMD_Id = a.HRMD_Id,
                                                      HRMD_DepartmentName = a.HRMD_DepartmentName + " : " + b.MI_Name
                                                  }).Distinct().ToList();

                    var getsecondary_deptdetails = (from a in _FMSContext.ISM_PDpnt_SDpnt_MappingDMO
                                                    from b in _FMSContext.Institution
                                                    from c in _FMSContext.HR_Master_Department
                                                    where (a.ISMPDSDMAP_SMI_Id == b.MI_Id && c.HRMD_Id == a.ISMPDSDMAP_SHRMD_Id && a.ISMPDSDMAP_ActiveFlag == true
                                                    && a.MI_Id == data.MI_Id && a.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id)
                                                    select new FMS_CorrespondenceDTO
                                                    {
                                                        MI_Id = a.ISMPDSDMAP_SMI_Id,
                                                        HRMD_Id = a.ISMPDSDMAP_SHRMD_Id,
                                                        HRMD_DepartmentName = c.HRMD_DepartmentName + " : " + b.MI_Name

                                                    }).Distinct().ToList();

                    foreach (var c in getsecondary_deptdetails)
                    {
                        getprimary_deptdetails.Add(c);
                    }

                    data.institutiondeptdetails = getprimary_deptdetails.ToArray();

                    data.getfilecategoryfiles = (from a in _FMSContext.FMS_Master_FileCategoryDMO
                                                 from b in _FMSContext.FMS_Department_FileCategoryDMO
                                                 where (a.FMSMFC_Id == b.FMSMFC_Id && a.MI_Id == data.MI_Id && b.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id
                                                 && a.FMSMFC_ActiveFlg == true && b.FMSDFC_ActiveFlg == true)
                                                 select a).Distinct().ToArray();

                    data.getsecondtabdata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                             from b in _FMSContext.FMS_Correspondence_FilesDMO
                                             from c in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                             from d in _FMSContext.FMS_Master_FileCabinetDMO
                                             from e in _FMSContext.FMS_Master_FileCabinet_RowDMO
                                             from f in _FMSContext.FMS_Master_FileCabinet_ColumnDMO
                                             from g in _FMSContext.FMS_Master_FileCategoryDMO
                                             from h in _FMSContext.FMS_Department_FileCategoryDMO
                                             from i in _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO
                                             where (a.FMSCOR_Id == b.FMSCOR_Id && b.FMSMPFN_Id == c.FMSMPFN_Id && b.FMSMPFN_Id == i.FMSMPFN_Id
                                             && i.FMSMFCBT_Id == d.FMSMFCBT_Id && i.FMSMFCR_Id == e.FMSMFCR_Id && i.FMSMFCC_Id == f.FMSMFCC_Id
                                             && a.FMSMFC_Id == g.FMSMFC_Id && g.FMSMFC_Id == h.FMSMFC_Id && a.FMSCOR_ActiveFlg == true
                                             && a.MI_Id == data.MI_Id && h.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id
                                             && h.FMSDFC_ActiveFlg == true && d.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                             select new FMS_CorrespondenceDTO
                                             {
                                                 FMSCOR_Id = a.FMSCOR_Id,
                                                 FMSMPFN_Id = b.FMSMPFN_Id,
                                                 FMSMFCBT_Id = i.FMSMFCBT_Id,
                                                 FMSMFCR_Id = i.FMSMFCR_Id,
                                                 FMSMFCC_Id = i.FMSMFCC_Id,
                                                 physicalfilename = c.FMSMPFN_PhysicalFileName,
                                                 cabinetname = d.FMSMFCBT_FileCabinetName,
                                                 cabinetrow = e.FMSMFCR_CabinetRowName,
                                                 cabinetcolumn = f.FMSMFCC_CabinetColumnName,
                                                 FMSCORFL_Id = b.FMSCORFL_Id,
                                                 FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                 FMSCOR_CreatedDate = b.FMSCORFL_CreatedDate,
                                                 FMSCORFL_ActiveFlg = b.FMSCORFL_ActiveFlg
                                             }).Distinct().OrderByDescending(a => a.FMSCOR_CreatedDate).ToArray();                  
                }
                else
                {
                    data.messageval = "NoPrivileges";
                }               

                data.getclientfiles = _FMSContext.ISM_Master_Client_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true).Distinct().ToArray();

                data.getsalesleadfiles = _FMSContext.ISM_Sales_Lead_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_ActiveFlag == true).Distinct().ToArray();

                data.getsuppliersfiles = _FMSContext.INV_Master_SupplierDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMS_ActiveFlg == true).Distinct().ToArray();

                data.getphysicalfilelist = (from e in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                            from d in _FMSContext.FMS_Department_PhysicalFileNameDMO
                                            where (e.FMSMPFN_Id == d.FMSMPFN_Id && e.MI_Id == data.MI_Id && e.FMSMPFN_ActiveFlg == true
                                             && d.HRMD_Id == data.HRMD_Id && d.FMSDPFN_ActiveFlg == true)
                                            select new FMS_Master_PhysicalFileNameDTO
                                            {
                                                FMSMPFN_Id = e.FMSMPFN_Id,
                                                FMSMPFN_PhysicalFileName = ((e.FMSMPFN_PhysicalFileName == null || e.FMSMPFN_PhysicalFileName == "" ? "" :
                                                e.FMSMPFN_PhysicalFileName) +
                                                (e.FMSMPFN_CreatedYear == null || e.FMSMPFN_CreatedYear == "" ? "" : "/" + e.FMSMPFN_CreatedYear) +
                                                (e.FMSMPFN_FileNo == null || e.FMSMPFN_FileNo == "" ? "" : "/" + e.FMSMPFN_FileNo)).Trim(),
                                            }).Distinct().OrderBy(a => a.FMSMPFN_PhysicalFileName).ToArray();

                if (data.getphysicalfilelist.Length > 0)
                {
                    data.messageval = "Mapped";
                }
                else
                {
                    data.messageval = "NotMappeed";
                }


                data.getcabinetlist = _FMSContext.FMS_Master_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMFCBT_ActiveFlg == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO onchangefilecabinet(FMS_CorrespondenceDTO data)
        {
            try
            {
                data.getrows = _FMSContext.FMS_Master_FileCabinet_RowDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
                data.getcolumns = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO savesecondtabcorrespondence(FMS_CorrespondenceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.FMSCORFL_Id > 0)
                {
                    var resultfiles = _FMSContext.FMS_Correspondence_FilesDMO.Single(a => a.FMSCORFL_Id == data.FMSCORFL_Id);
                    resultfiles.FMSMPFN_Id = data.FMSMPFN_Id;
                    resultfiles.FMSCORFL_UpdatedBy = data.userid;
                    resultfiles.FMSCORFL_UpdatedDate = indiantime0;
                    _FMSContext.Update(resultfiles);

                }
                else
                {
                    var checkduplicate = _FMSContext.FMS_Correspondence_FilesDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id && a.FMSMPFN_Id == data.FMSMPFN_Id).Count();

                    if (checkduplicate > 0)
                    {
                        var checkresult = _FMSContext.FMS_Correspondence_FilesDMO.Single(a => a.FMSCOR_Id == data.FMSCOR_Id && a.FMSMPFN_Id == data.FMSMPFN_Id);
                        checkresult.FMSMPFN_Id = data.FMSMPFN_Id;
                        checkresult.FMSCORFL_UpdatedBy = data.userid;
                        checkresult.FMSCORFL_UpdatedDate = indiantime0;
                        _FMSContext.Update(checkresult);
                    }
                    else
                    {
                        FMS_Correspondence_FilesDMO fMS_Correspondence_FilesDMO = new FMS_Correspondence_FilesDMO();
                        fMS_Correspondence_FilesDMO.FMSCOR_Id = data.FMSCOR_Id;
                        fMS_Correspondence_FilesDMO.FMSMPFN_Id = data.FMSMPFN_Id;
                        //fMS_Correspondence_FilesDMO.FMSMFCBT_Id = data.FMSMFCBT_Id;
                        //fMS_Correspondence_FilesDMO.FMSMFCR_Id = data.FMSMFCR_Id;
                        //fMS_Correspondence_FilesDMO.FMSMFCC_Id = data.FMSMFCC_Id;
                        fMS_Correspondence_FilesDMO.FMSCORFL_ActiveFlg = true;
                        fMS_Correspondence_FilesDMO.FMSCORFL_CreatedBy = data.userid;
                        fMS_Correspondence_FilesDMO.FMSCORFL_UpdatedBy = data.userid;
                        fMS_Correspondence_FilesDMO.FMSCORFL_CreatedDate = indiantime0;
                        fMS_Correspondence_FilesDMO.FMSCORFL_UpdatedDate = indiantime0;

                        _FMSContext.Add(fMS_Correspondence_FilesDMO);
                    }
                }

                if (data.FMS_Temp_UploadFiles.Length > 0)
                {

                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    //GETTING ALL FILES
                    foreach (FMS_Temp_UploadFiles ph in data.FMS_Temp_UploadFiles)
                    {
                        temparr.Add(ph.fmscorflsC_Id);
                    }

                    //REMOVING FILES
                    Array files_Noresultremove = _FMSContext.FMS_Correspondence_Files_SoftCopyDMO.Where(t => !temparr.Contains(t.FMSCORFLSC_Id)
                    && t.FMSCOR_Id == data.FMSCOR_Id).ToArray();
                    foreach (FMS_Correspondence_Files_SoftCopyDMO ph1 in files_Noresultremove)
                    {
                        _FMSContext.Remove(ph1);
                    }

                    foreach (FMS_Temp_UploadFiles ph in data.FMS_Temp_UploadFiles)
                    {
                        string FMSCORFL_SoftCopyFileName = ph.fmscorfL_SoftCopyFileName;
                        string FMSCORFL_SoftCopyFilePath = ph.fmscorfL_SoftCopyFilePath;

                        if (ph.fmscorflsC_Id > 0)
                        {
                            var Files_Noresult = _FMSContext.FMS_Correspondence_Files_SoftCopyDMO.Single(t => t.FMSCORFLSC_Id == ph.fmscorflsC_Id);
                            Files_Noresult.FMSCORFL_SoftCopyFileName = FMSCORFL_SoftCopyFileName;
                            Files_Noresult.FMSCORFL_SoftCopyFilePath = FMSCORFL_SoftCopyFilePath;
                            Files_Noresult.FMSCORFLSC_UpdatedDate = indiantime0;
                            Files_Noresult.FMSCORFLSC_UpdatedBy = data.userid;

                            _FMSContext.Update(Files_Noresult);
                        }
                        else
                        {
                            FMS_Correspondence_Files_SoftCopyDMO fMS_Correspondence_Files_SoftCopyDMO = new FMS_Correspondence_Files_SoftCopyDMO();

                            fMS_Correspondence_Files_SoftCopyDMO.FMSCOR_Id = data.FMSCOR_Id;
                            fMS_Correspondence_Files_SoftCopyDMO.FMSCORFL_SoftCopyFileName = FMSCORFL_SoftCopyFileName;
                            fMS_Correspondence_Files_SoftCopyDMO.FMSCORFL_SoftCopyFilePath = FMSCORFL_SoftCopyFilePath;
                            fMS_Correspondence_Files_SoftCopyDMO.FMSCORFLSC_ActiveFlg = true;
                            fMS_Correspondence_Files_SoftCopyDMO.FMSCORFLSC_CreatedDate = indiantime0;
                            fMS_Correspondence_Files_SoftCopyDMO.FMSCORFLSC_UpdatedDate = indiantime0;
                            fMS_Correspondence_Files_SoftCopyDMO.FMSCORFLSC_CreatedBy = data.userid;
                            fMS_Correspondence_Files_SoftCopyDMO.FMSCORFLSC_UpdatedBy = data.userid;

                            _FMSContext.Add(fMS_Correspondence_Files_SoftCopyDMO);
                        }
                    }
                }

                var i = _FMSContext.SaveChanges();
                if (i > 0)
                {
                    data.saveorupdate = "Add";
                    data.returnval = true;
                }
                else
                {
                    data.saveorupdate = "Add";
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.saveorupdate = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO Editdatasecondtab(FMS_CorrespondenceDTO data)
        {
            try
            {
                var getfmscorid = _FMSContext.FMS_Correspondence_FilesDMO.Where(a => a.FMSCORFL_Id == data.FMSCORFL_Id).ToList();

                var fmscor_id = getfmscorid.FirstOrDefault().FMSCOR_Id;

                data.getsecondeditdata = getfmscorid.ToArray();

                var getfmscoriddetails = _FMSContext.FMS_CorrespondenceDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == fmscor_id).ToList();

                string FMSCOR_ClientSupplierFlg = getfmscoriddetails.FirstOrDefault().FMSCOR_ClientSupplierFlg;

                data.getfmcoriddetails = getfmscoriddetails.ToArray();

                data.filenamedetails = (from e in _FMSContext.FMS_Master_PhysicalFileNameDMO
                                        where (e.MI_Id == data.MI_Id && e.FMSMPFN_ActiveFlg == true && e.FMSMPFN_Id == getfmscorid.FirstOrDefault().FMSMPFN_Id)
                                        select new FMS_Master_PhysicalFileNameDTO
                                        {
                                            FMSMPFN_Id = e.FMSMPFN_Id,
                                            FMSMPFN_PhysicalFileName = ((e.FMSMPFN_PhysicalFileName == null || e.FMSMPFN_PhysicalFileName == "" ? "" :
                                            e.FMSMPFN_PhysicalFileName) +
                                            (e.FMSMPFN_CreatedYear == null || e.FMSMPFN_CreatedYear == "" ? "" : "/" + e.FMSMPFN_CreatedYear) +
                                            (e.FMSMPFN_FileNo == null || e.FMSMPFN_FileNo == "" ? "" : "/" + e.FMSMPFN_FileNo)).Trim(),
                                        }).Distinct().OrderBy(a => a.FMSMPFN_PhysicalFileName).ToArray();

                //long clientid = 0;
                //long supplierid = 0;
                //long salesleadid = 0;

                //if (FMSCOR_ClientSupplierFlg == "Client")
                //{
                //    var geteditdataclient = (from a in _FMSContext.FMS_Correspondence_ClientDMO
                //                             from b in _FMSContext.ISM_Master_Client_DMO
                //                             where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.FMSCOR_Id == fmscor_id)
                //                             select new FMS_CorrespondenceDTO
                //                             {
                //                                 ISMMCLT_Id = a.ISMMCLT_Id,
                //                                 FMSCORCL_Id = a.FMSCORCL_Id,
                //                                 ISMMCLT_ClientName = b.ISMMCLT_ClientName
                //                             }).Distinct().ToList();

                //    data.geteditclientdata = geteditdataclient.ToArray();
                //    clientid = geteditdataclient.FirstOrDefault().ISMMCLT_Id;
                //    supplierid = 0;
                //    salesleadid = 0;
                //}

                //else if (FMSCOR_ClientSupplierFlg == "Sales Lead")
                //{
                //    var geteditsalesleaddata = (from a in _FMSContext.FMS_Correspondence_SalesLeadDMO
                //                                from b in _FMSContext.ISM_Sales_Lead_DMO
                //                                where (a.ISMSLE_Id == b.ISMSLE_Id && a.FMSCOR_Id == fmscor_id)
                //                                select new FMS_CorrespondenceDTO
                //                                {
                //                                    ISMSLE_Id = a.ISMSLE_Id,
                //                                    FMSCORSL_Id = a.FMSCORSL_Id,
                //                                    ISMSLE_LeadName = b.ISMSLE_LeadName
                //                                }).Distinct().ToArray();

                //    clientid = 0;
                //    supplierid = 0;
                //    salesleadid = geteditsalesleaddata.FirstOrDefault().ISMSLE_Id;

                //    data.geteditsalesdata = geteditsalesleaddata.ToArray();
                //}

                //else if (FMSCOR_ClientSupplierFlg == "Supplier")
                //{
                //    var geteditsupplierdata = (from a in _FMSContext.FMS_Correspondence_SupplierDMO
                //                               from b in _FMSContext.INV_Master_SupplierDMO
                //                               where (a.INVMS_Id == b.INVMS_Id && a.FMSCOR_Id == fmscor_id)
                //                               select new FMS_CorrespondenceDTO
                //                               {
                //                                   INVMS_Id = a.INVMS_Id,
                //                                   FMSCORSUP_Id = a.FMSCORSUP_Id,
                //                                   INVMS_SupplierName = b.INVMS_SupplierName

                //                               }).Distinct().ToArray();

                //    clientid = 0;
                //    supplierid = geteditsupplierdata.FirstOrDefault().INVMS_Id;
                //    salesleadid = 0;

                //    data.geteditsupplierdata = geteditsupplierdata.ToArray();
                //}

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                fromdatecon = Convert.ToDateTime(getfmscoriddetails.FirstOrDefault().FMSCOR_Date.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(getfmscoriddetails.FirstOrDefault().FMSCOR_Date.Value.Date.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");


                List<string> flag = new List<string>();

                foreach (var c in data.flagtypes)
                {
                    flag.Add(c.FMSCOR_ClientSupplierFlg);
                }


                data.getreferncenolist = (from a in _FMSContext.FMS_CorrespondenceDMO
                                          from b in _FMSContext.FMS_Master_FileCategoryDMO
                                          from c in _FMSContext.FMS_Department_FileCategoryDMO
                                          where (a.FMSMFC_Id == b.FMSMFC_Id && b.FMSMFC_Id == c.FMSMFC_Id && c.FMSDFC_ActiveFlg == true
                                          && a.FMSCOR_ActiveFlg == true && a.MI_Id == data.MI_Id && c.HRMD_Id == data.HRMD_Id
                                          && flag.Contains(a.FMSCOR_ClientSupplierFlg) && (a.FMSCOR_Date >= getfmscoriddetails.FirstOrDefault().FMSCOR_Date
                                          && a.FMSCOR_Date <= getfmscoriddetails.FirstOrDefault().FMSCOR_Date))
                                          select new FMS_CorrespondenceDTO
                                          {
                                              FMSCOR_Id = a.FMSCOR_Id,
                                              FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                              FMSCOR_Date = a.FMSCOR_Date,

                                          }).Distinct().OrderByDescending(a => a.FMSCOR_Date).ToArray();

                #region comment
                //using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "FMS_Search_RefernceNo_Details";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FMSCOR_ClientSupplierFlg", SqlDbType.VarChar)
                //    {
                //        Value = FMSCOR_ClientSupplierFlg
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                //    {
                //        Value = confromdate
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                //    {
                //        Value = contodate
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@FMSMFC_Id", SqlDbType.VarChar)
                //    {
                //        Value = getfmscoriddetails.FirstOrDefault().FMSMFC_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar)
                //    {
                //        Value = clientid
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@INVMS_Id", SqlDbType.VarChar)
                //    {
                //        Value = supplierid
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                //    {
                //        Value = salesleadid
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();

                //    using (var dataReader = cmd.ExecuteReader())
                //    {
                //        while (dataReader.Read())
                //        {
                //            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //            {
                //                dataRow.Add(
                //                    dataReader.GetName(iFiled),
                //                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                );
                //            }
                //            retObject.Add((ExpandoObject)dataRow);
                //        }
                //    }
                //    data.getreferncenolist = retObject.ToArray();
                //}
                #endregion comment

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Get_RefenceNo_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_ClientSupplierFlg", SqlDbType.VarChar)
                    {
                        Value = getfmscoriddetails.FirstOrDefault().FMSCOR_ClientSupplierFlg
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_Id", SqlDbType.VarChar)
                    {
                        Value = fmscor_id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreferncedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (FMSCOR_ClientSupplierFlg == "Internal")
                {
                    data.getviewemployeedetails = (from a in _FMSContext.FMS_CorrespondenceDMO
                                                   from b in _FMSContext.FMS_Correspondence_EmployeeDMO
                                                   from c in _FMSContext.MasterEmployee
                                                   where (a.FMSCOR_Id == b.FMSCOR_Id && b.HRME_Id == c.HRME_Id && a.FMSCOR_Id == fmscor_id
                                                   && b.FMSCOR_Id == fmscor_id)
                                                   select new FMS_CorrespondenceDTO
                                                   {
                                                       HRME_Id = b.HRME_Id,
                                                       FMSCOR_Id = b.FMSCOR_Id,
                                                       detailsid = b.FMSCOREMP_Id,
                                                       activeflag = b.FMSCOREMP_ActiveFlg,
                                                       HRME_EmployeeName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : ""
                                                       + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                       || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName)
                                                       + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " "
                                                       + c.HRME_EmployeeLastName) + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : "
                                                       + c.HRME_EmployeeCode)).Trim(),
                                                   }).Distinct().ToArray();
                }


                data.getfilesdetails = _FMSContext.FMS_Correspondence_Files_SoftCopyDMO.Where(a => a.FMSCOR_Id == fmscor_id).ToArray();

                var checkcabinet = _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id
                && a.FMSMPFN_Id == getfmscorid.FirstOrDefault().FMSMPFN_Id && a.FMSMPFN_ActiveFlg == true).ToList();

                if (checkcabinet.Count > 0)
                {
                    data.checkcabinetdetails = checkcabinet.ToArray();

                    data.FMSMFCBT_Id = checkcabinet.FirstOrDefault().FMSMFCBT_Id;

                    data.getcabinetlist = _FMSContext.FMS_Master_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMFCBT_ActiveFlg == true
                    && a.FMSMFCBT_Id == checkcabinet.FirstOrDefault().FMSMFCBT_Id).ToArray();

                    data.getrows = _FMSContext.FMS_Master_FileCabinet_RowDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
                    data.getcolumns = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();


                }
                else
                {
                    data.messageval = "NoRecords";
                }

                //data.getrows = _FMSContext.FMS_Master_FileCabinet_RowDMO.Where(a => a.FMSMFCBT_Id == getfmscorid.FirstOrDefault().FMSMFCBT_Id
                //&& a.FMSMFCR_ActiveFlg == true).ToArray();

                //data.getcolumns = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Where(a => a.FMSMFCBT_Id == getfmscorid.FirstOrDefault().FMSMFCBT_Id
                //&& a.FMSMFCC_ActiveFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                data.messageval = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO onchangerefnosecondtab(FMS_CorrespondenceDTO data)
        {
            try
            {
                var getrefnodetails = _FMSContext.FMS_CorrespondenceDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == data.FMSCOR_Id).ToList();

                data.getrefnodetails = getrefnodetails.ToArray();

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Get_RefenceNo_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_ClientSupplierFlg", SqlDbType.VarChar)
                    {
                        Value = getrefnodetails.FirstOrDefault().FMSCOR_ClientSupplierFlg
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_Id", SqlDbType.VarChar)
                    {
                        Value = data.FMSCOR_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreferncedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (getrefnodetails.FirstOrDefault().FMSCOR_ClientSupplierFlg == "Internal")
                {
                    data.getviewemployeedetails = (from a in _FMSContext.FMS_CorrespondenceDMO
                                                   from b in _FMSContext.FMS_Correspondence_EmployeeDMO
                                                   from c in _FMSContext.MasterEmployee
                                                   where (a.FMSCOR_Id == b.FMSCOR_Id && b.HRME_Id == c.HRME_Id && a.FMSCOR_Id == data.FMSCOR_Id
                                                   && b.FMSCOR_Id == data.FMSCOR_Id)
                                                   select new FMS_CorrespondenceDTO
                                                   {
                                                       HRME_Id = b.HRME_Id,
                                                       FMSCOR_Id = b.FMSCOR_Id,
                                                       detailsid = b.FMSCOREMP_Id,
                                                       activeflag = b.FMSCOREMP_ActiveFlg,
                                                       HRME_EmployeeName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : ""
                                                       + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                       || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName)
                                                       + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " "
                                                       + c.HRME_EmployeeLastName) + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : "
                                                       + c.HRME_EmployeeCode)).Trim(),
                                                   }).Distinct().ToArray();
                }

                data.getfilesdetails = _FMSContext.FMS_Correspondence_Files_SoftCopyDMO.Where(a => a.FMSCOR_Id == data.FMSCOR_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO activedeactivesecondtab(FMS_CorrespondenceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getdetailsfiles = _FMSContext.FMS_Correspondence_FilesDMO.Where(a => a.FMSCORFL_Id == data.FMSCORFL_Id).ToList();

                if (getdetailsfiles.Count > 0)
                {
                    var getdetailsfilesresult = _FMSContext.FMS_Correspondence_FilesDMO.Single(a => a.FMSCORFL_Id == data.FMSCORFL_Id);
                    if (getdetailsfilesresult.FMSCORFL_ActiveFlg == true)
                    {
                        getdetailsfilesresult.FMSCORFL_ActiveFlg = false;
                    }
                    else
                    {
                        getdetailsfilesresult.FMSCORFL_ActiveFlg = true;
                    }
                    getdetailsfilesresult.FMSCORFL_UpdatedBy = data.userid;
                    getdetailsfilesresult.FMSCORFL_UpdatedDate = indiantime0;

                    _FMSContext.Update(getdetailsfilesresult);
                    var i = _FMSContext.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO ViewCorrespondanceFiles(FMS_CorrespondenceDTO data)
        {
            try
            {
                data.getviewfile = (from a in _FMSContext.FMS_CorrespondenceDMO
                                    from b in _FMSContext.FMS_Correspondence_Files_SoftCopyDMO
                                    where (a.FMSCOR_Id == b.FMSCOR_Id && b.FMSCOR_Id == data.FMSCOR_Id)
                                    select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO DeleteCorrespondanceFile(FMS_CorrespondenceDTO data)
        {
            try
            {
                var result = _FMSContext.FMS_Correspondence_Files_SoftCopyDMO.Single(a => a.FMSCOR_Id == data.FMSCOR_Id && a.FMSCORFLSC_Id == data.FMSCORFLSC_Id);

                _FMSContext.Remove(result);
                var i = _FMSContext.SaveChanges();

                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                data.getviewfile = (from a in _FMSContext.FMS_CorrespondenceDMO
                                    from b in _FMSContext.FMS_Correspondence_Files_SoftCopyDMO
                                    where (a.FMSCOR_Id == b.FMSCOR_Id && b.FMSCOR_Id == data.FMSCOR_Id)
                                    select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO OnChangephysicalpath(FMS_CorrespondenceDTO data)
        {
            try
            {
                var checkcabinet = _FMSContext.FMS_Master_PhysicalFileName_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMPFN_Id == data.FMSMPFN_Id
                && a.FMSMPFN_ActiveFlg == true).ToList();

                if (checkcabinet.Count > 0)
                {
                    data.checkcabinetdetails = checkcabinet.ToArray();

                    data.FMSMFCBT_Id = checkcabinet.FirstOrDefault().FMSMFCBT_Id;

                    data.getcabinetlist = _FMSContext.FMS_Master_FileCabinetDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMFCBT_ActiveFlg == true
                    && a.FMSMFCBT_Id == checkcabinet.FirstOrDefault().FMSMFCBT_Id).ToArray();

                    data.getrows = _FMSContext.FMS_Master_FileCabinet_RowDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();
                    data.getcolumns = _FMSContext.FMS_Master_FileCabinet_ColumnDMO.Where(a => a.FMSMFCBT_Id == data.FMSMFCBT_Id).Distinct().ToArray();


                }
                else
                {
                    data.messageval = "NoRecords";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        /*THIRD TAB*/
        public FMS_CorrespondenceDTO onthirdtabcorrespondence(FMS_CorrespondenceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var startDate = new DateTime(indiantime0.Year, indiantime0.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                data.fromdate = startDate;
                data.todate = endDate;

                var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == data.userid).ToList();

                if (getempid.Count > 0)
                {
                    var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();

                    data.HRMD_Id = Convert.ToInt64(empdetails.FirstOrDefault().HRMD_Id);

                    var getprimary_deptdetails = (from a in _FMSContext.HR_Master_Department
                                                  from b in _FMSContext.Institution
                                                  where (a.MI_Id == b.MI_Id && a.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id)
                                                  select new FMS_CorrespondenceDTO
                                                  {
                                                      MI_Id = a.MI_Id,
                                                      HRMD_Id = a.HRMD_Id,
                                                      HRMD_DepartmentName = a.HRMD_DepartmentName + " : " + b.MI_Name
                                                  }).Distinct().ToList();

                    var getsecondary_deptdetails = (from a in _FMSContext.ISM_PDpnt_SDpnt_MappingDMO
                                                    from b in _FMSContext.Institution
                                                    from c in _FMSContext.HR_Master_Department
                                                    where (a.ISMPDSDMAP_SMI_Id == b.MI_Id && c.HRMD_Id == a.ISMPDSDMAP_SHRMD_Id && a.ISMPDSDMAP_ActiveFlag == true
                                                    && a.MI_Id == data.MI_Id && a.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id)
                                                    select new FMS_CorrespondenceDTO
                                                    {
                                                        MI_Id = a.ISMPDSDMAP_SMI_Id,
                                                        HRMD_Id = a.ISMPDSDMAP_SHRMD_Id,
                                                        HRMD_DepartmentName = c.HRMD_DepartmentName + " : " + b.MI_Name

                                                    }).Distinct().ToList();

                    foreach (var c in getsecondary_deptdetails)
                    {
                        getprimary_deptdetails.Add(c);
                    }

                    data.institutiondeptdetails = getprimary_deptdetails.ToArray();

                    data.getfilecategorymode = (from a in _FMSContext.FMS_Master_FileCategoryDMO
                                                from b in _FMSContext.FMS_Department_FileCategoryDMO
                                                where (a.FMSMFC_Id == b.FMSMFC_Id && a.MI_Id == data.MI_Id && b.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id
                                                && a.FMSMFC_ActiveFlg == true && b.FMSDFC_ActiveFlg == true)
                                                select a).Distinct().ToArray();

                    if (data.getfilecategorymode.Length > 0)
                    {
                        data.messageval = "Mapped";
                    }
                    else
                    {
                        data.messageval = "NotMappeed";
                    }

                    data.getthirdtabdata = (from a in _FMSContext.FMS_CorrespondenceDMO
                                            from b in _FMSContext.FMS_Correspondence_ModeDMO
                                            from c in _FMSContext.FMS_Master_DispatchModeDMO
                                            from d in _FMSContext.FMS_Master_FileCategoryDMO
                                            from e in _FMSContext.FMS_Department_FileCategoryDMO
                                            where (a.FMSCOR_Id == b.FMSCOR_Id && b.FMSMDMD_Id == c.FMSMDMD_Id && a.FMSMFC_Id == d.FMSMFC_Id
                                            && d.FMSMFC_Id == e.FMSMFC_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id
                                            && a.FMSCOR_ActiveFlg == true && e.FMSDFC_ActiveFlg == true
                                            && e.HRMD_Id == empdetails.FirstOrDefault().HRMD_Id)
                                            select new FMS_CorrespondenceDTO
                                            {
                                                FMSCORMD_Id = b.FMSCORMD_Id,
                                                FMSCOR_Id = b.FMSCOR_Id,
                                                FMSCORMD_ActiveFlg = b.FMSCORMD_ActiveFlg,
                                                FMSMTMD_DispatchMode = c.FMSMTMD_DispatchMode,
                                                FMSCORMD_FileName = b.FMSCORMD_FileName,
                                                FMSCORMD_FilePath = b.FMSCORMD_FilePath,
                                                FMSCOR_CreatedDate = b.FMSCORMD_CreatedDate,
                                                FMSCORMD_SentDate = b.FMSCORMD_SentDate,
                                                FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                                FMSCORMD_ReferenceNo = b.FMSCORMD_ReferenceNo,
                                                FMSMDMD_Id = b.FMSMDMD_Id
                                            }).Distinct().OrderByDescending(a => a.FMSCOR_CreatedDate).ToArray();
                }
                else
                {
                    data.messageval = "NoPrivileges";
                }



                /*data.getfilecategorymode = _FMSContext.FMS_Master_FileCategoryDMO.Where(a => a.MI_Id == data.MI_Id 
                 && a.FMSMFC_ActiveFlg == true).Distinct().ToArray();*/

                data.getclientmode = _FMSContext.ISM_Master_Client_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true).Distinct().ToArray();

                data.getsalesleadmode = _FMSContext.ISM_Sales_Lead_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_ActiveFlag == true).Distinct().ToArray();

                data.getsuppliersmode = _FMSContext.INV_Master_SupplierDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMS_ActiveFlg == true).Distinct().ToArray();

                data.getdispatchmode = _FMSContext.FMS_Master_DispatchModeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSMDMD_ActiveFlg == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO savethirdtabcorrespondence(FMS_CorrespondenceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.FMSCORMD_Id > 0)
                {
                    var checkresult = _FMSContext.FMS_Correspondence_ModeDMO.Single(a => a.FMSCORMD_Id == data.FMSCORMD_Id);
                    checkresult.FMSCORMD_SentDate = data.FMSCORMD_SentDate;
                    checkresult.FMSCORMD_ReferenceNo = data.FMSCORMD_ReferenceNo;
                    if (data.FMS_Temp_UploadFiles_Mode.Length > 0)
                    {
                        checkresult.FMSCORMD_FileName = data.FMS_Temp_UploadFiles_Mode[0].fmscormD_FileName;
                        checkresult.FMSCORMD_FilePath = data.FMS_Temp_UploadFiles_Mode[0].fmscormD_FilePath;
                    }
                    else
                    {
                        checkresult.FMSCORMD_FileName = "";
                        checkresult.FMSCORMD_FilePath = "";
                    }

                    checkresult.FMSCORMD_UpdatedBy = data.userid;
                    checkresult.FMSCORMD_UpdatedDate = indiantime0;
                    _FMSContext.Update(checkresult);
                    var i = _FMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.saveorupdate = "Update";
                        data.returnval = true;
                    }
                    else
                    {
                        data.saveorupdate = "Update";
                        data.returnval = false;
                    }
                }
                else
                {
                    FMS_Correspondence_ModeDMO fMS_Correspondence_ModeDMO = new FMS_Correspondence_ModeDMO();
                    fMS_Correspondence_ModeDMO.FMSCOR_Id = data.FMSCOR_Id;
                    fMS_Correspondence_ModeDMO.FMSMDMD_Id = data.FMSMDMD_Id;
                    fMS_Correspondence_ModeDMO.FMSCORMD_ReferenceNo = data.FMSCORMD_ReferenceNo;
                    fMS_Correspondence_ModeDMO.FMSCORMD_SentDate = data.FMSCORMD_SentDate;
                    fMS_Correspondence_ModeDMO.FMSCORMD_ActiveFlg = true;

                    if (data.FMS_Temp_UploadFiles_Mode.Length > 0)
                    {
                        fMS_Correspondence_ModeDMO.FMSCORMD_FileName = data.FMS_Temp_UploadFiles_Mode[0].fmscormD_FileName;
                        fMS_Correspondence_ModeDMO.FMSCORMD_FilePath = data.FMS_Temp_UploadFiles_Mode[0].fmscormD_FilePath;
                    }
                    else
                    {
                        fMS_Correspondence_ModeDMO.FMSCORMD_FileName = "";
                        fMS_Correspondence_ModeDMO.FMSCORMD_FilePath = "";
                    }

                    fMS_Correspondence_ModeDMO.FMSCORMD_CreatedBy = data.userid;
                    fMS_Correspondence_ModeDMO.FMSCORMD_UpdatedBy = data.userid;
                    fMS_Correspondence_ModeDMO.FMSCORMD_CreatedDate = indiantime0;
                    fMS_Correspondence_ModeDMO.FMSCORMD_UpdatedDate = indiantime0;

                    _FMSContext.Add(fMS_Correspondence_ModeDMO);

                    var i = _FMSContext.SaveChanges();

                    if (i > 0)
                    {
                        data.saveorupdate = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.saveorupdate = "Add";
                        data.returnval = false;
                    }

                }
            }
            catch (Exception ex)
            {
                data.saveorupdate = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO Editdatathirdtab(FMS_CorrespondenceDTO data)
        {
            try
            {
                var getfmscorid = _FMSContext.FMS_Correspondence_ModeDMO.Where(a => a.FMSCORMD_Id == data.FMSCORMD_Id).ToList();

                var fmscor_id = getfmscorid.FirstOrDefault().FMSCOR_Id;

                data.getthirdeditdata = getfmscorid.ToArray();

                var getfmscoriddetails = _FMSContext.FMS_CorrespondenceDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == fmscor_id).ToList();

                string FMSCOR_ClientSupplierFlg = getfmscoriddetails.FirstOrDefault().FMSCOR_ClientSupplierFlg;

                data.getfmcoriddetails1 = getfmscoriddetails.ToArray();



                //long clientid = 0;
                //long supplierid = 0;
                //long salesleadid = 0;

                //if (FMSCOR_ClientSupplierFlg == "Client")
                //{
                //    var geteditdataclient = (from a in _FMSContext.FMS_Correspondence_ClientDMO
                //                             from b in _FMSContext.ISM_Master_Client_DMO
                //                             where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.FMSCOR_Id == fmscor_id)
                //                             select new FMS_CorrespondenceDTO
                //                             {
                //                                 ISMMCLT_Id = a.ISMMCLT_Id,
                //                                 FMSCORCL_Id = a.FMSCORCL_Id,
                //                                 ISMMCLT_ClientName = b.ISMMCLT_ClientName
                //                             }).Distinct().ToList();

                //    data.geteditclientdata_third = geteditdataclient.ToArray();
                //    clientid = geteditdataclient.FirstOrDefault().ISMMCLT_Id;
                //    supplierid = 0;
                //    salesleadid = 0;
                //}

                //else if (FMSCOR_ClientSupplierFlg == "Sales Lead")
                //{
                //    var geteditsalesleaddata = (from a in _FMSContext.FMS_Correspondence_SalesLeadDMO
                //                                from b in _FMSContext.ISM_Sales_Lead_DMO
                //                                where (a.ISMSLE_Id == b.ISMSLE_Id && a.FMSCOR_Id == fmscor_id)
                //                                select new FMS_CorrespondenceDTO
                //                                {
                //                                    ISMSLE_Id = a.ISMSLE_Id,
                //                                    FMSCORSL_Id = a.FMSCORSL_Id,
                //                                    ISMSLE_LeadName = b.ISMSLE_LeadName
                //                                }).Distinct().ToArray();

                //    clientid = 0;
                //    supplierid = 0;
                //    salesleadid = geteditsalesleaddata.FirstOrDefault().ISMSLE_Id;

                //    data.geteditsalesdata_third = geteditsalesleaddata.ToArray();
                //}

                //else if (FMSCOR_ClientSupplierFlg == "Supplier")
                //{
                //    var geteditsupplierdata = (from a in _FMSContext.FMS_Correspondence_SupplierDMO
                //                               from b in _FMSContext.INV_Master_SupplierDMO
                //                               where (a.INVMS_Id == b.INVMS_Id && a.FMSCOR_Id == fmscor_id)
                //                               select new FMS_CorrespondenceDTO
                //                               {
                //                                   INVMS_Id = a.INVMS_Id,
                //                                   FMSCORSUP_Id = a.FMSCORSUP_Id,
                //                                   INVMS_SupplierName = b.INVMS_SupplierName

                //                               }).Distinct().ToArray();

                //    clientid = 0;
                //    supplierid = geteditsupplierdata.FirstOrDefault().INVMS_Id;
                //    salesleadid = 0;

                //    data.geteditsupplierdata_third = geteditsupplierdata.ToArray();
                //}

                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";

                fromdatecon = Convert.ToDateTime(getfmscoriddetails.FirstOrDefault().FMSCOR_Date.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                toatecon = Convert.ToDateTime(getfmscoriddetails.FirstOrDefault().FMSCOR_Date.Value.Date.ToString("yyyy-MM-dd"));
                contodate = toatecon.ToString("yyyy-MM-dd");

                List<string> flag = new List<string>();

                foreach (var c in data.flagtypes)
                {
                    flag.Add(c.FMSCOR_ClientSupplierFlg);
                }


                data.getreferncenolist = (from a in _FMSContext.FMS_CorrespondenceDMO
                                          from b in _FMSContext.FMS_Master_FileCategoryDMO
                                          from c in _FMSContext.FMS_Department_FileCategoryDMO
                                          where (a.FMSMFC_Id == b.FMSMFC_Id && b.FMSMFC_Id == c.FMSMFC_Id && c.FMSDFC_ActiveFlg == true
                                          && a.FMSCOR_ActiveFlg == true && a.MI_Id == data.MI_Id && c.HRMD_Id == data.HRMD_Id
                                          && flag.Contains(a.FMSCOR_ClientSupplierFlg) && (a.FMSCOR_Date >= getfmscoriddetails.FirstOrDefault().FMSCOR_Date
                                          && a.FMSCOR_Date <= getfmscoriddetails.FirstOrDefault().FMSCOR_Date))
                                          select new FMS_CorrespondenceDTO
                                          {
                                              FMSCOR_Id = a.FMSCOR_Id,
                                              FMSCOR_RefernceNo = a.FMSCOR_RefernceNo,
                                              FMSCOR_Date = a.FMSCOR_Date,

                                          }).Distinct().OrderByDescending(a => a.FMSCOR_Date).ToArray();

                ///* GETTING REFERNCE NO. */
                //using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "FMS_Search_RefernceNo_Details";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                //    {
                //        Value = data.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FMSCOR_ClientSupplierFlg", SqlDbType.VarChar)
                //    {
                //        Value = FMSCOR_ClientSupplierFlg
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                //    {
                //        Value = confromdate
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                //    {
                //        Value = contodate
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@FMSMFC_Id", SqlDbType.VarChar)
                //    {
                //        Value = getfmscoriddetails.FirstOrDefault().FMSMFC_Id
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar)
                //    {
                //        Value = clientid
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@INVMS_Id", SqlDbType.VarChar)
                //    {
                //        Value = supplierid
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@ISMSLE_Id", SqlDbType.VarChar)
                //    {
                //        Value = salesleadid
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();

                //    using (var dataReader = cmd.ExecuteReader())
                //    {
                //        while (dataReader.Read())
                //        {
                //            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //            {
                //                dataRow.Add(
                //                    dataReader.GetName(iFiled),
                //                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                //                );
                //            }
                //            retObject.Add((ExpandoObject)dataRow);
                //        }
                //    }
                //    data.getreferncenolist = retObject.ToArray();
                //}

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Get_RefenceNo_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_ClientSupplierFlg", SqlDbType.VarChar)
                    {
                        Value = getfmscoriddetails.FirstOrDefault().FMSCOR_ClientSupplierFlg
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_Id", SqlDbType.VarChar)
                    {
                        Value = fmscor_id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreferncedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                if (FMSCOR_ClientSupplierFlg == "Internal")
                {
                    data.getviewemployeedetails = (from a in _FMSContext.FMS_CorrespondenceDMO
                                                   from b in _FMSContext.FMS_Correspondence_EmployeeDMO
                                                   from c in _FMSContext.MasterEmployee
                                                   where (a.FMSCOR_Id == b.FMSCOR_Id && b.HRME_Id == c.HRME_Id && a.FMSCOR_Id == fmscor_id
                                                   && b.FMSCOR_Id == fmscor_id)
                                                   select new FMS_CorrespondenceDTO
                                                   {
                                                       HRME_Id = b.HRME_Id,
                                                       FMSCOR_Id = b.FMSCOR_Id,
                                                       detailsid = b.FMSCOREMP_Id,
                                                       activeflag = b.FMSCOREMP_ActiveFlg,
                                                       HRME_EmployeeName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : ""
                                                       + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                       || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName)
                                                       + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " "
                                                       + c.HRME_EmployeeLastName) + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : "
                                                       + c.HRME_EmployeeCode)).Trim(),
                                                   }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO activedeactivethirdtab(FMS_CorrespondenceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _FMSContext.FMS_Correspondence_ModeDMO.Single(a => a.FMSCORMD_Id == data.FMSCORMD_Id);
                if (checkresult.FMSCORMD_ActiveFlg == true)
                {
                    checkresult.FMSCORMD_ActiveFlg = false;
                }
                else
                {
                    checkresult.FMSCORMD_ActiveFlg = true;
                }

                checkresult.FMSCORMD_UpdatedBy = data.userid;
                checkresult.FMSCORMD_UpdatedDate = indiantime0;
                _FMSContext.Update(checkresult);
                var i = _FMSContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO onchangerefnothirdtab(FMS_CorrespondenceDTO data)
        {
            try
            {
                var getrefnodetails = _FMSContext.FMS_CorrespondenceDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSCOR_Id == data.FMSCOR_Id).ToList();

                data.getrefnodetails = getrefnodetails.ToArray();

                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_Get_RefenceNo_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_ClientSupplierFlg", SqlDbType.VarChar)
                    {
                        Value = getrefnodetails.FirstOrDefault().FMSCOR_ClientSupplierFlg
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMSCOR_Id", SqlDbType.VarChar)
                    {
                        Value = data.FMSCOR_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreferncedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (getrefnodetails.FirstOrDefault().FMSCOR_ClientSupplierFlg == "Internal")
                {
                    data.getviewemployeedetails = (from a in _FMSContext.FMS_CorrespondenceDMO
                                                   from b in _FMSContext.FMS_Correspondence_EmployeeDMO
                                                   from c in _FMSContext.MasterEmployee
                                                   where (a.FMSCOR_Id == b.FMSCOR_Id && b.HRME_Id == c.HRME_Id && a.FMSCOR_Id == data.FMSCOR_Id
                                                   && b.FMSCOR_Id == data.FMSCOR_Id)
                                                   select new FMS_CorrespondenceDTO
                                                   {
                                                       HRME_Id = b.HRME_Id,
                                                       FMSCOR_Id = b.FMSCOR_Id,
                                                       detailsid = b.FMSCOREMP_Id,
                                                       activeflag = b.FMSCOREMP_ActiveFlg,
                                                       HRME_EmployeeName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : ""
                                                       + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                       || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName)
                                                       + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " "
                                                       + c.HRME_EmployeeLastName) + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : "
                                                       + c.HRME_EmployeeCode)).Trim(),
                                                   }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_CorrespondenceDTO OnChangeLetterHeadNo(FMS_CorrespondenceDTO data)
        {
            try
            {
                data.messageval = "";
                using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LetterheadsNUmberChecking";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = data.HRMD_Id });
                    cmd.Parameters.Add(new SqlParameter("@Rnnum ", SqlDbType.VarChar) { Value = data.FMSCOR_LetterHeadNo });
                    cmd.Parameters.Add(new SqlParameter("@LHNo", SqlDbType.VarChar, Int32.MaxValue) { Direction = ParameterDirection.Output });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data1 = cmd.ExecuteNonQuery();

                    if (cmd.Parameters["@LHNo"].Value.ToString() == "Letter Head Number Already Used")
                    {
                        data.messageval = "Letter Head Number Already Used";
                    }
                    else if (cmd.Parameters["@LHNo"].Value.ToString() == "Letter Head Number Not Exists")
                    {
                        data.messageval = "Letter Head Number Not Exists";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
