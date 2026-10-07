using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class SalesLeadImportDelegate
    {
    
        CommonVMSDelegate<SalesLeadImportDTO, SalesLeadImportDTO> COMMLD = new CommonVMSDelegate<SalesLeadImportDTO, SalesLeadImportDTO>();

        //public SalesLeadImportDTO saveadvance(SalesLeadImportDTO dto)
        //{
        //    //HttpClient client = new HttpClient();
        //    //// Set a longer timeout
        //    //client.Timeout = TimeSpan.FromMinutes(11);  // Increase the timeout
        //    return COMMLD.POSTData(dto, "SalesLeadImportFacade/saveadvance/");
        //}

        public async Task<SalesLeadImportDTO> saveadvance(SalesLeadImportDTO rtnData)
        {
            SalesLeadImportDTO enqdto = new SalesLeadImportDTO();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:18123/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Set a longer timeout
                    client.Timeout = TimeSpan.FromMinutes(11);  // Increase the timeout

                    // Serialize the object to JSON
                    var myContent = JsonConvert.SerializeObject(rtnData);
                    var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(myContent));
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri("http://localhost:18123/api/SalesLeadImportFacade/saveadvance/");
                        request.Content = byteContent;

                        // Send the request and await the response
                        HttpResponseMessage response = await client.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            // Read the response and deserialize the object
                            string responsedata = await response.Content.ReadAsStringAsync();
                            enqdto = JsonConvert.DeserializeObject<SalesLeadImportDTO>(responsedata, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                }
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                Console.WriteLine("Request timed out.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return enqdto;
        }

        public SalesLeadImportDTO saveRawdataLead(SalesLeadImportDTO dto)
        {
            //HttpClient client = new HttpClient();
            //// Set a longer timeout
            //client.Timeout = TimeSpan.FromMinutes(11);  // Increase the timeout
            return COMMLD.POSTData(dto, "SalesLeadImportFacade/saveRawdataLead/");
        }


    }
}
