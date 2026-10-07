using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Services
{
    public class EmailDepartmentMappingImpl : Interfaces.EmailDepartmentMappingInterface
    {
        public IssueManagerContext _context;
        ILogger<EmailDepartmentMappingDTO> _logger;

        public EmailDepartmentMappingImpl(IssueManagerContext _cont, ILogger<EmailDepartmentMappingDTO> _log)
        {
            _context = _cont;
            _logger = _log;
        }
        public EmailDepartmentMappingDTO getdetails(EmailDepartmentMappingDTO data)
        {
            try
            {
                data.getdepartment = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).ToArray();

                data.getsavelist = (from a in _context.HR_Master_Department
                                    from b in _context.ISM_Email_Department_MappingDMO
                                    where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id)
                                    select new EmailDepartmentMappingDTO
                                    {
                                        departmentname = a.HRMD_DepartmentName,
                                        emailids = b.ISM_Email_CC,
                                        ISM_EId = b.ISM_EId,
                                        HRMD_Id = b.HRMD_Id

                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Email Department Mapping Load : " + ex.Message);
            }
            return data;
        }
        public EmailDepartmentMappingDTO onchangedept(EmailDepartmentMappingDTO data)
        {
            try
            {
                data.getonchangedata = (from a in _context.HR_Master_Department
                                        from b in _context.ISM_Email_Department_MappingDMO
                                        where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && b.HRMD_Id == data.HRMD_Id)
                                        select new EmailDepartmentMappingDTO
                                        {
                                            departmentname = a.HRMD_DepartmentName,
                                            emailids = b.ISM_Email_CC,
                                            ISM_EId = b.ISM_EId,
                                            HRMD_Id = b.HRMD_Id

                                        }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Email Department Mapping onchangedept : " + ex.Message);
            }
            return data;
        }
        public EmailDepartmentMappingDTO editdata(EmailDepartmentMappingDTO data)
        {
            try
            {
                data.geteditdata = (from a in _context.HR_Master_Department
                                    from b in _context.ISM_Email_Department_MappingDMO
                                    where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && b.HRMD_Id == data.HRMD_Id && b.ISM_EId == data.ISM_EId)
                                    select new EmailDepartmentMappingDTO
                                    {
                                        departmentname = a.HRMD_DepartmentName,
                                        emailids = b.ISM_Email_CC,
                                        ISM_EId = b.ISM_EId,
                                        HRMD_Id = b.HRMD_Id

                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Email Department Mapping editdata : " + ex.Message);
            }
            return data;
        }
        public EmailDepartmentMappingDTO savedata(EmailDepartmentMappingDTO data)
        {
            try
            {
                if (data.multipleemailid.Length > 0)
                {
                    string emailid = "";

                    for (int k = 0; k < data.multipleemailid.Length; k++)
                    {
                        if (k == 0)
                        {
                            emailid = data.multipleemailid[k].emaild;
                        }
                        else
                        {
                            emailid = emailid + "," + data.multipleemailid[k].emaild;
                        }
                    }

                    if (data.ISM_EId > 0)
                    {
                        var checkdeptid = _context.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id
                        && a.ISM_EId == data.ISM_EId).ToList();
                        if (checkdeptid.Count() > 0)
                        {
                            var result = _context.ISM_Email_Department_MappingDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id
                            && a.ISM_EId == data.ISM_EId);
                            result.ISM_Email_CC = emailid;
                            result.UpdatedDate = DateTime.UtcNow;
                            _context.Update(result);

                            var i = _context.SaveChanges();
                            if (i > 0)
                            {
                                data.returnval = true;
                                data.message = "Update";
                            }
                            else
                            {
                                data.returnval = false;
                                data.message = "Update";
                            }
                        }
                    }

                    else
                    {
                        ISM_Email_Department_MappingDMO dmo = new ISM_Email_Department_MappingDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.HRMD_Id = data.HRMD_Id;
                        dmo.ISM_Email_CC = emailid;
                        dmo.UpdatedDate = DateTime.UtcNow;
                        dmo.CreatedDate = DateTime.UtcNow;
                        _context.Add(dmo);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Save";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Save";
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                data.message = "Failed";
                _logger.LogInformation("Email Department Mapping editdata : " + ex.Message);
            }
            return data;
        }

        // Department Deviation Remarks 

        public EmailDepartmentMappingDTO getloaddata(EmailDepartmentMappingDTO data)
        {
            try
            {
                List<long> institute_ids = new List<long>();

                var inst_id = _context.IVRM_User_Login_InstitutionwiseDMO.Where(m => m.Id == data.userid && m.Activeflag == 1).Distinct().ToList();
                if (inst_id.Count > 0)
                {
                    foreach (var inst in inst_id)
                    {
                        institute_ids.Add(inst.MI_Id);
                    }
                }

                data.getinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && institute_ids.Contains(a.MI_Id)).ToArray();

                if (inst_id.Count == 1)
                {
                    data.getdepartment = _context.HR_Master_Department.Where(a => institute_ids.Contains(a.MI_Id) && a.HRMD_ActiveFlag == true).ToArray();
                }


                data.getsavedlist = (from a in _context.ISM_Departmentwise_RemarksDMO
                                     from b in _context.HR_Master_Department
                                     from c in _context.Institution
                                     where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == c.MI_Id && institute_ids.Contains(a.MI_Id))
                                     select new EmailDepartmentMappingDTO
                                     {
                                         departmentname = b.HRMD_DepartmentName,
                                         instituitonname = c.MI_Name,
                                         remarks = a.ISMDR_Remarks,
                                         ISMDR_Id = a.ISMDR_Id,
                                         MI_Id = a.MI_Id,
                                         HRMD_Id = a.HRMD_Id,
                                         ISMDR_ActiveFlag = a.ISMDR_ActiveFlag,
                                         ISMDR_ReasonFlag=a.ISMDR_ReasonFlag
                                     }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                data.message = "Failed";
                _logger.LogInformation("Remarks Department Mapping getloaddata : " + ex.Message);
            }
            return data;
        }
        public EmailDepartmentMappingDTO onchangeinstitution(EmailDepartmentMappingDTO data)
        {
            try
            {

                data.getdepartment = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Remarks Department Mapping onchangeinstitution : " + ex.Message);
            }
            return data;
        }
        public EmailDepartmentMappingDTO onchangedepartment(EmailDepartmentMappingDTO data)
        {
            try
            {

                data.getinstdeptsavedlist = (from a in _context.ISM_Departmentwise_RemarksDMO
                                             from b in _context.HR_Master_Department
                                             from c in _context.Institution
                                             where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id)
                                             select a).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Remarks Department Mapping onchangedepartment : " + ex.Message);
            }
            return data;
        }
        public EmailDepartmentMappingDTO saveremarksdata(EmailDepartmentMappingDTO data)
        {
            try
            {
                if (data.temp_deptremarks.Length > 0)
                {
                    foreach (var c in data.temp_deptremarks)
                    {

                        if (c.ISMDR_Id > 0)
                        {
                            var result = _context.ISM_Departmentwise_RemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id && a.ISMDR_Id == c.ISMDR_Id);
                            result.ISMDR_Remarks = c.ISMDR_Remarks;
                            result.UpdatedDate = DateTime.UtcNow;
                            result.ISMDR_UptadedBy = data.userid;
                            result.ISMDR_ReasonFlag = c.ISMDR_ReasonFlag;
                            _context.Update(result);
                        }
                        else
                        {
                            ISM_Departmentwise_RemarksDMO dmo = new ISM_Departmentwise_RemarksDMO();

                            dmo.MI_Id = c.MI_Id;
                            dmo.HRMD_Id = c.HRMD_Id;
                            dmo.ISMDR_Remarks = c.ISMDR_Remarks;
                            dmo.ISMDR_ActiveFlag = true;
                            dmo.ISMDR_ReasonFlag = c.ISMDR_ReasonFlag;
                            dmo.ISMDR_CreatedBy = data.userid;
                            dmo.ISMDR_UptadedBy = data.userid;
                            dmo.CreatedDate = DateTime.UtcNow;
                            dmo.UpdatedDate = DateTime.UtcNow;
                            _context.Add(dmo);
                        }
                    }

                    var i = _context.SaveChanges();

                    if (i > 0)
                    {
                        data.message = "Save";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Save";
                        data.returnval = true;
                    }
                }

            }
            catch (Exception ex)
            {
                data.message = "Failed";
                _logger.LogInformation("Remarks Department Mapping saveremarksdata : " + ex.Message);
            }
            return data;
        }
        public EmailDepartmentMappingDTO Editremarksdata(EmailDepartmentMappingDTO data)
        {
            try
            {
                data.geteditremarksdata = _context.ISM_Departmentwise_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id
                && a.ISMDR_Id == data.ISMDR_Id).ToArray();

                data.getdepartment = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Remarks Department Mapping Editremarksdata : " + ex.Message);
            }
            return data;
        }
        public EmailDepartmentMappingDTO deactivate(EmailDepartmentMappingDTO data)
        {
            try
            {
                var result = _context.ISM_Departmentwise_RemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id && a.ISMDR_Id == data.ISMDR_Id);
                if (result.ISMDR_ActiveFlag == true)
                {
                    result.ISMDR_ActiveFlag = false;
                }
                else
                {
                    result.ISMDR_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.UtcNow;
                result.ISMDR_UptadedBy = data.userid;
                _context.Update(result);

                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                List<long> institute_ids = new List<long>();

                var inst_id = _context.IVRM_User_Login_InstitutionwiseDMO.Where(m => m.Id == data.userid && m.Activeflag == 1).Distinct().ToList();
                if (inst_id.Count > 0)
                {
                    foreach (var inst in inst_id)
                    {
                        institute_ids.Add(inst.MI_Id);
                    }
                }

                data.getsavedlist = (from a in _context.ISM_Departmentwise_RemarksDMO
                                     from b in _context.HR_Master_Department
                                     from c in _context.Institution
                                     where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == c.MI_Id && institute_ids.Contains(a.MI_Id))
                                     select new EmailDepartmentMappingDTO
                                     {
                                         departmentname = b.HRMD_DepartmentName,
                                         instituitonname = c.MI_Name,
                                         remarks = a.ISMDR_Remarks,
                                         ISMDR_Id = a.ISMDR_Id,
                                         MI_Id = a.MI_Id,
                                         HRMD_Id = a.HRMD_Id,
                                         ISMDR_ActiveFlag = a.ISMDR_ActiveFlag
                                     }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Remarks Department Mapping deactivate : " + ex.Message);
            }
            return data;
        }
    }
}
