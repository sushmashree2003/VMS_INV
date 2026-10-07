using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
   public interface FrontOfficeMonthEndReportInterface
    {
        Task<BirthDayDTO> getdata(BirthDayDTO data);
        Task<BirthDayDTO> getmonthreport(BirthDayDTO data);
        Task<BirthDayDTO> deleteduplicatepunch(BirthDayDTO data);
        Task<BirthDayDTO> getlunchreport(BirthDayDTO data);
        BirthDayDTO savereport(BirthDayDTO data);
        Task<BirthDayDTO> GetFinalReport(BirthDayDTO data);
        BirthDayDTO SendFinalMail(BirthDayDTO data);
        BirthDayDTO SendFinalMailMD(BirthDayDTO data);
        BirthDayDTO saveMDdata(BirthDayDTO data);
        BirthDayDTO sendmailAcc(BirthDayDTO data);
        BirthDayDTO Notapprovedstatus(BirthDayDTO data);
        BirthDayDTO DRWaveofReport(BirthDayDTO data);
        BirthDayDTO getdataapprover(BirthDayDTO data);
        Task<BirthDayDTO> getmonthreportforapproal(BirthDayDTO data);
        BirthDayDTO attendenceapprove(BirthDayDTO data);
        BirthDayDTO attendenceapprovedlist(BirthDayDTO data);

        //Unauthorized Leave Count Report
        BirthDayDTO getdata1(BirthDayDTO data);

        BirthDayDTO unauthorizedreport(BirthDayDTO data);

        BirthDayDTO getdepartment(BirthDayDTO data);
        BirthDayDTO MonthlyAutoMailTrigger(BirthDayDTO data);
        BirthDayDTO AdjustCompOff(BirthDayDTO data);
        BirthDayDTO iemonthendreport(BirthDayDTO data);
        BirthDayDTO.LopDetails getBasicData(BirthDayDTO.LopDetails dto);
        BirthDayDTO.LopDetails Lopsave(BirthDayDTO.LopDetails dto);
        BirthDayDTO.LopDetails deactivate(BirthDayDTO.LopDetails dto);
        BirthDayDTO saveRc(BirthDayDTO data);
        BirthDayDTO proxydetails(BirthDayDTO data);
        
    }
}
