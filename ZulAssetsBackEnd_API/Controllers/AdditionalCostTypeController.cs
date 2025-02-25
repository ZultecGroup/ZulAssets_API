using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("2")]
    //[ApiExplorerSettings(GroupName = "v2")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Additional Cost Type")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalCostTypeController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertDeleteAdditionalCostType = "[dbo].[SP_GetInsertDeleteAdditionalCostType]";

        #endregion

        #region Get All Additional Cost Type
        /// <summary>
        /// Get all Additional Cost Types by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Additional Cost Types</returns>
        [HttpPost("GetAllAddCostType")]
        [Authorize]
        public IActionResult GetAllAddCostType([FromBody] AddCostTypeReqParam addCostTypeReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetAllAddCostType(addCostTypeReqParam, SP_GetInsertDeleteAdditionalCostType);
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

        #region Add Additional Cost Type

        /// <summary>
        /// Insert a Additional Cost Type by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertAddCostType")]
        [Authorize]
        public IActionResult InsertAddCostType([FromBody] AddCostTypeReqParam addCostTypeReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertAddCostType(addCostTypeReqParam, SP_GetInsertDeleteAdditionalCostType);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Additional Cost Type", "Added", addCostTypeReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Additional Cost Type", "Added", addCostTypeReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Additional Cost Type", 1, "Inserted", addCostTypeReqParam.LoginName, "dbo.AddCostType");
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

        //#region Update Additional Cost Type

        ///// <summary>
        ///// Update a Additional Cost Type by passing all parameters with "Update = 1" and others as empty
        ///// </summary>
        ///// <returns>This will return a message of success</returns>
        //[HttpPost("UpdateAddCostType")]
        ////[Authorize]
        //public IActionResult UpdateCostCenter([FromBody] CostCenterRequest costCenterReq)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.UpdateCostCenter(costCenterReq, SP_UpdateCostCenter);
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
        //                    DataTable dt2 = DataLogic.InsertAuditLogs("Cost Center", 1, "Updated", costCenterReq.LoginName, "dbo.CostCenter");
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

        //#region Delete Cost Center

        ///// <summary>
        ///// Delete a Cost Center by passing "Delete = 1" and DesigatnionID as parameters and others as empty
        ///// </summary>
        ///// <returns>This will return a message of success</returns>
        //[HttpPost("DeleteCostCenter")]
        //[Authorize]
        //public IActionResult DeleteCostCenter([FromBody] CostCenterRequest costCenterReq)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.DeleteCostCenter(costCenterReq, SP_DeleteCostCenter);
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
        //                    DataTable dt2 = DataLogic.InsertAuditLogs("Cost Center", 1, "Deleted", costCenterReq.LoginName, "dbo.CostCenter");
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

    }
}
