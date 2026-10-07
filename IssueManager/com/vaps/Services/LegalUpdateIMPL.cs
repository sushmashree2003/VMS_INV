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
    public class LegalUpdateIMPL : Interfaces.LegalUpdateInterface
    {

        public IssueManagerContext _IssueContext;

        public DomainModelMsSqlServerContext _Context;

        ILogger<LegalUpdateIMPL> _acdimpl;

        private readonly ILogger _log;
        public LegalUpdateIMPL(IssueManagerContext IssueContext, DomainModelMsSqlServerContext Context)
        {
            _IssueContext = IssueContext;
            _Context = Context;
        }


        public LegalUpdateDTO loaddata(LegalUpdateDTO dto)
        {
            try
            {
                {
                    dto.countdetails = _IssueContext.Legal_UpdateDMO.Where(t => t.LU_ActiveFlag == true).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;
        }

        public LegalUpdateDTO saveData1(LegalUpdateDTO dto)
        {
            int recordsSaved = 0;
            try
            {
                {
                    if (dto.LegalUpdate != null)
                    {

                        //Legal_UpdateDMO enq = new Legal_UpdateDMO();
                        //enq.LU_Id = dto.LU_Id;
                        //enq.LU_CaseType = item.LU_CaseType;
                        //enq.LU_FillingNumber = item.LU_FillingNumber;
                        //enq.LU_FillingDate = item.LU_FillingDate;
                        //enq.LU_RegistrationNumber = item.LU_RegistrationNumber;
                        //enq.LU_RegistrationDate = item.LU_RegistrationDate;
                        //enq.LU_CNRNumber = item.LU_CNRNumber;
                        //enq.LU_FirstHearingDate = item.LU_FirstHearingDate;
                        //enq.LU_NextHearingDate = item.LU_NextHearingDate;
                        //enq.LU_CaseStatus = item.LU_CaseStatus;
                        //enq.LU_StageofCase = item.LU_StageofCase;
                        //enq.LU_CourtNumber = item.LU_CourtNumber;
                        //enq.LU_UnderActs = item.LU_UnderActs;
                        //enq.LU_UnderSection = item.LU_UnderSection;
                        //enq.LU_Judge = item.LU_Judge;
                        //enq.LU_BusinessOnDate = item.LU_BusinessOnDate;
                        //enq.LU_HearingDate = item.LU_HearingDate;
                        //enq.LU_PurposeofHearing = item.LU_PurposeofHearing;
                        //enq.LU_ProcessId = item.LU_ProcessId;
                        //enq.LU_ProcessDate = item.LU_ProcessDate;
                        //enq.LU_ProcessTittle = item.LU_ProcessTittle;
                        //enq.LU_PartyName = item.LU_PartyName;
                        //enq.LU_IssuedProcess = item.LU_IssuedProcess;
                        //enq.LU_PetitionerandAdvocate = item.LU_PetitionerandAdvocate;
                        //enq.LU_RespondentandAdvocate = item.LU_RespondentandAdvocate;
                        //enq.LU_ActiveFlag = true;
                        //enq.LU_CreatedDate = DateTime.Now;
                        //enq.LU_UpdatedDate = DateTime.Now;
                        //enq.LU_CreatedBy = dto.userId;
                        //enq.LU_UpdatedBy = dto.userId;
                        //_IssueContext.Add(enq);
                        //_IssueContext.SaveChanges();
                        //recordsSaved++;
                        //dto.RecordsSavedCount = recordsSaved;
                        //dto.returnvales = "Add";

                        int contact = 0;
                        foreach (var headlst in dto.LegalUpdate)
                        {
                            var contactAdd = _IssueContext.Database.ExecuteSqlCommand("Legal_DataImport @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23,@p24",
                                dto.userId, headlst.LU_CaseType,
                                headlst.LU_FillingNumber, headlst.LU_FillingDate,
                             headlst.LU_RegistrationNumber, headlst.LU_RegistrationDate, headlst.LU_CNRNumber,
                              headlst.LU_FirstHearingDate, headlst.LU_NextHearingDate, headlst.LU_CaseStatus,
                              headlst.LU_StageofCase, headlst.LU_CourtNumber, headlst.LU_UnderActs,
                              headlst.LU_UnderSection, headlst.LU_Judge, headlst.LU_BusinessOnDate,
                              headlst.LU_HearingDate, headlst.LU_PurposeofHearing, headlst.LU_ProcessId,
                              headlst.LU_ProcessDate, headlst.LU_ProcessTittle, headlst.LU_PartyName,
                              headlst.LU_IssuedProcess, headlst.LU_PetitionerandAdvocate, headlst.LU_RespondentandAdvocate);

                            contact = contact + contactAdd;
                        }
                        if (contact >= 1)
                        {
                            dto.returnvales = "Saved";
                        }
                        else
                        {
                            dto.returnvales = "not Saved";
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

        public async Task<LegalUpdateDTO> validation(LegalUpdateDTO data)
        {
            try
            {
                for (int i = 0; i < data.newlstget1.Length; i++)
                {
                    try
                    {
                        Legal_UpdateDMO enq = new Legal_UpdateDMO();

                        //---------- CaseType---------//

                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].CaseType))
                            {
                                enq.LU_CaseType = data.newlstget1[i].CaseType.Trim();
                            }
                            else
                            {
                                data.stuStatus = "CaseType can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "CaseType Header Is Changed , Header Should Be CaseType";
                            return data;
                        }


                        //---------- FillingNumber---------//
                        try
                        {
                            if ((data.newlstget1[i].FillingNumber?.Trim() ?? "") != "")
                            {
                                if (data.newlstget1[i].FillingNumber.Trim().Length <= 100)
                                {
                                    enq.LU_FillingNumber = data.newlstget1[i].FillingNumber.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Filling Number is not Valid for " + data.newlstget1[i].FillingNumber;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Filling Number can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Filling Number Header Is Changed , Header Should Be FillingNumber";
                            return data;
                        }

                        //---------- FillingDate---------//

                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].FillingDate) != null) && (Convert.ToString(data.newlstget1[i].FillingDate) != ""))
                            {

                                if (data.newlstget1[i].FillingDate.Length > 0)
                                {
                                    enq.LU_FillingDate = Convert.ToDateTime(data.newlstget1[i].FillingDate);
                                }
                                else
                                {
                                    data.stuStatus = "FillingDate is not Valid for" + " " + data.newlstget1[i].FillingDate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "FillingDate can not be Empty for" + " " + data.newlstget1[i].FillingDate;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "FillingDate Header Is Changed , Header Should Be FillingDate";
                            return data;
                        }

                        //---------- RegistrationNumber---------//
                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].RegistrationNumber))
                            {
                                enq.LU_RegistrationNumber = data.newlstget1[i].RegistrationNumber.Trim();
                            }
                            else
                            {
                                data.stuStatus = "RegistrationNumber can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "RegistrationNumber Header Is Changed , Header Should Be RegistrationNumber";
                            return data;
                        }

                        //---------- RegistrationDate---------//

                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].RegistrationDate) != null) && (Convert.ToString(data.newlstget1[i].RegistrationDate) != ""))
                            {

                                if (data.newlstget1[i].RegistrationDate.Length > 0)
                                {
                                    enq.LU_FillingDate = Convert.ToDateTime(data.newlstget1[i].RegistrationDate);
                                }
                                else
                                {
                                    data.stuStatus = "RegistrationDate is not Valid for" + " " + data.newlstget1[i].RegistrationDate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "RegistrationDate can not be Empty for" + " " + data.newlstget1[i].RegistrationDate;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "FillingDate Header Is Changed , Header Should Be FillingDate";
                            return data;
                        }


                        //---  CNR Number --//
                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].CNRNumber))
                            {
                                enq.LU_CNRNumber = data.newlstget1[i].CNRNumber.Trim();
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "CNR Number Header Is Changed , Header Should Be CNRNumber";
                            return data;
                        }


                        //FirstHearingDate

                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].FirstHearingDate) != null) && (Convert.ToString(data.newlstget1[i].FirstHearingDate) != ""))
                            {

                                if (data.newlstget1[i].FirstHearingDate.Length > 0)
                                {
                                    enq.LU_FirstHearingDate = Convert.ToDateTime(data.newlstget1[i].FirstHearingDate);
                                }
                                else
                                {
                                    data.stuStatus = "FirstHearingDate is not Valid for" + " " + data.newlstget1[i].FirstHearingDate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "FirstHearingDate can not be Empty for" + " " + data.newlstget1[i].FirstHearingDate;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "FirstHearingDate Header Is Changed , Header Should Be FirstHearingDate";
                            return data;
                        }

                        //NextHearingDate
                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].NextHearingDate) != null) && (Convert.ToString(data.newlstget1[i].NextHearingDate) != ""))
                            {

                                if (data.newlstget1[i].NextHearingDate.Length > 0)
                                {
                                    enq.LU_NextHearingDate = Convert.ToDateTime(data.newlstget1[i].NextHearingDate);
                                }
                                else
                                {
                                    data.stuStatus = "NextHearingDate is not Valid for" + " " + data.newlstget1[i].NextHearingDate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "NextHearingDate can not be Empty for" + " " + data.newlstget1[i].NextHearingDate;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "NextHearingDate Header Is Changed , Header Should Be NextHearingDate";
                            return data;
                        }


                        //CaseStatus
                        try
                        {
                            if ((data.newlstget1[i].CaseStatus?.Trim() ?? "") != "")
                            {
                                if (Regex.IsMatch(data.newlstget1[i].CaseStatus.Trim(), @"^[a-zA-Z.\s]+$") && (data.newlstget1[i].CaseStatus.Trim().Length <= 50))
                                {
                                    enq.LU_CaseStatus = data.newlstget1[i].CaseStatus.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Case Status is not Valid for " + data.newlstget1[i].CaseStatus;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Case Status can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Case Status Header Is Changed , Header Should Be Case Status";
                            return data;
                        }

                        //Stageofcase
                        try
                        {
                            if ((data.newlstget1[i].Stageofcase?.Trim() ?? "") != "")
                            {
                                if (Regex.IsMatch(data.newlstget1[i].Stageofcase.Trim(), @"^[a-zA-Z.\s]+$") && (data.newlstget1[i].Stageofcase.Trim().Length <= 50))
                                {
                                    enq.LU_StageofCase = data.newlstget1[i].Stageofcase.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Stage of case is not Valid for " + data.newlstget1[i].Stageofcase;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Stage of case can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Stage of case Header Is Changed , Header Should Be Stage of case";
                            return data;
                        }


                        //---------- CourtNumberandJudge---------//
                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].CourtNumberandJudge))
                            {
                                enq.LU_CourtNumber = data.newlstget1[i].CourtNumberandJudge.Trim();
                            }
                            else
                            {
                                data.stuStatus = "CourtNumberandJudge can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "CourtNumberandJudge Header Is Changed , Header Should Be CourtNumberandJudge";
                            return data;
                        }
                        //---------- CourtNumberandJudge---------//                      

                        //UnderActs

                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].UnderActs))
                            {
                                enq.LU_UnderActs = data.newlstget1[i].UnderActs.Trim();
                            }
                            else
                            {
                                data.stuStatus = "UnderActs can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "UnderActs Header Is Changed , Header Should Be UnderActs";
                            return data;
                        }

                        //UnderSection                       
                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].UnderSection))
                            {
                                enq.LU_UnderSection = data.newlstget1[i].UnderSection.Trim();
                            }
                            else
                            {
                                data.stuStatus = "UnderSection can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "UnderSection Header Is Changed , Header Should Be UnderSection";
                            return data;
                        }

                        //Judge
                        try
                        {
                            if (!string.IsNullOrEmpty(data.newlstget1[i].Judge))
                            {
                                enq.LU_Judge = data.newlstget1[i].Judge.Trim();
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

                        //BusinessOnDate

                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].BusinessOnDate) != null) && (Convert.ToString(data.newlstget1[i].BusinessOnDate) != ""))
                            {

                                if (data.newlstget1[i].BusinessOnDate.Length > 0)
                                {
                                    enq.LU_BusinessOnDate = Convert.ToDateTime(data.newlstget1[i].BusinessOnDate.TrimEnd().TrimStart());
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


                        //HearingDate

                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].HearingDate) != null) && (Convert.ToString(data.newlstget1[i].HearingDate) != ""))
                            {

                                if (data.newlstget1[i].HearingDate.Length > 0)
                                {
                                    enq.LU_HearingDate = Convert.ToDateTime(data.newlstget1[i].HearingDate);
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


                        //PurposeofHearing
                        try
                        {
                            if ((data.newlstget1[i].PurposeofHearing?.Trim() ?? "") != "")
                            {
                                if (Regex.IsMatch(data.newlstget1[i].PurposeofHearing.Trim(), @"^[a-zA-Z.\s]+$") && (data.newlstget1[i].PurposeofHearing.Trim().Length <= 50))
                                {
                                    enq.LU_PurposeofHearing = data.newlstget1[i].PurposeofHearing.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Purpose of Hearing is not Valid for " + data.newlstget1[i].PurposeofHearing;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Purpose of Hearing can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Purpose of Hearing Is Changed , Header Should Be Purpose of Hearing";
                            return data;
                        }

                        //ProcessId
                        try
                        {
                            if ((data.newlstget1[i].ProcessId?.Trim() ?? "") != "")
                            {
                                if (data.newlstget1[i].ProcessId.Trim().Length <= 100)
                                {
                                    enq.LU_ProcessId = data.newlstget1[i].ProcessId.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Process Id is not Valid for " + data.newlstget1[i].ProcessId;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Process Id can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Process Id Header Is Changed , Header Should Be FillingNumber";
                            return data;
                        }

                        //ProcessDate

                        try
                        {
                            if ((Convert.ToString(data.newlstget1[i].ProcessDate) != null) && (Convert.ToString(data.newlstget1[i].ProcessDate) != ""))
                            {

                                if (data.newlstget1[i].ProcessDate.Length > 0)
                                {
                                    enq.LU_ProcessDate = Convert.ToDateTime(data.newlstget1[i].ProcessDate.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    data.stuStatus = "ProcessDate is not Valid for" + " " + data.newlstget1[i].ProcessDate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "ProcessDate can not be Empty for" + " " + data.newlstget1[i].ProcessDate;
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "ProcessDate Header Is Changed , Header Should Be ProcessDate";
                            return data;
                        }


                        //ProcessTittle
                        try
                        {
                            if ((data.newlstget1[i].ProcessTittle?.Trim() ?? "") != "")
                            {
                                if (data.newlstget1[i].ProcessTittle.Trim().Length <= 100)
                                {
                                    enq.LU_ProcessTittle = data.newlstget1[i].ProcessTittle.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Process Tittle is not Valid for " + data.newlstget1[i].ProcessTittle;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Process Tittle can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Process Tittle Header Is Changed , Header Should Be Process Tittle";
                            return data;
                        }

                        //PartyName

                        try
                        {
                            if ((data.newlstget1[i].PartyName?.Trim() ?? "") != "")
                            {
                                if (data.newlstget1[i].PartyName.Trim().Length <= 100)
                                {
                                    enq.LU_PartyName = data.newlstget1[i].PartyName.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Party Name is not Valid for " + data.newlstget1[i].PartyName;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Party Name can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Party Name Header Is Changed , Header Should Be Party Name";
                            return data;
                        }

                        //IssuedProcess
                        try
                        {
                            if ((data.newlstget1[i].IssuedProcess?.Trim() ?? "") != "")
                            {
                                if (data.newlstget1[i].IssuedProcess.Trim().Length <= 100)
                                {
                                    enq.LU_IssuedProcess = data.newlstget1[i].IssuedProcess.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Issued Process is not Valid for " + data.newlstget1[i].IssuedProcess;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Issued Process can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Issued Process Header Is Changed , Header Should Be Issued Process";
                            return data;
                        }

                        //PetitionerAndAdvocate

                        try
                        {
                            if ((data.newlstget1[i].PetitionerAndAdvocate?.Trim() ?? "") != "")
                            {
                                if (data.newlstget1[i].PetitionerAndAdvocate.Trim().Length <= 100)
                                {
                                    enq.LU_PetitionerandAdvocate = data.newlstget1[i].PetitionerAndAdvocate.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Petitioner And Advocate is not Valid for " + data.newlstget1[i].PetitionerAndAdvocate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Petitioner And Advocate can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Petitioner And Advocate Header Is Changed , Header Should Be Petitioner And Advocate";
                            return data;
                        }

                        //RespondentAndAdvocate

                        try
                        {
                            if ((data.newlstget1[i].RespondentAndAdvocate?.Trim() ?? "") != "")
                            {
                                if (data.newlstget1[i].RespondentAndAdvocate.Trim().Length <= 100)
                                {
                                    enq.LU_RespondentandAdvocate = data.newlstget1[i].RespondentAndAdvocate.Trim();
                                }
                                else
                                {
                                    data.stuStatus = "Respondent And Advocate is not Valid for " + data.newlstget1[i].RespondentAndAdvocate;
                                    return data;
                                }
                            }
                            else
                            {
                                data.stuStatus = "Respondent And Advocate can not be null or empty";
                                return data;
                            }
                        }
                        catch (Exception ex)
                        {
                            data.stuStatus = "Respondent And Advocate Header Is Changed , Header Should Be Respondent And Advocate";
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
