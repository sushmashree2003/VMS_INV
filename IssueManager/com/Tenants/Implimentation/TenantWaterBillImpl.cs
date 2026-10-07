using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Implimentation
{
    public class TenantWaterBillImpl :Interface.TenantWaterBillInterface
    { 

    public IssueManagerContext _context;

    public TenantWaterBillImpl(IssueManagerContext _cont)
    {
        _context = _cont;
    }

        public TenantWaterBillDTO loaddata(TenantWaterBillDTO data)
        {
            try
            {
                data.years = _context.HR_Master_LeaveYearDMO.Where(t => t.HRMLY_ActiveFlag == true && t.MI_Id.Equals(data.MI_Id)).ToArray();


                data.tenant_array = (from a in _context.TenantAgreement

                                     where (a.MI_Id == data.MI_Id && a.TENANTAGR_ActiveFlg == true)
                                     select new TenantAgreementDTO
                                     {
                                         TENANTAGR_Id = a.TENANTAGR_Id,
                                         TENANTAGR_TenantName = a.TENANTAGR_TenantName

                                     }).ToArray();


                data.monthnames = _context.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();


                data.building = _context.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id && t.HRMB_ActiveFlag == true).ToArray();

                data.tenantdata = (from a in _context.Tenant_Monthly_WaterBillsDMO
                                   from b in _context.HR_Master_Building
                                   from c in _context.IVRM_Month_DMO
                                   where (a.HRMB_ID == b.HRMB_Id && b.MI_Id == a.MI_Id && c.IVRM_Month_Id == Convert.ToInt64(a.TENANTMWBILL_Month) && a.MI_Id==data.MI_Id && b.MI_Id==data.MI_Id)
                                   select new TenantWaterBillDTO
                                   {
                                       TENANTMWBILL_Id=a.TENANTMWBILL_Id,
                                       HRMB_ID=a.HRMB_ID,
                                       HRMB_BuildingName=b.HRMB_BuildingName,
                                       IVRM_Month_Name=c.IVRM_Month_Name,
                                       TENANTMWBILL_TotalAmount=a.TENANTMWBILL_TotalAmount,
                                       TENANTMWBILL_Year=a.TENANTMWBILL_Year



                                   }).ToArray();
                    
                    
                    
               




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TenantWaterBillDTO savetrainerfeedback(TenantWaterBillDTO data)
        {
            try
            {

                if (data.TENANTMWBILL_Id > 0)
                {



                    var result = _context.Tenant_Monthly_WaterBillsDMO.Single(t => t.TENANTMWBILL_Id == data.TENANTMWBILL_Id);
                    var res0 = _context.Tenant_Monthly_WaterBillsDMO.Where(a => a.TENANTMWBILL_Id == data.TENANTMWBILL_Id).ToList();
                    if (res0.Count > 0)
                    {
                        var res01 = _context.Tenant_Monthly_WaterBillsDMO.Single(a => a.TENANTMWBILL_Id == data.TENANTMWBILL_Id);
                        res01.HRMB_ID = data.HRMB_ID;
  
                        res01.TENANTMWBILL_Year = data.TENANTMWBILL_Year;
                        res01.TENANTMWBILL_Month = data.TENANTMWBILL_Month;
                        res01.TENANTMWBILL_CreatedBy = data.UserId;
                        res01.TENANTMWBILL_UpdatedBy = data.UserId;
                        res01.TENANTMWBILL_UpdatedDate = DateTime.Now;
                        res01.TENANTMWBILL_CreatedDate = DateTime.Now;
                        res01.TENANTMWBILL_TotalAmount = data.TENANTMWBILL_TotalAmount;
                        res01.TENANTMWBILL_ActiveFlg = true;


                        _context.Update(res01);
                    }
                    else
                    {
                        Tenant_Monthly_WaterBillsDMO uptaskclt = new Tenant_Monthly_WaterBillsDMO();
                        uptaskclt.TENANTMWBILL_Id = data.TENANTMWBILL_Id;
                        uptaskclt.HRMB_ID = data.HRMB_ID;
                        uptaskclt.TENANTMWBILL_Month = data.TENANTMWBILL_Month;
                        uptaskclt.TENANTMWBILL_CreatedBy = data.UserId;
                        uptaskclt.TENANTMWBILL_UpdatedBy = data.UserId;
                        uptaskclt.TENANTMWBILL_UpdatedDate = DateTime.Now;
                        uptaskclt.TENANTMWBILL_CreatedDate = DateTime.Now;
                        uptaskclt.TENANTMWBILL_TotalAmount = data.TENANTMWBILL_TotalAmount;
                        uptaskclt.TENANTMWBILL_ActiveFlg = true;
                        uptaskclt.TENANTMWBILL_Year = data.TENANTMWBILL_Year;

                        _context.Add(uptaskclt);
                    }



                }


                else
                {
                    var result1 = _context.Tenant_Monthly_WaterBillsDMO.Where(t => t.TENANTMWBILL_TotalAmount == data.TENANTMWBILL_TotalAmount && t.HRMB_ID == data.HRMB_ID && t.TENANTMWBILL_Month == data.TENANTMWBILL_Month && t.TENANTMWBILL_Year == data.TENANTMWBILL_Year).ToList();

                    if (result1.Count > 0)
                    {
                        data.returnmsg = "duplicate";
                    }
                    else
                    {


                        Tenant_Monthly_WaterBillsDMO result = new Tenant_Monthly_WaterBillsDMO();

                        result.TENANTMWBILL_Id = data.TENANTMWBILL_Id;
                        result.HRMB_ID = data.HRMB_ID;
                        result.TENANTMWBILL_Month = data.TENANTMWBILL_Month;
                        result.TENANTMWBILL_CreatedBy = data.UserId;
                        result.TENANTMWBILL_UpdatedBy = data.UserId;
                        result.TENANTMWBILL_UpdatedDate = DateTime.Now;
                        result.TENANTMWBILL_CreatedDate = DateTime.Now;
                        result.TENANTMWBILL_TotalAmount = data.TENANTMWBILL_TotalAmount;
                        result.TENANTMWBILL_ActiveFlg = true;
                        result.TENANTMWBILL_Year = data.TENANTMWBILL_Year;
                        result.MI_Id = data.MI_Id;
                        



                        _context.Add(result);




                    }






                }


                var contactExists = _context.SaveChanges();


              

                var outputval = 0;
                var month = 0;
                
                //if (data.TENANTMWBILL_Month < 10)
                //{
                //    month = ('0' + data.TENANTMWBILL_Month.ToString()).ToString();
                //}
                //else
                //{
                //    month = data.TENANTMWBILL_Month;
                //}


                if (contactExists > 0)
                {
                    var TENANTMWBILL_Id = _context.Tenant_Monthly_WaterBillsDMO.OrderByDescending(t => t.TENANTMWBILL_Id).Take(1).ToList();
                    outputval = _context.Database.ExecuteSqlCommand("TENANTWATERBILLINSERTION @p0,@p1,@p2,@p3,@p4,@p5", TENANTMWBILL_Id[0].TENANTMWBILL_Id, data.TENANTMWBILL_TotalAmount, data.HRMB_ID, data.TENANTMWBILL_Year, data.TENANTMWBILL_Month, data.UserId);

                    if (outputval > 0)
                    {
                        data.returnmsg = "true";
                    }
                


                }
                else
                {
                    data.returnmsg = "false";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public TenantWaterBillDTO viewroomwisewaterbill(TenantWaterBillDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "View_MonthlyWaterBillRoomWise";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TENANTMWBILL_Id", SqlDbType.BigInt) { Value = data.TENANTMWBILL_Id });
                 
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
                        data.tenant_array = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
            public TenantWaterBillDTO deactive(TenantWaterBillDTO data) 
        {
            //try
            //{
            //    var result = _context.Tenant_Monthly_WaterBillsDMO.Where(t => t.TENANTMWBILL_Id == data.TENANTMWBILL_Id).FirstOrDefault();

            //    if (result.TENANTMWBILL_Id > 0)
            //    {
            //        var result1 = _context.Tenant_Monthly_WaterBills_RoomWiseDMO.Where(t => t.TENANTMWBILL_Id == data.TENANTMWBILL_Id).ToList();

            //        if (result1.Count > 0)
            //        {
            //            foreach(var i in result1)
            //            {
            //                var result2 = _context.Tenant_Monthly_WaterBills_RoomWiseDMO.Where(t => t.TENANTMWBILLRW_Id == i.TENANTMWBILLRW_Id).FirstOrDefault();

            //                if (result2.TENANTMWBILLRW_Id > 0)
            //                {
            //                    _context.Remove(result2);

            //                }

            //            }
            //            int returnval1 = _context.SaveChanges();
            //        }


            //    }

            //    _context.Remove(result);
            //    int returnval = _context.SaveChanges();
            //    if (returnval > 0)
            //    {
            //        data.returnval = "true";
            //    }
            //    else
            //    {
            //        data.returnval = "false";
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}

            try
            {
                var result = _context.Tenant_Monthly_WaterBillsDMO.Where(t => t.TENANTMWBILL_Id == data.TENANTMWBILL_Id).FirstOrDefault();

                if (result.TENANTMWBILL_Id > 0)
                {
                    var result1 = _context.Tenant_Monthly_WaterBills_RoomWiseDMO.Where(t => t.TENANTMWBILL_Id == data.TENANTMWBILL_Id).ToList();

                    if (result1.Count > 0)
                    {
                        foreach (var i in result1)
                        {

                            _context.Remove(i);



                        }
                        int returnval1 = _context.SaveChanges();
                    }


                }

                _context.Remove(result);
                int returnval = _context.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
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
