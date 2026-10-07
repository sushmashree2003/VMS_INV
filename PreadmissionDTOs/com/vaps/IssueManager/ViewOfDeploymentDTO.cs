using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class ViewOfDeploymentDTO
    {
        public long VPSD_Id { get; set; }
        public string VPSD_Name { get; set; }
        public DateTime? VPSD_Date { get; set; }
        public string VPSD_Environment { get; set; }
        public string VPSD_Path { get; set; }
        public string VPSD_CodeType { get; set; }
        public string VPSD_ClientName { get; set; }
        public bool VPSD_ActiveFlag { get; set; }
        public long VPSD_CreatedBy { get; set; }

        public long VPSD_UpdatedBy { get; set; }
        public DateTime VPSD_UpdatedDate { get; set; }
        public DateTime VPSD_CreatedDate { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array get_mi_list { get; set; }

        public Array get_client { get; set; }
        public Array get_tasks { get; set; }
        public Array get_taskdetails { get; set; }
        public envlistarray1[] envlistarray { get; set; }
        public Array tasks_list { get; set; }
        public long ISMMCLT_Id { get; set; }
       public string  ISMMCLT_ClientName { get; set; }
        public complistarray1[] complistarray { get; set; }

        public clientlistarray1[] clientlistarray { get; set; }
        public tasklistarray1[] tasklistarray { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string returnduplicatestatus { get; set; }
        public string MI_name { get; set; }
        public string Client_Name { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMTCR_Title { get; set; }
        public string ISMTCR_Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        public bool returnval { get; set; }
        public long saved_count { get; set; }
        //scope report
       public Array get_report { get; set; }

        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }


        //added by adarsh ms
        public long IVRMCH_Id { get; set; }
        public long IVRMVT_Id { get; set; }
        public string flag { get; set; }
        public Array version_list { get; set; }
        public Array get_scope_approval_tasklist { get; set; }
        public long HRME_ID { get; set; }
        public featurestasklist[] featurestasklist { get; set; }
        public Approved_rejected_socpe_tasks[] Approved_rejected_socpe_tasks { get; set; }
    }
    public class complistarray1
    {
        public long MI_Id { get; set; }
    }
    public class clientlistarray1
    {
        public long ISMMCLT_Id { get; set; }
    }
    public class envlistarray1
    {
        public string VPSD_Environment { get; set; }
    }
    public class tasklistarray1
    {
        public long ISMTCR_Id { get; set; }
    }
    public class featurestasklist
    {
        public long IVRMVFD_Id { get; set; }
        public long IVRMVT_Id { get; set; }
        public string IVRMVFD_Features { get; set; }
        public long ISMTCR_Id { get; set; }
        public string IVRMVFD_Status { get; set; }
        public string IVRMVFD_Remarks { get; set; }
        public long closeflag { get; set; }
        public string IVRMVFD_Test_Case { get; set; }
        public string IVRMVFD_Test_Case_File_Name { get; set; }

    }
    public class Approved_rejected_socpe_tasks
    {
        public long IVRMVFD_Id { get; set; }
        
        public string IVRMVFD_Features { get; set; }
        public string IVRMVFD_Status { get; set; }
        public string IVRMVFD_Remarks { get; set; }
        public string IVRMVFD_Test_Case { get; set; }
        public string IVRMVFD_Test_Case_File_Name { get; set; }
        public bool IVRMVFD_ApprovedFlag { get; set; }
        public bool check { get; set; }
    }
}
