using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Barcode Labels")]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteBarcodeLabels = "[dbo].[SP_GetInsertUpdateDeleteBarcodeLabels]";
        private static string SP_GetBarcodeLabelDesignForPrinting = "[dbo].[SP_GetBarcodeLabelDesignForPrinting]";

        #endregion

        #region Get All Barcode Labels
        /// <summary>
        /// Get all Barcode Labels by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Brands</returns>
        [HttpPost("GetAllBarcodeLabels")]
        [Authorize]
        public IActionResult GetAllBarcodeLabels([FromBody] BarcodeLabelsRequest barcodeLabelsReq)
        {
            Message msg = new Message();
            try
            {

                DataSet ds = DataLogic.GetAllBarcodeLabels(barcodeLabelsReq, SP_GetInsertUpdateDeleteBarcodeLabels);

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

        #region Add BarcodeLabels

        /// <summary>
        /// Insert a Barcode Label by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertBarcodeLabels")]
        [Authorize]
        public IActionResult InsertBarcodeLabels([FromBody] BarcodeLabelsRequest barcodeLabelsReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertBarcodeLabels(barcodeLabelsReq, SP_GetInsertUpdateDeleteBarcodeLabels);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Brand", "Added", brandReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Brand", 1, "Inserted", barcodeLabelsReq.LoginName, "dbo.BarcodeLabels");
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

        #region Update Barcode Labels

        /// <summary>
        /// Update a Barcode label by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateBarcodeLabels")]
        [Authorize]
        public IActionResult UpdateBarcodeLabels([FromBody] BarcodeLabelsRequest barcodeLabelsReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateBarcodeLabels(barcodeLabelsReq, SP_GetInsertUpdateDeleteBarcodeLabels);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Brand", "Added", brandReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Brand", 1, "Updated", barcodeLabelsReq.LoginName, "dbo.BarcodeLabels");
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

        #region Delete Barcode Labels

        /// <summary>
        /// Delete a Barcode Label by passing "Delete = 1" and LabelID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteBarcodeLabels")]
        [Authorize]
        public IActionResult DeleteBarcodeLabels([FromBody] BarcodeLabelsRequest barcodeLabelsReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteBarcodeLabels(barcodeLabelsReq, SP_GetInsertUpdateDeleteBarcodeLabels);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Brand", "Added", brandReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Brand", 1, "Deleted", barcodeLabelsReq.LoginName, "dbo.BarcodeLabels");
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

        #region Get Label Design For Printing

        /// <summary>
        /// This endpoint will you to return the Label Design of passing Label Name
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("GetLabelDesignForPrinting")]
        [Authorize]
        public IActionResult GetLabelDesignForPrinting([FromBody] BarcodeLabelsRequest barcodeLabelsReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetLabelDesignForPrinting(barcodeLabelsReq, SP_GetBarcodeLabelDesignForPrinting);
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

    }
}
