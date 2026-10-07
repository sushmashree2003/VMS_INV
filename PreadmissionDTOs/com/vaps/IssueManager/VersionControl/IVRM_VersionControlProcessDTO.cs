using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.VersionControl
{
    public class IVRM_VersionControlProcessDTO
    {
        public long ISMMCLT_Id { get; set; }
        public Array clientList { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long roleId { get; set; }
        public long IVRMCH_Id { get; set; }
        public long IVRMVT_Id { get; set; }
        public string returnval { get; set; }
        public string IVRMCH_HostName { get; set; }
        public string IVRMVT_VersionName { get; set; }
        public string IVRMCH_HostUrl { get; set; }
        public bool IVRMVT_ActiveFlag { get; set; }
        public DateTime ? IVRMVT_DeployementDate { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public Array getclientlist { get; set; }
        public Array editdata { get; set; }
        public Array getgriddata { get; set; }
        public Array clientHosting { get; set; }
        public Array taskidlist { get; set; }
        public Array versionlist { get; set; }
        public getClientList[] getClientList { get; set; }
        public getFeaturelist[] getFeaturelist { get; set; }
        ///added by adarsh ms
        public DateTime lastdeployeddate { get; set; } 
        public string lastdeployedversion { get; set; }
        public Array gettaskforapproval { get; set; }
        public Array getversionapprovedlist { get; set; }
        public DateTime deploymentdate { get; set; }
        public temtasklist[] temtasklist { get; set; }
        public string message { get; set; }
        public string IVRMVRT_DeployType { get; set; }
        public string flag { get; set; }
        public string currentversion { get; set; }
        public string updatedversion { get; set; }
        public string ISMMCLT_IVRM_URL { get; set; }
        public Array IVRM_Version { get; set; }


        ///////
    }
    public class getClientList
    {
        public long ISMMCLT_Id { get; set; }
    }
    public class getFeaturelist
    {
        public long ISMTCR_Id { get; set; }
    }
    //added by -- adarsh ms
    public class temtasklist
    {
        public long IVRMVT_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long IVRMCH_Id { get; set; }
        public int IVRMVRT_ManHour { get; set; }
        public string IVRMVRT_AddedFeature { get; set; }
        public bool IVRMVRT_ApprovedFlag { get; set; }
        public string IVRMVRT_ApprovedBy { get; set; }
        public string IVRMVRT_DeployType { get; set; }
        public string IVRMVRT_Remarks { get; set; }


    }
}
