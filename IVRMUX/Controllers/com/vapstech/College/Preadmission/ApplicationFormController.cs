using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.College.Preadmission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Preadmission
{
    [Route("api/[controller]")]
    public class ApplicationFormController : Controller
    {
        ApplicationFormDelegate delegates = new ApplicationFormDelegate();
        StudentApplicationDelegate sad = new StudentApplicationDelegate();
        [Route("Getdetails")]
        public CollegePreadmissionstudnetDto Getdetails()
        {
            CollegePreadmissionstudnetDto dto = new CollegePreadmissionstudnetDto();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ID = UserId;

            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegates.Getdetails(dto);
        }
        [Route("getCourse/{YearId:int}")]
        public CollegePreadmissionstudnetDto getCourse(int YearId)
        {
            CollegePreadmissionstudnetDto obj = new CollegePreadmissionstudnetDto();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            obj.ASMAY_Id = YearId;
            return delegates.getCourse(obj);
        }

        [Route("Dashboarddetails")]
        public CollegePreadmissionstudnetDto Dashboarddetails([FromBody] CollegePreadmissionstudnetDto ctry)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ctry.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ctry.ID = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            ctry.ASMAY_Id = ASMAY_Id;

            ctry.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            //return sad.getIndependentDropDowns(ctry);
            return delegates.Dashboarddetails(ctry);
        }

        [Route("paynow")]
        public CollegePreadmissionstudnetDto paynow([FromBody] CollegePreadmissionstudnetDto data)
        {
            CollegePreadmissionstudnetDto dt = new CollegePreadmissionstudnetDto();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ID = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;



            return delegates.paynow(data);
        }
        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            StudentApplicationDTO schoolimp = new StudentApplicationDTO();
            schoolimp.MI_Id= Convert.ToInt64(response.udf3);
            string querystring = "";
            var sub = "";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();
            dtoapp.MI_Id = Convert.ToInt64(response.udf3);
            dtoapp = getdashboardpage(schoolimp);
            if (dtoapp.dashboardpage != null)
            {
                sub = dtoapp.dashboardpage;
            }
            else
            {
                sub = "hutchings";
            }

            try
            {
                dto = delegates.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/ 12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }

            return Redirect(querystring);
        }


        [Route("paymentresponsepaytm/")]
        public ActionResult paymentresponse(PaymentDetails.PAYTM response)
        {

            var sub = "";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();

            string[] tokens = response.MERC_UNQ_REF.Split('_');

            dtoapp.MI_Id = Convert.ToInt32(tokens[6]);

            dtoapp.MI_Id = Convert.ToInt64(dtoapp.MI_Id);
            dtoapp = getdashboardpage(dtoapp);
            if (dtoapp.dashboardpage != null)
            {
                sub = dtoapp.dashboardpage;
            }
            else
            {
                sub = "hutchings";
            }

            PaymentDetails.PAYTM dto = new PaymentDetails.PAYTM();

            string querystring = "";
            try
            {
                dto = delegates.getpaymentresponsepaytm(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/ 12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        [Route("getdashboardpage")]
        public StudentApplicationDTO getdashboardpage([FromBody] StudentApplicationDTO data)
        {
            if (data.MI_Id == 0)
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            else
            {
                data.MI_Id = data.MI_Id;

            }
            return sad.getdashboardpage(data);
        }


        [Route("getBranch")]
        public CollegePreadmissionstudnetDto getBranch([FromBody]CollegePreadmissionstudnetDto dt)
        {
            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return delegates.getBranch(dt);
        }
        [Route("getSemester")]
        public CollegePreadmissionstudnetDto getSemester([FromBody]CollegePreadmissionstudnetDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return delegates.getSemester(dto);
        }
        [Route("getCaste")]
        public CollegePreadmissionstudnetDto getCaste([FromBody] CollegePreadmissionstudnetDto caste)
        {
            caste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getCaste(caste);
        }
        //[Route("getQuotaCategory/{quotaId:int}")]
        //public AdmMasterCollegeStudentDTO getQuotaCategory(int quotaId)
        //{
        //    AdmMasterCollegeStudentDTO dto = new AdmMasterCollegeStudentDTO();
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.ACQ_Id = quotaId;
        //    return delegates.getQuotaCategory(dto);
        //}
        [Route("saveStudentDetails")]
        public CollegePreadmissionstudnetDto saveStudentDetails([FromBody]CollegePreadmissionstudnetDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ID= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delegates.saveStudentDetails(dto);
        }
        [Route("Edit/{Id:int}")]
        public CollegePreadmissionstudnetDto Edit(int Id)
        {
            CollegePreadmissionstudnetDto edit = new CollegePreadmissionstudnetDto();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            edit.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            edit.PACA_Id = Id;
            return delegates.Edit(edit);
        }
        [Route("getprintdata/{Id:int}")]
        public CollegePreadmissionstudnetDto getprintdata(int Id)
        {
            CollegePreadmissionstudnetDto edit = new CollegePreadmissionstudnetDto();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            edit.PACA_Id = Id;
            return delegates.getprintdata(edit);
        }
        //[Route("checkDuplicate")]
        //public AdmMasterCollegeStudentDTO checkDuplicate([FromBody]AdmMasterCollegeStudentDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return delegates.checkDuplicate(data);
        //}
        [Route("getdpstate/{countryId:int}")]
        public CollegePreadmissionstudnetDto getdpstate(int countryId)
        {
            CollegePreadmissionstudnetDto st = new CollegePreadmissionstudnetDto();
            st.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            st.IVRMMC_Id = countryId;
            return delegates.getdpstate(st);
        }

        [Route("saveAddress")]
        public CollegePreadmissionstudnetDto saveAddress([FromBody]CollegePreadmissionstudnetDto add)
        {
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveAddress(add);
        }
        [Route("saveParentsDetails")]
        public CollegePreadmissionstudnetDto saveParentsDetails([FromBody]CollegePreadmissionstudnetDto parentsData)
        {
            parentsData.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveParentsDetails(parentsData);
        }
        [Route("StateByCountryName")]
        public CollegePreadmissionstudnetDto StateByCountryName([FromBody]CollegePreadmissionstudnetDto country)
        {
            country.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.StateByCountryName(country);
        }
        [Route("saveOthersDetails")]
        public CollegePreadmissionstudnetDto saveOthersDetails([FromBody]CollegePreadmissionstudnetDto others)
        {
            others.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveOthersDetails(others);
        }
        [Route("saveDocuments")]
        public CollegePreadmissionstudnetDto saveDocuments([FromBody]CollegePreadmissionstudnetDto documents)
        {
            documents.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveDocuments(documents);
        }

        //[Route("SearchByColumn")]
        //public AdmMasterCollegeStudentDTO SearchByColumn([FromBody]AdmMasterCollegeStudentDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return delegates.SearchByColumn(data);
        //}
        //[Route("DeleteEntry")]
        //public AdmMasterCollegeStudentDTO DeleteEntry([FromBody]AdmMasterCollegeStudentDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return delegates.DeleteEntry(data);
        //}
    }
}
