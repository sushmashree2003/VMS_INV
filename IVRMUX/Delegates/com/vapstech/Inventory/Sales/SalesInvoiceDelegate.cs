using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class SalesInvoiceDelegate
    {
        CommonDelegate<SalesInvoiceDTO, SalesInvoiceDTO> COMINV = new CommonDelegate<SalesInvoiceDTO, SalesInvoiceDTO>();
        public SalesInvoiceDTO getloaddata(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getloaddata/");
        }
        public SalesInvoiceDTO getStudentClsSec(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getStudentClsSec/");
        }
        public SalesInvoiceDTO getsectionlist(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getsectionlist/");
        }
        public SalesInvoiceDTO getStudentlist(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getStudentlist/");
        }
        public SalesInvoiceDTO getitem(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getitem/");
        }
        public SalesInvoiceDTO getitemDetail(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getitemDetail/");
        }
        public SalesInvoiceDTO savedetails(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/savedetails/");
        }
        public SalesInvoiceDTO getSaletypes(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getSaletypes/");
        }
        public SalesInvoiceDTO getSaleItemDetails(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getSaleItemDetails/");
        }
        public SalesInvoiceDTO getSaleItemTax(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/getSaleItemTax/");
        }
        public SalesInvoiceDTO deactive(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/deactive/");
        }
        public SalesInvoiceDTO deactiveS(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/deactiveS/");
        }
        public SalesInvoiceDTO deactivetax(SalesInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceFacade/deactivetax/");
        }


    }
}
