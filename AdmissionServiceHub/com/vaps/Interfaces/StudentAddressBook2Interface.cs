using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentAddressBook2Interface
    {


        Task<StudentAddressBook2DTO> getInitailData(int id);
        //Task<StudentAddressBook2DTO> getclass(StudentAddressBook2DTO id);
        //Task<StudentAddressBook2DTO> getyear(StudentAddressBook2DTO id);
        //Task<StudentAddressBook2DTO> getsection(StudentAddressBook2DTO id);
        Task<StudentAddressBook2DTO> getdetails(StudentAddressBook2DTO resource);
        Task<StudentAddressBook2DTO> sectionchange(StudentAddressBook2DTO id);
        Task<StudentAddressBook2DTO> yearchange(StudentAddressBook2DTO id);
        Task<StudentAddressBook2DTO> classchange(StudentAddressBook2DTO id);
        //StudentAddressBook1DTO getinitialdata(int id);
        //StudentAddressBook1DTO getinitialdata(int id);
        //StudentAddressBook1DTO getinitialdata(StudentAddressBook1DTO id);
        StudentAddressBook2DTO getdetailsstdemp(StudentAddressBook2DTO id);
        
    }
}
