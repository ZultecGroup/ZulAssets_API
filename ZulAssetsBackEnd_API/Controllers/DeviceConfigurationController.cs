using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using ZulAssetsBackEnd_API.BAL;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.Controllers
{
    /// <summary>
    /// Device Controller
    /// </summary>
    /// 
    [Tags("Device Configuration")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class DeviceConfigurationController : ControllerBase
    {

        #region Declarations & Constructor

        public readonly static string SP_DeviceInsertUpdateDelete = "[dbo].[SP_DeviceInsertUpdateDelete]";
        public readonly static string SP_VerifyDeviceRegistrationKey = "[dbo].[SP_VerifyDeviceRegistrationKey]";
        public readonly static string SP_GetUpdateDeleteDevice = "[dbo].[SP_GetUpdateDeleteDevice]";
        public readonly static string SP_GetDeleteProcessingData = "[dbo].[SP_GetDeleteProcessingData]";

        private readonly Constants _constants;


        private readonly IConfiguration _configuration;

        public DeviceConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Initialize Device
        /// <summary>
        /// Initialize Device API
        /// </summary>
        /// <param name="deviceReg"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("InitializeDevice")]
        //[Authorize]
        public IActionResult InitializeDevice([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InitializeDevice(deviceReg, SP_DeviceInsertUpdateDelete);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        string deviceID = deviceReg.DeviceID;
                        var _1 = new AssetsController(_configuration).TransferFromBEToTemp(deviceID);
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
                    }

                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }
        #endregion

        #region Verify Device Lic Key

        /// <summary>
        /// Verify Device License Key API
        /// </summary>
        /// <param name="deviceReg"></param>
        /// <returns>Returns License Key</returns>
        [HttpPost("VerifyDeviceLicKey")]
        //[Authorize]
        public IActionResult VerifyDeviceLicKey([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.VerifyDeviceLicKey(deviceReg.DeviceSerialNo, SP_VerifyDeviceRegistrationKey);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        var dbLicKey = dt.Rows[0]["LicKey"].ToString();
                        bool LicKeyVerify = EncryptDecryptPassword.ValidateKey(dbLicKey, deviceReg.DeviceSerialNo);
                        if (LicKeyVerify)
                        {
                            msg.message = "License key verified";
                            msg.status = "200";
                            return Ok(msg);
                        }
                        else
                        {
                            msg.message = LicKeyVerify ? "Device not registered!" : "Invalid license key";
                            msg.status = "401";
                            return Ok(msg);
                            
                        }
                    }

                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }

        }

        #endregion

        #region Generate Lic Key

        /// <summary>
        /// Generate License Key API
        /// </summary>
        /// <param name="deviceReg"></param>
        /// <returns>Returns License Key</returns>
        [HttpPost("GenerateLicKey")]
        //[Authorize]
        public IActionResult GenerateLicKey([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();
            try
            {
                var dbLicKey = "1300-3994-3868-8509";
                //var substringSerialNo = deviceReg.DeviceSerialNo.Substring(4);
                string LicKey = EncryptDecryptPassword.GenerateLicKey(dbLicKey, deviceReg.DeviceSerialNo);
                //string LicKey = EncryptDecryptPassword.Encrypt2("ABTAK56", substringSerialNo, 2);
                msg.message = LicKey;
                msg.status = "200";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Get All Devices
        /// <summary>
        /// Get all Devices by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <param name="deviceReg"></param>
        [HttpPost("GetAllDevices")]
        [Authorize]
        public IActionResult GetAllDevices([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllDevices(deviceReg, SP_GetUpdateDeleteDevice);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        #region Replace Table Names

                        DataTable table = ds.Tables["Table"];
                        DataTable table1 = ds.Tables["Table1"];

                        table.TableName = "TotalRowsCount";
                        table1.TableName = "data";

                        #endregion

                        int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);

                        return Ok(
                            new
                            {
                                totalRowsCount = totalRowsCounts,
                                data = table1
                            });
                        //return Ok(ds);
                    }
                }
                else
                {
                    return Ok(ds);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Update Device Info

        /// <summary>
        /// Update a Device by passing Device Description parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateDevice")]
        [Authorize]
        public IActionResult UpdateDevice([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();
            try
            {
                if (deviceReg.LicKey != "")
                {
                    bool LicKeyVerify = EncryptDecryptPassword.ValidateKey(deviceReg.LicKey, deviceReg.DeviceSerialNo);
                    if (!LicKeyVerify)
                    {
                        msg.message = "License Key is not verified";
                        msg.status = "401";
                        return BadRequest(msg);
                    }
                }
                DataTable dt = DataLogic.UpdateDevice(deviceReg, SP_GetUpdateDeleteDevice);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("GL Code", "Updated", glcodeReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Devices", 1, "Updated", deviceReg.LoginName, "dbo.Device");
                        }
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
                    }
                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Delete Device Info

        /// <summary>
        /// Delete a Device by passing DeviceID with "Delete = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteDevice")]
        [Authorize]
        public IActionResult DeleteDevice([FromBody] DeviceReg deviceReg)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteDevice(deviceReg, SP_GetUpdateDeleteDevice);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("GL Code", "Updated", glcodeReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Devices", 1, "Deleted", deviceReg.LoginName, "dbo.Device");
                        }
                        msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
                    }
                }
                else
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Data Processing APIs

        #region Get Audit & Anonymous Data

        /// <summary>
        /// This API is used to get Processing Data for Audit and Anonymous Records. For Audit Records, pass AuditData = 1 and for Anonymous Record, pass AuditData = 0
        /// along with Get = 1
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetProcessingData")]
        [Authorize]
        public IActionResult GetProcessingData([FromBody] DataProcessingReqParam dataProcReqParams)
        {
            Message msg = new Message();
            try
            {
                if (dataProcReqParams.Get == 0)
                {
                    msg.message = "Get Parameter is not available";
                    msg.status = "401";
                    return Ok(msg);
                }
                else
                {

                    DataSet ds = DataLogic.GetProcessingData(dataProcReqParams, SP_GetDeleteProcessingData);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                        {
                            msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                            msg.status = "401";
                            return Ok(msg);
                        }
                        else
                        {
                            #region Replace Table Names

                            DataTable table = ds.Tables["Table"];
                            DataTable table1 = ds.Tables["Table1"];

                            table.TableName = "TotalRowsCount";
                            table1.TableName = "data";

                            #endregion

                            int totalRowsCounts = Convert.ToInt32(table.Rows[0][0]);

                            return Ok(
                                new
                                {
                                    totalRowsCount = totalRowsCounts,
                                    data = table1
                                });
                            //return Ok(ds);
                        }
                    }
                    else
                    {
                        return Ok(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Delete Audit & Anonymous Data

        /// <summary>
        /// This API is used to delete Processing Data for Audit and Anonymous Records. For Audit Records, pass AuditData = 1 and for Anonymous Record, pass AuditData = 0
        /// along with Delete = 1
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteProcessingData")]
        [Authorize]
        public IActionResult DeleteProcessingData([FromBody] DeleteProcessingReqParam deleteProcReqParams)
        {
            Message msg = new Message();
            DataTable dt = new DataTable();
            try
            {
                
                if (deleteProcReqParams.AuditData == 1)
                {
                    DataTable auditDT = new DataTable();
                    auditDT = ListintoDataTable.ToDataTable(deleteProcReqParams.auditProcessingDataTree);
                    dt = DataLogic.DeleteProcessingData(deleteProcReqParams, auditDT, SP_GetDeleteProcessingData);
                }
                else
                {
                    DataTable anonymousDT = new DataTable();
                    anonymousDT = ListintoDataTable.ToDataTable(deleteProcReqParams.anonymousProcessingDataTree);
                    dt = DataLogic.DeleteProcessingData(deleteProcReqParams, anonymousDT, SP_GetDeleteProcessingData);
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return Ok(msg);
                    }
                    else
                    {
                        return Ok(dt);
                    }
                }
                else
                {
                    return Ok(dt);
                }
                
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Process Audit Data

        /// <summary>
        /// This API will used to Process Audit Data
        /// </summary>
        /// <param name="procAuditData"></param>
        [HttpPost("ProcessAuditData")]
        [Authorize]
        public IActionResult ProcessAuditData([FromBody] ProcessAuditData procAuditData)
        {
            Message msg = new Message();
            try
            {

                #region Validations

                if (procAuditData.InvSchCode == 0 || procAuditData.InvSchCode == null)
                {
                    msg.message = "Inventory Schedule is not selected.";
                    msg.status = "401";
                    return Ok(msg);
                    
                }
                else if (procAuditData.processAuditDataTree.Count == 0 || procAuditData.processAuditDataTree == null)
                {
                    msg.message = "No selected assets, please select some assets and try again.";
                    msg.status = "401";
                    return Ok(msg);
                }
                #endregion

                else
                {
                    DataTable processAuditDT = new DataTable();

                    if (procAuditData.processAuditDataTree != null)
                    {
                        processAuditDT = ListintoDataTable.ToDataTable(procAuditData.processAuditDataTree);
                    }

                    DataTable Ast_HistoryTable = new DataTable();

                    Ast_HistoryTable.Columns.Add(new DataColumn("InvSchCode", typeof(int)));        
                    Ast_HistoryTable.Columns.Add(new DataColumn("AstID", typeof(string)));
                    Ast_HistoryTable.Columns.Add(new DataColumn("Fr_Loc", typeof(string)));
                    Ast_HistoryTable.Columns.Add(new DataColumn("To_Loc", typeof(string)));
                    Ast_HistoryTable.Columns.Add(new DataColumn("HisDate", typeof(string)));
                    Ast_HistoryTable.Columns.Add(new DataColumn("Status", typeof(int)));
                    Ast_HistoryTable.Columns.Add(new DataColumn("AssetStatus", typeof(string)));
                    Ast_HistoryTable.Columns.Add(new DataColumn("NoPiece", typeof(int)));
                    Ast_HistoryTable.Columns.Add(new DataColumn("DeviceID", typeof(string)));

                    DateTime LastInventoryDate = DateTime.Now;

                    string AstID = string.Empty, HisDate = string.Empty, fromLoc = string.Empty, toLoc = string.Empty, AssetStatus = string.Empty, DeviceID = string.Empty;
                    int status = 0, NoPiece = 0;
                    long invSchCode = 0;

                    for (int i = 0; i < procAuditData.processAuditDataTree.Count; i++)
                    {
                        DataTable FrToLocDT = DataLogic.GetAll_AssetTransferTemp(processAuditDT.Rows[i]["AstID"].ToString(), "[dbo].[SP_GetAllAssetsTrasnferTempByAstID]");
                        DataRow newRow = Ast_HistoryTable.NewRow();
                        invSchCode = procAuditData.InvSchCode;
                        AstID = processAuditDT.Rows[i]["AstID"].ToString();

                        if (Convert.ToInt32(processAuditDT.Rows[i]["Status"]) == 2 || Convert.ToInt32(processAuditDT.Rows[i]["Status"]) == 3)
                        {
                            fromLoc = FrToLocDT.Rows[0]["FromLoc"].ToString();
                            toLoc = FrToLocDT.Rows[0]["ToLoc"].ToString();
                            DateTime transDate = Convert.ToDateTime(FrToLocDT.Rows[0]["TransDate"]);
                            string formattedDate = transDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            HisDate = formattedDate;
                            LastInventoryDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        }
                        else
                        {
                            fromLoc = processAuditDT.Rows[i]["LocID"].ToString();
                            toLoc = processAuditDT.Rows[i]["LocID"].ToString();
                            HisDate = DateTime.Now.ToString();
                        }
                        status = Convert.ToInt32(processAuditDT.Rows[i]["Status"]);
                        AssetStatus = processAuditDT.Rows[i]["AssetStatus"].ToString();
                        NoPiece = Convert.ToInt32(processAuditDT.Rows[i]["NoPiece"]);
                        DeviceID = processAuditDT.Rows[i]["DeviceID"].ToString();

                        int count = Convert.ToInt32(DataLogic.CheckChild_AstHistory(AstID, invSchCode, "[dbo].[SP_GetCountofAstIDinAst_HistoryTable]").Rows[0]["Counts"]);

                        if (count > 0)
                        {
                            DataTable teet = DataLogic.Update_Ast_History(AstID, status, invSchCode, HisDate, fromLoc, toLoc, NoPiece, AssetStatus, DeviceID, "[dbo].[SP_UpdateAstInAst_History]");
                        }
                        else
                        {
                            DataTable teet = DataLogic.Insert_Ast_History(AstID, status, invSchCode, HisDate, fromLoc, toLoc, NoPiece, AssetStatus, DeviceID, "[dbo].[SP_InsertAstInAst_History]");
                        }

                        DataTable updateAssetDetailsDT = DataLogic.UpdateAssetDetailsForInventory(toLoc, AstID, status, AssetStatus, 
                            processAuditDT.Rows[i]["AstDesc"].ToString(), invSchCode, LastInventoryDate, procAuditData.LoginName, "[dbo].[SP_UpdateAssetsInvStatus]");

                        DataTable deleteAstsTempReceive = DataLogic.DeleteFromTempDB(AstID, DeviceID, "[dbo].[SP_DeleteFromAssetsTempReceiving]");

                        if (Convert.ToInt32(deleteAstsTempReceive.Rows[0]["Status"]) == 200)
                        {
                            DataTable deleteAstsTransferTemp = DataLogic.DeleteFromTempDB(AstID, DeviceID, "[dbo].[SP_DeleteFromAssetTransferTemp]");

                            if (i == (procAuditData.processAuditDataTree.Count - 1))
                            {
                                msg.message = "Data Processed Successfully!";
                                msg.status = "200";
                            }

                        }
                        
                    }

                    return Ok(msg);

                }
                
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #endregion

    }
}
