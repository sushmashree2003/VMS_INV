using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class StudentAttendanceReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();
        public StudentAttendanceReportDelegate() { }

        CommonDelegate<StudentAttendanceReportDTO, StudentAttendanceReportDTO> comm = new CommonDelegate<StudentAttendanceReportDTO, StudentAttendanceReportDTO>();
        public StudentAttendanceReportDelegate(FacadeUrl config)
        {
            _config = config;
            fdu = config;
        }
        public StudentAttendanceReportDTO getDataBySelectedType(int id)
        {
            StudentAttendanceReportDTO enqdto = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StudentAttendanceReportFacade/getdatabyselectedtype/" + id).Result;
                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<StudentAttendanceReportDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public StudentAttendanceReportDTO getinitialdata(StudentAttendanceReportDTO orgdet)
        {
            StudentAttendanceReportDTO stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(orgdet);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentAttendanceReportFacade/getinitialdata/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<StudentAttendanceReportDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public StudentAttendanceReportDTO getserdata(StudentAttendanceReportDTO data)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentAttendanceReportFacade/searchdata/", byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<StudentAttendanceReportDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return data;
        }
        public StudentAttendanceReportDTO getdatatype(StudentAttendanceReportDTO data)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentAttendanceReportFacade/getdatatype/", byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<StudentAttendanceReportDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return data;
        }

        public StudentAttendanceReportDTO getreportdiv(StudentAttendanceReportDTO data)
        {
            return comm.POSTDataaADM(data,"StudentAttendanceReportFacade/getreportdiv");           
        }
        public StudentAttendanceReportDTO savetmpldatanew(StudentAttendanceReportDTO data)
        {
            return comm.POSTDataaADM(data, "StudentAttendanceReportFacade/savetmpldatanew");
        }
        public StudentAttendanceReportDTO onchangeyear(StudentAttendanceReportDTO data)
        {
            return comm.POSTDataaADM(data, "StudentAttendanceReportFacade/onchangeyear");
        }
        public StudentAttendanceReportDTO onclasschange(StudentAttendanceReportDTO data)
        {
            return comm.POSTDataaADM(data, "StudentAttendanceReportFacade/onclasschange");
        }
        public StudentAttendanceReportDTO onsectionchange(StudentAttendanceReportDTO data)
        {
            return comm.POSTDataaADM(data, "StudentAttendanceReportFacade/onsectionchange");
        }
        public StudentAttendanceReportDTO getreport(StudentAttendanceReportDTO data)
        {
            return comm.POSTDataaADM(data, "StudentAttendanceReportFacade/getreport");
        }
        
    }

}
