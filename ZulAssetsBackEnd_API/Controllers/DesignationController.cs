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
    public class DesignationController : ControllerBase
    {
        #region Declaration

        private static string SP_GetDesignations = "[dbo].[SP_GetInsertUpdateDeleteDesignation]";
        private static string SP_InsertDesignations = "[dbo].[SP_GetInsertUpdateDeleteDesignation]";
        private static string SP_UpdateDesignations = "[dbo].[SP_GetInsertUpdateDeleteDesignation]";
        private static string SP_DeleteDesignations = "[dbo].[SP_GetInsertUpdateDeleteDesignation]";

        #endregion

        #region Get All Designations
        /// <summary>
        /// Get all Designations by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the Designations</returns>
        [HttpPost("GetAllDesignations")]
        [Authorize]
        public IActionResult GetAllDesignations([FromBody] DesignationRequest designationReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllDesignations(designationReq, SP_GetDesignations);
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

        #region Add Designation

        /// <summary>
        /// Insert a Designation by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertDesignation")]
        [Authorize]
        public IActionResult InsertDesignation([FromBody] DesignationRequest designationReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertDesignation(designationReq, SP_InsertDesignations);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Designation", "Added", designationReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Depreciation Method", 1, "Inserted", designationReq.LoginName, "dbo.Designation");
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

        #region Update Designation

        /// <summary>
        /// Update a Designation by passing all parameters with "Update = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateDesignation")]
        [Authorize]
        public IActionResult UpdateDesignation([FromBody] DesignationRequest designationReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateDesignation(designationReq, SP_UpdateDesignations);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Designation", "Updated", designationReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Depreciation Method", 1, "Updated", designationReq.LoginName, "dbo.Designation");
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

        #region Delete Designation

        /// <summary>
        /// Delete a Designation by passing "Delete = 1" and DesigatnionID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteDesignation")]
        [Authorize]
        public IActionResult DeleteDesignation([FromBody] DesignationRequest designationReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteDesignation(designationReq, SP_DeleteDesignations);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("Designation", "Deleted", designationReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("Depreciation Method", 1, "Deleted", designationReq.LoginName, "dbo.Designation");
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
