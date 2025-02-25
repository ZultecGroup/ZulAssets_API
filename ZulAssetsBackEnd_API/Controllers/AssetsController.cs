using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ZulAssetsBackEnd_API.BAL;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X500;
using Microsoft.AspNetCore.CookiePolicy;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {

        #region Declaration & Constructor

        private static string SP_AssetTracking = "[dbo].[SP_AssetTracking]";
        private static string SP_AnonymousAssetInsetUpdate = "[dbo].[SP_AnonymousAssetInsetUpdate]";
        private static string SP_GetAllAnonymousAssets = "[dbo].[SP_GetAllAnonymousAssets]";
        private static string SP_UpdateAssetLocation = "[dbo].[SP_UpdateAssetLocation]";
        private static string SP_TransferData_BE_Temp = "[dbo].[SP_TransferDataFromBEToTemp]";
        private static string SP_GetAllAssetsFrom_Temp = "[dbo].[GetAllAssetsFromTemp]";
        private static string SP_GetAllAssetsStatus = "[dbo].[SP_GetAllAssetsStatus]";
        private static string SP_GetAssetStatus = "[dbo].[SP_GetAssetStatus]";
        private static string SP_GetAssetStatusWeb = "[dbo].[SP_GetAssetStatusWeb]";
        private static string SP_UpdateAssetStatusByBarocde = "[dbo].[SP_UpdateAssetStatusByBarocde]";
        private static string SP_GetInsertUpdateDeleteAstCoding = "[dbo].[SP_GetInsertUpdateDeleteAstCoding]";
        private static string SP_GetInsertUpdateDeleteAssetItem = "[dbo].[SP_GetInsertUpdateDeleteAssetItem]";
        private static string SP_GetAssetInfoAgainstAstID = "[dbo].[SP_GetAssetInfoAgainstAstID]";
        private static string SP_GetAllAssetsForAdministrator = "[dbo].[SP_GetAllAssetsForAdministrator]";
        private static string SP_InsertUpdateDeleteAssetDetails = "[dbo].[SP_InsertUpdateDeleteAssetDetails]";
        private static string SP_AssetSearch = "[dbo].[SP_AssetSearch]";
        private static string SP_Assets_Loc_Cust_Status_Transfer = "[dbo].[SP_Assets_Loc_Cust_Status_Transfer]";
        private static string SP_Item_Category_Transfer = "[dbo].[SP_Item_Category_Transfer]";
        private static string SP_GetAst_HistoryHighestHistoryID = "[dbo].[SP_GetHighestHistoryID]";
        private static string SP_GetAst__Cust_HistoryHighestHistoryID = "[dbo].[SP_GetHighestAst_Cust_History]";
        private static string SP_VerifyAssetCodingRange = "[dbo].[SP_VerifyAssetCodingRange]";
        private static string SP_CheckRelatedAssets = "[dbo].[SP_CheckRelatedAssets]";
        private static string SP_CheckSerialExistsForCompanyID = "[dbo].[SP_CheckSerialExistsForCompanyID]";
        private static string SP_GetAssetItemsAgainstCatID = "[dbo].[SP_GetAssetItemsAgainstCatID]";
        private static string SP_GetAstInfoAgainstAstID_BelowAdminData = "[dbo].[SP_GetAstInfoAgainstAstID_BelowAdminData]";
        private static string SP_GetAstInfoAgainstItemCode_BelowAdminData = "[dbo].[SP_GetAstInfoAgainstItemCode_BelowAdminData]";
        private static string SP_InsertDepPolicy_History = "[dbo].[SP_InsertDepPolicy_History]";
        private static string SP_UpdateAstBookAgainstAstIDAndBookID = "[dbo].[SP_UpdateAstBookAgainstAstIDAndBookID]";
        private static string SP_GetAstBookByAstID = "[dbo].[SP_GetAstBookByAstID]";

        private Constants _constants;

        private readonly IConfiguration _configuration;

        public AssetsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Get All Assets

        /// <summary>
        /// Get All Assets
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAllAssets")]
        //[Authorize]
        public IActionResult GetAllAssets()
        {
            Message msg = new Message();
            try
            {

                DataTable dt = DataLogic.GetAllAssets(SP_GetAllAssetsFrom_Temp);

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

        #region Asset Tracking

        /// <summary>
        /// Asset Tracking API
        /// </summary>
        /// <param name="assetTrackingReq"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("AssetTrackingByID")]
        [Authorize]
        public IActionResult AssetTrackingByID([FromBody] AssetTrackingRequest assetTrackingReq)
        {
            Message msg = new Message();
            AssetTrackingResponse astTrkRes = new AssetTrackingResponse();
            try
            {
                DataTable dt = DataLogic.AssetTracking(assetTrackingReq, SP_AssetTracking);
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
                        var status = dt.Rows[0]["Status"].ToString();

                        if (status == "200")
                        {
                            var baseCost = dt.Rows[0]["AcquisitionPrice"].ToString();
                            var baseCostThousand = Convert.ToDouble(baseCost).ToString("N");
                            var currentBV = dt.Rows[0]["CurrentBV"].ToString();
                            var currentBVThousand = Convert.ToDouble(currentBV).ToString("N");

                            astTrkRes.Barcode = dt.Rows[0]["Barcode"].ToString();
                            astTrkRes.Status = status;
                            astTrkRes.AssestDescription = dt.Rows[0]["AssetDescription"].ToString();
                            astTrkRes.ArDescription = dt.Rows[0]["ArDescription"].ToString();
                            astTrkRes.CatID = dt.Rows[0]["CatID"].ToString();
                            astTrkRes.AssetCategoryDescription = dt.Rows[0]["AssetCategoryDescription"].ToString();
                            astTrkRes.LocCompCode = dt.Rows[0]["LocCompCode"].ToString();
                            astTrkRes.LocID = dt.Rows[0]["LocID"].ToString();
                            astTrkRes.CostCenter = dt.Rows[0]["CostCenter"].ToString();
                            astTrkRes.PurchaseDate = dt.Rows[0]["AssetPurchaseDate"].ToString();
                            astTrkRes.AcquisitionPrice = baseCostThousand;
                            astTrkRes.Custodian = dt.Rows[0]["Custodian"].ToString();
                            astTrkRes.AssetLocationDescription = dt.Rows[0]["AssetLocationDescription"].ToString();
                            astTrkRes.AsstStatus = dt.Rows[0]["AsstStatus"].ToString();
                            astTrkRes.Message = "";
                            astTrkRes.netBook = "";

                            Int16 salvageYeardt = Convert.ToInt16(dt.Rows[0]["SalvageYear"].ToString());
                            Int16 salvageMonthdt = Convert.ToInt16(dt.Rows[0]["SalvageMonth"].ToString());
                            double salvageValueDouble = Convert.ToDouble(dt.Rows[0]["SalvageValue"].ToString());
                            DateTime lastBookUpdateDate = Convert.ToDateTime(dt.Rows[0]["BookUpdateDate"].ToString());
                            DateOnly lastBookUpdateDateonly = DateOnly.FromDateTime(lastBookUpdateDate);
                            DateTime serviceDate = Convert.ToDateTime(dt.Rows[0]["ServiceDate"].ToString());
                            DateOnly serviceDateonly = DateOnly.FromDateTime(serviceDate);
                            int intDepreciationType = Convert.ToInt32(dt.Rows[0]["DepreciationCode"].ToString());

                            DataTable depDataTable = new DataTable();
                            DataTable depDataTable2 = new DataTable();

                            depDataTable = DepreciationAlgorithm.CalcDepAnnual(salvageYeardt, salvageMonthdt, lastBookUpdateDateonly, baseCostThousand, currentBVThousand, intDepreciationType, salvageValueDouble, serviceDateonly);


                            DateTime dateTime = DateTime.UtcNow.Date;
                            int currentOnlyYear = dateTime.Year;

                            for (int i = 0; i < (depDataTable.Rows.Count); i++)
                            {
                                DateTime newDateTime = Convert.ToDateTime(depDataTable.Rows[i]["CurrDate"].ToString());
                                DateOnly newDateTimeDateOnly = DateOnly.FromDateTime(newDateTime);
                                int currentOnlyYearFromDT = newDateTimeDateOnly.Year;
                                if (currentOnlyYearFromDT == currentOnlyYear)
                                {
                                    double CBV = Convert.ToDouble(depDataTable.Rows[i]["CBV"].ToString()); // CBV = Current Book Value
                                    double CBVd = Math.Round(CBV, 2);   //CBVd = Current Book Vale in double datatype
                                    var CBVdThousand = Convert.ToDouble(CBVd).ToString("N");    //CBVdThousand = Current Book Value in double datatype and in thousand format
                                    astTrkRes.netBook = CBVdThousand;
                                }
                            }

                            return Ok(astTrkRes);
                        }
                        else
                        {
                            astTrkRes.Barcode = "";
                            astTrkRes.Status = status;
                            astTrkRes.Message = "Asset not found";
                            astTrkRes.AssestDescription = "";
                            astTrkRes.ArDescription = "";
                            astTrkRes.CatID = "";
                            astTrkRes.AssetCategoryDescription = "";
                            astTrkRes.LocCompCode = "";
                            astTrkRes.PurchaseDate = "";
                            astTrkRes.AcquisitionPrice = "";
                            astTrkRes.Custodian = "";
                            astTrkRes.LocID = "";
                            astTrkRes.CostCenter = "";
                            astTrkRes.AssetLocationDescription = "";
                            astTrkRes.AsstStatus = "";
                            astTrkRes.netBook = "";
                            return Ok(astTrkRes);
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

        #region Get Assets Status

        /// <summary>
        /// Get All Assets Status
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAssetsStatus")]
        //[Authorize]
        public IActionResult GetAssetsStatus()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllAssetsStatus(SP_GetAllAssetsStatus);
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

        #region Get Assets Status Web

        /// <summary>
        /// Get All Assets Status
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAssetsStatusWeb")]
        [Authorize]
        public IActionResult GetAssetsStatusWeb()
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllAssetsStatusDataSet(SP_GetAssetStatusWeb);

                #region Replace Table Names

                DataTable table = ds.Tables["Table"];
                DataTable table1 = ds.Tables["Table1"];

                table.TableName = "TotalRowsCount";
                table1.TableName = "data";

                #endregion

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
                        return Ok(ds);
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

        #region Update Asset Status By Barcode

        /// <summary>
        /// Update Asset Status By Barcode
        /// </summary>
        /// <returns>Returns a message "Asset Status Updated"</returns>
        [HttpPost("UpdateAssetStatusByBarcode")]
        [Authorize]
        public IActionResult UpdateAssetStatusByBarcode([FromBody] UpdateAssetStatus updAstStatus)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateAssetStatusByBarcode(updAstStatus, SP_UpdateAssetStatusByBarocde);
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
                        var status = dt.Rows[0]["StatusCode"].ToString();

                        if (status == "200")
                        {
                            msg.status = status;
                            msg.message = dt.Rows[0]["Message"].ToString();
                            return Ok(msg);
                        }
                        else
                        {
                            msg.status = status;
                            msg.message = dt.Rows[0]["Message"].ToString();
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

        #region Anonymous Asset 

        /// <summary>
        /// Anonymous Asset API
        /// </summary>
        /// <param name="anonymousAstReq"></param>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("AnonymousAssets")]
        [Authorize]
        public IActionResult AnonymousAssets([FromBody] AnonymousAssetsRequests anonymousAstReq)
        {
            Message msg = new Message();
            AnonymousAssetResponse anonymousAstRes = new AnonymousAssetResponse();
            try
            {
                DataTable dt = DataLogic.AnonymousAssets(anonymousAstReq, SP_AnonymousAssetInsetUpdate);
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
                        var status = dt.Rows[0]["Status"].ToString();

                        if (status == "200")
                        {
                            anonymousAstRes.Status = status;
                            anonymousAstRes.Message = dt.Rows[0]["Message"].ToString();
                            return Ok(anonymousAstRes);
                        }
                        else
                        {
                            anonymousAstRes.Status = status;
                            anonymousAstRes.Message = dt.Rows[0]["Message"].ToString();
                            return Ok(anonymousAstRes);
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

        #region Get Anonymous Assets

        /// <summary>
        /// Asset Tracking API
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpGet("GetAllAnonymousAssets")]
        [Authorize]
        public IActionResult GetAllAnonymousAssets()
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllAnonymousAssets(SP_GetAllAnonymousAssets);
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

        #region Update Asset Location

        /// <summary>
        /// Update Asset Location
        /// </summary>
        /// <returns>Returns a message "Device is created"</returns>
        [HttpPost("UpdateAssetLocation")]
        [Authorize]
        public IActionResult UpdateAssetLocation([FromBody] UpdateAssetLocation updAstLoc)
        {
            //var logResult = GeneralFunctions.CreateAndWriteToFile("GL Code", "Added", updAstLoc.Barcode);
            Message msg = new Message();
            DataTable dt = new DataTable();
            UpdateAssetLocationResponse updAstLocRes = new UpdateAssetLocationResponse();
            try
            {

                var length = updAstLoc.Barcode.Split("|");
                for (int i = 0; i < length.Length; i++)
                {
                    var barcode = updAstLoc.Barcode.Split("|")[i];
                    var LocID = updAstLoc.LocID.Split("|")[i];
                    var DeviceID = updAstLoc.DeviceID;
                    var InventoryDate = updAstLoc.InventoryDate;
                    var LastEditDate = updAstLoc.LastEditDate;
                    var LastEditBy = updAstLoc.LastEditBy;
                    var Status = updAstLoc.Status.Split("|")[i];
                    var AssetStatus = updAstLoc.AssetStatus.Split("|")[i];

                    UpdateAssetLocation updAstLocRes2 = new UpdateAssetLocation();

                    updAstLocRes2.Status = Status;
                    updAstLocRes2.AssetStatus = AssetStatus;
                    updAstLocRes2.Barcode = barcode;
                    updAstLocRes2.LocID = LocID;
                    updAstLocRes2.DeviceID = DeviceID;
                    updAstLocRes2.InventoryDate = InventoryDate;
                    updAstLocRes2.LastEditDate = LastEditDate;
                    updAstLocRes2.LastEditBy = LastEditBy;

                    dt = DataLogic.UpdateAssetLocation(updAstLocRes2, SP_UpdateAssetLocation);

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
                        LocationRequest locReq = new LocationRequest();
                        locReq.LocID = updAstLoc.LocID.Split("|")[0];
                        var _1 = new LocationsController(_configuration).GetAssetsByLocationID(locReq);
                        updAstLocRes.Message = dt.Rows[0]["Message"].ToString();
                        updAstLocRes.Status = dt.Rows[0]["Status"].ToString();
                        updAstLocRes.dt = _1;
                        return Ok(updAstLocRes);
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

        #region Transfer Assets From ZulAssetsBE to ZulAssetsBE_Temp

        /// <summary>
        /// Transfer Assets From BE To Temp
        /// </summary>
        /// <returns>Returns a DataSet with a message and status as well</returns>
        [HttpPost("TransferFromBEToTemp")]
        [Authorize]
        public IActionResult TransferFromBEToTemp(string deviceID)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.TransferAssetsFromBEToTemp(deviceID, SP_TransferData_BE_Temp);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        msg.message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        DataSet newDS = new DataSet();
                        DataTable newDT = new DataTable();
                        DataRow newDR = newDT.NewRow();
                        newDT.Columns.Add("ErrorMessage");
                        newDR["ErrorMessage"] = msg.message;
                        newDT.Rows.Add(newDR);
                        newDS.Tables.Add(newDT);
                        return Ok(newDS);
                    }
                    else
                    {
                        return Ok(ds);
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

        #region Asset Coding Definition

        #region Get All Assets Coding Definition
        /// <summary>
        /// Get all Asset Coding Definitions by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <param name="astCodingDef"></param>
        /// <returns>This will return all the Asset Coding Definitions</returns>
        [HttpPost("GetAllAstCodingDef")]
        [Authorize]
        public IActionResult GetAllAstCodingDef([FromBody] AstCodingDefReqParam astCodingDef)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllAstCodingDef(astCodingDef, SP_GetInsertUpdateDeleteAstCoding);
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

        #region Add Asset Coding Definition

        /// <summary>
        /// Insert a Asset Coding Definition by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertAstCodingDef")]
        [Authorize]
        public IActionResult InsertAstCodingDef([FromBody] AstCodingDefReqParam astCodingDef)
        {
            Message msg = new Message();
            try
            {

                DataTable verifyRange = DataLogic.VerifyAssetCodingRange(astCodingDef.StartSerial, astCodingDef.EndSerial, SP_VerifyAssetCodingRange);
                DataTable verifyCompanyExists = DataLogic.VerifyCompanyExists(astCodingDef.CompanyID, SP_CheckSerialExistsForCompanyID);

                if (verifyRange.Rows.Count > 0)
                {
                    msg.message = "Range Already Exists!";
                    msg.status = "404";
                    return Ok(msg);
                }
                else if (verifyCompanyExists.Rows.Count > 0)
                {
                    msg.message = "Range Already Exists for this Company!";
                    msg.status = "404";
                    return Ok(msg);
                }
                else
                {

                    DataTable dt = DataLogic.InsertAstCodingDef(astCodingDef, SP_GetInsertUpdateDeleteAstCoding);
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
                            string msgFromDB = dt.Rows[0]["Message"].ToString();
                            if (msgFromDB.Contains("successfully"))
                            {
                                //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Coding Definition", "Added", astCodingDef.LoginName);
                                DataTable dt2 = DataLogic.InsertAuditLogs("Asset Coding Definition", 1, "Inserted", astCodingDef.LoginName, "dbo.AssetCoding");
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
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Update Asset Coding Definition

        /// <summary>
        /// Update a Asset Coding Definition by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateAstCodingDef")]
        [Authorize]
        public IActionResult UpdateAstCodingDef([FromBody] AstCodingDefReqParam astCodingDef)
        {
            Message msg = new Message();
            try
            {

                DataTable verifyRange = DataLogic.VerifyAssetCodingRange(astCodingDef.StartSerial, astCodingDef.EndSerial, SP_VerifyAssetCodingRange);

                if (verifyRange.Rows.Count > 0)
                {
                    msg.message = "Range Already Exists!";
                    msg.status = "404";
                    return Ok(msg);
                }
                else
                {
                    DataTable dt = DataLogic.UpdateAstCodingDef(astCodingDef, SP_GetInsertUpdateDeleteAstCoding);
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
                            string msgFromDB = dt.Rows[0]["Message"].ToString();
                            if (msgFromDB.Contains("successfully"))
                            {
                                //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Coding Definition", "Added", astCodingDef.LoginName);
                                DataTable dt2 = DataLogic.InsertAuditLogs("Asset Coding Definition", 1, "Updated", astCodingDef.LoginName, "dbo.AssetCoding");
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
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Delete Asset Coding Definition

        /// <summary>
        /// Delete a Asset Coding Definition by passing "Delete = 1" and AssetCodingID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteAstCodingDef")]
        [Authorize]
        public IActionResult DeleteAstCodingDef([FromBody] AstCodingDefReqParam astCodingDef)
        {
            Message msg = new Message();
            try
            {

                DataTable checkRelatedDataDT = DataLogic.CheckRelatedDataDT(astCodingDef.StartSerial, astCodingDef.EndSerial, SP_CheckRelatedAssets);

                if (checkRelatedDataDT.Rows.Count > 0)
                {
                    msg.message = "This record cannot be deleted as related records exists.";
                    msg.status = "404";
                    return Ok(msg);
                }
                else
                {
                    DataTable dt = DataLogic.DeleteAstCodingDef(astCodingDef, SP_GetInsertUpdateDeleteAstCoding);
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
                            string msgFromDB = dt.Rows[0]["Message"].ToString();
                            if (msgFromDB.Contains("successfully"))
                            {
                                //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Coding Definition", "Added", astCodingDef.LoginName);
                                DataTable dt2 = DataLogic.InsertAuditLogs("Asset Coding Definition", 1, "Deleted", astCodingDef.LoginName, "dbo.AssetCoding");
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
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #endregion

        #region Asset Item

        #region Get Asset Items Against CategoryID
        /// <summary>
        /// Get all Asset Items by passing a parameter "Get = 1 and pagination Parameters"
        /// </summary>
        /// <param name="astItemReq"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("GetAssetItemsAgainstCatID")]
        [Authorize]
        public IActionResult GetAssetItemsAgainstCatID([FromBody] AssetItemReq astItemReq)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            var logResult = string.Empty;
            DataTable dtTest = new DataTable();
            try
            {
                DataSet ds = DataLogic.GetAssetItemsAgainstCatID(astItemReq, SP_GetAssetItemsAgainstCatID);
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

        #region Get All Asset Items
        /// <summary>
        /// Get all Asset Items by passing a parameter "Get = 1 and pagination Parameters"
        /// </summary>
        /// <param name="astItemReq"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("GetAllAssetItems")]
        [Authorize]
        public IActionResult GetAllAssetItems([FromBody] AssetItemReq astItemReq)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            var logResult = string.Empty;
            DataTable dtTest = new DataTable();
            try
            {
                DataSet ds = DataLogic.GetAllAssetItems(astItemReq, SP_GetInsertUpdateDeleteAssetItem);
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
                //if (dt.Rows.Count > 0)
                //{
                //    if (dt.Columns.Contains("ErrorMessage"))
                //    {
                //        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                //        msg.status = "401";
                //        return Ok(msg);
                //    }
                //    else
                //    {
                //        //if (dt.Rows[0]["ImageStorageLoc"].ToString() != "Database")
                //        //{
                //        //    DataTable ItemCodeDT = new DataTable();
                //        //    ItemCodeDT.Columns.Add("ItemCode");
                //        //    for (int i = 0; i < dt.Rows.Count; i++)
                //        //    {
                //        //        DataRow dtRow = ItemCodeDT.NewRow();
                //        //        dtRow["ItemCode"] = dt.Rows[i]["ItemCode"].ToString();
                //        //        ItemCodeDT.Rows.Add(dtRow);
                //        //    }

                //        //    DataTable ImgFromDirDT = GF.GetAllFilesFromDirectory(ItemCodeDT, "AssetImg");

                //        //    if (ImgFromDirDT.Rows.Count > 0)
                //        //    {
                //        //        for (int i = 0; i < dt.Rows.Count; i++)
                //        //        {
                //        //            string oldValue = dt.Rows[i]["ImageBase64"].ToString();
                //        //            if (oldValue == "")
                //        //            {
                //        //                oldValue = " ";
                //        //                dt.Rows[i]["ImageBase64"] = " ";
                //        //            }
                //        //            string newValue = ImgFromDirDT.Rows[i]["ImageBase64Dir"].ToString();
                //        //            var a = dt.Rows[i]["ImageBase64"].ToString().Replace(oldValue, newValue);
                //        //            dt.Rows[i]["ImageBase64"] = a.ToString();
                //        //        }
                //        //    }
                //        //    else
                //        //    {
                //        //        for (int i = 0; i < dt.Rows.Count; i++)
                //        //        {
                //        //            string oldValue = dt.Rows[i]["ImageBase64"].ToString();
                //        //            if (oldValue == "")
                //        //            {
                //        //                oldValue = " ";
                //        //                dt.Rows[i]["ImageBase64"] = " ";
                //        //            }
                //        //            string newValue = " ";
                //        //            var a = dt.Rows[i]["ImageBase64"].ToString().Replace(oldValue, newValue);
                //        //            dt.Rows[i]["ImageBase64"] = a.ToString();
                //        //        }
                //        //    }

                //        //    return Ok(dt);
                //        //}
                //        //else
                //        //{
                //        //    return Ok(dt);
                //        //}



                //    }
                //}
                //else
                //{
                //    return Ok(dt);
                //}
            }
            catch (Exception ex)
            {
                //int lineNumber = 0;
                //var stackTrace = new StackTrace(ex, true);
                //if (stackTrace.FrameCount > 0)
                //{
                //    var frame = stackTrace.GetFrame(0);
                //    lineNumber = frame.GetFileLineNumber();
                //}

                //// Handle or log the exception along with the line number
                ////Console.WriteLine($"An error occurred at line {lineNumber}: {ex.Message}");
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Add Asset Item

        /// <summary>
        /// Insert a Asset Item by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertAssetItem")]
        [Authorize]
        public IActionResult InsertAssetItem([FromBody] AssetItemReq astItemReq)
        {
            ImageFunctionality imgFun = new ImageFunctionality();
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertAssetItem(astItemReq, SP_GetInsertUpdateDeleteAssetItem);
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (dt.Rows[0]["ImgStorageLoc"].ToString() != "Database" && msgFromDB.Contains("successfully"))
                        {
                            bool aa = imgFun.ConvertBase64toImg_SavetoServer(astItemReq.ImageBase64, dt.Rows[0]["NewItemCode"].ToString(), "AssetImg");
                        }
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Item", "Added", astItemReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Asset Item", 1, "Inserted", astItemReq.LoginName, "dbo.Assets");
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

        #region Update Asset Item

        /// <summary>
        /// Update a Asset Item Definition by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateAssetItem")]
        [Authorize]
        public IActionResult UpdateAssetItem([FromBody] AssetItemReq astItemReq)
        {
            GeneralFunctions GF = new GeneralFunctions();
            Message msg = new Message();
            ImageFunctionality imgFun = new ImageFunctionality();
            try
            {
                DataTable dt = DataLogic.UpdateAssetItem(astItemReq, SP_GetInsertUpdateDeleteAssetItem);
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (dt.Rows[0]["ImgStorageLoc"].ToString() != "Database" && msgFromDB.Contains("successfully"))
                        {
                            string deleteImg = GF.DeleteImg(astItemReq.ItemCode, "AssetImg");
                            bool aa = imgFun.ConvertBase64toImg_SavetoServer(astItemReq.ImageBase64, astItemReq.ItemCode, "AssetImg");
                        }
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Item", "Added", astItemReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Asset Item", 1, "Updated", astItemReq.LoginName, "dbo.Assets");
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

        #region Delete Asset Item

        /// <summary>
        /// Delete a Asset Item by passing "Delete = 1" and ItemCode as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteAssetItem")]
        [Authorize]
        public IActionResult DeleteAssetItem([FromBody] AssetItemReq astItemReq)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                DataTable dt = DataLogic.DeleteAssetItem(astItemReq, SP_GetInsertUpdateDeleteAssetItem);
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (dt.Rows[0]["ImgStorageLoc"].ToString() != "Database" && msgFromDB.Contains("successfully"))
                        {
                            string deleteImg = GF.DeleteImg(astItemReq.ItemCode,"AssetImg");
                        }
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Asset Item", 1, "Deleted", astItemReq.LoginName, "dbo.Assets");
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

        #region Get Ast Info Below Admin Screen Grid

        /// <summary>
        /// Get Asset Information for lower grid in Administrator Screen
        /// </summary>
        /// <param name="astInfoReqParam"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("GetAstInfoByItemCodeLowerAdminGrid")]
        [Authorize]
        public IActionResult GetAstInfoByItemCodeLowerAdminGrid([FromBody] AstInformationReqParam astInfoReqParam)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                if (astInfoReqParam.ItemCode == null || astInfoReqParam.ItemCode == "")
                {
                    msg.message = "Please provide ItemCode";
                    msg.status = "401";
                    return BadRequest(msg);
                }
                DataSet ds = DataLogic.GetAstInfoByItemCode(astInfoReqParam.ItemCode, SP_GetAstInfoAgainstItemCode_BelowAdminData);
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

                        DataTable table = ds.Tables["Table"];
                        table.TableName = "data";

                        return Ok(ds);
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
                msg.status = "401";
                return BadRequest(msg);
            }
        }

        #endregion

        #endregion

        #region Assets Administrator

        #region Get All Assets for Administrator Screen

        /// <summary>
        /// Get all Assets for Administrator by passing a parameter "Get = 1 or for Searching = 1 and for the filters, send that filter column
        /// and others as empty"
        /// </summary>
        /// <returns>This will return all the Assets</returns>
        [HttpPost("GetAllAssetsAdministrator")]
        [Authorize]
        public IActionResult GetAllAssetsAdministrator([FromBody] AssetAdministrator astAdmin)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllAssetsAdministrator(astAdmin, SP_GetAllAssetsForAdministrator);

                #region Replace Table Names

                DataTable table = ds.Tables["Table"];
                DataTable table1 = ds.Tables["Table1"];

                table.TableName = "TotalRowsCount";
                table1.TableName = "data";

                #endregion

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
                        return Ok(ds);
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

        #region Get Ast Info Below Admin Screen Grid

        /// <summary>
        /// Get Asset Information for lower grid in Administrator Screen
        /// </summary>
        /// <param name="astInfoReqParam"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("GetAstInfoByAstIDLowerAdminGrid")]
        [Authorize]
        public IActionResult GetAstInfoByAstIDLowerAdminGrid([FromBody] AstInformationReqParam astInfoReqParam)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                if (astInfoReqParam.AstID == null || astInfoReqParam.AstID == "")
                {
                    msg.message = "Please provide AstID";
                    msg.status = "401";
                    return BadRequest(msg);
                }
                DataSet ds = DataLogic.GetAstInfoByAstID(astInfoReqParam.AstID, SP_GetAstInfoAgainstAstID_BelowAdminData);
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

                        DataTable table = ds.Tables["Table"];
                        table.TableName = "data";

                        return Ok(ds);
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
                msg.status = "401";
                return BadRequest(msg);
            }
        }

        #endregion

        #endregion

        #region Details & Maintenance

        #region Get Complete Information

        /// <summary>
        /// Get Asset Information By AstID by passing AstID and it will return multiple tables. Table showing the Asset Information, Table1 showing the Depreciation Information, 
        /// Table2 showing the Tracking History, Table3 showing the Custodian Transfer History, Table4 showing the Additional Cost History,
        /// Table5 showing the Annual Expected Depreciation and Table6 showing the Monthly Expected Depreciation
        /// </summary>
        /// <param name="astInfoReqParam"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("GetAstInfoByAstID")]
        [Authorize]
        public IActionResult GetAstInfoByAstID([FromBody] AstInformationReqParam astInfoReqParam)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                DataSet ds = DataLogic.GetAstInfoByAstID(astInfoReqParam.AstID, SP_GetAssetInfoAgainstAstID);
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
                        if (ds.Tables[1].Rows.Count > 0)
                        {

                            var baseCostThousand = Convert.ToDouble(ds.Tables[0].Rows[0]["AcquisitionPrice"].ToString()).ToString("N");
                            var currentBVThousand = Convert.ToDouble(ds.Tables[1].Rows[0]["CurrentBV"].ToString()).ToString("N");

                            Int16 salvageYeardt = Convert.ToInt16(ds.Tables[1].Rows[0]["SalvageYear"].ToString());
                            Int16 salvageMonthdt = Convert.ToInt16(ds.Tables[1].Rows[0]["SalvageMonth"].ToString());
                            double salvageValueDouble = Convert.ToDouble(ds.Tables[1].Rows[0]["SalvageValue"].ToString());
                            DateOnly lastBookUpdateDateonly = DateOnly.FromDateTime(Convert.ToDateTime(ds.Tables[1].Rows[0]["BookUpdateDate"].ToString()));
                            DateOnly serviceDateonly = DateOnly.FromDateTime(Convert.ToDateTime(ds.Tables[0].Rows[0]["ServiceDate"].ToString()));
                            int intDepreciationType = Convert.ToInt32(ds.Tables[0].Rows[0]["DepreciationCode"].ToString());

                            #region Annual Depreciation Section

                            DataTable depDataTableYear = new DataTable();
                            depDataTableYear = DepreciationAlgorithm.CalcDepAnnual(salvageYeardt, salvageMonthdt, lastBookUpdateDateonly, baseCostThousand, currentBVThousand, intDepreciationType, salvageValueDouble, serviceDateonly);
                            
                            depDataTableYear.Columns.Add("Year");
                            depDataTableYear.Columns.Add("StartValues");
                            depDataTableYear.Columns.Add("DepreciationExpenses");
                            depDataTableYear.Columns.Add("AccumulatedDepreciations");
                            depDataTableYear.Columns.Add("EndValues");

                            for (int i = 0; i < depDataTableYear.Rows.Count; i++)
                            {
                                depDataTableYear.Rows[i]["Year"] = DateOnly.FromDateTime(Convert.ToDateTime(depDataTableYear.Rows[i]["CurrDate"].ToString())).Year;
                                depDataTableYear.Rows[i]["StartValues"] = Convert.ToDouble(depDataTableYear.Rows[i]["StartValue"]).ToString("N");
                                depDataTableYear.Rows[i]["DepreciationExpenses"] = Convert.ToDouble(depDataTableYear.Rows[i]["Dep"]).ToString("N");
                                depDataTableYear.Rows[i]["AccumulatedDepreciations"] = Convert.ToDouble(depDataTableYear.Rows[i]["AccDep"]).ToString("N");
                                depDataTableYear.Rows[i]["EndValues"] = Convert.ToDouble(depDataTableYear.Rows[i]["CBV"]).ToString("N");
                            }

                            depDataTableYear.Columns.Remove("CurrDate");
                            depDataTableYear.Columns.Remove("StartValue");
                            depDataTableYear.Columns.Remove("Dep");
                            depDataTableYear.Columns.Remove("AccDep");
                            depDataTableYear.Columns.Remove("CBV");
                            depDataTableYear.Columns["Year"].SetOrdinal(0);

                            ds.Tables.Add(depDataTableYear);

                            #endregion

                            #region Monthly Depreciation Section

                            DataTable depDataTableMonth = new DataTable();
                            depDataTableMonth = DepreciationAlgorithm.CalcDepMonthly(salvageYeardt, salvageMonthdt, lastBookUpdateDateonly, baseCostThousand, currentBVThousand, intDepreciationType, salvageValueDouble, serviceDateonly);

                            depDataTableMonth.Columns.Add("Year");
                            depDataTableMonth.Columns.Add("StartValues");
                            depDataTableMonth.Columns.Add("DepreciationExpenses");
                            depDataTableMonth.Columns.Add("AccumulatedDepreciations");
                            depDataTableMonth.Columns.Add("EndValues");

                            for (int i = 0; i < depDataTableMonth.Rows.Count; i++)
                            {
                                depDataTableMonth.Rows[i]["Year"] = depDataTableMonth.Rows[i]["CurrDate"].ToString();
                                depDataTableMonth.Rows[i]["StartValues"] = Convert.ToDouble(depDataTableMonth.Rows[i]["StartValue"]).ToString("N");
                                depDataTableMonth.Rows[i]["DepreciationExpenses"] = Convert.ToDouble(depDataTableMonth.Rows[i]["Dep"]).ToString("N");
                                depDataTableMonth.Rows[i]["AccumulatedDepreciations"] = Convert.ToDouble(depDataTableMonth.Rows[i]["AccDep"]).ToString("N");
                                depDataTableMonth.Rows[i]["EndValues"] = Convert.ToDouble(depDataTableMonth.Rows[i]["CBV"]).ToString("N");
                            }

                            depDataTableMonth.Columns.Remove("CurrDate");
                            depDataTableMonth.Columns.Remove("StartValue");
                            depDataTableMonth.Columns.Remove("Dep");
                            depDataTableMonth.Columns.Remove("AccDep");
                            depDataTableMonth.Columns.Remove("CBV");
                            depDataTableMonth.Columns["Year"].SetOrdinal(0);

                            ds.Tables.Add(depDataTableMonth);

                            #endregion

                        }

                        else
                        {
                            DataTable depDataTableYear = new DataTable();

                            depDataTableYear.Columns.Add("Year");
                            depDataTableYear.Columns.Add("StartValues");
                            depDataTableYear.Columns.Add("DepreciationExpenses");
                            depDataTableYear.Columns.Add("AccumulatedDepreciations");
                            depDataTableYear.Columns.Add("EndValues");
                            ds.Tables.Add(depDataTableYear);

                            DataTable depDataTableMonth = new DataTable();

                            depDataTableMonth.Columns.Add("Year");
                            depDataTableMonth.Columns.Add("StartValues");
                            depDataTableMonth.Columns.Add("DepreciationExpenses");
                            depDataTableMonth.Columns.Add("AccumulatedDepreciations");
                            depDataTableMonth.Columns.Add("EndValues");

                            ds.Tables.Add(depDataTableMonth);

                        }

                        #region Table Names Replacement

                        DataTable table = ds.Tables["Table"];
                        DataTable table1 = ds.Tables["Table1"];
                        DataTable table2 = ds.Tables["Table2"];
                        DataTable table3 = ds.Tables["Table3"];
                        DataTable table4 = ds.Tables["Table4"];
                        DataTable table5 = ds.Tables["Table5"];
                        DataTable table6 = ds.Tables["Table6"];
                        DataTable table7 = ds.Tables["Table7"];
                        table.TableName = "AssetInformationExtendentedInformation";
                        table1.TableName = "AssetBookInformation";
                        table2.TableName = "TrackingInformation";
                        table3.TableName = "CustodyTransferInformation";
                        table4.TableName = "AdditionalCostInformation";
                        table5.TableName = "WarrantyData";
                        table6.TableName = "AnnualExpectedDepreciation";
                        table7.TableName = "MonthlyExpectedDepreciation";

                        #endregion

                        return Ok(ds);
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

        #region Add Asset Details

        /// <summary>
        /// Insert Asset Details by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertAssetDetails")]
        [Authorize]
        public IActionResult InsertAssetDetails([FromBody] AstInformationReqParam astInfoReqParam)
        {
            ImageFunctionality imgFun = new ImageFunctionality();
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                if (astInfoReqParam.PurDate == "" || astInfoReqParam.SrvDate == "")
                {
                    msg.message = "Dates are not valid";
                    msg.status = "401";
                    return BadRequest(msg);
                }
                DateTime purchaseDate = Convert.ToDateTime(astInfoReqParam.PurDate);
                DateTime serviceDate = Convert.ToDateTime(astInfoReqParam.SrvDate);

                if (DateTime.Compare(purchaseDate.Date, serviceDate.Date) > 0)
                {
                    msg.message = "Purchase Date Should Be Less Than Service Date";
                    msg.status = "401";
                    return BadRequest(msg);
                }

                if (DataLogic.CheckBookExistsAgainstCompanyID(astInfoReqParam.CompanyID.ToString(), "[dbo].[SP_CheckBookExistsAgainstCompanyID]").Rows.Count == 0)
                {
                    msg.message = "No Book selected against the provided Company";
                    msg.status = "401";
                    return BadRequest(msg);
                }


                DataTable dt = DataLogic.InsertAssetDetails(astInfoReqParam, SP_InsertUpdateDeleteAssetDetails);

                DataTable barcode_Assign = DataLogic.Barcode_AssignToSelectedCompany(astInfoReqParam.CompanyID.ToString(), "[dbo].[SP_GetCompanyBarcode_Assign]");

                string barcode_Structure_Assign = barcode_Assign.Rows[0]["BarCode"].ToString();
                string valueSep = barcode_Assign.Rows[0]["ValueSep"].ToString();
                int CompanyID = astInfoReqParam.CompanyID;

                DataTable dtapplyBarcode = GF.ApplyPolicySingleAst(astInfoReqParam.AstID, barcode_Structure_Assign, CompanyID, valueSep);

                DataTable dtupdateBarcode = DataLogic.ApplyBarcodePolicy(dtapplyBarcode, "[dbo].[SP_ApplyBarcodePolicy]");

                DataTable dtupdateAstNumber = DataLogic.UpdateCompanyLastAssetNumber(astInfoReqParam.CompanyID.ToString(), dt.Rows[0]["Astnum"].ToString(), "[dbo].[SP_UpdateLastAssetNumberOfCompany]");

                DataTable dtGetDepPolicyAgainstItemCode = DataLogic.GetDepPolicyAgainstItemCode(astInfoReqParam.ItemCode, "[dbo].[SP_GetDepPolicyAgainstItemCode]");

                DataTable dtBookInfoAgainstCompanyID = DataLogic.GetBookAgainstCompanyID(astInfoReqParam.CompanyID.ToString(), "[dbo].[SP_GetBooksAgainstCompanyID]");

                bool checkBookExistsAgainstAstID = GF.Check_BookExists(Convert.ToInt32(dtBookInfoAgainstCompanyID.Rows[0]["BookID"]), astInfoReqParam.AstID);

                if (GF.Check_BookExists(Convert.ToInt32(dtBookInfoAgainstCompanyID.Rows[0]["BookID"]), astInfoReqParam.AstID) == false)
                {
                    double Cost = astInfoReqParam.BaseCost + astInfoReqParam.Tax;
                    double LastBookValue = 0.00;
                    double salvageValuePercentage;
                    double salvageValue = Convert.ToDouble(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageValue"]);
                    double salvageValue2;
                    bool isSalvageValuePercentage = Convert.ToBoolean(dtGetDepPolicyAgainstItemCode.Rows[0]["IsSalvageValuePercent"]);
                    if (isSalvageValuePercentage)
                    {
                        salvageValuePercentage = salvageValue;
                        salvageValue2 = (Convert.ToDouble(Cost) * Convert.ToDouble(salvageValue)) / 100;
                    }
                    else
                    {
                        if (Cost > 0)
                        {
                            salvageValuePercentage = Math.Round((salvageValue / Cost) * 100, 2);
                        }
                        else
                        {
                            salvageValuePercentage = 0;
                        }
                        salvageValue2 = salvageValue;
                    }
                    DataTable insertIntoAstBook = DataLogic.AddBookForPOAsset(
                        Convert.ToInt32(dtBookInfoAgainstCompanyID.Rows[0]["BookID"]),
                        astInfoReqParam.AstID,
                        Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["DepCode"]),
                        Convert.ToDouble(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageValue"]),
                        Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageYear"]),
                        0.00,
                        astInfoReqParam.BaseCost + astInfoReqParam.Tax,
                        astInfoReqParam.SrvDate,
                        Convert.ToInt32(dtGetDepPolicyAgainstItemCode.Rows[0]["SalvageMonth"]),
                        salvageValuePercentage,
                        "[dbo].[SP_InsertAstIn_AstBook]"
                        );
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (dt.Rows[0]["ImgStorageLoc"].ToString() != "Database" && msgFromDB.Contains("successfully"))
                        {
                            bool aa = imgFun.ConvertBase64toImg_SavetoServer(astInfoReqParam.ImageBase64, astInfoReqParam.AstID, "AssetImg");
                        }
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Item", "Added", astItemReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Asset Details", 1, "Inserted", astInfoReqParam.LoginName, "dbo.AssetDetails");
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

        #region Update Asset Item

        /// <summary>
        /// Update Asset Details by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateAssetDetails")]
        [Authorize]
        public IActionResult UpdateAssetDetails([FromBody] AstInformationReqParam astInfoReqParam)
        {
            GeneralFunctions GF = new GeneralFunctions();
            Message msg = new Message();
            ImageFunctionality imgFun = new ImageFunctionality();
            try
            {
                DataTable dt = DataLogic.UpdateAssetDetails(astInfoReqParam, SP_InsertUpdateDeleteAssetDetails);
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (dt.Rows[0]["ImgStorageLoc"].ToString() != "Database" && msgFromDB.Contains("successfully"))
                        {
                            string deleteImg = GF.DeleteImg(astInfoReqParam.AstID, "AssetImg");
                            bool aa = imgFun.ConvertBase64toImg_SavetoServer(astInfoReqParam.ImageBase64, astInfoReqParam.AstID, "AssetImg");
                        }
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Asset Item", "Added", astItemReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Asset Details", 1, "Updated", astInfoReqParam.LoginName, "dbo.AssetDetails");
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

        #region Delete Asset Item

        /// <summary>
        /// Delete Asset Details by passing "Delete = 1" and AstID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteAssetDetails")]
        [Authorize]
        public IActionResult DeleteAssetDetails([FromBody] AstInformationReqParam astInfoReqParam)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                DataTable dt = DataLogic.DeleteAssetDetails(astInfoReqParam, SP_InsertUpdateDeleteAssetDetails);
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
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (dt.Rows[0]["ImgStorageLoc"].ToString() != "Database" && msgFromDB.Contains("successfully"))
                        {
                            string deleteImg = GF.DeleteImg(astInfoReqParam.AstID, "AssetImg");
                        }
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Asset Details", 1, "Deleted", astInfoReqParam.LoginName, "dbo.AssetDetails");
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

        #endregion

        #region Search Assets

        /// <summary>
        /// Search By Asset by passing "Get = 1" and other Filtered paramters and Pagination Attributes
        /// </summary>
        /// <param name="searchAstsParams"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("SearchAssets")]
        [Authorize]
        public IActionResult SearchAssets([FromBody] SearchAstsParams searchAstsParams)
        {

            Message msg = new Message();

            try
            {
                DataSet ds = DataLogic.SearchAssets(searchAstsParams, SP_AssetSearch);
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

        #region Location/Custody Transfer

        /// <summary>
        /// This API is used when transferring the assets from a Location to other Location or from a Custodian to other Custodian or Changing the Assets
        /// Status in BULK
        /// </summary>
        /// <param name="locCustTransferReqParams"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("Location_Custody_Transfer")]
        [Authorize]
        public IActionResult Location_Custody_Transfer([FromBody] LocCustTransferReqParams locCustTransferReqParams)
        {

            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                #region Declaration

                string fr_Loc, to_Loc, assetStatus;

                DataTable assetsToBeTransferDTLocationTransfer = new DataTable(); DataTable assetsToBeTransferDTCustodianTransfer = new DataTable(); DataTable assetsToBeTransferDTAssetStatusTransfer = new DataTable();

                assetsToBeTransferDTLocationTransfer.Columns.Add("historyID"); assetsToBeTransferDTLocationTransfer.Columns.Add("InvSchCode"); assetsToBeTransferDTLocationTransfer.Columns.Add("AstID"); assetsToBeTransferDTLocationTransfer.Columns.Add("Fr_Loc"); assetsToBeTransferDTLocationTransfer.Columns.Add("To_Loc"); assetsToBeTransferDTLocationTransfer.Columns.Add("HisDate"); assetsToBeTransferDTLocationTransfer.Columns.Add("Status"); assetsToBeTransferDTLocationTransfer.Columns.Add("AssetStatus"); assetsToBeTransferDTLocationTransfer.Columns.Add("LastEditBy"); assetsToBeTransferDTLocationTransfer.Columns.Add("Barcode");

                assetsToBeTransferDTCustodianTransfer.Columns.Add("HistoryID"); assetsToBeTransferDTCustodianTransfer.Columns.Add("AstID"); assetsToBeTransferDTCustodianTransfer.Columns.Add("ToCustodian"); assetsToBeTransferDTCustodianTransfer.Columns.Add("FromCustodian"); assetsToBeTransferDTCustodianTransfer.Columns.Add("HisDate");

                if (assetsToBeTransferDTLocationTransfer != null)
                {
                    assetsToBeTransferDTLocationTransfer = ListintoDataTable.ToDataTable(locCustTransferReqParams.locTransferTree);
                }

                if (assetsToBeTransferDTCustodianTransfer != null)
                {
                    assetsToBeTransferDTCustodianTransfer = ListintoDataTable.ToDataTable(locCustTransferReqParams.custTransferTree);
                }

                if (assetsToBeTransferDTAssetStatusTransfer != null)
                {
                    assetsToBeTransferDTAssetStatusTransfer = ListintoDataTable.ToDataTable(locCustTransferReqParams.assetStatusTransferTree);
                }

                #endregion

                if (assetsToBeTransferDTLocationTransfer.Rows.Count <= 0 && assetsToBeTransferDTCustodianTransfer.Rows.Count <= 0 && assetsToBeTransferDTAssetStatusTransfer.Rows.Count <= 0)
                {
                    msg.message = "Asset must be selected!";
                    msg.status = "401";
                    return Ok(msg);
                }
                else
                {
                    if (locCustTransferReqParams.LocationCheckbox == 0 && locCustTransferReqParams.CustodianCheckbox == 0 && locCustTransferReqParams.AssetStatusCheckbox == 0)
                    {
                            msg.message = "Please select Transfer Method!";
                            msg.status = "401";
                            return Ok(msg);
                    }
                    else
                    {
                        #region Location Transfer Work

                        if (locCustTransferReqParams.LocationCheckbox == 1)
                        {
                            DataTable historyID = DataLogic.GetHighestHistoryID(SP_GetAst_HistoryHighestHistoryID);
                            for (int i = 0; i < assetsToBeTransferDTLocationTransfer.Rows.Count; i++)
                            {
                                string astidnew = assetsToBeTransferDTLocationTransfer.Rows[i]["AstID"].ToString();
                                string Barcode_Policy_Text = DataLogic.GetBarcodeStructureAgainstAstID(astidnew, "[dbo].[SP_GetBarcodeStructureAgainstAstID]").Rows[0]["BarCode"].ToString();
                                string AstNum = DataLogic.GetBarcodeStructureAgainstAstID(astidnew, "[dbo].[SP_GetBarcodeStructureAgainstAstID]").Rows[0]["AstNum"].ToString();
                                string RefNo = DataLogic.GetBarcodeStructureAgainstAstID(astidnew, "[dbo].[SP_GetBarcodeStructureAgainstAstID]").Rows[0]["RefNo"].ToString();
                                string valueSep = DataLogic.GetBarcodeStructureAgainstAstID(astidnew, "[dbo].[SP_GetBarcodeStructureAgainstAstID]").Rows[0]["ValueSep"].ToString();
                                string Location = DataLogic.GetLocCompCode(assetsToBeTransferDTLocationTransfer.Rows[i]["To_Loc"].ToString()).Rows[0]["CompCode"].ToString();
                                string replaceLocCompCode = Location.Replace("\\", "-");
                                string Category = DataLogic.GetBarcodeStructureAgainstAstID(astidnew, "[dbo].[SP_GetBarcodeStructureAgainstAstID]").Rows[0]["CatCompCode"].ToString();
                                assetsToBeTransferDTLocationTransfer.Rows[i]["HistoryID"] = Convert.ToInt32(historyID.Rows[0]["HistoryID"]) + (i + 1);
                                assetsToBeTransferDTLocationTransfer.Rows[i]["InvSchCode"] = 1;
                                assetsToBeTransferDTLocationTransfer.Rows[i]["AstID"] = astidnew;
                                assetsToBeTransferDTLocationTransfer.Rows[i]["Fr_Loc"] = assetsToBeTransferDTLocationTransfer.Rows[i]["Fr_Loc"].ToString();
                                assetsToBeTransferDTLocationTransfer.Rows[i]["To_Loc"] = assetsToBeTransferDTLocationTransfer.Rows[i]["To_Loc"].ToString();
                                assetsToBeTransferDTLocationTransfer.Rows[i]["HisDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                assetsToBeTransferDTLocationTransfer.Rows[i]["Status"] = 3; //These records are considered as Transferred because they are moving to other Location
                                assetsToBeTransferDTLocationTransfer.Rows[i]["AssetStatus"] = "3";  //I think this is basically asset condition of asest i.e., In Used, Damaged, Scraped etc
                                string Barcode = GF.Generate_Barcode(1, assetsToBeTransferDTLocationTransfer.Rows[i]["AstID"].ToString(), AstNum, RefNo, Category, Category, Location, Location, replaceLocCompCode, valueSep, Barcode_Policy_Text);
                                assetsToBeTransferDTLocationTransfer.Rows[i]["Barcode"] = Barcode;
    
                            }

                        }

                        #endregion

                        #region Custodian Transfer Work

                        if (locCustTransferReqParams.CustodianCheckbox == 1)
                        {
                            DataTable custHistoryID = DataLogic.GetHighestHistoryID(SP_GetAst__Cust_HistoryHighestHistoryID);

                            for (int i = 0; i < assetsToBeTransferDTCustodianTransfer.Rows.Count; i++)
                            {
                                assetsToBeTransferDTCustodianTransfer.Rows[i]["HistoryID"] = Convert.ToInt32(custHistoryID.Rows[0]["HistoryID"]) + (i + 1);
                                assetsToBeTransferDTCustodianTransfer.Rows[i]["HisDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                            }
                        }

                        #endregion

                        DataTable dt = DataLogic.Location_Custody_Transfer(locCustTransferReqParams, assetsToBeTransferDTLocationTransfer, assetsToBeTransferDTCustodianTransfer, assetsToBeTransferDTAssetStatusTransfer, SP_Assets_Loc_Cust_Status_Transfer);

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
                                msg.message = dt.Rows[0]["Message"].ToString();
                                msg.status = "200";
                                return Ok(msg);
                            }
                        }
                        else
                        {
                            return Ok(dt);
                        }


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

        #region Item Category Transfer

        /// <summary>
        /// This API is used when transferring the Category of an Item or multiple items to another Category
        /// Status in BULK
        /// </summary>
        /// <param name="itemCategoryTransferReqParams"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("ItemCategoryTransfer")]
        [Authorize]
        public IActionResult ItemCategoryTransfer([FromBody] ItemCategoryTransferReqParams itemCategoryTransferReqParams)
        {

            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                #region Declaration

                DataTable itemsToBeTransferDTCategoryTransfer = new DataTable();

                itemsToBeTransferDTCategoryTransfer.Columns.Add("ItemCode"); itemsToBeTransferDTCategoryTransfer.Columns.Add("NewCatID");

                if (itemsToBeTransferDTCategoryTransfer != null)
                {
                    itemsToBeTransferDTCategoryTransfer = ListintoDataTable.ToDataTable(itemCategoryTransferReqParams.itemCategoryTransferTrees);
                }

                #endregion

                if (itemsToBeTransferDTCategoryTransfer.Rows.Count < 0)
                {
                    msg.message = "Item(s) must be selected!";
                    msg.status = "401";
                    return Ok(msg);
                }
                else
                {

                    DataTable dt = DataLogic.ItemCategoryTransfer(itemsToBeTransferDTCategoryTransfer, SP_Item_Category_Transfer);

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

                            string msgFromDB = dt.Rows[0]["Message"].ToString();
                            
                            if (msgFromDB.Contains("successfully"))
                            {
                                DataTable dt2 = DataLogic.InsertAuditLogs("Assets Category", 1, "Updated", itemCategoryTransferReqParams.LoginName, "dbo.Assets");
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

            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Update Deprecation For Asset

        /// <summary>
        /// This API is used when transferring the Category of an Item or multiple items to another Category
        /// Status in BULK
        /// </summary>
        /// <param name="updDepAsset"></param>
        /// <returns>This will return all the Asset Items</returns>
        [HttpPost("UpdateDepForAsset")]
        [Authorize]
        public IActionResult UpdateDepForAsset([FromBody] UpdateDepreciationOnAsset updDepAsset)
        {

            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            BooksReqParam bookReqParams = new BooksReqParam();
            DepPolicy_History depPolicy_His = new DepPolicy_History();
            AstBookReqParams astBookReqParams = new AstBookReqParams();
            DataTable dt_InsertDepPolicy_History = new DataTable();
            DataTable dt_UpdateAstBook = new DataTable();

            try
            {

                double intVal = 0.00;

                updDepAsset.SalvagePercent = GF.CalculatePercent(updDepAsset.SalvageYear, updDepAsset.SalvageMonth);
                intVal = Convert.ToDouble(updDepAsset.SalvagePercent);
                if (intVal < 0 || intVal > 100)
                {
                    msg.message = "Unable to update Depreciation because the Percentage range is out.";
                    msg.status = "404";
                    return BadRequest(msg);
                }

                double currentBookValue = 0.00;
                DateTime BVUpdate = DateTime.Now;
                
                //bookView count needs to check

                DateTime bvUpdate = Convert.ToDateTime(updDepAsset.BVUpdate.ToString());

                if (DateTime.Compare(bvUpdate.Date, DateTime.Now.Date) <= 0)
                {
                    bookReqParams.Description = updDepAsset.BookDescription.Trim();
                    //if (updDepAsset.btnSaveDep)
                    //{
                        currentBookValue = updDepAsset.CurrentBV;
                    //}
                    //else
                    //{
                    //    currentBookValue = Convert.ToDecimal((updDepAsset.CurrentBV)) + (Convert.ToDecimal(updDepAsset.CurrentBV) - Convert.ToDecimal());
                    //    //currentBookValue = Convert.ToDecimal((updDepAsset.CurrentBV)) + (Convert.ToDecimal(updDepAsset.CurrentBV) - PreviousItemCost);
                    //}

                    astBookReqParams.BookID = updDepAsset.BookID;
                    astBookReqParams.AstID = updDepAsset.AstID;
                    astBookReqParams.DepCode = Convert.ToInt32(updDepAsset.DepText);
                    astBookReqParams.SalvageValue = updDepAsset.SalvageValue;
                    astBookReqParams.SalvageYear = Convert.ToDouble(updDepAsset.SalvageYear);
                    astBookReqParams.LastBV = 0;
                    astBookReqParams.CurrentBV = updDepAsset.CurrentBV;
                    astBookReqParams.BVUpdate = Convert.ToDateTime(updDepAsset.BVUpdate);
                    //astBookReqParams.SalvagePercent = updDepAsset.SalvageValuePercent;

                    if (updDepAsset.TotalCost > 0)
                    {
                        updDepAsset.SalvageValuePercent = Math.Round((updDepAsset.SalvageValue / updDepAsset.TotalCost) * 100, 2);
                    }
                    else
                    {
                        updDepAsset.SalvageValuePercent = 0;
                    }

                    astBookReqParams.SalvagePercent = updDepAsset.SalvageValuePercent;

                    if (updDepAsset.btnSaveDep)
                    {
                        depPolicy_His.DepCode = Convert.ToInt32(updDepAsset.DepText);
                        depPolicy_His.SalvageValue = updDepAsset.SalvageValue;
                        depPolicy_His.SalvageYear = Convert.ToDouble(updDepAsset.SalvageYearPrevious);
                        depPolicy_His.SalvageMonth = Convert.ToInt32(updDepAsset.SalvageMonthPrevious);
                        depPolicy_His.BookID = updDepAsset.BookID;
                        depPolicy_His.AstID = updDepAsset.AstID;
                        depPolicy_His.CurrentBV = Convert.ToDouble(updDepAsset.CurrentBVPrevious);
                        depPolicy_His.BVUpdate = Convert.ToDateTime(updDepAsset.BVUpdate.ToString()).Date;
                        dt_InsertDepPolicy_History = DataLogic.InsertDepPolicy_History(depPolicy_His, SP_InsertDepPolicy_History);
                    }

                    if (dt_InsertDepPolicy_History.Rows[0]["Status"].ToString() == "200")
                    {
                        dt_UpdateAstBook = DataLogic.UpdateAstBook(astBookReqParams, SP_UpdateAstBookAgainstAstIDAndBookID);
                    }

                    DataTable newAstBookData = DataLogic.GetAstBookDetailsAgainstAstID(updDepAsset.AstID, SP_GetAstBookByAstID);
                    //Desktop App mai Fiscal Yr ka kaam bhi wo Cloud solution mai nh h tw is waja s include nh kiya

                    DateOnly serviceDateonly = DateOnly.FromDateTime(Convert.ToDateTime(newAstBookData.Rows[0]["ServiceDate"].ToString()));

                    DataTable depreciationDataTableAnnually = DepreciationAlgorithm.CalcDepAnnual(Convert.ToInt16(updDepAsset.SalvageYear), Convert.ToInt16(updDepAsset.SalvageMonth), 
                        DateOnly.FromDateTime(Convert.ToDateTime(updDepAsset.BVUpdate)), updDepAsset.TotalCost.ToString(), 
                        updDepAsset.CurrentBV.ToString(), Convert.ToInt32(updDepAsset.DepText), updDepAsset.SalvageValue, serviceDateonly);


                    DataTable depreciationDataTableMonthly = DepreciationAlgorithm.CalcDepMonthly(Convert.ToInt16(updDepAsset.SalvageYear), Convert.ToInt16(updDepAsset.SalvageMonth),
                        DateOnly.FromDateTime(Convert.ToDateTime(updDepAsset.BVUpdate)), updDepAsset.TotalCost.ToString(),
                        updDepAsset.CurrentBV.ToString(), Convert.ToInt32(updDepAsset.DepText), updDepAsset.SalvageValue, serviceDateonly);

                    DataSet completeData = new DataSet();
                    completeData.Tables.Add(newAstBookData);
                    completeData.Tables.Add(depreciationDataTableAnnually);
                    completeData.Tables.Add(depreciationDataTableMonthly);

                    return Ok(completeData);

                }
                else
                {

                    msg.message = "Cannot change book information, because depreciation already ran on current asset, only asset information will be saved.";
                    msg.status = "";

                }

                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

    }
}
