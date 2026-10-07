using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;


namespace FrontOffice.com.vaps.Controllers
{
    public class clientIEmappinglmpl : Interfaces.clientIEmappingInterface
    {
        //public HRMSContext _context;
        public FOContext _cont;
        public clientIEmappinglmpl(FOContext _co)
        {

            _cont = _co;
        }

        public clientIEmappingDTO onloaddata(clientIEmappingDTO data)
        {
            try
            {
                data.client_list = _cont.ISM_Master_Client_DMO.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true).ToArray();

                data.employee_list = (from d in _cont.HR_Master_Employee_DMO
                                      where (d.MI_Id == data.MI_Id && d.HRME_ActiveFlag == true && d.HRME_LeftFlag == false)
                                      select new clientIEmappingDTO
                                      {
                                          EmplYoeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                          FOBVIEM_HRMEId = d.HRME_Id,
                                          BioMetricCode = d.HRME_BiometricCode

                                      }
                               ).Distinct().ToArray();


                using (var cmd = _cont.Database.GetDbConnection().CreateCommand())

                {
                    cmd.CommandText = "HR_VAPSBIOMATRICDETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.getloaddetails = retObject.ToArray();
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

        public clientIEmappingDTO saverecord(clientIEmappingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                var mi_id = _cont.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.FOBVIEM_HRMEId).Select(t => t.MI_Id).FirstOrDefault();

                List<long> employeeids = new List<long>();
                if (data.FOBVIEM_Id > 0)
                {

                    var checkduplicate = _cont.FO_Biometric_VAPS_IEMapping_DMO.Where(a => a.FOBVIEM_Id == data.FOBVIEM_Id && a.MI_Id == mi_id && a.FOBVIEM_HRMEId == data.FOBVIEM_HRMEId && a.FOBVIEM_BiometricId == data.FOBVIEM_BiometricId && a.FOBVIEM_Insert_MI_Id == data.FOBVIEM_Insert_MI_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                        var checkresult = _cont.FO_Biometric_VAPS_IEMapping_DMO.Single(a => a.MI_Id == data.MI_Id && a.FOBVIEM_Id == data.FOBVIEM_Id);
                        checkresult.MI_Id = mi_id;
                        checkresult.FOBVIEM_HRMEId = data.FOBVIEM_HRMEId;
                        checkresult.FOBVIEM_BiometricId = data.FOBVIEM_BiometricId;
                        checkresult.FOBVIEM_Insert_MI_Id = data.FOBVIEM_Insert_MI_Id;
                        checkresult.FOBVIEM_UpdatedBy = data.Userid;
                        checkresult.FOBVIEM_UpdatedDate = indiantime0;
                        _cont.Update(checkresult);
                        var i = _cont.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {

                    var checkduplicate = _cont.FO_Biometric_VAPS_IEMapping_DMO.Where(a => a.FOBVIEM_BiometricId == data.FOBVIEM_BiometricId).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                        FO_Biometric_VAPS_IEMapping_DMO obj = new FO_Biometric_VAPS_IEMapping_DMO();
                        obj.MI_Id = mi_id;
                        obj.FOBVIEM_HRMEId = data.FOBVIEM_HRMEId;
                        obj.FOBVIEM_BiometricId = data.FOBVIEM_BiometricId;
                        obj.FOBVIEM_Insert_MI_Id = data.FOBVIEM_Insert_MI_Id;
                        obj.FOBVIEM_ActiveFlg = true;
                        obj.FOBVIEM_UpdatedBy = data.Userid;
                        obj.FOBVIEM_UpdatedDate = indiantime0;
                        obj.FOBVIEM_CreatedBy = data.Userid;
                        obj.FOBVIEM_CreatedDate = indiantime0;
                        _cont.Add(obj);

                        var i = _cont.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //public clientIEmappingDTO deactive(clientIEmappingDTO data)
        //{
        //    //try
        //    //{
        //    //    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //    //    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
        //    //    var checkactivestatus = _context.External_TrainingDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id == data.HREXTTRN_Id);

        //    //    if (checkactivestatus.HREXTTRN_ActiveFlag == true)
        //    //    {
        //    //        var resultdeactive = _context.External_TrainingDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id == data.HREXTTRN_Id);

        //    //        if (resultdeactive.HREXTTRN_ActiveFlag == true)
        //    //        {
        //    //            resultdeactive.HREXTTRN_ActiveFlag = false;
        //    //        }
        //    //        else
        //    //        {
        //    //            resultdeactive.HREXTTRN_ActiveFlag = true;
        //    //        }

        //    //        resultdeactive.HREXTTRN_UpdatedDate = indiantime0;
        //    //        resultdeactive.HREXTTRN_UpdatedBy = data.Userid;
        //    //        _context.Update(resultdeactive);

        //    //        var i = _context.SaveChanges();
        //    //        if (i > 0)
        //    //        {
        //    //            data.returnval = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            data.returnval = false;
        //    //        }

        //    //    }
        //    //    else
        //    //    {
        //    //        var resultdeactive = _context.External_TrainingDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id == data.HREXTTRN_Id);

        //    //        if (resultdeactive.HREXTTRN_ActiveFlag == true)
        //    //        {
        //    //            resultdeactive.HREXTTRN_ActiveFlag = false;
        //    //        }
        //    //        else
        //    //        {
        //    //            resultdeactive.HREXTTRN_ActiveFlag = true;
        //    //        }

        //    //        resultdeactive.HREXTTRN_UpdatedDate = indiantime0;
        //    //        resultdeactive.HREXTTRN_UpdatedBy = data.Userid;
        //    //        _context.Update(resultdeactive);

        //    //        var i = _context.SaveChanges();
        //    //        if (i > 0)
        //    //        {
        //    //            data.returnval = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            data.returnval = false;
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.Message);
        //    //}
        //    return data;
        //}

        public clientIEmappingDTO deactive(clientIEmappingDTO data)
        {
            try
            {
                var result = _cont.FO_Biometric_VAPS_IEMapping_DMO.Single(t => t.FOBVIEM_Id == data.FOBVIEM_Id);

                if (result.FOBVIEM_ActiveFlg == true)
                {
                    result.FOBVIEM_ActiveFlg = false;
                }
                else if (result.FOBVIEM_ActiveFlg == false)
                {
                    result.FOBVIEM_ActiveFlg = true;
                }
                result.FOBVIEM_UpdatedDate = DateTime.Now;
                _cont.Update(result);
                int returnval = _cont.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }

        public clientIEmappingDTO validateData(clientIEmappingDTO data)
        {
            data.retrunMsg = "";
            try
            {
                // if (dto.HRME_Id == 0)
                //  {
                if (data.Type.Equals("BiometricCode"))
                {
                    return BiometricCodeDuplicateCheck(data);
                }
            }
            catch (Exception ex)
            {
                data.retrunMsg = "error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        private clientIEmappingDTO BiometricCodeDuplicateCheck(clientIEmappingDTO data)
        {
            try
            {
                if (data.HRME_Id == 0)
                {
                    var resultCout = _cont.HR_Master_Employee_DMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_BiometricCode.Equals(data.HRME_BiometricCode)).Count();

                    if (resultCout > 0)
                    {
                        data.retrunMsg = "Duplicate";
                        return data;
                    }
                }
                else
                {
                    var resultCout = _cont.HR_Master_Employee_DMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_BiometricCode.Equals(data.HRME_BiometricCode) && t.HRME_Id != data.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        data.retrunMsg = "Duplicate";
                        return data;
                    }
                }

            }
            catch (Exception e)
            {
                data.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return data;
        }




    }
}
