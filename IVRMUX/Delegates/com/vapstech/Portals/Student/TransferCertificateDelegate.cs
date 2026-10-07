using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Student
{
    public class TransferCertificateDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TransferCertificate_DTO, TransferCertificate_DTO> COMMM = new CommonDelegate<TransferCertificate_DTO, TransferCertificate_DTO>();

        public TransferCertificate_DTO getdetails(TransferCertificate_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "TransferCertificateFacade/getdetails/");
        }

        public TransferCertificate_DTO tcApply(TransferCertificate_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "TransferCertificateFacade/tcApply/");
        }

        public TransferCertificate_DTO deactiveY(TransferCertificate_DTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "TransferCertificateFacade/deactiveY/");
        }

        public TransferCertificate_DTO certificateApproved(TransferCertificate_DTO data)
        {
            return COMMM.POSTPORTALData(data, "TransferCertificateFacade/certificateApproved/");
        }

        public TransferCertificate_DTO certificateRejected(TransferCertificate_DTO data)
        {
            return COMMM.POSTPORTALData(data, "TransferCertificateFacade/certificateRejected/");
        }


    }
}
