using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class InstitutionDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<InstitutionDTO, InstitutionDTO> COMMM = new CommonDelegate<InstitutionDTO, InstitutionDTO>();
        CommonDelegate<Master_Institution_SubscriptionValidityDTO, Master_Institution_SubscriptionValidityDTO> COMMMM = new CommonDelegate<Master_Institution_SubscriptionValidityDTO, Master_Institution_SubscriptionValidityDTO>();
        CommonDelegate<Master_Institution_SubscriptionValidityDTO, SortingPagingInfoDTO> COO = new CommonDelegate<Master_Institution_SubscriptionValidityDTO, SortingPagingInfoDTO>();
        CommonDelegate<InstitutionDTO, SortingPagingInfoDTO> CO = new CommonDelegate<InstitutionDTO, SortingPagingInfoDTO>();
        public InstitutionDTO saveInstitutiondetails(InstitutionDTO instute)
        {
            return COMMM.POSTData(instute, "InstitutionFacade/");
        }

        public InstitutionDTO getInstitutiondata(InstitutionDTO Instidto)
        {
            return COMMM.POSTData(Instidto, "InstitutionFacade/getAllDetails");
            
        }

        //by id

        public InstitutionDTO getInstitutionDetailsbyInstituteId(int InstuteId)
        {
            return COMMM.GetDataById(InstuteId, "InstitutionFacade/getdetailsById/");
        }

        //delete record
        public InstitutionDTO deleterec(int id)
        {
            return COMMM.DeleteDataById(id, "InstitutionFacade/deletedetails/");
            
        }

         public InstitutionDTO DuplicateInstitionDataFind(InstitutionDTO instute)
        {
            return COMMM.POSTData(instute, "InstitutionFacade/DuplicateDataFind");
        }



        public Master_Institution_SubscriptionValidityDTO SaveSubscriptionValidity(Master_Institution_SubscriptionValidityDTO instute)
        {

            return COMMMM.POSTData(instute, "InstitutionFacade/SaveSubscriptionValidity/");
        }


        //delete subscription

        public Master_Institution_SubscriptionValidityDTO deleteSubscriptionrec(int id)
        {
            return COMMMM.DeleteDataById(id, "InstitutionFacade/deleteSubscriptiondetails/");
        }




        //for search and pagination

        public InstitutionDTO getInstitutionSearchedDetails(SortingPagingInfoDTO searchdata)
        {

            return CO.POSTDataa(searchdata, "InstitutionFacade/getInstitutionSearchedDetails/");

            //InstitutionDTO instute = new InstitutionDTO();
            //string product = "";
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:65140/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(searchdata);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.PostAsync("api/InstitutionFacade/getInstitutionSearchedDetails/", byteContent).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        Console.WriteLine("", product);
            //        instute = JsonConvert.DeserializeObject<InstitutionDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //return instute;
        }

        public Master_Institution_SubscriptionValidityDTO getSubscriptionSearchedDetails(SortingPagingInfoDTO searchdata)
        {

            return COO.POSTDataa(searchdata, "InstitutionFacade/getSubscriptionSearchedDetails/");

            //Master_Institution_SubscriptionValidityDTO instute = new Master_Institution_SubscriptionValidityDTO();
            //string product = "";
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:65140/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(searchdata);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.PostAsync("api/InstitutionFacade/getSubscriptionSearchedDetails/", byteContent).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        Console.WriteLine("", product);
            //        instute = JsonConvert.DeserializeObject<Master_Institution_SubscriptionValidityDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //return instute;
        }
    }
}
