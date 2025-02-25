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
    public class UnitController : ControllerBase
    {

        #region Declaration

        private static string SP_GetUnits = "[dbo].[SP_GetInsertUpdateDeleteUnit]";
        private static string SP_InsertUnit = "[dbo].[SP_GetInsertUpdateDeleteUnit]";
        private static string SP_UpdateUnit = "[dbo].[SP_GetInsertUpdateDeleteUnit]";
        private static string SP_DeleteUnit = "[dbo].[SP_GetInsertUpdateDeleteUnit]";

        #endregion

        #region Get All Units
        /// <summary>
        /// Get all units by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Units</returns>
        [HttpPost("GetAllUnits")]
        [Authorize]
        public IActionResult GetAllUnits([FromBody] UnitRequestParam unitReqParam)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllUnits(unitReqParam, SP_GetUnits);
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

        #region Add Unit

        /// <summary>
        /// Insert a unit by passing all parameters with "Add = 1" and without UnitID parameter
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertUnit")]
        [Authorize]
        public IActionResult InsertUnit([FromBody] UnitRequestParam unitReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertUnit(unitReqParam, SP_InsertUnit);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Unit", "Added", unitReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Unit", 1, "Inserted", unitReqParam.LoginName, "dbo.Units");
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

        #region Update Unit

        /// <summary>
        /// Update a unit by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateUnit")]
        [Authorize]
        public IActionResult UpdateUnit([FromBody] UnitRequestParam unitReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateUnit(unitReqParam, SP_UpdateUnit);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Unit", "Updated", unitReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Unit", 1, "Updated", unitReqParam.LoginName, "dbo.Units");
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

        #region Delete Unit

        /// <summary>
        /// Delete a unit by passing "Delete = 1" and UnitID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteUnit")]
        [Authorize]
        public IActionResult DeleteUnit([FromBody] UnitRequestParam unitReqParam)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteUnit(unitReqParam, SP_DeleteUnit);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Unit", "Deleted", unitReqParam.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Unit", 1, "Deleted", unitReqParam.LoginName, "dbo.Units");
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
