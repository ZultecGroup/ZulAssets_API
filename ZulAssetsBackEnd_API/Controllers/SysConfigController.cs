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
    [Tags("System Configuration")]
    [Route("api/[controller]")]
    [ApiController]
    public class SysConfigController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateSysSettings = "[dbo].[SP_GetInsertUpdateSysSettings]";

        #endregion

        #region Get System Configuration Information
        /// <summary>
        /// Get System Configuration Information by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return System Configuration Information</returns>
        [HttpPost("GetSysConfigInfo")]
        [Authorize]
        public IActionResult GetSysConfigInfo([FromBody] SysConfigReqParams sysConfigReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.GetSysConfigInfo(sysConfigReqParams, SP_GetInsertUpdateSysSettings);
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

        #region Add System Configuration

        /// <summary>
        /// Insert System Configuration by passing all parameters with "Add = 1" and without ID parameter
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertSysConfigInfo")]
        [Authorize]
        public IActionResult InsertSysConfigInfo([FromBody] SysConfigReqParams sysConfigReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertSysConfigInfo(sysConfigReqParams, SP_GetInsertUpdateSysSettings);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("System Settings", "Added", sysConfigReqParams.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("System Settings", 1, "Insert", sysConfigReqParams.LoginName, "dbo.SysSettings");
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

        #region Update System Configuration

        /// <summary>
        /// Update System Configuration by passing all parameters with "Update = 1" and ID
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateSysConfigInfo")]
        [Authorize]
        public IActionResult UpdateSysConfigInfo([FromBody] SysConfigReqParams sysConfigReqParams)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateSysConfigInfo(sysConfigReqParams, SP_GetInsertUpdateSysSettings);
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
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", addTempReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("System Configuration", 1, "Update", sysConfigReqParams.LoginName, "dbo.SysSettings");
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

        //#region Delete System Configuration

        ///// <summary>
        ///// Delete System Configuration by passing "Delete = 1" and ID as parameters and others as empty
        ///// </summary>
        ///// <returns>This will return a message of success</returns>
        //[HttpPost("DeleteSysConfigInfo")]
        ////[Authorize]
        //public IActionResult DeleteSysConfigInfo([FromBody] SysConfigReqParams sysConfigReqParams)
        //{
        //    Message msg = new Message();
        //    try
        //    {
        //        DataTable dt = DataLogic.DeleteSysConfigInfo(sysConfigReqParams, SP_GetInsertUpdateSysSettings);
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
        //                    //var logResult = GeneralFunctions.CreateAndWriteToFile("System Configuration", "Added", addTempReqParam.LoginName);
        //                    DataTable dt2 = DataLogic.InsertAuditLogs("System Configuration", 1, "Delete", sysConfigReqParams.LoginName, "dbo.SysSettings");
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
