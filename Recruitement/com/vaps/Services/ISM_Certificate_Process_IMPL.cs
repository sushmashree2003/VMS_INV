using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.Exit;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using Recruitment.com.vaps.Interfaces;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class ISM_Certificate_Process_IMPL : Interfaces.ISM_Certificate_Process_Interface
    {
        public HRMSContext _HRMSContext;
        public VMSContext _vmsconte;

        public ISM_Certificate_Process_IMPL(VMSContext vm, HRMSContext HRMSContext)
        {
            _vmsconte = vm;
            _HRMSContext = HRMSContext;
        }

        //---------------------------------ISM_Certificate_Request---------------------------------------
        public ISM_Certificate_RequestDTO getloaddata(ISM_Certificate_RequestDTO data)
        {
            try

            {



                var HRME_Id = _vmsconte.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();

                var Authorisation = (from a in _vmsconte.HR_Process_Auth_OrderNoDMO
                                     from b in _vmsconte.HR_PROCESSDMO
                                     from c in _vmsconte.HR_Master_Employee_DMO
                                     from d in _vmsconte.IVRM_Staff_User_Login
                                     where (c.HRME_Id == HRME_Id && b.HRPA_Id == a.HRPA_Id && b.HRMD_Id == c.HRMD_Id && b.HRMDES_Id == c.HRMDES_Id
                                            && b.HRMGT_Id == c.HRMGT_Id && b.HRMG_Id == c.HRMG_Id && a.IVRMUL_Id == d.Id &&
                                            b.HRPA_TypeFlag == "CertificateRequest")
                                     select a).OrderByDescending(t => t.HRPAON_SanctionLevelNo).ToList();

                data.maxmumlevel = Authorisation.Select(t => t.HRPAON_SanctionLevelNo).FirstOrDefault();
                var DateEntryLevelID = Authorisation.Where(t => t.HRPAON_SanctionLevelNo == data.maxmumlevel - 1).Select(t => t.IVRMUL_Id).FirstOrDefault();

                data.HRME_DOJ = _vmsconte.HR_Master_Employee_DMO.Where(t => t.HRME_Id == HRME_Id).Select(t => t.HRME_DOJ).FirstOrDefault();

                data.document = _vmsconte.Master_Employee_Documents.Where(t => t.HRME_Id == HRME_Id && t.HRMEDS_ReturnFlg == true).ToArray();

                var ISMCERTREQ_Id = (from a in _vmsconte.ISM_Certificate_RequestDMO
                                                    from b in _vmsconte.ISM_Certificate_ApprovalDMO
                                                    from c in _vmsconte.Staff_User_Login
                                                    where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && c.Emp_Code == b.HRME_Id && a.ISMCERTREQ_Id == b.ISMCERTREQ_Id && a.HRME_Id == HRME_Id && c.Id == DateEntryLevelID)
                                                    select new ISM_Certificate_RequestDTO
                                                    {
                                                        ISMCERTREQ_Id = b.ISMCERTREQ_Id,
                                                    }).Select(t => t.ISMCERTREQ_Id).FirstOrDefault();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Certificate_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMCERTREQ_Id", SqlDbType.VarChar)
                    {
                        Value = ISMCERTREQ_Id
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
                        data.getloaddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


                data.ISMCERTREQ_FilePath = (from a in _vmsconte.ISM_Certificate_RequestDMO
                                            from b in _vmsconte.ISM_Certificate_ApprovalDMO
                                            where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.ISMCERTREQ_Id == b.ISMCERTREQ_Id && a.HRME_Id == HRME_Id && b.ISMCERTREQAPP_FinalFlg == true)
                                            select new ISM_Certificate_RequestDTO
                                            {
                                                ISMCERTREQ_FilePath = b.ISMCERTREQAPP_ACK,
                                            }).Select(t => t.ISMCERTREQ_FilePath).FirstOrDefault();


                data.display_data = (from a in _vmsconte.ISM_Certificate_RequestDMO
                                     from b in _vmsconte.HR_Master_Employee_DMO
                                     where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.HRME_Id == b.HRME_Id   && a.HRME_Id == HRME_Id)
                                     select new ISM_Certificate_RequestDTO
                                     {
                                         ISMCERTREQ_Id = a.ISMCERTREQ_Id,
                                         HRME_Id = b.HRME_Id,
                                         ISMCERTREQ_RequestDate = a.ISMCERTREQ_RequestDate,
                                         ISMCERTREQ_Remarks = a.ISMCERTREQ_Remarks,
                                         ISMCERTREQ_ActiveFlag = a.ISMCERTREQ_ActiveFlag,
                                         ISMCERTREQ_AppRejFlag = a.ISMCERTREQ_AppRejFlag,
                                         ISMCERTREQ_FilePath = a.ISMCERTREQ_FilePath,
                                         ISMCERTREQ_ReceivingMode = a.ISMCERTREQ_ReceivingMode,
                                         ISMCERTREQ_RecivingAddress = a.ISMCERTREQ_RecivingAddress,
                                         HRME_EmployeeFirstName = (b.HRME_EmployeeFirstName + ' ' + b.HRME_EmployeeMiddleName + ' ' + b.HRME_EmployeeLastName)

                                     }).ToArray();




            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;

        }
        public ISM_Certificate_RequestDTO savedetails(ISM_Certificate_RequestDTO data)
        {
            try
            {
                var emplooyeecode = _vmsconte.IVRM_Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().ToList();

                List<ISM_Certificate_RequestDTO> result = new List<ISM_Certificate_RequestDTO>();


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 500000;
                    cmd.CommandText = "HR_SactionLevel_MailIds";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(emplooyeecode[0].Emp_Code)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRPAON_SanctionLevelNo", SqlDbType.BigInt)
                    {
                        Value = 1
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {

                            while (dataReader.Read())
                            {
                                try
                                {
                                    result.Add(new ISM_Certificate_RequestDTO
                                    {
                                        email = dataReader["HRMEM_EmailId"].ToString(),


                                    });
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }


                            data.emaildetails = result.ToArray();

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (data.emaildetails.Length == 0)
                {
                    data.returnduplicatestatus = "AuthoritySetting";
                }
                else
                {


                    var email = result[0].email;





                    var ccemail = (from b in _vmsconte.Multiple_Email_DMO
                                   from c in _vmsconte.IVRM_Staff_User_Login
                                   where (c.Id == data.UserId && c.Emp_Code == b.HRME_Id && b.HRMEM_DeFaultFlag == "default")
                                   select b).Select(t => t.HRMEM_EmailId).FirstOrDefault();



                    if (data.ISMCERTREQ_Id > 0)
                    {

                        var res = _vmsconte.ISM_Certificate_RequestDMO.Where(t => t.ISMCERTREQ_Id != data.ISMCERTREQ_Id && t.HRME_Id == emplooyeecode[0].Emp_Code && (t.ISMCERTREQ_AppRejFlag != "Pending" || t.ISMCERTREQ_AppRejFlag != "Approved")).ToList();

                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {

                            var result1 = _vmsconte.ISM_Certificate_RequestDMO.Single(t => t.ISMCERTREQ_Id == data.ISMCERTREQ_Id);

                            result1.ISMCERTREQ_CertificateName = data.ISMCERTREQ_CertificateName;
                            result1.ISMCERTREQ_Remarks = data.ISMCERTREQ_Remarks;
                            result1.ISMCERTREQ_ReceivingMode = data.ISMCERTREQ_ReceivingMode;
                            result1.ISMCERTREQ_FileName = data.ISMCERTREQ_FileName;
                            result1.ISMCERTREQ_FilePath = data.ISMCERTREQ_FilePath;
                            result1.ISMCERTREQ_RecivingAddress = data.ISMCERTREQ_RecivingAddress;
                            result1.ISMCERTREQ_ActiveFlag = true;                           
                            result1.ISMCERTREQ_RequestDate = DateTime.Now;
                            result1.ISMCERTREQ_CreatedDate = DateTime.Now;
                            result1.ISMCERTREQ_UpdatedDate = DateTime.Now;
                            result1.ISMCERTREQ_CreatedBy = data.UserId;
                            result1.ISMCERTREQ_UpdatedBy = data.UserId;
                            result1.ISMCERTREQ_AppRejFlag = "Pending";
                            _vmsconte.Update(result1);

                            var contactExists = _vmsconte.SaveChanges();
                            if (contactExists > 0)
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


                        var res = _vmsconte.ISM_Certificate_RequestDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == emplooyeecode[0].Emp_Code && t.ISMCERTREQ_AppRejFlag != "Rejected").ToList();
                        if (res.Count > 0)
                        {

                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            ISM_Certificate_RequestDMO obj = new ISM_Certificate_RequestDMO();

                            obj.MI_Id = data.MI_Id;
                            obj.HRME_Id = emplooyeecode[0].Emp_Code;
                            obj.ISMCERTREQ_CertificateName = data.ISMCERTREQ_CertificateName;
                            obj.ISMCERTREQ_Remarks = data.ISMCERTREQ_Remarks;
                            obj.ISMCERTREQ_ReceivingMode = data.ISMCERTREQ_ReceivingMode;
                            obj.ISMCERTREQ_FileName = data.ISMCERTREQ_FileName;
                            obj.ISMCERTREQ_FilePath = data.ISMCERTREQ_FilePath;
                            obj.ISMCERTREQ_ActiveFlag = true;
                            obj.ISMCERTREQ_RequestTypeFlag = "Certificate";
                            obj.ISMCERTREQ_RequestDate = data.ISMCERTREQ_RequestDate;
                            obj.ISMCERTREQ_RecivingAddress = data.ISMCERTREQ_RecivingAddress;
                            obj.ISMCERTREQ_CreatedDate = DateTime.Now;
                            obj.ISMCERTREQ_UpdatedDate = DateTime.Now;
                            obj.ISMCERTREQ_CreatedBy = data.UserId;
                            obj.ISMCERTREQ_UpdatedBy = data.UserId;
                            obj.ISMCERTREQ_AppRejFlag = "Pending";
                            _vmsconte.Add(obj);

                            var contactExists = _vmsconte.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;
                                sendmailcertificate(data.MI_Id, "Certificate_Request", emplooyeecode[0].Emp_Code, result[0].email, ccemail);
                            }
                            else
                            {
                                data.returnval = false;
                            }


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
        public ISM_Certificate_RequestDTO Edit(ISM_Certificate_RequestDTO data)
        {
            try
            {
                data.getloaddetails = _vmsconte.ISM_Certificate_RequestDMO.Where(a =>  a.ISMCERTREQ_Id == data.ISMCERTREQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Certificate_RequestDTO deactive(ISM_Certificate_RequestDTO data)
        {
            try
            {
                var result = _vmsconte.ISM_Certificate_RequestDMO.Single(t => t.ISMCERTREQ_Id == data.ISMCERTREQ_Id);

                if (result.ISMCERTREQ_ActiveFlag == true)
                {
                    result.ISMCERTREQ_ActiveFlag = false;
                }
                else if (result.ISMCERTREQ_ActiveFlag == false)
                {
                    result.ISMCERTREQ_ActiveFlag = true;
                }
                result.ISMCERTREQ_UpdatedDate = DateTime.Now;
                _vmsconte.Update(result);
                int returnval = _vmsconte.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public ISM_Certificate_RequestDTO Viewdata(ISM_Certificate_RequestDTO data)
        {
            try
            {




                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Certificate_Request_ApprovalLevel";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMCERTREQ_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ISMCERTREQ_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }
                        data.modalview = retObject.ToArray();

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
        //---------------certificate approval----------------------
        public ISM_Certificate_RequestDTO loaddata(ISM_Certificate_RequestDTO dto)
        {
            try
            {


                var HRME_Id = _vmsconte.Staff_User_Login.Where(t => t.Id == dto.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_CertificateRequest_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.UserId)
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
                        dto.Cerfificatelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Certificate_Approval_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = HRME_Id
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
                        dto.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public ISM_Certificate_RequestDTO Viewsdata(ISM_Certificate_RequestDTO data)
        {
            try
            {
                data.viewlist = (from a in _vmsconte.HR_PROCESSDMO
                                 from b in _vmsconte.HR_Process_Auth_OrderNoDMO
                                 from c in _vmsconte.ISM_Certificate_RequestDMO
                                 from d in _vmsconte.HR_Master_Employee_DMO
                                 where (c.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.HRMD_Id == d.HRMD_Id && a.HRMDES_Id == d.HRMDES_Id && a.HRMGT_Id == d.HRMGT_Id && a.HRMG_Id == d.HRMG_Id && c.HRME_Id == d.HRME_Id && c.ISMCERTREQ_Id == data.ISMCERTREQ_Id && b.IVRMUL_Id == data.UserId && a.HRPA_Id == b.HRPA_Id && a.HRPA_TypeFlag == "CertificateRequest")
                                 select b).ToArray();


                var Authorisation = (from a in _vmsconte.HR_Process_Auth_OrderNoDMO
                                     from b in _vmsconte.HR_PROCESSDMO
                                     from c in _vmsconte.HR_Master_Employee_DMO
                                     from d in _vmsconte.IVRM_Staff_User_Login
                                     where (c.HRME_Id == data.HRME_Id && b.HRPA_Id == a.HRPA_Id && b.HRMD_Id == c.HRMD_Id && b.HRMDES_Id == c.HRMDES_Id
                                            && b.HRMGT_Id == c.HRMGT_Id && b.HRMG_Id == c.HRMG_Id && a.IVRMUL_Id == d.Id &&
                                            b.HRPA_TypeFlag == "CertificateRequest")
                                     select a).OrderByDescending(t => t.HRPAON_SanctionLevelNo).ToList();

                data.maxmumlevel = Authorisation.Select(t => t.HRPAON_SanctionLevelNo).FirstOrDefault();
                var DateEntryLevelID = Authorisation.Where(t => t.HRPAON_SanctionLevelNo == data.maxmumlevel - 1).Select(t => t.IVRMUL_Id).FirstOrDefault();
                data.ISMCERTREQAPP_ReceiningDate = (from a in _vmsconte.ISM_Certificate_RequestDMO
                                                    from b in _vmsconte.ISM_Certificate_ApprovalDMO
                                                    from c in _vmsconte.Staff_User_Login
                                                    where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && c.Emp_Code == b.HRME_Id && a.ISMCERTREQ_Id == b.ISMCERTREQ_Id && a.HRME_Id == data.HRME_Id && c.Id == DateEntryLevelID)
                                                    select new ISM_Certificate_RequestDTO
                                                    {
                                                        ISMCERTREQAPP_ReceiningDate = b.ISMCERTREQAPP_ReceiningDate,
                                                    }).Select(t => t.ISMCERTREQAPP_ReceiningDate).FirstOrDefault();

                data.maxmumlevel = Authorisation.Select(t => t.HRPAON_SanctionLevelNo).FirstOrDefault();

                data.attachment = _vmsconte.ISM_Certificate_RequestDMO.Where(a => a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.ISMCERTREQ_Id == data.ISMCERTREQ_Id).Select(R => R.ISMCERTREQ_FilePath).FirstOrDefault();

                data.employees = (from a in _vmsconte.Hr_Master_Employee_con
                                 where(a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                 select new ISM_Certificate_RequestDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                                  + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                                  + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                 }).ToArray();

                // data.document= _vmsconte.Master_Employee_Documents.where(a=>a.HRMEDS_Id==data.HRMEDS_Id && a.HRME_Id==data.HRME_Id)
                List<Master_Employee_Documents> images_map = new List<Master_Employee_Documents>();
                images_map = _vmsconte.Master_Employee_Documents.Where(a => a.HRME_Id == data.HRME_Id && a.HRMEDS_ReturnFlg == true).ToList();
                data.document = images_map.ToArray();
                //data.viewattachment = _vmsconte.Master_Employee_Documents.Where(a => a.HRMEDS_Id == data.HRMEDS_Id && a.MI_Id == data.MI_Id).Select(R => R.HRMEDS_DocumentImageName).FirstOrDefault();
                data.viewattachment = _vmsconte.Master_Employee_Documents.Where(a => a.HRME_Id == data.HRME_Id && a.HRMEDS_ReturnFlg == true).Select(R => R.HRMEDS_DocumentImageName).FirstOrDefault();

                var mi_id = _vmsconte.Hr_Master_Employee_con.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();

                data.ISMCERTREQAPP_Remarks = _vmsconte.ISM_Certificate_RequestDMO.Where(t => t.ISMCERTREQ_RequestTypeFlag == "Certificate" && t.ISMCERTREQ_Id == data.ISMCERTREQ_Id).Select(t => t.ISMCERTREQ_Remarks).FirstOrDefault();
                data.ISMCERTREQ_FilePath = _vmsconte.ISM_Certificate_ApprovalDMO.Where(t => t.ISMCERTREQ_Id == data.ISMCERTREQ_Id && t.ISMCERTREQAPP_FinalFlg == true).Select(t => t.ISMCERTREQAPP_ACK).FirstOrDefault();

                data.RoleType = (from a in _vmsconte.ApplicationUserRole
                                 from b in _vmsconte.IVRM_Role_Type
                                 where (a.RoleTypeId == b.IVRMRT_Id && a.UserId == data.UserId && b.IVRMRT_RoleFlag == "Front Office Executive")
                                 select b).Select(t => t.IVRMRT_RoleFlag).FirstOrDefault();

                data.aprovedlist = (from a in _vmsconte.ISM_Certificate_RequestDMO
                                    from b in _vmsconte.ISM_Certificate_ApprovalDMO
                                    from c in _vmsconte.Hr_Master_Employee_con
                                    where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.ISMCERTREQ_Id == b.ISMCERTREQ_Id && a.ISMCERTREQ_Id == data.ISMCERTREQ_Id && b.HRME_Id == c.HRME_Id)
                                    select new ISM_Certificate_RequestDTO
                                    {
                                        HRME_Id = c.HRME_Id,
                                        ISMCERTREQAPP_ApprovedDate = b.ISMCERTREQAPP_ApprovedDate,
                                        ISMCERTREQAPP_ReceiningDate = b.ISMCERTREQAPP_ReceiningDate,
                                        ISMCERTREQAPP_Remarks = b.ISMCERTREQAPP_Remarks,
                                        ISMCERTREQAPP_FinalFlg = b.ISMCERTREQAPP_FinalFlg,
                                        ISMCERTREQAPP_AppRejFlag = b.ISMCERTREQAPP_AppRejFlag,
                                        ISMCERTREQ_Remarks = a.ISMCERTREQ_Remarks,
                                        HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : c.HRME_EmployeeFirstName)
                                                  + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" ? "" : " " + c.HRME_EmployeeMiddleName)
                                                  + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" ? "" : " " + c.HRME_EmployeeLastName)).Trim(),
                                    }).ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Certificate_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMCERTREQ_Id", SqlDbType.VarChar)
                    {
                        Value = data.ISMCERTREQ_Id
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
                        data.display_data = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }



                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = mi_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.getloaddetails = retObject.ToArray();
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
        public ISM_Certificate_RequestDTO approvedrecord(ISM_Certificate_RequestDTO data)
        {
            try
            {
                var email = "";
                var ccemail = "";
                var ccemail1 = "";
                var ccemail2 = "";
                var ccemail3 = "";
                //var HRPAON_SanctionLevelNo
                var HRME_Id = _vmsconte.ISM_Certificate_RequestDMO.Where(t => t.ISMCERTREQ_RequestTypeFlag == "Certificate" && t.ISMCERTREQ_Id == data.ISMCERTREQ_Id).Select(t => t.HRME_Id).FirstOrDefault();
                var Authorisation = (from a in _vmsconte.HR_Process_Auth_OrderNoDMO
                                     from b in _vmsconte.HR_PROCESSDMO
                                     from c in _vmsconte.HR_Master_Employee_DMO
                                     from d in _vmsconte.IVRM_Staff_User_Login
                                     where (c.HRME_Id == HRME_Id && d.Id == data.UserId && b.HRPA_Id == a.HRPA_Id && b.HRMD_Id == c.HRMD_Id && b.HRMDES_Id == c.HRMDES_Id
                                            && b.HRMGT_Id == c.HRMGT_Id && b.HRMG_Id == c.HRMG_Id && a.IVRMUL_Id == d.Id &&
                                            b.HRPA_TypeFlag == "CertificateRequest")
                                     select a).ToList();


                if (Authorisation[0].HRPAON_SanctionLevelNo == data.maxmumlevel)
                {
                    email = _vmsconte.Multiple_Email_DMO.Where(t => t.HRMEM_DeFaultFlag == "default" && t.HRME_Id == HRME_Id).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    ccemail1 = (from b in _vmsconte.Multiple_Email_DMO
                                from c in _vmsconte.IVRM_Staff_User_Login
                                where (c.Id == data.UserId && c.Emp_Code == b.HRME_Id && b.HRMEM_DeFaultFlag == "default")
                                select b).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    ccemail2 = _vmsconte.Multiple_Email_DMO.Where(t => t.HRMEM_DeFaultFlag == "default" && t.HRME_Id == data.HRME_Id).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    ccemail3 = (from a in _vmsconte.HR_Process_Auth_OrderNoDMO
                             from b in _vmsconte.HR_PROCESSDMO
                             from c in _vmsconte.HR_Master_Employee_DMO
                             from d in _vmsconte.IVRM_Staff_User_Login
                             from e in _vmsconte.Multiple_Email_DMO
                             where (c.HRME_Id == HRME_Id && b.HRPA_Id == a.HRPA_Id && b.HRMD_Id == c.HRMD_Id && b.HRMDES_Id == c.HRMDES_Id
                                    && b.HRMGT_Id == c.HRMGT_Id && b.HRMG_Id == c.HRMG_Id && a.IVRMUL_Id == d.Id && e.HRME_Id == d.Emp_Code &&
                                    b.HRPA_TypeFlag == "CertificateRequest" && e.HRMEM_DeFaultFlag == "default" &&
                                     a.HRPAON_SanctionLevelNo == Authorisation[0].HRPAON_SanctionLevelNo + 1)
                             select e).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    ccemail = ccemail1 + ',' + ccemail2 + ',' + ccemail3;
                }
                else if (Authorisation[0].HRPAON_FinalFlg == true)
                {
                    email = _vmsconte.Multiple_Email_DMO.Where(t => t.HRMEM_DeFaultFlag == "default" && t.HRME_Id == HRME_Id).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    ccemail = (from b in _vmsconte.Multiple_Email_DMO
                                from c in _vmsconte.IVRM_Staff_User_Login
                                where (c.Id == data.UserId && c.Emp_Code == b.HRME_Id && b.HRMEM_DeFaultFlag == "default")
                                select b).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                }
                else
                {
                    email = (from a in _vmsconte.HR_Process_Auth_OrderNoDMO
                             from b in _vmsconte.HR_PROCESSDMO
                             from c in _vmsconte.HR_Master_Employee_DMO
                             from d in _vmsconte.IVRM_Staff_User_Login
                             from e in _vmsconte.Multiple_Email_DMO
                             where (c.HRME_Id== HRME_Id && b.HRPA_Id == a.HRPA_Id && b.HRMD_Id == c.HRMD_Id && b.HRMDES_Id == c.HRMDES_Id
                                    && b.HRMGT_Id == c.HRMGT_Id && b.HRMG_Id == c.HRMG_Id && a.IVRMUL_Id == d.Id && e.HRME_Id == d.Emp_Code &&
                                    b.HRPA_TypeFlag == "CertificateRequest" && e.HRMEM_DeFaultFlag == "default" &&
                                     a.HRPAON_SanctionLevelNo == Authorisation[0].HRPAON_SanctionLevelNo + 1)
                             select e).Select(t => t.HRMEM_EmailId).FirstOrDefault();


                    ccemail1 = (from b in _vmsconte.Multiple_Email_DMO
                                from c in _vmsconte.IVRM_Staff_User_Login
                                where (c.Id == data.UserId && c.Emp_Code == b.HRME_Id && b.HRMEM_DeFaultFlag == "default")
                                select b).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    //ccemail2 = _vmsconte.Multiple_Email_DMO.Where(t => t.HRMEM_DeFaultFlag == "default" && t.HRME_Id == HRME_Id).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                    ccemail = ccemail1;
                    //ccemail = ccemail1 + ',' + ccemail2;

                }

                var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_Certificate_Approval @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", data.ISMCERTREQ_Id, data.ISMCERTREQAPP_ReceivingTime, data.ISMCERTREQAPP_AppRejFlag, data.ISMCERTREQAPP_Remarks, data.UserId, data.ISMCERTREQ_FilePath,data.Fromdate,data.Todate,data.HRME_Id);
                if (contactExistsP > 0)
                {
                    data.returnval = true;
                    var templatename = "";
                    var FinalFlg = _vmsconte.ISM_Certificate_ApprovalDMO.Where(s => s.ISMCERTREQAPP_FinalFlg == true && s.ISMCERTREQ_Id == data.ISMCERTREQ_Id).Select(t => t.ISMCERTREQAPP_FinalFlg).FirstOrDefault();

                    var HRME_ID = _vmsconte.ISM_Certificate_RequestDMO.Where(s => s.ISMCERTREQ_RequestTypeFlag == "Certificate" && s.ISMCERTREQ_Id == data.ISMCERTREQ_Id).Select(s => s.HRME_Id).FirstOrDefault();
                    if (FinalFlg == true)
                    {
                        if (data.ISMCERTREQAPP_AppRejFlag == "Approved")
                        {
                            //templatename = "Certificate_Approved";
                        }
                        else
                        {
                            templatename = "Certificate_Approval";
                        }

                    }else if (Authorisation[0].HRPAON_SanctionLevelNo == data.maxmumlevel)
                    {
                        templatename = "Certificate_Approval_Dated";
                    }
                    else
                    {
                        templatename = "Certificate_Approval";
                    }

                    if (email.Length > 0 && templatename.Length>0)
                    {
                        sendmailcertificate(data.MI_Id, templatename, HRME_ID, email, ccemail);
                    }



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

        //------------------ ISM Certificate Approval Report------------------------------
        public ISM_Certificate_RequestDTO Onload(ISM_Certificate_RequestDTO data)
        {
            try
            {


                data.fillyear = (from a in _vmsconte.HR_Master_LeaveYearDMO
                                 where (a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                 select new HR_Master_LeaveYearDTO
                                 {
                                     HRMLY_Id = a.HRMLY_Id,
                                     HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                     HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                 }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();


                data.fillmonth = _vmsconte.month.Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();

                data.employees = (from a in _vmsconte.ISM_Certificate_RequestDMO
                                  from b in _vmsconte.HR_Master_Employee_DMO
                                  where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.HRME_Id == b.HRME_Id && a.ISMCERTREQ_ActiveFlag == true )
                                  select new MasterEmployeeDTO
                                  {
                                      HRME_Id = b.HRME_Id,
                                      HRME_EmployeeFirstName = (b.HRME_EmployeeFirstName + ' ' + b.HRME_EmployeeMiddleName + ' ' + b.HRME_EmployeeLastName)
                                  }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ISM_Certificate_RequestDTO getapprovalreport(ISM_Certificate_RequestDTO data)
        {
            try
            {
                var from_date = data.Fromdate.Value.Date.ToString("yyyy-MM-dd");
                var To_date = data.Todate.Value.Date.ToString("yyyy-MM-dd");

                List<long> IVRM_Month_Id = new List<long>();

                for (var i = 0; i < data.MONTHID.Length; i++)
                {
                    IVRM_Month_Id.Add(data.MONTHID[i].IVRM_Month_Id);
                }
                string monthid = string.Join(",", IVRM_Month_Id);

                List<long> HRME_Id = new List<long>();

                for (var i = 0; i < data.HRMEID.Length; i++)
                {
                    HRME_Id.Add(data.HRMEID[i].HRME_Id);
                }
                string employeeid = string.Join(",", HRME_Id);

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_CERTIFICATE_APPROVAL_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value = employeeid });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = data.yearid });
                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Id", SqlDbType.VarChar) { Value = monthid });
                    cmd.Parameters.Add(new SqlParameter("@From_Date", SqlDbType.VarChar) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.VarChar) { Value = To_date });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.rdbbutton });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = data.Status_Type });

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
                        data.certificateapprovalReport = retObject.ToArray();

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
        public ISM_Certificate_RequestDTO DocView(ISM_Certificate_RequestDTO data)
        {
            //try
            //{
            //    using (var cmd = _cont.Database.GetDbConnection().CreateCommand())
            //    {
            //        cmd.CommandText = "IEMONTHEND_SUBMISSION_AND_APPROVED_DOCUMENT_REPORT";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add(new SqlParameter("@ISMCLMER_Id", SqlDbType.VarChar) { Value = data.ISMCLMER_Id });

            //        if (cmd.Connection.State != ConnectionState.Open)
            //            cmd.Connection.Open();

            //        var retObject = new List<dynamic>();
            //        try
            //        {
            //            using (var dataReader = cmd.ExecuteReader())
            //            {
            //                while (dataReader.Read())
            //                {
            //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                    {
            //                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
            //                    }
            //                    retObject.Add((ExpandoObject)dataRow);
            //                }
            //            }
            //            data.MultidocView = retObject.ToArray();

            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }

            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }

        public ISM_Certificate_RequestDTO getIEDetails(ISM_Certificate_RequestDTO data)
        {
            //try
            //{
            //    data.ielist = (from a in _cont.HR_Master_Employee_DMO
            //                   from b in _cont.ISM_Master_Client_IEMapping_DMO
            //                   from c in _cont.ISM_Master_Client_DMO
            //                   where (a.HRME_Id == b.ISMCIM_IEList && b.ISMMCLT_Id == c.ISMMCLT_Id && c.ISMMCLT_Id == data.ISMMCLT_Id)
            //                   select new MasterEmployeeDTO
            //                   {
            //                      HRME_Id = a.HRME_Id,
            //                      HRME_EmployeeFirstName = (a.HRME_EmployeeFirstName+' '+a.HRME_EmployeeMiddleName+' '+a.HRME_EmployeeLastName)
            //                   }).ToArray();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }

        //ISM_ADMIN_CERTIFICATE_APPLY
        public ISM_Certificate_RequestDTO getadminloaddata(ISM_Certificate_RequestDTO data)        {            try            {                data.employees = (from a in _vmsconte.HR_Master_Employee_DMO                                  where (a.HRME_ActiveFlag == true)                                  select new ISM_Certificate_RequestDTO                                  {                                      HRME_Id = a.HRME_Id,                                      HRME_EmployeeFirstName = (a.HRME_EmployeeFirstName + ' ' + a.HRME_EmployeeMiddleName + ' ' + a.HRME_EmployeeLastName)                                  }).ToArray();                data.HRME_DOJ = _vmsconte.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.HRME_DOJ).FirstOrDefault();                data.document = _vmsconte.Master_Employee_Documents.Where(t => t.HRME_Id == data.HRME_Id).ToArray();                data.ISMCERTREQAPP_ReceiningDate = (from a in _vmsconte.ISM_Certificate_RequestDMO                                                    from b in _vmsconte.ISM_Certificate_ApprovalDMO                                                    where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.ISMCERTREQ_Id == b.ISMCERTREQ_Id && a.HRME_Id == data.HRME_Id && b.ISMCERTREQAPP_FinalFlg == true)                                                    select new ISM_Certificate_RequestDTO                                                    {                                                        ISMCERTREQAPP_ReceiningDate = b.ISMCERTREQAPP_ReceiningDate,                                                    }).Select(t => t.ISMCERTREQAPP_ReceiningDate).FirstOrDefault();                data.ISMCERTREQ_FilePath = (from a in _vmsconte.ISM_Certificate_RequestDMO                                            from b in _vmsconte.ISM_Certificate_ApprovalDMO                                            where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.ISMCERTREQ_Id == b.ISMCERTREQ_Id && a.HRME_Id == data.HRME_Id && b.ISMCERTREQAPP_FinalFlg == true)                                            select new ISM_Certificate_RequestDTO                                            {                                                ISMCERTREQ_FilePath = b.ISMCERTREQAPP_ACK,                                            }).Select(t => t.ISMCERTREQ_FilePath).FirstOrDefault();                data.display_data = (from a in _vmsconte.ISM_Certificate_RequestDMO                                     from b in _vmsconte.HR_Master_Employee_DMO


                                     where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.HRME_Id == b.HRME_Id && a.ISMCERTREQ_CreatedBy == data.UserId)                                     select new ISM_Certificate_RequestDTO                                     {                                         ISMCERTREQ_Id = a.ISMCERTREQ_Id,                                         HRME_Id = b.HRME_Id,                                         ISMCERTREQ_RequestDate = a.ISMCERTREQ_RequestDate,                                         ISMCERTREQ_Remarks = a.ISMCERTREQ_Remarks,                                         ISMCERTREQ_ActiveFlag = a.ISMCERTREQ_ActiveFlag,                                         ISMCERTREQ_AppRejFlag = a.ISMCERTREQ_AppRejFlag,                                         ISMCERTREQ_FilePath = a.ISMCERTREQ_FilePath,                                         HRME_EmployeeFirstName = (b.HRME_EmployeeFirstName + ' ' + b.HRME_EmployeeMiddleName + ' ' + b.HRME_EmployeeLastName)                                     }).OrderByDescending(t=>t.ISMCERTREQ_Id).ToArray();                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Page_Privilege_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@pageurl", SqlDbType.VarChar)
                    {
                        Value = "app.ISM_Admin_Certificate_Apply"
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
                        data.pagePrivilege = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }
        public ISM_Certificate_RequestDTO getcertificate(ISM_Certificate_RequestDTO data)        {            try            {                data.MI_Id = _vmsconte.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.MI_Id).FirstOrDefault();                data.HRME_DOJ = _vmsconte.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id).Select(t => t.HRME_DOJ).FirstOrDefault();                data.document = _vmsconte.Master_Employee_Documents.Where(t => t.HRME_Id == data.HRME_Id && t.HRMEDS_ReturnFlg == true).ToArray();                data.ISMCERTREQAPP_ReceiningDate = (from a in _vmsconte.ISM_Certificate_RequestDMO                                                    from b in _vmsconte.ISM_Certificate_ApprovalDMO                                                    where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.ISMCERTREQ_Id == b.ISMCERTREQ_Id && a.HRME_Id == data.HRME_Id && b.ISMCERTREQAPP_FinalFlg == true)                                                    select new ISM_Certificate_RequestDTO                                                    {                                                        ISMCERTREQAPP_ReceiningDate = b.ISMCERTREQAPP_ReceiningDate,                                                    }).Select(t => t.ISMCERTREQAPP_ReceiningDate).FirstOrDefault();                data.ISMCERTREQ_FilePath = (from a in _vmsconte.ISM_Certificate_RequestDMO                                            from b in _vmsconte.ISM_Certificate_ApprovalDMO                                            where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.ISMCERTREQ_Id == b.ISMCERTREQ_Id && a.HRME_Id == data.HRME_Id && b.ISMCERTREQAPP_FinalFlg == true)                                            select new ISM_Certificate_RequestDTO                                            {                                                ISMCERTREQ_FilePath = b.ISMCERTREQAPP_ACK,                                            }).Select(t => t.ISMCERTREQ_FilePath).FirstOrDefault();                data.display_data = (from a in _vmsconte.ISM_Certificate_RequestDMO                                     from b in _vmsconte.HR_Master_Employee_DMO                                     where (a.ISMCERTREQ_RequestTypeFlag == "Certificate" && a.HRME_Id == b.HRME_Id  && a.HRME_Id == data.HRME_Id)                                     select new ISM_Certificate_RequestDTO                                     {                                         ISMCERTREQ_Id = a.ISMCERTREQ_Id,                                         HRME_Id = b.HRME_Id,                                         ISMCERTREQ_RequestDate = a.ISMCERTREQ_RequestDate,                                         ISMCERTREQ_Remarks = a.ISMCERTREQ_Remarks,                                         ISMCERTREQ_ActiveFlag = a.ISMCERTREQ_ActiveFlag,                                         ISMCERTREQ_AppRejFlag = a.ISMCERTREQ_AppRejFlag,                                         ISMCERTREQ_FilePath = a.ISMCERTREQ_FilePath,                                         HRME_EmployeeFirstName = (b.HRME_EmployeeFirstName + ' ' + b.HRME_EmployeeMiddleName + ' ' + b.HRME_EmployeeLastName)                                     }).ToArray();




                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.getloaddetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }
        public ISM_Certificate_RequestDTO saveadmindetails(ISM_Certificate_RequestDTO data)        {            try            {

                List<ISM_Certificate_RequestDTO> result = new List<ISM_Certificate_RequestDTO>();
                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 500000;
                    cmd.CommandText = "HR_SactionLevel_MailIds";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRPAON_SanctionLevelNo", SqlDbType.BigInt)
                    {
                        Value = 1
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {

                            while (dataReader.Read())
                            {
                                try
                                {
                                    result.Add(new ISM_Certificate_RequestDTO
                                    {
                                        email = dataReader["HRMEM_EmailId"].ToString(),


                                    });
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }


                            data.emaildetails = result.ToArray();

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                if (data.emaildetails.Length == 0)
                {
                    data.returnduplicatestatus = "AuthoritySetting";
                }
                else
                {


                    var email = result[0].email;
                    var ccemail = "";
                   //var  ccemail = (from b in _vmsconte.Multiple_Email_DMO
                   //            from c in _vmsconte.Hr_Master_Employee_con
                   //            where (c.HRME_Id == data.HRME_Id && c.HRME_Id == b.HRME_Id && b.HRMEM_DeFaultFlag == "default")
                   //            select b).Select(t => t.HRMEM_EmailId).FirstOrDefault();


                    if (data.ISMCERTREQ_Id > 0)
                    {

                        var res = _vmsconte.ISM_Certificate_RequestDMO.Where(t => t.ISMCERTREQ_RequestTypeFlag == "Certificate" && t.ISMCERTREQ_Id != data.ISMCERTREQ_Id && t.HRME_Id == data.HRME_Id && (t.ISMCERTREQ_AppRejFlag != "Pending" || t.ISMCERTREQ_AppRejFlag != "Approved")).ToList();

                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {

                            var result1 = _vmsconte.ISM_Certificate_RequestDMO.Single(t => t.ISMCERTREQ_Id == data.ISMCERTREQ_Id);

                            result1.ISMCERTREQ_CertificateName = data.ISMCERTREQ_CertificateName;
                            result1.ISMCERTREQ_Remarks = data.ISMCERTREQ_Remarks;
                            result1.ISMCERTREQ_ReceivingMode = data.ISMCERTREQ_ReceivingMode;
                            result1.ISMCERTREQ_FileName = data.ISMCERTREQ_FileName;
                            result1.ISMCERTREQ_FilePath = data.ISMCERTREQ_FilePath;
                            result1.ISMCERTREQ_FilePath = data.ISMCERTREQ_FilePath;
                            result1.ISMCERTREQ_ActiveFlag = true;
                            result1.ISMCERTREQ_RequestDate = data.ISMCERTREQ_RequestDate;
                            result1.ISMCERTREQ_RecivingAddress = data.ISMCERTREQ_RecivingAddress;
                            result1.ISMCERTREQ_CreatedDate = DateTime.Now;
                            result1.ISMCERTREQ_UpdatedDate = DateTime.Now;
                            result1.ISMCERTREQ_CreatedBy = data.UserId;
                            result1.ISMCERTREQ_UpdatedBy = data.UserId;
                            result1.ISMCERTREQ_AppRejFlag = "Pending";
                            _vmsconte.Update(result1);

                            var contactExists = _vmsconte.SaveChanges();
                            if (contactExists > 0)
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


                        var res = _vmsconte.ISM_Certificate_RequestDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.ISMCERTREQ_AppRejFlag != "Rejected").ToList();
                        if (res.Count > 0)
                        {

                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            ISM_Certificate_RequestDMO obj = new ISM_Certificate_RequestDMO();

                            obj.MI_Id = data.MI_Id;
                            obj.HRME_Id = data.HRME_Id;
                            obj.ISMCERTREQ_CertificateName = data.ISMCERTREQ_CertificateName;
                            obj.ISMCERTREQ_Remarks = data.ISMCERTREQ_Remarks;
                            obj.ISMCERTREQ_ReceivingMode = data.ISMCERTREQ_ReceivingMode;
                            obj.ISMCERTREQ_FileName = data.ISMCERTREQ_FileName;
                            obj.ISMCERTREQ_FilePath = data.ISMCERTREQ_FilePath;
                            obj.ISMCERTREQ_ActiveFlag = true;
                            obj.ISMCERTREQ_RequestTypeFlag = "Certificate";
                            obj.ISMCERTREQ_RequestDate = data.ISMCERTREQ_RequestDate;
                            obj.ISMCERTREQ_RecivingAddress = data.ISMCERTREQ_RecivingAddress;
                            obj.ISMCERTREQ_CreatedDate = DateTime.Now;
                            obj.ISMCERTREQ_UpdatedDate = DateTime.Now;
                            obj.ISMCERTREQ_CreatedBy = data.UserId;
                            obj.ISMCERTREQ_UpdatedBy = data.UserId;
                            obj.ISMCERTREQ_AppRejFlag = "Pending";
                            _vmsconte.Add(obj);

                            var contactExists = _vmsconte.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.returnval = true;
                                sendmailcertificate(data.MI_Id, "Certificate_Request", data.HRME_Id, result[0].email, ccemail);
                            }
                            else
                            {
                                data.returnval = false;
                            }


                        }
                    }
                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }
        public string sendmailcertificate(long MI_Id, string Template, long HRME_ID, string email, string ccemail)        {            try            {                Dictionary<string, string> val = new Dictionary<string, string>();                var template = _vmsconte.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template                && e.ISES_MailActiveFlag == true).ToList();                if (template.Count == 0)                {                    return "Email Template not Mapped to the selected Institution";                }                var institutionName = _vmsconte.Institution.Where(i => i.MI_Id == MI_Id).ToList();                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Resultsms = Mailcontent;                string result = Mailmsg;                var Paramaeters = _vmsconte.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();                var ParamaetersName = _vmsconte.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_Email_Prametars_Replace";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",                  SqlDbType.VarChar)                    {                        Value = HRME_ID                    });                    cmd.Parameters.Add(new SqlParameter("@Type",                  SqlDbType.VarChar)                    {                        Value = Template                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                    var datatype = dataReader.GetFieldType(iFiled);                                    if (datatype.Name == "DateTime")                                    {                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else if (datatype.Name == "Decimal")                                    {                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else                                    {                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());                                    }                                }                            }                        }                    }                    catch (Exception ex)                    {                        return ex.Message;                    }                }                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailmsg = result;                        }                    }                }                Mailmsg = result;                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {


                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailcontent = Resultsms;                        }                    }                }                Mailcontent = Resultsms;                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();                alldetails = _vmsconte.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();                string Attechement = "";                if (alldetails.Count > 0)                {                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);                    string Subject = template[0].ISES_MailSubject.ToString();                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();                    smstpdetails = _vmsconte.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();                    string mailcc = "";                    string mailbcc = "";                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")                    {                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');                        mailcc = ccmail[0].ToString();                        if (ccmail.Length > 1)                        {                            if (ccmail[1] != null || ccmail[1] != "")                            {                                mailbcc = ccmail[1].ToString();                            }                        }                    }                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")                    {                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();                    }                    var message = new SendGridMessage();                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);                    message.Subject = Subject;                    message.AddTo(email);

                    // mailto:message.addto("kavitajnandoor21@gmail.com");
                    //message.AddTo("sreepriyagangadharan15@gmail.com");

                    if (Attechement.Equals("1"))                    {                        var img = _vmsconte.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();                        if (img.Count > 0)                        {                            for (int i = 0; i < img.Count; i++)                            {                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;                                Stream stream = response.GetResponseStream();                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);                            }                        }                    }
                    //****************** EMAIL CC DETAILS ***************//

                    if (ccemail != null && ccemail != "")                    {                        string[] ccmaildetails = ccemail.Split(',');                        foreach (var c in ccmaildetails)                        {                            message.AddCc(c);                        }                    }                    //if (mailcc != null && mailcc != "")                    //{                    //    string[] ccmaildetails = mailcc.Split(',');                    //    foreach (var c in ccmaildetails)                    //    {                    //        message.AddCc(c);                    //    }                    //}

                    //if (ccemail != null && ccemail != "")
                    //{
                    //    message.AddBcc(ccemail);
                    //}

                    //****************** EMAIL BCC DETAILS ***************//




                    if (mailcc != null && mailcc != "")                    {
                        message.AddCc(mailcc);
                    }                    if (mailbcc != null && mailbcc != "")                    {                        message.AddBcc(mailbcc);                    }                    message.HtmlContent = Mailmsg;                    var client = new SendGridClient(sengridkey);                    try                    {                        client.SendEmailAsync(message).Wait();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                    using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())                    {                        var template1010 = _vmsconte.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();                        var moduleid = _vmsconte.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();                        var modulename = _vmsconte.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();                        cmd.CommandText = "IVRM_Email_Outgoing";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@EmailId",                            SqlDbType.NVarChar)                        {                            Value = email                        });                        cmd.Parameters.Add(new SqlParameter("@Message",                           SqlDbType.NVarChar)                        {                            Value = Mailcontent                        });                        cmd.Parameters.Add(new SqlParameter("@module",                        SqlDbType.VarChar)                        {                            Value = modulename[0]                        });                        cmd.Parameters.Add(new SqlParameter("@MI_Id",                       SqlDbType.BigInt)                        {                            Value = MI_Id                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        try                        {                            using (var dataReader = cmd.ExecuteReader())                            {                            }                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return "success";        }


    }
}



