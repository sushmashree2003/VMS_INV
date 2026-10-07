using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using DomainModel.Model.com.vapstech.FMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Services
{
    public class ModuleFeedbackImpl : Interface.ModuleFeedbackInterface
    {
        public FMSContext _FMSContext;
        public ModuleFeedbackImpl(FMSContext FMS)
        {
            _FMSContext = FMS;
        }

        public ModuleFeedbackDTO getfeedback(ModuleFeedbackDTO data)
        {
            try
            {
                var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == data.userid).ToList();
                if (getempid.Count > 0)
                {
                    var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();
                    data.HRME_Id = empdetails[0].HRME_Id;
                    data.useronloadfeedback = _FMSContext.ModuleFeedback.Where(F => F.MI_Id == data.MI_Id && F.HRME_Id == data.HRME_Id && F.VMF_ActiveFlag == true).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public ModuleFeedbackDTO submitfeedback(ModuleFeedbackDTO x)
        {
            try
            {

                if (x.nofeedbackfound == "silerjubilee")
                {
                    var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == x.userid).ToList();

                    if (getempid.Count > 0)
                    {
                        var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();
                        x.HRME_Id = empdetails[0].HRME_Id;
                    }

                    var contactExistsP = _FMSContext.Database.ExecuteSqlCommand("SilverJubilee_Suggestions_insert @p0,@p1,@p2,@p3,@p4,@p5", x.MI_Id, x.HRME_Id, x.feedbacktask, x.VMF_Rating, x.userid, "Insert");
                    if (contactExistsP >= 1)
                    {
                        x.existfeedback = "saved";
                    }

                }

                else
                {
                    DateTime revievwmonth = DateTime.Now;
                    int currentmonth1 = DateTime.Now.Month;
                    var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == x.userid).ToList();
                    if (getempid.Count > 0)
                    {
                        var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();
                        x.HRME_Id = empdetails[0].HRME_Id;
                    }
                    //checking the exist feedback for the current month
                    var priorfeedback = _FMSContext.ModuleFeedback.Where(F => F.HRME_Id == x.HRME_Id && F.IVRMM_Id == x.IVRMM_Id && F.VMF_CreatedDate.Month == revievwmonth.Month).ToList();
                    if (priorfeedback.Count > 0)
                    {
                        x.existfeedback = "exist";

                    }
                    else
                    {
                        List<ModuleFeedbackDTO> createdlist_task = new List<ModuleFeedbackDTO>();
                        if (x.createtask == true)
                        {
                            // List<ModuleFeedbackDTO> createdlist_task = new List<ModuleFeedbackDTO>();
                            if (getempid.Count > 0)
                            {
                                var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();
                                x.HRME_Id = empdetails[0].HRME_Id;
                            }


                            using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_Auto_TaskCreation";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = x.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = x.HRME_Id });
                                cmd.Parameters.Add(new SqlParameter("@Desc", SqlDbType.VarChar) { Value = x.VMF_Remarks });
                                cmd.Parameters.Add(new SqlParameter("@BugOREnh", SqlDbType.VarChar) { Value = x.BugOREnh });
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
                                            createdlist_task.Add(new ModuleFeedbackDTO
                                            {
                                                feedbacktask = Convert.ToString(dataReader["feedbacktask"]),

                                            });
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                        }

                        //----------------------------------------------------

                        long rating;
                        if (x.feedbacktype == "Outstanding")
                        {
                            rating = 5;
                        }
                        else if (x.feedbacktype == "Excellent")
                        {
                            rating = 4;
                        }
                        else if (x.feedbacktype == "Good")
                        {
                            rating = 3;
                        }
                        else if (x.feedbacktype == "Average")
                        {
                            rating = 2;
                        }
                        else
                        {
                            rating = 1;
                        }
                        ModuleFeedbackDMO MFD = new ModuleFeedbackDMO();
                        MFD.IVRMM_Id = x.IVRMM_Id;
                        MFD.VMF_Rating = rating;
                        MFD.VMF_ModuleName = x.VMF_ModuleName;
                        MFD.HRME_Id = x.HRME_Id;
                        MFD.MI_Id = x.MI_Id;
                        MFD.VMF_Remarks = x.VMF_Remarks;
                        MFD.VMF_ActiveFlag = true;
                        MFD.VMF_UpdatedDate = DateTime.Now;
                        MFD.VMF_CreatedDate = DateTime.Now;
                        MFD.VMF_CreatedBy = x.userid.Value;
                        MFD.VMF_UpdatedBy = x.userid.Value;
                        if (x.createtask == true)
                        {
                            MFD.ISMTCR_TaskNo = createdlist_task[0].feedbacktask;
                            // MFD.ISMTCR_TaskNo = "0";
                        }
                        else
                        {
                            MFD.ISMTCR_TaskNo = "0";
                        }

                        _FMSContext.Add(MFD);
                        var s = _FMSContext.SaveChanges();
                        if (s >= 1)
                        {
                            x.saved = "saved";
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return x;
        }
        public ModuleFeedbackDTO checkfeedback(ModuleFeedbackDTO status)
        {
            if (status.nofeedbackfound == "silerjubilee")
            {


                var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == status.userid).ToList();
                if (getempid.Count > 0)
                {
                    var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();
                    status.HRME_Id = empdetails[0].HRME_Id;
                }
                Array getexistsuggestion;
                List<dynamic> getexistsuggestionList = new List<dynamic>();  // Empty list

                if (status.HRME_Id > 0)
                {

                    using (var cmd = _FMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SilverJubilee_Suggestions_insert";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = status.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = status.HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@SJS_Suggestions", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@Ratings", SqlDbType.VarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = status.userid });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "Check" });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    // Create a dynamic ExpandoObject for each row, if you want to store just the 'Suggestion' column
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;

                                    // Let's assume we are only interested in the 'Suggestion' column
                                    dataRow["RCount"] = dataReader.IsDBNull(dataReader.GetOrdinal("RCount")) ? null : dataReader["RCount"];

                                    // Add the dynamic object (which now contains only 'Suggestion') to the list
                                    getexistsuggestionList.Add((ExpandoObject)dataRow);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    foreach (var item in getexistsuggestionList)
                    {
                        Console.WriteLine($"RCount: {item.RCount}");
                        if (item.RCount > 0)
                        {
                            status.existfeedback = "found";
                            break;
                        }
                    }

                }
            }
            else
            {
                var rolet = _FMSContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == status.IVRMRT_Id).ToList();
                status.Role_flag = rolet[0].IVRMRT_Role;
                var getempid = _FMSContext.Staff_User_Login.Where(a => a.Id == status.userid).ToList();
                if (getempid != null && getempid.Count > 0)
                {
                    try
                    {
                        DateTime revievwmonth = DateTime.Now;
                        string currentmonth, currentyear;
                        currentmonth = DateTime.Now.Month.ToString(); //check for the current month 
                        currentyear = DateTime.Now.Year.ToString();
                        int currntday = DateTime.Now.Day;
                        // int currntday = int.Parse(DateTime.Now.Day.ToString());
                        DateTime accountcreated;
                        double totaldays = 0.0;
                        if (true)
                        {
                            if (getempid.Count > 0)
                            {
                                var empdetails = _FMSContext.MasterEmployee.Where(a => a.HRME_Id == getempid.FirstOrDefault().Emp_Code).ToList();
                                status.HRME_Id = empdetails[0].HRME_Id;
                                var account = _FMSContext.ApplicationUser.Where(x => x.Id == getempid.FirstOrDefault().Id).ToList();
                                accountcreated = account[0].Entry_Date;
                                totaldays = (DateTime.Now - accountcreated).TotalDays;
                            }
                            if (totaldays > 31.00)
                            {
                                var priorfeedback = _FMSContext.ModuleFeedback.Where(F => F.HRME_Id == status.HRME_Id && F.IVRMM_Id == status.IVRMM_Id && F.VMF_CreatedDate.Month == revievwmonth.Month).ToList();
                                //var priorfeedback = _FMSContext.ModuleFeedback.Where(F => F.HRME_Id == status.HRME_Id && F.IVRMM_Id == status.IVRMM_Id).ToList();
                                if (priorfeedback.Count > 0)
                                {

                                    status.existfeedback = "exist";
                                }
                                else
                                {
                                    status.nofeedbackfound = "notfound";
                                }
                            }

                        }

                    }
                    catch (Exception exx)
                    {
                        Console.WriteLine(exx);
                    }
                }
            }


            return status;
        }
    }
}
