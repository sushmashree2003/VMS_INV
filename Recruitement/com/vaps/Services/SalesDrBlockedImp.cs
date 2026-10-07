using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class SalesDrBlockedImp : SalesDrBlockedInterface 
    {
        private readonly VMSContext _context;
        private readonly DomainModelMsSqlServerContext _db;

        public SalesDrBlockedImp(VMSContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }

        public EmployeeDataDto OnLoadData(EmployeeDataDto dto)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "HR_Employee_Names";
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
                    dto.EmpDetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

           

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SalesDRBlocked_onloaddata";
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
                    dto.MasterData = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ISM_Sales_Negotiation_Onload";
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
                    dto.SalesNegaotionData = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }


            return dto;
        }
        public EmployeeDataDto Save (EmployeeDataDto dto)
        {
            try
            {            
                var existingData = _context.Master_DRBlocked_ScenarioWiseDMO .Where(dm => dm.HRME_ID == dto.HRME_ID && dm.Scenario== dto.Scenario).ToList();
                if (existingData.Count > 0)
                {
                    dto.returndata = "Duplicate";
                }
                else
                {
                    Master_DRBlocked_ScenarioWiseDMO cmc = new Master_DRBlocked_ScenarioWiseDMO();
                    cmc.HRME_ID = dto.HRME_ID;
                    cmc.Scenario = dto.Scenario;
                    cmc.ActiveFlag = true;
                    cmc.MDS_CreatedDate = DateTime.Now;
                    cmc.MDS_CreatedBy = dto.User_Id;
                    cmc.MDS_UpdatedDate = DateTime.Now;
                    cmc.MDS_UpdatedBy = dto.User_Id;
                    cmc.User_Id = dto.User_Id;
                    _context.Add(cmc);
                    var vv = _context.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Notsave";
                    }
                }              
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returndata = "admin";
            }          
            return dto;
        }
        public EmployeeDataDto Deactivate(EmployeeDataDto dto)
        {
            try
            {
                var result = _context.Master_DRBlocked_ScenarioWiseDMO.Single(a => a.MDS_id == dto.MDS_id);                  
                if (result.ActiveFlag == true)
                {
                    result.ActiveFlag = false;                   
                }
                else
                {
                    result.ActiveFlag = true;
                }
                result.MDS_UpdatedDate = DateTime.Now;
                result.MDS_UpdatedBy = dto.User_Id;
                _context.Update(result);
                var vv = _context.SaveChanges();
                if (vv > 0)
                {
                    dto.returndata = "false";
                }
                else
                {
                    dto.returndata = "Error";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        //  master data
        public Ivrm_ModuleDto OnLoadIvrmData(Ivrm_ModuleDto dto)
        {
                 
           // dto.ivrmData = _context.masterModule.Where(iv => iv.Module_ActiveFlag == 1).Distinct().ToArray();
            dto.ivrmData = _context.masterModule.Where(R=>R.Module_ActiveFlag==1).Distinct().ToArray();
            dto.ism_SalesData = _context.ISM_Sales_BOMDMO.ToArray();

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ISM_Master_Module_Details";
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
                    dto.MasterData = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

           
            return dto;
        }


        public Ivrm_ModuleDto ISM_Master_Module_BOMSave(Ivrm_ModuleDto dto)
        {
            try
            {    
               
                if(dto.IVRMMIDS !=null  && dto.IVRMMIDS.Length > 0)
                {
                    foreach(var d  in dto.IVRMMIDS)
                    {
                        var duplicate = _context.ISM_Master_Module_BOMDMO.Where(R => R.IMMB_BOMID == dto.IMMB_BOMID && R.IMMB_IVRMMID == d.ivrmM_Id && R.IMMB_InsType == dto.IMMB_InsType).ToList();
                        if(duplicate.Count >0)
                        {
                            dto.returndata = "Duplicate";
                        }
                        else
                        {
                            ISM_Master_Module_BOMDMO obj = new ISM_Master_Module_BOMDMO();
                            obj.IMMB_IVRMMID = d.ivrmM_Id;
                            obj.IMMB_BOMID = dto.IMMB_BOMID;
                            obj.IMMB_InsType = dto.IMMB_InsType;
                            obj.IMMB_ActiveFlag = true;
                            obj.IMMB_CreatedBy = dto.User_Id;
                            obj.IMMB_UpdatedBy = dto.User_Id;
                            obj.IMMB_CreatedDate = DateTime.Now;
                            obj.IMMB_UpdatedDate= DateTime.Now;
                            obj.IMMB_IVRMMID_Order = 1;
                            _context.Add(obj);

                        }
                        
                    }
                    var vv = _context.SaveChanges();
                    if (vv > 0)
                    {
                        dto.returndata = "Add";
                    }
                    else
                    {
                        dto.returndata = "Notsave";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returndata = "admin";
            }
            return dto;
        }


        public Ivrm_ModuleDto SaveModuleOrder(Ivrm_ModuleDto dto)
        {
            try
            {
                int i = 0;
                if (dto.ModuleDetails != null && dto.ModuleDetails.Length > 0)
                {
                    foreach (var d in dto.ModuleDetails)
                    {
                        var details = _context.ISM_Master_Module_BOMDMO.Where(R => R.IMMB_ID == d.IMMB_ID).FirstOrDefault();
                        if (details != null)
                        {
                            details.IMMB_IVRMMID_Order = d.IMMB_IVRMMID_Order;
                            _context.Update(details);
                            _context.SaveChanges();
                            i = i + 1;
                        }
                        else
                        {
                            //ISM_Master_Module_BOMDMO obj = new ISM_Master_Module_BOMDMO();
                            //obj.IMMB_IVRMMID = d.ivrmM_Id;
                            //obj.IMMB_BOMID = dto.IMMB_BOMID;
                            //obj.IMMB_InsType = dto.IMMB_InsType;
                            //obj.IMMB_ActiveFlag = true;
                            //obj.IMMB_CreatedBy = dto.User_Id;
                            //obj.IMMB_UpdatedBy = dto.User_Id;
                            //obj.IMMB_CreatedDate = DateTime.Now;
                            //obj.IMMB_UpdatedDate = DateTime.Now;
                            //_context.Add(obj);
                            dto.returndata = "Notsave";

                        }

                    }
                   
                   
                }
                if (i > 0)
                {
                    dto.returndata = "update";
                }
                else
                {
                    dto.returndata = "Notupdate";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returndata = "admin";
            }
            return dto;
        }

        public Ivrm_ModuleDto MasterDataDeactivate (Ivrm_ModuleDto dto)
        {
            try
            {
                 var result1 = _context.ISM_Master_Module_BOMDMO.Single(a => a.IMMB_ID == dto.IMMB_ID);

                // var result2 = _context.ISM_Sales_BOMDMO.Single(a => a.ISMSBOM_Id == dto.IMMB_BOMID);

               // var result1 = _context.masterModule.Single(a => a.IVRMM_Id == dto.IMMB_IVRMMID);

                result1.IMMB_UpdatedDate = DateTime.Now;
                result1.IMMB_UpdatedBy =dto.userid;
                if (result1.IMMB_ActiveFlag == true)
                {
                    result1.IMMB_ActiveFlag = false;
                }
                else
                {
                    result1.IMMB_ActiveFlag = true;
                }
                _context.Update(result1);
                //result2.ISMSBOM_ActiveFlag = result2.ISMSBOM_ActiveFlag == true ? result2.ISMSBOM_ActiveFlag == false : result2.ISMSBOM_ActiveFlag == true;
                //result2.ISMSBOM_UpdatedDate = DateTime.Now;
                //result2.ISMSBOM_UpdatedBy = dto.User_Id;
                //_context.Update(result2);
                //if (result.IMMB_ActiveFlag == true)
                //{
                //    result.IMMB_ActiveFlag = false;
                //}
                //else
                //{
                //    result.IMMB_ActiveFlag = true;
                //}
                //result.IMMB_UpdatedDate = DateTime.Now;
                //result.IMMB_UpdatedBy = dto.User_Id;
                //_context.Update(result);
                var vv = _context.SaveChanges();
                //var vv1 = _context.SaveChanges();
                if (vv > 0 )
                {
                    dto.returndata = "false";
                }
                else
                {
                    dto.returndata = "Error";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionSave(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            try
            {

                if (dto.ISLIDN_ID > 0)
                {
                    var ISM_Sales_Lead_Negaotion_DesignationDMO_update = _context.ISM_Sales_Lead_Negaotion_DesignationDMO.Where(a => a.ISLIDN_Value == dto.ISLIDN_Value &&a.HRME_Id==dto.HRME_Id &&a.ISLIDN_Remark.Equals(dto.ISLIDN_Remark) && a.ISLIDN_ID != dto.ISLIDN_ID).ToList();

                    if (ISM_Sales_Lead_Negaotion_DesignationDMO_update.Count == 0)
                    {
                        var result = _context.ISM_Sales_Lead_Negaotion_DesignationDMO.Single(a => a.ISLIDN_ID == dto.ISLIDN_ID && a.HRME_Id == dto.HRME_Id);
                        result.ISLIDN_Remark = dto.ISLIDN_Remark;
                        result.ISLIDN_Value = dto.ISLIDN_Value;
                        result.ISLIDN_CreatedDate = result.ISLIDN_CreatedDate;
                        result.ISLIDN_UpdatedDate = DateTime.Now;
                        result.ISLIDN_UpdatedBy = dto.User_Id;
                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            dto.returndata = "Update";
                        }
                        else
                        {
                            dto.returndata = "Update";
                        }
                    }
                    else
                    {
                        dto.returndata = "Duplicate";
                    }
                }
                else
                {

                    var existingData = _context.ISM_Sales_Lead_Negaotion_DesignationDMO.Where(dm => dm.HRME_Id == dto.HRME_Id).ToList();
                    if (existingData.Count > 0)
                    {
                        dto.returndata = "Duplicate";
                    }
                    else
                    {
                        ISM_Sales_Lead_Negaotion_DesignationDMO cmc = new ISM_Sales_Lead_Negaotion_DesignationDMO();
                        cmc.HRME_Id = dto.HRME_Id;
                        cmc.ISLIDN_Value = dto.ISLIDN_Value;
                        cmc.ISLIDN_Remark = dto.ISLIDN_Remark;
                        cmc.ISLIDN_ActiveFlag = true;
                        cmc.ISLIDN_CreatedDate = DateTime.Now;
                        cmc.ISLIDN_CreatedBy = dto.User_Id;
                        cmc.ISLIDN_UpdatedDate = DateTime.Now;
                        cmc.ISLIDN_UpdatedBy = dto.User_Id;
                        _context.Add(cmc);
                        var vv = _context.SaveChanges();
                        if (vv > 0)
                        {
                            dto.returndata = "Add";
                        }
                        else
                        {
                            dto.returndata = "Notsave";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.returndata = "admin";
            }
         
            return dto;
        }


        public ISM_Sales_Lead_Negaotion_DesignationDto SalesNegaotionDeactivate(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            try
            {
                var result = _context.ISM_Sales_Lead_Negaotion_DesignationDMO.Single(a => a.ISLIDN_ID == dto.ISLIDN_ID);
                if (result.ISLIDN_ActiveFlag == true)
                {
                    result.ISLIDN_ActiveFlag = false;
                }
                else
                {
                    result.ISLIDN_ActiveFlag = true;
                }
                result.ISLIDN_UpdatedDate = DateTime.Now;
                result.ISLIDN_UpdatedBy = dto.User_Id;
                _context.Update(result);
                var vv = _context.SaveChanges();
                if (vv > 0)
                {
                    dto.returndata = "false";
                }
                else
                {
                    dto.returndata = "Error";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public ISM_Sales_Lead_Negaotion_DesignationDto SalesQuatationReport(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Quatation_Approved_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = dto.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = dto.todate
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
                        dto.SalesQuatationReport = retObject.ToArray();
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
        public ISM_Sales_Lead_Negaotion_DesignationDto OnLoadDataReport(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_Ind_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar)
                    {
                        Value = dto.User_Id
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
                        dto.EmployeeData = retObject.ToArray();
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

        public ISM_Sales_Lead_Negaotion_DesignationDto Employee_Ind_Detail(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Demo_Rating_Indvl_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar)
                    {
                        Value = dto.User_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.VarChar)
                    {
                        Value = dto.Year
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
                        dto.Employee_Ind_Details = retObject.ToArray();
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

        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Detail_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Negotation_Detail_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = dto.todate
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
                        dto.Sales_Negotation_Detail_Report = retObject.ToArray();
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

        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Lead_Demo_Swapping_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Lead_Demo_Swapping_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = dto.todate
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
                        dto.Sales_Lead_Demo_Swapping_Report = retObject.ToArray();
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

        public ISM_Sales_Lead_Negaotion_DesignationDto Sales_Negotation_Payment_Report(ISM_Sales_Lead_Negaotion_DesignationDto dto)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sales_Negotation_Payment_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = dto.todate
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
                        dto.Sales_Negotation_Payment_Report = retObject.ToArray();
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
