using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Amount")]
    public class FeeAmountEntryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMA_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMG_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long FTI_Id { get; set; }
        public decimal FMA_Amount { get; set; }
        public string FMA_Flag { get; set; }
        public long FMH_Id { get; set; }
    }
}
