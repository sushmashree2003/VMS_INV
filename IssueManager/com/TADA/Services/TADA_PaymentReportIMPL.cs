using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Services
{
    public class TADA_PaymentReportIMPL : Interface.TADA_PaymentReportInterface
    {

        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;

        public TADA_PaymentReportIMPL(IssueManagerContext context)
        {
            _context = context;
        }

        public TADA_PaymentReportDTO getalldata(TADA_PaymentReportDTO data)
        {
            try
            {               
                data.get_emp = (from a in _context.MasterEmployee
                               
                                where (a.HRME_ActiveFlag == true && a.MI_Id==data.MI_Id
                                 )
                                select new TADA_PaymentReportDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                                }
                     ).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TADA_PaymentReportDTO GetReport(TADA_PaymentReportDTO data)
        {
            try
            {
                data.MI_Logo = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && a.MI_Id == data.MI_Id).Select(R => R.MI_Logo).FirstOrDefault();
                List<long> HRME_ids = new List<long>();
                foreach (var item in data.HRME_Id_List)
                {
                    HRME_ids.Add(item.HRME_IdS);
                }
                if (data.RDO_PUNCH == "TA_ApplyPayment")
                {
                    
                  

                        data.GetReport = (from p in _context.TADAApply_PaymentDMO
                                         from Q in _context.TADA_ApplyDMO
                                         from t in _context.MasterEmployee
                                         from u in _context.IVRM_Master_CityDMO
                                          from e in _context.Institution
                                          where (p.VTADAAP_ActiveFlg == true && HRME_ids.Contains(Q.HRME_Id)   && Q.MI_Id == e.MI_Id && e.MI_ActiveFlag == 1 && p.VTADAA_Id == Q.VTADAA_Id && Q.IVRMMCT_Id == u.IVRMMCT_Id && Q.HRME_Id==t.HRME_Id && Q.MI_Id==data.MI_Id &&
                              (p.VTADAAP_Date >= data.FromDate && p.VTADAAP_Date <= data.ToDate))
                                          select new TADAApply_PaymentDTO
                                         {
                                             HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),

                                             VTADAAP_AmountPaid = p.VTADAAP_AmountPaid,
                                             VTADAAP_Date = p.VTADAAP_Date,
                                             VTADAAP_PaymentMode = p.VTADAAP_PaymentMode,                                            
                                             IVRMMCT_Name = u.IVRMMCT_Name,
                                              MI_Name = e.MI_Name,
                                              HRME_EmployeeCode = t.HRME_EmployeeCode
                                          }).Distinct().ToArray();
                    
                }
                if (data.RDO_PUNCH == "TA_Apply_AdavancePayment")
                {
                   
                        data.GetReport = (from p in _context.TADA_AdavanceApply_PaymentDMO
                                          from Q in _context.TADA_AdavanceApplyDMO
                                          from t in _context.MasterEmployee
                                          from u in _context.IVRM_Master_CityDMO
                                          from e in _context.Institution
                                          where (p.VTADAAAP_ActiveFlg == true && HRME_ids.Contains(Q.HRME_Id) && Q.MI_Id == e.MI_Id && e.MI_ActiveFlag == 1 && p.VTADAAA_Id == Q.VTADAAA_Id && Q.IVRMMCT_Id == u.IVRMMCT_Id && Q.HRME_Id == t.HRME_Id && Q.MI_Id==data.MI_Id
                                          &&
                              (p.VTADAAAP_Date >= data.FromDate && p.VTADAAAP_Date <= data.ToDate))
                                          select new TADA_AdavanceApply_PaymentDTO
                                          {
                                              HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),

                                              VTADAAAP_AmountPaid = p.VTADAAAP_AmountPaid,
                                              VTADAAAP_Date = p.VTADAAAP_Date,
                                              VTADAAAP_PaymentMode = p.VTADAAAP_PaymentMode,

                                              IVRMMCT_Name = u.IVRMMCT_Name,

                                              MI_Name = e.MI_Name,
                                              HRME_EmployeeCode = t.HRME_EmployeeCode
                                          }).Distinct().ToArray();
                    
                }
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
