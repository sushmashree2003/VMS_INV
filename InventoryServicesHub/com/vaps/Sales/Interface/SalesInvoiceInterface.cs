using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface SalesInvoiceInterface
    {
        Task<SalesInvoiceDTO> getloaddata(SalesInvoiceDTO data);
        Task<SalesInvoiceDTO> getStudentClsSec(SalesInvoiceDTO data);
        SalesInvoiceDTO getsectionlist(SalesInvoiceDTO data);
        Task<SalesInvoiceDTO> getStudentlist(SalesInvoiceDTO data);
        Task<SalesInvoiceDTO> getitem(SalesInvoiceDTO data);
        Task<SalesInvoiceDTO> getitemDetail(SalesInvoiceDTO data);
       Task<SalesInvoiceDTO> savedetails(SalesInvoiceDTO data);
        Task<SalesInvoiceDTO> getSaletypes(SalesInvoiceDTO data);
        Task<SalesInvoiceDTO> getSaleItemDetails(SalesInvoiceDTO data);
        SalesInvoiceDTO getSaleItemTax(SalesInvoiceDTO data);
        SalesInvoiceDTO deactive(SalesInvoiceDTO data);
        SalesInvoiceDTO deactiveS(SalesInvoiceDTO data);
        SalesInvoiceDTO deactivetax(SalesInvoiceDTO data);

    }


}
