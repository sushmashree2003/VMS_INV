using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class EnquiryDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Enq, Enq> COMMM = new CommonDelegate<Enq, Enq>();
        CommonDelegate<StateDTO, StateDTO> COMMMM = new CommonDelegate<StateDTO, StateDTO>();
        CommonDelegate<CityDTO, CityDTO> COMM = new CommonDelegate<CityDTO, CityDTO>();
        
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/PreadmissionFacade/" + resource).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("{0}\t${1}\t{2}", product);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return product;
        }

        public string enqdata(Enq en)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                //var pairs = new List<KeyValuePair<string, string>>
                //{
                //    new KeyValuePair<string, string>("username", lo.username ),
                //    new KeyValuePair<string, string>("password", lo.password )
                //};
                //var content = new FormUrlEncodedContent(pairs);
                //var response = client.PostAsJsonAsync("api/PreadmissionFacade/", content).Result;

                var myContent = JsonConvert.SerializeObject(en);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/EnquiryFacade/", byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }
            return "";
        }

        public Enq getcountrydata(Enq en)
        {
            return COMMM.POSTData(en, "EnquiryFacade/loaddata/");
        }

        public StateDTO enqdatacountrydrp(int id)
        {
            return COMMMM.GetDataById(id, "EnquiryFacade/getenquirycontroller/");
        }

        public Enq saveEnqdetails(Enq en)
        {
            return COMMM.POSTData(en, "EnquiryFacade/");
        }

        public Enq clearEnqdata(int id)
        {
            return COMMM.GetDataById(id, "EnquiryFacade/getenquirycontroller/");
        }
        public CityDTO cityfill(int id)
        {
            return COMM.GetDataById(id, "EnquiryFacade/getenquirystatecontroller/");
        }
        public Enq Editdetailss(int id)
        {
            return COMMM.GetDataById(id, "EnquiryFacade/GetEnqdetails/");

        }
        public Enq DeleteEnqrecord(Enq en)
        {
            return COMMM.POSTData(en, "EnquiryFacade/DeleteDetails/");
        }

    }
}















































