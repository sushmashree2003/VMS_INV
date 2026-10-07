
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace IssueManager.com.vaps.Services
{
    public class VMS_Live_InterviewImpl : Interfaces.VMS_Live_InterviewInterface
    {
        public VMSContext _context;
        public DomainModelMsSqlServerContext _db;
        public VMS_Live_InterviewImpl(VMSContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }

        public VMS_Live_InterviewDTO Getdetails(VMS_Live_InterviewDTO dto)
        {
            try
            {

                dto.HRME_Id = _context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                var interviewlist = _db.HR_CandidateInterviewScheduleDMO.Where(p => (p.HRCISC_Status == "Upcomming" || p.HRCISC_Status == "InProgress") && p.HRCISC_ActiveFlg == true).Select(p => p.HRCD_Id).Distinct().ToList();
                dto.get_userEmplist = (from emp in _context.HR_Candidate_DetailsDMO
                                       where emp.HRCD_ActiveFlg == true && emp.MI_Id == dto.MI_Id && !interviewlist.Contains(emp.HRCD_Id)
                                       select new VMS_Live_InterviewDTO
                                       {
                                           HRCD_Id = emp.HRCD_Id,
                                           //HRCD_FirstName = emp.HRCD_FirstName,
                                           HRCD_FirstName = emp.HRCD_FirstName + (string.IsNullOrEmpty(emp.HRCD_MiddleName) ? "" : ' ' + emp.HRCD_MiddleName) + (string.IsNullOrEmpty(emp.HRCD_LastName) ? "" : ' ' + emp.HRCD_LastName),

                                       }).Distinct().ToArray();

                dto.staffDetailsList = (from emp in _context.HR_Master_Employee_DMO
                                       where emp.HRME_LeftFlag == false && emp.HRME_ActiveFlag==true && emp.MI_Id == dto.MI_Id 
                                       select new VMS_Live_InterviewDTO
                                       {
                                           HRME_Id = emp.HRME_Id,
                                           HRME_EmployeeFirstName = emp.HRME_EmployeeFirstName + (string.IsNullOrEmpty(emp.HRME_EmployeeMiddleName) ? "" : ' ' + emp.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(emp.HRME_EmployeeLastName) ? "" : ' ' + emp.HRME_EmployeeLastName),


                                       }).Distinct().ToArray();
                
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LiveInterviewList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                  
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.vmslivemeetinglist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public VMS_Live_InterviewDTO GetOnloaddata(VMS_Live_InterviewDTO dto)
        {
            try
            {


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public VMS_Live_InterviewDTO savemeeting(VMS_Live_InterviewDTO dto)
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }

            var remoteIpAddress = "";
            //string netip = remoteIpAddress.ToString();
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            remoteIpAddress = addr[addr.Length - 1].ToString();
            string hostName = Dns.GetHostName();
            var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
            //  string myIP1 = ip_list.ToString();
            string myIP1 = addr[addr.Length - 2].ToString();

           var userid = _context.Staff_User_Login.Single(c => c.Emp_Code == dto.HRME_Id).Id;

            var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == dto.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            // var role = _context.ApplicationUserDMO.Where(a => a.UserId == dto.UserId).ToList();
           // var staff = _context.Staff_User_Login.Where(a => a.Id == dto.UserId).ToList();

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "VMS_OrderID";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                {
                    Value = dto.MI_Id
                });


                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    dto.orderid = retObject[0].orderid;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //VMS_Live_MeetingDMO MM = Mapper.Map<VMS_Live_MeetingDMO>(dto);

            VMS_Live_InterviewDMO LN = new VMS_Live_InterviewDMO();
            LN.MI_Id = dto.MI_Id;
            LN.VMSLMEET_MeetingId = "VMS-VC-1";
            LN.VMSLMEET_MeetingTopic = dto.VMSLMEET_MeetingTopic;
            LN.User_Id = userid;
            LN.HRME_Id = dto.HRME_Id;
            LN.VMSLMEET_PIPAddress = myIP1;
            LN.VMSLMEET_ActiveFlg = true;
            LN.VMSLMEET_PMACAddress = sMacAddress;
            LN.VMSLMEET_PlannedDate = dto.VMSLMEET_PlannedDate;
            LN.VMSLMEET_PlannedStartTime = dto.VMSLMEET_PlannedStartTime;
            LN.VMSLMEET_PlannedEndTime = dto.VMSLMEET_PlannedEndTime;
            LN.VMSLMEET_CreatedDate = DateTime.Today;
            LN.VMSLMEET_UpdatedDate = DateTime.Today;
            _context.Add(LN);
            _context.SaveChanges();
            int j = 0;
            if (dto.arrayuserEmp.Length > 0)
            {
               
                foreach (var item in dto.arrayuserEmp)
                {
                    VMS_Live_Meeting_CandidateOthersDMO ms = new VMS_Live_Meeting_CandidateOthersDMO();
                    ms.VMSLITVW_Id = LN.VMSLITVW_Id;
                    ms.User_Id = dto.UserId;
                    ms.HRCD_Id = item.HRCD_Id;
                    ms.VMSLMEETSTFOTH_ActiveFlg = true;
                    ms.VMSLMEETSTFOTH_CreatedDate = DateTime.Today;
                    ms.VMSLMEETSTFOTH_UpdatedDate = DateTime.Today;
                    ms.VMSLMEETSTFOTH_CreatedBy = dto.UserId;
                    _context.Add(ms);
                    j= _context.SaveChanges();
                    if(j > 0)
                    {
                        j = j + 1;
                    }
                  

                    dto.HRCD_Id = item.HRCD_Id;

                    dto = candidateRegistration(dto);

                }
            }
            //var check = _context.SaveChanges();
            if (j > 0)
            {
                dto.message = "Add";
            }
            else
            {
                dto.message = "Error";
            }
            return dto;
        }

        public VMS_Live_InterviewDTO getempdetails(VMS_Live_InterviewDTO dto)
        {
            try
            {
                dto.vcflag = "BBB";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                dto.HRME_Id = _context.VMS_Live_InterviewDMO.Where(c => c.User_Id == dto.UserId).FirstOrDefault().HRME_Id;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_CanidateInterview";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRCD_Id",
                   SqlDbType.BigInt)
                    {
                        Value = dto.HRCD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                    {
                        Value = indianTime.Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                       SqlDbType.VarChar)
                    {
                        Value = "BeforeMeeting"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                       SqlDbType.VarChar)
                    {
                        Value = "Start"
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.meetinglist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_CanidateInterview";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRCD_Id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.HRCD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                    {
                        Value = indianTime.Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                       SqlDbType.VarChar)
                    {
                        Value = "AfterMeeting"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                       SqlDbType.VarChar)
                    {
                        Value = "Start"
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.meetingstart = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public VMS_Live_InterviewDTO onstartmeeting(VMS_Live_InterviewDTO dto)
        {

            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");
                string meetingidinternal = "";
                bool start = false;
                //CREATE MEETING
                //var VCCODECREATE = _context.Institution.Where(c => c.MI_Id == dto.MI_Id && (c.MI_VCStudentFlag == "BBB")).ToList();
                //if (VCCODECREATE.Count() > 0)
                //{
                var CheckDuplicateMeeting = _context.VMS_Live_InterviewDMO.Where(c => c.VMSLMEET_MeetingId == dto.VMSLMEET_MeetingId && c.VMSLMEET_MeetingDate == null && (c.VMSLMEET_StartedTime == null || c.VMSLMEET_StartedTime == "")).ToList();
                if (CheckDuplicateMeeting.Count() == 1)
                {
                    start = true;
                    // BIGBLUE BUTTON START
                    Payment pay = new Payment(_db);
                    string moderatorpassword = "mp";
                    string attendeepassword = "ap";
                    string rnamee = dto.VMSLMEET_MeetingTopic;
                    rnamee = rnamee.Replace(" ", "+");
                    string generatedchecksum = "";
                    string logouturl = "http://localhost:57606/#/app/VMS_CandidateMeeting_Start/";
                    //string callbackurl = "http://localhost:51263/api/LiveMeetingScheduleFacade/callback?meetingID=" + data.LMSLMEET_MeetingId;
                    //string recordcallbackurl = "http://localhost:51263/api/LiveMeetingScheduleFacade/recordcallback";
                    string callbackurl = "https://portalhub.azurewebsites.net/api/LiveMeetingScheduleFacade/callback?meetingID=" + dto.VMSLMEET_MeetingId;
                    string recordcallbackurl = "https://portalhub.azurewebsites.net/api/LiveMeetingScheduleFacade/recordcallback";

                    logouturl = HttpUtility.UrlEncode(logouturl);
                    callbackurl = HttpUtility.UrlEncode(callbackurl);
                    recordcallbackurl = HttpUtility.UrlEncode(recordcallbackurl);

                    string hash_string = "name=randomname&attendeePW=Apwd&meetingID=Mid&moderatorPW=Mpwd&logoutURL=lgout&meta_endCallbackUrl=callback&record=true&meta_bbb-recording-ready-url=recordurl&lockSettingsDisablePrivateChat=true";

                    hash_string = hash_string.Replace("randomname", rnamee);
                    hash_string = hash_string.Replace("Apwd", attendeepassword);
                    hash_string = hash_string.Replace("Mid", dto.VMSLMEET_MeetingId);
                    hash_string = hash_string.Replace("Mpwd", moderatorpassword);
                    hash_string = hash_string.Replace("callback", callbackurl);
                    hash_string = hash_string.Replace("lgout", logouturl);
                    hash_string = hash_string.Replace("recordurl", recordcallbackurl);

                    generatedchecksum = pay.Generatehash256("create" + hash_string + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();

                    string urlBB = "https://conference.vapssmartecampus.com/bigbluebutton/api/create?" + hash_string + "&checksum=" + generatedchecksum;

                    System.Net.WebRequest req = System.Net.WebRequest.Create(urlBB);
                    req.Method = "GET";
                    // req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                    System.Net.WebResponse resp = req.GetResponseAsync().Result;
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string s = sr.ReadToEnd().Trim();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(s);
                    string jsonText = JsonConvert.SerializeXmlNode(doc);
                    var joResponse1 = JObject.Parse(jsonText);
                    meetingidinternal = Convert.ToString(joResponse1["response"]["internalMeetingID"]);
                    // BIGBLUE BUTTON END
                }
                // }

                //START MEETING MY MODERATER
                //var VCCODE = _db.Institution.Where(c => c.MI_Id == dto.MI_Id && (c.MI_VCStudentFlag == "BBB")).ToList();
                var VCCODE = 1;
                if (VCCODE > 0)
                {
                    var employee = _db.HR_Master_Employee_DMO.Where(c => c.MI_Id == dto.MI_Id && c.HRME_Id == dto.HRME_Id).ToList();
                    string empname = "staffname";
                    if (employee.Count() > 0)
                    {
                        empname = ((employee.FirstOrDefault().HRME_EmployeeFirstName == null ? "" : employee.FirstOrDefault().HRME_EmployeeFirstName.ToUpper()) + " " + (employee.FirstOrDefault().HRME_EmployeeMiddleName == null ? "" : employee.FirstOrDefault().HRME_EmployeeMiddleName.ToUpper()) + " " + (employee.FirstOrDefault().HRME_EmployeeLastName == null ? "" : employee.FirstOrDefault().HRME_EmployeeLastName.ToUpper())).Trim().ToString();
                    }
                    empname = empname.Trim();
                    empname = empname.Replace(" ", "+");
                    // join moderator url
                    Payment pay = new Payment(_db);

                    string hash_string = "fullName=" + empname + "&meetingID=" + dto.VMSLMEET_MeetingId + "&password=mp&redirect=true";

                    string generatedchecksum = pay.Generatehash256("join" + hash_string + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();

                    dto.moderatorurl = "https://conference.vapssmartecampus.com/bigbluebutton/api/join?" + hash_string + "&checksum=" + generatedchecksum;

                    // join moderator url
                }

                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();
                var ress = _context.VMS_Live_InterviewDMO.Single(e => e.VMSLITVW_Id == dto.VMSLITVW_Id);
                ress.VMSLMEET_MeetingDate = indianTime.Date;
                ress.VMSLMEET_StartedTime = time;
                if (start == true)
                {
                    ress.LMSLMEET_internalMeetingID = meetingidinternal;
                }
                ress.VMSLMEET_MACAddress = sMacAddress;
                ress.VMSLMEET_IPAddress = myIP1;
                ress.VMSLMEET_UpdatedBy = dto.UserId;
                ress.VMSLMEET_UpdatedDate = DateTime.Now;
                _context.Update(ress);
                var ss = _context.SaveChanges();
                if (ss > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = true;
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public VMS_Live_InterviewDTO endmainmeeting(VMS_Live_InterviewDTO dto)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                var ress = _context.VMS_Live_InterviewDMO.Single(e => e.VMSLITVW_Id == dto.VMSLITVW_Id);
                ress.VMSLMEET_EndTime = time;

                ress.VMSLMEET_MACAddress = sMacAddress;
                ress.VMSLMEET_IPAddress = myIP1;
                ress.VMSLMEET_UpdatedBy = dto.UserId;
                ress.VMSLMEET_UpdatedDate = DateTime.Now;
                _context.Update(ress);
                int ss = _context.SaveChanges();
                if (ss > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = true;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public VMS_Live_InterviewDTO getempdetails_join(VMS_Live_InterviewDTO dto)
        {
            try
            {
                dto.vcflag = "BBB";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //dto.HRME_Id = _context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                dto.HRCD_Id = _context.VMS_Live_Meeting_CandidateOthersDMO.Single(a => a.User_Id == dto.UserId).HRCD_Id;

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_CanidateInterview";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRCD_Id",
                   SqlDbType.BigInt)
                    {
                        Value = dto.HRCD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                    {
                        Value = indianTime.Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                       SqlDbType.VarChar)
                    {
                        Value = "BeforeMeeting"
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
                       SqlDbType.VarChar)
                    {
                        Value = "Join"
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.meetinglist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_CanidateInterview";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRCD_Id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.HRCD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                    {
                        Value = indianTime.Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                       SqlDbType.VarChar)
                    {
                        Value = "AfterMeeting"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                      SqlDbType.VarChar)
                    {
                        Value = "Join"
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.meetingstart = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public VMS_Live_InterviewDTO onstartmeeting_join(VMS_Live_InterviewDTO dto)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");
                string meetingidinternal = "";
                bool start = false;
                //CREATE MEETING
                //var VCCODECREATE = _context.Institution.Where(c => c.MI_Id == dto.MI_Id && (c.MI_VCStudentFlag == "BBB")).ToList();
                //if (VCCODECREATE.Count() > 0)
                //{
                var CheckDuplicateMeeting = _context.VMS_Live_InterviewDMO.Where(c => c.VMSLMEET_MeetingId == dto.VMSLMEET_MeetingId && c.VMSLMEET_MeetingDate == null && (c.VMSLMEET_StartedTime == null || c.VMSLMEET_StartedTime == "")).ToList();
                if (CheckDuplicateMeeting.Count() == 1)
                {
                    start = true;
                    // BIGBLUE BUTTON START
                    Payment pay = new Payment(_db);
                    string moderatorpassword = "mp";
                    string attendeepassword = "ap";
                    string rnamee = dto.VMSLMEET_MeetingTopic;
                    rnamee = rnamee.Replace(" ", "+");
                    string generatedchecksum = "";
                    string logouturl = "http://localhost:57606/#/app/VMS_CandiateMeeting_Join/";
                    //string callbackurl = "http://localhost:51263/api/LiveMeetingScheduleFacade/callback?meetingID=" + data.LMSLMEET_MeetingId;
                    //string recordcallbackurl = "http://localhost:51263/api/LiveMeetingScheduleFacade/recordcallback";

                    string callbackurl = "https://portalhub.azurewebsites.net/api/LiveMeetingScheduleFacade/callback?meetingID=" + dto.VMSLMEET_MeetingId;
                    string recordcallbackurl = "https://portalhub.azurewebsites.net/api/LiveMeetingScheduleFacade/recordcallback";

                    logouturl = HttpUtility.UrlEncode(logouturl);
                    callbackurl = HttpUtility.UrlEncode(callbackurl);
                    recordcallbackurl = HttpUtility.UrlEncode(recordcallbackurl);

                    string hash_string = "name=randomname&attendeePW=Apwd&meetingID=Mid&moderatorPW=Mpwd&logoutURL=lgout&meta_endCallbackUrl=callback&record=true&meta_bbb-recording-ready-url=recordurl&lockSettingsDisablePrivateChat=true";

                    hash_string = hash_string.Replace("randomname", rnamee);
                    hash_string = hash_string.Replace("Apwd", attendeepassword);
                    hash_string = hash_string.Replace("Mid", dto.VMSLMEET_MeetingId);
                    hash_string = hash_string.Replace("Mpwd", moderatorpassword);
                    hash_string = hash_string.Replace("callback", callbackurl);
                    hash_string = hash_string.Replace("lgout", logouturl);
                    hash_string = hash_string.Replace("recordurl", recordcallbackurl);

                    generatedchecksum = pay.Generatehash256("create" + hash_string + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();

                    string urlBB = "https://conference.vapssmartecampus.com/bigbluebutton/api/create?" + hash_string + "&checksum=" + generatedchecksum;

                    System.Net.WebRequest req = System.Net.WebRequest.Create(urlBB);
                    req.Method = "GET";
                    // req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                    System.Net.WebResponse resp = req.GetResponseAsync().Result;
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string s = sr.ReadToEnd().Trim();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(s);
                    string jsonText = JsonConvert.SerializeXmlNode(doc);
                    var joResponse1 = JObject.Parse(jsonText);
                    meetingidinternal = Convert.ToString(joResponse1["response"]["internalMeetingID"]);
                    // BIGBLUE BUTTON END
                }
                // }

                //START MEETING MY MODERATER
                //var VCCODE = _db.Institution.Where(c => c.MI_Id == dto.MI_Id && (c.MI_VCStudentFlag == "BBB")).ToList();
                var VCCODE = 1;
                if (VCCODE > 0)
                {
                    var candidatedetails = _db.HR_Candidate_DetailsDMO.Where(c =>c.HRCD_Id == dto.HRCD_Id).ToList();
                    string canidatename = "Candidate";
                    if (candidatedetails.Count() > 0)
                    {
                        canidatename = ((candidatedetails.FirstOrDefault().HRCD_FirstName == null ? "" : candidatedetails.FirstOrDefault().HRCD_FirstName.ToUpper()) + " " + (candidatedetails.FirstOrDefault().HRCD_MiddleName == null ? "" : candidatedetails.FirstOrDefault().HRCD_MiddleName.ToUpper()) + " " + (candidatedetails.FirstOrDefault().HRCD_LastName == null ? "" : candidatedetails.FirstOrDefault().HRCD_LastName.ToUpper())).Trim().ToString();
                    }
                    canidatename = canidatename.Trim();
                    canidatename = canidatename.Replace(" ", "+");
                    // join moderator url
                    Payment pay = new Payment(_db);

                    string hash_string = "fullName=" + candidatedetails + "&meetingID=" + dto.VMSLMEET_MeetingId + "&password=mp&redirect=true";

                    string generatedchecksum = pay.Generatehash256("join" + hash_string + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();

                    dto.moderatorurl = "https://conference.vapssmartecampus.com/bigbluebutton/api/join?" + hash_string + "&checksum=" + generatedchecksum;

                    // join moderator url
                }

                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();
                var ress = _context.VMS_Live_Meeting_CandidateOthersDMO.Single(e => e.VMSLITVW_Id == dto.VMSLITVW_Id && e.HRCD_Id == dto.HRCD_Id);
                ress.VMSLMEETSTFOTH_LoginTime = time;
                ress.VMSLMEETSTFOTH_MACAddress = sMacAddress;
                ress.VMSLMEETSTFOTH_IPAddress = myIP1;
                ress.VMSLMEETSTFOTH_UpdatedBy = dto.UserId;
                ress.VMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                _context.Update(ress);
                var ss = _context.SaveChanges();
                if (ss > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = true;
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public VMS_Live_InterviewDTO endmainmeeting_join(VMS_Live_InterviewDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();


                var ress = _context.VMS_Live_Meeting_CandidateOthersDMO.Single(e => e.VMSLITVW_Id == data.VMSLITVW_Id && e.HRCD_Id == data.HRCD_Id);
                ress.VMSLMEETSTFOTH_LogoutTime = time;

                ress.VMSLMEETSTFOTH_MACAddress = sMacAddress;
                ress.VMSLMEETSTFOTH_IPAddress = myIP1;
                ress.VMSLMEETSTFOTH_UpdatedBy = data.UserId;
                ress.VMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                _context.Update(ress);
                int ss = _context.SaveChanges();
                if (ss > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }



            }



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public VMS_Live_InterviewDTO GetReport(VMS_Live_InterviewDTO dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LiveInterviewReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                   SqlDbType.VarChar)
                    {
                        Value = dto.ToDate
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.vmslivemeetingreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public VMS_Live_InterviewDTO candidateRegistration(VMS_Live_InterviewDTO dto)
        {
            long count = 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var username = "";
                    var candidatelist = _context.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == dto.HRCD_Id && t.HRCD_ActiveFlg == true).ToList();
                    var dublicatecheck1 = _context.ApplicationUserDMO.Where(t=>t.RoleTypeFlag== "New Candidate").ToArray();
                    foreach (var dub in dublicatecheck1)
                    {
                        if (candidatelist[0].HRCD_CreatedBy == dub.Id)
                        {
                            var update = _context.ApplicationUserDMO.Where(t => t.Id == dub.Id).ToList();
                             username = dub.UserName;

                            var sheduledinterview = _context.VMS_Live_Meeting_CandidateOthersDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).FirstOrDefault();
                            if (sheduledinterview.HRCD_Id > 0)
                            {
                                sheduledinterview.User_Id = dub.Id;
                                _context.Update(sheduledinterview);
                                count = _context.SaveChanges();
                            }

                            var password = "Password@123";
                            sendmail(dto.MI_Id, candidatelist[0].HRCD_EmailId, "VcInterveiwTemplate", dto.VMSLMEET_PlannedDate, dto.VMSLMEET_PlannedStartTime, dto.VMSLMEET_PlannedEndTime, username, password);

                        }
                    }

                    if (username.Length == 0)
                    {

                        username = "Vaps" + candidatelist[0].HRCD_FirstName + candidatelist[0].HRCD_FirstName.Trim();
                        var dublicatecheck = _context.ApplicationUserDMO.Where(t => t.UserName == username).ToList();
                        if (dublicatecheck.Count > 0)
                        {
                            var dubliusername = username.Count() + 1;
                            username = dubliusername + username;
                        }

                        ApplicationUserDMO user = new ApplicationUserDMO();
                        user.AccessFailedCount = 0;
                        user.ConcurrencyStamp = "";
                        user.Email = candidatelist[0].HRCD_EmailId;
                        user.EmailConfirmed = true;
                        user.LockoutEnabled = true;
                        user.NormalizedEmail = candidatelist[0].HRCD_EmailId;
                        user.NormalizedUserName = username;
                        user.PasswordHash = "AQAAAAEAACcQAAAAEPhnWI7McHZqXzg5vE1PUDo4WVzezsjaMX8ZBj1agl6az3F7JjxQtHp2VSIx7oYRYg==";
                        user.PhoneNumber = candidatelist[0].HRCD_MobileNo.ToString();
                        user.PhoneNumberConfirmed = false;
                        user.SecurityStamp = "";
                        user.TwoFactorEnabled = false;
                        user.UserName = username;
                        user.Entry_Date = DateTime.Now;
                        user.Machine_Ip_Address = null;
                        //user.UserImagePath = null;
                        user.Entry_Date = DateTime.Now;
                        user.CreatedDate = DateTime.Now;
                        user.UpdatedDate = DateTime.Now;
                        user.RoleTypeFlag = "New Candidate";
                        user.Name = username;
                        _context.Add(user);
                        var n = _context.SaveChanges();


                        var sheduledinterview = _context.VMS_Live_Meeting_CandidateOthersDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).FirstOrDefault();
                        if (sheduledinterview.HRCD_Id > 0)
                        {
                            sheduledinterview.User_Id = user.Id;
                            _context.Update(sheduledinterview);
                            _context.SaveChanges();
                        }


                        if (n > 0)
                        {
                            var applicationrole = _db.applicationRole.Where(t => t.roleType == "New Candidate").ToList();
                            var roletype = _db.MasterRoleType.Where(t => t.IVRMRT_RoleFlag == "New Candidate" && t.IVRMR_Id == applicationrole[0].Id).ToList();
                            DataAccessMsSqlServerProvider.ApplicationUserRole appuserrole = new DataAccessMsSqlServerProvider.ApplicationUserRole();

                            appuserrole.UserId = user.Id;
                            appuserrole.RoleId = applicationrole[0].Id;
                            appuserrole.RoleTypeId = roletype[0].IVRMRT_Id;
                            _context.Add(appuserrole);

                            UserRoleWithInstituteDMO userrole = new UserRoleWithInstituteDMO();
                            userrole.MI_Id = 17;
                            userrole.Id = user.Id;
                            userrole.Activeflag = 1;
                            userrole.CreatedDate = DateTime.Now;
                            userrole.UpdatedDate = DateTime.Now;
                            _context.Add(userrole);
                            count = _context.SaveChanges();
                            if (count > 0)
                            {

                                var password = "Password@123";

                                if (candidatelist[0].HRCD_EmailId.Length > 0)
                                {
                                    sendmail(dto.MI_Id, candidatelist[0].HRCD_EmailId, "VcInterveiwTemplate", dto.VMSLMEET_PlannedDate, dto.VMSLMEET_PlannedStartTime, dto.VMSLMEET_PlannedEndTime, username, password);
                                }
                            }
                        }
                        
                        transaction.Commit();

                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);

                }
            }

            return dto;
        }



        public string sendmail(long MI_Id, string Email, string Template, DateTime? planneddate, string starttime, string endtime, string username, string password)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _context.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                //var Empname = (from c in _context.HR_Master_Employee_DMO
                //               where (c.HRME_Id == HRMEID)
                //               select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();
                //var approvarname = (from a in _context.Staff_User_Login
                //                    from c in _context.HR_Master_Employee_DMO
                //                    where (a.Id == userid && a.Emp_Code == c.HRME_Id)
                //                    select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();

                //string Empname = _context.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).Select(p => p.HRME_EmployeeFirstName).FirstOrDefault();
                Mailmsg = Mailmsg.Replace("[USR]", username);
                Mailmsg = Mailmsg.Replace("[PASSWORD]", password);
                //  Mailmsg = Mailmsg.Replace("[DATE]", planneddate.ToString("dd/MM/yyyy"));
                Mailmsg = Mailmsg.Replace("[TIME]", starttime);
                Mailmsg = Mailmsg.Replace("[ETIME]", endtime);
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";

                    //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    //{
                    //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                    //    mailcc = ccmail[0].ToString();

                    //    if (ccmail.Length > 1)
                    //    {
                    //        if (ccmail[1] != null || ccmail[1] != "")
                    //        {
                    //            mailbcc = ccmail[1].ToString();
                    //        }
                    //    }

                    //}
                    //if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    //{
                    //    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    //}

                    var message = new SendGridMessage();
                    //if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    //{
                    //    string[] mail_id = template[0].ISES_MailCCId.Split(',');
                    //    if (mail_id.Length > 0)
                    //    {
                    //        for (int i = 0; i < mail_id.Length; i++)
                    //        {
                    //            message.AddTo(mail_id[i]);

                    //        }
                    //    }
                    //}
                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] mail_id = template[0].ISES_MailCCId.Split(',');
                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddCc(mail_id[i]);

                            }
                        }
                    }
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }

                    //if (mailcc != null && mailcc != "")
                    //{
                    //    message.AddCc(mailcc);
                    //}
                    //if (mailbcc != null && mailbcc != "")
                    //{
                    //    message.AddBcc(mailbcc);
                    //}
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}
