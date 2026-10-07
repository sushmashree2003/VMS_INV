using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace IssueManager.com.vaps.Services
{
    public class VMS_ChequePrinting_SettingImpl : Interfaces.VMS_ChequePrinting_SettingInterface
    {
        public IssueManagerContext _IssueManagercontext;
        public DomainModelMsSqlServerContext _db;

        public VMS_ChequePrinting_SettingImpl(IssueManagerContext hh, DomainModelMsSqlServerContext db)
        {
            _IssueManagercontext = hh;
            _db = db;

        }
        public VMS_ChequePrinting_SettingDTO getloaddata(VMS_ChequePrinting_SettingDTO data)
        {
            try
            {
                data.allcompany = _IssueManagercontext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_ChequePrinting_SettingDTO companychange(VMS_ChequePrinting_SettingDTO data)
        {
            try
            {
                data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();
                if(data.optionflag == "ChequePrinting")
                {
                    using (var cmd = _IssueManagercontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VMS_ChequePrinting_Settingpro";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRMBD_Id", SqlDbType.BigInt) { Value = data.HRMBD_Id });
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
                            data.alldata = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
       
        public VMS_ChequePrinting_SettingDTO savedetails(VMS_ChequePrinting_SettingDTO data)
        {
            try
             {
                string bankaccont = "";
                var banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_Id == data.HRMBD_Id).Distinct().ToList();
                if (banklist.Count > 0)
                {
                    bankaccont = banklist.FirstOrDefault().HRMBD_BankAccountNo;
                }
                if (data.VCPSET_Id != 0)
                {
                    var res = _IssueManagercontext.VMS_ChequePrinting_SettingDMO.Where(t => t.VCPSET_Id == data.VCPSET_Id && t.VCPSET_ActiveFlg == true ).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _IssueManagercontext.VMS_ChequePrinting_SettingDMO.Single(t => t.VCPSET_Id == data.VCPSET_Id);
                        result.VCPSET_PrintingTemplateId = data.VCPSET_PrintingTemplateId;
                        result.VCPSET_ChequeImageFileName = data.VCPSET_ChequeImageFileName;
                        result.VCPSET_ChequeImageFilePath = data.VCPSET_ChequeImageFilePath;
                        result.VCPSET_Remarks = data.VCPSET_Remarks;
                        result.VCPSET_Id = data.VCPSET_Id;
                        result.HRMBD_Id = data.HRMBD_Id;
                        result.VCPSET_ActiveFlg = true;
                        result.VCPSET_UpdatedBy = data.VCPSET_UpdatedBy;
                        result.VCPSET_UpdatedDate = DateTime.Now;
                        _IssueManagercontext.Update(result);
                        var contactExists = _IssueManagercontext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    var res = _IssueManagercontext.VMS_ChequePrinting_SettingDMO.Where(t => t.VCPSET_Id == data.VCPSET_Id ).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        VMS_ChequePrinting_SettingDMO Vmschequeprint = new VMS_ChequePrinting_SettingDMO();
                        Vmschequeprint.HRMBD_Id = data.HRMBD_Id;
                        Vmschequeprint.VCPSET_PrintingTemplateId = data.VCPSET_PrintingTemplateId;
                        Vmschequeprint.VCPSET_ChequeImageFileName = data.VCPSET_ChequeImageFileName;
                        Vmschequeprint.VCPSET_ChequeImageFilePath = data.VCPSET_ChequeImageFilePath;
                        Vmschequeprint.VCPSET_Remarks = data.VCPSET_Remarks;
                        Vmschequeprint.VCPSET_ActiveFlg = true;
                        Vmschequeprint.VCPSET_CreatedBy = data.UserId;
                        Vmschequeprint.VCPSET_CreatedDate = DateTime.Now;
                        Vmschequeprint.VCPSET_UpdatedDate = DateTime.Now;
                        _IssueManagercontext.Add(Vmschequeprint);
                        var contactExists = _IssueManagercontext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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
     
        public VMS_ChequePrinting_SettingDTO deactive(VMS_ChequePrinting_SettingDTO data)
        {
            try
            {
                var result = _IssueManagercontext.VMS_ChequePrinting_SettingDMO.Single(t => t.VCPSET_Id == data.VCPSET_Id);

                if (result.VCPSET_ActiveFlg == true)
                {
                    result.VCPSET_ActiveFlg = false;
                }
                else if (result.VCPSET_ActiveFlg == false)
                {
                    result.VCPSET_ActiveFlg = true;
                }
                result.VCPSET_UpdatedDate = DateTime.Now;
                _IssueManagercontext.Update(result);
                int returnval = _IssueManagercontext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
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





