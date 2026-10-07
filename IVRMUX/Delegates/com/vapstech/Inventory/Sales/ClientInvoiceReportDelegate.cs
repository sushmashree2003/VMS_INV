using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class ClientInvoiceReportDelegate
    {
        CommonDelegate<ClientInvoiceReportDTO, ClientInvoiceReportDTO> COMINV = new CommonDelegate<ClientInvoiceReportDTO, ClientInvoiceReportDTO>();
        public ClientInvoiceReportDTO getloaddata(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getloaddata/");
        }
        public ClientInvoiceReportDTO getStudentClsSec(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getStudentClsSec/");
        }
        public ClientInvoiceReportDTO getsectionlist(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getsectionlist/");
        }
        public ClientInvoiceReportDTO getStudentlist(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getStudentlist/");
        }
        public ClientInvoiceReportDTO getitem(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getitem/");
        }
        public ClientInvoiceReportDTO getitemDetail(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getitemDetail/");
        }
        public ClientInvoiceReportDTO savedetails(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/savedetails/");
        }
        public ClientInvoiceReportDTO getSaletypes(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getSaletypes/");
        }
        public ClientInvoiceReportDTO getSaleItemDetails(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getSaleItemDetails/");
        }
        public ClientInvoiceReportDTO getSaleItemTax(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/getSaleItemTax/");
        }
        public ClientInvoiceReportDTO deactive(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/deactive/");
        }
        public ClientInvoiceReportDTO deactiveS(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/deactiveS/");
        }
        public ClientInvoiceReportDTO deactivetax(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/deactivetax/");
        }
        public ClientInvoiceReportDTO Editdata(ClientInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ClientInvoiceReportFacade/Editdata/");
        }


    }
}
