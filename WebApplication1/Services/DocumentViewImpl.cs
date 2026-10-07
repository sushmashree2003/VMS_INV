
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class DocumentViewImpl : Interfaces.DocumentViewInterface
    {
        //  private readonly AcademicContext _db;
        private static ConcurrentDictionary<string, DocumentViewDTO> _login =
       new ConcurrentDictionary<string, DocumentViewDTO>();
        public StudentApplicationContext _StudentApplicationContext;
        public AcademicContext _AcademicContext;
        ILogger<DocumentViewImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public FeeGroupContext _feecontext;
        public DocumentViewImpl(StudentApplicationContext StudentApplicationContext, FeeGroupContext feecontext, AcademicContext academiccontext, ILogger<DocumentViewImpl> acdimpl, DomainModelMsSqlServerContext db)
        {
            _StudentApplicationContext = StudentApplicationContext;
            _AcademicContext = academiccontext;
            _acdimpl = acdimpl;
            _db = db;
            _feecontext = feecontext;
        }

        public DocumentViewDTO getInitailData(DocumentViewDTO miid)
        {
            DocumentViewDTO ctdo = new DocumentViewDTO();
            try
            {
                ctdo.fillyear = _db.AcademicYear.Where(t => t.MI_Id == miid.mi_id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToArray();

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                //*****MAXMINAGE****
                //   allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == miid.mi_id && t.ASMCL_ActiveFlag == true).ToList();
                ctdo.admissioncatdrp = allclass.ToArray();

            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public DocumentViewDTO getDpData(DocumentViewDTO miid)
        {
            DocumentViewDTO ctdo = new DocumentViewDTO();
            List<DocumentViewDTO> alldocList = new List<DocumentViewDTO>();
            List<DocumentViewDTO> alldocListmain = new List<DocumentViewDTO>();
            try
            {

                List<AdmissionClass> allclass = new List<AdmissionClass>();
                //*****MAXMINAGE****
                //   allclass = await _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id.Equals(stu.MI_Id)).ToListAsync();
                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == miid.mi_id && t.ASMCL_ActiveFlag == true).ToList();
                ctdo.admissioncatdrp = allclass.ToArray();

                allclass = _StudentApplicationContext.AdmissionClass.Where(t => t.MI_Id == miid.mi_id && t.ASMCL_ActiveFlag == true).ToList();
                ctdo.admissioncatdrpall = allclass.ToArray();

                // flag ISPAC_ApplFeeFlag
                if (miid.configurationsettings == 1)
                {
                    List<StudentApplication> allRegStudentmain = new List<StudentApplication>();
                    //   List<StudentTrnxDoc> alldocList = new List<StudentTrnxDoc>();
                    List<StudentApplication> allRegStudent = new List<StudentApplication>();
                    if (miid.ASMCL_Id == 0)
                    {

                        allRegStudent = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id == miid.asmay_id).ToList();
                    }
                    else
                    {
                        allRegStudent = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id == miid.asmay_id && d.ASMCL_Id == miid.ASMCL_Id).ToList();
                    }

                    List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                    mstConfig = _StudentApplicationContext.masterConfig.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id.Equals(miid.asmay_id)).ToList();



                    if (mstConfig.FirstOrDefault().ISPAC_Healthapp == 0)
                    {
                        for (int i = 0; i < allRegStudent.Count; i++)
                        {
                            //payment checking 
                            miid.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R" && t.PASA_Id == allRegStudent[i].pasr_id).Count();
                            if (miid.payementcheck > 0)
                            {
                                allRegStudentmain.Add(allRegStudent[i]);
                                ctdo.registrationList = allRegStudentmain.ToArray();
                                ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R").ToArray();
                            }
                        }
                    }
                    else
                    {


                        if (miid.ASMCL_Id == 0)
                        {
                            ctdo.studentDetailsHelth = (from a in _StudentApplicationContext.StudentHelthcertificate
                                                        from b in _StudentApplicationContext.Enq
                                                        from c in _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                        where (a.PASR_Id == b.pasr_id && b.pasr_id == c.PASA_Id && b.MI_Id == miid.mi_id && c.FYPPA_Type == "R")
                                                        select new StudentHelthcertificateDTO
                                                        {
                                                            pasr_id = b.pasr_id,
                                                            PASR_FirstName = b.PASR_FirstName,
                                                            PASR_MiddleName = b.PASR_MiddleName,
                                                            PASR_LastName = b.PASR_LastName,
                                                            PASHD_Id = a.PASHD_Id,
                                                            pasR_RegistrationNo = b.PASR_RegistrationNo,
                                                            pasR_Student_Pic_Path = b.PASR_Student_Pic_Path,
                                                            ASMCL_Id = b.ASMCL_Id

                                                        }
        ).ToList().ToArray();
                        }
                        else
                        {
                            ctdo.studentDetailsHelth = (from a in _StudentApplicationContext.StudentHelthcertificate
                                                        from b in _StudentApplicationContext.Enq
                                                        from c in _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                        where (a.PASR_Id == b.pasr_id && b.pasr_id == c.PASA_Id && b.MI_Id == miid.mi_id && b.ASMCL_Id==miid.ASMCL_Id && c.FYPPA_Type == "R")
                                                        select new StudentHelthcertificateDTO
                                                        {
                                                            pasr_id = b.pasr_id,
                                                            PASR_FirstName = b.PASR_FirstName,
                                                            PASR_MiddleName = b.PASR_MiddleName,
                                                            PASR_LastName = b.PASR_LastName,
                                                            PASHD_Id = a.PASHD_Id,
                                                            pasR_RegistrationNo = b.PASR_RegistrationNo,
                                                            pasR_Student_Pic_Path = b.PASR_Student_Pic_Path,
                                                            ASMCL_Id = b.ASMCL_Id

                                                        }
        ).ToList().ToArray();
                        }
            

                        ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R").ToArray();

                        ctdo.registrationList = ctdo.studentDetailsHelth;
                    }
                }
                else
                {
                    List<StudentApplication> allRegStudentmain = new List<StudentApplication>();

                    List<StudentApplication> allRegStudent = new List<StudentApplication>();
                    if (miid.ASMCL_Id == 0)
                    {
                        allRegStudent = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id == miid.asmay_id).ToList();
                    }
                    else
                    {
                        allRegStudent = _StudentApplicationContext.Enq.Where(d => d.MI_Id.Equals(miid.mi_id) && d.ASMAY_Id == miid.asmay_id && d.ASMCL_Id == miid.ASMCL_Id).ToList();
                    }


                    for (int i = 0; i < allRegStudent.Count; i++)
                    {
                        allRegStudentmain.Add(allRegStudent[i]);
                        ctdo.registrationList = allRegStudentmain.ToArray();
                        ctdo.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "R").ToArray();
                    }
                }



                ctdo.ddoc = (from a in _StudentApplicationContext.trxDoc
                             from b in _StudentApplicationContext.MasterDocumentDMO
                             from c in _StudentApplicationContext.Enq
                             where (a.AMSMD_Id == b.AMSMD_Id && a.PASR_Id == c.pasr_id && c.MI_Id == miid.mi_id)
                             select new DocumentViewDTO
                             {
                                 pasr_id = c.pasr_id,
                                 PASRD_Id = a.PASRD_Id,
                                 Document_Path = a.Document_Path,
                                 AMSMD_DocumentName = b.AMSMD_DocumentName,
                                 AMSMD_Id = a.AMSMD_Id
                             }
                      ).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }





    }
}
