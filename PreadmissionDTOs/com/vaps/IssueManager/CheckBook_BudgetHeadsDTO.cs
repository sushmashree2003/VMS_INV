using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class CheckBook_BudgetHeadsDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long? HRMD_Id { get; set; }
        public Array allcompany { get; set; }
        public long VMBDGHDAMT_Id { get; set; }
        public long VMBDGHD_Id { get; set; }
        public long IMFY_ID { get; set; }
        public decimal VMBDGHDAMT_Amount { get; set; }
        public decimal? VMBDGHDAMT_MonthwiseAmount { get; set; }
        public string VMBDGHDAMT_Remarks { get; set; }
        public bool VMBDGHDAMT_ActiveFlg { get; set; }
        public string VMBDGHD_BudgetHead { get; set; }
        public string VMBDGHDN_SatuswiseFlg { get; set; }
        public string VMBDGHDAMTN_Name { get; set; }
        public bool VMBDGHD_MonthwiseBudgetFlg { get; set; }
        public string VMBDGHD_Remarks { get; set; }
        public bool VMBDGHD_ActiveFlg { get; set; }
        public string returnval { get; set; }
        public Array alldata { get; set; }
        public Array DepartmentArray { get; set; }
        public Array OverallArray { get; set; }
        public Array OverallTotal { get; set; }
        public string Flag { get; set; }
        public Array FinancialYear { get; set; }
        public Array BudgetArray { get; set; }
        public Array CheckBookBudget { get; set; }
        public Array BudgetReport { get; set; }
        public Array BudgetReportotal { get; set; }
        public Array deptList { get; set; }
        public BudgetHead[] BudgetHeadlist { get; set; }
        public string Mi_logo { get; set; }
        public long? ISMMCLT_Id { get; set; }

        public Array clientList { get; set; }
        public MultipleComapnyBudget[] MultipleComapnyBudget { get; set; }
        public Array Month_List { get; set; }

        public DepartmentSelected[] departmentSelected { get; set; }


        public temp_AmountMonth[] Amount_List { get; set; }


        //newly added
        public budgetUpdate[] saveremrksdata { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDC_Name { get; set; }
        public string VMBDGHDN_BudgetHead { get; set; }



        public temp_MonthList[] temMonth_List { get; set; }
        public long MonthId { get; set; }

        //Month
        public long VMBDGHDAMTNM_Id { get; set; }  // Primary key
        public decimal VMBDGHDAMTNM_Amount { get; set; }
        public long VMBDGHDAMTN_Id { get; set; }   // Foreign key to VMBDGHDAMTN
        public long VMBDGHDAMTNM_MonthId { get; set; } // Month ID
        public bool VMBDGHDAMTNM_ActiveFlag { get; set; } // Active Flag
        public long VMBDGHDAMTNM_CreatedBy { get; set; } // Created by user
        public long VMBDGHDAMTNM_UpdatedBy { get; set; } // Updated by user


    }
    public class BudgetHead
    {
        public long VMBDGHD_Id { get; set; }
        public long MI_Id { get; set; }
    }
    public class MultipleComapnyBudget
    {
        public long MI_Id { get; set; }
    }
    public class temp_MonthList
    {
        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
    }

    public class temp_AmountMonth
    {
        public long VMBDGHDAMTNM_MonthId { get; set; }
        public decimal VMBDGHDAMTNM_Amount { get; set; }
    }
    public class DepartmentSelected
    {
        public long HRMD_Id { get; set; }

    }

    public class budgetUpdate
    {
        public long VMBDGHDAMTN_Id { get; set; }
        public long HRMD_Id { get; set; }
        public decimal TotalAmount { get; set; }
        public long VMBDGHDN_Id { get; set; }
        //public string January { get; set; }
        //public string February { get; set; }
        //public string March { get; set; }
        //public string April { get; set; }
        //public string May { get; set; }
        //public string June { get; set; }
        //public string July { get; set; }
        //public string August { get; set; }
        //public string September { get; set; }
        //public string October { get; set; }
        //public string November { get; set; }
        //public string December { get; set; }

    }


}
