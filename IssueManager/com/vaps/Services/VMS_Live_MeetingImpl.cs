
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace IssueManager.com.vaps.Services
{
    public class VMS_Live_MeetingImpl : Interfaces.VMS_Live_MeetingInterface
    {
        public IssueManagerContext _context;
        public DomainModelMsSqlServerContext _db;
        public VMS_Live_MeetingImpl(IssueManagerContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }

        public VMS_Live_MeetingDTO Getdetails(VMS_Live_MeetingDTO dto)
        {
            try
            {

                dto.HRME_Id = _context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_EmployeeList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    //cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    //{
                    //    Value = "Employee"
                    //});
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
                        dto.get_userEmplist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LiveMeetingList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = "VMSList"
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

        public VMS_Live_MeetingDTO savemeeting(VMS_Live_MeetingDTO dto)
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


            var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == dto.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

            var role = _context.ApplicationUserRole_con.Where(a => a.UserId == dto.UserId).ToList();
            var staff = _context.Staff_User_Login.Where(a => a.Id == dto.UserId).ToList();

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


            VMS_Live_MeetingDMO LN = new VMS_Live_MeetingDMO();
            LN.MI_Id = dto.MI_Id;
            LN.VMSLMEET_MeetingId = "VMS-VC-1";
            LN.VMSLMEET_MeetingTopic = dto.VMSLMEET_MeetingTopic;
            LN.User_Id = dto.UserId;
            LN.HRME_Id = staff[0].Emp_Code;
            LN.VMSLMEET_PIPAddress = myIP1;
            LN.VMSLMEET_ActiveFlg = true;
            LN.VMSLMEET_PMACAddress = sMacAddress;
            LN.VMSLMEET_PlannedDate = dto.VMSLMEET_PlannedDate;
            LN.VMSLMEET_PlannedStartTime = dto.VMSLMEET_PlannedStartTime;
            LN.VMSLMEET_PlannedEndTime = dto.VMSLMEET_PlannedEndTime;
            LN.VMSLMEET_CreatedDate = DateTime.Today;
            LN.VMSLMEET_UpdatedDate = DateTime.Today;
            _context.Add(LN);

            if (dto.arrayuserEmp.Length > 0)
            {
                foreach (var item in dto.arrayuserEmp)
                {
                    VMS_Live_Meeting_StaffOthersDMO ms = new VMS_Live_Meeting_StaffOthersDMO();
                    ms.VMSLMEET_Id = LN.VMSLMEET_Id;
                    ms.User_Id = dto.UserId;
                    ms.HRME_Id = item.HRME_Id;
                    ms.VMSLMEETSTFOTH_ActiveFlg = true;
                    ms.VMSLMEETSTFOTH_CreatedDate = DateTime.Today;
                    ms.VMSLMEETSTFOTH_UpdatedDate = DateTime.Today;
                    ms.VMSLMEETSTFOTH_CreatedBy = dto.UserId;
                    _context.Add(ms);
                }


            }
            var check = _context.SaveChanges();
            if (check > 0)
            {
                dto.message = "Add";
            }
            else
            {
                dto.message = "Error";
            }
            return dto;
        }

        public VMS_Live_MeetingDTO getempdetails(VMS_Live_MeetingDTO dto)
        {
            try
            {
                //var VCCODE = _context.Institution.Where(c => c.MI_Id == dto.MI_Id && (c.MI_VCStudentFlag != null && c.MI_VCStudentFlag != "")).ToList();
                //if (VCCODE.Count() > 0)
                //{
                //    dto.vcflag = VCCODE.FirstOrDefault().MI_VCStudentFlag;
                //}
                //else
                //{
                //    dto.vcflag = "BBB";
                //}
                dto.vcflag = "JITSI";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                dto.HRME_Id = _context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_StaffMeeting";
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
                    cmd.CommandText = "VMS_StaffMeeting";
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

        public VMS_Live_MeetingDTO onstartmeeting(VMS_Live_MeetingDTO dto)
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
                var CheckDuplicateMeeting = _context.VMS_Live_MeetingDMO_con.Where(c => c.VMSLMEET_MeetingId == dto.VMSLMEET_MeetingId && c.VMSLMEET_MeetingDate == null && (c.VMSLMEET_StartedTime == null || c.VMSLMEET_StartedTime == "")).ToList();
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
                    string logouturl = "http://localhost:57606/#/app/VMS_StaffMeeting_Start/";
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
                var ress = _context.VMS_Live_MeetingDMO_con.Single(e => e.VMSLMEET_Id == dto.VMSLMEET_Id);
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

        public VMS_Live_MeetingDTO endmainmeeting(VMS_Live_MeetingDTO dto)
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

                var ress = _context.VMS_Live_MeetingDMO_con.Single(e => e.VMSLMEET_Id == dto.VMSLMEET_Id);
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


        public VMS_Live_MeetingDTO getempdetails_join(VMS_Live_MeetingDTO dto)
        {
            try
            {

                dto.vcflag = "JITSI";
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                dto.HRME_Id = _context.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_StaffMeeting";
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
                    cmd.CommandText = "VMS_StaffMeeting";
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

        public VMS_Live_MeetingDTO onstartmeeting_join(VMS_Live_MeetingDTO dto)
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
                var CheckDuplicateMeeting = _context.VMS_Live_MeetingDMO_con.Where(c => c.VMSLMEET_MeetingId == dto.VMSLMEET_MeetingId && c.VMSLMEET_MeetingDate == null && (c.VMSLMEET_StartedTime == null || c.VMSLMEET_StartedTime == "")).ToList();
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
                    string logouturl = "http://localhost:57606/#/app/VMS_StaffMeeting_Join/";
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
                var ress = _context.VMS_Live_Meeting_StaffOthersDMO_con.Single(e => e.VMSLMEET_Id == dto.VMSLMEET_Id && e.HRME_Id == dto.HRME_Id);
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
        public VMS_Live_MeetingDTO endmainmeeting_join(VMS_Live_MeetingDTO data)
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


                var ress = _context.VMS_Live_Meeting_StaffOthersDMO_con.Single(e => e.VMSLMEET_Id == data.VMSLMEET_Id && e.HRME_Id == data.HRME_Id);
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

        public VMS_Live_MeetingDTO GetReport(VMS_Live_MeetingDTO dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LiveMeetingReport";
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
    }
}
