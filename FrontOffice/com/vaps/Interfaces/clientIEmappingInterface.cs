using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface clientIEmappingInterface
    {

        clientIEmappingDTO onloaddata(clientIEmappingDTO data);
        clientIEmappingDTO saverecord(clientIEmappingDTO data);
        clientIEmappingDTO deactive(clientIEmappingDTO data);
        clientIEmappingDTO validateData(clientIEmappingDTO data);
    }
}
