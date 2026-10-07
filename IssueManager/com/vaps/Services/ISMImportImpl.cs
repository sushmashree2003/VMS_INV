using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISMImportImpl : Interfaces.ISMImportInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ISMImportImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public ISMImportDTO Savedata(ISMImportDTO data)
        {
            try
            {
                // long idd = 2587;

                //var a = "aa";
                //var b = "10 h";

                if (data.addtaskcontent.Length > 0)
                {
                    data.message = "";
                    var rowno = 1;
                    foreach (var item in data.addtaskcontent)
                    {
                        ///check the validation for start day and end day
                        ///
                        rowno += 1;

                        item.StartDay = item.StartDay.Trim();
                        item.EndDay = item.EndDay.Trim();
                        item.Effort_in_hrs = item.Effort_in_hrs.Trim();
                        if (item.Effort_in_hrs == "" || item.Effort_in_hrs == "NULL")
                        {
                            item.Effort_in_hrs = "8";
                        }

                        for (int i = 0; i < item.StartDay.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.StartDay.Trim()[i]))
                            {
                                data.message = "StartDay Column Field should not contain Any characters or special characters : Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.StartDay.Trim()[i]))
                            {
                                data.message = "StartDay Column Field should not contain Any characters or special characters: Row No:" + rowno;
                                return data;
                            }

                        }


                        for (int i = 0; i < item.EndDay.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.EndDay.Trim()[i]))
                            {
                                data.message = "EndDay Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.EndDay.Trim()[i]))
                            {
                                data.message = "EndDay Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }


                        for (int i = 0; i < item.Effort_in_hrs.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.Effort_in_hrs.Trim()[i]))
                            {
                                data.message = "Effort_in_hrs Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.Effort_in_hrs.Trim()[i]))
                            {
                                data.message = "Effort_in_hrs Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }


                        if (item.StartDay.Length > 2 || item.EndDay.Length > 2)
                        {
                            data.message = "StartDay/Endday Column Field should  contain 1 to 31 based on Month: Row No:" + rowno;
                            return data;
                        }

                        if (item.StartMonth.Length > 2 || item.EndMonth.Length > 2)
                        {
                            data.message = "StartMonth/EndMonth Column Field should  contain 1 to 12 : Row No:" + rowno;
                            return data;
                        }




                        for (int i = 0; i < item.StartMonth.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.StartMonth.Trim()[i]))
                            {
                                data.message = "StartMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.StartMonth.Trim()[i]))
                            {
                                data.message = "StartMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }



                        for (int i = 0; i < item.EndMonth.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.EndMonth.Trim()[i]))
                            {
                                data.message = "EndMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.EndMonth.Trim()[i]))
                            {
                                data.message = "EndMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }


                        for (int i = 0; i < item.StratYear.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.StratYear.Trim()[i]))
                            {
                                data.message = "StratYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.StratYear.Trim()[i]))
                            {
                                data.message = "StratYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }

                        for (int i = 0; i < item.Assigned_By_empcmp.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.Assigned_By_empcmp.Trim()[i]))
                            {
                                data.message = "Assigned_By_empcmp Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.Assigned_By_empcmp.Trim()[i]))
                            {
                                data.message = "Assigned_By_empcmp Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }

                        for (int i = 0; i < item.Assigned_to_empcmp.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.Assigned_to_empcmp.Trim()[i]))
                            {
                                data.message = "Assigned_to_empcmp Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.Assigned_to_empcmp.Trim()[i]))
                            {
                                data.message = "Assigned_to_empcmp Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }



                        for (int i = 0; i < item.EndYear.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.EndYear.Trim()[i]))
                            {
                                data.message = "EndYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.EndYear.Trim()[i]))
                            {
                                data.message = "EndYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }

                        if (item.StratYear.Length != 4 || item.EndYear.Length != 4)
                        {
                            data.message = "StratYear/EndYear Column Field should be correct format Row " + rowno + "   " + "EXAMPLE: 2019";
                            return data;
                        }

                        long smonth = Convert.ToInt64(item.StartMonth);


                        long monthday = _Context.month.Single(w => w.IVRM_Month_Id == smonth).IVRM_Month_Max_Days;

                        long sdate = Convert.ToInt64(item.StartDay);

                        if (sdate > monthday)
                        {
                            data.message = "StratDay is exceeding Month End Day at the Row " + rowno;
                            return data;
                        }

                        long emonth = Convert.ToInt64(item.EndMonth);

                        monthday = _Context.month.Single(w => w.IVRM_Month_Id == smonth).IVRM_Month_Max_Days;

                        long edate = Convert.ToInt64(item.EndDay);




                        if (edate > monthday)
                        {
                            data.message = "EndDay is exceeding Month End Day at the Row " + rowno;
                            return data;
                        }


                        if (edate <= 0 || sdate <= 0)
                        {
                            data.message = "StartDay/EndDay should be greater than ZERO at the Row " + rowno;
                            return data;
                        }






                        if (item.StartDay.Length == 1)
                        {
                            item.StartDay = "0" + item.StartDay;
                        }
                        if (item.EndDay.Length == 1)
                        {
                            item.EndDay = "0" + item.EndDay;
                        }

                        if (item.StartMonth.Length == 1)
                        {
                            item.StartMonth = "0" + item.StartMonth;
                        }
                        if (item.EndMonth.Length == 1)
                        {
                            item.EndMonth = "0" + item.EndMonth;
                        }


                        var hrmEempidassignbych = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.Assigned_By_empcmp.Trim()) && t.HRME_EmployeeCode.Trim() == item.Assigned_By_empcode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id })
                               .Distinct().ToList();

                        if (hrmEempidassignbych.Count == 0)
                        {
                            data.message = "Assigned_By_empcmp and  Assigned_By_empcode is missmatching(Employee does not belongs to company code  " + item.Assigned_By_empcmp + " OR Employee deactivated) at the Row " + rowno;
                            return data;
                        }



                        var hrmEempidassigntoch = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.Assigned_to_empcmp.Trim()) && t.HRME_EmployeeCode.Trim() == item.Assigned_to_empcode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id }).Distinct().ToList();


                        if (hrmEempidassigntoch.Count == 0)
                        {
                            data.message = "Assigned_To_empcmp and  Assigned_To_empcode is missmatching(Employee does not belongs to company code  " + item.Assigned_to_empcmp + " OR Employee deactivated) at the Row " + rowno;
                            return data;
                        }



                    }


                }






                if (data.addtaskcontent.Length > 0)
                {
                    foreach (var item in data.addtaskcontent)
                    {

                        long client_id = 0;
                        var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                        long empid = 0;
                        if (hrmEempid.Length > 0)
                        {
                            empid = hrmEempid.FirstOrDefault().Emp_Code;
                        }


                        //data.Auto_id = "TASK/"+idd+"/2019-2020";
                        if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Context);
                            data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                            data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                            data.Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                        }

                        ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                        task.ISMTCR_TaskNo = data.Auto_id;
                        task.MI_Id = data.MI_Id;

                        if (item.Project != "NULL" && item.Project != "" && item.Project != null)
                        {
                            var projectlist = _IssueContext.MastersProject_DMO.Where(r => r.MI_Id == data.MI_Id && r.ISMMPR_ProjectName.ToLower().Trim() == item.Project.ToLower().Trim() && r.ISMMPR_ActiveFlg == true).ToList();

                            if (projectlist.Count > 0)
                            {
                                task.ISMMPR_Id = projectlist.FirstOrDefault().ISMMPR_Id;
                            }
                            else
                            {
                                MastersProject_DMO prjobj = new MastersProject_DMO();
                                prjobj.MI_Id = data.MI_Id;
                                prjobj.ISMMPR_ProjectName = item.Project.Trim();
                                prjobj.ISMMPR_InternalProjectFlg = true;
                                prjobj.ISMMPR_ActiveFlg = true;
                                prjobj.ISMMPR_CreatedBy = data.UserId;
                                prjobj.ISMMPR_UpdatedBy = data.UserId;
                                _IssueContext.Add(prjobj);
                                _IssueContext.SaveChanges();
                                var projectlist1 = _IssueContext.MastersProject_DMO.Where(r => r.MI_Id == data.MI_Id && r.ISMMPR_ProjectName.ToLower().Trim() == item.Project.ToLower().Trim() && r.ISMMPR_ActiveFlg == true).ToList();

                                if (projectlist1.Count > 0)
                                {
                                    task.ISMMPR_Id = projectlist1.FirstOrDefault().ISMMPR_Id;
                                }

                            }

                        }

                        if (item.Module != "NULL" && item.Module != "" && item.Module != null)
                        {
                            var projectlist = _IssueContext.MasterModules.Where(r => r.IVRMM_ModuleName.ToLower().Trim() == item.Module.ToLower().Trim() && r.Module_ActiveFlag == 1).ToList();

                            if (projectlist.Count > 0)
                            {
                                task.IVRMM_Id = projectlist.FirstOrDefault().IVRMM_Id;
                            }
                            else
                            {
                                MasterModules modobj = new MasterModules();

                                modobj.IVRMM_ModuleName = item.Module.Trim();
                                modobj.IVRMM_ModuleName = item.Module.Trim();
                                modobj.Module_ActiveFlag = 1;
                                _IssueContext.Add(modobj);
                                _IssueContext.SaveChanges();
                                var projectlist1 = _IssueContext.MasterModules.Where(r => r.IVRMM_ModuleName.ToLower().Trim() == item.Module.ToLower().Trim() && r.Module_ActiveFlag == 1).ToList();

                                if (projectlist1.Count > 0)
                                {
                                    task.IVRMM_Id = projectlist1.FirstOrDefault().IVRMM_Id;
                                }

                            }

                        }

                        task.ISMTCR_Status = "Open";
                        //department
                        if (item.Department != "NULL" && item.Department != "" && item.Department != null)
                        {
                            var deplist = _IssueContext.HR_Master_Department.Where(r => r.HRMD_DepartmentName.ToLower().Trim() == item.Department.ToLower().Trim() && r.HRMD_ActiveFlag == true && r.MI_Id == data.MI_Id).ToList();

                            if (deplist.Count > 0)
                            {
                                task.HRMD_Id = deplist.FirstOrDefault().HRMD_Id;
                            }
                            else
                            {
                                HR_Master_Department depobj = new HR_Master_Department();

                                depobj.HRMD_DepartmentName = item.Department.Trim();
                                depobj.HRMD_ActiveFlag = true;
                                depobj.MI_Id = data.MI_Id;

                                _IssueContext.Add(depobj);
                                _IssueContext.SaveChanges();

                                var deplist1 = _IssueContext.HR_Master_Department.Where(r => r.HRMD_DepartmentName.ToLower().Trim() == item.Department.ToLower().Trim() && r.HRMD_ActiveFlag == true && r.MI_Id == data.MI_Id).ToList();

                                if (deplist1.Count > 0)
                                {
                                    task.HRMD_Id = deplist1.FirstOrDefault().HRMD_Id;
                                }


                            }

                        }

                        if (item.Priority != "NULL" && item.Priority != "" && item.Priority != null)
                        {
                            var priorlist = _IssueContext.MastersPriorities_DMO.Where(r => r.HRMP_Name.ToLower().Trim() == item.Priority.ToLower().Trim() && r.HRMP_ActiveFlag == true && r.MI_Id == data.MI_Id).ToList();

                            if (priorlist.Count > 0)
                            {
                                task.HRMPR_Id = priorlist.FirstOrDefault().HRMPR_Id;
                            }
                            else
                            {
                                var orderid = 0;
                                var orderlist = _IssueContext.MastersPriorities_DMO.Where(r => r.MI_Id == data.MI_Id).ToList();

                                if (orderlist.Count > 0)
                                {
                                    orderid = _IssueContext.MastersPriorities_DMO.Where(r => r.MI_Id == data.MI_Id).Select(f => f.HRMP_Order).Max();
                                }
                                else
                                {
                                    orderid = 0;
                                }

                                MastersPriorities_DMO obj = new MastersPriorities_DMO();
                                // obj.HRMPR_Id = data.HRMPR_Id;
                                obj.MI_Id = data.MI_Id;
                                obj.HRMP_Name = item.Priority.Trim();
                                obj.HRMP_Order = orderid + 1;
                                obj.HRMP_CreatedBy = data.UserId;
                                obj.HRMP_UpdatedBy = data.UserId;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                obj.HRMP_ActiveFlag = true;
                                _IssueContext.Add(obj);
                                _IssueContext.SaveChanges();
                                var priorlist1 = _IssueContext.MastersPriorities_DMO.Where(r => r.HRMP_Name.ToLower().Trim() == item.Priority.ToLower().Trim() && r.HRMP_ActiveFlag == true && r.MI_Id == data.MI_Id).ToList();
                                if (priorlist1.Count > 0)
                                {
                                    task.HRMPR_Id = priorlist1.FirstOrDefault().HRMPR_Id;
                                }

                            }

                        }

                        if (item.Assigned_By_empcode != "NULL" && item.Assigned_By_empcode != "" && item.Assigned_By_empcode != null)
                        {
                            var hrmEempidassignby = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.Assigned_By_empcmp.Trim()) && t.HRME_EmployeeCode.Trim() == item.Assigned_By_empcode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id })
                                .Distinct().ToArray();

                            if (hrmEempidassignby.Length > 0)
                            {
                                task.HRME_Id = hrmEempidassignby.FirstOrDefault().HRME_Id;
                            }
                        }


                        if (item.Issue_Type_Activity != "NULL" && item.Issue_Type_Activity != "" && item.Issue_Type_Activity != null)
                        {
                            if (item.Issue_Type_Activity.Contains("Bugs & Complaints"))
                            {
                                task.ISMTCR_BugOREnhancementFlg = "B";
                            }
                            else if (item.Issue_Type_Activity.Contains("Enhancement"))
                            {
                                task.ISMTCR_BugOREnhancementFlg = "E";
                            }
                            else
                            {
                                task.ISMTCR_BugOREnhancementFlg = "O";
                            }
                        }

                        task.ISMTCR_CreationDate = DateTime.Now;
                        task.ISMTCR_Title = item.Task_Title.Trim();
                        task.ISMTCR_Desc = item.Task_Description.Trim();




                        task.ISMTCR_ReOpenFlg = false;

                        task.ISMTCR_ActiveFlg = true;
                        task.ISMTCR_CreatedBy = data.UserId;
                        task.ISMTCR_UpdatedBy = data.UserId;
                        task.CreatedDate = DateTime.Now;
                        task.UpdatedDate = DateTime.Now;
                        _IssueContext.Add(task);
                        /// _IssueContext.SaveChanges();

                        if (item.Client != "NULL" && item.Client != "" && item.Client != null)
                        {
                            var clientlist = _IssueContext.clientTable.Where(r => r.ISMMCLT_ClientName.ToLower().Trim() == item.Client.ToLower().Trim() && r.ISMMCLT_ActiveFlag == true && r.MI_Id == data.MI_Id).ToList();

                            if (clientlist.Count > 0)
                            {
                                client_id = clientlist.FirstOrDefault().ISMMCLT_Id;
                            }
                            else
                            {
                                ISM_Master_Client_DMO cltobj = new ISM_Master_Client_DMO();

                                cltobj.ISMMCLT_ClientName = item.Client.Trim();
                                cltobj.ISMMCLT_ActiveFlag = true;
                                cltobj.MI_Id = data.MI_Id;

                                _IssueContext.Add(cltobj);
                                _IssueContext.SaveChanges();
                                var clientlist1 = _IssueContext.clientTable.Where(r => r.ISMMCLT_ClientName.ToLower().Trim() == item.Client.ToLower().Trim() && r.ISMMCLT_ActiveFlag == true && r.MI_Id == data.MI_Id).ToList();

                                if (clientlist1.Count > 0)
                                {
                                    client_id = clientlist1.FirstOrDefault().ISMMCLT_Id;
                                }

                            }

                            if (client_id > 0)
                            {
                                ISM_TaskCreation_ClientDMO taskclt = new ISM_TaskCreation_ClientDMO();
                                taskclt.ISMTCR_Id = task.ISMTCR_Id;
                                taskclt.ISMMCLT_Id = client_id;
                                taskclt.ISMTCRCL_ActiveFlg = true;
                                taskclt.ISMTCRCL_CreatedBy = data.UserId;
                                taskclt.ISMTCRCL_UpdatedBy = data.UserId;
                                taskclt.CreatedDate = DateTime.Now;
                                taskclt.UpdatedDate = DateTime.Now;
                                _IssueContext.Add(taskclt);
                            }

                        }



                        ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                        taskasign.ISMTCR_Id = task.ISMTCR_Id;

                        taskasign.ISMTCRASTO_AssignedDate = DateTime.Now; ;

                        if (item.Assigned_to_empcode != "NULL" && item.Assigned_to_empcode != "" && item.Assigned_to_empcode != null)
                        {
                            var hrmEempidassignto = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.Assigned_to_empcmp.Trim()) && t.HRME_EmployeeCode.Trim() == item.Assigned_to_empcode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id }).Distinct().ToArray();

                            if (hrmEempidassignto.Length > 0)
                            {
                                taskasign.HRME_Id = hrmEempidassignto.FirstOrDefault().HRME_Id;
                            }
                        }

                        taskasign.HRME_Id = taskasign.HRME_Id;
                        taskasign.ISMTCRASTO_AssignedBy = task.HRME_Id;
                        taskasign.ISMTCRASTO_Remarks = item.Remarks.Trim();

                        if (item.Effort_in_hrs != "NULL" && item.Effort_in_hrs != "" && item.Effort_in_hrs != null)
                        {
                            taskasign.ISMTCRASTO_EffortInHrs = Convert.ToInt64(item.Effort_in_hrs);
                        }
                        else
                        {
                            taskasign.ISMTCRASTO_EffortInHrs = 8;
                        }

                        string startdate = item.StartDay + "/" + item.StartMonth + "/" + item.StratYear;
                        startdate = startdate.Trim();
                        if (startdate != null && startdate != "" && startdate != "NULL")
                        {


                            if (startdate.Contains("-"))
                            {
                                //string date1 = "";
                                //string month1 = "";
                                //string year1 = "";
                                //string[] StartDateList = startdate.Trim().Split("-");

                                //for (int i = 0; i < StartDateList.Length; i++)
                                //{
                                //    if (i == 0)
                                //    {
                                //        date1 = StartDateList[0].ToString();
                                //    }
                                //    if (i == 1)
                                //    {
                                //        month1 = StartDateList[1].ToString();
                                //    }
                                //    if (i == 2)
                                //    {
                                //        year1 = StartDateList[2].ToString();
                                //    }
                                //}

                                //if (date1.Length == 1)
                                //{
                                //    date1 = "0" + date1;
                                //}
                                //if (month1.Length == 1)
                                //{
                                //    month1 = "0" + month1;
                                //}
                                //if (year1.Length == 2)
                                //{
                                //    year1 = "20" + year1;
                                //}
                                //startdate = date1 + "-" + month1 + "-" + year1;


                                DateTime DT = DateTime.ParseExact(startdate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                taskasign.ISMTCRASTO_StartDate = DT;
                            }
                            else
                            {
                                //string date1 = "";
                                //string month1 = "";
                                //string year1 = "";
                                //string[] StartDateList = startdate.Trim().Split("/");

                                //for (int i = 0; i < StartDateList.Length; i++)
                                //{
                                //    if (i == 0)
                                //    {
                                //        date1 = StartDateList[0].ToString();
                                //    }
                                //    if (i == 1)
                                //    {
                                //        month1 = StartDateList[1].ToString();
                                //    }
                                //    if (i == 2)
                                //    {
                                //        year1 = StartDateList[2].ToString();
                                //    }
                                //}

                                //if (date1.Length == 1)
                                //{
                                //    date1 = "0" + date1;
                                //}
                                //if (month1.Length == 1)
                                //{
                                //    month1 = "0" + month1;
                                //}
                                //if (year1.Length == 2)
                                //{
                                //    year1 = "20" + year1;
                                //}
                                //startdate = date1 + "/" + month1 + "/" + year1;



                                DateTime DT = DateTime.ParseExact(startdate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                taskasign.ISMTCRASTO_StartDate = DT;
                            }
                            //taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(item.Start_Date); 
                            //taskasign.ISMTCRASTO_StartDate = item.Start_Date; 
                        }
                        else
                        {
                            taskasign.ISMTCRASTO_StartDate = DateTime.Now;
                        }

                        string enddate = item.EndDay + "/" + item.EndMonth + "/" + item.EndYear;
                        enddate = enddate.Trim();

                        if (enddate != null && enddate != "" && enddate != "NULL")
                        {

                            if (enddate.Contains("-"))
                            {
                                //string date1 = "";
                                //string month1 = "";
                                //string year1 = "";
                                //string[] StartDateList = item.End_Date.Trim().Split("-");
                                //for (int i = 0; i < StartDateList.Length; i++)
                                //{
                                //    if (i == 0)
                                //    {
                                //        date1 = StartDateList[0].ToString();
                                //    }
                                //    if (i == 1)
                                //    {
                                //        month1 = StartDateList[1].ToString();
                                //    }
                                //    if (i == 2)
                                //    {
                                //        year1 = StartDateList[2].ToString();
                                //    }
                                //}

                                //if (date1.Length == 1)
                                //{
                                //    date1 = "0" + date1;
                                //}
                                //if (month1.Length == 1)
                                //{
                                //    month1 = "0" + month1;
                                //}
                                //if (year1.Length == 2)
                                //{
                                //    year1 = "20" + year1;
                                //}
                                // item.End_Date = date1 + "-" + month1 + "-" + year1;
                                DateTime DT = DateTime.ParseExact(enddate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                taskasign.ISMTCRASTO_EndDate = DT;
                            }
                            else
                            {

                                //string date1 = "";
                                //string month1 = "";
                                //string year1 = "";
                                //string[] StartDateList = item.End_Date.Trim().Split("/");
                                //for (int i = 0; i < StartDateList.Length; i++)
                                //{
                                //    if (i == 0)
                                //    {
                                //        date1 = StartDateList[0].ToString();
                                //    }
                                //    if (i == 1)
                                //    {
                                //        month1 = StartDateList[1].ToString();
                                //    }
                                //    if (i == 2)
                                //    {
                                //        year1 = StartDateList[2].ToString();
                                //    }
                                //}

                                //if (date1.Length == 1)
                                //{
                                //    date1 = "0" + date1;
                                //}
                                //if (month1.Length == 1)
                                //{
                                //    month1 = "0" + month1;
                                //}
                                //if (year1.Length == 2)
                                //{
                                //    year1 = "20" + year1;
                                //}
                                ////   item.End_Date = date1 + "/" + month1 + "/" + year1;

                                DateTime DT = DateTime.ParseExact(enddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                taskasign.ISMTCRASTO_EndDate = DT;
                            }

                        }
                        else
                        {
                            taskasign.ISMTCRASTO_EndDate = DateTime.Now; ;
                        }



                        taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                        taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                        taskasign.ISMTCRASTO_ActiveFlg = true;
                        taskasign.CreatedDate = DateTime.Now;
                        taskasign.UpdatedDate = DateTime.Now;
                        _IssueContext.Add(taskasign);

                        int rowAffected = _IssueContext.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.suscnt += 1;
                            //idd += 1; 
                        }
                        else
                        {
                            data.failcnt += 1;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            data.returnval = true;
            return data;
        }

        public ISMImportDTO saveadvance(ISMImportDTO data)
        {
            string returnduplicatestatus = "";
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            try
            {
                bool insertdata = true;

                if (data.advimppln.Length > 0)
                {
                    data.message = "";
                    var rowno = 1;
                    foreach (var item in data.advimppln)
                    {
                        try
                        {
                            rowno += 1;

                            item.StartDay = item.StartDay.Trim();
                            item.EndDay = item.EndDay.Trim();
                            item.Effort = item.Effort.Trim();
                            if (item.Effort == "" || item.Effort == "NULL")
                            {
                                item.Effort = "8";
                            }

                            for (int i = 0; i < item.StartDay.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.StartDay.Trim()[i]))
                                {
                                    data.message = "StartDay Column Field should not contain Any characters or special characters : Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.StartDay.Trim()[i]))
                                {
                                    data.message = "StartDay Column Field should not contain Any characters or special characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            if (item.Client.Trim() != "NULL" && item.Client.Trim() != "" && item.Client.Trim() != null)
                            {
                                var clientlist = _IssueContext.clientTable.Where(r => r.ISMMCLT_ClientName.ToLower().Trim() == item.Client.ToLower().Trim() && r.ISMMCLT_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode)).ToList();
                                if (clientlist.Count == 0)
                                {
                                    data.message = "Client Name " + item.Client +
                                        "Not exist, Please register Client  or select any existing Client Name: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }
                            else
                            {
                                data.message = "Please Enter Client: Row No:" + rowno;
                                insertdata = false;
                                return data;
                            }

                            if (item.Priority.Trim() != "NULL" && item.Priority.Trim() != "" && item.Priority.Trim() != null)
                            {
                                var priorlist = _IssueContext.MastersPriorities_DMO.Where(r => r.HRMP_Name.ToLower().Trim() == item.Priority.ToLower().Trim() && r.HRMP_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode)).ToList();
                                if (priorlist.Count == 0)
                                {
                                    data.message = "Priority Not exist Please Add or select any existing Priority Name: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }
                            else
                            {
                                data.message = "Please Enter Priority: Row No:" + rowno;
                                insertdata = false;
                                return data;
                            }

                            if (item.Department.Trim() != "NULL" && item.Department.Trim() != "" && item.Department.Trim() != null)
                            {
                                string departmentstring = System.Text.RegularExpressions.Regex.Replace(item.Department, @"\s+", " ");

                                var deplist = _IssueContext.HR_Master_Department.Where(r => System.Text.RegularExpressions.Regex.Replace(r.HRMD_DepartmentName.ToLower(), @"\s+", " ").Trim() == departmentstring.ToLower().Trim() && r.HRMD_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode)).ToList();
                                if (deplist.Count == 0)
                                {
                                    data.message = "Department Not exist Please Add or select any existing Department Name: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }
                            else
                            {
                                data.message = "Please Enter Department: Row No:" + rowno;
                                insertdata = false;
                                return data;
                            }

                            if (item.Project.Trim() != "NULL" && item.Project.Trim() != "" && item.Project.Trim() != null)
                            {
                                var projectlist = _IssueContext.MastersProject_DMO.Where(r => r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode) && r.ISMMPR_ProjectName.ToLower().Trim() == item.Project.ToLower().Trim() && r.ISMMPR_ActiveFlg == true).ToList();
                                if (projectlist.Count == 0)
                                {
                                    data.message = "Project Not exist Please Add or select any existing Project Name: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }
                            else
                            {
                                data.message = "Please Enter Project: Row No:" + rowno;
                                insertdata = false;
                                return data;
                            }

                            if (item.Periodicity.Trim().Equals("yearly once", StringComparison.OrdinalIgnoreCase) || item.Periodicity.Trim().Equals("Specific Day", StringComparison.OrdinalIgnoreCase))
                            {
                                for (int i = 0; i < item.YearlyTaskDay.Trim().Length; i++)
                                {
                                    if (char.IsLetter(item.YearlyTaskDay.Trim()[i]))
                                    {
                                        data.message = "YearlyTaskDay Column Field should not contain Any characters or special characters : Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                    if (!char.IsLetterOrDigit(item.YearlyTaskDay.Trim()[i]))
                                    {
                                        data.message = "YearlyTaskDay Column Field should not contain Any characters or special characters: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                }

                                if (item.YearlyTaskMonth.Length > 2 || item.YearlyTaskMonth.Length > 2)
                                {
                                    data.message = "StartMonth/EndMonth Column Field should  contain 1 to 12 : Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }

                                for (int i = 0; i < item.YearlyTaskMonth.Trim().Length; i++)
                                {
                                    if (char.IsLetter(item.YearlyTaskMonth.Trim()[i]))
                                    {
                                        data.message = "YearlyTaskMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                    if (!char.IsLetterOrDigit(item.YearlyTaskMonth.Trim()[i]))
                                    {
                                        data.message = "YearlyTaskMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }

                                }

                                for (int i = 0; i < item.YearlyTaskYear.Trim().Length; i++)
                                {
                                    if (char.IsLetter(item.YearlyTaskYear.Trim()[i]))
                                    {
                                        data.message = "YearlyTaskYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                    if (!char.IsLetterOrDigit(item.YearlyTaskYear.Trim()[i]))
                                    {
                                        data.message = "YearlyTaskYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                }
                            }

                            if (item.YearlyTaskYear.Length != 4)
                            {
                                data.message = "YearlyTaskYearr Column Field should be correct format Row " + rowno + "   " + "EXAMPLE: 2019";
                                insertdata = false;
                                return data;
                            }

                            if (item.TaskDay.Trim() != "NULL" && item.TaskDay.Trim() != "" && item.TaskDay.Trim() != null)
                            {
                                for (int i = 0; i < item.TaskDay.Trim().Length; i++)
                                {
                                    if (char.IsLetter(item.TaskDay.Trim()[i]))
                                    {
                                        data.message = "TaskDay Column Field should not contain Any characters or special characters : Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                    if (!char.IsLetterOrDigit(item.TaskDay.Trim()[i]))
                                    {
                                        data.message = "TaskDay Column Field should not contain Any characters or special characters: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                }
                            }

                            if (item.Periodicity.ToLower().Trim() == "weekly")
                            {
                                if (item.TaskDay.Trim() == "NULL" || item.TaskDay.Trim() == "" || item.TaskDay.Trim() == null)
                                {
                                    data.message = "TaskDay  Field Required(Enter 1 -6): Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                else
                                {
                                    if (Convert.ToInt64(item.TaskDay) == 0 || Convert.ToInt64(item.TaskDay) > 6)
                                    {
                                        data.message = "Enter TaskDay  between 1 -6: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                }
                            }

                            if (item.Periodicity.ToLower().Trim() == "monthly" || item.Periodicity.ToLower().Trim() == "once in fortnight")
                            {
                                if (item.TaskDay == "NULL" || item.TaskDay == "" || item.TaskDay == null)
                                {
                                    data.message = "TaskDay  Field Required(Enter 1 -31): Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                else
                                {
                                    if (Convert.ToInt64(item.TaskDay) == 0 || Convert.ToInt64(item.TaskDay) > 31)
                                    {
                                        data.message = "Enter TaskDay  between 1 -31: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                }
                            }

                            if (item.Module.Trim() != "NULL" && item.Module.Trim() != "" && item.Module.Trim() != null)
                            {
                                var modulelist = _IssueContext.MasterModules.Where(r => r.IVRMM_ModuleName.ToLower().Trim() == item.Module.ToLower().Trim() && r.Module_ActiveFlag == 1).ToList();
                                if (modulelist.Count == 0)
                                {
                                    data.message = "Module Not exist Please register module or select any existing module : Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            if (item.Category.Trim() != "NULL" && item.Category.Trim() != "" && item.Category.Trim() != null)
                            {
                                string departmentstring = System.Text.RegularExpressions.Regex.Replace(item.Department, @"\s+", " ");
                                var deplist = _IssueContext.HR_Master_Department.Where(r => System.Text.RegularExpressions.Regex.Replace(r.HRMD_DepartmentName.ToLower(), @"\s+", " ").Trim() == departmentstring.ToLower().Trim() && r.HRMD_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode)).ToList();

                                var catelist = _IssueContext.ISM_Master_TaskCategory_DMO.Where(r => r.ISMMTCAT_TaskCategoryName.ToLower().Trim() == item.Category.ToLower().Trim() && r.ISMMTCAT_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode) && r.HRMD_Id == deplist.FirstOrDefault().HRMD_Id).ToList();
                                if (catelist.Count == 0)
                                {
                                    data.message = "Category Not exist Please register Category or select any existing Category : Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            if (item.Category.Trim() == "NULL" || item.Category.Trim() == "" || item.Category.Trim() == null)
                            {
                                data.message = "Please Enter  Category : Row No:" + rowno;
                                insertdata = false;
                                return data;
                            }

                            //if (item.Periodicity.Trim() == "NULL" || item.Periodicity.Trim() == "" || item.Periodicity.Trim() == null)
                            //{
                            //    data.message = "Invalid Periodicity : Row No:" + rowno;
                            //    insertdata = false;
                            //    return data;
                            //}
                            //else
                            //{
                            //    if (item.Periodicity.ToLower().Trim() == "specific day" || item.Periodicity.ToLower().Trim() == "daily" || item.Periodicity.ToLower().Trim() == "weekly" || item.Periodicity.ToLower().Trim() == "once in fortnight" || item.Periodicity.ToLower().Trim() == "monthly" || item.Periodicity.ToLower().Trim() == "yearly once" || item.Periodicity.ToLower().Trim() == "once")
                            //    {
                            //    }
                            //    else
                            //    {
                            //        data.message = "Invalid Periodicity : Row No:" + rowno;
                            //        insertdata = false;
                            //        return data;
                            //    }
                            //}

                            if (item.EffortType.Trim() == "NULL" || item.EffortType.Trim() == "" || item.EffortType.Trim() == null)
                            {
                                data.message = "Invalid Effort Type : Row No:" + rowno;
                                insertdata = false;
                                return data;
                            }
                            else
                            {
                                if (item.EffortType.ToLower().Trim() == "days" || item.EffortType.ToLower().Trim() == "hours" || item.EffortType.ToLower().Trim() == "mins")
                                {
                                }
                                else
                                {
                                    data.message = "Invalid Effort Type : Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            for (int i = 0; i < item.EndDay.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.EndDay.Trim()[i]))
                                {
                                    data.message = "EndDay Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.EndDay.Trim()[i]))
                                {
                                    data.message = "EndDay Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            for (int i = 0; i < item.Effort.Trim().Length; i++)
                            {
                                string numString = item.Effort.Trim();
                                long number1 = 0;
                                bool canConvert = long.TryParse(numString, out number1);
                                if (canConvert == false)
                                {
                                    decimal number3 = 0;
                                    canConvert = decimal.TryParse(numString, out number3);
                                    if (canConvert == false)
                                    {
                                        data.message = "Effort Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                        insertdata = false;
                                        return data;
                                    }
                                }

                                //if (char.IsLetter(item.Effort.Trim()[i]))
                                //{
                                //    data.message = "Effort Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                //    insertdata = false;
                                //    return data;
                                //}
                                //if (!char.IsLetterOrDigit(item.Effort.Trim()[i]))
                                //{
                                //    data.message = "Effort Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                //    insertdata = false;
                                //    return data;
                                //}
                            }

                            if (item.StartDay.Length > 2 || item.EndDay.Length > 2)
                            {
                                data.message = "StartDay/Endday Column Field should  contain 1 to 31 based on Month: Row No:" + rowno;
                                insertdata = false;
                                return data;
                            }

                            if (item.StartMonth.Length > 2 || item.EndMonth.Length > 2)
                            {
                                data.message = "StartMonth/EndMonth Column Field should  contain 1 to 12 : Row No:" + rowno;
                                insertdata = false;
                                return data;
                            }

                            for (int i = 0; i < item.StartMonth.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.StartMonth.Trim()[i]))
                                {
                                    data.message = "StartMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.StartMonth.Trim()[i]))
                                {
                                    data.message = "StartMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            for (int i = 0; i < item.EndMonth.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.EndMonth.Trim()[i]))
                                {
                                    data.message = "EndMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.EndMonth.Trim()[i]))
                                {
                                    data.message = "EndMonth Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            for (int i = 0; i < item.StartYear.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.StartYear.Trim()[i]))
                                {
                                    data.message = "StratYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.StartYear.Trim()[i]))
                                {
                                    data.message = "StratYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            for (int i = 0; i < item.AssignedByCompanyCode.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.AssignedByCompanyCode.Trim()[i]))
                                {
                                    data.message = "AssignedByCompanyCode Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.AssignedByCompanyCode.Trim()[i]))
                                {
                                    data.message = "AssignedByCompanyCode Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            for (int i = 0; i < item.AssignedToCompanyCode.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.AssignedToCompanyCode.Trim()[i]))
                                {
                                    data.message = "AssignedToCompanyCode Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.AssignedToCompanyCode.Trim()[i]))
                                {
                                    data.message = "AssignedToCompanyCode Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            for (int i = 0; i < item.EndYear.Trim().Length; i++)
                            {
                                if (char.IsLetter(item.EndYear.Trim()[i]))
                                {
                                    data.message = "EndYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                                if (!char.IsLetterOrDigit(item.EndYear.Trim()[i]))
                                {
                                    data.message = "EndYear Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                    insertdata = false;
                                    return data;
                                }
                            }

                            if (item.StartYear.Length != 4 || item.EndYear.Length != 4)
                            {
                                data.message = "StratYear/EndYear Column Field should be correct format Row " + rowno + "   " + "EXAMPLE: 2019";
                                insertdata = false;
                                return data;
                            }

                            //long smonth = Convert.ToInt64(item.StartMonth);
                            //long monthday = _Context.month.Single(w => w.IVRM_Month_Id == smonth).IVRM_Month_Max_Days;
                            //long sdate = Convert.ToInt64(item.StartDay);
                            //if (sdate > monthday)
                            //{
                            //    data.message = "StratDay is exceeding Month End Day at the Row " + rowno;
                            //    insertdata = false;
                            //    return data;
                            //}
                            //long emonth = Convert.ToInt64(item.EndMonth);
                            //monthday = _Context.month.Single(w => w.IVRM_Month_Id == smonth).IVRM_Month_Max_Days;
                            //long edate = Convert.ToInt64(item.EndDay);
                            //if (edate > monthday)
                            //{
                            //    data.message = "EndDay is exceeding Month End Day at the Row " + rowno;
                            //    insertdata = false;
                            //    return data;
                            //}

                            //if (edate <= 0 || sdate <= 0)
                            //{
                            //    data.message = "StartDay/EndDay should be greater than ZERO at the Row " + rowno;
                            //    insertdata = false;
                            //    return data;
                            //}

                            if (item.StartDay.Length == 1)
                            {
                                item.StartDay = "0" + item.StartDay;
                            }
                            if (item.EndDay.Length == 1)
                            {
                                item.EndDay = "0" + item.EndDay;
                            }

                            if (item.StartMonth.Length == 1)
                            {
                                item.StartMonth = "0" + item.StartMonth;
                            }
                            if (item.EndMonth.Length == 1)
                            {
                                item.EndMonth = "0" + item.EndMonth;
                            }

                            //ADDED june 5 2020
                            DateTime Stdate = indianTime;
                            DateTime Eddate = indianTime;
                            string startdat = item.StartDay + "/" + item.StartMonth + "/" + item.StartYear;
                            startdat = startdat.Trim();
                            if (startdat != null && startdat != "" && startdat != "NULL")
                            {
                                if (startdat.Contains("-"))
                                {
                                    DateTime DT = DateTime.ParseExact(startdat.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    Stdate = DT;
                                }
                                else
                                {
                                    DateTime DT = DateTime.ParseExact(startdat.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    Stdate = DT;

                                }
                            }

                            string enddat = item.EndDay + "/" + item.EndMonth + "/" + item.EndYear;
                            enddat = enddat.Trim();

                            if (enddat != null && enddat != "" && enddat != "NULL")
                            {
                                if (enddat.Contains("-"))
                                {
                                    DateTime DT = DateTime.ParseExact(enddat.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    Eddate = DT;
                                }
                                else
                                {
                                    DateTime DT = DateTime.ParseExact(enddat.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    Eddate = DT;
                                }
                            }


                            if (Stdate > Eddate)
                            {
                                data.message = "Task End Date Should be Greater Or Equal to Task Start Date" + rowno;
                                insertdata = false;
                                return data;
                            }



                            var hrmEempidassignbych = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.AssignedByCompanyCode.Trim()) && t.HRME_EmployeeCode.Trim() == item.AssignedByEmployeeCode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id })
                                   .Distinct().ToList();
                            if (hrmEempidassignbych.Count == 0)
                            {
                                data.message = "AssignedByemployeecompany and  AssignedByemployeecode is missmatching(Employee does not belongs to company code  " + item.AssignedByCompanyCode + " OR Employee deactivated) at the Row " + rowno;
                                insertdata = false;
                                return data;
                            }

                            //var hrmEempidassigntoch = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode.Trim()) && t.HRME_EmployeeCode.Trim() == item.AssignedToEmployeeCode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id }).Distinct().ToList();
                            //if (hrmEempidassigntoch.Count == 0)
                            //{
                            //    data.message = "AssignedToCompanyCode and  AssignedToemployeecode is missmatching(Employee does not belongs to company code  " + item.AssignedToCompanyCode + " OR Employee deactivated) at the Row " + rowno;
                            //    insertdata = false;
                            //    return data;
                            //}

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(rowno);
                        }
                    }

                    if (data.advimppln.Length > 0 && insertdata == true)
                    {
                        long countarry = 1;

                        foreach (var item in data.advimppln)
                        {
                            countarry += 1;

                            long client_id = 0;
                            var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                            long empid = 0;
                            if (hrmEempid.Length > 0)
                            {
                                empid = hrmEempid.FirstOrDefault().Emp_Code;
                            }

                            Master_NumberingDTO check = new Master_NumberingDTO();
                            data.transnumbconfigurationsettingsss = check;
                            List<Master_Numbering> MM = new List<Master_Numbering>();
                            MM = _Context.Master_Numbering.Where(t => t.MI_Id == Convert.ToInt64(item.AssignedByCompanyCode) && t.IMN_Flag == "ISMTASK").ToList();
                            if (MM.Count() > 0)
                            {
                                data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                                data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                                data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                                data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                                data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                                data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                                data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                                data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                                data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                                data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                                data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                                data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                                data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                                data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                                data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                                data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                                data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                            }

                            if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                            {
                                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Context);
                                data.transnumbconfigurationsettingsss.MI_Id = Convert.ToInt64(item.AssignedByCompanyCode);
                                data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                                data.Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                            }


                            ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                            task.ISMTCR_TaskNo = data.Auto_id;
                            task.MI_Id = Convert.ToInt64(item.AssignedByCompanyCode);

                            if (item.Project != "NULL" && item.Project != "" && item.Project != null)
                            {
                                var projectlist = _IssueContext.MastersProject_DMO.Where(r => r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode) && r.ISMMPR_ProjectName.ToLower().Trim() == item.Project.ToLower().Trim() && r.ISMMPR_ActiveFlg == true).ToList();

                                if (projectlist.Count > 0)
                                {
                                    task.ISMMPR_Id = projectlist.FirstOrDefault().ISMMPR_Id;
                                }
                            }

                            if (item.Module != "NULL" && item.Module != "" && item.Module != null)
                            {
                                var projectlist = _IssueContext.MasterModules.Where(r => r.IVRMM_ModuleName.ToLower().Trim() == item.Module.ToLower().Trim() && r.Module_ActiveFlag == 1).ToList();

                                if (projectlist.Count > 0)
                                {
                                    task.IVRMM_Id = projectlist.FirstOrDefault().IVRMM_Id;
                                }
                            }

                            task.ISMTCR_Status = "Open";

                            if (item.Department != "NULL" && item.Department != "" && item.Department != null)
                            {
                                string departmentstring = System.Text.RegularExpressions.Regex.Replace(item.Department, @"\s+", " ");

                                var deplist = _IssueContext.HR_Master_Department.Where(r => System.Text.RegularExpressions.Regex.Replace(r.HRMD_DepartmentName.ToLower(), @"\s+", " ").Trim() == departmentstring.ToLower().Trim() && r.HRMD_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode)).ToList();

                                if (deplist.Count > 0)
                                {
                                    task.HRMD_Id = deplist.FirstOrDefault().HRMD_Id;
                                }
                            }

                            if (item.Priority != "NULL" && item.Priority != "" && item.Priority != null)
                            {
                                var priorlist = _IssueContext.MastersPriorities_DMO.Where(r => r.HRMP_Name.ToLower().Trim() == item.Priority.ToLower().Trim() && r.HRMP_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode)).ToList();

                                if (priorlist.Count > 0)
                                {
                                    task.HRMPR_Id = priorlist.FirstOrDefault().HRMPR_Id;
                                }
                            }

                            if (item.AssignedByEmployeeCode != "NULL" && item.AssignedByEmployeeCode != "" && item.AssignedByEmployeeCode != null)
                            {
                                var hrmEempidassignby = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.AssignedByCompanyCode.Trim()) && t.HRME_EmployeeCode.Trim() == item.AssignedByEmployeeCode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id })
                                    .Distinct().ToArray();

                                if (hrmEempidassignby.Length > 0)
                                {
                                    task.HRME_Id = hrmEempidassignby.FirstOrDefault().HRME_Id;
                                }
                            }

                            if (item.IssueType != "NULL" && item.IssueType != "" && item.IssueType != null)
                            {
                                if (item.IssueType.Contains("Bugs & Complaints"))
                                {
                                    task.ISMTCR_BugOREnhancementFlg = "B";
                                }
                                else if (item.IssueType.Contains("Enhancement"))
                                {
                                    task.ISMTCR_BugOREnhancementFlg = "E";
                                }
                                else
                                {
                                    task.ISMTCR_BugOREnhancementFlg = "O";
                                }
                            }

                            if (item.Category != "NULL" && item.Category != "" && item.Category != null)
                            {
                                string departmentstring = System.Text.RegularExpressions.Regex.Replace(item.Department, @"\s+", " ");
                                var deplist = _IssueContext.HR_Master_Department.Where(r => System.Text.RegularExpressions.Regex.Replace(r.HRMD_DepartmentName.ToLower(), @"\s+", " ").Trim() == departmentstring.ToLower().Trim() && r.HRMD_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode)).ToList();

                                var catelist = _IssueContext.ISM_Master_TaskCategory_DMO.Where(r => r.ISMMTCAT_TaskCategoryName.ToLower().Trim() == item.Category.ToLower().Trim() && r.ISMMTCAT_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode) && r.HRMD_Id == deplist.FirstOrDefault().HRMD_Id).ToList();
                                if (catelist.Count > 0)
                                {
                                    task.ISMMTCAT_Id = catelist.FirstOrDefault().ISMMTCAT_Id;
                                }
                            }

                            task.ISMTCR_CreationDate = indianTime;
                            task.ISMTCR_Title = item.TaskTitle.Trim();
                            task.ISMTCR_Desc = item.TaskDescription.Trim();
                            task.ISMTCR_ReOpenFlg = false;
                            task.ISMTCR_ActiveFlg = true;
                            task.ISMTCR_CreatedBy = data.UserId;
                            task.ISMTCR_UpdatedBy = data.UserId;
                            task.CreatedDate = indianTime;
                            task.UpdatedDate = indianTime;

                            long assignemp = 0;
                            DateTime Stdate = indianTime;
                            DateTime Eddate = indianTime;
                            var Assigntocheck = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode.Trim()) && t.HRME_EmployeeCode.Trim() == item.AssignedToEmployeeCode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id }).Distinct().ToArray();

                            if (Assigntocheck.Length > 0)
                            {
                                assignemp = Convert.ToInt64(Assigntocheck.FirstOrDefault().HRME_Id);
                            }

                            string startdat = item.StartDay + "/" + item.StartMonth + "/" + item.StartYear;
                            startdat = startdat.Trim();
                            if (startdat != null && startdat != "" && startdat != "NULL")
                            {
                                if (startdat.Contains("-"))
                                {
                                    DateTime DT = DateTime.ParseExact(startdat.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    Stdate = DT;
                                }
                                else
                                {
                                    DateTime DT = DateTime.ParseExact(startdat.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    Stdate = DT;

                                }
                            }

                            string enddat = item.EndDay + "/" + item.EndMonth + "/" + item.EndYear;
                            enddat = enddat.Trim();

                            if (enddat != null && enddat != "" && enddat != "NULL")
                            {
                                if (enddat.Contains("-"))
                                {
                                    DateTime DT = DateTime.ParseExact(enddat.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    Eddate = DT;
                                }
                                else
                                {
                                    DateTime DT = DateTime.ParseExact(enddat.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    Eddate = DT;
                                }
                            }

                            var plannerlist = _IssueContext.ISM_Master_Implementation_PlannerDMO.Where(r => r.MI_Id == Convert.ToInt64(item.AssignedByCompanyCode.ToLower().Trim()) && r.ISMMIMPPL_ActiveFlag == true).OrderByDescending(t => t.ISMMIMPPL_Id).ToList();
                            if (plannerlist.Count > 0)
                            {
                                task.ISMIMPPL_Id = plannerlist[0].ISMMIMPPL_Id;
                            }

                            List<ISMImportDTO> Duplicatetask = new List<ISMImportDTO>();

                            if ((item.TaskDay.Trim() == "NULL" || item.TaskDay.Trim() == "" || item.TaskDay.Trim() == null) && (!item.Periodicity.Trim().Equals("yearly once", StringComparison.OrdinalIgnoreCase) && !item.Periodicity.Trim().Equals("Specific Day", StringComparison.OrdinalIgnoreCase)))
                            {
                                Duplicatetask = (from r in _IssueContext.ISM_TaskCreationDMO
                                                 from B in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                 where (r.ISMTCR_Id == B.ISMTCR_Id && r.MI_Id == task.MI_Id && r.ISMMPR_Id == task.ISMMPR_Id && r.IVRMM_Id == task.IVRMM_Id && r.HRMD_Id == task.HRMD_Id && r.HRME_Id == task.HRME_Id && r.ISMMTCAT_Id == task.ISMMTCAT_Id && r.ISMTCR_BugOREnhancementFlg == task.ISMTCR_BugOREnhancementFlg && r.ISMTCR_Title == task.ISMTCR_Title && B.HRME_Id == assignemp && B.ISMTCRASTO_StartDate == Stdate && B.ISMTCRASTO_EndDate == Eddate && r.ISMTCR_Desc == task.ISMTCR_Desc)
                                                 select new ISMImportDTO
                                                 {
                                                     ISMTCR_Id = r.ISMTCR_Id
                                                 }).ToList();
                            }
                            else
                            {
                                if (item.Periodicity.ToLower().Trim() == "monthly" || item.Periodicity.ToLower().Trim() == "once in fortnight" || item.Periodicity.ToLower().Trim() == "weekly" && (!item.Periodicity.Trim().Equals("yearly once", StringComparison.OrdinalIgnoreCase) && !item.Periodicity.Trim().Equals("Specific Day", StringComparison.OrdinalIgnoreCase)))
                                {
                                    Duplicatetask = (from r in _IssueContext.ISM_TaskCreationDMO
                                                     from B in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                     from C in _IssueContext.ISM_Task_Advance_PlannerDMO
                                                     where (r.ISMTCR_Id == B.ISMTCR_Id && r.MI_Id == task.MI_Id && r.ISMMPR_Id == task.ISMMPR_Id && r.IVRMM_Id == task.IVRMM_Id && r.ISMTCR_Id == C.ISMTCR_Id && r.HRMD_Id == task.HRMD_Id && r.HRME_Id == task.HRME_Id && r.ISMMTCAT_Id == task.ISMMTCAT_Id && r.ISMTCR_BugOREnhancementFlg == task.ISMTCR_BugOREnhancementFlg && r.ISMTCR_Title == task.ISMTCR_Title && B.HRME_Id == assignemp && B.ISMTCRASTO_StartDate == Stdate && B.ISMTCRASTO_EndDate == Eddate && C.ISMTAPL_Periodicity == item.Periodicity.Trim() && C.ISMTAPL_Day == Convert.ToInt64(item.TaskDay.Trim()) && r.ISMTCR_Desc == task.ISMTCR_Desc)
                                                     select new ISMImportDTO
                                                     {
                                                         ISMTCR_Id = r.ISMTCR_Id
                                                     }).ToList();
                                }
                            }

                            if (Duplicatetask.Count == 0 && assignemp == 0)
                            {
                                var Duplicatetasktemp = (from r in _IssueContext.ISM_TaskCreationDMO
                                                         where (r.MI_Id == task.MI_Id && r.ISMMPR_Id == task.ISMMPR_Id && r.IVRMM_Id == task.IVRMM_Id && r.HRMD_Id == task.HRMD_Id && r.HRME_Id == task.HRME_Id && r.ISMMTCAT_Id == task.ISMMTCAT_Id && r.ISMTCR_BugOREnhancementFlg == task.ISMTCR_BugOREnhancementFlg && r.ISMTCR_Title == task.ISMTCR_Title && r.ISMTCR_Desc == task.ISMTCR_Desc)
                                                         select new ISMImportDTO
                                                         {
                                                             ISMTCR_Id = r.ISMTCR_Id
                                                         }).ToList();

                                List<long> transferto = new List<long>();
                                foreach (var at in Duplicatetasktemp)
                                {
                                    transferto.Add(at.ISMTCR_Id);
                                }

                                Duplicatetask = (from r in _IssueContext.ISM_TaskCreationDMO
                                                 from B in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                 where (r.ISMTCR_Id == B.ISMTCR_Id && transferto.Contains(B.ISMTCR_Id))
                                                 select new ISMImportDTO
                                                 {
                                                     ISMTCR_Id = r.ISMTCR_Id
                                                 }).ToList();
                                if (Duplicatetask.Count == 0)
                                {
                                    Duplicatetask = Duplicatetasktemp;
                                }
                                else
                                {
                                    if (Duplicatetask.Count == transferto.Count)
                                    {
                                        Duplicatetask.Clear();
                                    }
                                }
                            }
                            //data.returnduplicatestatus = "Duplicate Rows:";
                            if (Duplicatetask.Count == 0)
                            {
                                _IssueContext.Add(task);
                            }
                            else
                            {
                                data.duplicate += 1;
                                if (returnduplicatestatus == null || returnduplicatestatus == "")
                                {
                                    returnduplicatestatus = countarry.ToString();
                                }
                                else
                                {
                                    returnduplicatestatus = returnduplicatestatus + "," + countarry;
                                }

                                continue;
                            }
                            /// _IssueContext.SaveChanges();

                            if (item.Client != "NULL" && item.Client != "" && item.Client != null)
                            {
                                var clientlist = _IssueContext.clientTable.Where(r => r.ISMMCLT_ClientName.ToLower().Trim() == item.Client.ToLower().Trim() && r.ISMMCLT_ActiveFlag == true && r.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode)).ToList();

                                if (clientlist.Count > 0)
                                {
                                    client_id = clientlist.FirstOrDefault().ISMMCLT_Id;
                                }
                                task.ISMMCLT_Id = client_id;
                            }

                            ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                            ISM_Task_Advance_PlannerDMO adv = new ISM_Task_Advance_PlannerDMO();
                            ISM_Task_Advance_Planner_DatesDMO datedmo = new ISM_Task_Advance_Planner_DatesDMO();
                            ISM_TaskCreation_ClientDMO uptaskclt = new ISM_TaskCreation_ClientDMO();


                            taskasign.ISMTCR_Id = task.ISMTCR_Id;
                            adv.ISMTCR_Id = task.ISMTCR_Id;
                            uptaskclt.ISMTCR_Id = task.ISMTCR_Id;
                            taskasign.ISMTCRASTO_AssignedDate = indianTime;
                            if (item.AssignedToEmployeeCode != "NULL" && item.AssignedToEmployeeCode != "" && item.AssignedToEmployeeCode != null)
                            {
                                var hrmEempidassignto = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == Convert.ToInt64(item.AssignedToCompanyCode.Trim()) && t.HRME_EmployeeCode.Trim() == item.AssignedToEmployeeCode.Trim() && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Select(d => new ISMImportDTO { HRME_Id = d.HRME_Id }).Distinct().ToArray();

                                if (hrmEempidassignto.Length > 0)
                                {
                                    taskasign.HRME_Id = hrmEempidassignto.FirstOrDefault().HRME_Id;
                                    adv.HRME_Id = hrmEempidassignto.FirstOrDefault().HRME_Id;

                                }
                            }

                            taskasign.HRME_Id = taskasign.HRME_Id;
                            taskasign.ISMTCRASTO_AssignedBy = task.HRME_Id;
                            taskasign.ISMTCRASTO_Remarks = item.Remarks.Trim();
                            adv.ISMTAPL_Remarks = item.Remarks.Trim();
                            adv.MI_Id = Convert.ToInt64(item.AssignedToCompanyCode.Trim());
                            adv.ISMTAPL_Year = data.ASMAY_Id;

                            uptaskclt.ISMMCLT_Id = client_id;
                            uptaskclt.ISMTCRCL_ActiveFlg = true;
                            uptaskclt.ISMTCRCL_CreatedBy = data.UserId;
                            uptaskclt.ISMTCRCL_UpdatedBy = data.UserId;
                            uptaskclt.CreatedDate = indianTime;
                            uptaskclt.UpdatedDate = indianTime;


                            if (item.Effort != "NULL" && item.Effort != "" && item.Effort != null)
                            {
                                if (item.EffortType.ToLower().Trim() == "mins" || item.EffortType.ToLower().Trim() == "minutes" || item.EffortType.ToLower().Trim() == "minute")
                                {
                                    TimeSpan spWorkMin = TimeSpan.FromMinutes(Convert.ToInt64(item.Effort));
                                    string workHours = spWorkMin.ToString(@"hh\:mm");

                                    taskasign.ISMTCRASTO_EffortInHrs = Convert.ToDecimal(spWorkMin.TotalHours);
                                    adv.ISMTAPL_TimeRequired = Convert.ToDecimal(item.Effort);
                                }
                                else if (item.EffortType.ToLower().Trim() == "days" || item.EffortType.ToLower().Trim() == "day")
                                {
                                    taskasign.ISMTCRASTO_EffortInHrs = Convert.ToDecimal(item.Effort) * 8;
                                    adv.ISMTAPL_TimeRequired = Convert.ToDecimal(item.Effort);
                                }
                                else
                                {
                                    taskasign.ISMTCRASTO_EffortInHrs = Convert.ToDecimal(item.Effort);
                                    adv.ISMTAPL_TimeRequired = Convert.ToDecimal(item.Effort);
                                }

                            }
                            else
                            {
                                taskasign.ISMTCRASTO_EffortInHrs = 8;
                                adv.ISMTAPL_TimeRequired = 8;
                            }

                            string startdate = item.StartDay + "/" + item.StartMonth + "/" + item.StartYear;
                            startdate = startdate.Trim();
                            if (startdate != null && startdate != "" && startdate != "NULL")
                            {
                                if (startdate.Contains("-"))
                                {
                                    DateTime DT = DateTime.ParseExact(startdate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    taskasign.ISMTCRASTO_StartDate = DT;
                                    adv.ISMTAPL_FromDate = DT;
                                    datedmo.ISMTAPLD_StartDate = DT;
                                }
                                else
                                {
                                    DateTime DT = DateTime.ParseExact(startdate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    taskasign.ISMTCRASTO_StartDate = DT;
                                    adv.ISMTAPL_FromDate = DT;
                                    datedmo.ISMTAPLD_StartDate = DT;
                                }
                            }
                            else
                            {
                                taskasign.ISMTCRASTO_StartDate = indianTime;
                                adv.ISMTAPL_FromDate = indianTime;
                                datedmo.ISMTAPLD_StartDate = indianTime;
                            }

                            string enddate = item.EndDay + "/" + item.EndMonth + "/" + item.EndYear;
                            enddate = enddate.Trim();

                            if (enddate != null && enddate != "" && enddate != "NULL")
                            {
                                if (enddate.Contains("-"))
                                {
                                    DateTime DT = DateTime.ParseExact(enddate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    taskasign.ISMTCRASTO_EndDate = DT;
                                    adv.ISMTAPL_ToDate = DT;
                                    datedmo.ISMTAPLD_EndDate = DT;
                                }
                                else
                                {
                                    DateTime DT = DateTime.ParseExact(enddate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                    taskasign.ISMTCRASTO_EndDate = DT;
                                    adv.ISMTAPL_ToDate = DT;
                                    datedmo.ISMTAPLD_EndDate = DT;
                                }
                            }
                            else
                            {
                                taskasign.ISMTCRASTO_EndDate = indianTime;
                                adv.ISMTAPL_ToDate = indianTime;
                                datedmo.ISMTAPLD_EndDate = indianTime;
                            }

                            taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                            taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                            taskasign.ISMTCRASTO_ActiveFlg = true;
                            taskasign.CreatedDate = indianTime;
                            taskasign.UpdatedDate = indianTime;
                            datedmo.ISMTAPLD_CreatedBy = data.UserId;
                            datedmo.ISMTAPLD_UpdatedBy = data.UserId;
                            datedmo.ISMTAPLD_ActiveFlg = true;
                            datedmo.ISMTAPLD_CreatedDate = indianTime;
                            datedmo.ISMTAPLD_UpdatedDate = indianTime;
                            adv.ISMTAPL_Periodicity = item.Periodicity.Trim();
                            adv.ISMTAPL_TimeRequiredFlg = item.EffortType.Trim();
                            adv.ISMTAPL_CreatedBy = data.UserId;
                            adv.ISMTAPL_UpdatedBy = data.UserId;
                            adv.ISMTAPL_ActiveFlg = true;
                            adv.CreatedDate = indianTime;
                            adv.UpdatedDate = indianTime;
                            if (item.Periodicity.ToLower().Trim() == "monthly" || item.Periodicity.ToLower().Trim() == "once in fortnight" || item.Periodicity.ToLower().Trim() == "weekly")
                            {
                                adv.ISMTAPL_Day = Convert.ToInt64(item.TaskDay.Trim());
                            }

                            if (item.Periodicity.Trim().Equals("yearly once", StringComparison.OrdinalIgnoreCase) || item.Periodicity.Trim().Equals("Specific Day", StringComparison.OrdinalIgnoreCase))
                            {
                                string yeardate = item.YearlyTaskDay + "/" + item.YearlyTaskMonth + "/" + item.YearlyTaskYear;
                                startdate = yeardate.Trim();
                                if (yeardate != null && yeardate != "" && yeardate != "NULL")
                                {
                                    string date1 = "";
                                    string month1 = "";
                                    string year1 = "";
                                    string[] StartDateList = yeardate.Trim().Split("/");

                                    for (int i = 0; i < StartDateList.Length; i++)
                                    {
                                        if (i == 0)
                                        {
                                            date1 = StartDateList[0].ToString();
                                        }
                                        if (i == 1)
                                        {
                                            month1 = StartDateList[1].ToString();
                                        }
                                        if (i == 2)
                                        {
                                            year1 = StartDateList[2].ToString();
                                        }
                                    }

                                    if (date1.Length == 1)
                                    {
                                        date1 = "0" + date1;
                                    }
                                    if (month1.Length == 1)
                                    {
                                        month1 = "0" + month1;
                                    }
                                    if (year1.Length == 2)
                                    {
                                        year1 = "20" + year1;
                                    }
                                    yeardate = date1 + "/" + month1 + "/" + year1;
                                    DateTime DT = DateTime.ParseExact(yeardate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    adv.ISMTAPL_OFFDate = DT;
                                }
                            }
                            if (item.Periodicity.Trim().Equals("yearly once", StringComparison.OrdinalIgnoreCase) || item.Periodicity.Trim().Equals("Specific Day", StringComparison.OrdinalIgnoreCase))
                            {
                                Duplicatetask = (from r in _IssueContext.ISM_TaskCreationDMO
                                                 from B in _IssueContext.ISM_TaskCreation_AssignedToDMO
                                                 from c in _IssueContext.ISM_Task_Advance_PlannerDMO
                                                 where (r.ISMTCR_Id == B.ISMTCR_Id && r.MI_Id == task.MI_Id && r.ISMMPR_Id == task.ISMMPR_Id && r.IVRMM_Id == task.IVRMM_Id && c.ISMTCR_Id == r.ISMTCR_Id && c.ISMTCR_Id == B.ISMTCR_Id && r.HRMD_Id == task.HRMD_Id && r.HRME_Id == task.HRME_Id && r.ISMMTCAT_Id == task.ISMMTCAT_Id && r.ISMTCR_BugOREnhancementFlg == task.ISMTCR_BugOREnhancementFlg && r.ISMTCR_Title == task.ISMTCR_Title && B.HRME_Id == assignemp && B.ISMTCRASTO_StartDate == Stdate && B.ISMTCRASTO_EndDate == Eddate && r.ISMTCR_Desc == task.ISMTCR_Desc && c.ISMTAPL_OFFDate == adv.ISMTAPL_OFFDate)
                                                 select new ISMImportDTO
                                                 {
                                                     ISMTCR_Id = r.ISMTCR_Id
                                                 }).ToList();
                            }
                            var dup = false;
                            if (Duplicatetask.Count > 0)
                            {
                                data.duplicate += 1;
                                if (returnduplicatestatus == null || returnduplicatestatus == "")
                                {
                                    returnduplicatestatus = countarry.ToString();
                                }
                                else
                                {
                                    returnduplicatestatus = returnduplicatestatus + "," + countarry;
                                }
                                dup = true;
                            }
                            if (dup == false)
                            {
                                if (item.AssignedToEmployeeCode.Trim() != "NULL")
                                {
                                    _IssueContext.Add(taskasign);
                                }
                                _IssueContext.Add(uptaskclt);
                                if (item.Periodicity.Trim() != "NULL" && item.AssignedToEmployeeCode.Trim() != "NULL")
                                {
                                    _IssueContext.Add(adv);
                                    datedmo.ISMTAPL_Id = adv.ISMTAPL_Id;
                                    _IssueContext.Add(datedmo);
                                }
                                int rowAffected = _IssueContext.SaveChanges();
                                if (rowAffected > 0)
                                {
                                    data.suscnt += 1;
                                }
                                else
                                {
                                    data.failcnt += 1;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            data.returnduplicatestatus = returnduplicatestatus;
            data.returnval = true;
            return data;
        }
        public ISMImportDTO save_adv_planner_task(ISMImportDTO data)
        {
            string returnduplicatestatus = "";
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            try
            {
                bool insertdata = true;


                data.message = "";
                var rowno = 1;



                // long countarry = 1;


                //  countarry += 1;

                long client_id = 0;
                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
                long empid = 0;
                if (hrmEempid.Length > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }

                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _Context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "ISMTASK").ToList();
                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                //---------------------------------------------------------------------------------------

                if (data.MulttaskDepartments != null && data.MulttaskDepartments.Length > 0)
                {
                    var empHRMEid = _Context.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
                    int count = 0;
                    long emp_hrme_new = 0;
                    // foreach (var d in data.MulttaskDepartments)
                    for (int i = 0; i < data.MulttaskDepartments.Length; i++)
                    {
                        var d = data.MulttaskDepartments[i];
                        count++;
                        if (count > 1)
                        {

                            emp_hrme_new = data.MulttaskDepartments[i - 1].HRME_Id;
                        }
                        else
                        {

                            emp_hrme_new = empHRMEid[0].Emp_Code;
                        }

                        ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                        task.ISMTCR_TaskNo = data.Auto_id;
                        task.MI_Id = data.MI_Id;
                        task.ISMMPR_Id = data.ISMMPR_Id;
                        task.IVRMM_Id = data.IVRMM_Id;
                        task.ISMTCR_Status = "Open";
                        task.HRMD_Id = d.TTEDDP_HRMDCID;
                        task.HRMPR_Id = data.HRMPR_Id;
                        //task.HRME_Id = d.HRME_Id;
                        task.HRME_Id = emp_hrme_new;
                        task.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                        task.ISMMTCAT_Id = data.ISMMTCAT_Id;
                        task.ISMTCR_CreationDate = indianTime;
                        task.ISMTCR_Title = data.ISMTCR_Title;
                        task.ISMTCR_Desc = data.ISMTCR_Desc;
                        task.ISMTCR_ReOpenFlg = false;
                        task.ISMTCR_ActiveFlg = true;
                        task.ISMTCR_CreatedBy = data.UserId;
                        task.ISMTCR_UpdatedBy = data.UserId;
                        task.CreatedDate = indianTime;
                        task.UpdatedDate = indianTime;
                        task.ISMTCR_MainGroupTaskFlg = null;
                        task.ISMMCLT_Id = data.ISMMCLT_Id;

                        var plannerlist = _IssueContext.ISM_Master_Implementation_PlannerDMO.Where(r => r.MI_Id == data.MI_Id && r.ISMMIMPPL_ActiveFlag == true).OrderByDescending(t => t.ISMMIMPPL_Id).ToList();
                        if (plannerlist.Count > 0)
                        {
                            task.ISMIMPPL_Id = plannerlist[0].ISMMIMPPL_Id;
                        }
                        _IssueContext.Add(task);
                        data.ISMTCR_Id = task.ISMTCR_Id;

                        DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(d.TTEDDP_FromDate), INDIAN_ZONE);
                        DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(d.TTEDDP_ToDate), INDIAN_ZONE);

                        string confromdate = "";
                        string contodate = "";
                        try
                        {
                            confromdate = fromdatecon.ToString("yyyy-MM-dd");
                            contodate = todatecon.ToString("yyyy-MM-dd");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }



                        ISM_Task_Advance_PlannerDMO adv = new ISM_Task_Advance_PlannerDMO();
                        adv.ISMTCR_Id = task.ISMTCR_Id;
                        adv.ISMTAPL_Remarks = data.remarks;
                        adv.MI_Id = data.MI_Id;
                        adv.HRME_Id = d.HRME_Id;
                        adv.ISMTAPL_Year = data.ASMAY_Id;
                        adv.ISMTAPL_TimeRequired = Convert.ToInt64(d.TTEDDP_Hours);
                        adv.ISMTAPL_FromDate = Convert.ToDateTime(confromdate);
                        adv.ISMTAPL_ToDate = Convert.ToDateTime(contodate);
                        adv.ISMTAPL_Periodicity = "Daily";
                        adv.ISMTAPL_TimeRequiredFlg = d.TimeRequiredFlg.Trim();
                        adv.ISMTAPL_CreatedBy = data.UserId;
                        adv.ISMTAPL_UpdatedBy = data.UserId;
                        adv.ISMTAPL_ActiveFlg = true;
                        adv.CreatedDate = DateTime.Now;
                        adv.UpdatedDate = DateTime.Now;
                        adv.ISMTAPL_OFFDate = data.Yearlydate;
                        //adv.ISMTAPL_ApprovalFlag = 0;
                        adv.ISMTAPL_Day = Convert.ToInt64(data.TaskDay);
                        _IssueContext.Add(adv);


                        ISM_Task_Advance_Planner_DatesDMO datedmo = new ISM_Task_Advance_Planner_DatesDMO();
                        datedmo.ISMTAPL_Id = adv.ISMTAPL_Id;
                        datedmo.ISMTAPLD_StartDate = Convert.ToDateTime(confromdate);
                        datedmo.ISMTAPLD_EndDate = Convert.ToDateTime(contodate);
                        datedmo.ISMTAPLD_CreatedBy = data.UserId;
                        datedmo.ISMTAPLD_UpdatedBy = data.UserId;
                        datedmo.ISMTAPLD_ActiveFlg = true;
                        datedmo.ISMTAPLD_CreatedDate = DateTime.Now;
                        datedmo.ISMTAPLD_UpdatedDate = DateTime.Now;
                        _IssueContext.Add(datedmo);



                        ISM_TaskEnhancement_DeptwiseDMO dep = new ISM_TaskEnhancement_DeptwiseDMO();
                        dep.MI_ID = data.MI_Id;
                        dep.HRMD_Id = d.TTEDDP_HRMDCID;
                        dep.TTEDDP_Percentage = d.TTEDDP_Percentage;
                        dep.ISMTCR_Id = data.ISMTCR_Id;
                        dep.ISMTCR_TaskNo = data.Auto_id;
                        dep.TTEDDP_Remarks = d.TTEDDP_Remarks;
                        dep.TTEDDP_CreatedBy = data.UserId;
                        dep.TTEDDP_UpdatedBy = data.UserId;
                        dep.CreatedDate = indianTime;
                        dep.UpdatedDate = indianTime;
                        dep.TTEDDP_Days = d.TTEDDP_Days;
                        dep.TTEDDP_level = d.TTEDDP_level;
                        dep.HRME_Id = d.HRME_Id;
                        dep.TTEDDP_ToDate = d.TTEDDP_ToDate;
                        dep.TTEDDP_FromDate = d.TTEDDP_FromDate;
                        dep.TTEDDP_Hours = d.TTEDDP_Hours;

                        _IssueContext.Add(dep);

                        ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                        taskasign.ISMTCR_Id = data.ISMTCR_Id;
                        taskasign.HRME_Id = d.HRME_Id;
                        taskasign.ISMTCRASTO_AssignedDate = indianTime;
                        taskasign.ISMTCRASTO_AssignedBy = emp_hrme_new;
                        taskasign.ISMTCRASTO_Remarks = data.remarks;
                        taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(confromdate);
                        taskasign.ISMTCRASTO_EndDate = Convert.ToDateTime(contodate);
                        taskasign.ISMTCRASTO_EffortInHrs = Convert.ToInt64(d.TTEDDP_Hours);
                        taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                        taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                        taskasign.ISMTCRASTO_TimeRequiredFlg = d.TimeRequiredFlg;
                        taskasign.ISMTCRASTO_ActiveFlg = true;
                        taskasign.CreatedDate = indianTime;
                        taskasign.UpdatedDate = indianTime;
                        _IssueContext.Add(taskasign);

                        // _IssueContext.SaveChanges();

                    }
                    if (data.ISMMCLT_Id > 0)
                    {

                        ISM_TaskCreation_ClientDMO uptaskclt = new ISM_TaskCreation_ClientDMO();
                        uptaskclt.ISMTCR_Id = data.ISMTCR_Id;
                        uptaskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                        uptaskclt.ISMTCRCL_ActiveFlg = true;
                        uptaskclt.ISMTCRCL_CreatedBy = data.UserId;
                        uptaskclt.ISMTCRCL_UpdatedBy = data.UserId;
                        uptaskclt.CreatedDate = indianTime;
                        uptaskclt.UpdatedDate = indianTime;
                        _IssueContext.Add(uptaskclt);

                    }
                    if (data.attachmentArray != null)
                    {
                        foreach (var file in data.attachmentArray)
                        {
                            if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                            {

                                ISM_TaskCreation_AttachmentDMO att = new ISM_TaskCreation_AttachmentDMO();
                                att.ISMTCR_Id = data.ISMTCR_Id;
                                att.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                att.ISMTCRAT_File = file.ISMTCRAT_File;
                                att.ISMTCRAT_ActiveFlg = true;
                                att.ISMTCRAT_CreatedBy = data.UserId;
                                att.ISMTCRAT_UpdatedBy = data.UserId;
                                att.CreatedDate = indianTime;
                                att.UpdatedDate = indianTime;
                                _IssueContext.Add(att);

                            }

                        }
                    }
                }

                else
                {

                    ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                    task.ISMTCR_TaskNo = data.Auto_id;
                    task.MI_Id = data.MI_Id;
                    task.ISMMPR_Id = data.ISMMPR_Id;
                    task.IVRMM_Id = data.IVRMM_Id;
                    task.ISMTCR_Status = "Open";
                    task.HRMD_Id = data.HRMD_Id;
                    task.HRMPR_Id = data.HRMPR_Id;
                    //task.HRME_Id = data.HRME_Id;
                    task.HRME_Id = empid;
                    task.ISMTCR_BugOREnhancementFlg = data.ISMTCR_BugOREnhancementFlg;
                    task.ISMMTCAT_Id = data.ISMMTCAT_Id;
                    task.ISMTCR_CreationDate = indianTime;
                    task.ISMTCR_Title = data.ISMTCR_Title;
                    task.ISMTCR_Desc = data.ISMTCR_Desc;
                    task.ISMTCR_ReOpenFlg = false;
                    task.ISMTCR_ActiveFlg = true;
                    task.ISMTCR_CreatedBy = data.UserId;
                    task.ISMTCR_UpdatedBy = data.UserId;
                    task.CreatedDate = indianTime;
                    task.UpdatedDate = indianTime;
                    task.ISMTCR_MainGroupTaskFlg = null;
                    task.ISMMCLT_Id = data.ISMMCLT_Id;


                    var plannerlist = _IssueContext.ISM_Master_Implementation_PlannerDMO.Where(r => r.MI_Id == data.MI_Id && r.ISMMIMPPL_ActiveFlag == true).OrderByDescending(t => t.ISMMIMPPL_Id).ToList();
                    if (plannerlist.Count > 0)
                    {
                        task.ISMIMPPL_Id = plannerlist[0].ISMMIMPPL_Id;
                    }

                    _IssueContext.Add(task);
                    data.ISMTCR_Id = task.ISMTCR_Id;

                    //---------------------------------------------------
                    if (data.taskEmpArray.Length > 0)
                    {
                        foreach (var te in data.taskEmpArray)
                        {

                            DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.startdate), INDIAN_ZONE);
                            DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.enddate), INDIAN_ZONE);

                            string confromdate = "";
                            string contodate = "";
                            try
                            {
                                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                contodate = todatecon.ToString("yyyy-MM-dd");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            var empHRMEid = _Context.Staff_User_Login.Where(t => t.Id == data.UserId).ToList();
                            long employeeid = 0;
                            var depart_id = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empHRMEid[0].Emp_Code).ToList();

                            var departmenthead = (from a in _IssueContext.HR_Master_Department
                                                  from b in _IssueContext.HR_Master_DepartmentCode_Head
                                                  from c in _IssueContext.Staff_User_Login
                                                  where (a.HRMD_Id == depart_id[0].HRMD_Id && a.HRMDC_ID == b.HRMDC_ID && b.HRME_ID == c.Emp_Code && a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                                  select new ISM_TaskCreationDTO
                                                  {
                                                      HRME_Id = b.HRME_ID,
                                                      UserId = c.Id
                                                  }).ToList();




                            ISM_Task_Advance_PlannerDMO adv = new ISM_Task_Advance_PlannerDMO();
                            adv.ISMTCR_Id = data.ISMTCR_Id;
                            adv.ISMTAPL_Remarks = data.remarks;
                            adv.MI_Id = data.MI_Id;
                            adv.HRME_Id = te.HRME_Id;
                            adv.ISMTAPL_Year = data.ASMAY_Id;
                            adv.ISMTAPL_TimeRequired = data.effortinhrs;
                            adv.ISMTAPL_FromDate = Convert.ToDateTime(confromdate);
                            adv.ISMTAPL_ToDate = Convert.ToDateTime(contodate);
                            adv.ISMTAPL_Periodicity = data.periodicity.Trim();
                            adv.ISMTAPL_TimeRequiredFlg = data.TimeRequiredFlg.Trim();
                            adv.ISMTAPL_CreatedBy = data.UserId;
                            adv.ISMTAPL_UpdatedBy = data.UserId;
                            adv.ISMTAPL_ActiveFlg = true;
                            adv.CreatedDate = DateTime.Now;
                            adv.UpdatedDate = DateTime.Now;
                            adv.ISMTAPL_OFFDate = data.Yearlydate;

                           // adv.ISMTAPL_ApprovalFlag = 0;

                            adv.ISMTAPL_ApprovalFlag = null;

                            adv.ISMTAPL_Day = Convert.ToInt64(data.TaskDay);
                            _IssueContext.Add(adv);

                            ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                            taskasign.ISMTCR_Id = data.ISMTCR_Id;
                            taskasign.HRME_Id = te.HRME_Id;
                            taskasign.ISMTCRASTO_AssignedDate = indianTime;
                            taskasign.ISMTCRASTO_AssignedBy = employeeid = empid;
                            taskasign.ISMTCRASTO_Remarks = data.remarks;
                            taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(confromdate);
                            taskasign.ISMTCRASTO_EndDate = Convert.ToDateTime(contodate);
                            taskasign.ISMTCRASTO_EffortInHrs = data.effortinhrs;
                            taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                            taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                            taskasign.ISMTCRASTO_TimeRequiredFlg = data.TimeRequiredFlg;
                            taskasign.ISMTCRASTO_ActiveFlg = true;
                            taskasign.CreatedDate = indianTime;
                            taskasign.UpdatedDate = indianTime;
                            _IssueContext.Add(taskasign);

                            ISM_Task_Advance_Planner_DatesDMO datedmo = new ISM_Task_Advance_Planner_DatesDMO();
                            datedmo.ISMTAPL_Id = adv.ISMTAPL_Id;
                            datedmo.ISMTAPLD_StartDate = Convert.ToDateTime(confromdate);
                            datedmo.ISMTAPLD_EndDate = Convert.ToDateTime(contodate);
                            datedmo.ISMTAPLD_CreatedBy = data.UserId;
                            datedmo.ISMTAPLD_UpdatedBy = data.UserId;
                            datedmo.ISMTAPLD_ActiveFlg = true;
                            datedmo.ISMTAPLD_CreatedDate = DateTime.Now;
                            datedmo.ISMTAPLD_UpdatedDate = DateTime.Now;
                            _IssueContext.Add(datedmo);



                            // _IssueContext.SaveChanges();


                        }


                    }
                    if (data.ISMMCLT_Id > 0)
                    {

                        ISM_TaskCreation_ClientDMO uptaskclt = new ISM_TaskCreation_ClientDMO();
                        uptaskclt.ISMTCR_Id = data.ISMTCR_Id;
                        uptaskclt.ISMMCLT_Id = data.ISMMCLT_Id;
                        uptaskclt.ISMTCRCL_ActiveFlg = true;
                        uptaskclt.ISMTCRCL_CreatedBy = data.UserId;
                        uptaskclt.ISMTCRCL_UpdatedBy = data.UserId;
                        uptaskclt.CreatedDate = indianTime;
                        uptaskclt.UpdatedDate = indianTime;
                        _IssueContext.Add(uptaskclt);

                    }
                    if (data.attachmentArray != null)
                    {
                        foreach (var file in data.attachmentArray)
                        {
                            if (file.ISMTCRAT_Attatchment != null && file.ISMTCRAT_Attatchment != "")
                            {

                                ISM_TaskCreation_AttachmentDMO att = new ISM_TaskCreation_AttachmentDMO();
                                att.ISMTCR_Id = data.ISMTCR_Id;
                                att.ISMTCRAT_Attatchment = file.ISMTCRAT_Attatchment;
                                att.ISMTCRAT_File = file.ISMTCRAT_File;
                                att.ISMTCRAT_ActiveFlg = true;
                                att.ISMTCRAT_CreatedBy = data.UserId;
                                att.ISMTCRAT_UpdatedBy = data.UserId;
                                att.CreatedDate = indianTime;
                                att.UpdatedDate = indianTime;
                                _IssueContext.Add(att);

                            }

                        }
                    }
                }

                int rowAffected = _IssueContext.SaveChanges();

                if (rowAffected > 0)
                {
                    data.returnval = true;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //data.returnduplicatestatus = returnduplicatestatus;
            // data.returnval = true;
            return data;
        }
        public ISMImportDTO saveIP(ISMImportDTO data)
        {
            try
            {
                // long idd = 2587;

                //var a = "aa";
                //var b = "10 h";

                if (data.plannercontent.Length > 0)
                {
                    long moduleid = 0;
                    data.message = "";
                    var rowno = 1;
                    foreach (var item in data.plannercontent)
                    {
                        ///check the validation for start day and end day
                        ///
                        rowno += 1;


                        item.TimeDuration = item.TimeDuration.Trim();
                        if (item.TimeDuration == "" || item.TimeDuration == "NULL")
                        {
                            item.TimeDuration = "1";
                        }

                        for (int i = 0; i < item.TimeDuration.Trim().Length; i++)
                        {
                            if (char.IsLetter(item.TimeDuration.Trim()[i]))
                            {
                                data.message = "TimeDuration Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }
                            if (!char.IsLetterOrDigit(item.TimeDuration.Trim()[i]))
                            {
                                data.message = "TimeDuration Column Field should not contain Any characters or spcecial characters: Row No:" + rowno;
                                return data;
                            }

                        }

                        if (item.Periodicity.ToLower().Trim() == "specific day" || item.Periodicity.ToLower().Trim() == "daily" || item.Periodicity.ToLower().Trim() == "weekly" || item.Periodicity.ToLower().Trim() == "once in fortnight" || item.Periodicity.ToLower().Trim() == "monthly" || item.Periodicity.ToLower().Trim() == "yearly once" || item.Periodicity.ToLower().Trim() == "once")
                        {

                        }
                        else
                        {
                            data.message = "Please Enter ony ane of these Periodicity--Specific Day,Daily,Weekly,Once in Fortnight,MonthlyYearly Once,Monthly,Yearly Once,Once..: Row No:" + rowno;
                            return data;
                        }

                        if (item.DurationType.ToLower().Trim() == "days" || item.DurationType.ToLower().Trim() == "hours" || item.DurationType.ToLower().Trim() == "mins")
                        {

                        }
                        else
                        {
                            data.message = "Please Enter ony ane of these DurationType--Days,Hours,Mins ..: Row No:" + rowno;
                            return data;
                        }

                        if (item.Module.ToLower().Trim() != "null" && item.Module.ToLower().Trim() != "")
                        {
                            var modulelist = _IssueContext.MasterModules.Where(r => r.IVRMM_ModuleName.ToLower().Trim() == item.Module.ToLower().Trim() && r.Module_ActiveFlag == 1).Distinct().ToList();
                            if (modulelist.Count > 0)
                            {
                                moduleid = modulelist[0].IVRMM_Id;
                            }
                            else
                            {
                                data.message = "Please Enter Correct Module Name: Row No:" + rowno;
                                return data;
                            }
                        }


                        var plannerlist = _IssueContext.ISM_Master_Implementation_PlannerDMO.Where(r => r.ISMMIMPPL_PlannerName.ToLower().Trim() == item.PlannerName.ToLower().Trim() && r.ISMMIMPPL_ActiveFlag == true).Distinct().ToList();
                        if (plannerlist.Count > 0)
                        {

                        }
                        else
                        {
                            data.message = "Please Enter Correct Planner Name";
                            return data;
                        }



                    }


                }






                if (data.plannercontent.Length > 0)
                {
                    long moduleid = 0;
                    long planneridd = 0;
                    foreach (var item in data.plannercontent)
                    {
                        ISM_Implementation_PlannerDMO taskasign = new ISM_Implementation_PlannerDMO();

                        if (item.Module.ToLower().Trim() != "null" && item.Module.ToLower().Trim() != "")
                        {
                            var modulelist = _IssueContext.MasterModules.Where(r => r.IVRMM_ModuleName.ToLower().Trim() == item.Module.ToLower().Trim() && r.Module_ActiveFlag == 1).Distinct().ToList();
                            if (modulelist.Count > 0)
                            {
                                moduleid = modulelist[0].IVRMM_Id;
                            }
                            else
                            {
                                data.message = "Please Enter Correct Module Name";
                                return data;
                            }
                        }
                        if (item.PlannerName.ToLower().Trim() != "null" && item.PlannerName.ToLower().Trim() != "")
                        {
                            var plannerlist = _IssueContext.ISM_Master_Implementation_PlannerDMO.Where(r => r.ISMMIMPPL_PlannerName.ToLower().Trim() == item.PlannerName.ToLower().Trim() && r.ISMMIMPPL_ActiveFlag == true).Distinct().ToList();
                            if (plannerlist.Count > 0)
                            {
                                planneridd = plannerlist[0].ISMMIMPPL_Id;
                            }
                            else
                            {
                                data.message = "Please Enter Correct Planner Name";
                                return data;
                            }
                        }


                        taskasign.MI_Id = data.MI_Id;
                        taskasign.ISMIMPPL_ActivityName = item.Activity.Trim();
                        taskasign.ISMIMPPL_Periodicity = item.Periodicity.Trim();
                        taskasign.ISMIMPPL_Remarks = item.Remark.Trim();
                        if (item.Module.ToLower().Trim() != "null" && item.Module.ToLower().Trim() != "")
                        {
                            taskasign.IVRMM_Id = moduleid;
                        }
                        taskasign.ISMIMPPL_Remarks = item.Remark.Trim();
                        taskasign.ISMMIMPPL_Id = planneridd;
                        taskasign.ISMIMPPL_TimeRequiredFlg = item.DurationType.Trim();
                        taskasign.ISMIMPPL_TimeRequired = Convert.ToInt64(item.TimeDuration.Trim());
                        taskasign.ISMIMPPL_CreatedBy = data.UserId;
                        taskasign.ISMIMPPL_UpdatedBy = data.UserId;
                        taskasign.ISMIMPPL_ActiveFlg = true;
                        taskasign.ISMIMPPL_CreatedDate = DateTime.Now;
                        taskasign.ISMIMPPL_UpdatedDate = DateTime.Now;
                        _IssueContext.Add(taskasign);

                        int rowAffected = _IssueContext.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.suscnt += 1;
                            //idd += 1; 
                        }
                        else
                        {
                            data.failcnt += 1;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            data.returnval = true;
            return data;
        }

        public ISMImportDTO swaptask_list(ISMImportDTO data)
        {
            var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
            long empid = 0;
            if (hrmEempid.Length > 0)
            {
                empid = hrmEempid.FirstOrDefault().Emp_Code;
            }
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Yearly_Planner_Task_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Falg", SqlDbType.VarChar)
                    {
                        Value = "onload"
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
                        data.getyearplantasklist = retObject.ToArray();
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

            public ISMImportDTO OnloadApproval(ISMImportDTO data)
        {
            try
            {
                  using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Names";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        data.get_employeelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                  //institution list
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Institution";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        data.get_taskmodule = retObject.ToArray();
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


        public ISMImportDTO getyearplannerreport(ISMImportDTO data)
        {
            try
            {

                string confromdate = data.Fromdate.ToString("yyyy-MM-dd");
                string contodate = data.Todate.ToString("yyyy-MM-dd");


                var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();

                long empid = 0;

                if (hrmEempid.Length > 0)

                {

                    empid = hrmEempid.FirstOrDefault().Emp_Code;

                }


                //TOTAL TASKS LIST FOR APRROVE OR REJECT
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Task_List_Year_Planner_Total";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMTAPL_Year", SqlDbType.BigInt) { Value = 143 });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.Date) { Value = contodate });
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
                        data.get_plannerdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //CATEGORY WISE EFFORT CREATED AND ASSIGNED BY THEMSELF
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "ISM_Task_List_Year_Planner_Total_Sum";
                    cmd.CommandText = "ISM_YP_Createassignbyme";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMTAPL_Year", SqlDbType.BigInt) { Value = 143 });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.Date) { Value = contodate });
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
                        data.get_effortdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //CATEGORY WISE EFFORT ASSIGNED TO ME FROM OTHERS
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "ISM_Task_List_Year_Planner_Total_Sum";
                    cmd.CommandText = "ISM_YP_assigntoothers";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = data.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMTAPL_Year", SqlDbType.BigInt) { Value = 143 });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.Date) { Value = contodate });
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
                        data.get_taskdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //DEPT CATEGORY WISE EFFORT OVER ALL EFFORT
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "ISM_Task_List_Year_Planner_Total_Sum";
                    //cmd.CommandText = "ISM_YP_DeptWise";
                    cmd.CommandText = "ISM_YP_DeptWise_Emp_Total";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt) { Value = data.HRMD_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMTAPL_Year", SqlDbType.BigInt) { Value = 143 });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.Date) { Value = contodate });

                }
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Yearly_Planner_Task_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = empid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Falg", SqlDbType.VarChar)
                    {
                        Value = ""
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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);

                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );

                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_extratasklist = retObject.ToArray();
                        data.getyearplangridtasklist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // DEPT ALL EMP CATEGORY WISE EFFORT
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "ISM_Task_List_Year_Planner_Total_Sum";
                    cmd.CommandText = "ISM_YP_DeptWise";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.BigInt) { Value = data.HRMD_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMTAPL_Year", SqlDbType.BigInt) { Value = 143 });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.Date) { Value = contodate });
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
                        data.get_Assignedtasklist = retObject.ToArray();
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

        public ISMImportDTO yearplannerapp(ISMImportDTO dto)
        {
            try
            {
                if (dto.yearplannerapproval.Length > 0)
                {
                    foreach (var x in dto.yearplannerapproval)
                    {
                        var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("Task_Advance_Planner_Approval @p0,@p1,@p2",
                        x.ISMTAPL_Id,dto.approvalflg,dto.UserId);
                        if (contactExistsP > 0)
                        {
                            dto.message = "saved";
                        }
                        else
                        {
                            dto.message = "failed";
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

        public ISMImportDTO department(ISMImportDTO dto)
        {
            try
            {

                //TO GET DEPARTMENT LIST
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                   
                    cmd.CommandText = "Department";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                   
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
                        dto.get_department = retObject.ToArray();
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

        public ISMImportDTO Depemplist(ISMImportDTO dto)
        {
            try
            {

                //DEPT WISE EMP LIST
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "HR_Employee_MI_dept";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id", SqlDbType.Int) { Value = dto.HRMD_Id });

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
                        dto.get_employeelist = retObject.ToArray();
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


        public ISMImportDTO Depemptasklist(ISMImportDTO dto)
        {
            try
            {
                string confromdate = dto.Fromdate.ToString("yyyy-MM-dd");
                string contodate = dto.Todate.ToString("yyyy-MM-dd");

                //DEPT WISE EMP OVERALL TASK LIST
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "ISM_Task_List_Year_Planner_Total";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = dto.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMTAPL_Year", SqlDbType.BigInt) { Value = 143 });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.Date) { Value = contodate });

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
                        dto.get_taskproject = retObject.ToArray();
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




        public ISMImportDTO swaptask_list_save(ISMImportDTO data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            var hrmEempid = _Context.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToArray();
            long empid = 0;
            if (hrmEempid.Length > 0)
            {
                empid = hrmEempid.FirstOrDefault().Emp_Code;
            }
            try
            {
                if (data.swaptasklist.Length > 0)
                {
                    int saved_count = 0;
                    foreach (var s_task in data.swaptasklist)
                    {

                        Master_NumberingDTO check = new Master_NumberingDTO();
                        data.transnumbconfigurationsettingsss = check;
                        List<Master_Numbering> MM = new List<Master_Numbering>();
                        MM = _Context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "ISMTASK").ToList();
                        if (MM.Count() > 0)
                        {
                            data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                            data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                            data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                            data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                            data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                            data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                            data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                            data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                            data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                            data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                            data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                            data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                            data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                            data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                            data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                            data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                            data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                        }

                        if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Context);
                            data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                            data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                            data.Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                        }

                        var check_match_task = (from TC in _Context.ISM_TaskCreationDMO
                                                join TCA in _Context.ISM_TaskCreation_AssignedToDMO
                                                on new { TC.ISMTCR_Id, TC.HRME_Id } equals new { TCA.ISMTCR_Id, TCA.HRME_Id }
                                                join AP in _Context.ISM_Task_Advance_PlannerDMO
                                                on new { TC.ISMTCR_Id, TC.HRME_Id } equals new { AP.ISMTCR_Id, AP.HRME_Id }
                                                where TC.ISMTCR_Id == s_task.ISMTCR_Id
                                                select new ISM_DailyReportGenerationDTO
                                                {

                                                    ISMTCR_Id = TC.ISMTCR_Id,

                                                }).Distinct().Take(10).ToList();

                        if (check_match_task.Count > 0)
                        {
                            var previous_task = _Context.ISM_TaskCreationDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTCR_Id == s_task.ISMTCR_Id).Distinct().ToArray();
                            if (previous_task.Length > 0)
                            {
                                ISM_TaskCreationDMO task = new ISM_TaskCreationDMO();
                                task.ISMTCR_TaskNo = data.Auto_id;
                                task.MI_Id = data.MI_Id;
                                task.ISMMPR_Id = previous_task[0].HRMPR_Id;
                                task.IVRMM_Id = previous_task[0].IVRMM_Id;
                                task.ISMTCR_Status = "Open";
                                task.HRMD_Id = previous_task[0].HRMD_Id;
                                task.HRMPR_Id = previous_task[0].HRMPR_Id;
                                task.HRME_Id = previous_task[0].HRME_Id;
                                task.ISMTCR_BugOREnhancementFlg = previous_task[0].ISMTCR_BugOREnhancementFlg;
                                task.ISMMTCAT_Id = previous_task[0].ISMMTCAT_Id;
                                task.ISMTCR_CreationDate = indianTime;
                                task.ISMTCR_Title = previous_task[0].ISMTCR_Title;
                                task.ISMTCR_Desc = previous_task[0].ISMTCR_Desc;
                                task.ISMTCR_ReOpenFlg = false;
                                task.ISMTCR_ActiveFlg = true;
                                task.ISMTCR_CreatedBy = data.UserId;
                                task.ISMTCR_UpdatedBy = data.UserId;
                                task.CreatedDate = indianTime;
                                task.UpdatedDate = indianTime;
                                task.ISMTCR_MainGroupTaskFlg = false;
                                task.ISMMCLT_Id = previous_task[0].ISMMCLT_Id;

                                var plannerlist = _IssueContext.ISM_Master_Implementation_PlannerDMO.Where(r => r.MI_Id == data.MI_Id && r.ISMMIMPPL_ActiveFlag == true).OrderByDescending(t => t.ISMMIMPPL_Id).ToList();
                                if (plannerlist.Count > 0)
                                {
                                    task.ISMIMPPL_Id = plannerlist[0].ISMMIMPPL_Id;
                                    data.ISMMIMPPL_Id = plannerlist[0].ISMMIMPPL_Id;
                                }
                                _IssueContext.Add(task);
                                data.ISMTCR_Id = task.ISMTCR_Id;

                            }

                            // var previous_Task_Advance = _Context.ISM_Task_Advance_PlannerDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTAPL_Id == s_task.ISMTAPL_Id).Distinct().ToArray();
                            var previous_Task_Advance = _Context.ISM_Task_Advance_PlannerDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMTAPL_Id == s_task.ISMTAPL_Id).Select(t => new
                            {
                                t.ISMTAPL_Id,
                                t.MI_Id,
                                t.HRME_Id,
                                t.ISMTAPL_Periodicity,
                                t.ISMTAPL_Remarks,
                                t.ISMTAPL_ActiveFlg,
                                t.ISMTAPL_CreatedBy,
                                t.ISMTAPL_UpdatedBy,
                                t.ISMTCR_Id,
                                t.ISMTAPL_Year,
                                t.ISMTAPL_FromDate,
                                t.ISMTAPL_ToDate,
                                t.ISMTAPL_TimeRequired,
                                t.ISMTAPL_OFFDate,
                                ISMTAPL_Day = Convert.ToInt64(t.ISMTAPL_Day), // Ensuring conversion to long
                                t.ISMTAPL_TimeRequiredFlg
                            }).Distinct().ToArray();




                            if (previous_Task_Advance.Length > 0)
                            {
                                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(s_task.swap_start_date), INDIAN_ZONE);
                                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(s_task.swap_end_date), INDIAN_ZONE);

                                ISM_Task_Advance_PlannerDMO adv = new ISM_Task_Advance_PlannerDMO();
                                adv.ISMTCR_Id = data.ISMTCR_Id;
                                adv.ISMTAPL_Remarks = previous_Task_Advance[0].ISMTAPL_Remarks;
                                adv.MI_Id = data.MI_Id;
                                adv.HRME_Id = previous_Task_Advance[0].HRME_Id;
                                adv.ISMTAPL_Year = data.ASMAY_Id;
                                adv.ISMTAPL_TimeRequired = previous_Task_Advance[0].ISMTAPL_TimeRequired;
                                adv.ISMTAPL_FromDate = Convert.ToDateTime(fromdatecon);
                                adv.ISMTAPL_ToDate = Convert.ToDateTime(todatecon);
                                adv.ISMTAPL_Periodicity = previous_Task_Advance[0].ISMTAPL_Periodicity;
                                adv.ISMTAPL_TimeRequiredFlg = previous_Task_Advance[0].ISMTAPL_TimeRequiredFlg;
                                adv.ISMTAPL_CreatedBy = data.UserId;
                                adv.ISMTAPL_UpdatedBy = data.UserId;
                                adv.ISMTAPL_ActiveFlg = true;
                                adv.CreatedDate = DateTime.Now;
                                adv.UpdatedDate = DateTime.Now;
                                adv.ISMTAPL_OFFDate = previous_Task_Advance[0].ISMTAPL_OFFDate;
                                adv.ISMTAPL_Day = Convert.ToInt64(previous_Task_Advance[0].ISMTAPL_Day);
                                _IssueContext.Add(adv);
                                data.ISMTAPL_Id = adv.ISMTAPL_Id;
                            }

                            var previous_Advance_Planner = _Context.ISM_Task_Advance_Planner_DatesDMO.Where(t => t.ISMTAPL_Id == s_task.ISMTAPL_Id).Distinct().ToArray();
                            if (previous_Advance_Planner.Length > 0)
                            {
                                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(s_task.swap_start_date), INDIAN_ZONE);
                                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(s_task.swap_end_date), INDIAN_ZONE);

                                ISM_Task_Advance_Planner_DatesDMO datedmo = new ISM_Task_Advance_Planner_DatesDMO();
                                datedmo.ISMTAPL_Id = data.ISMTAPL_Id;
                                datedmo.ISMTAPLD_StartDate = Convert.ToDateTime(fromdatecon);
                                datedmo.ISMTAPLD_EndDate = Convert.ToDateTime(todatecon);
                                datedmo.ISMTAPLD_CreatedBy = data.UserId;
                                datedmo.ISMTAPLD_UpdatedBy = data.UserId;
                                datedmo.ISMTAPLD_ActiveFlg = true;
                                datedmo.ISMTAPLD_CreatedDate = DateTime.Now;
                                datedmo.ISMTAPLD_UpdatedDate = DateTime.Now;
                                _IssueContext.Add(datedmo);
                            }

                            var previous_TaskCreation_Assigned = _Context.ISM_TaskCreation_AssignedToDMO.Where(t => t.ISMTCR_Id == s_task.ISMTCR_Id).Distinct().ToArray();

                            if (previous_TaskCreation_Assigned.Length > 0)
                            {
                                DateTime fromdatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(s_task.swap_start_date), INDIAN_ZONE);
                                DateTime todatecon = TimeZoneInfo.ConvertTime(Convert.ToDateTime(s_task.swap_end_date), INDIAN_ZONE);

                                ISM_TaskCreation_AssignedToDMO taskasign = new ISM_TaskCreation_AssignedToDMO();
                                taskasign.ISMTCR_Id = data.ISMTCR_Id;
                                taskasign.HRME_Id = previous_TaskCreation_Assigned[0].HRME_Id;
                                taskasign.ISMTCRASTO_AssignedDate = indianTime;
                                taskasign.ISMTCRASTO_AssignedBy = previous_TaskCreation_Assigned[0].ISMTCRASTO_AssignedBy;
                                taskasign.ISMTCRASTO_Remarks = previous_TaskCreation_Assigned[0].ISMTCRASTO_Remarks;
                                taskasign.ISMTCRASTO_StartDate = Convert.ToDateTime(fromdatecon);
                                taskasign.ISMTCRASTO_EndDate = Convert.ToDateTime(todatecon);
                                taskasign.ISMTCRASTO_EffortInHrs = previous_TaskCreation_Assigned[0].ISMTCRASTO_EffortInHrs;
                                taskasign.ISMTCRASTO_CreatedBy = data.UserId;
                                taskasign.ISMTCRASTO_UpdatedBy = data.UserId;
                                taskasign.ISMTCRASTO_TimeRequiredFlg = previous_TaskCreation_Assigned[0].ISMTCRASTO_TimeRequiredFlg;
                                taskasign.ISMTCRASTO_ActiveFlg = true;
                                taskasign.CreatedDate = indianTime;
                                taskasign.UpdatedDate = indianTime;
                                _IssueContext.Add(taskasign);
                            }

                            try
                            {
                                var Insert_ISM_YearlyPlan_Swapped_Task = _IssueContext.Database.ExecuteSqlCommand("sp_ISM_YearlyPlan_Swapped_Task_insert @p0,@p1,@p2,@p3,@p4,@p5", empid, s_task.ISMTCR_Id,  s_task.ISMMIMPPL_Id, data.ISMMIMPPL_Id, s_task.ISMTAPL_Id, data.UserId, data.UserId);

                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            saved_count++;
                            data.suscnt = saved_count;
                        }
                      
                    }
                    int rowAffected = _IssueContext.SaveChanges();

                    if (rowAffected > 0)
                    {
                        data.returnval = true;
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
