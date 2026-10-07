using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Services
{
    public class IEMontEndReportForReportSubmissionIMPL: Interfaces.IEMontEndReportForReportSubmissionInterface
    {
        //public HRMSContext _context;
        public FOContext _cont;
        public IEMontEndReportForReportSubmissionIMPL(FOContext _co)
        {

            _cont = _co;
        }

        public IEMontEndReportForReportSubmissionDTO Onload(IEMontEndReportForReportSubmissionDTO data)
        {
            try
            {
                data.HRME_Id = _cont.Staff_User_Login.Where(a => a.Id == data.Userid).Select(t => t.Emp_Code).FirstOrDefault();
                //data.Year = _cont.AcademicYear.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.Year = (from a in _cont.HR_Master_LeaveYearDMO
                                 where (a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                 select new HR_Master_LeaveYearDTO
                                 {
                                     HRMLY_Id = a.HRMLY_Id,
                                     HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                     HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                 }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();

                data.Month = _cont.Month.Where(t => t.Is_Active == true).ToArray();
                var clientdid = (from a in _cont.ISM_Master_Client_DMO
                                     from b in _cont.ISM_Master_Client_IEMapping_DMO
                                     where (a.ISMMCLT_Id==b.ISMMCLT_Id && b.ISMCIM_IEList==data.HRME_Id && a.ISMMCLT_ActiveFlag==true && b.ISMMCLTIE_ActiveFlag==true)
                                     select b).Select(t=> t.ISMMCLT_Id).ToList();

                string Clientidstring="0";
                List<long> ClinetIDs = new List<long>();
                if (clientdid.Count > 0)
                {
                    foreach (var i in clientdid)
                    {
                        ClinetIDs.Add(i);
                        Clientidstring = Clientidstring + "," + i;
                    }
                }
                else
                {
                    ClinetIDs.Add(0);
                }
                data.intistutename = _cont.ISM_Master_Client_DMO.Where(t => t.ISMMCLT_ActiveFlag == true && clientdid.Contains(t.ISMMCLT_Id)).ToArray();

                using (var cmd = _cont.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "MonthEndReportSubmissionIEGrid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.VarChar)
                    {
                        Value = Clientidstring  
                    });
                    
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {

                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? "-" : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.GetGriddata = retObject1.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.empdetils = (from x in _cont.HR_Master_Employee_DMO
                                where (x.HRME_ActiveFlag == true && x.HRME_LeftFlag == false)
                                select new IEMontEndReportForReportSubmissionDTO
                                {
                                    HRME_Id = x.HRME_Id,
                                    HRME_EmployeeFirstName = x.HRME_EmployeeFirstName +
                                    (x.HRME_EmployeeMiddleName == null || x.HRME_EmployeeMiddleName == "" ? "" : " " + x.HRME_EmployeeMiddleName) +
                                    (x.HRME_EmployeeLastName == null || x.HRME_EmployeeLastName == "" ? "" : " " + x.HRME_EmployeeLastName)
                                }).Distinct().ToArray();

                using (var cmd = _cont.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_Master_MER_Modules";
                    cmd.CommandType = CommandType.StoredProcedure;
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
                        data.Master_MER_Modules = retObject.ToArray();
                                           }
                    catch (Exception ex)

                    {
                        Console.Write(ex.Message);
                    }

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IEMontEndReportForReportSubmissionDTO saverecord(IEMontEndReportForReportSubmissionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);


                List<long> employeeids = new List<long>();
                if (data.ISMCLMER_Id > 0)
                {

                    var checkduplicate = _cont.ISM_Client_MERDMO.Where(a => a.Month_Id == data.IVRM_Month_Id && a.ISMCLMER_Year == data.ASMAY_Id && a.ISMCLMER_Id != data.ISMCLMER_Id).ToList();
                    data.HRME_Id = _cont.Staff_User_Login.Where(a => a.Id == data.Userid).Select(t=>t.Emp_Code).FirstOrDefault();

                    //data.HRME_Id = (from a in _cont.ApplicationUserDMO
                    //               from b in _cont.Staff_User_Login
                    //                where (a.Id == b.Id && a.Id == data.Userid)
                    //               select b).Select(t => t.Emp_Code).FirstOrDefault();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checkresultMain = _cont.ISM_Client_MERDMO.Single(c => c.Month_Id == data.IVRM_Month_Id && c.ISMCLMER_Year == data.ASMAY_Id && c.ISMCLMER_Id == data.ISMCLMER_Id);
                        checkresultMain.ISMMCLT_Id = data.ISMMCLT_Id;
                        checkresultMain.ISMCLMER_Year = data.ASMAY_Id;
                        checkresultMain.Month_Id = data.IVRM_Month_Id;
                        checkresultMain.HRME_Id = data.HRME_Id;
                        checkresultMain.ISMCLMER_ActiveFlag = true;
                        checkresultMain.ISMCLMER_UpdatedBy = data.Userid;
                        checkresultMain.ISMCLMER_UpdatedDate = indiantime0;
                        _cont.Update(checkresultMain);
                        var a = _cont.SaveChanges();
                        if (data.FilePath_Array != null)
                        {
                            var checkresultone = _cont.ISM_Client_MER_FilesDMO.Where(b => b.ISMCLMER_Id == data.ISMCLMER_Id).ToList();
                           if(checkresultone.Count > 0)
                            {
                                foreach (var R in checkresultone)
                                {
                                    var remove = _cont.ISM_Client_MER_FilesDMO.Where(K => K.ISMCLMERFL_Id == R.ISMCLMERFL_Id).FirstOrDefault();
                                    _cont.Remove(remove);
                                }
                            }
                           
                           
                            for (int j = 0; j < data.FilePath_Array.Length; j++)
                            {
                                ISM_Client_MER_FilesDMO checkresult = new ISM_Client_MER_FilesDMO();
                                checkresult.ISMCLMER_Id = checkresultMain.ISMCLMER_Id;
                                checkresult.ISMCLMERFL_FileName = data.FilePath_Array[j].ISMCLMERFL_FileName;
                                checkresult.ISMCLMERFL_FileDesc = data.FilePath_Array[j].ISMCLMERFL_FileDesc;
                                checkresult.ISMCLMERFL_FilePath = data.FilePath_Array[j].ISMCLMERFL_FilePath;
                                checkresult.IMMM_ID = data.FilePath_Array[j].IMMM_ID;
                                checkresult.ISMCLMERFL_ActiveFlag = true;
                                checkresult.ISMCLMERFL_ApprovedFlg = false;
                                checkresult.ISMCLMERFL_UpdatedBy = data.Userid;
                                checkresult.ISMCLMERFL_CreatedBy = data.Userid;
                                checkresult.ISMCLMERFL_UpdatedDate = indiantime0;
                                checkresult.ISMCLMERFL_CreatedDate = indiantime0;
                                _cont.Add(checkresult);
                            }
                        }
                        var i = _cont.SaveChanges();
                        if (i > 0)
                        {
                           
                            data.message = "Update";
                        }
                        else
                        {
                           
                            data.message = "UpdateFaild";
                        }
                    }
                }
                else
                {


                    var checkduplicate = _cont.ISM_Client_MERDMO.Where(a => a.Month_Id == data.IVRM_Month_Id && a.ISMCLMER_Year == data.ASMAY_Id && a.ISMCLMER_Id == data.ISMCLMER_Id).ToList();
                    data.HRME_Id = _cont.Staff_User_Login.Where(a => a.Id == data.Userid).Select(t => t.Emp_Code).FirstOrDefault();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        ISM_Client_MERDMO checkresultMain = new ISM_Client_MERDMO();
                          checkresultMain.ISMMCLT_Id = data.ISMMCLT_Id;
                        checkresultMain.ISMCLMER_Year = data.ASMAY_Id;
                        checkresultMain.Month_Id = data.IVRM_Month_Id;
                        checkresultMain.HRME_Id = data.HRME_Id;
                        checkresultMain.ISMCLMER_ActiveFlag = true;
                        checkresultMain.ISMCLMER_UpdatedBy = data.Userid;
                        checkresultMain.ISMCLMER_CreatedBy = data.Userid;
                        checkresultMain.ISMCLMER_UpdatedDate = indiantime0;
                        checkresultMain.ISMCLMER_CreatedDate = indiantime0;
                        _cont.Add(checkresultMain);
                        var t = _cont.SaveChanges();
                        if (data.FilePath_Array != null)
                        {
                            for (int j = 0; j < data.FilePath_Array.Length; j++)
                            {
                                ISM_Client_MER_FilesDMO checkresult  = new ISM_Client_MER_FilesDMO();
                                checkresult.ISMCLMER_Id = checkresultMain.ISMCLMER_Id;
                                checkresult.ISMCLMERFL_FileName = data.FilePath_Array[j].ISMCLMERFL_FileName;
                                checkresult.ISMCLMERFL_FileDesc = data.FilePath_Array[j].ISMCLMERFL_FileDesc;
                                checkresult.ISMCLMERFL_FilePath = data.FilePath_Array[j].ISMCLMERFL_FilePath;
                                checkresult.IMMM_ID = data.FilePath_Array[j].IMMM_ID;
                                checkresult.ISMCLMERFL_ActiveFlag = true;
                                checkresult.ISMCLMERFL_ApprovedFlg = false;
                                checkresult.ISMCLMERFL_UpdatedBy = data.Userid;
                                checkresult.ISMCLMERFL_CreatedBy = data.Userid;
                                checkresult.ISMCLMERFL_UpdatedDate = indiantime0;
                                checkresult.ISMCLMERFL_CreatedDate = indiantime0;
                                _cont.Add(checkresult);
                            }
                        }
                        var i = _cont.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "AddFaild";
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
        public IEMontEndReportForReportSubmissionDTO DocView(IEMontEndReportForReportSubmissionDTO data)
        {
            try
            {

                data.MultidocView =_cont.ISM_Client_MER_FilesDMO.Where(a => a.IMMM_ID==data.IMMM_ID &&   a.ISMCLMER_Id == data.ISMCLMER_Id && a.ISMCLMERFL_ActiveFlag==true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IEMontEndReportForReportSubmissionDTO deactiveY(IEMontEndReportForReportSubmissionDTO data)
        {
            try
            {

                var update = _cont.ISM_Client_MERDMO.Single(a => a.ISMCLMER_Id == data.ISMCLMER_Id);

                if (update.ISMCLMER_ActiveFlag==true)
                {
                    update.ISMCLMER_ActiveFlag = false;
                    _cont.Update(update);
                    var count= _cont.SaveChanges();
                    if(count>0)
                    {
                        data.message = "updated";
                    }
                    else
                    {
                        data.message = "notupdated";
                    }
                }
                else if(update.ISMCLMER_ActiveFlag == false)
                {
                    update.ISMCLMER_ActiveFlag = true;
                    _cont.Update(update);
                    var count = _cont.SaveChanges();
                    if (count > 0)
                    {
                        data.message = "updated";
                    }
                    else
                    {
                        data.message = "notupdated";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IEMontEndReportForReportSubmissionDTO EditData(IEMontEndReportForReportSubmissionDTO data)
        {
            try
            {
                data.geteditdetils = _cont.ISM_Client_MERDMO.Where(t => t.ISMCLMER_Id == data.ISMCLMER_Id).ToArray();

                data.MultidocView = _cont.ISM_Client_MER_FilesDMO.Where(t => t.ISMCLMER_Id == data.ISMCLMER_Id && t.IMMM_ID==data.IMMM_ID).ToArray();

                using (var cmd = _cont.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_edit_filesdata";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    cmd.Parameters.Add(new SqlParameter("@ISMCLMER_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ISMCLMER_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMMM_ID",
                    SqlDbType.BigInt)
                    {
                        Value = data.IMMM_ID
                    });
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
                        data.MultidocView = retObject.ToArray();
                    }
                    catch (Exception ex)

                    {
                        Console.Write(ex.Message);
                    }

                }
                using (var cmd = _cont.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "usp_s_Master_MER_Modules";
                    cmd.CommandType = CommandType.StoredProcedure;
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
                        data.Master_MER_Modules = retObject.ToArray();
                    }
                    catch (Exception ex)

                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return data;
        }

    }
}
