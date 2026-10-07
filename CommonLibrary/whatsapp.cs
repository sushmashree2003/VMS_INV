using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class whatsapp
    {

        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;

        public whatsapp(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public async Task<string> Whatsapp(long MI_Id, string mobileNo, string Template, string filenames,string fileid,string esubject,DateTime? fromdate,string trans_id,string roundoffttl,string words,string MI_Name,Array Prametres)
        {
            try
            {
                
                
               var query2 = _db.Institution.Where(d => d.MI_Id == MI_Id).ToList();
                var datalist = _db.smsEmailSetting.Where(R => R.MI_Id == MI_Id && R.ISES_Template_Name == Template).ToList();
                var messagePayload = new
                {
                    messaging_product = query2.FirstOrDefault().MI_WhatsAppName,
                    to = mobileNo,
                    type = query2.FirstOrDefault().MI_WhatsApptype,
                    template = new
                    {
                        name = datalist.FirstOrDefault().ISES_WhatsAppTemplateId,
                        language = new
                        {
                            code = "en"
                        },
                        components = new object[]
                      {
                    new
                    {
                        type = "header",
                        parameters = new object[]
                        {
                            new
                            {
                                type = "document",
                                document = new
                                {
                                    link = fileid,
                                    filename = filenames
                                }
                            }
                        }
                    },
                    new
                    {
                        type = "body",
                      //   parameter=Prametres
                        parameters = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = esubject
                            },
                            new
                            {
                                type = "text",
                                text = trans_id
                            },
                            new
                            {
                                type = "text",
                                text = "test"
                            },
                            new
                            {
                                type = "text",
                                text = roundoffttl
                            },
                            new
                            {
                                type = "text",
                                text = words
                            },
                            new
                            {
                                type = "text",
                                text = MI_Name
                            }
                        }
                    }
               }
                    }
                };
                try
                {
                    var client = new HttpClient();
                    var jsonContent = JsonConvert.SerializeObject(messagePayload);
                    var requestnew = new HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Post,
                        RequestUri = new Uri(query2.FirstOrDefault().MI_WhatsAppToken),
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };
                    using (var response123 = await client.SendAsync(requestnew))
                    {
                        response123.EnsureSuccessStatusCode();
                        var body = await response123.Content.ReadAsStringAsync();
                        Console.WriteLine(body);
                        JObject joResponse1 = JObject.Parse(body);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return "success";

        }

        public async Task<string> Qutation(long MI_Id, string mobileNo, string Template, string filenamee, string fileid, string esubject,string MI_Name)
        {
            try
            {
                var query2 = _db.Institution.Where(d => d.MI_Id == MI_Id).ToList();
                var datalist = _db.smsEmailSetting.Where(R => R.MI_Id == MI_Id && R.ISES_Template_Name == Template).ToList();
                var messagePayload = new
                {
                    messaging_product = query2.FirstOrDefault().MI_WhatsAppName,
                    to = mobileNo,
                    type = query2.FirstOrDefault().MI_WhatsApptype,
                    template = new
                    {
                        name = datalist.FirstOrDefault().ISES_WhatsAppTemplateId,
                        language = new
                        {
                            code = "en"
                        },
                        components = new object[]
                      {
                    new
                    {
                        type = "header",
                        parameters = new object[]
                        {
                            new
                            {
                                type = "document",
                                document = new
                                {
                                    link = fileid,
                                    filename = filenamee
                                }
                            }
                        }
                    },
                    new
                    {
                        type = "body",                     
                        parameters = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = esubject
                            },                         
                            new
                            {
                                type = "text",
                                text = MI_Name
                            }
                        }
                    }


               }
                    }
                };

                try
                {
                    var client = new HttpClient();
                    var jsonContent = JsonConvert.SerializeObject(messagePayload);
                    var requestnew = new HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Post,
                        RequestUri = new Uri(query2.FirstOrDefault().MI_WhatsAppToken),
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };
                    using (var response123 = await client.SendAsync(requestnew))
                    {
                        response123.EnsureSuccessStatusCode();
                        var body = await response123.Content.ReadAsStringAsync();
                        Console.WriteLine(body);
                        JObject joResponse1 = JObject.Parse(body);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return "success";

        }
        public async Task<string> QutationRodashow(long MI_Id, string mobileNo, string Template, string filenamee, string fileid, string esubject, string EventDate, string EventName)
        {
            try
            {

                var query2 = _db.Institution.Where(d => d.MI_Id == MI_Id).ToList();
                //  var datalist = _db.smsEmailSetting.Where(R => R.MI_Id == MI_Id && R.ISES_Template_Name == Template).ToList();
                var messagePayload = new
                {
                    messaging_product = query2.FirstOrDefault().MI_WhatsAppName,
                    to = mobileNo,
                    type = query2.FirstOrDefault().MI_WhatsApptype,
                    template = new
                    {
                        name = Template,
                        language = new
                        {
                            code = "en"
                        },
                        components = new object[]
                      {
                    new
                    {
                        type = "header",
                        parameters = new object[]
                        {
                            new
                            {
                                type = "document",
                                document = new
                                {
                                    link = fileid,
                                    filename = filenamee
                                }
                            }
                        }
                    },
                    new
                    {
                        type = "body",
                        parameters = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = EventName
                            },
                            new
                            {
                                type = "text",
                                text = EventDate
                            },
                            new
                            {
                                type = "text",
                                text = esubject
                            }
                        }
                    }


               }
                    }
                };

                try
                {
                    var client = new HttpClient();
                    var jsonContent = JsonConvert.SerializeObject(messagePayload);
                    var requestnew = new HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Post,
                        RequestUri = new Uri(query2.FirstOrDefault().MI_WhatsAppToken),
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };
                    using (var response123 = await client.SendAsync(requestnew))
                    {
                        response123.EnsureSuccessStatusCode();
                        var body = await response123.Content.ReadAsStringAsync();
                        Console.WriteLine(body);
                        JObject joResponse1 = JObject.Parse(body);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return "success";

        }
        //UplodaAzure
        public async Task<string> uploadtoazure(IFormFile file, string containername, string folder,long mid,string accountname,string accesskey)
        {
            string newImageNamePath = "";
            string newImageName = "";            
            try
            {                
                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);              
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();                
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                string fileExt = "";
                if (file.ContentType == "image/jpeg")
                {
                    fileExt = ".jpg";
                }
                if (file.ContentType == "image/jpg")
                {
                    fileExt = ".jpg";
                }
                if (file.ContentType == "application/pdf")
                {
                    fileExt = ".pdf";
                }
                if (file.ContentType == "application/kswps")
                {
                    fileExt = ".pdf";
                }
                else if (file.ContentType == "image/png")
                {
                    fileExt = ".png";
                }
                else if (file.ContentType == "video/mp4")
                {
                    fileExt = ".mp4";
                }
                else if (file.ContentType == "audio/mp3")
                {
                    fileExt = ".mp3";
                }
                else if (file.ContentType == "audio/mpeg")
                {
                    fileExt = ".mp3";
                }
                else if (file.ContentType == "video/x-ms-wmv")
                {
                    fileExt = ".wmv";
                }
                else if (file.ContentType == "application/vnd.ms-excel")
                {
                    fileExt = ".xls";
                }
                else if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    fileExt = ".xlsx";
                }
                else if (file.ContentType == "application/msword")
                {
                    fileExt = ".doc";
                }
                else if (file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    fileExt = ".docx";
                }
                else if (file.ContentType == "application/vnd.ms-powerpoint")
                {
                    fileExt = ".ppt";
                }
                else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
                {
                    fileExt = ".pptx";
                }
                else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.slideshow")
                {
                    fileExt = ".ppsx";
                }
                else if (file.ContentType == "application/octet-stream" || file.ContentType == "application/x-rar-compressed")
                {
                    fileExt = ".rar";
                }
                else if (file.ContentType == "application/x-zip-compressed" || file.ContentType == "application/zip" || file.ContentType == "multipart/x-zip")
                {
                    fileExt = ".zip";
                }               
                var ext = fileExt.ToLower();
                var guid = Guid.NewGuid().ToString();
                newImageName = guid + ext;               
                string blobName = newImageName;
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                var blockBlob = container.GetBlockBlobReference(blobName);
                //var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                blockBlob.Properties.ContentType = file.ContentType;
                using (var fileStream = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }
              
                newImageNamePath = blockBlob.Uri.ToString();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }

        //
        public async Task<string> OrderQutation(long MI_Id, string mobileNo, string Template, string filenamee, string fileid, string esubject, string MI_Name)
        {
            try
            {
                var query2 = _db.Institution.Where(d => d.MI_Id == MI_Id).ToList();              
                var messagePayload = new
                {
                    messaging_product = query2.FirstOrDefault().MI_WhatsAppName,
                    to = mobileNo,
                    type = query2.FirstOrDefault().MI_WhatsApptype,
                    template = new
                    {
                        name = Template,
                        language = new
                        {
                            code = "en"
                        },
                        components = new object[]
                      {
                    new
                    {
                        type = "header",
                        parameters = new object[]
                        {
                            new
                            {
                                type = "document",
                                document = new
                                {
                                    link = fileid,
                                    filename = filenamee
                                }
                            }
                        }
                    }
                    


               }
                    }
                };

                try
                {
                    var client = new HttpClient();
                    var jsonContent = JsonConvert.SerializeObject(messagePayload);
                    var requestnew = new HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Post,
                        RequestUri = new Uri(query2.FirstOrDefault().MI_WhatsAppToken),
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };
                    using (var response123 = await client.SendAsync(requestnew))
                    {
                        response123.EnsureSuccessStatusCode();
                        var body = await response123.Content.ReadAsStringAsync();
                        Console.WriteLine(body);
                        JObject joResponse1 = JObject.Parse(body);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return "success";

        }

    }
}
//

