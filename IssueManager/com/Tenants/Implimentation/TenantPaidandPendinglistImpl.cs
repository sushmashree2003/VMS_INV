using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
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
    public class TenantPaidandPendinglistImpl : Interface.TenantPaidandPendinglistInterface
    {
        public IssueManagerContext _IssueManagerContext;
        public DomainModelMsSqlServerContext _Context;
        public TenantPaidandPendinglistImpl(IssueManagerContext IssueManagerContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _IssueManagerContext = IssueManagerContext;
            _Context = MsSqlServerContext;

        }

        public TenantPaidandPendinglistDTO getBasicData(TenantPaidandPendinglistDTO dto)
        {
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();
            List<Month> Monthlist = new List<Month>();
           
            try
            {
                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();

                
                leaveyear = _Context.HR_Master_LeaveYearDMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToList();
                dto.leaveyeardropdown = leaveyear.OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
        public TenantPaidandPendinglistDTO get_report(TenantPaidandPendinglistDTO data)
        {
            try
            {
             
                // var TENANTAGR_AgrFromDate = "";
                //var TENANTAGR_AgrToDate = "";
                //if (data.TENANTAGR_AgrFromDate != null)
                //{
                //    TENANTAGR_AgrFromDate = data.TENANTAGR_AgrFromDate.ToString("yyyy-MM-dd");
                //}
                //if (data.TENANTAGR_AgrToDate != null)
                //{
                //    TENANTAGR_AgrToDate = data.TENANTAGR_AgrToDate.ToString("yyyy-MM-dd");
                //}


                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Tenant_Payment_Details_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = data.MI_Id });
                    //cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = TENANTAGR_AgrFromDate });
                    //cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = TENANTAGR_AgrToDate });
                    cmd.Parameters.Add(new SqlParameter("@year_id", SqlDbType.VarChar) { Value = data.HRMLY_LeaveYear });
                    cmd.Parameters.Add(new SqlParameter("@month_id", SqlDbType.VarChar) { Value = data.monthid });
                    //cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = data.flag });
                    //cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = data.type });

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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]); 
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }

                        }
                        data.tenantpendingreport = retObject.Distinct().ToArray();
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

        public TenantPaidandPendinglistDTO sendmail(TenantPaidandPendinglistDTO data)
        {
            try
            {
                foreach (var i in data.tenantpendingpaymentlistarr)
                {
                    var tenantdetails = (from a in _Context.TenantAgreement
                                         from b in _Context.HR_Master_Building
                                         from c in _Context.HR_Master_Room
                                         where (a.MI_Id == b.MI_Id && a.HRMB_Id == b.HRMB_Id && b.MI_Id == c.MI_Id && b.HRMB_Id == c.HRMB_Id && a.HRMR_Id == c.HRMR_Id &&
                                         a.TENANTAGR_ActiveFlg == true && a.TENANTAGR_LeftFlg == false && a.TENANTAGR_Id == i.TENANTAGR_Id)
                                         select new TenantPaidandPendinglistDTO
                                         {
                                             Tenant_Name = a.TENANTAGR_TenantName,
                                             TENANTAGR_EmailId = a.TENANTAGR_EmailId,
                                             TENANTAGR_ContactNo = a.TENANTAGR_ContactNo,
                                             TENANTAGR_Address = a.TENANTAGR_Address,
                                             Buildingname = b.HRMB_BuildingName,
                                             FlatNo = c.HRMR_RoomName

                                         }).ToList();

                    string emailccdetails = "";
                    string emailbccdetails = "";

                }



                //    string emailccdetails = "";
                //    string emailbccdetails = "";





            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
               // data.returnmsg = ex.Message;
            }



            return data;

        }


    }
}

