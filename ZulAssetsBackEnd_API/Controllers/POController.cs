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
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Purchase Order")]
    [Route("api/[controller]")]
    [ApiController]
    public class POController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeletePO = "[dbo].[SP_GetInsertUpdateDeletePO]";
        private static string SP_GetPODetailsCount = "[dbo].[SP_GetPODetailsCount]";
        private static string SP_GetPOItmIDAgainstPOCode = "[dbo].[SP_GetPOItmIDAgainstPOCode]";
        private static string SP_GetPOItemsAgainstPOCode = "[dbo].[SP_GetPOItemsAgainstPOCode]";
        private static string SP_GetUpdatePOApproval = "[dbo].[SP_GetUpdatePOApproval]";
        private static string SP_GetPOsForTransit = "[dbo].[SP_GetPOsForTransit]";
        private static string SP_GetBooksAgainstCompanyID = "[dbo].[SP_GetBooksAgainstCompanyID]";

        #endregion

        #region Get All Purchase Orders
        /// <summary>
        /// Get all Purchase Orders by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Purchase Orders</returns>
        [HttpPost("GetAllPurchaseOrders")]
        [Authorize]
        public IActionResult GetAllPurchaseOrders([FromBody] POReqParams POReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllPurchaseOrders(POReqParams, SP_GetInsertUpdateDeletePO);
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

        #region Add Purchase Order With Items

        /// <summary>
        /// Insert a Purchase Order with Items by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertPOWithItems")]
        [Authorize]
        public IActionResult InsertPOWithItems([FromBody] POReqParams POReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable poDetailsCount = DataLogic.Count(SP_GetPODetailsCount);

                int POItmID = Convert.ToInt32(poDetailsCount.Rows[0]["POItmID"]);

                DataTable POItemDT = new DataTable();
                POItemDT.Columns.Add("POItmID"); POItemDT.Columns.Add("POCode"); POItemDT.Columns.Add("ItemCode");
                POItemDT.Columns.Add("POItmDesc"); POItemDT.Columns.Add("POItmBaseCost"); POItemDT.Columns.Add("AddCharges");
                POItemDT.Columns.Add("IsTrans"); POItemDT.Columns.Add("UnitID"); POItemDT.Columns.Add("PORecQty");

                if (POItemDT != null)
                {
                    POItemDT = ListintoDataTable.ToDataTable(POReqParams.poItemDetailsList);
                }

                for (int i = 0; i < POItemDT.Rows.Count; i++)
                {
                    POItemDT.Rows[i]["POItmID"] = POItmID + (i + 1);
                }

                DataTable dt = DataLogic.InsertPOWithItems(POReqParams, POItemDT, SP_GetInsertUpdateDeletePO);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Brand", "Added", POReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("PO", 1, "Inserted", POReqParams.LoginName, "dbo.PurchaseOrder & dbo.PODetails");
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

        #region Update Purchase Order & Items

        /// <summary>
        /// Update Purchase Order Items by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdatePOWithItems")]
        [Authorize]
        public IActionResult UpdatePOWithItems([FromBody] POReqParams POReqParams)
        {
            Message msg = new Message();
            try
            {

                DataTable poItmIDs = DataLogic.GetPOItmID(POReqParams.POCode, SP_GetPOItmIDAgainstPOCode);

                DataTable POItemDT = new DataTable();
                POItemDT.Columns.Add("POItmID"); POItemDT.Columns.Add("POCode"); POItemDT.Columns.Add("ItemCode");
                POItemDT.Columns.Add("POItmDesc"); POItemDT.Columns.Add("POItmBaseCode"); POItemDT.Columns.Add("AddCharges");
                POItemDT.Columns.Add("IsTrans"); POItemDT.Columns.Add("UnitID"); POItemDT.Columns.Add("PORecQty");

                if (POItemDT != null)
                {
                    POItemDT = ListintoDataTable.ToDataTable(POReqParams.poItemDetailsList);
                }

                for (int i = 0; i < POItemDT.Rows.Count; i++)
                {
                    POItemDT.Rows[i]["POItmID"] = poItmIDs.Rows[i]["POItmID"];
                }

                DataTable dt = DataLogic.UpdatePOWithItems(POReqParams, POItemDT, SP_GetInsertUpdateDeletePO);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("PO", "Added", POReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("PO", 1, "Updated", POReqParams.LoginName, "dbo.PurchaseOrder & dbo.PODetails");
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

        #region Delete Purchase Order or Item

        /// <summary>
        /// Delete a Purchase Order or Item/s by passing "Delete = 1" and POCode to delete Purchase Order with Items or POItmID to delete
        /// Item from the list as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeletePOWithItems")]
        [Authorize]
        public IActionResult DeletePOWithItems([FromBody] POReqParams POReqParams)
        {
            Message msg = new Message();
            try
            {

                DataTable POItemDT = new DataTable();
                POItemDT.Columns.Add("POItmID"); POItemDT.Columns.Add("POCode"); POItemDT.Columns.Add("ItemCode");
                POItemDT.Columns.Add("POItmDesc"); POItemDT.Columns.Add("POItmBaseCode"); POItemDT.Columns.Add("AddCharges");
                POItemDT.Columns.Add("IsTrans"); POItemDT.Columns.Add("UnitID"); POItemDT.Columns.Add("PORecQty");

                if (POItemDT != null)
                {
                    POItemDT = ListintoDataTable.ToDataTable(POReqParams.poItemDetailsList);
                }

                DataTable dt = DataLogic.DeletePOWithItems(POReqParams, POItemDT, SP_GetInsertUpdateDeletePO);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("PO", "Added", POReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("PO", 1, "Deleted", POReqParams.LoginName, "dbo.PurchaseOrder & dbo.PODetails");
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

        #region Get All Purchase Order Items Against POCode
        /// <summary>
        /// Get all Purchase Order Items by passing a parameter "Get = 1, POCode and others as empty"
        /// </summary>
        /// <returns>This will return all the Purchase Order Items</returns>
        [HttpPost("GetAllPOItems")]
        [Authorize]
        public IActionResult GetAllPOItems([FromBody] PODetailsReqParams PODetailsReqParams)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllPOItems(PODetailsReqParams, SP_GetPOItemsAgainstPOCode);
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

        #region Get Pending POs For Approval

        /// <summary>
        /// Get Pending POs For Approval by passing no parameters
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetPendingPOsForApproval")]
        [Authorize]
        public IActionResult GetPendingPOsForApproval([FromBody] POReqParams pOReqParams)
        {
            Message msg = new Message();

            try
            {

                DataSet ds = DataLogic.GetPendingPOsForApproval(pOReqParams, SP_GetUpdatePOApproval);
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

        #region Purchase Order Approval Cases

        /// <summary>
        /// Update Purchase Order Items by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("ApprovePOWithItems")]
        [Authorize]
        public IActionResult ApprovePOWithItems([FromBody] POReqParams POReqParams)
        {
            Message msg = new Message();

            try
            {

                DataTable dt = DataLogic.ApprovePOWithItems(POReqParams, SP_GetInsertUpdateDeletePO);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("PO", "Added", POReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("PO", 1, "Updated", POReqParams.LoginName, "dbo.PurchaseOrder & dbo.PODetails");
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

        #region Get Approved POs For Transit

        /// <summary>
        /// Get Approved POs by passing Get = 1 and other parameters as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetApprovedPOsForTransit")]
        [Authorize]
        public IActionResult GetApprovedPOsForTransit([FromBody] POReqParams pOReqParams)
        {
            Message msg = new Message();

            try
            {

                DataSet ds = DataLogic.GetApprovedPOsForTransit(pOReqParams, SP_GetPOsForTransit);
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

        #region Transfer Assets From PO to ERP

        /// <summary>
        /// Transfer Assets From Purchasse Orders to ERP
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("TransferAssetsFromPOToERP")]
        [Authorize]
        public IActionResult TransferAssetsFromPOToERP([FromBody] AssetsInTransit assetsInTransit)
        {
            Message msg = new Message();
            GeneralFunctions GF = new GeneralFunctions();
            try
            {
                string result = string.Empty;
                DataTable BooksDT = DataLogic.GetBookAgainstCompanyID(assetsInTransit.CompanyID, SP_GetBooksAgainstCompanyID);
                if (BooksDT.Rows.Count < 1)
                {
                    msg.message = "No Book Define For Selected Company";
                    msg.status = "401";
                    return Ok(msg);
                }
                else
                {
                    DataTable barcode_Assign = DataLogic.Barcode_AssignToSelectedCompany(assetsInTransit.CompanyID, "[dbo].[SP_GetCompanyBarcode_Assign]");

                    string barcode_Structure_Assign = barcode_Assign.Rows[0]["BarCode"].ToString();
                    string valueSep = barcode_Assign.Rows[0]["ValueSep"].ToString();
                    result = GF.AddNew_AssetDetails(assetsInTransit, valueSep, barcode_Structure_Assign);
                }

                DataTable dt = DataLogic.GetBookAgainstCompanyID(assetsInTransit.CompanyID, SP_GetBooksAgainstCompanyID);
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
                        if (result.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Brand", "Added", POReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("PO", 1, "Inserted", assetsInTransit.LoginName, "dbo.PurchaseOrder & dbo.PODetails");
                        }
                        msg.message = result;
                        msg.status = "200";
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

    }
}
