using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Services
{
    public class TransferCertificateImpl : Interfaces.TransferCertificateInterface
    {
        public DomainModelMsSqlServerContext _db;
        private PortalContext _PortalContext;
        public TransferCertificateImpl(PortalContext studentDashboardContext, DomainModelMsSqlServerContext db)
        {
            _PortalContext = studentDashboardContext;
            _db = db;
        }
        public TransferCertificate_DTO getdetails(TransferCertificate_DTO data)
        {
            try
            {
                if (data.flag != "" && data.flag != null)
                {
                    var roletyp = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                }

                if (data.roletype.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;
                    data.studentdetails = (from a in _PortalContext.Adm_M_Student
                                           from b in _PortalContext.School_Adm_Y_StudentDMO
                                           from c in _PortalContext.School_M_Class
                                           from s in _PortalContext.School_M_Section
                                           where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                           select new TransferCertificate_DTO
                                           {
                                               ASMCL_Id = c.ASMCL_Id,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMS_Id = b.ASMS_Id,
                                               ASMC_SectionName = s.ASMC_SectionName,
                                               AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                               AMST_RegistrationNo = a.AMST_RegistrationNo,
                                               AMST_MobileNo = a.AMST_MobileNo,
                                               AMST_emailId = a.AMST_emailId,
                                               AMST_Id = a.AMST_Id,
                                           }).Distinct().ToArray();

                    data.studlist = (from a in _PortalContext.Adm_Students_Certificate_Apply_DMO
                                     from b in _PortalContext.School_Adm_Y_StudentDMO
                                     from c in _PortalContext.Adm_M_Student
                                     from cl in _PortalContext.School_M_Class
                                     from sc in _PortalContext.School_M_Section
                                     where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == cl.ASMCL_Id && b.ASMS_Id == sc.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && c.AMST_SOL == "S")
                                     select new TransferCertificate_DTO
                                     {
                                         ASCA_Id = a.ASCA_Id,
                                         AMST_Id = a.AMST_Id,
                                         ASCA_CertificateType = a.ASCA_CertificateType,
                                         ASCA_Reason = a.ASCA_Reason,
                                         ASCA_ApplyDate = a.ASCA_ApplyDate,
                                         ASCA_Status = a.ASCA_Status,
                                         AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                         ASMS_Id = sc.ASMS_Id,
                                         ASMC_SectionName = sc.ASMC_SectionName,
                                         ASMCL_ClassName = cl.ASMCL_ClassName,
                                         ASMCL_Id = cl.ASMCL_Id,
                                         ASCA_ActiveFlg = a.ASCA_ActiveFlg,
                                         AMST_RegistrationNo = c.AMST_RegistrationNo,
                                         AMST_MobileNo = c.AMST_MobileNo,
                                         AMST_emailId = c.AMST_emailId,
                                     }).Distinct().OrderBy(t => t.ASCA_ApplyDate).ToArray();
                }
                else if (data.roletype.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;
                    //data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                    //var clstchname = (from a in _db.Adm_SchAttLoginUserClass
                    //                  from b in _db.Adm_SchAttLoginUser
                    //                  from c in _db.School_M_Class
                    //                  where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                    //                  && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                    //                  && b.HRME_Id == data.HRME_Id
                    //                  && c.ASMCL_ActiveFlag == true)
                    //                  select new TransferCertificate_DTO
                    //                  {
                    //                      ASMCL_Id = c.ASMCL_Id,
                    //                      ASMCL_ClassName = c.ASMCL_ClassName,
                    //                      ASMS_Id = a.ASMS_Id,
                    //                  }
                    //              ).Distinct().ToArray();


                    data.applylist = (from a in _PortalContext.Adm_Students_Certificate_Apply_DMO
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      from c in _PortalContext.School_M_Class
                                      from d in _PortalContext.School_M_Section
                                      from e in _PortalContext.Adm_M_Student
                                      where (a.MI_Id == c.MI_Id && b.AMST_Id == a.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.AMST_Id == e.AMST_Id && a.MI_Id == data.MI_Id && e.AMST_SOL == "S" && a.ASCA_ActiveFlg == true && a.ASCA_Status == "Pending")
                                      select new TransferCertificate_DTO
                                      {
                                          AMST_Id = a.AMST_Id,
                                          AMST_AdmNo = e.AMST_AdmNo,
                                          AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                                          ASMS_Id = d.ASMS_Id,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMCL_Id = c.ASMCL_Id,
                                          MI_Id = c.MI_Id,
                                          ASMAY_Id = a.ASMAY_Id,
                                          AMST_RegistrationNo = e.AMST_RegistrationNo,
                                          AMST_emailId = e.AMST_emailId,
                                          AMST_MobileNo = e.AMST_MobileNo,
                                          ASCA_Status = a.ASCA_Status,
                                          ASCA_Id = a.ASCA_Id,
                                          ASCA_CertificateType = a.ASCA_CertificateType,
                                          ASCA_Reason = a.ASCA_Reason,
                                          ASCA_ApplyDate = a.ASCA_ApplyDate,
                                      }).Distinct().OrderBy(t => t.ASCA_ApplyDate).ToArray();


                    data.aply_aprvlist = (from a in _PortalContext.Adm_Students_Certificate_Apply_DMO
                                          from b in _PortalContext.Adm_Students_Certificate_Approve_DMO
                                          from std in _PortalContext.Adm_M_Student
                                          from Y in _PortalContext.School_Adm_Y_StudentDMO
                                          from cl in _PortalContext.School_M_Class
                                          from sc in _PortalContext.School_M_Section
                                          from year in _PortalContext.AcademicYearDMO

                                          where (a.ASCA_Id == b.ASCA_Id && a.AMST_Id == std.AMST_Id && a.ASMAY_Id == Y.ASMAY_Id && a.AMST_Id == Y.AMST_Id &&
                                          cl.ASMCL_Id == Y.ASMCL_Id && sc.ASMS_Id == Y.ASMS_Id && Y.ASMAY_Id==year.ASMAY_Id && a.MI_Id==data.MI_Id && a.ASCA_ActiveFlg == true && std.AMST_SOL == "S" && Y.AMAY_ActiveFlag == 1)
                                          select new TransferCertificate_DTO
                                          {
                                              AMST_FirstName = ((std.AMST_FirstName == null ? " " : std.AMST_FirstName) + " " + (std.AMST_MiddleName == null ? " " : std.AMST_MiddleName) + " " + (std.AMST_LastName == null ? " " : std.AMST_LastName)).Trim(),                                          
                                              ASMC_SectionName = sc.ASMC_SectionName,
                                              ASMCL_ClassName = cl.ASMCL_ClassName,
                                              AMST_AdmNo=std.AMST_AdmNo,
                                              AMST_RegistrationNo = std.AMST_RegistrationNo,                                            
                                              ASCA_Status = a.ASCA_Status,
                                              ASCA_CertificateType = a.ASCA_CertificateType,
                                              ASCA_Reason = a.ASCA_Reason,
                                              ASCA_ApplyDate = a.ASCA_ApplyDate,
                                              ASCAP_ApproveDate=b.ASCAP_ApproveDate,
                                              ASCAP_ApproveReason=b.ASCAP_ApproveReason,
                                              ASMAY_Year=year.ASMAY_Year,
                                          }).Distinct().OrderBy(t => t.ASCA_ApplyDate).ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<TransferCertificate_DTO> tcApply(TransferCertificate_DTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.ASCA_Id > 0)
                {
                    var Duplicate = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_Id != data.ASCA_Id && t.ASCA_CertificateType == data.ASCA_CertificateType && t.ASCA_Status == "Pending").ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id).SingleOrDefault();

                        update.ASCA_CertificateType = data.ASCA_CertificateType;
                        update.ASCA_Reason = data.ASCA_Reason;
                        update.ASCA_ApplyDate = data.ASCA_ApplyDate;
                        update.ASCA_Status = "Pending";

                        update.UpdatedDate = DateTime.Now;
                        _PortalContext.Update(update);

                        int rowAffected = _PortalContext.SaveChanges();

                        if (rowAffected > 0)
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
                    var Duplicate = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_CertificateType == data.ASCA_CertificateType && t.AMST_Id == data.AMST_Id && t.ASCA_Status == "Pending" | t.ASCA_Status == "Approved").ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        Adm_Students_Certificate_Apply_DMO obj = new Adm_Students_Certificate_Apply_DMO();

                        obj.MI_Id = data.MI_Id;
                        obj.AMST_Id = data.AMST_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.ASCA_CertificateType = data.ASCA_CertificateType;
                        obj.ASCA_Reason = data.ASCA_Reason;
                        obj.ASCA_ApplyDate = data.ASCA_ApplyDate;
                        obj.ASCA_Status = "Pending";
                        obj.ASCA_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _PortalContext.Add(obj);

                        int rowAffected = _PortalContext.SaveChanges();

                        if (rowAffected > 0)
                        {
                            var clsid = (from d in _PortalContext.AcademicYearDMO
                                         from a in _PortalContext.School_M_Class
                                         from c in _PortalContext.School_Adm_Y_StudentDMO
                                         where (a.ASMCL_Id == c.ASMCL_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                         select new ExamDTO
                                         {
                                             ASMCL_Id = c.ASMCL_Id,
                                             ASMCL_ClassName = a.ASMCL_ClassName,
                                         }
                                ).Distinct().ToList();

                            var hrmeids = (from a in _PortalContext.School_M_Class
                                           from b in _PortalContext.HR_Master_Employee_DMO
                                           from c in _PortalContext.IVRM_PrincipalDMO
                                           from d in _PortalContext.IVRM_Principal_ClassDMO
                                           where (a.ASMCL_Id == d.ASMCL_Id && b.HRME_Id == c.IVRMUL_Id && a.MI_Id == c.MI_Id && c.MI_Id == data.MI_Id && a.ASMCL_Id == clsid.FirstOrDefault().ASMCL_Id)
                                           select new TransferCertificate_DTO
                                           {
                                               HRME_Id = c.IVRMUL_Id
                                           }
                                          ).Distinct().ToArray();

                            data.HRME_Id = hrmeids.FirstOrDefault().HRME_Id;

                            long mobileno = 0;
                            var emailid = "";
                            var mobile = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEMNO_DeFaultFlag == "default").ToList();
                            var email = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == data.HRME_Id && e.HRMEM_DeFaultFlag == "default").ToList();

                            if (mobile.Count > 0)
                            {
                                mobileno = _PortalContext.Multiple_Mobile_DMO.Single(p => p.HRME_Id == data.HRME_Id && p.HRMEMNO_DeFaultFlag == "default").HRMEMNO_MobileNo;
                                SMS sms = new SMS(_db);
                                s = await sms.sendCertificateSms(data.MI_Id, mobileno, "TCREQUEST", data.AMST_Id, data.HRME_Id, data.ASMAY_Id, obj.ASCA_Id);
                            }
                            if (email.Count > 0)
                            {
                                emailid = _PortalContext.Multiple_Email_DMO.Single(e => e.HRME_Id == data.HRME_Id && e.HRMEM_DeFaultFlag == "default").HRMEM_EmailId;
                                Email Email = new Email(_db);
                                m = Email.sendCertificatemail(data.MI_Id, emailid, "TCREQUEST", data.AMST_Id, data.HRME_Id, data.ASMAY_Id, obj.ASCA_Id);
                            }
                            if (s == "success" && m == "success")
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                            // data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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

        public TransferCertificate_DTO deactiveY(TransferCertificate_DTO data)
        {
            try
            {
                var result = _PortalContext.Adm_Students_Certificate_Apply_DMO.Single(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id);

                if (result.ASCA_ActiveFlg == true)
                {
                    result.ASCA_ActiveFlg = false;
                }
                else if (result.ASCA_ActiveFlg == false)
                {
                    result.ASCA_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _PortalContext.Update(result);
                int rowAffected = _PortalContext.SaveChanges();
                if (rowAffected > 0)
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

        public async Task<TransferCertificate_DTO> certificateApproved(TransferCertificate_DTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.ASCA_Id > 0)
                {
                    var update = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id).SingleOrDefault();

                    update.ASCA_Status = "Approved";
                    update.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(update);

                    Adm_Students_Certificate_Approve_DMO obj2 = new Adm_Students_Certificate_Approve_DMO();

                    obj2.ASCA_Id = update.ASCA_Id;
                    obj2.MI_Id = data.MI_Id;
                    obj2.IVRMALU_Id = data.UserId;
                    obj2.ASCAP_Status = "Approved";
                    obj2.ASCAP_ApproveReason = data.ASCAP_ApproveReason;
                    obj2.ASCAP_ApproveDate = DateTime.Now;
                    obj2.ASCAP_ActiveFlg = true;
                    obj2.CreatedDate = DateTime.Now;
                    obj2.UpdatedDate = DateTime.Now;

                    _PortalContext.Add(obj2);
                    _PortalContext.Update(update);
                    int rowAffected = _PortalContext.SaveChanges();

                    if (rowAffected > 0)
                    {
                        var asmstid = _PortalContext.Adm_Students_Certificate_Apply_DMO.Single(a => a.ASCA_Id == data.ASCA_Id).AMST_Id;
                        var mobilemail = _PortalContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == asmstid && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S").Distinct().ToList();


                        long mobileno = 0;
                        var emailid = "";
                        if (mobilemail.Count > 0)
                        {
                            mobileno = mobilemail.FirstOrDefault().AMST_MobileNo;
                            SMS sms = new SMS(_db);
                            s = await sms.sendCertificateSms(data.MI_Id, mobileno, "TCSTATUS", asmstid, data.HRME_Id,  data.ASMAY_Id, obj2.ASCAP_Id);
                        }
                        if (mobilemail.Count > 0)
                        {
                            emailid = mobilemail.FirstOrDefault().AMST_emailId;
                            Email Email = new Email(_db);
                            m = Email.sendCertificatemail(data.MI_Id, emailid, "TCSTATUS",  asmstid, data.HRME_Id, data.ASMAY_Id, obj2.ASCAP_Id);
                        }
                        if (s == "success" && m == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                        //SMS sms = new SMS(_db);


                        ////   s = await sms.sendSms(data.MI_Id, mobileno, "TCSTATUS", asmstid);
                        //Email Email = new Email(_db);
                      //// m = Email.sendmail(data.MI_Id, emailid, "TCSTATUS", asmstid);
                        //if (s == "success" && m == "success")
                        //{
                        //    data.returnval = true;
                        //}
                        //else
                        //{
                        //    data.returnval = false;
                        //}
                        //data.returnval = true;
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

        public async Task<TransferCertificate_DTO> certificateRejected(TransferCertificate_DTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.ASCA_Id > 0)
                {
                    var update = _PortalContext.Adm_Students_Certificate_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASCA_Id == data.ASCA_Id).SingleOrDefault();

                    update.ASCA_Status = "Rejected";
                    update.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(update);

                    Adm_Students_Certificate_Approve_DMO obj2 = new Adm_Students_Certificate_Approve_DMO();

                    obj2.ASCA_Id = update.ASCA_Id;
                    obj2.MI_Id = data.MI_Id;
                    obj2.IVRMALU_Id = data.UserId;
                    obj2.ASCAP_Status = "Rejected";
                    obj2.ASCAP_ApproveReason = data.ASCAP_ApproveReason;
                    obj2.ASCAP_ApproveDate = DateTime.Now;
                    obj2.ASCAP_ActiveFlg = true;
                    obj2.CreatedDate = DateTime.Now;
                    obj2.UpdatedDate = DateTime.Now;

                    _PortalContext.Add(obj2);
                    _PortalContext.Update(update);
                    int rowAffected = _PortalContext.SaveChanges();               
                    if (rowAffected > 0)
                    {
                        var asmstid = _PortalContext.Adm_Students_Certificate_Apply_DMO.Single(a => a.ASCA_Id == data.ASCA_Id).AMST_Id;
                        var mobilemail = _PortalContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == asmstid && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S").Distinct().ToList();


                        long mobileno = 0;
                        var emailid = "";
                        if (mobilemail.Count > 0)
                        {
                            mobileno = mobilemail.FirstOrDefault().AMST_MobileNo;
                            SMS sms = new SMS(_db);
                            s = await sms.sendCertificateSms(data.MI_Id, mobileno, "TCSTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, obj2.ASCAP_Id);
                        }
                        if (mobilemail.Count > 0)
                        {
                            emailid = mobilemail.FirstOrDefault().AMST_emailId;
                            Email Email = new Email(_db);
                            m = Email.sendCertificatemail(data.MI_Id, emailid, "TCSTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, obj2.ASCAP_Id);
                        }
                        if (s == "success" && m == "success")
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


    }
}
