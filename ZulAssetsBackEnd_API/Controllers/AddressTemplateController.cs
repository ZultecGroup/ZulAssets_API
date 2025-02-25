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
    [Tags("Address Template")]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressTemplateController : ControllerBase
    {

        #region Declaration

        private static string SP_GetAddressTemplates = "[dbo].[SP_GetInsertUpdateDeleteAddressTemplates]";
        private static string SP_InsertAddressTemplate = "[dbo].[SP_GetInsertUpdateDeleteAddressTemplates]";
        private static string SP_UpdateAddressTemplate = "[dbo].[SP_GetInsertUpdateDeleteAddressTemplates]";
        private static string SP_DeleteAddressTemplate = "[dbo].[SP_GetInsertUpdateDeleteAddressTemplates]";

        #endregion

        #region Get All Address Templates
        /// <summary>
        /// Get all Address Templates by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Address Templates</returns>
        [HttpPost("GetAllAddressTemplates")]
        [Authorize]
        public IActionResult GetAllAddressTemplates([FromBody] AddTempReqParam addTempReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllAddTemp(addTempReqParam, SP_GetAddressTemplates);
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

        #region Add Address Template

        /// <summary>
        /// Insert an Address Template by passing all parameters with "Add = 1" and without AddressID parameter
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertAddressTemplate")]
        [Authorize]
        public IActionResult InsertAddressTemplate([FromBody] AddTempReqParam addTempReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertAddTemp(addTempReqParam, SP_InsertAddressTemplate);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Address Templates", 1, "Insert", addTempReqParam.LoginName, "dbo.Address");
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

        #region Update Address Template

        /// <summary>
        /// Update an Address Template by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateAddressTemplate")]
        [Authorize]
        public IActionResult UpdateAddressTemplate([FromBody] AddTempReqParam addTempReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateAddTemp(addTempReqParam, SP_UpdateAddressTemplate);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Address Templates", 1, "Update", addTempReqParam.LoginName, "dbo.Address");
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

        #region Delete Address Template

        /// <summary>
        /// Delete an Address Template by passing "Delete = 1" and AddressID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteAddressTemplate")]
        [Authorize]
        public IActionResult DeleteAddressTemplate([FromBody] AddTempReqParam addTempReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteAddTemp(addTempReqParam, SP_DeleteAddressTemplate);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Address Templates", 1, "Delete", addTempReqParam.LoginName, "dbo.Address");
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
