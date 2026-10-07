using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.Tally
{
    public class TallyMTransactionDTO: CommonParamDTO
    {
        public long TMT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime TMT_Date { get; set; }
        public bool TMT_VoucherTypeFlg { get; set; }
        public long TMT_VoucherNo { get; set; }
        public decimal TMT_Amount { get; set; }
        public bool TMT_TransactionStatusFlg { get; set; }
        public bool TMT_TransactionTypeFlg { get; set; }
        public bool TMT_APIStatusFlg { get; set; }
        public string TMT_ChequeNo { get; set; }

        public DateTime TMT_ChequeDate { get; set; }
        public long TMT_RefNo { get; set; }
        public int TMT_FinancialYear { get; set; }

        public bool TMT_ActiveFlg { get; set; }
        public long ASMAY_Id { get; set; }
        public Array academicdrp { get; set; }
        public Array fillconfig { get; set; }
        public Array allclsdata { get; set; }
        public Array allinsdata { get; set; }
        public Array fillstudent { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public Array TempararyArrayList { get; set; }

        public long ASMCL_Id { get; set; }
        public TallyMTransactionDTO[] savetmpdata { get; set; }
        public string returnval { get; set; }

        public string ftiidss { get; set; }
        public long[] Amst_Ids { get; set; }
        public DateTime? From_Date { get; set; }
        public DateTime? To_Date { get; set; }

        public string AMST_AdmNo { get; set; }
        public Array tallyoutput { get; set; }

    }
}
