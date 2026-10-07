using PreadmissionDTOs.com.vaps.VMS.Exit;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface HR_NOC_Process_Interface
    {
        //--------------------ISM_Certificate_Request--------------------

        ISM_Certificate_RequestDTO getloaddata(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO savedetails(ISM_Certificate_RequestDTO data);

        ISM_Certificate_RequestDTO deactive(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO Edit(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO Viewdata(ISM_Certificate_RequestDTO data);


        //----------------------certificate approval------------
        ISM_Certificate_RequestDTO loaddata(ISM_Certificate_RequestDTO data);


        ISM_Certificate_RequestDTO Viewsdata(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO approvedrecord(ISM_Certificate_RequestDTO data);

        ISM_Certificate_RequestDTO Onload(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO getapprovalreport(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO DocView(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO getIEDetails(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO getadminloaddata(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO getcertificate(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO saveadmindetails(ISM_Certificate_RequestDTO data);
        ISM_Certificate_RequestDTO nocdetails(ISM_Certificate_RequestDTO data);



    }
}
