using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;
using System.Data;
using ZulAssetsBackEnd_API.DAL;
using Microsoft.AspNetCore.Authorization;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Tags("GL Codes")]
    [Route("api/[controller]")]
    [ApiController]
    public class GLCodesController : ControllerBase
    {

        #region Declaration

        private static string SP_GetInsertUpdateDeleteGlCodes = "[dbo].[SP_GetInsertUpdateDeleteGlCodes]";

        #endregion

        #region Get All GLCodes
        /// <summary>
        /// Get all GLCodes by passing a parameter "Get = 1 and others as empty"
        /// </summary>
        /// <returns>This will return all the GLCodes</returns>
        [HttpPost("GetAllGLCodes")]
        [Authorize]
        public IActionResult GetAllGLCodes([FromBody] GLCodeRequest glcodeReq)
        {
            Message msg = new Message();
            try
            {
                DataSet ds = DataLogic.GetAllGLCodes(glcodeReq, SP_GetInsertUpdateDeleteGlCodes);
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

        #region Add GLCode

        /// <summary>
        /// Insert a GLCode by passing all parameters with "Add = 1" and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("InsertGLCode")]
        [Authorize]
        public IActionResult InsertGLCode([FromBody] GLCodeRequest glcodeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.InsertGLCode(glcodeReq, SP_GetInsertUpdateDeleteGlCodes);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("GL Code", "Added", glcodeReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("GL Code", 1, "Inserted", glcodeReq.LoginName, "dbo.GLCodes");
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

        #region Update GLCode

        /// <summary>
        /// Update a GLCode by passing all parameters with "Update = 1"
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("UpdateGLCode")]
        [Authorize]
        public IActionResult UpdateGLCode([FromBody] GLCodeRequest glcodeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.UpdateGLCode(glcodeReq, SP_GetInsertUpdateDeleteGlCodes);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("GL Code", "Updated", glcodeReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("GL Code", 1, "Updated", glcodeReq.LoginName, "dbo.GLCodes");
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

        #region Delete GLCode

        /// <summary>
        /// Delete a GLCode by passing "Delete = 1" and GLCodeID as parameters and others as empty
        /// </summary>
        /// <returns>This will return a message of success</returns>
        [HttpPost("DeleteGLCode")]
        [Authorize]
        public IActionResult DeleteGLCode([FromBody] GLCodeRequest glcodeReq)
        {
            Message msg = new Message();
            try
            {
                DataTable dt = DataLogic.DeleteGLCode(glcodeReq, SP_GetInsertUpdateDeleteGlCodes);
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
                        //var logResult = GeneralFunctions.CreateAndWriteToFile("GL Code", "Deleted", glcodeReq.LoginName);
                        string msgFromDB = dt.Rows[0]["Message"].ToString();
                        if (msgFromDB.Contains("successfully"))
                        {
                            DataTable dt2 = DataLogic.InsertAuditLogs("GL Code", 1, "Deleted", glcodeReq.LoginName, "dbo.GLCodes");
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
