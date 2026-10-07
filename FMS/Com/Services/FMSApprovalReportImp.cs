using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Services
{
    public class FMSApprovalReportImp : Interface.FMSApprovalReportInterface
    {
        public FMSContext _context;
        public FMSApprovalReportImp(FMSContext _cont)
        {
            _context = _cont;
        }
        public FMSApprovalReportDTO loaddata(FMSApprovalReportDTO data)
        {
            try
            {

                data.filename = _context.FMS_Master_PhysicalFileNameDMO.Where(R => R.FMSMPFN_ActiveFlg == true ).Distinct().ToArray();
                data.institution = _context.Institution.Distinct().ToArray();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FMSApprovalReportDTO report(FMSApprovalReportDTO data)
        {
            try
            {
                if (data.grid_arraydatas != null && data.grid_arraydatas.Length > 0)
                {           
                    List<long> fnameid = new List<long>();
                    foreach (var c in data.grid_arraydatas)
                    {
                        fnameid.Add(c.fnid);
                    }

                    List<long> MIId = new List<long>();
                    foreach (var c in data.grid_mid)
                    {
                        MIId.Add(c.Id);
                    }
                    
                    data.griddata = (from a in _context.FMSApprovalRequestPageDMO
                                         from b in _context.FMSApprovalDMO
                                         from c in _context.FMS_Master_PhysicalFileNameDMO
                                         from d in _context.Institution
                                         from e in _context.ApplicationUser
                                         from f in _context.MasterEmployee
                                         where (MIId.Contains(c.MI_Id) && MIId.Contains(d.MI_Id) && b.FMSFLREQ_Id == a.FMSFLREQ_Id && c.FMSMPFN_Id == a.FMSMPFN_Id && fnameid.Contains(c.FMSMPFN_Id) && e.Id == b.FMSFLREQAPP_UpdatedBy && f.HRME_Id == a.HRME_Id && a.FMSFLREQ_RequestedDateTime >= data.fromdate && a.FMSFLREQ_RequestedDateTime <= data.todate && a.FMSFLREQ_ApprovedFlg == data.FMSFLREQ_ApprovedFlg)
                                         select new FMSApprovalReportDTO
                                         {
                                             physicalname = c.FMSMPFN_PhysicalFileName,
                                             requireddayss = a.FMSFLREQ_RequiredDays,
                                             requesteddate = a.FMSFLREQ_RequestedDateTime,
                                             approveddayss = b.FMSFLREQAPP_ApprovedDays,
                                             fmsmpfn_id = c.FMSMPFN_Id,
                                             createdby = (f.HRME_EmployeeFirstName + f.HRME_EmployeeMiddleName + f.HRME_EmployeeLastName),
                                             approvedby = e.NormalizedUserName,
                                             createdbyid = f.HRME_Id,
                                             approvedbyid = e.Id,
                                             approveddate = b.FMSFLREQAPP_UpdatedDate,
                                             approvedflag = a.FMSFLREQ_ApprovedFlg,
                                             FMSFLREQ_Id=a.FMSFLREQ_Id,
                                         }).Distinct().ToArray();

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
