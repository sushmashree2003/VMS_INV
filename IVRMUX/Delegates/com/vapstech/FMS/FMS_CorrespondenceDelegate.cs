using PreadmissionDTOs.com.vaps.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class FMS_CorrespondenceDelegate
    {
        CommonDelegate<FMS_CorrespondenceDTO, FMS_CorrespondenceDTO> _comm = new CommonDelegate<FMS_CorrespondenceDTO, FMS_CorrespondenceDTO>();

        public FMS_CorrespondenceDTO onfirsttabcorrespondence(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/onfirsttabcorrespondence");
        }
        public FMS_CorrespondenceDTO onchangeclient(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/onchangeclient");
        }

        public FMS_CorrespondenceDTO getleaddetails(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/getleaddetails");
        }


        public FMS_CorrespondenceDTO checkrefnoduplicate(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/checkrefnoduplicate");
        }
        public FMS_CorrespondenceDTO saveonfirsttabcorrespondence(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/saveonfirsttabcorrespondence");
        }
        public FMS_CorrespondenceDTO activedeactivefirsttab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/activedeactivefirsttab");
        }
        public FMS_CorrespondenceDTO Editdatafirsttab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/Editdatafirsttab");
        }
        public FMS_CorrespondenceDTO viewfirsttabdetails(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/viewfirsttabdetails");
        }
        public FMS_CorrespondenceDTO activedeactivesubdetails(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/activedeactivesubdetails");
        }
        public FMS_CorrespondenceDTO searchrefernceno(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/searchrefernceno");
        }

        /*GENERAL DETILS FOR SEARCH REFERNCE NO. */
        public FMS_CorrespondenceDTO searchreferncenosecondtab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/searchreferncenosecondtab");
        }
        public FMS_CorrespondenceDTO OnChangeDept(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/OnChangeDept");
        }

        /*SECOND TAB */
        public FMS_CorrespondenceDTO onsecondtabcorrespondence(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/onsecondtabcorrespondence");
        }
        public FMS_CorrespondenceDTO onchangefilecabinet(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/onchangefilecabinet");
        }
        public FMS_CorrespondenceDTO savesecondtabcorrespondence(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/savesecondtabcorrespondence");
        }
        public FMS_CorrespondenceDTO Editdatasecondtab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/Editdatasecondtab");
        }
        public FMS_CorrespondenceDTO onchangerefnosecondtab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/onchangerefnosecondtab");
        }
        public FMS_CorrespondenceDTO activedeactivesecondtab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/activedeactivesecondtab");
        }
        public FMS_CorrespondenceDTO ViewCorrespondanceFiles(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/ViewCorrespondanceFiles");
        }
        public FMS_CorrespondenceDTO DeleteCorrespondanceFile(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/DeleteCorrespondanceFile");
        }
        public FMS_CorrespondenceDTO OnChangephysicalpath(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/OnChangephysicalpath");
        }

        /*THIRD TAB */
        public FMS_CorrespondenceDTO onthirdtabcorrespondence(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/onthirdtabcorrespondence");
        }
        public FMS_CorrespondenceDTO onchangerefnothirdtab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/onchangerefnothirdtab");
        }
        public FMS_CorrespondenceDTO savethirdtabcorrespondence(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/savethirdtabcorrespondence");
        }
        public FMS_CorrespondenceDTO Editdatathirdtab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/Editdatathirdtab");
        }
        public FMS_CorrespondenceDTO activedeactivethirdtab(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/activedeactivethirdtab");
        }
        public FMS_CorrespondenceDTO OnChangeLetterHeadNo(FMS_CorrespondenceDTO data)
        {
            return _comm.FMSGetPostData(data, "FMS_CorrespondenceFacade/OnChangeLetterHeadNo");
        }
        
    }
}
