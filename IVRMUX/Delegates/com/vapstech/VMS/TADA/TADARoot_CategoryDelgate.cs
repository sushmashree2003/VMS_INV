using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADARoot_CategoryDelgate
    {
        CommonDelegate<TADARoot_CategoryDTO, TADARoot_CategoryDTO> COMMI = new CommonDelegate<TADARoot_CategoryDTO, TADARoot_CategoryDTO>();

        public TADARoot_CategoryDTO getalldata(TADARoot_CategoryDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADARoot_CategoryFacade/getalldata/");
        }
        public TADARoot_CategoryDTO SaveCategory(TADARoot_CategoryDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/SaveCategory");
        }
        public TADARoot_CategoryDTO SaveRouteEmpCategory(TADARoot_CategoryDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/SaveRouteEmpCategory");
        }
        public TADARoot_CategoryDTO saveRegionMapping(TADARoot_CategoryDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/saveRegionMapping");
        }

        public TADARoot_CategoryDTO deactiveY(TADARoot_CategoryDTO dTO)
        {

            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/deactiveY/");
        }

        public TADARoot_CategoryDTO viewrecordspopup(TADARoot_CategoryDTO dTO)
        {

            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/viewrecordspopup/");
        }

        public TADARoot_CategoryDTO deactiveCat(TADARoot_CategoryDTO dTO)
        {

            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/deactiveCat/");
        }
        public TADARoot_CategoryDTO deactiveRegion(TADARoot_CategoryDTO dTO)
        {

            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/deactiveRegion/");
        }

        public TADARoot_CategoryDTO Saveprimary(TADARoot_CategoryDTO dTO)
        {

            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/Saveprimary/");
        }

        public TADARoot_CategoryDTO ActivatePrimary(TADARoot_CategoryDTO dTO)
        {

            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/ActivatePrimary/");
        }
        public TADARoot_CategoryDTO validateordernumber(TADARoot_CategoryDTO dTO)
        {

            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/validateordernumber/");
        }
        public TADARoot_CategoryDTO SetOrder(TADARoot_CategoryDTO dTO)
        {

            return COMMI.POSTIssueManager(dTO, "TADARoot_CategoryFacade/SetOrder/");
        }
    }
}
