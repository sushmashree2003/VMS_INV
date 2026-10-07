using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ImsClientImpl : Interfaces.ImsClientInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ImsClientImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public Clients_DTO getdetails(Clients_DTO data)
        {
            try
            {
                data.institution = _IssueContext.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                data.cordinartorlist = (from a in _IssueContext.MasterEmployee
                                        where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                        select new Clients_DTO
                                        {
                                            ISMMCLT_CordinatorId = Convert.ToInt64(a.HRME_Id),
                                            cordinatorname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                            MI_Id = a.MI_Id
                                        }).Distinct().OrderBy(s => s.cordinatorname).ToArray();


                data.teamleadlist = (from a in _IssueContext.MasterEmployee
                                     where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new Clients_DTO
                                     {
                                         ISMMCLT_TeamLeadId = Convert.ToInt64(a.HRME_Id),
                                         leadername = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                         MI_Id = a.MI_Id
                                     }).Distinct().OrderBy(s => s.leadername).ToArray();

                data.clientlist = _IssueContext.clientTable.Where(t => t.MI_Id == data.MI_Id && t.ISMMCLT_ActiveFlag == true).Distinct().OrderBy(t => t.ISMMCLT_ClientName).ToArray();

                var ieslist = (from a in _IssueContext.clientMappingTable
                               from b in _IssueContext.clientTable
                               where (a.ISMMCLT_Id == b.ISMMCLT_Id)
                               select a).Distinct().ToList();

                data.ieEmpList = (from b in _IssueContext.MasterEmployee
                                  where (b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                  select new Clients_DTO
                                  {
                                      HRME_Id = b.HRME_Id,
                                      ieEmpname = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                  }).Distinct().OrderBy(t => t.ieEmpname).ToArray();




                data.allIeMappingdata = (from a in _IssueContext.clientTable
                                         from b in _IssueContext.clientMappingTable
                                         from c in _IssueContext.MasterEmployee
                                         where (a.MI_Id == data.MI_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMCIM_IEList == c.HRME_Id && c.HRME_ActiveFlag == true && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.ISMMCLT_ActiveFlag == true)
                                         select new Clients_DTO
                                         {
                                             ISMMCLT_Id = a.ISMMCLT_Id,
                                             ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                         }).Distinct().ToArray();


                data.get_clentlist = (from a in _IssueContext.clientTable
                                          //from b in _IssueContext.MasterEmployee
                                      where (a.MI_Id == data.MI_Id /*&& a.ISMMCLT_CordinatorId == b.HRME_Id && a.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false*/)
                                      select new Clients_DTO
                                      {
                                          ISMMCLT_Id = a.ISMMCLT_Id,

                                          ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                          ISMMCLT_Code = a.ISMMCLT_Code,
                                          ISMMCLT_Desc = a.ISMMCLT_Desc,
                                          ISMMCLT_ContactNo = a.ISMMCLT_ContactNo,
                                          ISMMCLT_EmailId = a.ISMMCLT_EmailId,
                                          ISMMCLT_Address = a.ISMMCLT_Address,
                                          ISMMCLT_NOName = a.ISMMCLT_NOName,
                                          ISMMCLT_NOEmailId = a.ISMMCLT_NOEmailId,
                                          ISMMCLT_NOContactNo = a.ISMMCLT_NOContactNo,
                                          ISMMCLT_ActiveFlag = a.ISMMCLT_ActiveFlag,
                                          ISMMCLT_CordinatorId = a.ISMMCLT_CordinatorId,
                                          ISMMCLT_RemainderDays = a.ISMMCLT_RemainderDays,
                                          //cordinatorname = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),

                                          cordinatorFirstName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_CordinatorId).SingleOrDefault().HRME_EmployeeFirstName),
                                          cordinatorMiddleName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_CordinatorId).SingleOrDefault().HRME_EmployeeMiddleName),
                                          cordinatorLastName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_CordinatorId).SingleOrDefault().HRME_EmployeeLastName),

                                          ISMMCLT_TeamLeadId = a.ISMMCLT_TeamLeadId,

                                          teamleadFirstName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_TeamLeadId).SingleOrDefault().HRME_EmployeeFirstName),
                                          teamleadMiddleName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_TeamLeadId).SingleOrDefault().HRME_EmployeeMiddleName),
                                          teamleadLastName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_TeamLeadId).SingleOrDefault().HRME_EmployeeLastName),

                                      }).Distinct().ToArray();

                //get_clentlist
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Master_ClientList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
          SqlDbType.BigInt)
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
                        data.arrayclientlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //clientgrid 
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_ClientFeedback";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
          SqlDbType.BigInt)
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
                        data.clientgrid = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Clients_DTO OnChangeTab1Inst(Clients_DTO data)
        {
            try
            {
                data.get_clentlist = (from a in _IssueContext.clientTable
                                      where (a.MI_Id == data.MI_Id)
                                      select new Clients_DTO
                                      {
                                          ISMMCLT_Id = a.ISMMCLT_Id,

                                          ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                          ISMMCLT_Code = a.ISMMCLT_Code,
                                          ISMMCLT_Desc = a.ISMMCLT_Desc,
                                          ISMMCLT_ContactNo = a.ISMMCLT_ContactNo,
                                          ISMMCLT_EmailId = a.ISMMCLT_EmailId,
                                          ISMMCLT_Address = a.ISMMCLT_Address,
                                          ISMMCLT_NOName = a.ISMMCLT_NOName,
                                          ISMMCLT_NOEmailId = a.ISMMCLT_NOEmailId,
                                          ISMMCLT_NOContactNo = a.ISMMCLT_NOContactNo,
                                          ISMMCLT_ActiveFlag = a.ISMMCLT_ActiveFlag,
                                          ISMMCLT_CordinatorId = a.ISMMCLT_CordinatorId,
                                          ISMMCLT_RemainderDays = a.ISMMCLT_RemainderDays,

                                          cordinatorFirstName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_CordinatorId).SingleOrDefault().HRME_EmployeeFirstName),
                                          cordinatorMiddleName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_CordinatorId).SingleOrDefault().HRME_EmployeeMiddleName),
                                          cordinatorLastName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_CordinatorId).SingleOrDefault().HRME_EmployeeLastName),

                                          ISMMCLT_TeamLeadId = a.ISMMCLT_TeamLeadId,

                                          teamleadFirstName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_TeamLeadId).SingleOrDefault().HRME_EmployeeFirstName),
                                          teamleadMiddleName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_TeamLeadId).SingleOrDefault().HRME_EmployeeMiddleName),
                                          teamleadLastName = (_IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.ISMMCLT_TeamLeadId).SingleOrDefault().HRME_EmployeeLastName),
                                          MI_Id = a.MI_Id

                                      }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Clients_DTO saveClientdata(Clients_DTO data)
        {
            try
            {
                if (data.ISMMCLT_Id == 0)
                {
                    foreach (var d in data.multipleclients)
                    {
                        var duplicate = _IssueContext.clientTable.Where(t => t.MI_Id == d.MI_Id && t.ISMMCLT_ClientName == data.ISMMCLT_ClientName).ToList();
                        var duplicate1 = _IssueContext.clientTable.Where(t => t.MI_Id == d.MI_Id && t.ISMMCLT_Code == data.ISMMCLT_Code).ToList();
                        if (duplicate.Count > 0 || duplicate1.Count > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            ISM_Master_Client_DMO obj = new ISM_Master_Client_DMO();

                            //obj.ISMMCLT_Id = data.ISMMCLT_Id;
                            obj.MI_Id = d.MI_Id;
                            obj.ISMMCLT_ClientName = data.ISMMCLT_ClientName;
                            obj.ISMMCLT_Code = data.ISMMCLT_Code;
                            obj.ISMMCLT_Desc = data.ISMMCLT_Desc;
                            obj.ISMMCLT_ContactNo = data.ISMMCLT_ContactNo;
                            obj.ISMMCLT_EmailId = data.ISMMCLT_EmailId;
                            obj.ISMMCLT_Address = data.ISMMCLT_Address;
                            obj.ISMMCLT_NOName = data.ISMMCLT_NOName;
                            obj.ISMMCLT_NOEmailId = data.ISMMCLT_NOEmailId;
                            obj.ISMMCLT_NOContactNo = data.ISMMCLT_NOContactNo;
                            obj.ISMMCLT_CordinatorId = data.ISMMCLT_CordinatorId;
                            obj.ISMMCLT_TeamLeadId = data.ISMMCLT_TeamLeadId;
                            obj.ISMMCLT_RemainderDays = data.ISMMCLT_RemainderDays;
                            obj.ISMMCLT_GSTNO = data.ISMMCLT_GSTNO;
                            obj.ISMMCLT_ActiveFlag = true;
                            obj.ISMMCLT_CreatedBy = data.UserId;
                            obj.ISMMCLT_UpdatedBy = data.UserId;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;
                            _IssueContext.Add(obj);

                         
                        }
                    }

                    int rowAffected = _IssueContext.SaveChanges();
                    if (rowAffected > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else if (data.ISMMCLT_Id > 0)
                {
                    var duplicate = _IssueContext.clientTable.Where(t => t.MI_Id == data.MI_Id && t.ISMMCLT_Id != data.ISMMCLT_Id && t.ISMMCLT_ClientName == data.ISMMCLT_ClientName).ToList();

                    var duplicate1 = _IssueContext.clientTable.Where(t => t.MI_Id == data.MI_Id && t.ISMMCLT_Id != data.ISMMCLT_Id && t.ISMMCLT_Code == data.ISMMCLT_Code).ToList();

                    if (duplicate.Count > 0 || duplicate1.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var obj = _IssueContext.clientTable.Single(t => t.MI_Id == data.MI_Id && t.ISMMCLT_Id == data.ISMMCLT_Id);
                        obj.ISMMCLT_ClientName = data.ISMMCLT_ClientName;
                        obj.ISMMCLT_Code = data.ISMMCLT_Code;
                        obj.ISMMCLT_Desc = data.ISMMCLT_Desc;
                        obj.ISMMCLT_ContactNo = data.ISMMCLT_ContactNo;
                        obj.ISMMCLT_EmailId = data.ISMMCLT_EmailId;
                        obj.ISMMCLT_Address = data.ISMMCLT_Address;
                        obj.ISMMCLT_NOName = data.ISMMCLT_NOName;
                        obj.ISMMCLT_NOEmailId = data.ISMMCLT_NOEmailId;
                        obj.ISMMCLT_NOContactNo = data.ISMMCLT_NOContactNo;
                        obj.ISMMCLT_CordinatorId = data.ISMMCLT_CordinatorId;
                        obj.ISMMCLT_TeamLeadId = data.ISMMCLT_TeamLeadId;
                        obj.ISMMCLT_RemainderDays = data.ISMMCLT_RemainderDays;
                        obj.ISMMCLT_GSTNO = data.ISMMCLT_GSTNO;
                        obj.ISMMCLT_UpdatedBy = data.UserId;
                        obj.UpdatedDate = DateTime.Now;

                        _IssueContext.Update(obj);
                        int rowAffected = _IssueContext.SaveChanges();
                        if (rowAffected > 0)
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
        public Clients_DTO clientDecative(Clients_DTO data)
        {
            try
            {
                var result = _IssueContext.clientTable.Single(t => t.MI_Id == data.MI_Id && t.ISMMCLT_Id == data.ISMMCLT_Id);
                if (result.ISMMCLT_ActiveFlag == true)
                {
                    result.ISMMCLT_ActiveFlag = false;
                }
                else
                {
                    result.ISMMCLT_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _IssueContext.Update(result);
                int rowAffected = _IssueContext.SaveChanges();
                if (rowAffected > 0)
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
            }

            return data;
        }
        public Clients_DTO editClientdata(Clients_DTO data)
        {
            try
            {

                var editclient = _IssueContext.clientTable.Where(t => t.ISMMCLT_Id == data.ISMMCLT_Id).ToList();
                data.editClient = editclient.ToArray();

                data.cordinartorlist = (from a in _IssueContext.MasterEmployee
                                        where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                        select new Clients_DTO
                                        {
                                            ISMMCLT_CordinatorId = Convert.ToInt64(a.HRME_Id),
                                            cordinatorname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName)
                                        }).Distinct().OrderByDescending(s => s.HRME_Id).ToArray();

                data.teamleadlist = (from a in _IssueContext.MasterEmployee
                                     where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new Clients_DTO
                                     {
                                         ISMMCLT_TeamLeadId = Convert.ToInt64(a.HRME_Id),
                                         leadername = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName)
                                     }).Distinct().OrderByDescending(s => s.HRME_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Clients_DTO OnChangeTab2Inst(Clients_DTO data)
        {
            try
            {

                data.clientlist = _IssueContext.clientTable.Where(t => t.MI_Id == data.MI_Id && t.ISMMCLT_ActiveFlag == true).Distinct().OrderBy(t => t.ISMMCLT_ClientName).ToArray();

                if (data.Flag == "Tab2")
                {
                    data.allIeMappingdata = (from a in _IssueContext.clientTable
                                             from b in _IssueContext.clientMappingTable
                                             from c in _IssueContext.MasterEmployee
                                             where (a.MI_Id == data.MI_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMCIM_IEList == c.HRME_Id && c.HRME_ActiveFlag == true && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.ISMMCLT_ActiveFlag == true)
                                             select new Clients_DTO
                                             {
                                                 ISMMCLT_Id = a.ISMMCLT_Id,
                                                 ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                             }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Clients_DTO OnChangeemployee(Clients_DTO data)
        {
            try
            {
                data.ieEmpList = (from a in _IssueContext.clientTable
                                         from b in _IssueContext.clientMappingTable
                                         from c in _IssueContext.MasterEmployee
                                         where (a.MI_Id == data.MI_Id && a.ISMMCLT_Id == data.ISMMCLT_Id && b.ISMCIM_IEList == c.HRME_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMMCLTIE_ActiveFlag == true)
                                         select new Clients_DTO
                                         {
                                             ISMMCLT_Id = a.ISMMCLT_Id,
                                             ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                             HRME_EmployeeFirstName = c.HRME_EmployeeFirstName
                                         }).Distinct().ToArray();

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Clients_DTO saveClientMappingdata(Clients_DTO data)
        {
            try
            {
                if (data.ISMMCLTIE_Id == 0)
                {
                    for (int i = 0; i < data.ieEmpListdata.Length; i++)
                    {
                        var tempid = data.ieEmpListdata[i].HRME_Id;
                        var duplicate = _IssueContext.clientMappingTable.Where(t => t.ISMCIM_IEList == tempid).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            ISM_Master_Client_IEMapping_DMO obj = new ISM_Master_Client_IEMapping_DMO();
                            obj.ISMMCLTIE_Id = data.ISMMCLTIE_Id;
                            obj.ISMMCLT_Id = data.ISMMCLT_Id;
                            obj.ISMCIM_IEList = tempid;
                            obj.ISMMCLTIE_ActiveFlag = true;
                            obj.ISMMCLTIE_CreatedBy = data.UserId;
                            obj.ISMMCLTIE_UpdatedBy = data.UserId;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;

                            _IssueContext.Add(obj);

                        }
                    }
                    int row = _IssueContext.SaveChanges();
                    if (row > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                else if (data.ISMMCLTIE_Id > 0)
                {


                    var remove = _IssueContext.clientMappingTable.Where(t => t.ISMMCLT_Id == data.ISMMCLT_Id).ToList();

                    if (remove.Count > 0)
                    {
                        foreach (var item in remove)
                        {
                            var remove1 = _IssueContext.clientMappingTable.Single(t => t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMCIM_IEList == item.ISMCIM_IEList);
                            _IssueContext.Remove(remove1);
                        }

                    }
                    for (int s = 0; s < data.ieEmpListdata.Length; s++)
                    {
                        var updateid = data.ieEmpListdata[s].HRME_Id;

                        //var duplicate = _IssueContext.clientMappingTable.Where(t => t.ISMMCLTIE_Id != data.ISMMCLTIE_Id && t.ISMCIM_IEList == updateid).ToList();
                        //if (duplicate.Count > 0)
                        //{
                        //    data.duplicate = true;
                        //}
                        //else
                        //{
                        ISM_Master_Client_IEMapping_DMO obj = new ISM_Master_Client_IEMapping_DMO();

                        //obj.ISMMCLTIE_Id = data.ISMMCLTIE_Id;
                        obj.ISMMCLT_Id = data.ISMMCLT_Id;
                        obj.ISMCIM_IEList = updateid;
                        obj.ISMMCLTIE_ActiveFlag = true;
                        obj.ISMMCLTIE_CreatedBy = data.UserId;
                        obj.ISMMCLTIE_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _IssueContext.Add(obj);
                    }
                    // }
                    int row = _IssueContext.SaveChanges();
                    if (row > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.allIeMappingdata = (from a in _IssueContext.clientTable
                                         from b in _IssueContext.clientMappingTable
                                         from c in _IssueContext.MasterEmployee
                                         where (a.MI_Id == data.MI_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMCIM_IEList == c.HRME_Id && c.HRME_ActiveFlag == true && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.ISMMCLT_ActiveFlag == true)
                                         select new Clients_DTO
                                         {
                                             ISMMCLT_Id = a.ISMMCLT_Id,
                                             ISMMCLT_ClientName = a.ISMMCLT_ClientName
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Clients_DTO editClientMappingdata(Clients_DTO data)
        {
            try
            {
                var edit = (from a in _IssueContext.clientMappingTable
                            from b in _IssueContext.clientTable
                            where (a.ISMMCLT_Id == b.ISMMCLT_Id && b.MI_Id == data.MI_Id && b.ISMMCLT_Id == data.ISMMCLT_Id)
                            select new Clients_DTO
                            {
                                ISMMCLT_Id = b.ISMMCLT_Id,
                                ISMMCLT_ClientName = b.ISMMCLT_ClientName,
                                ISMMCLTIE_Id = a.ISMMCLTIE_Id,
                                ISMCIM_IEList = a.ISMCIM_IEList,
                            }).Distinct().ToList();

                data.editCltMappinglist = edit.ToArray();

                data.ieEmpList = (from b in _IssueContext.MasterEmployee
                                  where (b.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                                  select new Clients_DTO
                                  {
                                      HRME_Id = b.HRME_Id,
                                      ieEmpname = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                  }).Distinct().OrderBy(t => t.HRME_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Clients_DTO deactiveClientMappingdata(Clients_DTO data)
        {
            try
            {
                var result = _IssueContext.clientMappingTable.Single(t => t.ISMMCLTIE_Id == data.ISMMCLTIE_Id);
                if (result.ISMMCLTIE_ActiveFlag == true)
                {
                    result.ISMMCLTIE_ActiveFlag = false;
                }
                else if (result.ISMMCLTIE_ActiveFlag == false)
                {
                    result.ISMMCLTIE_ActiveFlag = true;
                }

                result.UpdatedDate = DateTime.Now;

                _IssueContext.Update(result);
                int s = _IssueContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.modaliEMapingList = (from a in _IssueContext.clientTable
                                          from b in _IssueContext.clientMappingTable
                                          from c in _IssueContext.MasterEmployee
                                          where (a.MI_Id == c.MI_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMCIM_IEList == c.HRME_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_Id == data.ISMMCLT_Id)
                                          select new Clients_DTO
                                          {
                                              ISMMCLT_Id = a.ISMMCLT_Id,
                                              ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                              ISMMCLTIE_Id = b.ISMMCLTIE_Id,
                                              ISMCIM_IEList = b.ISMCIM_IEList,
                                              ieEmployName = c.HRME_EmployeeFirstName + (string.IsNullOrEmpty(c.HRME_EmployeeMiddleName) ? "" : ' ' + c.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(c.HRME_EmployeeLastName) ? "" : ' ' + c.HRME_EmployeeLastName),
                                              ISMMCLTIE_ActiveFlag = b.ISMMCLTIE_ActiveFlag
                                          }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Clients_DTO modalListdata(Clients_DTO data)
        {
            try
            {
                data.modaliEMapingList = (from a in _IssueContext.clientTable
                                          from b in _IssueContext.clientMappingTable
                                          from c in _IssueContext.MasterEmployee
                                          where (a.MI_Id == c.MI_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMCIM_IEList == c.HRME_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_Id == data.ISMMCLT_Id)
                                          select new Clients_DTO
                                          {
                                              ISMMCLT_Id = a.ISMMCLT_Id,
                                              ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                              ISMMCLTIE_Id = b.ISMMCLTIE_Id,
                                              ISMCIM_IEList = b.ISMCIM_IEList,
                                              ieEmployName = c.HRME_EmployeeFirstName + (string.IsNullOrEmpty(c.HRME_EmployeeMiddleName) ? "" : ' ' + c.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(c.HRME_EmployeeLastName) ? "" : ' ' + c.HRME_EmployeeLastName),
                                              ISMMCLTIE_ActiveFlag = b.ISMMCLTIE_ActiveFlag
                                          }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        //VMS Client And IVRM Client Mapping
        public Clients_DTO OnChangeClientTab3(Clients_DTO data)
        {
            try
            {
                data.clientdetails = _IssueContext.clientTable.Where(a => a.MI_Id == data.MI_Id && a.ISMMCLT_Id == data.ISMMCLT_Id).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public Clients_DTO SaveVMSIVRMMapping(Clients_DTO data)
        {
            try
            {
                data.returnval = false;
                var result = _IssueContext.clientTable.Single(a => a.ISMMCLT_Id == data.ISMMCLT_Id);
                result.IVRM_MI_Id = data.IVRM_MI_Id;
                result.ISMMCLT_IVRM_URL = data.ISMMCLT_IVRM_URL;
                result.ISMMCLT_ClientCode = data.ISMMCLT_ClientCode;
                result.UpdatedDate = DateTime.Now;
                result.ISMMCLT_UpdatedBy = data.UserId;

                _IssueContext.Update(result);
                var i = _IssueContext.SaveChanges();

                if (i > 0)
                {
                    data.returnval = true;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        //savefeedback      
        public Clients_DTO savefeedback(Clients_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.multipleclients !=null   && data.multipleclients.Length >0)
                {
                    if(data.ISMMCF_Id  > 0)
                    {
                        foreach (var d in data.multipleclients)
                        {
                            var obj = _IssueContext.ISM_Master_Client_FeedbackDMO.Where(R => R.ISMMCF_Id == data.ISMMCF_Id && R.ISMMCLT_Id == d.ISMMCLT_Id).FirstOrDefault();                          
                            obj.ISMMCF_Date = DateTime.Now;
                            obj.ISMMCF_Time = DateTime.Now.ToString("hh:mm:ss tt");
                            obj.ISMMCF_Remarks = d.ISMMCF_Remarks;
                            obj.ISMMCF_Negative = d.ISMMCF_Negative;
                            obj.ISMMCF_UpdatedDate = indianTime;                           
                            obj.ISMMCF_UpdatedBy = data.UserId;
                            _IssueContext.Update(obj);
                        }
                    }
                    else
                    {
                        foreach (var d in data.multipleclients)
                        {
                            ISM_Master_Client_FeedbackDMO obj = new ISM_Master_Client_FeedbackDMO();
                            obj.ISMMCLT_Id = d.ISMMCLT_Id;
                            obj.ISMMCF_Date = DateTime.Now;
                            obj.ISMMCF_Time = DateTime.Now.ToString("hh:mm:ss tt");
                            obj.ISMMCF_Remarks = d.ISMMCF_Remarks;
                            obj.ISMMCF_Negative = d.ISMMCF_Negative;
                            obj.ISMMCF_CreatedDate = indianTime;
                            obj.ISMMCF_UpdatedDate = indianTime;
                            obj.ISMMCF_CreatedBy = data.UserId;
                            obj.ISMMCF_UpdatedBy = data.UserId;
                            _IssueContext.Add(obj);

                        }
                    }
                    
                    int i = _IssueContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Saved";
                    }
                    else
                    {
                        data.message = "FailedSave";
                    }
                }

            }
            catch (Exception ex)
            {
                data.message = "admin";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //editdata
        public Clients_DTO editdata(Clients_DTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_ClientFeedbackDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id",
          SqlDbType.BigInt)
                    {
                        Value = data.ISMMCLT_Id  
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMMCF_Id",
          SqlDbType.BigInt)
                    {
                        Value = data.ISMMCF_Id
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
                        data.editClient = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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

