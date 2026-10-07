using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using Microsoft.Net.Http.Headers;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using CommonLibrary;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using DataAccessMsSqlServerProvider;
//using ImageMagick;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ImageUploadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        private readonly DomainModelMsSqlServerContext _context;

        //string accountname = "dcampusstrg";
        //string accesskey = "DqwhzgBbVF53spoTwJrPQWMMVaKbban9Ls+y73xceTeCZKAHV4C+C2x1H7R4ltGn9thflAX+6ARlwzjx7w4XNw==";
        string accountname = "";
        string accesskey = "";
        //private static FacadeUrl _config;

        StudentApplicationDelegate sad = new StudentApplicationDelegate();
        public ImageUploadController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, DomainModelMsSqlServerContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _context = context;
        }

        [Route("Uploadprofilepicpreadmission")]

        public async Task<string> Uploadprofilepicpreadmin(FileDescriptionDTO fileDescriptionShort)
        {

            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));

            string newImageName = "";

            //string fp = null;
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_Preadmission(file, containername, "OnlineUserProfiles");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return newImageName;
        }



        [Route("emailfileattachupload")]
        public async Task<string> emailfileattachupload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "emailfileattachupload");
                    }
                }
            }
            return newImageName;
        }
        public async Task<string> uploadtoazure_Preadmission(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            try
            {
                var data = _context.IVRM_Storage_path_Details.ToList();
                if (data.Count() > 0)
                {
                    accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                }
                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                // Create a blob client.
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();

                // Get a reference to a container 
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                string fileExt = "";
                if (file.ContentType == "image/jpeg")
                {
                    fileExt = ".jpg";
                }
                if (file.ContentType == "application/pdf")
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
                //else if (file.ContentType == "video/x-ms-wmv")
                //{
                //    fileExt = ".wmv";
                //}
                //string fileExt = ".jpg";
                var ext = fileExt.ToLower();
                var guid = Guid.NewGuid().ToString();
                newImageName = guid + ext;

                //string blobName = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                string blobName = newImageName;
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                //Get reference to a blob

                var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                //var fileStr = file.OpenReadStream();
                //await blockBlob.UploadFromStreamAsync(ss);
                //ss.Dispose();
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
        public async Task<string> uploadtoazure(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            try
            {
                var data = _context.IVRM_Storage_path_Details.ToList();
                if (data.Count() > 0)
                {
                    accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                }

                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                // Create a blob client.
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();

                // Get a reference to a container 
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
                //string fileExt = ".jpg";
                var ext = fileExt.ToLower();
                var guid = Guid.NewGuid().ToString();
                newImageName = guid + ext;

                //string blobName = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                string blobName = newImageName;
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                //Get reference to a blob

                var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                //var fileStr = file.OpenReadStream();
                //await blockBlob.UploadFromStreamAsync(ss);
                //ss.Dispose();
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
        public async Task<string> uploadtoazureSales(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            try
            {
                var data = _context.IVRM_Storage_path_Details.ToList();
                if (data.Count() > 0)
                {
                    accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                }

                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                // Create a blob client.
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();

                // Get a reference to a container 
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
                //string fileExt = ".jpg";
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

        public async Task<string> uploadtoazureNew(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            var newFileName = string.Empty;
            var fileName = string.Empty;
            var nameofthefile = "";
            var files = HttpContext.Request.Form.Files;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            try
            {
                if (HttpContext.Request.Form.Files != null)
                {
                    foreach (var file1 in files)
                    {
                        if (file.Length > 0)
                        {
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
                            if (fileExt == ".jpg" || fileExt == ".png")
                            {
                                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                fileName = file.FileName;
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    nameofthefile = fileName;
                                }
                                fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file1.ContentDisposition).FileName.Trim('"');
                                fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";
                            }
                            var data = _context.IVRM_Storage_path_Details.ToList();
                            if (data.Count() > 0)
                            {
                                accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                                accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                            }
                            StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                            CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                            CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference(containername);
                            var ext = fileExt.ToLower();
                            var guid = Guid.NewGuid().ToString();
                            newImageName = guid + ext;
                            string blobName = newImageName;
                            await container.CreateIfNotExistsAsync();
                            await container.SetPermissionsAsync(new BlobContainerPermissions
                            {
                                PublicAccess = BlobContainerPublicAccessType.Blob
                            });
                            var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                            if (fileExt == ".jpg" || fileExt == ".png")
                            {
                                const int size = 900;
                                const int quality = 50;
                                //using (var image = new MagickImage(fileName))
                                //{
                                //    image.Resize(size, size);
                                //    image.Strip();
                                //    image.Quality = quality;
                                //    image.Write(fileName);
                                //}
                                await blockBlob.UploadFromFileAsync(fileName);
                                newImageNamePath = blockBlob.Uri.ToString();
                                if (System.IO.File.Exists(fileName))
                                {
                                    System.IO.File.Delete(fileName);
                                }
                            }
                            else
                            {
                                using (var fileStream = file.OpenReadStream())
                                {
                                    await blockBlob.UploadFromStreamAsync(fileStream);
                                }
                                newImageNamePath = blockBlob.Uri.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }



        [HttpPost]
        public async Task<bool> UploadFiles(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            var fileContent = reader.ReadToEnd();
                            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                            Console.WriteLine(fileContent.ToString());
                        }

                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                        //contentTypes.Add(file.ContentType);
                        names.Add(fileName.ToString().Trim());
                        string ss = file.ContentDisposition.ToString();

                        Console.WriteLine(ss);
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        var uploads = Path.Combine(webRootPath, "images\\uploads\\");

                        var filePath = uploads + fileName;

                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                        stu.PASMD_DocumentName = fileName.ToString().Trim();
                        stu.PASMD_Path = filePath;
                        stu.pasR_Id = fileDescriptionShort.Id;
                        sad.savestudentdetails(stu);
                        // Extension method update RC2 has removed this 
                        //await file.SaveAsAsync(Path.Combine(_optionsApplicationConfiguration.Value.ServerUploadFolder, fileName));
                    }
                }
            }

            //var files = new FileResult
            //{
            //    FileNames = names,
            //    ContentTypes = contentTypes,
            //    Description = fileDescriptionShort.Description,
            //    CreatedTimestamp = DateTime.UtcNow,
            //    UpdatedTimestamp = DateTime.UtcNow,
            //};

            // _fileRepository.AddFileDescriptions(files);

            return true;
        }

        //Student Profile
        [Route("Uploadprofilepic")]
        public async Task<string> Uploadprofilepic(FileDescriptionDTO fileDescriptionShort)
        {

            var names = new List<string>();
            var contentTypes = new List<string>();

            //int orgid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Idpreadmin"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //if(mid==0)
            //{
            // mid = orgid;
            //}

            string newImageName = "";

            //string fp = null;
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {

                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "StudentProfilePics");

                            //StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                            //CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                            //// Create a blob client.
                            //CloudBlobClient blobClient = acc.CreateCloudBlobClient();

                            //// Get a reference to a container 
                            //CloudBlobContainer container = blobClient.GetContainerReference("files");

                            //string fileExt = ".jpg";
                            //var ext = fileExt.ToLower();
                            //var guid = Guid.NewGuid().ToString();
                            //newImageName = guid + ext;

                            ////string blobName = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                            //string blobName = newImageName;
                            //await container.CreateIfNotExistsAsync();
                            //await container.SetPermissionsAsync(new BlobContainerPermissions
                            //{
                            //    PublicAccess = BlobContainerPublicAccessType.Blob
                            //});
                            ////Get reference to a blob

                            //var blockBlob = container.GetBlockBlobReference(blobName);
                            ////var fileStr = file.OpenReadStream();
                            ////await blockBlob.UploadFromStreamAsync(ss);
                            ////ss.Dispose();
                            //using (var fileStream = file.OpenReadStream())
                            //{
                            //    await blockBlob.UploadFromStreamAsync(fileStream);
                            //}
                            //var blobsName = blockBlob.Uri.ToString();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return newImageName;
        }

        [Route("Librarybooks")]
        public async Task<string> Librarybooks(FileDescriptionDTO fileDescriptionShort)
        {

            var names = new List<string>();
            var contentTypes = new List<string>();

            //int orgid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Idpreadmin"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //if(mid==0)
            //{
            // mid = orgid;
            //}

            string newImageName = "";

            //string fp = null;
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {

                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Librarybooks");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return newImageName;
        }

        [Route("Uploadtrnsportdocuments")]
        public async Task<string> Uploadtrnsportdocuments(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "Trnsportdocuments");
                    }
                }
            }
            return newImageName;
        }

        [Route("Uploadlegaldocuments")]
        public async Task<string> Uploadlegaldocuments(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "Legaldocuments");
                    }
                }
            }
            return newImageName;
        }


        //channel partner
        [Route("UploadChannelPartnerdocuments")]
        public async Task<List<filedetails>> UploadChannelPartnerdocuments(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            var folder = "";
                            if (fileDescriptionShort.folder == null)
                            {
                                folder = "ChannelPartnerdocuments";
                            }
                            else
                            {
                                folder = fileDescriptionShort.folder;
                            }

                            newImageName = await uploadtoazure(file, containername, folder);
                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
            //FilesPaths
        }


        [Route("ParentsProfilePics")]
        public async Task<string> ParentsProfilePics(FileDescriptionDTO fileDescriptionShort)
        {

            var names = new List<string>();
            var contentTypes = new List<string>();

            //int orgid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Idpreadmin"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //if(mid==0)
            //{
            // mid = orgid;
            //}

            string newImageName = "";

            //string fp = null;
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {

                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "ParentsProfilePics");

                            //StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                            //CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                            //// Create a blob client.
                            //CloudBlobClient blobClient = acc.CreateCloudBlobClient();

                            //// Get a reference to a container 
                            //CloudBlobContainer container = blobClient.GetContainerReference("files");

                            //string fileExt = ".jpg";
                            //var ext = fileExt.ToLower();
                            //var guid = Guid.NewGuid().ToString();
                            //newImageName = guid + ext;

                            ////string blobName = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                            //string blobName = newImageName;
                            //await container.CreateIfNotExistsAsync();
                            //await container.SetPermissionsAsync(new BlobContainerPermissions
                            //{
                            //    PublicAccess = BlobContainerPublicAccessType.Blob
                            //});
                            ////Get reference to a blob

                            //var blockBlob = container.GetBlockBlobReference(blobName);
                            ////var fileStr = file.OpenReadStream();
                            ////await blockBlob.UploadFromStreamAsync(ss);
                            ////ss.Dispose();
                            //using (var fileStream = file.OpenReadStream())
                            //{
                            //    await blockBlob.UploadFromStreamAsync(fileStream);
                            //}
                            //var blobsName = blockBlob.Uri.ToString();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return newImageName;
        }
        //[Route("Uploadprofilepic")]
        //public async Task<string> Uploadprofilepic(FileDescriptionDTO fileDescriptionShort)
        //{
        //    var names = new List<string>();
        //    var contentTypes = new List<string>();

        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

        //    string fp = null;
        //    StudentApplicationDTO stu = new StudentApplicationDTO();
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var file in fileDescriptionShort.File)
        //        {
        //            if (file.Length > 0)
        //            {
        //                using (var reader = new StreamReader(file.OpenReadStream()))
        //                {
        //                    var fileContent = reader.ReadToEnd();
        //                    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
        //                    Console.WriteLine(fileContent.ToString());
        //                }

        //                //  var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');


        //                var fileName = string.Format("{0}", Guid.NewGuid(), file.ContentType);
        //                string fileExt = "jpg";
        //                if (file.ContentType == "image/png") { fileExt = "png"; }
        //                else if (file.ContentType == "image/gif") { fileExt = "gif"; }
        //                else if (file.ContentType == "image/jpeg") { fileExt = "jpg"; }

        //                //contentTypes.Add(file.ContentType);
        //                names.Add(fileName);
        //                string ss = file.ContentDisposition.ToString();

        //                Console.WriteLine(ss);
        //                string webRootPath = _hostingEnvironment.WebRootPath;
        //                var uploads = Path.Combine(webRootPath, "images\\uploads\\studentprofilepic\\" + mid + "\\");

        //                var directoryInfo = new FileInfo(uploads).Directory;

        //                if (directoryInfo != null)
        //                    directoryInfo.Create();

        //                var filePath = uploads + fileName + "." + fileExt;


        //               // var filePath = uploads + fileName;

        //                file.CopyTo(new FileStream(filePath, FileMode.Create));
        //                stu.PASMD_DocumentName = fileName;

        //                fp = filePath;
        //                fp = fp.Substring(fp.IndexOf("images"));
        //            }
        //        }
        //    }
        //    return fp;
        //}

        [Route("UploadStudentDocuments")]
        public async Task<string> UploadStudentDocuments(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {

                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "StudentDocuments");


                        //using (var reader = new StreamReader(file.OpenReadStream()))
                        //{
                        //    var fileContent = reader.ReadToEnd();
                        //    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                        //    Console.WriteLine(fileContent.ToString());
                        //}
                        //  var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        //  var fileName = string.Format("{0}", Guid.NewGuid(), file.ContentType);
                        // string fileExt = "jpg";
                        //if (file.ContentType == "image/png") { fileExt = "png"; }
                        //else if (file.ContentType == "image/gif") { fileExt = "gif"; }
                        //else if (file.ContentType == "image/jpeg") { fileExt = "jpg"; }

                        //contentTypes.Add(file.ContentType);
                        //names.Add(fileName);
                        //string ss = file.ContentDisposition.ToString();

                        //Console.WriteLine(ss);
                        //string webRootPath = _hostingEnvironment.WebRootPath;
                        //var uploads = Path.Combine(webRootPath, "images\\uploads\\studentDocumnets\\" + mid + "\\");

                        //var directoryInfo = new FileInfo(uploads).Directory;

                        //if (directoryInfo != null)
                        //    directoryInfo.Create();

                        //var filePath = uploads + fileName + "." + fileExt;


                        // var filePath = uploads + fileName;

                        //file.CopyTo(new FileStream(filePath, FileMode.Create));
                        //stu.PASMD_DocumentName = fileName;

                        //fp = filePath;
                        //fp = fp.Substring(fp.IndexOf("images"));
                    }
                }
            }
            return newImageName;
        }
        [Route("uploadGuardianDocs")]
        public async Task<string> UploadGuardianPhoto(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "GuardianDocs");

                        //using (var reader = new StreamReader(file.OpenReadStream()))
                        //{
                        //    var fileContent = reader.ReadToEnd();
                        //    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                        //    Console.WriteLine(fileContent.ToString());
                        //}



                        //  var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');


                        //var fileName = string.Format("{0}", Guid.NewGuid(), file.ContentType);
                        //string fileExt = "jpg";
                        //if (file.ContentType == "image/png") { fileExt = "png"; }
                        //else if (file.ContentType == "image/gif") { fileExt = "gif"; }
                        //else if (file.ContentType == "image/jpeg") { fileExt = "jpg"; }

                        ////contentTypes.Add(file.ContentType);
                        //names.Add(fileName);
                        //string ss = file.ContentDisposition.ToString();

                        //Console.WriteLine(ss);
                        //string webRootPath = _hostingEnvironment.WebRootPath;
                        //var uploads = Path.Combine(webRootPath, "images\\uploads\\Guardian_Docs\\" + mid + "\\");

                        //var directoryInfo = new FileInfo(uploads).Directory;

                        //if (directoryInfo != null)
                        //    directoryInfo.Create();

                        //var filePath = uploads + fileName + "." + fileExt;


                        //// var filePath = uploads + fileName;

                        //file.CopyTo(new FileStream(filePath, FileMode.Create));
                        //stu.PASMD_DocumentName = fileName;

                        //fp = filePath;
                        //fp = fp.Substring(fp.IndexOf("images"));
                    }
                }
            }
            return newImageName;
        }
        [Route("UploadParentsDocs")]
        public async Task<string> UploadParentsDocs(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {

                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "StudentParentsDocs");

                        //using (var reader = new StreamReader(file.OpenReadStream()))
                        //{
                        //    var fileContent = reader.ReadToEnd();
                        //    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                        //    Console.WriteLine(fileContent.ToString());
                        //}

                        //  var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');


                        //var fileName = string.Format("{0}", Guid.NewGuid(), file.ContentType);
                        //string fileExt = "jpg";
                        //if (file.ContentType == "image/png") { fileExt = "png"; }
                        //else if (file.ContentType == "image/gif") { fileExt = "gif"; }
                        //else if (file.ContentType == "image/jpeg") { fileExt = "jpg"; }

                        //contentTypes.Add(file.ContentType);
                        //names.Add(fileName);
                        //string ss = file.ContentDisposition.ToString();

                        //Console.WriteLine(ss);
                        //string webRootPath = _hostingEnvironment.WebRootPath;
                        //var uploads = Path.Combine(webRootPath, "images\\uploads\\Parents_Docs\\" + mid + "\\");

                        //var directoryInfo = new FileInfo(uploads).Directory;

                        //if (directoryInfo != null)
                        //    directoryInfo.Create();

                        //var filePath = uploads + fileName + "." + fileExt;


                        // var filePath = uploads + fileName;

                        //file.CopyTo(new FileStream(filePath, FileMode.Create));
                        //stu.PASMD_DocumentName = fileName;

                        //fp = filePath;
                        //fp = fp.Substring(fp.IndexOf("images"));
                    }
                }
            }
            return newImageName;
        }

        [Route("Upload_GalleryImgVideos")]
        public async Task<List<string>> Upload_GalleryImgVideos(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "GalleryImgVideos");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }
        [Route("Upload_ISMAttachment")]
        public async Task<List<string>> Upload_ISMAttachment(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "ISM_Attachments");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        [Route("Upload_ISMAttachment_new")]
        public async Task<List<filedetails>> Upload_ISMAttachment_new(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "ISM_Attachments");


                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }

        //Mahaboob for COE Module
        [Route("Upload_imgs_vids")]
        public async Task<List<string>> Upload_imgs_vids(FileDescriptionDTO fileDescriptionShort)
        {

            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            //string fp = null;
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {

                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "COE_Images_Videos");
                            FilesPaths.Add(newImageName);



                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return FilesPaths;
        }

        [Route("ClgUpload_Img_Videos")]
        public async Task<List<string>> ClgUpload_Img_Videos(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "clg_portal_images_videos");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        //Documents Upload
        [Route("Upload_Docs")]
        public async Task<List<string>> Upload_Docs(FileDescriptionDTO fileDescriptionShort)
        {

            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";
            List<string> FilesPaths = new List<string>();

            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Portal_Document");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return FilesPaths;
        }

        //Documents Upload
        [Route("Upload_Noticefiles")]
        public async Task<List<string>> Upload_Noticefiles(FileDescriptionDTO fileDescriptionShort)
        {

            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";
            List<string> FilesPaths = new List<string>();

            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Notice_Files");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return FilesPaths;
        }

        [Route("Waviedofffiles")]
        public async Task<List<string>> Waviedofffiles(FileDescriptionDTO fileDescriptionShort)
        {

            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";
            List<string> FilesPaths = new List<string>();

            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Waviedofffiles");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            return FilesPaths;
        }

        [Route("Upload_Principal_sign")]
        public async Task<string> Upload_Principal_sign(FileDescriptionDTO fileprincipalsign)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";

            //string fp = null;
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileprincipalsign.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "principalsign");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("Upload_Manager_sign")]
        public async Task<string> Upload_Manager_sign(FileDescriptionDTO filemanagersign)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";

            //string fp = null;
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in filemanagersign.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "managersign");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("uploadvehicleimg")]
        public async Task<string> uploadvehicleimg(FileDescriptionDTO filevehicleimg)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";

            //string fp = null;
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in filevehicleimg.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Transport_VehicleImg");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        //Uploadsoundtrack IVRS
        [Route("Uploadsoundtrack")]
        public async Task<string> Uploadsoundtrack(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "IVRSSOUNDTRACKS");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        //Upload Lessonplanner Documents
        [Route("lessonplannerdoc")]
        public async Task<string> lessonplannerdoc(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "LessonPlanner");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("OnlineProgramdoc")]
        public async Task<string> OnlineProgramdoc(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "OnlineProgram");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        //NAAC DOCUMENTS UPLOAD 
        [Route("Uploadnaacdocuments")]
        public async Task<string> Uploadnaacdocuments(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "NAACDocumentsUpload");
                    }
                }
            }
            return newImageName;
        }

        [Route("Addcandidate")]
        public async Task<string> Addcandidate(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "files";
                        newImageName = await uploadtoazureNew(file, containername, "NCandidateDocuments");
                    }
                }
            }
            return newImageName;
        }

        [Route("UploadspaceBooking")]
        public async Task<string> UploadspaceBooking(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "UploadspaceBooking");
                    }
                }
            }
            return newImageName;
        }
        //UploadSales
        [Route("UploadSales")]
        public async Task<string> UploadSales(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "presales";
                        newImageName = await uploadtoazureSales(file, containername, "salesQutations");
                    }
                }
            }
            return newImageName;
        }
        [Route("fmsdocuments")]
        public async Task<string> fmsdocuments(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "FMS");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("DRReportCheckList")]
        public async Task<string> DRReportCheckList(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "DRReport");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }


        [Route("HWHealthCheckUpFiles")]
        public async Task<string> HWHealthCheckUpFiles(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "HWHealthCheckUpFiles");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("Takeascreenshot")]
        public async Task<string> Takeascreenshot(FileDescriptionDTO fileDescriptionShort)
        {
            string newImageNamePath = "";
            string newImageName = "";
            string accountname = "";
            string accesskey = "";
            fileDescriptionShort.Mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            try
            {
                var data = _context.IVRM_Storage_path_Details.ToList();
                if (data.Count() > 0)
                {
                    accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                }

                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                // Create a blob client.
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();

                // Get a reference to a container 
                CloudBlobContainer container = blobClient.GetContainerReference("pagescreenshots");
                string fileExt = "";
                fileExt = ".jpg";
                var ext = fileExt.ToLower();
                var guid = Guid.NewGuid().ToString();
                newImageName = guid + ext;
                string blobName = newImageName;
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                var blockBlob = container.GetBlockBlobReference(fileDescriptionShort.Mi_id + "/Screenshots/" + blobName);
                var Base64 = fileDescriptionShort.Description;
                string convert1 = Base64.Replace("data:image/png;base64,", String.Empty);
                convert1 = Base64.Replace("data:image/octet-stream;base64,", String.Empty);
                var convert2 = convert1.Replace("data:image/jpeg;base64,", String.Empty);
                convert2 = convert2.Replace("data:screnshot1;base64,", String.Empty);
                convert2 = convert2.Replace(" ", "+");
                int mod4 = convert2.Length % 4;
                if (mod4 > 0)
                {
                    convert2 += new string('=', 4 - mod4);
                }

                var bytes = Convert.FromBase64String(convert2);// without data:image/jpeg;base64 prefix, just base64 string
                using (var stream = new MemoryStream(bytes))
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }
                newImageNamePath = blockBlob.Uri.ToString();
                fileDescriptionShort.onlinepreadminfilepath = newImageNamePath;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return fileDescriptionShort.onlinepreadminfilepath;

        }

        [Route("upload_test_cases")]
        public async Task<string> upload_test_cases(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "uploadtestcases");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;

        }
    }
    public class filedetails
    {
        public string name { get; set; }
        public string path { get; set; }

    }
}