using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Model;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class UserMappingImpl : Interfaces.UserMappingInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public UserMappingImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public Clients_DTO getdetails(Clients_DTO data)
        {

            try
            {
                data.clientlist = _IssueContext.clientTable.Where(t => t.MI_Id == data.MI_Id && t.ISMMCLT_ActiveFlag == true).Distinct().OrderBy(t=>t.ISMMCLT_ClientName).ToArray();

                //var ieslist = (from a in _IssueContext.ISM_Master_Client_UserMappingDMO
                //               from b in _IssueContext.clientTable
                //               where (a.ISMMCLT_Id == b.ISMMCLT_Id && b.MI_Id == data.MI_Id)
                //               select a).Distinct().ToList();
                //if (ieslist.Count > 0)
                //{
                //    List<long> iesid = new List<long>();
                //    foreach (var item in ieslist)
                //    {
                //        iesid.Add(item.ISMCIM_IEList);
                //    }

                //data.ieEmpList = (from b in _IssueContext.MasterEmployee
                //                  where (!iesid.Contains(b.HRME_Id) && b.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                //                  select new Clients_DTO
                //                  {
                //                      HRME_Id = b.HRME_Id,
                //                      ieEmpname = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                //                  }).Distinct().OrderBy(t => t.HRME_Id).ToArray();
                //}
                //else
                //{
                string roles = "ClientUser,Admin";
                data.ieEmpList = (from a in _Context.appUserRole
                                  from b in _Context.MasterRoleType
                                  from c in _Context.ApplicationUser
                                  where (a.RoleTypeId == b.IVRMRT_Id && a.UserId == c.Id && roles.Contains(b.IVRMRT_RoleFlag))
                                  select new Clients_DTO
                                  {
                                      UserId = a.UserId,
                                      ieEmpname = c.NormalizedUserName,
                                  }).Distinct().OrderBy(t => t.ieEmpname).ToArray();

                data.allIeMappingdata = (from a in _IssueContext.clientTable
                                         from b in _IssueContext.ISM_Master_Client_UserMappingDMO
                                         from c in _IssueContext.ApplicationUser
                                         where (a.MI_Id == data.MI_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && c.Id == b.ISMCIM_IEList  && a.ISMMCLT_ActiveFlag == true)
                                         select new Clients_DTO
                                         {
                                             ISMMCLT_Id = a.ISMMCLT_Id,
                                             ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                             username = c.NormalizedUserName
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
                if (data.ISMMCLT_Id == 0)
                {
                    for (int i = 0; i < data.ieEmpListdata.Length; i++)
                    {
                        var tempid = data.ieEmpListdata[i].User_Id;

                        ISM_Master_Client_UserMappingDMO obj = new ISM_Master_Client_UserMappingDMO();

                        //obj.ISMMCLTUS_Id = data.ISMMCLTUS_Id;
                        obj.ISMMCLT_Id = data.ISMMCLT_Id;
                        obj.ISMCIM_IEList = tempid;
                        obj.ISMMCLTUS_ActiveFlag = true;
                        obj.ISMMCLTUS_CreatedBy = data.UserId;
                        obj.ISMMCLTUS_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _IssueContext.Add(obj);
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
                    var remove = _IssueContext.ISM_Master_Client_UserMappingDMO.Where(t => t.ISMMCLT_Id == data.ISMMCLT_Id).ToList();
                    if (remove.Count > 0)
                    {
                        foreach (var item in remove)
                        {
                            _IssueContext.Remove(item);
                        }
                    }

                    for (int s = 0; s < data.ieEmpListdata.Length; s++)
                    {
                        var tempid = data.ieEmpListdata[s].User_Id;
                        ISM_Master_Client_UserMappingDMO obj = new ISM_Master_Client_UserMappingDMO();

                        //obj.ISMMCLTUS_Id = data.ISMMCLTUS_Id;
                        obj.ISMMCLT_Id = data.ISMMCLT_Id;
                        obj.ISMCIM_IEList = tempid;
                        obj.ISMMCLTUS_ActiveFlag = true;
                        obj.ISMMCLTUS_CreatedBy = data.UserId;
                        obj.ISMMCLTUS_UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _IssueContext.Add(obj);

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
                var result = _IssueContext.ISM_Master_Client_UserMappingDMO.Single(t => t.ISMMCLTUS_Id == data.ISMMCLTUS_Id);
                if (result.ISMMCLTUS_ActiveFlag == true)
                {
                    result.ISMMCLTUS_ActiveFlag = false;
                }
                else
                {
                    result.ISMMCLTUS_ActiveFlag = true;
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

                data.modalUserMapingList = (from a in _IssueContext.clientTable
                                            from b in _IssueContext.ISM_Master_Client_UserMappingDMO
                                            from c in _IssueContext.MasterEmployee
                                            where (a.MI_Id == data.MI_Id && a.ISMMCLT_Id == data.ISMMCLT_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMCIM_IEList == c.HRME_Id && c.HRME_ActiveFlag == true && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.ISMMCLT_ActiveFlag == true)
                                            select new Clients_DTO
                                            {
                                                ISMMCLT_Id = a.ISMMCLT_Id,
                                                ISMMCLTUS_Id = b.ISMMCLTUS_Id,
                                                ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                                ISMCIM_IEList = b.ISMCIM_IEList,
                                                ISMMCLTUS_ActiveFlag = b.ISMMCLTUS_ActiveFlag,
                                                useempName = c.HRME_EmployeeFirstName + (string.IsNullOrEmpty(c.HRME_EmployeeMiddleName) ? " " : ' ' + c.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(c.HRME_EmployeeLastName) ? " " : ' ' + c.HRME_EmployeeLastName),
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
                data.editCltMappinglist = (from a in _IssueContext.clientTable
                                           from b in _IssueContext.ISM_Master_Client_UserMappingDMO
                                           where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.ISMMCLT_Id == data.ISMMCLT_Id && a.MI_Id == data.MI_Id)
                                           select new Clients_DTO
                                           {
                                               ISMMCLT_Id = a.ISMMCLT_Id,
                                               ISMMCLTUS_Id = b.ISMMCLTUS_Id,
                                               ISMCIM_IEList = b.ISMCIM_IEList,
                                           }).Distinct().ToArray();

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

        public Clients_DTO get_MappedUserlist(Clients_DTO data)
        {
            try
            {
                data.modalUserMapingList = (from a in _IssueContext.clientTable
                                            from b in _IssueContext.ISM_Master_Client_UserMappingDMO
                                            from c in _IssueContext.MasterEmployee
                                            where (a.MI_Id == data.MI_Id && a.ISMMCLT_Id == data.ISMMCLT_Id && a.ISMMCLT_Id == b.ISMMCLT_Id && b.ISMCIM_IEList == c.HRME_Id && c.HRME_ActiveFlag == true && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.ISMMCLT_ActiveFlag == true)
                                            select new Clients_DTO
                                            {
                                                ISMMCLT_Id = a.ISMMCLT_Id,
                                                ISMMCLTUS_Id = b.ISMMCLTUS_Id,
                                                ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                                ISMCIM_IEList = b.ISMCIM_IEList,
                                                ISMMCLTUS_ActiveFlag = b.ISMMCLTUS_ActiveFlag,
                                                useempName = c.HRME_EmployeeFirstName + (string.IsNullOrEmpty(c.HRME_EmployeeMiddleName) ? " " : ' ' + c.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(c.HRME_EmployeeLastName) ? " " : ' ' + c.HRME_EmployeeLastName),
                                            }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
