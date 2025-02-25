using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using ZulAssetsBackEnd_API.BAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("2")]
    //[ApiExplorerSettings(GroupName = "v2")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class BarcodeStructureController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteBarcode_Struct_Col = "[dbo].[SP_GetInsertUpdateDeleteBarcode_Struct_Col]";
        private static string SP_GetInsertUpdateDeleteBarcodeStructure = "[dbo].[SP_GetInsertUpdateDeleteBarcodeStructure]";
        private static string SP_GetAllBarcodePolicies = "[dbo].[SP_GetAllBarcodePolicies]";
        private static string SP_UpdateCompanyBarcodeStructure = "[dbo].[SP_UpdateCompanyBarcodeStructure]";
        private static string SP_GetBarcodeAgainstBarStructID = "[dbo].[SP_GetBarcodeAgainstBarStructID]";
        private static string SP_UpdateBarStructIDAgainstItemCode = "[dbo].[SP_UpdateBarStructIDAgainstItemCode]";

        #endregion

        #region Get All Barcode Structure Columns
        /// <summary>
        /// Get all Barcode Structure Columns Parameters by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Cost Centers</returns>
        [HttpPost("GetAllBarcodeStructureCol")]
        [Authorize]
        public IActionResult GetAllBarcodeStructureCol([FromBody] BarcodeStructCol barcodeStructColReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllBarcodeStructureCol(barcodeStructColReqParam, SP_GetInsertUpdateDeleteBarcode_Struct_Col);
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

        //#region Add Barcode Structure Column

        ///// <summary>
        ///// Insert a Barcode Structure Column by passing all parameters with "Add = 1" and others as empty
        ///// </summary>
        ///// <returns>This will return a message of success</returns>
        //[HttpPost("InsertBarcodeStructureCol")]
        //[Authorize]
        //public IActionResult InsertBarcodeStructureCol([FromBody] BarcodeStructCol barcodeStructColReqParam)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.InsertBarcodeStructureCol(barcodeStructColReqParam, SP_GetInsertUpdateDeleteBarcode_Struct_Col);
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Columns.Contains("ErrorMessage"))
        //            {
        //                msg.message = dt.Rows[0]["ErrorMessage"].ToString();
        //                msg.status = "401";
        //                return Ok(msg);
        //            }
        //            else
        //            {
        //                //var logResult = GeneralFunctions.CreateAndWriteToFile("Barcode Structure Column", "Added", barcodeStructCol.LoginName);
        //                string msgFromDB = dt.Rows[0]["Message"].ToString();
        //                if (msgFromDB.Contains("successfully"))
        //                {
        //                    //var logResult = GeneralFunctions.CreateAndWriteToFile("Barcode Structure Column", "Added", barcodeStructCol.LoginName);
        //                    DataTable dt2 = DataLogic.InsertAuditLogs("Barcode Structure Column", 1, "Inserted", barcodeStructColReqParam.LoginName, "dbo.BarCode_Struct_Col");
        //                }
        //                msg.message = dt.Rows[0]["Message"].ToString();
        //                msg.status = dt.Rows[0]["Status"].ToString();
        //                return Ok(msg);
        //            }
        //        }
        //        else
        //        {
        //            return Ok(dt);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.message = ex.Message;
        //        return Ok(msg);
        //    }
        //}

        //#endregion

        //#region Update Barcode Structure Column

        ///// <summary>
        ///// Update a Barcode Structure Column by passing all parameters with "Update = 1" and others as empty
        ///// </summary>
        ///// <returns>This will return a message of success</returns>
        //[HttpPost("UpdateBarcodeStructureCol")]
        //[Authorize]
        //public IActionResult UpdateBarcodeStructureCol([FromBody] BarcodeStructCol barcodeStructCol)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.UpdateBarcodeStructureCol(barcodeStructCol, SP_GetInsertUpdateDeleteBarcode_Struct_Col);
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Columns.Contains("ErrorMessage"))
        //            {
        //                msg.message = dt.Rows[0]["ErrorMessage"].ToString();
        //                msg.status = "401";
        //                return Ok(msg);
        //            }
        //            else
        //            {
        //                string msgFromDB = dt.Rows[0]["Message"].ToString();
        //                if (msgFromDB.Contains("successfully"))
        //                {
        //                    DataTable dt2 = DataLogic.InsertAuditLogs("Barcode Structure Column", 1, "Updated", barcodeStructCol.LoginName, "dbo.BarCode_Struct_Col");
        //                }
        //                msg.message = dt.Rows[0]["Message"].ToString();
        //                msg.status = dt.Rows[0]["Status"].ToString();
        //                return Ok(msg);
        //            }
        //        }
        //        else
        //        {
        //            return Ok(dt);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.message = ex.Message;
        //        return Ok(msg);
        //    }
        //}

        //#endregion

        //#region Delete Barcode Structure Column

        ///// <summary>
        ///// Delete a Barcode Structure Column by passing "Delete = 1" and ID as parameters and others as empty
        ///// </summary>
        ///// <returns>This will return a message of success</returns>
        //[HttpPost("DeleteBarcodeStructureCol")]
        //[Authorize]
        //public IActionResult DeleteBarcodeStructureCol([FromBody] BarcodeStructCol barcodeStructColReqParam)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.DeleteBarcodeStructureCol(barcodeStructColReqParam, SP_GetInsertUpdateDeleteBarcode_Struct_Col);
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Columns.Contains("ErrorMessage"))
        //            {
        //                msg.message = dt.Rows[0]["ErrorMessage"].ToString();
        //                msg.status = "401";
        //                return Ok(msg);
        //            }
        //            else
        //            {
        //                string msgFromDB = dt.Rows[0]["Message"].ToString();
        //                if (msgFromDB.Contains("successfully"))
        //                {
        //                    DataTable dt2 = DataLogic.InsertAuditLogs("Barcode Structure Column", 1, "Deleted", barcodeStructColReqParam.LoginName, "dbo.BarCode_Struct_Col");
        //                }
        //                msg.message = dt.Rows[0]["Message"].ToString();
        //                msg.status = dt.Rows[0]["Status"].ToString();
        //                return Ok(msg);
        //            }
        //        }
        //        else
        //        {
        //            return Ok(dt);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.message = ex.Message;
        //        return Ok(msg);
        //    }
        //}

        //#endregion

        #region Get All Barcode Structures
        /// <summary>
        /// Get all Barcode Structures Parameters by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Cost Centers</returns>
        [HttpPost("GetAllBarcodeStructures")]
        [Authorize]
        public IActionResult GetAllBarcodeStructures([FromBody] BarcodeStructureReqParam barcodeStructureReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllBarcodeStructures(barcodeStructureReqParam, SP_GetInsertUpdateDeleteBarcodeStructure);
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

        #region Add Barcode Structure

        /// <summary>
        /// Insert a Barcode Structure by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertBarcodeStructure")]
        [Authorize]
        public IActionResult InsertBarcodeStructure([FromBody] BarcodeStructureReqParam barcodeStructureReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertBarcodeStructure(barcodeStructureReqParam, SP_GetInsertUpdateDeleteBarcodeStructure);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Barcode Structure", "Added", barcodeStructCol.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Barcode Structure", "Added", barcodeStructCol.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Barcode Structure", 1, "Inserted", barcodeStructureReqParam.LoginName, "dbo.BarCode_Struct");
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

        #region Update Barcode Structure

        /// <summary>
        /// Update a Barcode Structure by passing all parameters with "Update = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateBarcodeStructure")]
        [Authorize]
        public IActionResult UpdateBarcodeStructure([FromBody] BarcodeStructureReqParam barcodeStructureReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateBarcodeStructure(barcodeStructureReqParam, SP_GetInsertUpdateDeleteBarcodeStructure);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Barcode Structure", 1, "Updated", barcodeStructureReqParam.LoginName, "dbo.BarCode_Struct");
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

        #region Delete Barcode Structure

        /// <summary>
        /// Delete a Barcode Structure by passing "Delete = 1" and ID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteBarcodeStructure")]
        [Authorize]
        public IActionResult DeleteBarcodeStructure([FromBody] BarcodeStructureReqParam barcodeStructureReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteBarcodeStructure(barcodeStructureReqParam, SP_GetInsertUpdateDeleteBarcodeStructure);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Barcode Structure", 1, "Deleted", barcodeStructureReqParam.LoginName, "dbo.BarCode_Struct");
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

        #region Applying Barcode Policy on All Assets

        /// <summary>
        /// This API is used to apply the Barcode Policy on all the Assets against a selected Company. Need to pass CompanyID to apply policy and LoginName to safe the Transaction
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("ApplyBarcodePolicy")]
        [Authorize]
        public IActionResult ApplyBarcodePolicy([FromBody] Barcode_AssignCompany barcode_AssignCompanyReqParam)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {

                if (barcode_AssignCompanyReqParam.CompanyID == 0)
                {
                    msg.message = "Please provide CompanyID";
                    msg.status = "404";
                    return BadRequest(msg);
                }

                if (barcode_AssignCompanyReqParam.BarcodeStructureID == 0)
                {
                    msg.message = "Please provide BarcodeStructureID";
                    msg.status = "404";
                    return BadRequest(msg);
                }

                DataTable dtUpdCompanyBarcodeStructure = DataLogic.UpdateCompanyBarcodeStructure(barcode_AssignCompanyReqParam, SP_UpdateCompanyBarcodeStructure);

                if (Convert.ToInt32(dtUpdCompanyBarcodeStructure.Rows[0]["Status"]) == 200)
                {
                    DataTable barcode_Assign = DataLogic.Barcode_AssignToSelectedCompany(barcode_AssignCompanyReqParam.CompanyID.ToString(), "[dbo].[SP_GetCompanyBarcode_Assign]");

                    string barcode_Structure_Assign = barcode_Assign.Rows[0]["BarCode"].ToString();
                    string valueSep = barcode_Assign.Rows[0]["ValueSep"].ToString();
                    int CompanyID = barcode_AssignCompanyReqParam.CompanyID;

                    DataTable tempTable = GF.ApplyPolicy(barcode_Structure_Assign, CompanyID, valueSep);

                    DataTable dt = DataLogic.ApplyBarcodePolicy(tempTable, "[dbo].[SP_ApplyBarcodePolicy]");

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
                                DataTable dt2 = DataLogic.InsertAuditLogs("Barcode Structure", 1, "Barcode Updated", barcode_AssignCompanyReqParam.LoginName, "dbo.AssetDetails");
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
                else
                {
                    msg.message = dtUpdCompanyBarcodeStructure.Rows[0]["Message"].ToString();
                    msg.status = dtUpdCompanyBarcodeStructure.Rows[0]["Status"].ToString();
                    return BadRequest(msg);
                }

                
            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Apply Barcode Policy on Asset Items

        /// <summary>
        /// This API is used to apply the Barcode Policy on all the Assets against a selected Item Code(s). 
        /// Need to pass Item Code to apply policy and LoginName to safe the transaction
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("ApplyBarcodePolicyAgainstAssetItems")]
        [Authorize]
        public IActionResult ApplyBarcodePolicyAgainstAssetItems([FromBody] BarcodePolicyOnAssetItems barcodePolicyOnAssetItems)
        {
            Message msg = new Message();
            DataTable dt = new DataTable();
            int countsForAssetsAgainstItemCode = 0;
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                DataTable itemCodeDataTable = new DataTable();
                itemCodeDataTable.Columns.Add("ItemCode");

                if (barcodePolicyOnAssetItems.BarcodeStructureID == 0)
                {
                    msg.message = "Please provide BarcodeStructureID";
                    msg.status = "404";
                    return BadRequest(msg);
                }

                if (barcodePolicyOnAssetItems.itemCodeTree.Count == 0)
                {
                    msg.message = "No Item Code is selected";
                    msg.status = "404";
                    return BadRequest(msg);
                }

                if (barcodePolicyOnAssetItems.itemCodeTree.Count > 0)
                {
                    itemCodeDataTable = ListintoDataTable.ToDataTable(barcodePolicyOnAssetItems.itemCodeTree);
                }

                DataTable barcodeStructureDT = DataLogic.BarcodeStructureAgainstBarStructID(barcodePolicyOnAssetItems.BarcodeStructureID, SP_GetBarcodeAgainstBarStructID);

                if (barcodeStructureDT.Rows.Count > 0)
                {
                    string barcode_Structure_Assign = barcodeStructureDT.Rows[0]["BarCode"].ToString();
                    string valueSep = barcodeStructureDT.Rows[0]["ValueSep"].ToString();

                    for (int i = 0; i < itemCodeDataTable.Rows.Count; i++)
                    {
                        //DataTable allAssetsAgainstItemCodeDT = DataLogic.GetAllAssetsAgainstItemCode(itemCodeDataTable.Rows[i]["ItemCode"].ToString(), SP_GetAllAssetsAgainstItemCode);
                        
                        if (string.IsNullOrEmpty(itemCodeDataTable.Rows[i]["ItemCode"].ToString()))
                        {
                            msg.message = "Process cancelled. Please check some value for ItemCode is passing as empty at index " + (i);
                            msg.status = "404";
                            return BadRequest(msg);
                        }
                        else
                        {
                            DataTable tempTable = GF.ApplyPolicyAgainstItemCode(barcode_Structure_Assign, 0, valueSep, itemCodeDataTable.Rows[i]["ItemCode"].ToString());
                            countsForAssetsAgainstItemCode = countsForAssetsAgainstItemCode + tempTable.Rows.Count;
                            if (tempTable.Rows.Count > 0)
                            {
                                DataTable updAssetItemBarStruct = DataLogic.UpdateBarStructIDAgainstItemCode(barcodePolicyOnAssetItems.BarcodeStructureID,
                                    itemCodeDataTable.Rows[i]["ItemCode"].ToString(), SP_UpdateBarStructIDAgainstItemCode);

                                if (updAssetItemBarStruct.Rows[0]["Message"].ToString().Contains("successfully"))
                                {
                                    dt = DataLogic.ApplyBarcodePolicy(tempTable, "[dbo].[SP_ApplyBarcodePolicy]");
                                }
                                else
                                {
                                    msg.message = updAssetItemBarStruct.Rows[0]["Message"].ToString();
                                    msg.status = "404";
                                    return BadRequest(msg);
                                }
                            }
                            else
                            {
                                msg.message = "No assets found against the selected asset item. ";
                                msg.status = "404";
                                return BadRequest(msg);
                            }
                        }
                    }
                }
                else
                {
                    msg.message = "There is no Barcode Structure available against.";
                    msg.status = "404";
                    return BadRequest(msg);
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ErrorMessage"))
                    {
                        msg.message = dt.Rows[0]["ErrorMessage"].ToString();
                        msg.status = "401";
                        return BadRequest(msg);
                    }
                    else
                    {
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Barcode Structure", 1, "Barcode Updated", barcodePolicyOnAssetItems.LoginName, "dbo.AssetDetails from Asset Items form");
                        }
                        msg.message = "Barcode Policy applied on " + countsForAssetsAgainstItemCode + " Assets successfully!";
                        //msg.message = dt.Rows[0]["Message"].ToString();
                        msg.status = dt.Rows[0]["Status"].ToString();
                        return Ok(msg);
                    }
                }
                else
                {
                    msg.message = dt.Rows[0]["Message"].ToString();
                    msg.status = dt.Rows[0]["Status"].ToString();
                    return BadRequest(msg);
                }

            }
            catch (Exception ex)
            {
                msg.message = ex.Message;
                return Ok(msg);
            }
        }

        #endregion

        #region Get Barcoding Policy

        /// <summary>
        /// Get all Barcode Structures Parameters by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Cost Centers</returns>
        [HttpGet("GetAllBarcodingPolicy")]
        [Authorize]
        public IActionResult GetAllBarcodingPolicy()
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllBarcodingPolicy(SP_GetAllBarcodePolicies);
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

    }
}
