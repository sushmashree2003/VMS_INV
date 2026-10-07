using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Lead_Target_MappingnewImpl : Interfaces.Lead_Target_MappingnewInterface
    {

        public VMSContext _vmsconte;
        public HRMSContext _hrmscon;
        private DomainModelMsSqlServerContext _db;
        public Lead_Target_MappingnewImpl(VMSContext vmsContext, HRMSContext hrms, DomainModelMsSqlServerContext db)
        {
            _vmsconte = vmsContext;
            _hrmscon = hrms;
            _db = db;
        }


        public Lead_Target_MappingnewDTO getBasicData(Lead_Target_MappingnewDTO dto)
        {


            try
            {

                List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
                List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();

                //Departmentlist
                Departmentlist = _vmsconte.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                dto.departmentdropdown = Departmentlist.ToArray();

                //Designationlist
                Designationlist = _vmsconte.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                dto.designationdropdown = Designationlist.ToArray();

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TargetMappinglist";

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.TargetMappinglists = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //added new things


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Master_BOM_List";

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeedropdown = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Ism_Sales_Lead_List_NewNegotion ";

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.status_dd = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Negotion_Master_Modules ";

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.emp_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_lead_negaotation_MappedModuleList ";

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.emp_target_count = retObject.ToArray();
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
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


        public Lead_Target_MappingnewDTO get_desig(Lead_Target_MappingnewDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _vmsconte.HRGroupDeptDessgDMO
                                            from b in _vmsconte.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Lead_Target_MappingnewDTO get_employee(Lead_Target_MappingnewDTO data)
        {
            try
            {

                data.employeedropdown = (from a in _vmsconte.HRGroupDeptDessgDMO
                                         from b in _vmsconte.HR_Master_Designation
                                         from c in _vmsconte.Hr_Master_Employee_con
                                         where (c.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRMDES_Id == b.HRMDES_Id && c.HRMD_Id == a.HRMD_Id 
                                         && data.hrmdes_IdList.Contains(a.HRMDES_Id)
                                         && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false)
                                         select c).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public Lead_Target_MappingnewDTO getdetails(Lead_Target_MappingnewDTO data)
        {
            try
            {
                var hrmE_Id = "0";
                if (data.Emptwolist.Length > 0)
                {
                    foreach (var ue in data.Emptwolist)
                    {
                        hrmE_Id = hrmE_Id + "," + ue.HRME_Id;

                    }

                }

                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Sales_Lead_employee";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = hrmE_Id
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
                        data.emp_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.status_dd = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMST_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Lead_Target_MappingnewDTO getcatdetails(Lead_Target_MappingnewDTO data)
        {
            try
            {


                using (var cmd = _vmsconte.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_Task_Category";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.emp_list_cat = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.status_dd = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMST_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Lead_Target_MappingnewDTO SaveTarget(Lead_Target_MappingnewDTO dto)
        {
            try
            {
                if (dto.TargetCreatedDate == null && dto.TargetCreatedDate != "")
                {
                    dto.TargetCreatedDate = DateTime.Now.ToString();
                }





                DateTime createdDate = DateTime.Parse(dto.TargetCreatedDate);
             
               
                //if (dto.ISMSLETARM_Id > 0)
                //{
                //    var result = _vmsconte.ISM_Sales_Lead_TargetMappingDMO.Single(t => t.ISMSLETARM_Id == dto.ISMSLETARM_Id);
                //    result.MI_Id = dto.MI_Id;
                //    result.HRME_Id = dto.HRME_Id;
                //    result.ISMMTCAT_Id = dto.ISMMTCAT_Id;
                //    result.ISMSLETARM_Numbers = dto.ISMSLETARM_Numbers;
                //    result.ISMSLETARM_Periodicity = dto.ISMSLETARM_Periodicity;
                //    result.ISMSLETARM_ApprovedBy = dto.ISMSLETARM_ApprovedBy;
                //    result.ISMSLETARM_ActiveFlag = true;
                //    result.ISMSLETARM_UpdatedBy = dto.User_Id;
                //    result.ISMSLETARM_UpdatedDate = dto.ISMSLETARM_UpdatedDate;
                //    _vmsconte.Update(result);
                //    _vmsconte.SaveChanges();



                //    dto.retrunMsg = "Update";
                //}

                //else
                //{
                // ISM_Sales_Lead_TargetMapping_DMO LTM = new ISM_Sales_Lead_TargetMapping_DMO();

                //    LTM.MI_Id = dto.MI_Id;
                //    LTM.HRME_Id = dto.HRME_Id;
                //    LTM.ISMMTCAT_Id = dto.ISMMTCAT_Id;
                //    LTM.ISMSLETARM_Numbers = dto.ISMSLETARM_Numbers;
                //    LTM.ISMSLETARM_Periodicity = dto.ISMSLETARM_Periodicity;
                //    LTM.ISMSLETARM_ApprovedBy = dto.ISMSLETARM_ApprovedBy;
                //    LTM.ISMSLETARM_CreatedBy = dto.User_Id;
                //    LTM.ISMSLETARM_CreatedDate = DateTime.Now;
                //    LTM.ISMSLETARM_ActiveFlag = true;
                //    _vmsconte.Add(LTM);


                //    var i = _vmsconte.SaveChanges();

                //    if (i > 0)
                //    {
                //        dto.retrunMsg = "Add";
                //    }


                //}

                if (dto.Emplistnew != null && dto.Emplistnew.Length > 0)
                {
                    foreach (var d in dto.Emplistnew)
                    {
                        foreach (var f in d.selectedcat)
                        {
                            var duplicate = _vmsconte.ISM_Sales_Lead_TargetMapping_DMONew.Where(R => R.HRME_Id == d.HRME_Id && R.ISMMTCAT_Id == f.ISMMTCAT_Id).ToList();
                            if (duplicate.Count > 0)
                            {

                            }
                            else
                            {
                                ISM_Sales_Lead_TargetMapping_DMO LTM = new ISM_Sales_Lead_TargetMapping_DMO();
                                LTM.MI_Id = dto.MI_Id;
                                LTM.HRME_Id = d.HRME_Id;
                                LTM.ISMMTCAT_Id = f.ISMMTCAT_Id;
                                LTM.ISMSLETARM_Numbers = f.ISMSLETARM_Numbers;
                                LTM.ISMSLETARM_Periodicity = f.ISMSLETARM_Periodicity;
                                LTM.ISMSLETARM_ApprovedBy = dto.User_Id;
                                LTM.ISMSLETARM_CreatedBy = dto.User_Id;
                                LTM.ISMSLETARM_UpdatedBy = dto.User_Id;
                                //  LTM.ISMSLETARM_CreatedDate = DateTime.Now;
                                LTM.ISMSLETARM_CreatedDate = createdDate;
                                LTM.ISMSLETARM_UpdatedDate = DateTime.Now;
                                LTM.ISMSLETARM_ActiveFlag = true;
                                _vmsconte.Add(LTM);
                            }

                        }
                    }
                    int i = _vmsconte.SaveChanges();
                    if (i > 0)
                    {
                        dto.retrunMsg = "save";
                    }
                    else
                    {
                        //dto.retrunMsg = "Not save ";
                        dto.retrunMsg = "save";
                    }
                }


                //added new things

                if (dto.tagtedit != null && dto.tagtedit.Length > 0)
                {
                    int count = 0;
                    foreach (var d in dto.tagtedit)
                    {
                        //dto.ISMSLETARM_Id bom id
                        //ISMSLETARM_Numbers  leadid
                        //ISMMTCAT_Id module id
                        var leadbomupdate = _vmsconte.Database.ExecuteSqlCommand("ISm_Sales_Negotion_ModuleInsert_New @p0,@p1,@p2,@p3", dto.ISMSLETARM_Numbers, dto.ISMSLETARM_Id, dto.User_Id, d.ISMMTCAT_Id);
                        if (leadbomupdate > 0)
                        {
                            count = count + 1;
                        }
                    }
                    if (count > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "NotAdd";
                    }
                }

                if(dto.ISMMTCAT_Id != null && dto.ISMMTCAT_Id > 0)
                {
                    var Activate = _vmsconte.Database.ExecuteSqlCommand("ISm_Sales_Negotion_Module_Activate_Deactivate @p0,@p1", dto.ISMMTCAT_Id, dto.User_Id);
                    if (Activate > 0)
                    {
                        dto.retrunMsg = "false";
                    }
                    else
                    {
                        dto.retrunMsg = "Error";
                    }

                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public Lead_Target_MappingnewDTO Edit (Lead_Target_MappingnewDTO dto)
        {
            try
            {
                
                if (dto.tagtedit != null && dto.tagtedit.Length > 0)
                {
                    foreach (var d in dto.tagtedit)
                    {
                        var result = _vmsconte.ISM_Sales_Lead_TargetMapping_DMONew.Single(a => a.ISMSLETARM_Id == d.ISMSLETARM_Id);
                           //result.ISMSLETARM_Numbers = d.ISMSLETARM_Numbers;
                           result.ISMSLETARM_UpdatedBy = dto.User_Id;
                           result.ISMSLETARM_UpdatedDate = DateTime.Now;
                           result.ISMSLETARM_ActiveFlag = false;
                           _vmsconte.Update(result);
                           var vv = _vmsconte.SaveChanges();
                           if (vv > 0)
                           {
                            ISM_Sales_Lead_TargetMapping_DMO dmo = new ISM_Sales_Lead_TargetMapping_DMO();
                            dmo.HRME_Id = d.HRME_Id;
                            dmo.ISMMTCAT_Id = d.ISMMTCAT_Id;
                            dmo.ISMSLETARM_ActiveFlag = true;
                            dmo.ISMSLETARM_ApprovedBy = result.ISMSLETARM_ApprovedBy;
                            dmo.ISMSLETARM_CreatedBy =  result.ISMSLETARM_CreatedBy;
                            dmo.ISMSLETARM_Numbers = d.ISMSLETARM_Numbers;
                            dmo.ISMSLETARM_UpdatedDate= DateTime.Now;
                           // dmo.ISMSLETARM_CreatedDate = DateTime.Now;
                            dmo.ISMSLETARM_CreatedDate = result.ISMSLETARM_CreatedDate;
                            dmo.ISMSLETARM_Periodicity = result.ISMSLETARM_Periodicity;
                            dmo.MI_Id = dto.MI_Id;
                            dmo.ISMSLETARM_UpdatedBy = dto.User_Id;
                            _vmsconte.Add(dmo);
                            var v = _vmsconte.SaveChanges();
                            
                                dto.retrunMsg = "update";

                           }
                           else
                           {
                             dto.retrunMsg = "Not save ";
                           }
                    }
                }
                dto.retrunMsg = "update";
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
    }
}
