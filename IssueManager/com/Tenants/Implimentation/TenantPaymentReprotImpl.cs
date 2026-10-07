using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
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
    public class TenantPaymentReprotImpl : Interface.TenantPaymentReprotInterface
    {
        public IssueManagerContext _context;


        public TenantPaymentReprotImpl(IssueManagerContext isc)
        {





            _context = isc;

        }

        public async Task<TenantAgreementDTO> getdetails(TenantAgreementDTO data)
        {
            try
            {




                data.getinstitution = _context.Institution.Where(t => t.MI_ActiveFlag==1).ToArray();
                data.building = _context.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.getfloordetails = _context.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id).ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<TenantAgreementDTO> get_report(TenantAgreementDTO data)
        {
            try
            {
                string buildingid = "0";
                string roomid = "0";


                //for (int i=0;i<data.buldid.Length;i++)
                //{
                //    buildingid = buildingid + data.buldid[i].ToString();
                //}
                //for (int i = 0; i < data.roomid.Length; i++)
                //{
                //    roomid = roomid + data.roomid[i].ToString();
                //}

                for (int i = 0; i < data.buldid.Length; i++)
                {
                    buildingid = buildingid + ',' + data.buldid[i];

                }

                for (int i = 0; i < data.roomid.Length; i++)
                {
                    roomid = roomid +',' + data.roomid[i];
                }

                var from_date = data.TENANTAGR_AgrFromDate.Date.ToString("yyyy-MM-dd");
                var to_date = data.TENANTAGR_AgrToDate.Date.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantMonthlyPayment";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@BUILDINGID", SqlDbType.VarChar) { Value = buildingid });
                    cmd.Parameters.Add(new SqlParameter("@ROOMID", SqlDbType.VarChar) { Value = roomid });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = to_date });
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
                            data.getloaddata = retObject.Distinct().ToArray();
                        }
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

        public TenantAgreementDTO institutionchange(TenantAgreementDTO data)
        {
            data.building = _context.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id).ToArray();

            //data.getfloordetails = _context.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id && data.buldid.Contains(t.HRMB_Id)).ToArray();


            return data;
        }
        public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
            data.rooms = _context.HR_Master_Room.Where(t =>  data.buldid.Contains(t.HRMB_Id)).ToArray();

            data.getfloordetails = _context.HR_Master_Floor.Where(t =>  data.buldid.Contains(t.HRMB_Id)).ToArray();


            return data;
        }

        public TenantAgreementDTO Floorchange(TenantAgreementDTO data)
        {
            data.rooms = _context.HR_Master_Room.Where(t =>  data.buldid.Contains(t.HRMB_Id) && data.floorid.Contains(t.HRMF_Id)).ToArray();
            return data;
        }

        public TenantAgreementDTO TenantDetails(TenantAgreementDTO data)
        {
            data.TenantDetails = _context.TenantAgreement.Where(t => data.buldid.Contains(t.HRMB_Id) && data.roomid.Contains(t.HRMR_Id)).ToArray();

            return data;
        }
        public TenantAgreementDTO TenantConsolidatedReport(TenantAgreementDTO data)
        {
            try
            {

                var tenantids = "0";
                //   List<TenantAgreementDTO> tenantid = new List<TenantAgreementDTO>();

                if (data.TENANTAGR_Id == 0)
                {
                    if (data.flag == "active")
                    {
                       var tenantid = _context.TenantAgreement.Where(t => t.MI_Id == data.MI_Id && data.buldid.Contains(t.HRMB_Id) && data.roomid.Contains(t.HRMR_Id) && (t.TENANTAGR_LeftFlg==false || t.TENANTAGR_LeftFlg==null)).Distinct().ToList();

                        if (tenantid.Count > 0)
                        {
                            for (var i = 0; i < tenantid.Count; i++)
                            {
                                tenantids = tenantids + "," + tenantid[i].TENANTAGR_Id;
                            }
                        }
                    }
                    else
                    {
                       var tenantid = _context.TenantAgreement.Where(t => t.MI_Id == data.MI_Id && data.buldid.Contains(t.HRMB_Id) && data.roomid.Contains(t.HRMR_Id) && t.TENANTAGR_LeftFlg == true).Distinct().ToList();
                        if (tenantid.Count > 0)
                        {
                            for (var i = 0; i < tenantid.Count; i++)
                            {
                                tenantids = tenantids + "," + tenantid[i].TENANTAGR_Id;
                            }
                        }
                    }

                }
                else
                {
                    if (data.flag == "active")
                    {
                       var tenantid = _context.TenantAgreement.Where(t => t.MI_Id == data.MI_Id && data.buldid.Contains(t.HRMB_Id) && data.roomid.Contains(t.HRMR_Id) && t.TENANTAGR_Id == data.TENANTAGR_Id && (t.TENANTAGR_LeftFlg == false || t.TENANTAGR_LeftFlg == null)).ToList();
                        if (tenantid.Count > 0)
                        {
                            for (var i = 0; i < tenantid.Count; i++)
                            {
                                tenantids = tenantids + "," + tenantid[i].TENANTAGR_Id;
                            }
                        }
                    }
                    else
                    {
                     var   tenantid = _context.TenantAgreement.Where(t => t.MI_Id == data.MI_Id && data.buldid.Contains(t.HRMB_Id) && data.roomid.Contains(t.HRMR_Id) && t.TENANTAGR_Id == data.TENANTAGR_Id && t.TENANTAGR_LeftFlg == true).ToList();
                        if (tenantid.Count > 0)
                        {
                            for (var i = 0; i < tenantid.Count; i++)
                            {
                                tenantids = tenantids + "," + tenantid[i].TENANTAGR_Id;
                            }
                        }

                    }

                }

             

                



                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantConsolidatedReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                   
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Ids", SqlDbType.VarChar) { Value = tenantids });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

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
                        data.TenantDetails = retObject.ToArray();
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
        public TenantAgreementDTO TenantMonthlyPaymentDetails(TenantAgreementDTO data)
        {
            try
            {


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TenantConsolidatedReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@TENANTAGR_Ids", SqlDbType.VarChar) { Value = data.TENANTAGR_Id });                   
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.BigInt) { Value = data.type });
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
