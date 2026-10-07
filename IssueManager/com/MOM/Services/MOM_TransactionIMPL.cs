using DataAccessMsSqlServerProvider;
using IssueManager.com.MOM.Interfaces;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.MOM.Services
{
    public class MOM_TransactionIMPL : Interfaces.MOM_TransactionInterface
    {
        public DomainModelMsSqlServerContext _db;
        public MOM_TransactionIMPL(DomainModelMsSqlServerContext abc)
        {
            _db = abc;
        }
        public MOM_TransactionDTO getDetails(MOM_TransactionDTO data)
        {
            try
            {
                data.staffData = _db.HR_Master_Employee_DMO.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        public MOM_TransactionDTO savedata(MOM_TransactionDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

    
    }
}
