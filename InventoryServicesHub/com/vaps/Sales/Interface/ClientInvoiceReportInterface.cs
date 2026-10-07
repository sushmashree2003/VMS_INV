using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface ClientInvoiceReportInterface
    {
        Task<ClientInvoiceReportDTO> getloaddata(ClientInvoiceReportDTO data);
        Task<ClientInvoiceReportDTO> getStudentClsSec(ClientInvoiceReportDTO data);
        ClientInvoiceReportDTO getsectionlist(ClientInvoiceReportDTO data);
        Task<ClientInvoiceReportDTO> getStudentlist(ClientInvoiceReportDTO data);
        Task<ClientInvoiceReportDTO> getitem(ClientInvoiceReportDTO data);
        Task<ClientInvoiceReportDTO> getitemDetail(ClientInvoiceReportDTO data);
       Task<ClientInvoiceReportDTO> savedetails(ClientInvoiceReportDTO data);
        Task<ClientInvoiceReportDTO> getSaletypes(ClientInvoiceReportDTO data);
        Task<ClientInvoiceReportDTO> getSaleItemDetails(ClientInvoiceReportDTO data);
        ClientInvoiceReportDTO getSaleItemTax(ClientInvoiceReportDTO data);
        ClientInvoiceReportDTO deactive(ClientInvoiceReportDTO data);
        ClientInvoiceReportDTO deactiveS(ClientInvoiceReportDTO data);
        ClientInvoiceReportDTO deactivetax(ClientInvoiceReportDTO data);
        ClientInvoiceReportDTO Editdata(ClientInvoiceReportDTO data);

    }


}
