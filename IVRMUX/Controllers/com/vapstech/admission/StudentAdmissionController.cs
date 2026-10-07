using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
   // [EnableCors("AllowSpecificOrigin")]

    public class StudentAdmissionController : Controller
    {
        // GET: /<controller>/
        StudentAdmissionDelegates StudentAdmissionStr = new StudentAdmissionDelegates();
        private readonly IHostingEnvironment _hostingEnvironment;
        public StudentAdmissionController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [Route("Getdetails/")]
        public Adm_M_StudentDTO Getdetails(Adm_M_StudentDTO Adm_M_StudentDTO)
        {
             Adm_M_StudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
             Adm_M_StudentDTO.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           
            // Adm_M_StudentDTO.MI_Id = mid;

            return StudentAdmissionStr.GetData(Adm_M_StudentDTO);
        }

        [Route("getdpstate/{id:int}")]
        public Adm_M_StudentDTO getdpstate(int id)
        {
            return StudentAdmissionStr.getdpstate(id);
        }

        [Route("getdpcities/{id:int}")]
        public Adm_M_StudentDTO getdpcities(int id)
        {
            return StudentAdmissionStr.getdpcities(id);
        }

        //[Route("GetSelectedRowdetails/{id:int}")]
        //public Adm_M_StudentDTO GetSelectedRowDetails(int ID)
        //{
        //   // HttpContext.Session.SetString("StuId", ID.ToString());
        //    return StudentAdmissionStr.GetSelectedRowDetails(ID);

        //}
        [Route("GetSelectedRowdetails/{id:int}")]
        public Adm_M_StudentDTO GetSelectedRowDetails(int ID)
        {
            Adm_M_StudentDTO data = new Adm_M_StudentDTO();
            // HttpContext.Session.SetString("StuId", ID.ToString());
          //  data = StudentAdmissionStr.GetSelectedRowDetails(ID);
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = ID;
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            // int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.MI_Id = mid;
            //  string accountname = "dcampusstrg";
            //  string accesskey = "DqwhzgBbVF53spoTwJrPQWMMVaKbban9Ls+y73xceTeCZKAHV4C+C2x1H7R4ltGn9thflAX+6ARlwzjx7w4XNw==";


            //  StorageCredentials cre = new StorageCredentials(accountname, accesskey);
            //  CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
            // Create a blob client.
            //  CloudBlobClient blobClient = acc.CreateCloudBlobClient();

            //string filename= ((Adm_M_StudentDTO)data.adm_m_student.GetValue(0)).AMST_Photoname;
            //  if(data.AMST_Photoname!=null && data.AMST_Photoname!="")
            //{
            //    // Get a reference to a container 
            //    CloudBlobContainer container = blobClient.GetContainerReference("files"+"/"+mid+"/"+"StudentProfilePics");
            //    string filename = data.AMST_Photoname;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.AMST_Photoname = blobsName;
            //}
            //else
            //{
            //    data.AMST_Photoname = "";
            //}
            //if (data.ANST_FatherPhoto != null && data.ANST_FatherPhoto != "")
            //{
            //    // Get a reference to a container 
            //    CloudBlobContainer container = blobClient.GetContainerReference("files" + "/" + mid + "/" + "StudentParentsDocs");
            //    string filename = data.ANST_FatherPhoto;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.ANST_FatherPhoto = blobsName;
            //}
            //else
            //{
            //    data.ANST_FatherPhoto = "";
            //}
            //if (data.AMST_Father_FingerPrint != null && data.AMST_Father_FingerPrint!= "")
            //{
            //    // Get a reference to a container 
            //    CloudBlobContainer container = blobClient.GetContainerReference("files" + "/" + mid + "/" + "StudentParentsDocs");
            //    string filename = data.AMST_Father_FingerPrint;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.AMST_Father_FingerPrint = blobsName;
            //}
            //else
            //{
            //    data.AMST_Father_FingerPrint = "";
            //}
            //if (data.AMST_Father_Signature != null && data.AMST_Father_Signature!= "")
            //{
            //    // Get a reference to a container 
            //    CloudBlobContainer container = blobClient.GetContainerReference("files" + "/" + mid + "/" + "StudentParentsDocs");
            //    string filename = data.AMST_Father_Signature;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.AMST_Father_Signature = blobsName;
            //}
            //else
            //{
            //    data.AMST_Father_Signature = "";
            //}
            //if (data.ANST_MotherPhoto != null && data.ANST_MotherPhoto!= "")
            //{
            //    // Get a reference to a container 
            //    CloudBlobContainer container = blobClient.GetContainerReference("files" + "/" + mid + "/" + "StudentParentsDocs");
            //    string filename = data.ANST_MotherPhoto;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.ANST_MotherPhoto = blobsName;
            //}
            //else
            //{
            //    data.ANST_MotherPhoto = "";
            //}
            //if (data.AMST_Mother_Signature != null && data.AMST_Mother_Signature!="")
            //{
            //    // Get a reference to a container 
            //    CloudBlobContainer container = blobClient.GetContainerReference("files" + "/" + mid + "/" + "StudentParentsDocs");
            //    string filename = data.AMST_Mother_Signature;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.AMST_Mother_Signature = blobsName;
            //}
            //else
            //{
            //    data.AMST_Mother_Signature = "";
            //}
            //if (data.AMST_Mother_FingerPrint != null && data.AMST_Mother_FingerPrint!= "")
            //{
            //    // Get a reference to a container 
            //    CloudBlobContainer container = blobClient.GetContainerReference("files" + "/" + mid + "/" + "StudentParentsDocs");
            //    string filename = data.AMST_Mother_FingerPrint;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.AMST_Mother_FingerPrint = blobsName;
            //}
            //else
            //{
            //    data.AMST_Mother_FingerPrint = "";
            //}
            //if (data.DocumentList1.Count > 0)
            //{
            //   for(int i=0;i< data.DocumentList1.Count;i++)
            //    {
            //      if(data.DocumentList1[i].Document_Path != null && data.DocumentList1[i].Document_Path != "")
            //        {
            //            // Get a reference to a container 
            //            CloudBlobContainer container = blobClient.GetContainerReference("files" + "/" + mid + "/" + "StudentDocuments");
            //            string filename = data.DocumentList1[i].Document_Path;
            //            var blockBlob = container.GetBlockBlobReference(filename);
            //            var blobsName = blockBlob.Uri.ToString();
            //            data.DocumentList1[i].Document_Path = blobsName;
            //        }
            //    }
            //    data.DocumentList = data.DocumentList1.ToArray();
            //}
            return StudentAdmissionStr.GetSelectedRowDetails(data);
        }

        //[HttpPost]
        //public Adm_M_StudentDTO SaveData([FromBody] Adm_M_StudentDTO MMD)
        //{
        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    MMD.MI_Id = mid;
        //    //int StuId = 0;
        //    //if (HttpContext.Session.GetString("StuId") != null)
        //    //{
        //    //    StuId = Convert.ToInt32(HttpContext.Session.GetString("StuId"));//Get
        //    //    MMD.AMST_Id = StuId;
        //    //    HttpContext.Session.Remove("StuId");
        //    //}
        //    return StudentAdmissionStr.SaveData(MMD);
        //}
        [HttpPost]
        public Adm_M_StudentDTO SaveData([FromBody] Adm_M_StudentDTO MMD)
        {
            // var names = new List<string>();
            // var contentTypes = new List<string>();

            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            //if (MMD.File != null)
            //{
            //    foreach (var file in MMD.File)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var reader = new StreamReader(file.OpenReadStream()))
            //            {
            //                var fileContent = reader.ReadToEnd();

            //                var fileName1 = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').Split('.');
            //                names.Add(fileName1[0]);
            //                string fileExt = "";
            //                if (fileName1[1] != "" && fileName1 != null)
            //                {
            //                    fileExt = fileName1[1];
            //                }
            //                else
            //                {
            //                    //get file extension from mime type.
            //                    fileExt = "jpg";

            //                }
            //                var fileName = fileName1[0] + "_" + Guid.NewGuid() + "." + fileExt;
            //                names.Add(fileName);
            //                string webRootPath = _hostingEnvironment.WebRootPath;
            //                var uploads = Path.Combine(webRootPath, "images\\uploads\\studentDocumnets\\" + dto.MI_Id + "\\");

            //                var directoryInfo = new FileInfo(uploads).Directory;

            //                if (directoryInfo != null)
            //                    directoryInfo.Create();

            //                var filePath = uploads + fileName;
            //                using (var myFile = new FileStream(filePath, FileMode.Create))
            //                {
            //                    // interact with myFile here, it will be disposed automatically
            //                    file.CopyTo(myFile);
            //                }

            //                contentTypes.Add(filePath);
                           
            //            }

            //        }
            //    }
            //}

            //dto.AMST_AadharNo = MMD.AMST_AadharNo;
            //dto.AMST_Id = MMD.AMST_Id;
            //dto.ASMAY_Id = MMD.ASMAY_Id;
            //dto.AMST_FirstName = MMD.AMST_FirstName;
            //dto.AMST_MiddleName= MMD.AMST_MiddleName;


            //dto.AMST_LastName = MMD.AMST_LastName;
            //dto.AMST_Date = MMD.AMST_Date;
            //dto.AMST_RegistrationNo = MMD.AMST_RegistrationNo;
            //dto.AMST_AdmNo = MMD.AMST_AdmNo;
            //dto.AMC_Id = MMD.AMC_Id;
            //dto.AMST_Sex = MMD.AMST_Sex;
            //dto.AMST_DOB = MMD.AMST_DOB;
            //dto.AMST_DOB_Words = MMD.AMST_DOB_Words;
            //dto.PASR_Age = MMD.PASR_Age;
            //dto.ASMCL_Id = MMD.ASMCL_Id;
            //dto.AMST_BloodGroup = MMD.AMST_BloodGroup;
            //dto.AMST_MotherTongue = MMD.AMST_MotherTongue;
            //dto.AMST_BirthCertNO = MMD.AMST_BirthCertNO;
            //dto.IVRMMR_Id = MMD.IVRMMR_Id;

            //dto.IMCC_Id = MMD.IMCC_Id;
            //dto.IC_Id = MMD.IC_Id;
            //dto.AMST_PerCountry = MMD.AMST_PerCountry;
            //dto.AMST_PerStreet = MMD.AMST_PerStreet;
            //dto.AMST_PerArea = MMD.AMST_PerArea;
            //dto.AMST_PerCity = MMD.AMST_PerCity;
            //dto.AMST_PerState = MMD.AMST_PerState;
            //dto.AMST_PerPincode = MMD.AMST_PerPincode;
            //dto.AMST_ConStreet = MMD.AMST_ConStreet;
            //dto.AMST_ConArea = MMD.AMST_ConArea;
            //dto.AMST_ConCity = MMD.AMST_ConCity;
            //dto.AMST_ConState = MMD.AMST_ConState;
            //dto.AMST_ConCountry = MMD.AMST_ConCountry;
            //dto.AMST_ConPincode = MMD.AMST_ConPincode;
            //dto.AMST_AadharNo = MMD.AMST_AadharNo;
            //dto.AMST_StuBankAccNo = MMD.AMST_StuBankAccNo;
            //dto.AMST_StuBankIFSC_Code = MMD.AMST_StuBankIFSC_Code;
            //dto.AMST_StuCasteCertiNo = MMD.AMST_StuCasteCertiNo;

          
            //dto.AMST_MobileNo = MMD.AMST_MobileNo;
            //dto.AMST_emailId = MMD.AMST_emailId;
            //dto.AMST_Photoname = MMD.AMST_Photoname;
            //dto.AMST_BirthPlace = MMD.AMST_BirthPlace;
            //dto.AMST_FatherAliveFlag = MMD.AMST_FatherAliveFlag;
            //dto.AMST_FatherName = MMD.AMST_FatherName;
            //dto.AMST_FatherAadharNo = MMD.AMST_FatherAadharNo;
            //dto.AMST_FatherSurname = MMD.AMST_FatherSurname;
            //dto.AMST_FatherEducation = MMD.AMST_FatherEducation;
            //dto.AMST_FatherOccupation = MMD.AMST_FatherOccupation;
            //dto.AMST_FatherOfficeAdd = MMD.AMST_FatherOfficeAdd;
            //dto.AMST_FatherDesignation = MMD.AMST_FatherDesignation;
            //dto.AMST_FatherMonIncome = MMD.AMST_FatherMonIncome;

            //dto.AMST_FatherAnnIncome = MMD.AMST_FatherAnnIncome;
            //dto.AMST_FatherNationality = MMD.AMST_FatherNationality;
            //dto.AMST_FatherMobleNo = MMD.AMST_FatherMobleNo;

            //dto.AMST_FatheremailId = MMD.AMST_FatheremailId;
            //dto.AMST_FatherBankAccNo = MMD.AMST_FatherBankAccNo;
            //dto.AMST_FatherBankIFSC_Code = MMD.AMST_FatherBankIFSC_Code;
            //dto.AMST_FatherCasteCertiNo = MMD.AMST_FatherCasteCertiNo;
            //dto.ANST_FatherPhoto = MMD.ANST_FatherPhoto;
            //dto.AMST_MotherAliveFlag = MMD.AMST_MotherAliveFlag;
            //dto.AMST_MotherName = MMD.AMST_MotherName;
            //dto.AMST_MotherAadharNo = MMD.AMST_MotherAadharNo;
            //dto.AMST_MotherSurname = MMD.AMST_MotherSurname;
            //dto.AMST_MotherEducation = MMD.AMST_MotherEducation;
            //dto.AMST_MotherOccupation = MMD.AMST_MotherOccupation;
            //dto.AMST_MotherOfficeAdd = MMD.AMST_MotherOfficeAdd;
            //dto.AMST_MotherDesignation = MMD.AMST_MotherDesignation;

            //dto.AMST_MotherMonIncome = MMD.AMST_MotherMonIncome;
            //dto.AMST_MotherAnnIncome = MMD.AMST_MotherAnnIncome;
            //dto.AMST_MotherNationality = MMD.AMST_MotherNationality;
            //dto.AMST_MotherMobileNo = MMD.AMST_MotherMobileNo;
            //dto.AMST_MotherEmailId = MMD.AMST_MotherEmailId;
            //dto.AMST_MotherBankAccNo = MMD.AMST_MotherBankAccNo;
            //dto.AMST_MotherBankIFSC_Code = MMD.AMST_MotherBankIFSC_Code;
            //dto.AMST_MotherCasteCertiNo = MMD.AMST_MotherCasteCertiNo;
            //dto.AMST_Nationality = MMD.AMST_Nationality;
            //dto.AMST_BPLCardFlag = MMD.AMST_BPLCardFlag;
            //dto.AMST_BPLCardNo = MMD.AMST_BPLCardNo;
            //dto.AMST_HostelReqdFlag = MMD.AMST_HostelReqdFlag;
            //dto.AMST_TransportReqdFlag = MMD.AMST_TransportReqdFlag;
            //dto.AMST_GymReqdFlag = MMD.AMST_GymReqdFlag;
            //dto.AMST_ECSFlag = MMD.AMST_ECSFlag;
            //dto.SelectedActivityDetails = MMD.SelectedActivityDetails;
            //dto.SelectedRefrenceDetails = MMD.SelectedRefrenceDetails;
            //dto.SelectedSourceDetails = MMD.SelectedSourceDetails;
            //dto.SelectedAchivementDetails = MMD.SelectedAchivementDetails;
            //dto.SelectedBondDetails = MMD.SelectedBondDetails;
            //dto.SelectedPrevSchoolDetails = MMD.SelectedPrevSchoolDetails;
            //dto.SelectedSiblingDetails = MMD.SelectedSiblingDetails;
            //dto.SelectedGuardianDetails = MMD.SelectedGuardianDetails;
            //dto.transnumconfigsettings = MMD.transnumconfigsettings;

            //dto.admissionNumbering = MMD.admissionNumbering;

            //dto.ASMAY_Id = MMD.ASMAY_Id;
            //dto.documents_Names = names;
            //dto.documents_path = contentTypes;
            return StudentAdmissionStr.SaveData(MMD);
        }








        [HttpDelete]
        [Route("DeleteBondEntry/{id:int}")]
        public Adm_M_StudentDTO DeleteBondEntry(int ID)
        {
            return StudentAdmissionStr.DeleteBondEntry(ID);

        }

        [HttpDelete]
        [Route("DeleteEntry/{id:int}")]
        public Adm_M_StudentDTO DeleteEntry(int ID)
        {
            return StudentAdmissionStr.DeletedEntry(ID);

        }
        [Route("checkDuplicate")]
        public Adm_M_StudentDTO checkDuplicate([FromBody] Adm_M_StudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return StudentAdmissionStr.checkDuplicate(MMD);

        }
        [Route("getCaste")]
        public Adm_M_StudentDTO getCaste([FromBody] Adm_M_StudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return StudentAdmissionStr.getCaste(MMD);

        }
        [Route("yearwisetcstd")]
        public Adm_M_StudentDTO yearwisetcstd([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return StudentAdmissionStr.yearwisetcstd(data);
        }
        [Route("addtocart")]
        public Adm_M_StudentDTO addtocart([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return StudentAdmissionStr.addtocart(data);
        }
        [Route("SearchByColumn")]
        public Adm_M_StudentDTO SearchByColumn([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return StudentAdmissionStr.searchByClmn(data);
        }
        [Route("StateByCountryName")]
        public Adm_M_StudentDTO StateByCountryName([FromBody]Adm_M_StudentDTO countryname)
        {
            return StudentAdmissionStr.StateByCountryName(countryname);
        }

        [Route("classchangemaxminage")]
        public Adm_M_StudentDTO getmaxminage([FromBody] Adm_M_StudentDTO maxmin)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            maxmin.MI_Id = mid;

            return StudentAdmissionStr.getmaxminage(maxmin);
        }

        [Route("savefirsttab")]
        public Adm_M_StudentDTO savefirsttab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return StudentAdmissionStr.savefirsttab(data); 
        }
        [Route("savesecondtab")]
        public Adm_M_StudentDTO savesecondtab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return StudentAdmissionStr.savesecondtab(data);
        }
        [Route("savethirdtab")]
        public Adm_M_StudentDTO savethirdtab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return StudentAdmissionStr.savethirdtab(data);
        }

        [Route("savefourthtab")]
        public Adm_M_StudentDTO savefourthtab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return StudentAdmissionStr.savefourthtab(data);
        }
        [Route("savefinaltab")]
        public Adm_M_StudentDTO savefinaltab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return StudentAdmissionStr.savefinaltab(data);
        }
        
    }

}
