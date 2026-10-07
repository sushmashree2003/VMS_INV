using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ComplaintRegisterImpl : Interfaces.ComplaintRegisterInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;

        public ComplaintRegisterImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        public ComplaintRegister_DTO save(ComplaintRegister_DTO data)
        {
            try
            {
                VMS_Complaint_Register_DMO a = new VMS_Complaint_Register_DMO();
                a.MI_Id = data.MI_Id;
                a.VMSCR_Name = data.VMSCR_Name;
                a.VMSCR_EmailId = data.VMSCR_EmailId;
                a.VMSCR_MobileNo = data.VMSCR_MobileNo;
                a.VMSCR_TaskTitle = data.VMSCR_TaskTitle;
                a.VMSCR_Desc = data.VMSCR_Desc;
                a.VMSCR_CreatedDate = DateTime.Today;
                a.VMSCR_UpdatedDate = DateTime.Today;
                _IssueContext.Add(a);
                _IssueContext.SaveChanges();
                var w = _IssueContext.SaveChanges();
                if (w > 0)
                {
                    data.message = "Saved";
                }
                else
                {
                    data.message = "Failed";
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
