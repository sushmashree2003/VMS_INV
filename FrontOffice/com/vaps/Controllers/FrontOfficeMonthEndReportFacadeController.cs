using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.BirthDay;
using FrontOfficeHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FrontOfficeMonthEndReportFacadeController : Controller
    {

        public FrontOfficeMonthEndReportInterface _inter;
        public FrontOfficeMonthEndReportFacadeController(FrontOfficeMonthEndReportInterface brth)
        {
            _inter = brth;
        }

        public Task<BirthDayDTO> getdata([FromBody] BirthDayDTO obj)
        {
            return _inter.getdata(obj);
        }

        [Route("getmonthreport")]
        public Task<BirthDayDTO> getmonthreport([FromBody] BirthDayDTO obj)
        {
            return _inter.getmonthreport(obj);
        }

        [Route("deleteduplicatepunch")]
        public Task<BirthDayDTO> deleteduplicatepunch([FromBody] BirthDayDTO obj)
        {
            return _inter.deleteduplicatepunch(obj);
        }

        [Route("getlunchreport")]
        public Task<BirthDayDTO> getlunchreport([FromBody] BirthDayDTO obj)
        {
            return _inter.getlunchreport(obj);
        }

        [Route("savereport")]
        public BirthDayDTO savereport([FromBody] BirthDayDTO obj)
        {
            return _inter.savereport(obj);
        }

        [Route("GetFinalReport")]
        public Task<BirthDayDTO> GetFinalReport([FromBody] BirthDayDTO obj)
        {
            return _inter.GetFinalReport(obj);
        }

        [Route("SendFinalMail")]
        public BirthDayDTO SendFinalMail([FromBody] BirthDayDTO obj)
        {
            return _inter.SendFinalMail(obj);
        }

        [Route("SendFinalMailMD")]
        public BirthDayDTO SendFinalMailMD([FromBody] BirthDayDTO obj)
        {
            return _inter.SendFinalMailMD(obj);
        }

        [Route("saveMDdata")]
        public BirthDayDTO saveMDdata([FromBody] BirthDayDTO obj)
        {
            return _inter.saveMDdata(obj);
        }

        [Route("sendmailAcc")]
        public BirthDayDTO sendmailAcc([FromBody] BirthDayDTO obj)
        {
            return _inter.sendmailAcc(obj);
        }

        [Route("Notapprovedstatus")]
        public BirthDayDTO Notapprovedstatus([FromBody] BirthDayDTO obj)
        {
            return _inter.Notapprovedstatus(obj);
        }
        [Route("DRWaveofReport")]
        public BirthDayDTO DRWaveofReport([FromBody] BirthDayDTO obj)
        {
            return _inter.DRWaveofReport(obj);
        }
        [Route("getdataapprover")]
        public BirthDayDTO getdataapprover([FromBody] BirthDayDTO obj)
        {
            return _inter.getdataapprover(obj);
        }
        [Route("getmonthreportforapproal")]
        public Task<BirthDayDTO> getmonthreportforapproal([FromBody] BirthDayDTO obj)
        {
            return _inter.getmonthreportforapproal(obj);
        }
        [Route("attendenceapprove")]
        public BirthDayDTO attendenceapprove([FromBody] BirthDayDTO obj)
        {
            return _inter.attendenceapprove(obj);
        }
        [Route("attendenceapprovedlist")]
        public BirthDayDTO attendenceapprovedlist([FromBody] BirthDayDTO obj)
        {
            return _inter.attendenceapprovedlist(obj);
        }

        //Unauthorized Leave Count Report

        [Route("getalldetails")]
        public BirthDayDTO getalldetails([FromBody] BirthDayDTO data)
        {
            return _inter.getdata1(data);
        }

        [Route("unauthorizedreport")]
        public BirthDayDTO unauthorizedreport([FromBody] BirthDayDTO data)
        {
            return _inter.unauthorizedreport(data);
        }

        [Route("getdepartment")]
        public BirthDayDTO getdepartment([FromBody] BirthDayDTO data)
        {
            return _inter.getdepartment(data);
        }

        //Montly Attandence Auto Mail Trigger For All Employee.
        [Route("MonthlyAutoMailTrigger")]
        public BirthDayDTO MonthlyAutoMailTrigger([FromBody] BirthDayDTO data)
        {
            return _inter.MonthlyAutoMailTrigger(data);
        }
        [Route("AdjustCompOff")]
        public BirthDayDTO AdjustCompOff([FromBody] BirthDayDTO data)
        {
            return _inter.AdjustCompOff(data);
        }
        [Route("iemonthendreport")]
        public BirthDayDTO iemonthendreport([FromBody] BirthDayDTO data)
        {
            return _inter.iemonthendreport(data);
        }

        [Route("onloadgetdetails")]
        public BirthDayDTO.LopDetails onloadgetdetails([FromBody]BirthDayDTO.LopDetails data)
        {
            return _inter.getBasicData(data);
        }
        [Route("Lopsave")]
        public BirthDayDTO.LopDetails Lopsave([FromBody]BirthDayDTO.LopDetails data)
        {
            return _inter.Lopsave(data);
        }

        [Route("deactivate")]
        public BirthDayDTO.LopDetails deactivate([FromBody]BirthDayDTO.LopDetails data)
        {
            return _inter.deactivate(data);
        }

        [Route("saveRc")]
        public BirthDayDTO saveRc([FromBody] BirthDayDTO obj)
        {
            return _inter.saveRc(obj);
        }
        [Route("proxydetails")]
        public BirthDayDTO proxydetails([FromBody] BirthDayDTO obj)
        {
            return _inter.proxydetails(obj);
        }
    }
}
