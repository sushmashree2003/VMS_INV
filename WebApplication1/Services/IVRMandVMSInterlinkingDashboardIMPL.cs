using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class IVRMandVMSInterlinkingDashboardIMPL : Interfaces.IVRMandVMSInterlinkingDashboardInterface
    {

        public IssueManagerContext _Context;
        public DomainModelMsSqlServerContext _Db;
        public IVRMandVMSInterlinkingDashboardIMPL(IssueManagerContext Context, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _Context = Context;
            _Db = MsSqlServerContext;
        }

        public IVRMandVMSInterlinkingDashboardDTO getBasicData(IVRMandVMSInterlinkingDashboardDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public IVRMandVMSInterlinkingDashboardDTO GetAllDropdownAndDatatableDetails(IVRMandVMSInterlinkingDashboardDTO dto)
        {
            try
            {
                dto.clintlist = _Context.clientTable.Where(t => t.MI_Id==dto.MI_Id && t.ISMMCLT_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public async Task<IVRMandVMSInterlinkingDashboardDTO> getclientdetails(IVRMandVMSInterlinkingDashboardDTO dto)
        {

            try
            {
               

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IE_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMMCLT_Id
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
                        dto.clientIEDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ClientPaymentDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMMCLT_Id
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
                        dto.clientpaymentdetails = retObject.ToArray();
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
        public IVRMandVMSInterlinkingDashboardDTO getalldetailsQForm(IVRMandVMSInterlinkingDashboardDTO data)
        {
            //data.employeeDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();

            return data;
        }
        public IVRMandVMSInterlinkingDashboardDTO getEmployeedetailsQForm(IVRMandVMSInterlinkingDashboardDTO data)
        {


            return data;
        }
        public IVRMandVMSInterlinkingDashboardDTO SearchUdetails(IVRMandVMSInterlinkingDashboardDTO data)
        {
            try
            {
                // data.companydetails = _HRMSContext.Institution.Where(t => t.MI_Id == data.MI_Id && t.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRMandVMSInterlinkingDashboardDTO SearchFReport(IVRMandVMSInterlinkingDashboardDTO data)
        {
            //try
            //{
            //    data.employeeDetailssingle = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == data.HRME_Id).ToArray();

            //    data.leavedetails = (from a in _HRMSContext.HR_Master_Leave
            //                         from b in _HRMSContext.HR_MasterLeaveYear
            //                         from c in _HRMSContext.HR_Emp_Leave_StatusDMO
            //                         from d in _HRMSContext.MasterEmployee
            //                         where (a.MI_Id == d.MI_Id && d.HRME_Id == data.HRME_Id && b.MI_Id == a.MI_Id && b.HRMLY_ActiveFlag == true && c.HRME_Id == d.HRME_Id && c.HRMLY_Id == b.HRMLY_Id)
            //                         select new IVRMandVMSInterlinkingDashboardDTO
            //                         {
            //                             HRML_LeaveName = a.HRML_LeaveName,
            //                             HRMLY_LeaveYear = b.HRMLY_LeaveYear,
            //                             HRELS_OBLeaves = c.HRELS_OBLeaves,
            //                             HRELS_CreditedLeaves = c.HRELS_CreditedLeaves
            //                         }).ToArray();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }
    }
}