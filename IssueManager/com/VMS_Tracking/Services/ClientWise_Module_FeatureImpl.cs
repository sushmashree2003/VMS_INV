using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IssueManager.com.VMS_Tracking.Services
{
    public class ClientWise_Module_FeatureImpl : Interface.ClientWise_Module_Feature_Inrerface
    {
        public IssueManagerContext _context;
        public ClientWise_Module_FeatureImpl(IssueManagerContext context)
        {
            _context = context;
        }

        public ClientWise_Module_Feature_DTO getloaddata(ClientWise_Module_Feature_DTO dto)
        {
            try
            {
                dto.clientlist = _context.clientTable.Where(a => a.MI_Id == dto.MI_Id && a.ISMMCLT_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public async Task<ClientWise_Module_Feature_DTO> getmoduleAsync(ClientWise_Module_Feature_DTO dto)
        {
            try
            {
                ClientWise_Module_Feature_DTO newclt = new ClientWise_Module_Feature_DTO();
                var geturl = _context.clientTable.Where(a => a.ISMMCLT_Id == dto.ISMMCLT_Id).ToList();

                if (geturl.Count > 0)
                {
                    string url = geturl.FirstOrDefault().ISMMCLT_IVRM_URL;

                    ClientWise_Module_Feature_DTO dtonew = new ClientWise_Module_Feature_DTO();

                    dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);



                    // url = "http://localhost:51263/";
                    if (url != null && url != "")
                    {
                        //Uri requestUri = new Uri(url + "api/ClientWise_Module_FeatureFacade/getmodule");
                        //dynamic dynamicJson = new ExpandoObject();
                        //dynamicJson.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        //string json = "";
                        //json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
                        //var objClint = new System.Net.Http.HttpClient();
                        //System.Net.Http.HttpResponseMessage respon = await objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                        //string responJsonText = await respon.Content.ReadAsStringAsync();
                        //abc clsobj = Newtonsoft.Json.JsonConvert.DeserializeObject<abc>(responJsonText);
                        //dto.modulelist = clsobj.modulelist.ToList();




                        HttpClient client1 = new HttpClient();
                        client1.BaseAddress = new Uri(url);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client1.PostAsJsonAsync("api/ClientWise_Module_FeatureFacade/getmodule", dtonew).Result;

                        string description = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {



                            string result = response.Content.ReadAsStringAsync().Result;
                            //description = result;

                            //dtonew = JsonConvert.DeserializeObject<ClientWise_Module_Feature_DTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                            abc clsobj = Newtonsoft.Json.JsonConvert.DeserializeObject<abc>(result);

                            dto.modulelist = clsobj.modulelist.ToList();

                            //dto.modulelist = dtonew.modulelist;
                        }
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ClientWise_Module_Feature_DTO getreport(ClientWise_Module_Feature_DTO data)
        {
            try
            {
                var geturl = _context.clientTable.Where(a => a.ISMMCLT_Id == data.ISMMCLT_Id).ToList();

                if (geturl.Count > 0)
                {
                    string url = geturl.FirstOrDefault().ISMMCLT_IVRM_URL;

                    ClientWise_Module_Feature_DTO dtonew = new ClientWise_Module_Feature_DTO();

                    if (data.moduleflage == "Birthday" || data.moduleflage == "COE")
                    {
                        dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        dtonew.IVRMM_Id = data.IVRMM_Id;
                        dtonew.Type = data.Type;
                        dtonew.Flag = data.Flag;
                        dtonew.moduleflage = data.moduleflage;
                    }
                    else if (data.moduleflage == "Admission")
                    {
                        dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        dtonew.IVRMM_Id = data.IVRMM_Id;
                        dtonew.fromdate = data.fromdate;
                        dtonew.todate = data.todate;
                        dtonew.moduleflage = data.moduleflage;
                    }
                    else if (data.moduleflage == "VM")
                    {
                        dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        dtonew.IVRMM_Id = data.IVRMM_Id;
                        dtonew.fromdate = data.fromdate;
                        dtonew.todate = data.todate;
                        dtonew.moduleflage = data.moduleflage;
                    }
                    else if (data.moduleflage == "Library")
                    {
                        dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        dtonew.IVRMM_Id = data.IVRMM_Id;
                        dtonew.fromdate = data.fromdate;
                        dtonew.todate = data.todate;
                        dtonew.moduleflage = data.moduleflage;
                    }
                    else if (data.moduleflage == "Preadmission")
                    {
                        dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        dtonew.IVRMM_Id = data.IVRMM_Id;
                        dtonew.fromdate = data.fromdate;
                        dtonew.todate = data.todate;
                        dtonew.moduleflage = data.moduleflage;
                    }
                    else if (data.moduleflage == "Exam")
                    {
                        dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        dtonew.IVRMM_Id = data.IVRMM_Id;
                        dtonew.fromdate = data.fromdate;
                        dtonew.todate = data.todate;
                        dtonew.moduleflage = data.moduleflage;
                    }
                    else if (data.moduleflage == "Fees")
                    {
                        dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        dtonew.IVRMM_Id = data.IVRMM_Id;
                        dtonew.fromdate = data.fromdate;
                        dtonew.todate = data.todate;
                        dtonew.moduleflage = data.moduleflage;
                    }
                    else if (data.moduleflage == "Transport")
                    {
                        dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                        dtonew.IVRMM_Id = data.IVRMM_Id;
                        dtonew.fromdate = data.fromdate;
                        dtonew.todate = data.todate;
                        dtonew.moduleflage = data.moduleflage;
                    }
                    if (url != null && url != "")
                    {
                        HttpClient client1 = new HttpClient();
                        client1.BaseAddress = new Uri(url);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client1.PostAsJsonAsync("api/ClientWise_Module_FeatureFacade/getreport", dtonew).Result;

                        string description = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            description = result;

                            dtonew = JsonConvert.DeserializeObject<ClientWise_Module_Feature_DTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                            data.getreportdetails = dtonew.getreportdetails;
                            data.getmodulename = dtonew.getmodulename;
                        }
                    }
                    //if (url != null && url != "")
                    //{
                    //    Uri requestUri = new Uri(url + "api/ClientWise_Module_FeatureFacade/getreport");
                    //    dynamic dynamicJson = new ExpandoObject();
                    //    dynamicJson.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);

                    //    string json = "";
                    //    json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

                    //    var objClint = new System.Net.Http.HttpClient();
                    //    System.Net.Http.HttpResponseMessage respon = await objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //    string responJsonText = await respon.Content.ReadAsStringAsync();
                    //    abc clsobj = Newtonsoft.Json.JsonConvert.DeserializeObject<abc>(responJsonText);


                    //    data.getreportdetails = dtonew.getreportdetails;
                    //    data.getmodulename = dtonew.getmodulename;

                    //}
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
