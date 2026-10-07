using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface EnquiryInterface
    {
        //string enqdata(Enq id);
        Enq countrydrp(Enq en);

        StateDTO enqdrpcountrydata(int id);
        Task<Enq>  saveEnqdata(Enq enqu);
        Enq clearEnqdata(int Id);
        CityDTO getcity(int id);
        Enq EditDetails(int  id);
        Enq DeleteEnqDetails(Enq enqu);
        //Enq GetAllDetais(Enq en);

    }
}
