using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class EmpLoanPayoutImpl : Interfaces.EmpLoanPayoutInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmpLoanPayoutImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public EmpLoanPayoutDTO getBasicData(EmpLoanPayoutDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }



            return dto;
        }

        public EmpLoanPayoutDTO SaveUpdate(EmpLoanPayoutDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_EmpLoanDisburse dmoObj = Mapper.Map<HR_EmpLoanDisburse>(dto);

                var duplicatecountresult = _HRMSContext.HR_EmpLoanDisburse.Where(t => t.MI_Id == dto.MI_Id && t.HRELDIS_Amount == dto.HRELDIS_Amount && t.HRELDIS_DisbursedDate == dto.HRELDIS_DisbursedDate && t.HRELDIS_Id != dto.HRELDIS_Id).Count();
                if (duplicatecountresult == 0)
                {

                    if (dmoObj.HRELDIS_Id > 0)
                    {

                        var duplicatecount = _HRMSContext.HR_EmpLoanDisburse.Where(t => t.MI_Id == dto.MI_Id && t.HRELDIS_Amount == dto.HRELDIS_Amount && t.HRELDIS_Id != dto.HRELDIS_Id).Count();
                        if (duplicatecount == 0)
                        {
                            var result = _HRMSContext.HR_EmpLoanDisburse.Single(t => t.HRELDIS_Id == dmoObj.HRELDIS_Id);

                            dto.HRELDIS_UpdatedDate = DateTime.Now;
                            result.HRELDIS_UpdatedBy = dto.LogInUserId;
                            result.HRELDIS_Amount = dto.HRELDIS_Amount;
                            result.HRELDIS_DisbursedDate = dto.HRELDIS_DisbursedDate;
                            result.HRELDIS_PaymentMode = dto.HRELDIS_PaymentMode;
                            result.HRELDIS_Remarks = dto.HRELDIS_Remarks;
                            result.HRELDIS_ActiveFlg = true;
                            
                            Mapper.Map(dto, result);
                            _HRMSContext.Update(result);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                        }
                    }
                    else
                    {
                        var duplicatecount = _HRMSContext.HR_EmpLoanDisburse.Where(t => t.MI_Id == dto.MI_Id && t.HRELDIS_Amount == dto.HRELDIS_Amount && t.HRELDIS_DisbursedDate==dto.HRELDIS_DisbursedDate && t.HRELDIS_PaymentMode==dto.HRELDIS_PaymentMode).Count();
                        if (duplicatecount == 0)
                        {


                            dmoObj.HRELDIS_ActiveFlg = true;
                            dmoObj.HRME_Id = dto.HRME_Id;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRELDIS_DisbursedDate = dto.HRELDIS_DisbursedDate;
                            dmoObj.HRELDIS_Month = dto.HRELDIS_DisbursedDate.Month;
                            dmoObj.HRELDIS_Year = dto.HRELDIS_DisbursedDate.Year;
                            dmoObj.HRELDIS_CreatedBy = dto.LogInUserId;
                            dmoObj.HRELDIS_UpdatedBy = dto.LogInUserId;
                            dmoObj.HRELDIS_CreatedDate = DateTime.Now;
                            dmoObj.HRELDIS_UpdatedDate = DateTime.Now;
                            dmoObj.HRELDIS_Amount = dto.HRELDIS_Amount;
                            dmoObj.HRELDIS_Remarks = dto.HRELDIS_Remarks;
                            dmoObj.HRELDIS_PaymentMode = dto.HRELDIS_PaymentMode;


                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                            var updateloan = _HRMSContext.HR_Emp_Loan.Single(t => t.HREL_Id == dto.HREL_Id);
                            updateloan.HREL_SanctionedAmount= dto.HRELDIS_Amount;
                            updateloan.HREL_LoanStatus = "Sanctioned";
                            _HRMSContext.Update(updateloan);
                            _HRMSContext.SaveChanges();
                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                }



                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public EmpLoanPayoutDTO editData(int id)
        {

            EmpLoanPayoutDTO dto = new EmpLoanPayoutDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_EmpLoanDisburse> lorg = new List<HR_EmpLoanDisburse>();
                lorg = _HRMSContext.HR_EmpLoanDisburse.AsNoTracking().Where(t => t.HRELDIS_Id.Equals(id)).ToList();
                dto.editdata = lorg.ToArray();

                dto.Employeelist = (from a in _HRMSContext.HR_Emp_Loan
                                     from b in _HRMSContext.MasterEmployee
                                     where (a.HRME_Id == b.HRME_Id && a.HREL_LoanStatus == "Approved")
                                     select new EmpLoanPayoutDTO
                                     {
                                         HREL_Id = a.HREL_Id,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + ' ' + b.HRME_EmployeeLastName





                                     }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public EmpLoanPayoutDTO employeechange(int id)
        {

            EmpLoanPayoutDTO dto = new EmpLoanPayoutDTO();
            dto.retrunMsg = "";
            try
            {

                dto.loandetails = (from a in _HRMSContext.HR_Emp_Loan
                                        from b in _HRMSContext.MasterEmployee
                                        
                                        from e in _HRMSContext.HRMasterLoan
                                        where (a.HRMLN_Id == e.HRMLN_Id &&  a.HREL_Id == id && a.HRME_Id == b.HRME_Id && a.HREL_ActiveFlag == true)
                                        select new EmpLoanPayoutDTO
                                        {
                                            HREL_Id=a.HREL_Id,
                                            HRME_EmployeeFirstName = b.HRME_EmployeeFirstName,
                                            HRME_Id = a.HRME_Id,
                                            
                                           HRML_LoanType = e.HRML_LoanType,
                                           HREL_SanctionedAmount=a.HREL_SanctionedAmount,
                                           HREL_LoanAmount=a.HREL_LoanAmount



                                        }).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public EmpLoanPayoutDTO deactivate(EmpLoanPayoutDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRELDIS_Id > 0)
                {
                    var result = _HRMSContext.HR_EmpLoanDisburse.Single(t => t.HRELDIS_Id == dto.HRELDIS_Id);

                    if (result.HRELDIS_ActiveFlg == true)
                    {
                        result.HRELDIS_ActiveFlg = false;
                    }
                    else if (result.HRELDIS_ActiveFlg == false)
                    {
                        result.HRELDIS_ActiveFlg = true;
                    }
                    result.HRELDIS_UpdatedDate = DateTime.Now;
                    result.HRELDIS_UpdatedBy = dto.LogInUserId;
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRELDIS_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public EmpLoanPayoutDTO GetAllDropdownAndDatatableDetails(EmpLoanPayoutDTO data)
        {
            List<HR_EmpLoanDisburse> datalist = new List<HR_EmpLoanDisburse>();
            try
            {

                data.gmasterloanList = (from a in _HRMSContext.HR_Emp_Loan
                                        from b in _HRMSContext.MasterEmployee                                       
                                        from d in _HRMSContext.HR_EmpLoanDisburse
                                        from e in _HRMSContext.HRMasterLoan
                                        where (a.HRMLN_Id ==e.HRMLN_Id  && a.HREL_Id==d.HREL_Id && a.HRME_Id==d.HRME_Id && a.MI_Id == data.MI_Id  && a.HRME_Id == b.HRME_Id && a.HREL_LoanStatus=="Approved")
                                        select new EmpLoanPayoutDTO
                                        {

                                            HRELDIS_Remarks = d.HRELDIS_Remarks,
                                            HRME_EmployeeFirstName = b.HRME_EmployeeFirstName,
                                            HRME_Id=a.HRME_Id,
                                            HRELDIS_PaymentMode=d.HRELDIS_PaymentMode,
                                          
                                            HRELDIS_DisbursedDate=d.HRELDIS_DisbursedDate,
                                            HRELDIS_Id=d.HRELDIS_Id,
                                            HREL_Id=a.HREL_Id,
                                            HRML_LoanType=e.HRML_LoanType,
                                            HRELDIS_Amount=d.HRELDIS_Amount,
                                            HRELDIS_ActiveFlg = d.HRELDIS_ActiveFlg



                                        }).ToArray();

                var finallevel = _HRMSContext.HR_EmpLoanDisburse.Where(t => t.MI_Id == data.MI_Id).Select(t => t.HREL_Id).ToList();
                var hrmeid = _HRMSContext.HR_Emp_Loan.Where(t => t.MI_Id == data.MI_Id && !finallevel.Contains(t.HREL_Id)).Select(t => t.HRME_Id).ToList();


                data.Employeelist= (from a in _HRMSContext.HR_Emp_Loan
                                    from b in _HRMSContext.MasterEmployee
                                    where (a.HRME_Id==b.HRME_Id && !finallevel.Contains(a.HREL_Id) && a.HREL_LoanStatus=="Approved")
                                    select new EmpLoanPayoutDTO
                                    {
                                        HREL_Id=a.HREL_Id,
                                        HRME_Id=a.HRME_Id,
                                        HRME_EmployeeFirstName=b.HRME_EmployeeFirstName+' '+b.HRME_EmployeeLastName

                                    



                                    }).ToArray();






                var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true && t.MI_Id== data.MI_Id).ToList();
                data.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();













            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
