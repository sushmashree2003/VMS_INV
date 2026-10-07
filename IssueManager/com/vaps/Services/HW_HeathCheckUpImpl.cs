using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.HWHealthCheckUp;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
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
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class HW_HeathCheckUpImpl : Interfaces.HW_HeathCheckUpInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _Contextdb;
        public HW_HeathCheckUpImpl(IssueManagerContext _cont, DomainModelMsSqlServerContext _Condb)
        {
            _context = _cont;
            _Contextdb = _Condb;
        }

        public HW_HeathCheckUpDTO OnLoadHWHealthCheckUp(HW_HeathCheckUpDTO data)
        {
            try
            {
                if (data.OnLoadOrOnChange == "OnLoad")
                {
                    var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    List<long> miids = new List<long>();

                    foreach (var c in getuserinstitution)
                    {
                        miids.Add(c.MI_Id);
                    }

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();
                }

                data.getclientlist = _context.clientTable.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();

                data.getemployeedetails = (from a in _context.MasterEmployee
                                           from b in _context.HR_Master_Department
                                           where (a.HRMD_Id == b.HRMD_Id)
                                           select new HW_HeathCheckUpDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) +
                                               (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                               (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                               (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode))

                                           }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();

                data.getloaddata = (from a in _context.HW_HealthCheckUpDMO
                                    from b in _context.clientTable
                                    from c in _context.MasterEmployee
                                    where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id)
                                    select new HW_HeathCheckUpDTO
                                    {
                                        HWHCHUP_Id = a.HWHCHUP_Id,
                                        ISMMCLT_Id = a.ISMMCLT_Id,
                                        ISMMCLT_ClientName = b.ISMMCLT_ClientName,
                                        VisitedDate = a.HWHCHUP_Date,
                                        Remarks = a.HWHCHUP_Remarks,
                                        ActiveFlag = a.HWHCHUP_ActiveFlag,
                                        HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" :
                                        c.HRME_EmployeeFirstName) +
                                        (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" ? "" : " " + c.HRME_EmployeeMiddleName) +
                                        (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" ? "" : " " + c.HRME_EmployeeLastName) +
                                        (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : " + c.HRME_EmployeeCode)),
                                        status = a.HWHCHUP_Stauts,
                                        count = _context.HW_HealthCheckUp_FilesDMO.Where(c => a.HWHCHUP_Id == c.HWHCHUP_Id).Count()

                                    }).Distinct().ToArray();


                //data.detailsarray = (from a in _context.ISM_ProposalDMO
                //                     from b in _context.clientTable
                //                     from c in _context.MasterEmployee
                //                     from d in _context.AcademicYear
                //                     where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id && a.ISMCLTPRMP_AcademicYear == d.ASMAY_Id)
                //                     select new HW_HeathCheckUpDTO
                //                     {
                //                         ISMPSSUP_Id = a.ISMPSSUP_Id,
                //                         ISMMCLT_Id = a.ISMMCLT_Id,
                //                         ISMMCLT_ClientName = b.ISMMCLT_ClientName,
                //                         VisitedDate = a.ISMPSSUP_Date,
                //                         Remarks = a.ISMPSSUP_TemplateName,
                //                         ActiveFlag = a.ISMPSSUP_ActiveFlag,
                //                         HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" :
                //                        c.HRME_EmployeeFirstName) +
                //                        (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" ? "" : " " + c.HRME_EmployeeMiddleName) +
                //                        (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" ? "" : " " + c.HRME_EmployeeLastName) +
                //                        (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : " + c.HRME_EmployeeCode)),
                //                         status = a.ISMPSSUP_Stauts,
                //                         count = _context.ISM_Proposal_FilesDMO.Where(c => a.ISMPSSUP_Id == c.ISMPSSUP_Id).Count()

                //                     }).Distinct().ToArray();

                data.viewcommens = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Propsal_SignUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    //cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //{
                    //    Value = data.ISMPSSUP_Id
                    //});

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
                        data.detailsarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //added by praveenkumar
            

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_MOU";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        data.Mompendinglist = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Closure_Letter_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        data.Welcomeclosearray = retObject.ToArray();
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
        public HW_HeathCheckUpDTO SaveHWHealthChekUp(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                string Flag = "";
                long HWHCHUP_Id = 0;
                HW_HealthCheckUpDMO hW_HealthCheckUpDMO = new HW_HealthCheckUpDMO();
                if (data.HWHCHUP_Id > 0)
                {
                    HWHCHUP_Id = data.HWHCHUP_Id;
                    Flag = "Update";
                    var Result = _context.HW_HealthCheckUpDMO.Single(a => a.MI_Id == data.MI_Id && a.HWHCHUP_Id == data.HWHCHUP_Id);
                    Result.HWHCHUP_Date = data.VisitedDate;
                    Result.HRME_Id = data.HRME_Id;
                    Result.HWHCHUP_Remarks = data.Remarks;
                    Result.HWHCHUP_UpdatedBy = data.UserId;
                    Result.HWHCHUP_UpdatedDate = indiantime0;
                    _context.Update(Result);
                }
                else
                {
                    Flag = "Save";
                    hW_HealthCheckUpDMO.MI_Id = data.MI_Id;
                    hW_HealthCheckUpDMO.ISMMCLT_Id = data.ISMMCLT_Id;
                    hW_HealthCheckUpDMO.HRME_Id = data.HRME_Id;
                    hW_HealthCheckUpDMO.HWHCHUP_Date = data.VisitedDate;
                    hW_HealthCheckUpDMO.HWHCHUP_Remarks = data.Remarks;
                    hW_HealthCheckUpDMO.HWHCHUP_ActiveFlag = true;
                    hW_HealthCheckUpDMO.HWHCHUP_CreatedBy = data.UserId;
                    hW_HealthCheckUpDMO.HWHCHUP_CreatedDate = indiantime0;
                    hW_HealthCheckUpDMO.HWHCHUP_UpdatedBy = data.UserId;
                    hW_HealthCheckUpDMO.HWHCHUP_UpdatedDate = indiantime0;
                    hW_HealthCheckUpDMO.HWHCHUP_Stauts = "Applied";
                    _context.Add(hW_HealthCheckUpDMO);
                    HWHCHUP_Id = hW_HealthCheckUpDMO.HWHCHUP_Id;
                }

                if (data.HWHCHUP_Id > 0)
                {
                    Array GetFiles = _context.HW_HealthCheckUp_FilesDMO.Where(a => a.HWHCHUP_Id == data.HWHCHUP_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.UploadedFiles != null && data.UploadedFiles.Length > 0)
                {
                    foreach (var d in data.UploadedFiles)
                    {
                        if (d.filepath != null && d.filepath != "")
                        {
                            HW_HealthCheckUp_FilesDMO hW_HealthCheckUp_FilesDMO = new HW_HealthCheckUp_FilesDMO();
                            hW_HealthCheckUp_FilesDMO.HWHCHUP_Id = HWHCHUP_Id;
                            hW_HealthCheckUp_FilesDMO.HWHCHUPF_FileName = d.filename;
                            hW_HealthCheckUp_FilesDMO.HWHCHUPF_FilePath = d.filepath;
                            hW_HealthCheckUp_FilesDMO.HWHCHUPF_CreatedBy = data.UserId;
                            hW_HealthCheckUp_FilesDMO.HWHCHUPF_CreatedDate = indiantime0;
                            hW_HealthCheckUp_FilesDMO.HWHCHUPF_UpdatedBy = data.UserId;
                            hW_HealthCheckUp_FilesDMO.HWHCHUPF_UpdatedDate = indiantime0;
                            _context.Add(hW_HealthCheckUp_FilesDMO);
                        }
                    }
                }

                var i = _context.SaveChanges();

                if (i > 0)
                {
                    data.HWHCHUP_Id = hW_HealthCheckUpDMO.HWHCHUP_Id;
                    data.returnval = true;

                    if (Flag == "Save")
                    {
                        var getfirstlevelapproval = (from a in _context.HR_PROCESSDMO
                                                     from b in _context.HR_Process_Auth_OrderNoDMO
                                                     from c in _context.Staff_User_Login
                                                     from d in _context.MasterEmployee
                                                     from e in _context.Multiple_Email_DMO
                                                     where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == c.Id && d.HRME_Id == c.Emp_Code
                                                     && d.HRME_Id == e.HRME_Id && b.HRPAON_SanctionLevelNo == 1 && e.HRMEM_DeFaultFlag == "default"
                                                     && a.HRPA_TypeFlag == "HW_HealthCheckUp")
                                                     select e).Distinct().ToList();

                        if (getfirstlevelapproval.Count > 0)
                        {
                            string emp_email = getfirstlevelapproval.FirstOrDefault().HRMEM_EmailId;

                            Email email = new Email(_Contextdb);
                            email.HW_HealthCheckup_Email_Notification_Send(data.MI_Id, emp_email, "HW_HealthApprovalRequest", data.HWHCHUP_Id);
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
        //added by praveenkuamr proposal signoff
        public HW_HeathCheckUpDTO ProposalSignOff(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                string Flag = "";
                long ISMPSSUP_Id = 0;
                ISM_Proposal_DMO ISM_ProposalDMO = new ISM_Proposal_DMO();
                if (data.ISMPSSUP_Id > 0)
                {
                    ISMPSSUP_Id = data.ISMPSSUP_Id;
                    Flag = "Update";
                    var Result = _context.ISM_ProposalDMO.Single(a => a.MI_Id == data.MI_Id && a.ISMPSSUP_Id == data.ISMPSSUP_Id);
                    Result.ISMPSSUP_Date = data.VisitedDate;
                    Result.ISMPSSUP_TemplateName = data.ISMPSSUP_TemplateName;
                    Result.ISMPSSUP_TemplateID = data.ISMPSSUP_TemplateID;
                    Result.ISMCLTPRMP_AcademicYear = data.ISMCLTPRMP_AcademicYear;
                    // Result.ISMPSSUP_EndUSer = data.HRME_ID.ToString();
                    Result.ISMPSSUP_UpdatedBy = data.UserId;
                    Result.ISMPSSUP_UpdatedDate = indiantime0;
                    _context.Update(Result);
                }
                else
                {
                    foreach (var d in data.EndUserName)
                    {
                        Flag = "Save";
                        ISM_ProposalDMO.MI_Id = data.MI_Id;
                        ISM_ProposalDMO.ISMMCLT_Id = data.ISMMCLT_Id;
                        ISM_ProposalDMO.HRME_Id = data.HRME_Id;
                        ISM_ProposalDMO.ISMPSSUP_Date = data.VisitedDate;
                        ISM_ProposalDMO.ISMPSSUP_TemplateName = data.ISMPSSUP_TemplateName;
                        ISM_ProposalDMO.ISMPSSUP_TemplateID = data.ISMPSSUP_TemplateID;
                        ISM_ProposalDMO.ISMCLTPRMP_AcademicYear = data.ISMCLTPRMP_AcademicYear;
                        ISM_ProposalDMO.ISMPSSUP_EndUSer = d.HRME_ID.ToString();
                        ISM_ProposalDMO.ISMPSSUP_ActiveFlag = true;
                        ISM_ProposalDMO.ISMPSSUP_CreatedBy = data.UserId;
                        ISM_ProposalDMO.ISMPSSUP_CreatedDate = indiantime0;
                        ISM_ProposalDMO.ISMPSSUP_UpdatedBy = data.UserId;
                        ISM_ProposalDMO.ISMPSSUP_UpdatedDate = indiantime0;
                        ISM_ProposalDMO.ISMPSSUP_Stauts = "Applied";


                        _context.Add(ISM_ProposalDMO);
                    }


                    _context.SaveChanges();


                    ISMPSSUP_Id = ISM_ProposalDMO.ISMPSSUP_Id;
                }


                if (data.ISMPSSUP_Id > 0)
                {
                    Array GetFiles = _context.ISM_Proposal_FilesDMO.Where(a => a.ISMPSSUP_Id == data.ISMPSSUP_Id).ToArray();

                    foreach (var c in GetFiles)
                    {
                        _context.Remove(c);
                    }
                }

                if (data.UploadedFiles != null && data.UploadedFiles.Length > 0)
                {
                    foreach (var d in data.UploadedFiles)
                    {
                        if (d.filepath != null && d.filepath != "")
                        {
                            ISM_ProposalDMO_Files ISM_Proposal_FilesDMO = new ISM_ProposalDMO_Files();
                            ISM_Proposal_FilesDMO.ISMPSSUP_Id = ISMPSSUP_Id;
                            ISM_Proposal_FilesDMO.ISMPSSUPF_FileName = d.filename;
                            ISM_Proposal_FilesDMO.ISMPSSUPF_FilePath = d.filepath;
                            ISM_Proposal_FilesDMO.ISMPSSUPF_CreatedBy = data.UserId;
                            ISM_Proposal_FilesDMO.ISMPSSUPF_CreatedDate = indiantime0;
                            ISM_Proposal_FilesDMO.ISMPSSUPF_UpdatedBy = data.UserId;
                            ISM_Proposal_FilesDMO.ISMPSSUPF_UpdatedDate = indiantime0;
                            _context.Add(ISM_Proposal_FilesDMO);
                        }
                    }
                }

                var i = _context.SaveChanges();

                if (i > 0)
                {
                    data.ISMPSSUP_Id = ISM_ProposalDMO.ISMPSSUP_Id;
                    data.returnval = true;

                    if (Flag == "Save")
                    {
                        var getfirstlevelapproval = (from a in _context.HR_PROCESSDMO
                                                     from b in _context.HR_Process_Auth_OrderNoDMO
                                                     from c in _context.Staff_User_Login
                                                     from d in _context.MasterEmployee
                                                     from e in _context.Multiple_Email_DMO
                                                     where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == c.Id && d.HRME_Id == c.Emp_Code
                                                     && d.HRME_Id == e.HRME_Id && b.HRPAON_SanctionLevelNo == 1 && e.HRMEM_DeFaultFlag == "default"
                                                     && a.HRPA_TypeFlag == "HW_HealthCheckUp")
                                                     select e).Distinct().ToList();

                        if (getfirstlevelapproval.Count > 0)
                        {
                            string emp_email = getfirstlevelapproval.FirstOrDefault().HRMEM_EmailId;

                            Email email = new Email(_Contextdb);
                            email.HW_HealthCheckup_Email_Notification_Send(data.MI_Id, emp_email, "HW_HealthApprovalRequest", data.HWHCHUP_Id);
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


          public HW_HeathCheckUpDTO welclosesavedata(HW_HeathCheckUpDTO data)
        {
            try
            {
                var result = _context.Database.ExecuteSqlCommand("ISM_PostSales_Closure_Letter_Insert @p0,@p1,@p2,@p3",
                   data.ISMMCLT_Id,
                   data.ASMAY_ID,
                   data.ISMPCL_TemplateName,
                   data.UserId
                   );


                if (result > 0)
                {
                    data.msg = "saved";
                }
                else
                {
                    data.msg = "Failed";
                }


            }

            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public HW_HeathCheckUpDTO EditHealthCheckUp(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.geteditdetails = (from a in _context.HW_HealthCheckUpDMO
                                       from b in _context.MasterEmployee
                                       where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.HWHCHUP_Id == data.HWHCHUP_Id)
                                       select new HW_HeathCheckUpDTO
                                       {
                                           HWHCHUP_Id = a.HWHCHUP_Id,
                                           ISMMCLT_Id = a.ISMMCLT_Id,
                                           VisitedDate = a.HWHCHUP_Date,
                                           Remarks = a.HWHCHUP_Remarks,
                                           status = a.HWHCHUP_Stauts
                                       }).Distinct().ToArray();

                data.geteditfilesdetails = (from a in _context.HW_HealthCheckUp_FilesDMO
                                            where (a.HWHCHUP_Id == data.HWHCHUP_Id)
                                            select new HW_HeathCheckUpDTO
                                            {
                                                HWHCHUP_Id = a.HWHCHUP_Id,
                                                filename = a.HWHCHUPF_FileName,
                                                filepath = a.HWHCHUPF_FilePath
                                            }).Distinct().ToArray();

                var details = _context.HW_HealthCheckUpDMO.Where(a => a.MI_Id == data.MI_Id && a.HWHCHUP_Id == data.HWHCHUP_Id).ToList();

                data.geteditclientdetails = _context.clientTable.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_Id == details.FirstOrDefault().ISMMCLT_Id).ToArray();

                data.geteditemployeedetails = (from a in _context.MasterEmployee
                                               from b in _context.HR_Master_Department
                                               where (a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true 
                                               && a.HRME_Id == details.FirstOrDefault().HRME_Id)
                                               select new HW_HeathCheckUpDTO
                                               {
                                                   HRME_Id = a.HRME_Id,
                                                   HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName) +
                                                   (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                                   (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName) +
                                                   (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode))

                                               }).Distinct().OrderBy(a => a.HRME_EmployeeFirstName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HW_HeathCheckUpDTO ViewHealthCheckUpFiles(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.getviewfilesdetails = (from a in _context.HW_HealthCheckUp_FilesDMO
                                            where (a.HWHCHUP_Id == data.HWHCHUP_Id)
                                            select new HW_HeathCheckUpDTO
                                            {
                                                HWHCHUP_Id = a.HWHCHUP_Id,
                                                HWHCHUPF_Id = a.HWHCHUPF_Id,
                                                filename = a.HWHCHUPF_FileName,
                                                filepath = a.HWHCHUPF_FilePath
                                            }).Distinct().ToArray();

                data.getviewdocumetsignoff = (from a in _context.ISM_Proposal_FilesDMO
                                             where (a.ISMPSSUP_Id == data.ISMPSSUP_Id)
                                             select new HW_HeathCheckUpDTO
                                             {
                                                 ISMPSSUP_Id = a.ISMPSSUP_Id,
                                                 ISMPSSUPF_Id = a.ISMPSSUPF_Id,
                                                 filename = a.ISMPSSUPF_FileName,
                                                 filepath = a.ISMPSSUPF_FilePath
                                             }).Distinct().ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HW_HeathCheckUpDTO ActiveDeactiveHealthCheckUp(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;
                var Result = _context.HW_HealthCheckUpDMO.Single(a => a.MI_Id == data.MI_Id && a.HWHCHUP_Id == data.HWHCHUP_Id);
                Result.HWHCHUP_ActiveFlag = Result.HWHCHUP_ActiveFlag == true ? false : true;
                Result.HWHCHUP_UpdatedBy = data.UserId;
                Result.HWHCHUP_UpdatedDate = indiantime0;
                _context.Update(Result);

                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HW_HeathCheckUpDTO DeleteHWHealthCheckUpFiles(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;
                var Result = _context.HW_HealthCheckUp_FilesDMO.Single(a => a.HWHCHUP_Id == data.HWHCHUP_Id && a.HWHCHUPF_Id == data.HWHCHUPF_Id);
                _context.Remove(Result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }

                data.getviewfilesdetails = (from a in _context.HW_HealthCheckUp_FilesDMO
                                            where (a.HWHCHUP_Id == data.HWHCHUP_Id)
                                            select new HW_HeathCheckUpDTO
                                            {
                                                HWHCHUP_Id = a.HWHCHUP_Id,
                                                HWHCHUPF_Id = a.HWHCHUPF_Id,
                                                filename = a.HWHCHUPF_FileName,
                                                filepath = a.HWHCHUPF_FilePath
                                            }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Approval Process
        public HW_HeathCheckUpDTO LoadHWHealthCheckUpdataApproval(HW_HeathCheckUpDTO data)
        {
            try
            {
                if (data.OnLoadOrOnChange == "OnLoad")
                {
                    var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    List<long> miids = new List<long>();

                    foreach (var c in getuserinstitution)
                    {
                        miids.Add(c.MI_Id);
                    }

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();
                }

                var checkapproval = (from a in _context.HR_PROCESSDMO
                                     from b in _context.HR_Process_Auth_OrderNoDMO
                                     where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == data.UserId && a.HRPA_TypeFlag.Equals("HW_HealthCheckUp"))
                                     select b).Distinct().ToArray();

                if (checkapproval.Length > 0)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HW_HealthCheckup_Approval_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@USER_Id", SqlDbType.VarChar)
                        {
                            Value = data.UserId
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
                            data.getapprovaldetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HW_HealthCheckup_UserWise_Approval_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar)
                        {
                            Value = data.UserId
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
                            data.getloaddata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    data.returnmsg = "NotMapped";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HW_HeathCheckUpDTO SaveHWHealthChekUpApproval(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                if (data.ApprovalDetails_Temp != null && data.ApprovalDetails_Temp.Length > 0)
                {
                    foreach (var c in data.ApprovalDetails_Temp)
                    {
                        HW_HealthCheckUp_ApprovalDMO hW_HealthCheckUp_ApprovalDMO = new HW_HealthCheckUp_ApprovalDMO();
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUP_Id = c.HWHCHUP_Id;
                        hW_HealthCheckUp_ApprovalDMO.IVRMUL_Id = data.UserId;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_Remarks = c.apprvlremarks;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_Status = c.approvalstatus;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_LevelNo = c.Approved_Level;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_Date = indiantime0.Date;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_CreatedBy = data.UserId;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_CreatedDate = indiantime0;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_UpdatedBy = data.UserId;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_UpdatedDate = indiantime0;
                        hW_HealthCheckUp_ApprovalDMO.HWHCHUPA_FinalFlag = c.FinalFlag;
                        _context.Add(hW_HealthCheckUp_ApprovalDMO);

                        var Result = _context.HW_HealthCheckUpDMO.Single(a => a.HWHCHUP_Id == c.HWHCHUP_Id);
                        Result.HWHCHUP_UpdatedBy = data.UserId;
                        Result.HWHCHUP_UpdatedDate = indiantime0;
                        if (c.FinalFlag == true)
                        {
                            Result.HWHCHUP_Stauts = c.approvalstatus;
                        }
                        else
                        {
                            Result.HWHCHUP_Stauts = c.approvalstatus == "Approved" ? "Partial Approved" : c.approvalstatus;
                        }
                        _context.Update(Result);
                    }

                    var i = _context.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                        try
                        {
                            var checkapprovallevel = (from a in _context.HR_PROCESSDMO
                                                      from b in _context.HR_Process_Auth_OrderNoDMO
                                                      where (a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "HW_HealthCheckUp" && b.IVRMUL_Id == data.UserId)
                                                      select b).Distinct().ToList();

                            var levelno = checkapprovallevel.FirstOrDefault().HRPAON_SanctionLevelNo;                           
                            foreach (var d in data.ApprovalDetails_Temp)
                            {
                                if (d.FinalFlag)
                                {
                                    string TemplateName = d.approvalstatus=="Approved" ? "HW_HealthCheckUpApproved": "HW_HealthCheckUpRejected";

                                    var getemaild = (from a in _context.HW_HealthCheckUpDMO
                                                     from b in _context.MasterEmployee
                                                     from c in _context.Multiple_Email_DMO
                                                     where (a.HRME_Id == b.HRME_Id && b.HRME_Id == c.HRME_Id && c.HRMEM_DeFaultFlag == "default"
                                                     && a.HWHCHUP_Id == d.HWHCHUP_Id)
                                                     select c).Distinct().ToList();

                                    string emp_email = getemaild.FirstOrDefault().HRMEM_EmailId;
                                    try
                                    {
                                        Email email = new Email(_Contextdb);
                                        email.HW_HealthCheckup_Email_Notification_Send(data.MI_Id, emp_email, TemplateName, d.HWHCHUP_Id);
                                    }
                                    catch(Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }                                  
                                }
                                else
                                {
                                    if (d.approvalstatus == "Approved")
                                    {
                                        var getfirstlevelapproval = (from a in _context.HR_PROCESSDMO
                                                                     from b in _context.HR_Process_Auth_OrderNoDMO
                                                                     from c in _context.Staff_User_Login
                                                                     from d1 in _context.MasterEmployee
                                                                     from e in _context.Multiple_Email_DMO
                                                                     where (a.HRPA_Id == b.HRPA_Id && b.IVRMUL_Id == c.Id && d1.HRME_Id == c.Emp_Code
                                                                     && d1.HRME_Id == e.HRME_Id && b.HRPAON_SanctionLevelNo == (levelno+1) 
                                                                     && e.HRMEM_DeFaultFlag == "default" && a.HRPA_TypeFlag == "HW_HealthCheckUp")
                                                                     select e).Distinct().ToList();

                                        if (getfirstlevelapproval.Count > 0)
                                        {
                                            string emp_email = getfirstlevelapproval.FirstOrDefault().HRMEM_EmailId;
                                            try
                                            {
                                                Email email = new Email(_Contextdb);
                                                email.HW_HealthCheckup_Email_Notification_Send(data.MI_Id, emp_email, "HW_HealthApprovalRequest", d.HWHCHUP_Id);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }                                            
                                        }
                                    }
                                    else
                                    {
                                        string TemplateName = d.approvalstatus == "Approved" ? "HW_HealthCheckUpApproved" : "HW_HealthCheckUpRejected";

                                        var getemaild = (from a in _context.HW_HealthCheckUpDMO
                                                         from b in _context.MasterEmployee
                                                         from c in _context.Multiple_Email_DMO
                                                         where (a.HRME_Id == b.HRME_Id && b.HRME_Id == c.HRME_Id && c.HRMEM_DeFaultFlag == "default"
                                                         && a.HWHCHUP_Id == d.HWHCHUP_Id)
                                                         select c).Distinct().ToList();

                                        string emp_email = getemaild.FirstOrDefault().HRMEM_EmailId;
                                        try
                                        {
                                            Email email = new Email(_Contextdb);
                                            email.HW_HealthCheckup_Email_Notification_Send(data.MI_Id, emp_email, TemplateName, d.HWHCHUP_Id);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
        public HW_HeathCheckUpDTO ViewHealthCheckUpComments(HW_HeathCheckUpDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                data.returnval = false;

                var viewcommens = (from a in _context.HW_HealthCheckUpDMO
                                   from b in _context.HW_HealthCheckUp_ApprovalDMO
                                   from c in _context.ApplicationUser
                                   where (a.HWHCHUP_Id == b.HWHCHUP_Id && b.IVRMUL_Id == c.Id && a.MI_Id == data.MI_Id && b.HWHCHUP_Id == data.HWHCHUP_Id)
                                   select new HW_HeathCheckUpDTO
                                   {
                                       Remarks = b.HWHCHUPA_Remarks,
                                       status = b.HWHCHUPA_Status,
                                       UserName = c.UserName,
                                       VisitedDate = b.HWHCHUPA_Date,
                                       LevelNo = b.HWHCHUPA_LevelNo
                                   }).Distinct().ToArray();

                data.viewcommens = viewcommens;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //health checkup report
        public HW_HeathCheckUpDTO HealthCheckUpInstitution(HW_HeathCheckUpDTO data)
        {
            try
            {
                if (data.OnLoadOrOnChange == "OnLoad")
                {
                    var getuserinstitution = _context.IVRM_User_Login_InstitutionwiseDMO.Where(a => a.Id == data.UserId).ToList();

                    List<long> miids = new List<long>();

                    foreach (var c in getuserinstitution)
                    {
                        miids.Add(c.MI_Id);
                    }

                    data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<HW_HeathCheckUpDTO> getdetails(HW_HeathCheckUpDTO data)
        {
            try
            {
                data.MI_Logo = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Select(R => R.MI_Logo).FirstOrDefault();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HW_HealthCheckupReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(data.FromDate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(data.ToDate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)
                    {
                        Value = data.HWHCHUP_Stauts
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        if (data.HWHCHUP_Stauts == "approved")
                        {
                            data.get_HWreport = retObject1.ToArray();
                        }
                        else
                        {
                            data.get_HWreportOne = retObject1.ToArray();
                        }
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

        public HW_HeathCheckUpDTO esculationmail(HW_HeathCheckUpDTO dto)
        {
            try
            {
                try
                {

                    string clientname = "";
                    string visteddate = "";
                    string empname = "";

                    List<HW_HeathCheckUpDTO> dtonewtemp = new List<HW_HeathCheckUpDTO>();
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "HW_HealthCheckup_Feedback_Details_Esculation";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@HWHCHUP_Id", SqlDbType.BigInt) { Value = dto.HWHCHUP_Id });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dtoReader = cmd.ExecuteReader())
                            {
                                while (dtoReader.Read())
                                {
                                    var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                    {
                                        dtonewtemp.Add(new HW_HeathCheckUpDTO
                                        {
                                            UserName = Convert.ToString(dtoReader["UserName"]),
                                            VisitedDate = Convert.ToDateTime(dtoReader["VisitedDate"]),
                                            HRME_EmployeeFirstName = Convert.ToString(dtoReader["HRME_EmployeeFirstName"]),

                                            //ISMSLEDM_Remarks = Convert.ToString(dataReader["bcc"])
                                        });
                                    }

                                }
                            }
                            if (dtonewtemp.Count > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    clientname = dtonewtemp[j].UserName;
                                    visteddate = dtonewtemp[j].VisitedDate.Value.ToString("dd/MM/yyyy");
                                    empname = dtonewtemp[j].HRME_EmployeeFirstName;
                                }
                            }

                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    esculationmail("HealthcheckupEsc", dto.MI_Id, clientname, visteddate, empname, dto.escpriority, dto.escreason);


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }





            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public string esculationmail(string Template, long mi_id, string clientname, string visteddate, string empname, string periodicity, string escreason)
        {
            try
            {

                string email = "manohara.nm@vapstech.com";

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == mi_id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = Mailmsg.Replace("[ClientName]", clientname);
                // Mailmsg = Mailmsg.Replace("[EMAIL]", email);
                Mailmsg = Mailmsg.Replace("[VisitedDate]", visteddate);
                Mailmsg = Mailmsg.Replace("[EmpName]", empname);
                Mailmsg = Mailmsg.Replace("[Periodicity]", periodicity);
                Mailmsg = Mailmsg.Replace("[Escreason]", escreason);

                Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(mi_id)).ToList();//

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(mi_id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
                    {
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
                        message.AddTo(email)
;


                        if (Attechement.Equals("1"))
                        {
                            var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }




                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        var message = new SendGridMessage();
                        // message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);

                        message.From = new EmailAddress(SendingEmail);
                        message.Subject = Subject;
                        message.AddTo(email);


                        // StringBuilder sb = new StringBuilder(data.Template);
                        //StringReader sr = new StringReader(sb.ToString());
                        //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        //HtmlWorker htmlparser = new HtmlWorker(pdfDoc);

                        //using (MemoryStream memoryStream = new MemoryStream())
                        //{
                        //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        //    pdfDoc.Open();
                        //    htmlparser.Parse(sr);
                        //    pdfDoc.Close();
                        //    byte[] bytess = memoryStream.ToArray();
                        //    var file = Convert.ToBase64String(bytess);
                        //    byte[] fileBytes = Convert.FromBase64String(file);
                        //    string emp;
                        //    emp = Convert.ToString(sr);
                        //    string c = "";
                        //    string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");
                        //    message.AddAttachment(v, file);
                        //    message.HtmlContent = Mailmsg;
                        //    var client1 = new SendGridClient(sengridkey);
                        //    memoryStream.Close();
                        //    client1.SendEmailAsync(message).Wait();
                        //    var datavalues = file.Substring(file.IndexOf(',') + 1);
                        //    var byteArray = Convert.FromBase64String(datavalues);
                        //    using (var stream = new MemoryStream(byteArray))
                        //    {
                        //        IFormFile formFile = new FormFile(stream, 0, byteArray.Length, "file", "Proposal.pdf")
                        //        {
                        //            Headers = new HeaderDictionary(),
                        //            ContentType = "application/pdf"
                        //        };
                        //        string accountname = "";
                        //        string accesskey = "";
                        //        var azure = _db.IVRM_Storage_path_Details.ToList();
                        //        if (azure.Count() > 0)
                        //        {
                        //            accountname = azure.FirstOrDefault().IVRM_SD_Access_Name;
                        //            accesskey = azure.FirstOrDefault().IVRM_SD_Access_Key;
                        //        }
                        //        var names = new List<string>();
                        //        var contentTypes = new List<string>();

                        //    }
                        //}





                        //******************EMAIL CC DETAILS ***************

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        ////****************** EMAIL BCC DETAILS ***************//

                        //if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        //{
                        //    string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                        //    foreach (var d in ccmaildetails)
                        //    {
                        //        message.AddCc(d);
                        //    }
                        //}

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();
                            //whatsapp whtsappnew = new whatsapp(_context);
                            //StringBuilder sb = new StringBuilder(data.Template);
                            //StringReader sr = new StringReader(sb.ToString());
                            //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            //HtmlWorker htmlparser = new HtmlWorker(pdfDoc);

                            //string fileid = "";
                            //using (MemoryStream memoryStream = new MemoryStream())
                            //{
                            //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                            //    pdfDoc.Open();
                            //    htmlparser.Parse(sr);
                            //    pdfDoc.Close();
                            //    byte[] bytess = memoryStream.ToArray();
                            //    var file = Convert.ToBase64String(bytess);
                            //    byte[] fileBytes = Convert.FromBase64String(file);
                            //    string emp;
                            //    emp = Convert.ToString(sr);
                            //    string c = "";
                            //    string v = emp.Replace("System.IO.StringReader", "Quotation.Pdf");

                            //    var datavalues = file.Substring(file.IndexOf(',') + 1);
                            //    var byteArray = Convert.FromBase64String(datavalues);
                            //    using (var stream = new MemoryStream(byteArray))
                            //    {
                            //        IFormFile formFile = new FormFile(stream, 0, byteArray.Length, "file", "Proposal.pdf")
                            //        {
                            //            Headers = new HeaderDictionary(),
                            //            ContentType = "application/pdf"
                            //        };
                            //        string accountname = "";
                            //        string accesskey = "";
                            //        var azure = _db.IVRM_Storage_path_Details.ToList();
                            //        if (azure.Count() > 0)
                            //        {
                            //            accountname = azure.FirstOrDefault().IVRM_SD_Access_Name;
                            //            accesskey = azure.FirstOrDefault().IVRM_SD_Access_Key;
                            //        }
                            //        var names = new List<string>();
                            //        var contentTypes = new List<string>();
                            //        fileid = await whtsappnew.uploadtoazure(formFile, "files", "Trnsportdocuments", data.MI_Id, accountname, accesskey);
                            //        data.ISMSLET_HtmlPdf = fileid;
                            //    }
                            //}

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }



                    }



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";


        }


    }
}
