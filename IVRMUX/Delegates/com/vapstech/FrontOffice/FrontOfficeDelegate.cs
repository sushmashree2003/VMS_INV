using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.BirthDay;
//using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class FrontOfficeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<BirthDayDTO, BirthDayDTO> COMFRNT = new CommonDelegate<BirthDayDTO, BirthDayDTO>();
        CommonDelegate<BirthDayDTO.LopDetails, BirthDayDTO.LopDetails> COMFRNT1 = new CommonDelegate<BirthDayDTO.LopDetails, BirthDayDTO.LopDetails>();


        //public BirthDayDTO getdata(int id)
        //{
        //    BirthDayDTO returnData = null;
        //    string product;
        //    HttpClient client = new HttpClient();

        //    client.BaseAddress = new Uri("http://localhost:50642/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(id);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.GetAsync("api/FrontOfficeMonthEndReportFacade/" + id).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            returnData = JsonConvert.DeserializeObject<BirthDayDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects,
        //                DateFormatHandling = DateFormatHandling.IsoDateFormat
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return returnData;
        //}

        public BirthDayDTO getdata(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/");
        }


        public BirthDayDTO getmonthreport(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/getmonthreport/");
        }

        public BirthDayDTO getlunchreport(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/getlunchreport/");
        }

        public BirthDayDTO savereport(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/savereport/");
        }

        public BirthDayDTO GetFinalReport(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/GetFinalReport/");
        }

        public BirthDayDTO SendFinalMail(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/SendFinalMail/");
        }

        public BirthDayDTO SendFinalMailMD(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/SendFinalMailMD/");
        }

        public BirthDayDTO saveMDdata(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/saveMDdata/");
        }

        public BirthDayDTO sendmailAcc(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/sendmailAcc/");
        }
        public BirthDayDTO Notapprovedstatus(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/Notapprovedstatus/");
        }
        public BirthDayDTO DRWaveofReport(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/DRWaveofReport/");
        }
        public BirthDayDTO getdataapprover(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/getdataapprover/");
        }
        public BirthDayDTO getmonthreportforapproal(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/getmonthreportforapproal/");
        }
        public BirthDayDTO attendenceapprove(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/attendenceapprove/");
        }

        public BirthDayDTO attendenceapprovedlist(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/attendenceapprovedlist/");
        }

        //Unauthorized Leave Count Report
        public BirthDayDTO getdata1(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/getalldetails/");
        }

        public BirthDayDTO unauthorizedreport(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/unauthorizedreport/");
        }

        public BirthDayDTO getdepartment(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/getdepartment/");
        }
        public BirthDayDTO AdjustCompOff(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/AdjustCompOff/");
        }
        public BirthDayDTO iemonthendreport(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/iemonthendreport/");
        }
        public BirthDayDTO.LopDetails onloadgetdetails(BirthDayDTO.LopDetails dto)
        {
            return COMFRNT1.POSTDataHolidayReport(dto, "FrontOfficeMonthEndReportFacade/onloadgetdetails");
        }
        public BirthDayDTO.LopDetails Lopsave(BirthDayDTO.LopDetails data)
        {
            return COMFRNT1.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/Lopsave");
        }
        public BirthDayDTO.LopDetails deactivate(BirthDayDTO.LopDetails data)
        {
            return COMFRNT1.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/deactivate");
        }
        public BirthDayDTO saveRc(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/saveRc/");
        }
        public BirthDayDTO proxydetails(BirthDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "FrontOfficeMonthEndReportFacade/proxydetails/");
        }
        
    }
}

