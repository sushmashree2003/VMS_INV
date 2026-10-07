using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
  public  interface TADARoot_CategoryInterface
    {
        TADARoot_CategoryDTO getalldata(TADARoot_CategoryDTO data);

        TADARoot_CategoryDTO SaveCategory(TADARoot_CategoryDTO data);

        TADARoot_CategoryDTO SaveRouteEmpCategory(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO saveRegionMapping(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO deactiveY(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO viewrecordspopup(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO deactiveCat(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO deactiveRegion(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO Saveprimary(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO ActivatePrimary(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO validateordernumber(TADARoot_CategoryDTO data);
        TADARoot_CategoryDTO SetOrder(TADARoot_CategoryDTO data);
    }
}
