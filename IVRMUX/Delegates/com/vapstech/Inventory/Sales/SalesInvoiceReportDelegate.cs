using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class SalesInvoiceReportDelegate
    {
        CommonDelegate<SalesInvoiceReportDTO, SalesInvoiceReportDTO> COMINV = new CommonDelegate<SalesInvoiceReportDTO, SalesInvoiceReportDTO>();
        public SalesInvoiceReportDTO getloaddata(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getloaddata/");
        }
        public SalesInvoiceReportDTO getStudentClsSec(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getStudentClsSec/");
        }
        public SalesInvoiceReportDTO getsectionlist(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getsectionlist/");
        }
        public SalesInvoiceReportDTO getStudentlist(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getStudentlist/");
        }
        public SalesInvoiceReportDTO getitem(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getitem/");
        }
        public SalesInvoiceReportDTO getitemDetail(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getitemDetail/");
        }
        public SalesInvoiceReportDTO savedetails(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/savedetails/");
        }
        public SalesInvoiceReportDTO getSaletypes(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getSaletypes/");
        }
        public SalesInvoiceReportDTO getSaleItemDetails(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getSaleItemDetails/");
        }
        public SalesInvoiceReportDTO getSaleItemTax(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/getSaleItemTax/");
        }
        public SalesInvoiceReportDTO deactive(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/deactive/");
        }
        public SalesInvoiceReportDTO deactiveS(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/deactiveS/");
        }
        public SalesInvoiceReportDTO deactivetax(SalesInvoiceReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "SalesInvoiceReportFacade/deactivetax/");
        }


    }
}
