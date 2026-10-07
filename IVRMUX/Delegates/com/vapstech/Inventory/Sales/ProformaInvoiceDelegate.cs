using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class ProformaInvoiceDelegate
    {
        CommonDelegate<ProformaInvoiceDTO, ProformaInvoiceDTO> COMINV = new CommonDelegate<ProformaInvoiceDTO, ProformaInvoiceDTO>();
        public ProformaInvoiceDTO getloaddata(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getloaddata/");
        }
        public ProformaInvoiceDTO getStudentClsSec(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getStudentClsSec/");
        }
        public ProformaInvoiceDTO getsectionlist(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getsectionlist/");
        }
        public ProformaInvoiceDTO getStudentlist(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getStudentlist/");
        }
        public ProformaInvoiceDTO getitem(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getitem/");
        }
        public ProformaInvoiceDTO getitemDetail(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getitemDetail/");
        }
        public ProformaInvoiceDTO savedetails(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/savedetails/");
        }
        public ProformaInvoiceDTO getSaletypes(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getSaletypes/");
        }
        public ProformaInvoiceDTO getSaleItemDetails(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getSaleItemDetails/");
        }
        public ProformaInvoiceDTO getSaleItemTax(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/getSaleItemTax/");
        }
        public ProformaInvoiceDTO deactive(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/deactive/");
        }
        public ProformaInvoiceDTO deactiveS(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/deactiveS/");
        }
        public ProformaInvoiceDTO deactivetax(ProformaInvoiceDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceFacade/deactivetax/");
        }


    }
}
