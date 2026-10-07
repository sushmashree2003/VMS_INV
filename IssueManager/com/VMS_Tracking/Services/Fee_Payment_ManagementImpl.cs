using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IssueManager.com.VMS_Tracking.Services
{
    public class Fee_Payment_ManagementImpl: Interface.Fee_Payment_ManagementInterface
    {
        public IssueManagerContext _context;
        public Fee_Payment_ManagementImpl(IssueManagerContext context)
        {
            _context = context;
        }

       public  Fee_Payment_ManagementDTO getloaddata(Fee_Payment_ManagementDTO dto)
        {
            try
            {
                dto.clientlist = _context.clientTable.Where(a => a.MI_Id == dto.MI_Id && a.ISMMCLT_ActiveFlag == true).Distinct().ToArray();
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Fee_Payment_ManagementDTO getreport(Fee_Payment_ManagementDTO data)
        {
            try
            {
                var geturl = _context.clientTable.Where(a => a.ISMMCLT_Id == data.ISMMCLT_Id).ToList();

                if (geturl.Count > 0)
                {
                    string url = geturl.FirstOrDefault().ISMMCLT_IVRM_URL;

                    Fee_Payment_ManagementDTO dtonew = new Fee_Payment_ManagementDTO();

                    dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                    dtonew.fromdate = data.fromdate;
                    dtonew.todate = data.todate;
                    dtonew.termswise = data.termswise;
                    // url = "http://localhost:51263/";
                    if (url != null && url != "")
                    {
                        HttpClient client1 = new HttpClient();
                        client1.BaseAddress = new Uri(url);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client1.PostAsJsonAsync("api/Fee_PaymentManagementFacade/getFee_PaymentManagement", dtonew).Result;

                        string description = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            description = result;

                            dtonew = JsonConvert.DeserializeObject<Fee_Payment_ManagementDTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                            data.getreportdetails = dtonew.getreportdetails;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    

    }
}
