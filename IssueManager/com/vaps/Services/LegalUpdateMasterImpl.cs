using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Legal;
using IssueManager.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class LegalUpdateMasterImpl : Interfaces.LegalUpdateMasterInterface
    {
        public IssueManagerContext _IssueContext;

        public DomainModelMsSqlServerContext _Context;

        ILogger<LegalUpdateMasterImpl> _acdimpl;

        private readonly ILogger _log;
        public LegalUpdateMasterImpl(IssueManagerContext IssueContext, DomainModelMsSqlServerContext Context)
        {
            _IssueContext = IssueContext;
            _Context = Context;
        }

        public Legal_CaseDetailsDTO loaddata(Legal_CaseDetailsDTO dto)
        {
            try
            {
                {
                    dto.category1 = _IssueContext.LNC_Master_Court_TypeDMO.Where(t => t.MCT_ActiveFlag == true).ToArray();
                    dto.Gridcasedetails = _IssueContext.Legal_CaseDetailsDMO.Where(t => t.LCD_ActiveFlag == true).ToArray();

                    //using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "Legal_UpdateMaster";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    //    {
                    //        Value = 0
                    //    });
                    //    //cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    //    //{
                    //    //    Value = dto.HRME_Id
                    //    //});
                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();
                    //    var retObject = new List<dynamic>();
                    //    try
                    //    {
                    //        using (var dataReader = cmd.ExecuteReader())
                    //        {
                    //            while (dataReader.Read())
                    //            {
                    //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                {
                    //                    dataRow.Add(
                    //                    dataReader.GetName(iFiled),
                    //                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                }
                    //                retObject.Add((ExpandoObject)dataRow);
                    //            }
                    //        }
                    //        dto.gridcasedetails = retObject.ToArray();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }

                    //}

                    dto.gridcasestatus = _IssueContext.Legal_CaseStatusDMO.Where(t => t.LCS_ActiveFlag == true).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;
        }

        public Legal_CaseDetailsDTO ViewLegalCaseDetails(Legal_CaseDetailsDTO data)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

            try
            {
                data.gridcasestatus = _IssueContext.Legal_CaseStatusDMO.Where(t => t.LCS_ActiveFlag == true && t.LCD_Id == data.LCD_Id).ToArray();
                data.gridadvocate = _IssueContext.Legal_AdvocateDMO.Where(t => t.LA_ActiveFlag == true && t.LCD_Id == data.LCD_Id).ToArray();
                data.gridacts = _IssueContext.Legal_ActsDMO.Where(t => t.LACT_ActiveFlag == true && t.LCD_Id == data.LCD_Id).ToArray();


                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Legal_Case_Details_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@LCD_Id", SqlDbType.BigInt) { Value = data.LCD_Id });
                  

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
                        }
                        data.gridcasehistory = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                // data.gridcasehistory = _IssueContext.Legal_CaseHistoryDMO.Where(t => t.LCH_ActiveFlag == true && t.LCD_Id == data.LCD_Id).ToArray();



                data.gridfinalorder = _IssueContext.Legal_FinalOrderDMO.Where(t => t.LFO_ActiveFlag == true && t.LCD_Id == data.LCD_Id).ToArray();
                data.Documentslist = _IssueContext.Legal_Documents.Where(t => t.LD_ActiveFlag == true && t.LCD_Id == data.LCD_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Legal_CaseDetailsDTO editdata(Legal_CaseDetailsDTO dto)
        {
            try
            {
                dto.gridcasestatus = _IssueContext.Legal_CaseStatusDMO.Where(t => t.LCS_ActiveFlag == true && t.LCD_Id ==dto.LCD_Id).ToArray();
                dto.gridadvocate = _IssueContext.Legal_AdvocateDMO.Where(t => t.LA_ActiveFlag == true && t.LCD_Id == dto.LCD_Id).ToArray();


                dto.gridacts = _IssueContext.Legal_ActsDMO.Where(t => t.LACT_ActiveFlag == true && t.LCD_Id == dto.LCD_Id).ToArray();


                // dto.gridcasehistory = _IssueContext.Legal_CaseHistoryDMO.Where(t => t.LCH_ActiveFlag == true && t.LCD_Id == dto.LCD_Id).ToArray();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Legal_Case_Details_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@LCD_Id", SqlDbType.BigInt) { Value = dto.LCD_Id });


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
                        }
                        dto.gridcasehistory = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                dto.gridfinalorder = _IssueContext.Legal_FinalOrderDMO.Where(t => t.LFO_ActiveFlag == true).ToArray();
                dto.Documentslist = _IssueContext.Legal_Documents.Where(t => t.LD_ActiveFlag == true && t.LCD_Id == dto.LCD_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public Legal_CaseDetailsDTO savelegalcasedetails(Legal_CaseDetailsDTO dto)
        {
            try
            {
                {
                    Legal_CaseDetailsDMO enq = new Legal_CaseDetailsDMO();
                    enq.LCD_Id = dto.LCD_Id;
                    enq.LCD_CaseType = dto.LCD_CaseType;
                    enq.LCD_FillingNumber = dto.LCD_FillingNumber;
                    enq.LCD_FillingDate = dto.LCD_FillingDate;
                    enq.LCD_RegistrationNumber = dto.LCD_RegistrationNumber;
                    enq.LCD_RegistrationDate = dto.LCD_RegistrationDate;
                    enq.LCD_CNRNumber = dto.LCD_CNRNumber;
                    enq.LCD_ActiveFlag = true;
                    enq.LCD_CreatedDate = DateTime.Now;
                    enq.LCD_UpdatedDate = DateTime.Now;
                    enq.LCD_CreatedBy = dto.userId;
                    enq.LCD_UpdatedBy = dto.userId;
                    enq.LCD_PartyName = dto.LCD_PartyName;
                    enq.LCD_NextHearingDate = dto.LCD_NextHearingDate;
                    _IssueContext.Add(enq);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Legal_CaseDetailsDTO savecasestatus(Legal_CaseDetailsDTO dto)
        {
            try
            {
                if (dto.LCS_Id > 0)
                {
                    var result = _IssueContext.Legal_CaseStatusDMO.Single(t => t.LCS_Id == dto.LCS_Id);
                    result.LCS_FirstHearingDate = dto.LCS_FirstHearingDate;
                    result.LCS_CaseStatus = dto.LCS_CaseStatus;
                    result.LCS_DecisionDate = dto.LCS_DecisionDate;
                    result.LCS_NatureofDisposal = dto.LCS_NatureofDisposal;
                    result.LCS_CourtNumberandJudge = dto.LCS_CourtNumberandJudge;
                    _IssueContext.Update(result);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    Legal_CaseStatusDMO enq1 = new Legal_CaseStatusDMO();
                    enq1.LCS_CaseStatus = dto.LCS_CaseStatus;
                    enq1.LCD_Id = dto.LCD_Id;
                    enq1.LCS_FirstHearingDate = dto.LCS_FirstHearingDate;
                    enq1.LCS_DecisionDate = dto.LCS_DecisionDate;
                    enq1.LCS_NatureofDisposal = dto.LCS_NatureofDisposal;
                    enq1.LCS_CourtNumberandJudge = dto.LCS_CourtNumberandJudge;
                    enq1.LCS_ActiveFlag = true;
                    enq1.LCS_CreatedDate = DateTime.Now;
                    enq1.LCS_UpdatedDate = DateTime.Now;
                    enq1.LCS_CreatedBy = dto.userId;
                    enq1.LCS_UpdatedBy = dto.userId;
                    _IssueContext.Add(enq1);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Legal_CaseDetailsDTO saveadvocate(Legal_CaseDetailsDTO dto)
        {
            try
            {
                if (dto.LA_Id > 0)
                {
                    var result = _IssueContext.Legal_AdvocateDMO.Single(t => t.LA_Id == dto.LA_Id);
                    result.LA_PetitionerandAdvocate = dto.LA_PetitionerandAdvocate;
                    result.LA_RespondentandAdvocate = dto.LA_RspondentandAdvocate;
                    _IssueContext.Update(result);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    Legal_AdvocateDMO enq2 = new Legal_AdvocateDMO();
                    enq2.LCD_Id = dto.LCD_Id;
                    enq2.LA_PetitionerandAdvocate = dto.LA_PetitionerandAdvocate;
                    enq2.LA_RespondentandAdvocate = dto.LA_RspondentandAdvocate;
                    enq2.LA_ActiveFlag = true;
                    enq2.LA_CreatedDate = DateTime.Now;
                    enq2.LA_UpdatedDate = DateTime.Now;
                    enq2.LA_CreatedBy = dto.userId;
                    enq2.LA_UpdatedBy = dto.userId;
                    _IssueContext.Add(enq2);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Legal_CaseDetailsDTO saveacts(Legal_CaseDetailsDTO dto)
        {
            try
            {
                if (dto.LACT_Id > 0)
                {
                    var result = _IssueContext.Legal_ActsDMO.Single(t => t.LACT_Id == dto.LACT_Id);
                    result.LACT_UnderActs = dto.LA_PetitionerandAdvocate;
                    result.LACT_UnderSection = dto.LA_RspondentandAdvocate;
                    _IssueContext.Update(result);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    Legal_ActsDMO enq3 = new Legal_ActsDMO();
                    enq3.LCD_Id = dto.LCD_Id;
                    enq3.LACT_UnderActs = dto.LACT_UnderActs;
                    enq3.LACT_UnderSection = dto.LACT_UnderSection;
                    enq3.LACT_ActiveFlag = true;
                    enq3.LACT_CreatedDate = DateTime.Now;
                    enq3.LACT_UpdatedDate = DateTime.Now;
                    enq3.LACT_CreatedBy = dto.userId;
                    enq3.LACT_UpdatedBy = dto.userId;
                    _IssueContext.Add(enq3);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Legal_CaseDetailsDTO savecasehistory(Legal_CaseDetailsDTO dto)
        {
            try
            {
                if (dto.LCH_Id > 0)
                {
                    var result = _IssueContext.Legal_CaseHistoryDMO.Single(t => t.LCH_Id == dto.LCH_Id);
                    var result1 = _IssueContext.Legal_CaseDetailsDMO.Single(t => t.LCD_Id == dto.LCD_Id);
                    //Legal_CaseDetailsDMO obj = new Legal_CaseDetailsDMO();
                    result1.LCD_PartyName = dto.LCD_PartyName;
                    result1.LCD_NextHearingDate = dto.LCD_NextHearingDate;
                    result.LCH_RegistrationNumber = dto.LCH_RegistrationNumber;
                    result.LCH_Judge = dto.LCH_Judge;
                    result.LCH_BusinessonDate = dto.LCH_BusinessonDate;
                    result.LCH_PurposeofHearing = dto.LCH_PurposeofHearing;
                    result.LCH_HearingDate = dto.LCH_HearingDate;
                    _IssueContext.Update(result);
                    _IssueContext.Update(result1);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    Legal_CaseHistoryDMO enq4 = new Legal_CaseHistoryDMO();
                    Legal_CaseDetailsDMO obj = new Legal_CaseDetailsDMO();
                    enq4.LCD_Id = dto.LCD_Id;
                    enq4.LCH_RegistrationNumber = dto.LCH_RegistrationNumber;
                    enq4.LCH_Judge = dto.LCH_Judge;
                    enq4.LCH_BusinessonDate = DateTime.Now;
                    enq4.LCH_PurposeofHearing = dto.LCH_PurposeofHearing;
                    obj.LCD_PartyName = dto.LCD_PartyName;
                    obj.LCD_NextHearingDate = dto.LCD_NextHearingDate;
                    enq4.LCH_HearingDate = DateTime.Now;
                    enq4.LCH_ActiveFlag = true;
                    enq4.LCH_CreatedDate = DateTime.Now;
                    enq4.LCH_UpdatedDate = DateTime.Now;
                    enq4.LCH_CreatedBy = dto.userId;
                    enq4.LCH_UpdatedBy = dto.userId;
                    _IssueContext.Add(enq4);
                    _IssueContext.Add(obj);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
                // if (dto.LCH_Id > 0)
                //{
                  
                //    _IssueContext.Add(obj);
                //    _IssueContext.SaveChanges();
                //}
              

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Legal_CaseDetailsDTO savefinalorder(Legal_CaseDetailsDTO dto)
        {
            try
            {
                if (dto.LFO_Id > 0)
                {
                    var result = _IssueContext.Legal_FinalOrderDMO.Single(t => t.LFO_Id == dto.LFO_Id);
                    result.LFO_OrderDetails = dto.LFO_OrderDetails;
                    result.LFO_OrderNumber = dto.LFO_OrderNumber;
                    result.LFO_Date = dto.LFO_Date;
                    _IssueContext.Update(result);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    Legal_FinalOrderDMO enq5 = new Legal_FinalOrderDMO();
                    enq5.LCD_Id = dto.LCD_Id;
                    enq5.LFO_OrderDetails = dto.LFO_OrderDetails;
                    enq5.LFO_OrderNumber = dto.LFO_OrderNumber;
                    enq5.LFO_Date = DateTime.Now;
                    enq5.LFO_ActiveFlag = true;
                    enq5.LFO_CreatedDate = DateTime.Now;
                    enq5.LFO_UpdatedDate = DateTime.Now;
                    enq5.LFO_CreatedBy = dto.userId;
                    enq5.LFO_UpdatedBy = dto.userId;
                    _IssueContext.Add(enq5);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Legal_CaseDetailsDTO SaveDocuments(Legal_CaseDetailsDTO dto)
        {
            try
            {
                if (dto.LD_Id > 0)
                {
                    var result = _IssueContext.Legal_Documents.Single(t => t.LD_Id == dto.LD_Id);
                    result.LD_DocumentName = dto.LD_DocumentName;
                    result.Ld_DocumentPath = dto.Ld_DocumentPath;
                    result.LD_Remarks = dto.LD_Remarks;
                    //result.LD_ActiveFlag = true;
                    //result.LD_CreatedDate = DateTime.Now;
                    result.LD_UpdatedDate = DateTime.Now;
                    //result.LD_CreatedBy = dto.userId;
                    result.LD_UpdatedBy = dto.userId;
                    _IssueContext.Update(result);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    Legal_DocumentsDMO DOC = new Legal_DocumentsDMO();
                    DOC.LCD_Id = dto.LCD_Id;
                    DOC.LD_DocumentName = dto.LD_DocumentName;
                    DOC.Ld_DocumentPath = dto.Ld_DocumentPath;
                    DOC.LD_Remarks = dto.LD_Remarks;

                    DOC.LD_ActiveFlag = true;
                    DOC.LD_CreatedDate = DateTime.Now;
                    DOC.LD_UpdatedDate = DateTime.Now;
                    DOC.LD_CreatedBy = dto.userId;
                    DOC.LD_UpdatedBy = dto.userId;
                    _IssueContext.Add(DOC);
                    _IssueContext.SaveChanges();
                    dto.returnvales = "Add";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


    }
}
