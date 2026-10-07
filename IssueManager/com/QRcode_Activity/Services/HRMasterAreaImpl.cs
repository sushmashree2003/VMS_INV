using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager.HRMasterArea;
using DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.QRcode_Activity.Services
{
    public class HRMasterAreaImpl : Interfaces.HRMasterAreaInterface
    {

        public HRMasterAreaContext _INVContext;
        ILogger<HRMasterAreaImpl> _logInv;
        public HRMasterAreaImpl(HRMasterAreaContext InvContext, ILogger<HRMasterAreaImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public HRMasterAreaDTO getloaddata(HRMasterAreaDTO data)
        {
            try
            {

                data.get_building = _INVContext.HR_Master_Building_con.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.get_Area = (from a in _INVContext.HRMasterAreaDMO
                                 from b in _INVContext.HR_Master_Building_con
                                 from c in _INVContext.HR_Master_Floor_con
                                 where a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.MI_Id == data.MI_Id
                                 select new HRMasterAreaDTO
                                 {
                                     HRMAR_AreaName = a.HRMAR_AreaName,
                                     HRMAR_AreaCode = a.HRMAR_AreaCode,
                                     HRMAR_QRCode = a.HRMAR_QRCode,
                                     HRMAR_TypeFlag = a.HRMAR_TypeFlag,
                                     HRMAR_ActiveFlag = a.HRMAR_ActiveFlag,
                                     HRMB_BuildingName = b.HRMB_BuildingName,
                                     HRMF_FloorName = c.HRMF_FloorName,
                                     HRMAR_CreatedDate=a.HRMAR_CreatedDate,
                                     HRMF_Id =c.HRMF_Id,
                                     HRMB_Id=b.HRMB_Id,
                                     HRMAR_Id=a.HRMAR_Id

                                 }).OrderByDescending(a=>a.HRMAR_CreatedDate).ToArray();

                data.get_AreaActivity = (from a in _INVContext.HRMasterAreaActivityDMO
                                         from b in _INVContext.HRMasterAreaDMO
                                         from c in _INVContext.HR_Master_Building_con
                                         from d in _INVContext.HR_Master_Floor_con

                                         where c.HRMB_Id == d.HRMB_Id && d.HRMF_Id == b.HRMF_Id && a.HRMAR_Id == b.HRMAR_Id 
                                         select new HRMasterAreaDTO
                                         {
                                             HRMB_BuildingName = c.HRMB_BuildingName,
                                             HRMF_FloorName = d.HRMF_FloorName,
                                             HRMAR_AreaName = b.HRMAR_AreaName,
                                             HRMARACT_ActivityName = a.HRMARACT_ActivityName,
                                             HRMARACT_ActivityDesc = a.HRMARACT_ActivityDesc,
                                             HRMARACT_ActiveFlag = a.HRMARACT_ActiveFlag,
                                             HRMARACT_Id = a.HRMARACT_Id,
                                             HRMARACT_CreatedDate = a.HRMARACT_CreatedDate,
                                             HRMB_Id = c.HRMB_Id,
                                             HRMF_Id = d.HRMF_Id,
                                             HRMAR_Id = b.HRMAR_Id,
                                         }).OrderByDescending(a => a.HRMARACT_CreatedDate).ToArray();


                data.get_AreaActivityTime = (from a in _INVContext.HRMasterAreaDMO
                                             from b in _INVContext.HR_Master_Building_con
                                             from c in _INVContext.HR_Master_Floor_con
                                             from d in _INVContext.HRMasterAreaActivityDMO
                                             from e in _INVContext.HRMasterAreaActivityTimeDMO

                                             where b.HRMB_Id==a.HRMB_Id && c.HRMF_Id==a.HRMF_Id && d.HRMAR_Id==a.HRMAR_Id && e.HRMARACT_Id == d.HRMARACT_Id 

                                             select new HRMasterAreaDTO
                                         {
                                             HRMB_BuildingName = b.HRMB_BuildingName,
                                             HRMF_FloorName = c.HRMF_FloorName,
                                             HRMAR_AreaName = a.HRMAR_AreaName,
                                             HRMARACT_ActivityName = d.HRMARACT_ActivityName,
                                             HRMARACTT_Time = e.HRMARACTT_Time,
                                             HRMARACTT_ActivityDuration = e.HRMARACTT_ActivityDuration,
                                             HRMARACTT_ActiveFlag = e.HRMARACTT_ActiveFlag,
                                             HRMARACTT_Id = e.HRMARACTT_Id,
                                                 HRMARACTT_CreatedDate=e.HRMARACTT_CreatedDate,
                                                 HRMB_Id = b.HRMB_Id,
                                                 HRMF_Id = c.HRMF_Id,
                                                 HRMAR_Id = a.HRMAR_Id,
                                                 HRMARACT_Id =d.HRMARACT_Id,



                                             }).OrderByDescending(a => a.HRMARACTT_CreatedDate).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }

        public HRMasterAreaDTO getfloors(HRMasterAreaDTO data)
        {
            try
            {
                data.get_floor = _INVContext.HR_Master_Floor_con.Where(a => a.HRMF_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMasterAreaDTO getAreaName(HRMasterAreaDTO data)
        {
            try
            {
                data.get_getAreaName = _INVContext.HRMasterAreaDMO.Where(a => a.HRMAR_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRMB_Id == data.HRMB_Id && a.HRMF_Id == data.HRMF_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public HRMasterAreaDTO getActivityName(HRMasterAreaDTO data)
        {
            

            try
            {
                data.get_getActivity = _INVContext.HRMasterAreaActivityDMO.Where(a => a.HRMARACT_ActiveFlag == true && a.HRMAR_Id == data.HRMAR_Id).Distinct().ToArray();

              

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;


          
        }

        public HRMasterAreaDTO savedetailsAREA(HRMasterAreaDTO data)
        {
            try
            {
                if (data.HRMAR_Id != 0)
                {
                    var res = _INVContext.HRMasterAreaDMO.Where(t => t.HRMAR_AreaCode == data.HRMAR_AreaCode && t.MI_Id == data.MI_Id && t.HRMAR_Id != data.HRMAR_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.HRMasterAreaDMO.Single(t => t.HRMAR_Id == data.HRMAR_Id);
                        result.MI_Id = data.MI_Id;
                        result.HRMF_Id = data.HRMF_Id;
                        result.HRMB_Id = data.HRMB_Id;
                        result.HRMAR_AreaName = data.HRMAR_AreaName;
                        result.HRMAR_AreaCode = data.HRMAR_AreaCode;
                        result.HRMA_QRCODEPath = data.HRMA_QRCODEPath;
                        result.HRMAR_QRCode = data.HRMAR_QRCode;
                        result.HRMAR_TypeFlag = data.HRMAR_TypeFlag;
                        result.HRMAR_ActiveFlag = true;
                        result.HRMAR_UpdatedDate = DateTime.Now;
                        _INVContext.Update(result);

                        var contactExists = _INVContext.SaveChanges();
                        data.HRMAR_Id = result.HRMAR_Id;
                        string valueqr = "";
                        valueqr = data.HRMB_BuildingName + "#" + data.HRMF_FloorName + "#" + data.HRMAR_AreaName + "#" + Convert.ToInt32(data.HRMAR_Id).ToString();

                        if (contactExists > 0)
                        {
                            Bitmap bitmap = new Bitmap(100, 100);


                            QRCodeGenerator QrGenerator = new QRCodeGenerator();
                            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                            QRCode QrCode = new QRCode(QrCodeInfo);
                            Bitmap QrBitmap = QrCode.GetGraphic(60);

                            byte[] byteArray = BitmapToByteArray(QrBitmap);
                            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));

                            data.HRMAR_QRCode = QrUri;

                            var result12 = _INVContext.HRMasterAreaDMO.Single(t => t.HRMAR_Id == data.HRMAR_Id);
                            result12.HRMAR_QRCode = data.HRMAR_QRCode;
                            _INVContext.Update(result12);
                            var save = _INVContext.SaveChanges();
                            data.returnduplicatestatus = "Update";
                        }
                        else
                        {
                            data.returnduplicatestatus = "Not Update";
                        }
                    }
                }
                else
                {
                    var res = _INVContext.HRMasterAreaDMO.Where(t => (t.HRMAR_AreaCode == data.HRMAR_AreaCode) && t.MI_Id == data.MI_Id && t.HRMF_Id == data.HRMF_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        //string st = data.HRMAR_QRCode;
                        //st = st.Insert(0, "<");
                        //int stlength;
                        //stlength = st.Length;
                        //// stlength = stlength ;
                        //st += st.Insert(stlength, " width=" + "400" + " height=" + "400" + " />");


                        HRMasterAreaDMO area = new HRMasterAreaDMO();
                        area.MI_Id = data.MI_Id;
                        area.HRMF_Id = data.HRMF_Id;
                        area.HRMB_Id = data.HRMB_Id;
                        area.HRMAR_AreaName = data.HRMAR_AreaName;
                        area.HRMAR_AreaCode = data.HRMAR_AreaCode;
                        area.HRMA_QRCODEPath = data.HRMA_QRCODEPath;
                        area.HRMAR_QRCode = null;
                        area.HRMAR_TypeFlag = data.HRMAR_TypeFlag;
                        area.HRMAR_ActiveFlag = true;
                        area.HRMAR_CreatedDate = DateTime.Now;
                        area.HRMAR_UpdatedDate = DateTime.Now;
                        _INVContext.Add(area);
                       
                        var contactExists = _INVContext.SaveChanges();
                        data.HRMAR_Id = area.HRMAR_Id;
                        string valueqr = "";
                        valueqr =  data.HRMB_BuildingName + "#" + data.HRMF_FloorName + "#" + data.HRMAR_AreaName + "#" + Convert.ToInt32(data.HRMAR_Id).ToString() ;
                        if (contactExists > 0)
                        {

                            
                           // valueqr = Convert.ToInt32(data.HRMB_Id).ToString() + "#" + Convert.ToInt32(data.HRMF_Id).ToString();
                            Bitmap bitmap = new Bitmap(100, 100);


                            QRCodeGenerator QrGenerator = new QRCodeGenerator();
                            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                            QRCode QrCode = new QRCode(QrCodeInfo);
                            Bitmap QrBitmap = QrCode.GetGraphic(60);

                            byte[] byteArray = BitmapToByteArray(QrBitmap);
                            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));

                            data.HRMAR_QRCode = QrUri;

                            var result12 = _INVContext.HRMasterAreaDMO.Single(t => t.HRMAR_Id == data.HRMAR_Id);
                            result12.HRMAR_QRCode = data.HRMAR_QRCode;
                            _INVContext.Update(result12);
                            var save = _INVContext.SaveChanges();

                            data.returnduplicatestatus = "Saved";
                        }
                        else
                        {
                            data.returnduplicatestatus = "Not Saved";
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

        public HRMasterAreaDTO savedetailsAREAActivity(HRMasterAreaDTO data)
        {
            try
            {
                if (data.HRMARACT_Id != 0)
                {
                    var res = _INVContext.HRMasterAreaActivityDMO.Where(t =>t.HRMAR_Id==data.HRMAR_Id && t.HRMARACT_ActivityName == data.HRMARACT_ActivityName && t.HRMARACT_Id != data.HRMARACT_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.HRMasterAreaActivityDMO.Single(t => t.HRMARACT_Id == data.HRMARACT_Id);
                        result.HRMAR_Id = data.HRMAR_Id;
                        result.HRMARACT_ActivityName = data.HRMARACT_ActivityName;
                        result.HRMARACT_ActivityDesc = data.HRMARACT_ActivityDesc;
                        result.HRMARACT_ActiveFlag = true;
                        result.HRMARACT_UpdatedDate = DateTime.Now;
                        result.HRMARACT_CreatedDate = DateTime.Now;

                        _INVContext.Update(result);

                        var contactExists = _INVContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnduplicatestatus = "Update";
                        }
                        else
                        {
                            data.returnduplicatestatus = "Not Update";
                        }
                    }
                }
                else
                {
                    var res = _INVContext.HRMasterAreaActivityDMO.Where(t => (t.HRMAR_Id == data.HRMAR_Id && t.HRMARACT_ActivityName == data.HRMARACT_ActivityName)).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {



                        HRMasterAreaActivityDMO area = new HRMasterAreaActivityDMO();
                        area.HRMAR_Id = data.HRMAR_Id;
                        area.HRMARACT_ActivityName = data.HRMARACT_ActivityName;
                        area.HRMARACT_ActivityDesc = data.HRMARACT_ActivityDesc;
                        area.HRMARACT_ActiveFlag = true;
                        area.HRMARACT_UpdatedDate = DateTime.Now;
                        area.HRMARACT_CreatedDate = DateTime.Now;
                        _INVContext.Add(area);
                        var contactExists = _INVContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnduplicatestatus = "Saved";
                        }
                        else
                        {
                            data.returnduplicatestatus = "Not Saved";
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

        public HRMasterAreaDTO savedetailsAREAActivityTime(HRMasterAreaDTO data)
        {
            try
            {

                if (data.HRMARACTT_Id != 0)
                {
                    var res = _INVContext.HRMasterAreaActivityTimeDMO.Where(t => t.HRMARACTT_Time == data.HRMARACTT_Time && t.HRMARACTT_Id != data.HRMARACTT_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.HRMasterAreaActivityTimeDMO.Single(t => t.HRMARACTT_Id == data.HRMARACTT_Id);
                        result.HRMARACT_Id = data.HRMARACT_Id;
                        result.HRMARACTT_Time = data.HRMARACTT_Time;
                        result.HRMARACTT_ActivityDuration = data.HRMARACTT_ActivityDuration;
                        result.HRMARACTT_ActiveFlag = true;
                        result.HRMARACTT_UpdatedDate = DateTime.Now;
                        result.HRMARACTT_CreatedDate = DateTime.Now;

                        _INVContext.Update(result);

                        var contactExists = _INVContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnduplicatestatus = "Update";
                        }
                        else
                        {
                            data.returnduplicatestatus = "Not Update";
                        }
                    }
                }
                else
                {
                    var res = _INVContext.HRMasterAreaActivityTimeDMO.Where(t => (t.HRMARACTT_Time == data.HRMARACTT_Time)).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        HRMasterAreaActivityTimeDMO area = new HRMasterAreaActivityTimeDMO();
                        area.HRMARACT_Id = data.HRMARACT_Id;
                        area.HRMARACTT_Time = data.HRMARACTT_Time;
                        area.HRMARACTT_ActivityDuration = data.HRMARACTT_ActivityDuration;

                        area.HRMARACTT_ActiveFlag = true;
                        area.HRMARACTT_UpdatedDate = DateTime.Now;
                        area.HRMARACTT_CreatedDate = DateTime.Now;
                        _INVContext.Add(area);
                        var contactExists = _INVContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnduplicatestatus = "Saved";
                        }
                        else
                        {
                            data.returnduplicatestatus = "Not Saved";
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

        public HRMasterAreaDTO deactiveArea(HRMasterAreaDTO data)
        {
            try
            {
                var result = _INVContext.HRMasterAreaDMO.Single(t => t.HRMAR_Id == data.HRMAR_Id);

                if (result.HRMAR_ActiveFlag == true)
                {
                    result.HRMAR_ActiveFlag = false;
                }
                else if (result.HRMAR_ActiveFlag == false)
                {
                    result.HRMAR_ActiveFlag = true;
                }
                result.HRMAR_UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public HRMasterAreaDTO deactiveAreaActivity(HRMasterAreaDTO data)
        {
            try
            {
                var result = _INVContext.HRMasterAreaActivityDMO.Single(t => t.HRMARACT_Id == data.HRMARACT_Id);

                if (result.HRMARACT_ActiveFlag == true)
                {
                    result.HRMARACT_ActiveFlag = false;
                }
                else if (result.HRMARACT_ActiveFlag == false)
                {
                    result.HRMARACT_ActiveFlag = true;
                }
                result.HRMARACT_UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


        public HRMasterAreaDTO deactiveAreaActivityTime(HRMasterAreaDTO data)
        {
            try
            {
                var result = _INVContext.HRMasterAreaActivityTimeDMO.Single(t => t.HRMARACTT_Id == data.HRMARACTT_Id);

                if (result.HRMARACTT_ActiveFlag == true)
                {
                    result.HRMARACTT_ActiveFlag = false;
                }
                else if (result.HRMARACTT_ActiveFlag == false)
                {
                    result.HRMARACTT_ActiveFlag = true;
                }
                result.HRMARACTT_UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public HRMasterAreaDTO FloorQRCODE(HRMasterAreaDTO data)
        {
            //try
            //{

            //    HRMasterAreaDMO area = new HRMasterAreaDMO();
            //    area.MI_Id = data.MI_Id;
            //    area.HRMF_Id = data.HRMF_Id;
            //    area.HRMB_Id = data.HRMB_Id;
            //    area.HRMAR_AreaName = data.HRMAR_AreaName;
            //    area.HRMAR_AreaCode = data.HRMAR_AreaCode;
            //    area.HRMA_QRCODEPath = data.HRMA_QRCODEPath;
            //    area.HRMAR_QRCode = st;
            //    area.HRMAR_TypeFlag = data.HRMAR_TypeFlag;
            //    area.HRMAR_ActiveFlag = true;
            //    area.HRMAR_CreatedDate = DateTime.Now;
            //    area.HRMAR_UpdatedDate = DateTime.Now;
            //    _INVContext.Add(area);

            //    var contactExists = _INVContext.SaveChanges();
            //    if (contactExists > 0)
            //    {
            //        data.returnduplicatestatus = "Saved";
            //    }
            //    else
            //    {
            //        data.returnduplicatestatus = "Not Saved";
            //    }

















            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}

            return data;
        }
        public HRMasterAreaDTO edit(HRMasterAreaDTO data)
        {
            try
            {
                var list = _INVContext.HRMasterAreaDMO.Where(p => p.HRMAR_Id == data.HRMAR_Id).ToList();
                if (data.HRMAR_Id > 0 )
                {
data.Edit_Data= (from a in _INVContext.HRMasterAreaDMO
                from b in _INVContext.HR_Master_Building_con
                from c in _INVContext.HR_Master_Floor_con
                where (a.MI_Id == b.MI_Id && a.HRMB_Id == b.HRMB_Id && a.HRMF_Id == c.HRMF_Id && a.HRMAR_Id == data.HRMAR_Id)
               select new HRMasterAreaDTO
               {
                   HRMAR_Id=a.HRMAR_Id,
                   HRMAR_AreaName=a.HRMAR_AreaName,
                   HRMB_BuildingName=b.HRMB_BuildingName,
                   HRMF_FloorName=c.HRMF_FloorName,
                   HRMAR_AreaCode=a.HRMAR_AreaCode,
                   HRMB_Id=b.HRMB_Id,
                   HRMF_Id=c.HRMF_Id

               }).Distinct().ToArray();


                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public static byte[] BitmapToByteArray(Bitmap QrBitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                QrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

    }
}
