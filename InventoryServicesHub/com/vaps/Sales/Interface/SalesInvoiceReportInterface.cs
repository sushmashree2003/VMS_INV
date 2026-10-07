using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface SalesInvoiceReportInterface
    {
        Task<SalesInvoiceReportDTO> getloaddata(SalesInvoiceReportDTO data);
        Task<SalesInvoiceReportDTO> getStudentClsSec(SalesInvoiceReportDTO data);
        SalesInvoiceReportDTO getsectionlist(SalesInvoiceReportDTO data);
        Task<SalesInvoiceReportDTO> getStudentlist(SalesInvoiceReportDTO data);
        Task<SalesInvoiceReportDTO> getitem(SalesInvoiceReportDTO data);
        Task<SalesInvoiceReportDTO> getitemDetail(SalesInvoiceReportDTO data);
       Task<SalesInvoiceReportDTO> savedetails(SalesInvoiceReportDTO data);
        Task<SalesInvoiceReportDTO> getSaletypes(SalesInvoiceReportDTO data);
        Task<SalesInvoiceReportDTO> getSaleItemDetails(SalesInvoiceReportDTO data);
        SalesInvoiceReportDTO getSaleItemTax(SalesInvoiceReportDTO data);
        SalesInvoiceReportDTO deactive(SalesInvoiceReportDTO data);
        SalesInvoiceReportDTO deactiveS(SalesInvoiceReportDTO data);
        SalesInvoiceReportDTO deactivetax(SalesInvoiceReportDTO data);

    }


}
