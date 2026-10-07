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
    public class TenantPaymentOrFlatwiseReportImpl : Interface.TenantPaymentOrFlatwiseReportInterface
    {
        public IssueManagerContext _context;


        public TenantPaymentOrFlatwiseReportImpl(IssueManagerContext isc)
        {

            _context = isc;

        }

        public async Task<TenantAgreementDTO> getdetails(TenantAgreementDTO data)
        {
            try
            {

                data.getyear = _context.HR_Master_LeaveYearDMO.Distinct().Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.getmonth = _context.IVRM_Month_DMO.Distinct().ToArray();
                //data.getinstitution = _context.Institution.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.getinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1).ToArray();

                //data.building = _context.HR_Master_Building.Where(t => t.MI_Id == data.MI_Id).ToArray();
                //data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id).ToArray();
                //data.getfloordetails = _context.HR_Master_Floor.Where(t => t.MI_Id == data.MI_Id).ToArray();

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
                string flooridss = "0";
                string companyid = "0";
                string monthid = "0";

                if (data.maintype == "Flatwise")
                {

                    for (int i = 0; i < data.company_array.Length; i++)
                    {
                        companyid = companyid + ',' + data.company_array[i].ToString();
                    }

                    for (int i = 0; i < data.buldid.Length; i++)
                    {
                        buildingid = buildingid + ',' + data.buldid[i].ToString();
                    }
                    for (int i = 0; i < data.roomid.Length; i++)
                    {
                        roomid = roomid + ',' + data.roomid[i].ToString();
                    }
                    for (int i = 0; i < data.monthid.Length; i++)
                    {
                        monthid = monthid + ',' + data.monthid[i].ToString();
                    }

                    var TENANTAGR_AgrFromDate = "";
                    var TENANTAGR_AgrToDate = "";
                    if (data.TENANTAGR_AgrFromDate != null)
                    {
                        TENANTAGR_AgrFromDate = data.TENANTAGR_AgrFromDate.ToString("yyyy-MM-dd");
                    }
                    if (data.TENANTAGR_AgrToDate != null)
                    {
                        TENANTAGR_AgrToDate = data.TENANTAGR_AgrToDate.ToString("yyyy-MM-dd");
                    }


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Tenant_Paymentvacantedetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.VarChar) { Value = companyid });
                        cmd.Parameters.Add(new SqlParameter("@HRMB_ID", SqlDbType.VarChar) { Value = buildingid });
                        cmd.Parameters.Add(new SqlParameter("@HRMR_ID", SqlDbType.VarChar) { Value = roomid });
                        cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.VarChar) { Value = data.HRMLY_LeaveYear });
                        cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.VarChar) { Value = monthid });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = data.type });
                        cmd.Parameters.Add(new SqlParameter("@MAINTYPE", SqlDbType.VarChar) { Value = data.maintype });

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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]); // use null instead of {}
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }

                            }
                            data.getloaddata = retObject.Distinct().ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                else if(data.maintype == "NotOccupied")
                {
                    for (int i = 0; i < data.company_array.Length; i++)
                    {
                        companyid = companyid + ',' + data.company_array[i].ToString();
                    }

                    for (int i = 0; i < data.buldid.Length; i++)
                    {
                        buildingid = buildingid + ',' + data.buldid[i].ToString();
                    }
                    for (int i = 0; i < data.floorid.Length; i++)
                    {
                        flooridss = flooridss + ',' + data.floorid[i].ToString();
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Tenant_Vacant_RoomDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = companyid });
                        cmd.Parameters.Add(new SqlParameter("@HRMB_Id", SqlDbType.VarChar) { Value = buildingid });
                        cmd.Parameters.Add(new SqlParameter("@HRMF_Id", SqlDbType.VarChar) { Value = flooridss });
                        //cmd.Parameters.Add(new SqlParameter("@MAINTYPE", SqlDbType.VarChar) { Value = data.maintype });

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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]); // use null instead of {}
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }

                            }
                            data.getloaddata = retObject.Distinct().ToArray();
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public TenantAgreementDTO companychange(TenantAgreementDTO data)
        {
            data.building = _context.HR_Master_Building.Where(t => data.company_array.Contains(t.MI_Id)).ToArray();

            data.getfloordetails = _context.HR_Master_Floor.Where(t => data.company_array.Contains(t.MI_Id)).ToArray();

            data.rooms = _context.HR_Master_Room.Where(t => data.company_array.Contains(t.MI_Id)).ToArray();

            return data;

        }
        public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
            data.getfloordetails = _context.HR_Master_Floor.Where(t => data.company_array.Contains(t.MI_Id) && data.buldid.Contains(t.HRMB_Id)).ToArray();

            data.rooms = _context.HR_Master_Room.Where(t => data.company_array.Contains(t.MI_Id) && data.buldid.Contains(t.HRMB_Id)).ToArray();

            return data;

        }

        public TenantAgreementDTO floorchange(TenantAgreementDTO data)
        {
            data.rooms = _context.HR_Master_Room.Where(t => data.company_array.Contains(t.MI_Id) && data.buldid.Contains(t.HRMB_Id) && data.floorid.Contains(t.HRMF_Id)).ToArray();

            
            return data;
        }
    }
}
