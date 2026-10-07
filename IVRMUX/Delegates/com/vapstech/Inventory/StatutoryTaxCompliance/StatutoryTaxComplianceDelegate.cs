using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.StatutoryTaxCompliance
{
    public class StatutoryTaxComplianceDelegate
    {
        CommonDelegate<StatutoryTaxComplianceDTO, StatutoryTaxComplianceDTO> COMMI = new CommonDelegate<StatutoryTaxComplianceDTO, StatutoryTaxComplianceDTO>();

        public StatutoryTaxComplianceDTO getdetails(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/getdetails/");
        }
        public StatutoryTaxComplianceDTO SaveTabOne(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/SaveTabOne/");
        }
        public StatutoryTaxComplianceDTO GetReference(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/GetReference/");
        }
        public StatutoryTaxComplianceDTO Getincome(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/Getincome/");
        }
        public StatutoryTaxComplianceDTO viewData(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/viewData/");
        }
        public StatutoryTaxComplianceDTO SaveTabtwo(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/SaveTabtwo/");
        }
        public StatutoryTaxComplianceDTO Savedetails(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/SaveTabOne/");
        }
        public StatutoryTaxComplianceDTO SaveTabThree(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/SaveTabThree/");
        }
        public StatutoryTaxComplianceDTO SaveTabFour(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/SaveTabFour/");
        }
        public StatutoryTaxComplianceDTO SaveTabFive(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/SaveTabFive/");
        }
        public StatutoryTaxComplianceDTO SaveTabSix(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/SaveTabSix/");
        }
        public StatutoryTaxComplianceDTO SaveTabSeven(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/SaveTabSeven/");
        }
        public StatutoryTaxComplianceDTO ViewFiles(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/ViewFiles/");
        }
        public StatutoryTaxComplianceDTO Edit(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/Edit/");
        }
        public StatutoryTaxComplianceDTO Consultantreport(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/Consultantreport/");
        }
        public StatutoryTaxComplianceDTO Casereports(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/Casereports/");
        }
        public StatutoryTaxComplianceDTO ClosedCasereports(StatutoryTaxComplianceDTO data)
        {
            return COMMI.POSTDataInventory(data, "StatutoryTaxComplianceFacade/ClosedCasereports/");
        }
    }
}
