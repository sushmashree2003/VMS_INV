using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface ProformaInvoiceInterface
    {
        Task<ProformaInvoiceDTO> getloaddata(ProformaInvoiceDTO data);
        Task<ProformaInvoiceDTO> getStudentClsSec(ProformaInvoiceDTO data);
        ProformaInvoiceDTO getsectionlist(ProformaInvoiceDTO data);
        Task<ProformaInvoiceDTO> getStudentlist(ProformaInvoiceDTO data);
        Task<ProformaInvoiceDTO> getitem(ProformaInvoiceDTO data);
        Task<ProformaInvoiceDTO> getitemDetail(ProformaInvoiceDTO data);
       Task<ProformaInvoiceDTO> savedetails(ProformaInvoiceDTO data);
        Task<ProformaInvoiceDTO> getSaletypes(ProformaInvoiceDTO data);
        Task<ProformaInvoiceDTO> getSaleItemDetails(ProformaInvoiceDTO data);
        ProformaInvoiceDTO getSaleItemTax(ProformaInvoiceDTO data);
        ProformaInvoiceDTO deactive(ProformaInvoiceDTO data);
        ProformaInvoiceDTO deactiveS(ProformaInvoiceDTO data);
        ProformaInvoiceDTO deactivetax(ProformaInvoiceDTO data);

    }


}
