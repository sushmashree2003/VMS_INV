using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_SciencePark_Transaction")]
    public class ISM_SciencePark_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMST_Id { get; set; }

        public string ItemName { get; set; }
        public string Cost { get; set; }
        public string FirstYear { get; set; }
        public string FirstYearNew { get; set; }
        public string SecondYear { get; set; }

        public string SecondYearNew { get; set; }
        public string ThirdYear { get; set; }
        public string ThirdYearNew { get; set; }
        public string FourYear { get; set; }
        public string FourYearNew { get; set; }
        public string FifthYear { get; set; }
        public string FifthYearNew { get; set; }
        public long? ISMSBOM_Id { get; set; }
        public long? ISMSLE_Id { get; set; }

        public string PakageType { get; set; }
        public string ScienceParkFlag { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
