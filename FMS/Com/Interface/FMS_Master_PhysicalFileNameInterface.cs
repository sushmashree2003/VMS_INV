using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMS.Com.Interface
{
    public interface FMS_Master_PhysicalFileNameInterface
    {
        FMS_Master_PhysicalFileNameDTO loadPhysicalFiledata(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO loadFileCategorydata(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO loadDispatchModedata(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO loadFileCabinetdata(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO savedata(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO saveFileCategory(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ViewDeptList(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ActiveDeactiveDept(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO saveDispatchMode(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO saveFileCabinet(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO editRecord(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO editRecord2(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO editRecord3(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO editRecord4(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord2(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord3(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord4(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO loadreferenceconfg(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO savereferenceconfg(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO editreferenceconfg(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ActiveDeactivereferenceconfg(FMS_Master_PhysicalFileNameDTO data);



        /* File Name & Cabinet Mapping */

        FMS_Master_PhysicalFileNameDTO LoadFilenameAndCabinet(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO OnChangephysicalname(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO OnChangeFileCabinet(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO SaveFileNameAndCabinet(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO Editfilenamecabinet(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ActivateRecord5(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO OnChangeInstitution(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ViewPhysicalFileDeptList(FMS_Master_PhysicalFileNameDTO data);
        FMS_Master_PhysicalFileNameDTO ActiveDeactiveDeptPhyscialfile(FMS_Master_PhysicalFileNameDTO data);
    }
}
