using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface ProformaInvoiceReportInterface
    {
        Task<ProformaInvoiceReportDTO> getloaddata(ProformaInvoiceReportDTO data);
        Task<ProformaInvoiceReportDTO> getStudentClsSec(ProformaInvoiceReportDTO data);
        ProformaInvoiceReportDTO getsectionlist(ProformaInvoiceReportDTO data);
        Task<ProformaInvoiceReportDTO> getStudentlist(ProformaInvoiceReportDTO data);
        Task<ProformaInvoiceReportDTO> getitem(ProformaInvoiceReportDTO data);
        Task<ProformaInvoiceReportDTO> getitemDetail(ProformaInvoiceReportDTO data);
       Task<ProformaInvoiceReportDTO> savedetails(ProformaInvoiceReportDTO data);
        Task<ProformaInvoiceReportDTO> getSaletypes(ProformaInvoiceReportDTO data);
        Task<ProformaInvoiceReportDTO> getSaleItemDetails(ProformaInvoiceReportDTO data);
        ProformaInvoiceReportDTO getSaleItemTax(ProformaInvoiceReportDTO data);
        ProformaInvoiceReportDTO deactive(ProformaInvoiceReportDTO data);
        ProformaInvoiceReportDTO deactiveS(ProformaInvoiceReportDTO data);
        ProformaInvoiceReportDTO deactivetax(ProformaInvoiceReportDTO data);
        ProformaInvoiceReportDTO Editdata(ProformaInvoiceReportDTO data);

    }


}
