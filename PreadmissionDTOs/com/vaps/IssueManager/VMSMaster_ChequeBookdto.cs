using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
  public  class VMSMaster_ChequeBookdto
    {
        public long VMCHEBK_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRMBD_Id { get; set; }
        public DateTime? VMCHEBK_ReceivedDate { get; set; }
        public long VMCHEBK_NoOfChequeLeafs { get; set; }
        public string VMCHEBK_SLNoFrom { get; set; }
        public string VMCHEBK_SLNoTo { get; set; }
        public string VMCHEBK_Remarks { get; set; }
        public bool VMCHEBK_ActiveFlg { get; set; }
        public long VMCHEBK_CreatedBy { get; set; }
        public Array get_supplier { get; set; }
        public DateTime? VMCHEBK_CreatedDate { get; set; }
        public long VMCHEBK_UpdatedBy { get; set; }
        public DateTime? VMCHEBK_UpdatedDate { get; set; }
        public bool returnval { get; set; }
        public int failcnt { get; set; }
       public VMSMaster_ChequeBook_SLNoDTO[] slnocheck { get; set; }
        public Array MasterChecks { get; set; }
        public Array banklist { get; set; }
        public string HRMBD_BankName { get; set; }
        public string HRMBD_BankAccountNo { get; set; }
        public string MI_Name { get; set; }
        public Array edtilistM { get; set; }
        public Array edtilistP { get; set; }
        public MI_list[] emplist { get; set; }
        public string INVMS_SupplierName { get; set; }
        public long INVMS_Id { get; set; }
        public long VPAYVOU_Id { get; set; }
        public string VPAYVOU_Idtwo { get; set; }
        public string optionflag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public HRMD_list[] hrmdlist { get; set; }
        public Array checckleafTotal { get; set; }
        public Inmvts_list[] Inmvs_list { get; set; }
        public Array checkarray { get; set; }
        public string VPAYVOU_ChequeNo { get; set; }
    }
    public class Inmvts_list
    {
        public long INVMS_Id { get; set; }
    }
    public class HRMD_list
    {
        public long HRMD_Id { get; set; }
    }
    public class MI_list
    {
        public long MI_Id { get; set; }
    }
}
