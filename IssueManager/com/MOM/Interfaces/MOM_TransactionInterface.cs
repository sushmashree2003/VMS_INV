using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.MOM.Interfaces
{
    public interface MOM_TransactionInterface
    {
        MOM_TransactionDTO getDetails(MOM_TransactionDTO data);
        MOM_TransactionDTO savedata(MOM_TransactionDTO data);
    }
}
