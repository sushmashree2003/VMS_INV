using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class EventsStudentRecordDTO:CommonParamDTO
    {
        public long SPCCEST_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCME_Id { get; set; }
        public long SPCCMCL_Id { get; set; }
        public long SPCCMCC_Id { get; set; }
        public long SPCCMSCC_Id { get; set; }
        public long? SPCCMH_Id { get; set; }
        public long SPCCMUOM_Id { get; set; }
        public string SPCCEST_House_Class_Flag { get; set; }
        public string SPCCEST_OldRecord { get; set; }
        public string SPCCEST_Remarks { get; set; }
        public bool SPCCEST_ActiveFlag { get; set; }


        public long SPCCESTR_Id { get; set; }       
        public long AMST_Id { get; set; }
        public string SPCCESTR_Rank { get; set; }
        public double? SPCCESTR_Points { get; set; }
        public string SPCCESTR_RecordBrokenFlag { get; set; }
        public string SPCCESTR_Remarks { get; set; }
        public bool SPCCESTR_ActiveFlag { get; set; }
        public long SPCCMSCCG_Id { get; set; }

        public long house_id { get; set; }        
        public long house_idcls { get; set; }        
        public long class_id { get; set; }        
        public string housename { get; set; }
        public string classname { get; set; }
        public string housenamecls { get; set; }
        public long comcat_ids { get; set; }
        public long comcat_idcls { get; set; }
        public string comptcatename { get; set; }
        public string comptcatenamecls { get; set; }
       

        public string AMST_AdmNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string SPCCMSCCG_SCCFlag { get; set; }
        public Array groupsportdata { get; set; }
        public Array categoryListtt2 { get; set; }
        public Array houselistedit { get; set; }
        public Array houselistclass { get; set; }
        public Array categoryListttCls { get; set; }



        public int count { get; set; }
        public string returnVal { get; set; }
        public Array events { get; set; }
        public Array eventname { get; set; }
        public string radiotype { get; set; }
        public Array classList { get; set; }
        public Array listof_class { get; set; }
        public Array sectionList { get; set; }
        public Array studentList { get; set; }
        public Array compitionLevelList { get; set; }
        public Array categoryList { get; set; }
        public Array sportsCCList { get; set; }
        public Array uomList { get; set; }
        public Array eventsStudentRecordList { get; set; }
        public Array editDetails { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string studentName { get; set; }
        public string eventName { get; set; }
        public string compitionLevel { get; set; }
        public string categoryName { get; set; }
        public string sportsName { get; set; }
        public string uomName { get; set; }
        public string ASMAY_Year { get; set; }
        //public EventsStudentRecordDTO[] student { get; set; }
        public EventsStudentRecordDTO[] selectedSectionlist { get; set; }
        public EventsStudentRecordDTO[] houslistdat { get; set; }
        public EventsStudentRecordDTO[] houslistdattt2 { get; set; }
        public EventsStudentRecordDTO[] clslistdat { get; set; }
        public EventsStudentRecordDTO[] clslistdat234 { get; set; }
        public EventsStudentRecordDTO[] hosueclssecllist { get; set; }

        public Array academicYear { get; set; }       
        public string SPCCPM_Name { get; set; }

        public string SPCCMH_HouseName { get; set; }
        public string evename { get; set; }
        public bool sendmail { get; set; }
        public bool sendsms { get; set; }
        public long? fatherno { get; set; }
        public Array houselist { get; set; }
        public bool returnVal2 { get; set; }
        public Array modlastudlist { get; set; }
        public Array datastudentas { get; set; }
        public Array studlistdata { get; set; }

        public string SPCCME_EventName { get; set; }
        public string SPCCMCL_CompitionLevel { get; set; }
        public string SPCCMCC_CompitionCategory { get; set; }
        public string SPCCMSCC_SportsCCName { get; set; }
        public string SPCCMUOM_UOMName { get; set; }
        public string SPCCMSCCG_SportsCCGroupName { get; set; }
        public Array editstulist { get; set; }
        public Array editClsSecYear { get; set; }
        public int ASMCL_Order { get; set; } 
        
        public student1[] student1 { get; set; }
        public studentids[] studentids { get; set; }
       public EventsStudentRecordDTO[] sectonlst { get; set; }

        

        //Kiosk Sports Winner List.
        public class SportsWinnersDTO
        {
            public string studentName { get; set; }
            public string eventName { get; set; }
            public string sportsName { get; set; }
            public int SPCCESTR_Place { get; set; }
            public string studentPhotoPath { get; set; }
            public SportsWinnersDTO[] winnerList { get; set; }
        }

    }
    public class studentids
    {
        public long amsT_Id { get; set; }
    }
    public class student1
    {
        public long amsT_Id { get; set; }
        public double? spccestR_Points { get; set; }
        public string spccestR_Rank { get; set; }
        public string spccestR_Remarks { get; set; }
        public string spccestR_RecordBrokenFlag { get; set; }

    }

}
