using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class ProformaInvoiceReportDelegate
    {
        CommonDelegate<ProformaInvoiceReportDTO, ProformaInvoiceReportDTO> COMINV = new CommonDelegate<ProformaInvoiceReportDTO, ProformaInvoiceReportDTO>();
        public ProformaInvoiceReportDTO getloaddata(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getloaddata/");
        }
        public ProformaInvoiceReportDTO getStudentClsSec(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getStudentClsSec/");
        }
        public ProformaInvoiceReportDTO getsectionlist(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getsectionlist/");
        }
        public ProformaInvoiceReportDTO getStudentlist(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getStudentlist/");
        }
        public ProformaInvoiceReportDTO getitem(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getitem/");
        }
        public ProformaInvoiceReportDTO getitemDetail(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getitemDetail/");
        }
        public ProformaInvoiceReportDTO savedetails(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/savedetails/");
        }
        public ProformaInvoiceReportDTO getSaletypes(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getSaletypes/");
        }
        public ProformaInvoiceReportDTO getSaleItemDetails(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getSaleItemDetails/");
        }
        public ProformaInvoiceReportDTO getSaleItemTax(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/getSaleItemTax/");
        }
        public ProformaInvoiceReportDTO deactive(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/deactive/");
        }
        public ProformaInvoiceReportDTO deactiveS(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/deactiveS/");
        }
        public ProformaInvoiceReportDTO deactivetax(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/deactivetax/");
        }
        public ProformaInvoiceReportDTO Editdata(ProformaInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "ProformaInvoiceReportFacade/Editdata/");
        }


    }
}
