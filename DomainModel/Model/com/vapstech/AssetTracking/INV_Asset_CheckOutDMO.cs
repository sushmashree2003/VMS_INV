using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("INV_Asset_CheckOut", Schema = "INV")]
    public class INV_Asset_CheckOutDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVACO_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public long INVMLO_Id { get; set; }
        public DateTime INVACO_CheckoutDate { get; set; }
        public decimal INVACO_CheckOutQty { get; set; }
        public string INVACO_ReceivedBy { get; set; }
        public string INVACO_CheckOutRemarks { get; set; }
        public string INVACO_ItemRemarks { get; set; }
        public string INVACO_QuantityRemark { get; set; }
        public string INVACO_No { get; set; }
        public bool INVACO_ActiveFlg { get; set; }
        public long? HRME_Id { get; set; }
        public string INVACO_Tolocation { get; set; }
        public string INVACO_ISSUED { get; set; }
        public string INVACO_MODE { get; set; }
        public string INVMLO_Status { get; set; }
        public string INVACO_Vehicalno { get; set; }
        public string INVACO_Time { get; set; }
        public long INVACO_CreatedBy { get; set; }
        public long INVACO_UpdatedBy { get; set; }
        public string INVACO_CallCenterRemarks { get; set; }
    }
}
