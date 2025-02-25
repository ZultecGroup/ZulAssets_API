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
    [Tags("Cost Center")]
    [Route("api/[controller]")]
    [ApiController]
    public class CostCenterController : ControllerBase
    {

        #region Declaration

        private static string SP_GetCostCenters = "[dbo].[SP_GetInsertUpdateDeleteCostCenter]";
        private static string SP_InsertCostCenter = "[dbo].[SP_GetInsertUpdateDeleteCostCenter]";
        private static string SP_UpdateCostCenter = "[dbo].[SP_GetInsertUpdateDeleteCostCenter]";
        private static string SP_DeleteCostCenter = "[dbo].[SP_GetInsertUpdateDeleteCostCenter]";

        #endregion

        #region Get All Cost Centers
        /// <summary>
        /// Get all Cost Centers by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Cost Centers</returns>
        [HttpPost("GetAllCostCenters")]
        [Authorize]
        public IActionResult GetAllCostCenters([FromBody] CostCenterRequest costCenterReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllCostCenters(costCenterReq, SP_GetCostCenters);

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

        #region Add Cost Center

        /// <summary>
        /// Insert a Cost Center by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertCostCenter")]
        [Authorize]
        public IActionResult InsertCostCenter([FromBody] CostCenterRequest costCenterReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertCostCenter(costCenterReq, SP_InsertCostCenter);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Cost Center", "Added", costCenterReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            //var logResult = GeneralFunctions.CreateAndWriteToFile("Address Template", "Added", addTempReqParam.LoginName);
                            DataTable dt2 = DataLogic.InsertAuditLogs("Cost Center", 1, "Inserted", costCenterReq.LoginName, "dbo.CostCenter");
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

        #region Update Cost Center

        /// <summary>
        /// Update a Cost Center by passing all parameters with "Update = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateCostCenter")]
        [Authorize]
        public IActionResult UpdateCostCenter([FromBody] CostCenterRequest costCenterReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateCostCenter(costCenterReq, SP_UpdateCostCenter);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Cost Center", 1, "Updated", costCenterReq.LoginName, "dbo.CostCenter");
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

        #region Delete Cost Center

        /// <summary>
        /// Delete a Cost Center by passing "Delete = 1" and CostCenterID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteCostCenter")]
        [Authorize]
        public IActionResult DeleteCostCenter([FromBody] CostCenterRequest costCenterReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteCostCenter(costCenterReq, SP_DeleteCostCenter);
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
                            DataTable dt2 = DataLogic.InsertAuditLogs("Cost Center", 1, "Deleted", costCenterReq.LoginName, "dbo.CostCenter");
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
