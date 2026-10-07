using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;
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
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class LaptopApplicationImpl : Interfaces.LaptopApplicationInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public LaptopApplicationImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public VMS_Laptop_Application_DTO getdetails(VMS_Laptop_Application_DTO data)
        {
            try
            {
                //======================= Driver Indent
           //     using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
           //     {
           //         cmd.CommandText = "Insert_ISM_Driver_Indent";
           //         cmd.CommandType = CommandType.StoredProcedure;

           //         cmd.Parameters.Add(new SqlParameter("@MI_Id",
           // SqlDbType.BigInt)
           //         {
           //             Value = data.MI_Id
           //         });

           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_Date",
           //  SqlDbType.DateTime)
           //         {
           //             Value = data.ISMDIT_Date
           //         });

           //         cmd.Parameters.Add(new SqlParameter("@TRMV_Id",
           // SqlDbType.BigInt)
           //         {
           //             Value = data.TRMV_Id
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_BillNo",
           // SqlDbType.VarChar)
           //         {
           //             Value = data.ISMDIT_BillNo
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_Qty",
           // SqlDbType.Decimal)
           //         {
           //             Value = data.ISMDIT_Qty
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_Amount",
           // SqlDbType.Decimal)
           //         {
           //             Value = data.ISMDIT_Amount
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_Remarks",
           // SqlDbType.VarChar)
           //         {
           //             Value = data.ISMDIT_Remarks
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_OpeningKM",
           // SqlDbType.Decimal)
           //         {
           //             Value = data.ISMDIT_OpeningKM
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_ClosingKM",
           // SqlDbType.Decimal)
           //         {
           //             Value = data.ISMDIT_ClosingKM
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_BalanceDiesel",
           // SqlDbType.Decimal)
           //         {
           //             Value = data.ISMDIT_BalanceDiesel
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_PreviousReading",
           //SqlDbType.Decimal)
           //         {
           //             Value = data.ISMDIT_PreviousReading
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_CurrentReading",
           //SqlDbType.Decimal)
           //         {
           //             Value = data.ISMDIT_CurrentReading
           //         });

           //         cmd.Parameters.Add(new SqlParameter("@CreatedDate",
           //                    SqlDbType.DateTime)
           //         {
           //             Value = data.CreatedDate
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@UpdatedDate",
           //SqlDbType.DateTime)
           //         {
           //             Value = data.UpdatedDate
           //         });
           //         cmd.Parameters.Add(new SqlParameter("@ISMDIT_PreparedByUserId",
           //SqlDbType.BigInt)
           //         {
           //             Value = data.ISMDIT_PreparedByUserId
           //         });

           //         if (cmd.Connection.State != ConnectionState.Open)
           //             cmd.Connection.Open();
           //         var data1 = cmd.ExecuteNonQuery();
           //         if(data1>0)
           //         {
           //             data.responsemessage = "Sucess";
           //         }
           //         else
           //         {
           //             data.responsemessage = "fail";
           //         }
           //         //var retObject = new List<dynamic>();
           //         //try
           //         //{
           //         //    using (var dataReader1 = cmd.ExecuteReader())
           //         //    {
           //         //        while (dataReader1.Read())
           //         //        {
           //         //            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
           //         //            for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
           //         //            {
           //         //                dataRow1.Add(
           //         //                    dataReader1.GetName(iFiled1),
           //         //                    dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
           //         //                );
           //         //            }

           //         //            retObject.Add((ExpandoObject)dataRow1);
           //         //        }
           //         //    }
           //         //    data.DriverIndentList = retObject.ToArray();
           //         //}
           //         //catch (Exception ex)
           //         //{
           //         //    Console.WriteLine(ex.Message);
           //         //}
           //     }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_Laptop_Application_DTO loaddata(VMS_Laptop_Application_DTO data)
        {
            try
            {
                data.HRME_Id = _IssueContext.Staff_User_Login.Where(t => t.Id == data.User_Id).Select(t => t.Emp_Code).FirstOrDefault();

                data.alldata = (from a in _IssueContext.VMS_Laptop_Application_DMO
                                where (a.VLAPAPPL_ActiveFlag == true && a.HRME_Id == data.HRME_Id)
                                select new VMS_Laptop_Application_DTO
                                {
                                    VLAPAPPL_Id = a.VLAPAPPL_Id,
                                    VLAPAPPL_ApplicationDate = a.VLAPAPPL_ApplicationDate,
                                    VLAPAPPL_FromDate = a.VLAPAPPL_FromDate,
                                    VLAPAPPL_ToDate = a.VLAPAPPL_ToDate,
                                    VLAPAPPL_TotalDays = a.VLAPAPPL_TotalDays,
                                    VLAPAPPL_Reason = a.VLAPAPPL_Reason,
                                    VLAPAPPL_ApplicationStatus = a.VLAPAPPL_ApplicationStatus,
                                }).Distinct().OrderBy(t => t.VLAPAPPL_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_Laptop_Application_DTO save(VMS_Laptop_Application_DTO data)
        {
            try
            {
                data.HRME_Id = _IssueContext.Staff_User_Login.Where(c => c.Id == data.User_Id && c.MI_Id == data.MI_Id).FirstOrDefault().Emp_Code;

                long asmayid = _IssueContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1 && t.Is_Active == true && t.ASMAY_From_Date <= DateTime.Today && t.ASMAY_To_Date >= DateTime.Today).Select(t => t.ASMAY_Id).FirstOrDefault();

                if (data.VLAPAPPL_Id == 0)
                {
                    var duplicate = _IssueContext.VMS_Laptop_Application_DMO.Where(t => t.MI_Id == data.MI_Id && t.VLAPAPPL_FromDate == data.VLAPAPPL_FromDate && t.VLAPAPPL_ToDate == data.VLAPAPPL_ToDate && t.VLAPAPPL_TotalDays == data.VLAPAPPL_TotalDays).Distinct().ToList();

                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var transnumconfigsettings = _Context.Master_Numbering.Where(d => d.MI_Id == data.MI_Id && d.IMN_Flag.Equals("LaptopApproval")).ToList();
                        if (transnumconfigsettings.Count > 0)
                        {
                            GenerateTransactionNumbering aa = new GenerateTransactionNumbering(_Context);
                            Master_NumberingDTO num = new Master_NumberingDTO();
                            num.MI_Id = data.MI_Id;
                            num.ASMAY_Id = asmayid;
                            num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                            num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                            num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                            num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                            num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                            num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                            num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                            num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                            num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                            num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                            num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                            num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                            num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                            num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                            num.IMN_Flag = "LeaveNo";
                            data.VLAPAPPL_ApplicationID = aa.GenerateNumber(num);
                        }

                        VMS_Laptop_Application_DMO a = new VMS_Laptop_Application_DMO();
                        a.MI_Id = data.MI_Id;
                        a.HRME_Id = data.HRME_Id;
                        a.VLAPAPPL_ApplicationID = data.VLAPAPPL_ApplicationID;
                        a.VLAPAPPL_ApplicationDate = DateTime.Today;
                        a.VLAPAPPL_FromDate = data.VLAPAPPL_FromDate;
                        a.VLAPAPPL_ToDate = data.VLAPAPPL_ToDate;
                        a.VLAPAPPL_TotalDays = data.VLAPAPPL_TotalDays;
                        a.VLAPAPPL_Reason = data.VLAPAPPL_Reason;
                        a.VLAPAPPL_ApplicationStatus = "Applied";
                        a.VLAPAPPL_SupportingDocument = data.VLAPAPPL_SupportingDocument;
                        a.VLAPAPPL_ActiveFlag = true;
                        a.VLAPAPPL_CreatedDate = DateTime.Today;
                        a.VLAPAPPL_UpdatedDate = DateTime.Today;
                        _IssueContext.Add(a);
                        var w = _IssueContext.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "Saved";
                            //SendEmailIndentApply(data, "Driver Indent Request", "Driver Indent Request", data.MI_Id);
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.VLAPAPPL_Id > 0)
                {
                    var duplicate = _IssueContext.VMS_Laptop_Application_DMO.Where(t => t.VLAPAPPL_Id != data.VLAPAPPL_Id && t.MI_Id == data.MI_Id && t.VLAPAPPL_FromDate == data.VLAPAPPL_FromDate && t.VLAPAPPL_ToDate == data.VLAPAPPL_ToDate).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var j = _IssueContext.VMS_Laptop_Application_DMO.Where(t => t.MI_Id == data.MI_Id & t.VLAPAPPL_Id == data.VLAPAPPL_Id).SingleOrDefault();
                        j.VLAPAPPL_FromDate = data.VLAPAPPL_FromDate;
                        j.VLAPAPPL_ToDate = data.VLAPAPPL_ToDate;
                        j.VLAPAPPL_TotalDays = data.VLAPAPPL_TotalDays;
                        j.VLAPAPPL_Reason = data.VLAPAPPL_Reason;
                        j.VLAPAPPL_ApplicationStatus = data.VLAPAPPL_ApplicationStatus;
                        j.VLAPAPPL_SupportingDocument = data.VLAPAPPL_SupportingDocument;
                        _IssueContext.Update(j);
                        var r = _IssueContext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_Laptop_Application_DTO edittab1(VMS_Laptop_Application_DTO data)
        {
            try
            {
                data.editlist = _IssueContext.VMS_Laptop_Application_DMO.Where(t => t.VLAPAPPL_Id == data.VLAPAPPL_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<VMS_Laptop_Application_DTO> getApprovalStatusAsync(VMS_Laptop_Application_DTO data)
        {
            try
            {
                long userhrmeid = _IssueContext.Staff_User_Login.Where(t => t.Id == data.User_Id).Select(t => t.Emp_Code).FirstOrDefault();

                string userdepthead = (from a in _IssueContext.HR_Master_DepartmentCodeDMO
                                       from b in _IssueContext.HR_Master_DepartmentCode_Head
                                       where (a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == userhrmeid)
                                       select a).Select(t => t.HRMDC_Code).FirstOrDefault();

                if(userdepthead == "HW") {
                    data.Approvalrequestlist = (from a in _IssueContext.VMS_Laptop_Application_DMO
                                                from b in _IssueContext.MasterEmployee
                                                from c in _IssueContext.VMS_Laptop_Application_Authorisation_DMO
                                                where (a.HRME_Id == b.HRME_Id && a.VLAPAPPL_Id == c.VLAPAPPL_Id && c.VLAPAPPLAUT_SanctioningLevel ==1 && a.VLAPAPPL_ApplicationStatus == "TL Approved" && a.VLAPAPPL_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                                select new VMS_Laptop_Application_DTO
                                                {
                                                    VLAPAPPL_Id = a.VLAPAPPL_Id,
                                                    HRME_Id = a.HRME_Id,
                                                    HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                                    VLAPAPPL_FromDate = a.VLAPAPPL_FromDate,
                                                    VLAPAPPL_ToDate = a.VLAPAPPL_ToDate,
                                                    VLAPAPPL_TotalDays = a.VLAPAPPL_TotalDays,
                                                    VLAPAPPL_Reason = a.VLAPAPPL_Reason,
                                                    VLAPAPPL_SupportingDocument = a.VLAPAPPL_SupportingDocument,
                                                    VLAPAPPL_ApplicationStatus = a.VLAPAPPL_ApplicationStatus
                                                }).OrderBy(t => t.VLAPAPPL_ApplicationDate).ToArray();

                    data.applicationApproved = (from a in _IssueContext.VMS_Laptop_Application_DMO
                                                from b in _IssueContext.MasterEmployee
                                                from c in _IssueContext.VMS_Laptop_Application_Authorisation_DMO
                                                where (a.HRME_Id == b.HRME_Id && a.VLAPAPPL_Id == c.VLAPAPPL_Id && a.VLAPAPPL_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && c.VLAPAPPLAUT_SanctioningLevel == 2 && c.VLAPAPPLAUT_ActiveFlg == true && c.VLAPAPPLAUT_CreatedBy == data.User_Id)
                                                select new VMS_Laptop_Application_DTO
                                                {
                                                    VLAPAPPL_Id = a.VLAPAPPL_Id,
                                                    HRME_Id = a.HRME_Id,
                                                    HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                                    VLAPAPPL_FromDate = a.VLAPAPPL_FromDate,
                                                    VLAPAPPL_ToDate = a.VLAPAPPL_ToDate,
                                                    VLAPAPPL_TotalDays = a.VLAPAPPL_TotalDays,
                                                    VLAPAPPL_Reason = a.VLAPAPPL_Reason,
                                                    VLAPAPPL_SupportingDocument = a.VLAPAPPL_SupportingDocument,
                                                    VLAPAPPLAUT_Remarks = c.VLAPAPPLAUT_Remarks,
                                                    VLAPAPPL_ApplicationStatus = a.VLAPAPPL_ApplicationStatus
                                                }).OrderBy(t => t.VLAPAPPL_ApplicationDate).ToArray();
                }
                else if(userdepthead == "ADMIN") {

                    data.Approvalrequestlist = (from a in _IssueContext.VMS_Laptop_Application_DMO
                                                from b in _IssueContext.MasterEmployee
                                                from c in _IssueContext.VMS_Laptop_Application_Authorisation_DMO
                                                where (a.HRME_Id == b.HRME_Id && a.VLAPAPPL_Id == c.VLAPAPPL_Id && c.VLAPAPPLAUT_SanctioningLevel == 2 && a.VLAPAPPL_ApplicationStatus == "HW Approved" && a.VLAPAPPL_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                                select new VMS_Laptop_Application_DTO
                                                {
                                                    VLAPAPPL_Id = a.VLAPAPPL_Id,
                                                    HRME_Id = a.HRME_Id,
                                                    HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                                    VLAPAPPL_FromDate = a.VLAPAPPL_FromDate,
                                                    VLAPAPPL_ToDate = a.VLAPAPPL_ToDate,
                                                    VLAPAPPL_TotalDays = a.VLAPAPPL_TotalDays,
                                                    VLAPAPPL_Reason = a.VLAPAPPL_Reason,
                                                    VLAPAPPL_SupportingDocument = a.VLAPAPPL_SupportingDocument,
                                                    VLAPAPPL_ApplicationStatus = a.VLAPAPPL_ApplicationStatus
                                                }).OrderBy(t => t.VLAPAPPL_ApplicationDate).ToArray();

                    data.applicationApproved = (from a in _IssueContext.VMS_Laptop_Application_DMO
                                                from b in _IssueContext.MasterEmployee
                                                from c in _IssueContext.VMS_Laptop_Application_Authorisation_DMO
                                                where (a.HRME_Id == b.HRME_Id && a.VLAPAPPL_Id == c.VLAPAPPL_Id && a.VLAPAPPL_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && c.VLAPAPPLAUT_SanctioningLevel == 3 && c.VLAPAPPLAUT_ActiveFlg == true && c.VLAPAPPLAUT_CreatedBy == data.User_Id)
                                                select new VMS_Laptop_Application_DTO
                                                {
                                                    VLAPAPPL_Id = a.VLAPAPPL_Id,
                                                    HRME_Id = a.HRME_Id,
                                                    HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                                    VLAPAPPL_FromDate = a.VLAPAPPL_FromDate,
                                                    VLAPAPPL_ToDate = a.VLAPAPPL_ToDate,
                                                    VLAPAPPL_TotalDays = a.VLAPAPPL_TotalDays,
                                                    VLAPAPPL_Reason = a.VLAPAPPL_Reason,
                                                    VLAPAPPL_SupportingDocument = a.VLAPAPPL_SupportingDocument,
                                                    VLAPAPPLAUT_Remarks = c.VLAPAPPLAUT_Remarks,
                                                    VLAPAPPL_ApplicationStatus = a.VLAPAPPL_ApplicationStatus
                                                }).OrderBy(t => t.VLAPAPPL_ApplicationDate).ToArray();
                }
                else {
                    data.Approvalrequestlist = (from a in _IssueContext.VMS_Laptop_Application_DMO
                                           from b in _IssueContext.MasterEmployee
                                           from c in _IssueContext.HR_Master_Department
                                           from d in _IssueContext.HR_Master_DepartmentCode_Head
                                           where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && c.HRMDC_ID == d.HRMDC_ID && d.HRME_ID == userhrmeid && a.VLAPAPPL_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && a.VLAPAPPL_ApplicationStatus == "Applied")
                                           select new VMS_Laptop_Application_DTO
                                           {
                                               VLAPAPPL_Id = a.VLAPAPPL_Id,
                                               HRME_Id = a.HRME_Id,
                                               HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "":b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "":b.HRME_EmployeeLastName),
                                               VLAPAPPL_FromDate = a.VLAPAPPL_FromDate,
                                               VLAPAPPL_ToDate = a.VLAPAPPL_ToDate,
                                               VLAPAPPL_TotalDays = a.VLAPAPPL_TotalDays,
                                               VLAPAPPL_Reason = a.VLAPAPPL_Reason,
                                               VLAPAPPL_SupportingDocument = a.VLAPAPPL_SupportingDocument,
                                               VLAPAPPL_ApplicationStatus = a.VLAPAPPL_ApplicationStatus
                                           }).OrderBy(t => t.VLAPAPPL_ApplicationDate).ToArray();

                    data.applicationApproved = (from a in _IssueContext.VMS_Laptop_Application_DMO
                                                from b in _IssueContext.MasterEmployee
                                                from c in _IssueContext.HR_Master_Department
                                                from d in _IssueContext.HR_Master_DepartmentCode_Head
                                                from e in _IssueContext.VMS_Laptop_Application_Authorisation_DMO
                                                where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && c.HRMDC_ID == d.HRMDC_ID && d.HRME_ID == userhrmeid && a.VLAPAPPL_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && a.VLAPAPPL_Id == e.VLAPAPPL_Id && e.VLAPAPPLAUT_SanctioningLevel == 1 && e.VLAPAPPLAUT_ActiveFlg == true && e.VLAPAPPLAUT_CreatedBy == data.User_Id)
                                                select new VMS_Laptop_Application_DTO
                                                {
                                                    VLAPAPPL_Id = a.VLAPAPPL_Id,
                                                    HRME_Id = a.HRME_Id,
                                                    HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                                    VLAPAPPL_FromDate = a.VLAPAPPL_FromDate,
                                                    VLAPAPPL_ToDate = a.VLAPAPPL_ToDate,
                                                    VLAPAPPL_TotalDays = a.VLAPAPPL_TotalDays,
                                                    VLAPAPPL_Reason = a.VLAPAPPL_Reason,
                                                    VLAPAPPL_SupportingDocument = a.VLAPAPPL_SupportingDocument,
                                                    VLAPAPPLAUT_Remarks = e.VLAPAPPLAUT_Remarks,
                                                    VLAPAPPL_ApplicationStatus = a.VLAPAPPL_ApplicationStatus
                                                }).OrderBy(t => t.VLAPAPPL_ApplicationDate).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_Laptop_Application_DTO get_status(VMS_Laptop_Application_DTO data)
        {
            try
            {
                long userhrmeid = _IssueContext.Staff_User_Login.Where(t => t.Id == data.User_Id).Select(t => t.Emp_Code).FirstOrDefault();

                string userdepthead = (from a in _IssueContext.HR_Master_DepartmentCodeDMO
                                       from b in _IssueContext.HR_Master_DepartmentCode_Head
                                       where (a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == userhrmeid)
                                       select a).Select(t => t.HRMDC_Code).FirstOrDefault();

                if(userdepthead == "ADMIN")
                {
                    data.VLAPAPPLAUT_SanctioningLevel = 3;
                }
                else if (userdepthead == "HW") {
                    data.VLAPAPPLAUT_SanctioningLevel = 2;
                }
                else
                {
                    userdepthead = "TL";
                    data.VLAPAPPLAUT_SanctioningLevel = 1;
                }

                for (int i = 0; i < data.get_approval_status.Length; i++)
                {
                    VMS_Laptop_Application_Authorisation_DMO obj1 = new VMS_Laptop_Application_Authorisation_DMO();
                    obj1.VLAPAPPL_Id = data.get_approval_status[i].VLAPAPPL_Id;
                    obj1.IVRMUL_Id = userhrmeid;
                    obj1.VLAPAPPLAUT_SanctioningLevel = data.VLAPAPPLAUT_SanctioningLevel;
                    obj1.VLAPAPPLAUT_Remarks = data.VLAPAPPLAUT_Remarks;
                    obj1.VLAPAPPLAUT_Date = DateTime.Today;
                    obj1.VLAPAPPLAUT_ActiveFlg = true;
                    obj1.VLAPAPPLAUT_CreatedBy = data.User_Id;
                    obj1.VLAPAPPLAUT_CreatedDate = DateTime.Today;
                    obj1.VLAPAPPLAUT_UpdatedBy = data.User_Id;
                    obj1.VLAPAPPLAUT_UpdatedDate = DateTime.Today;
                    _IssueContext.Add(obj1);
                    var op = _IssueContext.SaveChanges();
                    if (op > 0)
                    {
                        VMS_Laptop_Application_DMO obj = new VMS_Laptop_Application_DMO();
                        obj = _IssueContext.VMS_Laptop_Application_DMO.Where(t => t.VLAPAPPL_Id == data.get_approval_status[i].VLAPAPPL_Id).FirstOrDefault();
                        obj.VLAPAPPL_ApplicationStatus = userdepthead + " Approved";
                        _IssueContext.Update(obj);
                        _IssueContext.SaveChanges();
                    }
                }

                data.msg = "Saved";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.msg = "Failed";
            }
            return data;
        }

        public VMS_Laptop_Application_DTO reject_status(VMS_Laptop_Application_DTO data)
        {
            try
            {
                long userhrmeid = _IssueContext.Staff_User_Login.Where(t => t.Id == data.User_Id).Select(t => t.Emp_Code).FirstOrDefault();

                string userdepthead = (from a in _IssueContext.HR_Master_DepartmentCodeDMO
                                       from b in _IssueContext.HR_Master_DepartmentCode_Head
                                       where (a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == userhrmeid)
                                       select a).Select(t => t.HRMDC_Code).FirstOrDefault();

                if (userdepthead == "ADMIN")
                {
                    data.VLAPAPPLAUT_SanctioningLevel = 3;
                }
                else if (userdepthead == "HW")
                {
                    data.VLAPAPPLAUT_SanctioningLevel = 2;
                }
                else
                {
                    userdepthead = "TL";
                    data.VLAPAPPLAUT_SanctioningLevel = 1;
                }

                for (int i = 0; i < data.get_approval_status.Length; i++)
                {
                    VMS_Laptop_Application_Authorisation_DMO obj1 = new VMS_Laptop_Application_Authorisation_DMO();
                    obj1.VLAPAPPL_Id = data.get_approval_status[i].VLAPAPPL_Id;
                    obj1.IVRMUL_Id = userhrmeid;
                    obj1.VLAPAPPLAUT_SanctioningLevel = data.VLAPAPPLAUT_SanctioningLevel;
                    obj1.VLAPAPPLAUT_Remarks = data.VLAPAPPLAUT_Remarks;
                    obj1.VLAPAPPLAUT_Date = DateTime.Today;
                    obj1.VLAPAPPLAUT_ActiveFlg = true;
                    obj1.VLAPAPPLAUT_CreatedBy = data.User_Id;
                    obj1.VLAPAPPLAUT_CreatedDate = DateTime.Today;
                    obj1.VLAPAPPLAUT_UpdatedBy = data.User_Id;
                    obj1.VLAPAPPLAUT_UpdatedDate = DateTime.Today;
                    _IssueContext.Add(obj1);
                    var op = _IssueContext.SaveChanges();
                    if (op > 0)
                    {
                        VMS_Laptop_Application_DMO obj = new VMS_Laptop_Application_DMO();
                        obj = _IssueContext.VMS_Laptop_Application_DMO.Where(t => t.VLAPAPPL_Id == data.get_approval_status[i].VLAPAPPL_Id).FirstOrDefault();
                        obj.VLAPAPPL_ApplicationStatus = userdepthead + " Rejected";
                        _IssueContext.Update(obj);
                        _IssueContext.SaveChanges();
                    }
                }

                data.msg = "Saved";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.msg = "Failed";
            }
            return data;
        }

        public VMS_Laptop_Application_DTO getrpt(VMS_Laptop_Application_DTO data)
        {
            try
            {
                //if (data.rdotype == "Approved")
                //{
                //    data.indentreport = (from a in _IssueContext.VMS_Laptop_Application_DMO
                //                         from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                //                         from c in _IssueContext.Master_VehicleDMO
                //                         from d in _IssueContext.Staff_User_Login
                //                         from e in _IssueContext.MasterEmployee
                //                         where (a.ISMDIT_Id == b.ISMDIT_Id && a.ISMDIT_Date >= data.fromdate && a.ISMDIT_Date <= data.todate && b.ISMDIT_PreparedByFlag.ToUpper() == "ADMIN" && b.ISMDIT_Status == "Approved" && a.TRMV_Id == c.TRMV_Id && b.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                //                         select new VMS_Laptop_Application_DTO
                //                         {
                //                             ISMDIT_Date = a.ISMDIT_Date,
                //                             TRMV_Id = a.TRMV_Id,
                //                             TRMV_VehicleNo = c.TRMV_VehicleNo,
                //                             TRMV_VehicleName = c.TRMV_VehicleName,
                //                             ISMDIT_BillNo = a.ISMDIT_BillNo,
                //                             ISMDIT_Qty = a.ISMDIT_Qty,
                //                             ISMDIT_Amount = a.ISMDIT_Amount,
                //                             ISMDIT_Remarks = a.ISMDIT_Remarks,
                //                             ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                //                             ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                //                             ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                //                             HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null? "": e.HRME_EmployeeLastName),
                //                             ISMDIT_Status = b.ISMDIT_Status
                //                         }).OrderBy(t => t.ISMDIT_Date).ToArray();
                //}
                //else if (data.rdotype == "Rejected")
                //{
                //    data.indentreport = (from a in _IssueContext.VMS_Laptop_Application_DMO
                //                         from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                //                         from c in _IssueContext.Master_VehicleDMO
                //                         from d in _IssueContext.Staff_User_Login
                //                         from e in _IssueContext.MasterEmployee
                //                         where (a.ISMDIT_Id == b.ISMDIT_Id && a.ISMDIT_Date >= data.fromdate && a.ISMDIT_Date <= data.todate && b.ISMDIT_PreparedByFlag.ToUpper() == "ADMIN" && b.ISMDIT_Status == "Rejected" && a.TRMV_Id == c.TRMV_Id && b.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                //                         select new VMS_Laptop_Application_DTO
                //                         {
                //                             ISMDIT_Date = a.ISMDIT_Date,
                //                             TRMV_Id = a.TRMV_Id,
                //                             TRMV_VehicleNo = c.TRMV_VehicleNo,
                //                             TRMV_VehicleName = c.TRMV_VehicleName,
                //                             ISMDIT_BillNo = a.ISMDIT_BillNo,
                //                             ISMDIT_Qty = a.ISMDIT_Qty,
                //                             ISMDIT_Amount = a.ISMDIT_Amount,
                //                             ISMDIT_Remarks = a.ISMDIT_Remarks,
                //                             ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                //                             ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                //                             ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                //                             HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null ? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? "" : e.HRME_EmployeeLastName),
                //                             ISMDIT_Status = b.ISMDIT_Status
                //                         }).OrderBy(t => t.ISMDIT_Date).ToArray();
                //}
                //else if (data.rdotype == "Applied")
                //{
                //    data.indentreport = (from a in _IssueContext.VMS_Laptop_Application_DMO
                //                         from b in _IssueContext.ISM_Driver_Indent_ApprovalDMO
                //                         from c in _IssueContext.Master_VehicleDMO
                //                         from d in _IssueContext.Staff_User_Login
                //                         from e in _IssueContext.MasterEmployee
                //                         where (a.ISMDIT_Id == b.ISMDIT_Id && a.ISMDIT_Date >= data.fromdate && a.ISMDIT_Date <= data.todate && b.ISMDIT_PreparedByFlag.ToUpper() == "DRIVER" && b.ISMDIT_Status == "Applied" && a.TRMV_Id == c.TRMV_Id && b.ISMDIT_PreparedByUserId == d.Id && d.Emp_Code == e.HRME_Id)
                //                         select new VMS_Laptop_Application_DTO
                //                         {
                //                             ISMDIT_Date = a.ISMDIT_Date,
                //                             TRMV_Id = a.TRMV_Id,
                //                             TRMV_VehicleNo = c.TRMV_VehicleNo,
                //                             TRMV_VehicleName = c.TRMV_VehicleName,
                //                             ISMDIT_BillNo = a.ISMDIT_BillNo,
                //                             ISMDIT_Qty = a.ISMDIT_Qty,
                //                             ISMDIT_Amount = a.ISMDIT_Amount,
                //                             ISMDIT_Remarks = a.ISMDIT_Remarks,
                //                             ISMDIT_OpeningKM = a.ISMDIT_OpeningKM,
                //                             ISMDIT_ClosingKM = a.ISMDIT_ClosingKM,
                //                             ISMDIT_BalanceDiesel = a.ISMDIT_BalanceDiesel,
                //                             HRME_EmployeeFirstName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null ? "" : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? "" : e.HRME_EmployeeLastName),
                //                             ISMDIT_Status = b.ISMDIT_Status
                //                         }).OrderBy(t=>t.ISMDIT_Date).ToArray();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public void SendEmailIndentApply(VMS_Laptop_Application_DTO obj, string subject, string body, long id)
        {
            //try
            //{
            //    var drivername = (from a in _IssueContext.Staff_User_Login
            //                           from c in _IssueContext.MasterEmployee
            //                           where (a.Id == obj.User_Id && a.Emp_Code == c.HRME_Id)
            //                           select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();
                
            //    string drivermailid = (from a in _IssueContext.Multiple_Email_DMO
            //                           where (a.HRME_Id == obj.HRME_Id && a.HRMEM_DeFaultFlag == "default")
            //                           select a.HRMEM_EmailId).FirstOrDefault();

            //    var vehicleno = _IssueContext.Master_VehicleDMO.Where(t => t.TRMV_Id == obj.TRMV_Id).FirstOrDefault();

            //    string mailid = "siddesh@vapstech.com";//"goutamkumar@vapstech.com"; 
            //    Dictionary<string, string> val = new Dictionary<string, string>();
            //    var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("IndentApply", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
            //    var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

            //    string Mailmsg = body;
            //    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
            //    alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

            //    if (alldetails.Count > 0)
            //    {
            //        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
            //        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
            //        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
            //        string mailcc = alldetails[0].IVRM_mailcc;
            //        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
            //        string Subject = subject;
            //        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
            //        {
            //            string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
            //        }

            //        string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

            //        var message = new SendGridMessage();
            //        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
            //        message.Subject = Subject;

            //        if (mailcc != null && mailcc != "")
            //        {
            //            string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

            //            if (mail_id.Length > 0)
            //            {
            //                for (int i = 0; i < mail_id.Length; i++)
            //                {
            //                    message.AddBcc(mail_id[i]);
            //                }
            //            }
            //        }
            //        if (drivermailid != null) { message.AddBcc(drivermailid); }
            //        message.AddTo(mailid);

            //        if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
            //        {
            //            message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
            //            message.HtmlContent = message.HtmlContent.Replace("[NAME]", drivername.HRME_EmployeeFirstName + " " + (drivername.HRME_EmployeeMiddleName == null ? "" : drivername.HRME_EmployeeMiddleName) + " " + (drivername.HRME_EmployeeLastName == null ? "" : drivername.HRME_EmployeeLastName));
            //            message.HtmlContent = message.HtmlContent.Replace("[VEHICLENO]", vehicleno.TRMV_VehicleNo);
            //            message.HtmlContent = message.HtmlContent.Replace("[FUELQUANTITY]", obj.ISMDIT_Qty.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[AMOUNT]", obj.ISMDIT_Amount.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[OPENINGKM]", obj.ISMDIT_OpeningKM.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[CLOSINGKM]", obj.ISMDIT_ClosingKM.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[BALANCEDIESEL]", obj.ISMDIT_BalanceDiesel.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[REMARK]", obj.ISMDIT_Remarks);
            //        }
            //        else
            //        {
            //            message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
            //        }

            //        if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
            //        {
            //            var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
            //            client.SendEmailAsync(message).Wait();
            //        }
            //        else
            //        {
            //            // return "Sendgrid key is not available";
            //        }

            //        using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
            //        {
            //            var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("IndentApply", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

            //            var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

            //            var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

            //            cmd.CommandText = "IVRM_Email_Outgoing_1";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
            //            {
            //                Value = mailid
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
            //            {
            //                Value = Mailmsg
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
            //            {
            //                Value = subject
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
            //            {
            //                Value = id
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
            //            {
            //                Value = "Staff"
            //            });

            //            if (cmd.Connection.State != ConnectionState.Open)
            //                cmd.Connection.Open();
            //            try
            //            {
            //                using (var dataReader = cmd.ExecuteReader())
            //                {
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                //return ex.Message;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        public void IndentApproveMail(VMS_Laptop_Application_DTO obj,long index, string subject, string body, long id,string status)
        {
            //try
            //{
            //    string drivermailid = (from a in _IssueContext.Multiple_Email_DMO
            //                           from c in _IssueContext.Staff_User_Login
            //                           where (a.HRME_Id == c.Emp_Code && a.HRMEM_DeFaultFlag == "default" && c.Id == obj.ISMDIT_PreparedByUserId)
            //                           select a.HRMEM_EmailId).FirstOrDefault();

            //    string mailid = "accounts@vapstech.com"; //"goutamkumar@vapstech.com";
            //    Dictionary<string, string> val = new Dictionary<string, string>();
            //    var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("IndentApprove", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
            //    var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

            //    string Mailmsg = body;
            //    List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
            //    alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

            //    if (alldetails.Count > 0)
            //    {
            //        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
            //        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
            //        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
            //        string mailcc = alldetails[0].IVRM_mailcc;
            //        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
            //        string Subject = subject;
            //        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
            //        {
            //            string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
            //        }

            //        string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

            //        var message = new SendGridMessage();
            //        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
            //        message.Subject = Subject;

            //        if (mailcc != null && mailcc != "")
            //        {
            //            string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

            //            if (mail_id.Length > 0)
            //            {
            //                for (int i = 0; i < mail_id.Length; i++)
            //                {
            //                    message.AddBcc(mail_id[i]);
            //                }
            //            }
            //        }
            //        if (drivermailid != null) { message.AddBcc(drivermailid); }
            //        message.AddTo(mailid);

            //        if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
            //        {
            //            message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);                        
            //            message.HtmlContent = message.HtmlContent.Replace("[STATUS]", status);
            //            message.HtmlContent = message.HtmlContent.Replace("[REMARK]", obj.ISMDIT_Remark);
            //            message.HtmlContent = message.HtmlContent.Replace("[NAME]", obj.get_indent_status[index].HRME_EmployeeFirstName);
            //            message.HtmlContent = message.HtmlContent.Replace("[VEHICLENO]", obj.get_indent_status[index].TRMV_VehicleNo);
            //            message.HtmlContent = message.HtmlContent.Replace("[FUELQUANTITY]", obj.get_indent_status[index].ISMDIT_Qty.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[AMOUNT]", obj.get_indent_status[index].ISMDIT_Amount.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[OPENINGKM]", obj.get_indent_status[index].ISMDIT_OpeningKM.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[CLOSINGKM]", obj.get_indent_status[index].ISMDIT_ClosingKM.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[BALANCEDIESEL]", obj.get_indent_status[index].ISMDIT_BalanceDiesel.ToString());
            //            message.HtmlContent = message.HtmlContent.Replace("[ROLE]", "ADMIN");
            //        }
            //        else
            //        {
            //            message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
            //        }

            //        if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
            //        {
            //            var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
            //            client.SendEmailAsync(message).Wait();
            //        }
            //        else
            //        {
            //            // return "Sendgrid key is not available";
            //        }

            //        using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
            //        {
            //            var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("IndentApprove", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

            //            var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

            //            var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

            //            cmd.CommandText = "IVRM_Email_Outgoing_1";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
            //            {
            //                Value = mailid
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
            //            {
            //                Value = Mailmsg
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
            //            {
            //                Value = subject
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
            //            {
            //                Value = id
            //            });
            //            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
            //            {
            //                Value = "Staff"
            //            });

            //            if (cmd.Connection.State != ConnectionState.Open)
            //                cmd.Connection.Open();
            //            try
            //            {
            //                using (var dataReader = cmd.ExecuteReader())
            //                {
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                //return ex.Message;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }
    }
}
