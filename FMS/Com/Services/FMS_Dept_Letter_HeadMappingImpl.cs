using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using DomainModel.Model.com.vapstech.FMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Services
{
    public class FMS_Dept_Letter_HeadMappingImpl : Interface.FMS_Dept_Letter_HeadMappingInterface
    {
        public FMSContext _context;
        public FMS_Dept_Letter_HeadMappingImpl(FMSContext _cont)
        {
            _context = _cont;
        }
        public FMS_Dept_Letter_HeadMappingDTO LoadData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                data.getdepartment = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id
                && a.HRMD_ActiveFlag == true).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.getdetails = (from a in _context.FMS_Dept_Letter_HeadMappingDMO
                                   from b in _context.Institution
                                   from c in _context.HR_Master_Department
                                   where (a.HRMD_Id == c.HRMD_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id
                                   && a.FMSDFC_ActiveFlg == true)
                                   select new FMS_Dept_Letter_HeadMappingDTO
                                   {
                                       FMSDLH_Id = a.FMSDLH_Id,
                                       HRMD_Id = a.HRMD_Id,
                                       MI_Id = a.MI_Id,
                                       MI_Name = b.MI_Name,
                                       HRMD_DepartmentName = c.HRMD_DepartmentName,
                                       FMSDLH_LetterheadsFromNo = a.FMSDLH_LetterheadsFromNo,
                                       FMSDLH_LetterheadsToNo = a.FMSDLH_LetterheadsToNo,
                                       FMSDLH_ReceivedDate = a.FMSDLH_ReceivedDate,
                                       FMSDLH_ReceivedBy = a.FMSDLH_ReceivedBy,
                                       createddate = a.FMSDFC_CreatedDate
                                   }).Distinct().OrderByDescending(a => a.createddate).ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO OnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                data.getdepartment = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id
               && a.HRMD_ActiveFlag == true).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.getdetails = (from a in _context.FMS_Dept_Letter_HeadMappingDMO
                                   from b in _context.Institution
                                   from c in _context.HR_Master_Department
                                   where (a.HRMD_Id == c.HRMD_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.FMSDFC_ActiveFlg == true)
                                   select new FMS_Dept_Letter_HeadMappingDTO
                                   {
                                       FMSDLH_Id = a.FMSDLH_Id,
                                       HRMD_Id = a.HRMD_Id,
                                       MI_Id = a.MI_Id,
                                       MI_Name = b.MI_Name,
                                       HRMD_DepartmentName = c.HRMD_DepartmentName,
                                       FMSDLH_LetterheadsFromNo = a.FMSDLH_LetterheadsFromNo,
                                       FMSDLH_LetterheadsToNo = a.FMSDLH_LetterheadsToNo,
                                       FMSDLH_ReceivedDate = a.FMSDLH_ReceivedDate,
                                       FMSDLH_ReceivedBy = a.FMSDLH_ReceivedBy,
                                       createddate = a.FMSDFC_CreatedDate
                                   }).Distinct().OrderByDescending(a => a.createddate).ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO OnSaveData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.FMSDLH_Id > 0)
                {
                    data.message = "Update";
                    var checkresult = _context.FMS_Dept_Letter_HeadMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSDLH_Id == data.FMSDLH_Id).ToList();
                    if (checkresult.Count > 0)
                    {
                        var result = _context.FMS_Dept_Letter_HeadMappingDMO.Single(a => a.MI_Id == data.MI_Id && a.FMSDLH_Id == data.FMSDLH_Id);

                        result.FMSDLH_ReceivedDate = data.FMSDLH_ReceivedDate;
                        result.FMSDLH_ReceivedBy = data.FMSDLH_ReceivedBy;
                        result.FMSDLH_LetterheadsFromNo = data.FMSDLH_LetterheadsFromNo;
                        result.FMSDLH_LetterheadsToNo = data.FMSDLH_LetterheadsToNo;
                        result.FMSDFC_UpdatedBy = data.UserId;
                        result.FMSDFC_UpdatedDate = indiantime0;
                        _context.Update(result);
                        var i = _context.SaveChanges();
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
                else
                {
                    data.message = "Add";
                    FMS_Dept_Letter_HeadMappingDMO fMS_Dept_Letter_HeadMappingDMO = new FMS_Dept_Letter_HeadMappingDMO();
                    fMS_Dept_Letter_HeadMappingDMO.MI_Id = data.MI_Id;
                    fMS_Dept_Letter_HeadMappingDMO.HRMD_Id = data.HRMD_Id;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDLH_ReceivedDate = data.FMSDLH_ReceivedDate;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDLH_ReceivedBy = data.FMSDLH_ReceivedBy;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDLH_LetterheadsFromNo = data.FMSDLH_LetterheadsFromNo;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDLH_LetterheadsToNo = data.FMSDLH_LetterheadsToNo;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDFC_ActiveFlg = true;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDFC_CreatedBy = data.UserId;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDFC_UpdatedBy = data.UserId;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDFC_CreatedDate = indiantime0;
                    fMS_Dept_Letter_HeadMappingDMO.FMSDFC_UpdatedDate = indiantime0;
                    _context.Add(fMS_Dept_Letter_HeadMappingDMO);
                    var i = _context.SaveChanges();
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
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO EditLetterHead(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                data.geteditdetails = _context.FMS_Dept_Letter_HeadMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.FMSDLH_Id == data.FMSDLH_Id).ToArray();

                data.getdepartment = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id
               && a.HRMD_ActiveFlag == true).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO OnDelete(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                var result = _context.FMS_Dept_Letter_HeadMappingDMO.Single(a => a.MI_Id == data.MI_Id && a.FMSDLH_Id == data.FMSDLH_Id);
                _context.Remove(result);
                var i = _context.SaveChanges();
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Letter Head Tracking
        public FMS_Dept_Letter_HeadMappingDTO TrackingLoadData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                data.getdepartment = (from a in _context.HR_Master_Department
                                      from b in _context.FMS_Dept_Letter_HeadMappingDMO
                                      where (a.HRMD_Id == b.HRMD_Id && b.FMSDFC_ActiveFlg == true && b.MI_Id == data.MI_Id)
                                      select a).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Distinct().ToArray();

                data.getusedletterheads = (from a in _context.FMS_Department_LetterHeads_TrackingDMO
                                           from b in _context.HR_Master_Department
                                           from c in _context.ApplicationUser
                                           where (a.HRMD_Id == b.HRMD_Id && a.FMSDLHT_CreatedBy == c.Id && a.MI_Id == data.MI_Id)
                                           select new FMS_Dept_Letter_HeadMappingDTO
                                           {
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               FMSDLHT_LetterheadNo = a.FMSDLHT_LetterheadNo,
                                               FMSDLHT_CancelledFlag = a.FMSDLHT_CancelledFlag,
                                               FMSDLHT_Reason = a.FMSDLHT_Reason,
                                               FMSDLHT_Date = a.FMSDLHT_Date,
                                               UserName = c.UserName,
                                               FMSDLHT_Id = a.FMSDLHT_Id,
                                               UserId = a.FMSDLHT_CreatedBy
                                           }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                data.getdepartment = (from a in _context.HR_Master_Department
                                      from b in _context.FMS_Dept_Letter_HeadMappingDMO
                                      where (a.HRMD_Id == b.HRMD_Id && b.FMSDFC_ActiveFlg == true && b.MI_Id == data.MI_Id)
                                      select a).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Distinct().ToArray();

                data.getusedletterheads = (from a in _context.FMS_Department_LetterHeads_TrackingDMO
                                           from b in _context.HR_Master_Department
                                           from c in _context.ApplicationUser
                                           where (a.HRMD_Id == b.HRMD_Id && a.FMSDLHT_CreatedBy == c.Id && a.MI_Id == data.MI_Id)
                                           select new FMS_Dept_Letter_HeadMappingDTO
                                           {
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               FMSDLHT_LetterheadNo = a.FMSDLHT_LetterheadNo,
                                               FMSDLHT_CancelledFlag = a.FMSDLHT_CancelledFlag,
                                               FMSDLHT_Reason = a.FMSDLHT_Reason,
                                               FMSDLHT_Date = a.FMSDLHT_Date,
                                               UserName = c.UserName,
                                               FMSDLHT_Id = a.FMSDLHT_Id
                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeDept(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                data.getusedletterheads = (from a in _context.FMS_Department_LetterHeads_TrackingDMO
                                           from b in _context.HR_Master_Department
                                           from c in _context.ApplicationUser
                                           where (a.HRMD_Id == b.HRMD_Id && a.FMSDLHT_CreatedBy == c.Id && a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id)
                                           select new FMS_Dept_Letter_HeadMappingDTO
                                           {
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               FMSDLHT_LetterheadNo = a.FMSDLHT_LetterheadNo,
                                               FMSDLHT_CancelledFlag = a.FMSDLHT_CancelledFlag,
                                               FMSDLHT_Reason = a.FMSDLHT_Reason,
                                               FMSDLHT_Date = a.FMSDLHT_Date,
                                               UserName = c.UserName,
                                               FMSDLHT_Id = a.FMSDLHT_Id,
                                               UserId = a.FMSDLHT_CreatedBy
                                           }).Distinct().ToArray();

                data.getdeptletterheadlist = _context.FMS_Dept_Letter_HeadMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id
                && a.FMSDFC_ActiveFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnSaveData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.FMSDLHT_Id > 0)
                {
                    data.message = "Update";

                    var checkresult = _context.FMS_Department_LetterHeads_TrackingDMO.Where(a => a.FMSDLHT_Id == data.FMSDLHT_Id).ToList();
                    if (checkresult.Count > 0)
                    {
                        var result = _context.FMS_Department_LetterHeads_TrackingDMO.Single(a => a.FMSDLHT_Id == data.FMSDLHT_Id);

                        result.FMSDLHT_CancelledFlag = data.FMSDLHT_CancelledFlag;
                        result.FMSDLHT_Reason = data.FMSDLHT_Reason;
                        result.FMSDLHT_UpdatedBy = data.UserId;
                        result.FMSDLHT_UpdatedDate = indiantime0;
                        _context.Update(result);
                        var i = _context.SaveChanges();
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
                else
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LetterheadsNUmberChecking";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.VarChar) { Value = data.HRMD_Id });
                        cmd.Parameters.Add(new SqlParameter("@Rnnum ", SqlDbType.VarChar) { Value = data.FMSDLHT_LetterheadNo });
                        cmd.Parameters.Add(new SqlParameter("@LHNo", SqlDbType.VarChar, Int32.MaxValue) { Direction = ParameterDirection.Output });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        if (cmd.Parameters["@LHNo"].Value.ToString() == "Letter Head Number Already Used")
                        {
                            data.message = "Letter Head Number Already Used";
                            return data;
                        }
                        else if (cmd.Parameters["@LHNo"].Value.ToString() == "Letter Head Number Not Exists")
                        {
                            data.message = "Letter Head Number Not Exists";
                            return data;
                        }
                    }

                    data.message = "Add";
                    FMS_Department_LetterHeads_TrackingDMO fMS_Department_LetterHeads_TrackingDMO = new FMS_Department_LetterHeads_TrackingDMO();
                    fMS_Department_LetterHeads_TrackingDMO.MI_Id = data.MI_Id;
                    fMS_Department_LetterHeads_TrackingDMO.HRMD_Id = data.HRMD_Id;
                    fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_LetterheadNo = data.FMSDLHT_LetterheadNo;
                    fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_CancelledFlag = data.FMSDLHT_CancelledFlag;
                    fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_Reason = data.FMSDLHT_Reason;
                    fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_Date = data.FMSDLHT_Date;
                    fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_CreatedBy = data.UserId;
                    fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_UpdatedBy = data.UserId;
                    fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_CreatedDate = indiantime0;
                    fMS_Department_LetterHeads_TrackingDMO.FMSDLHT_UpdatedDate = indiantime0;

                    _context.Add(fMS_Department_LetterHeads_TrackingDMO);
                    var i = _context.SaveChanges();
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
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO TrackingEditLetterHead(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.trackingeditdata = _context.FMS_Department_LetterHeads_TrackingDMO.Where(a => a.FMSDLHT_Id == data.FMSDLHT_Id).ToArray();

            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnDelete(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _context.FMS_Department_LetterHeads_TrackingDMO.Single(a => a.FMSDLHT_Id == data.FMSDLHT_Id);
                result.FMSDLHT_UpdatedBy = data.UserId;
                result.FMSDLHT_UpdatedDate = indiantime0;
                _context.Update(result);
                var i = _context.SaveChanges();
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
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Report
        public FMS_Dept_Letter_HeadMappingDTO ReportLoadData(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

                data.getdepartment = (from a in _context.HR_Master_Department
                                      from b in _context.FMS_Dept_Letter_HeadMappingDMO
                                      where (a.HRMD_Id == b.HRMD_Id && b.FMSDFC_ActiveFlg == true && b.MI_Id == data.MI_Id)
                                      select a).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO ReportOnChangeInstiution(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                data.getdepartment = (from a in _context.HR_Master_Department
                                      from b in _context.FMS_Dept_Letter_HeadMappingDMO
                                      where (a.HRMD_Id == b.HRMD_Id && b.FMSDFC_ActiveFlg == true && b.MI_Id == data.MI_Id)
                                      select a).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO ReportOnChangeDept(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {

                data.getdeptletterheadlist = _context.FMS_Dept_Letter_HeadMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id
                && a.FMSDFC_ActiveFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FMS_Dept_Letter_HeadMappingDTO GetLetterHeadReport(FMS_Dept_Letter_HeadMappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FMS_LetterHeadNotExistNoList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRMD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromNo", SqlDbType.VarChar)
                    {
                        Value = data.FMSDLH_LetterheadsFromNo
                    });

                    cmd.Parameters.Add(new SqlParameter("@ToNo", SqlDbType.VarChar)
                    {
                        Value = data.FMSDLH_LetterheadsToNo
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getreport = retObject.ToArray();
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
    }
}