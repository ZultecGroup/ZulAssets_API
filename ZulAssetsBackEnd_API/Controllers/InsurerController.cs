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
    public class InsurerController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteInsurer = "[dbo].[SP_GetInsertUpdateDeleteInsurer]";

        #endregion

        #region Get All Insurers
        /// <summary>
        /// Get all Insurers by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Insurers</returns>
        [HttpPost("GetAllInsurers")]
        [Authorize]
        public IActionResult GetAllInsurers([FromBody] InsurerReqParam insReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllInsurers(insReqParam, SP_GetInsertUpdateDeleteInsurer);
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

        #region Add Insurer

        /// <summary>
        /// Insert a Insurer by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertInsurer")]
        [Authorize]
        public IActionResult InsertInsurer([FromBody] InsurerReqParam insReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertInsurer(insReqParam, SP_GetInsertUpdateDeleteInsurer);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Insurer", "Added", insReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Insurer", 1, "Inserted", insReqParam.LoginName, "dbo.Insurer");
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

        #region Update Insurer

        /// <summary>
        /// Update a Insurer by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateInsurer")]
        [Authorize]
        public IActionResult UpdateInsurer([FromBody] InsurerReqParam insReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateInsurer(insReqParam, SP_GetInsertUpdateDeleteInsurer);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Insurer", "Updated", insReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Insurer", 1, "Updated", insReqParam.LoginName, "dbo.Insurer");
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

        #region Delete Insurer

        /// <summary>
        /// Delete a Insurer by passing "Delete = 1" and InsurerCode as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteInsurer")]
        [Authorize]
        public IActionResult DeleteInsurer([FromBody] InsurerReqParam insReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteInsurer(insReqParam, SP_GetInsertUpdateDeleteInsurer);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Insurer", "Deleted", insReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Insurer", 1, "Deleted", insReqParam.LoginName, "dbo.Insurer");
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

    }
}
