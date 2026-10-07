using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.FMS;

namespace FMS.Com.Interface
{
    public interface FMS_CorrespondenceInterface
    {
        FMS_CorrespondenceDTO onfirsttabcorrespondence(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO onchangeclient(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO getleaddetails(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO checkrefnoduplicate(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO saveonfirsttabcorrespondence(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO activedeactivefirsttab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO Editdatafirsttab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO viewfirsttabdetails(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO activedeactivesubdetails(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO searchrefernceno(FMS_CorrespondenceDTO data);

        /*GENERAL DETILS FOR SEARCH REFERNCE NO. */      
        FMS_CorrespondenceDTO searchreferncenosecondtab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO OnChangeDept(FMS_CorrespondenceDTO data);

        /*SECOND TAB */
        FMS_CorrespondenceDTO onsecondtabcorrespondence(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO onchangefilecabinet(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO savesecondtabcorrespondence(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO Editdatasecondtab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO onchangerefnosecondtab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO activedeactivesecondtab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO ViewCorrespondanceFiles(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO DeleteCorrespondanceFile(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO OnChangephysicalpath(FMS_CorrespondenceDTO data);

        /*THIRD TAB */
        FMS_CorrespondenceDTO onthirdtabcorrespondence(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO onchangerefnothirdtab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO savethirdtabcorrespondence(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO Editdatathirdtab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO activedeactivethirdtab(FMS_CorrespondenceDTO data);
        FMS_CorrespondenceDTO OnChangeLetterHeadNo(FMS_CorrespondenceDTO data);
    }
}
