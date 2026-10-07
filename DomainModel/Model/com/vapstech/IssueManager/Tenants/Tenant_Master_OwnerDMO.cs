using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{

    [Table("Tenant_Master_Owner")]
    public class Tenant_Master_Owner : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTAGROWN_Id { get; set; }
        public string TENANTAGROWN_Name { get; set; }
        public long TENANTAGROWN_ContactNo { get; set; }
        public string TENANTAGROWN_EmailID { get; set; }
        public long HRMBD_Id { get; set; }
        public DateTime TENANTAGROWN_DOB { get; set; }
        //public long Createdby { get; set; }
        public bool TENANTAGROWN_ActiveFlag { get; set; }
        //public long Updateby { get; set; }

    }

}

