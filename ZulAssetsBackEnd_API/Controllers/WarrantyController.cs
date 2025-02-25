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
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantyController : ControllerBase
    {

        #region Declaration

        private static string SP_InsertUpdateDeleteWarrantyInfo = "[dbo].[SP_InsertUpdateDeleteWarrantyInfo]";
        private static string SP_GetAlarmBeforeDaysForWarranty = "[dbo].[SP_GetAlarmBeforeDaysForWarranty]";
        private static string SP_GetAllAssetsWarrantyEnd = "[dbo].[SP_GetAllAssetsWarrantyEnd]";

        #endregion

        #region Add Warranty Information

        /// <summary>
        /// Insert a Warranty Information by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertWarranty")]
        [Authorize]
        public IActionResult InsertWarranty([FromBody] WarrantyReqParams warrantyReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertWarranty(warrantyReqParams, SP_InsertUpdateDeleteWarrantyInfo);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Warranty Info", "Added", warrantyReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Warranty Info", 1, "Inserted", warrantyReqParams.LoginName, "dbo.AssetWarranty");
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

        #region Update Warranty Info

        /// <summary>
        /// Update a Warranty Info by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateWarranty")]
        [Authorize]
        public IActionResult UpdateWarranty([FromBody] WarrantyReqParams warrantyReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateWarranty(warrantyReqParams, SP_InsertUpdateDeleteWarrantyInfo);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Warranty Info", "Added", warrantyReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Warranty Info", 1, "Updated", warrantyReqParams.LoginName, "dbo.AssetWarranty");
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

        #region Delete Warranty Info

        /// <summary>
        /// Delete a Warranty Info by passing "Delete = 1" and WarrantyID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteWarranty")]
        [Authorize]
        public IActionResult DeleteWarranty([FromBody] WarrantyReqParams warrantyReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteWarranty(warrantyReqParams, SP_InsertUpdateDeleteWarrantyInfo);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Warranty Info", "Added", brandReq.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Warranty Info", 1, "Deleted", warrantyReqParams.LoginName, "dbo.AssetWarranty");
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

        #region Get All Assets Whose Warranty are near to end

        /// <summary>
        /// Get All Assets whose warranty are nearby to be expire
        /// </summary>
        [HttpGet("GetAllWarrantyAssets")]
        [Authorize]
        public IActionResult GetAllWarrantyAssets()
        {
            Message msg = new Message();
            try
            {
                DataTable dtAlarmBeforeDays = DataLogic.GetAlarmBeforeDaysValue(SP_GetAlarmBeforeDaysForWarranty);
                DataTable dt = DataLogic.GetAllWarrantyAssets(Convert.ToInt32(dtAlarmBeforeDays.Rows[0]["AlarmBeforeDays"].ToString()), SP_GetAllAssetsWarrantyEnd);

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
