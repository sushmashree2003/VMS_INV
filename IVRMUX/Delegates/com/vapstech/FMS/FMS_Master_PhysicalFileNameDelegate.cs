using CommonLibrary;
using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class FMS_Master_PhysicalFileNameDelegate
    {
        CommonDelegate<FMS_Master_PhysicalFileNameDTO, FMS_Master_PhysicalFileNameDTO> COMFRNT = new CommonDelegate<FMS_Master_PhysicalFileNameDTO, FMS_Master_PhysicalFileNameDTO>();

        public FMS_Master_PhysicalFileNameDTO loadPhysicalFiledata(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/loadPhysicalFiledata/");
        }
        public FMS_Master_PhysicalFileNameDTO loadDispatchModedata(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/loadDispatchModedata/");
        }
        public FMS_Master_PhysicalFileNameDTO loadFileCategorydata(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/loadFileCategorydata/");
        }
        public FMS_Master_PhysicalFileNameDTO loadFileCabinetdata(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/loadFileCabinetdata/");
        }
        public FMS_Master_PhysicalFileNameDTO savedata(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/savedata/");
        }
        public FMS_Master_PhysicalFileNameDTO saveFileCategory(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/saveFileCategory/");
        }
        public FMS_Master_PhysicalFileNameDTO ViewDeptList(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ViewDeptList/");
        }
        public FMS_Master_PhysicalFileNameDTO ViewPhysicalFileDeptList(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ViewPhysicalFileDeptList/");
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveDeptPhyscialfile(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ActiveDeactiveDeptPhyscialfile/");
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveDept(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ActiveDeactiveDept/");
        }
        public FMS_Master_PhysicalFileNameDTO saveDispatchMode(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/saveDispatchMode/");
        }
        public FMS_Master_PhysicalFileNameDTO saveFileCabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/saveFileCabinet/");
        }
        public FMS_Master_PhysicalFileNameDTO editRecord(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/editRecord/");
        }
        public FMS_Master_PhysicalFileNameDTO editRecord2(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/editRecord2/");
        }
        public FMS_Master_PhysicalFileNameDTO editRecord3(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/editRecord3/");
        }
        public FMS_Master_PhysicalFileNameDTO editRecord4(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/editRecord4/");
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ActiveDeactiveRecord/");
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord2(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ActiveDeactiveRecord2/");
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord3(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ActiveDeactiveRecord3/");
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord4(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ActiveDeactiveRecord4/");
        }
        public FMS_Master_PhysicalFileNameDTO loadreferenceconfg(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/loadreferenceconfg/");
        }
        public FMS_Master_PhysicalFileNameDTO savereferenceconfg(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/savereferenceconfg/");
        }
        public FMS_Master_PhysicalFileNameDTO editreferenceconfg(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/editreferenceconfg/");
        }
        public FMS_Master_PhysicalFileNameDTO ActiveDeactivereferenceconfg(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ActiveDeactivereferenceconfg/");
        }

        /* File Name & Cabinet Mapping */
        public FMS_Master_PhysicalFileNameDTO LoadFilenameAndCabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/LoadFilenameAndCabinet/");
        }
        public FMS_Master_PhysicalFileNameDTO OnChangephysicalname(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/OnChangephysicalname/");
        }
        public FMS_Master_PhysicalFileNameDTO OnChangeFileCabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/OnChangeFileCabinet/");
        }
        public FMS_Master_PhysicalFileNameDTO SaveFileNameAndCabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/SaveFileNameAndCabinet/");
        }
        public FMS_Master_PhysicalFileNameDTO Editfilenamecabinet(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/Editfilenamecabinet/");
        }
        public FMS_Master_PhysicalFileNameDTO ActivateRecord5(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/ActivateRecord5/");
        }
        public FMS_Master_PhysicalFileNameDTO OnChangeInstitution(FMS_Master_PhysicalFileNameDTO data)
        {
            return COMFRNT.FMSGetPostData(data, "FMS_Master_PhysicalFileNameFacade/OnChangeInstitution/");
        }
    }
}
