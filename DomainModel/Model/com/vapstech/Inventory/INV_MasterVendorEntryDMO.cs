using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_MasterVendorEntry")]
    public class INV_MasterVendorEntryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long MVE_Id { get; set; }
            public long VCM_Id { get; set; }
            public long MI_Id { get; set; }

            public string MVE_VendorName { get; set; }
            public string MVE_CompanyName { get; set; }

            public string MVE_PAN_Number { get; set; }
            public string MVE_MSME_Register { get; set; }

            public long MVE_MobileNumber { get; set; }

            public long MVE_Createdby { get; set; }
            public long MVE_Updatedby { get; set; }

            public bool MVE_Activeflag { get; set; }

            public DateTime? MVE_Createddate { get; set; }
            public DateTime? MVE_Updateddate { get; set; }

            public string MVE_Register_OfficeAddress { get; set; }

            public string MVE_Account_name { get; set; }
            public string MVE_Account_Number { get; set; }

            public string MVE_Service_Offered { get; set; }

            public string MVE_GST_Number { get; set; }
            public string MVE_CIN_Number { get; set; }

            public string MVE_Contact_Person { get; set; }
            public string MVE_Email_Address { get; set; }

            public string MVE_Bank_Name { get; set; }
            public string MVE_Branch_Name { get; set; }
            public string MVE_IFSC_Code { get; set; }

            public string MVE_Payment_Terms { get; set; }

            public DateTime? MVE_Agreement_Startdate { get; set; }
            public DateTime? MVE_Agreement_Enddate { get; set; }

            public string MVE_GST_Certificate_filepath { get; set; }
            public string MVE_PAN_Upload_filepath { get; set; }
            public string MVE_Bank_Details_filepath { get; set; }
            public string MVE_MOU_filepath { get; set; }
            public string MVE_Company_Register_Certificate_filepath { get; set; }
            public string MVE_MSME_Certificate_filepath { get; set; }
            public string MVE_ISO_Certificate_filepath { get; set; }
            public string MVE_Workorder_filepath { get; set; }
            public string ApprovalStatus { get; set; }



    }
}
