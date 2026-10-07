using DataAccessMsSqlServerProvider.com.vapstech.FMS;
using DomainModel.Model.com.vapstech.FMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Services
{
    public class FMSApprovalRequestPageImp : Interface.FMSApprovalRequestPageInterface
    {
        public FMSContext _context;
        public FMSApprovalRequestPageImp(FMSContext _cont)
        {
            _context = _cont;
        }





        //        //consolidated_report
        public FMSApprovalRequestPageDTO loaddata(FMSApprovalRequestPageDTO data)
        {
            try
            {
              
                data.filename = _context.FMS_Master_PhysicalFileNameDMO.Where(R => R.FMSMPFN_ActiveFlg == true ).Distinct().ToArray();

                //data.filename = (from a in _context.FMS_Master_PhysicalFileNameDMO
                //                 from b in _context.FMSApprovalRequestPageDMO
                //                 where (a.FMSMPFN_ActiveFlg == true)
                //                 select new FMSApprovalRequestPageDTO
                //                 {
                //                     FMSMPFN_PhysicalFileName = data.FMSMPFN_PhysicalFileName,
                //                     FMSFLREQ_Id = data.FMSFLREQ_Id,
                //                     FMSMPFN_Id = data.FMSMPFN_Id
                //                 }).Distinct().ToArray();


                data.registration = _context.FMSApprovalRequestPageDMO.Select(R => R.FMSFLREQ_Id).Count();
                data.get_tax = (from a in _context.FMSApprovalRequestPageDMO
                                from b in _context.FMS_Master_PhysicalFileNameDMO
                                where (a.FMSMPFN_Id == b.FMSMPFN_Id )
                                select new FMSApprovalRequestPageDTO
                                {
                                    filenames = b.FMSMPFN_PhysicalFileName,
                                    requireddays = a.FMSFLREQ_RequiredDays,
                                    FMSFLREQ_Id = a.FMSFLREQ_Id,
                                    FMSFLREQ_ActiveFlg = a.FMSFLREQ_ActiveFlg,
                                    FMSFLREQ_ApprovedFlg = a.FMSFLREQ_ApprovedFlg

                                }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FMSApprovalRequestPageDTO savedetails(FMSApprovalRequestPageDTO data)
        {
            try
            {
                string FromDate = data.FMSFLREQ_RequestedFromDate.ToString("yyyy-MM-dd");
                string ToDate = data.FMSFLREQ_RequestedToDate.ToString("yyyy-MM-dd");
                var template = _context.MasterEmployee.Where(e => e.MI_Id == data.MI_Id).ToList();


                var Mailcontent = template.FirstOrDefault().HRME_Id;

                if (data.FMSFLREQ_Id != 0)
                {
                   
                    var result = _context.FMSApprovalRequestPageDMO.Single(t => t.FMSFLREQ_Id == data.FMSFLREQ_Id);
                    
                    result.MI_Id = data.MI_Id;
                    result.FMSMPFN_Id = data.FMSMPFN_Id;
                    result.HRME_Id = Mailcontent;
                    result.FMSFLREQ_RequestedDateTime = DateTime.Now;
                    result.FMSFLREQ_RequiredDays = data.FMSFLREQ_RequiredDays;  
                    result.FMSFLREQ_CreatedDate = DateTime.Now;  
                    result.FMSFLREQ_UpdatedDate = DateTime.Now;
                    result.FMSFLREQ_ActiveFlg = true;
                    result.FMSFLREQ_CreatedBy = data.UserId;
                    result.FMSFLREQ_UpdatedBy = Mailcontent;
                    result.FMSFLREQ_RequestedFromDate = data.FMSFLREQ_RequestedFromDate;
                    result.FMSFLREQ_RequestedToDate = data.FMSFLREQ_RequestedToDate;




                    _context.Update(result);

                    var contactExists = _context.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                else
                {

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FMSRequestdata";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FMSFLREQ_Id", SqlDbType.BigInt) { Value = data.@FMSFLREQ_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = ToDate });
                        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.BigInt, Int32.MaxValue) { Direction = ParameterDirection.Output });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        data.duplicate = cmd.Parameters["@Result"].Value.ToString();
                    }

                    if (data.duplicate == "0")
                    {

                        var res = _context.FMSApprovalRequestPageDMO.Where(t => (t.FMSFLREQ_Id == data.FMSFLREQ_Id)).ToList();

                    //if (res.Count > 0)
                    //{
                    //    data.returnduplicatestatus = "Duplicate";
                    //}
                    //else
                    //{
                        FMSApprovalRequestPageDMO tax = new FMSApprovalRequestPageDMO();
                        
                        tax.MI_Id = data.MI_Id;
                        tax.FMSMPFN_Id = data.FMSMPFN_Id;
                        tax.HRME_Id = Mailcontent;
                        tax.FMSFLREQ_RequestedDateTime = DateTime.Now;
                        tax.FMSFLREQ_RequiredDays = data.FMSFLREQ_RequiredDays;
                        tax.FMSFLREQ_CreatedDate = DateTime.Now;
                        tax.FMSFLREQ_UpdatedDate = DateTime.Now;
                        tax.FMSFLREQ_ActiveFlg = true;
                        tax.FMSFLREQ_CreatedBy = data.UserId;
                        tax.FMSFLREQ_UpdatedBy = Mailcontent;
                        tax.FMSFLREQ_RequestedFromDate = data.FMSFLREQ_RequestedFromDate;
                        tax.FMSFLREQ_RequestedToDate = data.FMSFLREQ_RequestedToDate;

                        _context.Add(tax);

                        var contactExists = _context.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;

                        }
                        else
                        {
                            data.returnval = false;
                        }
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

        public FMSApprovalRequestPageDTO editdetails(FMSApprovalRequestPageDTO dto)
        {
            try
            {
               
                dto.editdata = (from a in _context.FMSApprovalRequestPageDMO
                                from b in _context.FMS_Master_PhysicalFileNameDMO
                                where (a.FMSFLREQ_Id == dto.FMSFLREQ_Id  &&  b.FMSMPFN_PhysicalFileName == dto.FMSMPFN_PhysicalFileName)
                                select new FMSApprovalRequestPageDTO
                                {
                                    FMSMPFN_PhysicalFileName = b.FMSMPFN_PhysicalFileName,
                                    FMSMPFN_Id = b.FMSMPFN_Id,
                                    FMSFLREQ_RequiredDays = a.FMSFLREQ_RequiredDays,
                                    FMSFLREQ_Id = a.FMSFLREQ_Id,
                                    FMSFLREQ_RequestedFromDate = a.FMSFLREQ_RequestedFromDate,
                                    FMSFLREQ_RequestedToDate = a.FMSFLREQ_RequestedToDate,
                                }
                                ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public FMSApprovalRequestPageDTO deactive(FMSApprovalRequestPageDTO data)
        {
            try
            {
                var result = _context.FMSApprovalRequestPageDMO.Single(t => t.FMSFLREQ_Id == data.FMSFLREQ_Id);

                if (result.FMSFLREQ_ActiveFlg == true)
                {
                    result.FMSFLREQ_ActiveFlg = false;
                }
                else if (result.FMSFLREQ_ActiveFlg == false)
                {
                    result.FMSFLREQ_ActiveFlg = true;
                }
                result.FMSFLREQ_UpdatedDate = DateTime.Now;
                _context.Update(result);
                int returnval = _context.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        

        public FMSApprovalRequestPageDTO viewuploadflies(FMSApprovalRequestPageDTO data)
        {
            try
            {
                    data.hrmetable = _context.MasterEmployee.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();
                    data.iddata = data.FMSFLREQ_Id;

                    var result = _context.FMSApprovalRequestPageDMO.Single(t => t.FMSFLREQ_Id == data.FMSFLREQ_Id);
                    if(result.FMSFLREQ_ApprovedFlg == true)
                    {
                       data.FMSFLREQ_ApprovedFlg = result.FMSFLREQ_ApprovedFlg;
                    }
                    else
                    {
                    data.FMSFLREQ_ApprovedFlg = false;
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FMSApprovalRequestPageDTO Updatedetails(FMSApprovalRequestPageDTO data)
        {
            try
            {
                var submit = _context.FMSApprovalRequestPageDMO.Single(a => a.FMSFLREQ_Id == data.FMSFLREQ_Id);

                submit.FMSFLREQ_HandedOverBy = data.FMSFLREQ_HandedOverBy;
                submit.FMSFLREQ_ReturnedFlg = true;
                submit.FMSFLREQ_ReturnedDateTime = DateTime.Now;
                submit.FMSFLREQ_ReturnRemarks = data.FMSFLREQ_ReturnRemarks;

                _context.Update(submit);

                var contactExists = _context.SaveChanges();
                if (contactExists > 0)
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

    }
}
