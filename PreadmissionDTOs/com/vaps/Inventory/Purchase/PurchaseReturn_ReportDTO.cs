using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class PurchaseReturn_ReportDTO
    {

        public long MI_Id { get; set; }

        public Array GRNETLIst
        { get; set; }

        public DateTime? From_Date { get; set; }

        public DateTime? To_Date { get; set; }
        public sectionselect[] INVMGRNRET_Id_Filter { get; set; }

        public Array getReport { get; set; }
        public string INVMGRNRET_GRNRETNo { get; set; }
        public string INVMI_ItemName { get; set; }

        public decimal INVTGRNRET_ReturnQty { get; set; }

        public decimal INVMGRNRET_TotalAmount { get; set; }

        public decimal INVTGRNRET_ReturnAmount { get; set; }

        public string INVMGRNRET_Remarks { get; set; }
        public long? INVMGRNRETAPP_Id { get; set; }

        public long INVMGRNET_Id { get; set; }
        public string CheckValues { get; set; }
    }
    public class sectionselect
    {
        public long INVMGRNRET_Id { get; set; }
       
    }
}
