using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class Lead_To_ClientIMPL : Interfaces.Lead_To_Clientinterface
    {
        public Lead_To_ClientContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public Lead_To_ClientIMPL(Lead_To_ClientContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public Lead_To_ClientDTO getdetails(Lead_To_ClientDTO data)
        {
            try
            {
                data.leadlist_dd = _IssueContext.ISM_Sales_Lead_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSLE_ActiveFlag == true).Distinct().ToArray();
                data.project_list = _IssueContext.MastersProject_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMPR_ActiveFlg == true).Distinct().ToArray();
                //data.get_department = (from a in _IssueContext.HR_Master_Department
                //                       where (a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                //                       select new ISM_TaskCreationDTO
                //                       {
                //                           HRMD_Id = a.HRMD_Id,
                //                           HRMD_DepartmentName = a.HRMD_DepartmentName
                //                       }).Distinct().ToArray();
                data.components_dd = _IssueContext.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMCLTC_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO OnChangeTab1Inst(Lead_To_ClientDTO data)
        {
            try
            {
                if(data.complist !=null && data.complist.Length > 0)
                {
                    List<long> ISMCLTC_Id = new List<long>();
                    foreach (var t in data.complist)
                    {
                        ISMCLTC_Id.Add(t.ISMCLTC_Id);
                    }
                    data.components_dd = _IssueContext.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMCLTC_ActiveFlag == true &&  !ISMCLTC_Id.Contains(a.ISMCLTC_Id)).ToArray();

                }
                else
                {
                    data.components_dd = _IssueContext.ISM_Client_Master_Components_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMCLTC_ActiveFlag == true).ToArray();

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO saveClientdata(Lead_To_ClientDTO data)
        {
            try
            {
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO clientDecative(Lead_To_ClientDTO data)
        {
            try
            {
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public Lead_To_ClientDTO editClientdata(Lead_To_ClientDTO data)
        {
            try
            {

             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO OnChangeTab2Inst(Lead_To_ClientDTO data)
        {
            try
            {

                try
                {
                   
                    data.module_list = (from a in _IssueContext.MasterModules
                                        from b in _IssueContext.MastersModule_DMO
                                        from c in _IssueContext.MastersProject_DMO
                                        where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMPR_Id == c.ISMMPR_Id && b.MI_Id == c.MI_Id
                                        && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == data.ISMMPR_Id)
                                        select new ISM_ClientProject_MappingDTO
                                        {
                                            IVRMM_Id = b.IVRMM_Id,
                                            ISMMMD_Id = b.ISMMMD_Id,
                                            IVRMM_ModuleName = a.IVRMM_ModuleName,
                                        }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();
                    //data.module_list = (from a in _IssueContext.MasterModules
                    //                    from b in _IssueContext.MastersModule_DMO
                    //                    from c in _IssueContext.MastersProject_DMO
                    //                    where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMPR_Id == c.ISMMPR_Id && b.MI_Id == c.MI_Id
                    //                    && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == data.ISMMPR_Id && b.HRMD_Id == data.HRMD_Id)
                    //                    select new ISM_ClientProject_MappingDTO
                    //                    {
                    //                        IVRMM_Id = b.IVRMM_Id,
                    //                        ISMMMD_Id = b.ISMMMD_Id,
                    //                        IVRMM_ModuleName = a.IVRMM_ModuleName,
                    //                    }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO saveClientMappingdata(Lead_To_ClientDTO data)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO editClientMappingdata(Lead_To_ClientDTO data)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO deactiveClientMappingdata(Lead_To_ClientDTO data)
        {
            try
            {
              
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO modalListdata(Lead_To_ClientDTO data)
        {
            try
            {
                data.project_list = (from a in _IssueContext.clientTable
                                     from b in _IssueContext.MastersProject_DMO
                                     from c in _IssueContext.MastersModule_DMO
                                     where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.ISMMCLT_ActiveFlag == true && b.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == c.ISMMPR_Id && c.HRMD_Id == data.HRMD_Id)
                                     select new ISM_ClientProject_MappingDTO
                                     {
                                         ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                         ISMMPR_Id = b.ISMMPR_Id
                                     }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        //VMS Client And IVRM Client Mapping
        public Lead_To_ClientDTO OnChangeClientTab3(Lead_To_ClientDTO data)
        {
            try
            {
              
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public Lead_To_ClientDTO SaveVMSIVRMMapping(Lead_To_ClientDTO data)
        {
            try
            {
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
    }
}
