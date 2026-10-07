using System;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class AdmissionStandardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AdmissionStandardDTO, AdmissionStandardDTO> COMMM = new CommonDelegate<AdmissionStandardDTO, AdmissionStandardDTO>();
        public AdmissionStandardDTO getlisttwo(AdmissionStandardDTO student)
        {
            return COMMM.POSTDataADM(student, "AdmissionStandardFacade/savedata/");

            //AdmissionStandardDTO ads = null;
            //string product;
            //Array[] dropDownArray = new Array[2];
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:53497/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(student);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.PostAsync("api/AdmissionStandardFacade/savedata/", byteContent).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;

            //        ads = JsonConvert.DeserializeObject<AdmissionStandardDTO>(product, new JsonSerializerSettings
            //        {
            //            TypeNameHandling = TypeNameHandling.Objects
            //        });
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            //// return output;
            //return ads;
        }


        public AdmissionStandardDTO getlistdata(int id)
        {

            return COMMM.GetDataByIdADM(id, "AdmissionStandardFacade/loaddata/");

            //AdmissionStandardDTO ads = null;
            //string product;
            //Array[] dropDownArray = new Array[2];
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:53497/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(id);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.GetAsync("api/AdmissionStandardFacade/loaddata/" + id).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;

            //        ads = JsonConvert.DeserializeObject<AdmissionStandardDTO>(product, new JsonSerializerSettings
            //        {
            //            TypeNameHandling = TypeNameHandling.Objects
            //        });
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            //// return output;
            //return ads;
        }



    }
}
