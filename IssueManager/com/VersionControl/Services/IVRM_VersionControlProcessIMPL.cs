using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.VersionControl;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager.VersionControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.VersionControl.Services
{
    public class IVRM_VersionControlProcessIMPL : Interfaces.IVRM_VersionControlProcessInterface
    {


        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public IVRM_VersionControlProcessIMPL(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }

        //===========Client Hosting Master
        public IVRM_VersionControlProcessDTO getdetails(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                var mappepd_client_list = _IssueContext.IVRM_Hosting_Client_MappingDMO.Where(m => m.IVRMHCM_ActiveFlag == true).Select(m => m.ISMMCLT_Id).ToList();

                data.getclientlist = _IssueContext.clientTable.Where(t => t.ISMMCLT_ActiveFlag == true && !mappepd_client_list.Contains(t.ISMMCLT_Id) && t.MI_Id==17 ).ToArray();





                data.getgriddata = _IssueContext.IVRM_Client_HostingDMO.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_VersionControlProcessDTO savedata(IVRM_VersionControlProcessDTO data)
        {
            try
            {
                if (data.IVRMCH_Id > 0)
                {
                    //var dublicatecheck = _IssueContext.IVRM_Client_HostingDMO.Where(t => t.IVRMCH_HostName == data.IVRMCH_HostName && t.IVRMCH_HostUrl == data.IVRMCH_HostUrl && t.IVRMCH_Id!=data.IVRMCH_Id).ToList();
                    //if (dublicatecheck.Count() > 0)
                    //{
                    //    data.returnval = "Duplicate";
                    //}
                    //else
                    //{
                    var update = _IssueContext.IVRM_Client_HostingDMO.Single(t => t.IVRMCH_Id == data.IVRMCH_Id);
                    update.IVRMCH_HostName = data.IVRMCH_HostName;
                    update.IVRMCH_HostUrl = data.IVRMCH_HostUrl;
                    update.Updateddate = DateTime.Now;
                    update.Updatedby = data.User_Id;
                    _IssueContext.Update(update);

                    var deleteexisting = _IssueContext.IVRM_Hosting_Client_MappingDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).ToList();
                    if (deleteexisting.Count > 0)
                    {
                        foreach (var item in deleteexisting)
                        {
                            _IssueContext.Remove(item);
                        }
                    }
                    if (data.getClientList.Length > 0)
                    {
                        foreach (var obj in data.getClientList)
                        {
                            IVRM_Hosting_Client_MappingDMO IHCM = new IVRM_Hosting_Client_MappingDMO();
                            IHCM.IVRMCH_Id = data.IVRMCH_Id;
                            IHCM.ISMMCLT_Id = obj.ISMMCLT_Id;
                            IHCM.IVRMHCM_ActiveFlag = true;
                            IHCM.Createddate = DateTime.Now;
                            IHCM.Updateddate = DateTime.Now;
                            IHCM.Createdby = data.User_Id;
                            IHCM.Updatedby = data.User_Id;
                            _IssueContext.Add(IHCM);
                        }
                    }

                    var saveddata = _IssueContext.SaveChanges();
                    if (saveddata > 0)
                    {
                        data.returnval = "Update";
                    }
                    else
                    {
                        data.returnval = "NotAdd";
                    }



                    //}
                }
                else
                {
                    var dublicatecheck = _IssueContext.IVRM_Client_HostingDMO.Where(t => t.IVRMCH_HostName == data.IVRMCH_HostName && t.IVRMCH_HostUrl == data.IVRMCH_HostUrl).ToList();
                    if (dublicatecheck.Count() > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        IVRM_Client_HostingDMO ICH = new IVRM_Client_HostingDMO();
                        ICH.IVRMCH_HostName = data.IVRMCH_HostName;
                        ICH.IVRMCH_HostUrl = data.IVRMCH_HostUrl;
                        ICH.IVRMCH_ActiveFlag = true;
                        ICH.Createddate = DateTime.Now;
                        ICH.Updateddate = DateTime.Now;
                        ICH.Createdby = data.User_Id;
                        ICH.Updatedby = data.User_Id;
                        _IssueContext.Add(ICH);
                        _IssueContext.SaveChanges();
                        if (data.getClientList.Length > 0)
                        {
                            foreach (var obj in data.getClientList)
                            {
                                IVRM_Hosting_Client_MappingDMO IHCM = new IVRM_Hosting_Client_MappingDMO();
                                IHCM.IVRMCH_Id = ICH.IVRMCH_Id;
                                IHCM.ISMMCLT_Id = obj.ISMMCLT_Id;
                                IHCM.IVRMHCM_ActiveFlag = true;
                                IHCM.Createddate = DateTime.Now;
                                IHCM.Updateddate = DateTime.Now;
                                IHCM.Createdby = data.User_Id;
                                IHCM.Updatedby = data.User_Id;
                                _IssueContext.Add(IHCM);
                            }


                        }

                        var saveddata = _IssueContext.SaveChanges();
                        if (saveddata > 0)
                        {
                            data.returnval = "Add";
                        }
                        else
                        {
                            data.returnval = "NotAdd";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRM_VersionControlProcessDTO editdata(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_ClientHostingMappingDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@IVRMCH_Id", SqlDbType.VarChar) { Value = data.IVRMCH_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getclientlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //data.getclientlist = _IssueContext.IVRM_Hosting_Client_MappingDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_VersionControlProcessDTO activedeactive(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                var result = _IssueContext.IVRM_Client_HostingDMO.Single(t => t.IVRMCH_Id == data.IVRMCH_Id);
                if (result.IVRMCH_ActiveFlag == true)
                {
                    result.IVRMCH_ActiveFlag = false;
                    result.Updatedby = data.User_Id;
                    result.Updateddate = DateTime.Now;
                    _IssueContext.Update(result);
                }
                else
                {
                    result.IVRMCH_ActiveFlag = true;
                    result.Updatedby = data.User_Id;
                    result.Updateddate = DateTime.Now;
                    _IssueContext.Update(result);
                }
                int ctx = _IssueContext.SaveChanges();
                if (ctx > 0)
                {
                    data.returnval = "Updated";
                }
                else
                {
                    data.returnval = "NotUpdated";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //===========Client Hosting Master END

        //===========Version Transaction
        public IVRM_VersionControlProcessDTO VTgetdetails(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                data.clientHosting = _IssueContext.IVRM_Client_HostingDMO.Where(t => t.IVRMCH_ActiveFlag == true).ToArray();

                data.getversionapprovedlist = (from a in _IssueContext.IVRM_Version_TransactionDMO
                                    from b in _IssueContext.IVRM_Client_HostingDMO
                                    where (a.IVRMCH_Id == b.IVRMCH_Id && a.IVRMVT_ActiveFlag==false)
                                    select new IVRM_VersionControlProcessDTO
                                    {
                                        IVRMVT_Id = a.IVRMVT_Id,
                                        IVRMCH_Id = a.IVRMCH_Id,
                                        IVRMCH_HostName = b.IVRMCH_HostName,
                                        IVRMVT_VersionName = a.IVRMVT_VersionName,
                                        IVRMVT_DeployementDate = a.IVRMVT_DeployementDate,
                                        IVRMVT_ActiveFlag = a.IVRMVT_ActiveFlag
                                    }).ToArray();

                data.getgriddata = (from a in _IssueContext.IVRM_Version_TransactionDMO
                                     from b in _IssueContext.IVRM_Client_HostingDMO
                                     where (a.IVRMCH_Id == b.IVRMCH_Id && a.IVRMVT_ActiveFlag == true)
                                     select new IVRM_VersionControlProcessDTO
                                     {
                                         IVRMVT_Id = a.IVRMVT_Id,
                                         IVRMCH_Id = a.IVRMCH_Id,
                                         IVRMCH_HostName = b.IVRMCH_HostName,
                                         IVRMVT_VersionName = a.IVRMVT_VersionName,
                                         IVRMVT_DeployementDate = a.IVRMVT_DeployementDate,
                                         IVRMVT_ActiveFlag = a.IVRMVT_ActiveFlag
                                     }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_VersionControlProcessDTO VTgettasklist(IVRM_VersionControlProcessDTO data)
        {
            try
            {
                data.clientList = (from x in _IssueContext.IVRM_Hosting_Client_MappingDMO
                                   from y in _IssueContext.clientTable
                                   from z in _IssueContext.IVRM_Client_HostingDMO
                                   where (x.ISMMCLT_Id == y.ISMMCLT_Id && x.IVRMCH_Id == data.IVRMCH_Id && z.IVRMCH_Id == x.IVRMCH_Id && x.IVRMCH_Id == z.IVRMCH_Id)
                                   select new IVRM_VersionControlProcessDTO
                                   {
                                       ISMMCLT_Id = y.ISMMCLT_Id,
                                       IVRMCH_Id = x.IVRMCH_Id,
                                       ISMMCLT_ClientName = y.ISMMCLT_ClientName,
                                   }).ToArray();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_TasksForVersion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IVRMCH_Id", SqlDbType.BigInt) { Value = data.IVRMCH_Id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = DateTime.Now });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = DateTime.Now });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.taskidlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_VersionControlProcessDTO VTsave(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                if (data.IVRMVT_Id > 0)
                {
                    var updaterecord = _IssueContext.IVRM_Version_TransactionDMO.Single(t => t.IVRMVT_Id == data.IVRMVT_Id);
                    updaterecord.IVRMCH_Id = data.IVRMCH_Id;
                    updaterecord.IVRMVT_DeployementDate = data.IVRMVT_DeployementDate;
                    updaterecord.Updateddate = DateTime.Now;
                    updaterecord.Updatedby = data.User_Id;
                    _IssueContext.Update(updaterecord);
                    _IssueContext.SaveChanges();

                    var deleteexisting = _IssueContext.IVRM_Version_FeaturesDetailsDMO.Where(t => t.IVRMVT_Id == data.IVRMVT_Id).ToList();
                    if (deleteexisting.Count > 0)
                    {
                        foreach (var item in deleteexisting)
                        {
                            _IssueContext.Remove(item);
                        }
                    }
                    if (data.getFeaturelist.Length > 0)
                    {
                        foreach (var obj in data.getFeaturelist)
                        {
                            IVRM_Version_FeaturesDetailsDMO IVFD = new IVRM_Version_FeaturesDetailsDMO();
                            IVFD.IVRMVT_Id = data.IVRMVT_Id;
                            IVFD.ISMTCR_Id = obj.ISMTCR_Id;
                            IVFD.IVRMVFD_ActiveFlag = true;
                            IVFD.IVRMVFD_Status = "Pending";
                            IVFD.Createddate = DateTime.Now;
                            IVFD.Updateddate = DateTime.Now;
                            IVFD.Createdby = data.User_Id;
                            IVFD.Updatedby = data.User_Id;
                            _IssueContext.Add(IVFD);
                        }
                    }
                    var saveddata = _IssueContext.SaveChanges();
                    if (saveddata > 0)
                    {
                        data.returnval = "Update";
                    }
                    else
                    {
                        data.returnval = "NotUpdate";
                    }

                }
                else
                {
                    var currentversion = "";
                    var updatedversion = "";
                    var existingHosting = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).ToArray();
                    if (existingHosting.Length > 0)
                    {
                        var dublicate = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).OrderByDescending(t=>t.IVRMVT_Id).ToList().FirstOrDefault();

                        currentversion = dublicate.IVRMVT_VersionName;
                        int[] n = Array.ConvertAll(currentversion.Split(new char[] { '.' }), int.Parse);
                        if (n[2] > 9)
                        {
                            n[2] = 0;
                            n[1]++;
                        }
                        else
                        {
                            n[2]++;
                        }
                        if (n[1] > 9)
                        {
                            n[1] = 0;
                            n[0]++;
                        }
                        updatedversion = string.Join(".", Array.ConvertAll(n, s => s.ToString()));

                    }
                    else
                    {
                        updatedversion = "1.0.0";
                    }


                    IVRM_Version_TransactionDMO VT = new IVRM_Version_TransactionDMO();
                    VT.IVRMCH_Id = data.IVRMCH_Id;
                    VT.IVRMVT_VersionName = updatedversion;
                    VT.IVRMVT_DeployementDate = data.IVRMVT_DeployementDate;
                    VT.IVRMVT_ActiveFlag = true;
                    VT.Createddate = DateTime.Now;
                    VT.Updateddate = DateTime.Now;
                    VT.Createdby = data.User_Id;
                    VT.Updatedby = data.User_Id;
                    _IssueContext.Add(VT);
                    _IssueContext.SaveChanges();
                    if (data.getFeaturelist.Length > 0)
                    {
                        foreach (var obj in data.getFeaturelist)
                        {
                            IVRM_Version_FeaturesDetailsDMO IVFD = new IVRM_Version_FeaturesDetailsDMO();
                            IVFD.IVRMVT_Id = VT.IVRMVT_Id;
                            IVFD.ISMTCR_Id = obj.ISMTCR_Id;
                            IVFD.IVRMVFD_ActiveFlag = true;
                            IVFD.IVRMVFD_Status = "Pending";
                            IVFD.Createddate = DateTime.Now;
                            IVFD.Updateddate = DateTime.Now;
                            IVFD.Createdby = data.User_Id;
                            IVFD.Updatedby = data.User_Id;
                            _IssueContext.Add(IVFD);
                        }
                    }
                    var saveddata = _IssueContext.SaveChanges();
                    if (saveddata > 0)
                    {
                        data.returnval = "Add";
                    }
                    else
                    {
                        data.returnval = "NotAdd";
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_VersionControlProcessDTO VTeditdata(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Version_Approved_Tasks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IVRMVT_Id", SqlDbType.BigInt) { Value = data.IVRMVT_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.taskidlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_VersionControlProcessDTO VTactivedeactive(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                var result = _IssueContext.IVRM_Version_TransactionDMO.Single(t => t.IVRMVT_Id == data.IVRMVT_Id);
                if (result.IVRMVT_ActiveFlag == true)
                {
                    result.IVRMVT_ActiveFlag = false;
                    result.Updatedby = data.User_Id;
                    result.Updateddate = DateTime.Now;
                    _IssueContext.Update(result);
                }
                else
                {
                    result.IVRMVT_ActiveFlag = true;
                    result.Updatedby = data.User_Id;
                    result.Updateddate = DateTime.Now;
                    _IssueContext.Update(result);
                }
                int ctx = _IssueContext.SaveChanges();
                if (ctx > 0)
                {
                    data.returnval = "Updated";
                }
                else
                {
                    data.returnval = "NotUpdated";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_VersionControlProcessDTO ReportOnload(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                data.clientHosting = _IssueContext.IVRM_Client_HostingDMO.Where(t => t.IVRMCH_ActiveFlag == true).ToArray();
                data.versionlist = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMVT_ActiveFlag == true && t.IVRMVT_Id==data.IVRMVT_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_VersionControlProcessDTO GetReport(IVRM_VersionControlProcessDTO data)
        {
            try
            {
                if (data.flag== "gethostversions")
                {
                    data.versionlist = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMVT_ActiveFlag == true && t.IVRMCH_Id == data.IVRMCH_Id).ToArray();
                    return data;
                }
                else
                {
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_VersionControlReport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IVRMVT_Id", SqlDbType.BigInt) { Value = data.IVRMVT_Id });
                        cmd.Parameters.Add(new SqlParameter("@IVRMCH_Id", SqlDbType.BigInt) { Value = data.IVRMCH_Id });
                      //cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = data.Fromdate });
                      //cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = data.Todate });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.getgriddata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //---------Get task list for approval of deployment  --adarsh ms 22-11-2024
        public IVRM_VersionControlProcessDTO getversiondetails(IVRM_VersionControlProcessDTO data)
        {
            try
            {
                if (data.flag=="existingversion")
                {
                    var hasData = _IssueContext.IVRM_Version_TransactionDMO
                                       .Any(V => V.IVRMCH_Id == data.IVRMCH_Id);
                    if (hasData)
                    {
                        var dataaa = _IssueContext.IVRM_Version_TransactionDMO
                            .Where(V => V.IVRMCH_Id == data.IVRMCH_Id)
                            .Last();
                        data.lastdeployeddate = dataaa.IVRMVT_DeployementDate.Value;
                        data.lastdeployedversion = dataaa.IVRMVT_VersionName;
                    }
                }
                else {
                   // var currentversion = "";
                   // var updatedversion = "";
                    var existingHosting = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).ToArray();
                    if (existingHosting.Length > 0)
                    {
                        var dublicate = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).OrderByDescending(t => t.IVRMVT_Id).ToList().FirstOrDefault();

                        data.currentversion = dublicate.IVRMVT_VersionName;
                        if (data.IVRMVRT_DeployType == "P")
                        {
                            int[] n = Array.ConvertAll(data.currentversion.Split(new char[] { '.' }), int.Parse);
                            if (n[2] > 9)
                            {
                                n[2] = 0;
                                n[1]++;
                            }
                            else
                            {
                                n[2]++;
                            }
                            if (n[1] > 9)
                            {
                                n[1] = 0;
                                n[0]++;
                            }
                            data.updatedversion = string.Join(".", Array.ConvertAll(n, s => s.ToString()));
                        }
                        else
                        {
                            int[] n = Array.ConvertAll(data.currentversion.Split(new char[] { '.' }), int.Parse);
                            n[0]++;
                            n[1] = 0;
                            n[2] = 0;
                            data.updatedversion = string.Join(".", Array.ConvertAll(n, s => s.ToString()));
                        }
                    }
                    else
                    {
                        data.updatedversion = "1.0.0";
                    }


                }
               
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //---------Get task list for approval of deployment  --adarsh ms 22-11-2024
        public IVRM_VersionControlProcessDTO Loadthetasklist(IVRM_VersionControlProcessDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_TasksForVersion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IVRMCH_Id", SqlDbType.BigInt) { Value = data.IVRMCH_Id });
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)
                    {
                        Value = data.IVRMVT_DeployementDate.Value.ToString("yyyy-MM-dd")
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.gettaskforapproval = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //---------Save task list for approval of deployment --adarsh ms 22-11-2024
        public IVRM_VersionControlProcessDTO savetasklist(IVRM_VersionControlProcessDTO data)
        {
            try
            {
                if (data.IVRMVT_Id > 0)
                {
                    var updaterecord = _IssueContext.IVRM_Version_TransactionDMO.Single(t => t.IVRMVT_Id == data.IVRMVT_Id);
                    updaterecord.IVRMCH_Id = data.IVRMCH_Id;
                    updaterecord.IVRMVT_DeployementDate = data.IVRMVT_DeployementDate;
                    updaterecord.Updateddate = DateTime.Now;
                    updaterecord.Updatedby = data.User_Id;
                    _IssueContext.Update(updaterecord);
                    _IssueContext.SaveChanges();

                    var deleteexisting = _IssueContext.IVRM_Version_FeaturesDetailsDMO.Where(t => t.IVRMVT_Id == data.IVRMVT_Id).ToList();
                    if (deleteexisting.Count > 0)
                    {
                        foreach (var item in deleteexisting)
                        {
                            _IssueContext.Remove(item);
                        }
                    }
                    if (data.getFeaturelist.Length > 0)
                    {
                        foreach (var obj in data.getFeaturelist)
                        {
                            IVRM_Version_FeaturesDetailsDMO IVFD = new IVRM_Version_FeaturesDetailsDMO();
                            IVFD.IVRMVT_Id = data.IVRMVT_Id;
                            IVFD.ISMTCR_Id = obj.ISMTCR_Id;
                            IVFD.IVRMVFD_ActiveFlag = true;
                            IVFD.IVRMVFD_Status = "Pending";
                            IVFD.Createddate = DateTime.Now;
                            IVFD.Updateddate = DateTime.Now;
                            IVFD.Createdby = data.User_Id;
                            IVFD.IVRMVFD_Test_Case = "";
                            IVFD.IVRMVFD_Test_Case_File_Name = "";
                            IVFD.IVRMVFD_ApprovedFlag = false;
                            _IssueContext.Add(IVFD);
                        }
                    }
                    var saveddata = _IssueContext.SaveChanges();
                    if (saveddata > 0)
                    {
                        data.returnval = "Update";
                    }
                    else
                    {
                        data.returnval = "NotUpdate";
                    }

                }
                else
                {
                    var currentversion = "";
                    var updatedversion = "";
                    var existingHosting = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).ToArray();
                    if (existingHosting.Length > 0)
                    {
                        var dublicate = _IssueContext.IVRM_Version_TransactionDMO.Where(t => t.IVRMCH_Id == data.IVRMCH_Id).OrderByDescending(t => t.IVRMVT_Id).ToList().FirstOrDefault();

                        currentversion = dublicate.IVRMVT_VersionName;
                        if (data.IVRMVRT_DeployType == "P")
                        {
                            int[] n = Array.ConvertAll(currentversion.Split(new char[] { '.' }), int.Parse);
                            if (n[2] > 9)
                            {
                                n[2] = 0;
                                n[1]++;
                            }
                            else
                            {
                                n[2]++;
                            }
                            if (n[1] > 9)
                            {
                                n[1] = 0;
                                n[0]++;
                            }
                            updatedversion = string.Join(".", Array.ConvertAll(n, s => s.ToString()));
                        }
                        else
                        {
                            int[] n = Array.ConvertAll(currentversion.Split(new char[] { '.' }), int.Parse);
                            n[0]++;
                            n[1] = 0;
                            n[2] = 0;
                            updatedversion = string.Join(".", Array.ConvertAll(n, s => s.ToString()));
                        }
                    }
                    else
                    {
                        updatedversion = "1.0.0";
                    }


                    IVRM_Version_TransactionDMO VT = new IVRM_Version_TransactionDMO();
                    VT.IVRMCH_Id = data.IVRMCH_Id;
                    VT.IVRMVT_VersionName = updatedversion;
                   // VT.IVRMVT_DeployementDate = data.IVRMVT_DeployementDate;
                    VT.IVRMVT_DeployementDate = null;
                    VT.IVRMVT_ActiveFlag = false;
                    VT.Createddate = DateTime.Now;
                    VT.Updateddate = DateTime.Now;
                    VT.Createdby = data.User_Id;
                    VT.Updatedby = data.User_Id;
                    _IssueContext.Add(VT);
                    _IssueContext.SaveChanges();
                    if (data.temtasklist.Length > 0)
                    {
                        foreach (var obj in data.temtasklist)
                        {
                            IVRM_Version_FeaturesDetailsDMO IVFD = new IVRM_Version_FeaturesDetailsDMO();
                            IVFD.IVRMVT_Id = VT.IVRMVT_Id;
                            IVFD.ISMTCR_Id = obj.ISMTCR_Id;
                            IVFD.IVRMVFD_ActiveFlag = true;
                            IVFD.IVRMVFD_Status = "Pending";
                            IVFD.Createddate = DateTime.Now;
                            IVFD.Updateddate = DateTime.Now;
                            IVFD.Createdby = data.User_Id;
                            IVFD.Updatedby = data.User_Id;
                            _IssueContext.Add(IVFD);
                        }
                    }

                    if (data.temtasklist.Length>0)
                    {
                        foreach (var x in data.temtasklist)
                        {
                            var contactExistsP = _IssueContext.Database.ExecuteSqlCommand("sp_Version_Release_TaskBucket @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7",
                              VT.IVRMVT_Id, x.ISMTCR_Id, data.IVRMCH_Id, x.IVRMVRT_ManHour, x.IVRMVRT_AddedFeature, data.User_Id, data.IVRMVRT_DeployType, x.IVRMVRT_Remarks);
                            data.message = "save";
                        }
                    }
                    
                    var saveddata = _IssueContext.SaveChanges();
                    if (saveddata > 0)
                    {
                        data.returnval = "Add";
                    }
                    else
                    {
                        data.returnval = "NotAdd";
                    }
                }

            }
           catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }


            return data;
        }
        // Save the deployment status of the hostig
        public IVRM_VersionControlProcessDTO savedeployment(IVRM_VersionControlProcessDTO data)
        {
            try
            {

                var check_scope_approved = _IssueContext.IVRM_Version_FeaturesDetailsDMO.Where(f => f.IVRMVT_Id == data.IVRMVT_Id && f.IVRMVFD_ApprovedFlag == false).ToList();

                if (check_scope_approved.Count>0)
                {
                    data.returnval = "Scope Approval Is Pending";
                }
                else
                {
                    var result = _IssueContext.IVRM_Version_TransactionDMO.Single(t => t.IVRMVT_Id == data.IVRMVT_Id);
                    result.IVRMVT_ActiveFlag = true;
                    result.IVRMVT_DeployementDate = data.IVRMVT_DeployementDate;
                    result.Updateddate = DateTime.Now;
                    result.Updatedby = data.User_Id;
                    _IssueContext.Update(result);


                    int ctx = _IssueContext.SaveChanges();
                    if (ctx > 0)
                    {
                        data.returnval = "Updated";
                    }
                    else
                    {
                        data.returnval = "NotUpdated";
                    }
                }

               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRM_VersionControlProcessDTO IVRM_CLient_version(IVRM_VersionControlProcessDTO data)
        {
            try
            {
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_Get_Client_Basedon_URL";
                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_IVRM_URL", SqlDbType.VarChar)
                    {
                        Value = data.ISMMCLT_IVRM_URL
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.IVRM_Version = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
