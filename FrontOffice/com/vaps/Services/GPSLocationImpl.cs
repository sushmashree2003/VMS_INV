using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class GPSLocationImpl : Interfaces.GPSLocationInterface
    {
        public FOContext _FOContext;

        public GPSLocationImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }
        public VMS_Attendance_GPSLocationDTO getdata(VMS_Attendance_GPSLocationDTO data)
        {
            try
            {

                long hrmeid = (from q in _FOContext.Staff_User_Login
                               from r in _FOContext.HR_Master_Employee_DMO
                               where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId && r.MI_Id == data.MI_Id)
                               select (r.HRME_Id)).FirstOrDefault();

                if (hrmeid > 0)
                {
                    data.employee_name = (from a in _FOContext.HR_Master_Employee_DMO
                                          from b in _FOContext.HR_Master_Designation_DMO
                                          from d in _FOContext.Multiple_Email_DMO
                                          from e in _FOContext.Multiple_Mobile_DMO
                                          where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == hrmeid && d.HRME_Id == a.HRME_Id && e.HRME_Id == a.HRME_Id && d.HRMEM_DeFaultFlag == "default" && e.HRMEMNO_DeFaultFlag == "default")
                                          select new VMS_Attendance_GPSLocationDTO
                                          {
                                              HRME_Id = a.HRME_Id,
                                              HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                              HRME_DOJ = a.HRME_DOJ,
                                              HRME_MobileNo = e.HRMEMNO_MobileNo,
                                              HRME_EmailId = d.HRMEM_EmailId,
                                              HRMDES_DesignationName = b.HRMDES_DesignationName,
                                              HRME_EmployeeCode = a.HRME_EmployeeCode
                                          }).Distinct().ToArray();
                }

                data.clientlist = _FOContext.ISM_Master_Client_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMCLT_ActiveFlag==true).ToArray();
                data.saleslist = _FOContext.ISM_Sales_Lead_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMSLE_ActiveFlag==true && t.ISMSLE_VerifiedFlg==true).ToArray();

                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GPSDETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = hrmeid
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.empdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
              

                try
                {
                    var url1 = "https://api.opencagedata.com/geocode/v1/json?key=bbb18659a9b24fb1889a430863a2abbb&q=" + data.VMSATGPSL_LATLONGValue + "&pretty=1&no_annotations=1";
                    System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                    System.Net.HttpWebResponse response1 = request1.GetResponse() as System.Net.HttpWebResponse;
                    Stream stream1 = response1.GetResponseStream();

                    StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                    string responseparameters1 = readStream1.ReadToEnd();
                    var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                    dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                    var statusdetails = JsonConvert.DeserializeObject<VMS_Attendance_GPSLocationDTO>(responsedata1);
                    data.VMSATGPSL_Address = statusdetails.results[0].formatted;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_Attendance_GPSLocationDTO savedata(VMS_Attendance_GPSLocationDTO data)
        {
            try
            {
                long hrmeid = (from q in _FOContext.Staff_User_Login
                              from r in _FOContext.HR_Master_Employee_DMO
                              where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId && r.MI_Id == data.MI_Id)
                              select (r.HRME_Id)).FirstOrDefault();


                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                VMS_Attendance_GPSLocationDMO OBJ = new VMS_Attendance_GPSLocationDMO();
                OBJ.MI_Id = data.MI_Id;
                OBJ.HRME_Id = hrmeid;
                OBJ.ISMMCLT_Id = data.ISMMCLT_Id;
                OBJ.ISMSLE_Id = data.ISMSLE_Id;
                OBJ.VMSATGPSL_DateTime = indiantime0;
                OBJ.VMSATGPSL_Address = data.VMSATGPSL_Address;
                OBJ.VMSATGPSL_LATLONGValue = data.VMSATGPSL_LATLONGValue;
                OBJ.VMSATGPSL_Remarks = data.VMSATGPSL_Remarks;
                OBJ.VMSATGPSL_ActiveFlg = true;
                OBJ.VMSATGPSL_CreatedDate = indiantime0;
                OBJ.VMSATGPSL_UpdatedDate = indiantime0;
                OBJ.VMSATGPSL_CreatedBy = data.UserId;
                OBJ.VMSATGPSL_UpdatedBy = data.UserId;
                _FOContext.Add(OBJ);
                var count = _FOContext.SaveChanges();
                if(count > 0)
                {
                    data.returnmsg = "Add";
                }
                else
                {
                    data.returnmsg = "Failed";
                }
                if (data.punchflag==true)
                {
                    var contactExistsP = _FOContext.Database.ExecuteSqlCommand("GPS_Punch_Insert @p0,@p1", hrmeid, data.MI_Id);
                    if (contactExistsP > 0)
                    {
                        data.returnmsg = "Add";
                    }
                    else
                    {
                        data.returnmsg = "Failed";
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
