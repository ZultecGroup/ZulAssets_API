using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.Controllers
{
    //[ApiVersion("1")]
    //[ApiExplorerSettings(GroupName = "v1")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Tags("Disposal Methods")]
    [Route("api/[controller]")]
    [ApiController]
    public class DisposalMethodController : ControllerBase
    {

        #region Declaration

        private static string SP_GetDispMethods = "[dbo].[SP_GetInsertUpdateDeleteDisposal_Method]";
        private static string SP_InsertDispMethod = "[dbo].[SP_GetInsertUpdateDeleteDisposal_Method]";
        private static string SP_UpdateDispMethod = "[dbo].[SP_GetInsertUpdateDeleteDisposal_Method]";
        private static string SP_DeleteDispMethod = "[dbo].[SP_GetInsertUpdateDeleteDisposal_Method]";

        #endregion

        #region Get All Disposal Methods
        /// <summary>
        /// Get all Disposal Methods by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Disposal Methods</returns>
        [HttpPost("GetAllDispMethods")]
        [Authorize]
        public IActionResult GetAllDepMethods([FromBody] DisposalMethodsRequest dispMethodReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllDisposalMethods(dispMethodReqParam, SP_GetDispMethods);
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

        #region Add Disposal Method

        /// <summary>
        /// Insert a Disposal Method by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertDispMethod")]
        [Authorize]
        public IActionResult InsertDispMethod([FromBody] DisposalMethodsRequest dispMethodReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertDisposalMethod(dispMethodReqParam, SP_InsertDispMethod);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Disposal Method", "Added", dispMethodReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Disposal Method", 1, "Inserted", dispMethodReqParam.LoginName, "dbo.Disposal_Method");
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

        #region Update Disposal Method

        /// <summary>
        /// Update a Disposal Method by passing all parameters with "Update = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateDispMethod")]
        [Authorize]
        public IActionResult UpdateDispMethod([FromBody] DisposalMethodsRequest dispMethodReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateDisposalMethod(dispMethodReqParam, SP_UpdateDispMethod);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Disposal Method", "Updated", dispMethodReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Disposal Method", 1, "Updated", dispMethodReqParam.LoginName, "dbo.Disposal_Method");
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

        #region Delete Disposal Method

        /// <summary>
        /// Delete a Disposal Method by passing "Delete = 1" and DesigatnionID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteDispMethod")]
        [Authorize]
        public IActionResult DeleteDispMethod([FromBody] DisposalMethodsRequest dispMethodReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteDisposalMethod(dispMethodReqParam, SP_DeleteDispMethod);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Disposal Method", "Deleted", dispMethodReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Disposal Method", 1, "Deleted", dispMethodReqParam.LoginName, "dbo.Disposal_Method");
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
