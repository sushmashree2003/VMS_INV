using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class InOutBoundCallDetailImpl : Interfaces.InOutBoundCallDetailInterface
    {
        public FOContext _FOContext;

        public InOutBoundCallDetailImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO getdata(ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            try
            {
                data.inoutboundlist = _FOContext.ISM_Inbound_Outbound_Call_DetailsDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO report(ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            try
            {
                data.inoutboundlist = _FOContext.ISM_Inbound_Outbound_Call_DetailsDMO.Where(t=>t.ISMIOC_Date >= data.ISMIOC_Datefrom && t.ISMIOC_Date <= data.ISMIOC_Dateto).OrderBy(t=>t.ISMIOC_Date).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO savedata(ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            try
            {
                ISM_Inbound_Outbound_Call_DetailsDMO obj = new ISM_Inbound_Outbound_Call_DetailsDMO();
                if (data.ISMIOC_Id > 0)
                {
                    var duplicate = _FOContext.ISM_Inbound_Outbound_Call_DetailsDMO.Where(t => t.ISMIOC_Date == data.ISMIOC_Date && t.ISMIOC_From == data.ISMIOC_From && t.ISMIOC_To == data.ISMIOC_To && t.ISMIOC_Purpose == data.ISMIOC_Purpose && t.ISMIOC_Name == data.ISMIOC_Name && t.ISMIOC_Remarks == data.ISMIOC_Remarks && t.ISMIOC_TransferToName == data.ISMIOC_TransferToName && t.ISMIOC_InOutFlag == data.ISMIOC_InOutFlag && t.ISMIOC_CallNo == data.ISMIOC_CallNo).ToArray();

                    if (duplicate.Length > 0)
                    {
                        data.returnvalue = "Duplicate";
                    }
                    else
                    {
                        var updatedata = _FOContext.ISM_Inbound_Outbound_Call_DetailsDMO.Where(t=>t.ISMIOC_Id == data.ISMIOC_Id).FirstOrDefault();
                        updatedata.ISMIOC_Date = data.ISMIOC_Date;
                        updatedata.ISMIOC_From = data.ISMIOC_From;
                        updatedata.ISMIOC_To = data.ISMIOC_To;
                        updatedata.ISMIOC_Purpose = data.ISMIOC_Purpose;
                        updatedata.ISMIOC_Name = data.ISMIOC_Name;
                        updatedata.ISMIOC_Remarks = data.ISMIOC_Remarks;
                        updatedata.ISMIOC_UpdatedDate = DateTime.Today;
                        updatedata.ISMIOC_UserId = data.UserId;
                        updatedata.ISMIOC_TransferToName = data.ISMIOC_TransferToName;
                        updatedata.ISMIOC_InOutFlag = data.ISMIOC_InOutFlag;
                        updatedata.ISMIOC_CallNo = data.ISMIOC_CallNo;
                        _FOContext.Update(updatedata);
                        int cnt = _FOContext.SaveChanges();
                        if (cnt > 0) { data.returnvalue = "success"; }
                        else
                        { data.returnvalue = "false"; }
                    }
                }
                else
                {
                    var duplicate = _FOContext.ISM_Inbound_Outbound_Call_DetailsDMO.Where(t=>t.ISMIOC_Date == data.ISMIOC_Date && t.ISMIOC_From == data.ISMIOC_From && t.ISMIOC_To == data.ISMIOC_To && t.ISMIOC_Purpose == data.ISMIOC_Purpose && t.ISMIOC_Name == data.ISMIOC_Name && t.ISMIOC_Remarks == data.ISMIOC_Remarks && t.ISMIOC_TransferToName == data.ISMIOC_TransferToName && t.ISMIOC_InOutFlag == data.ISMIOC_InOutFlag && t.ISMIOC_CallNo == data.ISMIOC_CallNo).ToArray();

                    if (duplicate.Length > 0)
                    {
                        data.returnvalue = "Duplicate";
                    }
                    else
                    {
                        obj.ISMIOC_Date = data.ISMIOC_Date;
                        obj.ISMIOC_From = data.ISMIOC_From;
                        obj.ISMIOC_To = data.ISMIOC_To;
                        obj.ISMIOC_Purpose = data.ISMIOC_Purpose;
                        obj.ISMIOC_Name = data.ISMIOC_Name;
                        obj.ISMIOC_Remarks = data.ISMIOC_Remarks;
                        obj.ISMIOC_CreatedDate = DateTime.Today;
                        obj.ISMIOC_UpdatedDate = DateTime.Today;
                        obj.ISMIOC_UserId = data.UserId;
                        obj.ISMIOC_TransferToName = data.ISMIOC_TransferToName;
                        obj.ISMIOC_InOutFlag = data.ISMIOC_InOutFlag;
                        obj.ISMIOC_CallNo = data.ISMIOC_CallNo;
                        _FOContext.Add(obj);
                        int cnt = _FOContext.SaveChanges();
                        if (cnt > 0) { data.returnvalue = "success"; } else
                        { data.returnvalue = "false"; }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO getdetails(int id)
        {
            ISM_Inbound_Outbound_Call_DetailsDTO data = new ISM_Inbound_Outbound_Call_DetailsDTO();
            var getdata = _FOContext.ISM_Inbound_Outbound_Call_DetailsDMO.Where(t => t.ISMIOC_Id == id).ToArray();
            data.editdata = getdata;
            return data;
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO deletedata(int id)
        {
            ISM_Inbound_Outbound_Call_DetailsDTO data = new ISM_Inbound_Outbound_Call_DetailsDTO();
            var getdata = _FOContext.ISM_Inbound_Outbound_Call_DetailsDMO.Where(t => t.ISMIOC_Id == id);
            _FOContext.Remove(getdata);
            _FOContext.SaveChanges();
            data.returnvalue = "success";
            return data;
        }
    }
}
