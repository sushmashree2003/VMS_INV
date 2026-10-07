using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class StudentTCDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<StudentTCDTO, StudentTCDTO> _tcdelegate = new CommonDelegate<StudentTCDTO, StudentTCDTO>();

        public StudentTCDTO LoadInitialData(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/getstudenttcdata/");
        }

        public StudentTCDTO getTcDetails(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/gettcDetails/");
        }

        public StudentTCDTO getActiveDetails(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/getactiveDetails/");
        }
        public StudentTCDTO getStatusDetails(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/getStatusDetails/");
        }
        public StudentTCDTO savedetails(StudentTCDTO org)
        {
            return _tcdelegate.POSTDataADM(org, "StudentTCFacade/savedetails/");
        }
        public StudentTCDTO chk_tc_dup(StudentTCDTO org)
        {
            return _tcdelegate.POSTDataADM(org, "StudentTCFacade/chk_tc_dup/");
        }
        public StudentTCDTO getstudent_name_list(StudentTCDTO name_list)
        {
            return _tcdelegate.POSTDataADM(name_list, "StudentTCFacade/getstudent_name_list/");
        }
        public StudentTCDTO saveOtherdetails(StudentTCDTO org)
        {
            return _tcdelegate.POSTDataADM(org, "StudentTCFacade/otherdetails/");
        }
        public StudentTCDTO getsearchfilter(StudentTCDTO enqdto)
        {
            return _tcdelegate.POSTDataADM(enqdto, "StudentTCFacade/searchfilter/");
        }

        public StudentTCDTO searchfilter(StudentTCDTO data)
        {
            return _tcdelegate.POSTDataADM(data, "StudentTCFacade/searchfilter/");
        }

        //public StudentTCDTO LoadInitialData(StudentTCDTO tcDto)
        //{
        //    StudentTCDTO studto = new StudentTCDTO();
        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(tcDto);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/getstudenttcdata", byteContent).Result;

        //        // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            studto = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return studto;
        //}










        //public StudentTCDTO getTcDetails(StudentTCDTO tcDto)
        //{
        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(tcDto);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/gettcDetails", byteContent).Result;

        //        // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            tcDto = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return tcDto;
        //}



        //public StudentTCDTO getActiveDetails(StudentTCDTO tcDto)
        //{
        //    StudentTCDTO studto = new StudentTCDTO();
        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(tcDto);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/getactiveDetails", byteContent).Result;

        //        // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            studto = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return studto;
        //}



        //public StudentTCDTO getStatusDetails(StudentTCDTO tcDto)
        //{
        //    StudentTCDTO studto = new StudentTCDTO();
        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(tcDto);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/getStatusDetails", byteContent).Result;

        //        // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            studto = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return studto;
        //}







        //public StudentTCDTO savedetails(StudentTCDTO org)
        //{
        //    string product = "";
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(org);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/savedetails/", byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            org = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return org;
        //}


        //public StudentTCDTO chk_tc_dup(StudentTCDTO org)
        //{
        //    string product = "";
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(org);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/chk_tc_dup", byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            org = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return org;
        //}


        //public StudentTCDTO getstudent_name_list(StudentTCDTO name_list)
        //{
        //    string product = "";
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(name_list);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/getstudent_name_list", byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            name_list = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return name_list;
        //}







        //public StudentTCDTO saveOtherdetails(StudentTCDTO org)
        //{
        //    string product = "";
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(org);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/otherdetails", byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            org = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return org;
        //}







        //public StudentTCDTO getLeftDetails(StudentTCDTO tcDto)
        //{
        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(tcDto);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/getleftDetails", byteContent).Result;

        //        // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            tcDto = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return tcDto;
        //}



        //public StudentTCDTO getDeactiveDetails(StudentTCDTO tcDto)
        //{
        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(tcDto);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/getdeactiveDetails", byteContent).Result;

        //        // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            tcDto = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return tcDto;
        //}

        //public StudentTCDTO getsearchfilter(StudentTCDTO enqdto)
        //{

        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(enqdto);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentTCFacade/searchfilter/", byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            enqdto = JsonConvert.DeserializeObject<StudentTCDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return enqdto;

        //}

    }
}
