using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
     public class FinancialYearWisePFVPFInterestImplement : Interfaces.FinancialYearWisePFVPFInterestInterface
        {
            public HRMSContext _HRMSContext;
            public DomainModelMsSqlServerContext _db;
            public FinancialYearWisePFVPFInterestImplement(HRMSContext HRMSContext, DomainModelMsSqlServerContext db)
            {
                _HRMSContext = HRMSContext;
                _db = db;
            }
            public FinancialYearWisePFVPFInterestDTO getalldetails(FinancialYearWisePFVPFInterestDTO data)
            {
                try
                {
                    data.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.Distinct().ToArray();

                    data.get_store = (from a in _HRMSContext.FinancialYearWisePFVPFInterestDMO
                                      from b in _HRMSContext.IVRM_Master_FinancialYear
                                      where(a.IMFY_Id==b.IMFY_Id)
                                      select new FinancialYearWisePFVPFInterestDTO
                                      {
                                          HRMPFVPFINT_Id = a.HRMPFVPFINT_Id,
                                          HRMPFVPFINT_PFInterestRate=a.HRMPFVPFINT_PFInterestRate,
                                          HRMPFVPFINT_VPFInterestRate = a.HRMPFVPFINT_VPFInterestRate,
                                          IMFY_FinancialYear = b.IMFY_FinancialYear,
                                          HRMPFVPFINT_ActiveFlg = a.HRMPFVPFINT_ActiveFlg
                                      }).Distinct().ToArray();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return data;

            }

        public FinancialYearWisePFVPFInterestDTO Savedetails(FinancialYearWisePFVPFInterestDTO data)
        {
            try
       
            {
                if (data.HRMPFVPFINT_Id != 0)
                {
                    data.returnduplicatestatus = "Update";
                    data.returnval = false;
                    var res = _HRMSContext.FinancialYearWisePFVPFInterestDMO.Where(t => t.HRMPFVPFINT_Id == data.HRMPFVPFINT_Id && t.IMFY_Id == data.IMFY_Id && t.HRMPFVPFINT_PFInterestRate==data.HRMPFVPFINT_PFInterestRate && t.HRMPFVPFINT_VPFInterestRate == data.HRMPFVPFINT_VPFInterestRate).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var update = _HRMSContext.FinancialYearWisePFVPFInterestDMO.Where(t => t.HRMPFVPFINT_Id == data.HRMPFVPFINT_Id).FirstOrDefault();
                        update.IMFY_Id = data.IMFY_Id;
                        update.HRMPFVPFINT_VPFInterestRate = data.HRMPFVPFINT_VPFInterestRate;
                        update.HRMPFVPFINT_PFInterestRate = data.HRMPFVPFINT_PFInterestRate;
                        update.HRMPFVPFINT_ActiveFlg = true;
                        update.HRMPFVPFINT_UpdatedDate = DateTime.Now;
                        _HRMSContext.Update(update);
                        var updated = _HRMSContext.SaveChanges();
                        if (updated > 0)
                        {
                            
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                   
                }
                else
                {
                    data.returnduplicatestatus = "Add";
                    data.returnval = true;

                    FinancialYearWisePFVPFInterestDMO tax = new FinancialYearWisePFVPFInterestDMO();
                    tax.MI_Id = data.MI_Id;
                    tax.IMFY_Id = data.IMFY_Id;
                    tax.HRMPFVPFINT_VPFInterestRate = data.HRMPFVPFINT_VPFInterestRate;
                    tax.HRMPFVPFINT_PFInterestRate = data.HRMPFVPFINT_PFInterestRate;
                    tax.HRMPFVPFINT_ActiveFlg = true;
                    tax.HRMPFVPFINT_CreatedDate = DateTime.Now;
                    tax.HRMPFVPFINT_UpdatedDate = DateTime.Now;
                    _HRMSContext.Add(tax);
                }

                       var contactExists = _HRMSContext.SaveChanges();

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
        public FinancialYearWisePFVPFInterestDTO edit(FinancialYearWisePFVPFInterestDTO data)
        {
            try
            {
                data.GetMaterTypeEditData = _HRMSContext.FinancialYearWisePFVPFInterestDMO.Where(a => a.HRMPFVPFINT_Id == data.HRMPFVPFINT_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //public FinancialYearWisePFVPFInterestDTO deactive(FinancialYearWisePFVPFInterestDTO data)
        //{
        //    try
        //    {
        //        data.returnval = false;

        //        TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //        DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

        //        var Result = _HRMSContext.FinancialYearWisePFVPFInterestDMO.Single(a => a.HRMPFVPFINT_Id == data.HRMPFVPFINT_Id);
        //        Result.HRMPFVPFINT_ActiveFlg = Result.HRMPFVPFINT_ActiveFlg == true ? false : true;

        //        _HRMSContext.Update(Result);
        //        var i = _HRMSContext.SaveChanges();
        //        if (i > 0)
        //        {
        //            data.returnval = true;
        //        }
        //        data.GetMasterOptionLoadData = _HRMSContext.FinancialYearWisePFVPFInterestDMO.Where(a => data.MI_Id.Equals(a.MI_Id)).ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}

        public FinancialYearWisePFVPFInterestDTO deactive(FinancialYearWisePFVPFInterestDTO data)
        {
            try
            {

                var result = _HRMSContext.FinancialYearWisePFVPFInterestDMO.Single(t => t.HRMPFVPFINT_Id == data.HRMPFVPFINT_Id);
                if (result.HRMPFVPFINT_ActiveFlg == true)
                {
                    result.HRMPFVPFINT_ActiveFlg = false;
                }
                else if (result.HRMPFVPFINT_ActiveFlg == false)
                {
                    result.HRMPFVPFINT_ActiveFlg = true;
                }
                result.HRMPFVPFINT_UpdatedDate = DateTime.Now;
                _HRMSContext.Update(result);
                int returnval = _HRMSContext.SaveChanges();
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

    }

}

