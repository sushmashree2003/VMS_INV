using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.VMS.Legal;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Legal;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class LegalHistoryIMPL : Interfaces.LegalHistoryInterface
    {
        public IssueManagerContext _IssueContext;

        public DomainModelMsSqlServerContext _Context;

        ILogger<LegalHistoryIMPL> _acdimpl;

        private readonly ILogger _log;
        public LegalHistoryIMPL(IssueManagerContext IssueContext, DomainModelMsSqlServerContext Context)
        {
            _IssueContext = IssueContext;
            _Context = Context;
        }


        public LegalHistoryDTO saveDataHistory(LegalHistoryDTO dto)
        {

            try
            {
                {
                    if (dto.LegalHistory != null)
                    {
                        //DateTime TargetDate = DateTime.Parse(dto.newlstget1[i].TargetDate);
                        foreach (var item in dto.LegalHistory)
                        {
                            Legal_HistoryDMO enq = new Legal_HistoryDMO();                           
                            enq.LH_Id = item.LH_Id;
                            enq.LH_RegistrationNumber = item.LH_RegistrationNumber;
                            enq.LH_Judge = item.LH_Judge;
                            enq.LH_BusinessOnDate = item.LH_BusinessOnDate;
                            enq.LH_HearingDate = item.LH_HearingDate;
                            enq.LH_PurposeofHearing = item.LH_PurposeofHearing;
                            enq.LH_Business = item.LH_Business;
                            enq.LH_NextPurpose = item.LH_NextPurpose;
                            enq.LH_CNRNumber = item.LH_CNRNumber;
                            enq.LH_CaseNumber = item.LH_CaseNumber;
                            enq.LH_ActiveFlag = true;
                            enq.LH_CreatedDate = DateTime.Now;
                            enq.LH_UpdatedDate = DateTime.Now;
                            enq.LH_CreatedBy = dto.userId;
                            enq.LH_UpdatedBy = dto.userId;                           
                            _IssueContext.Add(enq);
                            _IssueContext.SaveChanges();                           
                            dto.returnvales = "Add";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }





        public async Task<LegalHistoryDTO> checkvalidation1(LegalHistoryDTO data)
        {
            try
            {
                for (int i = 0; i < data.newlstget1.Length; i++)
                {
                    try
                    {
                        Legal_HistoryDMO enq = new Legal_HistoryDMO();                        

                        //---------- RegistrationNumber---------//
                        try
                        {
                            if ((data.newlstget1[i].RegistrationNumber?.Trim() ?? "") != "")
                            {
                                if (data.newlstget1[i].RegistrationNumber.Trim().Length <= 100)
                                {
                                    enq.LH_RegistrationNumber = data.newlstget1[i].RegistrationNumber.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Registration Number is not Valid for " + data.newlstget1[i].RegistrationNumber;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Registration Number can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Registration Number Header Is Changed , Header Should Be Registration Number";
                            return data;
                        }

                        //---------- Judge---------//
                        try
                        {
                            if ((data.newlstget1[i].Judge?.Trim() ?? "") != "")
                            {
                                if (Regex.IsMatch(data.newlstget1[i].Judge.Trim(), @"^[a-zA-Z.\s]+$") && (data.newlstget1[i].Judge.Trim().Length <= 50))
                                {
                                    enq.LH_Judge = data.newlstget1[i].Judge.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Judge is not Valid for " + data.newlstget1[i].Judge;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Judge can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Judge Header Is Changed , Header Should Be Judge";
                            return data;
                        }

                        //---------- BusinessOnDate---------//

                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].BusinessOnDate.TrimEnd().TrimStart()) != null) && (Convert.ToString(data.newlstget1[i].BusinessOnDate.TrimEnd().TrimStart()) != ""))
                            {

                                if (data.newlstget1[i].BusinessOnDate.Length > 0)
                                {
                                    enq.LH_BusinessOnDate = Convert.ToDateTime(data.newlstget1[i].BusinessOnDate.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    data.stuStatus = "BusinessOnDate is not Valid for" + " " + data.newlstget1[i].BusinessOnDate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "BusinessOnDate can not be Empty for" + " " + data.newlstget1[i].BusinessOnDate;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "BusinessOnDate Header Is Changed , Header Should Be BusinessOnDate";
                            return data;
                        }

                        //---------- HearingDate---------//

                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].HearingDate.TrimEnd().TrimStart()) != null) && (Convert.ToString(data.newlstget1[i].HearingDate.TrimEnd().TrimStart()) != ""))
                            {

                                if (data.newlstget1[i].HearingDate.Length > 0)
                                {
                                    enq.LH_HearingDate = Convert.ToDateTime(data.newlstget1[i].HearingDate.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    data.stuStatus = "HearingDate is not Valid for" + " " + data.newlstget1[i].HearingDate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "HearingDate can not be Empty for" + " " + data.newlstget1[i].HearingDate;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "HearingDate Header Is Changed , Header Should Be HearingDate";
                            return data;
                        }
                       

                        //---------- PurposeofHearing---------//
                        try
                        {
                            if ((data.newlstget1[i].PurposeofHearing?.Trim() ?? "") != "")
                            {
                                if (Regex.IsMatch(data.newlstget1[i].PurposeofHearing.Trim(), @"^[a-zA-Z.\s]+$") && (data.newlstget1[i].PurposeofHearing.Trim().Length <= 50))
                                {
                                    enq.LH_PurposeofHearing = data.newlstget1[i].PurposeofHearing.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Purpose Of Hearing is not Valid for " + data.newlstget1[i].PurposeofHearing;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Purpose Of Hearing can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Purpose Of Hearing Header Is Changed , Header Should Be Purpose Of Hearing";
                            return data;
                        }

                        //---  Business --//
                        try
                        {
                            if ((data.newlstget1[i].Business?.Trim() ?? "") != "")
                            {
                                if (Regex.IsMatch(data.newlstget1[i].Business.Trim(), @"^[a-zA-Z.\s]+$") && (data.newlstget1[i].Business.Trim().Length <= 50))
                                {
                                    enq.LH_Business = data.newlstget1[i].Business.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Business is not Valid for " + data.newlstget1[i].Business;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Business can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Business Header Is Changed , Header Should Be Business";
                            return data;
                        }


                        //NextPurpose

                        try
                        {
                            if ((data.newlstget1[i].NextPurpose?.Trim() ?? "") != "")
                            {
                                if (Regex.IsMatch(data.newlstget1[i].NextPurpose.Trim(), @"^[a-zA-Z.\s]+$") && (data.newlstget1[i].NextPurpose.Trim().Length <= 50))
                                {
                                    enq.LH_NextPurpose = data.newlstget1[i].NextPurpose.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "NextPurpose is not Valid for " + data.newlstget1[i].NextPurpose;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "NextPurpose can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Next Purpose Header Is Changed , Header Should Be NextPurpose";
                            return data;
                        }

                        //NextHearingDate

                        //try
                        //{
                        //    if (!string.IsNullOrEmpty(data.newlstget1[i].NextHearingDate))
                        //    {
                        //        if (DateTime.TryParse(data.newlstget1[i].NextHearingDate.Trim(), out DateTime nextHearingDate))
                        //        {
                        //            enq.MOMMNT_TargetDate = nextHearingDate;
                        //        }
                        //        else
                        //        {
                        //            data.stuStatus = "NextHearingDate is not Valid for " + data.newlstget1[i].NextHearingDate;
                        //            return data;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        data.stuStatus = "NextHearingDate can not be null or empty";
                        //        return data;
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    data.stuStatus = "NextHearingDate Header Is Changed , Header Should Be NextHearingDate";
                        //    return data;
                        //}

                        //CNRNumber
                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].CNRNumber))
                            {
                                enq.LH_CNRNumber = data.newlstget1[i].CNRNumber.Trim();
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "CNR Number Header Is Changed , Header Should Be CNR Number";
                            return data;
                        }

                        //CaseNumber
                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].CNRNumber))
                            {
                                enq.LH_CaseNumber = data.newlstget1[i].CNRNumber.Trim();
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "CNR Number Header Is Changed , Header Should Be CNR Number";
                            return data;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.stuStatus = "Select Proper Excel Sheet";
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogInformation(" Import Checking Validation" + ex.Message);
            }
            return data;
        }


    }
}
