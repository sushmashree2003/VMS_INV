using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using DomainModel.Model.com.vapstech.VMS.Training;
using IssueManager.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class RoomBankDeatilsImpl : Interfaces.RoomBankDeatilsInterface
    {
        public IssueManagerContext _context;

        public RoomBankDeatilsImpl(IssueManagerContext _cont)
        {
            _context = _cont;
        }

        public RoomBankDeatilsDTO getloaddata(RoomBankDeatilsDTO data)
        {
            try

            {
                data.rooms = _context.HR_Master_Room.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
                data.banklist = _context.HR_Master_BankDeatils.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
                data.get_levl = (from a in _context.RoomBankDeatilsDMO
                                 from b in _context.HR_Master_BankDeatils
                                 from c in _context.HR_Master_Room
                                 where (a.HRMBD_Id == b.HRMBD_Id && a.HRMR_Id==c.HRMR_Id && b.MI_Id==c.MI_Id && b.MI_Id==data.MI_Id && c.MI_Id==data.MI_Id)
                                 select new RoomBankDeatilsDTO
                                 {
                                     HRMBD_Id = b.HRMBD_Id,                              
                                     HRMBD_BankName = (b.HRMBD_BankName +'-' + b.HRMBD_BankAccountNo),
                                     HRMR_Id =c.HRMR_Id,
                                     HRMR_RoomName=c.HRMR_RoomName,
                                     HRMRBD_ActiveFlag = a.HRMRBD_ActiveFlag,
                                     HRMRBD_Id = a.HRMRBD_Id
                                 }).Distinct().ToArray();


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public RoomBankDeatilsDTO savedetails(RoomBankDeatilsDTO data)
        {


            try
            {
                if (data.HRMRBD_Id != 0)
                {
                    var res = _context.RoomBankDeatilsDMO.Where(t => t.HRMR_Id == data.HRMR_Id && t.HRMBD_Id == data.HRMBD_Id  && t.HRMRBD_Id != data.HRMRBD_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _context.RoomBankDeatilsDMO.Single(t => t.HRMRBD_Id == data.HRMRBD_Id);

                        result.HRMR_Id = data.HRMR_Id;
                        result.HRMBD_Id = data.HRMBD_Id;                   
                        result.HRMRBD_ActiveFlag = true;
                        result.HRMRBD_CreatedDate = DateTime.Now;
                        result.HRMRBD_UpdatedDate = DateTime.Now;
                        result.HRMRBD_CreatedBy = data.UserId;
                        result.HRMRBD_UpdatedBy = data.UserId;

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
                }
                else
                {
                    var res = _context.RoomBankDeatilsDMO.Where(t => t.HRMR_Id == data.HRMR_Id && t.HRMBD_Id == data.HRMBD_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        RoomBankDeatilsDMO obj = new RoomBankDeatilsDMO();

                        obj.HRMR_Id = data.HRMR_Id;
                        obj.HRMBD_Id = data.HRMBD_Id;
                        obj.HRMRBD_ActiveFlag = true;
                        obj.HRMRBD_CreatedDate = DateTime.Now;
                        obj.HRMRBD_UpdatedDate = DateTime.Now;
                        obj.HRMRBD_CreatedBy = data.UserId;
                        obj.HRMRBD_UpdatedBy = data.UserId;
                        _context.Add(obj);

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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public RoomBankDeatilsDTO deactive(RoomBankDeatilsDTO data)
        {
            try
            {
                var result = _context.RoomBankDeatilsDMO.Single(t => t.HRMRBD_Id == data.HRMRBD_Id);

                if (result.HRMRBD_ActiveFlag == true)
                {
                    result.HRMRBD_ActiveFlag = false;
                }
                else if (result.HRMRBD_ActiveFlag == false)
                {
                    result.HRMRBD_ActiveFlag = true;
                }
                result.HRMRBD_UpdatedDate = DateTime.Now;
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
    }
}



