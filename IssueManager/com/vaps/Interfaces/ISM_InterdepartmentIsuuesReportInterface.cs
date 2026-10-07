using PreadmissionDTOs.com.vaps.IssueManager;
using System.Threading.Tasks;
namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_InterdepartmentIsuuesReportInterface 
    {
        Task<ISM_InterDepartmentReport> getdetails(ISM_InterDepartmentReport data);
        //ISM_InterDepartmentReport getreport(ISM_InterDepartmentReport data);
        ISM_InterDepartmentReport SendEmail(ISM_InterDepartmentReport data);
    }
}
