
using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Exam
{
    public class MarksEntryDelegates
    {


        CommonDelegate<ExamMarksDTO, ExamMarksDTO> COMMM = new CommonDelegate<ExamMarksDTO, ExamMarksDTO>();

        //public MasterSubjectGroupDTO savedetail(MasterSubjectGroupDTO categorypage)
        //{
        //    string product;
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:50257/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(categorypage);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/MasterSubjectGroupFacade/savedetail", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            categorypage = JsonConvert.DeserializeObject<MasterSubjectGroupDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return categorypage;
        //}


        //public MasterSubjectGroupDTO deactivate(MasterSubjectGroupDTO categorypage)
        //{
        //    string product;
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:50257/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(categorypage);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/MasterSubjectGroupFacade/deactivate", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            categorypage = JsonConvert.DeserializeObject<MasterSubjectGroupDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return categorypage;
        //}



        public ExamMarksDTO getdetails(ExamMarksDTO id)
        {

            return COMMM.POSTDataExam(id, "MarksEntryFacade/Getdetails/");

            //ExamMarksDTO orgdet = null;
            //string product;
            //Array[] dropDownArray = new Array[2];
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:50257/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(id);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.GetAsync("api/MasterSubjectGroupFacade/getdetails/" + id).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;

            //        orgdet = JsonConvert.DeserializeObject<ExamMarksDTO>(product, new JsonSerializerSettings
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
            //return orgdet;
        }

        public ExamMarksDTO onselectAcdYear(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectAcdYear/");
        }
        public ExamMarksDTO onselectclass(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectclass/");
        }
        public ExamMarksDTO onselectSection(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectSection/");
        }
        public ExamMarksDTO onselectExam(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectExam/");
        }
        public ExamMarksDTO onsearch(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onsearch/");
        }
        public ExamMarksDTO SaveMarks(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/SaveMarks/");
        }


    }
}
