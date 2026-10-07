using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeInstallmentImpl : interfaces.FeeInstallmentInterface
    {
        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeInstallmentImpl> _logger;
        public FeeInstallmentImpl(FeeGroupContext frgContext, ILogger<FeeInstallmentImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }
        public FeeInstallmentDTO getdetails(FeeInstallmentDTO FGRDT)
        {            
            try
            {
                List<FeeInstallmentDMO> feegrp = new List<FeeInstallmentDMO>();
                feegrp = _FeeGroupContext.feeMI.Where(t => t.MI_Id == FGRDT.MI_Id).OrderBy(t => t.FMI_Name).ToList();
                FGRDT.InstallmentData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public FeeInstallmentDTO SaveGroupData(FeeInstallmentDTO FGpage)
        {
            FeeInstallmentDMO feepge =new FeeInstallmentDMO();
            FeeInstallmentyeralyDTO te = new FeeInstallmentyeralyDTO();
            
            string retval = "";
            try
            {
                if (FGpage.FMI_Id > 0)
                {
                    var duplicateKeys = FGpage.fydto.GroupBy(x => x.FTI_Name)
                           .Where(group => group.Count() > 1)
                           .Select(group => group.Key).ToList();
                    if (duplicateKeys.Count > 0)
                    {
                        FGpage.msg="Installment Name Contains Some Duplicate Values";
                        return FGpage;
                    }
                    else
                    {
                        var res = _FeeGroupContext.feeMI.Where(t => t.FMI_Id != FGpage.FMI_Id && t.FMI_Name == FGpage.FMI_Name && t.MI_Id == FGpage.MI_Id);
                        if (res.Count() > 0)
                        {
                            retval = "Duplicate";
                            FGpage.returnduplicatestatus = retval;
                        }
                        else
                        {
                            var result = _FeeGroupContext.feeMI.Single(t => t.FMI_Id == FGpage.FMI_Id);
                            result.MI_Id = FGpage.MI_Id;
                            result.FMI_Name = FGpage.FMI_Name;
                            result.FMI_No_Of_Installments = FGpage.FMI_No_Of_Installments;
                            result.FMI_Installment_Type = FGpage.FMI_Installment_Type;
                            result.FMI_ActiceFlag = FGpage.FMI_ActiceFlag;
                            result.UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Update(result);

                            for (int i = 0; i < FGpage.fydto.Count; i++)
                            {                              
                                if (FGpage.fydto[i].FTI_Id == 0)
                                {
                                    FeeInstallmentsyearlyDMO feepgeY = Mapper.Map<FeeInstallmentsyearlyDMO>(FGpage.fydto[i]);
                                    feepgeY.FMI_Id = FGpage.FMI_Id;
                                    feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;
                                    feepgeY.MI_ID = FGpage.MI_Id;
                                    feepgeY.CreatedDate = DateTime.Now;
                                    feepgeY.UpdatedDate = DateTime.Now;
                                    _FeeGroupContext.Add(feepgeY);
                                }
                                else {
                                    FeeInstallmentsyearlyDMO feepgeY = _FeeGroupContext.FeeInstallmentsyearlyDMO.Single(t => t.FTI_Id == FGpage.fydto[i].FTI_Id);
                                    feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;                                
                                    feepgeY.UpdatedDate = DateTime.Now;
                                    _FeeGroupContext.Update(feepgeY);
                                }                                
                            }
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                retval = "Update";
                                FGpage.returnduplicatestatus = retval;

                            }
                            else
                            {
                                retval = "NotUpdate";
                                FGpage.returnduplicatestatus = retval;
                            }                           
                        }
                    }
                }

                else
                {
                    var duplicateKeys = FGpage.fydto.GroupBy(x => x.FTI_Name).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
                    if (duplicateKeys.Count > 0)
                    {
                        FGpage.msg = "Installment Name Contains Some Duplicate Values";
                        return FGpage;
                    }
                    else
                    {
                        var result = _FeeGroupContext.feeMI.Where(t => t.FMI_Id == FGpage.FMI_Id || t.FMI_Name == FGpage.FMI_Name && t.MI_Id == FGpage.MI_Id);

                        if (FGpage.FMI_Installment_Type == "2")
                        {
                            feepge.FMI_No_Of_Installments = 1;
                        }
                        else if (FGpage.FMI_Installment_Type == "3")
                        {
                            feepge.FMI_No_Of_Installments = 12;
                        }
                        else if (FGpage.FMI_Installment_Type == "4")
                        {
                            feepge.FMI_No_Of_Installments = 4;
                        }
                        else if (FGpage.FMI_Installment_Type == "5")
                        {
                            feepge.FMI_No_Of_Installments = 1;
                        }
                        else if (FGpage.FMI_Installment_Type == "6")
                        {
                            feepge.FMI_No_Of_Installments = 2;
                        }
                        else if (FGpage.FMI_Installment_Type == "7")
                        {
                            feepge.FMI_No_Of_Installments = 24;
                        }

                        if (result.Count() > 0)
                        {
                            retval = "Duplicate";
                            FGpage.returnduplicatestatus = retval;
                            return FGpage;
                        }
                        else
                        {
                            
                            feepge.CreatedDate = DateTime.Now;
                            feepge.UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Add(feepge);
                            for (int i = 0; i < FGpage.fydto.Count; i++)
                            {
                                FeeInstallmentsyearlyDMO feepgeY = Mapper.Map<FeeInstallmentsyearlyDMO>(FGpage.fydto[i]);
                                feepgeY.FMI_Id = feepge.FMI_Id;
                                feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;
                                feepgeY.MI_ID = FGpage.MI_Id;
                                feepgeY.CreatedDate = DateTime.Now;
                                feepgeY.UpdatedDate = DateTime.Now;
                                _FeeGroupContext.Add(feepgeY);                                
                            }
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                retval = "Save";
                                FGpage.returnduplicatestatus = retval;

                            }
                            else
                            {
                                retval = "NotSave";
                                FGpage.returnduplicatestatus = retval;
                            }
                        }
                    }                    
                }               
                
                List<FeeInstallmentDMO> allpages = new List<FeeInstallmentDMO>();
                allpages = _FeeGroupContext.feeMI.OrderByDescending(t => t.CreatedDate).ToList();
                FGpage.InstallmentData = allpages.ToArray();
               
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }
        public FeeInstallmentDTO getpageedit(int id)
        {
            FeeInstallmentDTO page = new FeeInstallmentDTO();
            try
            {
                List<FeeInstallmentDMO> lorg = new List<FeeInstallmentDMO>();
                lorg = _FeeGroupContext.feeMI.AsNoTracking().Where(t => t.FMI_Id.Equals(id)).ToList();
                page.InstallmentData = lorg.ToArray();

                List<FeeInstallmentsyearlyDMO> lorg123 = new List<FeeInstallmentsyearlyDMO>();
                lorg123 = _FeeGroupContext.feeMIY.AsNoTracking().Where(t => t.FMI_Id.Equals(id)).ToList();
                page.InTData = lorg123.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeInstallmentDTO deactivate(FeeInstallmentDTO acd)
        {
            try
            {
                FeeInstallmentDMO feepge = Mapper.Map<FeeInstallmentDMO>(acd);
                if (feepge.FMI_Id > 0)
                {
                    var result = _FeeGroupContext.feeMI.Single(t => t.FMI_Id == feepge.FMI_Id);

                    var feestutrans = _FeeGroupContext.feeMIY.Where(t => t.FMI_Id == feepge.FMI_Id).ToList();
                    if (feestutrans.Count > 0)
                    {
                        acd.returnvalue = "used";
                        return acd;
                    }
                    else
                    {
                        result.UpdatedDate = DateTime.Now;

                        if (result.FMI_ActiceFlag == true)
                        {
                            result.FMI_ActiceFlag = false;
                            result.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            result.FMI_ActiceFlag = true;
                            result.UpdatedDate = DateTime.Now;
                        }
                        _FeeGroupContext.Update(result);
                        var flag = _FeeGroupContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }
                    }
                    List<FeeInstallmentDMO> allorganisation = new List<FeeInstallmentDMO>();
                    allorganisation = _FeeGroupContext.feeMI.ToList();
                    acd.InstallmentData = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
    }
}