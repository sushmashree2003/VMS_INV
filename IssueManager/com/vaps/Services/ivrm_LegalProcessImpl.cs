using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Legal;
using IssueManager.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ivrm_LegalProcessImpl : Interfaces.ivrm_LegalProcessInterface
    {
        public IssueManagerContext _IssueContext;

        public DomainModelMsSqlServerContext _Context;


        public ivrm_LegalProcessImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        string departmentdatacode = "";

        public Legal_Notice_RequestDTO Onloaddata(Legal_Notice_RequestDTO dto)
        {
            //var retObject = new List<dynamic>();
            try
            {
                departmentdatacode = "";
                //to check role 
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.roleId).ToList();
                //to get department code 
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    var emp_hrme = _IssueContext.Staff_User_Login.Where(S => S.Id == dto.UserId && S.MI_Id == dto.MI_Id).ToArray();

                    var employeedata = _IssueContext.MasterEmployee.Where(B => B.HRME_Id == emp_hrme[0].Emp_Code && B.MI_Id == dto.MI_Id).ToArray();

                    var departmentdata = _IssueContext.HR_Master_Department.Where(D => D.HRMD_Id == employeedata[0].HRMD_Id && D.MI_Id == dto.MI_Id).ToArray();

                    departmentdatacode = _IssueContext.HR_Master_DepartmentCodeDMO.Where(C => C.HRMDC_ID == departmentdata[0].HRMDC_ID).Select(c => c.HRMDC_Code).FirstOrDefault();

                }

                if (rolet.Count > 0)
                {
                    dto.Role_flag = rolet[0].IVRMRT_Role;


                    if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase) && departmentdatacode.Equals("MANG", StringComparison.OrdinalIgnoreCase))
                    {


                        dto.category = _IssueContext.Legal_Notice_Master_CategoryDMO.Where(t => t.LNC_ActiveFlag == true).ToArray();
                        dto.employees = (from a in _IssueContext.MasterEmployee
                                         join b in _IssueContext.Legal_Notice_RequestDMO on a.HRME_Id equals b.RequestingFor_id
                                         where b.LNR_ActiveFlag == true && b.LNR_Status == "p" && b.LNC_Id == 1
                                         select new Legal_Notice_RequestDTO
                                         {
                                             HRME_EmployeeFirstName = string.Concat(a.HRME_EmployeeFirstName, " ", a.HRME_EmployeeLastName),
                                             HRME_Id = a.HRME_Id,
                                             LNR_Id = b.LNR_Id
                                         })
             .Distinct()
             .ToArray();


                        dto.clientlist = (from a in _IssueContext.clientTable
                                          join b in _IssueContext.Legal_Notice_RequestDMO on a.ISMMCLT_Id equals b.RequestingFor_id
                                          where b.LNR_ActiveFlag == true && b.LNR_Status == "p" && b.LNC_Id == 2
                                          select new Legal_Notice_RequestDTO
                                          {
                                              ISMMCLT_ClientName = string.Concat(a.ISMMCLT_ClientName, " ", a.ISMMCLT_ClientName),
                                              ISMMCLT_Id = a.ISMMCLT_Id,
                                              LNR_Id = b.LNR_Id
                                          })
                  .Distinct()
                  .ToArray();


                        dto.vendorlist = (from a in _IssueContext.INV_Master_SupplierDMO
                                          join b in _IssueContext.Legal_Notice_RequestDMO on a.INVMS_Id equals b.RequestingFor_id
                                          where b.LNR_ActiveFlag == true && b.LNR_Status == "p" && b.LNC_Id == 3
                                          select new Legal_Notice_RequestDTO
                                          {
                                              INVMS_SupplierName = string.Concat(a.INVMS_SupplierName, " ", a.INVMS_SupplierName),
                                              INVMS_Id = a.INVMS_Id,
                                              LNR_Id = b.LNR_Id
                                          })
                  .Distinct()
                  .ToArray();

                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "LNC_Categorywise_LegalDetails";
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
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }

                                dto.griddata = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        //dto.griddata = (from a in _IssueContext.Legal_Notice_Master_CategoryDMO
                        //                join b in _IssueContext.Legal_Notice_RequestDMO
                        //                on a.LNC_Id equals b.LNC_Id
                        //                select new Legal_Notice_RequestDTO
                        //                {
                        //                    LNC_Category_Name = a.LNC_Category_Name,
                        //                    LNR_Remarks = b.LNR_Remarks,
                        //                    LNR_Status = b.LNR_Status,
                        //                    LNR_CreatedDate = b.LNR_CreatedDate,
                        //                    LNR_CreatedBy = b.LNR_CreatedBy,
                        //                    RequestingFor_id = b.RequestingFor_id
                        //                })
                        //               .Distinct()
                        //               .ToArray();

                    }
                    else
                    {
                        dto.category = _IssueContext.Legal_Notice_Master_CategoryDMO.Where(t => t.LNC_ActiveFlag == true).ToArray();

                        // dto.employees = _IssueContext.MasterEmployee.Where(mm => mm.HRME_ActiveFlag == true).OrderBy(mm => mm.HRME_EmployeeOrder).Distinct().ToArray();

                        dto.employees = (from a in _IssueContext.MasterEmployee
                                         where a.HRME_ActiveFlag == true
                                         select new Legal_Notice_RequestDTO
                                         {
                                             HRME_EmployeeFirstName = string.Concat(a.HRME_EmployeeFirstName, " ", a.HRME_EmployeeLastName),
                                             HRME_Id = a.HRME_Id
                                             //LNR_Id = b.LNR_Id
                                         })
             .Distinct()
             .ToArray();

                        dto.clientlist = _IssueContext.clientTable.Where(a => a.MI_Id == dto.MI_Id && a.ISMMCLT_ActiveFlag == true).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();

                        dto.vendorlist = _IssueContext.INV_Master_SupplierDMO.Where(a => a.MI_Id == dto.MI_Id && a.INVMS_ActiveFlg == true).Distinct().OrderBy(a => a.INVMS_SupplierName).ToArray();

                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "LNC_Categorywise_LegalDetails";
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
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }

                                dto.griddata = retObject.ToArray();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;
        }

        public Legal_Notice_RequestDTO Onloadcourtdata(Legal_Notice_RequestDTO dto)
        {
            //var retObject = new List<dynamic>();
            try
            {
                {

                    dto.category1 = _IssueContext.LNC_Master_Court_TypeDMO.Where(t => t.MCT_ActiveFlag == true).ToArray();

                    dto.gridcourt = (from a in _IssueContext.LNC_Master_Court_TypeDMO
                                     join b in _IssueContext.LNC_Master_CourtDMO
                                    on a.MCT_Id equals b.MCT_Id
                                     select new Legal_Notice_RequestDTO
                                     {
                                         MC_Id = b.MC_Id,
                                         MCT_Id = a.MCT_Id,
                                         MCT_Type = a.MCT_Type,
                                         MC_Name = b.MC_Name,
                                         MC_ActiveFlag = b.MC_ActiveFlag,
                                         MC_CreatedDate = b.MC_CreatedDate,
                                         MC_UpdatedDate = b.MC_UpdatedDate
                                     })
                                   .Distinct()
                                   .ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;
        }

        public Legal_Notice_RequestDTO Onloadcasetype(Legal_Notice_RequestDTO dto)
        {
            //var retObject = new List<dynamic>();
            try
            {
                {
                    dto.gridcasetype = _IssueContext.LNC_Master_Case_TypeDMO.ToArray();                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;
        }

        public Legal_Notice_RequestDTO Onloadcasetypemapping(Legal_Notice_RequestDTO dto)
        {           
            try
            {
                {
                    dto.category1 = _IssueContext.LNC_Master_Court_TypeDMO.Where(t => t.MCT_ActiveFlag == true).ToArray();

                    dto.courtcomplex = (from a in _IssueContext.LNC_Master_Court_TypeDMO
                                       from b in _IssueContext.LNC_Master_CourtDMO
                                       where a.MCT_Id==b.MCT_Id && b.MCT_Id==1
                                       select b
                                      ).ToArray();



                    dto.courtestablishment = (from a in _IssueContext.LNC_Master_Court_TypeDMO
                                              from b in _IssueContext.LNC_Master_CourtDMO
                                              where a.MCT_Id == b.MCT_Id && b.MCT_Id == 2
                                              select b
                                      ).ToArray();

                    dto.casetype = _IssueContext.LNC_Master_Case_TypeDMO.ToArray();

                    //Where(t => t.MC_Name == dto.MC_Name)


                    dto.gridcasetypemapping = _IssueContext.LNC_Court_Casetype_MappingDMO.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;
        }

        public Legal_Notice_RequestDTO savedetail(Legal_Notice_RequestDTO dto)
        {
            try
            {
                departmentdatacode = "MANG";
                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == dto.roleId).ToList();
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("staff", StringComparison.OrdinalIgnoreCase) && departmentdatacode.Equals("MANG", StringComparison.OrdinalIgnoreCase))
                {


                    var remarks = _IssueContext.Legal_Notice_RequestDMO.Where(T => T.LNC_Id == dto.LNC_Id && T.RequestingFor_id == dto.RequestingFor_id && T.LNR_Id == dto.LNR_Id && T.LNR_ActiveFlag == true).ToArray();
                    string lrh_Remarks = remarks[0].LNR_Remarks;
                    // Legal team is logged in
                    var existingRecord = _IssueContext.Legal_Notice_RequestDMO
                .SingleOrDefault(t => t.LNR_Id == dto.LNR_Id && t.LNR_ActiveFlag == true);

                    if (existingRecord != null)
                    {
                        // Move existing record to history
                        Legal_Notice_Request_HistoryDMO hist = new Legal_Notice_Request_HistoryDMO();
                        {
                            hist.LNRH_Id = dto.LNRH_Id;
                            hist.LNC_Id = existingRecord.LNC_Id;
                            hist.LNR_Id = existingRecord.LNR_Id;
                            hist.MI_ID = dto.MI_Id;
                            hist.LNR_Remarks = existingRecord.LNR_Remarks;
                            hist.LNRH_ActiveFlag = true;
                            hist.LNR_CreatedDate = DateTime.Now;
                            hist.LNR_UpdatedDate = DateTime.Now;
                            hist.LNR_CreatedBy = dto.UserId;
                            hist.LNR_UpdatedBy = dto.UserId;
                            hist.LNRH_RequestingFor_id = dto.RequestingFor_id;
                        }

                        _IssueContext.Add(hist);
                        _IssueContext.SaveChanges();

                        var result = _IssueContext.Legal_Notice_RequestDMO.Single(M => M.LNR_Id == remarks[0].LNR_Id);
                        result.LNC_Id = dto.LNC_Id;
                        result.MI_ID = dto.MI_Id;
                        result.ASMAY_ID = dto.ASMAY_ID;
                        result.LNR_Remarks = dto.LNR_Remarks;
                        result.LNR_ActiveFlag = true;
                        result.LNR_CreatedDate = DateTime.Now;
                        result.LNR_UpdatedDate = DateTime.Now;
                        result.LNR_CreatedBy = dto.UserId;
                        result.LNR_UpdatedBy = dto.UserId;
                        result.RequestingFor_id = dto.RequestingFor_id;
                        result.LNR_Status = "G";
                        _IssueContext.Update(result);
                        var update = _IssueContext.SaveChanges();
                        if (update > 0)
                        {
                            dto.update = "updated";
                        }

                    }

                }
                else //this is for admin login
                {
                    var check = _IssueContext.MasterEmployee.Where(t => t.HRME_Id == dto.HRME_Id).ToList();

                    if (dto.LNR_Id > 0)
                    {
                        var result = _IssueContext.Legal_Notice_RequestDMO.Single(t => t.LNR_Id == dto.LNR_Id);
                        result.RequestingFor_id = dto.RequestingFor_id;
                        result.LNC_Id = dto.LNC_Id;
                        result.LNR_Remarks = dto.LNR_Remarks;
                        _IssueContext.Update(result);
                        _IssueContext.SaveChanges();
                        dto.returnvales = "Update";
                    }
                    else
                    {
                        Legal_Notice_RequestDMO lr = new Legal_Notice_RequestDMO();
                        lr.LNC_Id = dto.LNC_Id;
                        lr.MI_ID = dto.MI_Id;
                        lr.ASMAY_ID = dto.ASMAY_ID;
                        lr.LNR_Remarks = dto.LNR_Remarks;
                        lr.LNR_ActiveFlag = true;
                        lr.LNR_CreatedDate = DateTime.Now;
                        lr.LNR_UpdatedDate = DateTime.Now;
                        lr.LNR_CreatedBy = dto.UserId;
                        lr.LNR_UpdatedBy = dto.UserId;
                        lr.RequestingFor_id = dto.RequestingFor_id;

                        lr.LNR_Status = "P";

                        _IssueContext.Add(lr);
                        _IssueContext.SaveChanges();
                        dto.returnvales = "Add";

                        for (int j = 0; j < dto.FilePath_Array.Length; j++)
                        {
                            Legal_Notice_Request_DocumentDMO checkresult = new Legal_Notice_Request_DocumentDMO();
                            checkresult.LNR_Id = lr.LNR_Id;
                            checkresult.LNRD_Document = dto.FilePath_Array[j].LNRD_Document;
                            checkresult.LNRD_UpdatedBy = dto.UserId;
                            checkresult.LNRD_CreatedBy = dto.UserId;
                            checkresult.LNRD_UpdatedDate = DateTime.Now;
                            checkresult.LNRD_CreatedDate = DateTime.Now;
                            _IssueContext.Add(checkresult);

                            var count = _IssueContext.SaveChanges();
                            if (count > 0)
                            {
                                dto.returnvales = "Add";
                            }
                            else
                            {
                                dto.returnvales = "false";
                            }
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Legal_Notice_RequestDTO legalcourtsave(Legal_Notice_RequestDTO dto)
        {
            try
            {
                if (dto.MC_Id > 0)
                {
                    var result = _IssueContext.LNC_Master_CourtDMO.Single(t => t.MC_Id == dto.MC_Id);
                    result.MC_Id = dto.MC_Id;
                    result.MCT_Id = dto.MCT_Id;
                    result.MC_Name = dto.MC_Name;
                    result.MC_ActiveFlag = dto.MC_ActiveFlag;
                    result.MC_CreatedDate = DateTime.Now;
                    result.MC_UpdatedDate = DateTime.Now;
                    result.MC_CreatedBy = dto.UserId;
                    result.MC_UpdatedBy = dto.UserId;
                    _IssueContext.Update(result);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    LNC_Master_CourtDMO hm = new LNC_Master_CourtDMO();
                    hm.MC_Id = dto.MC_Id;
                    hm.MCT_Id = dto.MCT_Id;
                    hm.MC_Name = dto.MC_Name;
                    hm.MC_ActiveFlag = dto.MC_ActiveFlag;
                    hm.MC_CreatedDate = DateTime.Now;
                    hm.MC_UpdatedDate = DateTime.Now;
                    hm.MC_CreatedBy = dto.UserId;
                    hm.MC_UpdatedBy = dto.UserId;
                    _IssueContext.Add(hm);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //save funcationality for legalcasetype
        public Legal_Notice_RequestDTO legalcasetypesave(Legal_Notice_RequestDTO dto)
        {
            try
            {
                if (dto.MCST_Id > 0)
                {
                    var result = _IssueContext.LNC_Master_Case_TypeDMO.Single(t => t.MCST_Id == dto.MCST_Id);
                    result.MCST_Id = dto.MCST_Id;
                    result.MCST_Name = dto.MCST_Name;
                    result.MCST_ActiveFlag = dto.MCST_ActiveFlag;
                    result.MCST_CreatedDate = DateTime.Now;
                    result.MCST_UpdatedDate = DateTime.Now;
                    result.MCST_CreatedBy = dto.UserId;
                    result.MCST_UpdatedBy = dto.UserId;
                    _IssueContext.Update(result);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    LNC_Master_Case_TypeDMO hm = new LNC_Master_Case_TypeDMO();
                    hm.MCST_Id = dto.MCST_Id;
                    hm.MCST_Name = dto.MCST_Name;
                    hm.MCST_ActiveFlag = dto.MCST_ActiveFlag;
                    hm.MCST_CreatedDate = DateTime.Now;
                    hm.MCST_UpdatedDate = DateTime.Now;
                    hm.MCST_CreatedBy = dto.UserId;
                    hm.MCST_UpdatedBy = dto.UserId;
                    _IssueContext.Add(hm);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Legal_Notice_RequestDTO Legalcasetypemappingsave(Legal_Notice_RequestDTO dto)
        {
            try
            {
                if (dto.CCM_Id > 0)
                {
                    var result = _IssueContext.LNC_Court_Casetype_MappingDMO.Single(t => t.CCM_Id == dto.CCM_Id);
                    result.CCM_Id = dto.CCM_Id;
                    result.CCM_Court = dto.CCM_Court;
                    result.CCM_Case = dto.CCM_Case;
                    result.CCM_ActiveFlag = dto.CCM_ActiveFlag;
                    result.CCM_CreatedDate = DateTime.Now;
                    result.CCM_UpdatedDate = DateTime.Now;
                    result.CCM_CreatedBy = dto.UserId;
                    result.CCM_UpdatedBY = dto.UserId;
                    _IssueContext.Update(result);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    LNC_Court_Casetype_MappingDMO hm = new LNC_Court_Casetype_MappingDMO();
                    hm.CCM_Id = dto.CCM_Id;                  
                    hm.CCM_Court = dto.CCM_Court;
                    hm.CCM_Case = dto.CCM_Case;
                    hm.CCM_ActiveFlag = dto.CCM_ActiveFlag;
                    hm.CCM_CreatedDate = DateTime.Now;
                    hm.CCM_UpdatedDate = DateTime.Now;
                    hm.CCM_CreatedBy = dto.UserId;
                    hm.CCM_UpdatedBY = dto.UserId;
                    _IssueContext.Add(hm);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
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
